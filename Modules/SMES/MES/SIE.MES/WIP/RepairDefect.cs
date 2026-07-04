using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using System;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 缺陷维修
    /// </summary>
    [Serializable]
    public class RepairDefect
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RepairDefect()
        {
            Responsiblities = new EntityList<DefectResponsibility>();
            Measures = new EntityList<RepairMeasure>();
        }

        /// <summary>
        /// 缺陷记录ID
        /// </summary>
        public double? ProductDefectId { get; set; }

        /// <summary>
        /// 是否修好
        /// </summary>
        public bool IsFixed { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty { get; set; }

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason { get; set; }

        /// <summary>
        /// 缺陷责任列表
        /// </summary>
        public EntityList<DefectResponsibility> Responsiblities { get; set; }

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public EntityList<RepairMeasure> Measures { get; set; }
    }
}