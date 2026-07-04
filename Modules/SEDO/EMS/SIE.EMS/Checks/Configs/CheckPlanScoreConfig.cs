using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Checks.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否需要进行点检评分")]
    [System.ComponentModel.Description("是否需要进行点检评分，为‘是’进行点检确认时带出TABLE页评分项")]
    public class CheckPlanScoreConfig : ModuleConfig<CheckPlanScoreConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CheckPlanScoreConfigValue defaultValue = new CheckPlanScoreConfigValue { IsNeedCheckScore = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckPlanScoreConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否需要进行点检评分")]
    public class CheckPlanScoreConfigValue : ConfigValue
    {
        #region 是否需要进行点检评分 IsNeedCheckScore
        /// <summary>
        /// 是否需要进行点检评分
        /// </summary>
        [Label("是否需要进行点检评分")]
        public static readonly Property<YesNo> IsNeedCheckScoreProperty = P<CheckPlanScoreConfigValue>.Register(e => e.IsNeedCheckScore);

        /// <summary>
        /// 是否需要进行点检评分
        /// </summary>
        public YesNo IsNeedCheckScore
        {
            get { return this.GetProperty(IsNeedCheckScoreProperty); }
            set { this.SetProperty(IsNeedCheckScoreProperty, value); }
        }
        #endregion



        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsNeedCheckScore == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
