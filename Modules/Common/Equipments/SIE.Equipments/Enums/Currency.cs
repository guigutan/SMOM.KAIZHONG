using SIE.ObjectModel;

namespace SIE.Equipments.Enums
{
    /// <summary>
    /// 币种
    /// </summary>
    public enum Currency
    {
        /// <summary>
        /// 人民币
        /// </summary>
        [Label("人民币")]
        CNY = 5,
        /// <summary>
        /// 美元
        /// </summary>
        [Label("美元")]
        USD = 10,
        /// <summary>
        /// 港币
        /// </summary>
        [Label("港币")]
        HKD = 15,
        /// <summary>
        /// 台币
        /// </summary>
        [Label("台币")]
        TWD = 20,
        /// <summary>
        /// 日元
        /// </summary>
        [Label("日元")]
        JPY = 25,
        /// <summary>
        /// 欧元
        /// </summary>
        [Label("欧元")]
        EUR = 30,
        /// <summary>
        /// 英镑
        /// </summary>
        [Label("英镑")]
        GBP = 35,
        /// <summary>
        /// 泰币
        /// </summary>
        [Label("泰币")]
        THB = 40,
    }

    /// <summary>
    /// 币种对应的金额单位
    /// </summary>
    public enum AmountUnit
    {
        /// <summary>
        /// 元
        /// </summary>
        [Label("元")]
        CNY = 5,
        /// <summary>
        /// 美元
        /// </summary>
        [Label("美元")]
        USD = 10,
        /// <summary>
        /// 元
        /// </summary>
        [Label("元")]
        HKD = 15,
        /// <summary>
        /// 圆
        /// </summary>
        [Label("圆")]
        TWD = 20,
        /// <summary>
        /// 円
        /// </summary>
        [Label("円")]
        JPY = 25,
        /// <summary>
        /// 欧元
        /// </summary>
        [Label("欧元")]
        EUR = 30,
        /// <summary>
        /// 磅
        /// </summary>
        [Label("磅")]
        GBP = 35,
        /// <summary>
        /// 泰铢
        /// </summary>
        [Label("泰铢")]
        THB = 40
    }
}
