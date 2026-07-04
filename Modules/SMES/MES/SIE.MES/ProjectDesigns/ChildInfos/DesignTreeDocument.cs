using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-文件上传
    /// </summary>
    [RootEntity, Serializable]
    [Label("工艺资料-文件上传")]
    public class DesignTreeDocument : DataEntity
    {
        /// <summary>
        /// 文档类型快码
        /// </summary>
        public const string DesignTreeDocumentType = "DESIGNTREE_DOCTYPE";

        #region 需求设计 ProjectDesign
        /// <summary>
        /// 需求设计Id
        /// </summary>
        [Label("需求设计")]
        public static readonly IRefIdProperty ProjectDesignIdProperty =
            P<DesignTreeDocument>.RegisterRefId(e => e.ProjectDesignId, ReferenceType.Normal);

        /// <summary>
        /// 需求设计Id
        /// </summary>
        public double ProjectDesignId
        {
            get { return (double)this.GetRefId(ProjectDesignIdProperty); }
            set { this.SetRefId(ProjectDesignIdProperty, value); }
        }

        /// <summary>
        /// 需求设计
        /// </summary>
        public static readonly RefEntityProperty<ProjectDesignDetail> ProjectDesignProperty =
            P<DesignTreeDocument>.RegisterRef(e => e.ProjectDesign, ProjectDesignIdProperty);

        /// <summary>
        /// 需求设计
        /// </summary>
        public ProjectDesignDetail ProjectDesign
        {
            get { return this.GetRefEntity(ProjectDesignProperty); }
            set { this.SetRefEntity(ProjectDesignProperty, value); }
        }
        #endregion

        #region 文档编码 DocCode
        /// <summary>
        /// 文档编码
        /// </summary>
        [Label("文档编码")]
        public static readonly Property<string> DocCodeProperty = P<DesignTreeDocument>.Register(e => e.DocCode);

        /// <summary>
        /// 文档编码
        /// </summary>
        public string DocCode
        {
            get { return this.GetProperty(DocCodeProperty); }
            set { this.SetProperty(DocCodeProperty, value); }
        }
        #endregion

        #region 文档名称 DocName
        /// <summary>
        /// 文档名称
        /// </summary>
        [Label("文档名称")]
        public static readonly Property<string> DocNameProperty = P<DesignTreeDocument>.Register(e => e.DocName);

        /// <summary>
        /// 文档名称
        /// </summary>
        public string DocName
        {
            get { return this.GetProperty(DocNameProperty); }
            set { this.SetProperty(DocNameProperty, value); }
        }
        #endregion

        #region 文档版本号 DocVer
        /// <summary>
        /// 文档版本号
        /// </summary>
        [Label("文档版本号")]
        public static readonly Property<string> DocVerProperty = P<DesignTreeDocument>.Register(e => e.DocVer);

        /// <summary>
        /// 文档版本号
        /// </summary>
        public string DocVer
        {
            get { return this.GetProperty(DocVerProperty); }
            set { this.SetProperty(DocVerProperty, value); }
        }
        #endregion

        #region 文档类型 DocType
        /// <summary>
        /// 文档类型
        /// </summary>
        [Label("文档类型")]
        public static readonly Property<string> DocTypeProperty = P<DesignTreeDocument>.Register(e => e.DocType);

        /// <summary>
        /// 文档类型
        /// </summary>
        public string DocType
        {
            get { return this.GetProperty(DocTypeProperty); }
            set { this.SetProperty(DocTypeProperty, value); }
        }
        #endregion

        #region 产品编码 Product
        /// <summary>
        /// 产品编码Id
        /// </summary>
        [Label("产品编码")]
        public static readonly IRefIdProperty ProductIdProperty =
            P<DesignTreeDocument>.RegisterRefId(e => e.ProductId, ReferenceType.Normal);

        /// <summary>
        /// 产品编码Id
        /// </summary>
        public double? ProductId
        {
            get { return (double?)this.GetRefNullableId(ProductIdProperty); }
            set { this.SetRefNullableId(ProductIdProperty, value); }
        }

        /// <summary>
        /// 产品编码
        /// </summary>
        public static readonly RefEntityProperty<Item> ProductProperty =
            P<DesignTreeDocument>.RegisterRef(e => e.Product, ProductIdProperty);

        /// <summary>
        /// 产品编码
        /// </summary>
        public Item Product
        {
            get { return this.GetRefEntity(ProductProperty); }
            set { this.SetRefEntity(ProductProperty, value); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<DesignTreeDocument>.RegisterView(e => e.ProductName, p => p.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<DesignTreeDocument>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<DesignTreeDocument>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 附件列表 AttachmentList
        /// <summary>
        /// 附件列表
        /// </summary>
        [Label("附件列表")]
        public static readonly ListProperty<EntityList<DesignTreeDocumentAttachment>> AttachmentListProperty = P<DesignTreeDocument>.RegisterList(e => e.AttachmentList);

        /// <summary>
        /// 附件列表
        /// </summary>
        public EntityList<DesignTreeDocumentAttachment> AttachmentList
        {
            get { return this.GetLazyList(AttachmentListProperty); }
        }
        #endregion

    }   

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignTreeDocumentConfig : EntityConfig<DesignTreeDocument>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRODES_DOC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
