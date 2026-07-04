using SIE.EMS.MeteringEquipment.Calibrations;
using SIE.EMS.MeteringEquipment.Calibrations.ViewModels;
using SIE.Web.Common;

namespace SIE.Web.EMS.MeteringEquipment.Calibrations.ViewModels
{
    /// <summary>
    /// 选择设备清单视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class SelCalibrationEquipmentModelViewConfig : WebViewConfig<SelCalibrationEquipmentModel>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.Name).Show(ShowInWhere.All).Readonly();

            View.Property(p => p.EquipModelName).HasLabel("设备型号").Show(ShowInWhere.All).Readonly();
            View.Property(p => p.EquipTypeName).HasLabel("设备类型").Show(ShowInWhere.All).Readonly();

            View.Property(p => p.Specifications).HasLabel("规格型号").Show(ShowInWhere.All).Readonly();
            View.Property(p => p.UseDepartmentName).HasLabel("使用部门").Show(ShowInWhere.All).Readonly();

            View.Property(p => p.IsDowngrade).UseCheckDropDownEditor().Show(ShowInWhere.All).Readonly();
            View.Property(p => p.PrecisionClass).UseCatalogEditor(e => { e.CatalogType = CalibrationEquipment.PrecisionClassType; e.CatalogReloadData = true; }).UseListSetting(e => { e.HelpInfo = "精度级别类型(PRECISION_CLASS_TYPE)"; }).Show(ShowInWhere.All).Readonly();

            View.Property(p => p.Manufacturer).Show(ShowInWhere.All).Readonly();
            View.Property(p => p.CardDate).UseDateEditor().Show(ShowInWhere.All).Readonly();
            View.Property(p => p.UseState).Show(ShowInWhere.All).Readonly();
            
        }
    }
}
