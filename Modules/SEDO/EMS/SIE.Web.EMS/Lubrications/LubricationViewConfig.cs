using SIE.Domain;
using SIE.EMS.Equipments;
using SIE.EMS.Lubrications;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.WorkFlows;
using SIE.MetaModel.View;
using SIE.Resources.Enterprises;
using SIE.Web;
using SIE.Web.EMS.Lubrications;
using SIE.Web.EMS.Lubrications.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.WPF.EMS.Lubrications
{
    /// <summary>
    /// 设备润滑视图配置
    /// </summary>
    public class LubricationViewConfig : WebViewConfig<Lubrication>
    {
        /// <summary>
        /// 添加记录
        /// </summary>
        public const string AddReportView = "AddReportView";

        /// <summary>
        /// 查看记录
        /// </summary>
        public const string SeeReportView = "SeeReportView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AddReportView, SeeReportView);
            if (ViewGroup == AddReportView)
            {
                ConfigAddReportView();
            }
            if (ViewGroup == SeeReportView)
            {
                ConfigSeeReportView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Common.Script.ApprovalBehavior");
            View.AddBehavior("SIE.Web.EMS.Lubrications.Behaviors.LubricationBehavior");
            View.FormEdit();
            View.UseCommands(WebCommandNames.Add, LubricationCommands.AddReportCommand,LubricationCommands.LubricationSubmitCommand, LubricationCommands.LubricationApprovalCommand, LubricationCommands.SeeReportCommand, LubricationCommands.LubricationDeleteCommand, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.LubricationNo);
                View.Property(p => p.EquipAccountId).HasLabel("设备编码");
                View.Property(p => p.EquipAccountName).HasLabel("设备名称");
                View.Property(p => p.EquipModelCode);
                View.Property(p => p.EquipModelName);
                View.Property(p => p.EquipTypeCode);
                View.Property(p => p.EquipTypeName);
                View.Property(p => p.UseDepartment);
                View.Property(p => p.WorkShopName);
                View.Property(p => p.InstallationLocation);
                View.Property(p => p.CycleType);
                View.Property(p => p.BillSourceType);
                View.Property(p => p.LubricationStatus);
                View.Property(p => p.ApprovalStatus);
                View.Property(p => p.StartDateTime);
                View.Property(p => p.EndDateTime);
                View.Property(p => p.PlanDate).UseDateEditor();
                View.Property(p => p.ExecutorName).HasLabel("执行人");
                View.Property(p => p.Department).HasLabel("责任部门");
                View.Property(p => p.TotalHours).UseSpinEditor(p =>
                {
                    p.AllowDecimals = true;
                    p.DecimalPrecision = 1;
                });
                View.Property(p => p.Remark);

                View.ChildrenProperty(p => p.LubricationDetailList).UseViewGroup(LubricationDetailViewConfig.SeeView).HasLabel("润滑项目").HasOrderNo(1);
                View.ChildrenProperty(p => p.LubricationSparePartList).HasLabel("备件更换").Show(ChildShowInWhere.All).HasOrderNo(2);
                View.ChildrenProperty(p => p.LubricationSparePartApplyList).HasLabel("备件申请").Show(ChildShowInWhere.All).HasOrderNo(3);

                View.ChildrenProperty(p => p.LubricationAttachmentList).UseViewGroup(LubricationAttachmentViewConfig.SeeView).HasLabel("图片上传").HasOrderNo(4);
                View.ChildrenProperty(p => p.LubricationWorkHourList).UseViewGroup(LubricationWorkHourViewConfig.SeeView).HasLabel("工时登记").HasOrderNo(5);

                View.AttachChildrenProperty(typeof(WorkFlowRecord), w =>
                {
                    var args = w as ChildPagingDataArgs;
                    var parent = args.Parent.CastTo<Lubrication>();
                    if (parent == null)
                    {
                        return new EntityList<WorkFlowRecord>();
                    }
                    return RT.Service.Resolve<WorkFlowRecordController>().GetWorkFlowRecordBySourceId(parent.Id, typeof(Lubrication).FullName, args.SortInfo, args.PagingInfo);
                }).HasLabel("审核记录").HasOrderNo(6);
            }
        }

        /// <summary>
        /// 添加页面
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.Lubrications.Behaviors.LubricationDetailBehavior");
            View.AddBehavior("SIE.Web.EMS.Lubrications.Behaviors.AddEquipmentCardBehavior");
            View.HasDetailColumnsCount(4);
            View.UseCommands(LubricationCommands.LubricationSaveCommand);
            //只有保存
            using (View.OrderProperties())
            {
                View.Property(p => p.LubricationNo).Readonly();

                View.Property(p => p.EquipAccountId).UseDataSource((e, c, r) =>
                {
                    //只能润滑管理状态为使用中的设备台账
                    var list = RT.Service.Resolve<EquipAccountSelectController>().GetAllCheckPlanEquipAccounts(r, c);                    
                    return list;
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                    dic.Add(nameof(e.WorkShopName), nameof(e.EquipAccount.WorkShopName));
                    dic.Add(nameof(e.InstallationLocation), nameof(e.EquipAccount.InstallationLocation));
                    m.DicLinkField = dic;
                }).HasLabel("设备编码");
                View.Property(p => p.EquipAccountName).Readonly().HasLabel("设备名称");
                View.Property(p => p.WorkShopName).Readonly();

                View.Property(p => p.InstallationLocation).Readonly();
                View.Property(p => p.CycleType).Readonly();
                View.Property(p => p.PlanDate).UseDateEditor(p =>
                {
                    p.Format = "Y-m-d";
                    p.MinValue = DateTime.Today.ToString();
                }).DefaultValue(DateTime.Today);
                View.Property(p => p.BillSourceType).Readonly();

                View.Property(p => p.LubricationStatus).Readonly();
                View.Property(p => p.StartDateTime);
                View.Property(p => p.EndDateTime);
                View.Property(p => p.Department).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword);
                    if (departments == null || departments.Count <= 0)
                    {
                        return new EntityList<Enterprise>();
                    }
                    departments.ForEach(p => p.TreePId = null);
                    return departments;
                }).UsePagingLookUpEditor().HasLabel("责任部门");

                View.Property(p => p.ExecutorName).Readonly().HasLabel("执行人");
                View.Property(p => p.TotalHours).UseSpinEditor(p =>
                {
                    p.AllowDecimals = true;
                    p.DecimalPrecision = 1;
                }).Readonly();
                View.Property(p => p.Remark).ShowInDetail(columnSpan: 2);

                View.ChildrenProperty(p => p.LubricationDetailList).HasLabel("润滑项目").HasOrderNo(1);
                View.ChildrenProperty(p => p.LubricationSparePartList).HasLabel("备件更换").Show(ChildShowInWhere.Hide).HasOrderNo(2);
                View.ChildrenProperty(p => p.LubricationSparePartApplyList).HasLabel("备件申请").Show(ChildShowInWhere.Hide).HasOrderNo(3);
                View.ChildrenProperty(p => p.LubricationAttachmentList).HasLabel("图片上传").Show(ChildShowInWhere.Hide).HasOrderNo(4);
                View.ChildrenProperty(p => p.LubricationWorkHourList).HasLabel("工时登记").Show(ChildShowInWhere.Hide).HasOrderNo(5);

            }
        }


        /// <summary>
        /// 添加记录页面
        /// </summary>
        private void ConfigAddReportView()
        {
            //保存,提交,报修
            View.AddBehavior("SIE.Web.EMS.Lubrications.Behaviors.LubricationDetailBehavior");
            View.HasDetailColumnsCount(4);
            View.UseCommands(LubricationCommands.LubricationDetailSaveCommand, LubricationCommands.LubricationDetailSubmitCommand);
            View.UseCommands("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.AddLubricationRepairCommand");

            //只有保存
            using (View.OrderProperties())
            {
                View.Property(p => p.LubricationNo).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountId).Readonly().HasLabel("设备编码").Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountName).Readonly().HasLabel("设备名称").Show(ShowInWhere.All);
                View.Property(p => p.WorkShopName).Readonly().Show(ShowInWhere.All);

                View.Property(p => p.InstallationLocation).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CycleType).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.PlanDate).UseDateEditor().Readonly(p => p.BillSourceType == SIE.Equipments.Enums.BillSourceType.Automatically).Show(ShowInWhere.All);
                View.Property(p => p.BillSourceType).Readonly().Show(ShowInWhere.All);

                View.Property(p => p.LubricationStatus).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.StartDateTime).Show(ShowInWhere.All);
                View.Property(p => p.EndDateTime).Show(ShowInWhere.All);
                View.Property(p => p.Department).UseDataSource((source, pagingInfo, keyword) =>
                {
                    var departments = RT.Service.Resolve<EnterpriseController>().GetDepartments(pagingInfo, keyword);
                    if (departments == null || departments.Count <= 0)
                    {
                        return new EntityList<Enterprise>();
                    }
                    departments.ForEach(p => p.TreePId = null);
                    return departments;
                }).UsePagingLookUpEditor().HasLabel("责任部门").Readonly().Show(ShowInWhere.All);

                View.Property(p => p.ExecutorName).Readonly().HasLabel("执行人").Show(ShowInWhere.All);
                View.Property(p => p.TotalHours).UseSpinEditor(p =>
                {
                    p.AllowDecimals = true;
                    p.DecimalPrecision = 1;
                }).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Remark).ShowInDetail(columnSpan: 2);

                View.Property(p => p.LastLubricationSummary).UseMemoEditor().ShowInDetail(columnSpan: 2, rowSpan: 2).Readonly().Show(ShowInWhere.All); 
                View.Property(p => p.LubricationSummary).UseMemoEditor().ShowInDetail(columnSpan: 2, rowSpan: 2)
                    .Readonly(p => p.LubricationStatus != SIE.EMS.Enums.LubricationStatus.Pending && p.LubricationStatus != SIE.EMS.Enums.LubricationStatus.Doing).Show(ShowInWhere.All);


                View.ChildrenProperty(p => p.LubricationDetailList).UseViewGroup(LubricationDetailViewConfig.AddReportView).HasLabel("润滑项目").HasOrderNo(1).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.LubricationSparePartList).HasLabel("备件更换").Show(ChildShowInWhere.All).HasOrderNo(2);
                View.ChildrenProperty(p => p.LubricationSparePartApplyList).HasLabel("备件申请").Show(ChildShowInWhere.All).HasOrderNo(3);
                View.ChildrenProperty(p => p.LubricationAttachmentList).HasLabel("图片上传").HasOrderNo(4).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.LubricationWorkHourList).HasLabel("工时登记").HasOrderNo(5).Show(ChildShowInWhere.All);

            }

        }
        /// <summary>
        /// 查看记录页面
        /// </summary>

        private void ConfigSeeReportView()
        {
            View.AddBehavior("SIE.Web.EMS.Lubrications.Behaviors.LubricationDetailBehavior");
            View.AddBehavior("SIE.Web.EMS.Lubrications.Behaviors.LubricationDetailCloseBehavior");
            View.HasDetailColumnsCount(4);
            using (View.OrderProperties())
            {
                View.Property(p => p.LubricationNo).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountId).Readonly().HasLabel("设备编码").Show(ShowInWhere.All);
                View.Property(p => p.EquipAccountName).Readonly().HasLabel("设备名称").Show(ShowInWhere.All);
                View.Property(p => p.WorkShopName).Readonly().Show(ShowInWhere.All);

                View.Property(p => p.InstallationLocation).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.CycleType).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.PlanDate).Readonly().UseDateEditor().Show(ShowInWhere.All);
                View.Property(p => p.BillSourceType).Readonly().Show(ShowInWhere.All);

                View.Property(p => p.LubricationStatus).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.StartDateTime).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.EndDateTime).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Department).Readonly().HasLabel("责任部门").Show(ShowInWhere.All);

                View.Property(p => p.ExecutorName).Readonly().HasLabel("执行人").Show(ShowInWhere.All);
                View.Property(p => p.TotalHours).UseSpinEditor(p =>
                {
                    p.AllowDecimals = true;
                    p.DecimalPrecision = 1;
                }).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Remark).Readonly().ShowInDetail(columnSpan: 2);

                View.Property(p => p.LastLubricationSummary).UseMemoEditor().ShowInDetail(columnSpan: 2, rowSpan: 2).Readonly().Show(ShowInWhere.All).Readonly();
                View.Property(p => p.LubricationSummary).UseMemoEditor().ShowInDetail(columnSpan: 2, rowSpan: 2).Readonly().Show(ShowInWhere.All);

                View.ChildrenProperty(p => p.LubricationDetailList).UseViewGroup(LubricationDetailViewConfig.SeeView).HasLabel("润滑项目").HasOrderNo(1).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.LubricationSparePartList).HasLabel("备件更换").Show(ChildShowInWhere.All).HasOrderNo(2);
                View.ChildrenProperty(p => p.LubricationSparePartApplyList).HasLabel("备件申请").Show(ChildShowInWhere.All).HasOrderNo(3);
                View.ChildrenProperty(p => p.LubricationAttachmentList).UseViewGroup(LubricationAttachmentViewConfig.SeeView).HasLabel("图片上传").HasOrderNo(4).Show(ChildShowInWhere.All);
                View.ChildrenProperty(p => p.LubricationWorkHourList).UseViewGroup(LubricationWorkHourViewConfig.SeeView).HasLabel("工时登记").HasOrderNo(5).Show(ChildShowInWhere.All);
            }
        }
    }
}