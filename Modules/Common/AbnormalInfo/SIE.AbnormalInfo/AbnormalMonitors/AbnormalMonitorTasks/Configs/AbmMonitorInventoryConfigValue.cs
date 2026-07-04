using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors.Configs
{
	/// <summary>
	/// 自动生成异常任务配置
	/// </summary>
	[RootEntity, Serializable]
    [Label("自动生成异常任务配置")]
    public class AbmMonitorInventoryConfigValue : ConfigValue
    {

        #region 任务类型 TaskType
        /// <summary>
        /// 自动生成异常任务
        /// </summary>
        [Label("自动生成异常任务")]
        public static readonly Property<bool> AutoTaskProperty = P<AbmMonitorInventoryConfigValue>.Register(e => e.AutoTask);

        /// <summary>
        /// 自动生成异常任务
        /// </summary>
        public bool AutoTask
        {
            get { return GetProperty(AutoTaskProperty); }
            set { SetProperty(AutoTaskProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return AutoTask?"True":"False";
        }
    }
}
