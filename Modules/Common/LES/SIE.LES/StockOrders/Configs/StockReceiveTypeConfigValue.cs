using SIE.Common.Configs;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Text;

namespace SIE.LES.StockOrders.Configs
{
    /// <summary>
    /// 备料单APP接收方式
    /// </summary>
    [RootEntity, Serializable]
    [Label("接收方式的默认值")]
    public class StockReceiveTypeConfigValue : ConfigValue
    {
        #region 接收方式 ReceiveType
        /// <summary>
        /// 接收方式
        /// </summary>
        [Label("接收方式")]
        public static readonly Property<StockReceiveType> ReceiveTypeProperty = P<StockReceiveTypeConfigValue>.Register(e => e.ReceiveType);

        /// <summary>
        /// 接收方式
        /// </summary>
        public StockReceiveType ReceiveType
        {
            get { return this.GetProperty(ReceiveTypeProperty); }
            set { this.SetProperty(ReceiveTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示值
        /// </summary>
        /// <returns></returns>
        public override string Display()
        {
            return ReceiveType.ToLabel().L10N();
        }
    }
}
