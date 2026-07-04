using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.WIP.Configs;
using SIE.ObjectModel;
using System;

namespace SIE.Andon.Andons.Configs
{
    /// <summary>
    /// 安灯事件看板自动刷新时间配置
    /// </summary>
    [System.ComponentModel.DisplayName("安灯事件看板自动刷新时间配置")]
    [System.ComponentModel.Description("安灯事件看板自动刷新时间配置")]
    public class RefreshAndonManageConfig : ModuleConfig<RefreshAndonManageConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override RefreshAndonManageConfigValue DefaultValue { get; } = new RefreshAndonManageConfigValue { };
    }

    /// <summary>
    /// 安灯事件看板自动刷新时间配置的值
    /// </summary>
    [RootEntity, Serializable]
    [Label("安灯事件看板自动刷新时间配置的值")]
    public class RefreshAndonManageConfigValue : ConfigValue
    {
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public RefreshAndonManageConfigValue()
        {
            this.RefreshTime = 0;
        }
        #endregion

        //0表示不自动刷新

        /// <summary>
        /// 刷新时间
        /// </summary>
        [MinValue(0)]
        [Label("刷新时间")]
        public static readonly Property<int> RefreshTimeProperty = P<RefreshAndonManageConfigValue>.Register(e => e.RefreshTime);

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
