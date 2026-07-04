using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库资料
    /// </summary>
    [RootEntity, Serializable]
    [Label("仓库资料")]
    [DisplayMember(nameof(WarehouseInfo.Id))]
    public partial class WarehouseInfo : DataEntity
    {
        #region 面积 Area
        /// <summary>
        /// 面积
        /// </summary>
        [MinValue(0)]
        [Label("仓库面积(㎡)")]
        public static readonly Property<decimal?> AreaProperty = P<WarehouseInfo>.Register(e => e.Area);

        /// <summary>
        /// 面积
        /// </summary>
        public decimal? Area
        {
            get { return GetProperty(AreaProperty); }
            set { SetProperty(AreaProperty, value); }
        }
        #endregion

        #region 容积 Volume
        /// <summary>
        /// 容积
        /// </summary>
        [MinValue(0)]
        [Label("仓库容积(CBM)")]
        public static readonly Property<decimal?> VolumeProperty = P<WarehouseInfo>.Register(e => e.Volume);

        /// <summary>
        /// 容积
        /// </summary>
        public decimal? Volume
        {
            get { return GetProperty(VolumeProperty); }
            set { SetProperty(VolumeProperty, value); }
        }
        #endregion

        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty = P<WarehouseInfo>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId
        {
            get { return (double)GetRefId(WarehouseIdProperty); }
            set { SetRefId(WarehouseIdProperty, value); }
        }

        /// <summary>
        /// 仓库
        /// </summary>
        public static readonly RefEntityProperty<Warehouse> WarehouseProperty = P<WarehouseInfo>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return GetRefEntity(WarehouseProperty); }
            set { SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 仓库形式 WarehouseForm
        /// <summary>
        /// 仓库形式
        /// </summary>
        [Label("仓库形式")]
        public static readonly Property<WarehouseForm?> WarehouseFormProperty = P<WarehouseInfo>.Register(e => e.WarehouseForm);

        /// <summary>
        /// 仓库形式
        /// </summary>
        public WarehouseForm? WarehouseForm
        {
            get { return GetProperty(WarehouseFormProperty); }
            set { SetProperty(WarehouseFormProperty, value); }
        }
        #endregion       
    }

    /// <summary>
    /// 仓库资料 实体配置
    /// </summary>
    internal class WarehouseInfoConfig : EntityConfig<WarehouseInfo>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WH_INFO").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}