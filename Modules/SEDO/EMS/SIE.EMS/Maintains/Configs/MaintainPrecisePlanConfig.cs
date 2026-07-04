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
    [System.ComponentModel.DisplayName("配置是否按精确计划生成")]
    [System.ComponentModel.Description("配置是否按精确计划生成，默认为‘否’,为‘是’时，添加计划时必填指定计划开始时间、指定计划结束时间")]
    public class MaintainPrecisePlanConfig : ModuleConfig<MaintainPrecisePlanConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaintainPrecisePlanConfigValue defaultValue = new MaintainPrecisePlanConfigValue { IsMaintainForPrecisePlan = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaintainPrecisePlanConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否按精确计划生成")]
    public class MaintainPrecisePlanConfigValue : ConfigValue
    {
        #region 是否按精确计划生成 IsMaintainForPrecisePlan
        /// <summary>
        /// 是否按精确计划生成
        /// </summary>
        [Label("是否按精确计划生成")]
        public static readonly Property<YesNo> IsMaintainForPrecisePlanProperty = P<MaintainPrecisePlanConfigValue>.Register(e => e.IsMaintainForPrecisePlan);

        /// <summary>
        /// 是否按精确计划生成
        /// </summary>
        public YesNo IsMaintainForPrecisePlan
        {
            get { return this.GetProperty(IsMaintainForPrecisePlanProperty); }
            set { this.SetProperty(IsMaintainForPrecisePlanProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsMaintainForPrecisePlan == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
