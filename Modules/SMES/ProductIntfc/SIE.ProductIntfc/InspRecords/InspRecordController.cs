using Newtonsoft.Json;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.Core.Logs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.Inspection;
using SIE.EventMessages.MES.Inspection;
using SIE.EventMessages.MES.Inspection.Models;
using SIE.MES.WorkOrders;
using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.InspLogs.Configs;
using SIE.ProductIntfc.InspLogs.Enums;
using SIE.ProductIntfc.InspSettings;
using SIE.ProductIntfc.ProductInsps;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SIE.ProductIntfc.InspRecords
{
    /// <summary>
    /// 报检记录控制器
    /// </summary>
    /// <seealso cref="SIE.DomainController" />
    public partial class InspRecordController : DomainController, IProductInsp
    {
        /// <summary>
        /// InspRecordController控制器
        /// </summary>
        private readonly string _inspRecordControllerString = "InspRecordController";

        /// <summary>
        /// 根据工单ID获取报检记录
        /// </summary>
        /// <param name="workOrderId">工单</param>
        /// <param name="resourceId">资源ID</param>
        /// <param name="inspType">报检类型</param>
        /// <returns>报检记录</returns>
        public virtual InspRecord GetInspRecord(double workOrderId, double resourceId, InspType inspType)
        {
            return Query<InspRecord>().Where(p => p.WorkOrderId == workOrderId && p.ResourceId == resourceId && p.InspType == inspType).FirstOrDefault();
        }

        /// <summary>
        /// 根据工单ID列表获取报检记录
        /// </summary>
        /// <param name="workOrderIds">工单ID列表</param>
        /// <param name="inspType">报检类型</param>
        /// <returns>报检记录</returns>
        public virtual EntityList<InspRecord> GetInspRecordList(List<double> workOrderIds, InspType inspType)
        {
            return Query<InspRecord>().Where(p => workOrderIds.Contains(p.WorkOrderId) && p.InspType == inspType).ToList();
        }

        /// <summary>
        /// 获取报检条码日志明细列表
        /// </summary>
        /// <param name="woId">工单ID</param>
        /// <param name="shopId">车间ID</param>
        /// <param name="resourceId">产线ID</param>
        /// <param name="inspType">检验类型</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>报检条码日志明细列表</returns>
        public virtual EntityList<InspBarcodeLog> GetInspBarcodeLogs(double woId, double shopId, double resourceId, InspType inspType, PagingInfo pagingInfo = null)
        {
            return Query<InspBarcodeLog>().Where(p => p.InspLog.WorkOrderId == woId && p.InspLog.ShopId == shopId && p.InspLog.ResourceId == resourceId && p.InspLog.InspType == inspType).ToList(pagingInfo);
        }

        /// <summary>
        /// 获取报检日志列表(贪婪加载子表)
        /// </summary>
        /// <param name="woId">工单ID</param>
        /// <param name="shopId">车间ID</param>
        /// <param name="resourceId">产线ID</param>
        /// <param name="inspType">检验类型</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <param name="sortInfo">排序参数</param>
        /// <returns>报检日志列表</returns>
        public virtual EntityList<InspLog> GetInspLogs(double woId, double shopId, double resourceId, InspType inspType, PagingInfo pagingInfo = null, IList<OrderInfo> sortInfo = null)
        {
            EagerLoadOptions eagerLoad = new EagerLoadOptions();
            eagerLoad.LoadWith(InspLog.InspBarcodeLogListProperty);
            var qry = Query<InspLog>();
            qry.Where(p => p.WorkOrderId == woId && p.ShopId == shopId && p.ResourceId == resourceId && p.InspType == inspType);
            return qry.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取最新一笔报检日志
        /// </summary>
        /// <param name="woId">工单ID</param>
        /// <param name="shopId">车间ID</param>
        /// <param name="resourceId">产线ID</param>
        /// <param name="inspType">检验类型</param>
        /// <returns>最新一笔报检日志</returns>
        public virtual InspLog GetLastInspLog(double woId, double shopId, double resourceId, InspType inspType)
        {
            return Query<InspLog>().Where(p => p.WorkOrderId == woId && p.ShopId == shopId && p.ResourceId == resourceId && p.InspType == inspType)
                .OrderByDescending(p => p.InspectionDate).FirstOrDefault();
        }

        /// <summary>
        /// 获取报检条码列表
        /// </summary>
        /// <param name="inspRecordId">报检记录ID</param>
        /// <param name="pagingInfo">分页参数</param>
        /// <param name="sortInfo">排序参数</param>
        /// <returns>报检条码列表</returns>
        public virtual EntityList<InspBarcode> GetInspBarcodes(double inspRecordId, PagingInfo pagingInfo, IList<OrderInfo> sortInfo)
        {
            return Query<InspBarcode>().Where(p => p.InspRecordId == inspRecordId).OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据报检条码Id列表获取报检条码列表
        /// </summary>
        /// <param name="inspBarcodeIds">报检条码Id列表</param>
        /// <returns>报检条码列表</returns>
        public virtual EntityList<InspBarcode> GetInspBarcodes(List<double> inspBarcodeIds)
        {
            return Query<InspBarcode>().Where(p => inspBarcodeIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWith(InspBarcode.InspRecordProperty));
        }

        /// <summary>
        /// 根据报检条码SN列表获取报检条码列表
        /// </summary>
        /// <param name="snList">报检条码SN列表</param>
        /// <param name="inspType">报检类型</param>
        /// <returns>报检条码列表</returns>
        public virtual EntityList<InspBarcode> GetInspBarcodeBySn(List<string> snList, InspType? inspType = null)
        {
            var q = Query<InspBarcode>();
            if (inspType.HasValue)
                q.Join<InspRecord>((a, b) => a.InspRecordId == b.Id && b.InspType == inspType.Value);
            return q.Where(p => snList.Contains(p.Barcode)).ToList();
        }

        /// <summary>
        /// 获取成品报检记录列表
        /// </summary>
        /// <param name="criteria">成品报检记录查询实体</param>
        /// <returns>报检记录列表</returns>
        public virtual EntityList<ProductInsp> GetProductInsps(ProductInspCriteria criteria)
        {
            var query = DB.Query<ProductInsp>().Where(p => p.InspType == InspType.Product);
            if (criteria.ShopId.HasValue)
                query.Where(p => p.ShopId == criteria.ShopId);
            if (criteria.ResourceId.HasValue)
                query.Where(p => p.ResourceId == criteria.ResourceId);
            if (!criteria.WorkOrder.IsNullOrEmpty())
                query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrder));
            if (!criteria.Barcode.IsNullOrEmpty())
            {
                ////两个Exists无法或，已报检条码不做查询
                query.Exists<InspBarcode>((x, y) => y.Where(p => p.InspRecordId == x.Id && p.Barcode.Contains(criteria.Barcode)));
                ////query.Exists<InspBarcodeLog>((x, y) => y.Join<InspLog>((a, b) => a.InspLogId == b.Id && a.Barcode.Contains(criteria.Barcode)).Where<InspLog>((a, b) => b.WorkOrderId == x.WorkOrderId && b.ShopId == x.ShopId && b.ResourceId == x.ResourceId)); 
            }
            if (criteria.InspNo.IsNotEmpty() || criteria.QmsNo.IsNotEmpty())
            {
                query.Exists<InspLog>((x, y) => y.WhereIf(criteria.InspNo.IsNotEmpty(), x => x.InspNo.Contains(criteria.InspNo))
                .WhereIf(criteria.QmsNo.IsNotEmpty(), x => x.CheckNo.Contains(criteria.QmsNo))
                .Where(p => p.WorkOrderId == x.WorkOrderId && p.ShopId == x.ShopId && p.ResourceId == x.ResourceId && p.InspType == InspType.Product));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取待自动报检报检记录
        /// </summary>
        /// <returns>报检记录</returns>
        public virtual EntityList<InspRecord> GetNeedInspRecords()
        {
            return Query<InspRecord>().Where(p => p.InspectionQty < p.PlanQty && p.InspType == InspType.Product).ToList();
        }

        /// <summary>
        /// 根据报检参数获取报检记录数量
        /// </summary>
        /// <param name="parameterId">报检参数ID</param>
        /// <returns>报检记录数</returns>
        public virtual int GetInspRecordCount(double parameterId)
        {
            return Query<InspRecord>().Where(p => p.InspParameterId == parameterId).Count();
        }

        /// <summary>
        /// 获取报检条码明细
        /// </summary>
        /// <param name="insptype">报检类型</param>
        /// <returns>列表InspLog</returns>
        public virtual EntityList<InspLog> GetInspLog(InspType insptype)
        {
            return Query<InspLog>().Where(p => p.InspType == insptype && p.InspectionResult == InspectionResult.Pass).ToList();
        }

        /// <summary>
        /// 获取报检条码信息
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="inspType">报检类型</param>
        /// <returns>报检条码信息列表</returns>
        public virtual IList<InspBarcodeInfo> GetInspBarcodeInfos(double? workOrderId, InspType inspType)
        {
            var query = Query<InspBarcode>().Join<Process>((b, p) => b.ProcessId == p.Id);
            if (workOrderId.HasValue)
                query.Join<InspRecord>((b, r) => b.InspRecordId == r.Id && r.WorkOrderId == workOrderId && r.InspType == inspType);
            else
                query.Join<InspRecord>((b, r) => b.InspRecordId == r.Id && r.InspType == inspType);
            var barcodes = query.Select<Process, InspRecord>((b, p, r) => new
            {
                InspBarcodeId = b.Id,
                WorkOrderId = r.WorkOrderId,
                Sn = b.Barcode,
                ProcessId = b.ProcessId,
                ProcessName = p.Name,
                CollectData = b.CollectionDate,
                Batch = r.BatchNo

            }).ToList<InspBarcodeInfo>();
            return barcodes;
        }

        #region 成品报检 
        /// <summary>
        /// 成品报检
        /// </summary>
        /// <param name="barcodeIds">报检条码Id列表</param>
        public virtual string ExecuteProductInsp(List<double> barcodeIds)
        {
            string errMsg = string.Empty;
            try
            {
                ProductInsp(barcodeIds);
            }
            catch (Exception exc)
            {
                errMsg = exc.Message;
            }

            return errMsg;
        }

        /// <summary>
        /// 成品报检
        /// </summary>
        /// <param name="barcodeIds">报检条码Id列表</param> 
        public virtual void ProductInsp(List<double> barcodeIds)
        {
            var barcodes = GetInspBarcodes(barcodeIds);
            decimal qty = barcodes.Sum(p => p.InspQty);
            if (qty == 0)
                return;
            using (var tran = DB.TransactionScope(CommonEntityDataProvider.ConnectionStringName))
            {
                var barcode = barcodes.OrderByDescending(p => p.CollectionDate).FirstOrDefault();
                var inspRecord = barcode.InspRecord;
                inspRecord.InspectionQty += qty; ////更新已报检数量
                var res = barcode.GetRepository() as EntityRepository;
                var inspDate = res.GetDbTime();
                var callQms = false;
                var config = ConfigService.GetConfig<InspLogCallConfigValue>(new InspLogCallConfig(), typeof(InspLog));
                if (config != null)
                {
                    callQms = config.IsCall;
                }
                var inspLog = CreateProductInspLog(inspRecord, qty, inspDate, barcode.ReportRecordId, callQms);
                CreateInspBarcodeLogs(barcodes, inspRecord, inspLog);
                var billEvent = CreateInspBillEvent(barcode, qty, inspLog.InspNo, inspLog.Id, barcodes);
                SaveGenerateShippingBillLog(billEvent);

                var qmsBillNo = string.Empty;
                if (callQms) // 是否传qms
                {
                    qmsBillNo = RT.Service.Resolve<ICreateShippingBill>().GenerateShippingBill(billEvent);
                }
                inspLog.CheckNo = qmsBillNo;
                RF.Save(inspLog);
                RF.Save(barcodes);
                RF.Save(inspRecord);
                tran.Complete();
            }
        }

        /// <summary>
        /// 成品报检，生成报检单
        /// </summary>
        /// <param name="inspEvent">成品报检事件</param>
        public virtual void ProductInsp(ProductInspEvent inspEvent)
        {
            SaveProductInspLog(inspEvent);
            var paramter = RT.Service.Resolve<InspParameterController>().GetInspParameter(inspEvent.ItemId, InspType.Product);
            if (paramter == null)//获取不到对应产品的报检参数，获取通用的报检参数
            {
                paramter = RT.Service.Resolve<InspParameterController>().GetGeneralInspParameter(InspType.Product);
            }
            if (CheckInspParameter(paramter, inspEvent))
            {
                SaveInspRecord(paramter, inspEvent);
            }
        }

        /// <summary>
        /// 保存生成成品检验单日志
        /// </summary>
        /// <param name="billEvent">报检参数</param>
        private void SaveGenerateShippingBillLog(InspBillEvent billEvent)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(billEvent);
                var inputValue = "报检参数:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "ICreateProductBill",
                    Method = "GenerateShippingBill",
                    ControllerName = _inspRecordControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 创建报检参数
        /// </summary>
        /// <param name="barcode">报检条码</param>
        /// <param name="qty">报检数量</param>
        /// <param name="inspNo">报检单号</param>
        /// <param name="inspLogId">报检日志ID</param>
        /// <param name="barcodes">条码列表</param>
        /// <returns>报检参数</returns>
        public virtual InspBillEvent CreateInspBillEvent(InspBarcode barcode, decimal qty, string inspNo, double inspLogId, EntityList<InspBarcode> barcodes)
        {
            var infos = new List<ProductBarcodeInfo>();
            foreach (var item in barcodes)
            {
                infos.Add(new ProductBarcodeInfo() { SN = item.Barcode.IsNotEmpty() ? item.Barcode : item.BatchNo, Qty = item.InspQty });
            }
            var inspRecord = barcode.InspRecord;
            var wo = RF.GetById<WorkOrder>(inspRecord.WorkOrderId);
            if (wo == null)
                throw new ValidationException("创建报检参数失败，找不到id为{0}的工单".L10nFormat(inspRecord.WorkOrderId));
            return new InspBillEvent()
            {
                InspNo = inspNo,
                StationId = barcode.StationId ?? 0,
                ProcessId = barcode.ProcessId ?? 0,
                InspLogId = inspLogId,
                ProductId = wo.ProductId,
                ShopId = inspRecord.ShopId,
                ResourceId = inspRecord.ResourceId,
                WorkOrderId = inspRecord.WorkOrderId,
                BatchNo = inspRecord.BatchNo,
                Qty = qty,
                CollectionDate = barcode.CollectionDate,
                Barcodes = infos,
                FactoryId = wo.FactoryId
            };
        }

        /// <summary>
        /// 创建报检条码日志明细
        /// </summary>
        /// <param name="barcodes">报检条码</param>
        /// <param name="inspRecord">报检记录</param>
        /// <param name="inspLog">报检日志</param>
        public virtual void CreateInspBarcodeLogs(EntityList<InspBarcode> barcodes, InspRecord inspRecord, InspLog inspLog)
        {
            barcodes.ForEach(e =>
            {
                var barcodeLog = new InspBarcodeLog()
                {
                    Barcode = e.Barcode,
                    BatchNo = e.BatchNo,
                    CollectionDate = e.CollectionDate,
                    ////InspDate = inspDate, //2018-09-11 移除字段InspDate
                    InspState = InspState.Inspection,
                    ProcessId = e.ProcessId,
                    StationId = e.StationId,
                    PersistenceStatus = PersistenceStatus.New
                };
                RT.Service.Resolve<InspRecordBaseController>().SetInspBarcodeLog(barcodeLog, e);
                barcodeLog.GenerateId();
                inspLog.InspBarcodeLogList.Add(barcodeLog);
                e.PersistenceStatus = PersistenceStatus.Deleted;
            });
        }

        /// <summary>
        /// 创建成品报检日志
        /// </summary>
        /// <param name="inspRecord">报检记录</param>
        /// <param name="qty">报检数量</param>
        /// <param name="inspDate">报检时间</param> 
        /// <param name="reportRecordId">报工记录Id</param> 
        /// <param name="callQms">是否回传qms</param> 
        /// <returns>报检日志</returns>
        public virtual InspLog CreateProductInspLog(InspRecord inspRecord, decimal qty, DateTime inspDate, double? reportRecordId = null, bool? callQms = false)
        {
            System.Diagnostics.Debug.Assert(inspRecord != null, "报检记录为空");
            var inspLog = new InspLog()
            {
                InspNo = RT.Service.Resolve<CommonController>().GetNo<ProductInsp>("成品报检单单号"),
                WorkOrderId = inspRecord.WorkOrderId,
                ShopId = inspRecord.ShopId,
                ResourceId = inspRecord.ResourceId,
                InspType = inspRecord.InspType,
                InspectionQty = qty,
                ReportRecordId = reportRecordId,
                ////BatchNo = inspRecord.BatchNo, //2018-09-11 移除字段BatchNo
                CustomerId = inspRecord.CustomerId,
                InspectionDate = inspDate,
                InspState = InspState.UnInspection,
                IsCall = callQms == null ? false : callQms.Value,
                InspectionStatus = InspLogs.Enums.InspectionStatus.WaitInspection,
                PersistenceStatus = PersistenceStatus.New
            };
            if (RT.IdentityId > 0)
                inspLog.OperateById = RT.IdentityId;  //自动报检无操作人，调度无法给到上下文
            inspLog.GenerateId();
            return inspLog;
        }

        /// <summary>
        /// 自动报检
        /// </summary>
        /// <returns>自动报检结果信息</returns>
        public virtual string AutoProductInsp()
        {
            StringBuilder message = new StringBuilder();
            var inspRecords = RT.Service.Resolve<InspRecordController>().GetNeedInspRecords();
            foreach (InspRecord inspRecord in inspRecords)
            {
                try
                {
                    var inspBarcodeList = RT.Service.Resolve<InspRecordBaseController>().GetInspBarcodeList(inspRecord.InspBarcodeList);
                    var count = inspBarcodeList.Count;
                    if (count == 0)
                    {
                        continue;
                    }
                    Logging.LogManager.GetLogger("job").Info("开始工单[{0}]成品自动报检".L10nFormat(inspRecord.WorkOrder?.No));
                    if (inspRecord.InspParameter.InspDimension == InspDimension.BatchQty)//按数量报检
                    {
                        decimal inspCount = inspRecord.InspParameter.InspParm;
                        if (count < inspCount && inspRecord.InspectionQty + count != inspRecord.PlanQty)
                            continue;
                        if (count < inspCount && inspRecord.InspectionQty + count == inspRecord.PlanQty)
                            inspCount = count;
                        var barcodes = inspBarcodeList.OrderBy(p => p.Id).Take((int)inspCount);
                        var barcodeIds = barcodes.Select(p => p.Id).Distinct().ToList();
                        ProductInsp(barcodeIds);
                    }
                    else if (inspRecord.InspParameter.InspDimension == InspDimension.Time)//按时间（分钟）报检
                    {
                        double inspMinute = inspRecord.InspParameter.InspParm;
                        var now = RF.Find<InspRecord>().GetDbTime();
                        var lastInapTime = now.AddMinutes(0 - inspMinute);
                        var lastInspLog = RT.Service.Resolve<InspRecordController>().GetLastInspLog(inspRecord.WorkOrderId, inspRecord.ShopId, inspRecord.ResourceId, InspType.Product);
                        if (lastInspLog != null && lastInspLog.InspectionDate > lastInapTime)
                            continue;
                        var workOrder = RF.GetById<WorkOrder>(inspRecord.WorkOrderId);
                        if (workOrder == null || workOrder.ActuStartDate == null)
                            continue;
                        var beginTime = workOrder.ActuStartDate.Value;
                        if (beginTime.AddMinutes(inspMinute) > now)
                            continue;
                        var barcodes = inspBarcodeList.ToList();
                        var barcodeIds = barcodes.Select(p => p.Id).Distinct().ToList();
                        ProductInsp(barcodeIds);
                    }
                    else
                    {
                        //
                    }
                    Logging.LogManager.GetLogger("job").Info("结束工单[{0}]成品自动报检".L10nFormat(inspRecord.WorkOrder?.No));
                }
                catch (Exception exc)
                {
                    message.AppendLine("工单[{0}]自动报检失败，失败信息：{1}".L10nFormat(inspRecord.WorkOrder?.No, exc.Message));
                }
                finally
                {
                    Thread.Sleep(10);
                }
            }

            return message.ToString();
        }

        /// <summary>
        /// 保存成品报检，生成报检单日志
        /// </summary>
        /// <param name="inspEvent">成品报检事件</param>
        private void SaveProductInspLog(ProductInspEvent inspEvent)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(inspEvent);
                var inputValue = "成品报检事件:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IProductInsp",
                    Method = "ProductInsp",
                    ControllerName = _inspRecordControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 验证是否满足报检条件
        /// </summary>
        /// <param name="paramter">报检参数</param>
        /// <param name="inspEvent">成品报检事件</param>
        /// <returns>满足返回true，不满足返回false</returns>
        public virtual bool CheckInspParameter(InspParameter paramter, ProductInspEvent inspEvent)
        {
            if (paramter == null || paramter.InspType != InspType.Product)
                return false;
            ////配置为最后工序，而当前采集非最后工序
            if (paramter.ProcessType == InspProcess.Last && !inspEvent.IsEndProcess)
                return false;
            else if (paramter.ProcessType == InspProcess.First && !inspEvent.IsStartProcess)
                return false;
            ////配置为自定义工序，而当前工序不一致
            else if (paramter.ProcessType == InspProcess.Custom && inspEvent.ProcessId != paramter.InspProcessId)
                return false;
            return true;
        }

        /// <summary>
        /// 保存报检记录
        /// </summary>
        /// <param name="paramter">报检参数</param>
        /// <param name="inspEvent">成品报检事件</param>
        private void SaveInspRecord(InspParameter paramter, ProductInspEvent inspEvent)
        {
            using (var tran = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                //加锁，解决InspRecord偶发重复创建问题
                DB.Update<InspParameter>().Set(p => p.InspType, p => p.InspType).Where(p => p.Id == paramter.Id).Execute();
                InspRecord inspRecord = RT.Service.Resolve<InspRecordController>().GetInspRecord(inspEvent.WorkOrderId, inspEvent.ResourceId, InspType.Product);
                if (inspRecord == null)
                {
                    inspRecord = new InspRecord();
                    inspRecord.WorkOrderId = inspEvent.WorkOrderId;
                    inspRecord.ResourceId = inspEvent.ResourceId;
                    inspRecord.PlanQty = inspRecord.WorkOrder.PlanQty;
                    inspRecord.InspectionQty = 0;
                    inspRecord.CustomerId = inspEvent.CustomerId;
                    inspRecord.ShopId = inspEvent.ShopId;
                    inspRecord.ResourceId = inspEvent.ResourceId;
                    inspRecord.InspType = InspType.Product;
                    inspRecord.InspParameterId = paramter.Id;
                }
                inspRecord.InspParameterId = paramter.Id;
                InspBarcode insBarcode = Query<InspBarcode>().Where(p => p.Barcode == inspEvent.Barcode && p.InspRecordId == inspRecord.Id).FirstOrDefault();
                if (insBarcode == null)
                {
                    insBarcode = new InspBarcode();
                }
                insBarcode.Barcode = inspEvent.Barcode;
                insBarcode.OperateById = RT.IdentityId;
                insBarcode.ProcessId = inspEvent.ProcessId;
                insBarcode.StationId = inspEvent.StationId;
                insBarcode.InspDate = inspEvent.CollectionDate;
                insBarcode.CollectionDate = inspEvent.CollectionDate;
                insBarcode.InspQty = inspEvent.OkQty;
                RT.Service.Resolve<InspRecordBaseController>().SetInspBarcode(insBarcode, inspEvent);

                ////inspRecord.InspBarcodeList.Add(insBarcode); 2023/3/27 csp 优化为 insBarcode.InspRecordId = inspRecord.Id; RF.Save(insBarcode);
                RF.Save(inspRecord);

                insBarcode.InspRecordId = inspRecord.Id;

                RF.Save(insBarcode);
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新报检记录
        /// </summary>
        /// <param name="billEvents">单据检验提交事件</param>
        /// <exception cref="EntityNotFoundException">报检日志未找到异常</exception>
        public virtual void UpdateInspResult(List<ShippingBillSubmittedEvent> billEvents)
        {
            using (var tran = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                SaveUpdateInspResultLog(billEvents);
                billEvents.ForEach(billEvent =>
                {
                    var inspLog = RF.GetById<InspLog>(billEvent.InspLogId);
                    if (inspLog == null)
                        throw new EntityNotFoundException(typeof(InspLog), billEvent.InspLogId);
                    inspLog.InspectionResult = billEvent.Result;
                    inspLog.InspectionStatus = InspLogs.Enums.InspectionStatus.Inspectioned;
                    inspLog.InspectDate = billEvent.InspectDate;
                    inspLog.CheckNo = billEvent.CheckNo;

                    if (billEvent.DefectIds != null && billEvent.DefectIds.Count > 0)
                    {
                        inspLog.DefectIds = string.Join(",", billEvent.DefectIds);
                    }

                    inspLog.InspBarcodeLogList.ForEach(log =>
                    {
                        var sample = billEvent.SampleList.FirstOrDefault(p => p.SN == log.Barcode);
                        if (sample != null)
                        {
                            log.InspectionResult = sample.InspectionResult;
                        }
                        else
                            log.InspectionResult = billEvent.Result;
                    });
                    RF.Save(inspLog);

                    //任务单报检成功回调处理方法
                    if (inspLog.ReportRecordId > 0)
                    {
                        var barcodeEvent = new ToStorageBarcodeEvent()
                        {
                            WorkOrderId = inspLog.WorkOrderId,
                            ReportRecordId = inspLog.ReportRecordId,
                            Qty = inspLog.InspectionQty,
                            InspectionResult = Convert.ToInt32(inspLog.InspectionResult),
                            InspectionStatus = Convert.ToInt32(inspLog.InspectionStatus),
                            ProcessMode = inspLog.ProcessMode,
                            DefectIds = billEvent.DefectIds,
                        };

                        if (billEvent.Result == InspectionResult.Pass)
                        {
                            RT.Service.Resolve<ITaskReport>().ToStorageTaskBarcode(barcodeEvent);//成品入库
                            RT.Service.Resolve<ITaskReport>().ToUpdateTaskReportState(barcodeEvent);// 更新报工记录状态
                        }
                        else
                        {
                            RT.Service.Resolve<ITaskReport>().ToUpdateTaskReportQty(barcodeEvent);//更新报检记录的合格数和不合格数
                        }
                    }
                });
                tran.Complete();
            }

        }

        /// <summary>
        /// 保存根据成品检验单结果更新成品报检结果日志
        /// </summary>
        /// <param name="billEvents">成品检验提交后事件集合</param>
        private void SaveUpdateInspResultLog(List<ShippingBillSubmittedEvent> billEvents)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(billEvents);
                var inputValue = "成品检验提交后事件集合:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IProductInsp",
                    Method = "UpdateInspResult",
                    ControllerName = _inspRecordControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新报检记录
        /// </summary>
        /// <param name="billEvents">单据不合格审核提交事件</param>
        /// <exception cref="EntityNotFoundException">报检日志未找到异常</exception>
        public virtual void UpdateAuditResult(List<ShippingBillSubmittedEvent> billEvents)
        {
            SaveUpdateAuditResultLog(billEvents);
            billEvents.ForEach(billEvent =>
            {
                var inspLog = RF.GetById<InspLog>(billEvent.InspLogId);
                if (inspLog == null)
                    throw new EntityNotFoundException(typeof(InspLog), billEvent.InspLogId);
                inspLog.InspectionResult = billEvent.Result;
                inspLog.InspectionStatus = InspectionStatus.Inspectioned;
                inspLog.InspectDate = billEvent.InspectDate;
                inspLog.CheckNo = billEvent.CheckNo;
                inspLog.ProcessMode = billEvent.ProcessMode;
                inspLog.Remark = billEvent.Remark;

                if (billEvent.DefectIds != null && billEvent.DefectIds.Count > 0)
                {
                    inspLog.DefectIds = string.Join(",", billEvent.DefectIds);
                }

                inspLog.InspBarcodeLogList.ForEach(log =>
                {
                    var sample = billEvent.SampleList.FirstOrDefault(p => p.SN == log.Barcode);
                    if (sample != null)
                    {
                        log.InspectionResult = sample.InspectionResult;
                    }
                    else
                        log.InspectionResult = billEvent.Result;
                });
                RF.Save(inspLog);

                //任务单报检成功回调处理方法
                if (inspLog.ReportRecordId > 0)
                {
                    var barcodeEvent = new ToStorageBarcodeEvent()
                    {
                        WorkOrderId = inspLog.WorkOrderId,
                        ReportRecordId = inspLog.ReportRecordId,
                        Qty = inspLog.InspectionQty,
                        InspectionResult = Convert.ToInt32(inspLog.InspectionResult),
                        InspectionStatus = Convert.ToInt32(inspLog.InspectionStatus),
                        ProcessMode = inspLog.ProcessMode,
                        DefectIds = billEvent.DefectIds,
                    };

                    if (billEvent.Result == InspectionResult.Pass)
                    {
                        RT.Service.Resolve<ITaskReport>().ToStorageTaskBarcode(barcodeEvent);//成品入库
                        RT.Service.Resolve<ITaskReport>().ToUpdateTaskReportState(barcodeEvent);// 更新报工记录状态
                    }  
                    else
                        RT.Service.Resolve<ITaskReport>().ToUpdateTaskReportQty(barcodeEvent);//更新报检记录的合格数和不合格数
                }
            });
        }

        /// <summary>
        /// 保存根据成品不合格审核更新成品报检审核结果日志
        /// </summary>
        /// <param name="billEvents">成品不合格审核提交后事件集合</param>
        private void SaveUpdateAuditResultLog(List<ShippingBillSubmittedEvent> billEvents)
        {
            using (var tran = DB.AutonomousTransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
            {
                var strValue = JsonConvert.SerializeObject(billEvents);
                var inputValue = "成品不合格审核提交后事件集合:{0}".L10nFormat(strValue);
                var log = new InterfaceLog()
                {
                    Name = "IProductInsp",
                    Method = "UpdateAuditResult",
                    ControllerName = _inspRecordControllerString,
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }
        #endregion

        #region 任务单成品报检

        /// <summary>
        /// 任务单成品报检
        /// </summary>
        /// <param name="inspEvent">成品报检事件</param>
        /// <returns>报检成功返回true，失败返回false</returns>
        public virtual bool GenerateTaskProductInsp(List<ProductInspEvent> inspEvent)
        {
            var paramter = GetProductInspParameter(inspEvent.First());
            if (paramter != null)
            {
                if (inspEvent.Any(p => p.BatchNo.IsNullOrEmpty()))
                {
                    throw new ValidationException("请输入批次号或维护批次号编码规则".L10N());
                }
                SaveTaskProductInsp(inspEvent, paramter);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取成品报检参数
        /// </summary>
        /// <param name="inspEvent"></param>
        /// <returns></returns>
        public virtual InspParameter GetProductInspParameter(ProductInspEvent inspEvent)
        {
            var ctl = RT.Service.Resolve<InspLogController>();
            var inspProcess = ctl.ConvertInspProcess(inspEvent.IsStartProcess, inspEvent.IsEndProcess);
            return ctl.GetInspParameter(inspEvent.ItemId, inspEvent.ProcessId, InspType.Product, inspProcess);
        }

        /// <summary>
        /// 保存任务单成品报检记录
        /// </summary>
        /// <param name="inspEventList">成品报检事件</param>
        /// <param name="paramter">报检参数</param>
        private void SaveTaskProductInsp(List<ProductInspEvent> inspEventList, InspParameter paramter)
        {
            var workOrderIds = inspEventList.Select(p => p.WorkOrderId).ToList();
            var inspRecordList = RT.Service.Resolve<InspRecordController>().GetInspRecordList(workOrderIds, InspType.Product);
            foreach (var inspEvent in inspEventList)
            {
                var inspRecord = inspRecordList.FirstOrDefault(p => p.WorkOrderId == inspEvent.WorkOrderId && p.ResourceId == inspEvent.ResourceId);
                if (inspRecord == null)
                {
                    inspRecord = new InspRecord();
                    inspRecord.WorkOrderId = inspEvent.WorkOrderId;
                    inspRecord.ResourceId = inspEvent.ResourceId;
                    inspRecord.PlanQty = inspRecord.WorkOrder.PlanQty;
                    inspRecord.InspectionQty = 0;
                    inspRecord.CustomerId = inspEvent.CustomerId;
                    inspRecord.ShopId = inspEvent.ShopId;
                    inspRecord.InspType = InspType.Product;
                }
                inspRecord.InspParameterId = paramter.Id;

                var inspBarcode = new InspBarcode();
                inspBarcode.Barcode = inspEvent.Barcode;
                inspBarcode.OperateById = RT.IdentityId;
                inspBarcode.ProcessId = inspEvent.ProcessId;
                inspBarcode.StationId = inspEvent.StationId;
                inspBarcode.InspDate = inspEvent.CollectionDate;
                inspBarcode.CollectionDate = inspEvent.CollectionDate;
                inspBarcode.BatchNo = inspEvent.BatchNo;
                inspBarcode.DispatchTaskId = inspEvent.DispatchTaskId;
                inspBarcode.ReportRecordId = inspEvent.ReportRecordId;
                inspBarcode.InspQty = inspEvent.OkQty;
                RT.Service.Resolve<InspRecordBaseController>().SetInspBarcode(inspBarcode, inspEvent);

                ////inspRecord.InspBarcodeList.Add(inspBarcode); 2023/3/27 csp 优化 inspBarcode.InspRecordId = inspRecord.Id;

                using (var tran = DB.TransactionScope(ProductIntfcEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(inspRecord);
                    inspBarcode.InspRecordId = inspRecord.Id;
                    RF.Save(inspBarcode);
                    tran.Complete();
                }

                //调用QMS首检接口进行报检，这里报检失败时不影响前面的执行结果
                var barcodeIds = inspRecord.InspBarcodeList.Select(p => p.Id).ToList();
                var messge = ExecuteProductInsp(barcodeIds);
                if (!messge.IsNullOrEmpty())
                {
                    throw new ValidationException(messge);
                }
            }
        }

        #endregion
    }
}