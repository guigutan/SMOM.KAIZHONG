using SIE.EMS.Checks.Plans;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.EMS.Checks.Plans.Commands;
using System;

namespace SIE.Web.EMS.Checks.Plans
{
    /// <summary>
    /// 点检计划备件申请 视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class CheckPlanSparePartAplViewConfig : WebViewConfig<CheckPlanSparePartApl>
    {
        /// <summary>
        /// 点检确认所用的备件申请视图
        /// </summary>
        public const string CheckConfirmationListView = "CheckConfirmationListView";

        /// <summary>
        /// 
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(CheckPlanViewModel));
            View.DeclareExtendViewGroup(CheckConfirmationListView);
            if (ViewGroup == CheckConfirmationListView)
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
            View.UseCommands(WebCommandNames.Edit, typeof(DeleteCheckSparePartAplCommand).FullName);
            View.AddBehavior("SIE.Web.EMS.Checks.Plans.Scripts.CheckPlanSparePartAplBehavior");

            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).HasLabel("备件编码".L10N()+"*").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartNameView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecificationView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.OutStockWarehouseId).HasLabel("出库仓库".L10N() + "*").UseDataSource((s, p, k) =>
                {
                    var warehouses = RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(p, k);
                    return warehouses;
                }).Show(ShowInWhere.All).Readonly(p => p.IsApply).Readonly(ViewGroup == CheckConfirmationListView);
                View.Property(p => p.StoreQty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ApplyQty).HasLabel("申请数量".L10N() + "*").UseSpinEditor(p => { p.MinValue = 0; p.AllowNegative = false; }).Show(ShowInWhere.All).Readonly(p => p.IsApply).Readonly(ViewGroup == CheckConfirmationListView);
                View.Property(p => p.ApplyDetailId).UsePagingLookUpEditor((m, e) =>
                {
                    m.DisplayField = CheckPlanSparePartApl.ApplyNoViewProperty.Name;
                    m.BindDisplayField = CheckPlanSparePartApl.ApplyNoViewProperty.Name;
                }).ShowInList(width: 150).Readonly();
                View.Property(p => p.AppStateView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UnitView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsApply).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Remark).ShowInList(width: 200).Readonly(p => p.IsApply).Readonly(ViewGroup == CheckConfirmationListView);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
