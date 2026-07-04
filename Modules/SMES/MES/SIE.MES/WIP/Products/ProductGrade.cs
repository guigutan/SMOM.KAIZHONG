using SIE.ObjectModel;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品等级
    /// </summary>
    public enum ProductGrade
    {
        /// <summary>
        /// A级
        /// </summary>
        [Label("A级")]
        A = 10,

        /// <summary>
        /// B级
        /// </summary>
        [Label("B级")]
        B = 20,

        /// <summary>
        /// C级
        /// </summary>
        [Label("C级")]
        C = 30,

        /// <summary>
        /// 报废
        /// </summary>
        [Label("报废")]
        Scrap = 999,
    }
}