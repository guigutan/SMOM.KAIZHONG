using SIE.Core.Equipments;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;

namespace SIE.Wpf.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工任务视图配置
    /// </summary>
    //[CompiledPropertyDeclarer]
    public class DispatchTaskViewConfig : WPFViewConfig<DispatchTask>
    {

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            //parentItems = null;
            //View.DeclareExtendViewGroup(workOrderTaskView);
            //if (ViewGroup == workOrderTaskView)
            //    WorkOrderTaskView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {

            //View.UseGridSelectionModel();
            View.Property(p => p.No).ShowInList(150).Readonly();
            View.Property(p => p.DispatchQty).Readonly();
            View.Property(p => p.ReportQty).Readonly();
            View.Property(p => p.OkQty).Readonly();
            View.Property(p => p.NgQty).Readonly();
            View.Property(p => p.ReworkQty).Readonly();
            View.Property(p => p.SuspectQty).Readonly();
            //View.Property(p => p.ExcessReportRatio).ShowInList().Readonly().HasLabel("超额比例");
            //View.Property(p => p.ExcessReportQty).ShowInList().Readonly().HasLabel("超额数量");

            View.Property(p => p.TaskStatus).Readonly();
            //View.Property(DisplayPriorityProperty).HasLabel("优先级");
            View.Property(p => p.TaskPerformer).Readonly();
            View.Property(p => p.PlanBeginTime);
            View.Property(p => p.PlanEndTime);
            View.Property(p => p.AssociatedWorkOrder).Readonly();
            //View.Property(DisplayStateProperty).Readonly().HasLabel("工单状态");
            View.Property(p => p.ProductCode).Readonly();
            View.Property(p => p.ProductName).Readonly();
            View.Property(p => p.ShortDescription).Readonly();
            //View.Property(PieWightProperty).Readonly().HasLabel("饼重");
            //View.Property(ParShortDescriptionProperty).Readonly().HasLabel("父旧物料号");
            View.Property(p => p.ParShortDescription).Readonly().Show();
            View.Property(p => p.Fevor).Readonly();
            View.Property(p => p.Seq).Readonly();
            View.Property(p => p.ProcessCode).Readonly();
            View.Property(p => p.ProcessName).Readonly();
            View.Property(p => p.IsNeedEquipment).Readonly();
            View.Property(p => p.EquipAccountId).UseDataSource((e, p, s) =>
            {
                var entity = e as DispatchTask;
                if (entity != null)
                    return RT.Service.Resolve<DispatchController>().GetEquipAccounts(entity.ProductId, p, s);
                return new EntityList<EquipAccount>();
            });
            View.Property(p => p.SpecificationCode).Readonly();
            View.Property(p => p.SpecificationName).Readonly();
            View.Property(p => p.IsVirtualPart).Readonly();
            View.Property(p => p.VirtualPartCode).Readonly();
            View.Property(p => p.VirtualPartName).Readonly();
            //View.Property(p => p.ProcessStandardHour).ShowInList(width: 150).Readonly();
            //View.Property(p => p.ProcessHourSum).ShowInList(width: 150).Readonly();
            //View.Property(p => p.ExpectedProductionTime).ShowInList(width: 150).Readonly();
            View.Property(p => p.ReportMode).Readonly();
            View.Property(p => p.WorkShopName).Readonly();
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.ResourceCode).Readonly();
            View.Property(p => p.ResourceSourceType).Readonly();
            View.Property(p => p.IsSyntype).Readonly();
            View.Property(p => p.TechNo).Readonly();
            View.Property(p => p.Classes).Readonly();
            View.Property(p => p.SourceType).Readonly();
            View.Property(p => p.ImportTime).Readonly();
            View.Property(p => p.IsFeedingClose).Show().Readonly();
            View.Property(p => p.IsSchedulingInfReturn).Show().Readonly();
            View.Property(p => p.SchedulingInfReturnReason).Show().Readonly();
            View.Property(p => p.IsOutsourcing).Show().Readonly();
            View.ChildrenProperty(p => p.Details).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.TaskList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.Boms).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.Records).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.OptLogList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.TransferLabels).Show(ChildShowInWhere.Hide);


        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            // 配置明细视图
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            // 配置下拉视图
            using (View.OrderProperties())
            {

                View.Property(p => p.No).ShowInList(150).Readonly();
                View.Property(p => p.DispatchQty).Readonly();
                View.Property(p => p.ReportQty).Readonly();
                //View.Property(p => p.OkQty).Readonly();
                //View.Property(p => p.NgQty).Readonly();
                //View.Property(p => p.ReworkQty).Readonly();
                View.Property(p => p.SuspectQty).Readonly();

                View.Property(p => p.TaskStatus).Readonly();
                ////View.Property(DisplayPriorityProperty).HasLabel("优先级");
                //View.Property(p => p.TaskPerformer).Readonly();
                //View.Property(p => p.PlanBeginTime);
                //View.Property(p => p.PlanEndTime);
                //View.Property(p => p.AssociatedWorkOrder).Readonly();
                ////View.Property(DisplayStateProperty).Readonly().HasLabel("工单状态");
                View.Property(p => p.ProductCode).Readonly();
                View.Property(p => p.ProductName).Readonly();
                //View.Property(p => p.ShortDescription).Readonly();
                ////View.Property(PieWightProperty).Readonly().HasLabel("饼重");
                ////View.Property(ParShortDescriptionProperty).Readonly().HasLabel("父旧物料号");
                //View.Property(p => p.ParShortDescription).Readonly().Show();
                //View.Property(p => p.Fevor).Readonly();
                //View.Property(p => p.Seq).Readonly();
                View.Property(p => p.ProcessCode).Readonly();
                View.Property(p => p.ProcessName).Readonly();
                //View.Property(p => p.WorkShopName).Readonly();
                View.Property(p => p.ResourceCode).Readonly();
                View.Property(p => p.ResourceName).Readonly();
                //View.Property(p => p.ResourceSourceType).Readonly();
                //View.Property(p => p.IsSyntype).Readonly();
                //View.Property(p => p.TechNo).Readonly();
                //View.Property(p => p.Classes).Readonly();
                //View.Property(p => p.SourceType).Readonly();
                //View.Property(p => p.ImportTime).Readonly();
                //View.Property(p => p.IsFeedingClose).Show().Readonly();
                //View.Property(p => p.IsSchedulingInfReturn).Show().Readonly();
                //View.Property(p => p.SchedulingInfReturnReason).Show().Readonly();
                View.Property(p => p.IsOutsourcing).Readonly();
            }
        }


    }
}