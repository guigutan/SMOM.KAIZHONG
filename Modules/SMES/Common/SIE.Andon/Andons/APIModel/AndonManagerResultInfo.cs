using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Andon.Andons.APIModel
{
    /// <summary>
    /// 安灯经验记录集合
    /// </summary>
    [Serializable]
    public class AndonExperienceInfos : PagingBaseDataInfo
    {
       /// <summary>
       /// 安灯经验记录
       /// </summary>
        public List<AndonExperienceInfo> AndonExperienceResults { get; set; } = new List<AndonExperienceInfo>();

    }

    /// <summary>
    /// 安灯管理结果信息
    /// </summary>
    [Serializable]
    public class AndonExperienceInfo
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 事件编码
        /// </summary>

        public string Code { get; set; }

        /// <summary>
        /// 事件原因
        /// </summary>

        public string EventReason { get; set; }

        /// <summary>
        /// 处理方式
        /// </summary>
        public string HandleMethod { get; set; }

        /// <summary>
        /// 预防措施
        /// </summary>
        public string Measures { get; set; }

    }



}
