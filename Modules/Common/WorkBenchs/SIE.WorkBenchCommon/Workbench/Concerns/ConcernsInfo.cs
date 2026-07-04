using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.WorkBenchCommon.Workbench.Concerns
{
    /// <summary>
    /// 关注信息
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("关注信息")]
    public partial class ConcernsInfo : DataEntity
    {
        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<ConcernsInfo>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<string> TypeProperty = P<ConcernsInfo>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public string Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 参数 Arguments
        /// <summary>
        /// 参数
        /// </summary>
        [Label("参数")]
        public static readonly Property<string> ArgumentsProperty = P<ConcernsInfo>.Register(e => e.Arguments);

        /// <summary>
        /// 参数
        /// </summary>
        public string Arguments
        {
            get { return GetProperty(ArgumentsProperty); }
            set { SetProperty(ArgumentsProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 关注信息 实体配置
    /// </summary>
    internal class ConcernsInfoConfig : EntityConfig<ConcernsInfo>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WB_CONCERNS").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}