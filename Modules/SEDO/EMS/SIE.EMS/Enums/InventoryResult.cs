using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.EMS.Enums
{
    /// <summary>
    /// 盘点结果
    /// </summary>
    public enum InventoryResult
    {
        /// <summary>
        /// 正常
        /// </summary>
        [Category("Account")]
        [Label("正常")]
        Normal = 10,
        /// <summary>
        /// 信息变动
        /// </summary>
        [Category("Account")]
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
        [Category("Account")]
        [Label("盘亏")]
        Loss = 40,
    }
}