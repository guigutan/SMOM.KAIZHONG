using SIE.Common.Configs;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ESop.EngDocuments.Enums;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ESop.EngDocuments
{
    /// <summary>
    /// 工程文件维护实体
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EngDocCriteria))]
    [Label("工程文件维护")]
    public class EngDocument : DataEntity
    {
        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<EngDocType> TypeProperty = P<EngDocument>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public EngDocType Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 产品 Product
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<EngDocument>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<EngDocument>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<EngDocument>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<EngDocument>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工程文件 EngDocumentDetailList
        /// <summary>
        /// 工程文件
        /// </summary>
        [Label("工程文件")]
        public static readonly ListProperty<EntityList<EngDocumentDetail>> EngDocumentDetailListProperty = P<EngDocument>.RegisterList(e => e.EngDocumentDetailList);

        /// <summary>
        /// 工程文件
        /// </summary>
        public EntityList<EngDocumentDetail> EngDocumentDetailList
        {
            get { return this.GetLazyList(EngDocumentDetailListProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<EngDocument>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class EngDocumentConfig : EntityConfig<EngDocument>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ENG_DOCUMENT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
