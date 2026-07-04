using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepairs.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修工时视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EquipRepairWorkingHoursViewConfig : WebViewConfig<EquipRepairWorkingHours>
    {
        #region 维修工时 WorkingHours
        /// <summary>
        /// 维修工时
        /// </summary>
        [Label("维修工时(H)")]
        public static readonly Property<double?> WorkingHoursProperty = P<EquipRepairWorkingHours>.RegisterExtensionReadOnly("WorkingHours", typeof(EquipRepairWorkingHoursViewConfig),
            GetWorkingHours, EquipRepairWorkingHours.BeginTimeProperty, EquipRepairWorkingHours.EndTimeProperty);

        /// <summary>
        /// 维修工时
        /// </summary>
        public static double? GetWorkingHours(EquipRepairWorkingHours me)
        {
            double? workingHours = null;
            if (me != null && me.BeginTime.HasValue && me.EndTime.HasValue)
            {
                workingHours = Math.Round((me.EndTime.Value - me.BeginTime.Value).TotalHours, 2);
            }
            return workingHours;
        }
        #endregion

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Repairer).Readonly().HasLabel("其他维修人员");
            View.Property(p => p.BeginTime).ShowInList(width: 150).Readonly(p => p.RepairBillState != EquipRepairState.Repairing && p.RepairBillState != EquipRepairState.Suspending);
            View.Property(p => p.EndTime).ShowInList(width: 150).Readonly(p => p.RepairBillState != EquipRepairState.Repairing && p.RepairBillState != EquipRepairState.Suspending);
            View.Property(WorkingHoursProperty).Readonly();
            View.Property(p => p.IsRepairMaster).Readonly();
            View.Property(p => p.IsRepairEmployee).Readonly();
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }
    }
}
