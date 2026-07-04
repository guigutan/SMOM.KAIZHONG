using SIE.EMS.Checks.Plans.ViewModels;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipTypes;
using SIE.Web.Common;

namespace SIE.Web.EMS.Equipments.Accounts
{
    /// <summary>
    /// 设备台账视图视图配置
    /// </summary>
    public class EquipAccountSelectViewConfig : WebViewConfig<EquipAccountSelect>
    {
        /// <summary>
        /// 字体显示宽度
        /// </summary>
        private const int charDisplayWidth = 20;

        /// <summary>
        /// 批量添加点检计划窗口-设备台账
        /// </summary>
        public const string CheckPlanBatchAddList = "CheckPlanBatchAddList";

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(CheckPlanBatchAddList);

            if (ViewGroup == CheckPlanBatchAddList)
            {
                CheckPlanBatchAddListView();
            }
        }


        /// <summary>
        /// 配置批量添加点检计划视图
        /// </summary>
        protected void CheckPlanBatchAddListView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            View.ClearCommands();
            View.DisableEditing();
            View.UseCommands("SIE.Web.EMS.Equipments.Accounts.Commands.SelEquipAccountCommand","SIE.Web.EMS.Equipments.Accounts.Commands.DeleteSelCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).HasLabel("设备编码").Readonly().ShowInList(width: charDisplayWidth * 8);
                View.Property(p => p.Name).HasLabel("设备名称").Readonly().ShowInList(width: charDisplayWidth * 8);

                View.Property(p => p.Alias).Readonly().ShowInList(width: charDisplayWidth * 6);

                View.Property(p => p.ModelCode).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.ModelName).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.State).Readonly().ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.UseState).Readonly().ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.Frozen).Readonly().ShowInList(width: charDisplayWidth * 3);

                View.Property(p => p.EquipTypeCode).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.EquipTypeName).HasLabel("设备类型").Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.EquipTypeCategory)
                    .UseCatalogEditor(c => { c.CatalogType = EquipType.EquipTypeCatalogType;c.ReloadDataOnPopping = true; }).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.IndustryCategory).Readonly().ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.IsVirtual).Readonly().ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.IsCustomsSupervision).Readonly().HasLabel("海关监管").ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.EquipmentGrading).Readonly().ShowInList(width: charDisplayWidth * 4);

                View.Property(p => p.UseLevel).Readonly().HasLabel("ABC分类").ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.FactoryId).Readonly().ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.UseDepartmentId).Readonly().ShowInList(width: charDisplayWidth * 6);

                View.Property(p => p.WorkShopId).Readonly().HasLabel("车间").ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.ResourceId).Readonly().HasLabel("产线").ShowInList(width: charDisplayWidth * 6);

                View.Property(p => p.WarehouseId).Readonly().HasLabel("仓库").ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.StorageLocationId).Readonly().HasLabel("库位").ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.AdministratorId).Readonly().HasLabel("保管人").ShowInList(width: charDisplayWidth * 4);

                View.Property(p => p.ProcessId).Readonly().HasLabel("工序").ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.ManageDepartmentId).Readonly().HasLabel("管理部门").ShowInList(width: charDisplayWidth * 6);

                View.Property(p => p.Proprietorship).Readonly().ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.ResPersonId).Readonly().ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.UserId).Readonly().ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.PurchaseUnit).Readonly().ShowInList(width: charDisplayWidth * 10);
                View.Property(p => p.Manufacturer).Readonly().ShowInList(width: charDisplayWidth * 10);

                View.Property(p => p.SupplierId).Readonly().HasLabel("供应商编码").ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.SupplierName).Readonly().ShowInList(width: charDisplayWidth * 5);
                View.Property(p => p.PurchaseOrderNo).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.InstallationLocation).Readonly().ShowInList(width: charDisplayWidth * 10);
                View.Property(p => p.OriginalSerialNumber).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.RFID).Readonly().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.EnterDate).Readonly().UseDateEditor().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.CardDate).Readonly().UseDateEditor().ShowInList(width: charDisplayWidth * 6);
                View.Property(p => p.UsefulLife).Readonly().UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; }).ShowInList(width: charDisplayWidth * 4);
                View.Property(p => p.WarrantyPeriod).Readonly().ShowInList(width: charDisplayWidth * 5);
            }
            View.ChildrenProperty(p => p.AttachmentList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.ResumeList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.EquipAccountLocationList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.EquipAccountPhysicalUnionList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.ProcessList).Show(ChildShowInWhere.Hide);
            View.ChildrenProperty(p => p.PcbSlotList).Show(ChildShowInWhere.Hide);
        }
    }
}
