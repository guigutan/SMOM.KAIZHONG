using SIE.Warehouses;
using SIE.Web.Common;

namespace SIE.Web.Warehouses
{
    /// <summary>
    /// 库位 仓储资料 视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class StorageLocationLayinInfoViewConfig : WebViewConfig<StorageLocationLayinInfo>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(StorageLocationLayinInfo.IdProperty);
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.Warehouses.ViewBehaviors.StorageLocationLayinInfoBehavior");
            View.HasDetailColumnsCount(9);
            using (View.OrderProperties())
            {
                View.Property(p => p.TemperatureType).DefaultValue((int)TemperatureType.Custom).ShowInDetail(columnSpan: 1)
                    .UseEnumEditor(p => p.XType = "TemperatureTypeComboList");
                View.Property(p => p.TemperatureLower).ShowInDetail(columnSpan: 1, hideLabel: true).HasLabel(string.Empty)
                    .UseSpinEditor(p => p.XType = "TemperatureNumberfield");
                View.Property(p => p.TemperatureUpper).ShowInDetail(columnSpan: 1, hideLabel: true).HasLabel(string.Empty)
                    .UseSpinEditor(p => p.XType = "TemperatureNumberfield");
                View.Property(p => p.HumidityType).DefaultValue((int)HumidityType.Custom).ShowInDetail(columnSpan: 1)
                    .UseEnumEditor(p => p.XType = "HumidityTypeComboList");
                View.Property(p => p.HumidityLower).ShowInDetail(columnSpan: 1, hideLabel: true).HasLabel(string.Empty)
                    .UseSpinEditor(p => p.XType = "HumidityNumberfield");
                View.Property(p => p.HumidityUpper).ShowInDetail(columnSpan: 1, hideLabel: true).HasLabel(string.Empty)
                    .UseSpinEditor(p => p.XType = "HumidityNumberfield");
                View.Property(p => p.RoHsGradeValue)
                    .UseCatalogEditor(p => { p.CatalogReloadData = true; p.CatalogType = StorageLocationLayinInfo.ROHSLEVEL; })
                    .UseListSetting(e => { e.HelpInfo = "RoHS等级快码类型(ROHS_LEVEL)"; }).ShowInDetail(columnSpan: 3);
                View.Property(p => p.WeightLimit).ShowInDetail(columnSpan: 3);
                View.Property(p => p.VolumeLimit).ShowInDetail(columnSpan: 3);
                View.Property(p => p.BoxCountLimit).ShowInDetail(columnSpan: 3);
                View.Property(p => p.TrayCountLimit).ShowInDetail(columnSpan: 3);
                View.Property(p => p.AmountLimit).ShowInDetail(columnSpan: 3);
                View.Property(p => p.IsSpecialItem).ShowInDetail(columnSpan: 3);
                View.Property(p => p.IsElecSenGrade).ShowInDetail(columnSpan: 3);
                View.Property(p => p.IsHumSenGrade).ShowInDetail(columnSpan: 3);
                View.Property(p => p.IsBanMixedBatch09).ShowInDetail(columnSpan: 3);
                View.Property(p => p.IsBanMixed).ShowInDetail(columnSpan: 3);
                View.Property(p => p.IsSingleSku).ShowInDetail(columnSpan: 3);
                View.Property(p => p.IsBanMixedBatch10).ShowInDetail(columnSpan: 3);
            }
        }
    }
}
