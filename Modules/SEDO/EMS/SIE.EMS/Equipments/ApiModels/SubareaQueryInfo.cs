using SIE.Core.ApiModels;
using System;

namespace SIE.EMS.Elec.Fixtrue.Equipments.ApiModels
{
    /// <summary>
    /// 分区查询信息
    /// </summary>
    [Serializable]
    public class SubareaQueryInfo : PagingQueryInfo
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipId { get; set; }
    }
}