using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 评分项目API信息类
    /// </summary>
    [Serializable]
    public class RatedItemInfo
    {

        /// <summary>
        /// 评分项目Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 评分项目编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 评分项目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 最低分值
        /// </summary>
        public decimal MinScore { get; set; }

        /// <summary>
        /// 最大分值
        /// </summary>
        public decimal MaxScore { get; set; }

        /// <summary>
        /// 是否系统评分项目
        /// </summary>
        public bool IsSystem { get; set; }
    }
}
