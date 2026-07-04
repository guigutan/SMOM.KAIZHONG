using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Common.Configs
{
    /// <summary>
    /// 数据列数量配置值
    /// </summary>
    [RootEntity, Serializable]
    [Label("数据列数量配置值")]
    public class ValueConfigValue : ConfigValue
    {
        /// <summary>
        /// 数据列数量
        /// </summary>
        [Label("数据列数量")]
        public static readonly Property<int?> QtyProperty = P<ValueConfigValue>.Register(e => e.Qty);

        /// <summary>
        /// 数据列数量
        /// </summary>
        public int? Qty
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
            if (Qty == null)
                return "1";
            return Qty?.ToString();
        }
    }
}
