using SIE.EventMessages.MES.PanelBindings.Models;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.PanelBindings
{
    /// <summary>
    /// 
    /// </summary>
    [Services.Service(FallbackType = typeof(EmptyPanelBinding))]
    public interface IPanelBinding
    {
        /// <summary>
        /// 获取Pcb
        /// </summary>
        /// <param name="productId">产品Id</param>
        List<PcbItemDetailInfo> GetPcbItemDetailInfos(double productId);
    }

    /// <summary>
    /// 
    /// </summary>
    class EmptyPanelBinding : IPanelBinding
    {
        public List<PcbItemDetailInfo> GetPcbItemDetailInfos(double productId)
        {
            return new List<PcbItemDetailInfo>();
        }
    }
}
