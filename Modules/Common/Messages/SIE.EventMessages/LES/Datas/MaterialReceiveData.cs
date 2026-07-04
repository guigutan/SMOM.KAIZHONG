using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.LES.Datas
{
    /// <summary>
    /// 物料接收细实体
    /// </summary>
    public class MaterialReceiveData
    {
        /// <summary>
        /// 库存组织
        /// </summary>
        public int InvOrgId { get; set; }

        /// <summary>
        /// 发运单号
        /// </summary>
        public string SoNo { get; set; }
        /// <summary>
        /// 来源单号/备料单号
        /// </summary>
        public string SourceNo { get; set; }

        /// <summary>
        /// 生产部门编码
        /// </summary>
        public string EnterpriseCode { get; set; }   

        /// <summary>
        /// 发运分配行ID列表
        /// </summary>
        public List<string> SoLineNos { get; set; }
    }
}
