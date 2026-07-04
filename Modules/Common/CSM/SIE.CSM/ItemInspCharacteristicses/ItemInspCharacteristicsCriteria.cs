using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using System;

namespace SIE.CSM.ItemInspCharacteristicses
{
    /// <summary>
    /// 物料检验特性查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("物料检验特性查询实体")]
    public partial class ItemInspCharacteristicsCriteria : Criteria
    {
        #region 物料编码 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料编码")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemInspCharacteristicsCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemInspCharacteristicsCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料编码
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<ItemInspCharacteristicsCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 供应商编码 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商编码")]
        public static readonly IRefIdProperty SupplierIdProperty = P<ItemInspCharacteristicsCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商编码
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<ItemInspCharacteristicsCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商编码
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 供应商名称 ItemName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商名称")]
        public static readonly Property<string> SupplierNameProperty = P<ItemInspCharacteristicsCriteria>.Register(e => e.SupplierName);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return GetProperty(SupplierNameProperty); }
            set { SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 状态 SupplierState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State?> SupplierStateProperty = P<ItemInspCharacteristicsCriteria>.Register(e => e.SupplierState);

        /// <summary>
        /// 状态
        /// </summary>
        public State? SupplierState
        {
            get { return GetProperty(SupplierStateProperty); }
            set { SetProperty(SupplierStateProperty, value); }
        }
        #endregion

        #region 查询
        /// <summary>
        /// 查询物料检验特性信息
        /// </summary>
        /// <returns>物料检验特性信息列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ItemInspCharacteristicsController>().QueryItemInspCharacteristis(this);
        }
        #endregion
    }
}