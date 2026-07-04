using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档集 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("文档集查询实体")]
    public partial class DocumentCollectionCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<DocumentCollectionCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<DocumentCollectionCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 文件名 FileName
        /// <summary>
        /// 文件名
        /// </summary>
        [Label("文件名")]
        public static readonly Property<string> FileNameProperty = P<DocumentCollectionCriteria>.Register(e => e.FileName);

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return this.GetProperty(FileNameProperty); }
            set { this.SetProperty(FileNameProperty, value); }
        }
        #endregion

        #region 产品 Item
        /// <summary>
        /// 产品Id
        /// </summary>
        [Label("产品")]
        public static readonly IRefIdProperty ItemIdProperty = P<DocumentCollectionCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ItemId
        {
            get { return (double?)GetRefNullableId(ItemIdProperty); }
            set { SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 产品
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<DocumentCollectionCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 产品
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<DocumentCollectionCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<DocumentCollectionCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 来源 Source 
        /// <summary>
        /// 来源
        /// </summary>
        public static readonly Property<Source?> SourceProperty = P<DocumentCollectionCriteria>.Register(e => e.Source);

        /// <summary>
        /// 来源
        /// </summary>
        public Source? Source
        {
            get { return GetProperty(SourceProperty); }
            set { SetProperty(SourceProperty, value); }
        }
        #endregion

        #region 未处理 UnProcess
        /// <summary>
        /// 未处理
        /// </summary>
        public static readonly Property<bool> UnProcessProperty = P<DocumentCollectionCriteria>.Register(e => e.UnProcess);

        /// <summary>
        /// 未处理
        /// </summary>
        public bool UnProcess
        {
            get { return this.GetProperty(UnProcessProperty); }
            set { this.SetProperty(UnProcessProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>返回查询的结果集</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DocumentCollectionController>().GetList(this);
        }
    }
}