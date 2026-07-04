using SIE.Domain;
using SIE.MES.PanelBindings;
using SIE.MES.WoBarcodes;
using SIE.MES.WorkOrders;

namespace SIE.Web.MES.PanelBindings
{
    /// <summary>
    /// 拼板码领用-界面
    /// </summary>
    internal class WoPanelRangeCriteriaViewConfig : WebViewConfig<WoPanelRangeCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.PanelCode).ShowInDetail();
            View.Property(p => p.WipResourceId).Cascade(o => o.WorkOrderId, null).ShowInDetail();
            View.Property(p => p.WorkOrderId).UseDataSource((e, c, r) =>
            {
                var eq = e as WoBarcodeRangeCriteria;
                if (eq == null || eq.ResourceId == null)
                    return new EntityList<WorkOrder>();
                return RT.Service.Resolve<WorkOrderController>().GetWorkOrders(c, r, eq.ResourceId.Value);
            }).ShowInDetail();
            View.Property(p => p.ReceiveBy).ShowInDetail();
            View.Property(p => p.ReceiveDate).ShowInDetail();
        }
    }
}
