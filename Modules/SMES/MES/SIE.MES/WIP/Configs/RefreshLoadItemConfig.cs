using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Configs
{
    /// <summary>
    /// 自动刷新上料
    /// </summary>
    [System.ComponentModel.DisplayName("上料自动刷新")]
    [System.ComponentModel.Description("用于配置上料自动刷新的时间以秒为单位，0表示不自动刷新")]
    public class RefreshLoadItemConfig : ModuleCategoryConfig<ResourceStation, RefreshLoadItemConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override RefreshLoadItemConfigValue DefaultValue { get; } = new RefreshLoadItemConfigValue { };
    }

    /// <summary>
    /// 上料自动刷新
    /// </summary>
    [RootEntity, Serializable]
    [Label("自动刷新时间配置")]
    public class RefreshLoadItemConfigValue : ConfigValue
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RefreshLoadItemConfigValue()
        {
            this.RefreshTime = 0;
        }
        #endregion

        /// <summary>
        /// 刷新时间
        /// </summary>
        [MinValue(0)]
        [Label("刷新时间")]
        public static readonly Property<int> RefreshTimeProperty = P<RefreshLoadItemConfigValue>.Register(e => e.RefreshTime);

        /// <summary>
        ///  刷新时间
        /// </summary>
        public int RefreshTime
        {
            get { return this.GetProperty(RefreshTimeProperty); }
            set { this.SetProperty(RefreshTimeProperty, value); }
        }

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>刷新时间</returns>
        public override string Display()
        {
            return RefreshTime.ToString();
        }
    }
}
