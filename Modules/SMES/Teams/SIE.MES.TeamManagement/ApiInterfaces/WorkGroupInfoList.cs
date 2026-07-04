using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 班组人员API信息集合
    /// </summary>
    [Serializable]
    public class WorkGroupInfoList
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkGroupInfoList()
        {
            WorkGroupInfos = new List<WorkGroupInfo>();
            TotalCount = 0;
        }

        /// <summary>
        /// 班组人员API信息集合
        /// </summary>
        public List<WorkGroupInfo> WorkGroupInfos { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
