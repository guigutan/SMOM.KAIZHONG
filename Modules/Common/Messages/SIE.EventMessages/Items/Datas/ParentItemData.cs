using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.Items.Datas
{
    /// <summary>
    /// 父级物料数据
    /// </summary>
    [Serializable]
    public class ParentItemData
    {
        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 净重
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// 净重单位
        /// </summary>
        public string WeightUnit { get; set; }

        /// <summary>
        /// 规格型号
        /// </summary>
        public string SpecificationModel { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription { get; set; }
    }
}
