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
    [System.ComponentModel.DisplayName("点检时是否带出设备子项的检验项目")]
    [System.ComponentModel.Description("配置设备点检时是否带出设备子项的检验项目")]
    public class CheckChildProjectConfig : ModuleConfig<CheckChildProjectConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly CheckChildProjectConfigValue defaultValue = new CheckChildProjectConfigValue { IsBringChildCheckProject = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override CheckChildProjectConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否带出设备子项的检验项目")]
    public class CheckChildProjectConfigValue : ConfigValue
    {
        #region 是否带出设备子项的检验项目 IsBringChildCheckProject
        /// <summary>
        /// 是否带出设备子项的检验项目
        /// </summary>
        [Label("是否带出设备子项的检验项目")]
        public static readonly Property<YesNo> IsBringChildCheckProjectProperty = P<CheckChildProjectConfigValue>.Register(e => e.IsBringChildCheckProject);

        /// <summary>
        /// 是否带出设备子项的检验项目
        /// </summary>
        public YesNo IsBringChildCheckProject
        {
            get { return this.GetProperty(IsBringChildCheckProjectProperty); }
            set { this.SetProperty(IsBringChildCheckProjectProperty, value); }
        }
        #endregion


        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsBringChildCheckProject == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
