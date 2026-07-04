using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.MES.ReworkLayoutVersions.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ReworkLayoutVersions
{
    /// <summary>
    /// 返工信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("返工信息")]
    [EntityWithConfig(typeof(ReworkInfoRecordEntityConfig))]
    [ConditionQueryType(typeof(ReworkInfoRecordCriteria))]
    public class ReworkInfoRecord : DataEntity
    {
        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<ReworkInfoRecordState> StateProperty = P<ReworkInfoRecord>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public ReworkInfoRecordState State
        {
            get { return this.GetProperty(StateProperty); }
            set { this.SetProperty(StateProperty, value); }
        }
        #endregion

        #region 工厂 Factory
        /// <summary>
        /// 工厂
        /// </summary>
        [Label("工厂")]
        public static readonly Property<string> FactoryProperty = P<ReworkInfoRecord>.Register(e => e.Factory);

        /// <summary>
        /// 工厂
        /// </summary>
        public string Factory
        {
            get { return this.GetProperty(FactoryProperty); }
            set { this.SetProperty(FactoryProperty, value); }
        }
        #endregion

        #region 产品 Item
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ItemIdProperty =
            P<ReworkInfoRecord>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty =
            P<ReworkInfoRecord>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 订单数量 Qty
        /// <summary>
        /// 订单数量
        /// </summary>
        [Label("订单数量")]
        public static readonly Property<decimal> QtyProperty = P<ReworkInfoRecord>.Register(e => e.Qty);

        /// <summary>
        /// 订单数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 基本开始日期 BeginDateTime
        /// <summary>
        /// 基本开始日期
        /// </summary>
        [Label("基本开始日期")]
        public static readonly Property<DateTime?> BeginDateTimeProperty = P<ReworkInfoRecord>.Register(e => e.BeginDateTime);

        /// <summary>
        /// 基本开始日期
        /// </summary>
        public DateTime? BeginDateTime
        {
            get { return this.GetProperty(BeginDateTimeProperty); }
            set { this.SetProperty(BeginDateTimeProperty, value); }
        }
        #endregion

        #region 基本完成日期 EndDateTime
        /// <summary>
        /// 基本完成日期
        /// </summary>
        [Label("基本完成日期")]
        public static readonly Property<DateTime?> EndDateTimeProperty = P<ReworkInfoRecord>.Register(e => e.EndDateTime);

        /// <summary>
        /// 基本完成日期
        /// </summary>
        public DateTime? EndDateTime
        {
            get { return this.GetProperty(EndDateTimeProperty); }
            set { this.SetProperty(EndDateTimeProperty, value); }
        }
        #endregion

        #region 返工工艺路线版本 ReworkLayoutVersion
        /// <summary>
        /// 返工工艺路线版本Id
        /// </summary>
        [Label("返工工艺路线版本")]
        public static readonly IRefIdProperty ReworkLayoutVersionIdProperty =
            P<ReworkInfoRecord>.RegisterRefId(e => e.ReworkLayoutVersionId, ReferenceType.Normal);

        /// <summary>
        /// 返工工艺路线版本Id
        /// </summary>
        public double ReworkLayoutVersionId
        {
            get { return (double)this.GetRefId(ReworkLayoutVersionIdProperty); }
            set { this.SetRefId(ReworkLayoutVersionIdProperty, value); }
        }

        /// <summary>
        /// 返工工艺路线版本
        /// </summary>
        public static readonly RefEntityProperty<ReworkLayoutVersion> ReworkLayoutVersionProperty =
            P<ReworkInfoRecord>.RegisterRef(e => e.ReworkLayoutVersion, ReworkLayoutVersionIdProperty);

        /// <summary>
        /// 返工工艺路线版本
        /// </summary>
        public ReworkLayoutVersion ReworkLayoutVersion
        {
            get { return this.GetRefEntity(ReworkLayoutVersionProperty); }
            set { this.SetRefEntity(ReworkLayoutVersionProperty, value); }
        }
        #endregion

        #region 需求部门 Department
        /// <summary>
        /// 需求部门
        /// </summary>
        [Label("需求部门")]
        public static readonly Property<string> DepartmentProperty = P<ReworkInfoRecord>.Register(e => e.Department);

        /// <summary>
        /// 生产部门
        /// </summary>
        public string Department
        {
            get { return this.GetProperty(DepartmentProperty); }
            set { this.SetProperty(DepartmentProperty, value); }
        }
        #endregion

        #region 唯一码 UniqueCode
        /// <summary>
        /// 唯一码
        /// </summary>
        [Label("唯一码")]
        public static readonly Property<string> UniqueCodeProperty = P<ReworkInfoRecord>.Register(e => e.UniqueCode);

        /// <summary>
        /// 唯一码
        /// </summary>
        public string UniqueCode
        {
            get { return this.GetProperty(UniqueCodeProperty); }
            set { this.SetProperty(UniqueCodeProperty, value); }
        }
        #endregion

        #region 生产订单 ProductOrder
        /// <summary>
        /// 生产订单
        /// </summary>
        [Label("生产订单")]
        public static readonly Property<string> ProductOrderProperty = P<ReworkInfoRecord>.Register(e => e.ProductOrder);

        /// <summary>
        /// 生产订单
        /// </summary>
        public string ProductOrder
        {
            get { return this.GetProperty(ProductOrderProperty); }
            set { this.SetProperty(ProductOrderProperty, value); }
        }
        #endregion

        #region 反馈标识 Identification
        /// <summary>
        /// 反馈标识
        /// </summary>
        [Label("反馈标识")]
        public static readonly Property<string> IdentificationProperty = P<ReworkInfoRecord>.Register(e => e.Identification);

        /// <summary>
        /// 反馈标识
        /// </summary>
        public string Identification
        {
            get { return this.GetProperty(IdentificationProperty); }
            set { this.SetProperty(IdentificationProperty, value); }
        }
        #endregion

        #region 反馈信息 Msg
        /// <summary>
        /// 反馈信息
        /// </summary>
        [Label("反馈信息")]
        public static readonly Property<string> MsgProperty = P<ReworkInfoRecord>.Register(e => e.Msg);

        /// <summary>
        /// 反馈信息
        /// </summary>
        public string Msg
        {
            get { return this.GetProperty(MsgProperty); }
            set { this.SetProperty(MsgProperty, value); }
        }
        #endregion

        #region 是否上传 IsUpload
        /// <summary>
        /// 是否上传
        /// </summary>
        [Label("是否上传")]
        public static readonly Property<bool?> IsUploadProperty = P<ReworkInfoRecord>.Register(e => e.IsUpload);

        /// <summary>
        /// 是否上传
        /// </summary>
        public bool? IsUpload
        {
            get { return this.GetProperty(IsUploadProperty); }
            set { this.SetProperty(IsUploadProperty, value); }
        }
        #endregion

        #region 标签信息 ReworkInfoRecordDtlList
        /// <summary>
        /// 标签信息
        /// </summary>
        [Label("标签信息")]
        public static readonly ListProperty<EntityList<ReworkInfoRecordDtl>> ReworkInfoRecordDtlListProperty = P<ReworkInfoRecord>.RegisterList(e => e.ReworkInfoRecordDtlList);

        /// <summary>
        /// 标签信息
        /// </summary>
        public EntityList<ReworkInfoRecordDtl> ReworkInfoRecordDtlList
        {
            get { return this.GetLazyList(ReworkInfoRecordDtlListProperty); }
        }
        #endregion

        #region 视图属性

        #region 产品编码 ItemCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ItemCodeProperty = P<ReworkInfoRecord>.RegisterView(e => e.ItemCode, p => p.Item.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 产品名称 ItemName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ItemNameProperty = P<ReworkInfoRecord>.RegisterView(e => e.ItemName, p => p.Item.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 产品旧料号 ShortDescription
        /// <summary>
        /// 产品旧料号
        /// </summary>
        [Label("产品旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<ReworkInfoRecord>.RegisterView(e => e.ShortDescription, p => p.Item.ShortDescription);

        /// <summary>
        /// 产品旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }
        #endregion

        #region 生产版本 Version
        /// <summary>
        /// 生产版本
        /// </summary>
        [Label("生产版本")]
        public static readonly Property<string> VersionProperty = P<ReworkInfoRecord>.RegisterView(e => e.Version, p => p.ReworkLayoutVersion.Version);

        /// <summary>
        /// 生产版本
        /// </summary>
        public string Version
        {
            get { return this.GetProperty(VersionProperty); }
        }
        #endregion

        #endregion

    }

    internal class ReworkInfoRecordConfig : EntityConfig<ReworkInfoRecord>
    {

        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.AddRule(ReworkInfoRecord.UniqueCodeProperty, new NotDuplicateRule());
            base.AddValidations(rules);
        }
        protected override void ConfigMeta()
        {
            Meta.MapTable("REWORK_INFO_RECORD").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
