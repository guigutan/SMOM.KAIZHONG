using System;
using System.Collections.Generic;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 借调单API信息集合
    /// </summary>
    [Serializable]
    public class OnLoanInfoList
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OnLoanInfoList()
        {
            OnLoanInfos = new List<OnLoanInfo>();
            TotalCount = 0;
        }

        /// <summary>
        /// 借调单API信息集合
        /// </summary>
        public List<OnLoanInfo> OnLoanInfos { get; set; }

        /// <summary>
        /// 记录总条数
        /// </summary>
        public int TotalCount { get; set; }
    }
}
