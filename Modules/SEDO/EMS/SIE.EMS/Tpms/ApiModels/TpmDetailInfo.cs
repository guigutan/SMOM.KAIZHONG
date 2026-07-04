using System;

namespace SIE.EMS.Tpms.ApiModels
{
    /// <summary>
    /// TPM评分明细
    /// </summary>
    [Serializable]
    public class TpmDetailInfo
    {
        /// <summary>
        /// 评分项ID
        /// </summary>
        public double WeekJobScoreItemId { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 项目类型,0-检验项，1-规范项
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string StringType { get; set; }

        /// <summary>
        /// 分值比
        /// </summary>
        public int ScoreRate { get; set; }

        /// <summary>
        /// 扣分
        /// </summary>
        public int? DeductScore { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 检查标准或要求
        /// </summary>
        public string CheckStandard { get; set; }


        /// <summary>
        /// 图片
        /// </summary>
        public string Photo { get; set; }
    }
}
