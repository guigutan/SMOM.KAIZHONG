using SIE.Common.ImportHelper;
using SIE.Defects;
using SIE.Defects.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.ErpInfoDatas;
using SIE.ERPInterface.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download.Defects
{
    /// <summary>
    /// 缺陷控制器
    /// </summary>
    public class DownloadDefectController : DomainController
    {
        #region 缺陷分类
        /// <summary>
        /// 从API下载缺陷分类到业务表
        /// </summary>
        /// <param name="defectCategoryDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadDefectCategoryToBusiness(List<DefectCategoryData> defectCategoryDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<DefectCategoryData>(
                defectCategoryDatas,
                p => this.SaveDefectCategory(p.OrderByLastUpdateDate()),
                JobType.DefectCategory,
                invOrg);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="datas"></param>
        private List<ErpErrorData> SaveDefectCategory(List<DefectCategoryData> datas)
        {
            var errors = new List<ErpErrorData>();

            var codeList = datas.Select(p => p.Code).Distinct().ToList();
            var categoryList = RT.Service.Resolve<DefectController>().GetDefectCategoryByCodes(codeList);
            var categoryDic = categoryList.ToDictionary(p => p.Code);

            foreach (var p in datas)
            {
                var error = new ErpErrorData();
                error.Infkey = p.Infkey;

                try
                {
                    //删除
                    if (p.IsDelete && categoryDic.ContainsKey(p.Code))
                    {
                        categoryDic[p.Code].PersistenceStatus = PersistenceStatus.Deleted;
                        RF.Save(categoryDic[p.Code]);
                        continue;
                    }
                    else if (categoryDic.ContainsKey(p.Code))
                    {
                        //已存在，不可新增
                        error.ErrMsg = $"新增失败，缺陷分类[{p.Code}]已存在。";
                        errors.Add(error);
                        continue;
                    }
                    else
                    {
                        //新增
                        var newEntity = GenerateNewDefectCategory(p);
                        RF.Save(newEntity);
                    }

                }
                catch (Exception ex)
                {
                    error.ErrMsg = ex.Message;
                    errors.Add(error);
                }
            }
            return errors;
        }

        /// <summary>
        /// 创建缺陷分类 
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        private DefectCategory GenerateNewDefectCategory(DefectCategoryData p)
        {
            return new DefectCategory()
            {
                Code = p.Code,
                Description = p.Description
            };
        }
        #endregion

        #region 缺陷等级
        /// <summary>
        /// 从API下载缺陷等级到业务表
        /// </summary>
        /// <param name="defectGradeDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadDefectGradeToBusiness(List<DefectGradeData> defectGradeDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<DefectGradeData>(
                defectGradeDatas,
                p => this.SaveDefectGrade(p.OrderByLastUpdateDate()),
                JobType.DefectGrade,
                invOrg);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="datas"></param>
        private List<ErpErrorData> SaveDefectGrade(List<DefectGradeData> datas)
        {
            var errors = new List<ErpErrorData>();

            var nameList = datas.Select(p => p.Name).Distinct().ToList();
            var gradeList = RT.Service.Resolve<DefectController>().GetDefectGradeByNames(nameList);
            var gradeDic = gradeList.ToDictionary(p => p.Name);
            var defectSeverityDic = ImportExtension.GetEnumLabel(typeof(DefectSeverity), string.Empty);

            foreach (var p in datas)
            {
                var error = new ErpErrorData();
                error.Infkey = p.Infkey;

                try
                {
                    //删除
                    if (p.IsDelete && gradeDic.ContainsKey(p.Name))
                    {
                        gradeDic[p.Name].PersistenceStatus = PersistenceStatus.Deleted;
                        RF.Save(gradeDic[p.Name]);
                        continue;
                    }
                    else if (gradeDic.ContainsKey(p.Name))
                    {
                        //已存在，不可新增
                        error.ErrMsg = $"新增失败，缺陷等级[{p.Name}]已存在。";
                        errors.Add(error);
                        continue;
                    }
                    else
                    {
                        //新增
                        var newEntity = GenerateNewDefectGrade(p, defectSeverityDic);
                        RF.Save(newEntity);
                    }

                }
                catch (Exception ex)
                {
                    error.ErrMsg = ex.Message;
                    errors.Add(error);
                }
            }
            return errors;
        }

        /// <summary>
        /// 创建缺陷等级
        /// </summary>
        /// <param name="p"></param>
        /// <param name="defectSeverityDic"></param>
        /// <returns></returns>
        private DefectGrade GenerateNewDefectGrade(DefectGradeData p, Dictionary<string, Enum> defectSeverityDic)
        {
            ValidDefectGradeData(p, defectSeverityDic);
            var severity = defectSeverityDic[p.DefectSeverity];
            return new DefectGrade()
            {
                Name = p.Name,
                DefectSeverity = (DefectSeverity)severity
            };
        }

        /// <summary>
        /// 校验缺陷等级数据
        /// </summary>
        /// <param name="p"></param>
        /// <param name="defectSeverityDic"></param>
        /// <exception cref="ValidationException"></exception>
        private void ValidDefectGradeData(DefectGradeData p, Dictionary<string, Enum> defectSeverityDic)
        {
            if (p.DefectSeverity.IsNullOrEmpty())
                throw new ValidationException("严重度不能为空.".L10N());
            if (!defectSeverityDic.ContainsKey(p.DefectSeverity))
            {
                var messageTip = string.Concat("严重度只能选择".L10N(), string.Join(",", defectSeverityDic.Keys));
                throw new ValidationException(messageTip);
            }
        }
        #endregion

        #region 缺陷代码
        /// <summary>
        /// 从API下载缺陷代码到业务表
        /// </summary>
        /// <param name="defectCodeDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadDefectCodeToBusiness(List<DefectCodeData> defectCodeDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<DefectCodeData>(
                defectCodeDatas,
                p => this.SaveDefects(p.OrderByLastUpdateDate()),
                JobType.DefectCode,
                invOrg);
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="datas"></param>
        private List<ErpErrorData> SaveDefects(List<DefectCodeData> datas)
        {
            var errors = new List<ErpErrorData>();

            var codeList = datas.Select(p => p.Code).Distinct().ToList();
            DefectController defectController = RT.Service.Resolve<DefectController>();
            var defects = defectController.GetDefectsByCodes(codeList);
            var defectDic = defects.ToDictionary(p => p.Code);
            var defectGradeDic = defectController.GetAllDefectGrade().ToDictionary(p => p.Name);
            var defectCategoryDic = defectController.GetAllDefectCategory().ToDictionary(p => p.Code);
            var qualityTypeDic = ImportExtension.GetEnumLabel(typeof(QualityType), string.Empty);

            foreach (var p in datas)
            {
                var error = new ErpErrorData();
                error.Infkey = p.Infkey;

                try
                {
                    //删除
                    if (p.IsDelete && defectDic.ContainsKey(p.Code))
                    {
                        defectDic[p.Code].PersistenceStatus = PersistenceStatus.Deleted;
                        RF.Save(defectDic[p.Code]);
                        continue;
                    }
                    else if (defectDic.ContainsKey(p.Code))
                    {
                        //已存在，不可新增
                        error.ErrMsg = $"新增失败，缺陷代码[{p.Code}]已存在。";
                        errors.Add(error);
                        continue;
                    }
                    else
                    {
                        //新增
                        ValidDefectData(p, defectGradeDic, defectCategoryDic, qualityTypeDic);
                        var newEntity = GenerateNewDefectCode(p, defectGradeDic, defectCategoryDic, qualityTypeDic);
                        RF.Save(newEntity);
                    }

                }
                catch (Exception ex)
                {
                    error.ErrMsg = ex.Message;
                    errors.Add(error);
                }
            }
            return errors;
        }

        /// <summary>
        /// 创建缺陷代码
        /// </summary>
        /// <param name="p"></param>
        /// <param name="defectGradeDic"></param>
        /// <param name="defectCategoryDic"></param>
        /// <param name="qualityDic"></param>
        /// <returns></returns>
        private Defect GenerateNewDefectCode(DefectCodeData p, Dictionary<string, DefectGrade> defectGradeDic, Dictionary<string, DefectCategory> defectCategoryDic, Dictionary<string, Enum> qualityDic)
        {
            return new Defect()
            {
                Code = p.Code,
                Description = p.Description,
                DefectCategoryId = defectCategoryDic[p.DefectCategoryCode].Id,
                DefectGradeId = defectGradeDic[p.DefectGradeName].Id,
                QualityType = (QualityType)qualityDic[p.DefectQualityType]
            };
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="p"></param>
        /// <param name="defectGradeDic"></param>
        /// <param name="defectCategoryDic"></param>
        /// <param name="qualityDic"></param>
        /// <exception cref="ValidationException"></exception>
        private void ValidDefectData(DefectCodeData p, Dictionary<string, DefectGrade> defectGradeDic, Dictionary<string, DefectCategory> defectCategoryDic, Dictionary<string, Enum> qualityDic)
        {
            if (p.DefectGradeName.IsNullOrEmpty())
                throw new ValidationException("缺陷等级不能为空".L10N());
            if (p.DefectCategoryCode.IsNullOrEmpty())
                throw new ValidationException("缺陷分类不能为空".L10N());
            if (p.DefectQualityType.IsNullOrEmpty())
                throw new ValidationException("质量分类不能为空".L10N());

            if (!defectGradeDic.ContainsKey(p.DefectGradeName))
            {
                throw new ValidationException("缺陷等级[{0}]不存在".L10nFormat(p.DefectGradeName));
            }
            if (!defectCategoryDic.ContainsKey(p.DefectCategoryCode))
            {
                throw new ValidationException("缺陷分类[{0}]不存在".L10nFormat(p.DefectCategoryCode));
            }
            if (!qualityDic.ContainsKey(p.DefectQualityType))
            {
                var messageTip = string.Concat("质量分类只能选择".L10N(), string.Join(",", qualityDic.Keys));
                throw new ValidationException(messageTip);
            }
        }
        #endregion
    }
}
