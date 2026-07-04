using SIE.CSM.Suppliers.Configs;
using SIE.Domain;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.CSM.Suppliers.Configs
{
    /// <summary>
    /// 供应商委外参数配置视图
    /// </summary>
    public class SupplierOutsourcingConfigValueViewConfig : WebViewConfig<SupplierOutsourcingConfigValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.OutsourcingInLoc).UseDataSource((o, e, r) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetAllEnableStorageLocations(r, e, null, true);
                }).UsePagingLookUpEditor().Show();
                View.Property(p => p.OutsourcingOutLoc).UseDataSource((o, e, r) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetAllEnableStorageLocations(r, e, null, true);
                }).UsePagingLookUpEditor().Show();
                View.Property(p => p.OutsourcingReceive).Show();
                View.Property(p => p.OutsourcingUseTime).Show();
                View.Property(p => p.IsHasStorer).Show();
            }
        }
    }
}
