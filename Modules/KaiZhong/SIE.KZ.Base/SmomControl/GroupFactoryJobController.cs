using Newtonsoft.Json;
using SIE.Core.Common;
using SIE.Domain;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIE.Common.Configs;
using SIE.KZ.Base.Configs;
using SIE.Domain.Validation;
using Newtonsoft.Json.Linq;

namespace SIE.KZ.Base.SmomControl
{
    public class GroupFactoryJobController : DomainController
    {
        //public static IList<InfType> systemRetVOTypes = new List<InfType>() { InfType.Employee, InfType.ItemCategory, InfType.Item, InfType.Customer, InfType.Supplier, InfType.ProductProject, InfType.WorkCenter, InfType.WorkOrder, InfType.Process, InfType.EquipAccount };


        /// <summary>
        /// 执行工单总控到子工厂数据的传输
        /// </summary>
        /// <returns></returns>
        public virtual StringBuilder CorpFactorySOJobExecute(InfType infType,String BatchNo)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Tuple<OuterSystemRetVO, string> tuple = new(new OuterSystemRetVO(), string.Empty);
            int failCount = 5;
            var config = ConfigService.GetConfig(new InfLogFacFailConfig(), typeof(InfNcDataLogFactory));

            if (config != null && config.FailCount > 0)
            {
                failCount = config.FailCount;
            }
            //查找所有工厂
            var smomControlSettingDic = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettings().ToDictionary(p => p.FactoryCode);
            if (smomControlSettingDic == null || smomControlSettingDic.Count == 0)
            {
                throw new ValidationException("没有维护SMOM总控配置数据!".L10N());
            }

            var crtgroup = RT.Service.Resolve<InfNcDataLogGroupController>();
            var crtfactory = RT.Service.Resolve<InfNcDataLogFactoryController>();
            //优先查询之前同步失败的数据.条件是:未同步,或者已同步但是还未超过配置的次数的子工厂日志   其次查询总控需要传输的数据条件是:未传输的,传输后直接改成
            var groupWaitSendList = crtgroup.GetWaitSendInfNcDataLogSOGroups(infType, BatchNo);
            var factoryWaitSendList = crtfactory.GetWaitSendInfNcDataSOLogFactorys(infType, BatchNo);
            if (groupWaitSendList.Count == 0 && factoryWaitSendList.Count == 0)
            {
                return stringBuilder.Append("接口{0}没有需要传输的数据!".L10nFormat(infType.ToLabel()));
            }
            if (factoryWaitSendList.Count > 0)
            {
                //推送工厂日志
                PushFactoryDatas(factoryWaitSendList, smomControlSettingDic, infType, failCount, stringBuilder);
            }
            if (groupWaitSendList.Count > 0)
            {
                //推送NC总控日志
                PushGroupLogDatas(groupWaitSendList, smomControlSettingDic, infType, stringBuilder, ref tuple);

            }
            return stringBuilder;

        }


        /// <summary>
        /// 执行总控到子工厂数据的传输
        /// </summary>
        /// <returns></returns>
        public virtual StringBuilder CorpFactoryJobExecute(InfType infType)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Tuple<OuterSystemRetVO, string> tuple = new (new OuterSystemRetVO(), string.Empty);
            int failCount = 5;
            var config = ConfigService.GetConfig(new InfLogFacFailConfig(), typeof(InfNcDataLogFactory));

            if (config != null && config.FailCount > 0)
            {
                failCount = config.FailCount;
            }
            //查找所有工厂
            var smomControlSettingDic = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettings().ToDictionary(p => p.FactoryCode);
            if (smomControlSettingDic == null || smomControlSettingDic.Count == 0)
            {
                throw new ValidationException("没有维护SMOM总控配置数据!".L10N());
            }

            var crtgroup = RT.Service.Resolve<InfNcDataLogGroupController>();
            var crtfactory = RT.Service.Resolve<InfNcDataLogFactoryController>();
            //优先查询之前同步失败的数据.条件是:未同步,或者已同步但是还未超过配置的次数的子工厂日志   其次查询总控需要传输的数据条件是:未传输的,传输后直接改成
            var groupWaitSendList = crtgroup.GetWaitSendInfNcDataLogGroups(infType);
            var factoryWaitSendList = crtfactory.GetWaitSendInfNcDataLogFactorys(infType, failCount);
            if (groupWaitSendList.Count == 0 && factoryWaitSendList.Count == 0)
            {
                return stringBuilder.Append("接口{0}没有需要传输的数据!".L10nFormat(infType.ToLabel()));
            }
            if (factoryWaitSendList.Count > 0)
            {
                //推送工厂日志
                PushFactoryDatas(factoryWaitSendList, smomControlSettingDic, infType, failCount, stringBuilder);
            }
            if (groupWaitSendList.Count > 0)
            {
                //推送NC总控日志
                PushGroupLogDatas(groupWaitSendList, smomControlSettingDic, infType, stringBuilder, ref tuple);

            }
            return stringBuilder;

        }

        /// <summary>
        /// 推送NC总控日志到工厂
        /// </summary>
        /// <param name="groupWaitSendList"></param>
        /// <param name="smomControlSettingDic"></param>
        /// <param name="infMapping"></param>
        /// <param name="infTypeCrop"></param>
        public virtual void PushGroupLogDatas(EntityList<InfNcDataLogGroup> groupWaitSendList, Dictionary<string, SmomControlSetting> smomControlSettingDic, InfType infTypeCrop, StringBuilder stringBuilder, ref Tuple<OuterSystemRetVO, string> tuple)
        {
            //各工厂的错误失败日志,工厂名称,成功次数,失败次数
            Dictionary<string, TempVoData> tipMsgDic = new Dictionary<string, TempVoData>();
            foreach (var groupWaitSend in groupWaitSendList)
            {
                string errorMsg = string.Empty;
                try
                {
                    EntityList<InfNcDataLogFactory> factoryInfDatas = new EntityList<InfNcDataLogFactory>();
                    if (groupWaitSend.InvOrg.IsNullOrEmpty())
                    {
                        foreach (var smomControlSetting1 in smomControlSettingDic.Values)
                        {
                            try
                            {
                                tuple = PushNcBaseDataToFactory(smomControlSetting1.FactoryUrl, groupWaitSend.DataJsons, InfMappingUrlConfig._controller, InfMappingUrlConfig._method, groupWaitSend.NcInfCode, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, infTypeCrop, smomControlSetting1.FactoryCode, groupWaitSend.BatchNo);
                                var response = tuple.Item1;
                                if (response.errorList.Count > 0)
                                {
                                    //拆分工厂级日志
                                    foreach (var errorData in response.errorList)
                                    {
                                        var factoryInfData = GenerageInfNcDataLogFactory(smomControlSetting1.FactoryCode, smomControlSetting1.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
                                        factoryInfDatas.Add(factoryInfData);
                                    }
                                    groupWaitSend.SendState = SendState.SendFail;
                                    groupWaitSend.FactoryErrorMsg += response.errorMsg;
                                    if (tipMsgDic.ContainsKey(smomControlSetting1.FactoryName))
                                    {

                                        tipMsgDic[smomControlSetting1.FactoryName].FailCount++;
                                    }
                                    else
                                    {
                                        tipMsgDic[smomControlSetting1.FactoryName] = new TempVoData() { FailCount = 1 };
                                    }
                                }
                                else
                                {
                                    groupWaitSend.SendState = SendState.SendSuccess;
                                    if (tipMsgDic.ContainsKey(smomControlSetting1.FactoryName))
                                    {

                                        tipMsgDic[smomControlSetting1.FactoryName].SuccessCount++;
                                    }
                                    else
                                    {
                                        tipMsgDic[smomControlSetting1.FactoryName] = new TempVoData() { SuccessCount = 1 };
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                foreach (var errorData in JsonConvert.DeserializeObject<List<object>>(groupWaitSend.DataJsons))
                                {
                                    var factoryInfData = GenerageInfNcDataLogFactory(smomControlSetting1.FactoryCode, smomControlSetting1.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
                                    factoryInfDatas.Add(factoryInfData);
                                }
                                errorMsg += $"[{DateTime.Now}] 工厂:{smomControlSetting1.FactoryName}推送接口:{groupWaitSend.InfType.ToLabel()}的ID为{groupWaitSend.BatchNo}失败,原因是:{ex.Message};";
                                groupWaitSend.FactoryErrorMsg += errorMsg;
                                groupWaitSend.SendState = SendState.SendFail;
                                if (tipMsgDic.ContainsKey(smomControlSetting1.FactoryName))
                                {

                                    tipMsgDic[smomControlSetting1.FactoryName].FailCount++;
                                }
                                else
                                {
                                    tipMsgDic[smomControlSetting1.FactoryName] = new TempVoData() { FailCount = 1 };
                                }
                            }
                        }
                    }
                    else if (smomControlSettingDic.TryGetValue(groupWaitSend.InvOrg, out SmomControlSetting smomControlSetting) && smomControlSetting.FactoryUrl.IsNotEmpty())
                    {
                        try
                        {
                            string str = string.Empty;
                            //当这条信息是从 原工厂复制出来的时候，在工厂里面要做特殊处理
                            //工作中心类型，切实原SAP复制出来的
                            if (infTypeCrop == InfType.WorkCenter)
                            {
                                if (groupWaitSend.IsDistribute == true)
                                {
                                    //告诉工厂要做特殊处理
                                    str = "IsDistribute";
                                }
                            }
                            tuple = PushNcBaseDataToFactory(smomControlSetting.FactoryUrl, groupWaitSend.DataJsons, InfMappingUrlConfig._controller, InfMappingUrlConfig._method, groupWaitSend.NcInfCode, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, infTypeCrop, smomControlSetting.FactoryCode, groupWaitSend.BatchNo, str);
                            groupWaitSend.ResponseContent = JsonConvert.SerializeObject(tuple.Item2);
                            var response = tuple.Item1;
                            if (response.errorList.Count > 0)
                            {
                                //拆分工厂级日志
                                foreach (var errorData in response.errorList)
                                {
                                    var factoryInfData = GenerageInfNcDataLogFactory(groupWaitSend.InvOrg, groupWaitSend.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
                                    factoryInfDatas.Add(factoryInfData);
                                }
                                groupWaitSend.SendState = SendState.SendFail;
                                groupWaitSend.FactoryErrorMsg += response.errorMsg;
                                if (tipMsgDic.ContainsKey(groupWaitSend.FactoryName))
                                {

                                    tipMsgDic[groupWaitSend.FactoryName].FailCount++;
                                }
                                else
                                {
                                    tipMsgDic[groupWaitSend.FactoryName] = new TempVoData() { FailCount = 1 };
                                }
                            }
                            else
                            {
                                groupWaitSend.SendState = SendState.SendSuccess;
                                if (tipMsgDic.ContainsKey(groupWaitSend.FactoryName))
                                {

                                    tipMsgDic[groupWaitSend.FactoryName].SuccessCount++;
                                }
                                else
                                {
                                    tipMsgDic[groupWaitSend.FactoryName] = new TempVoData() { SuccessCount = 1 };
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            if (infTypeCrop == InfType.WorkOrder)
                            {
                                var errorData = JsonConvert.DeserializeObject<WorkOrderData>(groupWaitSend.DataJsons);
                                var factoryInfData = GenerageInfNcDataLogFactory(smomControlSetting.FactoryCode, smomControlSetting.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
                                factoryInfDatas.Add(factoryInfData);
                            }
                            else if (infTypeCrop == InfType.ReworkLayoutVersion)
                            {
                                var errorData = JsonConvert.DeserializeObject<Rlvd>(groupWaitSend.DataJsons);
                                var factoryInfData = GenerageInfNcDataLogFactory(smomControlSetting.FactoryCode, smomControlSetting.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
                                factoryInfDatas.Add(factoryInfData);
                            }
                            else
                            {
                                foreach (var errorData in JsonConvert.DeserializeObject<List<object>>(groupWaitSend.DataJsons))
                                {
                                    var factoryInfData = GenerageInfNcDataLogFactory(smomControlSetting.FactoryCode, smomControlSetting.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
                                    factoryInfDatas.Add(factoryInfData);
                                }
                                if (tipMsgDic.ContainsKey(groupWaitSend.FactoryName))
                                {

                                    tipMsgDic[groupWaitSend.FactoryName].FailCount++;
                                }
                                else
                                {
                                    tipMsgDic[groupWaitSend.FactoryName] = new TempVoData() { FailCount = 1 };
                                }
                            }
                            errorMsg += $"工厂:{groupWaitSend.FactoryName}推送接口:{groupWaitSend.InfType.ToLabel()}的ID为{groupWaitSend.BatchNo}失败,原因是:{ex.Message}";
                            groupWaitSend.SendState = SendState.SendFail;
                            groupWaitSend.FactoryErrorMsg += errorMsg;
                        }
                    }
                    else
                    {
                        groupWaitSend.SendState = SendState.SendIgnore;
                        throw new Exception("找不到对应SMOM总控配置或SMOM总控配置的Url地址为空");
                    }
                    using (var trans = DB.TransactionScope(BaseEntityDataProvider.ConnectionStringName))
                    {
                        if (factoryInfDatas.Any())
                            RF.Save(factoryInfDatas);
                        RF.Save(groupWaitSend);
                        trans.Complete();
                    }
                }
                catch (Exception ex)
                {
                    //接口调用失败，不拆分工厂级日志
                    groupWaitSend.ErrorMsg = ex.Message;
                    if (tipMsgDic.ContainsKey(groupWaitSend.FactoryName))
                    {

                        tipMsgDic[groupWaitSend.FactoryName].FailCount++;
                    }
                    else
                    {
                        tipMsgDic[groupWaitSend.FactoryName] = new TempVoData() { FailCount = 1 };
                    }
                    RF.Save(groupWaitSend);
                }
            }
            //统计各个工厂成功失败数及详细信息:
            stringBuilder.Append("\r\n总控与NC接口日志");
            var finalMsg = tipMsgDic.Select(x => { return $"{x.Key}成功[{x.Value.SuccessCount}]失败[{x.Value.FailCount}]\r\n"; });
            foreach (var item in finalMsg)
            {
                stringBuilder.Append(item);
            }
        }

        /// <summary>
        /// 拆分生成工厂日志
        /// </summary>
        /// <param name="factoryName"></param>
        /// <param name="groupGuid"></param>
        /// <param name="infType"></param>
        /// <param name="json"></param>
        /// <param name="ncSystemCode"></param>
        /// <param name="ncOperationType"></param>
        /// <param name="ncInfCode"></param>
        /// <returns></returns>
        //private InfNcDataLogFactory GenerageInfNcDataLogFactory(string factoryCode, string facName, string groupGuid, InfType? infType, object jsonObj, string ncSystemCode, string ncOperationType, string ncInfCode)
        //{
        //    InfNcDataLogFactory factoryData = new InfNcDataLogFactory()
        //    {
        //        FailCount = 0,
        //        InvOrg = factoryCode,
        //        FactoryName = facName,
        //        GroupGuid = groupGuid,
        //        BatchNo = Guid.NewGuid().ToString(),
        //        InfType = infType,
        //        DataJsons = JsonConvert.SerializeObject(new List<object>() { jsonObj }),
        //        SendState = SendState.NoSend,
        //        NcSystemCode = ncSystemCode,
        //        NcOperationType = ncOperationType,
        //        NcInfCode = ncInfCode
        //    };
        //    if (infType == InfType.ProductParameter)
        //    {
        //        //ProductParameterData data = jsonObj as ProductParameterData;
        //        //if (data != null)
        //        //{
        //        //    factoryData.KeyMsgone = data.ProductCode + "-" + data.ResourceCode;
        //        //}
        //    }
        //    else
        //    {
        //        var obj = JObject.Parse(JsonConvert.SerializeObject(jsonObj));
        //        if (obj.ContainsKey("Code"))
        //        {
        //            factoryData.KeyMsgone = obj["Code"].ToString();
        //        }
        //        else if (obj.ContainsKey("code"))
        //        {
        //            factoryData.KeyMsgone = obj["code"].ToString();
        //        }
        //    }
        //    return factoryData;
        //}

        /// <summary>
        /// 执行总控到子工厂数据的传输
        /// </summary>
        /// <returns></returns>
        //public virtual StringBuilder CorpFactoryJobExecute(InfTypeGroup infTypeGroup)
        //{
        //    StringBuilder stringBuilder = new StringBuilder();
        //    int failCount = 5;
        //    var config = ConfigService.GetConfig(new InfLogFacFailConfig(), typeof(InfNcDataLogFactory));

        //    if (config != null && config.FailCount > 0)
        //    {
        //        failCount = config.FailCount;
        //    }
        //    //查找所有工厂
        //    var smomControlSettingDic = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettings().ToDictionary(p => p.FactoryCode);
        //    if (smomControlSettingDic == null || smomControlSettingDic.Count == 0)
        //    {
        //        throw new ValidationException("没有维护SMOM总控配置数据!".L10N());
        //    }
        //    InfType infType = (InfType)(int)infTypeGroup;

        //    var infMapping = Query<InfMapping>().Where(p => p.InfType == infType).FirstOrDefault();
        //    if (infMapping == null)
        //    {
        //        throw new ValidationException("没有维护菜单基础数据接口信息!".L10N());
        //    }
        //    if (infMapping.ApiType.IsNullOrEmpty() || infMapping.Method.IsNullOrEmpty())
        //    {
        //        throw new ValidationException("接口类型{0}没有维护控制器或者方法名信息!".L10nFormat(infTypeGroup.ToLabel()));
        //    }

        //    var crtgroup = RT.Service.Resolve<InfNcDataLogGroupController>();
        //    var crtfactory = RT.Service.Resolve<InfNcDataLogFactoryController>();
        //    //优先查询之前同步失败的数据.条件是:未同步,或者已同步但是还未超过配置的次数的子工厂日志   其次查询总控需要传输的数据条件是:未传输的,传输后直接改成
        //    var groupWaitSendList = crtgroup.GetWaitSendInfNcDataLogGroups(infTypeGroup);
        //    var factoryWaitSendList = crtfactory.GetWaitSendInfNcDataLogFactorys(infTypeGroup, failCount);
        //    if (groupWaitSendList.Count == 0 && factoryWaitSendList.Count == 0)
        //    {
        //        return stringBuilder.Append("接口{0}没有需要传输的数据!".L10nFormat(infTypeGroup.ToLabel()));
        //    }
        //    if (factoryWaitSendList.Count > 0)
        //    {
        //        //推送工厂日志
        //        PushFactoryDatas(factoryWaitSendList, smomControlSettingDic, infMapping, infTypeGroup, failCount, stringBuilder);
        //    }
        //    if (groupWaitSendList.Count > 0)
        //    {
        //        //推送NC总控日志
        //        PushGroupLogDatas(groupWaitSendList, smomControlSettingDic, infMapping, infTypeGroup, stringBuilder);

        //    }
        //    return stringBuilder;

        //}

        /// <summary>
        /// 推送NC总控日志到工厂
        /// </summary>
        /// <param name="groupWaitSendList"></param>
        /// <param name="smomControlSettingDic"></param>
        /// <param name="infMapping"></param>
        /// <param name="infTypeCrop"></param>
        //public virtual void PushGroupLogDatas(EntityList<InfNcDataLogGroup> groupWaitSendList, Dictionary<string, SmomControlSetting> smomControlSettingDic, InfMapping infMapping, InfTypeGroup infTypeCrop, StringBuilder stringBuilder)
        //{
        //    //各工厂的错误失败日志,工厂名称,成功次数,失败次数
        //    Dictionary<string, TempVoData> tipMsgDic = new Dictionary<string, TempVoData>();
        //    foreach (var groupWaitSend in groupWaitSendList)
        //    {
        //        string errorMsg = string.Empty;
        //        try
        //        {
        //            EntityList<InfNcDataLogFactory> factoryInfDatas = new EntityList<InfNcDataLogFactory>();
        //            if (groupWaitSend.InvOrg.IsNullOrEmpty())
        //            {
        //                foreach (var smomControlSetting1 in smomControlSettingDic.Values)
        //                {
        //                    try
        //                    {
        //                        var tuple = PushNcBaseDataToFactory(smomControlSetting1.FactoryUrl, groupWaitSend.SuccessJson, infMapping.ApiType, infMapping.Method, groupWaitSend.NcInfCode, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, infTypeCrop, smomControlSetting1.FactoryCode);
        //                        var response = tuple.Item1;
        //                        if (response.errorList.Count > 0)
        //                        {
        //                            //拆分工厂级日志
        //                            foreach (var errorData in response.errorList)
        //                            {
        //                                var factoryInfData = GenerageInfNcDataLogFactory(smomControlSetting1.FactoryCode, smomControlSetting1.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
        //                                factoryInfDatas.Add(factoryInfData);
        //                            }
        //                            groupWaitSend.SendState = SendState.SendFail;
        //                            groupWaitSend.FactoryErrorMsg += response.errorMsg;
        //                            if (tipMsgDic.ContainsKey(smomControlSetting1.FactoryName))
        //                            {

        //                                tipMsgDic[smomControlSetting1.FactoryName].FailCount++;
        //                            }
        //                            else
        //                            {
        //                                tipMsgDic[smomControlSetting1.FactoryName] = new TempVoData() { FailCount = 1 };
        //                            }
        //                        }
        //                        else
        //                        {
        //                            groupWaitSend.SendState = SendState.SendSuccess;
        //                            if (tipMsgDic.ContainsKey(smomControlSetting1.FactoryName))
        //                            {

        //                                tipMsgDic[smomControlSetting1.FactoryName].SuccessCount++;
        //                            }
        //                            else
        //                            {
        //                                tipMsgDic[smomControlSetting1.FactoryName] = new TempVoData() { SuccessCount = 1 };
        //                            }
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        foreach (var errorData in JsonConvert.DeserializeObject<List<object>>(groupWaitSend.SuccessJson))
        //                        {
        //                            var factoryInfData = GenerageInfNcDataLogFactory(smomControlSetting1.FactoryCode, smomControlSetting1.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
        //                            factoryInfDatas.Add(factoryInfData);
        //                        }
        //                        errorMsg += $"[{DateTime.Now}] 工厂:{smomControlSetting1.FactoryName}推送接口:{groupWaitSend.InfType.ToLabel()}的ID为{groupWaitSend.BatchNo}失败,原因是:{ex.Message};";
        //                        groupWaitSend.FactoryErrorMsg += errorMsg;
        //                        groupWaitSend.SendState = SendState.SendFail;
        //                        if (tipMsgDic.ContainsKey(smomControlSetting1.FactoryName))
        //                        {

        //                            tipMsgDic[smomControlSetting1.FactoryName].FailCount++;
        //                        }
        //                        else
        //                        {
        //                            tipMsgDic[smomControlSetting1.FactoryName] = new TempVoData() { FailCount = 1 };
        //                        }
        //                    }
        //                }
        //            }
        //            else if (smomControlSettingDic.TryGetValue(groupWaitSend.InvOrg, out SmomControlSetting smomControlSetting) && smomControlSetting.FactoryUrl.IsNotEmpty())
        //            {
        //                try
        //                {
        //                    var tuple = PushNcBaseDataToFactory(smomControlSetting.FactoryUrl, groupWaitSend.SuccessJson, infMapping.ApiType, infMapping.Method, groupWaitSend.NcInfCode, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, infTypeCrop, smomControlSetting.FactoryCode);
        //                    groupWaitSend.ResponseContent = JsonConvert.SerializeObject(tuple.Item2);
        //                    var response = tuple.Item1;
        //                    if (response.errorList.Count > 0)
        //                    {
        //                        //拆分工厂级日志
        //                        foreach (var errorData in response.errorList)
        //                        {
        //                            var factoryInfData = GenerageInfNcDataLogFactory(groupWaitSend.InvOrg, groupWaitSend.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
        //                            factoryInfDatas.Add(factoryInfData);
        //                        }
        //                        groupWaitSend.SendState = SendState.SendFail;
        //                        groupWaitSend.FactoryErrorMsg += response.errorMsg;
        //                        if (tipMsgDic.ContainsKey(groupWaitSend.FactoryName))
        //                        {

        //                            tipMsgDic[groupWaitSend.FactoryName].FailCount++;
        //                        }
        //                        else
        //                        {
        //                            tipMsgDic[groupWaitSend.FactoryName] = new TempVoData() { FailCount = 1 };
        //                        }
        //                    }
        //                    else
        //                    {
        //                        groupWaitSend.SendState = SendState.SendSuccess;
        //                        if (tipMsgDic.ContainsKey(groupWaitSend.FactoryName))
        //                        {

        //                            tipMsgDic[groupWaitSend.FactoryName].SuccessCount++;
        //                        }
        //                        else
        //                        {
        //                            tipMsgDic[groupWaitSend.FactoryName] = new TempVoData() { SuccessCount = 1 };
        //                        }
        //                    }

        //                }
        //                catch (Exception ex)
        //                {
        //                    foreach (var errorData in JsonConvert.DeserializeObject<List<object>>(groupWaitSend.SuccessJson))
        //                    {
        //                        var factoryInfData = GenerageInfNcDataLogFactory(smomControlSetting.FactoryCode, smomControlSetting.FactoryName, groupWaitSend.BatchNo, groupWaitSend.InfType, errorData, groupWaitSend.NcSystemCode, groupWaitSend.NcOperationType, groupWaitSend.NcInfCode);
        //                        factoryInfDatas.Add(factoryInfData);
        //                    }
        //                    if (tipMsgDic.ContainsKey(groupWaitSend.FactoryName))
        //                    {

        //                        tipMsgDic[groupWaitSend.FactoryName].FailCount++;
        //                    }
        //                    else
        //                    {
        //                        tipMsgDic[groupWaitSend.FactoryName] = new TempVoData() { FailCount = 1 };
        //                    }
        //                    errorMsg += $"工厂:{groupWaitSend.FactoryName}推送接口:{groupWaitSend.InfType.ToLabel()}的ID为{groupWaitSend.BatchNo}失败,原因是:{ex.Message}";
        //                    groupWaitSend.SendState = SendState.SendFail;
        //                    groupWaitSend.FactoryErrorMsg += errorMsg;
        //                }
        //            }
        //            else
        //            {
        //                groupWaitSend.SendState = SendState.SendIgnore;
        //                throw new Exception("找不到对应SMOM总控配置或SMOM总控配置的Url地址为空");
        //            }
        //            using (var trans = DB.TransactionScope(BaseEntityDataProvider.ConnectionStringName))
        //            {
        //                if (factoryInfDatas.Any())
        //                    RF.Save(factoryInfDatas);
        //                RF.Save(groupWaitSend);
        //                trans.Complete();
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            //接口调用失败，不拆分工厂级日志
        //            groupWaitSend.ErrorMsg = ex.Message;
        //            if (tipMsgDic.ContainsKey(groupWaitSend.FactoryName))
        //            {

        //                tipMsgDic[groupWaitSend.FactoryName].FailCount++;
        //            }
        //            else
        //            {
        //                tipMsgDic[groupWaitSend.FactoryName] = new TempVoData() { FailCount = 1 };
        //            }
        //        }
        //    }
        //    //统计各个工厂成功失败数及详细信息:
        //    stringBuilder.Append("\r\n总控与NC接口日志");
        //    var finalMsg = tipMsgDic.Select(x => { return $"{x.Key}成功[{x.Value.SuccessCount}]失败[{x.Value.FailCount}]\r\n"; });
        //    foreach (var item in finalMsg)
        //    {
        //        stringBuilder.Append(item);
        //    }
        //}

        /// <summary>
        /// 拆分生成工厂日志
        /// </summary>
        /// <param name="factoryName"></param>
        /// <param name="groupGuid"></param>
        /// <param name="infType"></param>
        /// <param name="json"></param>
        /// <param name="ncSystemCode"></param>
        /// <param name="ncOperationType"></param>
        /// <param name="ncInfCode"></param>
        /// <returns></returns>
        private InfNcDataLogFactory GenerageInfNcDataLogFactory(string factoryCode, string facName, string groupGuid, InfType? infType, object jsonObj, string ncSystemCode, string ncOperationType, string ncInfCode)
        {
            var dataJsons = JsonConvert.SerializeObject(new List<object>() { jsonObj });
            if (infType != null && (infType.Value == InfType.WorkOrder || infType.Value == InfType.ReworkLayoutVersion))
            {
                dataJsons = JsonConvert.SerializeObject(jsonObj);
            }
            InfNcDataLogFactory factoryData = new InfNcDataLogFactory()
            {
                FailCount = 0,
                InvOrg = factoryCode,
                FactoryName = facName,
                GroupGuid = groupGuid,
                BatchNo = Guid.NewGuid().ToString(),
                InfType = infType,
                DataJsons = dataJsons,
                SendState = SendState.NoSend,
                NcSystemCode = ncSystemCode,
                NcOperationType = ncOperationType,
                NcInfCode = ncInfCode
            };
            var obj = JObject.Parse(JsonConvert.SerializeObject(jsonObj));
            if (obj.ContainsKey("Code"))
            {
                factoryData.KeyMsgone = obj["Code"].ToString();
            }
            else if (obj.ContainsKey("code"))
            {
                factoryData.KeyMsgone = obj["code"].ToString();
            }
            return factoryData;
        }

        /// <summary>
        /// 推送工厂日志到工厂
        /// </summary>
        /// <param name="factoryWaitSendList"></param>
        /// <param name="smomControlSettingDic"></param>
        /// <param name="infMapping"></param>
        /// <param name="failCount"></param>
        public virtual StringBuilder PushFactoryDatas(EntityList<InfNcDataLogFactory> factoryWaitSendList, Dictionary<string, SmomControlSetting> smomControlSettingDic, InfType infTypeCrop, int failCount, StringBuilder stringBuilder)
        {
            //各工厂的错误失败日志,工厂名称,成功次数,失败次数
            Dictionary<string, TempVoData> tipMsgDic = new Dictionary<string, TempVoData>();
            foreach (var factoryWaitSend in factoryWaitSendList)
            {
                string errorMsg = string.Empty;
                if (smomControlSettingDic.TryGetValue(factoryWaitSend.InvOrg, out SmomControlSetting smomControlSetting) && smomControlSetting.FactoryUrl.IsNotEmpty())
                {
                    try
                    {
                        var tuple = PushNcBaseDataToFactory(smomControlSetting.FactoryUrl, factoryWaitSend.DataJsons, InfMappingUrlConfig._controller, InfMappingUrlConfig._method, factoryWaitSend.NcInfCode, factoryWaitSend.NcSystemCode, factoryWaitSend.NcOperationType, infTypeCrop, smomControlSetting.FactoryCode, factoryWaitSend.GroupGuid);
                        //factoryWaitSend.ResponseContent = JsonConvert.SerializeObject(tuple.Item2);
                        var response = tuple.Item1;
                        if (response.errorList.Count > 0)
                        {
                            SetInfNcDataLogFactoryErrorMsg(failCount, factoryWaitSend, response.errorMsg);
                            factoryWaitSend.SendState = SendState.SendFail;
                            if (tipMsgDic.ContainsKey(factoryWaitSend.FactoryName))
                            {

                                tipMsgDic[factoryWaitSend.FactoryName].FailCount++;
                            }
                            else
                            {
                                tipMsgDic[factoryWaitSend.FactoryName] = new TempVoData() { FailCount = 1 };
                            }
                        }
                        else
                        {
                            factoryWaitSend.SendState = SendState.SendSuccess;
                            if (tipMsgDic.ContainsKey(factoryWaitSend.FactoryName))
                            {

                                tipMsgDic[factoryWaitSend.FactoryName].SuccessCount++;
                            }
                            else
                            {
                                tipMsgDic[factoryWaitSend.FactoryName] = new TempVoData() { SuccessCount = 1 };
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        factoryWaitSend.SendState = SendState.SendFail;
                        SetInfNcDataLogFactoryErrorMsg(failCount, factoryWaitSend, ex.Message);
                        if (tipMsgDic.ContainsKey(factoryWaitSend.FactoryName))
                        {
                            tipMsgDic[factoryWaitSend.FactoryName].FailCount++;
                        }
                        else
                        {
                            tipMsgDic[factoryWaitSend.FactoryName] = new TempVoData() { FailCount = 1 };
                        }
                        errorMsg = $"[{DateTime.Now}] 工厂:{factoryWaitSend.FactoryName}推送接口:{factoryWaitSend.InfType.ToLabel()}的ID为{factoryWaitSend.BatchNo}失败,原因是:{ex.Message}";
                        factoryWaitSend.ErrorMsg = errorMsg;
                    }
                }
                else
                {
                    factoryWaitSend.SendState = SendState.SendFail;
                    SetInfNcDataLogFactoryErrorMsg(failCount, factoryWaitSend, "找不到对应SMOM总控配置或SMOM总控配置的Url地址为空");
                    if (tipMsgDic.ContainsKey(factoryWaitSend.FactoryName))
                    {
                        tipMsgDic[factoryWaitSend.FactoryName].FailCount++;
                    }
                    else
                    {
                        tipMsgDic[factoryWaitSend.FactoryName] = new TempVoData() { FailCount = 1 };
                    }
                }
                RF.Save(factoryWaitSend);
            }
            //统计各个工厂成功失败数及详细信息:
            stringBuilder.Append("\r\n总控与工厂接口日志");
            var finalMsg = tipMsgDic.Select(x => { return $"{x.Key}成功[{x.Value.SuccessCount}]失败[{x.Value.FailCount}]\r\n"; });
            foreach (var item in finalMsg)
            {
                stringBuilder.Append(item);
            }
            return stringBuilder;
        }


        /// <summary>
        /// 设置工厂的错误消息
        /// </summary>
        /// <param name="failCount"></param>
        /// <param name="factoryWaitSend"></param>
        /// <param name="errorMsg"></param>
        private void SetInfNcDataLogFactoryErrorMsg(int failCount, InfNcDataLogFactory factoryWaitSend, string errorMsg)
        {
            factoryWaitSend.FailCount++;
            factoryWaitSend.ErrorMsg = errorMsg;
            if (factoryWaitSend.FailCount >= failCount)
                factoryWaitSend.SendState = SendState.SendFail;
        }

        /// <summary>
        /// 调用工厂的Api接口
        /// </summary>
        /// <param name="url"></param>
        /// <param name="json"></param>
        /// <param name="apiType"></param>
        /// <param name="method"></param>
        /// <param name="infCode"></param>
        /// <param name="systemCode"></param>
        /// <param name="operationType"></param>
        /// <param name="infTypeCrop"></param>
        /// <param name="str">附带传输给工厂的东西，按照自己的想法去传</param>
        /// <returns></returns>
        private Tuple<OuterSystemRetVO, string> PushNcBaseDataToFactory(string url, string json, string apiType, string method, string infCode, string systemCode, string operationType, InfType infTypeCrop, string invOrgId, string groupGuid, string str = null)
        {
            OuterSystemRetVO response = new OuterSystemRetVO();
            string responseStr = string.Empty;
            var smomParam = new List<SmomParam>()
                {
                    new SmomParam { Value =systemCode },
                                    new SmomParam { Value =infCode },
                                    new SmomParam { Value =operationType },
                                    new SmomParam { Value =json },
                                    new SmomParam{ Value =invOrgId },
                                    new SmomParam{ Value =groupGuid },
                                    new SmomParam{ Value =str }
                                 }.ToArray();
            response = SmomControlHepler.SmomPost<OuterSystemRetVO>(apiType, method, url, smomParam);
            responseStr = JsonConvert.SerializeObject(response);


            //if (systemRetVOTypes.Contains(infTypeCrop))
            //{
            //    var smomParam = new List<SmomParam>()
            //    {
            //        new SmomParam { Value =systemCode },
            //                        new SmomParam { Value =infCode },
            //                        new SmomParam { Value =operationType },
            //                        new SmomParam { Value =json },
            //                        new SmomParam{ Value =invOrgId }
            //                     }.ToArray();
            //    response = SmomControlHepler.SmomPost<OuterSystemRetVO>(apiType, method, url, smomParam);
            //    responseStr = JsonConvert.SerializeObject(response);
            //}
            //else
            //{
            //    var smomParam = new List<SmomParam>()
            //    {
            //                    new SmomParam { Value =JsonConvert.DeserializeObject<List<object>>(json)}
            //                 }.ToArray();

            //    var response1 = SmomControlHepler.SmomPost<ApiCommonRes>(apiType, method, url, smomParam);
            //    responseStr = JsonConvert.SerializeObject(response1);
            //    response.success = true;
            //    response.errorMsg = String.Join(",", response1.ErrorList);
            //    response.errorList = response1.ErrorObjList;
            //}

            return Tuple.Create(response, responseStr);
        }


    }


    [Serializable]
    public class TempVoData
    {
        public int SuccessCount { get; set; }
        public int FailCount { get; set; }

    }

}
