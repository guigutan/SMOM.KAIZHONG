using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.EquipmentSetups.ViewModels
{
    /// <summary>
    /// 安装调试备件使用条码
    /// </summary>
    [RootEntity, Serializable]
    [Label("安装调试备件使用条码")]
    [DisplayMember(nameof(Value))]
    public class SetupLotSnInfo : ViewModel
    {
        #region 条码 Value
        /// <summary>
        /// 条码
        /// </summary>
        public static readonly Property<string> ValueProperty = P<SetupLotSnInfo>.Register(e => e.Value);

        /// <summary>
        /// 条码
        /// </summary>
        public string Value
        {
            get { return GetProperty(ValueProperty); }
            set { SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<SetupLotSnInfo>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion
    }
}
