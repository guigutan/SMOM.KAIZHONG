using Newtonsoft.Json;
using SIE.Domain;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Logs;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.MES.TaskManagement.Reports;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    /// <summary>
    /// 更新报工结果控制器
    /// </summary>
    public class UpdateReportResultController : DomainController
    {

        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveReportResult(List<ReportReturnData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<ReportReturnData> list = new List<ReportReturnData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.ReportResult, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    foreach (var item in datas)
                    {
                        HandleReportResult(item);

                        list.Add(item);
                        apiResult.SuccessList.Add(item);
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<ReportReturnData>(erpDataInfLog, list, apiResult);

                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能为空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorObjList.Clear();
                apiResult.ErrorObjList.AddRange(datas);
                apiResult.ErrorList.Add(ex.Message);
                logController.UpadateLogData<ReportReturnData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }



        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="erpDataInfLog"></param>
        /// <returns></returns>
        public virtual ApiCommonRes SaveReportResult(List<ReportReturnData> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<ReportReturnData> list = new List<ReportReturnData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;

            try
            {
                if (datas != null || datas.Count > 0)
                {
                    foreach (var item in datas)
                    {
                        if (item != null)
                        {
                            HandleReportResult(item);
                            list.Add(item);
                        }
                    }
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能未空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorList.Add(ex.Message);
            }
            finally
            {
                erpDataInfLog = RT.Service.Resolve<InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<ReportReturnData>(erpDataInfLog, datas.Count, list, apiResult, false);

            }
            return apiResult;
        }


        /// <summary>
        /// 处理结果
        /// </summary>
        /// <param name="reportReturnData"></param>
        private void HandleReportResult(ReportReturnData reportReturnData)
        {
            var uploadTrans = Query<UploadTransaction>().Where(p => p.Zuid == reportReturnData.ZUID).ToList();
            //可能不是当前库存组织下的数据
            if (uploadTrans.Count == 0)
                return;
            var ids = uploadTrans.Select(p => p.Id).ToList();
            var recordIds = uploadTrans.Select(p => p.ReportRecordId ?? 0).ToList();
            using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
            {
                var state = reportReturnData.ZFKBS == "S" ? Common.Enums.ProcessState.Processed : Common.Enums.ProcessState.Failed;
                if (state == Common.Enums.ProcessState.Failed)
                {
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Failed)
                        .Set(p => p.ValidateMessage, reportReturnData.ZFKXX)
                        .Set(p => p.ProcessMessage, reportReturnData.ZFKXX)
                        .Set(p => p.UploadCount, p => p.UploadCount + 1)
                        .Where(p => ids.Contains(p.Id) && p.UploadCount >= 5)
                        .Execute();
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Retry)
                        .Set(p => p.ValidateMessage, reportReturnData.ZFKXX)
                        .Set(p => p.ProcessMessage, reportReturnData.ZFKXX)
                        .Set(p => p.UploadCount, p => p.UploadCount + 1)
                        .Where(p => ids.Contains(p.Id) && p.UploadCount < 5)
                        .Execute();
                }
                else
                {
                    DB.Update<UploadTransaction>()
                        .Set(p => p.State, Common.Enums.ProcessState.Processed)
                        .Set(p => p.ValidateMessage, reportReturnData.ZFKXX)
                        .Set(p => p.ProcessMessage, reportReturnData.ZFKXX)
                        .Set(p => p.UploadCount, p => p.UploadCount + 1)
                        .Where(p => ids.Contains(p.Id))
                        .Execute();
                }

                DB.Update<ReportRecord>()/*.Set(p => p.UploadFlag, false)*/.Set(p => p.UploadResult, reportReturnData.ZFKXX).Where(p => recordIds.Contains(p.Id)).Execute();
                trans.Complete();
            }
        }
    }
}
