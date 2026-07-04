using SIE.Core.Equipments;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.Equipments.EquipAccounts;
using SIE.Web.Common;

namespace SIE.Web.EMS.EquipMaint.Equipments.Accounts
{
    /// <summary>
    /// 设备台账扩展视图
    /// </summary>
    public class EquipAccountSelectExtensionViewConfig : WebViewConfig<EquipAccountSelect>
    {
        /// <summary>
        /// 保养计划批量添加-选择设备台账视图
        /// </summary>
        public readonly static string MaintainPlanBatchAddList = "MaintainPlanBatchAddList";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(MaintainPlanBatchAddList);
            if (ViewGroup == MaintainPlanBatchAddList)
            {
                MaintainPlanBatchAddView();
            }
        }

        /// <summary>
        /// 保养计划批量添加-选择设备台账视图
        /// </summary>
        protected void MaintainPlanBatchAddView()
        {
            View.AssignAuthorize(typeof(MaintainPlanViewModel));
            View.ClearCommands();
            View.DisableEditing();
            View.UseCommands("SIE.Web.EMS.EquipMaint.Equipments.Accounts.Commands.MaintainAddEquipCommand", "SIE.Web.EMS.EquipMaint.Equipments.Accounts.Commands.DeleteSelCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All).DisableSort().ShowInList(width: 105);
                View.Property(p => p.Name).Show(ShowInWhere.All).DisableSort().ShowInList(width: 105);
                View.Property(p => p.ModelCode).HasLabel("设备型号编码").Show(ShowInWhere.All).DisableSort().ShowInList(width: 105);
                View.Property(p => p.EquipTypeName).HasLabel("设备类型").Show(ShowInWhere.All).DisableSort().ShowInList(width: 105);
                View.Property(p => p.WorkShopName).HasLabel("车间").Show(ShowInWhere.All).DisableSort().ShowInList(width: 105);
                View.Property(p => p.ResourceName).HasLabel("产线").Show(ShowInWhere.All).DisableSort().ShowInList(width: 105);
                View.Property(p => p.EquipTypeCategory).ShowInList(width: 200).UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType;c.ReloadDataOnPopping = true; });

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.ResumeList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.Hide);

                View.ChildrenProperty(p => p.PcbSlotList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.ProcessList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipAccountLocationList).Show(ChildShowInWhere.Hide);
                View.ChildrenProperty(p => p.EquipAccountPhysicalUnionList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}
