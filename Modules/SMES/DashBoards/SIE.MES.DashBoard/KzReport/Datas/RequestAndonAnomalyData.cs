using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    [Serializable]
    public class RequestAndonAnomalyData
    {
        /// <summary>
        /// 产品线
        /// </summary>
        public string ProductLine { get; set; }
        /// <summary>
        /// 厂部名称
        /// </summary>
        public string PlantName { get; set; }
        /// <summary>
        /// 时间范围
        /// </summary>
        public DateRange DateRange { get; set; }
    }
}
