using System.Reflection;
using System.Runtime.InteropServices;

// 組件的一般資訊是由下列的屬性集控制。
// 變更這些屬性的值即可修改組件的相關
// 資訊。
[assembly: AssemblyTitle("ITS Studio")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("ILITek")]
[assembly: AssemblyProduct("ITS Studio")]
[assembly: AssemblyCopyright("Copyright © ILITek 2024")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// 將 ComVisible 設定為 false 會使得這個組件中的型別
// 對 COM 元件而言為不可見。如果您需要從 COM 存取這個組件中
// 的型別，請在該型別上將 ComVisible 屬性設定為 true。
[assembly: ComVisible(false)]

// 下列 GUID 為專案公開 (Expose) 至 COM 時所要使用的 typelib ID
[assembly: Guid("eb769508-0414-4cf8-8cdc-fc6249352ae8")]

// 組件的版本資訊是由下列四項值構成:
//
//      主要版本
//      次要版本
//      組建編號
//      修訂編號
//
// 您可以指定所有的值，也可以依照以下的方式，使用 '*' 將組建和修訂編號
// 指定為預設值:
// [assembly: AssemblyVersion("1.0.*")]
//express: ("cumstom_tag_high, cumstom_tag_low, cumstom_ver_high, cumstom_version_low")
//cumstom_tag: 使用10進制ascii表示客戶代碼
//custom_version:
//ex:AUO(00) versoin V0.1 :{48.48.0.1}.
//cumstom tag list:
//AUO(00) <=={48.48. x. x}.
//default version <== {0,0,0,0}
[assembly: AssemblyVersion("0.0.0.0")]
//[assembly: AssemblyFileVersion("1.0.11.0")]