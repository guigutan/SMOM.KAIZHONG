using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingQcs.Data
{
    public class InsulatePackWoinfo
    {
        /// <summary>
        /// 扫描蓝标还是物料标签 
        /// FALSE=蓝标，TRUE=物料标签
        /// </summary>
        public bool Scan { get; set; }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WoId { get; set; }
        /// <summary>
        /// 工单号
        /// </summary>
        public string WoNo { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 已装箱总数量
        /// </summary>
        public int BlueInt { get; set; }

        /// <summary>
        /// 蓝标装箱总数量
        /// </summary>
        public int BlueZInt { get; set; }

        /// <summary>
        /// 蓝标
        /// </summary>
        public string XtBlue { get; set; }

        /// <summary>
        /// 原蓝标
        /// </summary>
        public string YXtBlue { get; set; }

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// 错误提示
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// 正确提示
        /// </summary>
        public string Tips { get; set; }

        /// <summary>
        /// 是否可以使用扫码
        /// </summary>
        public bool IsUse { get; set; } = true;

        public List<InsulatePackModel> PackingDetail { get; set; }
    }
}
