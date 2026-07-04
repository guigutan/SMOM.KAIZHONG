using SIE.Domain;
using SIE.MES.WoBarcodes;
using SIE.MES.WorkOrders;
using System.Collections.Generic;

namespace SIE.Web.MES.WoBarcodes
{
    /// <summary>
    /// 条码领用-界面
    /// </summary>
    internal class WoBarcodeRangeCriteriaViewConfig : WebViewConfig<WoBarcodeRangeCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Barcode).ShowInDetail();
            View.Property(p => p.ResourceId).Cascade(o => o.WorkOrderId, null).UseDataSource((entity, pagingInfo, keyword) =>
            {
                var Resources = RT.Service.Resolve<SIE.Resources.WipResources.WipResourceController>().GetSchedResources(pagingInfo, keyword);
                return Resources;
            }).ShowInDetail();
            View.Property(p => p.WorkOrderId).UseDataSource((entity, pagingInfo, keyword) =>
            {
                var workOrders = RT.Service.Resolve<WorkOrderController>().GetWorkOrderList(pagingInfo, keyword);
                return workOrders;
            }).ShowInDetail();
            View.Property(p => p.ReceiveBy).UseDataSource((entity, pagingInfo, keyword) =>
            {
                var emp = RT.Service.Resolve<SIE.Resources.Employees.EmployeeController>().GetEmployeeList(pagingInfo, keyword);
                return emp;
            }).ShowInDetail();
            View.Property(p => p.ReceiveDate).ShowInDetail();
        }
    }
}
