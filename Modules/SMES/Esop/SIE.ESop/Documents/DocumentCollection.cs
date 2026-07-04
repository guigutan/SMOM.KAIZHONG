using SIE.Common.Configs;
using SIE.Domain;
using SIE.ESop.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档集
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(AttachmentConfig))]
    [ConditionQueryType(typeof(DocumentCollectionCriteria))]
    [Label("文档集")]
    [DisplayMember(nameof(DocumentCollection.Code))]
    public partial class DocumentCollection : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码"), MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<DocumentCollection>.Register(e => e.Code);

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
        [Required]
        [NotDuplicate]
        [Label("名称"), MaxLength(80)]
        public static readonly Property<string> NameProperty = P<DocumentCollection>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region MD5 Md5
        /// <summary>
        /// MD5
        /// </summary>
        [Label("MD5")]
        public static readonly Property<string> Md5Property = P<DocumentCollection>.Register(e => e.Md5);

        /// <summary>
        /// MD5
        /// </summary>
        public string Md5
        {
            get { return GetProperty(Md5Property); }
            set { SetProperty(Md5Property, value); }
        }
        #endregion

        #region 来源 Source
        /// <summary>
        /// 来源
        /// </summary>
        [Label("来源")]
        public static readonly Property<Source> SourceProperty = P<DocumentCollection>.Register(e => e.Source);

        /// <summary>
        /// 来源
        /// </summary>
        public Source Source
        {
            get { return GetProperty(SourceProperty); }
            set { SetProperty(SourceProperty, value); }
        }
        #endregion

        #region 文件名 FileName
        /// <summary>
        /// 文件名
        /// </summary>
        [Label("文件名")]
        public static readonly Property<string> FileNameProperty = P<DocumentCollection>.Register(e => e.FileName);

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return GetProperty(FileNameProperty); }
            set { SetProperty(FileNameProperty, value); }
        }
        #endregion

        #region 存储路径 FilePath
        /// <summary>
        /// 存储路径
        /// </summary>
        [Label("存储路径")]
        public static readonly Property<string> FilePathProperty = P<DocumentCollection>.Register(e => e.FilePath);

        /// <summary>
        /// 存储路径
        /// </summary>
        public string FilePath
        {
            get { return GetProperty(FilePathProperty); }
            set { SetProperty(FilePathProperty, value); }
        }
        #endregion

        #region 文件扩展名 FileExtesion
        /// <summary>
        /// 文件扩展名
        /// </summary>
        [Label("文件扩展名")]
        public static readonly Property<string> FileExtensionProperty = P<DocumentCollection>.Register(e => e.FileExtension);

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtension
        {
            get { return GetProperty(FileExtensionProperty); }
            set { SetProperty(FileExtensionProperty, value); }
        }
        #endregion

        #region 文件大小 FileSize
        /// <summary>
        /// 文件大小
        /// </summary>
        [Label("文件大小")]
        public static readonly Property<string> FileSizeProperty = P<DocumentCollection>.Register(e => e.FileSize);

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize
        {
            get { return GetProperty(FileSizeProperty); }
            set { SetProperty(FileSizeProperty, value); }
        }
        #endregion

        #region 已处理 IsProcessed
        /// <summary>
        /// 已处理
        /// </summary>
        [Label("已处理")]
        public static readonly Property<bool?> IsProcessedProperty = P<DocumentCollection>.Register(e => e.IsProcessed);

        /// <summary>
        /// 已处理
        /// </summary>
        public bool? IsProcessed
        {
            get { return this.GetProperty(IsProcessedProperty); }
            set { this.SetProperty(IsProcessedProperty, value); }
        }
        #endregion

        #region 处理时间
        /// <summary>
        /// 处理时间
        /// </summary>
        public static readonly Property<DateTime?> ProcessedDateProperty = P<DocumentCollection>.Register(e => e.ProcessedDate);

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessedDate
        {
            get { return this.GetProperty(ProcessedDateProperty); }
            set { this.SetProperty(ProcessedDateProperty, value); }
        }
        #endregion

        #region 内容 Content
        /// <summary>
        /// 内容
        /// </summary>
        public static readonly Property<byte[]> ContentProperty = P<DocumentCollection>.Register(e => e.Content);

        /// <summary>
        /// 内容
        /// </summary>
        public byte[] Content
        {
            get { return GetProperty(ContentProperty); }
            set { SetProperty(ContentProperty, value); }
        }
        #endregion



        #region 字符串内容 strContent
        /// <summary>
        /// 字符串内容
        /// </summary>
        [Label("字符串内容")]
        public static readonly Property<string> StrContentProperty = P<DocumentCollection>.Register(e => e.StrContent);

        /// <summary>
        /// 字符串内容
        /// </summary>
        public string   StrContent
        {
            get { return this.GetProperty(StrContentProperty); }
            set { this.SetProperty(StrContentProperty, value); }
        }
        #endregion


        #region 文档列表 DocumentList
        /// <summary>
        /// 文档列表
        /// </summary>
        [Label("文档")]
        public static readonly ListProperty<EntityList<Document>> DocumentListProperty = P<DocumentCollection>.RegisterList(e => e.DocumentList);

        /// <summary>
        /// 文档列表
        /// </summary>
        public EntityList<Document> DocumentList
        {
            get { return this.GetLazyList(DocumentListProperty); }
        }
        #endregion

        #region 上传 UploadFilePath
        /// <summary>
        /// 上传
        /// </summary>
        [Label("上传")]
        public static readonly Property<string> UploadFilePathProperty = P<DocumentCollection>.Register(e => e.UploadFilePath);

        /// <summary>
        /// 存储路径
        /// </summary>
        public string UploadFilePath
        {
            get { return GetProperty(UploadFilePathProperty); }
            set { SetProperty(UploadFilePathProperty, value); }
        }
        #endregion

        #region 适用产品集合 ItemList
        /// <summary>
        /// 适用产品集合
        /// </summary>
        [Label("适用产品")]
        public static readonly ListProperty<EntityList<DocumentCollectionItem>> ItemListProperty = P<DocumentCollection>.RegisterList(e => e.ItemList);

        /// <summary>
        /// 适用产品集合
        /// </summary>
        public EntityList<DocumentCollectionItem> ItemList
        {
            get { return this.GetLazyList(ItemListProperty); }
        }
        #endregion

        #region 适用工单 WorkOrderList
        /// <summary>
        /// 适用工单
        /// </summary>
        [Label("适用工单")]
        public static readonly ListProperty<EntityList<DocumentCollectionWorkOrder>> WorkOrderListProperty = P<DocumentCollection>.RegisterList(e => e.WorkOrderList);

        /// <summary>
        /// 适用工单
        /// </summary>
        public EntityList<DocumentCollectionWorkOrder> WorkOrderList
        {
            get { return this.GetLazyList(WorkOrderListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 文档集 实体配置
    /// </summary>
    internal class DocumentCollectionConfig : EntityConfig<DocumentCollection>
    {
        /// <summary>
        /// 数据库映射配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ESOP_DOC_SET").MapAllPropertiesExcept(DocumentCollection.ContentProperty, 
                DocumentCollection.UploadFilePathProperty,
                 DocumentCollection.StrContentProperty);
            Meta.Property(DocumentCollection.FilePathProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}