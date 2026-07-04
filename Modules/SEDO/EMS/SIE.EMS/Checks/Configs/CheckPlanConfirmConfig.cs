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
    [System.ComponentModel.DisplayName("点检完成后，是否需要其他部门确认")]
    [System.ComponentModel.Description("点检完成后，是否需要其他部门确认，为‘否’则不需要部门确认")]
    public class CheckPlanConfirmConfig : ModuleConfig<CheckPlanConfirmConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CheckPlanConfirmConfigValue defaultValue = new CheckPlanConfirmConfigValue { IsNeedDepartmentConfirm = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckPlanConfirmConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否需要其他部门确认")]
    public class CheckPlanConfirmConfigValue : ConfigValue
    {
        #region 是否需要其他部门确认 IsNeedDepartmentConfirm
        /// <summary>
        /// 是否需要其他部门确认
        /// </summary>
        [Label("是否需要其他部门确认")]
        public static readonly Property<YesNo> IsNeedDepartmentConfirmProperty = P<CheckPlanConfirmConfigValue>.Register(e => e.IsNeedDepartmentConfirm);

        /// <summary>
        /// 是否需要其他部门确认
        /// </summary>
        public YesNo IsNeedDepartmentConfirm
        {
            get { return this.GetProperty(IsNeedDepartmentConfirmProperty); }
            set { this.SetProperty(IsNeedDepartmentConfirmProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsNeedDepartmentConfirm == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
