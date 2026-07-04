using SIE.Domain;
using SIE.Items;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Tech.Routings.Technologys.Models
{
    /// <summary>
    /// 物料Bom
    /// </summary>
    public class ItemExtBom:ViewModel
    {
        /// <summary>
        /// 物料Id
        /// </summary>
       public double ItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 物料名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public string ItemExtProp { get; set; }

        /// <summary>
        /// 扩展属性名称
        /// </summary>
        public string ItemExtPropName { get; set; }

    }
}
