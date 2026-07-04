using SIE.Domain;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.EMS.Equipments.Accounts.ViewModels
{
    /// <summary>
    /// TPM信息VM 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class TpmViewModelViewConfig : WebViewConfig<TpmViewModel>
    {
        /// <summary>
        /// TPM点检
        /// </summary>
        public const string CheckPlanViewGroup = "CheckPlanViewGroup";

        /// <summary>
        /// TPM保养
        /// </summary>
        public const string MaintainPlanViewGroup = "MaintainPlanViewGroup";

        /// <summary>
        /// TPM校验
        /// </summary>
        public const string CalibrationPlanViewGroup = "CalibrationPlanViewGroup";

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(CheckPlanViewGroup, MaintainPlanViewGroup, CalibrationPlanViewGroup);

            if (ViewGroup == CheckPlanViewGroup)
                CheckPlanView();
            if (ViewGroup == MaintainPlanViewGroup)
                MaintainPlanView();
            if (ViewGroup == CalibrationPlanViewGroup)
                CalibrationPlanView();
        }

        /// <summary>
        /// 点检计划TPM记录
        /// </summary>
        void CheckPlanView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.LastExecuteTime).HasLabel("上次已点检时间").ShowInList(width:150);
                View.Property(p => p.CurrentToBeExecuteTime).HasLabel("当前待点检时间").ShowInList(width:150);
                View.Property(p => p.NextToBeExecuteTime).HasLabel("下次待点检时间").ShowInList(width:150);
            }
        }

        /// <summary>
        /// 保养计划TPM记录
        /// </summary>
        void MaintainPlanView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.LastExecuteTime).HasLabel("上次已保养时间").ShowInList(width:150);
                View.Property(p => p.CurrentToBeExecuteTime).HasLabel("当前待保养时间").ShowInList(width:150);
                View.Property(p => p.NextToBeExecuteTime).HasLabel("下次待保养时间").ShowInList(width:150);
            }
        }

        /// <summary>
        /// 校验计划TPM记录
        /// </summary>
        void CalibrationPlanView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.LastExecuteTime).HasLabel("上次已校验时间").ShowInList(width:150);
                View.Property(p => p.CurrentToBeExecuteTime).HasLabel("当前校验检时间").ShowInList(width:150);
                View.Property(p => p.NextToBeExecuteTime).HasLabel("下次校验检时间").ShowInList(width:150);
            }
        }
    }
}
