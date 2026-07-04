using System;
using System.Collections.Generic;

namespace SIE.Core.ApiModels
{
    /// <summary>
    /// 设备获取EAP实时值结果
    /// </summary>
    [Serializable]
    public class EquipEapRTValueInfo
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public EquipEapError Error { get; set; } = new EquipEapError();

        /// <summary>
        /// 请求ID
        /// </summary>
        public double? RequestId { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Timestamp { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public List<EquipEapRTValueInfoData> Data { get; set; } = new List<EquipEapRTValueInfoData>();
    }

    /// <summary>
    /// 数据对象
    /// </summary>
    [Serializable]
    public class EquipEapRTValueInfoData
    {
        /// <summary>
        /// Tag
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 质量戳
        /// </summary>
        public int QualityStamp { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 检验项ID
        /// </summary>
        public double? ProjectDetailId { get; set; }
    }

    /// <summary>
    /// 数据对象
    /// </summary>
    [Serializable]
    public class EquipEapError
    {
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 内部错误
        /// </summary>
        public object InnerError { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public int Code { get; set; }
    }
}
