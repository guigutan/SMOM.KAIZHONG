using SIE.Domain;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.EventMessages;
using SIE.Rbac.InvOrgs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Common.Controller
{
    /// <summary>
    /// 下载到业务表
    /// </summary>
    public partial class DownloadBusBaseController : DownloadBaseController
    {
        /// <summary>
        /// API保存业务表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceDatas">来源数据</param>
        /// <param name="func">执行业务逻辑</param>
        /// <param name="type">接口类型</param>
        /// <param name="invOrg">库存组织</param>
        /// <returns></returns>
        public virtual ApiResult ApiSaveBusinessData<T>(List<T> sourceDatas, Func<List<T>, List<ErpErrorData>> func, JobType type, int invOrg, bool hasInfKey = false) where T : ErpInfoData
        {
            var result = new ApiResult();
            var datas = new List<T>();
            var beginDateTime = DateTime.Now;
            var dataCount = sourceDatas.Count;
            var failCount = 0;
            var resultsDict = new Dictionary<string, ErpErrorData>();
            //初始化环境
            this.InitEnvironment(invOrg);

            //过滤空KEY值数据            
            if (hasInfKey)
                datas = sourceDatas.Where(p => p.Infkey.IsNotEmpty()).ToList();
            else
                datas = sourceDatas;
            //执行保存业务逻辑
            var results = func.Invoke(datas);
            if (hasInfKey)
                resultsDict = results.ToDictionary(p => p.Infkey, p => p);
            DateTime endDateTime = DateTime.Now;

            sourceDatas.ForEach(p =>
            {
                ErpErrorData errorData = null;
                ProcessState processState = ProcessState.Processed;
                var key = string.Empty;
                var currentSuccess = 0;

                //处理错误信息
                if (hasInfKey && (p.Infkey.IsNullOrEmpty() || resultsDict.TryGetValue(p.Infkey, out errorData)))
                {
                    //空KEY已过滤，resultsDict不会存在空key值，当且仅当p.Infkey为空，errorData为null
                    if (errorData == null)
                    {
                        errorData = new ErpErrorData
                        {
                            Infkey = "[CODE]:".L10nFormat(p.Code),
                            ErrMsg = "数据主键[InfKey]不能为空。".L10nFormat(),
                            IsChild = false
                        };
                        results.Add(errorData);
                    }

                    failCount++;
                    processState = ProcessState.Failed;
                }

                //记录日志
                key = processState == ProcessState.Processed ? p.Infkey : errorData.ErrMsg;
                currentSuccess = processState == ProcessState.Processed ? 1 : 0;
                SaveDownloadERPLog(type, 1, currentSuccess, 1 - currentSuccess,
                    JobDirection.ErpToBusiness, JobMode.Push, processState, beginDateTime, endDateTime, OperationType.API, errorData?.ErrMsg, key);
            });

            //构建返回结果
            result.ErpErrorDatas.AddRange(results);
            result.DataCount = dataCount;
            result.SuccessCount = dataCount - failCount;
            result.FailCount = failCount;
            result.BeginTime = beginDateTime;
            result.EndTime = endDateTime;

            return result;
        }

        /// <summary>
        /// 从中间表下载到业务表
        /// </summary>
        /// <typeparam name="T">主表数据类型</typeparam>
        /// <param name="sourceFunc">数据来源</param>
        /// <param name="resultFunc">执行结果</param>
        /// <param name="type">主表任务类型</param>
        /// <param name="isManual">是否手动</param>
        /// <returns></returns>
        public virtual ProcessResult SaveBusinessData<T>(Func<IEnumerable<T>> sourceFunc, Func<IEnumerable<T>, List<ErpErrorData>> resultFunc, JobType type, bool isManual = false)
            where T : DownloadBaseEntity
        {
            var result = new ProcessResult();
            var operationType = isManual ? OperationType.Manual : OperationType.Scheduling;
            int dataCount = 0;
            int successCount = 0;
            DateTime beginDateTime = DateTime.Now;

            //执行保存业务逻辑
            var sourceDatas = sourceFunc.Invoke();
            var results = resultFunc.Invoke(sourceDatas);
            var resultsDict = results.ToDictionary(p => p.Infkey, p => p);

            DateTime endDateTime = DateTime.Now;
            dataCount = sourceDatas.Count();

            //更新中间表状态
            sourceDatas.ForEach(p =>
            {
                ErpErrorData errorData;
                if (resultsDict.TryGetValue(p.Id, out errorData))
                {
                    UpdateCuxDataState(p, ProcessState.Failed);        //更新中间表数据状态
                    SaveDownloadERPLog(type, 1, 0, 1, JobDirection.InfToBusiness, JobMode.Pull, ProcessState.Failed
                        , beginDateTime, endDateTime, operationType, errorData.ErrMsg, p.Id);
                    result.AddFailMsg(errorData.ErrMsg);
                }
                else
                {
                    UpdateCuxDataState(p, ProcessState.Processed);     //更新中间表数据状态

                    successCount++;
                    result.AddSuccessMsg();
                }
            });

            //记录下载日志
            SaveDownloadERPLog(type, dataCount, successCount, dataCount - successCount, JobDirection.InfToBusiness, JobMode.Pull, ProcessState.Processed
                , beginDateTime, endDateTime, operationType);

            return result;
        }

        /// <summary>
        /// 从中间表下载到业务表(主从)
        /// </summary>
        /// <typeparam name="T">主表数据类型</typeparam>
        /// <typeparam name="D">从表数据类型</typeparam>
        /// <param name="sourceFunc">数据来源</param>
        /// <param name="sourceDtlFunc">明细数据来源</param>
        /// <param name="resultFunc">执行结果</param>
        /// <param name="type">任务类型</param>
        /// <param name="dtlType">明细任务类型</param>
        /// <param name="isManual">是否手动</param>
        /// <returns></returns>
        public virtual ProcessResult SaveBusinessData<T, D>(Func<IEnumerable<T>> sourceFunc, Func<IEnumerable<T>, IEnumerable<D>> sourceDtlFunc,
            Func<IEnumerable<T>, IEnumerable<D>, List<ErpErrorData>> resultFunc, JobType type, JobType dtlType, bool isManual = false)
            where T : DownloadBaseEntity
            where D : DownloadBaseEntity
        {
            var result = new ProcessResult();
            var operationType = isManual ? OperationType.Manual : OperationType.Scheduling;
            int dataCount = 0;
            int successCount = 0;
            DateTime beginDateTime = DateTime.Now;

            //执行保存业务逻辑
            var sourceDatas = sourceFunc.Invoke();
            var sourceDtlDatas = sourceDtlFunc.Invoke(sourceDatas);
            var results = resultFunc.Invoke(sourceDatas, sourceDtlDatas);
            var resultsDict = results.ToDictionary(p => p.Infkey, p => p);

            DateTime endDateTime = DateTime.Now;
            dataCount = sourceDatas.Count();

            //执行结果
            ////results.ToDictionary(p => p.Infkey, p => p.IsChild == false);

            //更新主表中间表状态
            sourceDatas.ForEach(p =>
            {
                ErpErrorData errorData;
                if (resultsDict.TryGetValue(p.Id, out errorData))
                {
                    UpdateCuxDataState(p, ProcessState.Failed);        //更新中间表数据状态
                    SaveDownloadERPLog(type, 1, 0, 1, JobDirection.InfToBusiness, JobMode.Pull, ProcessState.Failed
                    , beginDateTime, endDateTime, operationType, errorData.ErrMsg, p.Id);
                    result.AddFailMsg(errorData.ErrMsg);
                }
                else
                {
                    UpdateCuxDataState(p, ProcessState.Processed);     //更新中间表数据状态
                    successCount++;
                    result.AddSuccessMsg();
                    //更新明细数据状态
                    p.Children.ForEach(d =>
                    {
                        UpdateCuxDataState(d, ProcessState.Processed);
                    });
                }
            });

            //记录下载日志
            SaveDownloadERPLog(type, dataCount, successCount, dataCount - successCount, JobDirection.InfToBusiness, JobMode.Pull, ProcessState.Processed
                , beginDateTime, endDateTime, operationType);

            return result;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <typeparam name="E">实体类型</typeparam>
        /// <param name="dic">实体数据字典</param>
        /// <param name="key">待删除主键</param>
        /// <param name="entity">待删除实体</param>
        public virtual void DeleteEntity<E>(Dictionary<string, E> dic, string key, E entity) where E : Entity
        {
            if (entity.PersistenceStatus != PersistenceStatus.New)
            {
                entity.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(entity);
            }
            if (dic.ContainsKey(key))
                dic.Remove(key);
        }

        /// <summary>
        /// EBsToSmom, API保存业务表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceDatas">来源数据</param>
        /// <param name="func">执行业务逻辑</param>
        /// <param name="type">接口类型</param>
        /// <param name="extentInvOrg">库存组织</param>
        /// <returns></returns>
        public virtual ApiResult EbsApiSaveBusinessData<T>(List<T> sourceDatas, Func<List<T>, List<ErpErrorData>> func, JobType type, string extentInvOrg) where T : EbsOrderBaseData
        {
            var result = new ApiResult();

            var beginDateTime = DateTime.Now;
            var dataCount = sourceDatas.Count;
            var failCount = 0;
            var resultsDict = new Dictionary<string, ErpErrorData>();
            var allOrgs = RF.GetAll<InvOrg>();
            if (!allOrgs.Any(a => a.ExternalId == extentInvOrg))
            {
                result.EndTime = DateTime.Now;
                result.ErpErrorDatas = new List<ErpErrorData>() { new ErpErrorData() { ErrMsg = "库存组织[外部Id={0}]不存在".L10nFormat(extentInvOrg), Infkey = extentInvOrg } };

                return result;
            }
            var firstInvOg = allOrgs.FirstOrDefault(a => a.ExternalId == extentInvOrg);
            //初始化环境
            this.InitEnvironment(firstInvOg.Code);

            //执行保存业务逻辑
            var results = func.Invoke(sourceDatas);

            resultsDict = results.Where(f=>!f.IsSuccess).ToDictionary(p => p.Infkey, p => p);
            DateTime endDateTime = DateTime.Now;

            sourceDatas.ForEach(p =>
            {
                ErpErrorData errorData = null;
                ProcessState processState = ProcessState.Processed;
                var key = string.Empty;
                var currentSuccess = 0;

                //处理错误信息
                if (resultsDict.TryGetValue(p.ErpDetailId, out errorData))
                {
                    //空KEY已过滤，resultsDict不会存在空key值，当且仅当p.Infkey为空，errorData为null                    
                    failCount++;
                    processState = ProcessState.Failed;
                }

                //记录日志
                key = processState == ProcessState.Processed ? p.OrderNumber : errorData.ErrMsg;
                currentSuccess = processState == ProcessState.Processed ? 1 : 0;
                SaveDownloadERPLog(type, 1, currentSuccess, 1 - currentSuccess,
                    JobDirection.ErpToBusiness, JobMode.Push, processState, beginDateTime, endDateTime, OperationType.API, errorData?.ErrMsg, key);
            });

            //构建返回结果
            result.ErpErrorDatas.AddRange(results);
            result.DataCount = dataCount;
            result.SuccessCount = dataCount - failCount;
            result.FailCount = failCount;
            result.BeginTime = beginDateTime;
            result.EndTime = endDateTime;

            return result;
        }
    }
}
