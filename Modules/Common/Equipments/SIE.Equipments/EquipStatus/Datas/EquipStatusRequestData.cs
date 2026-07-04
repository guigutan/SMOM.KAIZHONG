using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Equipments.EquipStatus.Datas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class EquipStatusRequestData
    {
        /// <summary>
        /// 设备编码
        /// </summary>
        public string EqupNo { get; set; }
        
        /// <summary>
        /// 状态(1:待机,2:运行,0:离线)
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
