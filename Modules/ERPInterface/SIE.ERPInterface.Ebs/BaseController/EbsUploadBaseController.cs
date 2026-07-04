using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.ERPInterface.Ebs
{
    /// <summary>
    /// EBS上传数据基类
    /// </summary>
    public class EbsUploadBaseController : DomainController
    {
        /// <summary>
        /// 获取上传的基类实体
        /// </summary>
        /// <param name="f"></param>
        /// <param name="item">下载数据</param>
        /// <returns></returns>
        public virtual void GetEbsUploadDataBase(UploadTransaction f, EbsUploadDataBase item)
        {
            item.BillNo = f.BillNo;
            item.LineNo = f.BillLineNo;
            item.ProductBatch = f.ProductBatch;
            item.Quantity = f.Quantity;
            item.OrgName = f.ErpOrgName;
            item.OrganizationName = f.ErpOrganizationName;
            item.LocationCode = f.FromLocationCode;
            item.TransactionDate = f.TransactionDate.ToString("yyyy-MM-dd HH:mm:ss");
            item.BillLineErpKey = f.BillLineErpKey;
            item.ErpWarehouseCode = f.ErpWarehouseCode;
            item.TranId = f.InvTransactionId;
            item.UnitCode = f.UnitCode;
        }

        /// <summary>
        /// 获取上传的报文
        /// </summary>
        /// <param name="f"></param>
        /// <param name="customerStr"></param>
        /// <returns></returns>
        public virtual string GetRequireStr(EbsUploadDataBase f, string customerStr)
        {
            var str = @"{{12}            
            ""SCUX_SOURCE_NUM"": ""{0}"",
            ""SCUX_SOURCE_LINE_NUM"": ""{1}"",
            ""SCUX_SOURCE_LOT_NUM"": ""{2}"",
            ""ORGANIZATION_NAME"": ""{4}"",
            ""LINE_ID"": ""{5}"",
            ""SUBINVENTORY"": ""{6}"",            
            ""TRANSACTION_DATE"": ""{8}"",
            ""QUANTITY"": ""{9}"",
            ""LOT_NUMBER"": ""{10}"",            
            ""TRANSACTION_UOM"": ""{11}""
                        }".FormatArgs(f.BillNo, f.LineNo, f.TranId, f.OrgName, f.OrganizationName, f.BillLineErpKey, f.ErpWarehouseCode, f.LocationCode, f.TransactionDate,
                        f.Quantity, f.ProductBatch, f.UnitCode, customerStr);
            return str;
        }

        /// <summary>
        /// 设置上传EBS的报文内容
        /// </summary>
        /// <param name="uploadDatas"></param>
        /// <returns></returns>
        public virtual string SetUploadStr<T>(List<T> uploadDatas) where T : EbsUploadDataBase
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < uploadDatas.Count; i++)
            {
                var a = uploadDatas[i];
                sb.Append(a.RequestStr);
                if (i < uploadDatas.Count - 1)//不是最后一个
                    sb.Append(",");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 添加日志上传记录
        /// </summary>
        /// <param name="uploadDatas">上传的数据</param>
        /// <param name="ebsUploadLogs">日志</param>
        /// <param name="ebsp">参数</param>
        public virtual string AddEbsUploadLog<T>(List<T> uploadDatas, List<ErpUploadLog> ebsUploadLogs, EbsParameter ebsp) where T : EbsUploadDataBase
        {
            EntityList<ErpUploadLog> saveEntitys = new EntityList<ErpUploadLog>();
            //优先更新成功的数据，失败的就算后面报错，也不影响重传
            #region 更新上传成功的数据
            var successIds = ebsUploadLogs.Where(a => a.IsSuccess).Select(a => a.TransactionId).ToList();
            if (successIds.Any())
            {
                successIds.SplitDataExecute(sons =>
                {
                    DB.Update<ErpUploadLog>().Set(a => a.IsSuccess, true).Set(a => a.State, Common.Enums.ProcessState.Processed).Set(a => a.ResponseMessage, "").Set(a => a.ReloadCount, a => a.ReloadCount + 1)
                    .Where(p => sons.Contains(p.TransactionId)).Execute();
                    DB.Update<UploadTransaction>().Set(a => a.State, Common.Enums.ProcessState.Processed).Where(p => sons.Contains(p.InvTransactionId)).Execute();
                });
            }
            #endregion
            try
            {
                #region 更新上传失败的数据

                var tranIds = uploadDatas.Select(a => a.TranId).ToList();
                EntityList<ErpUploadLog> ebsUploads = new EntityList<ErpUploadLog>();
                tranIds.SplitDataExecute(a =>
                {
                    var rst = DB.Query<ErpUploadLog>().Where(p => !p.IsSuccess == a.Contains(p.TransactionId.Value)).ToList();
                    ebsUploads.AddRange(rst);
                });
                List<double> existTranIds = ebsUploads.Select(a => a.TransactionId.Value).ToList();


                #endregion

                #region 插入新的上传记录日志
                uploadDatas.Where(a => !existTranIds.Contains(a.TranId)).ForEach(p =>
                {
                    var log = ebsUploadLogs.FirstOrDefault(a => a.TransactionId == p.TranId);
                    if (log == null)
                    {
                        log = new ErpUploadLog()
                        {
                            TransactionId = p.TranId,
                            OrderNo = p.BillNo,
                            LineNo = p.LineNo,
                            ErpWhCode = p.ErpWarehouseCode,
                            ErpDetailId = p.BillLineErpKey,
                            State = Common.Enums.ProcessState.Retry,
                            IsSuccess = false,
                            ResponseMessage = "ERP返回的参数结果没有当前的交易Id[{0}]".L10nFormat(p.TranId),
                            UploadTransactionId = p.UploadTranId,
                            InterfaceName = ebsp.InterfaceName,
                            InterfaceCode = ebsp.InterfaceCode,
                        };
                    }
                    log.JobType = ebsp.UploadJobType;
                    log.InterfaceCode = ebsp.InterfaceCode;
                    log.OrderType = ebsp.OrderType;
                    log.ErpWhCode = p.ErpWarehouseCode;
                    log.ErpDetailId = p.BillLineErpKey;
                    log.RequestStr = p.RequestStr;
                    log.UploadTransactionId = p.UploadTranId;
                    log.InterfaceName = ebsp.InterfaceName;
                    log.InterfaceCode = ebsp.InterfaceCode;
                    saveEntitys.Add(log);
                });
                RT.Service.Resolve<SIE.Core.Common.Controllers.CommonController>().BatchSave(saveEntitys);
                #endregion

                List<double> failIds = ebsUploadLogs.Where(a => !a.IsSuccess).Select(a => a.TransactionId.Value).ToList();
                ebsUploads.Where(a => failIds.Contains(a.TransactionId.Value)).ForEach(a =>
                 {
                     var log = ebsUploadLogs.FirstOrDefault(f => f.TransactionId == a.TransactionId);
                     a.ReloadCount = a.ReloadCount + 1;
                     a.ResponseMessage = log.ResponseMessage;
                     a.JobType = ebsp.UploadJobType;
                 });
                RF.Save(ebsUploads);
                List<double> failIdsNoPedding = ebsUploadLogs.Where(a => !a.IsSuccess && a.State != ProcessState.Processing).Select(a => a.TransactionId.Value).ToList();
                failIdsNoPedding.SplitDataExecute(sons =>
                {
                    DB.Update<UploadTransaction>().Set(a => a.State, Common.Enums.ProcessState.Retry).Where(p => sons.Contains(p.InvTransactionId)).Execute();
                });

                return "";
            }
            catch (Exception ex)
            {
                return "上传成功的数据已记录，失败的数据记录出现问题，".L10N() + ex.Message;
            }
        }

        ///// <summary>
        ///// 上传数据到EBS
        ///// </summary>
        ///// <param name="logIds">重传的记录</param>
        ///// <returns>处理结果</returns>
        //public virtual string ReUpload(List<double> logIds)
        //{
        //    var logs = Query<ErpUploadLog>().Where(f => logIds.Contains(f.Id)).ToList();
        //    if (!logs.Any())
        //        throw new ValidationException("没有记录".L10N());
        //    if (logs.Any(f => f.IsSuccess))
        //        throw new ValidationException("成功的不需要重传".L10N());
        //    var log = logs.FirstOrDefault();
        //    if (logs.Any(f => f.OrderType != log.OrderType))
        //        throw new ValidationException("重传类型必须一致".L10N());
        //    var ebsPara = EbsHelper.GetEbsParameter(false);

        //    ebsPara.InterfaceCode = log.InterfaceCode;//接口编码，接口卡有
        //    ebsPara.OrderType = log.OrderType;
        //    ebsPara.UploadJobType = log.JobType.Value;

        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < logs.Count; i++)
        //    {
        //        var a = logs[i];
        //        sb.Append(a.RequestStr);
        //        if (i < logs.Count - 1)//不是最后一个
        //            sb.Append(",");
        //    }
        //    ebsPara.UploadStr = sb.ToString();
        //    var tuple = EbsHelper.UploadExecuteEbsBase(ebsPara);
        //    if (tuple.Item2.Count > 0)
        //    {//请求成功，总的请求失败（ERP接口返回接口状态不是S，压根就没到处理层级）的不写入失败回传这里，待ERP恢复在调度重新跑

        //        var ebsUploadLogs = tuple.Item2;
        //        var successIds = ebsUploadLogs.Where(a => a.IsSuccess).Select(a => a.TransactionId).ToList();
        //        if (successIds.Any())
        //        {
        //            successIds.SplitDataExecute(sons =>
        //            {
        //                DB.Update<ErpUploadLog>().Set(a => a.IsSuccess, true).Set(a => a.ResponseMessage, "").Set(a => a.ReloadCount, a => a.ReloadCount + 1).Where(p => sons.Contains(p.TransactionId)).Execute();
        //                DB.Update<UploadTransaction>().Set(a => a.State, Common.Enums.ProcessState.Processed).Where(p => sons.Contains(p.InvTransactionId)).Execute();
        //            });
        //        }
        //        try
        //        {
        //            List<double> failIds = ebsUploadLogs.Where(a => !a.IsSuccess).Select(a => a.TransactionId).ToList();
        //            logs.Where(a => failIds.Contains(a.TransactionId)).ForEach(a =>
        //            {
        //                var log = ebsUploadLogs.FirstOrDefault(f => f.TransactionId == a.TransactionId);
        //                a.ReloadCount = a.ReloadCount + 1;
        //                a.ResponseMessage = log.ResponseMessage;
        //            });
        //            RF.Save(logs);
        //        }
        //        catch (Exception ex)
        //        {
        //            tuple.Item1.TailMsg = "上传成功的数据已记录，失败的数据记录出现问题，".L10N() + ex.Message;
        //        }
        //    }

        //    return tuple.Item1.Result ? "上传成功".L10N() : tuple.Item1.Msg;
        //}

        /// <summary>
        /// 更新事务上传记录
        /// </summary>
        /// <param name="logIds">事务ID</param>
        /// <param name="state">状态</param>
        /// <returns></returns>
        public virtual string UpdateEbsUploadLog(List<double> logIds, ProcessState state)
        {
            using (var tran = DB.TransactionScope(EbsInterfaceEntityDataProvider.ConnectionStringName))
            {
                var query = Query<ErpUploadLog>().Where(p => logIds.Contains(p.Id) && !p.IsSuccess);
                if (state == ProcessState.Abandon)
                {
                    var ebsUploadLogs = query.Where(p => p.State != ProcessState.Retry).ToList();
                    if (ebsUploadLogs.Any())
                    {
                        return "事务上传记录存在状态不是重试状态的数据，请刷新数据后再操作".L10N();
                    }
                    else
                    {
                        DB.Update<ErpUploadLog>().Set(p => p.State, ProcessState.Abandon).Where(p => logIds.Contains(p.Id)).Execute();
                        tran.Complete();
                        return "关闭上传记录成功".L10N();
                    }
                }
                else if (state == ProcessState.Retry)
                {
                    var ebsUploadLogs = query.Where(p => p.State != ProcessState.Abandon).ToList();
                    if (ebsUploadLogs.Any())
                    {
                        return "事务上传记录存在状态不是放弃状态的数据，请刷新数据后再操作".L10N();
                    }
                    else
                    {
                        DB.Update<ErpUploadLog>().Set(p => p.State, ProcessState.Retry).Where(p => logIds.Contains(p.Id)).Execute();
                        tran.Complete();
                        return "恢复上传记录成功".L10N();
                    }
                }
            }
            return null;
        }
    }
}
