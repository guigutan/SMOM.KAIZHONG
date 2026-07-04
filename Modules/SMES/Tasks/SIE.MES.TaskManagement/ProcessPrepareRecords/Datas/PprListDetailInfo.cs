using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.ProcessPrepareRecords.Datas
{
    /// <summary>
    /// 产前准备任务项目明细列表
    /// </summary>
    [Serializable]
    public class PprListDetailInfo
    {
        /// <summary>
        /// 工序属性维护-产前准备项目明细Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 项目Id
        /// </summary>
        public double ProId { get; set; }

        /// <summary>
        /// 项目类型
        /// </summary>
        public string ProType { get; set; }

        /// <summary>
        /// 项目描述
        /// </summary>
        public string ProDesc { get; set; }

    }
}
