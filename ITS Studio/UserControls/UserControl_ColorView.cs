using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using CommonExt;

namespace ITS_Studio.UserControls
{
    public partial class UserControl_ColorView : UserControl
    {
        private emDirection _axisDirection = emDirection.Left;
        private double _dScaleX;
        private double _dScaleY;
        private Point _end;
        private int _endIndex;
        private float _iOffset = 40f;
        private bool _isShowMarks;
        private List<RectangleF> _lsAllRectangelF = new List<RectangleF>();
        private List<int> _lsIndexAfterZoom = new List<int>();
        private List<short> _lsRawData = new List<short>();
        private List<int> _lsXAxisAfterZoom = new List<int>();
        private List<int> _lsYAxisAfterZoom = new List<int>();
        private emPlayStatus _playStatus = emPlayStatus.Stop;
        private Point _start;
        //Zoom專用
        private int _startIndex;
        private int _xChanel;
        private bool _xInverted = false;
        private int _yChanel;
        private bool _yInverted = false;
        private bool _paintXInverted = false;
        private bool _paintYInverted = false;
        private float _fontSizeSetting = 1.0f;
        private float _fontAxisSizeSetting = 0.5f;
        private double _redFactor = 2.0d;
        private double _greenFactor = 1.0d;
        private double _blueFactor = 1.0d;
        private int _iMutual_Min = 0;
        private int _PaletteStep = 1;
        private bool _isShowSupportLine;
        private bool _isShowCoordinates;
        private PointF _zoomEndPoint;
        private PointF _zoomStartPoint;
        private emZoomStatus _zoomStatus = emZoomStatus.Normal;
        private SolidBrush backgroundBrush = new SolidBrush(Color.White);
        private Graphics bitDrawLine;
        private Bitmap bitmapDrawLine;
        private Graphics MouseMoveDraw;
        private Bitmap bitMouseMove;
        private SolidBrush[] BrushSelect;
        private Font font1 = new Font("Consolas", 6, FontStyle.Regular, GraphicsUnit.Point);
        private Font fontAxis = new Font("Consolas", 5, FontStyle.Regular, GraphicsUnit.Point);
        private List<DrawingObject> mi_DrawObjects = new List<DrawingObject>();
        private StringFormat stringFormat = new StringFormat();
        private List<Color> lsTChartColor = new List<Color>();

        public UserControl_ColorView()
        {
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            // Load the default colors
            BackColor = Color.White;

            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            bitmapDrawLine = new Bitmap(this.Width - (int)_iOffset, this.Height - (int)_iOffset);
            bitDrawLine = Graphics.FromImage(bitmapDrawLine);

            SetColorSteps(256);
        }

        public enum emDirection : int
        {
            Left = 0,
            Right,
        }

        public enum emPlayStatus : int
        {
            Play = 0,
            Pause,
            Stop,
        }

        enum DrawType : int
        {
            DrawLine = 0,
            Fillrectangle,
            DrawAxis,
        }
        enum emZoomStatus : int
        {
            ZoomIn,
            ZoomOut,
            Normal,
        }
        public emDirection AxisDirection
        {
            get
            {
                return _axisDirection;
            }
            set
            {
                _axisDirection = value;
            }
        }

        public double dScaleX
        {
            get
            {
                return _dScaleX;
            }
            set
            {
                _dScaleX = value;
            }
        }

        public double dScaleY
        {
            get
            {
                return _dScaleY;
            }
            set
            {
                _dScaleY = value;
            }
        }

        /// <summary>
        /// 是否顯示文字
        /// </summary>
        public bool isShowMarks
        {
            get
            {
                return _isShowMarks;
            }
            set
            {
                _isShowMarks = value;

                if((_playStatus == emPlayStatus.Pause || _playStatus == emPlayStatus.Stop)
                    && _lsRawData.Count > 0)
                    ShowColorGridData(_xChanel, _yChanel, _lsRawData);
            }
        }

        /// <summary>
        /// 是否顯示輔助線
        /// </summary>
        public bool isShowSupportLine
        {
            get
            {
                return _isShowSupportLine;
            }
            set
            {
                _isShowSupportLine = value;

                if((_playStatus == emPlayStatus.Pause || _playStatus == emPlayStatus.Stop)
                    && _lsRawData.Count > 0)
                    ShowColorGridData(_xChanel, _yChanel, _lsRawData);
            }
        }

        /// <summary>
        /// 是否顯示座標
        /// </summary>
        public bool isShowCoordinates
        {
            get
            {
                return _isShowCoordinates;
            }
            set
            {
                _isShowCoordinates = value;

                if((_playStatus == emPlayStatus.Pause || _playStatus == emPlayStatus.Stop)
                    && _lsRawData.Count > 0)
                    ShowColorGridData(_xChanel, _yChanel, _lsRawData);
            }
        }

        public double RedFactor
        {
            get
            {
                return _redFactor;
            }
            set
            {
                _redFactor = value;
            }
        }
        public double GreendFactor
        {
            get
            {
                return _greenFactor;
            }
            set
            {
                _greenFactor = value;
            }
        }
        public double BlueFactor
        {
            get
            {
                return _blueFactor;
            }
            set
            {
                _blueFactor = value;
            }
        }

        public emPlayStatus PlayStatus
        {
            get
            {
                return _playStatus;
            }
            set
            {
                _playStatus = value;
            }
        }

        public bool XInverted
        {
            get
            {
                return _xInverted;
            }
            set
            {
                _xInverted = value;
            }
        }

        public bool YInverted
        {
            get
            {
                return _yInverted;
            }
            set
            {
                _yInverted = value;
            }
        }

        public bool PaintX_Inverted
        {
            get
            {
                return _paintXInverted;
            }
            set
            {
                _paintXInverted = value;
            }
        }

        public bool PaintY_Inverted
        {
            get
            {
                return _paintYInverted;
            }
            set
            {
                _paintYInverted = value;
            }
        }

        public int iMutual_Min
        {
            get
            {
                return _iMutual_Min;
            }
            set
            {
                _iMutual_Min = value;
            }
        }

        public int PaletteStep
        {
            get
            {
                return _PaletteStep;
            }
            set
            {
                _PaletteStep = value;
            }
        }

        public void ClearRawData()
        {
            _lsRawData.Clear();
        }

        public void ClearPaintingBitmap()
        {
            if(this.Width < _iOffset || this.Height < _iOffset)
                return;
            bitmapDrawLine = new Bitmap(this.Width - (int)_iOffset, this.Height - (int)_iOffset);
            bitDrawLine = Graphics.FromImage(bitmapDrawLine);
            //_lsRawData.Clear();
        }

        public void DrawLine(Color PenColor, Point OldPoint, Point NewPoint, enPaintingCount PaintType, int iFingerNum)
        {
            Color PColor = Color.FromArgb(200, PenColor.R, PenColor.G, PenColor.B);

            Color StringColor;
            if(PaintType == enPaintingCount.Painting1)
                StringColor = Color.Red;
            else
                StringColor = Color.Purple;

            Pen p = new Pen(StringColor, 0.5f);

            Point Old_Position = new Point((int)(OldPoint.X * _dScaleX), (int)(OldPoint.Y * _dScaleY));
            Point New_Position = new Point((int)(NewPoint.X * _dScaleX), (int)(NewPoint.Y * _dScaleY));

            Old_Position = new Point((_paintXInverted) ? bitmapDrawLine.Width - Old_Position.X : Old_Position.X, (_paintYInverted) ? bitmapDrawLine.Height - Old_Position.Y : Old_Position.Y);
            New_Position = new Point((_paintXInverted) ? bitmapDrawLine.Width - New_Position.X : New_Position.X, (_paintYInverted) ? bitmapDrawLine.Height - New_Position.Y : New_Position.Y);

            SolidBrush brush = new SolidBrush(PColor);

            bitDrawLine.FillEllipse(brush, New_Position.X - 4, New_Position.Y - 4, 8, 8);

            if(_isShowSupportLine)
            {
                bitDrawLine.DrawLine(p, new Point(0, New_Position.Y), new Point(New_Position.X, New_Position.Y));
                bitDrawLine.DrawLine(p, new Point(New_Position.X, this.Height), new Point(New_Position.X, New_Position.Y));
            }

            if(_isShowCoordinates)
            {
                SolidBrush bursh = new SolidBrush(StringColor);
                Point CoorPoint = new Point(New_Position.X - 6, New_Position.Y - 12);

                if(New_Position.X < _iOffset + 15)
                    CoorPoint = new Point(CoorPoint.X + 60, CoorPoint.Y);

                if(New_Position.Y < 15)
                    CoorPoint = new Point(CoorPoint.X, CoorPoint.Y + 24);

                if(New_Position.X > bitmapDrawLine.Width - 45)
                    CoorPoint = new Point(CoorPoint.X - 60, CoorPoint.Y);

                string sCoor = string.Format("P{2}_{3}:({0},{1})", NewPoint.X.ToString(), NewPoint.Y.ToString(), ((int)(++PaintType)).ToString(), iFingerNum.ToString());
                Font font = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Point);
                bitDrawLine.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                bitDrawLine.DrawString(sCoor, font, bursh, CoorPoint, stringFormat);
            }
            //輔助線

            //bitDrawLine.DrawLine(p, Old_Position, New_Position);

        }

        public void SetColorSteps(int iSteps)
        {
            lsTChartColor = new List<Color>();
            double num = 3.141592653589793 / (double)iSteps;
            for(int i = 0; i < iSteps; i++)
            {
                double num4 = num * (double)i;
                Color aColor;
                aColor = Color.FromArgb(0xFF, (int)Math.Round(127.0 * (Math.Sin(num4 / _redFactor) + 1.0)), (int)Math.Round(127.0 * (Math.Sin(num4 / _greenFactor) + 1.0)), (int)Math.Round(127.0 * (Math.Cos(num4 / _blueFactor) + 1.0)));
                lsTChartColor.Add(aColor);
            }
            InitBrushColorRange();
        }

        public void DrawLine(enPaintingCount PaintType, bool isHover, Point OldPoint, Point NewPoint)
        {

            Color tmp = new Color();
            if(PaintType == enPaintingCount.Painting1)
            {
                tmp = Color.FromArgb(127, __Paint1ContactColor.R, __Paint1ContactColor.G, __Paint1ContactColor.B);
            }
            else
            {
                tmp = Color.FromArgb(127, __Paint2ContactColor.R, __Paint2ContactColor.G, __Paint2ContactColor.B);
            }

            Pen p = new Pen(tmp, 4.5f);

            Point Old_Position = new Point((int)(OldPoint.X * _dScaleX), (int)(OldPoint.Y * _dScaleY));
            Point New_Position = new Point((int)(NewPoint.X * _dScaleX), (int)(NewPoint.Y * _dScaleY));

            Old_Position = new Point((_paintXInverted) ? bitmapDrawLine.Width - Old_Position.X : Old_Position.X, (_paintYInverted) ? bitmapDrawLine.Height - Old_Position.Y : Old_Position.Y);
            New_Position = new Point((_paintXInverted) ? bitmapDrawLine.Width - New_Position.X : New_Position.X, (_paintYInverted) ? bitmapDrawLine.Height - New_Position.Y : New_Position.Y);

            SolidBrush brush = new SolidBrush(tmp);

            //for (int i = 0; i < mi_DrawObjects.Count; i++)
            //{
            //    if (mi_DrawObjects[i].Rect.Contains(New_Position.X, New_Position.Y))
            //    {
            //        bitDrawLine.FillEllipse(brush, New_Position.X - 4, New_Position.Y - 4, 8, 8);
            //        break;
            //    }
            //}

            bitDrawLine.FillEllipse(brush, New_Position.X - 4, New_Position.Y - 4, 8, 8);

            //bitDrawLine.DrawLine(p, Old_Position, New_Position);

        }

        public void InvalidateUI()
        {
            this.Invalidate();
        }

        public void MappingResolution(double xRes, double yRes)
        {
            if(this.Width < _iOffset || this.Height < _iOffset)
            {
                MessageBox.Show("畫面超過限制大小\n請使用右鍵選單UI Control來控制", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            bitmapDrawLine = new Bitmap(this.Width - (int)_iOffset, this.Height - (int)_iOffset);
            bitDrawLine = Graphics.FromImage(bitmapDrawLine);

            _dScaleX = (double)(bitmapDrawLine.Width - 1) / (double)(xRes - 1);
            _dScaleY = (double)(bitmapDrawLine.Height - 1) / (double)(yRes - 1);
        }

        public void ResetDrawObjects()
        {
            mi_DrawObjects.Clear();
        }

        public void ShowColorGridData(int x, int y, List<short> data)
        {
            mi_DrawObjects.Clear();
            _lsAllRectangelF.Clear();
            //_lsRawData.Clear();
            _lsRawData = data;

            _xChanel = x;
            _yChanel = y;

            if(_xChanel == 0 && _yChanel == 0)
                return;

            if(data.Count > (_xChanel * _yChanel)) //資料不正確
                return;

            if(_zoomStatus == emZoomStatus.Normal)
                ShowColorGrid_Normal(data);
            else
                ShowColorGrid_ZoomIn(data);

            Invalidate();  // repaint
            //Update();
        }

        private ColorMap map = new ColorMap();
        private Color __StartColor = Color.White;
        private Color __EndColor = Color.White;
        private Color __Paint1ContactColor = Color.FromArgb(127, 0, 0, 255);
        private Color __Paint1HoverColor = Color.FromArgb(127, 255, 0, 0);
        private Color __Paint2ContactColor = Color.FromArgb(127, 0, 255, 255);
        private Color __Paint2HoverColor = Color.FromArgb(127, 255, 255, 0);
        public Color StartColor
        {
            set
            {
                __StartColor = value;
            }
            get
            {
                return __StartColor;
            }
        }
        public Color EndColor
        {
            set
            {
                __EndColor = value;
            }
            get
            {
                return __EndColor;
            }
        }
        public Color Paint1ContactColor
        {
            set
            {
                __Paint1ContactColor = value;
            }
            get
            {
                return __Paint1ContactColor;
            }
        }
        public Color Paint1HoverColor
        {
            set
            {
                __Paint1HoverColor = value;
            }
            get
            {
                return __Paint1HoverColor;
            }
        }
        public Color Paint2ContactColor
        {
            set
            {
                __Paint2ContactColor = value;
            }
            get
            {
                return __Paint2ContactColor;
            }
        }
        public Color Paint2HoverColor
        {
            set
            {
                __Paint2HoverColor = value;
            }
            get
            {
                return __Paint2HoverColor;
            }
        }

        //public void InitBrushColorRange(Color _Color1, Color _Color2, int iMinValue, int iMaxValue)
        public void InitBrushColorRange(int iColorResNum)
        {
            map.MapColors(new Color[] { __StartColor, __EndColor }, 0, iColorResNum);
            //var Delta = Math.Abs(iMaxValue - iMinValue) + 1;
            BrushSelect = new SolidBrush[iColorResNum];

            //int _Max = iMaxValue > iMinValue ? iMaxValue : iMinValue;
            for(int iValue = 0; iValue < lsTChartColor.Count; iValue++)
                BrushSelect[iValue] = new SolidBrush(lsTChartColor[iValue]);
        }

        public void InitBrushColorRange()
        {
            //map.MapColors(new Color[] { __StartColor, __EndColor }, 0, iColorResNum);
            //var Delta = Math.Abs(iMaxValue - iMinValue) + 1;
            BrushSelect = new SolidBrush[lsTChartColor.Count];

            //int _Max = iMaxValue > iMinValue ? iMaxValue : iMinValue;
            for(int iValue = 0; iValue < lsTChartColor.Count; iValue++)
                BrushSelect[iValue] = new SolidBrush(lsTChartColor[iValue]);
        }

        PointF _ZoomStartTest;
        PointF _ZoomEndTest;

        bool m_down = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if(_playStatus == emPlayStatus.Stop)
                return;

            if(_xChanel == 0 || _yChanel == 0)
                return;
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _zoomStartPoint = e.Location;
                m_down = true;

                if(_zoomStatus == emZoomStatus.ZoomIn)
                    return;

                for(int i = 0; i < _lsAllRectangelF.Count; i++)
                {
                    if(_lsAllRectangelF[i].Contains(_zoomStartPoint))
                    {
                        _startIndex = i;
                        //_start = new Point((_startIndex % _xChanel) + 1, (_startIndex / _xChanel) + 1);
                        _start = GetPointAxisFromIndex(_xChanel, _startIndex);

                        _ZoomStartTest = new PointF(_lsAllRectangelF[i].X, _lsAllRectangelF[i].Y);
                        return;
                    }
                }

            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if(m_down)
            {
                bitMouseMove = new Bitmap(this.Width, this.Height);
                MouseMoveDraw = Graphics.FromImage(bitMouseMove);
                Pen p = new Pen(Color.LightGray, 1.5f);
                p.DashStyle = DashStyle.Dash;

                if(e.X < _zoomStartPoint.X && e.Y < _zoomStartPoint.Y)
                    MouseMoveDraw.DrawRectangle(p, e.X, e.Y, System.Math.Abs(e.X - _zoomStartPoint.X), System.Math.Abs(e.Y - _zoomStartPoint.Y));
                else if(e.X > _zoomStartPoint.X && e.Y < _zoomStartPoint.Y)
                    MouseMoveDraw.DrawRectangle(p, _zoomStartPoint.X, e.Y, System.Math.Abs(e.X - _zoomStartPoint.X), System.Math.Abs(e.Y - _zoomStartPoint.Y));
                else if(e.X < _zoomStartPoint.X && e.Y > _zoomStartPoint.Y)
                    MouseMoveDraw.DrawRectangle(p, e.X, _zoomStartPoint.Y, System.Math.Abs(e.X - _zoomStartPoint.X), System.Math.Abs(e.Y - _zoomStartPoint.Y));
                else
                    MouseMoveDraw.DrawRectangle(p, _zoomStartPoint.X, _zoomStartPoint.Y, System.Math.Abs(e.X - _zoomStartPoint.X), System.Math.Abs(e.Y - _zoomStartPoint.Y));

                MouseMoveDraw.Dispose();

                p.Dispose();
                this.Invalidate();
            }

        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if(_playStatus == emPlayStatus.Stop)
                return;

            if(_xChanel == 0 || _yChanel == 0)
                return;

            if(_zoomStartPoint == new PointF(0, 0))
                return;

            bitMouseMove = new Bitmap(this.Width, this.Height);
            MouseMoveDraw = Graphics.FromImage(bitMouseMove);
            MouseMoveDraw.Clear(Color.Transparent);

            MouseMoveDraw.Dispose();
            m_down = false;
            this.Invalidate();

            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                _zoomEndPoint = e.Location;
                if(_zoomStartPoint == _zoomEndPoint)
                    return;

                if(_zoomEndPoint.X < _zoomStartPoint.X || _zoomEndPoint.Y < _zoomStartPoint.Y)
                {
                    _zoomStatus = emZoomStatus.Normal;
                    if(_playStatus == emPlayStatus.Pause || _playStatus == emPlayStatus.Stop)
                        ShowColorGridData(_xChanel, _yChanel, _lsRawData);
                }
                else
                {
                    if(_zoomStatus == emZoomStatus.ZoomIn)
                        return;

                    for(int i = 0; i < _lsAllRectangelF.Count; i++)
                    {
                        if(_lsAllRectangelF[i].Contains(_zoomEndPoint))
                        {
                            _ZoomEndTest = new PointF(_lsAllRectangelF[i].X + _lsAllRectangelF[i].Width
                                                    , _lsAllRectangelF[i].Y + _lsAllRectangelF[i].Height);
                            _endIndex = i;
                            break;
                        }
                    }
                    _end = GetPointAxisFromIndex(_xChanel, _endIndex);
                    //Console.WriteLine("_endIndex = {0}", _endIndex);
                    _zoomStatus = emZoomStatus.ZoomIn;
                    if(_playStatus == emPlayStatus.Pause || _playStatus == emPlayStatus.Stop)
                        ShowColorGridData(_xChanel, _yChanel, _lsRawData);
                }
                _zoomStartPoint = new Point(0, 0);
            }
        }

        protected override void OnPaint(PaintEventArgs _event)
        {
            BufferedGraphicsContext current = BufferedGraphicsManager.Current;
            //Bitmap bitmapCDC = new Bitmap(this.Width, this.Height);
            //Graphics bitDrawer = Graphics.FromImage(bitmapCDC);
            //Rectangle _rec = new Rectangle(0, 0, _event.ClipRectangle.Width - 243, _event.ClipRectangle.Height - 15);
            Rectangle resultRect = new Rectangle(_event.ClipRectangle.Location, _event.ClipRectangle.Size);
            BufferedGraphics bg = current.Allocate(_event.Graphics, resultRect);
            Graphics grp = bg.Graphics;
            grp.Clear(Color.White);

            //BufferedGraphics bgCDC = current.Allocate(Graphics.FromImage(bitmapCDC), resultRect);
            //Graphics bitDrawer = bgCDC.Graphics;
            //bitDrawer.Clear(Color.White);

            if(mi_DrawObjects.Count == 0)
            {
                //grp.DrawImage(bitmapCDC, 0, 0);
                grp.DrawImage(bitmapDrawLine, _iOffset, 0);
                bg.Render();
                bg.Dispose();
                ClearPaintingBitmap();
                return;
            }

            //Bitmap bitmapAxis = new Bitmap(this.Width, this.Height);
            //Graphics bitAxisDrawer = Graphics.FromImage(bitmapAxis);


            //Stopwatch sw = new Stopwatch();
            //sw.Reset();
            //sw = Stopwatch.StartNew();
            foreach(var item in mi_DrawObjects)
            {
                switch(item.DrawType)
                {
                    case DrawType.DrawLine:
                        break;

                    case DrawType.Fillrectangle:

                        int iValue = Convert.ToInt32(item.Text);

                        var _SetVal = (iValue - _iMutual_Min) / _PaletteStep;
                        if(_SetVal > BrushSelect.Length - 1)
                            _SetVal = BrushSelect.Length - 1;
                        grp.FillRectangle(BrushSelect[(int)(_SetVal)], item.Rect);

                        if(_isShowMarks)
                            grp.DrawString(item.Text, item.FontStyle, item.SolidBrush, item.Rect.X + (item.Rect.Width / 2), item.Rect.Y + (item.Rect.Height / 2), item.StringFormat);

                        break;

                    case DrawType.DrawAxis:
                        int iMaxChannel = Math.Max(_xChanel, _yChanel);
                        int iInterval = 1;

                        if(iMaxChannel < 30)
                            iInterval = 1;
                        else if(iMaxChannel < 60)
                            iInterval = 2;
                        else if(iMaxChannel < 120)
                            iInterval = 5;
                        else
                            iInterval = 10;

                        if(Convert.ToInt32(item.Text) % iInterval == 0 || _zoomStatus == emZoomStatus.ZoomIn)
                        {
                            grp.FillRectangle(backgroundBrush, item.Rect);
                            grp.DrawString(item.Text, item.FontStyle, item.SolidBrush, item.Rect.X + (item.Rect.Width / 2), item.Rect.Y + (item.Rect.Height / 2), item.StringFormat);
                        }

                        break;

                    default:
                        break;
                }
            }

            //sw.Stop();
            //long ms = sw.ElapsedMilliseconds;
            //Console.WriteLine("{1} : 花費 {0} 毫秒", ms, this.Name);
            //bgCDC.Render();

            //grp.DrawImage(bitmapCDC, 0, 0);
            //grp.DrawImage(bitmapAxis, 0, 0);


            if(_zoomStatus == emZoomStatus.ZoomIn)
            {

                Bitmap tmpBmpPainting = new Bitmap(this.Width - (int)_iOffset, this.Height - (int)_iOffset);
                Graphics tmpGPainting = Graphics.FromImage(tmpBmpPainting);

                tmpGPainting.InterpolationMode = InterpolationMode.HighQualityBicubic;
                tmpGPainting.SmoothingMode = SmoothingMode.HighQuality;
                tmpGPainting.CompositingQuality = CompositingQuality.HighQuality;

                tmpGPainting.DrawImage(bitmapDrawLine, new RectangleF(0f, 0f, (float)bitmapDrawLine.Width, (float)bitmapDrawLine.Height),
                                         new RectangleF(_ZoomStartTest.X - _iOffset, _ZoomStartTest.Y, (_ZoomEndTest.X - _ZoomStartTest.X),
                                                                                            (_ZoomEndTest.Y - _ZoomStartTest.Y)),
                                         GraphicsUnit.Pixel);


                //grp.DrawImage(tmpBmp, _iOffset, 0);
                grp.DrawImage(tmpBmpPainting, _iOffset, 0);
                tmpGPainting.Dispose();
                //tmpG.Dispose();
            }
            else
            {
                grp.DrawImage(bitmapDrawLine, _iOffset, 0);
            }

            if(bitMouseMove != null)
                grp.DrawImage(bitMouseMove, 0, 0);

            bg.Render();
            bg.Dispose();
            //bitDrawer.Dispose();

            //bgCDC.Dispose();
            //bitmapAxis.Dispose();
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
            //e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, 0, 0, 0)), this.ClientRectangle);
        }

        private int GetIndexsFromPoint(int x, Point p)
        {
            return (p.Y - 1) * x + p.X - 1;
        }

        private Point GetPointAxisFromIndex(int x, int index)
        {
            return new Point((index % x) + 1, (index / x) + 1);
        }

        private void SetAxis(int x, int y, float rectWidth, float rectHeight)
        {
            SolidBrush brush = new SolidBrush(Color.Black);
            int xAxisOffset = 0;
            int yAxisOffset = 0;
            //Pen pen = new Pen(Color.Black, 2);
            float StartX_L = (_axisDirection == emDirection.Left) ? _iOffset : 0; //左下第一格的左上角
            float StartY_L = this.Height - _iOffset - rectHeight; //左下第一格的左上角

            float StartX_R = this.Width - _iOffset;
            float StartY_R = this.Height - _iOffset - rectHeight;
            float fActualRectSize = 0;

            if(_zoomStatus == emZoomStatus.Normal)
            {
                xAxisOffset = 0;
                yAxisOffset = 0;
            }
            else
            {
                //xAxisOffset = (_xInverted) ? _lsXAxisAfterZoom.Max() : _lsXAxisAfterZoom.Min();
                //yAxisOffset = (_yInverted) ? _lsYAxisAfterZoom.Max() : _lsYAxisAfterZoom.Min();
                xAxisOffset = _lsXAxisAfterZoom.Min();
                yAxisOffset = _lsYAxisAfterZoom.Min();
                xAxisOffset = xAxisOffset - 1;
                yAxisOffset = yAxisOffset - 1;
            }

            if(_xInverted)
            {
                int txtNum = x + xAxisOffset;
                for(int i = 0; i < x; i++)
                {
                    RectangleF rect1 = new RectangleF(StartX_L + (i * rectWidth), StartY_L + rectHeight, rectWidth, (_iOffset / 2));
                    fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                    fActualRectSize = fActualRectSize * _fontAxisSizeSetting;

                    if(fActualRectSize < 8)
                        fontAxis = new Font("Consolas", 8, FontStyle.Regular, GraphicsUnit.Point);
                    else
                        fontAxis = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                    mi_DrawObjects.Add(new DrawingObject(DrawType.DrawAxis, null, brush, stringFormat, rect1, fontAxis, txtNum--));
                }
            }
            else
            {
                for(int i = 0; i < x; i++)
                {
                    RectangleF rect1 = new RectangleF(StartX_L + (i * rectWidth), StartY_L + rectHeight, rectWidth, (_iOffset / 2));
                    fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                    fActualRectSize = fActualRectSize * _fontAxisSizeSetting;

                    if(fActualRectSize < 8)
                        fontAxis = new Font("Consolas", 8, FontStyle.Regular, GraphicsUnit.Point);
                    else
                        fontAxis = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                    mi_DrawObjects.Add(new DrawingObject(DrawType.DrawAxis, null, brush, stringFormat, rect1, fontAxis, (i + 1 + xAxisOffset)));
                }
            }

            if(_axisDirection == emDirection.Left)//字顯示在左邊
            {
                if(_yInverted)
                {
                    int txtNum = y + yAxisOffset;
                    for(int i = 0; i < y; i++)
                    {
                        RectangleF rect1 = new RectangleF((StartX_L - (_iOffset / 2)), StartY_L - (i * rectHeight), (_iOffset / 2), rectHeight);

                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontAxisSizeSetting;


                        if(fActualRectSize < 8)
                            fontAxis = new Font("Consolas", 8, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            fontAxis = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        mi_DrawObjects.Add(new DrawingObject(DrawType.DrawAxis, null, brush, stringFormat, rect1, fontAxis, txtNum--));
                    }
                }
                else
                {
                    for(int i = 0; i < y; i++)
                    {
                        RectangleF rect1 = new RectangleF((StartX_L - (_iOffset / 2)), StartY_L - (i * rectHeight), (_iOffset / 2), rectHeight);

                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontAxisSizeSetting;

                        if(fActualRectSize < 8)
                            fontAxis = new Font("Consolas", 8, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            fontAxis = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        mi_DrawObjects.Add(new DrawingObject(DrawType.DrawAxis, null, brush, stringFormat, rect1, fontAxis, (i + 1 + yAxisOffset)));
                    }
                }
            }
            else//字顯示在右邊
            {
                if(_yInverted)
                {
                    int txtNum = y + yAxisOffset;
                    for(int i = 0; i < y; i++)
                    {
                        RectangleF rect1 = new RectangleF(StartX_R, StartY_R - (i * rectHeight), (_iOffset / 2), rectHeight);

                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontAxisSizeSetting;

                        if(fActualRectSize < 8)
                            fontAxis = new Font("Consolas", 8, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            fontAxis = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        mi_DrawObjects.Add(new DrawingObject(DrawType.DrawAxis, null, brush, stringFormat, rect1, fontAxis, txtNum--));
                    }
                }
                else
                {
                    for(int i = 0; i < y; i++)
                    {
                        RectangleF rect1 = new RectangleF(StartX_R, StartY_R - (i * rectHeight), (_iOffset / 2), rectHeight);

                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontAxisSizeSetting;

                        if(fActualRectSize < 8)
                            fontAxis = new Font("Consolas", 8, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            fontAxis = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        mi_DrawObjects.Add(new DrawingObject(DrawType.DrawAxis, null, brush, stringFormat, rect1, fontAxis, (i + 1 + yAxisOffset)));
                    }
                }
            }

            //TitleRegion = new Rectangle();
            //foreach(var item in mi_DrawObjects)
            //{
            //    TitleRegion.X += (int)(item.Rect.X + 0.5);
            //    TitleRegion.Y += (int)(item.Rect.Y + 0.5);
            //    TitleRegion.Width += (int)(item.Rect.Width + 0.5);
            //    TitleRegion.Height += (int)(item.Rect.Height + 0.5);
            //}            
        }

        private void ShowColorGrid_Normal(List<short> data)
        {
            if(data.Count == 0)
            {
                _zoomStatus = emZoomStatus.Normal;
                return;
            }

            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black, 2);
            //每一格子的X & Y 大小 / 
            float rectwidth = (float)(this.Width - _iOffset) / (float)_xChanel;
            float rectheight = (float)(this.Height - _iOffset) / (float)_yChanel;
            //if(bNeedUpdateAxis)            
            SetAxis(_xChanel, _yChanel, rectwidth, rectheight);
            float fActualRectSize = 0;

            if(_xInverted && _yInverted)
            {
                float StartX = (_axisDirection == emDirection.Left) ? this.Width - rectwidth : this.Width - rectwidth - _iOffset; //左下第一格的左上角
                float StartY = 0; //左下第一格的左上角

                for(int i = 0; i < _yChanel; i++)
                {
                    for(int j = 0; j < _xChanel; j++)
                    {
                        int id = i * _xChanel + j;
                        RectangleF rect1 = new RectangleF(StartX - (j * rectwidth), StartY + (i * rectheight), rectwidth, rectheight);
                        _lsAllRectangelF.Add(rect1);
                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontSizeSetting;
                        if(fActualRectSize > 12)
                            font1 = new Font("Consolas", 12, FontStyle.Regular, GraphicsUnit.Point);
                        else if(fActualRectSize < 6)
                            font1 = new Font("Consolas", 6, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            font1 = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, data[id]));
                    }
                }
            }
            else if(_xInverted)
            {
                float StartX = (_axisDirection == emDirection.Left) ? this.Width - rectwidth : this.Width - rectwidth - _iOffset; //左下第一格的左上角
                float StartY = this.Height - _iOffset - rectheight; //左下第一格的左上角

                for(int i = 0; i < _yChanel; i++)
                {
                    for(int j = 0; j < _xChanel; j++)
                    {
                        int id = i * _xChanel + j;
                        RectangleF rect1 = new RectangleF(StartX - (j * rectwidth), StartY - (i * rectheight), rectwidth, rectheight);
                        _lsAllRectangelF.Add(rect1);
                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontSizeSetting;
                        if(fActualRectSize > 12)
                            font1 = new Font("Consolas", 12, FontStyle.Regular, GraphicsUnit.Point);
                        else if(fActualRectSize < 6)
                            font1 = new Font("Consolas", 6, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            font1 = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, data[id]));
                    }
                }
            }
            else if(_yInverted)
            {
                float StartX = (_axisDirection == emDirection.Left) ? _iOffset : 0; //左上第一格的左上角
                float StartY = 0; //左上第一格的左上角

                for(int i = 0; i < _yChanel; i++)
                {
                    for(int j = 0; j < _xChanel; j++)
                    {
                        int id = i * _xChanel + j;
                        RectangleF rect1 = new RectangleF(StartX + (j * rectwidth), StartY + (i * rectheight), rectwidth, rectheight);
                        _lsAllRectangelF.Add(rect1);
                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontSizeSetting;
                        if(fActualRectSize > 12)
                            font1 = new Font("Consolas", 12, FontStyle.Regular, GraphicsUnit.Point);
                        else if(fActualRectSize < 6)
                            font1 = new Font("Consolas", 6, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            font1 = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, data[id]));
                    }
                }
            }
            else
            {
                float StartX = (_axisDirection == emDirection.Left) ? _iOffset : 0; //右下第一格的左上角
                float StartY = this.Height - _iOffset - rectheight; //右下第一格的左上角

                for(int i = 0; i < _yChanel; i++)
                {
                    for(int j = 0; j < _xChanel; j++)
                    {
                        int id = i * _xChanel + j;
                        RectangleF rect1 = new RectangleF(StartX + (j * rectwidth), StartY - (i * rectheight), rectwidth, rectheight);
                        _lsAllRectangelF.Add(rect1);
                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontSizeSetting;
                        if(fActualRectSize > 12)
                            font1 = new Font("Consolas", 12, FontStyle.Regular, GraphicsUnit.Point);
                        else if(fActualRectSize < 6)
                            font1 = new Font("Consolas", 6, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            font1 = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, data[id]));
                    }
                }
            }
        }

        private void ShowColorGrid_ZoomIn(List<short> data)
        {
            if(data.Count == 0)
            {
                _zoomStatus = emZoomStatus.Normal;
                return;
            }

            int xRange = Math.Abs(_start.X - _end.X) + 1;
            int yRange = Math.Abs(_start.Y - _end.Y) + 1;

            SolidBrush brush = new SolidBrush(Color.Black);
            Pen pen = new Pen(Color.Black, 2);
            //每一格子的X & Y 大小 / 

            //float rectwidth = (float)(this.Width - _iOffset) / (float)_xChanel;
            //float rectheight = (float)(this.Height - _iOffset) / (float)_yChanel;


            float rectwidth = (float)(this.Width - _iOffset) / (float)xRange;
            float rectheight = (float)(this.Height - _iOffset) / (float)yRange;
            _lsXAxisAfterZoom.Clear();
            _lsYAxisAfterZoom.Clear();
            _lsIndexAfterZoom.Clear();
            float fActualRectSize = 0;
            //X軸數值
            if(_start.X > _end.X)
                _lsXAxisAfterZoom.AddRange(Enumerable.Range(_end.X, _start.X - _end.X + 1).ToArray());
            else
                _lsXAxisAfterZoom.AddRange(Enumerable.Range(_start.X, _end.X - _start.X + 1).ToArray());

            //Console.WriteLine("_lsXAxisAfterZoom = {0}", string.Join(",", _lsXAxisAfterZoom.Select(x => x.ToString())));

            //Y軸數值
            if(_start.Y > _end.Y)
                _lsYAxisAfterZoom.AddRange(Enumerable.Range(_end.Y, _start.Y - _end.Y + 1).ToArray());
            else
                _lsYAxisAfterZoom.AddRange(Enumerable.Range(_start.Y, _end.Y - _start.Y + 1).ToArray());
            //Console.WriteLine("_lsYAxisAfterZoom = {0}", string.Join(",", _lsYAxisAfterZoom.Select(x => x.ToString())));

            //互容數值
            for(int i = 0; i < _lsYAxisAfterZoom.Count; i++)
            {
                for(int j = 0; j < _lsXAxisAfterZoom.Count; j++)
                {
                    int iData = GetIndexsFromPoint(_xChanel, new Point(_lsXAxisAfterZoom[j], _lsYAxisAfterZoom[i]));
                    if(iData < 0)
                    {
                        _zoomStatus = emZoomStatus.Normal;
                        if(_playStatus == emPlayStatus.Pause || _playStatus == emPlayStatus.Stop)
                            ShowColorGridData(_xChanel, _yChanel, _lsRawData);

                        return;
                    }

                    _lsIndexAfterZoom.Add(iData);
                }
            }
            _lsIndexAfterZoom.Sort();

            //在mi_DrawObjects設定軸的數值
            //if(bNeedUpdateAxis)
            SetAxis((int)xRange, (int)yRange, rectwidth, rectheight);


            //if (_xInverted && _yInverted)
            //{
            //    float StartX = (_axisDirection == emDirection.Left) ? this.Width - rectwidth : this.Width - rectwidth - _iOffset; //左下第一格的左上角
            //    float StartY = 0; //左下第一格的左上角

            //    for (int i = 0; i < _yChanel; i++)
            //    {
            //        for (int j = 0; j < _xChanel; j++)
            //        {
            //            int id = i * _xChanel + j;
            //            RectangleF rect1 = new RectangleF(StartX - (j * rectwidth), StartY + (i * rectheight), rectwidth, rectheight);
            //            _lsAllRectangelF.Add(rect1);
            //            fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
            //            fActualRectSize = fActualRectSize * _fontSizeSetting;
            //            if (fActualRectSize > 12)
            //                font1 = new Font("Consolas", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            //            else if (fActualRectSize < 6)
            //                font1 = new Font("Consolas", 6, FontStyle.Regular, GraphicsUnit.Pixel);
            //            else
            //                font1 = new Font("Consolas", fActualRectSize * _fontSizeSetting, FontStyle.Regular, GraphicsUnit.Pixel);
            //            mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, data[id]));
            //        }
            //    }
            //}
            //else if (_xInverted)
            //{
            //    float StartX = (_axisDirection == emDirection.Left) ? this.Width - rectwidth : this.Width - rectwidth - _iOffset; //左下第一格的左上角
            //    float StartY = this.Height - _iOffset - rectheight; //左下第一格的左上角

            //    for (int i = 0; i < _yChanel; i++)
            //    {
            //        for (int j = 0; j < _xChanel; j++)
            //        {
            //            int id = i * _xChanel + j;
            //            RectangleF rect1 = new RectangleF(StartX - (j * rectwidth), StartY - (i * rectheight), rectwidth, rectheight);
            //            _lsAllRectangelF.Add(rect1);
            //            fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
            //            fActualRectSize = fActualRectSize * _fontSizeSetting;
            //            if (fActualRectSize > 12)
            //                font1 = new Font("Consolas", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            //            else if (fActualRectSize < 6)
            //                font1 = new Font("Consolas", 6, FontStyle.Regular, GraphicsUnit.Pixel);
            //            else
            //                font1 = new Font("Consolas", fActualRectSize * _fontSizeSetting, FontStyle.Regular, GraphicsUnit.Pixel);
            //            mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, data[id]));
            //        }
            //    }
            //}
            //else if (_yInverted)
            //{
            //    float StartX = (_axisDirection == emDirection.Left) ? _iOffset : 0; //左上第一格的左上角
            //    float StartY = 0; //左上第一格的左上角

            //    for (int i = 0; i < _yChanel; i++)
            //    {
            //        for (int j = 0; j < _xChanel; j++)
            //        {
            //            int id = i * _xChanel + j;
            //            RectangleF rect1 = new RectangleF(StartX + (j * rectwidth), StartY + (i * rectheight), rectwidth, rectheight);
            //            _lsAllRectangelF.Add(rect1);
            //            fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
            //            fActualRectSize = fActualRectSize * _fontSizeSetting;
            //            if (fActualRectSize > 12)
            //                font1 = new Font("Consolas", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            //            else if (fActualRectSize < 6)
            //                font1 = new Font("Consolas", 6, FontStyle.Regular, GraphicsUnit.Pixel);
            //            else
            //                font1 = new Font("Consolas", fActualRectSize * _fontSizeSetting, FontStyle.Regular, GraphicsUnit.Pixel);
            //            mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, data[id]));
            //        }
            //    }
            //}
            //else
            //{
            //    float StartX = (_axisDirection == emDirection.Left) ? _iOffset : 0; //右下第一格的左上角
            //    float StartY = this.Height - _iOffset - rectheight; //右下第一格的左上角

            //    for (int i = 0; i < _yChanel; i++)
            //    {
            //        for (int j = 0; j < _xChanel; j++)
            //        {
            //            int id = i * _xChanel + j;
            //            RectangleF rect1 = new RectangleF(StartX + (j * rectwidth), StartY - (i * rectheight), rectwidth, rectheight);
            //            _lsAllRectangelF.Add(rect1);
            //            fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
            //            fActualRectSize = fActualRectSize * _fontSizeSetting;
            //            if (fActualRectSize > 12)
            //                font1 = new Font("Consolas", 12, FontStyle.Regular, GraphicsUnit.Pixel);
            //            else if (fActualRectSize < 6)
            //                font1 = new Font("Consolas", 6, FontStyle.Regular, GraphicsUnit.Pixel);
            //            else
            //                font1 = new Font("Consolas", fActualRectSize * _fontSizeSetting, FontStyle.Regular, GraphicsUnit.Pixel);
            //            mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, data[id]));
            //        }
            //    }
            //}


            List<short> lsTempData = new List<short>();
            for(int i = 0; i < _lsIndexAfterZoom.Count; i++)
            {
                lsTempData.Add(data[_lsIndexAfterZoom[i]]);
            }
            //foreach(var index in _lsIndexAfterZoom)
            //    lsTempData.Add(data[index]);

            if(_xInverted && _yInverted)
            {
                float StartX = (_axisDirection == emDirection.Left) ? this.Width - rectwidth : this.Width - rectwidth - _iOffset; //左下第一格的左上角
                float StartY = 0; //左下第一格的左上角

                for(int i = 0; i < yRange; i++)
                {
                    for(int j = 0; j < xRange; j++)
                    {
                        int id = i * xRange + j;
                        RectangleF rect1 = new RectangleF(StartX - (j * rectwidth), StartY + (i * rectheight), rectwidth, rectheight);
                        _lsAllRectangelF.Add(rect1);
                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontSizeSetting;
                        if(fActualRectSize > 20)
                            font1 = new Font("Consolas", 20, FontStyle.Regular, GraphicsUnit.Point);
                        else if(fActualRectSize < 10)
                            font1 = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            font1 = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        //mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, lsTempData[id].ToString(), lsTempData[id]));
                        mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, lsTempData[id]));
                    }
                }
            }
            else if(_xInverted)
            {
                float StartX = (_axisDirection == emDirection.Left) ? this.Width - rectwidth : this.Width - rectwidth - _iOffset; //左下第一格的左上角
                float StartY = this.Height - _iOffset - rectheight; //左下第一格的左上角

                for(int i = 0; i < yRange; i++)
                {
                    for(int j = 0; j < xRange; j++)
                    {
                        int id = i * xRange + j;
                        RectangleF rect1 = new RectangleF(StartX - (j * rectwidth), StartY - (i * rectheight), rectwidth, rectheight);
                        _lsAllRectangelF.Add(rect1);
                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontSizeSetting;
                        if(fActualRectSize > 20)
                            font1 = new Font("Consolas", 20, FontStyle.Regular, GraphicsUnit.Point);
                        else if(fActualRectSize < 10)
                            font1 = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            font1 = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        //mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, lsTempData[id].ToString(), lsTempData[id]));
                        mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, lsTempData[id]));
                    }
                }
            }
            else if(_yInverted)
            {
                float StartX = (_axisDirection == emDirection.Left) ? _iOffset : 0; //左上第一格的左上角
                float StartY = 0; //左上第一格的左上角

                for(int i = 0; i < yRange; i++)
                {
                    for(int j = 0; j < xRange; j++)
                    {
                        int id = i * xRange + j;

                        RectangleF rect1 = new RectangleF(StartX + (j * rectwidth), StartY + (i * rectheight), rectwidth, rectheight);
                        _lsAllRectangelF.Add(rect1);
                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontSizeSetting;
                        if(fActualRectSize > 20)
                            font1 = new Font("Consolas", 20, FontStyle.Regular, GraphicsUnit.Point);
                        else if(fActualRectSize < 10)
                            font1 = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            font1 = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        //mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, lsTempData[id].ToString(), lsTempData[id]));
                        mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, lsTempData[id]));
                    }
                }
            }
            else
            {
                float StartX = (_axisDirection == emDirection.Left) ? _iOffset : 0; //右下第一格的左上角
                float StartY = this.Height - _iOffset - rectheight; //右下第一格的左上角

                for(int i = 0; i < yRange; i++)
                {
                    for(int j = 0; j < xRange; j++)
                    {
                        int id = i * xRange + j;

                        RectangleF rect1 = new RectangleF(StartX + (j * rectwidth), StartY - (i * rectheight), rectwidth, rectheight);
                        _lsAllRectangelF.Add(rect1);
                        fActualRectSize = (rect1.Width >= rect1.Height) ? rect1.Height : rect1.Width;
                        fActualRectSize = fActualRectSize * _fontSizeSetting;
                        if(fActualRectSize > 20)
                            font1 = new Font("Consolas", 20, FontStyle.Regular, GraphicsUnit.Point);
                        else if(fActualRectSize < 10)
                            font1 = new Font("Consolas", 10, FontStyle.Regular, GraphicsUnit.Point);
                        else
                            font1 = new Font("Consolas", fActualRectSize, FontStyle.Regular, GraphicsUnit.Point);
                        //Console.WriteLine("id:{0}", id);
                        //mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, lsTempData[id].ToString(), lsTempData[id]));
                        mi_DrawObjects.Add(new DrawingObject(DrawType.Fillrectangle, pen, brush, stringFormat, rect1, font1, lsTempData[id]));
                    }
                }
            }
        }

        private class DrawingObject
        {
            private short colorlevel;
            private Font fontStyle;
            private List<Point> lsAllPointData = new List<Point>();
            private Pen mDrawLinePen;
            private Pen mDrawTextPen;
            private DrawType mDrawType;
            private SolidBrush mSolidBrush;

            private RectangleF rect;
            private StringFormat stringFormat;
            private string text;
            //SolidBrush solidBackColor = new SolidBrush(Color.LightGreen);

            //public DrawingObject(DrawType inDrawType, Pen inDrawPen, List<Point> inAllPointData)
            //{
            //    mDrawType = inDrawType;
            //    mDrawLinePen = inDrawPen;
            //    lsAllPointData = inAllPointData;
            //}

            //public DrawingObject(DrawType inDrawType, Pen inDrawPen, SolidBrush inSolidBrush, StringFormat inStringFormat, RectangleF inRect, Font inFontStyle, string inText, short inColorLevel)
            //{
            //    mDrawType = inDrawType;
            //    mDrawLinePen = inDrawPen;
            //    mSolidBrush = inSolidBrush;
            //    stringFormat = inStringFormat;
            //    rect = inRect;
            //    fontStyle = inFontStyle;
            //    text = inText;
            //    colorlevel = (inColorLevel == short.MinValue) ? (short)(Math.Abs((int)inColorLevel) - 1) : Math.Abs(inColorLevel);
            //}
            public DrawingObject(DrawType inDrawType, Pen inDrawPen, SolidBrush inSolidBrush, StringFormat inStringFormat, RectangleF inRect, Font inFontStyle, int inColorLevel)
            {
                mDrawType = inDrawType;
                mDrawLinePen = inDrawPen;
                mSolidBrush = inSolidBrush;
                stringFormat = inStringFormat;
                rect = inRect;
                fontStyle = inFontStyle;
                colorlevel = (inColorLevel == short.MinValue) ? (short)(Math.Abs(inColorLevel) - 1) : (short)Math.Abs(inColorLevel);
                text = inColorLevel.ToString();
            }

            //public DrawingObject(DrawType inDrawType, SolidBrush inSolidBrush, List<Point> inAllPointData)
            //{
            //    mDrawType = inDrawType;
            //    mSolidBrush = inSolidBrush;
            //    lsAllPointData = inAllPointData;
            //}

            public short ColorLevel
            {
                get
                {
                    return colorlevel;
                }
            }

            public Pen DrawLinePen
            {
                get
                {
                    return mDrawLinePen;
                }
                set
                {
                    mDrawLinePen = value;
                }
            }

            public Pen DrawTextPen
            {
                get
                {
                    return mDrawTextPen;
                }
                set
                {
                    mDrawTextPen = value;
                }
            }

            public DrawType DrawType
            {
                get
                {
                    return mDrawType;
                }
                set
                {
                    mDrawType = value;
                }
            }

            public Font FontStyle
            {
                get
                {
                    return fontStyle;
                }

            }

            public RectangleF Rect
            {
                get
                {
                    return rect;
                }

            }

            public SolidBrush SolidBrush
            {
                get
                {
                    return mSolidBrush;
                }
                set
                {
                    mSolidBrush = value;
                }
            }

            public StringFormat StringFormat
            {
                get
                {
                    return stringFormat;
                }
            }

            public string Text
            {
                get
                {
                    return text;
                }

            }
        }
    }

    public class ColorMap
    {
        private float _minTemp;
        private float _maxTemp;
        private Color[] _colors;

        public void MapColors(Color[] colors, int minTemp, int maxTemp)
        {
            _colors = colors;
            _minTemp = minTemp;
            _maxTemp = maxTemp;
        }

        public Color GetColorFromTemperature(int temp)
        {
            if(_colors == null || _colors.Length < 2 || temp <= _minTemp)
            {
                return _colors[0];
            }
            if(temp >= _maxTemp)
            {
                return _colors[_colors.Length - 1];
            }

            float interval = (_maxTemp - _minTemp) / (float)(_colors.Length - 1);
            int index = (int)((temp - _minTemp) / interval);
            float frac = (temp - _minTemp) % interval / interval;

            Color c1 = _colors[index];
            Color c2 = _colors[index + 1];

            int a = (int)(c1.A + (c2.A - c1.A) * frac);
            int r = (int)(c1.R + (c2.R - c1.R) * frac);
            int g = (int)(c1.G + (c2.G - c1.G) * frac);
            int b = (int)(c1.B + (c2.B - c1.B) * frac);

            return Color.FromArgb(a, r, g, b);
        }

        public float MaxValue
        {
            set
            {
                _maxTemp = value;
            }
            get
            {
                return _maxTemp;
            }
        }
        public float MinValue
        {
            set
            {
                _minTemp = value;
            }
            get
            {
                return _minTemp;
            }
        }
    }
}