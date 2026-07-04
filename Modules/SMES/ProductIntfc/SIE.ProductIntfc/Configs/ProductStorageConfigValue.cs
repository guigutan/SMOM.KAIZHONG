using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.ProductIntfc.Configs
{
    /// <summary>
    /// 成品入库配置值
    /// </summary>
    [RootEntity, Serializable]
    public class ProductStorageConfigValue : ConfigValue
    {
        #region 入库单号 NumberRule
        /// <summary>
        /// 入库单号Id
        /// </summary>
        [Label("入库单号")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<ProductStorageConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 入库单号Id
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 入库单号
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty =
            P<ProductStorageConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 入库单号
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 默认仓库 Warehouse
        /// <summary>
        /// 默认仓库Id
        /// </summary> 
        [Label("入库仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<ProductStorageConfigValue>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 默认仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 默认仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<ProductStorageConfigValue>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 默认仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把配置值显示出来
        /// </summary>
        /// <returns>配置值</returns>
        public override string Display()
        {
            return base.Display();
        }
    }
}