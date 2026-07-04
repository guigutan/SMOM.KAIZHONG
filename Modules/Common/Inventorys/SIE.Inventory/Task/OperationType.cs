using SIE.ObjectModel;

namespace SIE.Inventory.Task
{
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 上架
        /// </summary>
        [Label("上架")]
        PutOn = 0,

        /// <summary>
        /// 下架
        /// </summary>
        [Label("下架")]
        PullOff = 1,

        /// <summary>
        /// 移动
        /// </summary>
        [Label("移动")]
        Move = 2,

        /// <summary>
        /// 调拨
        /// </summary>
        [Label("调拨")]
        Allot = 3,

        /// <summary>
        /// 补货
        /// </summary>
        [Label("补货")]
        Replenish = 4,

        /// <summary>
        /// 盘点
        /// </summary>
        [Label("盘点")]
        Check = 5,

        /// <summary>
		/// 播种
		/// </summary>
		[Label("播种")]
        Sow = 6,

        /// <summary>
        /// 盘点回库
        /// </summary>
        [Label("盘点回库")]
        CheckBack = 7,

        /// <summary>
        /// 拣货
        /// </summary>
        [Label("拣货")]
        Pick = 8,

        /// <summary>
        /// 拣货回库
        /// </summary>
        [Label("拣货回库")]
        PickBack = 9,

        /// <summary>
        /// 搬运
        /// </summary>
        [Label("搬运")]
        Carry = 10,

        ///<summary>
        /// 调拨拣货
        /// </summary>
        [Label("调拨拣货")]
        AllotPick = 11,
    }
}