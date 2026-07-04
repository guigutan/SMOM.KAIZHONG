using SIE.Domain;
using SIE.ProductIntfc.OutputProducts;
using SIE.Resources.WipResources;
using SIE.Web.ProductIntfc._Extentions_;
using SIE.Web.Resources;
using System.Collections.Generic;

namespace SIE.Web.ProductIntfc.OutputProducts
{
    /// <summary>
    /// 报检记录查询实体视图配置
    /// </summary>
    internal class OutputProductCriteriaViewConfig : WebViewConfig<OutputProductCriteria>
    {
        /// <summary>
        /// 成品报检查询视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.WorkOrder).ShowInDetail();
                View.Property(p => p.Factory).UseFactoryEditor().ShowInDetail();
                View.Property(p => p.Shop).UseFactoryWorkshopEditor().Cascade(p => p.Resource, null).ShowInDetail()
                    .UseListSetting(e => { e.HelpInfo = "显示企业类型为车间的企业资源,更改车间清空资源"; });
                View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                {
                    var criteria = e as OutputProductCriteria;
                    if (criteria == null)
                        return new EntityList<WipResource>();

                    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                    var srcTypeList = new List<SyncSourceType>() { SyncSourceType.Enterprise };
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, criteria.ShopId, srcTypeList, c, r);
                }).UsePagingLookUpEditor((c, e) => c.ReloadDataOnPopping = true).ShowInDetail()
                .UseListSetting(e => { e.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源"; });
               
                View.Property(p => p.State).ShowInDetail();
                View.Property(p => p.InstorageBarcode).ShowInDetail().HasLabel("入库单号");
                View.Property(p => p.Barcode).ShowInDetail().HasLabel("入库条码");
                View.Property(p => p.Lot).ShowInDetail();
                View.Property(p => p.PlanBeginTime).UseDateRangeEditor(p=>p.DateRangeType=ObjectModel.DateRangeType.All).ShowInDetail();
            }
        }
    }
}
