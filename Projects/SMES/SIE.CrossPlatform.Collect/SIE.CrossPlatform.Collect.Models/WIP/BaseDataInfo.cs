using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    /// <summary>
    /// 分页基本数据信息列表
    /// </summary>
    [Serializable]
    public class PagingBaseDataInfo
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// 页数据数量
        /// </summary>
        public int? PageSize { get; set; }

        /// <summary>
        /// 数据总数
        /// </summary>
        public int? TotalCount { get; set; }

        /// <summary>
        /// 基本数据信息列表
        /// </summary>
        public List<BaseDataInfo> DataInfos { get; } = new List<BaseDataInfo>();
    }

    /// <summary>
    /// 基本数据信息：ID、编码、名称
    /// </summary>
    [Serializable]
    public class BaseDataInfo
    {
        /// <summary>
        /// ID
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
    }

    /// <summary>
    /// 基本数据信息：Id, 字符串
    /// </summary>
    [Serializable]
    public class BaseDataStringInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }
}
