using DocumentFormat.OpenXml.Presentation;
using Microsoft.Extensions.DependencyModel.Resolution;
using Newtonsoft.Json;
using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Smom.Download;
using SIE.ERPInterface.Smom.Download.KaiZhong;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.MES.Checker;
using SIE.MES.Fixture;
using SIE.MES.ItemChecker;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemFixture;
using SIE.MES.ItemLine;
using SIE.Security;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DownloadEmployeeController = SIE.ERPInterface.Smom.Download.KaiZhong.DownloadEmployeeController;
using DownloadEnterpriseController = SIE.ERPInterface.Smom.Download.KaiZhong.DownloadEnterpriseController;
using DownloadItemController = SIE.ERPInterface.Smom.Download.KaiZhong.DownloadItemController;
using DownloadWorkOrderController = SIE.ERPInterface.Smom.Download.KaiZhong.DownloadWorkOrderController;
using EmployeeData = SIE.KZ.Base.Interfaces.Datas.EmployeeData;
using EnterpriseData = SIE.KZ.Base.Interfaces.Datas.EnterpriseData;
using ItemCategoryData = SIE.KZ.Base.Interfaces.Datas.ItemCategoryData;
using ItemData = SIE.KZ.Base.Interfaces.Datas.ItemData;
using WorkOrderData = SIE.KZ.Base.Interfaces.Datas.WorkOrderData;

namespace SIE.ERPInterface.Api.WebApi.KaiZhongGroup
{
    public class KzGroupBaseDateInfController : KzLoginController
    {
        /// <summary>
        /// 其他系统调用，数据传给集团
        /// </summary>
        /// <param name="systemCode">系统编码</param>
        /// <param name="infCode">主数据类型</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="dataJsons">主数据(JSON数组格式)</param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("NC主数据接口")]
        public virtual OuterSystemRetVO distributeMd(string systemCode, string infCode, string operationType, string dataJsons, string invOrg)
        {
            Login();
            RT.InvOrg = 1;
            OuterSystemRetVO result = new OuterSystemRetVO();
            List<string> errorMsgList = new List<string>();

            if (infCode.IsNullOrEmpty())
                errorMsgList.Add("主数据类型不能为空");
            if (dataJsons.IsNullOrEmpty())
                errorMsgList.Add("主数据不能为空");
            string errorMsg = null;
            if (errorMsgList.Count > 0)
                errorMsg = string.Join("、", errorMsgList);
            string parameterCom = $"系统编码:{systemCode} 主数据类型:{infCode} 操作类型:{operationType}";
            try
            {
                if (!errorMsg.IsNullOrEmpty())
                {
                    result.success = false;
                    result.errorMsg = errorMsg;
                    InfNcDataLogGroup infDataLog = RT.Service.Resolve<InfNcDataLogGroupController>().SaveInfNcDataLogGroup(invOrg, dataJsons, DateTime.Now, null, CallResult.Fail, Guid.NewGuid().ToString(), errorMsg, KeyMsgone: parameterCom, systemCode: systemCode, operationType: operationType, infCode: infCode);
                    return result;
                }

                var infType = (InfType)EnumViewModel.LabelToEnum(infCode, typeof(InfType));
                try
                {
                    //将NC接口数据保存到MES业务表中
                    result = SaveInfDatasToBusiness(systemCode, infCode, operationType, dataJsons, infType, parameterCom, invOrg);
                }
                catch (Exception ex)
                {
                    throw new ValidationException(ex.GetBaseException()?.Message);
                }
                finally
                {
                    //指定工厂分发给其他工厂
                    DistributeFactory(systemCode, infCode, operationType, dataJsons, infType, parameterCom, invOrg);
                }

            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                result.success = false;
                result.errorMsg = "SAP接口数据保存失败:[0]".L10nFormat(errorMsg);

            }
            return result;
        }


        //[ApiService("")]
        //public virtual void copyJson()
        //{
        //    var list = Query<InfNcDataLogGroup>().Where(p => p.InvOrg == "1021" && p.CreateDate >= DateTime.Now.AddDays(-2).Date).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

        //    foreach (var l in list)
        //    {
        //        DistributeFactory(l.NcSystemCode, l.NcInfCode, l.NcOperationType, l.DataJsons, l.InfType.Value, l.KeyMsgone, l.InvOrg);
        //    }

        //}

        /// <summary>
        /// 指定工厂分发给其他工厂
        /// </summary>
        /// <param name="systemCode"></param>
        /// <param name="infCode"></param>
        /// <param name="operationType"></param>
        /// <param name="dataJsons"></param>
        /// <param name="infType"></param>
        /// <param name="parameterCom"></param>
        /// <param name="invOrg"></param>
        public virtual void DistributeFactory(string systemCode, string infCode, string operationType, string dataJsons, InfType infType, string parameterCom, string invOrg)
        {
            var smomControlSetting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(invOrg);
            if (smomControlSetting != null) {
                //判断是否存在当前的需要同步的数据类型
                if (smomControlSetting.TypeParamDetail.Any(p => p.ParamType == infType))
                {
                    foreach (var factory in smomControlSetting.FactoryDetail)
                    {
                        //创建记录，然后等待调度执行
                        InfNcDataLogGroup infDataLog = RT.Service.Resolve<InfNcDataLogGroupController>().SaveInfNcDataLogGroup(factory.FactoryCode, dataJsons, DateTime.Now, infType, CallResult.Success, Guid.NewGuid().ToString(), null, KeyMsgone: parameterCom, systemCode: systemCode, operationType: operationType, infCode: infCode, isDistribute: true, endDate: DateTime.Now);

                    }
                }
            }
        }

        /// <summary>
        /// 将NC接口数据保存到MES业务表中
        /// </summary>
        /// <param name="systemCode">系统编码</param>
        /// <param name="infCode">主数据类型</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="dataJsons">主数据(JSON数组格式)</param>
        /// <param name="infMapping">接口基础信息</param>
        /// <param name="parameterCom">接口关键信息</param>
        public virtual OuterSystemRetVO SaveInfDatasToBusiness(string systemCode, string infCode, string operationType, string dataJsons, InfType infType, string parameterCom, string invOrg, InfNcDataLogGroup infDataLog = null)
        {
            OuterSystemRetVO result = new OuterSystemRetVO();
            result.success = true;
            result.errorMsg = null;
            result.mdMapings = null;
            ApiCommonRes apiCommonRes = new ApiCommonRes();

            string batchNo = Guid.NewGuid().ToString();
            //InfNcDataLogGroup infDataLog = null;
            if (infDataLog == null)
            {
                infDataLog = RT.Service.Resolve<InfNcDataLogGroupController>().SaveInfNcDataLogGroup(invOrg, dataJsons, DateTime.Now, infType, CallResult.UnSave, batchNo, null, parameterCom, parameterCom, systemCode: systemCode, operationType: operationType, infCode: infCode);
            }
            switch (infType)
            {
                case InfType.ItemCategory:
                    var itemCategoryDatas = JsonConvert.DeserializeObject<List<ItemCategoryData>>(dataJsons);
                    infDataLog.KeyMsgone = string.Join(",", itemCategoryDatas?.Select(p => p.MATKL));
                    if (itemCategoryDatas == null)
                    {
                        var errorMsg = "物料分类主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadItemCategoryController>();
                        apiCommonRes = ctl.SaveItemCategorys(itemCategoryDatas, ref infDataLog);
                    }
                    break;
                case InfType.Item:
                    var itemDatas = JsonConvert.DeserializeObject<List<ItemData>>(dataJsons);
                    infDataLog.KeyMsgone = string.Join(",", itemDatas?.Select(p => p.MATNR));
                    if (itemDatas == null)
                    {
                        var errorMsg = "物料主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadItemController>();
                        apiCommonRes = ctl.SaveItems(itemDatas, ref infDataLog);
                    }
                    break;
                case InfType.WorkCenter:
                    var workCenterDatas = JsonConvert.DeserializeObject<List<WorkCenterData>>(dataJsons);
                    infDataLog.KeyMsgone = string.Join(",", workCenterDatas?.Select(p => p.ARBPL));

                    if (workCenterDatas == null)
                    {
                        var errorMsg = "工作中心主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadWorkCenterController>();
                        apiCommonRes = ctl.SaveWorkCenters(workCenterDatas, ref infDataLog);
                    }
                    break;
                case InfType.Employee:
                    var employeeDatas = JsonConvert.DeserializeObject<List<EmployeeData>>(dataJsons);
                    infDataLog.KeyMsgone = string.Join(",", employeeDatas?.Select(p => p.PERNR));
                    if (employeeDatas == null)
                    {
                        var errorMsg = "员工主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadEmployeeController>();
                        apiCommonRes = ctl.SaveEmployees(employeeDatas, ref infDataLog);
                    }
                    break;
                case InfType.OrgLevel:
                    var orgLevelDatas = JsonConvert.DeserializeObject<List<SIE.MES.OrgLevels.OrgLevelInfo>>(dataJsons);
                    infDataLog.KeyMsgone = string.Join(",", orgLevelDatas?.Select(p => p.OrgCode));
                    if (orgLevelDatas == null)
                    {
                        var errorMsg = "人员组织架构数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadOrgLevelController>();
                        apiCommonRes = ctl.SaveOrgLevels(orgLevelDatas, ref infDataLog);                       
                    }
                    break;
                case InfType.Process:
                    var processDatas = JsonConvert.DeserializeObject<List<ProcessData>>(dataJsons);
                    infDataLog.KeyMsgone = string.Join(",", processDatas?.Select(p => p.VLSCH));
                    if (processDatas == null)
                    {
                        var errorMsg = "工序主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadProcessController>();
                        apiCommonRes = ctl.SaveProcesss(processDatas, ref infDataLog);
                    }
                    break;
                case InfType.WorkOrder:

                    var workWorkData = JsonConvert.DeserializeObject<WorkOrderData>(dataJsons);
                    if (workWorkData == null)
                    {
                        var errorMsg = "工单主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadWorkOrderController>();
                        apiCommonRes = ctl.SaveWorkOrder(workWorkData, ref infDataLog);
                    }
                    break;
                case InfType.EquipAccount:
                    var equipAccountDatas = JsonConvert.DeserializeObject<List<EquipAccountData>>(dataJsons);
                    if (equipAccountDatas == null)
                    {
                        var errorMsg = "设备主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadEquipAccountController>();
                        apiCommonRes = ctl.SaveEquipAccount(equipAccountDatas, ref infDataLog);
                    }
                    break;
                case InfType.Skill:
                    var skillDatas = JsonConvert.DeserializeObject<List<SkillData>>(dataJsons);
                    if (skillDatas == null)
                    {
                        var errorMsg = "人员技能主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadSkillsController>();
                        apiCommonRes = ctl.SaveSkills(skillDatas, ref infDataLog);
                    }
                    break;
                case InfType.ItemLabel:
                    var itemLabelDatas = JsonConvert.DeserializeObject<List<ItemLabelData>>(dataJsons);
                    if (itemLabelDatas == null)
                    {
                        var errorMsg = "物料标签主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadItemLabelController>();
                        apiCommonRes = ctl.SaveItemLabel(itemLabelDatas, ref infDataLog);
                    }
                    break;
                case InfType.BlueLabel:
                    var blueLabelDatas = JsonConvert.DeserializeObject<List<BlueLabelData>>(dataJsons);
                    if (blueLabelDatas == null)
                    {
                        var errorMsg = "蓝标标签主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadBlueLabelController>();
                        apiCommonRes = ctl.SaveBlueLabels(blueLabelDatas, ref infDataLog);
                    }
                    break;
                case InfType.ReportResult:
                    var reportReturnDatas = JsonConvert.DeserializeObject<List<ReportReturnData>>(dataJsons);
                    if (reportReturnDatas == null)
                    {
                        var errorMsg = "报工结果JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<UpdateReportResultController>();
                        apiCommonRes = ctl.SaveReportResult(reportReturnDatas, ref infDataLog);
                    }
                    break;
                case InfType.Enterprise:
                    var enterpriseDatas = JsonConvert.DeserializeObject<List<EnterpriseData>>(dataJsons);
                    if (enterpriseDatas == null)
                    {
                        var errorMsg = "企业模型结果JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadEnterpriseController>();
                        apiCommonRes = ctl.GroupSaveEnterprise(enterpriseDatas, ref infDataLog);
                    }
                    break;
                case InfType.Threshold:
                    var thresholdDatas = JsonConvert.DeserializeObject<List<ThresholdData>>(dataJsons);
                    if (thresholdDatas == null)
                    {
                        var errorMsg = "可疑品阈值JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        foreach (var data in thresholdDatas)
                        {
                            RT.Service.Resolve<DownloadThresholdController>().Valid(data, ref result);
                        }
                        //当没有报错信息的时候，直接调用工厂接口
                        if (result.errorMsg.IsNullOrEmpty())
                        {
                            result = GroupToFactory(infDataLog, InfType.Threshold);
                        }
                    }
                    break;
                case InfType.AndonLine:
                    var andonLineDatas = JsonConvert.DeserializeObject<List<AndonLineData>>(dataJsons);
                    if (andonLineDatas == null)
                    {
                        var errorMsg = "产线与安灯区域JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        foreach (var data in andonLineDatas)
                        {
                            RT.Service.Resolve<DownloadAndonLineController>().Valid(data, ref result);
                        }
                        //当没有报错信息的时候，直接调用工厂接口
                        if (result.errorMsg.IsNullOrEmpty())
                        {
                            result = GroupToFactory(infDataLog, InfType.AndonLine);
                        }
                    }
                    break;
                case InfType.ProductLine:
                    var productLineDatas = JsonConvert.DeserializeObject<List<ProductLineData>>(dataJsons);
                    if (productLineDatas == null)
                    {
                        var errorMsg = "产品与产线的关系JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        foreach (var data in productLineDatas)
                        {
                            RT.Service.Resolve<DownloadProductLineController>().Valid(data, ref result);
                        }
                        //当没有报错信息的时候，直接调用工厂接口
                        if (result.errorMsg.IsNullOrEmpty())
                        {
                            result = GroupToFactory(infDataLog, InfType.ProductLine);
                        }
                    }
                    break;
                case InfType.FixtureUphold:
                    var fixtureUpholdDatas = JsonConvert.DeserializeObject<List<FixtureUpholdData>>(dataJsons);
                    if (fixtureUpholdDatas == null)
                    {
                        var errorMsg = "工装维护JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        foreach (var data in fixtureUpholdDatas)
                        {
                            RT.Service.Resolve<DownloadFixtureUpholdController>().Valid(data, ref result);
                        }
                        //当没有报错信息的时候，直接调用工厂接口
                        if (result.errorMsg.IsNullOrEmpty())
                        {
                            result = GroupToFactory(infDataLog, InfType.FixtureUphold);
                        }
                    }
                    break;
                case InfType.FixtureItem:
                    var fixtureItemDatas = JsonConvert.DeserializeObject<List<FixtureItemData>>(dataJsons);
                    if (fixtureItemDatas == null)
                    {
                        var errorMsg = "工装与产品的关系JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        foreach (var data in fixtureItemDatas)
                        {
                            RT.Service.Resolve<DownloadFixtureItemController>().Valid(data, ref result);
                        }
                        //当没有报错信息的时候，直接调用工厂接口
                        if (result.errorMsg.IsNullOrEmpty())
                        {
                            result = GroupToFactory(infDataLog, InfType.FixtureItem);
                        }
                    }
                    break;
                case InfType.CheckerUphold:
                    var checkerUpholdDatas = JsonConvert.DeserializeObject<List<CheckerUpholdData>>(dataJsons);
                    if (checkerUpholdDatas == null)
                    {
                        var errorMsg = "检具维护JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        foreach (var data in checkerUpholdDatas)
                        {
                            RT.Service.Resolve<DownloadCheckerUpholdController>().Valid(data, ref result);
                        }
                        //当没有报错信息的时候，直接调用工厂接口
                        if (result.errorMsg.IsNullOrEmpty())
                        {
                            result = GroupToFactory(infDataLog, InfType.CheckerUphold);
                        }
                    }
                    break;
                case InfType.CheckerItem:
                    var checkerItemDatas = JsonConvert.DeserializeObject<List<CheckerItemData>>(dataJsons);
                    if (checkerItemDatas == null)
                    {
                        var errorMsg = "检具与产品的关系JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        foreach (var data in checkerItemDatas)
                        {
                            RT.Service.Resolve<DownloadCheckerItemController>().Valid(data, ref result);
                        }
                        //当没有报错信息的时候，直接调用工厂接口
                        if (result.errorMsg.IsNullOrEmpty())
                        {
                            result = GroupToFactory(infDataLog, InfType.CheckerItem);
                        }
                    }
                    break;
                case InfType.EquipAccountItem:
                    var equipAccountItemDatas = JsonConvert.DeserializeObject<List<EquipAccountItemData>>(dataJsons);
                    if (equipAccountItemDatas == null)
                    {
                        var errorMsg = "模具与产品的关系JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        foreach (var data in equipAccountItemDatas)
                        {
                            RT.Service.Resolve<DownloadEquipAccountItemController>().Valid(data, ref result);
                        }
                        //当没有报错信息的时候，直接调用工厂接口
                        if (result.errorMsg.IsNullOrEmpty())
                        {
                            result = GroupToFactory(infDataLog, InfType.EquipAccountItem);
                        }
                    }
                    break;
                case InfType.ReworkLayoutVersion:
                    var rlvd = JsonConvert.DeserializeObject<Rlvd>(dataJsons);
                    if (rlvd == null)
                    {
                        var errorMsg = "返工工艺路线版本JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        foreach (var data in rlvd.Data1)
                        {
                            RT.Service.Resolve<DownloadReworkLayoutVersionController>().Valid(data, ref result);
                        }
                    }
                    break;
                default:
                    if (infDataLog != null)
                    {
                        //infDataLog.CallResult = CallResult.Success;
                        infDataLog.EndDate = DateTime.Now;
                        RF.Save(infDataLog);
                    }
                    break;
            }
            if (infType != InfType.Employee)
            {
                result = ProcessInfNcDataLogGroup(infDataLog, apiCommonRes, infType.ToLabel(), infCode, result);               
            }
            else
            {
                if (apiCommonRes != null && apiCommonRes.ErrorList != null && apiCommonRes.ErrorList.Count > 0)
                {
                    var errorMsg = string.Join("、", apiCommonRes.ErrorList);
                    result.success = false;
                    result.errorMsg = "[{0}]接口同步数据失败:[{1}]".L10nFormat(infType.ToLabel(), errorMsg);
                    result.errorList = apiCommonRes.ErrorObjList;
                }
                else
                {
                    infDataLog.CallResult = CallResult.Success;
                }
            }
            return result;
        }

        /// <summary>
        /// 集团直接发送到工厂
        /// </summary>
        /// <param name="infDataLog"></param>
        /// <param name="infType"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual OuterSystemRetVO GroupToFactory(InfNcDataLogGroup infDataLog, InfType infType)
        {
            //查找所有工厂
            var smomControlSettingDic = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettings().ToDictionary(p => p.FactoryCode);
            if (smomControlSettingDic == null || smomControlSettingDic.Count == 0)
            {
                throw new ValidationException("没有维护SMOM总控配置数据!".L10N());
            }
            StringBuilder stringBuilder = new StringBuilder();
            Tuple<OuterSystemRetVO, string> tuple = new(new OuterSystemRetVO(), string.Empty);
            RT.Service.Resolve<GroupFactoryJobController>().PushGroupLogDatas(new EntityList<InfNcDataLogGroup>() { infDataLog }, smomControlSettingDic, infType, stringBuilder, ref tuple);
            return tuple.Item1;
        }

        private OuterSystemRetVO ProcessInfNcDataLogGroup(InfNcDataLogGroup infDataLog, ApiCommonRes apiCommonRes, string infType, string infCode, OuterSystemRetVO result)
        {
            if (apiCommonRes != null && apiCommonRes.ErrorList != null && apiCommonRes.ErrorList.Count > 0)
            {
                var errorMsg = string.Join("、", apiCommonRes.ErrorList);
                result.success = false;
                result.errorMsg = "[{0}]接口同步数据失败:[{1}]".L10nFormat(infType, errorMsg);
                result.errorList = apiCommonRes.ErrorObjList;
                infDataLog.ErrorMsg = errorMsg;
                infDataLog.CallResult = CallResult.Fail;
            }
            else
            {
                infDataLog.CallResult = CallResult.Success;
            }
            infDataLog.ResponseContent = JsonConvert.SerializeObject(apiCommonRes);
            infDataLog.KeyMsgfour = infCode;
            RF.Save(infDataLog);
            return result;
        }

        /// <summary>
        /// 其他系统调用，数据传给集团
        /// </summary>
        /// <param name="systemCode">系统编码</param>
        /// <param name="infCode">主数据类型</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="dataJsons">主数据(JSON数组格式)</param>
        /// <returns></returns>
        //[AllowAnonymous]
        //[ApiService("NC主数据接口")]
        //public virtual OuterSystemRetVO distributeMd1(string systemCode, string infCode, string operationType, string dataJsons, string invOrg)
        //{
        //    Login();
        //    OuterSystemRetVO result = new OuterSystemRetVO();
        //    List<string> errorMsgList = new List<string>();

        //    if (infCode.IsNullOrEmpty())
        //        errorMsgList.Add("主数据类型不能为空");
        //    if (dataJsons.IsNullOrEmpty())
        //        errorMsgList.Add("主数据不能为空");
        //    string errorMsg = null;
        //    if (errorMsgList.Count > 0)
        //        errorMsg = string.Join("、", errorMsgList);
        //    string parameterCom = $"系统编码:{systemCode} 主数据类型:{infCode} 操作类型:{operationType}";
        //    try
        //    {
        //        if (!errorMsg.IsNullOrEmpty())
        //        {
        //            result.success = false;
        //            result.errorMsg = errorMsg;
        //            InfNcDataLogGroup infDataLog = RT.Service.Resolve<InfNcDataLogGroupController>().SaveInfNcDataLogGroup(invOrg, dataJsons, DateTime.Now, null, CallResult.Fail, Guid.NewGuid().ToString(), errorMsg, KeyMsgone: parameterCom, systemCode: systemCode, operationType: operationType, infCode: infCode);
        //            return result;
        //        }
        //        //var infMapping = RT.Service.Resolve<InfMappingController>().GetInfMapping(infCode);
        //        //if (infMapping == null)
        //        //{
        //        //    InfNcDataLogGroup infDataLog = RT.Service.Resolve<InfNcDataLogGroupController>().SaveInfNcDataLogGroup(invOrg, dataJsons, DateTime.Now, null, CallResult.UnSave, Guid.NewGuid().ToString(), errorMsg, parameterCom, parameterCom, systemCode: systemCode, operationType: operationType, infCode: infCode);
        //        //    errorMsg = "主数据类型[{0}]没有在维护[基础数据接口信息]中维护".L10nFormat(infCode);
        //        //    infDataLog.ErrorMsg = errorMsg;
        //        //    infDataLog.CallResult = CallResult.Fail;
        //        //    infDataLog.EndDate = DateTime.Now;
        //        //    infDataLog.KeyMsgfour = infCode;
        //        //    //InfTypeGroup? type = null;
        //        //    infDataLog.InfType = infMapping == null ? null : infMapping?.InfType;
        //        //    RF.Save(infDataLog);
        //        //    result.success = false;
        //        //    result.errorMsg = errorMsg;
        //        //    return result;
        //        //}
        //        var infType = (InfType)EnumViewModel.LabelToEnum(infCode, typeof(InfType));
        //        //将NC接口数据保存到MES业务表中
        //        result = SaveInfDatasToBusiness(systemCode, infCode, operationType, dataJsons, infType, parameterCom, invOrg);
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message;
        //        result.success = false;
        //        result.errorMsg = "NC接口数据保存失败:[0]".L10nFormat(errorMsg);

        //    }
        //    return result;
        //}

    }
}
