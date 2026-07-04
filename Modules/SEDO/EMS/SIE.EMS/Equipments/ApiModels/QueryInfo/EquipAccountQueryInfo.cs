using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Equipments.ApiModels
{
    /// <summary>
    /// 设备台账分页查询实体
    /// </summary>
    [Serializable]
    public class EquipAccountQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 设备型号ID
        /// </summary>
        public double? EquipModelId { get; set; }

        /// <summary>
        /// 设备类型ID
        /// </summary>
        public double? EquipTypeId { get; set; }
    }
}
