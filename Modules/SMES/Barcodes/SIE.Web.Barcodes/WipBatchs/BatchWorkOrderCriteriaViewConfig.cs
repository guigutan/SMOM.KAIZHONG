using SIE.Barcodes.WipBatchs;
using SIE.Resources.Employees;

namespace SIE.Web.Barcodes.WipBatchs
{
    /// <summary>
    /// 实体页面配置
    /// </summary>
    internal class BatchWorkOrderCriteriaViewConfig : WebViewConfig<BatchWorkOrderCriteria>
    {
        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Item).HasLabel("物料编码").Show(ShowInWhere.Hide);
            View.Property(p => p.No).ShowInDetail();
            View.Property(p => p.CreateBy).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<EmployeeController>().GetEmployeeList(pagingInfo, keyword);
            }).ShowInDetail();
            View.Property(p => p.ProductCode).ShowInDetail();
            View.Property(p => p.ItemName).ShowInDetail();
            View.Property(p => p.BatchNo).ShowInDetail();

            View.Property(p => p.PlanBeginDate).ShowInDetail().UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.All;
                e.Format = "Y/m/d";
            });
            View.Property(p => p.CreateDate).ShowInDetail().UseDateRangeEditor(e =>
            {
                e.DateRangeType = ObjectModel.DateRangeType.Week;
                e.Format = "Y/m/d";
            });
        }
    }
}