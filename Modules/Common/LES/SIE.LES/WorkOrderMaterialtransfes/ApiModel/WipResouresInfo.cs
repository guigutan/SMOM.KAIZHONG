using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.WorkOrderMaterialtransfes.ApiModel
{
    /// <summary>
    /// 资源信息集合
    /// </summary>
    [Serializable]
    public class WipResouresInfos : PagingBaseDataInfo
    {
        /// <summary>
        /// 资源信息
        /// </summary>
        public List<WipResouresInfo> ResultInfos { get; set; } = new List<WipResouresInfo>();

    }



    /// <summary>
    /// 资源信息
    /// </summary>
    [Serializable]
    public class WipResouresInfo
    {
        /// <summary>
        ///资源Id
        /// </summary>
        public double ResouceId { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResouceName { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string ResouceCode { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库编码
        /// </summary>
        public string WarehouseCode { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 库位编码
        /// </summary>
        public string LocationCode { get; set; }

        /// <summary>
        /// 库位名称
        /// </summary>
        public string LocationName { get; set; }
    }
}
