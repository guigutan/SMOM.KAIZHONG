using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.ESop.Documents
{
    /// <summary>
    /// 文档
    /// </summary>
    [ChildEntity, Serializable]
    [Label("文档")]
    [DisplayMember(nameof(Document.Name))]
    public partial class Document : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [Label("编码"), MaxLength(80)]
        public static readonly Property<string> CodeProperty = P<Document>.Register(e => e.Code);

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
        [Label("名称"), MaxLength(80)]
        public static readonly Property<string> NameProperty = P<Document>.Register(e => e.Name);

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
        [Label("文件名"), MaxLength(80)]
        public static readonly Property<string> FileNameProperty = P<Document>.Register(e => e.FileName);

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return GetProperty(FileNameProperty); }
            set { SetProperty(FileNameProperty, value); }
        }
        #endregion

        #region 文件扩展名 FileExtesion
        /// <summary>
        /// 文件扩展名
        /// </summary>
        [Label("文件扩展名"), MaxLength(80)]
        public static readonly Property<string> FileExtensionProperty = P<Document>.Register(e => e.FileExtension);

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
        public static readonly Property<string> FileSizeProperty = P<Document>.Register(e => e.FileSize);

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize
        {
            get { return GetProperty(FileSizeProperty); }
            set { SetProperty(FileSizeProperty, value); }
        }
        #endregion

        #region 存储路径 FilePath
        /// <summary>
        /// 存储路径
        /// </summary>
        [Label("存储路径"), MaxLength(200)]
        public static readonly Property<string> FilePathProperty = P<Document>.Register(e => e.FilePath);

        /// <summary>
        /// 存储路径
        /// </summary>
        public string FilePath
        {
            get { return GetProperty(FilePathProperty); }
            set { SetProperty(FilePathProperty, value); }
        }
        #endregion

        #region 是否已处理 IsProcessed
        /// <summary>
        /// 已处理
        /// </summary>
        [Label("已处理")]
        public static readonly Property<bool> IsProcessedProperty = P<Document>.Register(e => e.IsProcessed);

        /// <summary>
        /// 已处理
        /// </summary>
        public bool IsProcessed
        {
            get { return this.GetProperty(IsProcessedProperty); }
            set { this.SetProperty(IsProcessedProperty, value); }
        }
        #endregion

        #region 处理时间 ProcessedDate
        /// <summary>
        /// 处理时间
        /// </summary>
        public static readonly Property<DateTime?> ProcessedDateProperty = P<Document>.Register(e => e.ProcessedDate);

        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime? ProcessedDate
        {
            get { return this.GetProperty(ProcessedDateProperty); }
            set { this.SetProperty(ProcessedDateProperty, value); }
        }
        #endregion

        #region MD5 Md5
        /// <summary>
        /// MD5
        /// </summary>
        [Label("MD5")]
        public static readonly Property<string> Md5Property = P<Document>.Register(e => e.Md5);

        /// <summary>
        /// MD5
        /// </summary>
        public string Md5
        {
            get { return GetProperty(Md5Property); }
            set { SetProperty(Md5Property, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<Document>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<Document>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 文档类型 DocumentType
        /// <summary>
        /// 文档类型
        /// </summary>
        [Label("文档类型")]
        public static readonly Property<DocumentType> DocumentTypeProperty = P<Document>.Register(e => e.DocumentType);

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
        [Label("来源")]
        public static readonly Property<Source> SourceProperty = P<Document>.Register(e => e.Source);

        /// <summary>
        /// 来源
        /// </summary>
        public Source Source
        {
            get { return GetProperty(SourceProperty); }
            set { SetProperty(SourceProperty, value); }
        }
        #endregion

        #region 文档集 DocumentCollection
        /// <summary>
        /// 文档集Id
        /// </summary>
        public static readonly IRefIdProperty DocumentCollectionIdProperty = P<Document>.RegisterRefId(e => e.DocumentCollectionId, ReferenceType.Parent);

        /// <summary>
        /// 文档集Id
        /// </summary>
        public double? DocumentCollectionId
        {
            get { return (double?)GetRefNullableId(DocumentCollectionIdProperty); }
            set { SetRefNullableId(DocumentCollectionIdProperty, value); }
        }

        /// <summary>
        /// 文档集
        /// </summary>
        public static readonly RefEntityProperty<DocumentCollection> DocumentCollectionProperty = P<Document>.RegisterRef(e => e.DocumentCollection, DocumentCollectionIdProperty);

        /// <summary>
        /// 文档集
        /// </summary>
        public DocumentCollection DocumentCollection
        {
            get { return GetRefEntity(DocumentCollectionProperty); }
            set { SetRefEntity(DocumentCollectionProperty, value); }
        }
        #endregion

        #region 内容 Content
        /// <summary>
        /// 内容
        /// </summary>
        public static readonly Property<byte[]> ContentProperty = P<Document>.Register(e => e.Content);

        /// <summary>
        /// 内容
        /// </summary>
        public byte[] Content
        {
            get { return GetProperty(ContentProperty); }
            set { SetProperty(ContentProperty, value); }
        }
        #endregion

        #region 视图属性(关联实体属性平铺显示，一般用于Web)
        #region 工序 ProcessName
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<Document>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
            set { SetProperty(ProcessNameProperty, value); }
        }
        #endregion
        #endregion

        #region 对接工程文件字段
        #region Sheet页 SheetName
        /// <summary>
        /// Sheet页
        /// </summary>
        [Label("Sheet页")]
        public static readonly Property<string> SheetNameProperty = P<Document>.Register(e => e.SheetName);

        /// <summary>
        /// Sheet页
        /// </summary>
        public string SheetName
        {
            get { return this.GetProperty(SheetNameProperty); }
            set { this.SetProperty(SheetNameProperty, value); }
        }
        #endregion

        #endregion


        #region PDF播放开始页 PdfPlayBeginPage
        /// <summary>
        /// PDF播放开始页
        /// </summary>
        [Label("PDF播放开始页")]
        public static readonly Property<int?> PdfPlayBeginPageProperty = P<Document>.Register(e => e.PdfPlayBeginPage);

        /// <summary>
        /// PDF播放开始页
        /// </summary>
        public int? PdfPlayBeginPage
        {
            get { return GetProperty(PdfPlayBeginPageProperty); }
            set { SetProperty(PdfPlayBeginPageProperty, value); }
        }
        #endregion

        #region PDF播放结束页 PdfPlayEndPage
        /// <summary>
        /// PDF播放结束页
        /// </summary>
        [Label("PDF播放结束页")]
        public static readonly Property<int?> PdfPlayEndPageProperty = P<Document>.Register(e => e.PdfPlayEndPage);

        /// <summary>
        /// PDF播放结束页
        /// </summary>
        public int? PdfPlayEndPage
        {
            get { return GetProperty(PdfPlayEndPageProperty); }
            set { SetProperty(PdfPlayEndPageProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 文档 实体配置
    /// </summary>
    internal class DocumentConfig : EntityConfig<Document>
    {
        /// <summary>
        /// 数据库映射配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ESOP_DOC").MapAllPropertiesExcept(Document.ContentProperty);
            Meta.Property(Document.FilePathProperty).ColumnMeta.HasLength(2000);
            Meta.Property(Document.DocumentCollectionIdProperty).ColumnMeta.HasIndex();
            Meta.Property(Document.SheetNameProperty).DontMapColumn();
            Meta.EnablePhantoms();
            Meta.EnableSort();
        }

        /// <summary>
        /// 增加验证规则
        /// </summary>
        /// <param name="rules">验证规则集合</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(Document.CodeProperty, new NotDuplicateRule()
            {
                Properties = { Document.CodeProperty, Document.DocumentCollectionIdProperty },
                MessageBuilder = (o) =>
                {
                    return "文档编码重复".L10N();
                }
            });
            rules.AddRule(Document.NameProperty, new NotDuplicateRule()
            {
                Properties = { Document.NameProperty, Document.DocumentCollectionIdProperty },
                MessageBuilder = (o) =>
                {
                    return "文档名称重复".L10N();
                }
            });
        }
    }
}