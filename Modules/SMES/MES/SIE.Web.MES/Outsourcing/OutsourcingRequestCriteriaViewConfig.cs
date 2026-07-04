using SIE.CSM.Suppliers;
using SIE.Defects;
using SIE.Domain;
using SIE.MES.Outsourcing;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.Resources;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.Outsourcing
{
    public class OutsourcingRequestCriteriaViewConfig : WebViewConfig<OutsourcingRequestCriteria>
    {
        protected override void ConfigView()
        {
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.Property(p => p.NO).Show(ShowInWhere.All);
                View.Property(p => p.OutsourcingState).Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderId).Show(ShowInWhere.All);
                View.Property(p => p.BeginProcessId).Show(ShowInWhere.All);
                View.Property(p => p.EndProcessId).Show(ShowInWhere.All);
                View.Property(p => p.FactoryId).Show(ShowInWhere.All).UseFactoryEditor();
                View.Property(p => p.OutFactory).Show(ShowInWhere.All);
                View.Property(p => p.WorkShopCode).Show(ShowInWhere.All);
                //View.Property(p => p.WorkShopId).UseDataSource((r, pagingInfo, keyword) =>
                //{
                //    var criteria = r as OutsourcingRequestCriteria;
                //    if (criteria != null && criteria.FactoryId.HasValue)
                //    {
                //        var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword, criteria.FactoryId.Value);

                //        workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                //        return workShopList;
                //    }
                //    else
                //    {
                //        var workShopList = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword);
                //        workShopList.ForEach(enterprise => { enterprise.TreePId = null; });
                //        return workShopList;
                //    }
                //}).Show(ShowInWhere.All);
                View.Property(p => p.WipResourceId).UseDataSource((r, pagingInfo, keyword) =>
                {
                    var criteria = r as OutsourcingRequestCriteria;
                    if (criteria == null)
                    {
                        return new EntityList<WipResource>();
                    }
                    if (criteria.WorkShopId.HasValue)
                    {
                        return RT.Service.Resolve<WipResourceController>().GetWipResourcesByWorkShopId(new List<ResourceState>() {
                         ResourceState.Actived,//ResourceState.Stop, ResourceState.Unused
                        }, new List<double?>() { criteria.WorkShopId }, new List<SyncSourceType>() {
                         SyncSourceType.Enterprise
                        }, pagingInfo, keyword);
                    }
                    else
                    {
                        return RT.Service.Resolve<WipResourceController>().GetWipResourcesByWorkShopId(new List<ResourceState>() {
                         ResourceState.Actived, //ResourceState.Stop, ResourceState.Unused
                        }, null, new List<SyncSourceType>() {
                         SyncSourceType.Enterprise
                        }, pagingInfo, keyword);
                    }

                }).UseFormSetting(p => p.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源").Show(ShowInWhere.All);
                View.Property(p => p.SupplierNames).UsePagingLookUpGridPopupEditor(p =>
                {
                    p.Model = typeof(Supplier).FullName;
                    p.LinkField = OutsourcingRequestCriteria.SupplierIdsProperty.Name;
                    p.DisplayField = Supplier.NameProperty.Name;
                    p.XType = "OutsourcingRequestCriteriaComboPopup";
                    p.Editable = false;
                    p.Separator = ",";
                });
                View.Property(p => p.SupplierNames).Show(ShowInWhere.All);
                View.Property(p => p.ProduceName).Show(ShowInWhere.All);
                View.Property(p => p.OutboundState).Show(ShowInWhere.All);
                View.Property(p => p.ReportState).Show(ShowInWhere.All);
                View.Property(p => p.Sn).Show(ShowInWhere.All);
                View.Property(p => p.PlanBeginDate).UseDateRangeEditor(p=>p.DateRangeType= ObjectModel.DateRangeType.Week).Show(ShowInWhere.All);
            }
        }
    }
}
