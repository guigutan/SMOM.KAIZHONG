using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    [Serializable]
    public class PreStartupSetupModelTaskInfo: Pda_PreStartupSetupTaskInfo
    {
        /// <summary>
        /// 已扫描模具
        /// </summary>
        public List<Pda_PreStartupSetupScanEquipAccountInfo> ScannedModel { get; set; } = new List<Pda_PreStartupSetupScanEquipAccountInfo>();
    }
}
