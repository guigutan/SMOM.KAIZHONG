using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 预警设定
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(TargetWarnSettingCriteria))]
    [Label("预警设定")]
    public partial class TargetWarnSetting : DataEntity
    {
        #region 指标分类 Code
        /// <summary>
        /// 指标分类
        /// </summary>
        [Label("指标分类")]
        [Required]
        public static readonly Property<string> CodeProperty = P<TargetWarnSetting>.Register(e => e.Code);

        /// <summary>
        /// 指标分类
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 指标名称 Name
        /// <summary>
        /// 指标名称
        /// </summary>
        [Label("指标名称")]
        [Required]
        public static readonly Property<string> NameProperty = P<TargetWarnSetting>.Register(e => e.Name);

        /// <summary>
        /// 指标名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 达成率区间列表 TargetWarnDetailList
        /// <summary>
        /// 达成率区间列表
        /// </summary>
        public static readonly ListProperty<EntityList<TargetWarnDetail>> TargetWarnDetailListProperty = P<TargetWarnSetting>.RegisterList(e => e.TargetWarnDetailList);

        /// <summary>
        /// 达成率区间列表
        /// </summary>
        public EntityList<TargetWarnDetail> TargetWarnDetailList
        {
            get { return this.GetLazyList(TargetWarnDetailListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 预警设定 实体配置
    /// </summary>
    internal class TargetWarnSettingConfig : EntityConfig<TargetWarnSetting>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TARGET_WARN_SET").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}