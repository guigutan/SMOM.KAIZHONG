using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP
{
    /// <summary>
    /// 资源查询信息
    /// </summary>
    [Serializable]
    public class ResourceQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 车间ID
        /// </summary>
        public double? WorkShopId { get; set; }
    }

    /// <summary>
    /// 分页资源信息
    /// </summary>
    [Serializable]
    public class ResourceDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 资源信息列表
        /// </summary>
        public List<ResourceInfo> ResourceInfos { get; } = new List<ResourceInfo>();
    }

    /// <summary>
    /// 工序查询信息
    /// </summary>
    [Serializable]
    public class ProcessQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        public int ProcessType { get; set; }
    }

    /// <summary>
    /// 分页工序信息
    /// </summary>
    [Serializable]
    public class ProcessDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 工序信息列表
        /// </summary>
        public List<ProcessInfo> ProcessInfos { get; } = new List<ProcessInfo>();
    }

    /// <summary>
    /// 工位查询信息
    /// </summary>
    [Serializable]
    public class StationQueryInfo : PagingKeywordQueryInfo
    {
        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId { get; set; }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        public int ProcessType { get; set; }
    }

    /// <summary>
    /// 分页工位信息
    /// </summary>
    [Serializable]
    public class StationDataInfo : PagingBaseDataInfo
    {
        /// <summary>
        /// 工位信息列表
        /// </summary>
        public List<StationInfo> StationInfos { get; } = new List<StationInfo>();
    }

    /// <summary>
    /// 资源信息
    /// </summary>
    [Serializable]
    public class ResourceInfo
    {
        /// <summary>
        /// 资源Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 资源编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// 工序信息
    /// </summary>
    [Serializable]
    public class ProcessInfo
    {
        /// <summary>
        /// 工序Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 工序类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 工序类型整型
        /// </summary>
        public int EumType { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// 工位信息
    /// </summary>
    [Serializable]
    public class StationInfo
    {
        /// <summary>
        /// 工位Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工位编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}
