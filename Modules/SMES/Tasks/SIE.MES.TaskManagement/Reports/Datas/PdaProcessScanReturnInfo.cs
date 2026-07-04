using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Reports.Datas
{
    [Serializable]
    public class PdaProcessScanReturnInfo: PdaScanReturnInfo
    {

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Tip { get; set; }
    }
}
