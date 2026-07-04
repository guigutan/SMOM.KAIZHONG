using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Kit.UrgentOrder.ItemUrgentOrders.Configs
{
    /// <summary>
    /// 配置需求时间和创建加急单时间，最小间隔时数(小时)
    /// </summary>
    [RootEntity, Serializable]
    [Label("配置需求时间和创建加急单时间，最小间隔时数(小时)")]
    public class ItemUrgentOrderDateConfigValue : ConfigValue
    {
        #region  Time
        /// <summary>
        /// 配置需求时间和创建加急单时间，最小间隔时数(小时)
        /// </summary>
        [Label("最小间隔时数(小时)")]
        public static readonly Property<double> HasValueProperty = P<ItemUrgentOrderDateConfigValue>.Register(e => e.Time);

        /// <summary>
        /// 配置需求时间和创建加急单时间，最小间隔时数(小时)
        /// </summary>
        public double Time
        {
            get { return this.GetProperty(HasValueProperty); }
            set { this.SetProperty(HasValueProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示名称
        /// </summary>
        /// <returns>编码规则名称</returns>
        public override string Display()
        {
            return Time.ToString();
        }
    }
}
