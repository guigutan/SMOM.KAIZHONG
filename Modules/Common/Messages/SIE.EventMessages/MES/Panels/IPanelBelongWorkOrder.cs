namespace SIE.EventMessages.MES.Panels
{
    /// <summary>
    /// 拼板码归属工单接口
    /// </summary>
    public interface IPanelBelongWorkOrder
    {
        /// <summary>
        /// 拼板码归属工单
        /// </summary>
        void PanelBelongWorkOrder(double belongWorkOrderId, double panelId);
    }
}
