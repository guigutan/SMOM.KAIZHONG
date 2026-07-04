using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 投入产出日报表
    /// </summary>
    [Serializable]
    public class InputOutputDailyReportData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 园区名称
        /// </summary>
        public string Park { get; set; }

        /// <summary>
        /// 部门(厂部)
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 计划投入数
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 实际投入数
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        /// 差异
        /// </summary>
        public decimal DiffQty { get; set; }

        /// <summary>
        /// 唯一键
        /// </summary>
        public string Key { get; set; }
    }

    /// <summary>
    /// 明细
    /// </summary>
    [Serializable]
    public class InputOutputDailyReportDtlData
    {
        /// <summary>
        /// 序号
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }

        /// <summary>
        /// 园区名称
        /// </summary>
        public string Park { get; set; }

        /// <summary>
        /// 部门(厂部)
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 计划投入数
        /// </summary>
        public decimal PlanQty { get; set; }

        /// <summary>
        /// 实际投入数
        /// </summary>
        public decimal ActualQty { get; set; }

        /// <summary>
        /// 差异
        /// </summary>
        public decimal DiffQty { get; set; }
    }
}
