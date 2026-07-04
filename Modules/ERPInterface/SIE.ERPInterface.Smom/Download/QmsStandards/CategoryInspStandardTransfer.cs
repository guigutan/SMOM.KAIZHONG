using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Inspections;
using SIE.CSM.ItemInspCharacteristicses;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas.ErpInfoDatas.InspStandards;

namespace SIE.ERPInterface.Smom.Download.QmsStandards
{
    /// <summary>
    /// 分类检验标准校验
    /// </summary>
    public class CategoryInspStandardTransfer : InspStandardTransferBase
    {
        /// <summary>
        /// 质量分类"质量分类编码"-"质量分类ID"
        /// </summary>
        private Dictionary<string, double> dicQualityCategory;

        /// <summary>
        /// 创建物料检验标准
        /// </summary>
        /// <param name="stdDataBase"></param>
        /// <returns></returns>
        //protected override DataEntity GenerateNewInspStandard(InspStandardDataBase stdDataBase)
        //{
        //    var stdData = stdDataBase as CategoryInspStandardData;
        //    if (stdData == null)
        //        throw new ValidationException("检验标准格式有误".L10N());
        //    var std = new CategoryInspectionStandard()
        //    {
        //        Name = stdData.Name,
        //        InspectionType = (InspectionType)dicInspectionTypeRange[stdData.InspectionType],
        //        Version = stdData.Version,
        //        QualityCategoryId = dicQualityCategory[stdData.QualityCategory],
        //        Remark = stdData.Remark,
        //    };
        //    if (std.InspectionType == null)
        //        throw new ValidationException("检验类型不能为空".L10N());
        //    if (stdData.Customer.IsNotEmpty())
        //        std.CustomerId = dicCustomerName[stdData.Customer];
        //    std.State = State.Enable;

        //    //检验标准明细
        //    if (stdData.DetailList.IsNotEmpty())
        //    {
        //        foreach (var dtlData in stdData.DetailList)
        //        {
        //            CategoryInspectionDetail detail = GenerateDetail(stdData, std, dtlData);
        //            std.DetailList.Add(detail);
        //        }
        //    }
        //    return std;
        //}

        /// <summary>
        /// 生成明细
        /// </summary>
        /// <param name="stdData"></param>
        /// <param name="std"></param>
        /// <param name="dtlData"></param>
        /// <returns></returns>
        //private CategoryInspectionDetail GenerateDetail(CategoryInspStandardData stdData, CategoryInspectionStandard std, InspStandardDataDetailBase dtlData)
        //{
        //    var detail = new CategoryInspectionDetail()
        //    {
        //        Name = dtlData.Name,
        //        Category = dtlData.Category,
        //        InspectionCategory = (InspectionCategory)dicInspectionCategoryRange[dtlData.InspectionCategory],
        //        InspectionBasis = dtlData.InspectionBasis,
        //        TestTool = dtlData.TestTool,
        //        CheckTag = (CheckTag)dicCheckTagRange[dtlData.CheckTag],
        //        InspectionModeId = dicInspectionModeRange[stdData.InspectionType][dtlData.InspectionMode],
        //        IsSuitable = dtlData.IsSuitable,
        //        DefectGradeId = dicDefectGradeRange[dtlData.DefectGrade],
        //        TechnicalRequirements = dtlData.TechnicalRequirements,
        //        SamplingStepId = dicSamplingStep[dtlData.SamplingStep]?.Id,
        //        EffectiveStartTime = dtlData.EffectiveStartTime,
        //        EffectiveEndTime = dtlData.EffectiveEndTime,
        //    };
        //    //周期检相关字段
        //    if (detail.InspectionCategory == InspectionCategory.RecurInsp)
        //    {
        //        detail.PeriodType = (PeriodType)dicPeriodTypeRange[dtlData.PeriodType];
        //        detail.Period = dtlData.Period;
        //    }
        //    //工序
        //    if (dtlData.Process.IsNotEmpty() && RT.Service.Resolve<InspectionTypeController>().IsPqcInspType(std.InspectionType.Value))
        //    {
        //        detail.ProcessId = dicProcess[dtlData.Process];
        //    }
        //    //定量值
        //    if (detail.CheckTag == CheckTag.Quantitative)
        //    {
        //        if (dtlData.LimitMax.IsNotEmpty())
        //        {
        //            var compare = dtlData.LimitMax.Substring(0, 1);
        //            detail.LimitMaxCompare = (CompareType)dicLimitMaxCompare[compare];
        //            detail.LimitMax = Convert.ToDecimal(dtlData.LimitMax.Substring(1));
        //        }
        //        if (dtlData.LimitLow.IsNotEmpty())
        //        {
        //            var compare = dtlData.LimitLow.Substring(0, 1);
        //            detail.LimitLowCompare = (CompareType)dicLimitLowComare[compare];
        //            detail.LimitLow = Convert.ToDecimal(dtlData.LimitLow.Substring(1));
        //        }
        //        if (dtlData.Unit.IsNotEmpty())
        //            detail.UnitId = dicUnit[dtlData.Unit];
        //    }

        //    return detail;
        //}

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="p"></param>
        /// <exception cref="ValidationException"></exception>
        public override void ValidData(InspStandardDataBase p)
        {
            ValidInspectionType(p);//校验检验类型
            ValidQualityCategory(p as CategoryInspStandardData);
            base.ValidData(p);
        }

        /// <summary>
        /// 验证质量分类
        /// </summary>
        /// <returns>返回是否验证通过</returns>
        private void ValidQualityCategory(CategoryInspStandardData p)
        {
            if (p.QualityCategory.IsNullOrEmpty())
                throw new ValidationException("质量分类不能为空。".L10N());
            //BussinessDataValid.ValidQualityCategory(ref dicQualityCategory, p.QualityCategory, out string messageTip);
            //if (messageTip.IsNotEmpty()) throw new ValidationException("质量分类".L10N() + messageTip);
        }
    }
}
