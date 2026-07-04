using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models
{
   public class DefectGrade
    {
        public double Id
        {
            get; set;
        }

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// 严重度
        /// </summary>
        public DefectSeverity DefectSeverity
        {
            get; set;
        }
    }
}
