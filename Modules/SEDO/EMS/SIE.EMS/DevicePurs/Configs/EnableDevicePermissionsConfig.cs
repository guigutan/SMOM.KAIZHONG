using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.EMS.DevicePurs.Configs
{
    /// <summary>
    /// 启用设备权限管理配置项
    /// </summary>
    [DisplayName("启用设备权限管理配置项")]
    [Description("启用设备权限管理配置项")]
    public class EnableDevicePermissionsConfig : ModuleConfig<EnableDevicePermissionsConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly EnableDevicePermissionsConfigValue defaultValue = new EnableDevicePermissionsConfigValue
        {
            EnableDevicePermissions = YesNo.No
        };

        /// <summary>
        /// 默认值
        /// </summary>
        public override EnableDevicePermissionsConfigValue DefaultValue
        {
            get
            {
                return defaultValue;
            }
        }
    }

    /// <summary>
    /// 启用设备权限管理配置项的值
    /// </summary>
    [RootEntity, Serializable]
    [Label("启用设备权限管理配置项的值")]
    public class EnableDevicePermissionsConfigValue : ConfigValue
    {
        #region 启用设备权限管理 EnableDevicePermissions
        /// <summary>
        /// 启用设备权限管理
        /// </summary>
        [Label("启用设备权限管理")]
        public static readonly Property<YesNo?> EnableDevicePermissionsPProperty
            = P<EnableDevicePermissionsConfigValue>.Register(e => e.EnableDevicePermissions);

        /// <summary>
        /// 启用设备权限管理
        /// </summary>
        public YesNo? EnableDevicePermissions
        {
            get { return this.GetProperty(EnableDevicePermissionsPProperty); }
            set { this.SetProperty(EnableDevicePermissionsPProperty, value); }
        }
        #endregion


        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>启用设备权限管理</returns>
        public override string Display()
        {
            if (EnableDevicePermissions == null)
            {
                return "否".L10N();
            }

            return EnableDevicePermissions.ToLabel().L10N();
        }
    }
}
