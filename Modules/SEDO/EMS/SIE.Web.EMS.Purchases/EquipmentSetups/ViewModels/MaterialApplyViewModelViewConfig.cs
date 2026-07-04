using SIE.Domain;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.Purchases.EquipmentSetups.ViewModels;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.EMS.Purchases.EquipmentSetups.ViewModels
{
    /// <summary>
    /// 领料申请界面
    /// </summary>
    internal class MaterialApplyViewModelViewConfig : WebViewConfig<MaterialApplyViewModel>
    {
        /// <summary>
        /// 明细界面
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(EquipmentSetup));
            View.UseDetail(2);
            View.Property(p => p.DemandTime);
            View.Property(p => p.Remark);
            View.AttachChildrenProperty(typeof(MaterialApplyDetailViewModel), w => new EntityList<MaterialApplyDetailViewModel>()).HasLabel("").Show(ChildShowInWhere.All);
        }
    }

    /// <summary>
    /// 领料申请明细界面
    /// </summary>
    internal class MaterialApplyDetailViewModelViewConfig : WebViewConfig<MaterialApplyDetailViewModel>
    {
        /// <summary>
        /// 列表界面
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(EquipmentSetup));
            View.UseCommands("SIE.Web.EMS.Purchases.EquipmentSetups.Commands.AddApplyDetailCommand", WebCommandNames.Delete);
            View.Property(p => p.SparePartId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.SparePartName), nameof(e.SparePart.SparePartName));
                keyValues.Add(nameof(e.UnitName), nameof(e.SparePart.UnitName));
                keyValues.Add(nameof(e.Specification), nameof(e.SparePart.Specification));
                keyValues.Add(nameof(e.PartType), nameof(e.SparePart.SpartType));
                keyValues.Add(nameof(e.ControlMethod), nameof(e.SparePart.ControlMethod));
                m.DicLinkField = keyValues;
            }).ShowInList(120);
            View.Property(p => p.SparePartName).Readonly();
            View.Property(p => p.ApplyQty).UseSpinEditor(p =>
            {
                p.MinValue = 0.01;
                p.DecimalPrecision = 2;
            });
            View.Property(p => p.UnitName).ShowInList(60).Readonly();
            View.Property(p => p.WarehouseId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.WarehouseName), nameof(e.Warehouse.Name));
                m.DicLinkField = keyValues;
            });
            View.Property(p => p.WarehouseName).Readonly();
            View.Property(p => p.WarehouseQty).Readonly();
            View.Property(p => p.Specification).ShowInList(130).Readonly();
            View.Property(p => p.PartType).Readonly();
            View.Property(p => p.ControlMethod).ShowInList(80).Readonly();
        }
    }
}
