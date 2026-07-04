using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 生产效率报表
    /// </summary>
    [Serializable]
    public class ProductionEfficiencyData
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
        /// 部门(厂部)
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        /// <summary>
        /// 车间产出总人员工时（h）
        /// </summary>
        public decimal Putput { get; set; }

        /// <summary>
        /// 车间出勤人员总工时（h）
        /// </summary>
        public decimal Attendance { get; set; }

        /// <summary>
        /// 产能利用率
        /// </summary>
        public decimal Rate { get; set; }

    }
}
