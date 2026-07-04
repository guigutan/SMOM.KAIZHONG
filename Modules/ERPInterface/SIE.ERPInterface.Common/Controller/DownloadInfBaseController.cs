using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
using System.Linq;
using System.Xml;

namespace SIE.ERPInterface.Common.Controller
{
    /// <summary>
    /// 下载到中间表
    /// </summary>
    public partial class DownloadInfBaseController : DownloadBaseController
    {
        /// <summary>
        /// 获取任务下载时间信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual DownloadJobTime GetDownloadJobTime(JobType type)
        {
            return Query<DownloadJobTime>().Where(p => p.JobType == type).FirstOrDefault();
        }

        /// <summary>
        /// API保存中间表数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sourceDatas">来源数据</param>
        /// <param name="func">执行业务逻辑</param>
        /// <param name="type">接口类型</param>
        /// <param name="invOrg">库存组织</param>
        /// <returns></returns>
        public virtual ApiResult ApiSaveInfData<T>(List<T> sourceDatas, Action<T> func, JobType type, int invOrg) where T : ErpInfoData
        {
            var result = new ApiResult();
            var erpErrorDatas = new List<ErpErrorData>();
            var beginDateTime = DateTime.Now;
            var dataCount = sourceDatas.Count;
            var failCount = 0;

            //初始化环境
            this.InitEnvironment(invOrg);

            if (sourceDatas != null && dataCount > 0)
            {
                T data = null;
                DateTime singleBeginDate = DateTime.Now;

                //保存中间表数据
                for (int i = 0; i < dataCount; i++)
                {
                    data = sourceDatas[i];
                    try
                    {
                        singleBeginDate = DateTime.Now;
                        //执行保存中间表数据

                        if (data == null)
                            throw new ValidationException("传入数据为null。".L10N());

                        func.Invoke(data);
                    }
                    catch (Exception ex)
                    {
                        failCount++;
                        var sourceDataSerialize = JsonConvert.SerializeObject(data);//传入数据序列化，用于记录LOG
                        var erpErrorData = new ErpErrorData();
                        erpErrorData.ErrMsg = ex.GetBaseException().Message;
                        erpErrorDatas.Add(erpErrorData);

                        //失败记录
                        SaveDownloadERPLog(type, 1, 0, 1, JobDirection.ErpToInf, JobMode.Push, ProcessState.Failed, singleBeginDate, DateTime.Now
                            , OperationType.API, erpErrorData.ErrMsg, null, data?.ErpKey, sourceDataSerialize);
                    }
                }

                var sourceDatasSerialize = JsonConvert.SerializeObject(sourceDatas);
                //成功记录
                SaveDownloadERPLog(type, dataCount, dataCount - failCount, failCount, JobDirection.ErpToInf, JobMode.Push, ProcessState.Processed
                    , beginDateTime, DateTime.Now, OperationType.API, null, null, null, sourceDatasSerialize);
            }

            //构建返回结果
            result.ErpErrorDatas.AddRange(erpErrorDatas);
            result.DataCount = dataCount;
            result.SuccessCount = dataCount - failCount;
            result.FailCount = failCount;
            result.BeginTime = beginDateTime;
            result.EndTime = DateTime.Now;

            return result;
        }

        /// <summary>
        /// 下载ERP数据到中间表
        /// </summary>
        public virtual ProcessResult SaveInfData<T, E>(IEnumerable<DataRow> datas, Func<DataRow, T> func,
           Func<string, EntityList<E>> funcDtl, JobType type, DownloadJobTime jobTime, DownloadJobTimeDetail jobTimeDetail, DateTime lastUpdateDate, bool isManual)
            where T : DownloadBaseEntity
            where E : DownloadBaseEntity
        {
            DateTime? dataLastUpdateDate = null;
            var result = new ProcessResult();

            if (datas != null && datas.Any())
            {
                var operationType = isManual ? OperationType.Manual : OperationType.Scheduling;
                int dataCount = datas.Count();
                int successCount = 0;
                T entity = null;
                DateTime totalBeginDate = DateTime.Now;
                DateTime singleBeginDate = DateTime.Now;

                //保存中间表数据
                datas.ForEach(p =>
                {
                    try
                    {
                        using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                        {
                            singleBeginDate = DateTime.Now;
                            entity = func.Invoke(p);
                            entity.IsManual = isManual;
                            RF.Save(entity);

                            ////ERP到中间表逻辑，只有手动下载才跑子表数据，调度下载，通过调度跑子表
                            ////if (isManual == true && entity.Foreignkey.IsNotEmpty() && funcDtl != null)
                            ////{
                            ////    var dtlList = funcDtl.Invoke(entity.Foreignkey);
                            ////    RF.Save(dtlList);
                            ////}
                            trans.Complete();

                            //计算任务下载时间最后更新时间逻辑（取ERP数据最大的一个最后更新时间）
                            if (dataLastUpdateDate == null)
                                dataLastUpdateDate = entity.LastUpdateDate.Value;
                            else if (DateTime.Compare(dataLastUpdateDate.Value, entity.LastUpdateDate.Value) < 0)
                                dataLastUpdateDate = entity.LastUpdateDate.Value;
                        }
                        successCount++;
                        result.AddSuccessMsg();
                    }
                    catch (Exception ex)
                    {
                        //失败记录
                        SaveDownloadERPLog(type, 1, 0, 1, JobDirection.ErpToInf, JobMode.Pull, ProcessState.Failed, singleBeginDate, DateTime.Now, operationType, ex.Message);
                        result.AddFailMsg(ex);
                    }
                });

                //成功记录
                SaveDownloadERPLog(type, dataCount, successCount, dataCount - successCount, JobDirection.ErpToInf, JobMode.Pull, ProcessState.Processed, totalBeginDate, DateTime.Now, operationType);
            }

            //中间表最后更新时间记录
            if (jobTime == null) jobTime = new DownloadJobTime() { JobType = type };

            jobTimeDetail.ErpBatchId = jobTimeDetail.Id;
            jobTimeDetail.State = RequestState.Success;
            jobTimeDetail.RequestDate = DateTime.Now;
            jobTimeDetail.ResponseDate = DateTime.Now;
            jobTimeDetail.IsManual = isManual;
            jobTime.LastDownloadDate = (!isManual && dataLastUpdateDate.HasValue) ? dataLastUpdateDate.Value.AddSeconds(1) : jobTime.LastDownloadDate;
            RF.Save(jobTime);
            jobTimeDetail.DownloadJobTimeId = jobTime.Id;
            RF.Save(jobTimeDetail);
            return result;
        }

        /// <summary>
        /// 下载ERP数据到中间表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soapResult"></param>
        /// <param name="func"></param>
        /// <param name="type"></param>
        /// <param name="jobTime"></param>
        /// <param name="jobTimeDetail"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="isManual"></param>
        /// <returns></returns>
        public virtual ProcessResult SaveInfData<T>(SoapResult soapResult, Func<XmlNode, T> func,
           JobType type, DownloadJobTime jobTime, DownloadJobTimeDetail jobTimeDetail, DateTime lastUpdateDate, bool isManual) where T : DownloadBaseEntity
        {
            DateTime? dataLastUpdateDate = null;
            var result = new ProcessResult();
            XmlNodeList xmlLineList = null;

            var xdoc = new XmlDocument();
            xdoc.LoadXml(soapResult.X_RESPONSE_DATA);
            xmlLineList = xdoc.GetElementsByTagName("LINE");


            if (xmlLineList != null && xmlLineList.Count > 0)
            {
                var operationType = isManual ? OperationType.Manual : OperationType.Scheduling;
                int dataCount = xmlLineList.Count;
                int successCount = 0;
                T entity = null;
                DateTime totalBeginDate = DateTime.Now;
                DateTime singleBeginDate = DateTime.Now;

                //保存中间表数据
                for (int i = 0; i < xmlLineList.Count; i++)
                {
                    try
                    {
                        using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                        {
                            singleBeginDate = DateTime.Now;
                            entity = func.Invoke(xmlLineList[i]);
                            entity.IsManual = isManual;
                            RF.Save(entity);
                            trans.Complete();

                            //计算任务下载时间最后更新时间逻辑（取ERP数据最大的一个最后更新时间）
                            if (dataLastUpdateDate == null)
                                dataLastUpdateDate = entity.LastUpdateDate.Value;
                            else if (DateTime.Compare(dataLastUpdateDate.Value, entity.LastUpdateDate.Value) < 0)
                                dataLastUpdateDate = entity.LastUpdateDate.Value;
                        }
                        successCount++;
                        result.AddSuccessMsg();
                    }
                    catch (Exception ex)
                    {
                        //失败记录
                        SaveDownloadERPLog(type, 1, 0, 1, JobDirection.ErpToInf, JobMode.Pull, ProcessState.Failed, singleBeginDate, DateTime.Now, operationType, ex.Message, entity?.Id, entity?.ErpKey);
                        result.AddFailMsg(ex);
                    }
                }

                //成功记录
                SaveDownloadERPLog(type, dataCount, successCount, dataCount - successCount, JobDirection.ErpToInf, JobMode.Pull, ProcessState.Processed, totalBeginDate, DateTime.Now, operationType);
            }

            //中间表最后更新时间记录
            if (jobTime == null) jobTime = new DownloadJobTime() { JobType = type };

            jobTimeDetail.ErpBatchId = jobTimeDetail.Id;
            jobTimeDetail.State = soapResult.X_RESPONSE_STATUS == "S" ? RequestState.Success : RequestState.Failed;
            jobTimeDetail.ResponseCode = soapResult.X_RESPONSE_CODE;
            jobTimeDetail.ResponseMessage = soapResult.X_RESPONSE_MESSAGE;
            jobTimeDetail.RequestStr = soapResult.RequestStr;
            jobTimeDetail.ResponseStr = soapResult.ResponseStr;
            jobTimeDetail.RequestDate = soapResult.RequestDate;
            jobTimeDetail.ResponseDate = soapResult.ResponseDate;
            jobTimeDetail.IsManual = isManual;
            jobTime.LastDownloadDate = jobTimeDetail.State == RequestState.Success && !isManual && dataLastUpdateDate.HasValue ? dataLastUpdateDate.Value.AddSeconds(1) : jobTime.LastDownloadDate;
            RF.Save(jobTime);
            jobTimeDetail.DownloadJobTimeId = jobTime.Id;
            RF.Save(jobTimeDetail);

            if (isManual && soapResult.X_RESPONSE_STATUS != "S")
                throw new ValidationException(soapResult.X_RESPONSE_MESSAGE);

            return result;
        }

        /// <summary>
        /// 下载ERP数据到中间表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="E"></typeparam>
        /// <param name="sapResult"></param>
        /// <param name="func"></param>
        /// <param name="type"></param>
        /// <param name="jobTime"></param>
        /// <param name="jobTimeDetail"></param>
        /// <param name="isManual"></param>
        /// <returns></returns>
        //public virtual ProcessResult SaveInfData<T, E>(SapResult sapResult, Func<E, T> func,
        //   JobType type, DownloadJobTime jobTime, DownloadJobTimeDetail jobTimeDetail, bool isManual) where T : DownloadBaseEntity
        //{
        //    DateTime? dataLastUpdateDate = null;
        //    var result = new ProcessResult();
        //    List<E> resultCollection = sapResult.SapResultData as List<E>;

        //    if (resultCollection != null && resultCollection.Count > 0)
        //    {
        //        var operationType = isManual ? OperationType.Manual : OperationType.Scheduling;
        //        int dataCount = resultCollection.Count;
        //        int successCount = 0;
        //        T entity = null;
        //        DateTime totalBeginDate = DateTime.Now;
        //        DateTime singleBeginDate = DateTime.Now;

        //        //保存中间表数据
        //        foreach (E searchResult in resultCollection)
        //        {
        //            try
        //            {
        //                using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
        //                {
        //                    singleBeginDate = DateTime.Now;
        //                    entity = func.Invoke(searchResult);
        //                    entity.IsManual = isManual;
        //                    RF.Save(entity);
        //                    trans.Complete();

        //                    //计算任务下载时间最后更新时间逻辑（取ERP数据最大的一个最后更新时间）
        //                    if (dataLastUpdateDate == null)
        //                        dataLastUpdateDate = entity.LastUpdateDate.Value;
        //                    else if (DateTime.Compare(dataLastUpdateDate.Value, entity.LastUpdateDate.Value) < 0)
        //                        dataLastUpdateDate = entity.LastUpdateDate.Value;
        //                }
        //                successCount++;
        //                result.AddSuccessMsg();
        //            }
        //            catch (Exception ex)
        //            {
        //                //失败记录
        //                SaveDownloadERPLog(type, 1, 0, 1, JobDirection.ErpToInf, JobMode.Pull, ProcessState.Failed, singleBeginDate, DateTime.Now, operationType, ex.Message, entity?.Id, entity?.ErpKey);
        //                result.AddFailMsg(ex);
        //            }
        //        }

        //        //成功记录
        //        SaveDownloadERPLog(type, dataCount, successCount, dataCount - successCount, JobDirection.ErpToInf, JobMode.Pull, ProcessState.Processed, totalBeginDate, DateTime.Now, operationType);
        //    }

        //    //中间表最后更新时间记录
        //    if (jobTime == null) jobTime = new DownloadJobTime() { JobType = type };

        //    jobTimeDetail.ErpBatchId = jobTimeDetail.Id;
        //    jobTimeDetail.State = sapResult.IsSuccess ? RequestState.Success : RequestState.Failed;
        //    jobTimeDetail.ResponseCode = string.Empty;
        //    jobTimeDetail.ResponseMessage = sapResult.;
        //    jobTimeDetail.RequestStr = sapResult.RequestStr;
        //    jobTimeDetail.ResponseStr = sapResult.ResponseStr;
        //    jobTimeDetail.RequestDate = sapResult.RequestDate;
        //    jobTimeDetail.ResponseDate = sapResult.ResponseDate;
        //    jobTimeDetail.IsManual = isManual;
        //    jobTime.LastDownloadDate = jobTimeDetail.State == RequestState.Success && !isManual && dataLastUpdateDate.HasValue ? dataLastUpdateDate.Value.AddSeconds(1) : jobTime.LastDownloadDate;
        //    RF.Save(jobTime);
        //    jobTimeDetail.DownloadJobTimeId = jobTime.Id;

        //    RF.Save(jobTimeDetail);

        //    if (isManual && !sapResult.IsSuccess)
        //        throw new ValidationException(sapResult.Message);

        //    return result;
        //}

        /// <summary>
        /// 下载LDAP数据到中间表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultCollection"></param>
        /// <param name="func"></param>
        /// <param name="type"></param>
        /// <param name="jobTime"></param>
        /// <param name="jobTimeDetail"></param>
        /// <param name="lastUpdateDate"></param>
        /// <param name="isManual"></param>
        /// <returns></returns>
        public virtual ProcessResult SaveInfData<T>(SearchResultCollection resultCollection, Func<SearchResult, T> func,
            JobType type, DownloadJobTime jobTime, DownloadJobTimeDetail jobTimeDetail, DateTime lastUpdateDate, bool isManual) where T : DownloadBaseEntity
        {
            var result = new ProcessResult();

            int dataCount = resultCollection == null ? 0 : resultCollection.Count;
            if (dataCount > 0)
            {
                var operationType = isManual ? OperationType.Manual : OperationType.Scheduling;
                int successCount = 0;
                T entity = null;
                DateTime totalBeginDate = DateTime.Now;
                DateTime singleBeginDate = DateTime.Now;

                //保存中间表数据
                foreach (SearchResult searchResult in resultCollection)
                {
                    try
                    {
                        using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                        {
                            singleBeginDate = DateTime.Now;
                            entity = func.Invoke(searchResult);
                            RF.Save(entity);
                            trans.Complete();
                        }
                        successCount++;
                        result.AddSuccessMsg();
                    }
                    catch (Exception ex)
                    {
                        //失败记录
                        SaveDownloadERPLog(type, 1, 0, 1, JobDirection.ErpToInf, JobMode.Pull, ProcessState.Failed, singleBeginDate, DateTime.Now, operationType, ex.Message, entity?.Id, entity?.ErpKey);
                        result.AddFailMsg(ex);
                    }
                }

                //成功记录
                SaveDownloadERPLog(type, dataCount, successCount, dataCount - successCount, JobDirection.ErpToInf, JobMode.Pull, ProcessState.Processed, totalBeginDate, DateTime.Now, operationType);
            }

            //中间表最后更新时间记录
            if (!isManual)
            {
                if (jobTime == null) jobTime = new DownloadJobTime() { JobType = type };

                jobTimeDetail.ErpBatchId = jobTimeDetail.Id;
                jobTimeDetail.State = RequestState.Success;
                jobTime.LastDownloadDate = lastUpdateDate;
                RF.Save(jobTime);
                jobTimeDetail.DownloadJobTimeId = jobTime.Id;
                RF.Save(jobTimeDetail);
            }

            return result;
        }
    }
}
