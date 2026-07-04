using System;
using System.Collections.Generic;

namespace SIE.EMS.Report.SparePartMitReports
{
    /// <summary>
    /// 备件库综合报表数据对象
    /// </summary>
    [Serializable]
    public class SparePartMixReportInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SparePartMixReportInfo()
        {
            this.ClounNameList = new List<string>();
            this.Datas = new List<List<string>>();
            this.ExWarehouseList = new List<MonthValue>();
            this.TurnoverRateList = new List<MonthValue>();
        }
        /// <summary>
        /// 动态列名称
        /// </summary>
        public List<string> ClounNameList { get; set; }

        /// <summary>
        /// 结果数据
        /// </summary>
        public List<List<string>> Datas { get; set; }


        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; } = true;

        /// <summary>
        /// 错误信息
        /// </summary>
        public string err { get; set; }

        /// <summary>
        /// 周转率折线图统计数据
        /// </summary>
        public List<MonthValue> TurnoverRateList { get; set; }


        /// <summary>
        ///出库统计
        /// </summary>
        public List<MonthValue> ExWarehouseList { get; set; }

    }

    /// <summary>
    /// 月份与值对象
    /// </summary>
    [Serializable]
    public class MonthValue
    {
       /// <summary>
       /// 构造方法
       /// </summary>
       /// <param name="month"></param>
       /// <param name="value"></param>
        public MonthValue(string month,decimal value)
        {
            this.Month = month;
            this.Value = value;
        }
        /// <summary>
        /// 月份
        /// </summary>
        public string Month { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public decimal Value { get; set; }
    }
}
