using NPOI.SS.Formula.Functions;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库地址
    /// </summary>
    [ChildEntity, Serializable]
    [Label("WMS库位信息")]
    public class ErpWarehouseDetail : DataEntity
    {
        #region 仓库 Warehouse
        /// <summary>
        /// 仓库Id
        /// </summary>
        [Label("仓库")]
        public static readonly IRefIdProperty WarehouseIdProperty =
            P<ErpWarehouseDetail>.RegisterRefId(e => e.WarehouseId, ReferenceType.Normal);

        /// <summary>
        /// 仓库Id
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
            P<ErpWarehouseDetail>.RegisterRef(e => e.Warehouse, WarehouseIdProperty);

        /// <summary>
        /// 仓库
        /// </summary>
        public Warehouse Warehouse
        {
            get { return this.GetRefEntity(WarehouseProperty); }
            set { this.SetRefEntity(WarehouseProperty, value); }
        }
        #endregion

        #region 库区 Area
        /// <summary>
        /// 库区Id
        /// </summary>
        [Label("库区")]
        public static readonly IRefIdProperty AreaIdProperty =
            P<ErpWarehouseDetail>.RegisterRefId(e => e.AreaId, ReferenceType.Normal);

        /// <summary>
        /// 库区Id
        /// </summary>
        public double? AreaId
        {
            get { return (double?)this.GetRefNullableId(AreaIdProperty); }
            set { this.SetRefNullableId(AreaIdProperty, value); }
        }

        /// <summary>
        /// 库区
        /// </summary>
        public static readonly RefEntityProperty<StorageArea> AreaProperty =
            P<ErpWarehouseDetail>.RegisterRef(e => e.Area, AreaIdProperty);

        /// <summary>
        /// 库区
        /// </summary>
        public StorageArea Area
        {
            get { return this.GetRefEntity(AreaProperty); }
            set { this.SetRefEntity(AreaProperty, value); }
        }
        #endregion

        #region 库位 StorageLocation
        /// <summary>
        /// 库位Id
        /// </summary>
        [Label("库位")]
        public static readonly IRefIdProperty StorageLocationIdProperty =
            P<ErpWarehouseDetail>.RegisterRefId(e => e.StorageLocationId, ReferenceType.Normal);

        /// <summary>
        /// 库位Id
        /// </summary>
        public double? StorageLocationId
        {
            get { return (double?)this.GetRefNullableId(StorageLocationIdProperty); }
            set { this.SetRefNullableId(StorageLocationIdProperty, value); }
        }

        /// <summary>
        /// 库位
        /// </summary>
        public static readonly RefEntityProperty<StorageLocation> StorageLocationProperty =
            P<ErpWarehouseDetail>.RegisterRef(e => e.StorageLocation, StorageLocationIdProperty);

        /// <summary>
        /// 库位
        /// </summary>
        public StorageLocation StorageLocation
        {
            get { return this.GetRefEntity(StorageLocationProperty); }
            set { this.SetRefEntity(StorageLocationProperty, value); }
        }
        #endregion

        #region ERP子库 ErpWarehouse
        /// <summary>
        /// ERP子库Id
        /// </summary>
        [Label("ERP子库")]
        public static readonly IRefIdProperty ErpWarehouseIdProperty =
            P<ErpWarehouseDetail>.RegisterRefId(e => e.ErpWarehouseId, ReferenceType.Parent);

        /// <summary>
        /// ERP子库Id
        /// </summary>
        public double ErpWarehouseId
        {
            get { return (double)this.GetRefId(ErpWarehouseIdProperty); }
            set { this.SetRefId(ErpWarehouseIdProperty, value); }
        }

        /// <summary>
        /// ERP子库
        /// </summary>
        public static readonly RefEntityProperty<ErpWarehouse> ErpWarehouseProperty =
            P<ErpWarehouseDetail>.RegisterRef(e => e.ErpWarehouse, ErpWarehouseIdProperty);

        /// <summary>
        /// ERP子库
        /// </summary>
        public ErpWarehouse ErpWarehouse
        {
            get { return this.GetRefEntity(ErpWarehouseProperty); }
            set { this.SetRefEntity(ErpWarehouseProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 子库编码 ErpWarehouseCode
        /// <summary>
        /// 子库编码
        /// </summary>
        [Label("子库编码")]
        public static readonly Property<string> ErpWarehouseCodeProperty = P<ErpWarehouseDetail>.RegisterView(e => e.ErpWarehouseCode, p => p.ErpWarehouse.Code);

        /// <summary>
        /// 子库编码
        /// </summary>
        public string ErpWarehouseCode
        {
            get { return this.GetProperty(ErpWarehouseCodeProperty); }
        }
        #endregion

        #region 子库名称 ErpWarehouseName
        /// <summary>
        /// 子库名称
        /// </summary>
        [Label("子库名称")]
        public static readonly Property<string> ErpWarehouseNameProperty = P<ErpWarehouseDetail>.RegisterView(e => e.ErpWarehouseName, p => p.ErpWarehouse.Name);

        /// <summary>
        /// 子库名称
        /// </summary>
        public string ErpWarehouseName
        {
            get { return this.GetProperty(ErpWarehouseNameProperty); }
        }
        #endregion

        #region ERP库存组织 ErpInvOrgName
        /// <summary>
        /// ERP库存组织
        /// </summary>
        [Label("ERP库存组织")]
        public static readonly Property<string> ErpInvOrgNameProperty = P<ErpWarehouseDetail>.RegisterView(e => e.ErpInvOrgName, p => p.ErpWarehouse.ErpOrgName);

        /// <summary>
        /// ERP库存组织
        /// </summary>
        public string ErpInvOrgName
        {
            get { return this.GetProperty(ErpInvOrgNameProperty); }
        }
        #endregion

        #region 仓库编码 WarehouseCode
        /// <summary>
        /// 仓库编码
        /// </summary>
        [Label("仓库编码")]
        public static readonly Property<string> WarehouseCodeProperty = P<ErpWarehouseDetail>.RegisterView(e => e.WarehouseCode, p => p.Warehouse.Code);

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode
        {
            get { return this.GetProperty(WarehouseCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 仓库 实体配置
    /// </summary>
    internal class ErpWarehouseDetailConfig : EntityConfig<ErpWarehouseDetail>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ERP_WH_DTL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.DisableInvOrg();
        }
    }
}
