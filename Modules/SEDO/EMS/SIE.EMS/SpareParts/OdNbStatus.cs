using SIE.ObjectModel;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 序列号状态
    /// </summary>
    public enum OdNbStatus
    {
        /// <summary>
        /// 不良品
        /// </summary>
        [Label("不良品")]
        NoGoodProduct = 0,

        /// <summary>
        /// 良品
        /// </summary>
        [Label("良品")]
        GoodProduct = 1,

        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap = 2,
    }
}