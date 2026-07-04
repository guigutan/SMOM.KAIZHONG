using SIE.Domain;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks;
using SIE.Web.Common;
using SIE.Web.EMS.InventoryPlans.Commands;
using SIE.Web.EMS.InventoryTasks.Commands;

namespace SIE.Web.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务界面
    /// </summary>
    internal class InventoryTaskViewConfig : WebViewConfig<InventoryTask>
    {
        /// <summary>
        /// 盘点计划视图
        /// </summary>
        public const string PlanView = "PlanView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(PlanView);
            if (ViewGroup == PlanView)
            {
                ConfigPlanView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.InventoryTasks.InventoryTaskListBehavior");
            // typeof(SeePhotoCommand).FullName, "SIE.Web.EMS.InventoryTasks.Commands.UploadPhotoCommand"
            View.UseCommands(typeof(SaveInventoryTaskCommand).FullName, typeof(ReleaseTaskCommand).FullName, typeof(FirstCompleteCommand).FullName,
                typeof(SecondCompleteCommand).FullName, typeof(InventoryTasks.Commands.ShutdownCommand).FullName);
            View.Property(p => p.TaskNo).Readonly().ShowInList(130);
            View.Property(p => p.FactoryId).Readonly().ShowInList(120);
            View.Property(p => p.InventoryPlanId).HasLabel("计划单号").Readonly().ShowInList(130);
            View.Property(p => p.InventoryAssetObject).Readonly().ShowInList(80);
            View.Property(p => p.PercentageString).Readonly().ShowInList(80);
            View.Property(p => p.InventoryTaskStatus).Readonly().ShowInList(80);
            View.Property(p => p.InventoryType).UseCatalogEditor(e => { e.CatalogType = InventoryPlan.InventoryTypeCatalog;e.ReloadDataOnPopping = true; }).Readonly().ShowInList(80);
            View.Property(p => p.Remark).Readonly().ShowInList(400);
            View.Property(p => p.PlanEndDate).UseDateEditor().Readonly().ShowInList(150);
            View.Property(p => p.ResponsibleId).ShowInList(120);
            View.Property(p => p.InventoryExecuteType).Readonly().ShowInList(80);
            View.Property(p => p.NeedPhoto).Readonly().ShowInList(80);
            View.Property(p => p.PhotoFilePath).Readonly().Show(ShowInWhere.Hide);
            View.Property(p => p.CloseRemark).Readonly().ShowInList(300);

            ConfigInventoryScope();

            ConfigInventoryList();
        }

        /// <summary>
        /// 配置盘点范围
        /// </summary>
        private void ConfigInventoryScope()
        {
            View.AttachDetailChildrenProperty(typeof(InventoryPlanEquipment), c =>
            {
                var task = c.Parent as InventoryTask;
                if (task == null)
                {
                    return new InventoryPlanEquipment();
                }
                task = RF.GetById<InventoryTask>(task.Id);
                if (task == null)
                {
                    return new InventoryPlanEquipment();
                }
                var range = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanEquipment(task.InventoryPlanId);
                if (range == null)
                {
                    return new InventoryPlanEquipment();
                }
                range.ManageDeptName = task.ManageDept?.Name;
                return range;
            }, DetailsView).HasLabel("盘点范围").OrderNo = 1;

            View.AttachDetailChildrenProperty(typeof(InventoryPlanFixture), c =>
            {
                var task = c.Parent as InventoryTask;
                if (task == null)
                {
                    return new InventoryPlanFixture();
                }
                task = RF.GetById<InventoryTask>(task.Id);
                if (task == null)
                {
                    return new InventoryPlanFixture();
                }
                var range = RT.Service.Resolve<InventoryPlanController>().GetInventoryPlanFixture(task.InventoryPlanId);
                if (range == null)
                {
                    return new InventoryPlanFixture();
                }
                return range;               
            }, DetailsView).HasLabel("盘点范围").OrderNo = 2;

            //备件盘点范围
            View.AttachDetailChildrenProperty(typeof(InventoryTaskSparePartScope), c =>
            {
                var task = c.Parent as InventoryTask;
                if (task == null)
                {
                    return new InventoryTaskSparePartScope();
                }
                var range = RT.Service.Resolve<InventoryTaskSpartPartController>().GetInventoryTaskSparePartScope(task.Id);
                if (range == null)
                {
                    return new InventoryTaskSparePartScope();
                }
                return range;
            }, DetailsView).HasLabel("盘点范围").OrderNo = 3;
        }

        /// <summary>
        /// 配置盘点清单
        /// </summary>
        private void ConfigInventoryList()
        {            
            //设备清单
            View.ChildrenProperty(p => p.InventoryTaskEquipmentList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryTask.InventoryTaskEquipmentListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<InventoryTask>();
                if (parent == null)
                {
                    return new EntityList<InventoryTaskEquipment>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryTaskController>().GetTaskEquipments(parent, arg.PagingInfo, arg.SortInfo);
                }
            }).HasLabel("设备清单").OrderNo = 5;

            //编码明细
            View.ChildrenProperty(p => p.InventoryTaskFixtureEncodeList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryTask.InventoryTaskFixtureEncodeListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<InventoryTask>();
                if (parent == null)
                {
                    return new EntityList<InventoryTaskFixtureEncode>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryTaskController>().GetInventoryTaskFixtureEncodeList(parent, arg.PagingInfo, arg.SortInfo);
                }
            }).HasLabel("编码明细").OrderNo = 6;

            //ID管控工治具
            View.ChildrenProperty(p => p.InventoryTaskFixtureIdAccountList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryTask.InventoryTaskFixtureIdAccountListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<InventoryTask>();
                if (parent == null)
                {
                    return new EntityList<InventoryTaskFixtureIdAccount>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryTaskController>().GetInventoryTaskFixtureIdAccountList(parent, arg.PagingInfo, arg.SortInfo);
                }
            }).HasLabel("序列号明细").OrderNo = 7;

            //备件汇总
            View.ChildrenProperty(p => p.InventoryTaskSparePartList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryTask.InventoryTaskSparePartListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;

                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<InventoryTask>();

                if (parent == null)
                {
                    return new EntityList<InventoryTaskSparePart>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryTaskSpartPartController>().GetInventoryTaskSpareParts(parent, arg.PagingInfo, arg.SortInfo);
                }

            }).HasLabel("备件汇总").OrderNo = 8;

            //备件清单
            View.ChildrenProperty(p => p.InventoryTaskSparePartDetailList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryTask.InventoryTaskSparePartDetailListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<InventoryTask>();
                if (parent == null)
                {
                    return new EntityList<InventoryTaskSparePartDetail>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryTaskSpartPartController>().GetInventoryTaskSparePartDetails(parent, arg.PagingInfo, arg.SortInfo);
                }
            }).HasLabel("备件清单").OrderNo = 9;

            View.ChildrenProperty(p => p.InventoryTaskCounterList).HasLabel("盘点人").OrderNo = 10;
            View.ChildrenProperty(p => p.InventoryTaskFixtureCounterList).HasLabel("盘点人").OrderNo = 15;
            View.ChildrenProperty(p => p.InventoryTaskSparePartCounterList).HasLabel("盘点人").OrderNo = 16;

            View.ChildrenProperty(p => p.InventoryCauseList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.InventoryTaskSparePartDiffList).Show(ChildShowInWhere.Hide);
        }

        /// <summary>
        /// 盘点任务视图
        /// </summary>
        protected void ConfigPlanView()
        {
            View.AssignAuthorize(typeof(InventoryPlan));
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.TaskNo).ShowInList(130);
                View.Property(p => p.FactoryId).ShowInList(120);
                View.Property(p => p.InventoryPlanId).HasLabel("计划单号").ShowInList(130);
                View.Property(p => p.InventoryAssetObject).ShowInList(80);
                View.Property(p => p.PercentageString).ShowInList(80);
                View.Property(p => p.InventoryTaskStatus).ShowInList(80);
                View.Property(p => p.InventoryType).UseCatalogEditor(e => { e.CatalogType = InventoryPlan.InventoryTypeCatalog; e.ReloadDataOnPopping = true; }).ShowInList(80);
                View.Property(p => p.Remark).ShowInList(400);
                View.Property(p => p.PlanEndDate).UseDateEditor().ShowInList(150);
                View.Property(p => p.ResponsibleId).ShowInList(120);
                View.Property(p => p.InventoryExecuteType).ShowInList(80);
                View.Property(p => p.NeedPhoto).ShowInList(80);
                View.Property(p => p.PhotoFilePath).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryTaskCounterList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryTaskEquipmentList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryTaskFixtureCounterList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryTaskFixtureEncodeList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryTaskFixtureIdAccountList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryCauseList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryTaskSparePartList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryTaskSparePartDetailList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryTaskSparePartCounterList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.InventoryTaskSparePartDiffList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}
