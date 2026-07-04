using SIE.ObjectModel;

namespace SIE.Inventory.Task
{
    /// <summary>
	/// 来源
	/// </summary>
	public enum SourceType
    {
        /// <summary>
        /// PC端自建
        /// </summary>
        [Label("PC端自建")]
        PC,

        /// <summary>
        /// 移动端自建
        /// </summary>
        [Label("移动端自建")]
        APP,

        /// <summary>
        /// 系统任务创建
        /// </summary>
        [Label("系统任务创建")]
        TASK,

        /// <summary>
		/// 调拨出库单
		/// </summary>
		[Label("调拨出库单")]
        ALLOCATEOUT,

        /// <summary>
        /// 外部接口
        /// </summary>
        [Label("外部接口")]
        EXTINTERFACE,

        /// <summary>
		/// 盘点单
		/// </summary>
		[Label("盘点单")]
        STORCKCOUNT,

        /// <summary>
        /// ASN单
        /// </summary>
        [Label("ASN单")]
        ASN,

        /// <summary>
        /// 发运单
        /// </summary>
        [Label("发运单")]
        SHIPPINGORDER,

        /// <summary>
        /// 提仓返检
        /// </summary>
        [Label("提仓返检")]
        WAREHOUSERETRUNINSP,

        /// <summary>
        /// MES接口
        /// </summary>
        [Label("MES接口")]
        MES,

        /// <summary>
        /// 批次LPN库存
        /// </summary>
        [Label("批次LPN库存")]
        LPNONHAND,

        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        Wo,

        /// <summary>
        /// 备料需求单
        /// </summary>
        [Label("备料需求单")]
        Require,

        /// <summary>
        /// 发货计划
        /// </summary>
        [Label("发货计划")]
        ShipPlan,
    }
}
