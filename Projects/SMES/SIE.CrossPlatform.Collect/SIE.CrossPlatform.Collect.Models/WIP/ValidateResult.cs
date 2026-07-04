using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    [Serializable]
    public class ValidateResult
    {
        /// <summary>
        /// 工单信息
        /// </summary>
        public WorkOrder WorkOrderInfo { get; set; }

        /// <summary>
        /// 产品信息
        /// </summary>
        public ProductInfo ProductInfo { get; set; }

    }
}
