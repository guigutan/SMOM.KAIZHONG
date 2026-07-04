using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP.Entity
{
    [Serializable]
    public class UnloadItemDefect
    {
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get; set;
        }

        /// <summary>
        /// 不良Id
        /// </summary>
        public double DefectId
        {
            get; set;
        }
        /// <summary>
        /// 不良
        /// </summary>
        public Defect Defect
        {
            get; set;
        }

        /// <summary>
        /// 不良列表Id
        /// </summary>
        public double UnloadItemId
        {
            get; set;
        }
        /// <summary>
        /// 不良列表
        /// </summary>
        public UnloadItem UnloadItem
        {
            get; set;
        }
    }
}
