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
    [System.ComponentModel.DisplayName("配置工时登记是否为必填")]
    [System.ComponentModel.Description("配置工时登记是否为必填，默认为否")]
    public class MaintainWorkTimeConfig : ModuleConfig<MaintainWorkTimeConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        readonly MaintainWorkTimeConfigValue defaultValue = new MaintainWorkTimeConfigValue { IsMaintainForWorkTime = YesNo.No };

        /// <summary>
        /// 默认值
        /// </summary>
        public override MaintainWorkTimeConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("工时登记是否必填")]
    public class MaintainWorkTimeConfigValue : ConfigValue
    {
        #region 工时登记是否必填 IsMaintainForWorkTime
        /// <summary>
        /// 工时登记是否必填
        /// </summary>
        [Label("工时登记是否必填")]
        public static readonly Property<YesNo> IsMaintainForWorkTimeProperty = P<MaintainWorkTimeConfigValue>.Register(e => e.IsMaintainForWorkTime);

        /// <summary>
        /// 工时登记是否必填
        /// </summary>
        public YesNo IsMaintainForWorkTime
        {
            get { return this.GetProperty(IsMaintainForWorkTimeProperty); }
            set { this.SetProperty(IsMaintainForWorkTimeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            string display = IsMaintainForWorkTime == YesNo.Yes ? "是".L10N() : "否".L10N();
            return display;
        }
    }
}
