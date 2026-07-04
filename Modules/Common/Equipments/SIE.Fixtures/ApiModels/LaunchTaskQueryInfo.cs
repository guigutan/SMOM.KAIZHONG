using SIE.Core.ApiModels;
using System;

namespace SIE.Fixtures.ApiModels
{
    /// <summary>
    /// 上架任务查询信息
    /// </summary>
    [Serializable]
    public class LaunchTaskQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 工治具ID/上架任务号
        /// </summary>
        public string Code { get; set; }
    }

    /// <summary>
    /// 库位查询信息
    /// </summary>
    [Serializable]
    public class LocationQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 仓库Id
        /// </summary>
        public double WarehouseId { get; set; }
    }

    /// <summary>
    /// 仓库查询信息
    /// </summary>
    [Serializable]
    public class WarehouseQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 工治具ID
        /// </summary>
        public string Code { get; set; }
    }
}
