using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Tech.Routings.ApiModels
{
    /// <summary>
    /// 工艺路线接口参数
    /// </summary>
    [Serializable]
    public class ProcessRoutingInterFaceParamas
    {
        /// <summary>
        /// 产品族分类
        /// </summary>
        public string Category { get; set; }
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string RoutingName { get; set; }
        /// <summary>
        /// 工艺路线描述
        /// </summary>
        public string RoutDesc { get; set; }
        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 序列
        /// </summary>
        public string SortOrder { get; set; }

        /// <summary>
        /// 返回序列
        /// </summary>
        public string SortOrderBack { get; set; }

        /// <summary>
        /// 结果(通过,失败,任意,自定义)
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// 结果描述
        /// </summary>
        public string ResultDesc { get; set; }

        /// <summary>
        /// 是否可选
        /// </summary>
        public string Choose { get; set; }

        /// <summary>
        /// 是否重复
        /// </summary>
        public string Repeat { get; set; }

        /// <summary>
        /// 是否创建SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// 是否生成工序任务
        /// </summary>

        public string GenerateTask { get; set; }

        /// <summary>
        /// 是否需求任务清单
        /// </summary>
        public string IsRequirementTask { get; set; }
    }
}
