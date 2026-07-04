using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.MES.DashBoard.TeamManagement
{
    public enum DateType
    {
        /// <summary>
        /// 年
        /// </summary>
        [Label("年")]
        Year,

        /// <summary>
        /// 月
        /// </summary>
        [Category("MonthDay")]
        [Label("月")]
        Month,

        /// <summary>
        /// 日
        /// </summary>
        [Category("MonthDay")]
        [Label("日")]
        Day,

        /// <summary>
        /// 周
        /// </summary>     
        [Label("周")]
        Week,
    }
}
