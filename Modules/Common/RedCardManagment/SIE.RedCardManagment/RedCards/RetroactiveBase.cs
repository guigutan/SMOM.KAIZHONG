using SIE;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;

namespace SIE.RedCardManagment.RedCards
{

    /// <summary>
    /// 追溯清单基类
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("追溯清单基类")]
    public class RetroactiveBase : DataEntity
    {
        #region WmsKey WmsKey
        /// <summary>
        /// WmsKey（避免重复算）
        /// </summary>
        [Label("WmsKey")]
        public static readonly Property<string> WmsKeyProperty = P<RetroactiveBase>.Register(e => e.WmsKey);

        /// <summary>
        /// WmsKey
        /// </summary>
        public string WmsKey
        {
            get { return GetProperty(WmsKeyProperty); }
            set { SetProperty(WmsKeyProperty, value); }
        }
        #endregion

        #region 数量 Quannity
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QuannityProperty = P<RetroactiveBase>.Register(e => e.Quannity);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quannity
        {
            get { return GetProperty(QuannityProperty); }
            set { SetProperty(QuannityProperty, value); }
        }
        #endregion

        #region 执行时间 ApplyTime
        /// <summary>
        /// 执行时间
        /// </summary>
        [Label("执行时间")]
        public static readonly Property<DateTime?> ApplyTimeProperty = P<RetroactiveBase>.Register(e => e.ApplyTime);

        /// <summary>
        /// 执行时间
        /// </summary>
        public DateTime? ApplyTime
        {
            get { return GetProperty(ApplyTimeProperty); }
            set { SetProperty(ApplyTimeProperty, value); }
        }
        #endregion

        #region 生产日期 ProductDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime?> ProductDateProperty = P<RetroactiveBase>.Register(e => e.ProductDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime? ProductDate
        {
            get { return GetProperty(ProductDateProperty); }
            set { SetProperty(ProductDateProperty, value); }
        }
        #endregion

        #region 状态 Status
        /// <summary>
        /// 状态
        /// </summary>
        [Label("物料红牌状态")]
        public static readonly Property<RedCardState> StatusProperty = P<RetroactiveBase>.Register(e => e.Status);

        /// <summary>
        /// 状态
        /// </summary>
        public RedCardState Status
        {
            get { return GetProperty(StatusProperty); }
            set { SetProperty(StatusProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<RetroactiveBase>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        [Required]
        public double? ItemId
        {
            get { return (double?)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<RetroactiveBase>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 物料批次 ItemBatch
        /// <summary>
        /// 物料批次
        /// </summary>
        [Label("物料批次")]
        public static readonly Property<string> ItemBatchProperty = P<RetroactiveBase>.Register(e => e.ItemBatch);

        /// <summary>
        /// 物料批次
        /// </summary>
        public string ItemBatch
        {
            get { return GetProperty(ItemBatchProperty); }
            set { SetProperty(ItemBatchProperty, value); }
        }
        #endregion

        #region 生产批次 Batch
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> BatchProperty = P<RetroactiveBase>.Register(e => e.Batch);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string Batch
        {
            get { return GetProperty(BatchProperty); }
            set { SetProperty(BatchProperty, value); }
        }
        #endregion

        #region 供应商 Supplier
        /// <summary>
        /// 供应商Id
        /// </summary>
        public static readonly IRefIdProperty SupplierIdProperty = P<RetroactiveBase>.RegisterRefId(e => e.SupplierId, ReferenceType.Normal);

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId
        {
            get { return (double?)GetRefId(SupplierIdProperty); }
            set { SetRefId(SupplierIdProperty, value); }
        }

        /// <summary>
        /// 供应商
        /// </summary>
        public static readonly RefEntityProperty<Supplier> SupplierProperty = P<RetroactiveBase>.RegisterRef(e => e.Supplier, SupplierIdProperty);

        /// <summary>
        /// 供应商
        /// </summary>
        public Supplier Supplier
        {
            get { return GetRefEntity(SupplierProperty); }
            set { SetRefEntity(SupplierProperty, value); }
        }
        #endregion

        #region 执行人 Applicant
        /// <summary>
        /// 执行人Id
        /// </summary>
        public static readonly IRefIdProperty ApplicantIdProperty = P<RetroactiveBase>.RegisterRefId(e => e.ApplicantId, ReferenceType.Normal);

        /// <summary>
        /// 执行人Id
        /// </summary>
        public double? ApplicantId
        {
            get { return (double?)GetRefNullableId(ApplicantIdProperty); }
            set { SetRefNullableId(ApplicantIdProperty, value); }
        }

        /// <summary>
        /// 执行人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ApplicantProperty = P<RetroactiveBase>.RegisterRef(e => e.Applicant, ApplicantIdProperty);

        /// <summary>
        /// 执行人
        /// </summary>
        public Employee Applicant
        {
            get { return GetRefEntity(ApplicantProperty); }
            set { SetRefEntity(ApplicantProperty, value); }
        }
        #endregion

        #region 红牌 RedCard
        /// <summary>
        /// 红牌Id
        /// </summary>
        public static readonly IRefIdProperty RedCardIdProperty = P<RetroactiveBase>.RegisterRefId(e => e.RedCardId, ReferenceType.Normal);

        /// <summary>
        /// 红牌Id
        /// </summary>
        public double RedCardId
        {
            get { return (double)GetRefId(RedCardIdProperty); }
            set { SetRefId(RedCardIdProperty, value); }
        }

        /// <summary>
        /// 红牌
        /// </summary>
        public static readonly RefEntityProperty<RedCard> RedCardProperty = P<RetroactiveBase>.RegisterRef(e => e.RedCard, RedCardIdProperty);

        /// <summary>
        /// 红牌
        /// </summary>
        public RedCard RedCard
        {
            get { return GetRefEntity(RedCardProperty); }
            set { SetRefEntity(RedCardProperty, value); }
        }
        #endregion

        #region 执行人名称 ApplicantName
        /// <summary>
        /// 执行人名称
        /// </summary>
        [Label("执行人")]
        public static readonly Property<string> ApplicantNameProperty = P<RetroactiveBase>.RegisterView(e => e.ApplicantName, p => p.Applicant.Name);

        /// <summary>
        /// 执行人名称
        /// </summary>
        public string ApplicantName
        {
            get { return this.GetProperty(ApplicantNameProperty); }
        }
        #endregion
    }
}
