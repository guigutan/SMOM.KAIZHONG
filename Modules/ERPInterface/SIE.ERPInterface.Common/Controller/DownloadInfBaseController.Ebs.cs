using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using System;
using System.Linq;

namespace SIE.ERPInterface.Common.Controller
{
    public partial class DownloadInfBaseController
    {
        /// <summary>
        /// 下载ERP数据到业务表（集体成功）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soapResult"></param>
        /// <param name="func"></param>
        /// <param name="type"></param>
        /// <param name="jobTime"></param>
        /// <param name="jobTimeDetail"></param>
        /// <param name="beginTime"></param>        
        /// <param name="isManual"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public virtual ProcessResult DownloadBusData<T, E>(EbsResult<T> soapResult, Func<T, E> func,
           JobType type, DownloadJobTime jobTime, DownloadJobTimeDetail jobTimeDetail, DateTime beginTime, bool isManual, bool isUpdate = false) where E : Entity where T : EbsDataBase
        {
            DateTime? dataLastUpdateDate = null;
            var result = new ProcessResult();
            EntityList<E> entitylist = new EntityList<E>();


            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var operationType = isManual ? OperationType.Manual : OperationType.Scheduling;

            E entity = null;

            foreach (var invOrgdata in soapResult.XV_RESULT)
            {
                entity = func.Invoke(invOrgdata);
                if (entity != null)
                {
                    entitylist.Add(entity);
                    result.SuccessMsg.Add("Success");
                }
                else
                {
                    if (invOrgdata.IsRepeat)
                    {
                        result.FailMsg.Add("编码或名称重复".L10N());
                    }
                    else result.FailMsg.Add("Fail");
                }
                //计算任务下载时间最后更新时间逻辑（取ERP数据最大的一个最后更新时间）
                if (dataLastUpdateDate == null)
                    dataLastUpdateDate = invOrgdata.LAST_UPDATE_DATE.Value;
                else if (DateTime.Compare(dataLastUpdateDate.Value, invOrgdata.LAST_UPDATE_DATE.Value) < 0)
                    dataLastUpdateDate = invOrgdata.LAST_UPDATE_DATE.Value;

            }
            try
            {   //这里考虑要么都成功要么都不成功，是基于最后更新时间考虑，
                //如果分开保存的话，如果最后一个是成功的，但是前面的不成功，最后更新时间就是最后的那个的时间，导致前面的数据一直不能下载
                if (entitylist.Any())
                {
                    RF.Save(entitylist);
                    ctl.SaveDownloadERPLog(type, soapResult.XV_RESULT.Count, result.SuccessCount, soapResult.XV_RESULT.Count - result.SuccessCount, JobDirection.ErpToBusiness, JobMode.Pull, ProcessState.Processed,
                        beginTime, DateTime.Now, operationType, isUpdate ? "数据更新" : "数据新增");
                    jobTimeDetail.State = RequestState.Success;
                }
                else
                {
                    jobTimeDetail.ResponseMessage = soapResult.XV_RET_MESSAGE;
                    jobTimeDetail.State = RequestState.Failed;
                }
            }
            catch (Exception ex)
            {
                //这里要么都成功下载，要么都不成功下载
                //ctl.SaveDownloadERPLog(type, soapResult.XV_RESULT.Count, 0, soapResult.XV_RESULT.Count, JobDirection.ErpToBusiness, JobMode.Pull, ProcessState.Failed,
                //    beginTime, DateTime.Now, operationType, ex.Message);
                result.AddFailMsg(ex);
                jobTimeDetail.ResponseMessage = "返回数据成功，写入数据失败[{0}]".FormatArgs(ex.Message);
                jobTimeDetail.State = RequestState.Failed;
            }
            if (jobTime == null) jobTime = new DownloadJobTime() { JobType = type };

            jobTimeDetail.ErpBatchId = jobTimeDetail.Id;
            jobTimeDetail.RequestStr = soapResult.RequestStr;
            jobTimeDetail.ResponseStr = soapResult.ResponseStr;
            jobTimeDetail.RequestDate = soapResult.RequestDate;
            jobTimeDetail.ResponseDate = soapResult.ResponseDate;
            jobTimeDetail.RequestStr = soapResult.RequestStr;
            jobTimeDetail.IsManual = isManual;
            jobTime.LastDownloadDate = jobTimeDetail.State == RequestState.Success && !isManual && dataLastUpdateDate.HasValue ? dataLastUpdateDate.Value.AddSeconds(1) : jobTime.LastDownloadDate;           
            RF.Save(jobTime);
            jobTimeDetail.DownloadJobTimeId = jobTime.Id;
            RF.Save(jobTimeDetail);

            if (isManual && soapResult.XV_RET_STATUS != "S")
                throw new ValidationException(soapResult.XV_RET_MESSAGE);

            return result;
        }

        /// <summary>
        /// 下载ERP数据到业务表(单独成功)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soapResult"></param>
        /// <param name="func"></param>
        /// <param name="type"></param>
        /// <param name="jobTime"></param>
        /// <param name="jobTimeDetail"></param>
        /// <param name="beginTime"></param>        
        /// <param name="isManual"></param>
        /// <param name="isUpdate"></param>
        /// <returns></returns>
        public virtual ProcessResult DownloadBusDataLonely<T, E>(EbsResult<T> soapResult, Func<T, E> func,
           JobType type, DownloadJobTime jobTime, DownloadJobTimeDetail jobTimeDetail, DateTime beginTime, bool isManual, bool isUpdate = false) where E : Entity where T : EbsDataBase
        {
            DateTime? dataLastUpdateDate = null;
            var result = new ProcessResult();


            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var operationType = isManual ? OperationType.Manual : OperationType.Scheduling;

            E entity = null;

            foreach (var invOrgdata in soapResult.XV_RESULT)
            {
                entity = func.Invoke(invOrgdata);
                if (entity != null)
                {
                    try
                    {
                        RF.Save(entity);
                        result.SuccessMsg.Add("Success");
                    }
                    catch (Exception ex)
                    {
                        result.AddFailMsg(ex);
                    }
                }
                //计算任务下载时间最后更新时间逻辑（取ERP数据最大的一个最后更新时间）
                if (dataLastUpdateDate == null)
                    dataLastUpdateDate = invOrgdata.LAST_UPDATE_DATE.Value;
                else if (DateTime.Compare(dataLastUpdateDate.Value, invOrgdata.LAST_UPDATE_DATE.Value) < 0)
                    dataLastUpdateDate = invOrgdata.LAST_UPDATE_DATE.Value;

            }

            if (result.SuccessCount > 0)
            {

                ctl.SaveDownloadERPLog(type, soapResult.XV_RESULT.Count, result.SuccessCount, soapResult.XV_RESULT.Count - result.SuccessCount, JobDirection.ErpToBusiness, JobMode.Pull, ProcessState.Processed,
                    beginTime, DateTime.Now, operationType, isUpdate ? "数据更新" : "数据新增");
                jobTimeDetail.State = RequestState.Success;
            }
            else
            {
                ctl.SaveDownloadERPLog(type, soapResult.XV_RESULT.Count, result.SuccessCount, soapResult.XV_RESULT.Count - result.SuccessCount, JobDirection.ErpToBusiness, JobMode.Pull, ProcessState.Failed,
                      beginTime, DateTime.Now, operationType, isUpdate ? "数据更新" : "数据新增");
                jobTimeDetail.ResponseMessage = "返回数据成功，写入数据失败[{0}]".FormatArgs(result.FailMsg.FirstOrDefault());
                jobTimeDetail.State = RequestState.Failed;
            }


            if (jobTime == null) jobTime = new DownloadJobTime() { JobType = type };

            jobTimeDetail.ErpBatchId = jobTimeDetail.Id;
            jobTimeDetail.RequestStr = soapResult.RequestStr;
            jobTimeDetail.ResponseStr = soapResult.ResponseStr;
            jobTimeDetail.RequestDate = soapResult.RequestDate;
            jobTimeDetail.ResponseDate = soapResult.ResponseDate;
            jobTimeDetail.IsManual = isManual;
            jobTime.LastDownloadDate = jobTimeDetail.State == RequestState.Success && !isManual && dataLastUpdateDate.HasValue ? dataLastUpdateDate.Value.AddSeconds(1) : jobTime.LastDownloadDate;
            RF.Save(jobTime);
            jobTimeDetail.DownloadJobTimeId = jobTime.Id;
            RF.Save(jobTimeDetail);

            if (isManual && soapResult.XV_RET_STATUS != "S")
                throw new ValidationException(soapResult.XV_RET_MESSAGE);

            return result;
        }
    }
}
