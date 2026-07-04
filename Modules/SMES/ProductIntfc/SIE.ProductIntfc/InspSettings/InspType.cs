using SIE.ObjectModel;

namespace SIE.ProductIntfc.InspSettings
{
    /// <summary>
    /// 报检类型
    /// </summary>
    public enum InspType
    {
        /// <summary>
        /// 成品
        /// </summary>
        [Label("成品")]
        Product = 0,

        /// <summary>
        /// 首件
        /// </summary>
        [Label("首件")]
        FirstProduct = 1,

        ///// <summary>
        ///// 抽检
        ///// </summary>
        //[Label("抽检")]
        //SampleInsp = 2,
    }
}