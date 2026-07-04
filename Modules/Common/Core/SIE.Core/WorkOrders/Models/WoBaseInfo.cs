using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.WorkOrders.Models
{
    /// <summary>
    /// 工单Id和工单号
    /// </summary>
    public class WoBaseInfo
    {
        /// <summary>
        /// 工单Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工单号
        /// </summary>
        public string No { get; set; }
    }
}
