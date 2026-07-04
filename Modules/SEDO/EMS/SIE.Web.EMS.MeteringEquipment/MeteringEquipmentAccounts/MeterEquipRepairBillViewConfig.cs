using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.MeteringEquipment.MeteringEquipmentAccounts;
using SIE.Web.Common.Configs.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts
{
    /// <summary>
    /// 计量设备台账维修纪录视图
    /// </summary>
    public class MeterEquipRepairBillViewConfig : WebViewConfig<EquipRepairBill>
    {
        /// <summary>
        /// 设备台账维修单视图
        /// </summary>
        public static readonly string MeterEquipAccountRepairView = "MeterEquipAccountRepairView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(MeterEquipAccountRepairView);
            if (ViewGroup == MeterEquipAccountRepairView)
            {
                ConfigEquipAccountRepair();
            }
        }

        /// <summary>
        /// 设备台账维修单视图
        /// </summary>
        public void ConfigEquipAccountRepair()
        {
            View.AssignAuthorize(typeof(MeteringEquipmentAccount));
            View.AddBehavior("SIE.Web.EMS.EquipRepair.EquipRepairs.Behaviors.EquipRepairToolBarBehavior");
            View.UseCommand("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.OpenRepairBillViewCommand");
            View.UseCommand("SIE.Web.EMS.MeteringEquipment.MeteringEquipmentAccounts.Commands.MeterEquipSearchRepairCommand");
            View.RemoveCommands(ConfigCommands.ModuleConfigEditCommand);

            using (View.OrderProperties())
            {
                View.Property(p => p.RepairState).Show().Readonly();
                View.Property(p => p.RepairNo).Show().Readonly();
                View.Property(p => p.SourceNo).Show().Readonly();
                View.Property(p => p.SourceType).Show().Readonly();
                View.Property(p => p.RepairType).Show().Readonly();
                View.Property(p => p.UrgentDegree).Show().Readonly();
                View.Property(p => p.ApplyRepairEmployeeId).Show().Readonly();
                View.Property(p => p.ApplyRepairDate).Show().Readonly();
                View.Property(p => p.RepairMasterId).Show().Readonly();
                View.Property(p => p.ReceiveOrderDate).Show().Readonly();
                View.Property(p => p.TransferOrderDate).Show().Readonly();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairBillProjectList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairOperationRecList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairWorkingHoursList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairSparePartChgList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipRepairSparePartAplList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.HandoverConfirmDetailList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EngineerConfirmDetailList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}
