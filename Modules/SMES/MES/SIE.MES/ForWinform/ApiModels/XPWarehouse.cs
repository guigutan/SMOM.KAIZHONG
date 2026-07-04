using Org.BouncyCastle.Bcpg.OpenPgp;
using SIE.LES.LinesideWarehouses;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class XPLinesideWarehouse
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string InvOrgName { get; set; }

        /// <summary>
        /// 库位Id
        /// </summary>
        public double StorageLocationId { get; set; }

       /// <summary>
       /// 库位名称
       /// </summary>
        public string StorageLocationName { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public XPLinesideWarehouse()
        { }

       
        /// <summary>
        /// 线边仓
        /// </summary>
        /// <param name="linesideWarehouse"></param>
        public XPLinesideWarehouse(LinesideWarehouse linesideWarehouse)
        {
            this.Id = linesideWarehouse.Id;
            this.WarehouseId= linesideWarehouse.WarehouseId;
            this.Code = linesideWarehouse.WarehouseCode;
            this.Name = linesideWarehouse.WarehouseName;
            this.StorageLocationId = linesideWarehouse.StorageLocationId;
            this.StorageLocationName = linesideWarehouse.LocaltionName;
        }
    }
}
