using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.Datas
{
    /// <summary>
    /// Razor推送模板类
    /// </summary>
    [Serializable]
    public class PushRazorModel
    {
        /// <summary>
        /// 条码
        /// </summary>
        public string Sn {  get; set; }


        /// <summary>
        /// 开始工序
        /// </summary>
        public string StartProcess { get; set; }

        /// <summary>
        /// 开始工序状态
        /// </summary>
        public string StartState { get; set; }

        /// <summary>
        /// 超时
        /// </summary>
        public decimal OverTime { get; set; }

        /// <summary>
        /// 结束工序
        /// </summary>
        public string EndProcess { get; set; }

        /// <summary>
        /// 结束状态
        /// </summary>
        public string EndState { get; set; }

        /// <summary>
        /// 有末工序
        /// </summary>
        public bool HasEnd {  get; set; }
    }
}
