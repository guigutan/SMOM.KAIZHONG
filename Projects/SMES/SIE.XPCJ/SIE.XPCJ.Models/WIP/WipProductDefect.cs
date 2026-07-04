using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.WIP.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models.WIP
{
    [Serializable]
    public class WipProductDefect
    {
        public WipProductDefect()
        {
            this.ResponsibilityList = new List<WipDefectResponsibility>();
            this.MeasureList = new List<WipDefectMeasure>();
        }


        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get; set;
        }


        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string Location
        {
            get; set;
        }

        /// <summary>
        /// 是否维修过
        /// </summary>
        public bool IsFixed
        {
            get; set;
        }


        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime? FixedDate
        {
            get; set;
        }

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType Result
        {
            get; set;
        }

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get; set;
        }

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get; set;
        }

        /// <summary>
        /// 资源
        /// </summary>
        public double ResourceId
        {
            get; set;
        }

        /// <summary>
        /// 工位Id
        /// </summary>
        public double StationId
        {
            get; set;
        }

        /// <summary>
        /// 维修人Id
        /// </summary>
        public double? FixedById
        {
            get; set;
        }
        /// <summary>
        /// 班次Id
        /// </summary>
        public double ShiftId
        {
            get; set;
        }
        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double? DefectId
        {
            get; set;
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get; set;
        }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDescription
        {
            get; set;
        }

        /// <summary>
        /// 产品版本
        /// </summary>
        public double VersionId
        {
            get; set;
        }

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get; set;
        }

        /// <summary>
        /// 检验项目Id
        /// </summary>
        public double? InspectionItemId
        {
            get; set;
        }
        /// <summary>
        /// 是否误判
        /// </summary>
        public bool IsMisjudgment
        {
            get; set;
        }

        /// <summary>
        /// 板号
        /// </summary>
        public int? BoardNo
        {
            get; set;
        }

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get; set;
        }

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public List<WipDefectResponsibility> ResponsibilityList
        {
            get; set;
        }
        /// <summary>
        /// 维修措施
        /// </summary>
        public List<WipDefectMeasure> MeasureList
        {
            get; set;
        }

        /// <summary>
        /// 工位名称
        /// </summary>
        public string StationName
        {
            get; set;
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get; set;
        }

        /// <summary>
        /// 资源名称
        /// </summary>
        public string ResourceName
        {
            get; set;
        }

        /// <summary>
        /// 维修人名称
        /// </summary>
        public string EmployeeName
        {
            get; set;
        }

        /// <summary>
        /// 缺陷编码
        /// </summary>
        public string DefectCode
        {
            get; set;
        }

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get; set;
        }

        /// <summary>
        /// 检验项描述
        /// </summary>
        public string InspItemName
        {
            get; set;
        }

    }
}
