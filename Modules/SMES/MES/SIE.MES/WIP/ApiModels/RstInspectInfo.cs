using SIE.Defects;
using System;
using System.Collections.Generic;

namespace SIE.MES.WIP.ApiModels
{
    /// <summary>
    /// 检验采集返回信息
    /// </summary>
    [Serializable]
    public class RstInspectInfo
    {
        /// <summary>
        /// 采集结果
        /// </summary>
        public string ResultType { get; set; }
    }

    /// <summary>
    /// 检验采集验证返回信息
    /// </summary>
    [Serializable]
    public class RstInspValidateInfo
    {
        /// <summary>
        /// 采集返回信息
        /// </summary>
        public RstWipInfo RstWipInfo { get; set; }

        /// <summary>
        /// 缺陷数据信息
        /// </summary>
        public DefectData DefectData { get; set; }
    }

    /// <summary>
    /// 检验查询信息
    /// </summary>
    [Serializable]
    public class InspectQueryInfo : WipQueryInfo
    {
        /// <summary>
        /// 是否是生产条码
        /// </summary>
        public bool IsSn { get; set; }
    }

    /// <summary>
    /// 检验采集提交信息
    /// </summary>
    [Serializable]
    public class InspectSumbitInfo : WipQueryInfo
    {
        /// <summary>
        /// 缺陷信息列表
        /// </summary>
        public List<DefectData> DefectDatas { get; set; } = new List<DefectData>();
    }
}
