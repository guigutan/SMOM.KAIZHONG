using SIE.Core.Inspections;
using System;
using System.Collections.Generic;

namespace SIE.EventMessages.PDCA
{
    /// <summary>
    /// 创建PDCA改善报告
    /// </summary>
    [Services.Service(FallbackType = typeof(CreatePdcaReportInfo))]
    public interface ICreatePdcaReportEvent
    {
        /// <summary>
        /// 生成PDCA改善报告
        /// </summary>
        /// <param name="reportEvent">PDCA改善报告参数</param>
        string GeneratePdcaReport(CreatePdcaReportEvent reportEvent);

        /// <summary>
        /// 检查是否存在pdca编码生成规则
        /// </summary>
        /// <returns></returns>
        bool CheckPdcaCodeConfig();
    }

    /// <summary>
    /// 接口的默认实现
    /// </summary>
    class CreatePdcaReportInfo : ICreatePdcaReportEvent
    {
        public string GeneratePdcaReport(CreatePdcaReportEvent reportEvent)
        {
            throw new NotImplementedException("缺少PDCA模块，无法创建PDCA改善报告");
        }
        /// <summary>
        /// 检查是否存在pdca编码生成规则
        /// </summary>
        /// <returns></returns>
        public bool CheckPdcaCodeConfig()
        {
            return false;
        }
    }

    /// <summary>
    /// 生成PDCA改善报告参数
    /// </summary>
    [Serializable]
    public class CreatePdcaReportEvent
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CreatePdcaReportEvent()
        {
            DefectInfoList = new List<DefectInfo>();
        }

        /// <summary>
        /// 检验单Id
        /// </summary>
        public double? BillId { get; set; }

        /// <summary>
        /// 检验单号
        /// </summary>
        public string BillNo { get; set; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BatchNo { get; set; }

        /// <summary>
        /// 批量数
        /// </summary>
        public decimal? BatchQty { get; set; }

        /// <summary>
        /// 物料Id
        /// </summary>
        public double? ItemId { get; set; }

        /// <summary>
        /// 质量分类Id
        /// </summary>
        public double? QualityCategoryId { get; set; }

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double DefectId { get; set; }

        /// <summary>
        /// 问题描述
        /// </summary>
        public string DefectDescription { get; set; }

        /// <summary>
        /// 问题等级
        /// </summary>
        public string DefectLevel { get; set; }
        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkShopId { get; set; }

        /// <summary>
        /// 产线Id
        /// </summary>
        public double? LineId { get; set; }

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        public double? CustomerId { get; set; }

        /// <summary>
        /// 供应商Id
        /// </summary>
        public double? SupplierId { get; set; }

        /// <summary>
        /// 检验类型
        /// </summary>
        public InspectionType? InspectionType { get; set; }

        /// <summary>
        /// 问题清单列表
        /// </summary>
        public List<DefectInfo> DefectInfoList { get; set; }

        /// <summary>
        /// 仓库
        /// </summary>
        public double? WarehouseId { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public double? FactoryId { get; set; }
    }
}