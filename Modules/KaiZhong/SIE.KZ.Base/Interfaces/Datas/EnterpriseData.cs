using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 企业模型接口数据
    /// </summary>
    [Serializable]
    public class EnterpriseData
    {
        /// <summary>
        /// 公司编码(公司编码)
        /// </summary>
        public string BUKRS { get; set; }

        /// <summary>
        /// 公司名称(公司名称)
        /// </summary>
        public string BUTXT { get; set; }

        /// <summary>
        /// 工厂(工厂编码)
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 工厂名称(工厂名称)
        /// </summary>
        public string NAME1 { get; set; }

        /// <summary>
        /// MRP 控制员 (车间编码)
        /// </summary>
        public string DISPO { get; set; }

        /// <summary>
        /// MRP 控制员姓名 (车间名称)
        /// </summary>
        public string DSNAM { get; set; }
    }
}
