using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.StandardWorkHours.ImportHandles
{
    /// <summary>
    /// 产品标准工时维护导入信息
    /// </summary>
    [Serializable]
    public class StandardHourSetImpInfo
    {
        /// <summary>
        /// 生产资源编码
        /// </summary>
        public string WipResourceCode { get; set; }

        /// <summary>
        /// 产品机型名称
        /// </summary>
        public string ProductModelName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 工序标准工时(分)
        /// </summary>
        public string StandardMin { get; set; }

        /// <summary>
        /// 附加合计工时(分)
        /// </summary>
        public string AttachMin { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } 

        /// <summary>
        /// 
        /// </summary>
        public DataRow DetailInfo { get; set; }
    }

    /// <summary>
    /// 产品标准工时维护导入结果
    /// </summary>
    public class StandardHourSetResultInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StandardHourSetResultInfo() 
        {
            Pass = true;
            Error = new StringBuilder();
        }

        /// <summary>
        /// 是否通过校验
        /// </summary>
        public bool Pass { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public StringBuilder Error { get; set; }

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? WipResourceId { get; set; }

        /// <summary>
        /// 产品机型Id
        /// </summary>
        public double? ProductModelId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double? ProductId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工序标准工时(分)
        /// </summary>
        public decimal StandardMin { get; set; }

        /// <summary>
        /// 附加合计工时(分)
        /// </summary>
        public decimal? AttachMin { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
