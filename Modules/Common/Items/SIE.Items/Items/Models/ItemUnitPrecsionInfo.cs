using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Items
{
    /// <summary>
    ///  单位对象
    /// </summary>
    [Serializable]
    public class ItemUnitPrecsionInfo
    {
        /// <summary>
        /// 精度
        /// </summary>
        public double unitPrecsion { get; set; }

        /// <summary>
        /// 进位
        /// </summary>
        public int carry { get; set; }
    }
}