using SIE.Common.NumberRules;
using SIE.Core.Items;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.LES;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace SIE.Barcodes.WipBatchs
{
    /// <summary>
    /// 生产批次控制器
    /// </summary>
    public partial class WipBatchController : DomainController
    {
        /// <summary>
        /// 根据包装任务单获取标签
        /// </summary>
        /// <param name="taskIds"></param>
        /// <returns></returns>
        public virtual EntityList<WipBatch> GetWipBatchesByPackingTaskIds(List<double> taskIds)
        {
            var list = taskIds.SplitContains(ids =>
            {
                return Query<WipBatch>().Where(p => ids.Contains((double)p.PackingTaskId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 计算标签类型
        /// </summary>
        /// <param name="wipBatch"></param>
        /// <returns></returns>
        public virtual string GetWipBatchType(WipBatch wipBatch)
        {
            var P_Type = wipBatch.IsSuspectProduct == YesNo.Yes ? "可疑品" : "";
            if (wipBatch.IsSuspectProduct == YesNo.No || wipBatch.IsSuspectProduct == null)
            {
                if (wipBatch.IsRework == true)
                    P_Type = "返工";
                else if (wipBatch.IsScraped == true)
                    P_Type = "报废";
                else
                    P_Type = "良品";
            }
            return P_Type;
        }

        /// <summary>
        /// 根据条码打印条件获取工单
        /// </summary>
        /// <param name="criteria">条码打印查询实体</param>
        /// <returns>工单列表</returns>
        public virtual EntityList<BatchWorkOrder> GetWorkOrders(BatchWorkOrderCriteria criteria)
        {
            var query = Query<BatchWorkOrder>();
            if (!criteria.No.IsNullOrWhiteSpace())
                query.Where(p => p.No.Contains(criteria.No));
            if (criteria.PlanBeginDate.BeginValue.HasValue)
                query.Where(p => p.PlanBeginDate >= criteria.PlanBeginDate.BeginValue);
            if (criteria.PlanBeginDate.EndValue.HasValue)
                query.Where(p => p.PlanEndDate < criteria.PlanBeginDate.EndValue.Value.AddDays(1));
            if (criteria.CreateDate.BeginValue.HasValue)
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue);
            if (criteria.CreateDate.EndValue.HasValue)
                query.Where(p => p.CreateDate < criteria.CreateDate.EndValue.Value.AddDays(1));
            if (criteria.CreateBy != null)
                query.Where(p => p.CreateBy == criteria.CreateById);
            if (criteria.ItemName.IsNotEmpty())
                query.Where(p => p.Product.Name.Contains(criteria.ItemName));
            if (criteria.ProductCode.IsNotEmpty())
                query.Where(p => p.Product.Code.Contains(criteria.ProductCode));

            if (criteria.BatchNo.IsNotEmpty())
            {
                query.Exists<WipBatch>((x, y) => y.Where(p => p.BatchNo.Contains(criteria.BatchNo) && p.WorkOrderId == x.Id));
            }
            query.Join<ItemBatchRule>((x, y) => x.ProductId == y.ItemId && y.RetrospectType == RetrospectType.Batch);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 设置合并规则方案缺省
        /// </summary>
        /// <param name="id">方案Id</param>
        /// <param name="flag">true/false</param>
        /// <returns>BatchMergeSolutions</returns>
        public virtual BatchMergeSolutions SetDefault(double id, bool flag)
        {
            if (id <= 0)
                throw new ArgumentNullException(nameof(id));
            var entity = RF.GetById<BatchMergeSolutions>(id);
            if (entity == null)
                throw new EntityNotFoundException(typeof(BatchMergeSolutions), id);
            entity.IsDefault = flag;
            RF.Save(entity);
            return entity;
        }

        /// <summary>
        /// 获取是缺省的合并规则方案
        /// </summary>
        /// <returns>合并规则方案</returns>
        public virtual BatchMergeSolutions GetDefaultSolution()
        {
            return Query<BatchMergeSolutions>().Where(p => p.IsDefault).FirstOrDefault();
        }

        /// <summary>
        /// 批次生成
        /// </summary>
        /// <param name="info">批次条码信息</param>
        /// <returns>工单</returns>
        public virtual Tuple<BatchWorkOrder, EntityList<WipBatch>> BatchsGenerating(BatchBarcodeInfo info)
        {
            var batchWo = RF.GetById<BatchWorkOrder>(info.WorkOrderId);
            if (batchWo == null)
                throw new EntityNotFoundException(typeof(WorkOrder), info.WorkOrderId);
            EntityList<WipBatch> barcodes = new EntityList<WipBatch>();
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                //规则明细
                var printQty = (int)Math.Ceiling(info.GeneratingQty / info.BatchQty);

                //创建并保存批次条码
                var batchBarcodes = SaveBatchBarcodes(info, batchWo, printQty);

                if (info.GenerateChild)
                {
                    barcodes.AddRange(SaveChildBarcodes(info, batchWo, printQty, batchBarcodes));
                }
                else
                {
                    barcodes.AddRange(batchBarcodes);
                }

                tran.Complete();
            }

            return new Tuple<BatchWorkOrder, EntityList<WipBatch>>(batchWo, barcodes);
        }

        /// <summary>
        /// 创建并保存批次条码
        /// </summary>
        /// <param name="info">批次条码信息</param>
        /// <param name="batchWo">批次工单</param>
        /// <param name="printQty">打印数量</param>
        /// <returns>批次列表</returns>
        public virtual EntityList<WipBatch> SaveBatchBarcodes(BatchBarcodeInfo info, BatchWorkOrder batchWo, int printQty, string processCode = null)
        {
            var barcodes = RT.Service.Resolve<NumberRuleController>().GenerateSegment(info.NumberRuleId, printQty, batchWo);

            if (batchWo.GeneratedQty + printQty > batchWo.PlanQty + batchWo.ScrapQty)
            {
                throw new Exception("已生成数量+本次生成数量大于工单计划数，生成失败！".L10N());
            }

            var range = new BarcodeRange()
            {
                PrintQty = printQty,
                StartSn = barcodes.FirstOrDefault(),
                EndSn = barcodes.LastOrDefault(),
                State = ReceiveState.NoReceive,
                WorkOrder = batchWo,
            };
            RF.Save(range);

            var barcodeList = new EntityList<WipBatch>();
            var now = RF.Find<WipBatch>().GetDbTime();

            decimal printedCount = info.GeneratingQty;
            foreach (var sn in barcodes)
            {
                bool isMantissa = false;
                decimal qty = info.BatchQty;
                if (printedCount < info.BatchQty)
                {
                    qty = printedCount;
                    isMantissa = true;
                }

                var barcode = new WipBatch()
                {
                    BatchNo = sn,
                    IsScraped = false,
                    Qty = qty,
                    BoxesQty = info.BatchQty,
                    IsMantissa = isMantissa,
                    WorkOrder = batchWo,
                    PrintDate = now,
                    BatchState = BatchState.Generated,
                    Range = range,
                    IsChild = false,
                    IsGenerateChild = info.GenerateChild,
                    IsGenerate = true,
                    EditQtyProcessCode = processCode
                };

                barcodeList.Add(barcode);

                printedCount -= info.BatchQty;
            }

            if (batchWo.GeneratedQty == null)
                batchWo.GeneratedQty = 0;
            batchWo.GeneratedQty += barcodeList.Sum(p => p.Qty);
            RF.Save(batchWo);
            RF.Save(barcodeList);

            return barcodeList;
        }

        /// <summary>
        /// 保存子批次条码
        /// </summary>
        /// <param name="info">批次条码信息</param>
        /// <param name="batchWo">批次工单</param>
        /// <param name="printQty">打印数量</param>
        /// <param name="batchBarcodes">批次条码列表</param>
        /// <returns>子批次列表</returns>
        private EntityList<SubWipBatch> SaveChildBarcodes(BatchBarcodeInfo info, BatchWorkOrder batchWo, int printQty, EntityList<WipBatch> batchBarcodes)
        {
            var batchQty = (int)Math.Ceiling(info.BatchQty / info.ChildBatchQty);
            var count = batchQty * printQty;
            var mantissaQty = (int)Math.Ceiling(info.GeneratingQty % info.BatchQty / info.ChildBatchQty);
            if (mantissaQty != 0)
            {
                count = (int)Math.Ceiling(info.BatchQty / info.ChildBatchQty) * (printQty - 1) + mantissaQty;
            }

            var childBarcodes = RT.Service.Resolve<NumberRuleController>().GenerateSegment(info.ChildNumberRuleId, count, batchWo);

            var range = new BarcodeRange()
            {
                PrintQty = count,
                StartSn = childBarcodes.FirstOrDefault(),
                EndSn = childBarcodes.LastOrDefault(),
                State = ReceiveState.NoReceive,
                WorkOrder = batchWo,
            };
            RF.Save(range);

            var barcodeList = new EntityList<SubWipBatch>();
            var now = RF.Find<WipBatch>().GetDbTime();

            int index = 0;
            foreach (var barcode in batchBarcodes)
            {
                bool isMantissa = false;
                decimal qty = info.ChildBatchQty;

                int childIndex = 1;
                childBarcodes.Skip(index * batchQty).Take(batchQty).ForEach(p =>
                {
                    if (childIndex * info.ChildBatchQty > barcode.Qty)
                    {
                        isMantissa = true;
                        if (barcode.Qty < info.ChildBatchQty)
                            qty = barcode.Qty;
                        else
                            qty = barcode.Qty % info.ChildBatchQty;
                    }

                    var childBarcode = new SubWipBatch()
                    {
                        BatchNo = p,
                        IsScraped = false,
                        Qty = qty,
                        BoxesQty = info.ChildBatchQty,
                        IsMantissa = isMantissa,
                        WorkOrder = batchWo,
                        PrintDate = now,
                        BatchState = BatchState.Generated,
                        Range = range,
                        WipBatch = barcode,
                        IsChild = true,
                        IsGenerate = true
                    };

                    barcodeList.Add(childBarcode);
                    childIndex++;
                });
                index++;
            }

            RF.Save(barcodeList);

            return barcodeList;
        }

        /// <summary>
        /// 保存已打印条码
        /// </summary>
        /// <param name="workOrder">批次工单</param>
        /// <param name="barcodes">已打印条码</param>
        /// <returns>处理批次工单</returns>
        public virtual BatchWorkOrder SavePrintedBarcodes(BatchWorkOrder workOrder, EntityList<WipBatch> barcodes)
        {
            if (!barcodes.Any())
            {
                throw new ValidationException("无需打印的批次条码，请检查参数".L10N());
            }
            if (workOrder == null)
            {
                throw new ValidationException("批次条码工单为空，请检查参数".L10N());
            }
            //重新取一次批次信息 避免信息已被更新但自身无变化
            barcodes = this.GetWipBatches(barcodes.Select(m => m.BatchNo).ToList());
            workOrder = RF.GetById<BatchWorkOrder>(workOrder.Id);

            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                if (barcodes.FirstOrDefault().GetType() == typeof(SubWipBatch))
                {
                    var batchIds = barcodes.OfType<SubWipBatch>().Select(p => p.WipBatchId).OfType<double>().ToList();
                    EntityList<WipBatch> batchBarcodes = GetWipBatchsByBatchIds<WipBatch>(batchIds);
                    batchBarcodes.ForEach(p =>
                    {
                        p.PrintTimes = 1;
                        p.PrintById = RT.IdentityId;
                        p.PrintedState = BarcodeState.Printed;
                    });
                    RF.Save(batchBarcodes);
                }
                barcodes.ForEach(p =>
                {
                    p.PrintTimes = 1;
                    p.PrintById = RT.IdentityId;
                    p.PrintedState = BarcodeState.Printed;
                });
                workOrder.PrintedQty += (int)barcodes.Sum(p => p.PrintTimes);

                RF.Save(workOrder);
                RF.Save(barcodes);
                tran.Complete();
            }

            return workOrder;
        }

        /// <summary>
        /// 打印失败时删除已生成条码
        /// </summary>
        /// <param name="workOrder">批次工单</param>
        /// <param name="barcodes">已打印条码</param>
        /// <returns>处理批次工单</returns>
        public virtual BatchWorkOrder RemoveBarcodesOnFailedPrint(BatchWorkOrder workOrder, EntityList<WipBatch> barcodes)
        {
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                if (barcodes.FirstOrDefault().GetType() == typeof(SubWipBatch))
                {
                    var batchIds = barcodes.OfType<SubWipBatch>().Select(p => p.WipBatchId).OfType<double>().ToList();
                    EntityList<WipBatch> batchBarcodes = GetWipBatchsByBatchIds<WipBatch>(batchIds);

                    batchBarcodes.ForEach(p =>
                    {
                        p.PersistenceStatus = PersistenceStatus.Deleted;
                    });

                    RF.Save(batchBarcodes);
                }

                barcodes.ForEach(p =>
                {
                    p.PersistenceStatus = PersistenceStatus.Deleted;
                });

                workOrder.GeneratedQty -= barcodes.Sum(p => p.Qty);
                RF.Save(workOrder);
                RF.Save(barcodes);
                tran.Complete();
            }

            return workOrder;
        }

        /// <summary>
        /// 验证批次条码是否可入站
        /// </summary>
        /// <param name="batchNo">条码</param>
        public virtual void ValidateBatchBarcode(string batchNo)
        {
            var barcode = GetWipBatch(batchNo);
            if (barcode == null)
                throw new ValidationException("条码不存在".L10N());
            if (barcode.BatchState == BatchState.In)
                throw new ValidationException("条码[{0}]已入站".L10nFormat(batchNo));
        }

        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <returns>条码</returns>
        public virtual WipBatch GetWipBatch(string batchNo)
        {
            return Query<WipBatch>().Where(p => p.BatchNo == batchNo).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="batchNos">批次号</param>
        /// <returns>条码</returns>
        public virtual EntityList<SubWipBatch> GetWipBatchs(List<string> batchNos)
        {
            return batchNos.SplitContains(cds =>
            {
                return Query<SubWipBatch>().Where(p => cds.Contains(p.BatchNo)).ToList();
            });
        }

        /// <summary>
        /// 获取批次条码
        /// </summary>
        /// <param name="batchNos">批次条码信息</param>
        /// <returns>批次条码</returns>
        public virtual EntityList<WipBatch> GetWipBatches(List<string> batchNos)
        {
            return batchNos.SplitContains(temp =>
            {
                return Query<WipBatch>().Where(p => temp.Contains(p.BatchNo)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });     
        }

        /// <summary>
        /// 根据Id获取批次条码
        /// </summary>
        /// <param name="Ids"></param>
        /// <returns></returns>
        public virtual EntityList<WipBatch> GetWipBatches(List<double> Ids)
        {
            var list = Ids.SplitContains(items =>
            {
                return Query<WipBatch>().Where(p => items.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 更新批次条码批次状态
        /// </summary>
        /// <param name="sn">批次条码</param>
        /// <param name="batchState">批次状态</param>
        /// <returns>更新结果</returns>
        public virtual int UpdateBatchBarcode(string sn, BatchState batchState)
        {
            return DB.Update<WipBatch>().Set(p => p.BatchState, batchState).Where(p => p.BatchNo == sn).Execute();
        }

        /// <summary>
        /// 获取子批次条码对应的中批次条码号
        /// </summary>
        /// <param name="sn">子批次号</param>
        /// <returns>中批次条码号</returns>
        public virtual string GetMidBarcode(string sn)
        {
            var barcode = GetWipBatch(sn);
            if (barcode == null)
                throw new ValidationException("批次条码[{0}]不存在".L10nFormat(sn));
            return barcode.IsChild ? (barcode as SubWipBatch)?.WipBatch.BatchNo : sn;
        }

        /// <summary>
        /// 根据工单ID获取条码消息
        /// </summary>
        /// <param name="workorderId">工单ID</param>
        /// <param name="isChild">是否子批次</param>
        /// <param name="sortInfo">排序</param>
        /// <param name="pagingInfo">分页信息</param>
        /// <returns>条码信息</returns>
        public virtual EntityList<WipBatch> GetWipBatchsByWorkOrder(double workorderId, bool? isChild = false, List<OrderInfo> sortInfo = null, PagingInfo pagingInfo = null)
        {
            var query = Query<WipBatch>().Where(p => p.WorkOrderId == workorderId);
            //if (isChild.HasValue)
            //    query.Where(p => p.IsChild == isChild.Value);
            return query.OrderBy(sortInfo).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 计算工单已生成条码数
        /// </summary>
        /// <param name="workorderId">工单ID</param>
        /// <returns>条码信息</returns>
        public virtual decimal CountGenerateBarcode(double workorderId)
        {
            return GetWipBatchsByWorkOrder(workorderId).Sum(p => p.Qty);
        }

        /// <summary>
        /// 根据生产批次获取子批次
        /// </summary>
        /// <param name="batchId">批次ID</param>
        /// <returns>子批次列表</returns>
        public virtual EntityList<SubWipBatch> GetSubWipBatchsByBatchId(double batchId)
        {
            var query = Query<SubWipBatch>().Where(p => p.WipBatchId == batchId);
            return query.ToList();
        }

        /// <summary>
        /// 根据生产批次Id集合获取批次列表
        /// </summary>
        /// <param name="batchIds">批次ID列表</param>
        /// <returns>子批次列表</returns>
        public virtual EntityList<T> GetWipBatchsByBatchIds<T>(List<double> batchIds, EagerLoadOptions eagerLoad = null) where T : DataEntity
        {
            var ids = batchIds.Distinct().ToList();
            var batchList = new EntityList<T>();
            var count = ids.Count / 1000M;

            for (int i = 0; i < count; i++)
            {
                var query = Query<T>().Where(p => ids.Skip(i * 1000).Take(1000).Contains(p.Id));
                batchList.AddRange(query.ToList(null, eagerLoad));
            }

            return batchList;
        }

        /// <summary>
        /// 获取批次生成的子批次
        /// </summary>
        /// <param name="batchId">批次ID</param>
        /// <param name="orderInfos">排序条件</param>
        /// <param name="pagingInfo">分页条件</param>
        /// <returns>子批次列表</returns>
        public virtual EntityList<SubWipBatch> GetGenerateSubWipBatches(double batchId, IList<OrderInfo> orderInfos, PagingInfo pagingInfo)
        {
            return Query<SubWipBatch>()
                .Where(p => p.WipBatchId == batchId && p.IsGenerate)
                .OrderBy(orderInfos)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 补打生产批次
        /// </summary>
        /// <param name="batchIds">批次ID列表</param>
        /// <returns>生产批次列表</returns>
        public virtual EntityList<WipBatch> ReprintWipBatch(List<double> batchIds)
        {
            var wipBatchs = GetWipBatchsByBatchIds<WipBatch>(batchIds);
            if (wipBatchs.Count == 0)
                throw new ValidationException("异常,找不到生产批次数据".L10N());
            var batchWo = RF.GetById<BatchWorkOrder>(wipBatchs.FirstOrDefault().WorkOrderId);
            if (batchWo == null)
                throw new EntityNotFoundException(typeof(WorkOrder), wipBatchs.FirstOrDefault().WorkOrderId);
            var now = RF.Find<WipBatch>().GetDbTime();
            foreach (var wipBatch in wipBatchs)
            {
                wipBatch.PrintTimes++;
                if (wipBatch.PrintedState == BarcodeState.Notprint && !wipBatch.IsGenerateChild)
                {
                    batchWo.PrintedQty += wipBatch.PrintTimes;
                    wipBatch.PrintedState = BarcodeState.Printed;
                }
                wipBatch.PrintById = RT.IdentityId;
                wipBatch.PrintDate = now;
            }
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                RF.Save(batchWo);
                RF.Save(wipBatchs);
                tran.Complete();
            }
            return wipBatchs;
        }

        /// <summary>
        /// 补打子生产批次
        /// </summary>
        /// <param name="subBatchIds">子批次ID列表</param>
        /// <returns>子生产批次列表</returns>
        public virtual EntityList<SubWipBatch> ReprintSubWipBatch(List<double> subBatchIds)
        {
            var subWipBatchs = GetWipBatchsByBatchIds<SubWipBatch>(subBatchIds);
            if (subWipBatchs.Count == 0)
                throw new ValidationException("异常,找不到子生产批次数据".L10N());
            var batchWo = RF.GetById<BatchWorkOrder>(subWipBatchs.FirstOrDefault().WorkOrderId);
            if (batchWo == null)
                throw new EntityNotFoundException(typeof(WorkOrder), subWipBatchs.FirstOrDefault().WorkOrderId);
            var wipBatch = RF.GetById<WipBatch>(subWipBatchs.FirstOrDefault().WipBatchId);
            Check.NotNull(wipBatch, nameof(WipBatch));
            var now = RF.Find<SubWipBatch>().GetDbTime();
            wipBatch.PrintedState = BarcodeState.Printed;
            wipBatch.PrintTimes++;
            wipBatch.PrintById = RT.IdentityId;
            wipBatch.PrintDate = now;
            foreach (var subWipBatch in subWipBatchs)
            {
                subWipBatch.PrintTimes++;
                if (subWipBatch.PrintedState == BarcodeState.Notprint)
                {
                    batchWo.PrintedQty += subWipBatch.PrintTimes;
                    subWipBatch.PrintedState = BarcodeState.Printed;
                }

                subWipBatch.PrintById = RT.IdentityId;
                subWipBatch.PrintDate = now;
            }
            using (var tran = DB.TransactionScope(BarcodeEntityDataProvider.ConnectionStringName))
            {
                RF.Save(batchWo);
                RF.Save(wipBatch);
                RF.Save(subWipBatchs);
                tran.Complete();
            }
            return subWipBatchs;
        }

        /// <summary>
        /// 拆分批准标签
        /// </summary>
        /// <param name="source"></param>
        /// <param name="qty">拆分数量</param>
        /// <returns></returns>
        public virtual WipBatch CreateSplitWipBatch(WipBatch source, decimal qty, string processCode = null)
        {
            if (qty > source.Qty)
                throw new ValidationException("标签拆分数量不能大于标签原有数量");
            var splitWipBatch = new WipBatch();
            splitWipBatch.Clone(source);
            splitWipBatch.Qty = qty;
            splitWipBatch.ScrapQty = 0;
            if (source.IsScraped)
                splitWipBatch.ScrapQty = qty;
            splitWipBatch.SourceNo = source.BatchNo;
            var splitCount = GetSplitWipBatchCount(source.BatchNo);
            splitWipBatch.BatchNo = $"{source.BatchNo}-{splitCount + 1}";
            splitWipBatch.EditQtyProcessCode = processCode;
            RF.Save(splitWipBatch);
            return splitWipBatch;
        }

        /// <summary>
        /// 获取批次标签拆分个数
        /// </summary>
        /// <param name="sourceNo"></param>
        /// <returns></returns>
        public virtual int GetSplitWipBatchCount(string sourceNo)
        {
            return Query<WipBatch>().Where(p => p.SourceNo == sourceNo).Count();
        }

        /// <summary>
        /// 获取条码
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <returns>条码</returns>
        public virtual WipBatch GetWipBatchPc(string batchNo)
        {
            //if (Query<WipBatch>().Where(p => p.BatchNo == batchNo) == null)
            //    return null;
            //else
                return Query<WipBatch>().Where(p => p.BatchNo == batchNo).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次子表 数量
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual decimal GetWipBatchSumBatchQty(double woId)
        {
            return Query<WipBatch>().Where(p => p.WorkOrderId == woId).ToList().Sum(p => p.Qty);
        }

        public virtual decimal GetWoQty(double woid)
        {
            return Query<WorkOrder>().Where(p => p.Id == woid).FirstOrDefault().PlanQty;
        }

        /// <summary>
        /// 获取工单
        /// </summary>
        /// <param name="WoId"></param>
        /// <returns></returns>
        public virtual WorkOrder GetWorkOrder(double? woId)
        {
            return Query<WorkOrder>().Where(p => p.Id == woId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取产品
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns></returns>
        public virtual Item GetItem(double itemId)
        {
            return Query<Item>().Where(p => p.Id == itemId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 新增批次标签
        /// </summary>
        /// <param name="wipBatch"></param>
        /// <returns></returns>
        public virtual bool SaveWipBatch(WipBatch wipBatch)
        {
            try
            {
                RF.Save(wipBatch);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 包装报工拆分查询
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual WipBatch GetWipBatchReport(string name)
        {
            return Query<WipBatch>().Where(p => p.BatchNo == name).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

        }
    }
}