using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 返工确认提交
    /// </summary>
    [Serializable]
    public class PdaSubmitReworkLayoutInfo
    {
        /// <summary>
        /// 返工工艺路线
        /// </summary>
        public double VersionId { get; set; }

        /// <summary>
        /// 返工数量
        /// </summary>
        public decimal ReworkQty { get; set; }

        /// <summary>
        /// 基本开始日期
        /// </summary>
        public DateTime? BeginDateTime { get; set; }

        /// <summary>
        /// 基本完成日期
        /// </summary>
        public DateTime? EndDateTime { get; set; }

        /// <summary>
        /// 需求部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 标签数据
        /// </summary>
        public List<double> WipBatchIds { get; set; } = new List<double>();
    }
}
