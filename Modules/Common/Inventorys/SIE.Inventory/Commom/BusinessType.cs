using SIE.ObjectModel;

namespace SIE.Inventory.Commom
{
    /// <summary>
    /// 业务类型
    /// </summary>
    public enum BusinessType
    {
        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        InStorage = 10,

        /// <summary>
        /// 出库
        /// </summary>
        [Label("出库")]
        OutStorage = 20,

        /// 取消出库
        /// </summary>
        [Label("取消出库")]
        CanelOutStorage = 21,

        /// <summary>
        /// 移动
        /// </summary>
        [Label("移动")]
        Move = 30,

        /// <summary>
        /// 调拨
        /// </summary>
        [Label("调拨")]
        Allocate = 40,

        /// <summary>
        /// 盘点
        /// </summary>
        [Label("盘点")]
        Count = 50,

        /// <summary>
        /// 搬运
        /// </summary>
        [Label("搬运")]
        Carry = 60,

        /// <summary>
        /// 调整
        /// </summary>
        [Label("调整")]
        Adjust = 70,

        /// <summary>
        /// 属性调整
        /// </summary>
        [Label("属性调整")]
        AttributeAdjust = 71,

        /// <summary>
        /// 冻结解冻
        /// </summary>
        [Label("冻结解冻")]
        FreezeThaw = 80,

        /// <summary>
        /// 库存报检
        /// </summary>
        [Label("库存报检")]
        InvInspection = 90,

        /// <summary>
        /// 波次播种
        /// </summary>
        [Label("波次播种")]
        Sow = 95,

        /// <summary>
        /// 补货
        /// </summary>
        [Label("补货")]
        Replenish = 100,
    }
}