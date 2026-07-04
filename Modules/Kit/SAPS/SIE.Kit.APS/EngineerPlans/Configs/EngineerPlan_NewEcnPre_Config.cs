using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.APS.EngineerPlans.Configs
{
    /// <summary>
    /// 定义新单与外部ECN单换算比关系
    /// </summary>
    [System.ComponentModel.DisplayName("定义新单与外部ECN单换算比关系")]
    [System.ComponentModel.Description("定义新单与外部ECN单换算比关系")]
    public class EngineerPlan_NewEcnPre_Config : ModuleConfig<EngineerPlan_NewEcnPre_ConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EngineerPlan_NewEcnPre_ConfigValue defaultValue = new EngineerPlan_NewEcnPre_ConfigValue()
        {
            WithEcnPrecent = 1
        };

        /// <summary>
        /// 默认值
        /// </summary>
        public override EngineerPlan_NewEcnPre_ConfigValue DefaultValue
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
                return "定义新单与外部ECN单换算比关系";
            }
        }
    }


    /// <summary>
    /// 新单与外部Ecn比例配置 配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("新单与外部Ecn比例配置")]
    public class EngineerPlan_NewEcnPre_ConfigValue : ConfigValue
    {
        #region 新单与外部Ecn比例配置 WithEcnPrecent
        /// <summary>
        /// 新单与外部Ecn比例配置
        /// </summary>
        [Label("新单与外部Ecn比例配置")]
        public static readonly Property<decimal> WithEcnPrecentProperty = P<EngineerPlan_NewEcnPre_ConfigValue>.Register(e => e.WithEcnPrecent);

        /// <summary>
        /// 新单与外部Ecn比例配置
        /// </summary>
        public decimal WithEcnPrecent
        {
            get { return GetProperty(WithEcnPrecentProperty); }
            set { SetProperty(WithEcnPrecentProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示名称
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return WithEcnPrecent.ToString();
        }
    }

}

