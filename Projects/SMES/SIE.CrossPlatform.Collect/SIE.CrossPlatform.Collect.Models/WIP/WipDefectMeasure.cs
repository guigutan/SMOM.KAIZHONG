using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    [Serializable]
    public class WipDefectMeasure
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        public WipProductDefect WipProductDefect { get; set; }

        public PersistenceStatus PersistenceStatus { get; set; }

        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        public double WipProductDefectId { get; set; }

        /// <summary>
        /// 维修措施Id
        /// </summary>
        public double RepairMeasureId
        {
            get; set;
        }

        /// <summary>
        /// 维修措施
        /// </summary>
        public RepairMeasure RepairMeasure
        {
            get; set;
        }

        /// <summary>
        /// 维修措施编码
        /// </summary>
        public string MeasureCode
        {
            get; set;
        }
        /// <summary>
        /// 维修措施名称
        /// </summary>
        public string MeasureName
        {
            get; set;
        }

        /// <summary>
        /// 维修措施描述
        /// </summary>
        public string MeasureDesc
        {
            get; set;
        }

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string DefectLocation
        {
            get; set;
        }
    }
}
