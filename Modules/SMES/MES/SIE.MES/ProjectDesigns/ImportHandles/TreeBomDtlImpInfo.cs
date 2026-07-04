using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ImportHandles
{
    /// <summary>
    /// 项目号需求设计-产品Bom导入类
    /// </summary>
    [Serializable]
    public class TreeBomDtlImpInfo
    {
        /// <summary>
        /// 项目号编码
        /// </summary>
        public string ProjectMaintainCode { get; set; }

        /// <summary>
        /// 项目号产品编码
        /// </summary>
        public string ProjectProductCode { get; set; }

        /// <summary>
        /// 产品Bom编码
        /// </summary>
        public string BomCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public string UnitQty { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public string LossRate { get; set; }

        /// <summary>
        /// 是否反冲物料
        /// </summary>
        public string IsRecoilItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DataRow DetailInfo { get; set; }
    }

    /// <summary>
    /// 项目号导入验证结果
    /// </summary>
    [Serializable]
    public class TreeBomDtlImpResult
    {
        /// <summary>
        /// 构造
        /// </summary>
        public TreeBomDtlImpResult()
        {
            Pass = true;
            Error = new StringBuilder();
        }

        /// <summary>
        /// 是否通过验证
        /// </summary>
        public bool Pass { get; set; }

        /// <summary>
        /// 产品BomId
        /// </summary>
        public double BomId { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId { get; set; }

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal UnitQty { get; set; }

        /// <summary>
        /// 损耗率
        /// </summary>
        public decimal LossRate { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public StringBuilder Error { get; set; }
    }
}
