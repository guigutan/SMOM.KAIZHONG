using SIE.Common.Configs;
using SIE.Domain;
using SIE.ESop.Displays;
using SIE.ObjectModel;
using System;

namespace SIE.ESop.Configs
{
    /// <summary>
    /// 显示点配置实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("显示点配置")]
    [DisplayMember(nameof(DisplayPointConfigValue.Id))]
    public class DisplayPointConfigValue : ConfigValue
    {
        #region 播放间隔(秒) Interval
        /// <summary>
        /// 播放间隔(秒)
        /// </summary>
        [MinValue(10)]
        [Label("播放间隔(秒)")]
        public static readonly Property<int> IntervalProperty = P<DisplayPointConfigValue>.Register(e => e.Interval);

        /// <summary>
        /// 播放间隔秒数
        /// </summary> 
        public int Interval
        {
            get { return this.GetProperty(IntervalProperty); }
            set { this.SetProperty(IntervalProperty, value); }
        }
        #endregion

        #region 重试次数 ReTryCount
        /// <summary>
        /// 重试次数
        /// </summary>
        [MinValue(0)]
        [Label("重试次数")]
        public static readonly Property<int> ReTryCountProperty = P<DisplayPointConfigValue>.Register(e => e.ReTryCount);

        /// <summary>
        /// 重试次数
        /// </summary>
        public int ReTryCount
        {
            get { return this.GetProperty(ReTryCountProperty); }
            set { this.SetProperty(ReTryCountProperty, value); }
        }
        #endregion

        #region 组合键 ModifierKeys
        /// <summary>
        /// 组合键
        /// </summary>
        [Label("组合键")]
        public static readonly Property<ModifierKeys> ModifierKeysProperty = P<DisplayPointConfigValue>.Register(e => e.ModifierKeys);

        /// <summary>
        /// 组合键
        /// </summary>
        public ModifierKeys ModifierKeys
        {
            get { return this.GetProperty(ModifierKeysProperty); }
            set { this.SetProperty(ModifierKeysProperty, value); }
        }
        #endregion

        #region 热键 Key
        /// <summary>
        /// 热键
        /// </summary>
        [Label("热键")]
        public static readonly Property<Key> KeyProperty = P<DisplayPointConfigValue>.Register(e => e.Key);

        /// <summary>
        /// 热键
        /// </summary>
        public Key Key
        {
            get { return this.GetProperty(KeyProperty); }
            set { this.SetProperty(KeyProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns>返回显示内容</returns>
        public override string Display()
        {
            return "间隔{0}秒  重试{1}次数".L10nFormat(Interval, ReTryCount);
        }
    }
}