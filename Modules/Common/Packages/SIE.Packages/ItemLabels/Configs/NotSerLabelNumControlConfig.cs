using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Packages.ItemLabels.Configs
{
    /// <summary>
    /// 是否控制非序列号标签生成数量
    /// </summary>
    [System.ComponentModel.DisplayName("是否控制非序列号标签生成数量")]
    [System.ComponentModel.Description("是否控制非序列号标签生成数量")]
    public class NotSerLabelNumControlConfig : GlobalConfig<NotSerLabelNumControlConfigValue>
    {
        /// <summary>
        /// 是否控制非序列号标签生成数量
        /// </summary>
        readonly NotSerLabelNumControlConfigValue defaultValue = new NotSerLabelNumControlConfigValue { IsControlCount = false };

        /// <summary>
        /// 获取默认值
        /// </summary>
        public override NotSerLabelNumControlConfigValue DefaultValue
        {
            get { return defaultValue; }
        }
    }

    /// <summary>
    /// 非序列号标签生成是否控制数量配置项
    /// </summary>
    [RootEntity, Serializable]
    [Label("非序列号标签生成是否控制数量")]
    public class NotSerLabelNumControlConfigValue : ConfigValue
    {
        #region 非序列号标签生成是否控制数量 IsControlCount
        /// <summary>
        /// 非序列号标签生成是否控制数量
        /// </summary>
        [Label("非序列号标签生成是否控制数量")]
        public static readonly Property<bool> IsControlCountProperty = P<NotSerLabelNumControlConfigValue>.Register(e => e.IsControlCount);

        /// <summary>
        /// 非序列号标签生成是否控制数量
        /// </summary>
        public bool IsControlCount
        {
            get { return this.GetProperty(IsControlCountProperty); }
            set { this.SetProperty(IsControlCountProperty, value); }
        }
        #endregion
    }
}
