using SIE.Domain;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Web.EMS.Purchases.EquipmentSetups.Commands;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SIE.Web.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试工时登记视图配置
    /// </summary>
    internal class SetupWorkHourViewConfig : WebViewConfig<SetupWorkHour>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //主表审核状态为【通过】，状态不为【已完成】和【交机确认】时，该页签才能进行操作
            Expression<Func<SetupWorkHour, bool>> exp =
                p => p.ApprovalStatus != ApprovalStatus.Audited || p.SetupStatus == SetupStatus.Done || p.SetupStatus == SetupStatus.DeliveryConfirm;

            View.AddBehavior("SIE.Web.EMS.Purchases.EquipmentSetups.SetupWorkHourBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.AddWorkHourCommand", WebCommandNames.Edit, typeof(DeleteWorkHourCommand).FullName);
            View.Property(p => p.EmployeeCode).Readonly();
            View.Property(p => p.EmployeeId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EmployeeCode), nameof(e.Employee.Code));
                m.DicLinkField = keyValues;
            }).HasLabel("姓名").Readonly(exp);
            View.Property(p => p.StartDateTime).ShowInList(150).Readonly(exp);
            View.Property(p => p.EndDateTime).ShowInList(150).Readonly(exp);
            View.Property(p => p.EquipmentSetupPlanId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var workHour = source as SetupWorkHour;
                if (workHour == null)
                {
                    return new EntityList<EquipmentSetupPlan>();
                }
                return RT.Service.Resolve<EquipmentSetupController>().GetPlansBySetupId(workHour.EquipmentSetupId, pagingInfo, keyword);
            }).ShowInList(150).HasLabel("工作节点").Readonly(exp);
            View.Property(p => p.EquipAccountId).UseDataSource((source, pagingInfo, keyword) =>
            {
                var workHour = source as SetupWorkHour;
                if (workHour == null)
                {
                    return new EntityList<EquipAccount>();
                }
                return RT.Service.Resolve<EquipmentSetupController>().GetEquipmentsBySetupId(workHour.EquipmentSetupId, pagingInfo);
            }).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("设备编码").Readonly(exp);
            View.Property(p => p.EquipAccountName).ShowInList(150).Readonly();
            View.Property(p => p.Hours).UseSpinEditor(p =>
            {
                p.MinValue = 0.1;
                p.DecimalPrecision = 1;
            }).Readonly(exp);
            View.Property(p => p.Remark).ShowInList(200).Readonly(exp);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}