using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Equipments.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 历史订单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("历史订单查询实体")]
    public class HistoryOrderViewModelCriteria : Criteria
    {
        #region 采购对象 PurchaseObjectType
        /// <summary>
        /// 采购对象
        /// </summary>
        [Label("采购对象")]
        public static readonly Property<PurchaseObjectType> PurchaseObjectTypeProperty = P<HistoryOrderViewModelCriteria>.Register(e => e.PurchaseObjectType);

        /// <summary>
        /// 采购对象
        /// </summary>
        public PurchaseObjectType PurchaseObjectType
        {
            get { return this.GetProperty(PurchaseObjectTypeProperty); }
            set { this.SetProperty(PurchaseObjectTypeProperty, value); }
        }
        #endregion

        #region 采购对象编码 ObjectCodeInfo
        /// <summary>
        /// 采购对象编码
        /// </summary>
        [Label("采购对象编码")]
        public static readonly Property<string> ObjectCodeInfoProperty = P<HistoryOrderViewModelCriteria>.Register(e => e.ObjectCodeInfo);

        /// <summary>
        /// 采购对象编码
        /// </summary>
        public string ObjectCodeInfo
        {
            get { return this.GetProperty(ObjectCodeInfoProperty); }
            set { this.SetProperty(ObjectCodeInfoProperty, value); }
        }
        #endregion

        #region 采购对象描述 ObjectName
        /// <summary>
        /// 采购对象描述
        /// </summary>
        [Label("采购对象描述")]
        public static readonly Property<string> ObjectNameProperty = P<HistoryOrderViewModelCriteria>.Register(e => e.ObjectName);

        /// <summary>
        /// 采购对象描述
        /// </summary>
        public string ObjectName
        {
            get { return this.GetProperty(ObjectNameProperty); }
            set { this.SetProperty(ObjectNameProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<HistoryOrderViewModelCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefNullableId(SupplierIdProperty); }
            set { SetRefNullableId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<HistoryOrderViewModelCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 供应商名称 SupplierName
        /// <summary>
        /// 供应商名称
        /// </summary>
        [Label("供应商描述")]
        public static readonly Property<string> SupplierNameProperty = P<HistoryOrderViewModelCriteria>.Register(e => e.SupplierName);

        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName
        {
            get { return this.GetProperty(SupplierNameProperty); }
            set { this.SetProperty(SupplierNameProperty, value); }
        }
        #endregion

        #region 采购日期 CreateDate
        /// <summary>
        /// 采购日期
        /// </summary>
        [Label("采购日期")]
        public static readonly Property<DateRange> CreateDateProperty = P<HistoryOrderViewModelCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 采购日期
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<PurchaseOrderController>().CriteriaHistoryOrders(this);
        }
    }
}
