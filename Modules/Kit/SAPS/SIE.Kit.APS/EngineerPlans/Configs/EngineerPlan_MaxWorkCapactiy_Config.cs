using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.APS.EngineerPlans.Configs
{
    /// <summary>
    /// 主计划编码配置
    /// </summary>
    [System.ComponentModel.DisplayName("每日工程计划数配置")]
    [System.ComponentModel.Description("定义工程计划能力")]
    public class EngineerPlan_MaxWorkCapactiy_Config : ModuleConfig<EngineerPlan_MaxWorkCapactiy_ConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EngineerPlan_MaxWorkCapactiy_ConfigValue defaultValue = new EngineerPlan_MaxWorkCapactiy_ConfigValue();

        /// <summary>
        /// 默认值
        /// </summary>
        public override EngineerPlan_MaxWorkCapactiy_ConfigValue DefaultValue
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
                return "定义工程计划能力";
            }
        }
    }


    /// <summary>
    /// 每日总产能限制 配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("每日总产能限制")]
    public class EngineerPlan_MaxWorkCapactiy_ConfigValue : ConfigValue
    {
        #region 每日总产能限制 WorkCeil
        /// <summary>
        /// 每日工程计划数
        /// </summary>
        [Label("每日总产能限制")]
        public static readonly Property<decimal> WorkCeilProperty = P<EngineerPlan_MaxWorkCapactiy_ConfigValue>.Register(e => e.WorkCeil);

        /// <summary>
        /// 每日总产能限制
        /// </summary>
        public decimal WorkCeil
        {
            get { return GetProperty(WorkCeilProperty); }
            set { SetProperty(WorkCeilProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示名称
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return WorkCeil.ToString();
        }
    }

}

