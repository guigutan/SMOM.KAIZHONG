namespace SIE.EventMessages.MES.WIP
{
    /// <summary>
    /// 在制
    /// </summary>
    public interface IWipController
    {
        /// <summary>
        /// SN归属工单运行时检查
        /// </summary>
        void BarcodeBelongWorkOrderCheck(string sn);
    }
}
