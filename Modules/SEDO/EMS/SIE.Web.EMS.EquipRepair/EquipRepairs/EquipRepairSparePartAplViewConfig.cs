using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.EMS.EquipRepair.EquipRepairs.Commands;
using System;

namespace SIE.Web.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 设备维修备件申请 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EquipRepairSparePartAplViewConfig : WebViewConfig<EquipRepairSparePartApl>
    {
        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(EquipRepairBill));
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelEquipBomAplCommand).FullName, typeof(SelStandardSparePartAplCommand).FullName,typeof(SelSparePartAplCommand).FullName);
            View.UseCommands(typeof(GenerateSparePartAppCommand).FullName);
            View.UseCommands("SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.EditEquipRepairSparePartAplCommand", typeof(DeleteEquipRepairSparePartAplCommand).FullName);
            View.DefineFormChildSaveMode(FormChildSaveMode.None);
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).Readonly().HasLabel("备件编码".L10N()+"*");
                View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartNameView).Readonly();
                View.Property(p => p.SpecificationView).Readonly();
                View.Property(p => p.OutStockWarehouseId).UseDataSource((s, p, k) =>
                {
                    var warehouses = RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(p, k);
                    return warehouses;
                }).Readonly(p => p.IsApply).HasLabel("出库仓库".L10N() + "*");
                View.Property(p => p.StoreQty).Readonly();
                View.Property(p => p.ApplyQty).Readonly(p => p.IsApply).UseSpinEditor(m=>m.MinValue=0).HasLabel("申请数量".L10N() + "*");
                View.Property(p => p.ApplyDetailId).UsePagingLookUpEditor((m, e) =>
                {
                    m.DisplayField = EquipRepairSparePartApl.ApplyNoViewProperty.Name;
                    m.BindDisplayField = EquipRepairSparePartApl.ApplyNoViewProperty.Name;
                }).Readonly();
                View.Property(p => p.AppStateView).Readonly();
                View.Property(p => p.UnitView).Readonly();
                View.Property(p => p.IsApply).Readonly();
                View.Property(p => p.Remark).Readonly(p => p.IsApply);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
