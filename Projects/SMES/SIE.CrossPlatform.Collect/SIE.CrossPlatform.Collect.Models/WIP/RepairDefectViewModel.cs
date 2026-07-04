using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models.WIP
{
    [Serializable]
    public class RepairDefectViewModel
    {

        public RepairDefectViewModel()
        {
            this.MeasureList = new List<RepairMeasure>();
            this.ResponsibilityList = new List<DefectResponsibility>();
        }
        /// <summary>
        /// 数据Id
        /// </summary>
        public string Id
        {
            get; set;
        }

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get; set;
        }

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get; set;
        }

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn
        {
            get; set;
        }

        /// <summary>
        /// 产品缺陷记录ID
        /// </summary>
        public double WipProductDefectId
        {
            get; set;
        }

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public WipProductDefect WipProductDefect
        {
            get; set;
        }

        /// <summary>
        /// 产品维修记录Id
        /// </summary>
        public double? WipProductRepairId
        {
            get; set;
        }

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
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
        /// 缺陷Id
        /// </summary>
        public double DefectId
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
        /// 缺陷代码
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
        /// 缺陷位置
        /// </summary>
        public string DefectLocation
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
        /// 维修措施描述
        /// </summary>
        public string MeasureDesc
        {
            get; set;
        }

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public string Responsibility
        {
            get; set;
        }

        /// <summary>
        /// 维修位置
        /// </summary>
        public string RepairLocation
        {
            get; set;
        }
        /// <summary>
        /// 实际缺陷Id
        /// </summary>
        public double? ActualDefectId
        {
            get; set;
        }


        /// <summary>
        /// 实际缺陷
        /// </summary>
        public Defect ActualDefect
        {
            get; set;
        }

        /// <summary>
        /// 实际缺陷编码
        /// </summary>
        public string ActualDefectCode
        {
            get; set;
        }
        /// <summary>
        /// 实际缺陷描述
        /// </summary>
        public string ActualDefectDesc
        {
            get; set;
        }
        /// <summary>
        /// 换料条码
        /// </summary>
        public string ReloadBarcode
        {
            get; set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get; set;
        }

        /// <summary>
        /// 维修员Id
        /// </summary>
        public double MaintenanceManId
        {
            get; set;
        }

        /// <summary>
        /// 维修员
        /// </summary>
        public string Maintenance
        {
            get; set;
        }
        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime? RepairDate
        {
            get; set;
        }

        /// <summary>
        /// 是否新增
        /// </summary>
        public bool IsNewAdd
        {
            get; set;
        }

        /// <summary>
        /// 实际缺陷只读
        /// </summary>
        public bool ActualDefectEnable
        {
            get; set;
        }

        /// <summary>
        /// 维修措施
        /// </summary>
        public List<RepairMeasure> MeasureList
        {
            get; set;
        }

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public List<DefectResponsibility> ResponsibilityList
        {
            get; set;
        }

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get; set;
        }

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason
        {
            get; set;
        }

        /// <summary>
        /// 是否修好
        /// </summary>
        public bool IsFixed
        {
            get; set;
        }

        /// <summary>
        /// 维修措施
        /// </summary>
        public string MeasureBarcode
        {
            get; set;
        }

        /// <summary>
        /// 维修方案Id
        /// </summary>
        public double? RepairSolutionId
        {
            get; set;
        }

        /// <summary>
        /// 维修方案
        /// </summary>
        public DefectRepairSolution RepairSolution
        {
            get; set;
        }

        /// <summary>
        /// 换料条码
        /// </summary>
        public string ReloadSn
        {
            get; set;
        }

        /// <summary>
        /// 检验项目名称
        /// </summary>
        public string InspItemName
        { get; set; }
    }

    /// <summary>
    /// 缺陷维修方案
    /// </summary>
    [Serializable]
    public class DefectRepairSolution
    {

        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 维修方案
        /// </summary>
        public string Solution { get; set; }

        /// <summary>
        /// 推荐次数
        /// </summary>
        public int? RecommendQty { get; set; }

        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        public double DefectId { get; set; }
        /// <summary>
        /// 缺陷代码
        /// </summary>
        public Defect Defect { get; set; }
    }

    /// <summary>
    /// 维修措施
    /// </summary>
    public class RepairMeasure
    {

        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }
    }

    /// <summary>
    /// 缺陷责任
    /// </summary>
    [Serializable]
    public class DefectResponsibility
    {

        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 数据库状态
        /// </summary>
        public PersistenceStatus PersistenceStatus { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }


        /// <summary>
        /// 描述
        /// </summary>        
        public string Description { get; set; }

        /// <summary>
        /// 缺陷责任分类Id
        /// </summary>
        public double CategoryId { get; set; }

        /// <summary>
        /// 缺陷责任分类
        /// </summary>
        public DefectResponsibilityCategory Category { get; set; }

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string CategoryDescription { get; set; }

    }

    /// <summary>
    ///缺陷责任分类
    /// </summary>
    [Serializable]
    public class DefectResponsibilityCategory
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

       
        public double? TreePId { get; set; }
    }
}
