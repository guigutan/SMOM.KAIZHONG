using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    [Serializable]
    public class Pda_ValidPreStartupSetupEquipAccountInfo
    {
        /// <summary>
        /// 开机准备记录Id
        /// </summary>
        public double RecordId { get; set; }

        /// <summary>
        /// 模具
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string State { get; set; }
    }
}
