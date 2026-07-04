using SIE.Core.ApiModels;
using System;
using System.Collections.Generic;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 分页库位信息
    /// </summary>
    [Serializable]
    public class LocationDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 库位信息列表
        /// </summary>
        public List<LocationInfo> LocationInfos { get; } = new List<LocationInfo>();
    }

    /// <summary>
    /// 库位信息
    /// </summary>
    [Serializable]
    public class LocationInfo
    {
        /// <summary>
        /// 库位Id
        /// </summary>
        public double LocationId { get; set; }

        /// <summary>
        /// 库位
        /// </summary>
        public string Location { get; set; }


        /// <summary>
        /// 编码用于查找
        /// </summary>
        public string Code { get; set; }
    }


    /// <summary>
    /// 分页仓库信息
    /// </summary>
    [Serializable]
    public class WarehouseDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 仓库信息列表
        /// </summary>
        public List<WarehouseInfo> WarehouseInfos { get; set; } = new List<WarehouseInfo>();
    }

    /// <summary>
    /// 仓库信息
    /// </summary>
    [Serializable]
    public class WarehouseInfo
    {
        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public string Warehouse { get; set; }
        /// <summary>
        ///编码 用于查找
        /// </summary>
        public string Code { get; set; }
    }
}
