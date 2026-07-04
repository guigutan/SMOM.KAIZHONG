using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.SpareParts.ApiModels
{
    /// <summary>
    /// 备件申请信息
    /// </summary>
    [Serializable]
    public class SpareApplyInfo
    {
        /// <summary>
        /// 备件Id
        /// </summary>
        public double SptId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 备件申请数量
        /// </summary>
        public int? ApplyQty { get; set; }
    }
}
