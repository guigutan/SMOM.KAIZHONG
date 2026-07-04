using SIE;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.APS.EngineerPlans.Configs
{
    /// <summary>
    /// 新单与外部Ecn比例配置
    /// </summary>
    [System.ComponentModel.DisplayName("未完成工程计划重算配置")]
    [System.ComponentModel.Description("定义未按时完成工程计划,是否占用下一天产能")]
    public class EngineerPlan_IsOverTimeTakeCapacity_Config : ModuleConfig<EngineerPlan_IsOverTimeTakeCapacity_ConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EngineerPlan_IsOverTimeTakeCapacity_ConfigValue defaultValue = new EngineerPlan_IsOverTimeTakeCapacity_ConfigValue()
        {
            IsYes = false
        };

        /// <summary>
        /// 默认值
        /// </summary>
        public override EngineerPlan_IsOverTimeTakeCapacity_ConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get
            {
                return "定义未按时完成工程计划,是否占用下一天产能";
            }
        }
    }


    /// <summary>
    /// 是否占用下一天产能 配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否占用下一天产能")]
    public class EngineerPlan_IsOverTimeTakeCapacity_ConfigValue : ConfigValue
    {
        #region 占用下一天产能 IsYes
        /// <summary>
        /// 占用下一天产能
        /// </summary>
        [Label("占用下一天产能")]
        public static readonly Property<bool> IsYesProperty = P<EngineerPlan_IsOverTimeTakeCapacity_ConfigValue>.Register(e => e.IsYes);

        /// <summary>
        /// 占用下一天产能
        /// </summary>
        public bool IsYes
        {
            get { return GetProperty(IsYesProperty); }
            set { SetProperty(IsYesProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示名称
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return IsYes == true ? "占用" : "不占";
        }
    }
}

