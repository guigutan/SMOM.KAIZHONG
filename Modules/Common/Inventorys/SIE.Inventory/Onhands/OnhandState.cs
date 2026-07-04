using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Inventory.Onhands
{
    /// <summary>
    /// 库存状态
    /// </summary>
    public enum OnhandState
    {
        /// <summary>
        /// 未质检
        /// </summary>
        [Label("未质检")]
        None = 1,

        /// <summary>
        /// 合格
        /// </summary>
        [Label("合格")]
        [Category("RESULT")]
        Ok = 10,

        /// <summary>
        /// 不合格
        /// </summary>
        [Label("不合格")]
        [Category("RESULT")]
        Ng = 20,
    }
}