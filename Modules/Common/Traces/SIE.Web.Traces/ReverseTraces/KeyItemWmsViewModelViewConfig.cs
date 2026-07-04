using Newtonsoft.Json;
using SIE.Traces.ForwardTraces;
using SIE.Traces.ReverseTraces;
using SIE.Web.Traces.ForwardTraces;

namespace SIE.Web.Traces.ReverseTraces
{
    /// <summary>
    /// 反向追溯-工序采集-关键件-入库明细视图配置
    /// </summary>
    internal class KeyItemWmsViewModelViewConfig : WebViewConfig<KeyItemWmsViewModel>
    {


        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.WithoutPaging();
            View.DisableEditing();
            View.ClearCommands();
            View.UseChildrenAsHorizontal();
            View.Property(p => p.AsnNo).ShowInList(width: 200).DisableSort();
            View.Property(p => p.SupplierName).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ProductionLot).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ItemLot).ShowInList(width: 200).DisableSort();
            View.Property(p => p.ProductionDate).ShowInList(width: 200).DisableSort();
            View.Property(p => p.CollectDate).ShowInList(width: 200).DisableSort();
            View.Property(p => p.InspectionNo).ShowInList().DisableSort();
            View.Property(p => p.InspectionBy).ShowInList().DisableSort();
            View.Property(p => p.InspectionTime).ShowInList().DisableSort();
            View.Property(p => p.InspectionResult).ShowInList().DisableSort();
            View.Property(p => p.DefectRecord).ShowInList().DisableSort();
            View.Property(p => p.FailedAuditWorkflowCode).ShowInList().DisableSort();
            View.Property(p => p.FailedAuditResult).ShowInList().DisableSort();
            View.Property(p => p.QualityWorkflowCode).ShowInList().DisableSort();
        }
    }
}