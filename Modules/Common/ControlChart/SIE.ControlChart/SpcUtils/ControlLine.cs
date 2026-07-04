using System;

namespace SIE.ControlChart.SpcUtils
{
    /// <summary>
    /// 控制线
    /// </summary>
    [Serializable]
    public class ControlLine
    {
        /// <summary>
        /// 上控制线
        /// </summary>
        public double Ucl { get; set; }
        /// <summary>
        /// 下控制线
        /// </summary>
        public double Lcl { get; set; }
        /// <summary>
        /// 中线
        /// </summary>
        public double Cl { get; set; }

        /// <summary>
        /// 规格上限
        /// </summary>
        public double? Usl { get; set; }

        /// <summary>
        /// 规格下限
        /// </summary>
        public double? Lsl { get; set; }

        /// <summary>
        /// 均值图上CB分区值
        /// </summary>
        public double Ucb
        {
            get
            {
                return Cl + (Ucl - Cl) / 3;
            }
        }
        /// <summary>
        /// 均值图上BA分区值
        /// </summary>
        public double Uba
        {
            get
            {
                return Cl + (Ucl - Cl) / 3 * 2;
            }
        }
        /// <summary>
        /// 均值图下CB分区值
        /// </summary>
        public double Lcb
        {
            get
            {
                return Cl - (Cl - Lcl) / 3;
            }
        }
        /// <summary>
        /// 均值图下BA分区值
        /// </summary>
        public double Lba
        {
            get
            {
                return Cl - (Cl - Lcl) / 3 * 2;
            }
        }

    }
}
