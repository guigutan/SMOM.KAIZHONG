using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.ProcessPrepareRecords.Datas
{
    /// <summary>
    /// 提交项目数据
    /// </summary>
    [Serializable]
    public class SubmitPprListDetailInfo
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
        /// 确认结果(是否通过,true:通过,false:不通过)
        /// </summary>
        public bool Result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
