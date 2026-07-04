using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.ApiModels
{
    /// <summary>
    /// TPM评分项信息实体
    /// </summary>
    [Serializable]
    public class TpmScoreInfo
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public double ProjectId { get; set; }

        /// <summary>
        /// 分数（1~5）
        /// </summary>
        public int EquipRepairScore { get; set; }

        /// <summary>
        /// 照片上下文
        /// </summary>
        public string PhotoContent { get; set; }

        /// <summary>
        /// 文件名(含扩展名)
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }
    }
}
