using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzReport.Datas
{
    [Serializable]
    public class OeeData
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
        /// 资源
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// 计划开机时间(m)
        /// </summary>
        public decimal PlanTime { get; set; }

        /// <summary>
        /// 正常生产时间(m)
        /// </summary>
        public decimal NormalTime { get; set; }

        /// <summary>
        /// 异常停线时间(m)
        /// </summary>
        public decimal AbnormalityTime { get; set; }

        /// <summary>
        /// 理论生产时间(m)
        /// </summary>
        public decimal TheoryTime { get; set; }
        
        /// <summary>
        /// 良品数量
        /// </summary>
        public decimal GoodQty { get; set; }

        /// <summary>
        /// 总生产数量
        /// </summary>
        public decimal TotalQty { get; set; }

        /// <summary>
        /// A
        /// </summary>
        public decimal A { get; set; }

        /// <summary>
        /// P
        /// </summary>
        public decimal P { get; set; }

        /// <summary>
        /// Q
        /// </summary>
        public decimal Q { get; set; }

        /// <summary>
        /// OEE
        /// </summary>
        public decimal Oee { get; set; }
    }

    [Serializable]
    public class EquipResource
    { 
        public double EquipmentId { get; set; }

        public string Process { get; set; }

        public double ProcessId { get; set; }

        public double ResourceId { get; set; }

        public string Resource { get; set; }
    }

    [Serializable]
    public class WoReportQty
    {
        public string Wo { get; set; }

        public decimal ReportQty { get; set; }

        public string Process { get; set; }

        public double ProcessId { get; set; }

        public double ResourceId { get; set; }

        public string Resource { get; set; }

        public decimal GoodQty { get; set; }

        public decimal ProcessQty { get; set; }
    }


}
