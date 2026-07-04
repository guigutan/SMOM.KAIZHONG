using SIE.MES.BarcodeProcesses;
using SIE.Web.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BarcodeProcesses
{
    /// <summary>
    /// 条码工序请求后台
    /// </summary>
    public class BarcodeProcessDataQueryer : DataQueryer
    {
        /// <summary>
        /// 同步工单工序清单
        /// </summary>
        /// <param name="mainId">条码id</param>
        /// <param name="woId">工单id</param>
        public void SynWoProcessListQuery(double mainId, double? woId)
        {
            RT.Service.Resolve<BarcodeProcessController>().SynWoProcessListQuery(mainId, woId);
        }
    }
}
