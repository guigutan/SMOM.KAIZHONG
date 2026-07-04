using SIE.Domain;
using SIE.EMS.InventoryBalances;
using SIE.EMS.InventoryTasks;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.Web.EMS.InventoryBalances.Commands;
using SIE.Web.EMS.InventoryTasks;
using SIE.Web.EMS.WorkFlows;
using System;

namespace SIE.Web.EMS.InventoryBalances
{
    /// <summary>
    /// 盘点平账界面
    /// </summary>
    internal class InventoryBalanceViewConfig : WebViewConfig<InventoryBalance>
    {
        /// <summary>
        /// 盘点平账界面
        /// </summary>
        public const string BalanceView = "BalanceView";

        /// <summary>
        /// 盘点平账原因分析界面
        /// </summary>
        public const string CauseAnalysisView = "CauseAnalysisView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BalanceView);
            if (ViewGroup == BalanceView)
            {
                ConfigBalanceView();
            }
            if (ViewGroup == CauseAnalysisView)
            {
                ConfigFixtureCauseAnalysisView();
            }

        }

        /// <summary>
        /// 盘点平账界面
        /// </summary> 
        protected void ConfigBalanceView()
        {
            View.AssignAuthorize(typeof(InventoryBalance));
            View.AddBehavior("SIE.Web.EMS.InventoryBalances.InventoryBalanceBehavior");
            View.DisableEditing();
            View.UseCommands(typeof(SaveBalanceCommand).FullName, typeof(SubmitBalanceCommand).FullName, typeof(CancelBalanceCommand).FullName, typeof(ExamineBalanceCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.TaskNo).ShowInList(130);
                View.Property(p => p.FactoryId).ShowInList(120);
                View.Property(p => p.InventoryPlanId).HasLabel("计划单号").ShowInList(130);
                View.Property(p => p.InventoryAssetObject).ShowInList(80);
                View.Property(p => p.ApprovalStatus).ShowInList(80);
                View.Property(p => p.InventoryType).ShowInList(80);
                View.Property(p => p.Remark).ShowInList(400);
                View.Property(p => p.PlanEndDate).UseDateEditor().ShowInList(130);
                View.Property(p => p.ResponsibleId).ShowInList(120);
                View.Property(p => p.InventoryExecuteType).ShowInList(80);
                View.Property(p => p.NeedPhoto).Readonly().ShowInList(80);
              //  View.Property(p => p.PhotoFilePath).ShowInList(80);

                //设备盘点人
                View.ChildrenProperty(p => p.InventoryTaskCounterList).Show(ChildShowInWhere.Hide);

                //工治具盘点人
                View.ChildrenProperty(p => p.InventoryTaskFixtureCounterList).Show(ChildShowInWhere.Hide);

                //备件盘点人
                View.ChildrenProperty(p => p.InventoryTaskSparePartCounterList).Show(ChildShowInWhere.Hide);
                
                //配置设备盘点的子列表
                ConfigEquipmentChilds();

                ConfigFixtureChilds();

                ConfigSparePartChilds();

                View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<InventoryBalance>();
                    if (parent == null)
                    {
                        return new EntityList<WorkFlowRecord>();
                    }
                    return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(InventoryBalance).FullName, args.SortInfo, args.PagingInfo);
                }, WorkFlowRecordViewConfig.EmsSeeView).HasLabel("审核记录").HasOrderNo(9);

                View.ChildrenProperty(p => p.AttachmentList).HasLabel("附件").HasOrderNo(100).Show(ChildShowInWhere.All).OrderNo = 10;
            }
        }

        /// <summary>
        /// 配置设备盘点的子列表
        /// </summary>
        private void ConfigEquipmentChilds()
        {
            //设备清单
            View.ChildrenProperty(p => p.InventoryTaskEquipmentList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryBalance.InventoryTaskEquipmentListProperty, e =>
            {
                var args = e as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<InventoryBalance>();
                if (parent == null)
                {
                    return new EntityList<InventoryTaskEquipment>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryBalanceController>().GetTaskEquipments(parent.Id, args.PagingInfo, args.SortInfo);
                }
            }, InventoryTaskEquipmentViewConfig.BalanceView).HasLabel("设备清单").HasOrderNo(1);

            //原因分析
            View.ChildrenProperty(p => p.InventoryCauseList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryBalance.InventoryCauseListProperty, e =>
            {
                var args = e as ChildPagingDataArgs;
                var parent = args.Parent.CastTo<InventoryBalance>();
                if (parent == null)
                {
                    return new EntityList<InventoryCause>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryBalanceController>().GetInventoryCauses(parent.Id, args.PagingInfo, args.SortInfo);
                }
            }).HasLabel("原因分析").HasOrderNo(2);

        }

        /// <summary>
        /// 配置工治具盘点的子列表
        /// </summary>
        private void ConfigFixtureChilds()
        {
            //工治具 编码明细
            View.ChildrenProperty(p => p.InventoryTaskFixtureEncodeList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryBalance.InventoryTaskFixtureEncodeListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<InventoryBalance>();
                if (parent == null)
                {
                    return new EntityList<InventoryTaskFixtureEncode>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryTaskController>().GetInventoryTaskFixtureEncodeList(parent, arg.PagingInfo, arg.SortInfo);
                }
            }, InventoryTaskFixtureEncodeViewConfig.BalanceView).HasLabel("编码明细").OrderNo = 3;

            //工治具 序列号明细
            View.ChildrenProperty(p => p.InventoryTaskFixtureIdAccountList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryBalance.InventoryTaskFixtureIdAccountListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<InventoryBalance>();
                if (parent == null)
                {
                    return new EntityList<InventoryTaskFixtureIdAccount>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryTaskController>().GetInventoryTaskFixtureIdAccountList(parent, arg.PagingInfo, arg.SortInfo);
                }
            }, InventoryTaskFixtureIdAccountViewConfig.BalanceView).HasLabel("序列号明细").OrderNo = 4;

            //工治具原因分析
            View.AttachDetailChildrenProperty(typeof(InventoryBalance), (c) =>
            {
                var account = c.Parent as InventoryBalance;
                account = RF.GetById<InventoryBalance>(account.Id, new EagerLoadOptions().LoadWithViewProperty());
                return account;
            }, CauseAnalysisView).HasLabel("原因分析").HasOrderNo(100).Show(ChildShowInWhere.All).OrderNo = 8;
        }

        /// <summary>
        /// 配置备件盘点的子列表
        /// </summary>
        private void ConfigSparePartChilds()
        {
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

            }, InventoryTaskSparePartViewConfig.BalanceView).HasLabel("备件汇总").OrderNo = 5;

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
                    return RT.Service.Resolve<InventoryTaskSpartPartController>().GetInventoryTaskSparePartDetailsForBalance(parent, arg.PagingInfo, arg.SortInfo);
                }
            }, InventoryTaskSparePartDetailViewConfig.BalanceView).HasLabel("备件清单").OrderNo = 6;

            //盘点差异
            View.ChildrenProperty(p => p.InventoryTaskSparePartDiffList).Show(ChildShowInWhere.Hide);
            View.AssociateChildrenProperty(InventoryTask.InventoryTaskSparePartDiffListProperty, e =>
            {
                var arg = e as ChildPagingDataWithParentEntityArgs;
                var parent = arg.ParentEntity == null ? null : arg.ParentEntity.ToJsonObject<InventoryTask>();
                if (parent == null)
                {
                    return new EntityList<InventoryTaskSparePartDiff>();
                }
                else
                {
                    return RT.Service.Resolve<InventoryTaskSpartPartController>().GetInventoryTaskSparePartDiffs(parent, arg.PagingInfo, arg.SortInfo);
                }
            }).HasLabel("盘点差异").OrderNo = 7;
        }

        /// <summary>
        /// 配置工治具原因分析界面
        /// </summary>
        protected void ConfigFixtureCauseAnalysisView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.UseDetail(4);
                View.Property(p => p.FixtureCauseAnalysis).HasLabel("原因分析*").ShowInDetail(columnSpan: 4).UseMemoEditor(p => p.Height = "400").Readonly(p => p.ApprovalStatus == ApprovalStatus.Audited);
                View.Property(p => p.FixtureImprovement).HasLabel("改善措施*").ShowInDetail(columnSpan: 4).UseMemoEditor(p => p.Height = "400").Readonly(p => p.ApprovalStatus == ApprovalStatus.Audited);
            }
        }
    }
}
