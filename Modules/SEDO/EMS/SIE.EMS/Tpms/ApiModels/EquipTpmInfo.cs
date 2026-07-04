using SIE.EMS.Equipments.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.EMS.Tpms.ApiModels
{
    /// <summary>
    /// TPM设备信息
    /// </summary>
    [Serializable]
    public class EquipTpmInfo
    {
        /// <summary>
        /// 设备信息
        /// </summary>
        public EquipInfo EquipInfo { get; set; }

        /// <summary>
        /// 班组
        /// </summary>
        public double WorkGroupId { get; set; }

        /// <summary>
        /// 得分
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// 是否完成评分
        /// </summary>
        public bool IsFinish { get; set; }

        /// <summary>
        /// 评分项
        /// </summary>
        public List<TpmDetailInfo> DetailDatas { get; } = new List<TpmDetailInfo>();
    }
}
