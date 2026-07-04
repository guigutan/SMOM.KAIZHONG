using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 物料平衡报表数据
    /// </summary>
    [Serializable]
    public class ItemBalanceData
    {
        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 车间(MRP控制者)
        /// </summary>
        public string WorkShopCode { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public string ItemType { get; set; }

        /// <summary>
        /// 投料量
        /// </summary>
        public decimal FeedingQty { get; set; }

        /// <summary>
        /// 产出用量
        /// </summary>
        public decimal ProductQty { get; set; }

        /// <summary>
        /// 副产品回收量
        /// </summary>
        public decimal OutputProductQty { get; set; }

        /// <summary>
        /// 余料量
        /// </summary>
        public decimal RemainingQty { get; set; }

        /// <summary>
        /// 差异量
        /// </summary>
        public decimal DiffQty { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal Rate { get; set; }

    }
}
