using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingQcs.Data
{
    public class InsulatePackModel
    {
        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 装箱数量
        /// </summary>
        public int PackingNum { get; set; }

        /// <summary>
        /// 物料标签
        /// </summary>
        public string ItemLabel { get; set; }

    }
}
