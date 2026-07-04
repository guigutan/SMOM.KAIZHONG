using SIE.LES.LinesideWarehouses;
using SIE.Warehouses;
using SIE.Web.LES.Extensions;
using SIE.Web.Resources;

namespace SIE.Web.LES.LinesideWarehouses
{
    /// <summary>
    /// 查询对象
    /// </summary>
    public class LinesideWarehouseCriteriaViewConfig : WebViewConfig<LinesideWarehouseCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.FactoryId).UseFactoryEditor().HasLabel("工厂").Cascade(p => p.WorkShopId, null).Show();
                View.Property(p => p.WorkShopId).UseFactoryWorkshopEditor().HasLabel("车间").Cascade(p => p.WipResouceId, null).Show();
                View.Property(p => p.WipResouceId).UseWorkShopWipResourceEditor().HasLabel("资源").Show();
                View.Property(p => p.WarehouseId).UseDataSource((source, pagingInfo, keyword) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetWarehouses(pagingInfo, keyword);
                }).HasLabel("仓库").Show();

                View.Property(p => p.CreateTiem).UseDateRangeEditor(p=>p.DateRangeType= ObjectModel.DateRangeType.All).HasLabel("创建时间").Show();
            }
        }
    }
}
