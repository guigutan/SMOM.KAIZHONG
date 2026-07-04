using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ForWinform.ApiModels
{

    /// <summary>
    ///缺陷责任分类
    /// </summary>
    [Serializable]
    public class XPDefectResponsibilityCategory
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 树Id
        /// </summary>
        public double? TreePId { get; set; }
    }
}
