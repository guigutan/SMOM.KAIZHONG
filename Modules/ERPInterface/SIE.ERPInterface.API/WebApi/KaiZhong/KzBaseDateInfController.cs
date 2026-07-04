using Newtonsoft.Json;
using SIE.Api;
using SIE.Domain;
using SIE.ERPInterface.Smom.Download.KaiZhong;
using SIE.EventMessages.MES.Dispatchs;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Checker;
using SIE.MES.Fixture;
using SIE.MES.ItemChecker;
using SIE.MES.ItemEquipAccount;
using SIE.MES.ItemFixture;
using SIE.Security;
using SIE.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Api.WebApi.KaiZhong
{
    public class KzBaseDateInfController : KzLoginController, IDispatchsErp
    {
        [ApiService("")]
        public virtual void takeInterface(List<double> ids)
        {
            var infNcDataLogs = Query<InfNcDataLog>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var invOrg = Query<SIE.Rbac.InvOrgs.InvOrg>().Where(p => p.Code == (int)RT.InvOrg).FirstOrDefault();

            foreach (var infNcDataLog in infNcDataLogs)
            {
                distributeMdFactory(infNcDataLog.SystemCode, infNcDataLog.InfCode, infNcDataLog.OperationType, infNcDataLog.DataJsons, invOrg.ExternalId, infNcDataLog.GroupGuid, null);
            }
        }

        /// <summary>
        /// 集团调用方法发给下面的工厂
        /// </summary>
        /// <param name="systemCode"></param>
        /// <param name="infCode"></param>
        /// <param name="operationType"></param>
        /// <param name="dataJsons"></param>
        /// <param name="str">里面可以夹带从集团带过来的信息</param>
        /// <returns></returns>
        [AllowAnonymous]
        [ApiService("NC主数据接口")]
        public virtual OuterSystemRetVO distributeMdFactory(string systemCode, string infCode, string operationType, string dataJsons, string invOrgId, string groupGuid, string str)
        {
            OuterSystemRetVO result = new OuterSystemRetVO();
            Login(invOrgId);
            List<string> errorMsgList = new List<string>();
            if (infCode.IsNullOrEmpty())
                errorMsgList.Add("主数据类型为空");
            if (dataJsons.IsNullOrEmpty())
                errorMsgList.Add("主数据为空");

            string errorMsg = null;
            if (errorMsgList.Count > 0)
                errorMsg = string.Join("、", errorMsgList);

            var logController = RT.Service.Resolve<InfNcDataLogController>();
            var infType = (InfType)EnumViewModel.LabelToEnum(infCode, typeof(InfType));

            var infDataLog = logController.SaveInfNcDataLog(systemCode, infCode, operationType, dataJsons, DateTime.Now, infType, CallDirection.SAPToMom, CallResult.UnSave, groupGuid, errorMsg, null);

            try
            {
                if (!errorMsg.IsNullOrEmpty())
                {
                    result.success = false;
                    result.errorMsg = errorMsg;
                    var resList = JsonConvert.DeserializeObject<List<object>>(dataJsons);
                    result.errorList.AddRange(resList);
                    return result;
                }
                infDataLog.InfType = infType;
                RF.Save(infDataLog);
                //将NC接口数据保存到MES业务表中
                result = SaveInfDatasToBusinessFactory(systemCode, infCode, operationType, dataJsons, infType, infDataLog, str);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                result.success = false;
                result.errorMsg = "NC接口数据保存失败:[0]".L10nFormat(errorMsg);
                infDataLog.ErrorMsg = errorMsg;
                RF.Save(infDataLog);
            }
            infDataLog.ResponseContent = JsonConvert.SerializeObject(result);
            infDataLog.EndDate = DateTime.Now;
            RF.Save(infDataLog);
            return result;
        }

        /// <summary>
        /// 集团调用方法发给下面的工厂
        /// </summary>
        /// <param name="systemCode"></param>
        /// <param name="infCode"></param>
        /// <param name="operationType"></param>
        /// <param name="dataJsons"></param>
        /// <returns></returns>
        public virtual void DistributeMdFactorySO(string systemCode, string infCode, string operationType, string dataJsons, string invOrgId, string groupGuid)
        {
            OuterSystemRetVO result = new OuterSystemRetVO();
            Login(invOrgId);
            List<string> errorMsgList = new List<string>();
            if (infCode.IsNullOrEmpty())
                errorMsgList.Add("主数据类型为空");
            if (dataJsons.IsNullOrEmpty())
                errorMsgList.Add("主数据为空");

            string errorMsg = null;
            if (errorMsgList.Count > 0)
                errorMsg = string.Join("、", errorMsgList);

            var logController = RT.Service.Resolve<InfNcDataLogController>();
            var infType = (InfType)EnumViewModel.LabelToEnum(infCode, typeof(InfType));

            var infDataLog = logController.SaveInfNcDataLog(systemCode, infCode, operationType, dataJsons, DateTime.Now, infType, CallDirection.NcToMom, CallResult.UnSave, groupGuid, errorMsg, null);

            try
            {
                if (!errorMsg.IsNullOrEmpty())
                {
                    result.success = false;
                    result.errorMsg = errorMsg;
                    var resList = JsonConvert.DeserializeObject<List<object>>(dataJsons);
                    result.errorList.AddRange(resList);
                    //return result;
                }
                infDataLog.InfType = infType;
                RF.Save(infDataLog);
                //将NC接口数据保存到MES业务表中
                result = SaveInfDatasToBusinessFactory(systemCode, infCode, operationType, dataJsons, infType, infDataLog);
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                result.success = false;
                result.errorMsg = "NC接口数据保存失败:[0]".L10nFormat(errorMsg);
                infDataLog.ErrorMsg = errorMsg;
                RF.Save(infDataLog);
            }
            infDataLog.ResponseContent = JsonConvert.SerializeObject(result);
            infDataLog.EndDate = DateTime.Now;
            RF.Save(infDataLog);
            //return result;
        }


        /// <summary>
        /// 将NC接口数据保存到MES业务表中
        /// </summary>
        /// <param name="systemCode">系统编码</param>
        /// <param name="infCode">主数据类型</param>
        /// <param name="operationType">操作类型</param>
        /// <param name="dataJsons">主数据(JSON数组格式)</param>
        /// <param name="infMapping">接口基础信息</param>
        /// <param name="infDataLog">接口日志</param>
        /// <param name="str">里面可以夹带从集团带过来的信息</param>
        public virtual OuterSystemRetVO SaveInfDatasToBusinessFactory(string systemCode, string infCode, string operationType, string dataJsons, InfType infType, InfNcDataLog infDataLog, string str = null)
        {
            OuterSystemRetVO result = new OuterSystemRetVO();
            result.success = true;
            result.errorMsg = null;
            result.mdMapings = null;
            ApiCommonRes apiCommonRes = new ApiCommonRes();
            switch (infType)
            {
                case InfType.ItemCategory:
                    var itemCategoryDatas = JsonConvert.DeserializeObject<List<ItemCategoryData>>(dataJsons);
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
                        apiCommonRes = ctl.SaveItemCategorysFactory(itemCategoryDatas);
                    }
                    break;
                case InfType.Item:
                    var itemDatas = JsonConvert.DeserializeObject<List<ItemData>>(dataJsons);
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
                        apiCommonRes = ctl.SaveItemsFactory(itemDatas);
                    }
                    break;
                case InfType.WorkCenter:
                    var workCenterDatas = JsonConvert.DeserializeObject<List<WorkCenterData>>(dataJsons);
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
                        apiCommonRes = ctl.SaveWorkCenters(workCenterDatas, str);
                    }
                    break;
                case InfType.Employee:
                    var employeeDatas = JsonConvert.DeserializeObject<List<EmployeeData>>(dataJsons);
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
                        apiCommonRes = ctl.SaveEmployees(employeeDatas);
                    }
                    break;
                case InfType.Process:
                    var processDatas = JsonConvert.DeserializeObject<List<ProcessData>>(dataJsons);
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
                        apiCommonRes = ctl.SaveProcesss(processDatas);
                    }
                    break;
                case InfType.WorkOrder:
                    var workOrderData = JsonConvert.DeserializeObject<WorkOrderData>(dataJsons);
                    if (workOrderData == null)
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
                        apiCommonRes = ctl.SaveWorkOrder(workOrderData);
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
                        apiCommonRes = ctl.SaveEquipAccount(equipAccountDatas);
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
                        apiCommonRes = ctl.SaveSkills(skillDatas);
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
                        apiCommonRes = ctl.SaveItemLabel(itemLabelDatas);
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
                        apiCommonRes = ctl.SaveBlueLabels(blueLabelDatas);
                    }
                    break;
                case InfType.ReportResult:
                    var reportReturnDatas = JsonConvert.DeserializeObject<List<ReportReturnData>>(dataJsons);
                    if (reportReturnDatas == null)
                    {
                        var errorMsg = "物料标签主数据的JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<UpdateReportResultController>();
                        apiCommonRes = ctl.SaveReportResult(reportReturnDatas);
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
                        apiCommonRes = ctl.GroupSaveEnterprise(enterpriseDatas);
                    }
                    break;
                case InfType.OAFlowReturn:
                    var oAFlowReturnDatas = JsonConvert.DeserializeObject<List<OAFlowReturnData>>(dataJsons);
                    if (oAFlowReturnDatas == null)
                    {
                        var errorMsg = "OA流程退回JSON格式不对".L10N();
                        infDataLog.ErrorMsg = errorMsg;
                        RF.Save(infDataLog);
                        result.success = false;
                        result.errorMsg = errorMsg;
                    }
                    else
                    {
                        var ctl = RT.Service.Resolve<DownloadOAFlowReturnController>();
                        apiCommonRes = ctl.GroupSaveOAFlowReturn(oAFlowReturnDatas);
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
                        apiCommonRes = RT.Service.Resolve<DownloadThresholdController>().SaveThresholdFactory(thresholdDatas);
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
                        apiCommonRes = RT.Service.Resolve<DownloadAndonLineController>().SaveAndonLineFactory(andonLineDatas);
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
                        apiCommonRes = RT.Service.Resolve<DownloadProductLineController>().SaveProductLineFactory(productLineDatas);
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
                        apiCommonRes = RT.Service.Resolve<DownloadFixtureUpholdController>().SaveFixtureUpholdFactory(fixtureUpholdDatas);
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
                        apiCommonRes = RT.Service.Resolve<DownloadFixtureItemController>().SaveFixtureItemFactory(fixtureItemDatas);
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
                        apiCommonRes = RT.Service.Resolve<DownloadCheckerUpholdController>().SaveCheckerUpholdFactory(checkerUpholdDatas);
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
                        apiCommonRes = RT.Service.Resolve<DownloadCheckerItemController>().SaveCheckerItemFactory(checkerItemDatas);
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
                        apiCommonRes = RT.Service.Resolve<DownloadEquipAccountItemController>().SaveEquipAccountItemFactory(equipAccountItemDatas);
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
                        apiCommonRes = RT.Service.Resolve<DownloadReworkLayoutVersionController>().SaveEquipAccountItemFactory(rlvd);
                    }
                    break;
            }
            if (apiCommonRes != null && apiCommonRes.ErrorList != null && apiCommonRes.ErrorList.Count > 0)
            {
                var errorMsg = string.Join("、", apiCommonRes.ErrorList);
                result.success = false;
                result.errorMsg = "[{0}]接口同步数据失败:[{1}]".L10nFormat(infType.ToLabel(), errorMsg);
                infDataLog.ErrorMsg = errorMsg;
                infDataLog.ResponseContent = JsonConvert.SerializeObject(apiCommonRes);
                RF.Save(infDataLog);
            }
            else
            {
                infDataLog.CallResult = CallResult.Success;
                infDataLog.ResponseContent = JsonConvert.SerializeObject(apiCommonRes);
                RF.Save(infDataLog);
            }
            result.errorList.AddRange(apiCommonRes.ErrorObjList);
            return result;
        }

        public virtual void UpdateTaskStartEndProcess(double? startProcessId, double? endProcessId, double workOrderId)
        {
            throw new NotImplementedException();
        }

        public virtual void GenerateTaskByWorkOrderIds(List<double> workOrderIds)
        {
            throw new NotImplementedException();
        }

        public virtual string GetPrintAxisNumberByTaskId(double id)
        {
            throw new NotImplementedException();
        }

        public virtual string GetResourceNameByTaskId(double id)
        {
            throw new NotImplementedException();
        }

        public virtual decimal GetProcessZcodeByTaskId(double Id)
        {
            throw new NotImplementedException();
        }
    }
}
