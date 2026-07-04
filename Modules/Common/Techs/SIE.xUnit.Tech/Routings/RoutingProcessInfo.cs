using SIE.Tech.Processs;
using System;

namespace SIE.xUnit.Tech.Routings
{
    /// <summary>
    /// 工艺路线工序信息
    /// </summary>
    [Serializable]
    public class RoutingProcessInfo
    {
        /// <summary>
        /// 工序
        /// </summary>
        public Process Process { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// 返回序列
        /// </summary>
        public int SortOrderBack { get; set; }

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultTypeForDesign ResultType { get; set; }

        /// <summary>
        /// 是否重复
        /// </summary>
        public bool? IsRepeat { get; set; }

        /// <summary>
        /// 是否可选
        /// </summary>
        public bool? CanChoose { get; set; }

        /// <summary>
        /// 创建SKU
        /// </summary>
        public bool? CreateSku { get; set; }
    }
}
