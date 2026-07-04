using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.PrepareProducts.Configs;
using SIE.MES.PrepareProducts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 产前准备项目维护实体
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(PrepareProjectCodeConfig))]
    [ConditionQueryType(typeof(PrepareProjectCriteria))]
    [DisplayMember(nameof(ProCode))]
    [Label("产前准备项目维护")]
    public class PrepareProject : DataEntity
    {
        #region 项目编码 ProCode
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> ProCodeProperty = P<PrepareProject>.Register(e => e.ProCode);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProCode
        {
            get { return this.GetProperty(ProCodeProperty); }
            set { this.SetProperty(ProCodeProperty, value); }
        }
        #endregion

        #region 项目名称 ProName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProNameProperty = P<PrepareProject>.Register(e => e.ProName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProName
        {
            get { return this.GetProperty(ProNameProperty); }
            set { this.SetProperty(ProNameProperty, value); }
        }
        #endregion

        #region 项目类型 ProType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<PrepareProjectType> ProTypeProperty = P<PrepareProject>.Register(e => e.ProType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public PrepareProjectType ProType
        {
            get { return this.GetProperty(ProTypeProperty); }
            set { this.SetProperty(ProTypeProperty, value); }
        }
        #endregion

        #region 项目描述 ProDesc
        /// <summary>
        /// 项目描述
        /// </summary>
        [Label("项目描述")]
        [MaxLength(200)]
        public static readonly Property<string> ProDescProperty = P<PrepareProject>.Register(e => e.ProDesc);

        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProDesc
        {
            get { return this.GetProperty(ProDescProperty); }
            set { this.SetProperty(ProDescProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产前准备项目维护数据配置
    /// </summary>
    public class PrepareProjectConfig : EntityConfig<PrepareProject>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PREPRO").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.Property(PrepareProject.ProDescProperty).ColumnMeta.HasLength(400);

        }
    }
}
