using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingQcs.Data
{
    public class NewPackingQcCModel
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
        /// 装箱标识
        /// </summary>
        public string PackIdent { get; set; }

        /// <summary>
        /// QC是否确认
        /// </summary>
        public string Confirm { get; set; }

        /// <summary>
        /// 装箱数量
        /// </summary>
        public int PackingNum { get; set; }

        /// <summary>
        /// 工序标签
        /// </summary>
        public string ProductLabel { get; set; }

        /// <summary>
        /// 箱子状态
        /// </summary>
        public string BoxState { get; set; }

    }
}
