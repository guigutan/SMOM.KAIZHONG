using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.ESop.Documents;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ESop.EngDocuments
{
    /// <summary>
    /// 工程文件子表
    /// </summary>
    [ChildEntity, Serializable]
    [Label("工程文件")]
    public class EngDocumentDetail : DataEntity
    {
        /// <summary>
        /// 使用类型快码
        /// </summary>
        public const string DocTypeCatalogType = "DOC_TYPE";

        #region 工程文件维护 EngDocument
        /// <summary>
        /// 工程文件维护Id
        /// </summary>
        [Label("工程文件维护")]
        public static readonly IRefIdProperty EngDocumentIdProperty =
            P<EngDocumentDetail>.RegisterRefId(e => e.EngDocumentId, ReferenceType.Parent);

        /// <summary>
        /// 工程文件维护Id
        /// </summary>
        public double EngDocumentId
        {
            get { return (double)this.GetRefId(EngDocumentIdProperty); }
            set { this.SetRefId(EngDocumentIdProperty, value); }
        }

        /// <summary>
        /// 工程文件维护
        /// </summary>
        public static readonly RefEntityProperty<EngDocument> EngDocumentProperty =
            P<EngDocumentDetail>.RegisterRef(e => e.EngDocument, EngDocumentIdProperty);

        /// <summary>
        /// 工程文件维护
        /// </summary>
        public EngDocument EngDocument
        {
            get { return this.GetRefEntity(EngDocumentProperty); }
            set { this.SetRefEntity(EngDocumentProperty, value); }
        }
        #endregion

        #region 文件编码 DocCode
        /// <summary>
        /// 文件编码
        /// </summary>
        [Label("文件编码")]
        public static readonly Property<string> DocCodeProperty = P<EngDocumentDetail>.Register(e => e.DocCode);

        /// <summary>
        /// 文件编码
        /// </summary>
        public string DocCode
        {
            get { return this.GetProperty(DocCodeProperty); }
            set { this.SetProperty(DocCodeProperty, value); }
        }
        #endregion

        #region 文件名称 DocName
        /// <summary>
        /// 文件名称
        /// </summary>
        [Label("文件名称")]
        public static readonly Property<string> DocNameProperty = P<EngDocumentDetail>.Register(e => e.DocName);

        /// <summary>
        /// 文件名称
        /// </summary>
        public string DocName
        {
            get { return this.GetProperty(DocNameProperty); }
            set { this.SetProperty(DocNameProperty, value); }
        }
        #endregion

        #region 使用类型 UseType
        /// <summary>
        /// 使用类型
        /// </summary>
        [Label("使用类型")]
        public static readonly Property<string> UseTypeProperty = P<EngDocumentDetail>.Register(e => e.UseType);

        /// <summary>
        /// 使用类型
        /// </summary>
        public string UseType
        {
            get { return this.GetProperty(UseTypeProperty); }
            set { this.SetProperty(UseTypeProperty, value); }
        }
        #endregion

        #region 存储路径 SavePath
        /// <summary>
        /// 存储路径
        /// </summary>
        [Label("存储路径")]
        [MaxLength(2000)]
        public static readonly Property<string> SavePathProperty = P<EngDocumentDetail>.Register(e => e.SavePath);

        /// <summary>
        /// 存储路径
        /// </summary>
        public string SavePath
        {
            get { return this.GetProperty(SavePathProperty); }
            set { this.SetProperty(SavePathProperty, value); }
        }
        #endregion

        #region 存储路径展示 SavePathDisplay
        /// <summary>
        /// 存储路径展示
        /// </summary>
        [Label("存储路径展示")]
        [MaxLength(2000)]
        public static readonly Property<string> SavePathDisplayProperty = P<EngDocumentDetail>.Register(e => e.SavePathDisplay);

        /// <summary>
        /// 存储路径展示
        /// </summary>
        public string SavePathDisplay
        {
            get { return this.GetProperty(SavePathDisplayProperty); }
            set { this.SetProperty(SavePathDisplayProperty, value); }
        }
        #endregion

        #region 文件大小 FileSize
        /// <summary>
        /// 文件大小
        /// </summary>
        [Label("文件大小")]
        public static readonly Property<string> FileSizeProperty = P<EngDocumentDetail>.Register(e => e.FileSize);

        /// <summary>
        /// 文件大小
        /// </summary>
        public string FileSize
        {
            get { return this.GetProperty(FileSizeProperty); }
            set { this.SetProperty(FileSizeProperty, value); }
        }
        #endregion

        #region 文件类型 DocumentType
        /// <summary>
        /// 文件类型
        /// </summary>
        [Label("文件类型")]
        public static readonly Property<DocumentType> DocumentTypeProperty = P<EngDocumentDetail>.Register(e => e.DocumentType);

        /// <summary>
        /// 文件类型
        /// </summary>
        public DocumentType DocumentType
        {
            get { return this.GetProperty(DocumentTypeProperty); }
            set { this.SetProperty(DocumentTypeProperty, value); }
        }
        #endregion

        #region 存储路径Id FId
        /// <summary>
        /// 存储路径Id
        /// </summary>
        [Label("存储路径Id")]
        public static readonly Property<double?> FIdProperty = P<EngDocumentDetail>.Register(e => e.FId);

        /// <summary>
        /// 存储路径Id
        /// </summary>
        public double? FId
        {
            get { return this.GetProperty(FIdProperty); }
            set { this.SetProperty(FIdProperty, value); }
        }
        #endregion

        #region 存储名称 ServerFileName
        /// <summary>
        /// 存储名称
        /// </summary>
        [Label("存储名称")]
        [MaxLength(2000)]
        public static readonly Property<string> ServerFileNameProperty = P<EngDocumentDetail>.Register(e => e.ServerFileName);

        /// <summary>
        /// 存储名称
        /// </summary>
        public string ServerFileName
        {
            get { return this.GetProperty(ServerFileNameProperty); }
            set { this.SetProperty(ServerFileNameProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<EngDocumentDetail>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<EngDocumentDetail>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region Sheet页 SheetPage
        /// <summary>
        /// Sheet页
        /// </summary>
        [Label("Sheet页")]
        public static readonly Property<string> SheetPageProperty = P<EngDocumentDetail>.Register(e => e.SheetPage);

        /// <summary>
        /// Sheet页
        /// </summary>
        public string SheetPage
        {
            get { return this.GetProperty(SheetPageProperty); }
            set { this.SetProperty(SheetPageProperty, value); }
        }
        #endregion

        #region MD5加密 MD5
        /// <summary>
        /// MD5加密
        /// </summary>
        [Label("MD5加密")]
        public static readonly Property<string> MD5Property = P<EngDocumentDetail>.Register(e => e.MD5);

        /// <summary>
        /// MD5加密
        /// </summary>
        public string MD5
        {
            get { return this.GetProperty(MD5Property); }
            set { this.SetProperty(MD5Property, value); }
        }
        #endregion

    }

    /// <summary>
    /// 工程文件子表配置
    /// </summary>
    public class EngDocumentDetailConfig : EntityConfig<EngDocumentDetail>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ENG_DOC_DETAIL").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(EngDocumentDetail.SavePathProperty).MapColumn().HasLength(4000);
            Meta.Property(EngDocumentDetail.SavePathDisplayProperty).MapColumn().HasLength(4000);
            Meta.Property(EngDocumentDetail.ServerFileNameProperty).MapColumn().HasLength(4000);
            Meta.EnableSort();
        }
    }
}
