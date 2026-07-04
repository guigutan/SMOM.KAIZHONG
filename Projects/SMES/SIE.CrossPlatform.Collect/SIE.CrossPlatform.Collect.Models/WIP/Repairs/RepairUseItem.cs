using SIE.CrossPlatform.Collect.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP.Repairs
{

    /// <summary>
    /// 维修数据
    /// </summary>
    [Serializable]
    public class RepairUseItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RepairUseItem()
        {
            UserItems = new Dictionary<double, decimal>();
        }

        /// <summary>
        /// 源条码
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 源产品生产关键件Id
        /// </summary>
        public double SoureKeyItemId { get; set; }

        /// <summary>
        /// 换料项 上料ID、换料数量
        /// </summary>
        public Dictionary<double, decimal> UserItems { get; set; }

        /// <summary>
        /// 置换后处理
        /// </summary>
        public ChangeItemHandleMethod HandleMethod { get; set; }

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId { get; set; }
    }
}