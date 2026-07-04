using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.ComponentModel;

namespace SIE.MES.BatchWIP.Configs
{
    /// <summary>
    /// 载具解绑配置
    /// </summary>
    [DisplayName("载具解绑条件")]
    [Description("设置批次采集载具解绑条件")]
    public class ContainerUnbindConfig : GlobalConfig<ContainerUnbindConfigValue>
    {
        /// <summary>
        /// 默认值
        /// </summary>
        public override ContainerUnbindConfigValue DefaultValue
        {
            get
            {
                return new ContainerUnbindConfigValue() { UnbindMode = UnbindMode.MoveIn };
            }
        }
    }

    /// <summary>
    /// 批次采集载具解绑方式配置值
    /// </summary>
    [RootEntity, Serializable]
    public class ContainerUnbindConfigValue : ConfigValue
    {
        #region 解绑方式 UnbindMode
        /// <summary>
        /// 解绑方式
        /// </summary>
        [Label("解绑方式")]
        public static readonly Property<UnbindMode> UnbindModeProperty = P<ContainerUnbindConfigValue>.Register(e => e.UnbindMode);

        /// <summary>
        /// 解绑方式
        /// </summary>
        public UnbindMode UnbindMode
        {
            get { return this.GetProperty(UnbindModeProperty); }
            set { this.SetProperty(UnbindModeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示名称
        /// </summary>
        /// <returns>解绑方式</returns>
        public override string Display()
        {
            return UnbindMode.ToLabel().L10N();
        }
    }

    /// <summary>
    /// 载具解绑方式
    /// </summary>
    public enum UnbindMode
    {
        /// <summary>
        /// 转入后解绑
        /// </summary>
        [Label("转入后解绑")]
        MoveIn,

        /// <summary>
        /// 转出后解绑
        /// </summary>
        [Label("转出后解绑")]
        MoveOut
    }
}