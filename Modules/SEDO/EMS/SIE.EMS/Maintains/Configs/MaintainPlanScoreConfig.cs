using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否需要进行保养评分")]
    [System.ComponentModel.Description("是否需要进行保养评分，为‘是’进行保养确认时带出TABLE页评分项")]
    public class MaintainPlanScoreConfig : ModuleConfig<MaintainPlanScoreConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaintainPlanScoreConfigValue defaultValue = new MaintainPlanScoreConfigValue { IsNeedMaintainScore = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaintainPlanScoreConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否需要进行保养评分")]
    public class MaintainPlanScoreConfigValue : ConfigValue
    {
        #region 是否需要进行保养评分 IsNeedMaintainScore
        /// <summary>
        /// 是否需要进行保养评分
        /// </summary>
        [Label("是否需要进行保养评分")]
        public static readonly Property<YesNo> IsNeedMaintainScoreProperty = P<MaintainPlanScoreConfigValue>.Register(e => e.IsNeedMaintainScore);

        /// <summary>
        /// 是否需要进行保养评分
        /// </summary>
        public YesNo IsNeedMaintainScore
        {
            get { return this.GetProperty(IsNeedMaintainScoreProperty); }
            set { this.SetProperty(IsNeedMaintainScoreProperty, value); }
        }
        #endregion



        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsNeedMaintainScore == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
