using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Common.Configs
{
    /// <summary>
    /// 配置
    /// </summary>
    [System.ComponentModel.DisplayName("是否按部门进行点检和保养")]
    [System.ComponentModel.Description("是否按部门进行点检和保养，点击是，则按部门点检和保养")]
    public class IsDepartmentPlanConfig : GlobalConfig<IsDepartmentPlanConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly IsDepartmentPlanConfigValue defaultValue = new IsDepartmentPlanConfigValue { IsDepartmentPlan = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override IsDepartmentPlanConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否按部门进行点检")]
    public class IsDepartmentPlanConfigValue : ConfigValue
    {
        #region 是否按部门进行点检 IsDepartmentPlan
        /// <summary>
        /// 是否按部门进行点检
        /// </summary>
        [Label("是否")]
        public static readonly Property<YesNo> IsDepartmentPlanProperty = P<IsDepartmentPlanConfigValue>.Register(e => e.IsDepartmentPlan);

        /// <summary>
        /// 是否按部门进行点检
        /// </summary>
        public YesNo IsDepartmentPlan
        {
            get { return this.GetProperty(IsDepartmentPlanProperty); }
            set { this.SetProperty(IsDepartmentPlanProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsDepartmentPlan == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
