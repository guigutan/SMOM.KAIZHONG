using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP.Entity
{
    /// <summary>
    /// 替代料
    /// </summary>
   [Serializable]
    public class AltItemViewModel
    {
        /// <summary>
        /// 装配清单Id
        /// </summary>
        public double AssemblyDetailId
        {
            get;
            set;
        }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get;
            set;
        }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get;
            set;
        }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get;
            set;
        }

        /// <summary>
        /// 物料扩展属性
        /// </summary>
        public string ItemExtProp
        {
            get;
            set;
        }
        /// <summary>
        /// 替代组合分组
        /// </summary>
        public string AlterGroup
        {
            get;
            set;
        }
    }
}
