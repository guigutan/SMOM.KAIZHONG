using SIE.ObjectModel;

namespace SIE.Inventory.Strategy
{
    /// <summary>
    /// 排序字段
    /// </summary>
    public enum SortField
    {
        /// <summary>
        /// 批次号
        /// </summary>
        [Label("批次号")]
        LotNo = 1,

        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        LotAtt01 = 10,

        /// <summary>
        /// 失效日期
        /// </summary>
        [Label("失效日期")]
        LotAtt02 = 20,

        /// <summary>
        /// 收货日期
        /// </summary>
        [Label("收货日期")]
        LotAtt03 = 30,

        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        LotAtt04 = 40,

        /// <summary>
        /// 批次属性05
        /// </summary>
        [Label("批次属性05")]
        LotAtt05 = 50,

        /// <summary>
        /// 批次属性06
        /// </summary>
        [Label("批次属性06")]
        LotAtt06 = 60,

        /// <summary>
        /// 批次属性07
        /// </summary>
        [Label("批次属性07")]
        LotAtt07 = 70,

        /// <summary>
        /// 批次属性08
        /// </summary>
        [Label("批次属性08")]
        LotAtt08 = 80,

        /// <summary>
        /// 批次属性09
        /// </summary>
        [Label("批次属性09")]
        LotAtt09 = 90,

        /// <summary>
        /// 批次属性10
        /// </summary>
        [Label("批次属性10")]
        LotAtt10 = 100,

        /// <summary>
        /// 批次属性11
        /// </summary>
        [Label("批次属性11")]
        LotAtt11 = 110,

        /// <summary>
        /// 批次属性12
        /// </summary>
        [Label("批次属性12")]
        LotAtt12 = 120,
    }
}