using DevExpress.XtraGauges.Core.Base;
using SIE.LES.MaterialPreparations;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.LES.MaterialPreparations
{
    /// <summary>
    /// 
    /// </summary>
    public class MaterialPreparationCriteriaViewConfig : WebViewConfig<MaterialPreparationCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.Wo).Show();
                View.Property(p => p.WorkShop).UseDataSource((e, p, k) =>
                {
                    var list = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(p, k);
                    foreach( var i in list)
                    {
                        i.TreePId = null;
                    }
                    return list;
                }).Show().Cascade(p => p.WipResourceId, null);
                View.Property(p => p.WipResource).UseDataSource((e, p, k) =>
                {
                    var criteria = e as MaterialPreparationCriteria;
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(p, k, criteria.WorkShopId);
                }).Show();
                View.Property(p => p.PrepareType).Show();
                View.Property(p => p.PrepareReason).UseCatalogEditor(p => { p.CatalogType = MaterialPreparation.MaterialPreReasonStr; p.CatalogReloadData = true; }).Show();
                View.Property(p => p.SaleOrderNo).Show();
                View.Property(p => p.CustomerOrderNo).Show();
                View.Property(p => p.ShippingOrderNo).Show();
                View.Property(p => p.CreateDate).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.Month).Show();
            }
        }
    }
}
