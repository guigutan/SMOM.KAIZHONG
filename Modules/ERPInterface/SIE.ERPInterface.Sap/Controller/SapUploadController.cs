using DocumentFormat.OpenXml.ExtendedProperties;
using Newtonsoft.Json;
using SIE.Common;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Connection;
using SIE.ERPInterface.Sap.Datas;
using SIE.Inventory.Transactions;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.WMS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SIE.ERPInterface.Sap.Controller
{
    /// <summary>
    /// SAP上传控制器
    /// </summary>
    public class SapUploadController : UploadBaseController
    {
        private EntityList<ErpUploadLog> _erpUploadLogs { get; set; } = new EntityList<ErpUploadLog>();

        private static DateTime _dtNow = DateTime.Now;

        /// <summary>
        /// 保存事务上传记录
        /// </summary>
        public virtual void SaveTransactionData(SapResult sapResult, List<UploadTransaction> uploadTransactions, ProcessResult result, string dataKey)
        {
            if (!sapResult.IsSuccess)
            {
                TimeOutHandle(sapResult, uploadTransactions, result, dataKey);
                return;
            }
            var sapResultData = sapResult.SapResultData as SapResultData;
            if (sapResultData == null)
                return;

            var dtlDatas = sapResultData.RETURN;

            if (dtlDatas == null || dtlDatas.Count <= 0) return;//没有明细数据，退出
            _erpUploadLogs = uploadTransactions.Select(a => a.Id).SplitContains(ids =>
            {
                return Query<ErpUploadLog>().Where(p => ids.Contains(p.UploadTransactionId)).ToList();
            });


            //保存事务上传及事务上传记录表数据
            for (int i = 0; i < dtlDatas.Count; i++)
            {
                try
                {
                    using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                    {
                        var dataNode = dtlDatas[i];
                        var state = dataNode.MSGTY == "S" ? ProcessState.Processed : ProcessState.Failed;
                        var processMessage = dataNode.MSGTX;
                        //var validateMessage = "凭证日期：" + dataNode.DOCUMENT_DATE + "物料凭证号：" + dataNode.MATERUAL_VOUCHER_CODE + "过账日期：" + dataNode.POSTING_DATE;   
                        var validateMessage = "SAP凭证号-凭证行号[{0}]||单据Key-明细Key[{2}]、请求Key[{1}]".L10nFormat(dataNode.SAPKEY, dataKey, dataNode.EXDOCKEY);
                        //SAP返回的KEY组成是我们传过去的 Ordkey+"-"+明细的Key
                        if (dataNode.EXDOCKEY.IsNullOrEmpty())
                            throw new ValidationException("ERP回传的EXDOCKEY是空的与SMOM传出的不匹配".L10N());
                        var smomkey = dataNode.EXDOCKEY.Trim().Split('-');
                        var billdata = new List<UploadTransaction>();

                        if (smomkey.Length > 1)
                        {//单个处理
                            var dtlKey = smomkey[1];
                            if (dtlKey.Contains("◇"))
                            {//库内加工、库存属性变更WMS会产生两笔事务，但是SAP那边只有一笔事务，需要对Key进行组合上传，回来的时候拆分更新
                                List<double> tranIds = new List<double>();
                                dtlKey.Split('◇').ForEach(f =>
                                {
                                    double.TryParse(dtlKey, out double uploadTranId);
                                    tranIds.Add(uploadTranId);
                                    if (state == ProcessState.Processed)
                                    {
                                        result.SuccessSapKey.Add(uploadTranId, dataNode.SAPKEY);
                                    }

                                });
                                billdata = uploadTransactions.Where(f => tranIds.Contains(f.Id)).ToList();
                            }
                            else
                            {
                                double.TryParse(dtlKey, out double uploadTranId);
                                billdata = uploadTransactions.Where(f => f.Id == uploadTranId).ToList();


                                if (state == ProcessState.Processed)
                                {
                                    result.SuccessSapKey.Add(uploadTranId, dataNode.SAPKEY);
                                }
                            }
                        }
                        else
                        {  //单头有错误                           
                            billdata = uploadTransactions.Where(f => f.OrdKey == smomkey[0]).ToList();
                        }
                        if (!billdata.Any())
                        {
                            billdata = uploadTransactions.Where(f => f.OrdKey == smomkey[0]).ToList();
                        }

                        if (!billdata.Any())
                            throw new ValidationException("ERP回传的EXDOCKEY与SMOM传出的不匹配".L10N());
                        if (state == ProcessState.Processed && smomkey.Length == 1)
                            continue;//成功的时候不用处理单头的报文
                        UpdateTranData(sapResult, billdata, state, validateMessage, processMessage, result);

                        trans.Complete();
                    }
                }
                catch (Exception ex)
                {
                    //失败记录
                    result.AddFailMsg(ex);
                }
            }
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchSave(_erpUploadLogs);           
        }

        /// <summary>
        /// 更新上传记录信息及日志信息
        /// </summary>      
        private void UpdateTranData(SapResult sapResult, List<UploadTransaction> uploadTransactions, ProcessState state, string validateMessage, string processMessage, ProcessResult result)
        {
            uploadTransactions.Select(f => f.Id).SplitDataExecute(ids =>
            {
                DB.Update<UploadTransaction>()
                .Set(p => p.State, state)
                .Set(p => p.UploadCount, p => p.UploadCount + 1)
                .Set(p => p.ValidateMessage, validateMessage)
                .Set(p => p.ProcessMessage, processMessage)
                .Set(p => p.DataKey, "")
                .Where(p => ids.Contains(p.Id))
                .Execute();
            });
            SetUploadLogs(sapResult, uploadTransactions, state, validateMessage, processMessage, result);
        }

        /// <summary>
        /// 设置日志数据
        /// </summary>        
        private void SetUploadLogs(SapResult sapResult, List<UploadTransaction> uploadTransactions, ProcessState state, string validateMessage, string processMessage, ProcessResult result)
        {
            uploadTransactions.ForEach(f =>
            {
                SetLogData(f, sapResult, state, validateMessage, processMessage);
                if (state == ProcessState.Processed)
                    result.AddSuccessMsg();
                else
                {
                    if (processMessage.IsNotEmpty())
                        processMessage = processMessage + "||";
                    result.AddFailMsg(processMessage + validateMessage);
                }
            });
            //return erpUploadLogs;
        }


        /// <summary>
        /// 更新上传记录状态信息（异步处理只更新状态）
        /// </summary>      
        public virtual void UpdateTranStateData(SapResult sapResult, List<UploadTransaction> uploadTransactions, ProcessState state, string validateMessage, string processMessage)
        {
            uploadTransactions.Select(f => f.Id).SplitDataExecute(ids =>
            {
                DB.Update<UploadTransaction>()
                .Set(p => p.State, state)
                .Set(p => p.ValidateMessage, validateMessage)
                .Set(p => p.ProcessMessage, processMessage)
                .Set(p => p.UploadCount, p => p.UploadCount + 1)
                .Where(p => ids.Contains(p.Id))
                .Execute();
            });
        }

        /// <summary>
        /// 设置日志数据
        /// </summary>      
        private void SetLogData(UploadTransaction uploadTrans, SapResult sapResult, ProcessState state, string validateMessage, string processMessage)
        {
            ErpUploadLog erpUploadLog = _erpUploadLogs.FirstOrDefault(a => a.UploadTransactionId == uploadTrans.Id);
            if (erpUploadLog == null)
            {
                erpUploadLog = new ErpUploadLog()
                {
                    InterfaceName = sapResult.InterfaceName,
                    IsSuccess = state == ProcessState.Processed,
                    TransactionId = uploadTrans.InvTransactionId,
                    OrderNo = uploadTrans.BillNo,
                    OrderType = uploadTrans.OrderType,
                    LineNo = uploadTrans.BillLineNo,
                    RequestStr = sapResult.RequestStr,
                    ResponseStr = sapResult.ResponseStr,
                    SapKeyMsg = validateMessage,
                    State = state,
                    UploadTransactionId = uploadTrans.Id,
                    ResponseMessage = state == ProcessState.Processed ? "" : processMessage,
                    ReloadCount = uploadTrans.UploadCount ?? 0 + 1,
                };
                _erpUploadLogs.Add(erpUploadLog);
            }
            else
            {
                erpUploadLog.RequestStr = sapResult.RequestStr;
                erpUploadLog.ResponseStr = sapResult.ResponseStr;
                erpUploadLog.SapKeyMsg = validateMessage;
                erpUploadLog.State = state;
                erpUploadLog.ReloadCount = uploadTrans.UploadCount ?? 0 + 1;
                erpUploadLog.ResponseMessage = state == ProcessState.Processed ? "" : processMessage;
                erpUploadLog.UpdateDate = _dtNow;
                erpUploadLog.IsSuccess = state == ProcessState.Processed;
            }

            //return erpUploadLog;
        }

        /// <summary>
        /// 超时请求处理
        /// </summary>        
        private void TimeOutHandle(SapResult sapResult, List<UploadTransaction> uploadTransactions, ProcessResult result, string dataKey)
        {

            var validateMessage = "请求超时，ERP正在处理。请求Key：" + dataKey;
            uploadTransactions.Select(f => f.Id).SplitDataExecute(ids =>
            {
                DB.Update<UploadTransaction>()
                .Set(p => p.State, ProcessState.Retry)
                .Set(p => p.UploadCount, p => p.UploadCount + 1)
                .Set(p => p.ValidateMessage, validateMessage)
                .Set(p => p.ProcessMessage, validateMessage)
                .Set(p => p.DataKey, dataKey)
                .Where(p => ids.Contains(p.Id))
                .Execute();
            });

            SetUploadLogs(sapResult, uploadTransactions, ProcessState.Retry, validateMessage, "", result);
            _erpUploadLogs.Where(f => f.PersistenceStatus == PersistenceStatus.Modified).Select(a => a.Id).SplitDataExecute(ids =>
            {
                DB.Update<ErpUploadLog>().Set(p => p.RequestStr, sapResult.RequestStr)
                .Set(p => p.ResponseStr, sapResult.ResponseStr)
                .Set(p => p.SapKeyMsg, validateMessage)
                .Set(p => p.State, ProcessState.Retry)
                .Set(p => p.UpdateDate, _dtNow)
                .Where(p => ids.Contains(p.Id))
                .Execute();
            });
            _erpUploadLogs.Where(f => f.PersistenceStatus == PersistenceStatus.Modified).ForEach(a => a.MarkSaved());
            RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchSave(_erpUploadLogs);
        }

        /// <summary>
        /// 设置基类的值
        /// </summary>
        /// <param name="uploadTransaction">主事务Id</param>
        /// <param name="sapItemParamBase"></param>
        /// <param name="relationTranId">关联的事务Id</param>
        public virtual void SetSapItemParamBase(UploadTransaction uploadTransaction, SapItemParamBase sapItemParamBase, double? relationTranId = null)
        {
            sapItemParamBase.EXTDOC_ITEMNO = uploadTransaction.Id.ToString();
            if (relationTranId.HasValue && relationTranId > 0)
            {
                sapItemParamBase.EXTDOC_ITEMNO += "◇" + relationTranId;
            }
            sapItemParamBase.MATNR = uploadTransaction.ItemCode;
        }

        /// <summary>
        /// 转换时间格式数据
        /// </summary>
        /// <returns></returns>
        public virtual int ConvertDate(DateTime dateTime)
        {
            return int.Parse(dateTime.ToString("yyyyMMdd"));
        }

        public virtual ProcessResult UploadInterface<T>(List<Tuple<OrderType, TransactionType, double?, string>> tuples, IUploadDataHandler<T> uploadDataHandler, string interfaceName, InfType infType, CallDirection callDirection, string zifcd = "", Expression<Func<UploadTransaction, bool>> exp = null)
        {
            Dictionary<string, string> cusHeadParams = new Dictionary<string, string>();
            if (zifcd.IsNotEmpty())
                cusHeadParams.Add("zifcd", zifcd);

            var ctl = RT.Service.Resolve<SapUploadController>();
            var sapParameters = new List<SapParameter>();
            var uploadTransactions = ctl.GetUploadTransactions(tuples, exp);//查询未处理事务上传表数据
            var beginDate = DateTime.Now;  //记录接口开始时间
            ProcessResult processResult = new ProcessResult();
            //分组（如果只有一组，就直接创建个Dictionary丢一个进去就行了）
             var dic = uploadDataHandler.Grouped(uploadTransactions);
            foreach (var d in dic)
            {
                if (d.Value.Count < 1)
                    continue;
                //设置参数
                var json = uploadDataHandler.SetParam(d.Value.AsEntityList());

                var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(infType, json, DateTime.Now, callDirection, CallResult.UnSave, d.Value.Count);
                try
                {
                    SapResult sapResult = RT.Service.Resolve<HttpHelper>().InvokeSapAPI(interfaceName, json, cusHeadParams);
                    erpDataInfLog.ResponseContent = sapResult.ResponseStr;

                    if (sapResult.IsSuccess == false)
                        throw new ValidationException("接口调用失败:{0}".L10nFormat(sapResult.ResponseStr));
                    sapResult.InterfaceName = interfaceName;
                    //上传后处理
                    processResult.CombineResult(uploadDataHandler.Uploaded(sapResult, d.Value.AsEntityList(), ""));

                    if (sapResult.IsSuccess == true)
                        erpDataInfLog.CallResult = CallResult.Success;
                    else
                        erpDataInfLog.CallResult = CallResult.Fail;
                }
                catch (Exception exx)
                {
                    erpDataInfLog.ErrorMsg = exx.InnerException != null ? exx.InnerException.Message : exx.Message;
                    erpDataInfLog.CallResult = CallResult.Fail;
                }
                finally
                {
                    erpDataInfLog.EndDate = DateTime.Now;
                    erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(erpDataInfLog);
                }
            }
            return processResult;
        }
        /// <summary>
        /// 从SMOM中间表上传销售出库事务到ERP
        /// </summary>
        /// <param name="tuples"></param>
        /// <param name="interfaceName">接口名称</param>
        /// <param name="zifcd">SAP接口参数，每个接口不一样</param>
        /// <param name="uploadDataHandler">继承IUploadDataHandler</param>
        /// <returns></returns>
        public virtual ProcessResult UploadDataToErp<T>(List<Tuple<OrderType, TransactionType, double?, string>> tuples, IUploadDataHandler<T> uploadDataHandler, string interfaceName, string zifcd = "")
        {
            Dictionary<string, string> cusHeadParams = new Dictionary<string, string>();
            if (zifcd.IsNotEmpty())
                cusHeadParams.Add("zifcd", zifcd);

            var ctl = RT.Service.Resolve<SapUploadController>();
            var sapParameters = new List<SapParameter>();
            var uploadTransactions = ctl.GetUploadTransactions(tuples);//查询未处理事务上传表数据

            var beginDate = DateTime.Now;  //记录接口开始时间           
            ProcessResult result = new ProcessResult();
            //分两次上传，第一次上传超时的，
            foreach (var g in uploadTransactions.GroupBy(p => p.OrderType))
            {
                g.Where(f => f.DataKey.IsNotEmpty()).GroupBy(f => f.DataKey).ForEach(f =>
            {
                var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.Report, "", DateTime.Now, CallDirection.MesToSap, CallResult.UnSave, f.Count());
                try
                {
                    var retryDatas = uploadDataHandler.SetUploadData(f.ToList());
                    //retryDatas.DATAKEY = f.Key;
                    if (!cusHeadParams.ContainsKey("datakey"))
                    {
                        cusHeadParams.Add("datakey", f.Key);
                    }
                    var sapResult = RT.Service.Resolve<HttpHelper>().InvokeSapAPI(interfaceName, retryDatas, cusHeadParams);
                    erpDataInfLog.RequestContent = sapResult.RequestStr;
                    erpDataInfLog.ResponseContent = sapResult.ResponseStr;

                    if (sapResult.IsSuccess == false)
                    {
                        erpDataInfLog.CallResult = CallResult.Fail;
                    }
                    else
                    {
                        erpDataInfLog.CallResult = CallResult.Success;
                    }

                    sapResult.InterfaceName = interfaceName;
                    //更新上传状态
                    ctl.UpdateTranStateData(sapResult, f.ToList(),
                        sapResult.IsSuccess ? ProcessState.Processing : ProcessState.Failed,
                        sapResult.ResponseStr,
                        sapResult.SapUploadResultData.message
                        );

                }
                catch (Exception ex)
                {
                    erpDataInfLog.CallResult = CallResult.Fail;
                    erpDataInfLog.ErrorMsg = erpDataInfLog.ResponseContent;
                }
                finally
                {
                    erpDataInfLog.EndDate = DateTime.Now;
                    erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                    RF.Save(erpDataInfLog);
                }

            });
            }

            cusHeadParams.Remove("datakey");
            //第二次上传新的记录
            var nuds = uploadTransactions.Where(f => f.DataKey.IsNullOrEmpty()).ToList();
            if (nuds.Any())
            {
                foreach (var g in nuds.GroupBy(p=>p.OrderType))
                {
                    var newUploadDatas = g.ToList();
                    var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.Report, "", DateTime.Now, CallDirection.MesToSap, CallResult.UnSave, newUploadDatas.Count());
                    try
                    {
                        Guid guid = Guid.NewGuid();
                        var billDatas = uploadDataHandler.SetUploadData(newUploadDatas);
                        cusHeadParams.Add("datakey", guid.ToString());
                        var sapResult = RT.Service.Resolve<HttpHelper>().InvokeSapAPI(interfaceName, billDatas, cusHeadParams);
                        erpDataInfLog.RequestContent = sapResult.RequestStr;
                        erpDataInfLog.ResponseContent = sapResult.ResponseStr;

                        if (sapResult.IsSuccess == false)
                        {
                            erpDataInfLog.CallResult = CallResult.Fail;
                        }
                        else
                        {
                            erpDataInfLog.CallResult = CallResult.Success;
                        }

                        string str = JsonConvert.SerializeObject(billDatas);
                        sapResult.InterfaceName = interfaceName;
                        //重新为成功或者失败
                        ctl.UpdateTranStateData(sapResult, newUploadDatas.Where(p => p.UploadCount >= 5).ToList(), sapResult.IsSuccess ? ProcessState.Processing : ProcessState.Failed, sapResult.ResponseStr, sapResult.SapUploadResultData.message);
                        //更新为成功或者重试
                        ctl.UpdateTranStateData(sapResult, newUploadDatas.Where(p => p.UploadCount < 5).ToList(), sapResult.IsSuccess ? ProcessState.Processing : ProcessState.Retry, sapResult.ResponseStr, sapResult.SapUploadResultData.message);

                        //更新上传状态
                        //ctl.UpdateTranStateData(sapResult, newUploadDatas,
                        //    sapResult.IsSuccess ? ProcessState.Processing : ProcessState.Failed,
                        //    sapResult.ResponseStr,
                        //    sapResult.SapUploadResultData.message
                        //    );
                    }
                    catch (Exception ex)
                    {
                        erpDataInfLog.CallResult = CallResult.Fail;
                        erpDataInfLog.ErrorMsg = erpDataInfLog.ResponseContent;
                    }
                    finally
                    {
                        erpDataInfLog.EndDate = DateTime.Now;
                        erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                        RF.Save(erpDataInfLog);
                    }
                }
            }

            return result;
        }




    }




}
