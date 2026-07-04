using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PackingQcs.Data
{
    /// <summary>
    /// 替换
    /// </summary>
    [Serializable]
    public class ConnectorSnReplaceData
    {

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 旧物料号
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// 工序标签
        /// </summary>
        public string BatchNo { get; set; }

    }
}
