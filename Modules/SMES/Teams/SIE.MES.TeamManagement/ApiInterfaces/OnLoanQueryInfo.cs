using System;

namespace SIE.MES.TeamManagement.ApiInterfaces
{
    /// <summary>
    /// 借调单查询API信息类
    /// </summary>
    [Serializable]
    public class OnLoanQueryInfo
    {
        /// <summary>
        /// 查询方式
        /// 0为发起人查询、1为审核人查询
        /// </summary>
        public int QueryMode { get; set; }

        /// <summary>
        /// 页号
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// 每页记录数
        /// </summary>
        public int PageSize { get; set; }
    }
}
