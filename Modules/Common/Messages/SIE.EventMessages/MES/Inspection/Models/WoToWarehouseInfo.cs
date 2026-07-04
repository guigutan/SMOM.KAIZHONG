using System;

namespace SIE.EventMessages.MES.Inspection.Models
{
    /// <summary>
    /// 工单入库仓库数据
    /// </summary>
    [Serializable]
    public class WoToWarehouseInfo
    {        
        /// <summary>
        /// 仓库
        /// </summary>
        public double WarehouseId { get; set; }       
    }
}
