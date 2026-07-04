using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.RedCardManagment.RedCards.Config
{
    /// <summary>
    /// 信息追溯天数配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("信息追溯天数配置值")]
    public class SyncDaysConfigValue : ConfigValue
    {
        /// <summary>
        /// 追溯天数
        /// </summary>
        [Label("追溯天数")]
        public static readonly Property<int?> QtyProperty = P<SyncDaysConfigValue>.Register(e => e.Days);

        /// <summary>
        /// 追溯天数
        /// </summary>
        public int? Days
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }

        /// <summary>
        /// 把值显示出来
        /// </summary>
        /// <returns>string</returns>
        public override string Display()
        {
            return Days?.ToString();
        }
    }
}
