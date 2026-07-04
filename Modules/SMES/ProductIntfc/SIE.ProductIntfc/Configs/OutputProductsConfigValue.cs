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
    public class OutputProductsConfigValue : ConfigValue
    {
        #region 入库单号 NumberRule
        /// <summary>
        /// 入库单号Id
        /// </summary>
        [Label("入库单号")]
        public static readonly IRefIdProperty NumberRuleIdProperty =
            P<OutputProductsConfigValue>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

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
            P<OutputProductsConfigValue>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 入库单号
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 副产品默认仓库 ByWarehouse
        /// <summary>
        /// 副产品默认仓库Id
        /// </summary>
        [Label("副产品默认仓库")]
        public static readonly IRefIdProperty ByWarehouseIdProperty =
            P<OutputProductsConfigValue>.RegisterRefId(e => e.ByWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 副产品默认仓库Id
        /// </summary>
        public double ByWarehouseId
        {
            get { return (double)this.GetRefId(ByWarehouseIdProperty); }
            set { this.SetRefId(ByWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 副产品默认仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> ByWarehouseProperty =
            P<OutputProductsConfigValue>.RegisterRef(e => e.ByWarehouse, ByWarehouseIdProperty);

        /// <summary>
        /// 副产品默认仓库
        /// </summary>
        public Warehouse ByWarehouse
        {
            get { return this.GetRefEntity(ByWarehouseProperty); }
            set { this.SetRefEntity(ByWarehouseProperty, value); }
        }
        #endregion

        #region 联产品默认仓库 JointWarehouse
        /// <summary>
        /// 默认仓库Id
        /// </summary> 
        [Label("联产品默认仓库")]
        public static readonly IRefIdProperty JointWarehouseIdProperty =
            P<OutputProductsConfigValue>.RegisterRefId(e => e.JointWarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 默认仓库Id
        /// </summary>
        public double JointWarehouseId
        {
            get { return (double)this.GetRefId(JointWarehouseIdProperty); }
            set { this.SetRefId(JointWarehouseIdProperty, value); }
        }

        /// <summary>
        /// 默认仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> JointWarehouseProperty =
            P<OutputProductsConfigValue>.RegisterRef(e => e.JointWarehouse, JointWarehouseIdProperty);

        /// <summary>
        /// 默认仓库
        /// </summary>
        public Warehouse JointWarehouse
        {
            get { return this.GetRefEntity(JointWarehouseProperty); }
            set { this.SetRefEntity(JointWarehouseProperty, value); }
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