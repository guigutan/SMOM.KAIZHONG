using SIE.EMS.Maintains.Plans;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.EMS.EquipMaint.Maintains.Plans.Commands;
using System;

namespace SIE.Web.EMS.EquipMaint.Maintains.Plans
{
    /// <summary>
    /// 保养计划备件申请 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class MaintainPlanSparePartAplViewConfig : WebViewConfig<MaintainPlanSparePartApl>
    {
        /// <summary>
        /// 保养确认所用的备件更换视图
        /// </summary>
        public const string MaintainConfirmationListView = "MaintainConfirmationListView";

        /// <summary>
        /// 
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

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelEquipBomAplCommand).FullName, typeof(SelSparePartAplCommand).FullName);
            View.UseCommands(typeof(GenerateSparePartAppCommand).FullName);
            View.UseCommands(WebCommandNames.Edit, typeof(DeleteMaintainSparePartAplCommand).FullName);
            View.AddBehavior("SIE.Web.EMS.EquipMaint.Maintains.Plans.Scripts.MaintainPlanSparePartAplBehavior");

            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).Show(ShowInWhere.All).Readonly().HasLabel("备件编码".L10N()+"*");
                View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartNameView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecificationView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.OutStockWarehouseId).UseDataSource((s, p, k) =>
                {
                    var warehouses = RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(p, k);
                    return warehouses;
                }).Show(ShowInWhere.All).Readonly(p => p.IsApply).Readonly(ViewGroup == MaintainConfirmationListView).HasLabel("出库仓库".L10N() + "*");
                View.Property(p => p.StoreQty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ApplyQty).Show(ShowInWhere.All).UseSpinEditor(m=>m.MinValue=0).Readonly(p => p.IsApply).Readonly(ViewGroup == MaintainConfirmationListView).HasLabel("申请数量".L10N() + "*");
                View.Property(p => p.ApplyDetailId).UsePagingLookUpEditor((m, e) =>
                {
                    m.DisplayField = MaintainPlanSparePartApl.ApplyNoViewProperty.Name;
                    m.BindDisplayField = MaintainPlanSparePartApl.ApplyNoViewProperty.Name;
                }).ShowInList(width: 150).Readonly();
                View.Property(p => p.AppStateView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UnitView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsApply).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Remark).ShowInList(width: 200).Readonly(p => p.IsApply).Readonly(ViewGroup == MaintainConfirmationListView);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
