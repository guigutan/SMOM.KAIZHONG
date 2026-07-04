using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCards.Config
{
    /// <summary>
    /// 自动生成异常任务配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("自动生成异常任务配置值")]
    public class GeneralAbnormalTaskConfigValue : ConfigValue
    {
        /// <summary>
        /// 自动生成异常任务
        /// </summary>
        [Label("自动生成异常任务")]
        public static readonly Property<bool> IsAutoProperty = P<GeneralAbnormalTaskConfigValue>.Register(e => e.IsAuto);

        /// <summary>
        /// 自动生成异常任务
        /// </summary>
        public bool IsAuto
        {
            get { return this.GetProperty(IsAutoProperty); }
            set { this.SetProperty(IsAutoProperty, value); }
        }

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return IsAuto?"True":"False";
        }
    }
}
