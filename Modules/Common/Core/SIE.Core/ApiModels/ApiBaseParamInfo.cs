using System;

namespace SIE.Core.ApiModels
{
    /// <summary>
    /// API分页查询参数类
    /// </summary>
    [Serializable]
    public class PagingQueryInfo : IPagingQuery
    {
        /// <summary>
        /// 页数 值为空查第一页
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// 页数据数量 值为空查所有
        /// </summary>
        public int? PageSize { get; set; }
    }

    /// <summary>
    /// API分页关键字查询参数类
    /// </summary>
    [Serializable]
    public class PagingKeywordQueryInfo : IPagingQuery, IKeywordQuery
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        public string Keyword { get; set; }

        /// <summary>
        /// 页数
        /// </summary>
        public int? PageNumber { get; set; }

        /// <summary>
        /// 页数据数量
        /// </summary>
        public int? PageSize { get; set; }
    }

    /// <summary>
    /// API分页结果类
    /// </summary>
    [Serializable]
    public class PagingResultInfo : IPagingResult
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
        public int TotalCount { get; set; }
    }

    /// <summary>
    /// API分页查询接口
    /// </summary>
    public interface IPagingQuery
    {
        /// <summary>
        /// 页数
        /// </summary>
        int? PageNumber { get; set; }

        /// <summary>
        /// 页数据数量
        /// </summary>
        int? PageSize { get; set; }
    }

    /// <summary>
    /// API分页结果接口
    /// </summary>
    public interface IPagingResult : IPagingQuery
    {
        /// <summary>
        /// 数据总数
        /// </summary>
        int TotalCount { get; set; }
    }

    /// <summary>
    /// API关键字查询接口
    /// </summary>
    public interface IKeywordQuery
    {
        /// <summary>
        /// 查询关键字
        /// </summary>
        string Keyword { get; set; }
    }
}