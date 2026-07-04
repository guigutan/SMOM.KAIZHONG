using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Warehouses;

namespace SIE.EMS.Warehouses
{
    /// <summary>
    /// 仓库扩展属性
    /// </summary>
    [Label("仓库扩展属性")]
    [CompiledPropertyDeclarer]
    public static class WarehouseExtension
    {
        #region bool IsZeroCost (不记成本)
        /// <summary>
        /// 不记成本 扩展属性。
        /// </summary>
        public static readonly Property<bool?> IsZeroCostProperty =
            P<Warehouse>.RegisterExtension<bool?>("IsZeroCost", typeof(WarehouseExtension));

        /// <summary>
        /// 获取 不记成本 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static bool GetIsZeroCost(this Warehouse me)
        {
            return me.GetProperty(IsZeroCostProperty) == true;
        }

        /// <summary>
        /// 设置 不记成本 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetIsZeroCost(this Warehouse me, bool value)
        {
            me.SetProperty(IsZeroCostProperty, value);
        }
        #endregion

        #region double? ScrapLocationId (报废后库位Id)
        /// <summary>
        /// 报废后库位Id 扩展属性。（EDO 资产报废功能， 选了仓库的联动)
        /// </summary>
        public static readonly Property<double?> ScrapLocationIdProperty =
            P<Warehouse>.RegisterExtension<double?>("ScrapLocationId", typeof(WarehouseExtension));

        /// <summary>
        /// 获取 报废后库位Id 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static double? GetScrapLocationId(this Warehouse me)
        {
            return me.GetProperty(ScrapLocationIdProperty);
        }

        /// <summary>
        /// 设置 报废后库位Id 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetScrapLocationId(this Warehouse me, double? value)
        {
            me.SetProperty(ScrapLocationIdProperty, value);
        }
        #endregion

        #region string ScrapLocationCode (报废后库位编码)
        /// <summary>
        /// 报废后库位编码 扩展属性。（EDO 资产报废功能，选了仓库的联动)
        /// </summary>
        public static readonly Property<string> ScrapLocationCodeProperty =
            P<Warehouse>.RegisterExtension<string>("ScrapLocationCode", typeof(WarehouseExtension));

        /// <summary>
        /// 获取 报废后库位编码 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static string GetScrapLocationCode(this Warehouse me)
        {
            return me.GetProperty(ScrapLocationCodeProperty);
        }

        /// <summary>
        /// 设置 报废后库位编码 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetScrapLocationCode(this Warehouse me, string value)
        {
            me.SetProperty(ScrapLocationCodeProperty, value);
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    internal class WarehouseExtensionConfig : EntityConfig<Warehouse>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.Property(WarehouseExtension.ScrapLocationIdProperty).DontMapColumn();
            Meta.Property(WarehouseExtension.ScrapLocationCodeProperty).DontMapColumn();
        }
    }
}
