using SIE.Pcb.EventMessages.EAP;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EAP.MES
{
    /// <summary>
    /// 接口默认实现类
    /// </summary>
    public class PcbMesRequestService : IMesRequestService
    {

        /// <summary>
        /// 下发EAP
        /// </summary>
        /// <param name="selectedIds">选择的id</param>
        public void ReleaseEap(List<double> selectedIds)
        { 
        }
    }
}
