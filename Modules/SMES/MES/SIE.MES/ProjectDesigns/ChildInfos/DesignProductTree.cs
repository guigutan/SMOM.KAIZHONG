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
    /// 项目号需求设计-工艺资料
    /// </summary>
    [RootEntity, Serializable]
    [Label("项目号需求设计-工艺资料")]
    public class DesignProductTree : DesignProduct
    {
        #region 需求设计 ProjectDesign
        /// <summary>
        /// 需求设计Id
        /// </summary>
        [Label("需求设计")]
        public static readonly IRefIdProperty ProjectDesignIdProperty =
            P<DesignProductTree>.RegisterRefId(e => e.ProjectDesignId, ReferenceType.Normal);

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
            P<DesignProductTree>.RegisterRef(e => e.ProjectDesign, ProjectDesignIdProperty);

        /// <summary>
        /// 需求设计
        /// </summary>
        public ProjectDesignDetail ProjectDesign
        {
            get { return this.GetRefEntity(ProjectDesignProperty); }
            set { this.SetRefEntity(ProjectDesignProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignProductTreeConfig : EntityConfig<DesignProductTree>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRODES_PROTREE").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.SupportTree();
        }
    }
}
