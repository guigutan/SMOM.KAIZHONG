using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.Items.Datas
{
    /// <summary>
    /// 客户料码数据
    /// </summary>
    [Serializable]
    public class ItemCusotmerRelationData
    {
        /// <summary>
        /// 客户
        /// </summary>
        public string Attribute2 { get; set; }

        /// <summary>
        /// 属性1(其他特性字段)
        /// </summary>
        public string Attribute1 { get; set; }
    }
}
