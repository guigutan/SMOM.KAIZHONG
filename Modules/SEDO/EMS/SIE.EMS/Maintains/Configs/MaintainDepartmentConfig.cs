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
    [System.ComponentModel.DisplayName("配置是否按部门进行保养")]
    [System.ComponentModel.Description("配置是否按部门进行保养，点击是，则按部门保养")]
    public class MaintainDepartmentConfig : ModuleConfig<MaintainDepartmentConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaintainDepartmentConfigValue defaultValue = new MaintainDepartmentConfigValue { IsMaintainForDepartment = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaintainDepartmentConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否按部门进行保养")]
    public class MaintainDepartmentConfigValue : ConfigValue
    {
        #region 是否按部门进行保养 IsMaintainForDepartment
        /// <summary>
        /// 是否按部门进行保养
        /// </summary>
        [Label("是否按部门进行保养")]
        public static readonly Property<YesNo> IsMaintainForDepartmentProperty = P<MaintainDepartmentConfigValue>.Register(e => e.IsMaintainForDepartment);

        /// <summary>
        /// 是否按部门进行保养
        /// </summary>
        public YesNo IsMaintainForDepartment
        {
            get { return this.GetProperty(IsMaintainForDepartmentProperty); }
            set { this.SetProperty(IsMaintainForDepartmentProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsMaintainForDepartment == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
