using SIE.Core.QmsStaticConst;

namespace SIE.Web.Core.QmsStaticConst
{
    /// <summary>
    /// d2*表视图配置
    /// </summary>
    internal class StaticConstD2ViewConfig : WebViewConfig<StaticConstD2>
    {


        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseGridSelectionModel(isCheckboxmodel: true, mode: "SINGLE", checkOnly: false, injectCheckbox: 0);
            View.WithoutPaging();
            View.ClearCommands();
            View.AddBehavior("SIE.Web.Core.QmsStaticConst.Behaviors.StaticConstD2Behavior");
            View.UseCommands("SIE.Web.Core.QmsStaticConst.Commands.D2.AddRow", "SIE.Web.Core.QmsStaticConst.Commands.D2.DeleteRow", "SIE.Web.Core.QmsStaticConst.Commands.D2.AddColumn", "SIE.Web.Core.QmsStaticConst.Commands.D2.DeleteColumn");
            View.Property(p => p.SampleQty).DisableSort();
            View.Property(p => p.TestQty).DisableSort();
            View.Property(p => p.Value).DisableSort();
            View.Property(p => p.MsaConstD2Type).DisableSort();
            View.Property(p => p.CreateByName).DisableSort();
            View.Property(p => p.CreateDate).DisableSort();
            View.Property(p => p.UpdateByName).DisableSort();
            View.Property(p => p.UpdateDate).DisableSort();
        }
    }
}