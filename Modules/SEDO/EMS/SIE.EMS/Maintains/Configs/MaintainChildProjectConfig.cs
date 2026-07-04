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
    [System.ComponentModel.DisplayName("保养时是否带出设备子项的检验项目")]
    [System.ComponentModel.Description("配置设备保养时是否带出设备子项的检验项目")]
    public class MaintainChildProjectConfig : ModuleConfig<MaintainChildProjectConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaintainChildProjectConfigValue defaultValue = new MaintainChildProjectConfigValue { IsBringChildMaintainProject = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaintainChildProjectConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("是否带出设备子项的检验项目")]
    public class MaintainChildProjectConfigValue : ConfigValue
    {
        #region 是否带出设备子项的检验项目 IsBringChildMaintainProject
        /// <summary>
        /// 是否带出设备子项的检验项目
        /// </summary>
        [Label("是否带出设备子项的检验项目")]
        public static readonly Property<YesNo> IsBringChildMaintainProjectProperty = P<MaintainChildProjectConfigValue>.Register(e => e.IsBringChildMaintainProject);

        /// <summary>
        /// 是否带出设备子项的检验项目
        /// </summary>
        public YesNo IsBringChildMaintainProject
        {
            get { return this.GetProperty(IsBringChildMaintainProjectProperty); }
            set { this.SetProperty(IsBringChildMaintainProjectProperty, value); }
        }
        #endregion


        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsBringChildMaintainProject == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
