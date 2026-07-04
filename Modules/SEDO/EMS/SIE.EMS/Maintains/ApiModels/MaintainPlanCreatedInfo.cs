using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Maintains.ApiModels
{
    /// <summary>
    /// 已生成的保养计划信息
    /// </summary>
    [Serializable]
    public class MaintainPlanCreatedInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 单号
        /// </summary>
        public string MachineNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double EquipAccountId { get; set; }

        /// <summary>
        /// 周期
        /// </summary>
        public int Cycle { get; set; }

    }
}
