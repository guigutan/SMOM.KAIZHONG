using SIE.Domain;
using SIE.EMS.Maintains.Confirmations;
using SIE.EMS.Maintains.Controller;
using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Projects;
using SIE.EMS.Maintains.Records;
using SIE.MetaModel.View;
using SIE.Web.EMS.EquipMaint.Maintains.Confirmations;
using SIE.Web.EMS.EquipMaint.Maintains.Plans;
using SIE.Web.EMS.EquipMaint.Maintains.Projects;
using SIE.Web.EMS.EquipMaint.Maintains.Records.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipMaint.Maintains.Records
{
    /// <summary>
    /// 设备保养记录视图配置
    /// </summary>
    internal class MaintainRecordViewConfig : WebViewConfig<MaintainRecord>
    {
        private readonly int singleCharWidth = 20;

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.EquipMaint.Maintains.Records.Scripts.MaintainRecordBehavior");
            View.ClearCommands();
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            View.UseCommands(typeof(MaintainPlanDeleteCommand).FullName, "SIE.Web.EMS.EquipMaint.Maintains.Records.Commands.MaintainExecuteCommand");

            using (View.OrderProperties())
            {
                View.Property(p => p.MaintainNo).ShowInList(width: 150).Readonly();
                View.Property(p => p.EquipAccount).ShowInList(width: 150).HasLabel("设备编码").Readonly();
                View.Property(p => p.MachineNo).HasLabel("设备名称").Readonly();
                View.Property(p => p.BeginDay).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.EndDay).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.WorkShopName).ShowInList(80).Readonly();
                View.Property(p => p.ResourceName).ShowInList(80).Readonly();
                View.Property(p => p.ExeState).HasLabel("保养状态").Readonly();
                View.Property(p => p.ExeResult).Readonly().ShowInList(singleCharWidth * 4);                
                View.Property(p => p.ConfirmResult).Readonly().ShowInList(singleCharWidth * 4);        
                View.Property(p => p.PlanBeginDate).Readonly();
                View.Property(p => p.PlanEndDate).Readonly();
                View.Property(p => p.PrecisePlanBeginDate).Readonly();
                View.Property(p => p.PrecisePlanEndDate).Readonly();
                View.Property(p => p.ActBeginDate).Readonly();
                View.Property(p => p.ActEndDate).Readonly();
                View.Property(p => p.IsExsitNgProject).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.ExecuteBy).Readonly();
                View.Property(p => p.Department).Readonly();
                View.Property(p => p.MaintainType).Readonly().ShowInList(singleCharWidth * 4);
                View.Property(p => p.MaintainSummary).Readonly();
                

                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);

                View.ChildrenProperty(p => p.ProjectList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainPlanSparePartAplList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainPlanSparePartList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainPlanAttachmentList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.WorkHoursRegisterList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.MaintainConfirmationList).Show(ChildShowInWhere.Hide);
            }

            View.AttachChildrenProperty(typeof(MaintainProject), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainRecord>();
                if (parent == null) { return new EntityList<MaintainProject>(); }

                var list = RT.Service.Resolve<MaintainController>().GetMaintainProjectList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainProjectViewConfig.MaintainConfirmationListView).HasLabel("保养项目").Show(ChildShowInWhere.All).HasOrderNo(1);

            View.AttachChildrenProperty(typeof(MaintainPlanSparePart), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainRecord>();
                if (parent == null) { return new EntityList<MaintainPlanSparePart>(); }

                var list = RT.Service.Resolve<MaintainController>().GetMaintainPlanSparePartList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainPlanSparePartViewConfig.MaintainConfirmationListView).HasLabel("备件更换").Show(ChildShowInWhere.All).HasOrderNo(2);

            View.AttachChildrenProperty(typeof(MaintainPlanSparePartApl), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainRecord>();
                if (parent == null) { return new EntityList<MaintainPlanSparePartApl>(); }

                var list = RT.Service.Resolve<MaintainController>().GetMaintainPlanSparePartAplList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainPlanSparePartAplViewConfig.MaintainConfirmationListView).HasLabel("备件申请").Show(ChildShowInWhere.All).HasOrderNo(3);

            View.AttachChildrenProperty(typeof(MaintainPlanAttachment), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainRecord>();
                if (parent == null) { return new EntityList<MaintainPlanAttachment>(); }

                var list = RT.Service.Resolve<MaintainController>().GetMaintainPlanAttachmentList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainPlanAttachmentViewConfig.MaintainConfirmationListView).HasLabel("执行图片").Show(ChildShowInWhere.All).HasOrderNo(4);

            View.AttachChildrenProperty(typeof(WorkHoursRegister), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainRecord>();
                if (parent == null) { return new EntityList<WorkHoursRegister>(); }

                var list = RT.Service.Resolve<MaintainController>().GetWorkHoursRegisterList(parent.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, WorkHoursRegisterViewConfig.MaintainConfirmationListView).HasLabel("工时登记").Show(ChildShowInWhere.All).HasOrderNo(5);

            View.AttachChildrenProperty(typeof(MaintainConfirmation), w =>
            {
                var args = w as ChildPagingDataWithParentEntityArgs;
                var parent = args.ParentEntity.ToJsonObject<MaintainRecord>();
                if (parent == null) { return new EntityList<MaintainConfirmation>(); }

                var list = RT.Service.Resolve<MaintainController>().GetMaintainConfirmationList(parent.Id, null, (List<OrderInfo>)args.SortInfo, args.PagingInfo);

                return list;
            }, MaintainConfirmationViewConfig.MaintainRecordListView).HasLabel("评分项").Show(ChildShowInWhere.All).HasOrderNo(6);

        }
    }
}
