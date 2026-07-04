using SIE.ObjectModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 盘点结果查询
    /// </summary>
    public enum InventorySeachResult
    {

        /// <summary>
        /// 正常
        /// </summary>
        [Label("正常")]
        Normal = 10,
        /// <summary>
        /// 信息变动
        /// </summary>
        [Label("信息变动")]
        InfoChange = 20,
        /// <summary>
        /// 盘盈
        /// </summary>
        [Label("盘盈")]
        Profit = 30,
        /// <summary>
        /// 盘亏
        /// </summary>
        [Label("盘亏")]
        Loss = 40,

        /// <summary>
        /// 盘盈+盘亏
        /// </summary>
        [Label("盘盈+盘亏")]
        ProfitOrLoss=50



    }
}
