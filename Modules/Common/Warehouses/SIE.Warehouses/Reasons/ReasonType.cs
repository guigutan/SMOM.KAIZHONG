using SIE.ObjectModel;

namespace SIE.Warehouses
{
    /// <summary>
    /// 类型
    /// </summary>
    public enum ReasonType
    {
        /// <summary>
        /// 移动原因
        /// </summary>
        [Label("移动原因")]
        MOVE_REASON,

        /// <summary>
        /// 调拨原因
        /// </summary>
        [Label("调拨原因")]
        ALLOCATE_REASON,

        /// <summary>
        /// 调整原因
        /// </summary>
        [Label("调整原因")]
        ADJUST_REASON,

        /// <summary>
        /// 冻结原因
        /// </summary>
        [Label("冻结原因")]
        FREEZE_REASON,

        /// <summary>
        /// 释放原因
        /// </summary>
        [Label("释放原因")]
        RELEASE_REASON,

        /// <summary>
        /// 任务异常原因
        /// </summary>
        [Label("任务异常原因")]
        TASK_EXCEPTION,

        /// <summary>
        /// 缺货原因
        /// </summary>
        [Label("缺货原因")]
        IMPERFECT_REASON,

        /// <summary>
        /// 盘点原因
        /// </summary>
        [Label("盘点原因")]
        COUNT_REASON,

        /// <summary>
        /// 取消预约原因
        /// </summary>
        [Label("取消预约原因")]
        CANCEL_APPOINTMENT,

        /// <summary>
        /// 解冻原因
        /// </summary>
        [Label("解冻原因")]
        UNFREEZE_REASON,

        /// <summary>
        /// 库存属性变更原因
        /// </summary>
        [Label("库存属性变更原因")]
        ONHAND_REASON,

        /// <summary>
        /// 超期复检原因
        /// </summary>
        [Label("超期复检原因")]
        OVERDUERECHECK_REASON,

        /// <summary>
        /// 其他复检原因
        /// </summary>
        [Label("其他复检原因")]
        OTHERRECHECK_REASON,

        /// <summary>
        /// 供应商退货原因
        /// </summary>
        [Label("供应商退货原因")]
        SUPPLIERRETURN_REASON,

        /// <summary>
        /// 取消排队原因
        /// </summary>
        [Label("取消排队原因")]
        CANCELQUEUE_REASON,

        /// <summary>
        /// 超期批次复检原因
        /// </summary>
        [Label("超期批次复检原因")]
        OVERDUEBATCHRECHECK_REASON,
    }
}