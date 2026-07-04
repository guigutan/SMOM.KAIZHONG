using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.Outsourcing;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    public class DownloadOAFlowReturnController : DomainController
    {

        public virtual ApiCommonRes GroupSaveOAFlowReturn(List<OAFlowReturnData> datas)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };
            List<OAFlowReturnData> list = new List<OAFlowReturnData>();
            var dataJson = JsonConvert.SerializeObject(datas);
            var logController = AppRuntime.Service.Resolve<InfDataLogController>();
            int failCount = 0;

            //记录日志
            var erpDataInfLog = logController.SaveErpDataInfLog(InfType.OAFlowReturn, dataJson, DateTime.Now, CallDirection.NcToMom, CallResult.UnSave, datas.Count);
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item.ZUID.IsNullOrEmpty())
                                throw new ValidationException("唯一码不能为空".L10N());
                            UpdateFlow(item);
                            list.Add(item);
                            apiResult.SuccessList.Add(item);
                        }
                        catch (Exception ex)
                        {
                            throw new ValidationException($"流程单号{item.FLOWNO}:" + ex.GetBaseException()?.Message);
                        }
                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                    logController.UpadateLogData<OAFlowReturnData>(erpDataInfLog, list, apiResult);

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
                logController.UpadateLogData<OAFlowReturnData>(erpDataInfLog, null, apiResult, ex.Message, 1);

            }
            return apiResult;
        }

        public virtual void UpdateFlow(OAFlowReturnData data)
        {
            //更新状态为退回
            DB.Update<OutboundConfirmDetail>().Set(p => p.State, OutboundConfirmDetailState.Return).Where(p => p.Zuid == data.ZUID).Execute();
        }
    }
}
