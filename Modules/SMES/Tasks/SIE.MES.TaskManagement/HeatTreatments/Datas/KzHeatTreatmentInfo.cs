using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.HeatTreatments.Datas
{
    [Serializable]
    public class KzHeatTreatmentInfo
    {
        /// <summary>
        /// 条码号
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 旧料号
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 物料编码
        /// </summary>
        public string MaterialCode { get; set; }

        /// <summary>
        /// 设备实际名称
        /// </summary>
        public string DevName { get; set; }

        /// <summary>
        /// 作业类型(1=入炉，2=出炉)
        /// </summary>
        public OperationType OperationType { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Count00 { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate { get; set; }

    }
}
