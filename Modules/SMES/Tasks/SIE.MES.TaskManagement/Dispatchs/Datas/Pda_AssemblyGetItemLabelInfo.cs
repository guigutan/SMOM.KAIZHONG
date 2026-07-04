using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    [Serializable]
    public class Pda_AssemblyGetItemLabelInfo
    {
        /// <summary>
        /// 标签Id
        /// </summary>
        public double ItemLabelId { get; set; }

        /// <summary>
        /// 物料标签
        /// </summary>
        public string ItemLabel { get; set; }

        /// <summary>
        /// 物料批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 上料数量
        /// </summary>
        public decimal AssemblyQty { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string ItemLabelState { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Unit { get; set; }
    }
}
