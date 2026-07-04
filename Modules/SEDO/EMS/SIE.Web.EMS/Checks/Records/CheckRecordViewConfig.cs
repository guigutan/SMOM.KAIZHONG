using SIE.Domain;
using SIE.EMS.Checks;
using SIE.EMS.Checks.Confirmations;
using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Projects;
using SIE.EMS.Checks.Records;
using SIE.MetaModel.View;
using SIE.Web.EMS.Checks.Confirmations;
using SIE.Web.EMS.Checks.Plans;
using SIE.Web.EMS.Checks.Projects;
using SIE.Web.EMS.Checks.Records.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.Checks.Records
{
    /// <summary>
    /// 点检计划维护视图配置
    /// </summary>
    internal class CheckRecordViewConfig : WebViewConfig<CheckRecord>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Checks.Records.Scripts.CheckRecordBehavior");
            View.ClearCommands();
            View.UseCommands(typeof(CheckRecordDeleteCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll,
                "SIE.Web.EMS.Checks.Records.Commands.CheckExecuteCommand");

            using (View.OrderProperties())
            {
                View.Property(p => p.CheckPlanNo).ShowInList(width: 150).Readonly();
                View.Property(p => p.EquipAccount).HasLabel("设备编码").ShowInList(width: 150).Readonly();
                View.Property(p => p.MachineNo).Readonly();
                View.Property(p => p.WorkShopName).ShowInList(80).Readonly();
                View.Property(p => p.ResourceName).ShowInList(80).Readonly();
                View.Property(p => p.CheckCycleType).HasLabel("周期类型").ShowInList(80).Readonly();
                View.Property(p => p.ExeResult).ShowInList(80).Readonly();
                View.Property(p => p.ExeState).HasLabel("点检状态").ShowInList(80).Readonly();
                View.Property(p => p.CheckDate).ShowInList(140).Readonly();
                View.Property(p => p.ConfirmDate).ShowInList(140).Readonly();
                View.Property(p => p.CheckBeginDate).ShowInList(140).Readonly();
                View.Property(p => p.CheckEndDate).ShowInList(140).Readonly();
                View.Property(p => p.IsExsitNgProject).ShowInList(110).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.CheckEmployee).HasLabel("点检执行人").Readonly();
                View.Property(p => p.Department).ShowInList(80).Readonly();
                View.Property(p => p.ConfirmResult).ShowInList(80).Readonly();
                View.Property(p => p.ConfirmNote).ShowInList(250).Readonly();
                View.Property(p => p.CheckSummary).ShowInList(250).Readonly();
                View.Property(p => p.ActCheckBeginDate).Show(ShowInWhere.Hide);
                View.Property(p => p.ActCheckEndDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.CheckProjectList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.CheckPlanAttachmentList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.CheckPlanSparePartList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.CheckPlanSparePartAplList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.CheckConfirmationList).Show(ChildShowInWhere.Hide);
            }

            View.AttachChildrenProperty(typeof(CheckProject), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckRecord>();
                if (parent == null) { return new EntityList<CheckProject>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckProjectList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckProjectViewConfig.CheckConfirmationListView).HasLabel("点检项目").Show(ChildShowInWhere.All).HasOrderNo(1);

            View.AttachChildrenProperty(typeof(CheckPlanSparePart), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckRecord>();
                if (parent == null) { return new EntityList<CheckPlanSparePart>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckPlanSpareParts(parent.Id, parent.CheckPlanNo, parent.EquipAccountId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckPlanSparePartViewConfig.CheckConfirmationListView).HasLabel("备件更换").Show(ChildShowInWhere.All).HasOrderNo(2);

            View.AttachChildrenProperty(typeof(CheckPlanSparePartApl), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckRecord>();
                if (parent == null) { return new EntityList<CheckPlanSparePartApl>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckPlanSparePartApls(parent.Id, parent.EquipAccountId, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckPlanSparePartAplViewConfig.CheckConfirmationListView).HasLabel("备件申请").Show(ChildShowInWhere.All).HasOrderNo(3);

            View.AttachChildrenProperty(typeof(CheckPlanAttachment), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckRecord>();
                if (parent == null) { return new EntityList<CheckPlanAttachment>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckPlanAttachmentList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckPlanAttachmentViewConfig.CheckConfirmationListView).HasLabel("执行图片").Show(ChildShowInWhere.All).HasOrderNo(4);

            View.AttachChildrenProperty(typeof(CheckConfirmation), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<CheckRecord>();
                if (parent == null) { return new EntityList<CheckConfirmation>(); }

                var list = RT.Service.Resolve<CheckPlanController>().GetCheckConfirmationList(parent.Id, null, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, CheckConfirmationViewConfig.CheckRecordListView).HasLabel("评分项").Show(ChildShowInWhere.All).HasOrderNo(5);
        }
    }
}
