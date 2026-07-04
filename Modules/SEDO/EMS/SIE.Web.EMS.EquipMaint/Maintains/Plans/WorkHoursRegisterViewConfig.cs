using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans
{
    /// <summary>
    /// 工时登记视图配置
    /// </summary>
    public class WorkHoursRegisterViewConfig : WebViewConfig<WorkHoursRegister>
    {
        /// <summary>
        /// 保养确认所用的工时登记视图
        /// </summary>
        public const string MaintainConfirmationListView = "MaintainConfirmationListView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            View.DeclareExtendViewGroup(MaintainConfirmationListView);
            if (ViewGroup == MaintainConfirmationListView)
            {
                ConfigListView();
                View.ClearCommands();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.EquipMaint.Maintains.Plans.Scripts.WorkHoursBehavior");
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete)
                .ReplaceCommands(WebCommandNames.Add, "SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands.AddWorkHoursRegisterCommand");

            using (View.OrderProperties())
            {
                View.Property(p => p.EmployeeId).Show(ShowInWhere.All).Readonly(ViewGroup == MaintainConfirmationListView).HasLabel("执行人".L10N() + "*"); ;
                View.Property(p => p.BeginDay).UseDateTimeEditor().ShowInList(200).Show(ShowInWhere.All).Readonly(ViewGroup == MaintainConfirmationListView).HasLabel("保养开始日期".L10N() + "*");
                View.Property(p => p.EndDay).UseDateTimeEditor().ShowInList(200).Show(ShowInWhere.All).Readonly(ViewGroup == MaintainConfirmationListView).HasLabel("保养结束日期".L10N() + "*");
                View.Property(p => p.WorkHours).Show(ShowInWhere.All).Readonly(ViewGroup == MaintainConfirmationListView);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}