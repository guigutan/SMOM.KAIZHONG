using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.TaskManagement.ProcessPrepareRecords
{
    public class ProcessPrepareRecordCriteriaViewConfig : WebViewConfig<ProcessPrepareRecordCriteria>
    {
        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Show();
                View.Property(p => p.DispatchTaskNo).Show();
                View.Property(p=>p.ProcessName).Show();
                View.Property(p => p.Factory).UseFactoryEditor()
                    .UseListSetting(p => p.HelpInfo = "更改工厂时清空车间").Cascade(p => p.WorkShopId, null).Show();
                View.Property(p => p.WorkShop)
                    .UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var entity = source as ProcessPrepareRecordCriteria;
                        double? factoryId = entity?.FactoryId;
                        var workshop = RT.Service.Resolve<EnterpriseController>().GetResourceWorkShops(pagingInfo, keyword, factoryId);
                        if (workshop == null || workshop.Count <= 0)
                        {
                            return new EntityList<Enterprise>();
                        }
                        workshop.ForEach(p => p.TreePId = null);
                        return workshop;
                    })
                    .UseListSetting(p => p.HelpInfo = "更改车间时清空资源").Cascade(p => p.ResourceId, null).Show();
                //View.Property(p => p.Resource).UseDataSource((e, c, r) =>
                //{
                //    var workOrder = e as ProcessPrepareRecordCriteria;
                //    if (workOrder == null)
                //        return new EntityList<WipResource>();

                //    var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                //    var sourceType = new List<SyncSourceType>() { SyncSourceType.Enterprise, SyncSourceType.Equipment };
                //    if (workOrder.WorkShop == null)
                //    {
                //        return RT.Service.Resolve<WipResourceController>().GetWipResources(c, r);
                //    }
                //    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, workOrder.WorkShopId.Value, sourceType, c, r);
                //}).UseListSetting(p => p.HelpInfo = "显示启用状态不失效且企业类型为企业模型的生产资源").Show();
                View.Property(p => p.ProductName).Show();
                View.Property(p => p.State).UseEnumMutilEditor(p => p.EnumType = typeof(WorkOrderState)).Show();
                View.Property(p => p.TaskStatus).UseEnumMutilEditor(p => p.EnumType = typeof(SIE.MES.TaskManagement.Dispatchs.DispatchTaskStatus)).Show();
                View.Property(p => p.PreState).UseEnumEditor("Criteria").Show();
                View.Property(p => p.PlanBeginTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
                View.Property(p => p.ConfirmTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All).Show();
                View.Property(p => p.TaskPerformer).Show();

            }
        }
    }
}
