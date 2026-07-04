using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Checks.ApiModels
{
    /// <summary>
    /// 备件仓库信息
    /// </summary>
    [Serializable]
    public class SparepartWareInfo
    {
        /// <summary>
        /// 备件Id
        /// </summary>
        public double SparePartId { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WareCode { get; set; }
        
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WareName { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int StoreQty { get; set; }
    }
}
