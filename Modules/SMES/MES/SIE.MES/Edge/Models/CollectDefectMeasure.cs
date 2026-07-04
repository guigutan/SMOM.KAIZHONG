using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.Edge.Models
{
    /// <summary>
    /// 缺陷维修记录
    /// </summary>
    [Serializable]
    public class CollectDefectMeasure
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        public double WipProductDefectId
        {
            get;
            set;
        }

        /// <summary>
        /// 维修措施Id
        /// </summary>
        public double RepairMeasureId
        {
            get;
            set;
        }

        /// <summary>
        /// 维修措施编码
        /// </summary>
        public string MeasureCode
        {
            get;
            set;
        }

        /// <summary>
        /// 维修措施名称
        /// </summary>
        public string MeasureName
        {
            get;
            set;
        }

        /// <summary>
        /// 维修措施描述
        /// </summary>
        public string MeasureDesc
        {
            get;
            set;
        }
        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string DefectLocation
        {
            get;
            set;
        }
    }
}
