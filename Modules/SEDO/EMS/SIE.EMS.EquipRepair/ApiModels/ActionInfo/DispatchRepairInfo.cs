using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// 派工参数实体
    /// </summary>
    [Serializable]
    public class DispatchRepairInfo: TakeRepairInfo
    {
        /// <summary>
        /// 派工类型(0:内修，1:外修)
        /// </summary>
        public int RepairWay { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 送修方式(0:厂外维修，1:现场维修)
        /// </summary>
        public int? SendRepairWay { get; set; }

        /// <summary>
        /// 快递/出厂单号
        /// </summary>
        public string DeliveryNo { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string ContactPhone { get; set; }

        /// <summary>
        /// 外修时间
        /// </summary>
        public DateTime? SendRepairDate { get; set; }

        /// <summary>
        /// 预计返厂时间
        /// </summary>
        public DateTime? PredictBackDate { get; set; }

        /// <summary>
        /// 项目ID
        /// </summary>
        public double? ProjectId { get; set; }

        /// <summary>
        /// 项目事项Id
        /// </summary>
        public double? ProjectKeyItemId { get; set; }

    }
}
