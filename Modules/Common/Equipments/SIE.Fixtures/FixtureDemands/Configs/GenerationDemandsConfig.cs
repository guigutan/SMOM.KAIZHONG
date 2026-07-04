using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Fixtures.FixtureDemands.Configs
{
    /// <summary>
    /// 工治具需求清单工单自动生成配置项
    /// </summary>
    [System.ComponentModel.DisplayName("工治具需求清单工单自动生成配置项")]
    [System.ComponentModel.Description("工治具需求清单工单自动生成配置项")]
    public class GenerationDemandsConfig : ModuleConfig<GenerationDemandsConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly GenerationDemandsConfigValue defaultValue = new GenerationDemandsConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override GenerationDemandsConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 工治具需求单配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("工治具需求清单工单自动生成配置项")]
    public class GenerationDemandsConfigValue : ConfigValue
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GenerationDemandsConfigValue()
        {
            this.AutomaticGeneration = false;
        }
        #region 前置时间 Leadtime
        /// <summary>
        /// 前置时间
        /// </summary>
        [Label("前置时间(小时)")]
        public static readonly Property<decimal> LeadtimeProperty = P<GenerationDemandsConfigValue>.Register(e => e.Leadtime);

        /// <summary>
        /// 前置时间
        /// </summary>
        public decimal Leadtime
        {
            get { return this.GetProperty(LeadtimeProperty); }
            set { this.SetProperty(LeadtimeProperty, value); }
        }
        #endregion

        #region 工单自动生成工治具需求单 AutomaticGeneration
        /// <summary>
        /// 工单自动生产工治具需求单
        /// </summary>
        [Label("工单自动生成工治具需求单")]
        public static readonly Property<bool> AutomaticGenerationProperty = P<GenerationDemandsConfigValue>.Register(e => e.AutomaticGeneration);

        /// <summary>
        /// 工单自动生成工治具需求单
        /// </summary>
        public bool AutomaticGeneration
        {
            get { return this.GetProperty(AutomaticGenerationProperty); }
            set { this.SetProperty(AutomaticGenerationProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return "是否工单自动生产工治具需求单:{0}".L10nFormat(this.AutomaticGeneration ? "yes" : "no");
        }
    }
}
