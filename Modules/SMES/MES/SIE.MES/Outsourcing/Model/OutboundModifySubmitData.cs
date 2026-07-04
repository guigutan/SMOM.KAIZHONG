using DocumentFormat.OpenXml.Office.CoverPageProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Outsourcing.Model
{
    /// <summary>
    /// PDA发货修改提交数据
    /// </summary>
    [Serializable]
    public class OutboundModifySubmitData
    {

        /// <summary>
        /// 发货确认明细Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 框数
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 删除明细数据
        /// </summary>
        public List<double> deleteDatas { get; set; } = new List<double>();

        /// <summary>
        /// 新增委外明细Id
        /// </summary>
        public List<double> addDatas { get; set; } = new List<double>();
    }

}
