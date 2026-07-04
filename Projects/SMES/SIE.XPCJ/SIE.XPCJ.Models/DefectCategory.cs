using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models
{
   [Serializable]
   public class DefectCategory
    {
        /// <summary>
        ///树形Id
        /// </summary>
        public double? TreePId
        {
            get;
            set;
        }

        public double Id
        {
            get;
            set;
        }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get;
            set;
        }
        
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }
    }
}
