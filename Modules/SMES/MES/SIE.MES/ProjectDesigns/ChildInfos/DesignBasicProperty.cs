using SIE.Domain;
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
    /// 项目号需求设计-基本属性
    /// </summary>
    [RootEntity, Serializable]
    [Label("项目号需求设计-基本属性")]
    public class DesignBasicProperty : DataEntity
    {
        #region 需求设计 ProjectDesign
        /// <summary>
        /// 需求设计Id
        /// </summary>
        [Label("需求设计")]
        public static readonly IRefIdProperty ProjectDesignIdProperty =
            P<DesignBasicProperty>.RegisterRefId(e => e.ProjectDesignId, ReferenceType.Normal);

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
            P<DesignBasicProperty>.RegisterRef(e => e.ProjectDesign, ProjectDesignIdProperty);

        /// <summary>
        /// 需求设计
        /// </summary>
        public ProjectDesignDetail ProjectDesign
        {
            get { return this.GetRefEntity(ProjectDesignProperty); }
            set { this.SetRefEntity(ProjectDesignProperty, value); }
        }
        #endregion

        #region 属性 BasicProperty
        /// <summary>
        /// 属性
        /// </summary>
        [Label("属性")]
        public static readonly Property<string> BasicPropertyProperty = P<DesignBasicProperty>.Register(e => e.BasicProperty);

        /// <summary>
        /// 属性
        /// </summary>
        public string BasicProperty
        {
            get { return this.GetProperty(BasicPropertyProperty); }
            set { this.SetProperty(BasicPropertyProperty, value); }
        }
        #endregion

        #region 属性值 BasicProValue
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> BasicProValueProperty = P<DesignBasicProperty>.Register(e => e.BasicProValue);

        /// <summary>
        /// 属性值
        /// </summary>
        public string BasicProValue
        {
            get { return this.GetProperty(BasicProValueProperty); }
            set { this.SetProperty(BasicProValueProperty, value); }
        }
        #endregion

        #region 单位 BasicProUnit
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> BasicProUnitProperty = P<DesignBasicProperty>.Register(e => e.BasicProUnit);

        /// <summary>
        /// 单位
        /// </summary>
        public string BasicProUnit
        {
            get { return this.GetProperty(BasicProUnitProperty); }
            set { this.SetProperty(BasicProUnitProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class DesignBasicPropertyConfig : EntityConfig<DesignBasicProperty>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PRODES_PROP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
