using SIE.Core.ApiModels;
using System;

namespace SIE.EMS.Elec.Fixtrue.Equipments.ApiModels
{
    /// <summary>
    /// 站位查询信息
    /// </summary>
    [Serializable]
    public class StanceQueryInfo : PagingQueryInfo
    {
        /// <summary>
        /// 设备ID
        /// </summary>
        public double EquipId { get; set; }

        /// <summary>
        /// 分区
        /// </summary>
        public string Subarea { get; set; }
    }
}