using SIE.Common.Configs;
using SIE.Domain;
using SIE.LES.MaterialReceptions.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.MaterialReceptions.Configs
{
    /// <summary>
    /// 
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料接收按单接收默认数量")]
    public class MaterialReceptionQtyConfigValue : ConfigValue
    {
        #region 按单接收数量 OrderQty
        /// <summary>
        /// 按单接收数量
        /// </summary>
        [Label("按单接收数量")]
        public static readonly Property<ConfigValues> OrderQtyProperty = P<MaterialReceptionQtyConfigValue>.Register(e => e.OrderQty);

        /// <summary>
        /// 按单接收数量
        /// </summary>
        public ConfigValues OrderQty
        {
            get { return this.GetProperty(OrderQtyProperty); }
            set { this.SetProperty(OrderQtyProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return OrderQty.ToLabel().L10N();
        }
    }
}
