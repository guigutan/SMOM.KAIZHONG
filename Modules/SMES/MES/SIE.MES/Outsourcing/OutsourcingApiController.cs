using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.EMMA;
using DocumentFormat.OpenXml.Office2016.Excel;
using DocumentFormat.OpenXml.Office2021.DocumentTasks;
using Microsoft.Scripting.Utils;

using MimeKit.Cryptography;
using Newtonsoft.Json;
using NPOI.HSSF.Record;
using Org.BouncyCastle.Asn1.Ocsp;
using SIE.Api;
using SIE.Barcodes;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Common;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.ErpCommon;
using SIE.EventMessages.ErpCommon.Datas;
using SIE.EventMessages.LES;
using SIE.EventMessages.MES.Inspection;
using SIE.Inventory.Commom;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using SIE.MES.BatchWIP;
using SIE.MES.Outsourcing.Configs;
using SIE.MES.Outsourcing.Datas;
using SIE.MES.Outsourcing.InStocks;
using SIE.MES.Outsourcing.Model;
using SIE.MES.Outsourcing.Outbounds;
using SIE.MES.WorkOrders;
using SIE.MES.WorkOrders.Configs;
using SIE.Packages.ItemLabels;
using SIE.Rbac.InvOrgs;
using SIE.Security;
using SIE.Tech.Processs;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WorkOrder = SIE.MES.WorkOrders.WorkOrder;
using WorkOrderController = SIE.MES.WorkOrders.WorkOrderController;

namespace SIE.MES.Outsourcing
{
    /// <summary>
    /// 工序委外Api接口
    /// </summary>
    public class OutsourcingApiController : OutsourcingController
    {
        #region 工厂之间创建委外需求单

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="type">操作类型:1(出库),2(入库)</param>
        [ApiService("")]
        [AllowAnonymous]
        public virtual RequestSyncResultData OutsourcingRequestSync(OutsourcingRequest request, int type, string invOrg)
        {
            RT.Service.Resolve<LoginController>().Login(invOrg);
            RequestSyncResultData data = new RequestSyncResultData();
            try
            {
                //获取委外需求单
                OutsourcingRequest outsourcing = GetOutsourcingRequestByNo(request.NO);

                var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrder(request.WorkOrderNo);
                if (wo == null)
                {
                    throw new ValidationException("未找到对应的工单号[{0}]".L10nFormat(request.WorkOrderNo));
                }

                if (outsourcing == null)
                {
                    //判断是否已经存在，如果不存在，就要同步过来，copy一份
                    outsourcing = new OutsourcingRequest();
                    outsourcing.Clone(request, new CloneOptions(CloneActions.NormalProperties));
                    outsourcing.GenerateId();
                    outsourcing.WorkOrderId = wo.Id;

                    //历史逻辑
                    //outsourcing.BeginProcessId = wo.RoutingProcessList.FirstOrDefault(p => p.Name == request.BeginProcessName).Id;
                    //outsourcing.EndProcessId = wo.RoutingProcessList.FirstOrDefault(p => p.Name == request.EndProcessName).Id;
                    outsourcing.PersistenceStatus = PersistenceStatus.New;
                    //RF.Save(outsourcing);
                }

                //获取工序对象并赋值 - 新逻辑修改 - 2026-5-26 - yjb
                var beginProcess = wo.RoutingProcessList.FirstOrDefault(p => p.Name == request.BeginProcessName);
                var endProcess = wo.RoutingProcessList.FirstOrDefault(p => p.Name == request.EndProcessName);

                if (beginProcess == null)
                    throw new ValidationException("起始工序[{0}]不存在工单[{1}]下的工序清单中 请检查".L10nFormat(request.BeginProcessName, request.WorkOrderNo));

                if (endProcess == null)
                    throw new ValidationException("结束工序[{0}]不存在工单[{1}]下的工序清单中 请检查".L10nFormat(request.EndProcessName, request.WorkOrderNo));

                outsourcing.BeginProcessId = beginProcess.Id;
                outsourcing.BeginProcess = beginProcess;  //加载对象
                outsourcing.EndProcessId = endProcess.Id;
                outsourcing.EndProcess = endProcess;      //加载对象
                RF.Save(outsourcing);

                outsourcing.OutboundQty = request.OutboundQty;
                outsourcing.WarehousingQty = request.WarehousingQty;
                outsourcing.ReportState = request.ReportState;
                outsourcing.OutboundState = request.OutboundState;
                outsourcing.OutsourcingState = request.OutsourcingState;


                EntityList<BarcodeRange> barcodeRanges = new EntityList<BarcodeRange>();
                EntityList<WipBatch> wipBatches = new EntityList<WipBatch>();
                EntityList<ProcessingOutbound> processingOutbounds = new EntityList<ProcessingOutbound>();
                //发料明细
                if (request.ProcessingOutsourcingOutboundList.Count >= 1)
                    SyncOutbound(request, outsourcing, wo, processingOutbounds, barcodeRanges, wipBatches);

                //收货明细
                EntityList<ProcessingInStock> processingInStocks = new EntityList<ProcessingInStock>();
                if (request.ProcessingOutsourcingInStockList.Count >= 1)
                {
                    SyncInStoc(request, outsourcing, wo, processingInStocks);
                }

                //报工记录
                EntityList<OutsourcingReportLog> logs = new EntityList<OutsourcingReportLog>();
                if (request.OutsourcingReportLogList.Count >= 1)
                    SyncLog(request, outsourcing, wo, logs, processingInStocks, processingOutbounds, barcodeRanges, wipBatches);

                using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                {
                    if (processingOutbounds.Count > 0)
                        RF.Save(processingOutbounds);
                    if (processingInStocks.Count > 0)
                        RF.Save(processingInStocks);
                    if (logs.Count > 0)
                        RF.Save(logs);
                    if (barcodeRanges.Count > 0)
                        RF.Save(barcodeRanges);
                    if (wipBatches.Count > 0)
                        RF.Save(wipBatches);
                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                data.errMsg = ex.GetBaseException()?.Message;
            }
            return data;
        }

        public virtual void SyncOutbound(OutsourcingRequest request, OutsourcingRequest outsourcing, WorkOrder wo, EntityList<ProcessingOutbound> processingOutbounds, EntityList<BarcodeRange> barcodeRanges, EntityList<WipBatch> wipBatches)
        {
            var sns = request.ProcessingOutsourcingOutboundList.Select(p => p.SN).Distinct().ToList();
            var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(sns);

            foreach (var p in request.ProcessingOutsourcingOutboundList)
            {
                var outbound = outsourcing.ProcessingOutsourcingOutboundList.FirstOrDefault(item => item.SN == p.SN);
                if (outbound != null)
                {
                    outbound.Qty = p.Qty;
                    processingOutbounds.Add(outbound);
                }
                else
                {
                    outbound = new ProcessingOutbound();
                    outbound.Qty = p.Qty;
                    outbound.OldId = p.Id;
                    outbound.OutsourcingRequestId = outsourcing.Id;
                    outbound.SourceId = 0;
                    outbound.SN = p.SN;
                    outbound.State = p.State;
                    outbound.LotNo = p.LotNo;
                    outbound.IsUpload = null;
                    outbound.GenerateId();
                    outbound.PersistenceStatus = PersistenceStatus.New;
                    outsourcing.ProcessingOutsourcingOutboundList.Add(outbound);
                    processingOutbounds.Add(outbound);
                    //创建标签
                    //var tuple = CreateWipBatch(wo, p.Qty, p.SN);
                    //barcodeRanges.Add(tuple.Item1);
                    //wipBatches.Add(tuple.Item2);
                }

                //获取标签是否存在，如果不存在就说明委外工厂进行了拆标签，那么就要创建新的，如果存在就更新数量
                var wipBatch = wipBatchs.FirstOrDefault(item => item.BatchNo == p.SN);
                if (wipBatch == null)
                {
                    var tuple = CreateWipBatch(wo, p.Qty, p.SN, outsourcing.BeginProcess.Process);
                    barcodeRanges.Add(tuple.Item1);
                    wipBatches.Add(tuple.Item2);
                    wipBatchs.Add(tuple.Item2);
                }
                else if (wipBatch.Qty != p.Qty)
                {
                    if (wipBatch.Qty != p.Qty)
                    {
                        wipBatch.EditQtyProcessCode = outsourcing.BeginProcess?.Process?.Code;
                    }
                    wipBatch.Qty = p.Qty;
                    wipBatch.PersistenceStatus = PersistenceStatus.Modified;
                    wipBatches.Add(wipBatch);
                }
            }
        }
        public virtual void SyncInStoc(OutsourcingRequest request, OutsourcingRequest outsourcing, WorkOrder wo, EntityList<ProcessingInStock> processingInStocks)
        {
            foreach (var p in request.ProcessingOutsourcingInStockList)
            {
                var inStock = outsourcing.ProcessingOutsourcingInStockList.FirstOrDefault(item => item.SN == p.SN);
                if (inStock != null)
                {
                    inStock.Qty = p.Qty;
                    inStock.ProcessingType = p.ProcessingType;
                    processingInStocks.Add(inStock);
                }
                else
                {
                    inStock = new ProcessingInStock();
                    inStock.Qty = p.Qty;
                    inStock.SN = p.SN;
                    inStock.LotNo = p.LotNo;
                    inStock.State = p.State;
                    inStock.ProcessingType = p.ProcessingType;
                    inStock.OldId = p.Id;
                    inStock.OutsourcingRequestId = outsourcing.Id;
                    inStock.SourceId = 0;
                    inStock.IsUpload = null;
                    inStock.GenerateId();
                    inStock.PersistenceStatus = PersistenceStatus.New;
                    //根据旧的Id，得到新的Id
                    if (p.OutboundId != null)
                    {
                        var first = outsourcing.ProcessingOutsourcingOutboundList.FirstOrDefault(item => item.OldId == p.OutboundId);
                        if (first != null)
                            inStock.OutboundId = first.Id;
                    }
                    outsourcing.ProcessingOutsourcingInStockList.Add(inStock);
                    processingInStocks.Add(inStock);
                }
            }
        }

        public virtual void SyncLog(OutsourcingRequest request, OutsourcingRequest outsourcing, WorkOrder wo, EntityList<OutsourcingReportLog> logs, EntityList<ProcessingInStock> processingInStocks, EntityList<ProcessingOutbound> processingOutbounds, EntityList<BarcodeRange> barcodeRanges, EntityList<WipBatch> wipBatches)
        {
            var sns = request.OutsourcingReportLogList.Select(p => p.SN).Distinct().ToList();
            var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(sns);
            foreach (var p in request.OutsourcingReportLogList)
            {
                var log = outsourcing.OutsourcingReportLogList.FirstOrDefault(item => item.SN == p.SN);
                if (log != null)
                {
                    log.Qty = p.Qty;
                    log.ProcessingType = p.ProcessingType;
                    log.State = p.State;
                    logs.Add(log);
                }
                else
                {
                    p.OldId = p.Id;
                    p.OutsourcingRequestId = outsourcing.Id;
                    p.IsUpload = null;
                    p.PersistenceStatus = PersistenceStatus.New;
                    p.GenerateId();
                    outsourcing.OutsourcingReportLogList.Add(p);
                    logs.Add(p);
                }
                //获取标签是否存在，如果不存在就说明委外工厂进行了拆标签，那么就要创建新的，如果存在就更新数量
                var wipBatch = wipBatchs.FirstOrDefault(item => item.BatchNo == p.SN);
                if (wipBatch == null)
                {
                    var tuple = CreateWipBatch(wo, p.Qty, p.SN, outsourcing.BeginProcess.Process);
                    barcodeRanges.Add(tuple.Item1);
                    wipBatches.Add(tuple.Item2);
                    wipBatchs.Add(tuple.Item2);
                }
                else if (wipBatch.Qty != p.Qty)
                {
                    if (wipBatch.Qty != p.Qty)
                    {
                        wipBatch.EditQtyProcessCode = outsourcing.BeginProcess?.Process?.Code;
                    }
                    wipBatch.Qty = p.Qty;
                    wipBatch.PersistenceStatus = PersistenceStatus.Modified;
                    wipBatches.Add(wipBatch);
                }

                //可能会拆标签,发料明细数据要重新校对
                //var outbound = outsourcing.ProcessingOutsourcingOutboundList.FirstOrDefault(item => item.SN == p.SN);
                //if (outbound != null)
                //{
                //    outbound.Qty = p.Qty;
                //}
                //else
                //{
                //    outbound = new ProcessingOutbound();
                //    outbound.GenerateId();
                //    outbound.Qty = p.Qty;
                //    outbound.SN = p.SN;
                //    outbound.LotNo = p.LotNo;
                //    outbound.State = OutsourcingDetailState.Submitted;
                //    outbound.OutsourcingRequestId = outsourcing.Id;
                //}
                //processingOutbounds.Add(outbound);
                //可能会拆标签,收货明细的数据要重新校对
                //var inStock = outsourcing.ProcessingOutsourcingInStockList.FirstOrDefault(item => item.SN == p.SN);
                //if (inStock != null)
                //{
                //    inStock.Qty = p.Qty;
                //    inStock.ProcessingType = p.ProcessingType;
                //}
                //else
                //{
                //    inStock = new ProcessingInStock();
                //    inStock.GenerateId();
                //    inStock.Qty = p.Qty;
                //    inStock.ProcessingType = p.ProcessingType;
                //    inStock.SN = p.SN;
                //    inStock.LotNo = p.LotNo;
                //    inStock.State = OutsourcingDetailState.Submitted;
                //    inStock.OutsourcingRequestId = outsourcing.Id;
                //}
                //processingInStocks.Add(inStock);
            }
        }

        public virtual (BarcodeRange, WipBatch) CreateWipBatch(WorkOrder workOrder, decimal qty, string Sn, Process process = null)
        {
            var range = new BarcodeRange()
            {
                PrintQty = (int)qty,
                StartSn = Sn,
                EndSn = Sn,
                State = ReceiveState.NoReceive,
                WorkOrderId = workOrder.Id,
            };
            var barcode = new WipBatch()
            {
                BatchNo = Sn,
                IsScraped = false,
                Qty = qty,
                BoxesQty = qty,
                IsMantissa = false,
                WorkOrderId = workOrder.Id,
                PrintDate = DateTime.Now,
                BatchState = BatchState.Generated,
                Range = range,
                IsChild = false,
                IsGenerateChild = false,
                IsGenerate = false,
                IsSuspectProduct = YesNo.No,
                IsOutsourcing = true,
                EditQtyProcessCode = process?.Code
                //DispatchTaskId = dispatchTask?.Id,
                //ResourceCode = dispatchTask?.ResourceCode,
                //ProcessCode = dispatchTask?.ProcessCode
            };
            return (range, barcode);
        }

        #endregion

        #region 发货修改

        /// <summary>
        /// 获取发货修改数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [ApiService("发货修改：获取发货修改数据")]
        public virtual OutboundModifyData GetOutboundModifyDatas(string key)
        {
            var snDetails = Query<OutboundConfirmSnDetail>().Where(p => p.OutboundConfirmDetail.FlowNo.Contains(key)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (snDetails == null || snDetails.Count < 1)
                throw new ValidationException("未找到流程单号为{0}数据".L10nFormat(key));
            if (snDetails.FirstOrDefault().OutboundConfirmDetail.State != OutboundConfirmDetailState.Return)
                throw new ValidationException("OA单不为退回，不允许修改".L10N());

            var detail = snDetails.FirstOrDefault().OutboundConfirmDetail;
            OutboundModifyData data = new OutboundModifyData();
            data.Id = detail.Id;
            data.FlowNo = detail.FlowNo;
            data.Outer = detail.Outer?.Name;
            data.OutFactory = detail.OutFactory;
            data.InitiatorFactory = detail.InitiatorFactory;
            data.DelveryDate = detail.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");
            data.Qty = detail.Qty;
            foreach (var snDetail in snDetails)
            {
                data.detailDatas.Add(new OutboundModifyDetailData()
                {
                    SnDetailId = snDetail.Id,
                    SnQty = snDetail.Qty,
                    Sn = snDetail.Sn,
                    ItemCode = snDetail.ItemCode,
                    ItemName = snDetail.ItemName
                });

            }
            return data;
        }

        /// <summary>
        /// 发货修改：扫描工序标签
        /// </summary>
        /// <param name="Sn"></param>
        /// <returns></returns>
        [ApiService("发货修改：扫描工序标签")]
        public virtual OutboundModifyAddDetailData GetOutboundModifyAddDetailData(string Sn)
        {
            var outbound = Query<ProcessingOutbound>().Where(p => p.SN == Sn).FirstOrDefault( new EagerLoadOptions().LoadWithViewProperty());
            if (outbound == null)
                throw new ValidationException("未找到产品条码{0}数据".L10nFormat(Sn));

            var outboundConfirmSnDetail = Query<OutboundConfirmSnDetail>().Where(p => p.ProcessingOutbound.SN == Sn).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (outboundConfirmSnDetail != null)
                throw new ValidationException("标签{0}属于流程单号{1}，不允许扫描".L10nFormat(Sn, outboundConfirmSnDetail.OutboundConfirmDetail?.FlowNo));

            OutboundModifyAddDetailData data = new OutboundModifyAddDetailData();
            data.ProcessingOutboundId = outbound.Id;
            data.SnQty = outbound.Qty;
            data.ItemCode = outbound.ProductCode;
            data.ItemName = outbound.ProductName;
            data.Sn = outbound.SN;
            return data;
        }

        /// <summary>
        /// 发货修改提交数据
        /// </summary>
        /// <param name="data"></param>
        [ApiService("发货修改：提交数据")]
        public virtual void OutboundModifySubmitData(OutboundModifySubmitData data)
        {
            if (data == null)
                throw new ValidationException("提交数据不能为空".L10N());

            var detail = RF.GetById<OutboundConfirmDetail>(data.Id, new EagerLoadOptions().LoadWithViewProperty());
            if (detail == null)
                throw new ValidationException("找不到对应的流程单".L10N());
            //判断是否修改了框数，要同步更新到事务上传
            var isModifyQty = false;
            if (detail.Qty != data.Qty)
            {
                detail.Qty = data.Qty;
                isModifyQty = true;
            }
            List<double> cancelConfirmIds = new List<double>();
            if (data.deleteDatas.Count > 0)
            {
                detail.OutboundConfirmSnDetailList.Where(p => data.deleteDatas.Contains(p.Id)).ForEach(p => {
                    //记录要取消确认的数据，后面会同一取消确认
                    cancelConfirmIds.Add(p.ProcessingOutboundId);
                    p.PersistenceStatus = PersistenceStatus.Deleted;
                });
            }

            if (data.addDatas.Count > 0)
            {
                foreach (var addId in data.addDatas)
                {
                    detail.OutboundConfirmSnDetailList.Add(new OutboundConfirmSnDetail()
                    {
                        ProcessingOutboundId = addId,
                        OutboundConfirmDetail = detail,
                        OutboundConfirmDetailId = detail.Id,
                        PersistenceStatus = PersistenceStatus.New
                    });
                }
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(detail);
                if (isModifyQty == true)
                    RT.Service.Resolve<IUploadLogControllercs>().UpdateOutboundConfirmTransaction(detail.Zuid, detail.Qty);

                //取消确认
                if (cancelConfirmIds.Count > 0)
                    DB.Update<ProcessingOutbound>().Set(p => p.IsConfirm, false).Where(p => cancelConfirmIds.Contains(p.Id)).Execute();
                //确认
                if (data.addDatas.Count > 0)
                    DB.Update<ProcessingOutbound>().Set(p => p.IsConfirm, true).Where(p => data.addDatas.Contains(p.Id)).Execute();
                tran.Complete();
            }
        }

        #endregion

        #region 发货确认

        /// <summary>
        /// 获取委外需求单工厂信息
        /// </summary>
        /// <returns></returns>
        [ApiService("获取委外需求单工厂信息")]
        public virtual List<string> GetOutsourcingFactoryData(string key)
        {
            var q = Query<OutsourcingRequest>().Where(p => p.OutFactory != null);
            if (!key.IsNullOrEmpty())
                q.Where(p => p.OutFactory.Contains("%" + key + "%"));
            var outFactorys = q.GroupBy(p => p.OutFactory).Distinct().Select(p => p.OutFactory).ToList<string>().OrderBy(p => p).ToList();

            return outFactorys;
        }

        /// <summary>
        /// 获取委外发货明细信息
        /// </summary>
        /// <param name="factory"></param>
        /// <returns></returns>
        [ApiService("获取委外发货明细信息")]
        public virtual List<OutboundConfirmData> GetOutboundConfirmDatas(string factory)
        {
            var config = ConfigService.GetConfig(new OutsourcingRequestPDAConfig(), typeof(OutsourcingRequest));
            int confirmDay = 0;
            if (config != null && config.ConfirmDay != null && config.ConfirmDay != 0 && config.ConfirmDay > 0)
                confirmDay = config.ConfirmDay.Value;

            var curDate = RF.Find<ProcessingOutbound>().GetDbTime();

            var outbounds = Query<ProcessingOutbound>().Where(p => p.OutsourcingRequest.OutFactory == factory && (p.IsConfirm == false || p.IsConfirm == null) && (curDate.AddDays(confirmDay * -1).Date) >= p.CreateDate)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            List<OutboundConfirmData> datas = new List<OutboundConfirmData>();
            foreach (var outbound in outbounds)
            {
                OutboundConfirmData data = new OutboundConfirmData();

                data.OutboundId = outbound.Id;
                data.WorkOrderNo = outbound.WorkOrderNo;
                data.ItemCode = outbound.ProductCode;
                data.ItemName = outbound.ProductName;
                data.ShortDescription = outbound.ShortDescription;
                data.SN = outbound.SN;
                data.Qty = outbound.Qty;
                data.Process = outbound.BeginProcessName;
                data.OutFactory = outbound.OutFactory;
                data.InitiatorFactory = outbound.InitiatorFactory;
                data.CreateDate = outbound.CreateDate.ToString("yyyy-MM-dd HH:mm:ss");

                datas.Add(data);
            }

            return datas;
        }

        /// <summary>
        /// 提交委外发货明细信息
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="qty"></param>
        [ApiService("提交委外发货明细信息")]
        public virtual void SubmitOutboundConfirmDatas(List<OutboundConfirmData> datas, decimal qty)
        {
            if (datas.Count < 1)
                throw new ValidationException("没有可提交数据".L10N());
            if (datas.Select(p => p.OutFactory).Distinct().Count() > 1)
                throw new ValidationException("接收工厂不一致不允许提交".L10N());
            if (qty <= 0)
                throw new ValidationException("框数必须大于0".L10N());
            OutboundConfirmDetail detail = new OutboundConfirmDetail();
            detail.OuterId = RT.IdentityId;
            detail.InitiatorFactory = datas.FirstOrDefault().InitiatorFactory;
            detail.OutFactory = datas.FirstOrDefault().OutFactory;
            detail.State = OutboundConfirmDetailState.Create;
            detail.Qty = qty;
            detail.PersistenceStatus = PersistenceStatus.New;
            detail.Zuid = Guid.NewGuid().ToString();
            detail.GenerateId();
            foreach (var data in datas)
            {
                detail.OutboundConfirmSnDetailList.Add(new OutboundConfirmSnDetail()
                {
                    ProcessingOutboundId = data.OutboundId,
                    OutboundConfirmDetail = detail,
                    OutboundConfirmDetailId = detail.Id,
                    PersistenceStatus = PersistenceStatus.New
                });
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(detail);
                var outboundIds = datas.Select(p => p.OutboundId).Distinct().ToList();
                DB.Update<ProcessingOutbound>().Set(p => p.IsConfirm, true).Where(p => outboundIds.Contains(p.Id)).Execute();
                tran.Complete();
            }

        }

        #endregion

        [ApiService("扫描工单或需求单号")]
        [return: ApiReturn("返回工单的委外需求单信息")]
        public virtual List<OutsourcingRequestInfo> Scan([ApiParameter("查询关键字")] string keyword, [ApiParameter("是否是入库")] bool isInstock = false)
        {        
            if (keyword.IsNullOrEmpty())
            {
                throw new ValidationException("请扫描物料标签!".L10nFormat(keyword));
            }
            List<OutsourcingRequestInfo> requestInfos = new List<OutsourcingRequestInfo>();
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);

            var requestList = Query<OutsourcingRequest>()
                .Join<WorkOrder>((x, y) => x.WorkOrderId == y.Id)
                .Join<WorkOrder, WipBatch>((x, y) => x.Id == y.WorkOrderId && y.BatchNo.Contains(keyword))
                .Where(m => m.OutsourcingState != OutsourcingState.Completed && m.OutsourcingState != OutsourcingState.Close && m.InitiatorFactory == invOrg.ExternalId)
                .OrderByDescending(m => m.CreateDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (!requestList.Any())
            {
                throw new ValidationException("没有找到工序委外需求单数据，请检查".L10nFormat(keyword));
            }
            if (isInstock && requestList.Count == 1 && requestList[0].OutsourcingState == OutsourcingState.NotStarted)
            {
                throw new ValidationException("委外需求单号为【{0}】的工序委外需求单数据状态为【未开始】，无法入库，请检查".L10nFormat(string.Join("、", requestList.Select(p => p.NO))));
            }

            foreach (var item in requestList)
            {
                if (isInstock && item.OutsourcingState == OutsourcingState.NotStarted)//开始状态的入库的时候不回传
                {
                    continue;
                }
                requestInfos.Add(new OutsourcingRequestInfo()
                {
                    Id = item.Id,
                    BeginProcess = item.BeginProcessName,
                    EndProcess = item.EndProcessName,
                    SupplierName = item.SupplierName,
                    ItemExtPropName = item.ItemExtPropName,
                    OutboundQty = item.OutboundQty,
                    OutsourcingState = (int)item.OutsourcingState,
                    OutsourcingStateDisplay = item.OutsourcingState.ToLabel().L10N(),
                    ProduceCode = item.ProduceCode,
                    ProduceName = item.ProduceName,
                    RequestNo = item.NO,
                    RequestQty = item.RequestQty,
                    WoNo = item.WorkOrderNo,
                    InboundQty = item.WarehousingQty
                });
            }
            return requestInfos;
        }


        [ApiService("扫描批次号或SN")]
        [return: ApiReturn("返回工单的委外可出库信息")]
        public virtual List<RequestDetailInfo> GetBarcodeInfo([ApiParameter("查询关键字")] string keyword, [ApiParameter("需求单Id")] double outsourcingId)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                throw new ValidationException("请输入条码或批次号".L10N());
            }
            OutsourcingRequest outsourcingRequest
                = RF.GetById<OutsourcingRequest>(outsourcingId, new EagerLoadOptions().LoadWithViewProperty());
            if (outsourcingRequest == null)
            {
                throw new EntityNotFoundException(typeof(OutsourcingRequest), outsourcingId);
            }
            var outbound = outsourcingRequest.ProcessingOutsourcingOutboundList.FirstOrDefault(p => p.LotNo == keyword);
            if (outbound != null)
            {
                throw new ValidationException("该标签已发料过，不能重复扫描".L10N());
            }

            //OutboundStrategy outboundStrategy = GetOutboundStrategy(outsourcingRequest);

            //if (outsourcingRequest.BeginProcess == null)
            //{
            //    throw new ValidationException("委外需求单【{0}】的【起始工序】为空".L10nFormat(outsourcingRequest.NO));
            //}
            //var outboundList = outboundStrategy.GetProcessingOutbounds(keyword, outsourcingRequest);
            //var infos = new List<RequestDetailInfo>();
            //foreach (var item in outboundList)
            //{
            //    infos.Add(new RequestDetailInfo()
            //    {
            //        SourceId = item.SourceId,
            //        Qty = item.Qty,
            //        Sn = item.SN

            //    });
            //}
            var wipBatch = Query<WipBatch>().Where(p => p.BatchNo == keyword).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            if (wipBatch == null)
                throw new ValidationException("标签不存在".L10N());
            if (wipBatch.WorkOrderId != outsourcingRequest.WorkOrderId)
                throw new ValidationException("标签不属于当前工单{0}".L10nFormat(outsourcingRequest.WorkOrderNo));
            //校验标签前工序是否报工
            ValidOutSourcing(wipBatch, outsourcingId);

            var infos = new List<RequestDetailInfo>();
            infos.Add(new RequestDetailInfo()
            {
                SourceId = 0,
                Qty = wipBatch.Qty,
                Sn = wipBatch.BatchNo
            });

            return infos;
        }

        /// <summary>
        /// 校验标签上个工序是否已经报工
        /// </summary>
        /// <param name="wipBatch"></param>
        /// <param name="outsourcingId"></param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ValidOutSourcing(WipBatch wipBatch, double outsourcingId)
        {
            var outsourcing = RF.GetById<OutsourcingRequest>(outsourcingId, new EagerLoadOptions().LoadWithViewProperty());
            //当前工序在工单工序中的顺序位置
            var curProcess = outsourcing.WorkOrder.RoutingProcessList.FirstOrDefault(p => p.ProcessId == outsourcing.BeginProcess.ProcessId);
            //获取工单当前工序的上一个工序
            var lastProcess = outsourcing.WorkOrder.RoutingProcessList.Where(p => p.Index < curProcess.Index).OrderByDescending(p => p.Index).FirstOrDefault();
            //如果为空，就证明当前工序是首工序，直接结束
            if (lastProcess == null)
                return;
            //获取上一个工序委外单
            var lastOutsourcing = Query<OutsourcingRequest>().Where(p => p.OutsourcingState != OutsourcingState.Close && p.WorkOrderId == outsourcing.WorkOrderId && p.BeginProcess.ProcessId == lastProcess.ProcessId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
            //存在，就校验是否已经完成
            if (lastOutsourcing != null)
            {
                if (lastOutsourcing.ProcessingOutsourcingInStockList.Any(p => p.SN == wipBatch.BatchNo))
                    return;
                else
                    throw new ValidationException("上一工序[{0}],委外需求单号[{1}]未收货，无法发料".L10nFormat(lastOutsourcing.BeginProcess.Process.Code, lastOutsourcing.NO));
            }
            else
            {
                //当委外需求单不存在，那么就是上个工序为当前工厂的任务单，判断任务单是否已经做完
                RT.Service.Resolve<ITaskReport>().ValidationLastProcessReport(wipBatch.Id, outsourcing.BeginProcess.ProcessId.Value);
                return;
            }
        }

        [ApiService("提交出库列表")]
        [return: ApiReturn("无返回")]
        public virtual void SubmitOutsourcingouts([ApiParameter("需求单Id")] double outsourcingId, [ApiParameter("提交列表")] List<RequestDetailInfo> requestOutDetailInfos)
        {
            OutsourcingRequest outsourcingRequest
               = RF.GetById<OutsourcingRequest>(outsourcingId, new EagerLoadOptions().LoadWithViewProperty());
            if (outsourcingRequest == null)
            {
                throw new EntityNotFoundException(typeof(OutsourcingRequest), outsourcingId);
            }
            if (outsourcingRequest.BeginProcess == null)
            {
                throw new ValidationException("委外需求单【{0}】的【起始工序】为空"
                        .L10nFormat(outsourcingRequest.NO));
            }
            if (outsourcingRequest.OutsourcingState == OutsourcingState.Completed)
            {
                throw new ValidationException("工序委外需求单已完成，无法提交，请检查".L10N());
            }
            if (requestOutDetailInfos == null || !requestOutDetailInfos.Any())
            {
                throw new ValidationException("请扫描条码或添加出库数量，请检查".L10N());
            }
            //if (requestOutDetailInfos.GroupBy(p => p.Sn).Any(p => p.Count() > 1))
            //{
            //    throw new ValidationException("不能提交多个相同的生产条码".L10N());
            //}
            if (requestOutDetailInfos.Exists(m => m.Qty <= 0))
            {
                throw new ValidationException("提交失败，存在数据的出库数量小于等于0，请检查".L10N());
            }

            var sns = requestOutDetailInfos.Select(p => p.Sn).Distinct().ToList();
            var WipBatchs = Query<WipBatch>().Where(p => sns.Contains(p.BatchNo)).ToList();
            //校验前工序是否报工完成
            RT.Service.Resolve<ITaskReport>().ValidationSeqTask(WipBatchs.Select(p => p.Id).Distinct().ToList());

            OutboundStrategy outboundStrategy = GetOutboundStrategy(outsourcingRequest);
            //存在来源Id 则使用outboundStrategy去执行保存
            var processingOutbounds = new List<ProcessingOutbound>();
            requestOutDetailInfos.ForEach(info =>
            {
                processingOutbounds.Add(new ProcessingOutbound()
                {
                    OutsourcingRequestId = outsourcingId,
                    LotNo = info.Sn,//outboundStrategy.RetrospectType == Core.Items.RetrospectType.Batch ? info.Sn : "",
                    SN = info.Sn,//outboundStrategy.RetrospectType == Core.Items.RetrospectType.Single ? info.Sn : "",
                    SourceId = info.SourceId,
                    PersistenceStatus = PersistenceStatus.New,
                    Qty = info.Qty,
                    State = OutsourcingDetailState.Submitted

                });

            });
            var hadSourceIdInfos = processingOutbounds.Where(m => m.SourceId != 0);//存在在制品
            var noSourceIdInfos = processingOutbounds.Where(m => m.SourceId == 0);//不存在在制品
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (hadSourceIdInfos.Any())
                {
                    outboundStrategy.SaveOutboundProduct(outsourcingRequest, hadSourceIdInfos.AsEntityList());
                }
                if (noSourceIdInfos.Any())
                {
                    var outQty = noSourceIdInfos.Sum(m => m.Qty);

                    var outboundState = OutboundState.PartOutbound;
                    if (outsourcingRequest.OutboundQty + outQty >= outsourcingRequest.RequestQty)
                        outboundState = OutboundState.Finish;

                    if (outsourcingRequest.OutsourcingState == OutsourcingState.NotStarted)
                    {
                        //更新出库量
                        DB.Update<OutsourcingRequest>()
                            .Set(x => x.OutboundQty, x => x.OutboundQty + outQty)
                            .Set(x => x.OutsourcingState, OutsourcingState.Outsourcing)
                            .Set(x => x.OutboundState, outboundState)
                            .Where(x => x.Id == outsourcingRequest.Id)
                            .Execute();
                    }
                    else
                    {
                        //更新出库量
                        DB.Update<OutsourcingRequest>()
                            .Set(x => x.OutboundQty, x => x.OutboundQty + outQty)
                            .Set(x => x.OutboundState, outboundState)
                            .Where(x => x.Id == outsourcingRequest.Id)
                            .Execute();
                    }
                    RF.Save(noSourceIdInfos.AsEntityList());
                    //RF.BatchInsert(noSourceIdInfos.AsEntityList());
                }
                trans.Complete();
            }

            //调用接口同步
            /**  切记：此处不能做保存,只为了做为接口参数    */
            OutsourcingRequest request = RF.GetById<OutsourcingRequest>(outsourcingRequest.Id, new EagerLoadOptions().LoadWithViewProperty());
            request.ProcessingOutsourcingInStockList.Clear();
            request.ProcessingOutsourcingOutboundList.Clear();
            request.ProcessingOutsourcingOutboundList.AddRange(noSourceIdInfos);
            //创建事务、调用同步接口
            RT.Service.Resolve<OutsourcingApiController>().SyncOutsourcingRequestToOtherFactory(request, 1, request.OutFactory);

        }

        [ApiService("传入批次号或SN ")]
        [return: ApiReturn("返回可入库条码和数量")]
        public virtual List<RequestDetailInfo> GetInboundBarcodeInfo([ApiParameter("查询关键字")] string keyword, [ApiParameter("需求单Id")] double outsourcingId)
        {

            if (string.IsNullOrEmpty(keyword))
            {
                throw new ValidationException("请输入条码或批次号".L10N());
            }
            OutsourcingRequest outsourcingRequest
                  = RF.GetById<OutsourcingRequest>(outsourcingId, new EagerLoadOptions().LoadWithViewProperty());
            if (outsourcingRequest == null)
            {
                throw new EntityNotFoundException(typeof(OutsourcingRequest), outsourcingId);
            }
            if (outsourcingRequest.BeginProcess == null)
            {
                throw new ValidationException("委外需求单【{0}】的【起始工序】为空"
                        .L10nFormat(outsourcingRequest.NO));
            }
            if (outsourcingRequest.ProcessingOutsourcingInStockList.Any(p => p.LotNo == keyword))
            {
                throw new ValidationException("标签已经扫过，不能重复扫描".L10N());
            }
            var config = GetOutsourcingReportConfig();
            if (!outsourcingRequest.OutsourcingReportLogList.Any(p => p.LotNo == keyword||p.SN == keyword) && config.IsOutsourcingInsVaildReportLog==true)
            {
                throw new ValidationException("标签{0}委外工序没有报工，不允许扫描".L10nFormat(keyword));
            }
            var outbound = outsourcingRequest.ProcessingOutsourcingOutboundList.FirstOrDefault(p => p.LotNo == keyword);
            if (outbound == null)
            {
                throw new ValidationException("该标签未发料过，无法扫描".L10N());
            }
            var infos = new List<RequestDetailInfo>();
            infos.Add(new RequestDetailInfo()
            {
                SourceId = 0,
                Qty = outbound.Qty,
                Sn = outbound.SN,
                OutboundId = outbound.Id,
            });
            //var inStockStrategy = GetInStockStrategy(outsourcingRequest);
            //var detailInfoList = inStockStrategy.GetProcessingInbounds(keyword, outsourcingRequest);
            //var infos = new List<RequestDetailInfo>();
            //foreach (var item in detailInfoList)
            //{
            //    infos.Add(new RequestDetailInfo()
            //    {
            //        SourceId = item.SourceId,
            //        Qty = item.Qty,
            //        Sn = item.SN,
            //        OutboundId = item.OutboundId.Value,
            //    });
            //}
            return infos;
        }

        [ApiService("提交入库列表")]
        [return: ApiReturn("无返回")]
        public virtual void SubmitOutsourcingIns([ApiParameter("需求单Id")] double outsourcingId, [ApiParameter("提交列表")] List<RequestDetailInfo> requestOutDetailInfos)
        {
            var curTime = RF.Find<OutsourcingRequest>().GetDbTime();
            OutsourcingRequest outsourcingRequest
               = RF.GetById<OutsourcingRequest>(outsourcingId, new EagerLoadOptions().LoadWithViewProperty());
            if (outsourcingRequest == null)
            {
                throw new EntityNotFoundException(typeof(OutsourcingRequest), outsourcingId);
            }
            if (outsourcingRequest.BeginProcess == null)
            {
                throw new ValidationException("委外需求单【{0}】的【起始工序】为空"
                        .L10nFormat(outsourcingRequest.NO));
            }
            if (outsourcingRequest.OutsourcingState != OutsourcingState.Outsourcing)
            {
                throw new ValidationException("工序委外需求单状态不为【委外中】，无法添加入库记录，请检查".L10N());
            }
            if (requestOutDetailInfos == null || !requestOutDetailInfos.Any())
            {
                throw new ValidationException("请扫描条码或添加入库数量，请检查".L10N());
            }
            //委外出库ID不为空的Ids
            var outboundIds = requestOutDetailInfos.Where(m => m.OutboundId != 0 && m.OutboundId.HasValue).Select(x => x.OutboundId).Distinct().ToList();
            //获取批次标签
            var Sns = requestOutDetailInfos.Select(p => p.Sn).Distinct().ToList();
            var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(Sns);
            var config = ConfigService.GetConfig(new OutsourcingReportConfig(), typeof(OutsourcingRequest));
            //当配置项启动校验报工记录明细的时候，校验条码是否已经报工过
            if (config != null && config.IsOutsourcingInsVaildReportLog == true)
            {
                var ss = Sns.Where(p => outsourcingRequest.OutsourcingReportLogList.All(a => a.SN != p)).Distinct().ToList();
                if (ss.Count > 0)
                {
                    throw new ValidationException("条码{0}未报工，无法收货".L10nFormat(string.Join('、', ss)));
                }
            }
            //委外出库
            var outbounds = outboundIds.SplitContains(tempIds =>
            {
                return Query<ProcessingOutbound>()
                    .Where(x => tempIds.Contains(x.Id))
                    .ToList();
            });
            //已存在的其他单的委外入库                
            var otherInStocks = outboundIds.SplitContains(tempIds =>
            {
                return Query<ProcessingInStock>()
                    .Where(x => tempIds.Contains(x.OutboundId))
                    .ToList();
            });
            var inStocks = new EntityList<ProcessingInStock>();//提交数据
            InStockStrategy inStockStrategy = GetInStockStrategy(outsourcingRequest);
            foreach (var requestOutDetailInfo in requestOutDetailInfos)
            {
                if (requestOutDetailInfo.Qty <= 0)
                {
                    throw new ValidationException("提交失败，存在数据的入库数量小于等于0，请检查".L10N());
                }
                if (requestOutDetailInfo.OutboundId != 0 && outbounds.Any())
                {
                    if (!outbounds.Any(m => m.Id == requestOutDetailInfo.OutboundId))
                    {
                        throw new ValidationException("找不到条码【{0}】的委外出库数据，请检查".L10nFormat(requestOutDetailInfo.Sn));
                    }
                    //校验数量
                    var otherInstockQty = otherInStocks
                       .Where(x => x.OutboundId == requestOutDetailInfo.OutboundId)
                       .Sum(x => x.Qty);
                    var outQty = outbounds.Where(x => x.Id == requestOutDetailInfo.OutboundId).Sum(x => x.Qty);


                    if (otherInstockQty + requestOutDetailInfo.Qty > outQty)
                    {
                        throw new ValidationException("【{0}】入库数【{1}】不能大于 同条码或同批次的委外出库数【{2}】"
                            .L10nFormat(requestOutDetailInfo.Sn, otherInstockQty + requestOutDetailInfo.Qty, outQty));
                    }
                }

                var wipBatch = wipBatchs.FirstOrDefault(p => p.BatchNo == requestOutDetailInfo.Sn);

                var processingType = GetProcessingTypeByWipBatch(wipBatch);
                //ProcessingType? processingType = wipBatch.IsSuspectProduct == YesNo.Yes ? ProcessingType.Sup : null;
                //if (wipBatch.IsSuspectProduct == YesNo.No || wipBatch.IsSuspectProduct == null)
                //{
                //    if (wipBatch.IsRework == true)
                //        processingType = ProcessingType.Rework;
                //    else if (wipBatch.IsScraped == true)
                //        processingType = ProcessingType.Scrap;
                //    else
                //        processingType = ProcessingType.Good;
                //}

                inStocks.Add(new ProcessingInStock()
                {
                    LotNo = requestOutDetailInfo.Sn,//inStockStrategy.RetrospectType == Core.Items.RetrospectType.Batch ? requestOutDetailInfo.Sn : "",
                    SN = requestOutDetailInfo.Sn,//inStockStrategy.RetrospectType == Core.Items.RetrospectType.Single ? requestOutDetailInfo.Sn : "",
                    OutboundId = requestOutDetailInfo.OutboundId,
                    OutsourcingRequestId = outsourcingRequest.Id,
                    Qty = requestOutDetailInfo.Qty,
                    PassQty = requestOutDetailInfo.Qty,
                    SourceId = requestOutDetailInfo.SourceId,
                    State = OutsourcingDetailState.Submitted,
                    PersistenceStatus = PersistenceStatus.New,
                    ProcessingType = processingType,
                });
            }

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                SubmitInstocks(inStockStrategy, outsourcingRequest, inStocks, null);

                if (config != null && config.IsInAutoReport == true)
                {
                    //入库上传报工
                    ProcessingInStockReport(inStocks, outsourcingRequest, curTime);

                    //创建报工记录(这个要放在上传报工方法之后，否则会影响上传SAP报工)
                    CreateLogs(outsourcingRequest, inStocks);

                    //当配置项启用了委外收货自动报工，且委外收货报工的工序为当前工单的最后一个工序，那么在委外收货时需要将工序标签生成到物料标签里面。（生成的物料标签数据参考末工序报工时生成的数据）
                    //最后一道工序
                    var lastProcess = outsourcingRequest.WorkOrder.RoutingProcessList.OrderByDescending(p => p.Index).FirstOrDefault()?.Process;
                    if (lastProcess != null && outsourcingRequest.BeginProcess.ProcessId == lastProcess.Id)
                    {
                        var sns = inStocks.Select(p => p.SN).Distinct().ToList();
                        if (sns.Count > 0)
                        {
                            //创建物料标签，和报工末工序创建物料一样
                            GenerateItemLabels(outsourcingRequest.WorkOrder, sns);
                        }
                    }
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 创建报工记录
        /// </summary>
        /// <param name="outsourcingRequest"></param>
        /// <param name="inStocks"></param>
        public virtual void CreateLogs(OutsourcingRequest outsourcingRequest, EntityList<ProcessingInStock> inStocks)
        {
            EntityList<OutsourcingReportLog> logs = new EntityList<OutsourcingReportLog>();
            var invOrg = RT.Service.Resolve<InvOrgController>().GetByCode(RT.InvOrg.Value);
            var sns = inStocks.Select(p => p.SN).Distinct().ToList();
            //var outsourcingReportLogList = Query<OutsourcingReportLog>().Where(p => sns.Contains(p.SN)).ToList();

            foreach (var inStock in inStocks)
            {
                OutsourcingReportLog log = null;/* outsourcingReportLogList.FirstOrDefault(p => p.SN == inStock.SN);*/
                if (log == null)
                    log = new OutsourcingReportLog();
                log.OutsourcingRequestId = outsourcingRequest.Id;
                log.SN = inStock.SN;
                log.LotNo = inStock.SN;
                log.Qty = inStock.Qty;
                log.PassQty = 0;
                log.NgQty = 0;
                log.State = OutsourcingDetailState.Submitted;
                log.PersistenceStatus = PersistenceStatus.New;
                log.ProcessingType = ProcessingType.Good;
                log.ReportFactory = invOrg.ExternalId;
                RF.Save(log);
                logs.Add(log);
            }
            var req = new OutsourcingRequest();
            req.Clone(outsourcingRequest, new CloneOptions(CloneActions.NormalProperties));
            //调用接口回传报工记录
            req.OutsourcingReportLogList.AddRange(logs);
            //if (processingInStocks.Count > 0)
            //{
            //    req.ProcessingOutsourcingInStockList.AddRange(processingInStocks);
            //}
            //if (processingOutbounds.Count > 0)
            //{
            //    req.ProcessingOutsourcingOutboundList.AddRange(processingOutbounds);
            //}
            //调用接口
            RT.Service.Resolve<OutsourcingApiController>().SyncOutsourcingRequestToOtherFactory(req, 3, req.OutFactory);
        }

        /// <summary>
        /// /创建物料标签，和报工末工序创建物料一样
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="batchNos"></param>
        public virtual void GenerateItemLabels(WorkOrder workOrder,List<string> batchNos)
        {
            var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(batchNos);
            var newLabels = new EntityList<ItemLabel>();

            var labelNos = wipBatchs.Select(p => p.BatchNo).ToList();
            var itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(labelNos);

            foreach (var wipBatch in wipBatchs)
            {
                if (wipBatch.IsScraped || wipBatch.IsSuspectProduct == YesNo.Yes)
                    continue;
                var itemLabel = itemLabels.FirstOrDefault(p => p.Label == wipBatch.BatchNo);

                //验证条码是否存在，存在则更新，不存在则新增
                if (itemLabel == null)
                {
                    itemLabel = new ItemLabel();
                }

                itemLabel.Label = wipBatch.BatchNo;
                itemLabel.Qty = wipBatch.Qty;
                if (itemLabel.InitialQty == null || itemLabel.InitialQty == 0)
                    itemLabel.InitialQty = itemLabel.Qty;
                itemLabel.ItemId = workOrder.ProductId;
                itemLabel.UnitId = workOrder.Product?.UnitId;
                itemLabel.SourceType = LabelSource.BatchWip;
                itemLabel.ItemLabelState = ItemLabelState.Receive;
                itemLabel.WorkOrderId = workOrder.Id;
                itemLabel.FactoryId = workOrder.FactoryId;
                itemLabel.Lot = workOrder.BatchNo;
                itemLabel.ProductionDate = wipBatch.CreateDate;
                itemLabel.Lgort = workOrder.Lgort;

                newLabels.Add(itemLabel);
            }
            RF.Save(newLabels);
        }

        /// <summary>
        /// 入库上传报工
        /// </summary>
        /// <param name="inStocks"></param>
        /// <param name="outsourcingRequest"></param>
        /// <param name="curTime"></param>
        public virtual void ProcessingInStockReport(EntityList<ProcessingInStock> inStocks, OutsourcingRequest outsourcingRequest, DateTime curTime)
        {
            List<ProcessingInStockReportTranData> datas = new List<ProcessingInStockReportTranData>();
            //1.可疑品不需要传
            //2.当工序标签在工序委外需求单中存在报工记录，则委外收货时不管有没有勾上需要报工，都不在进行委外收货报工了
            foreach (var inStock in inStocks.Where(p => p.ProcessingType != ProcessingType.Sup && outsourcingRequest.OutsourcingReportLogList.All(a => a.SN != p.SN)))
            {
                ProcessingInStockReportTranData data = new ProcessingInStockReportTranData();

                data.TransactionDate = curTime;
                data.WoId = outsourcingRequest.WorkOrderId;
                data.ItemId = outsourcingRequest.WorkOrder.ProductId;
                data.ItemCode = outsourcingRequest.ProduceCode;
                data.ItemName = outsourcingRequest.ProduceName;
                data.OrdKey = "ProcessingInStock_" + inStock.Id.ToString();
                data.WoNo = outsourcingRequest.WorkOrderNo;
                data.Quantity = inStock.Qty;
                data.BillLineId = inStock.Id;
                data.BillId = outsourcingRequest.Id;
                data.BillNo = outsourcingRequest.NO;

                var layoutInfo = outsourcingRequest.WorkOrder.LayoutInfoList.Where(p => p.ProcessCode == outsourcingRequest.BeginProcess.Process.Code && p.Factory == outsourcingRequest.OutFactory).FirstOrDefault();
                if (layoutInfo != null)
                {
                    data.WorkCenter = layoutInfo.WorkCenterCode;
                    data.WERKS = layoutInfo.Factory;
                    data.Vornr = layoutInfo.Vornr;
                }

                data.ProcessCode = outsourcingRequest.BeginProcess.Process.Code;
                if (inStock.ProcessingType == ProcessingType.Scrap)
                    data.NgQty = inStock.Qty;
                else
                    data.NgQty = 0;
                if (inStock.ProcessingType == ProcessingType.Good)
                    data.OkQty = inStock.Qty;
                else
                    data.OkQty = 0;

                if (inStock.ProcessingType == ProcessingType.Rework)
                    data.ReworkQty = inStock.Qty;
                else
                    data.ReworkQty = 0;
                if (inStock.ProcessingType == ProcessingType.Sup)
                    data.SuspectQty = inStock.Qty;
                else
                    data.SuspectQty = 0;
                datas.Add(data);
            }
            if (datas.Count > 0)
            {
                RT.Service.Resolve<IUploadLogControllercs>().ProcessingInStockReportTransaction(datas);
            }

        }

        public virtual ProcessingType GetProcessingTypeByWipBatch(WipBatch wipBatch)
        {
            ProcessingType? processingType = wipBatch.IsSuspectProduct == YesNo.Yes ? ProcessingType.Sup : null;
            if (wipBatch.IsSuspectProduct == YesNo.No || wipBatch.IsSuspectProduct == null)
            {
                if (wipBatch.IsRework == true)
                    processingType = ProcessingType.Rework;
                else if (wipBatch.IsScraped == true)
                    processingType = ProcessingType.Scrap;
                else
                    processingType = ProcessingType.Good;
            }
            return processingType.Value;
        }

        /// <summary>
        /// 同步委外需求单给其他工厂
        /// </summary>
        /// <param name="request"></param>
        /// <param name="type">操作类型:1(出库),2(入库),3(报工)</param>
        public virtual void SyncOutsourcingRequestToOtherFactory(OutsourcingRequest request, int type, string factory)
        {
            //创建事务上传记录、修改明细上传状态
            //CreateTransaction(request, factory);

            //调用接口，同步给其他工厂、并且更新事务上传
            var response = SyncOtherFactory(request, type, factory);
        }

        /// <summary>
        /// 同步
        /// </summary>
        /// <param name="request"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual RequestSyncResultData SyncOtherFactory(OutsourcingRequest request, int type, string factory)
        {
            //获取指定的接口地址
            var smomControlSetting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(factory);

            InfType infType = InfType.OutsourcingIns;
            switch (type)
            {
                case 1:
                    infType = InfType.Outsourcingouts;
                    break;
                case 2:
                    infType = InfType.OutsourcingIns;
                    break;
                case 3:
                    infType = InfType.OutsourcingReport;
                    break;
                case 4:
                    infType = InfType.ManuallySynchronize;
                    break;
            }

            var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(infType, "", DateTime.Now, CallDirection.FactoryToFactory, CallResult.Success, 1);
            erpDataInfLog.RequestContent = JsonConvert.SerializeObject(request);

            RequestSyncResultData response = new RequestSyncResultData();
            try
            {
                if (smomControlSetting == null || smomControlSetting.FactoryUrl.IsNullOrEmpty())
                    throw new ValidationException("未配置总控Url地址!".L10N());
                           

                var smomParam = new List<SmomParam>()
                {
                    new SmomParam { Value =request },
                                    new SmomParam { Value =type },
                                    new SmomParam { Value =factory }
                                 }.ToArray();
                response = SmomControlHepler.SmomPost<RequestSyncResultData>("OutsourcingApiController", "OutsourcingRequestSync", smomControlSetting.FactoryUrl, smomParam);

                erpDataInfLog.ResponseContent = JsonConvert.SerializeObject(response);
            }
            catch (Exception ex)
            {
                response.errMsg = ex.GetBaseException().Message;

                erpDataInfLog.CallResult = CallResult.Fail;
                erpDataInfLog.ErrorMsg = ex.GetBaseException().Message;
                erpDataInfLog.ResponseContent = ex.GetBaseException().Message;
            }
            finally
            {
                //更新事务上传
                //var outIds = request.ProcessingOutsourcingOutboundList.Select(p => p.Id).Distinct().ToList();
                //RT.Service.Resolve<IUploadLogControllercs>().RequestCreateTransaction(outIds, response.errMsg, 1);
                //var inIds = request.ProcessingOutsourcingInStockList.Select(p => p.Id).Distinct().ToList();
                //RT.Service.Resolve<IUploadLogControllercs>().RequestCreateTransaction(inIds, response.errMsg, 2);
                //var reportIds = request.OutsourcingReportLogList.Select(p => p.Id).Distinct().ToList();
                //RT.Service.Resolve<IUploadLogControllercs>().RequestCreateTransaction(reportIds, response.errMsg, 3);

                erpDataInfLog.EndDate = DateTime.Now;
                erpDataInfLog.PersistenceStatus = PersistenceStatus.Modified;
                RF.Save(erpDataInfLog);

                var outIds = request.ProcessingOutsourcingOutboundList.Select(p => p.Id).Distinct().ToList();
                var inIds = request.ProcessingOutsourcingInStockList.Select(p => p.Id).Distinct().ToList();
                var reportIds = request.OutsourcingReportLogList.Select(p => p.Id).Distinct().ToList();
                if (response.errMsg.IsNullOrEmpty())
                {
                    //修改上传状态
                    if (outIds.Count > 0)
                    {
                        DB.Update<ProcessingOutbound>().Set(p => p.IsUpload, true).Where(p => outIds.Contains(p.Id)).Execute();
                    }
                    if (inIds.Count > 0)
                    {
                        DB.Update<ProcessingInStock>().Set(p => p.IsUpload, true).Where(p => inIds.Contains(p.Id)).Execute();
                    }
                    if (reportIds.Count > 0)
                    {
                        DB.Update<OutsourcingReportLog>().Set(p => p.IsUpload, true).Where(p => reportIds.Contains(p.Id)).Execute();
                    }
                }
                else
                {
                    //修改重传次数
                    if (outIds.Count > 0)
                    {
                        DB.Update<ProcessingOutbound>().Set(p => p.ReLoadCount, p => p.ReLoadCount + 1).Where(p => outIds.Contains(p.Id) && p.ReLoadCount != null).Execute();
                        DB.Update<ProcessingOutbound>().Set(p => p.ReLoadCount, 0).Where(p => outIds.Contains(p.Id) && p.ReLoadCount == null).Execute();
                    }
                    if (inIds.Count > 0)
                    {
                        DB.Update<ProcessingInStock>().Set(p => p.ReLoadCount, p => p.ReLoadCount + 1).Where(p => inIds.Contains(p.Id) && p.ReLoadCount != null).Execute();
                        DB.Update<ProcessingInStock>().Set(p => p.ReLoadCount, 0).Where(p => inIds.Contains(p.Id) && p.ReLoadCount == null).Execute();
                    }
                    if (reportIds.Count > 0)
                    {
                        DB.Update<OutsourcingReportLog>().Set(p => p.ReLoadCount, p => p.ReLoadCount + 1).Where(p => reportIds.Contains(p.Id) && p.ReLoadCount != null).Execute();
                        DB.Update<OutsourcingReportLog>().Set(p => p.ReLoadCount, 0).Where(p => reportIds.Contains(p.Id) && p.ReLoadCount == null).Execute();
                    }
                }
            }

            return response;
        }

        /// <summary>
        /// 创建事务上传记录、修改明细上传状态
        /// </summary>
        /// <param name="request"></param>
        public virtual void CreateTransaction(OutsourcingRequest request, string factory)
        {
            var curDate = RF.Find<OutsourcingRequest>().GetDbTime();
            List<OutReqCreateTransactionData> datas = new List<OutReqCreateTransactionData>();
            foreach (var item in request.ProcessingOutsourcingOutboundList)
            {
                OutReqCreateTransactionData data = new OutReqCreateTransactionData();
                data.No = request.NO;
                data.Id = request.Id;
                data.DetailId = item.Id;
                data.Type = 1;
                data.WorkOrderId = request.WorkOrderId;
                data.WorkOrderNo = request.WorkOrder?.No;
                data.TransactionDate = curDate;
                data.Qty = request.RequestQty;
                data.WERKS = factory;
                data.LotNo = item.LotNo;
                data.ProcessCode = request.BeginProcess?.Process?.Code;
                datas.Add(data);
            }

            foreach (var item in request.ProcessingOutsourcingInStockList)
            {
                OutReqCreateTransactionData data = new OutReqCreateTransactionData();
                data.No = request.NO;
                data.Id = request.Id;
                data.DetailId = item.Id;
                data.Type = 2;
                data.WorkOrderId = request.WorkOrderId;
                data.WorkOrderNo = request.WorkOrder?.No;
                data.TransactionDate = curDate;
                data.Qty = request.RequestQty;
                data.WERKS = factory;
                data.LotNo = item.LotNo;
                data.ProcessCode = request.BeginProcess?.Process?.Code;
                datas.Add(data);
            }

            foreach (var item in request.OutsourcingReportLogList)
            {
                OutReqCreateTransactionData data = new OutReqCreateTransactionData();
                data.No = request.NO;
                data.Id = request.Id;
                data.DetailId = item.Id;
                data.Type = 3;
                data.WorkOrderId = request.WorkOrderId;
                data.WorkOrderNo = request.WorkOrder?.No;
                data.TransactionDate = curDate;
                data.Qty = request.RequestQty;
                data.WERKS = factory;
                data.LotNo = item.LotNo;
                data.ProcessCode = request.BeginProcess?.Process?.Code;
                datas.Add(data);
            }

            //创建事务上传
            RT.Service.Resolve<IUploadLogControllercs>().OutsourcingRequestCreateTransaction(datas);
            //这里保存的时候，一定要保存到明细，不能直接保存主表，因为主表只是用来做一个接口的传参的，是从现有的数据中复制出来的
            //改变上传状态
            //foreach (var item in request.ProcessingOutsourcingOutboundList)
            //{
            //    DB.Update<ProcessingOutbound>().Set(p => p.IsUpload, true).Where(p => p.Id == item.Id).Execute();
            //}
            //foreach (var item in request.ProcessingOutsourcingInStockList)
            //{
            //    DB.Update<ProcessingInStock>().Set(p => p.IsUpload, true).Where(p => p.Id == item.Id).Execute();
            //}
            //foreach (var item in request.OutsourcingReportLogList)
            //{
            //    DB.Update<OutsourcingReportLog>().Set(p => p.IsUpload, true).Where(p => p.Id == item.Id).Execute();
            //}
        }

        #region 手动同步 - 新增逻辑 - 2026-5-22 - yjb

        /// <summary>
        /// 手动同步出库数据到其他工厂
        /// </summary>
        /// <param name="outboundIds">出库明细ID列表</param>
        [ApiService("手动同步出库数据到其他工厂")]
        public virtual void ManualSyncOutboundsToOtherFactory([ApiParameter("出库明细ID列表")] double[] outboundIds)
        {
            if (outboundIds == null || outboundIds.Length == 0)
            {
                throw new ValidationException("请选择要同步的数据".L10N());
            }

            // 获取出库明细数据
            var outbounds = Query<ProcessingOutbound>()
                .Where(x => outboundIds.Contains(x.Id))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());

            if (outbounds == null || outbounds.Count == 0)
            {
                throw new ValidationException("未找到要同步的数据".L10N());
            }

            // 按委外需求单分组
            var groupedOutbounds = outbounds.GroupBy(x => x.OutsourcingRequestId);

            foreach (var group in groupedOutbounds)
            {
                var outsourcingRequest = RF.GetById<OutsourcingRequest>(group.Key, new EagerLoadOptions().LoadWithViewProperty());
                if (outsourcingRequest == null)
                {
                    throw new ValidationException("未找到委外需求单".L10N());
                }

                // 过滤出未同步的数据
                var unSyncOutbounds = group.ToList();
                if (!unSyncOutbounds.Any())
                {
                    throw new ValidationException("所选数据已全部同步，无需重复同步".L10N());
                }

                // 先调用检查接口，验证目标系统中是否存在这些条码
                var checkResult = CheckOutboundsExistInOtherFactory(outsourcingRequest, unSyncOutbounds, outsourcingRequest.OutFactory);

                if (!string.IsNullOrEmpty(checkResult.errMsg))
                {
                    throw new ValidationException(checkResult.errMsg);
                }

                // 构建同步数据
                OutsourcingRequest syncRequest = new OutsourcingRequest();
                syncRequest.Clone(outsourcingRequest, new CloneOptions(CloneActions.NormalProperties));
                syncRequest.ProcessingOutsourcingInStockList.Clear();
                syncRequest.ProcessingOutsourcingOutboundList.Clear();
                syncRequest.ProcessingOutsourcingInStockList.AddRange(outsourcingRequest.ProcessingOutsourcingInStockList);
                syncRequest.ProcessingOutsourcingOutboundList.AddRange(outsourcingRequest.ProcessingOutsourcingOutboundList);
                syncRequest.ProcessingOutsourcingOutboundList.AddRange(unSyncOutbounds);

                // 调用同步接口
                var response = SyncOtherFactory(syncRequest, 4, outsourcingRequest.OutFactory);

                if (!string.IsNullOrEmpty(response.errMsg))
                {
                    throw new ValidationException("同步失败：{0}".L10nFormat(response.errMsg));
                }
            }
        }

        /// <summary>
        /// 检查出库数据在目标工厂中是否存在
        /// </summary>
        /// <param name="outsourcingRequest">委外需求单</param>
        /// <param name="outbounds">出库明细列表</param>
        /// <returns>检查结果</returns>
        public virtual RequestSyncResultData CheckOutboundsExistInOtherFactory(OutsourcingRequest outsourcingRequest, List<ProcessingOutbound> outbounds, string factory)
        {
            RequestSyncResultData result = new RequestSyncResultData();

            try
            {
                // 获取目标工厂的URL配置
                var smomControlSetting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(outsourcingRequest.OutFactory);

               
                if (smomControlSetting == null || smomControlSetting.FactoryUrl.IsNullOrEmpty())
                {
                    result.errMsg = "未配置目标工厂的URL地址".L10N();
                    return result;
                }

                // 调用目标工厂的检查接口
                var sns = outbounds.Select(x => x.SN).Distinct().ToList();
                var smomParam = new List<SmomParam>()
                {
                    new SmomParam { Value = outsourcingRequest.NO },
                    new SmomParam { Value = sns },
                    new SmomParam { Value = factory }
                }.ToArray();

               
                var existResult = SmomControlHepler.SmomPost<List<string>>("OutsourcingApiController", "CheckOutboundsExist", smomControlSetting.FactoryUrl, smomParam);

                // 如果返回了已存在的条码列表，说明这些条码在目标系统中已存在
                if (existResult != null && existResult.Count > 0)
                {
                    result.errMsg = "以下条码在目标工厂中已存在，无法同步：{0}".L10nFormat(string.Join("、", existResult));
                }
            }
            catch (Exception ex)
            {
                result.errMsg = "检查目标工厂数据失败：{0}".L10nFormat(ex.GetBaseException()?.Message);
            }

            return result;
        }

        /// <summary>
        /// 检查出库条码是否已存在（供其他工厂调用）
        /// </summary>
        /// <param name="requestNo">委外需求单号</param>
        /// <param name="sns">条码列表</param>
        /// <param name="invOrg"></param>
        /// <returns>已存在的条码列表</returns>
        [ApiService("检查出库条码是否已存在")]
        [AllowAnonymous]
        public virtual List<string> CheckOutboundsExist([ApiParameter("委外需求单号")] string requestNo, [ApiParameter("条码列表")] List<string> sns, [ApiParameter("库存组织")] string invOrg)
        {
            var existList = new List<string>();
            //登入系统
            RT.Service.Resolve<LoginController>().Login(invOrg);

            if (string.IsNullOrEmpty(requestNo) || sns == null || sns.Count == 0)
            {
                return existList;
            }

            // 查询委外需求单
            var outsourcingRequest = Query<OutsourcingRequest>()
                .Where(x => x.NO == requestNo)
                .FirstOrDefault();

            if (outsourcingRequest == null)
            {
                return existList;
            }

            // 查询已存在的条码
            var existOutbounds = Query<ProcessingOutbound>()
                .Where(x => x.OutsourcingRequestId == outsourcingRequest.Id && sns.Contains(x.SN))
                .ToList()
                .Select(x => x.SN)
                .ToList();

            return existOutbounds;
        }

        #endregion
    }
}
