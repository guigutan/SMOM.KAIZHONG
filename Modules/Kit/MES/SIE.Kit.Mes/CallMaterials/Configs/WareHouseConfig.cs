using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Kit.MES.CallMaterials.Configs
{
    /// <summary>
    /// 端口类型配置
    /// </summary>
    [System.ComponentModel.DisplayName("设置发料仓库")]
    [System.ComponentModel.Description("根据不同资源，设置对应发料仓库")]
    public class WareHouseConfig : ModuleCategoryConfig<ResourceWarehouse, WareHouseConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly WareHouseConfigValue defaultValue = new WareHouseConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override WareHouseConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 端口类型值配置
    /// </summary>
    [RootEntity, Serializable]
    [Label("发料仓库")]
    public class WareHouseConfigValue : ConfigValue
    {
        #region 发料仓库 Warehouse
        /// <summary>
        /// 默认仓库Id
        /// </summary> 
        [Label("发料仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<WareHouseConfigValue>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 发料仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 发料仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<WareHouseConfigValue>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 发料仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示所有属性的名称
        /// </summary>
        /// <returns>返回所有属性的名称</returns>
        public override string Display()
        {
            return "发料仓库：{0}".L10nFormat(Warehouse?.Name);
        }
    }
}
