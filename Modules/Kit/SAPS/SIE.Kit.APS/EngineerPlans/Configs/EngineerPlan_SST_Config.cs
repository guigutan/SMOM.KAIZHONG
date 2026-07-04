using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.APS.EngineerPlans.Configs
{
    /// <summary>
    /// 销售订单行过滤配置
    /// </summary>
    [System.ComponentModel.DisplayName("销售订单行过滤配置")]
    [System.ComponentModel.Description("定义需要过滤的销售订单行的时间区间范围")]
    public class EngineerPlan_SST_Config : ModuleConfig<EngineerPlan_SST_ConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EngineerPlan_SST_ConfigValue defaultValue = new EngineerPlan_SST_ConfigValue()
        {
            
        };

        /// <summary>
        /// 默认值
        /// </summary>
        public override EngineerPlan_SST_ConfigValue DefaultValue
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
                return "定义需要过滤的销售订单行的时间区间范围";
            }
        }
    }


    /// <summary>
    /// 定义需要过滤的销售订单行的时间区间范围 配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("定义需要过滤的销售订单行的时间区间范围")]
    public class EngineerPlan_SST_ConfigValue : ConfigValue
    {
        #region 开始登记日期 StartRegisterDateTime
        /// <summary>
        /// 开始登记日期
        /// </summary>
        [Label("开始登记日期")]
        public static readonly Property<DateTime> StartRegisterDateTimeProperty = P<EngineerPlan_SST_ConfigValue>.Register(e => e.StartRegisterDateTime);

        /// <summary>
        /// 开始登记日期
        /// </summary>
        public DateTime StartRegisterDateTime
        {
            get { return GetProperty(StartRegisterDateTimeProperty); }
            set { SetProperty(StartRegisterDateTimeProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示名称
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return StartRegisterDateTime.ToString("yyyy-MM-dd");
        }
    }

}

