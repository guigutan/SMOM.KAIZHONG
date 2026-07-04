using Microsoft.Scripting.Utils;
using SIE.Barcodes;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.InvOrg;
using SIE.Core.Barcodes;
using SIE.Data;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.WIP;
using SIE.MES.WorkOrders;
using SIE.Tech.Routings;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.BatchWIP
{
    /// <summary>
    /// 批次管理控制器
    /// </summary>
    public class BatchManageController : DomainController
    {
        /// <summary>
        /// 获取生产批次列表
        /// </summary>
        /// <param name="criteria">生产批次查询实体</param>
        /// <returns>生产批次列表</returns>
        public virtual EntityList<WipBatch> GetWipBatches(WipBatchCriteria criteria)
        {
            var query = Query<WipBatch>().Where(p => p.IsGenerate && p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Finish && p.WorkOrder.State != Core.WorkOrders.WorkOrderState.Close && p.BatchState != BatchState.In && p.BatchState != BatchState.Out);
            ////取所有没有生成子批次的批次条码&&生成子批次的所有子批次（不包括其生产批次）
            query.Where(p => (!p.IsGenerateChild && !p.IsChild) || p.IsChild);
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                query.Join<WorkOrder>((x, y) => x.WorkOrderId == y.Id && y.No.Contains(criteria.WorkOrderNo));
            }

            if (criteria.ProcessId > 0)
            {
                query.Join<WorkOrderRoutingProcess>((w, p) => p.WorkOrderId == w.WorkOrderId && p.Sign == RoutingProcessSign.Start && p.ProcessId == criteria.ProcessId);
            }

            if (!criteria.BatchNo.IsNullOrEmpty())
            {
                query.Where(p => p.BatchNo.Contains(criteria.BatchNo));
            }

            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 查询批次列表
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>

        public virtual EntityList<SubWipBatch> CriteriaGetBatches(BatchCriteria criteria)
        {
            var qurey = Query<SubWipBatch>();
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                qurey.Join<WorkOrder>((X, Y) => X.WorkOrderId == Y.Id && Y.No.Contains(criteria.WorkOrderNo));
            }
            if (!criteria.WipBatchNo.IsNullOrEmpty())
            {
                qurey.Where(p => p.BatchNo.Contains(criteria.WipBatchNo));

            }
            if (!criteria.BatchNo.IsNullOrEmpty())
            {
                qurey.Where(p => p.BatchNo.Contains(criteria.BatchNo));
            }
            qurey.Exists<BatchWipProductVersion>((x, y) => y.Where(p => x.BatchNo == p.BatchNo));
            var results = qurey.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            SetWipBatchsRemainQty(results);
            return results;

        }



        /// <summary>
        /// 获取批次列表 
        /// </summary>
        /// <param name="criteria">批次产品查询实体</param>
        /// <returns>批次列表</returns>
        public virtual EntityList<SubWipBatch> GetBatches(BatchCriteria criteria)
        {
            var wipBatch = RF.Find<SubWipBatch>().EntityMeta;
            var wipBatchTable = wipBatch.TableMeta.TableName;
            var id = wipBatch.Property(WipBatch.IdProperty).ColumnMeta.ColumnName;
            var isGenerate = wipBatch.Property(WipBatch.IsGenerateProperty).ColumnMeta.ColumnName;
            var isChild = wipBatch.Property(WipBatch.IsChildProperty).ColumnMeta.ColumnName;
            var workOrderId = wipBatch.Property(WipBatch.WorkOrderIdProperty).ColumnMeta.ColumnName;
            var wipBatchId = wipBatch.Property(SubWipBatch.WipBatchIdProperty).ColumnMeta.ColumnName;
            var batchNo = wipBatch.Property(SubWipBatch.BatchNoProperty).ColumnMeta.ColumnName;
            var workOrder = RF.Find<WorkOrder>().EntityMeta;
            var workOrderTable = workOrder.TableMeta.TableName;
            var no = workOrder.Property(WorkOrder.NoProperty).ColumnMeta.ColumnName;
            var invOrg = workOrder.Property(InvOrgIdExtension.INV_ORG_IDProperty).ColumnMeta.ColumnName;
            var isPhantom = workOrder.Property(PhantomEntityExtension.IS_PHANTOMProperty).ColumnMeta.ColumnName;
            ////查询子批次
            StringBuilder subBuilder = new StringBuilder();
            subBuilder.Append("select b.{0} from {1} b".FormatArgs(id, wipBatchTable));
            if (!criteria.WorkOrderNo.IsNullOrEmpty())
            {
                subBuilder.Append(" inner join {0} wo on b.{1}=wo.{2} and wo.{3}='{4}' and wo.{5}={6} and wo.{7}=0".FormatArgs(workOrderTable, workOrderId, id, no, criteria.WorkOrderNo, invOrg, RT.InvOrg, isPhantom));
            }

            if (!criteria.WipBatchNo.IsNullOrEmpty())
            {
                subBuilder.Append(" inner join {0} b1 on b.{1}=b1.{2} and b1.{3}='{4}' and b1.{5}={6} and b1.{7}=0".FormatArgs(wipBatchTable, wipBatchId, id, batchNo, criteria.WipBatchNo, invOrg, RT.InvOrg, isPhantom));
            }

            subBuilder.Append(" where b.{0}=1 and b.{1}={2} and b.{3}=0".FormatArgs(isChild, invOrg, RT.InvOrg, isPhantom));
            if (!criteria.BatchNo.IsNullOrEmpty())
            {
                subBuilder.Append(" and b.{0}='{1}'".FormatArgs(batchNo, criteria.BatchNo));
            }

            string sql = "({0})".FormatArgs(subBuilder.ToString());
            ////查询生产批次，有子批次号查询的时候不查询生产批次
            if (criteria.BatchNo.IsNullOrEmpty())
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("select b.{0} from {1} b".FormatArgs(id, wipBatchTable));
                if (!criteria.WorkOrderNo.IsNullOrEmpty())
                {
                    builder.Append(" inner join {0} wo on b.{1}=wo.{2} and wo.{3}='{4}' and wo.{5}={6} and wo.{7}=0".FormatArgs(workOrderTable, workOrderId, id, no, criteria.WorkOrderNo, invOrg, RT.InvOrg, isPhantom));
                }

                builder.Append(" where b.{0}=1 and b.{1}=0 and b.{2}={3} and b.{4}=0".FormatArgs(isGenerate, isChild, invOrg, RT.InvOrg, isPhantom));
                if (!criteria.WipBatchNo.IsNullOrEmpty())
                {
                    builder.Append(" and b.{0}='{1}'".FormatArgs(batchNo, criteria.WipBatchNo));
                }

                sql = "({0} union all {1})".FormatArgs(builder.ToString(), subBuilder.ToString());
            }

            var query = DB.Query<SubWipBatch>("sub").Where(x => !x.IsGenerateChild);
            query.Where(p => p.SQL<bool>(new FormattedSql(" exists(select * from {0} s where s.{1}=sub.{2})".FormatArgs(sql, id, id))));
            ////过滤已将被拆分完的批次   

#pragma warning disable CS1587 // XML 注释没有放在有效语言元素上
            ///暂时不需要过滤剩余数量大于0  modify by 钟振权 20191107
            query.Join<BatchRelation>((x, y) => x.BatchNo == y.Bid);
#pragma warning restore CS1587 // XML 注释没有放在有效语言元素上
            var totalCount = query.Distinct().Count();
            var results = query.Distinct().ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            results.SetTotalCount(totalCount);
            SetWipBatchsRemainQty(results);
            return results;
        }

        /// <summary>
        /// 处理生成批次的剩余数量属性值
        /// </summary>
        /// <param name="wipBatchs">生成批次</param>
        private void SetWipBatchsRemainQty(EntityList<SubWipBatch> wipBatchs)
        {
            foreach (var curWipBatch in wipBatchs)
            {
                var subWipBatchs = wipBatchs.Where(x => x.WipBatchId == curWipBatch.Id).ToList();
                if (subWipBatchs != null && subWipBatchs.Count > 0)
                {
                    curWipBatch.RemainQty = curWipBatch.Qty - subWipBatchs.Sum(x => x.Qty);
                }
                else
                {
                    curWipBatch.RemainQty = curWipBatch.Qty;
                }
            }
        }

        /// <summary>
        /// 转入批次条码信息
        /// </summary> 
        /// <param name="relation">批次关联</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="inputType">转入条码类型</param>
        /// <param name="defects">缺陷</param>
        /// <param name="ngQty">不良数量</param>
        /// <returns>转入批次</returns>
        internal virtual InputBatch InputBatch(BatchRelation relation, Workcell workcell, double workOrderId, BarcodeType? inputType, EntityList<Defect> defects = null, decimal? ngQty = null)
        {
            var batchNo = relation.Bid;
            var batchBarcode = RT.Service.Resolve<WipBatchController>().GetWipBatch(batchNo);
            if (batchBarcode == null)
            {
                throw new ValidationException("批次条码[{0}]不存在".L10nFormat(batchNo));
            }

            batchBarcode.BatchState = BatchState.In;
            WipBatch midBatch = null;
            if (batchBarcode.IsChild)
            {
                var childBarcode = batchBarcode as SubWipBatch;
                midBatch = childBarcode.WipBatch;
            }
            ////工单批次则子批次号为空
            var inputBatch = new InputBatch()
            {
                BatchNo = midBatch == null ? batchBarcode.BatchNo : midBatch.BatchNo,
                SubBatchNo = batchBarcode.IsChild ? batchNo : string.Empty,
                BatchState = BatchState.In,
                InputDate = DateTime.Now,
                ProcessId = workcell.ProcessId,
                StationId = workcell.StationId,
                ResourceId = workcell.ResourceId,
                Qty = relation.Qty,
                NgQty = ngQty != null ? (decimal)ngQty : 0,
                RemainQty = relation.RemainQty,
                ContainerNo = relation.ContainerNo,
                WorkOrderId = workOrderId,
                WipBatchId = midBatch == null ? batchBarcode.Id : midBatch.Id,
                IsChild = batchBarcode.IsChild,
                InputType = inputType
            };
            EntityList<InputBatchDefect> inputBatchDefects = new EntityList<InputBatchDefect>();
            if (defects != null)
            {
                foreach (var defect in defects)
                {
                    InputBatchDefect inputBatchDefect = new InputBatchDefect
                    {
                        InputBatch = inputBatch,
                        DefectId = defect.Id,
                        DefectDesc = defect.Description,
                    };
                    inputBatchDefects.Add(inputBatchDefect);
                }
            }
            RF.Save(inputBatch);
            RF.Save(batchBarcode);
            if (inputBatchDefects.Count > 0)
            {
                inputBatchDefects.ForEach(p =>
                {
                    p.InputBatchId = p.InputBatch.Id;
                });
                RF.Save(inputBatchDefects);
            }
            return inputBatch;
        }

        ///// <summary>
        ///// 验证转入批次信息
        ///// </summary>
        ///// <param name="barcode">批次条码</param>
        ///// <param name="isGenerateChild">是否生产子批次</param>
        ////void ValidationInputBatch(string barcode, bool isGenerateChild)
        ////{
        ////    var inputBatch = GetInputBatch(barcode, isGenerateChild);
        ////    if (inputBatch != null)
        ////        throw new ValidationException("[批次条码：{0}] 已在{1}工位入站，请先移除再转入".L10nFormat(barcode, inputBatch.Station.Name));
        ////}

        /// <summary>
        /// 获取转入批次信息
        /// </summary>
        /// <param name="barcode">批次条码</param>
        /// <param name="isGenerateChild">是否生成子批次</param>
        /// <returns>转入批次信息</returns>
        public virtual InputBatch GetInputBatch(string barcode, bool isGenerateChild)
        {
            ////var query = Query<InputBatch>().Where(p => p.BatchState == BatchState.In);
            var query = Query<InputBatch>().Where(p => p.BatchState == BatchState.In && p.IsChild == isGenerateChild);
            if (isGenerateChild)
            {
                query.Where(p => p.SubBatchNo == barcode);
            }
            else
            {
                query.Where(p => p.BatchNo == barcode);
            }

            return query.OrderByDescending(p => p.CreateDate).FirstOrDefault();
        }

        /// <summary>
        /// 获取工单工位转入批次列表
        /// </summary>
        /// <param name="wipResourceId">资源ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>转入批次列表</returns>
        public virtual EntityList<InputBatch> GetInputBatchs(double wipResourceId, double processId, double stationId, double workOrderId)
        {
            return Query<InputBatch>().Where(p => p.WorkOrderId == workOrderId && p.ResourceId == wipResourceId && p.ProcessId == processId && p.StationId == stationId && p.BatchState == BatchState.In && p.RemainQty > 0).OrderBy(p => p.CreateDate).ToList(null, new EagerLoadOptions().LoadWith(SIE.MES.BatchWIP.InputBatch.DefectListProperty).LoadWithViewProperty());
        }

        /// <summary>
        /// 获取当前工序工位在制工单
        /// </summary>
        /// <param name="wipResourceId">资源ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <returns>转入批次列表</returns>
        public virtual EntityList<BatchWorkOrder> GetInputBatchs(double wipResourceId, double processId, double stationId)
        {
            return Query<BatchWorkOrder>().Join<InputBatch>((x, p) =>
                x.Id == p.WorkOrderId &&
                p.ResourceId == wipResourceId &&
                p.ProcessId == processId &&
                p.StationId == stationId &&
                p.BatchState == BatchState.In &&
                p.RemainQty > 0).Distinct().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }


        /// <summary>
        /// 获取工单工位转入批次列表(包含非入站状态以及剩余数量为0的)
        /// </summary>
        /// <param name="wipResourceId">资源ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>转入批次列表</returns>
        public virtual EntityList<InputBatch> GetCurrentInputBatchs(double wipResourceId, double processId, double stationId, double workOrderId)
        {
            return Query<InputBatch>().Where(p => p.WorkOrderId == workOrderId && p.ResourceId == wipResourceId && p.ProcessId == processId && p.StationId == stationId).OrderBy(p => p.CreateDate).ToList();
        }
        /// <summary>
        /// 获取批次信息
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <returns>返回批次信息，不存在返回null</returns>
        /// <exception cref="ArgumentNullException">采集条码为空</exception>
        /// <exception cref="ValidationException">载具未关联批次条码</exception>
        public virtual BatchRelation GetBatchRelation(CollectBarcode barcode)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            BatchRelation relation = null;
            var query = Query<BatchRelation>();
            if (barcode.Type == BarcodeType.ContainerNo)
            {
                relation = query.Where(p => p.ContainerNo == barcode.Code).FirstOrDefault();
                if (relation == null)
                {
                    throw new ValidationException("载具[{0}]未关联批次条码".L10nFormat(barcode.Code));
                }
            }
            else if (barcode.Type == BarcodeType.BatchBarocde)
            {
                relation = query.Where(p => p.Bid == barcode.Code).FirstOrDefault();
            }
            else
            {
                //
            }

            return relation;
        }

        /// <summary>
        /// 获取批次信息
        /// </summary>
        /// <param name="barcodes">采集条码</param>
        /// <returns>返回批次信息，不存在返回null</returns>
        /// <exception cref="ArgumentNullException">采集条码为空</exception>
        /// <exception cref="ValidationException">载具未关联批次条码</exception>
        public virtual EntityList<BatchRelation> GetBatchRelations(List<CollectBarcode> barcodes)
        {
            EntityList<BatchRelation> batchRelations = new EntityList<BatchRelation>();
            if (!barcodes.Any())
            {
                return batchRelations;
            }
            if (barcodes.Any(p => p.Type != barcodes[0].Type))
            {
                throw new ValidationException("条码过站类型不一致！".L10N());
            }
            var query = Query<BatchRelation>();
            List<string> codes = barcodes.Select(p => p.Code).ToList();
            if (barcodes[0].Type == BarcodeType.ContainerNo)
            {
                codes.SplitDataExecute(cds =>
                {
                    var q = query.Where(p => cds.Contains(p.ContainerNo)).ToList();
                    if (!q.Any())
                    {
                        throw new ValidationException("存在载具未关联批次条码".L10N());
                    }
                    batchRelations.AddRange(q);
                });
            }
            else if (barcodes[0].Type == BarcodeType.BatchBarocde)
            {
                codes.SplitDataExecute(cds =>
                {
                    batchRelations.AddRange(query.Where(p => cds.Contains(p.Bid)).ToList());
                });
            }
            return batchRelations;
        }

        /// <summary>
        /// 获取批次信息列表
        /// </summary>
        /// <param name="code">批次号或者载具号</param>
        /// <param name="type">类型</param>
        /// <returns>批次信息列表</returns>
        /// <exception cref="ArgumentNullException">批次号或者载具号为空</exception>
        /// <exception cref="ValidationException">载具未关联批次条码</exception>
        public virtual BatchRelation GetBatchRelation(string code, BarcodeType type)
        {
            var query = Query<BatchRelation>();
            if (type == BarcodeType.ContainerNo)
            {
                query.Where(p => p.ContainerNo == code);
            }
            else if (type == BarcodeType.BatchBarocde)
            {
                query.Where(p => p.Bid == code);
            }
            else
            {
                //
            }

            return query.FirstOrDefault();
        }

        /// <summary>
        /// 获取批次信息列表
        /// </summary>
        /// <param name="code">批次号或者载具号</param>
        /// <param name="type">类型</param>
        /// <returns>批次信息列表</returns>
        /// <exception cref="ArgumentNullException">批次号或者载具号为空</exception>
        /// <exception cref="ValidationException">载具未关联批次条码</exception>
        public virtual EntityList<BatchRelation> GetBatchRelations(string code, BarcodeType type)
        {
            Check.NotNullOrEmpty(code, nameof(code));
            var query = Query<BatchRelation>();
            if (type == BarcodeType.ContainerNo)
            {
                query.Where(p => p.ContainerNo == code);
            }
            else if (type == BarcodeType.BatchBarocde)
            {
                query.Where(p => p.Bid == code);
            }
            else
            {
                //
            }

            return query.ToList();
        }

        /// <summary>
        /// 获取子批次信息列表
        /// </summary>
        /// <param name="pidCode">Pid编号</param>
        /// <returns>获取子批次</returns>
        public virtual EntityList<BatchRelation> GetBatchRelations(string pidCode)
        {
            Check.NotNullOrEmpty(pidCode, nameof(pidCode));
            var querys = Query<BatchRelation>().Where(p => p.Pid == pidCode);

            return querys.ToList();
        }

        /// <summary>
        /// 根据id获取生产批次产品
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<BatchWipProduct> GetBatchWipProducts(List<double> ids)
        {
            return ids.SplitContains(tempIds =>
            {
                return Query<BatchWipProduct>().Where(p => ids.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 判断生产批是否暂停
        /// </summary>
        /// <param name="code">批次号或者载具号</param>
        /// <param name="type">类型</param>
        /// <returns>暂停：true，否则：false</returns>
        public virtual bool IsWipBatchRelationStoped(string code, BarcodeType type)
        {
            return GetBatchRelation(code, type).IsPause == YesNo.Yes;
        }

        /// <summary>
        /// 判断载具是否被关联
        /// </summary>
        /// <param name="containerNo">载具号</param>
        /// <returns>被关联返回true，否则返回false</returns>
        public virtual bool IsContainerRelation(string containerNo)
        {
            return GetBatchRelation(containerNo, BarcodeType.ContainerNo) != null;
        }

        /// <summary>
        /// 解除载具关联关系
        /// </summary>
        /// <param name="containerNo">载具号</param>
        /// <exception cref="ArgumentNullException">载具号为空</exception>
        public virtual void RelieveContainerRelation(string containerNo)
        {
            Check.NotNullOrEmpty(containerNo, nameof(containerNo));
            var relations = GetBatchRelations(containerNo, BarcodeType.ContainerNo);
            relations.ForEach(e =>
            {
                e.ContainerNo = string.Empty;
                e.PersistenceStatus = PersistenceStatus.Modified;
            });
            RF.Save(relations);
        }

        /// <summary>
        /// 验证批次关联关系
        /// 批次首次上线创建批次关联关系，批次转出时构建批次关系
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>批次关系</returns>
        public virtual BatchRelation ValidateBatchRelation(CollectBarcode barcode, Workcell workcell)
        {
            BatchRelation relation;
            relation = GetBatchRelation(barcode);
            if (relation == null && barcode != null && barcode.Type == BarcodeType.BatchBarocde)
            {
                var batch = RT.Service.Resolve<WipBatchController>().GetWipBatch(barcode.Code);
                if (batch == null)
                {
                    throw new ValidationException("生产批次[{0}]不存在".L10nFormat(barcode.Code));
                }

                if (batch.IsChild)
                {
                    var subBatch = batch as SubWipBatch;
                    if (subBatch == null || subBatch.WipBatch == null)
                    {
                        throw new ValidationException("生产批次[{0}]不存在".L10nFormat(barcode.Code));
                    }

                    CreateWipBatchRelation(subBatch);
                    relation = CreateSubWipBatchRelation(subBatch.BatchNo, subBatch.WipBatch.BatchNo, subBatch.Qty, batch.WorkOrderId, BatchSource.Split);
                }
                else
                {
                    relation = CreateSubWipBatchRelation(batch.BatchNo, batch.BatchNo, batch.Qty, batch.WorkOrderId, null);
                }
            }

            if (relation == null)
            {
                throw new ValidationException("未找到批次关联关系".L10N());
            }

            if (relation.RemainQty <= 0)
            {
                throw new ValidationException("转入失败，批次已拆分或已合并".L10N());
            }

            if (relation.IsPause == YesNo.Yes)
            {
                throw new ValidationException("[{0}]产品已暂停，不能继续生产".L10nFormat(relation.Bid));
            }

            return relation;
        }

        /// <summary>
        /// 创建子生产批次关联关系
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="wipBatch">生产批次号</param>
        /// <param name="qty">批次数量</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="source">批次来源</param>
        /// <param name="isSave"></param>
        /// <param name="isNg">不良数量</param>
        /// <returns>关联批次</returns>
        public virtual BatchRelation CreateSubWipBatchRelation(string batchNo, string wipBatch, decimal qty, double workOrderId, BatchSource? source, bool isSave = true, bool isNg = false)
        {
            var subwipBatchRelation = new BatchRelation()
            {
                Bid = batchNo,
                Qty = qty,
                RemainQty = qty,
                Pid = source == null ? string.Empty : wipBatch,
                WipBatch = wipBatch,
                BatchSource = source,
                WorkOrderId = workOrderId,
                IsNg = isNg,
            };
            if (isSave)
            {
                RF.Save(subwipBatchRelation);
            }
            return subwipBatchRelation;
        }

        /// <summary>
        /// 创建子生产批次关联关系
        /// </summary>
        /// <param name="batchNo">批次号</param>
        /// <param name="wipBatch">生产批次号</param>
        /// <param name="qty">批次数量</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="source">批次来源</param>
        /// <param name="isNg">是否不良</param>
        /// <returns>关联批次</returns>
        public virtual BatchRelation CreateSubWipBatchRelationNoSave(string batchNo, string wipBatch, decimal qty, double workOrderId, BatchSource? source, bool isNg = false)
        {
            return CreateSubWipBatchRelation(batchNo, wipBatch, qty, workOrderId, source, false, isNg);
        }

        /// <summary>
        /// 创建生产批次关联关系
        /// </summary>
        /// <param name="subBatch">生产子批次</param>
        private void CreateWipBatchRelation(SubWipBatch subBatch)
        {
            var wipBatch = subBatch.WipBatch;
            var wipRelation = GetBatchRelation(wipBatch.BatchNo, BarcodeType.BatchBarocde);
            if (wipRelation == null)
            {
                var wipBatchRelation = new BatchRelation()
                {
                    Bid = wipBatch.BatchNo,
                    Qty = wipBatch.Qty,
                    RemainQty = wipBatch.Qty - subBatch.Qty,
                    WipBatch = wipBatch.BatchNo,
                    WorkOrderId = wipBatch.WorkOrderId
                };
                RF.Save(wipBatchRelation);
            }
            else
            {
                //扣减生产批次剩余入站数量
                wipRelation.RemainQty -= subBatch.Qty;
                RF.Save(wipRelation);
            }
        }
        public virtual void DeleteWipBatchList(List<double> batchIds)
        {
            if (batchIds == null || !batchIds.Any())
            {
                throw new ValidationException("批次已不存在，请刷新界面".L10N());
            }
            var wipBatchs = batchIds.SplitContains((Ids) =>
            {
                return Query<WipBatch>().Where(p => Ids.Contains(p.Id)).ToList();
            });
            if (!wipBatchs.Any())
            {
                throw new ValidationException("批次已不存在，请刷新界面".L10N());
            }

            var woIdList = wipBatchs.Select(m => m.WorkOrderId).ToList();
            var woList = woIdList.SplitContains((Ids) =>
            {
                return Query<BatchWorkOrder>().Where(p => Ids.Contains(p.Id)).ToList();
            });
            var batchNoList = wipBatchs.Select(m => m.BatchNo).ToList();

            var batchWipProductVersionList = batchNoList.SplitContains((Ids) =>
            {
                return Query<BatchWipProductVersion>().Where(p => Ids.Contains(p.BatchNo)).ToList();
            });

            var batchWipProductProcessDetailList = batchNoList.SplitContains((Ids) =>
            {
                return Query<BatchWipProductProcessDetail>().Where(p => Ids.Contains(p.BatchNo)).ToList();
            });
            var now = RF.Find<BatchWipProductProcessDetail>().GetDbTime();
            var logs = new EntityList<WorkOrderLog>();
            foreach (var wipBatch in wipBatchs)
            {
                if (wipBatch.BatchState != BatchState.Generated)
                {
                    throw new ValidationException("批次{0}的状态不为已生成，无法删除".L10nFormat(wipBatch.BatchNo));
                }
                var batchWo = woList.FirstOrDefault(p => p.Id == wipBatch.WorkOrderId);
                if (batchWo == null)
                {
                    throw new ValidationException("批次{0}的工单不存在".L10nFormat(wipBatch.BatchNo));
                }
                var batchWipProductVersion = batchWipProductVersionList.FirstOrDefault(p => p.BatchNo == wipBatch.BatchNo);
                if (batchWipProductVersion != null)
                {
                    throw new ValidationException("批次{0}的已生产过站，无法删除".L10nFormat(wipBatch.BatchNo));
                }
                var batchWipProductProcessDetail = batchWipProductProcessDetailList.FirstOrDefault(p => p.BatchNo == wipBatch.BatchNo);
                if (batchWipProductProcessDetail != null)
                {
                    throw new ValidationException("批次{0}的已采集过站，无法删除".L10nFormat(wipBatch.BatchNo));
                }
                //记录日志
                var log = RT.Service.Resolve<WorkOrderController>().CreateWorkOrderLog(wipBatch.WorkOrderId, WorkOrderLogType.Other,
                    "删除批次".L10N(), now);
                logs.Add(log);
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                foreach (var wipBatch in wipBatchs)
                {
                    //更新已生成数量
                    DB.Update<BatchWorkOrder>().Set(p => p.GeneratedQty, p => p.GeneratedQty - wipBatch.Qty)
                        .Where(p => p.Id == wipBatch.WorkOrderId).Execute();
                }
                //删除批次
                batchIds.SplitDataExecute(ids =>
                {
                    DB.Delete<WipBatch>().Where(p => ids.Contains(p.Id)).Execute();
                });
                RF.BatchInsert(logs);
                tran.Complete();
            }

        }


        /// <summary>
        /// 删除批次（未过站的）
        /// </summary>
        /// <param name="batchId">批次ID</param>
        public virtual void DeleteWipBatch(double batchId)
        {
            var wipBatch = GetById<WipBatch>(batchId);
            if (wipBatch == null)
            {
                throw new ValidationException("批次已不存在，请刷新界面".L10N());
            }
            if (wipBatch.BatchState != BatchState.Generated)
            {
                throw new ValidationException("批次{0}的状态不为已生成，无法删除".L10nFormat(wipBatch.BatchNo));
            }
            var batchWo = GetById<BatchWorkOrder>(wipBatch.WorkOrderId);
            if (batchWo == null)
            {
                throw new ValidationException("批次{0}的工单不存在".L10nFormat(wipBatch.BatchNo));
            }
            var wipCount = Query<BatchWipProductVersion>().Where(p => p.BatchNo == wipBatch.BatchNo).Count();
            if (wipCount > 0)
            {
                throw new ValidationException("批次{0}的已生产过站，无法删除".L10nFormat(wipBatch.BatchNo));
            }
            var processCount = Query<BatchWipProductProcessDetail>().Where(p => p.BatchNo == wipBatch.BatchNo).Count();
            if (processCount > 0)
            {
                throw new ValidationException("批次{0}的已采集过站，无法删除".L10nFormat(wipBatch.BatchNo));
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //删除批次
                DB.Delete<WipBatch>().Where(p => p.Id == wipBatch.Id).Execute();

                //更新已生成数量
                DB.Update<BatchWorkOrder>().Set(p => p.GeneratedQty, p => p.GeneratedQty - wipBatch.Qty)
                    .Where(p => p.Id == batchWo.Id).Execute();

                //记录日志
                RT.Service.Resolve<WorkOrderController>().SaveWorkOrderLog(batchWo.Id, WorkOrderLogType.Other, "删除批次", DateTime.Now);
                tran.Complete();
            }
        }

        #region 批次打印设置
        /// <summary>
        /// 获取或新增批次打印设置
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>批次打印设置</returns>
        public virtual BatchPrintSetting GetOrCreateBatchPrintSetting(double workOrderId)
        {
            var setting = Query<BatchPrintSetting>().Where(p => p.WorkOrderId == workOrderId).FirstOrDefault();
            if (setting != null)
            {
                return setting;
            }

            return CreateBatchPrintSetting(workOrderId);
        }

        /// <summary>
        /// 创建批次打印设置
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>批次打印设置</returns>
        BatchPrintSetting CreateBatchPrintSetting(double workOrderId)
        {
            var workOrder = RF.GetById<WorkOrder>(workOrderId);
            if (workOrder == null)
            {
                throw new EntityNotFoundException(typeof(WorkOrder), workOrderId);
            }

            var setting = new BatchPrintSetting();
            setting.NumberRule = workOrder.Template?.NumberRule;
            setting.WorkOrder = workOrder;
            setting.GenerateId();
            RF.Save(setting);
            return setting;
        }
        #endregion


        /// <summary>
        /// 获取拆分数据去向
        /// </summary>
        /// <param name="sourceBatchNo">源批次号</param>
        /// <param name="resourceId">资源id</param>
        /// <param name="processId">工序id</param>
        /// <param name="stationId">工位id</param>
        /// <returns></returns>
        public virtual EntityList<BatchWipSplitViewModel> GetSplitSourceDatas(string sourceBatchNo, double resourceId, double processId, double stationId)
        {
            return Query<BatchRelation>()
                .Where(p => p.Pid == sourceBatchNo && p.ResourceId == resourceId && p.ProcessId == processId && p.StationId == stationId)
                .Select(p => new
                {
                    BatchNo = p.Bid,
                    BatchSource = p.BatchSource,
                    Qty = p.Qty,
                    IsDefect = p.IsNg ? YesNo.Yes : YesNo.No,
                }).ToList<BatchWipSplitViewModel>().AsEntityList();
        }
    }
}