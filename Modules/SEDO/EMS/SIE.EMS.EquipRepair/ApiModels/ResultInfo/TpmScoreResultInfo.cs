using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// TPM评分项信息实体
    /// </summary>
    [Serializable]
    public class TpmScoreResultInfo
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public double ProjectId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }


        /// <summary>
        /// 是否拍照
        /// </summary>
        public bool IsPhoto { get; set; }

    }
}
