using SIE.Common.Configs;
using SIE.Domain;
using SIE.EMS.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Lubrications.Configs
{
    /// <summary>
    /// 数据列数量配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("计划规则")]
   public  class PlanTypeConfigValue : ConfigValue
    {
        #region 计划规则 PlanType
        /// <summary>
        /// 计划规则
        /// </summary>
        [Label("计划规则")]
        public static readonly Property<PlanType?> PlanTypeProperty = P<PlanTypeConfigValue>.Register(e => e.PlanType);

        /// <summary>
        /// 计划规则
        /// </summary>
        public PlanType? PlanType
        {
            get { return this.GetProperty(PlanTypeProperty); }
            set { this.SetProperty(PlanTypeProperty, value); }
        }
        #endregion
    }
}
