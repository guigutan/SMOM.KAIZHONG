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
    [System.ComponentModel.DisplayName("配置是否按部门进行点检")]
    [System.ComponentModel.Description("配置是否按部门进行点检，点击是，则按部门点检")]
    public class CheckDepartmentConfig : ModuleConfig<CheckDepartmentConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CheckDepartmentConfigValue defaultValue = new CheckDepartmentConfigValue { IsCheckForDepartment = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckDepartmentConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否按部门进行点检")]
    public class CheckDepartmentConfigValue : ConfigValue
    {
        #region 是否按部门进行点检 IsCheckForDepartment
        /// <summary>
        /// 是否按部门进行点检
        /// </summary>
        [Label("是否按部门进行点检")]
        public static readonly Property<YesNo> IsCheckForDepartmentProperty = P<CheckDepartmentConfigValue>.Register(e => e.IsCheckForDepartment);

        /// <summary>
        /// 是否按部门进行点检
        /// </summary>
        public YesNo IsCheckForDepartment
        {
            get { return this.GetProperty(IsCheckForDepartmentProperty); }
            set { this.SetProperty(IsCheckForDepartmentProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsCheckForDepartment == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
