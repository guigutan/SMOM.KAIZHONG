using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ChildInfos
{
    /// <summary>
    /// 工艺资料-产品Bom
    /// </summary>
    [RootEntity, Serializable]
    [Label("工艺资料-产品Bom")]
    public class DesignTreeBom : DesignProduct
    {
        #region 需求设计 ProjectDesign
        /// <summary>
        /// 需求设计Id
        /// </summary>
        [Label("需求设计")]
        public static readonly IRefIdProperty ProjectDesignIdProperty =
            P<DesignTreeBom>.RegisterRefId(e => e.ProjectDesignId, ReferenceType.Normal);

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
            P<DesignTreeBom>.RegisterRef(e => e.ProjectDesign, ProjectDesignIdProperty);

        /// <summary>
        /// 需求设计
        /// </summary>
        public ProjectDesignDetail ProjectDesign
        {
            get { return this.GetRefEntity(ProjectDesignProperty); }
            set { this.SetRefEntity(ProjectDesignProperty, value); }
        }
        #endregion

        #region Bom编码 BomCode
        /// <summary>
        /// Bom编码
        /// </summary>
        [Label("Bom编码")]
        public static readonly Property<string> BomCodeProperty = P<DesignTreeBom>.Register(e => e.BomCode);

        /// <summary>
        /// Bom编码
        /// </summary>
        public string BomCode
        {
            get { return this.GetProperty(BomCodeProperty); }
            set { this.SetProperty(BomCodeProperty, value); }
        }
        #endregion

        #region Bom名称 BomName
        /// <summary>
        /// Bom名称
        /// </summary>
        [Label("Bom名称")]
        public static readonly Property<string> BomNameProperty = P<DesignTreeBom>.Register(e => e.BomName);

        /// <summary>
        /// Bom名称
        /// </summary>
        public string BomName
        {
            get { return this.GetProperty(BomNameProperty); }
            set { this.SetProperty(BomNameProperty, value); }
        }
        #endregion

        #region 版本号 Version
        /// <summary>
        /// 版本号
        /// </summary>
        [Label("版本号")]
        public static readonly Property<string> VersionProperty = P<DesignTreeBom>.Register(e => e.Version);

        /// <summary>
        /// 版本号
        /// </summary>
        public string Version
        {
            get { return this.GetProperty(VersionProperty); }
            set { this.SetProperty(VersionProperty, value); }
        }
        #endregion

        #region 是否已更新 HasUp
        /// <summary>
        /// 是否已更新
        /// </summary>
        [Label("是否已更新")]
        public static readonly Property<bool?> HasUpProperty = P<DesignTreeBom>.Register(e => e.HasUp);

        /// <summary>
        /// 是否已更新
        /// </summary>
        public bool? HasUp
        {
            get { return this.GetProperty(HasUpProperty); }
            set { this.SetProperty(HasUpProperty, value); }
        }
        #endregion

        #region 产品Bom明细 DetailList
        /// <summary>
        /// 产品Bom明细
        /// </summary>
        [Label("产品Bom明细")]
        public static readonly ListProperty<EntityList<DesignTreeBomDetail>> DetailListProperty = P<DesignTreeBom>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 产品Bom明细
        /// </summary>
        public EntityList<DesignTreeBomDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignTreeBomConfig : EntityConfig<DesignTreeBom>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRODES_PROTREE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
