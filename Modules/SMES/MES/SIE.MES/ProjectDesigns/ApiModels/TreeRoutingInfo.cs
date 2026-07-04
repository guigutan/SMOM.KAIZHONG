using SIE.Core.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ApiModels
{
    /// <summary>
    /// 工艺资料-产品工艺路线
    /// </summary>
    [Serializable]
    public class TreeRoutingInfo
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工艺资料Id
        /// </summary>
        public double DesignProductTreeId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public WorkOrderType? Type { get; set; }

        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 工艺路线Id
        /// </summary>
        public double? RoutingId { get; set; }

        /// <summary>
        /// 版本Id
        /// </summary>
        public double? RoutingVersionId { get; set; }
    }

    /// <summary>
    /// 工艺资料-产品工艺路线导入信息
    /// </summary>
    [Serializable]
    public class TreeRoutingImpInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 项目编码
        /// </summary>
        public string ProjectMaintainCode { get; set; }

        /// <summary>
        /// 项目产品编码
        /// </summary>
        public string ProjectProductCode { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 层级产品编码
        /// </summary>
        public string ProductCode { get; set; }
    }

    /// <summary>
    /// 工艺资料-产品工艺路线工序明细导入信息
    /// </summary>
    [Serializable]
    public class TreeRoutingDtlImpInfo
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public int Index { get; set; }
    }
}
