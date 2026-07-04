using SIE.Warehouses;
using SIE.Wpf.Common;
using SIE.Wpf.Warehouses.ViewBehaviors;
using System.Collections.Generic;

namespace SIE.Wpf.Warehouses
{
    /// <summary>
    /// 库位 仓储资料 视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class StorageLocationLayinInfoViewConfig : WPFViewConfig<StorageLocationLayinInfo>
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
            View.HasDetailColumnsCount(3);
            View.AddBehavior(typeof(SpecialItemBehavior));
            View.AddBehavior(typeof(TemperateAndHumidityBehavior));
            using (View.OrderProperties())
            {
                View.Property(p => p.TemperatureType).UseStorageTemperatureEditor(p =>
                {
                    p.BindFieldList = new List<string>() { StorageLocationLayinInfo.TemperatureTypeProperty.Name, StorageLocationLayinInfo.TemperatureLowerProperty.Name, "至", StorageLocationLayinInfo.TemperatureUpperProperty.Name };
                    p.ColumnRateList = new List<int>() { 3, 3, 1, 3 };
                });
                View.Property(p => p.HumidityType).UseStorageHumidityEditor(p =>
                {
                    p.BindFieldList = new List<string>() { StorageLocationLayinInfo.HumidityTypeProperty.Name, StorageLocationLayinInfo.HumidityLowerProperty.Name, "至", StorageLocationLayinInfo.HumidityUpperProperty.Name };
                    p.ColumnRateList = new List<int>() { 3, 3, 1, 3 };
                });
                View.Property(p => p.RoHsGradeValue).UseCatalogEditor(p => p.CatalogType = StorageLocationLayinInfo.ROHSLEVEL);
                View.Property(p => p.WeightLimit);
                View.Property(p => p.VolumeLimit);
                View.Property(p => p.BoxCountLimit);
                View.Property(p => p.TrayCountLimit);
                View.Property(p => p.AmountLimit);
                View.Property(p => p.IsSpecialItem);
                View.Property(p => p.IsElecSenGrade);
                View.Property(p => p.IsHumSenGrade);
                View.Property(p => p.IsBanMixedBatch09);
                View.Property(p => p.IsBanMixed);
                View.Property(p => p.IsSingleSku);
                View.Property(p => p.IsBanMixedBatch10);
            }
        }
    }
}
