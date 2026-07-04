using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 绩效等级配置
    /// </summary>
    [RootEntity, Serializable]
    ////[CriteriaQuery]
    [Label("绩效等级配置")]
    public partial class AchieveLevelSetting : DataEntity
    {
        #region 行号 RowIndex
        /// <summary>
        /// 行号
        /// </summary>
        [Label("行号")]
        public static readonly Property<int> RowIndexProperty = P<AchieveLevelSetting>.Register(e => e.RowIndex);

        /// <summary>
        /// 行号
        /// </summary>
        public int RowIndex
        {
            get { return this.GetProperty(RowIndexProperty); }
            set { this.SetProperty(RowIndexProperty, value); }
        }
        #endregion

        #region 最小值 MinValue
        /// <summary>
        /// 最小值
        /// </summary>
        [Label("最小值")]
        public static readonly Property<decimal?> MinValueProperty = P<AchieveLevelSetting>.Register(e => e.MinValue);

        /// <summary>
        /// 最小值
        /// </summary>
        public decimal? MinValue
        {
            get { return GetProperty(MinValueProperty); }
            set { SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 最大值 MaxValue
        /// <summary>
        /// 最大值
        /// </summary>
        [Label("最大值")]
        public static readonly Property<decimal?> MaxValueProperty = P<AchieveLevelSetting>.Register(e => e.MaxValue);

        /// <summary>
        /// 最大值
        /// </summary>
        public decimal? MaxValue
        {
            get { return GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 绩效等级 AchiLevel
        /// <summary>
        /// 绩效等级
        /// </summary>
        [Label("绩效等级")]
        public static readonly Property<AchieveLevel> AchiLevelProperty = P<AchieveLevelSetting>.Register(e => e.AchiLevel);

        /// <summary>
        /// 绩效等级
        /// </summary>
        public AchieveLevel AchiLevel
        {
            get { return GetProperty(AchiLevelProperty); }
            set { SetProperty(AchiLevelProperty, value); }
        }
        #endregion

        #region 运算符 Operator
        /// <summary>
        /// 运算符
        /// </summary>
        [Label("运算符")]
        public static readonly Property<Operator> OperatorProperty = P<AchieveLevelSetting>.Register(e => e.Operator);

        /// <summary>
        /// 运算符
        /// </summary>
        public Operator Operator
        {
            get { return GetProperty(OperatorProperty); }
            set { SetProperty(OperatorProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 绩效等级配置 实体配置
    /// </summary>
    internal class AchieveLevelSettingConfig : EntityConfig<AchieveLevelSetting>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WG_ACHEI_LEVEL_SET").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}