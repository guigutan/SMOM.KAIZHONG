using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Common.Datas.EbsData
{
    /// <summary>
    /// ERP库存数据
    /// </summary>
    [Serializable]
    public class ErpOnHandData
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string Item_Code { get; set; }

        /// <summary>
        /// 批次号码
        /// </summary>
        public string Lot_Number { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// ERP子库
        /// </summary>
        public string Subinventory { get; set; }

        /// <summary>
        /// 货位
        /// </summary>
        public string Locator_Code { get; set; }

        /// <summary>
        /// 库存组织
        /// </summary>
        public string Organization_Name { get; set; }

        /// <summary>
        /// 唯一键
        /// </summary>
        public string Pri_Key { get; set; }
    }
}
