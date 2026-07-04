using SIE.MES.ProcessProperty;
using SIE.MetaModel.View;

namespace SIE.Web.MES.ProcessProperty
{
    /// <summary>
    /// 工序参数数采界面
    /// </summary>
    public class ProcessParamCollectViewConfig : WebViewConfig<ProcessParamCollect>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls);
            View.Property(p => p.SN).ShowInList(150);
            View.Property(p => p.Time).ShowInList(150);
            View.Property(p => p.ProcessFlowCode).Show();
            View.Property(p => p.ProcessCode).ShowInList(150);
            View.Property(p => p.ProcessName).ShowInList(150);
            View.Property(p => p.Factory).Show();
            View.Property(p => p.EquipmentCode).ShowInList(150);
            View.Property(p => p.EquipmentName).ShowInList(150);
            View.Property(p => p.QualityStatus).Show();
            View.ChildrenProperty(p => p.ProcessParamCollectParamList).Show(ChildShowInWhere.All);
            View.ChildrenProperty(p => p.ProcessParamCollectComponentList).Show(ChildShowInWhere.All);
        }
    }
}
