using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Common.ImportHelper;
using SIE.Core.Inspections;
using SIE.CSM.Customers;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas.ErpInfoDatas.InspStandards;
using SIE.Items;
using SIE.Items.Units;
using SIE.Tech.Processs;

namespace SIE.ERPInterface.Smom.Download.QmsStandards
{
    /// <summary>
    /// 物料检验标准校验基类
    /// </summary>
    public class InspStandardTransferBase
    {
        #region 私有属性

        /// <summary>
        /// 检验类型范围
        /// </summary>
        protected Dictionary<string, Enum> dicInspectionTypeRange;

        /// <summary>
        /// 检验类别范围 名称-编码
        /// </summary>
        protected Dictionary<string, string> dicCategoryRange;

        /// <summary>
        /// 检测标识范围
        /// </summary>
        protected Dictionary<string, Enum> dicCheckTagRange;

        /// <summary>
        /// 检验方式范围 检验类型-(检验方式名称-检验方式ID)
        /// </summary>
        protected Dictionary<string, Dictionary<string, double>> dicInspectionModeRange;

        /// <summary>
        /// 抽样过程
        /// </summary>
        //protected Dictionary<string, SamplingStep> dicSamplingStep;

        /// <summary>
        /// 缺陷等级
        /// </summary>
        protected Dictionary<string, double> dicDefectGradeRange;

        /// <summary>
        /// 单位
        /// </summary>
        protected Dictionary<string, double> dicUnit;

        /// <summary>
        /// 下限比较符
        /// </summary>
        protected Dictionary<string, Enum> dicLimitLowComare;

        /// <summary>
        /// 上限比较符
        /// </summary>
        protected Dictionary<string, Enum> dicLimitMaxCompare;

        /// <summary>
        /// 项目类别范围
        /// </summary>
        protected Dictionary<string, Enum> dicInspectionCategoryRange;

        /// <summary>
        /// 周期类型范围
        /// </summary>
        protected Dictionary<string, Enum> dicPeriodTypeRange;

        /// <summary>
        /// 工序范围 "工序名称"-"工序ID"
        /// </summary>
        protected Dictionary<string, double> dicProcess;

        /// <summary>
        /// 客户信息"客户名称"-"客户ID"
        /// </summary>
        protected Dictionary<string, double> dicCustomerName;

        #endregion

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public DataEntity Transfer(InspStandardDataBase data)
        {
            ValidData(data);
            return GenerateNewInspStandard(data);
        }

        /// <summary>
        /// 创建物料检验标准
        /// </summary>
        /// <param name="stdDataBase"></param>
        /// <returns></returns>
        protected virtual DataEntity GenerateNewInspStandard(InspStandardDataBase stdDataBase)
        {
            return null;
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <param name="p"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ValidData(InspStandardDataBase p)
        {
            ValidInspectionType(p);//校验检验类型
            ValidCustomer(p);
            ValidStandardName(p);

            p.DetailList?.ForEach(detail =>
            {
                ValidName(detail);
                ValidCategory(detail);
                ValidCheckTag(detail);
                ValidInspectionMode(detail, p);
                ValidDefectGrade(detail);
                ValidSamplingStepCode(detail);
                ValidTestTool(detail);
                ValidTechnicalRequirements(detail);
                ValidInspectionBasis(detail);
                ValidLIMIT_LOW(detail);
                ValidLIMIT_MAX(detail);
                ValidUnit(detail);
                ValidInspectionCategory(detail);
                ValidPeriodType(detail);
                ValidProcess(detail, p);
                ValidInspDetail(detail, p);
            });
        }

        /// <summary>
        /// 验证检验类型
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        protected void ValidInspectionType(InspStandardDataBase p)
        {
            if (p.InspectionType.IsNullOrEmpty())
                throw new ValidationException("检验类型不能为空。".L10N());
            //BussinessDataValid.ValidInspectionType(ref dicInspectionTypeRange, null, p.InspectionType, out string messageTip);
            //if (messageTip.IsNotEmpty()) throw new ValidationException(messageTip);
        }

        /// <summary>
        /// 验证标准名称
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        protected void ValidStandardName(InspStandardDataBase p)
        {
            if (p.Name.IsNullOrEmpty())
                throw new ValidationException("标准名称不能为空。".L10N());
        }

        /// <summary>
        /// 验证项目名称
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidName(InspStandardDataDetailBase p)
        {
            if (p.Name.IsNullOrEmpty())
                throw new ValidationException("检验项目不能为空。".L10N());
        }

        /// <summary>
        /// 验证检验工具
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidTestTool(InspStandardDataDetailBase p)
        {
            if (p.TestTool.IsNullOrEmpty())
                throw new ValidationException("检验工具不能为空。".L10N());
        }

        /// <summary>
        /// 验证检验依据
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidInspectionBasis(InspStandardDataDetailBase p)
        {
            if (p.InspectionBasis.IsNullOrEmpty())
                throw new ValidationException("检验依据不能为空。".L10N());
        }

        /// <summary>
        /// 验证技术要求
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidTechnicalRequirements(InspStandardDataDetailBase p)
        {
            if (p.TechnicalRequirements.IsNullOrEmpty())
                throw new ValidationException("技术要求不能为空。".L10N());
        }

        /// <summary>
        /// 验证检验类别
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidCategory(InspStandardDataDetailBase p)
        {
            if (p.Category.IsNullOrEmpty())
                throw new ValidationException("检验类别不能为空。".L10N());
            //BussinessDataValid.ValidCategory(ref dicCategoryRange, p.Category, out string messageTip, "QMS_CATEGORY");
            //if (messageTip.IsNotEmpty()) throw new ValidationException("检验类别".L10N() + messageTip);
        }

        /// <summary>
        /// 验证检验标识
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidCheckTag(InspStandardDataDetailBase p)
        {
            if (p.CheckTag.IsNullOrEmpty())
                throw new ValidationException("检验标识不能为空。".L10N());
            //BussinessDataValid.ValidCheckTag(ref dicCheckTagRange, p.CheckTag, out string messageTip);
            //if (messageTip.IsNotEmpty()) throw new ValidationException("检验标识".L10N() + messageTip);
        }

        /// <summary>
        /// 验证检验方式
        /// </summary>
        /// <param name="p"></param>
        /// <param name="main"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidInspectionMode(InspStandardDataDetailBase p, InspStandardDataBase main)
        {
            if (p.InspectionMode.IsNullOrEmpty())
                throw new ValidationException("检验方式不能为空。".L10N());
            string inspectionType = main.InspectionType;
            string messageTip = string.Empty;
            if (dicInspectionTypeRange.ContainsKey(inspectionType))
            {
                if (dicInspectionModeRange == null)
                {
                    dicInspectionModeRange = new Dictionary<string, Dictionary<string, double>>();
                }

                if (!dicInspectionModeRange.ContainsKey(inspectionType))
                {
                    Dictionary<string, double> inspModeDic = new Dictionary<string, double>();
                    EntityList<InspectionMode> inspectionModeList = RT.Service.Resolve<InspectionItemController>().GetInspectionModes(null, string.Empty);
                    foreach (InspectionMode inspectionModeItem in inspectionModeList)
                    {
                        inspModeDic.Add(inspectionModeItem.Name, inspectionModeItem.Id);
                    }

                    dicInspectionModeRange.Add(inspectionType, inspModeDic);
                }

                if (!dicInspectionModeRange[inspectionType].ContainsKey(p.InspectionMode))
                {
                    messageTip = "只能选择".L10N() + string.Join("、", dicInspectionModeRange[inspectionType].Keys);
                }
            }
            if (messageTip.IsNotEmpty()) throw new ValidationException("检验方式".L10N() + messageTip);
        }

        /// <summary>
        /// 验证缺陷等级
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidDefectGrade(InspStandardDataDetailBase p)
        {
            if (p.DefectGrade.IsNullOrEmpty())
                throw new ValidationException("缺陷等级不能为空。".L10N());
            //BussinessDataValid.ValidDefectGrade(ref dicDefectGradeRange, p.DefectGrade, out string messageTip);
            //if (messageTip.IsNotEmpty()) throw new ValidationException("缺陷等级".L10N() + messageTip);
        }

        /// <summary>
        /// 验证抽样过程
        /// </summary>
        /// <param name="p"></param>
        /// <returns>是否通过</returns>
        private void ValidSamplingStepCode(InspStandardDataDetailBase p)
        {
            if (p.SamplingStep.IsNullOrEmpty())
                throw new ValidationException("抽样过程不能为空。".L10N());
            //BussinessDataValid.ValidSamplingStep(ref dicSamplingStep, p.SamplingStep, out string messageTip);
            //if (messageTip.IsNotEmpty()) throw new ValidationException("抽样过程".L10N() + messageTip);
        }

        /// <summary>
        /// 验证规格下限
        /// </summary>
        /// <param name="obj">规格下限</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private void ValidLIMIT_LOW(InspStandardDataDetailBase p)
        {
            string checkTag = p.CheckTag;
            string limitMax = p.LimitMax;
            string messageTip = string.Empty;
            if (dicLimitLowComare == null)
            {
                dicLimitLowComare = ImportExtension.GetEnumLabel(typeof(CompareType), "Greater");
            }
            if (checkTag.IsNullOrEmpty())
            {
                return;
            }
            string limitLow = p.LimitLow;
            if ((CheckTag)dicCheckTagRange[checkTag] == CheckTag.Quantitative && string.IsNullOrEmpty(limitLow) && string.IsNullOrEmpty(limitMax))
            {
                messageTip = "和规格上限不能同时为空".L10N();
            }

            if (dicCheckTagRange.ContainsKey(checkTag))
            {
                if ((CheckTag)dicCheckTagRange[checkTag] == CheckTag.Qualitative)
                {
                    if (!string.IsNullOrEmpty(limitLow))
                    {
                        messageTip = "检测标识选择【定性】，规格下限必须为空".L10N();
                    }
                }
                else if (!string.IsNullOrEmpty(limitLow))
                {
                    double result = 0;
                    if (limitLow.Length < 2 ||
                        !dicLimitLowComare.ContainsKey(limitLow[0].ToString()) ||
                        !double.TryParse(limitLow.Substring(1), out result))
                    {
                        messageTip = string.Format("输入的格式：({0})+数字".L10N(), string.Join("、", dicLimitLowComare.Keys));
                    }
                }
            }
            if (messageTip.IsNotEmpty()) throw new ValidationException("规格下限：".L10N() + messageTip);

        }

        /// <summary>
        /// 验证规格上限
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidLIMIT_MAX(InspStandardDataDetailBase p)
        {
            string checkTag = p.CheckTag;
            string messageTip = string.Empty;
            if (dicLimitMaxCompare == null)
            {
                dicLimitMaxCompare = ImportExtension.GetEnumLabel(typeof(CompareType), "Less");
            }

            string limitMax = p.LimitMax;
            if (dicCheckTagRange.ContainsKey(checkTag))
            {
                if ((CheckTag)dicCheckTagRange[checkTag] == CheckTag.Qualitative)
                {
                    if (!string.IsNullOrEmpty(limitMax))
                    {
                        messageTip = "检测标识选择【定性】，规格上限必须为空".L10N();
                    }
                }
                else if (!string.IsNullOrEmpty(limitMax))
                {
                    double result = 0;
                    if (limitMax.Length < 2 ||
                        !dicLimitMaxCompare.ContainsKey(limitMax[0].ToString()) ||
                        !double.TryParse(limitMax.Substring(1), out result))
                    {
                        messageTip = string.Format("输入的格式：({0})+数字".L10N(), string.Join("、", dicLimitMaxCompare.Keys));
                    }
                }
            }

            if (messageTip.IsNotEmpty()) throw new ValidationException(messageTip);
        }

        /// <summary>
        /// 验证单位名称
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidUnit(InspStandardDataDetailBase p)
        {
            string checkTag = p.CheckTag;
            string messageTip = string.Empty;
            if (dicUnit == null)
            {
                dicUnit = new Dictionary<string, double>();
            }

            string name = p.Unit;
            if (dicCheckTagRange.ContainsKey(checkTag))
            {
                if ((CheckTag)dicCheckTagRange[checkTag] == CheckTag.Qualitative)
                {
                    if (!string.IsNullOrEmpty(name))
                    {
                        messageTip = "检测标识选择【定性】，单位名称必须为空".L10N();
                    }
                }
                else if (string.IsNullOrEmpty(name))
                {
                    messageTip = "检测标识选择【定量】，单位名称不能为空".L10N();
                }
                else
                {
                    if (!dicUnit.ContainsKey(name))
                    {
                        Unit unit = RT.Service.Resolve<UnitsController>().GetUnitFromName(name);
                        if (unit != null)
                        {
                            dicUnit.Add(name, unit.Id);
                        }
                        else
                        {
                            messageTip = "不存在于系统".L10N();
                        }
                    }
                }
            }

            if (messageTip.IsNotEmpty()) throw new ValidationException("规格上限：".L10N() + messageTip);
        }


        /// <summary>
        /// 验证项目类别
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidInspectionCategory(InspStandardDataDetailBase p)
        {
            if (p.InspectionCategory.IsNullOrEmpty())
                throw new ValidationException("项目类别不能为空。".L10N());
            //BussinessDataValid.ValidInspectionCategory(ref dicInspectionCategoryRange, p.InspectionCategory, out string messageTip);
            //if (messageTip.IsNotEmpty()) throw new ValidationException("项目类别".L10N() + messageTip);
        }

        /// <summary>
        /// 验证周期类型
        /// </summary>
        /// <param name="p"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidPeriodType(InspStandardDataDetailBase p)
        {
            //if ((InspectionCategory)dicInspectionCategoryRange[p.InspectionCategory] == InspectionCategory.RecurInsp)
            //{
            //    if (p.PeriodType.IsNullOrEmpty())
            //        throw new ValidationException("周期检的周期类型不能为空".L10N());
            //    BussinessDataValid.ValidPeriodType(ref dicPeriodTypeRange, p.PeriodType, out string messageTip);
            //    if (messageTip.IsNotEmpty()) throw new ValidationException("【周期类型】".L10N() + messageTip);
            //}
        }

        /// <summary>
        /// 验证工序
        /// </summary>
        /// <param name="p"></param>
        /// <param name="main"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidProcess(InspStandardDataDetailBase p, InspStandardDataBase main)
        {
            string inspectionType = main.InspectionType;
            string messageTip = string.Empty;
            if (dicInspectionTypeRange.ContainsKey(inspectionType))
            {
                //非PQC不需要校验
                //if (!RT.Service.Resolve<InspectionTypeController>().IsPqcInspType((InspectionType)dicInspectionTypeRange[inspectionType]))
                //    return;
            }
            else if (p.Process.IsNotEmpty())
            {
                throw new ValidationException("非首检、抽检、巡检不需要录入工序".L10N());
            }
            if (p.Process.IsNullOrEmpty())
                throw new ValidationException("首检、抽检、巡检的工序不能为空.".L10N());

            if (dicProcess == null)
            {
                dicProcess = new Dictionary<string, double>();
            }

            string context = p.Process;
            if (!dicProcess.ContainsKey(context))
            {
                Process process = RT.Service.Resolve<ProcessController>().GetProcess(context);
                if (process != null)
                {
                    dicProcess.Add(context, process.Id);
                }
                else
                {
                    messageTip = "不存在于系统".L10N();
                }
            }

            if (messageTip.IsNotEmpty()) throw new ValidationException("【工序】".L10N() + messageTip);
        }

        /// <summary>
        /// 验证客户
        /// </summary>
        /// <param name="main"></param>
        /// <returns>返回是否验证通过</returns>
        private void ValidCustomer(InspStandardDataBase main)
        {
            string inspectionType = main.InspectionType;
            string messageTip = string.Empty;
            if (dicInspectionTypeRange.ContainsKey(inspectionType))
            {
                //非OQC不需要校验
                //if (!RT.Service.Resolve<InspectionTypeController>().IsOqcInspType((InspectionType)dicInspectionTypeRange[inspectionType]))
                //{
                //    if (main.Customer.IsNotEmpty())
                //    {
                //        throw new ValidationException("非出货检验和成品检验不需要录入客户".L10N());
                //    }
                //    return;
                //}
            }

            if (main.Customer.IsNullOrEmpty())
                return;

            if (dicCustomerName == null)
            {
                dicCustomerName = new Dictionary<string, double>();
            }

            string name = main.Customer;
            if (!dicCustomerName.ContainsKey(name))
            {
                Customer customer = RT.Service.Resolve<CustomerController>().GetCustomerFromName(name);
                if (customer != null)
                {
                    dicCustomerName.Add(name, customer.Id);
                }
                else
                {
                    messageTip = "不存在于系统".L10N();
                }
            }

            if (messageTip.IsNotEmpty()) throw new ValidationException("【客户】".L10N() + messageTip);
        }

        /// <summary>
        /// 验证明细
        /// </summary>
        /// <param name="inspDetail"></param>
        /// <param name="mainData"></param>
        /// <param name="dr"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void ValidInspDetail(InspStandardDataDetailBase inspDetail, InspStandardDataBase mainData)
        {
            //同一检验类型同一物料同一版本的检验项目不能相同
            if (mainData.DetailList.Any(p => p.Name == inspDetail.Name && p != inspDetail))
            {
                throw new ValidationException("检验类型 {0} 版本号 {1} 的检验标准在同一检验方式下已经存在检验项目 {2}".L10nFormat(mainData.InspectionType, mainData.Version, inspDetail.Name));
            }

            //只有来料检验和成品检验可以维护项目周期检
            var inspType = (InspectionType)dicInspectionTypeRange[mainData.InspectionType];
            //var category = (InspectionCategory)dicInspectionCategoryRange[inspDetail.InspectionCategory];
            //if (category == InspectionCategory.RecurInsp && inspType != InspectionType.IncomingInsp && inspType != InspectionType.ShippingInsp)
            //{
            //    throw new ValidationException("{0}不可以维护周期检项目。".L10nFormat(mainData.InspectionType));
            //}

            //项目周检项的周期类型与间隔周期必填
            //if (category == InspectionCategory.RecurInsp)
            //{
            //    if (inspDetail.PeriodType == null)
            //        throw new ValidationException("项目周期检的周期类型不能为空。".L10N());
            //    if (inspDetail.Period == null)
            //        throw new ValidationException("项目周期检的间隔周期不能为空。".L10N());
            //    if (!inspDetail.IsSuitable)
            //        throw new ValidationException("项目周期检要为必检。".L10N());
            //}

            //首检、巡检抽样过程验证。首检的抽样过程只能是固定抽样。巡检的抽样过程不能是抽样方案
            //if (inspType == InspectionType.FirstInsp && dicSamplingStep[inspDetail.SamplingStep]?.SamplingMethod != SamplingMethod.Fixed)
            //{
            //    throw new ValidationException("首检的抽样过程只能是固定抽样".L10nFormat());
            //}

            //if (inspType == InspectionType.PatrolInsp && dicSamplingStep[inspDetail.SamplingStep]?.SamplingMethod == SamplingMethod.Plan)
            //{
            //    throw new ValidationException("巡检的抽样过程不能是抽样方案".L10nFormat());
            //}
        }
    }
}
