using SIE.Domain;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SIE.KZ.Base.Interfaces.ViewModels;
using SIE.Domain.Validation;

namespace SIE.KZ.Base.SmomControl
{
    public class InfNcDataLogGroupController : DomainController
    {

        /// <summary>
        /// 同步到其他工厂
        /// </summary>
        /// <param name="viewModel"></param>
        public virtual void LogGroupSyncOtherFactory(LogGroupSyncOtherFactoryViewModel viewModel)
        {
            if (viewModel.Factory.IsNullOrEmpty())
                throw new ValidationException("请输入要同步到哪个工厂".L10N());
            if (viewModel.Ids.IsNullOrEmpty())
                throw new ValidationException("请选择要同步的数据".L10N());

            SmomControlSetting smomControlSetting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(viewModel.Factory);
            if (smomControlSetting == null)
                throw new ValidationException("【SMOM总控配置】未配置对应的工厂，无法同步".L10N());

            var ids = viewModel.Ids.Split(",").Select(p => double.Parse(p)).ToList();
            var logs = GetInfNcDataLogGroupsByIds(ids);

            EntityList<InfNcDataLogGroup> logGroups = new EntityList<InfNcDataLogGroup>();
            foreach (var log in logs)
            {
                InfNcDataLogGroup logGroup = new InfNcDataLogGroup();
                logGroup.Clone(log, new CloneOptions(CloneActions.NormalProperties));
                logGroup.PersistenceStatus = PersistenceStatus.New;
                logGroup.GenerateId();

                logGroup.SendState = KZ.Base.Interfaces.Enums.SendState.NoSend;
                logGroup.FactoryErrorMsg = null;
                logGroup.ErrorMsg = null;
                logGroup.SysncResult = null;
                logGroup.SuccessJson = null;
                logGroup.BeginDate = DateTime.Now;
                logGroup.EndDate = DateTime.Now;
                logGroup.CallResult = CallResult.UnSave;
                logGroup.FactoryErrorMsg = null;
                logGroup.IsDistribute = true;
                logGroup.FactoryName = smomControlSetting.FactoryName;
                logGroup.InvOrg = smomControlSetting.FactoryCode;
                logGroup.BatchNo = Guid.NewGuid().ToString();
                logGroup.ResponseContent = null;

                logGroups.Add(logGroup);
            }
            if (logGroups.Count > 0)
                RF.Save(logGroups);
        }

        /// <summary>
        /// 根据ID获取总控与SAP接口日志
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<InfNcDataLogGroup> GetInfNcDataLogGroupsByIds(List<double> ids)
        {
            var list = ids.SplitContains(i =>
            {
                return Query<InfNcDataLogGroup>().Where(p => i.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

            return list;
        }

        /// <summary>
        /// 接口日志finally调用
        /// </summary>
        /// <returns></returns>
        public virtual InfNcDataLogGroup UpdateInfNcDataLogGroupData<T>(InfNcDataLogGroup erpDataInfLog, int sumCount, List<T> list, ApiCommonRes apiResult, bool hasResponse = true)
        {
            erpDataInfLog.SuccessJson = list.Count == 0 ? null : JsonConvert.SerializeObject(list);
            var failCount = sumCount - list.Count;
            erpDataInfLog.SysncResult = $"总条数:{sumCount} 成功数:{list.Count} 失败数:{failCount}";
            erpDataInfLog.EndDate = DateTime.Now;
            if (failCount > 0)
            {
                erpDataInfLog.ErrorMsg = $"{erpDataInfLog.InfType?.ToLabel()}接口调用失败!失败原因是{string.Join(",", apiResult.ErrorList)}";
                erpDataInfLog.CallResult = CallResult.Fail;
            }
            else
            {
                erpDataInfLog.CallResult = CallResult.Success;
            }
            if (hasResponse)
            {
                erpDataInfLog.ResponseContent = JsonConvert.SerializeObject(apiResult);
            }
            RF.Save(erpDataInfLog);
            return erpDataInfLog;
        }


        public virtual InfNcDataLogGroup SaveInfNcDataLogGroup(string invOrg, string dataJsons
                                                , DateTime beginDate
                                                , InfType? infType
                                                , CallResult callResult
                                                , string batchNo
                                                , string errorMsg = null
                                                , string KeyMsgone = null
                                                , string KeyMsgfive = null
                                                , string systemCode = null
                                                , string operationType = null
                                                , string infCode = null
                                                , bool? isDistribute = null
                                                , DateTime? endDate = null
                                                )
        {

            InfNcDataLogGroup entity = new InfNcDataLogGroup();
            entity.BatchNo = batchNo;
            entity.InvOrg = invOrg;
            entity.DataJsons = dataJsons;
            entity.BeginDate = beginDate;
            entity.InfType = infType;
            entity.CallResult = callResult;
            entity.ErrorMsg = errorMsg;
            entity.KeyMsgone = KeyMsgone;
            entity.KeyMsgfive = KeyMsgfive;
            entity.NcSystemCode = systemCode;
            entity.NcOperationType = operationType;
            entity.NcInfCode = infCode;
            entity.IsDistribute = isDistribute;
            entity.EndDate = endDate;
            if (invOrg != null)
            {
                var smomsetting = Query<SmomControlSetting>().Where(p => p.FactoryCode == invOrg).FirstOrDefault();
                if (smomsetting != null)
                {
                    entity.FactoryName = smomsetting.FactoryName;
                }
            }
            RF.Save(entity);
            return entity;
        }

        /// <summary>
        /// 根据接口类型获取需要传输的数据
        /// </summary>
        /// <param name="infType"></param>
        /// <returns></returns>
        public virtual EntityList<InfNcDataLogGroup> GetWaitSendInfNcDataLogGroups(InfType infType)
        {
            return Query<InfNcDataLogGroup>().Where(p => p.InfType == infType && p.SendState == SendState.NoSend /*&& p.SuccessJson != null*/).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, 20000));
        }

        /// <summary>
        /// 查询工单类型获取需要传输的数据
        /// </summary>
        /// <param name="infType"></param>
        /// <returns></returns>
        public virtual EntityList<InfNcDataLogGroup> GetWaitSendInfNcDataLogSOGroups(InfType infType,string BatchNo)
        {
            var batchNos = BatchNo?.Split(',').Select(b => b.Trim()).Where(b => !string.IsNullOrEmpty(b)).Distinct().ToList() ?? new List<string>();

            if (batchNos.Count == 0)
            {
                // 如果没有有效的BatchNo，返回空列表或根据需求处理
                return null;
            }
            return Query<InfNcDataLogGroup>().Where(p => p.InfType == infType&& batchNos.Contains(p.BatchNo)).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, 20000));
            //return Query<InfNcDataLogGroup>().Where(p => p.InfType == infType&& batchNos.Contains(p.BatchNo)/*&& p.SuccessJson != null*/).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, 20000));
            //return Query<InfNcDataLogGroup>().Where(p => p.InfType == infType && p.BatchNo == BatchNo /*&& p.SuccessJson != null*/).OrderBy(p => p.CreateDate).ToList(new PagingInfo(1, 20000));
        }
    }
}
