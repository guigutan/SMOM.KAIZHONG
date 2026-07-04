using SIE.LES.MaterialPreparations;
using SIE.LES.MaterialReturnApplys;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialReturnApplys
{
    /// <summary>
    /// 退料申请查询实体视图配置
    /// </summary>
    public class MaterialReturnApplyCriteriaViewConfig : WebViewConfig<MaterialReturnApplyCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.ReStatus).Show();
                View.Property(p => p.WoNo).Show();
                View.Property(p => p.WorkShop).UseDataSource((e, p, k) =>
                {
                    var list = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(p, k);
                    foreach (var i in list)
                    {
                        i.TreePId = null;
                    }
                    return list;
                }).Cascade(p => p.WipResourceId, null).Show();
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    var criteria = e as MaterialReturnApplyCriteria;
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(p, k, criteria.WorkShopId);
                }).Show();
                View.Property(p => p.Warehouse).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<WarehouseController>().GetLineWareHouse(p, k);
                }).Show();
                View.Property(p => p.Reason).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show();
            }
        }
    }
}
