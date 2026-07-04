using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档 查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("文档查询实体")]
    public partial class DocumentCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<DocumentCriteria>.Register(e => e.Code);

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
        public static readonly Property<string> NameProperty = P<DocumentCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 文档集 DocumentCollection
        /// <summary>
        /// 文档集ID
        /// </summary>
        public static readonly IRefIdProperty DocumentCollectionIdProperty = P<DocumentCriteria>.RegisterRefId(e => e.DocumentCollectionId, ReferenceType.Normal);

        /// <summary>
        /// 文档集ID
        /// </summary>
        public double? DocumentCollectionId
        {
            get { return (double?)this.GetRefNullableId(DocumentCollectionIdProperty); }
            set { this.SetRefNullableId(DocumentCollectionIdProperty, value); }
        }

        /// <summary>
        /// 文档集
        /// </summary>
        public static readonly RefEntityProperty<DocumentCollection> DocumentCollectionProperty = P<DocumentCriteria>.RegisterRef(e => e.DocumentCollection, DocumentCollectionIdProperty);

        /// <summary>
        /// 文档集
        /// </summary>
        public DocumentCollection DocumentCollection
        {
            get { return this.GetRefEntity(DocumentCollectionProperty); }
            set { this.SetRefEntity(DocumentCollectionProperty, value); }
        }
        #endregion

        #region 文件名 FileName
        /// <summary>
        /// 文件名
        /// </summary>
        public static readonly Property<string> FileNameProperty = P<DocumentCriteria>.Register(e => e.FileName);

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return this.GetProperty(FileNameProperty); }
            set { this.SetProperty(FileNameProperty, value); }
        }
        #endregion

        #region 文件扩展名 FileExtesion
        /// <summary>
        /// 文件扩展名
        /// </summary>
        public static readonly Property<string> FileExtesionProperty = P<DocumentCriteria>.Register(e => e.FileExtesion);

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtesion
        {
            get { return GetProperty(FileExtesionProperty); }
            set { SetProperty(FileExtesionProperty, value); }
        }
        #endregion

        #region 文件大小 FileSize
        /// <summary>
        /// 文件大小
        /// </summary>
        public static readonly Property<string> FileSizeProperty = P<DocumentCriteria>.Register(e => e.FileSize);

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize
        {
            get { return GetProperty(FileSizeProperty); }
            set { SetProperty(FileSizeProperty, value); }
        }
        #endregion

        #region MD5 Md5
        /// <summary>
        /// MD5
        /// </summary>
        public static readonly Property<string> Md5Property = P<DocumentCriteria>.Register(e => e.Md5);

        /// <summary>
        /// MD5
        /// </summary>
        public string Md5
        {
            get { return GetProperty(Md5Property); }
            set { SetProperty(Md5Property, value); }
        }
        #endregion

        #region 工序  Process
        /// <summary>
        /// 工序Id
        /// </summary>
        public static readonly IRefIdProperty ProcessIdProperty = P<DocumentCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<DocumentCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 文档类型 DocumentType
        /// <summary>
        /// 文档类型
        /// </summary>
        public static readonly Property<DocumentType> DocumentTypeProperty = P<DocumentCriteria>.Register(e => e.DocumentType);

        /// <summary>
        /// 文档类型
        /// </summary>
        public DocumentType DocumentType
        {
            get { return GetProperty(DocumentTypeProperty); }
            set { SetProperty(DocumentTypeProperty, value); }
        }
        #endregion

        #region 来源 Source
        /// <summary>
        /// 来源
        /// </summary>
        public static readonly Property<Source?> SourceProperty = P<DocumentCriteria>.Register(e => e.Source);

        /// <summary>
        /// 来源
        /// </summary>
        public Source? Source
        {
            get { return GetProperty(SourceProperty); }
            set { SetProperty(SourceProperty, value); }
        }
        #endregion

        #region 物料 Item
        /// <summary>
        /// 物料ID
        /// </summary>
        public static readonly IRefIdProperty ItemIdProperty = P<DocumentCriteria>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double? ItemId
        {
            get { return (double?)this.GetRefNullableId(ItemIdProperty); }
            set { this.SetRefNullableId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<DocumentCriteria>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<DocumentCriteria>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

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
            P<DocumentCriteria>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns>返回查询结果集</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<DocumentCollectionController>().GetList(this);
        }
    }
}