using SIE.ObjectModel;

namespace SIE.Barcodes.Configs
{
    /// <summary>
    /// 工单归属验证模式
    /// </summary>
    public enum BelongWoValidationModel 
    {
        /// <summary>
        /// 原工单与归属工单属于同一个生产订单
        /// </summary>
        [Label("是否同个生产订单")]
        SameProductionOrder = 0,

        /// <summary>
        /// 原工单产品编码必须是归属工单的产品BOM物料
        /// </summary>
        [Label("是否产品BOM物料")]
        BelongBom = 1,
    }
}