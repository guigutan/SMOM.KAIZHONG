using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ApiModels
{
    /// <summary>
    /// 工艺资料-产品bom信息
    /// </summary>
    [Serializable]
    public class TreeBomInfo
    {
        /// <summary>
        /// 工艺资料Id
        /// </summary>
        public double DesignProductTreeId { get; set; }

        /// <summary>
        /// 项目号需求设计Id
        /// </summary>
        public double DesignDetailId { get; set; }

        /// <summary>
        /// 项目号
        /// </summary>
        public double ProjectMaintainId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public double ProductId { get; set; }

        /// <summary>
        /// 选择数据Id
        /// </summary>
        public double Id { get; set; }
    }

    /// <summary>
    /// 工艺资料-产品bom导入信息
    /// </summary>
    [Serializable]
    public class TreeBomImpInfo
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
        /// 产品Bom编码
        /// </summary>
        public string BomCode { get; set; }
    }
}
