using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WIP.Configs;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.MES.WIP.Packings.Configs
{
    /// <summary>
    /// 包装生成单据配置
    /// </summary>
    [System.ComponentModel.DisplayName("包装生成单据配置")]
    [System.ComponentModel.Description("包装生成单据配置，仓库")]
    public class WipPackingBillConfig : ModuleCategoryConfig<ResourceStation, WipPackingBillConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        WipPackingBillConfigValue defaultValue;

        /// <summary>
        /// 默认值
        /// </summary>
        public override WipPackingBillConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 包装生成单据配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("包装便捷性配置")]
    [DisplayMember(nameof(Id))]
    public class WipPackingBillConfigValue : ConfigValue
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库ID
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<WipPackingBillConfigValue>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// ID
        /// </summary>
        public double WarehouseId
        {
            get { return (double)this.GetRefId(WarehouseIdProperty); }
            set { this.SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty =
            P<WipPackingBillConfigValue>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>仓库名称</returns>
        public override string Display()
        {
            return "入库仓:[{0}]".L10nFormat(Warehouse?.Name);
        }
    }
}