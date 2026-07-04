using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.RedCardManagment.RedCardApplyBills.Service;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.RedCardManagment.RedCardApplyBills
{
    /// <summary>
    ///红牌申请单查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("红牌申请单查询实体")]
    public class RedCardApplyBillCriteria : Criteria
    {
        #region 申请单号 No
        /// <summary>
        /// 申请单号
        /// </summary>
        [Label("申请单号")]
        public static readonly Property<string> NoProperty = P<RedCardApplyBillCriteria>.Register(e => e.No);

        /// <summary>
        /// 申请单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<RedCardApplyBillCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<RedCardApplyBillCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        [Label("供应商")]
        public static readonly IRefIdProperty SupplierIdProperty = P<RedCardApplyBillCriteria>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<RedCardApplyBillCriteria>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 申请类型 ApplyType
        /// <summary>
        /// 申请类型
        /// </summary>
        [Label("申请类型")]
        public static readonly Property<ApplyType?> ApplyTypeProperty = P<RedCardApplyBillCriteria>.Register(e => e.ApplyType);

        /// <summary>
        /// 申请类型
        /// </summary>
        public ApplyType? ApplyType
        {
            get { return GetProperty(ApplyTypeProperty); }
            set { SetProperty(ApplyTypeProperty, value); }
        }
        #endregion

        #region   状态 BillStatus
        /// <summary>
        /// 
        /// </summary>
        [Label("状态")]
        public static readonly Property<BillStatus?> BillStatusProperty = P<RedCardApplyBillCriteria>.Register(e => e.BillStatus);

        /// <summary>
        /// 
        /// </summary>
        public BillStatus? BillStatus
        {
            get { return GetProperty(BillStatusProperty); }
            set { SetProperty(BillStatusProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<RedCardApplyBillCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion        

        /// <summary>
        /// 重写查询方法
        /// </summary>
        /// <returns>红牌申请单列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<RedCardApplyBillService>().GetApplyBills(this);
        }
    }
}
