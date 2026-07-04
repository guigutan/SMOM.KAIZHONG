using SIE.EMS.Lubrications;
using SIE.MetaModel.View;
using SIE.Warehouses;
using SIE.Web.EMS.Lubrications.Commands;

namespace SIE.Web.EMS.Lubrications
{
    /// <summary>
    /// 润滑备件申请视图配置
    /// </summary>
    public class LubricationSparePartApplyViewConfig : WebViewConfig<LubricationSparePartApply>
    {
        /// <summary>
        /// 查看记录
        /// </summary>
        public const string SeeView = "SeeView";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(SeeView);
            if (ViewGroup == SeeView)
            {
                ConfigSeeView();
            }

        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelEquipBomAplCommand).FullName, typeof(SelSparePartAplCommand).FullName);
            View.UseCommands(typeof(GenerateSparePartAppCommand).FullName);
            View.UseCommands(WebCommandNames.Edit, typeof(DeleteLubricationSparePartAplCommand).FullName);
            View.AddBehavior("SIE.Web.EMS.Lubrications.Behaviors.LubricationSparePartAplBehavior");

            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartNameView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecificationView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.OutStockWarehouseId).UseDataSource((s, p, k) =>
                {
                    var warehouses = RT.Service.Resolve<WarehouseController>().GetEnableWarehouses(p, k);
                    return warehouses;
                }).Show(ShowInWhere.All).Readonly(p => p.IsApply);
                View.Property(p => p.StoreQty).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.ApplyQty).Show(ShowInWhere.All).Readonly(p => p.IsApply);
                View.Property(p => p.ApplyDetailId).UsePagingLookUpEditor((m, e) =>
                {
                    m.DisplayField = LubricationSparePartApply.ApplyNoViewProperty.Name;
                    m.BindDisplayField = LubricationSparePartApply.ApplyNoViewProperty.Name;
                }).HasLabel("备件申请单").ShowInList(150).Readonly();
                View.Property(p => p.AppStateView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UnitView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.IsApply).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly(p => p.IsApply);

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }


        /// <summary>
        /// 查看页面
        /// </summary>
        public void ConfigSeeView()
        {
            View.FormEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCodeView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SparePartId).Readonly().Show(ShowInWhere.Hide);
                View.Property(p => p.SparePartNameView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.SpecificationView).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.OutStockWarehouseId).Readonly(p => p.IsApply).Show(ShowInWhere.All);
                View.Property(p => p.StoreQty).Show(ShowInWhere.All).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.ApplyQty).Show(ShowInWhere.All).Readonly(p => p.IsApply).Show(ShowInWhere.All);
                View.Property(p => p.ApplyDetailId).UsePagingLookUpEditor((m, e) =>
                {
                    m.DisplayField = LubricationSparePartApply.ApplyNoViewProperty.Name;
                    m.BindDisplayField = LubricationSparePartApply.ApplyNoViewProperty.Name;
                }).HasLabel("备件申请单").Readonly().ShowInList(150);
                View.Property(p => p.AppStateView).Show(ShowInWhere.All).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.UnitView).Show(ShowInWhere.All).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.IsApply).Show(ShowInWhere.All).Readonly().Show(ShowInWhere.All);
                View.Property(p => p.Remark).Show(ShowInWhere.All).Readonly(p => p.IsApply).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}