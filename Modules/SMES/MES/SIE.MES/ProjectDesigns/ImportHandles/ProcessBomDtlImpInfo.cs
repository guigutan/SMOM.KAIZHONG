using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ProjectDesigns.ImportHandles
{
    /// <summary>
    /// 项目号需求设计-产品工艺路线设置工序Bom导入类
    /// </summary>
    [Serializable]
    public class ProcessBomDtlImpInfo
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
        /// 层级
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// 层级产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 单位用量
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 顺序
        /// </summary>
        public string ProcessIndex { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DataRow DetailInfo { get; set; }
    }

    /// <summary>
    /// 导入验证结果
    /// </summary>
    [Serializable]
    public class ProcessBomDtlImpResult
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public ProcessBomDtlImpResult()
        {
            Pass = true;
            Error = new StringBuilder();
        }

        /// <summary>
        /// 验证通过
        /// </summary>
        public bool Pass { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public StringBuilder Error { get; set; }

        /// <summary>
        /// 项目号产品工艺路线设置Id
        /// </summary>
        public double DesignTreeRoutingId { get; set; }

        /// <summary>
        /// 工序清单Id
        /// </summary>
        public double RoutingProcessId { get; set; }

        /// <summary>
        /// 物料id
        /// </summary>
        public double MaterialId { get; set; }

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } 
    }
}
