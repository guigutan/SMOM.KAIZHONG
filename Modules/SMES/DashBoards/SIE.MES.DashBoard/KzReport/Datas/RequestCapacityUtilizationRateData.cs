using SIE.Core.Enums;
using SIE.MES.DashBoard.KzReport.ProductionProcesss.Enums;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class RequestCapacityUtilizationRateData
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
        /// 工序
        /// </summary>
        public string ProcessCode { get; set; }
        /// <summary>
        /// 年份
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        ///月份 1-12
        /// </summary>
        public string Month { get; set; }
        /// <summary>
        /// 查月时传0
        /// 传周时 1-5
        /// 传日时1-31
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public CapacityDataType CapacityDataType { get; set; }
    }

}
