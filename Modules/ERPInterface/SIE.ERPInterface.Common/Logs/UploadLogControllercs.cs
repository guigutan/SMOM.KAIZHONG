using NPOI.SS.Formula.Functions;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.ERPInterface.Common.Enums;
using SIE.EventMessages.ErpCommon;
using SIE.EventMessages.ErpCommon.Datas;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.Items.Items;
using SIE.MES.TaskManagement.FeedingRecords;
using SIE.MES.TaskManagement.Reports;
using SIE.MES.WorkOrders;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NPOI.HSSF.Util.HSSFColor;

namespace SIE.ERPInterface.Common.Logs
{
    /// <summary>
    /// 事务上传控制器
    /// </summary>
    public class UploadLogControllercs : DomainController, IUploadLogControllercs
    {

        /// <summary>
        /// 更新余料称重记录的数量
        /// </summary>
        /// <param name="ids"></param>
        public virtual void EditScrapWeighingRecordQty(List<double> ids)
        {
            //获取事务上传
            var trans = ids.SplitContains(i =>
            {
                return Query<UploadTransaction>().Where(p => i.Contains((double)p.BillId) && (p.State == ProcessState.Retry || p.State == ProcessState.Failed || p.State == ProcessState.Unprocessed) && p.TransactionType == TransactionType.ScrapWeighing && p.OrderType == OrderType.Deduction).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            //获取余料称重记录
            var records = RT.Service.Resolve<FeedingRecordController>().GetScrapWeighingRecordsByIds(ids);

            foreach (var record in records)
            {
                var tran = trans.FirstOrDefault(p => p.BillId == record.Id);
                if (tran != null)
                {
                    tran.PersistenceStatus = PersistenceStatus.Modified;
                    tran.Quantity = record.DeductedQty.Value;
                }
            }
            if (trans.Count > 0)
                RF.Save(trans);
        }

        /// <summary>
        /// 更新扣料记录的数量
        /// </summary>
        /// <param name="ids"></param>
        public virtual void EditDeductionRecordQty(List<double> ids)
        {
            foreach (var id in ids)
            {
                //循环like，虽然慢，但是他们改动的数据量不会大，且拥有修改权限的人不多，极少，不影响
                //找出包含了这个Id的数据
                var trans = Query<UploadTransaction>().Where(p => p.TransactionType == TransactionType.Deduction && p.OrderType == OrderType.Deduction && p.SourceId != null && (p.State == ProcessState.Retry || p.State == ProcessState.Failed || p.State == ProcessState.Unprocessed) && p.SQL<bool>($"(T0.Source_Id = '{id}' OR T0.Source_Id LIKE '{id};%' OR T0.Source_Id LIKE '%;{id}' OR T0.Source_Id LIKE '%;{id};%')")).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                
                foreach (var tran in trans)
                {
                    //根据这个事务上传包含的Id
                    var sourceIds = tran.SourceId.Split(';').Select(p => Convert.ToDouble(p)).ToList();
                    //找出组成这条事务上传的扣料记录
                    var records = RT.Service.Resolve<FeedingRecordController>().GetDeductionRecordsByIds(sourceIds);
                    if (records.Count > 0)
                    {
                        //对数量重新进行计算
                        decimal qty = 0;
                        foreach (var record in records)
                        {
                            qty += record.EditQty ?? (record.DeductedQty ?? 0);
                        }
                        tran.Quantity = qty;
                    }
                }
                if (trans.Count > 0)
                    RF.Save(trans);
            }        
        }

        /// <summary>
        /// 更新发货确认事务上传
        /// </summary>
        /// <param name="zuid"></param>
        /// <param name="qty"></param>
        public void UpdateOutboundConfirmTransaction(string zuid, decimal qty)
        {
            DB.Update<UploadTransaction>().Set(p => p.Quantity, qty).Set(p => p.State, ProcessState.Retry).Where(p => p.OrderType == OrderType.OutboundConfirm && p.TransactionType == TransactionType.OutboundConfirm && p.Zuid == zuid).Execute();
        }

        /// <summary>
        /// 发货确认提交SAP
        /// </summary>
        /// <param name="datas"></param>
        public virtual void OutboundConfirmTransaction(List<OutboundConfirmTransactionData> datas)
        {
            EntityList<UploadTransaction> uploadTransactions = new EntityList<UploadTransaction>();

            foreach (var data in datas)
            {
                UploadTransaction uploadTransaction = new UploadTransaction();

                uploadTransaction.OrderType = OrderType.OutboundConfirm; ;
                uploadTransaction.TransactionId = null;
                uploadTransaction.TransactionType = TransactionType.OutboundConfirm;
                uploadTransaction.State = ProcessState.Unprocessed;

                uploadTransaction.TransactionDate = data.TransactionDate;
                uploadTransaction.Remark = string.Empty;
                uploadTransaction.FromOnhandState = null;
                uploadTransaction.BillId = data.BillId;
                //原因
                uploadTransaction.ReasonName = string.Empty;
                uploadTransaction.Zuid = data.Zuid; 

                uploadTransaction.Quantity = data.Quantity;

                uploadTransaction.UploadCount = 0;

                uploadTransactions.Add(uploadTransaction);
            }

            if (uploadTransactions.Count > 0)
                RF.Save(uploadTransactions);
        }

        /// <summary>
        /// 委外需求单收货提交报工
        /// </summary>
        /// <param name="datas"></param>
        public virtual void ProcessingInStockReportTransaction(List<ProcessingInStockReportTranData> datas)
        {
            EntityList<UploadTransaction> uploadTransactions = new EntityList<UploadTransaction>();
            foreach (var data in datas)
            {
                var uploadTransaction = new UploadTransaction();
                uploadTransaction.OrderType = OrderType.ProcessingInStockReport;
                uploadTransaction.TransactionId = null;
                uploadTransaction.TransactionType = TransactionType.ProcessingInStockReport;
                uploadTransaction.State = ProcessState.Unprocessed;

                uploadTransaction.TransactionDate = data.TransactionDate;
                uploadTransaction.WoId = data.WoId;
                uploadTransaction.Remark = string.Empty;
                uploadTransaction.FromOnhandState = null;
                uploadTransaction.BillLineId = data.BillLineId;
                uploadTransaction.BillId = data.BillId;
                uploadTransaction.BillNo = data.BillNo;
                //原因
                uploadTransaction.ReasonName = string.Empty;


                uploadTransaction.ItemId = data.ItemId;
                uploadTransaction.ItemCode = data.ItemCode;
                uploadTransaction.ItemName = data.ItemName;

                uploadTransaction.OrdKey = data.OrdKey;

                //工单
                uploadTransaction.WoNo = data.WoNo;
                uploadTransaction.Quantity = data.Quantity;
                uploadTransaction.WorkCenter = data.WorkCenter;
                uploadTransaction.WERKS = data.WERKS;
                uploadTransaction.Vornr = data.Vornr;

                uploadTransaction.ProcessCode = data.ProcessCode;
                uploadTransaction.NgQty = data.NgQty;
                uploadTransaction.OkQty = data.OkQty;
                uploadTransaction.ReworkQty = data.ReworkQty;
                uploadTransaction.SuspectQty = data.SuspectQty;

                uploadTransaction.UploadCount = 0;
                uploadTransactions.Add(uploadTransaction);
            }

            if (uploadTransactions.Count > 0)
                RF.Save(uploadTransactions);
        }

        /// <summary>
        /// 更新标签事务上传状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="msg"></param>
        public virtual void UpdateWipBatchCreateTransaction(double id, string msg)
        {
            if (msg.IsNullOrEmpty())
            {
                DB.Update<UploadTransaction>().Set(p => p.ProcessMessage, msg).Set(p => p.ValidateMessage, msg).Set(p => p.UploadCount, p => p.UploadCount + 1).Set(p => p.State, ProcessState.Processed).Where(p => id == p.BillId && p.TransactionType == TransactionType.OutsourcingSupWipBatch && p.OrderType == OrderType.OutsourcingSupWipBatch).Execute();
            }
            else
            {
                DB.Update<UploadTransaction>().Set(p => p.ProcessMessage, msg).Set(p => p.ValidateMessage, msg).Set(p => p.UploadCount, p => p.UploadCount + 1).Set(p => p.State, ProcessState.Retry).Where(p => id == p.BillId && p.TransactionType == TransactionType.OutsourcingSupWipBatch && p.OrderType == OrderType.OutsourcingSupWipBatch && p.UploadCount <= 5).Execute();
                DB.Update<UploadTransaction>().Set(p => p.ProcessMessage, msg).Set(p => p.ValidateMessage, msg).Set(p => p.UploadCount, p => p.UploadCount + 1).Set(p => p.State, ProcessState.Failed).Where(p => id == p.BillId && p.TransactionType == TransactionType.OutsourcingSupWipBatch && p.OrderType == OrderType.OutsourcingSupWipBatch && p.UploadCount > 5).Execute();
            }
        }

        /// <summary>
        /// 批次标签创建事务上传
        /// </summary>
        /// <param name="datas"></param>
        public virtual void SyncWipBatchCreateTransaction(List<SyncWipBatchData> datas)
        {
            EntityList<UploadTransaction> transactions = new EntityList<UploadTransaction>();
            foreach (var data in datas)
            {
                OrderType OrderType = OrderType.OutsourcingSupWipBatch;
                TransactionType TransactionType = TransactionType.OutsourcingSupWipBatch;
                UploadTransaction transaction = new UploadTransaction();
                transaction.BillId = data.Id;

                transaction.OrderType = OrderType;
                transaction.TransactionType = TransactionType;

                transaction.WoId = data.WorkOrderId;
                transaction.WoNo = data.WorkOrderNo;

                transaction.TransactionId = null;
                transaction.State = ProcessState.Unprocessed;

                transaction.TransactionDate = data.TransactionDate;
                transaction.Remark = string.Empty;
                transaction.FromOnhandState = null;
                //原因
                transaction.ReasonName = string.Empty;
                //transaction.OrdKey = "DeductionRecord_" + data.DetailId.ToString();

                transaction.Quantity = data.Qty;
                transaction.WorkCenter = string.Empty;
                transaction.WERKS = data.WERKS;

                transaction.LotCode = data.LotNo;
                //transaction.Vornr = deductionRecord.ReportRecord.Vornr;

                transaction.UploadCount = 0;

                transaction.PersistenceStatus = PersistenceStatus.New;
                transactions.Add(transaction);
            }

            if (transactions.Count > 0)
                RF.Save(transactions);
        }

        /// <summary>
        /// 委外需求单接口更新事务上传
        /// </summary>
        /// <param name="dtlIds"></param>
        /// <param name="msg"></param>
        /// <param name="type"></param>
        public virtual void RequestCreateTransaction(List<double> dtlIds, string msg, int type)
        {
            TransactionType? transactionType = null;
            OrderType? orderType = null;
            if (type == 1)
            {
                transactionType = TransactionType.Outsourcingouts;
                orderType = OrderType.Outsourcingouts;
            }
            else if (type == 3)
            {
                transactionType = TransactionType.OutsourcingReport;
                orderType = OrderType.OutsourcingReport;
            }
            else
            {
                transactionType = TransactionType.OutsourcingIns;
                orderType = OrderType.OutsourcingIns;
            }
            if (msg.IsNullOrEmpty())
            {
                DB.Update<UploadTransaction>().Set(p => p.ProcessMessage, msg).Set(p => p.ValidateMessage, msg).Set(p => p.UploadCount, p => p.UploadCount + 1).Set(p => p.State, ProcessState.Processed).Where(p => dtlIds.Contains((double)p.BillLineId) && p.TransactionType == transactionType && p.OrderType == orderType).Execute();
            }
            else
            {
                DB.Update<UploadTransaction>().Set(p => p.ProcessMessage, msg).Set(p => p.ValidateMessage, msg).Set(p => p.UploadCount, p => p.UploadCount + 1).Set(p => p.State, ProcessState.Retry).Where(p => dtlIds.Contains((double)p.BillLineId) && p.TransactionType == transactionType && p.OrderType == orderType && p.UploadCount <= 5).Execute();
                DB.Update<UploadTransaction>().Set(p => p.ProcessMessage, msg).Set(p => p.ValidateMessage, msg).Set(p => p.UploadCount, p => p.UploadCount + 1).Set(p => p.State, ProcessState.Failed).Where(p => dtlIds.Contains((double)p.BillLineId) && p.TransactionType == transactionType && p.OrderType == orderType && p.UploadCount > 5).Execute();
            }
        }

        /// <summary>
        /// 委外需求单创建事务上传
        /// </summary>
        public virtual void OutsourcingRequestCreateTransaction(List<OutReqCreateTransactionData> datas)
        {
            EntityList<UploadTransaction> transactions = new EntityList<UploadTransaction>();
            foreach (var data in datas)
            {
                OrderType OrderType = (data.Type == 1 ? Core.Enums.OrderType.Outsourcingouts : Core.Enums.OrderType.OutsourcingIns);
                TransactionType TransactionType = (data.Type == 1 ? TransactionType.Outsourcingouts : TransactionType.OutsourcingIns);
                if (data.Type == 3)
                {
                    OrderType = OrderType.OutsourcingReport;
                    TransactionType = TransactionType.OutsourcingReport;
                }
                UploadTransaction transaction = new UploadTransaction();
                transaction.BillNo = data.No;
                transaction.BillId = data.Id;
                transaction.BillLineId = data.DetailId;

                transaction.OrderType = OrderType;
                transaction.TransactionType = TransactionType;

                transaction.WoId = data.WorkOrderId;
                transaction.WoNo = data.WorkOrderNo;

                transaction.TransactionId = null;
                transaction.State = ProcessState.Unprocessed;

                transaction.TransactionDate = data.TransactionDate;
                transaction.Remark = string.Empty;
                transaction.FromOnhandState = null;
                //原因
                transaction.ReasonName = string.Empty;
                //transaction.OrdKey = "DeductionRecord_" + data.DetailId.ToString();

                transaction.Quantity = data.Qty;
                transaction.WorkCenter = string.Empty;
                transaction.WERKS = data.WERKS;

                transaction.LotCode = data.LotNo;
                //transaction.Vornr = deductionRecord.ReportRecord.Vornr;
                transaction.ProcessCode = data.ProcessCode;

                transaction.UploadCount = 0;

                transaction.PersistenceStatus = PersistenceStatus.New;
                transactions.Add(transaction);
            }

            if (transactions.Count > 0)
                RF.Save(transactions);

        }

        public virtual EntityList<UploadTransaction> GetUploadTransactions(UploadTransactionCriteria criteria)
        {
            var q = Query<UploadTransaction>();

            if (criteria.OrderType.HasValue)
            {
                q.Where(p => p.OrderType == criteria.OrderType);
            }
            if (criteria.TransactionCode.IsNotEmpty())
            {
                q.Where(p => p.TransactionCode.Contains(criteria.TransactionCode));
            }
            if (criteria.TransactionType.HasValue)
            {
                q.Where(p => p.TransactionType == criteria.TransactionType);
            }
            if (criteria.State.IsNotEmpty())
            {
                var stateList = new List<int>();
                criteria.State.Split(',').ForEach(s =>
                {
                    stateList.Add(int.Parse(s));
                });

                q.Where(p => (stateList.Contains((int)p.State)));

            }
            if (criteria.ItemCode.IsNotEmpty())
            {
                q.Where(p => p.ItemCode.Contains(criteria.ItemCode));
            }
            if (criteria.ItemName.IsNotEmpty())
            {
                q.Where(p => p.ItemName.Contains(criteria.ItemName));
            }
            if (criteria.LotCode.IsNotEmpty())
            {
                q.Where(p => p.LotCode.Contains(criteria.LotCode));
            }
            if (criteria.ProcessCode.IsNotEmpty())
            {
                q.Where(p => p.ProcessCode.Contains(criteria.ProcessCode));
            }
            if (criteria.Mblnr.IsNotEmpty())
            {
                q.Where(p => p.Mblnr.Contains(criteria.Mblnr));
            }
            if (criteria.Mjahr.IsNotEmpty())
            {
                q.Where(p => p.Mjahr.Contains(criteria.Mjahr));
            }
            if (criteria.ProcessMessage.IsNotEmpty())
            {
                q.Where(p => p.ProcessMessage.Contains(criteria.ProcessMessage));
            }
            if (criteria.CreateDate.BeginValue != null)
                q.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            if (criteria.CreateDate.EndValue != null)
                q.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            if (!criteria.WoNo.IsNullOrEmpty())
                q.Where(p => p.WoNo.Contains(criteria.WoNo));
            if (!criteria.Zuid.IsNullOrEmpty())
                q.Where(p => p.Zuid.Contains(criteria.Zuid));
            if (!criteria.ShortDescription.IsNullOrEmpty())
                q.Join<Item>((x, y) => x.ItemCode == y.Code && y.ShortDescription.Contains(criteria.ShortDescription));
            if (!criteria.Bismt.IsNullOrEmpty())
                q.Exists<ParentItem>((x, y) => y.Where(p => p.ItemId == x.ItemId && p.Bismt.Contains(criteria.Bismt)));
            if (criteria.WorkShopId != null)
                q.Join<WorkOrder>((x, y) => x.WoNo == y.No).Join<WorkOrder, Enterprise>((x, y) => x.WorkShopId == y.Id && y.Name == criteria.WorkShop.Name);
            if (!criteria.WorkShopCode.IsNullOrEmpty())
                q.Join<WorkOrder>((x, y) => x.WoNo == y.No).Join<WorkOrder, Enterprise>((x, y) => x.WorkShopId == y.Id && y.Code == criteria.WorkShopCode);
            //ExtensionWoCrieriaCondition(q, criteria);
            var list = q.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

            //赋值工单车间
            var woNos = list.Where(p=>!p.WoNo.IsNullOrEmpty()).Select(p => p.WoNo).Distinct().ToList();
            var wos = RT.Service.Resolve<WorkOrderController>().GetWorkOrders(woNos);
            foreach (var wo in wos)
            {
                foreach (var item in list.Where(p => p.WoNo == wo.No))
                {
                    item.WorkShopName = wo.WorkShopName;
                    item.WorkShopCode = wo.WorkShopCode;
                }
            }

            var itemIds = list.Where(p => p.ItemId != null).Select(p => p.ItemId.Value).Distinct().ToList();
            if (itemIds.Count > 0)
            {
                var items = RT.Service.Resolve<ItemController>().GetItemList(itemIds);
                var parentItems = RT.Service.Resolve<ItemController>().GetParentItemsByItemIds(itemIds);
                foreach (var l in list)
                {
                    var item = items.FirstOrDefault(p => p.Id == l.ItemId);
                    if (item != null)
                    {
                        l.ShortDescription = item.ShortDescription;
                    }
                    var parentItem = parentItems.FirstOrDefault(p => p.ItemId == l.ItemId);
                    if (parentItem != null)
                    {
                        l.Bismt = parentItem.Bismt;
                    }
                }
            }

            return list;
        }
    }
}
