using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.OnOffDuty.ApiModel
{
    /// <summary>
    /// 上下岗信息
    /// </summary>
    [Serializable]
    public class OnOffDutyRecrodInfo
    {
        /// <summary>
        /// 工号
        /// </summary>
        public string StaffCode { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string StaffName { get; set; }

        /// <summary>
        /// 员工组
        /// </summary>
        public string StaffGroup { get; set; }

        /// <summary>
        /// 资源
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 工位
        /// </summary>
        public string Station { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string DutyType { get; set; }

        /// <summary>
        /// 上岗时间
        /// </summary>
        public string OnDutyTime { get; set; }

        /// <summary>
        /// 下岗时间
        /// </summary>
        public string OffDutyTime { get; set; }
    }
}
