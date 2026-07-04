using DocumentFormat.OpenXml.Vml.Office;
using Microsoft.Scripting.Utils;
using Newtonsoft.Json;
using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Barcodes;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Assemlys;
using SIE.MES.BatchWIP.Configs;
using SIE.MES.BatchWIP.Exceptions;
using SIE.MES.BatchWIP.Products;
using SIE.MES.BatchWIP.Products.SplitAndMerge;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using SIE.Packages.ItemLabels;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 批次采集控制器
    /// </summary>
    public partial class WipController
    {
        /// <summary>
        /// 批次管理控制器
        /// </summary>
        BatchManageController _batchController;

        /// <summary>
        /// 批次管理控制器
        /// </summary>
        protected virtual BatchManageController BatchController { get { return _batchController ?? (_batchController = RT.Service.Resolve<BatchManageController>()); } }

        /// <summary>
        /// 获取批次工单Id
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param> 
        /// <returns>工单Id</returns>
        public virtual ProductInfo GetBatchMoveIn(CollectBarcode barcode, Workcell workcell)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                ////验证转入条码 
                ValidateMoveInBatch(barcode, workcell);
                ////验证并生成批次关联关系  
                BatchController.ValidateBatchRelation(barcode, workcell);
                ////验证运行时产品工艺路线
                var product = ValidateBatchProduct(barcode, workcell.ProcessId);
                var result = new ProductInfo
                {
                    ItemId = product.ItemId,
                    Puid = product.Puid,
                    WorkOrderId = product.WorkOrderId
                };
                tran.Complete();
                return result;
            }
        }

        /// <summary>
        /// 批次转入
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param> 
        /// <returns>产品信息</returns> 
        public virtual BatchProductInfo BatchMoveIn(CollectBarcode barcode, Workcell workcell)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (barcode == null)
                    throw new ArgumentNullException(nameof(barcode));
                BatchProductInfo result = MoveIn(barcode, workcell);
                tran.Complete();
                return result;
            }
        }

        /// <summary>
        /// 验证转出解绑载具号是否有问题
        /// </summary>
        /// <param name="barcode">采集</param>
        /// <param name="isUnbind">是否解绑</param>
        /// <param name="workcell">转入批次</param>
        /// <param name="workOrderId">工作单元</param>
        private void ValidateContainNoRepeatIn(CollectBarcode barcode, bool isUnbind, Workcell workcell, double workOrderId)
        {
            if (barcode.Type == BarcodeType.ContainerNo && !isUnbind)
            {
                var input = BatchController.GetInputBatchs(workcell.ResourceId, workcell.ProcessId, workcell.StationId, workOrderId);
                if (input.FirstOrDefault(p => p.ContainerNo == barcode.Code) != null)
                {
                    throw new ValidationException("载具[{0}]已扫描入站".L10nFormat(barcode.Code));
                }
            }
        }

        /// <summary>
        /// 存在转入转出批次的批次转入，
        /// 例如：过站，上料，检验
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="workcell">工作单元</param> 
        /// <param name="defects">不良数量</param> 
        /// <param name="ngQty">不良数量</param> 
        /// <returns>产品信息</returns> 
        public virtual BatchProductInfo MoveIn(CollectBarcode barcode, Workcell workcell, EntityList<Defect> defects = null, decimal? ngQty = null)
        {
            ////验证转入条码 
            ValidateMoveInBatch(barcode, workcell);
            ////验证并生成批次关联关系  
            var relation = BatchController.ValidateBatchRelation(barcode, workcell);
            ////验证运行时产品工艺路线
            var product = ValidateBatchProduct(barcode, workcell.ProcessId);
            ////验证工单
            ValidateWorkOrder(product);
            //// 转出解绑验证是否重复验证
            ValidateContainNoRepeatIn(barcode, GetUnbindMode() == UnbindMode.MoveIn, workcell, relation.WorkOrderId);
            var inputBatch = BatchController.InputBatch(relation, workcell, relation.WorkOrderId, barcode.Type, defects, ngQty);
            ////载具解绑，只有在全局配置项配置载具为转入时解绑才进行解绑
            inputBatch = ContainerUnbind(barcode, relation, GetUnbindMode() == UnbindMode.MoveIn, inputBatch);
            ////当前在制版本(wipBatch -> Bid,子批生成子批，父批生成父批)
            var version = FindLastBatchWipProductVersion(relation.Bid);
            // 生成出入分离的采集记录
            CreateMoveInProductRecord(version, relation, inputBatch, workcell);
            return new BatchProductInfo
            {
                ItemId = product.ItemId,
                Puid = product.Puid,
                WorkOrderId = product.WorkOrderId,
                InputBatch = inputBatch
            };
        }

        /// <summary>
        /// 载具解绑
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="relation">批次关联关系</param>
        /// <param name="isUnbind">是否解绑</param>
        /// <param name="inputBatch">转入批次</param>
        /// <returns>处理后的转入批次</returns>
        InputBatch ContainerUnbind(CollectBarcode barcode, BatchRelation relation, bool isUnbind, InputBatch inputBatch = null)
        {
            if (barcode.Type == BarcodeType.ContainerNo && isUnbind)
            {
                var relations = BatchController.GetBatchRelations(barcode.Code, barcode.Type);
                var box = GetTurnoverBox(barcode.Code);
                box.State = BoxState.Unused;
                EntityList<TurnoverBoxActionLog> logs = new EntityList<TurnoverBoxActionLog>();
                foreach (var relationItem in relations)
                {
                    var log = RT.Service.Resolve<BoxController>().ReCreateActionLog(box.Id, null, relation.RemainQty, relationItem.Bid, TurnoverType.UnBinding);
                    logs.Add(log);
                    relationItem.ContainerNo = string.Empty;
                }
                RF.Save(logs);
                RF.Save(relations);
                RF.Save(box);
                RuntimeController.UnmapPuid(barcode);
                if (inputBatch == null || inputBatch.ContainerNo.IsNullOrEmpty())
                    return inputBatch;
                inputBatch.ContainerNo = string.Empty;
                RF.Save(inputBatch);
            }
            return inputBatch;
        }

        /// <summary>
        /// 移除转入批次
        /// </summary>
        /// <param name="batchId">转入批次ID</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="containerNo">载具号</param>
        public virtual void RemoveInputBatch(double batchId, Workcell workcell, string containerNo)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                TurnoverBox box = null;
                if (!containerNo.IsNullOrEmpty())
                    box = ValidateContainer(containerNo);
                ////修改批次条码状态为移除状态，修改转入批次状态，修改批次关系，添加工序记录
                var batch = RF.GetById<InputBatch>(batchId);
                if (batch == null)
                    throw new EntityNotFoundException(typeof(InputBatch), batchId);
                string batchNo = batch.IsChild ? batch.SubBatchNo : batch.BatchNo;
                ////更新批次条码状态
                UpdateWipBatch(batchNo);
                ////更新批次关联关系
                EntityList<BatchRelation> relations = UpdateBatchRelations(box, batch, batchNo);
                ////更新采集记录
                UpdateBProductRecord(workcell, batchNo, batch);
                ////删除转入批次状态
                batch.PersistenceStatus = PersistenceStatus.Deleted;
                RF.Save(batch);
                if (box != null)
                    RF.Save(box);
                tran.Complete();
            }
        }

        /// <summary>
        /// 更新批次关联关系
        /// </summary> 
        /// <param name="box">生产周转箱</param>
        /// <param name="batch">转入批次</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>批次关联关系列表</returns>
        private EntityList<BatchRelation> UpdateBatchRelations(TurnoverBox box, InputBatch batch, string batchNo)
        {
            var relations = BatchController.GetBatchRelations(batchNo, BarcodeType.BatchBarocde);
            if (relations.Count == 0)
                throw new ValidationException("未找到[{0}]的批次关联关系".L10nFormat(batchNo));
            relations.ForEach(relation =>
            {
                relation.RemainQty = batch.RemainQty;
                if (box != null)
                {
                    batch.ContainerNo = box.Code;
                    relation.ContainerNo = box.Code;
                    var product = RuntimeController.FindProduct(relation.Bid, BarcodeType.BatchBarocde);
                    if (product == null)
                        throw new ValidationException("未找到产品[{0}]运行时数据".L10nFormat(relation.Bid));
                    RuntimeController.MapPuid(box.Code, BarcodeType.ContainerNo, product.Puid);
                    box.State = BoxState.Inuse;
                }
            });
            RF.Save(box);
            RF.Save(relations);
            return relations;
        }

        /// <summary>
        /// 更新批次条码状态
        /// </summary>
        /// <param name="batchNo">批次号</param>
        private void UpdateWipBatch(string batchNo)
        {
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(batchNo);
            if (wipBatch == null)
                throw new ValidationException("批次条码[{0}]不存在".L10nFormat(batchNo));
            wipBatch.BatchState = BatchState.Removed;
            RF.Save(wipBatch);
        }

        /// <summary>
        /// 验证载具是否被使用
        /// </summary>
        /// <param name="containerNo">载具号</param>
        /// <returns>周转箱</returns>
        private TurnoverBox ValidateContainer(string containerNo)
        {
            var box = GetRelationContainer(containerNo);
            if (box.State == BoxState.Inuse)
                throw new ValidationException("载具[{0}]已经被使用".L10nFormat(containerNo));
            return box;
        }

        /// <summary>
        /// 转出批次
        /// </summary>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        public virtual void BatchMoveOut(CollectData collectData, Workcell workcell)
        {
            if (collectData == null)
                throw new ArgumentNullException(nameof(collectData));
            ////验证工作单元信息
            var process = ValidateWorkcell(workcell);
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                MoveOut(collectData, workcell, process);
                tran.Complete();
            }
        }

        /// <summary>
        /// 转出批次
        /// </summary> 
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="process">当前工序</param>
        /// <param name="isPda">PDA不需要再执行下一工序入站的逻辑</param> 
        public virtual product MoveOut(CollectData collectData, Workcell workcell, Process process, bool isPda = false)
        {
            List<CollectBarcode> collectBarcodes = null;
            product product = null;
            OutputBatch outputBatch = null;
            InputBatch inputBatch = null;
            if (process.Type == ProcessType.BatchFix)
            {
                FixContainerUnbind(collectData, out outputBatch, out collectBarcodes, out product, workcell, out inputBatch);
                product.Routing.Current = product.Routing.GetNext().FirstOrDefault(p => p.ProcessId == workcell.ProcessId);
            }
            else if (process.Type == ProcessType.BatchPacking)
            {
                outputBatch = collectData.OutputBatch;
                if (outputBatch == null || !(outputBatch.BatchNo.IsNotEmpty() || outputBatch.SubBatchNo.IsNotEmpty()))
                {
                    throw new ValidationException("转出批次不能为空".L10N());
                }

                collectBarcodes = InitCollectBarcode(outputBatch);
                product = CreateSubWipProduct(collectData, workcell.ProcessId, outputBatch, outputBatch.WorkOrderId.Value);
                ContainerBind(collectBarcodes[0].Type, outputBatch.SubBatchNo, collectBarcodes[0], outputBatch.Qty, true);
            }
            else
            {
                ValidateBatch(collectData, out outputBatch, out inputBatch);
                collectBarcodes = InitCollectBarcode(outputBatch);
                product = ValidateBatchProduct(collectData, workcell.ProcessId, outputBatch, inputBatch, collectBarcodes[0]);
            }
            ////验证工单
            ValidateWorkOrder(product);
            ////验证暂停的产品
            if (product.IsHold)
            {
                ValidateHoldProduct(process, collectData);
            }
            ////事件数据
            var data = new CollectEventData(product, collectBarcodes.ToArray(), collectData, workcell, RF.Find<Process>().GetDbTime());
            ////采集开始通知
            OnCollecting(data);
            ////更新转入批次数量
            UpdateInputBatch(process.Type, workcell, outputBatch, product.WorkOrderId, collectData);

            BatchRelationOperate(collectBarcodes, collectData, process, workcell, product, data);
            if (!isPda)
            {
                var workcellProcess = RF.GetById<Process>(workcell.ProcessId, new EagerLoadOptions().LoadWith(Process.CollectStepListProperty));
                //判断是否执行下一个工序入站
                var isNextMoveIn = product.Routing.Current == null ? workcellProcess.IsNextMoveIn : product.Routing.Current.IsNextMoveIn;
                //下一工序要入站的前提是当前工序不是最后工序
                if (isNextMoveIn == true && (product.Routing.Current != null && product.Routing.Current.Sign != Tech.Routings.RoutingProcessSign.End))
                {
                    //验证下一工序入站
                    RT.Service.Resolve<WipBatchApiController>().ValidateNextProcessMoveIn(workcell, product, out Process nextMoVEprocess, out EntityList<Station> stations);
                    //执行下一工序入站
                    RT.Service.Resolve<WipBatchApiController>().ExecutedNextProcessMoveIn(workcell, collectData.OutputBatch.BatchNo, nextMoVEprocess, stations);
                }
            }

            return product;
        }

        /// <summary>
        /// 新批次关系处理
        /// </summary>
        /// <param name="collectBarcodes">采集条码</param>
        /// <param name="collectData">相关采集数据</param>
        /// <param name="process">当前工序</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="product">产品运行时</param>
        /// <param name="data">事件数据</param>
        private void BatchRelationOperate(List<CollectBarcode> collectBarcodes, CollectData collectData, Process process, Workcell workcell, product product, CollectEventData data)
        {
            ////同一中批条码采集记录都在中批版本中记录
            // 批次关联关系(生成子批次)
            EntityList<BatchRelation> batchRelations = BatchController.GetBatchRelations(collectBarcodes);
            // 生产通用报表
            EntityList<BatchWipProductVersion> versions = FindLastBatchWipProductVersions(batchRelations.Select(p => p.Pid).ToList());
            versions.AddRange(FindLastBatchWipProductVersions(batchRelations.Select(p => p.Bid).ToList()));

            // 生产通用报表记录
            EntityList<BatchWipRecord> versionRecords = GetBatchWipRecords(versions.Select(p => p.Id).ToList(), workcell);
            // 子批次信息
            EntityList<SubWipBatch> subWipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatchs(batchRelations.Select(p => p.Bid).ToList());
            // 生产批次产品
            EntityList<BatchWipProduct> batchWipProducts = RT.Service.Resolve<BatchManageController>().GetBatchWipProducts(versions.Select(p => p.ProductId).ToList());

            // 子批次生产通用报表
            EntityList<BatchWipProductVersion> childWipVersions = new EntityList<BatchWipProductVersion>();
            // 子批次生产通用报表记录
            EntityList<BatchWipRecord> childWipVersionRecords = new EntityList<BatchWipRecord>();
            // 缺陷记录
            EntityList<BatchWipProductDefect> defectRecords = new EntityList<BatchWipProductDefect>();
            // 缺陷明细
            EntityList<BatchWipProductDefectDetail> defectDetails = new EntityList<BatchWipProductDefectDetail>();
            // 责任列表
            EntityList<BatchWipDefectResponsibility> responsibilities = new EntityList<BatchWipDefectResponsibility>();
            // 维修措施列表
            EntityList<BatchWipDefectMeasure> measures = new EntityList<BatchWipDefectMeasure>();
            //关键件
            EntityList<BatchWipProductProcessKeyItem> batchWipProductProcessKeyItems = new EntityList<BatchWipProductProcessKeyItem>();
            foreach (BatchRelation batchRelation in batchRelations)
            {
                ////更新批次关联关系数量
                UpdateBatchRelation(batchRelation, collectData, process.Type);
            }
            // 更新批次状态
            foreach (var sub in subWipBatchs)
            {
                sub.BatchState = BatchState.Out;
            }
            // 不做处理的数据
            var parentRelation = batchRelations.Where(p => p.BatchSource == null).ToList();

            // 拆分数据
            var splitBatchRelation = batchRelations.Where(p => p.BatchSource != null && p.BatchSource == BatchSource.Split).ToList();

            // 合并数据
            var merageBatchRelation = batchRelations.Where(p => p.BatchSource != null && p.BatchSource == BatchSource.Merge).ToList();
            var merageBatchNo = merageBatchRelation.Select(p => p.Bid).Distinct().ToList();

            // 班次
            var shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(workcell.ResourceId, DateTime.Now);
            if (shift == null)
            {
                throw new ValidationException("当前生产资源班次为空！".L10N());
            }
            // 无处理
            foreach (BatchRelation relation in parentRelation)
            {
                // 生产通用信息
                var version = versions.FirstOrDefault(p => p.BatchNo == relation.Bid);
                var record = new BatchWipRecord
                {
                    BatchVersion = version,
                    BatchNo = relation.Bid,
                    ContainerNo = relation.ContainerNo,
                    InOutType = PlugType.Out,
                    Qty = relation.RemainQty,
                    ResultType = ResultType.Pass,
                    ShiftId = shift.Id,
                    ResourceId = workcell.ResourceId,
                    ProcessId = workcell.ProcessId,
                    StationId = workcell.StationId,
                    ScrapQty = collectData.ScrapQty,
                    DefectQty = collectData.OutputBatch.NgQty,
                };
                record.GenerateId();
                if (version != null)
                {
                    version.StationId = record.StationId;
                    version.ProcessId = record.ProcessId;
                    version.ResourceId = record.ResourceId;
                    version.CurrentProcessId = record.Id;
                    version.RemainQty = relation.RemainQty;
                }
                //计算工艺路线后工序
                if (collectData.State == WipProductProcessState.Finish)
                {
                    if (collectData.ApiData)
                    {
                        // pda端报废不是立刻执行的
                        version.ScrapQty += collectData.ScrapQty;
                    }
                    version.NgQty += collectData.NgQty;
                    //工艺路线配置当前工序要创建SKU
                    var workOrderMove = product.WorkOrderMove;

                    if (product.Routing.Current.CreateSku)
                    {
                        RT.Service.Resolve<ItemLabelController>()
                            .CreateItemLabel(product.WorkOrder.Product, version.RemainQty, version.BatchNo,
                             LabelSource.BatchWip, product.WorkOrderId, workOrderMove.FactoryId, workOrderMove.ItemExtProp,
                             workOrderMove.ItemExtPropName, workOrderMove.ProjectMaintain?.Code);
                    }

                    ////设置产品不良
                    if (process.Type == ProcessType.BatchPqc && collectData.Result == ResultType.Fail)
                    {
                        product.IsNg = true;
                    }

                    OnBatchWipProductProcessFinished(record, product, collectBarcodes, collectData, workcell, collectData.OutputBatch.RelationBatchList);
                    if (record.KeyItemList.Any())
                    {
                        var newKeyItemList = JsonConvert.DeserializeObject<EntityList<BatchWipProductProcessKeyItem>>(JsonConvert.SerializeObject(record.KeyItemList));

                        newKeyItemList.ForEach(it =>
                        {
                            it.Id = 0;
                            it.PersistenceStatus = PersistenceStatus.New;

                        });
                        batchWipProductProcessKeyItems.AddRange(newKeyItemList);
                    }

                    ComputeNextProcess(product, collectData.Result, collectData);
                    SaveNextProcess(workcell, product, version);
                }
                childWipVersionRecords.Add(record);
            }
            // 拆分
            foreach (BatchRelation relation in splitBatchRelation)
            {
                // 更新批次来源
                relation.BatchSource = null;
                // 生产通用信息
                var version = versions.FirstOrDefault(p => p.BatchNo == relation.Pid);
                // 生产批次产品
                var batchWipProduct = batchWipProducts.FirstOrDefault(p => p.Id == version.ProductId);
                // 父生产批次记录
                var versionRecord = versionRecords.FirstOrDefault(p => p.BatchVersionId == version.Id);
                // 缺陷
                var defectListRelation = relation.Pid.IsNotEmpty() ? collectData.OutputBatch.RelationBatchList.Where(p => p.InputBatch.SubBatchNo == relation.Pid).AsEntityList() : collectData.OutputBatch.RelationBatchList.Where(p => p.InputBatch.BatchNo == relation.Pid).AsEntityList();
                ////添加批次过站拆分记录
                CreateMoveOutSplitWipRecord(product, version, versionRecord, collectData, workcell, batchWipProduct, shift, out BatchWipProductVersion childVersion, out BatchWipRecord batchWipRecord);

                //计算工艺路线后工序
                if (collectData.State == WipProductProcessState.Finish)
                {
                    childVersion.ScrapQty += collectData.ScrapQty;
                    childVersion.NgQty += collectData.NgQty;

                    //工艺路线配置当前工序要创建SKU
                    var workOrderMove = product.WorkOrderMove;

                    if (product.Routing.Current.CreateSku)
                    {
                        RT.Service.Resolve<ItemLabelController>()
                            .CreateItemLabel(product.WorkOrder.Product, childVersion.RemainQty, childVersion.BatchNo,
                             LabelSource.BatchWip, product.WorkOrderId, workOrderMove.FactoryId, workOrderMove.ItemExtProp,
                             workOrderMove.ItemExtPropName, workOrderMove.ProjectMaintain?.Code);
                    }

                    ////设置产品不良
                    if (process.Type == ProcessType.BatchPqc && collectData.Result == ResultType.Fail)
                    {
                        product.IsNg = true;
                    }

                    OnBatchWipProductProcessFinished(batchWipRecord, product, collectBarcodes, collectData, workcell, defectListRelation);
                    if (batchWipRecord.KeyItemList.Any())
                    {
                        var newKeyItemList = JsonConvert.DeserializeObject<EntityList<BatchWipProductProcessKeyItem>>(JsonConvert.SerializeObject(batchWipRecord.KeyItemList));

                        newKeyItemList.ForEach(it =>
                        {
                            it.Id = 0;
                            it.PersistenceStatus = PersistenceStatus.New;
                        });
                        batchWipProductProcessKeyItems.AddRange(newKeyItemList);
                    }
                    defectRecords.AddRange(defectListRelation.SelectMany(p => p.BatchWipProductDefects));
                    responsibilities.AddRange(defectRecords.SelectMany(p => p.ResponsibilityList));
                    defectDetails.AddRange(defectRecords.SelectMany(p => p.DetailList));
                    measures.AddRange(defectRecords.SelectMany(p => p.MeasureList));
                    ComputeNextProcess(product, collectData.Result, collectData);
                    //SaveNextProcess(workcell, product, version);
                }
                if (childVersion != null)
                {
                    SaveNextProcess(workcell, product, childVersion);
                    childVersion.IsFinish = data.Product.Routing.Current.IsEnd;
                    childWipVersions.Add(childVersion);
                    childWipVersionRecords.Add(batchWipRecord);
                }

            }

            // 合并
            foreach (string bid in merageBatchNo)
            {
                var relations = merageBatchRelation.Where(p => p.Bid == bid).ToList();
                // 缺陷列表
                EntityList<SIE.MES.BatchWIP.RelationBatch> defectRelations;
                BatchWipProductVersion childVersion; BatchWipRecord batchWipRecord;
                ////添加批次过站合并记录
                CreateMoveOutMergeWipRecord(relations, product, versions, versionRecords, collectData, workcell, batchWipProducts, shift, out childVersion, out batchWipRecord, out defectRelations);
                if (collectData.State == WipProductProcessState.Finish)
                {
                    childVersion.ScrapQty += collectData.ScrapQty;
                    childVersion.NgQty += collectData.NgQty;

                    //工艺路线配置当前工序要创建SKU
                    var workOrderMove = product.WorkOrderMove;

                    if (product.Routing.Current.CreateSku)
                    {
                        RT.Service.Resolve<ItemLabelController>()
                            .CreateItemLabel(product.WorkOrder.Product, childVersion.RemainQty, childVersion.BatchNo,
                             LabelSource.BatchWip, product.WorkOrderId, workOrderMove.FactoryId, workOrderMove.ItemExtProp,
                             workOrderMove.ItemExtPropName, workOrderMove.ProjectMaintain?.Code);
                    }

                    ////设置产品不良
                    if (process.Type == ProcessType.BatchPqc && collectData.Result == ResultType.Fail)
                    {
                        product.IsNg = true;
                    }

                    OnBatchWipProductProcessFinished(batchWipRecord, product, collectBarcodes, collectData, workcell, defectRelations);
                    if (batchWipRecord.KeyItemList.Any())
                    {
                        var newKeyItemList = JsonConvert.DeserializeObject<EntityList<BatchWipProductProcessKeyItem>>(JsonConvert.SerializeObject(batchWipRecord.KeyItemList));

                        newKeyItemList.ForEach(it =>
                        {
                            it.Id = 0;
                            it.PersistenceStatus = PersistenceStatus.New;
                        });
                        batchWipProductProcessKeyItems.AddRange(newKeyItemList);
                    }
                    defectRecords.AddRange(defectRelations.SelectMany(p => p.BatchWipProductDefects));
                    responsibilities.AddRange(defectRecords.SelectMany(p => p.ResponsibilityList));
                    defectDetails.AddRange(defectRecords.SelectMany(p => p.DetailList));
                    measures.AddRange(defectRecords.SelectMany(p => p.MeasureList));
                }
                if (childVersion != null)
                {
                    ComputeNextProcess(product, collectData.Result, collectData);
                    SaveNextProcess(workcell, product, childVersion);
                    childVersion.IsFinish = data.Product.Routing.Current.IsEnd;
                    childWipVersions.Add(childVersion);
                    childWipVersionRecords.Add(batchWipRecord);
                }
            }

            RF.Save(batchRelations);
            RF.Save(subWipBatchs);
            RF.Save(versions);
            RF.Save(versionRecords);
            RF.BatchInsert(childWipVersions);
            childWipVersionRecords.ForEach(p =>
            {
                if (p.BatchVersion != null)
                {
                    p.BatchVersionId = p.BatchVersion.Id;
                }
            });
            RF.BatchInsert(childWipVersionRecords);
            RF.BatchInsert(defectRecords);
            responsibilities.ForEach(p =>
            {
                p.DefectId = p.Defect.Id;
            });
            RF.BatchInsert(responsibilities);
            defectDetails.ForEach(p =>
            {
                p.DefectId = p.Defect.Id;
            });
            RF.BatchInsert(defectDetails);
            measures.ForEach(p =>
            {
                p.DefectId = p.Defect.Id;
            });
            RF.BatchInsert(measures);
            ////保存采集运行时产品
            RuntimeController.Save(product);
            if (batchWipProductProcessKeyItems.Any(p => p.PersistenceStatus != PersistenceStatus.Unchanged))
            {
                //保存关键件
                RF.Save(batchWipProductProcessKeyItems);
            }
            foreach (BatchRelation batchRelation in batchRelations)
            {
                ////采集结束通知
                OnBatchCollected(data, batchRelation);
            }
        }

        private void SaveNextProcess(Workcell workcell, product product, BatchWipProductVersion version)
        {
            try
            {
                version.CurrentProcessIndex = product.Routing.Current.Index;
                //获取工艺路线下一工序
                var nexts = product.Routing.GetNext();
                var nextProcess = nexts.FirstOrDefault(p => !p.Optional && p.ProcessId != workcell.ProcessId);
                if (nextProcess == null)
                {
                    //nextProcess = nexts.FirstOrDefault(p => !p.Optional && p.ProcessId == workcell.ProcessId);
                }
                if (!version.IsFinish)
                {
                    version.NextProcessId = nextProcess?.ProcessId;
                    version.NextProcessIndex = nextProcess?.Index;
                }
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc);
            }
        }

        /// <summary>
        /// 维修载具解绑
        /// </summary>
        /// <param name="collectData">采集数据</param>
        /// <param name="outputBatch">转出批次</param>
        /// <param name="collectBarcodes">采集条码</param>
        /// <param name="product">运行时产品</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="inputBatch">转入批次</param>
        void FixContainerUnbind(CollectData collectData, out OutputBatch outputBatch, out List<CollectBarcode> collectBarcodes, out product product, Workcell workcell, out InputBatch inputBatch)
        {
            collectBarcodes = new List<CollectBarcode> { collectData.CollectBarcode };
            ValidateBatch(collectData, out outputBatch, out inputBatch);
            product = ValidateBatchProduct(collectBarcodes[0], workcell.ProcessId);
            var batchNo = inputBatch.SubBatchNo.IsNullOrEmpty() ? inputBatch.BatchNo : inputBatch.SubBatchNo;
            if (outputBatch.Qty == 0)
            {
                BatchController.GetBatchRelation(collectBarcodes[0]);
                var relations = BatchController.GetBatchRelations(batchNo, BarcodeType.BatchBarocde);
                if (relations.Count <= 0) return;
                foreach (BatchRelation rela in relations)
                {
                    if (!rela.ContainerNo.IsNullOrEmpty())
                    {
                        ContainerUnbind(new CollectBarcode { Code = rela.ContainerNo, Type = BarcodeType.ContainerNo }, rela, GetUnbindMode() == UnbindMode.MoveOut, inputBatch);
                    }
                }

                RF.Save(relations);
            }
            else
            {
                if (outputBatch.ContainerNo.IsNotEmpty())
                    ContainerBind(outputBatch.BarcodeType, batchNo, new CollectBarcode { Code = outputBatch.ContainerNo, Type = outputBatch.BarcodeType }, outputBatch.Qty);
                var relation = BatchController.GetBatchRelation(collectBarcodes[0]);
                if (relation != null)
                {
                    relation.RemainQty -= outputBatch.Qty;
                    //relation.IsFinish = relation.RemainQty <= 0 ? true : false;
                    //RF.Save(relation);
                }
            }
        }

        /// <summary>
        /// 更新批次数量
        /// </summary>
        /// <param name="batchRelation">批次关系</param>
        /// <param name="collectData">采集结果</param>
        /// <param name="type">工序类型</param>
        private void UpdateBatchRelation(BatchRelation batchRelation, CollectData collectData, ProcessType? type)
        {
            if (collectData.ScrapQty > 0)
            {
                batchRelation.RemainQty -= collectData.ScrapQty;
            }
            if (type == ProcessType.BatchFix)
            {
                batchRelation.IsNg = false;
            }
        }

        /// <summary>
        /// 载具关联
        /// </summary>
        /// <param name="type">条码类型</param> 
        /// <param name="batchNo">批次号</param>
        /// <param name="collectBarcode">采集条码</param>
        /// <param name="itemId">物料id</param>
        /// <param name="qty">转出数量</param>
        /// <param name="isGenerateBatch">是否生成子批次</param>
        private void ContainerBind(BarcodeType type, string batchNo, CollectBarcode collectBarcode, decimal qty, bool isGenerateBatch = false)
        {
            if (type != BarcodeType.ContainerNo) return;
            ////1、生成子批次，将载具号与子批次进行关联，旧载具的解绑再将批次完全拆分后再解绑
            ////2、同一批次的则将转入批次的旧载具解绑，再将新载具进行关联 
            var relations = BatchController.GetBatchRelations(batchNo, BarcodeType.BatchBarocde);
            if (relations.Count <= 0) return;
            foreach (BatchRelation relation in relations)
            {
                var product = RuntimeController.FindProduct(relations[0].Bid, BarcodeType.BatchBarocde);
                if (!isGenerateBatch && !relation.ContainerNo.IsNullOrEmpty())
                    ContainerUnbind(new CollectBarcode { Code = relation.ContainerNo, Type = BarcodeType.ContainerNo }, relation, GetUnbindMode() == UnbindMode.MoveOut);
                relation.ContainerNo = collectBarcode.Code;
                if (isGenerateBatch) continue;
                if (product == null)
                    throw new ValidationException("未找到产品[{0}]运行时数据".L10nFormat(relation.Bid));
                RuntimeController.MapPuid(collectBarcode, product.Puid);
            }
            RF.Save(relations);

            var box = GetRelationContainer(collectBarcode.Code);
            if (box.State == BoxState.Inuse)
                throw new ValidationException("载具[{0}]已经被使用".L10nFormat(collectBarcode.Code));
            box.State = BoxState.Inuse;
            RF.Save(box);
            // 创建周转箱绑定日志
            RT.Service.Resolve<BoxController>().CreateActionLog(box.Id, null, qty, batchNo, TurnoverType.Binding);
        }

        /// <summary>
        /// 获取关联载具
        /// </summary>
        /// <param name="containerNo">载具号</param>
        /// <returns>周转箱</returns>
        private TurnoverBox GetRelationContainer(string containerNo)
        {
            var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
            var box = RT.Service.Resolve<BoxController>().GetTurnoverBox(containerNo, config.BoxType);
            if (box == null)
                throw new ValidationException("载具[{0}]不存在".L10nFormat(containerNo));
            if (box.State == BoxState.Scrap)
                throw new ValidationException("载具[{0}]已经报废".L10nFormat(containerNo));
            return box;
        }

        /// <summary>
        /// 验证批次
        /// </summary>
        /// <param name="collectData">采集数据</param>
        /// <param name="outputBatch">转出批次</param>
        /// <param name="inputBatch">转入批次</param>
        private void ValidateBatch(CollectData collectData, out OutputBatch outputBatch, out InputBatch inputBatch)
        {
            outputBatch = collectData.OutputBatch;
            if (outputBatch == null || !(outputBatch.BatchNo.IsNotEmpty() || outputBatch.SubBatchNo.IsNotEmpty()))
                throw new ValidationException("转出批次不能为空".L10N());
            inputBatch = outputBatch.RelationBatchList.FirstOrDefault()?.InputBatch;
            if (inputBatch == null)
                throw new ValidationException("关联转入批次不能为空".L10N());
        }

        /// <summary>
        /// 创建子批次信息
        /// </summary>
        /// <param name="outputBatch">转出批次</param>
        /// <param name="wipBatchNo">生产批号</param>
        /// <param name="scrapQty"></param>
        /// <returns>子批次信息</returns>
        private SubWipBatch CreateSubWipBatch(OutputBatch outputBatch, string wipBatchNo, decimal? scrapQty)
        {
            var wipQty = outputBatch.RelationBatchList.Sum(p => p.Qty);
            var subWipBatch = outputBatch.SubWipBatch;
            if (subWipBatch == null)
                throw new ValidationException("子批次条码不能为空".L10N());
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(wipBatchNo);
            if (wipBatch == null)
                throw new ValidationException("未找到生产批次".L10N());
            subWipBatch.Qty = subWipBatch.BoxesQty = wipQty;
            subWipBatch.WipBatch = wipBatch;
            subWipBatch.PersistenceStatus = PersistenceStatus.New;
            subWipBatch.ScrapQty = scrapQty;
            RF.Save(subWipBatch);
            BatchSource source = outputBatch.RelationBatchList.Count == 1 ? BatchSource.Split : BatchSource.Merge;
            outputBatch.RelationBatchList.ForEach(e =>
            {
                var relation = new BatchRelation()
                {
                    BatchSource = source,
                    Bid = subWipBatch.BatchNo,
                    Pid = e.InputBatch.SubBatchNo.IsNullOrEmpty() ? e.InputBatch.BatchNo : e.InputBatch.SubBatchNo,
                    ContainerNo = outputBatch.ContainerNo,
                    Qty = e.Qty,
                    RemainQty = e.Qty,
                    WipBatch = e.InputBatch.BatchNo,
                    WorkOrderId = e.InputBatch.WorkOrderId,
                    IsNg = outputBatch.IsNg,
                    ResourceId = e.InputBatch.ResourceId,
                    ProcessId = e.InputBatch.ProcessId,
                    StationId = e.InputBatch.StationId,
                };
                relation.GenerateId();
                RF.Save(relation);
                var parentRelation = BatchController.GetBatchRelation(relation.Pid, BarcodeType.BatchBarocde);
                if (parentRelation == null || parentRelation.IsPause == YesNo.Yes)
                    throw new ValidationException("[{0}]产品已暂停，不能继续生产".L10nFormat(relation.Pid));

                parentRelation.RemainQty -= relation.Qty;
                ////parentRelation.IsFinish = parentRelation.RemainQty <= 0 ? true : false;
                RF.Save(parentRelation);
            });
            return subWipBatch;
        }

        /// <summary>
        /// 初始化采集条码
        /// </summary>
        /// <param name="outputBatch">转出批次</param>
        /// <returns>采集条码</returns>
        private List<CollectBarcode> InitCollectBarcode(OutputBatch outputBatch)
        {
            List<CollectBarcode> collectBarcodes = new List<CollectBarcode>();
            var collectBarcode = new CollectBarcode() { Type = outputBatch.BarcodeType };
            if (outputBatch.BarcodeType == BarcodeType.ContainerNo)
                collectBarcode.Code = outputBatch.ContainerNo;
            else if (outputBatch.BarcodeType == BarcodeType.BatchBarocde)
            {
                if (outputBatch.IsGenerateBatch)
                    collectBarcode.Code = outputBatch.SubBatchNo;
                else
                {
                    collectBarcode.Code = outputBatch.RelationBatchList[0].InputBatch.IsChild ? outputBatch.BatchNo : outputBatch.BatchNo;
                    // 240719修改交互不区分父子
                }
            }

            collectBarcodes.Add(collectBarcode);
            return collectBarcodes;
        }

        /// <summary>
        /// 过站记录创建后，重写此方法保存过站记录额外的数据
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">运行时产品</param>
        /// <param name="barcodes">采集条码集合</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="relationBatches">关联批次</param>
        protected virtual void OnBatchWipProductProcessFinished(BatchWipRecord wipProductProcess,
            product product, IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell, EntityList<RelationBatch> relationBatches)
        {
            //上料，不良等需要额外保存过站数据的，可以重写此方法
        }

        /// <summary>
        /// 更新转入批次
        /// </summary>
        /// <param name="processType">工序类型</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="outputBatch">转出批次</param>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="collectData">采集数据</param>
        private void UpdateInputBatch(ProcessType? processType, Workcell workcell, OutputBatch outputBatch, double workOrderId, CollectData collectData)
        {
            if (/*processType == ProcessType.BatchFix ||*/ processType == ProcessType.BatchPacking)
                return;
            var inputBatchs = BatchController.GetInputBatchs(workcell.ResourceId, workcell.ProcessId, workcell.StationId, workOrderId);
            var wipBatchs = RT.Service.Resolve<WipBatchController>().GetWipBatches(outputBatch.RelationBatchList.Select(p => p.InputBatch.BatchNo).ToList());
            outputBatch.RelationBatchList.ForEach(e =>
            {
                var result = inputBatchs.FirstOrDefault(p => p.Id == e.InputBatchId);
                if (result == null)
                    throw new ValidationException("入站批次[{0}]不存在".L10nFormat(e.InputBatch.BatchNo));
                ////修改批次条码状态
                var wipBatch = wipBatchs.FirstOrDefault(p => p.BatchNo == result.BatchNo);
                if (wipBatch == null)
                {
                    throw new ValidationException("批次条码{0}不存在".L10nFormat(result.BatchNo));
                }
                result.RemainQty -= e.Qty;
                if (processType == ProcessType.BatchFix)
                {
                    result.RemainQty -= collectData.ScrapQty;
                    if (wipBatch.ScrapQty == null)
                    {
                        wipBatch.ScrapQty = 0;
                    }
                    wipBatch.ScrapQty += collectData.ScrapQty;
                }
                if (result.RemainQty <= 0)
                {
                    result.BatchState = BatchState.Out;
                    wipBatch.BatchState = BatchState.Out;
                    ////解绑
                    if (!result.ContainerNo.IsNullOrEmpty() && GetUnbindMode() == UnbindMode.MoveOut)
                        InputContainerUnBind(result.ContainerNo);
                }
                result.PersistenceStatus = PersistenceStatus.Modified;
            });
            RF.Save(wipBatchs);
            RF.Save(inputBatchs);
        }

        /// <summary>
        /// 转入载具解绑
        /// </summary>
        /// <param name="containerNo">载具号</param>
        private void InputContainerUnBind(string containerNo)
        {
            var relations = BatchController.GetBatchRelations(containerNo, BarcodeType.ContainerNo);
            relations.ForEach(f =>
            {
                RuntimeController.UnmapPuid(new CollectBarcode() { Code = f.ContainerNo, Type = BarcodeType.ContainerNo });
                f.ContainerNo = string.Empty;
                f.PersistenceStatus = PersistenceStatus.Modified;
            });
            RF.Save(relations);
            var box = GetRelationContainer(containerNo);
            box.State = BoxState.Unused;
            RF.Save(box);
        }

        /// <summary>
        /// 验证出站条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="type">条码类型</param>
        public virtual void ValidateOutputBarcode(string barcode, BarcodeType type)
        {
            switch (type)
            {
                case BarcodeType.BatchBarocde:
                    var batch = RT.Service.Resolve<WipBatchController>().GetWipBatch(barcode);
                    if (batch == null)
                        throw new ValidationException("批次条码[{0}]不存在".L10nFormat(barcode));
                    if (batch.IsScraped)
                        throw new ValidationException("批次条码[{0}]已经报废".L10nFormat(barcode));
                    break;
                case BarcodeType.ContainerNo:
                    var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
                    var box = RT.Service.Resolve<BoxController>().GetTurnoverBox(barcode, config.BoxType);
                    if (box == null)
                        throw new ValidationException("载具[{0}]不存在".L10nFormat(barcode));
                    if (box.State == BoxState.Scrap)
                        throw new ValidationException("载具[{0}]已经报废".L10nFormat(barcode));
                    if (box.State == BoxState.Inuse || BatchController.IsContainerRelation(barcode))
                        throw new ValidationException("载具[{0}]已经被使用".L10nFormat(barcode));
                    break;
                default:
                    throw new ValidationException("转出条码[{0}]类型错误".L10nFormat(barcode));
            }
        }

        /// <summary>
        /// 验证转入条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <exception cref="ValidationException">条码不存在、条码已报废</exception>
        /// <exception cref="ArgumentNullException">条码为空、工作单元为空</exception>
        public virtual void ValidateMoveInBatch(CollectBarcode barcode, Workcell workcell)
        {
            if (barcode == null)
                throw new ArgumentNullException(nameof(barcode));
            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));
            switch (barcode.Type)
            {
                case BarcodeType.BatchBarocde:
                    ValidateBatchBarcode(barcode);
                    break;
                case BarcodeType.ContainerNo:
                    var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
                    var box = RT.Service.Resolve<BoxController>().GetTurnoverBox(barcode.Code, config.BoxType);
                    if (box == null)
                        throw new ValidationException("[{0}]不存在".L10nFormat(barcode));
                    if (box.State == BoxState.Scrap)
                        throw new ValidationException("[{0}]已经报废".L10nFormat(barcode));
                    break;
                default:
                    throw new ValidationException("不支持{0}".L10nFormat(barcode.Type.ToLabel()));
            }
        }

        /// <summary>
        /// 验证批次
        /// </summary>
        /// <param name="barcode"></param>
        private void ValidateBatchBarcode(CollectBarcode barcode)
        {
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(barcode.Code);
            if (wipBatch == null)
                throw new ValidationException("[{0}]不存在".L10nFormat(barcode));
            if (wipBatch.Qty == 0)
                throw new ValidationException("[{0}]剩余数量为0".L10nFormat(barcode));
            if (wipBatch.IsScraped)
                throw new ValidationException("[{0}]已经报废".L10nFormat(barcode));
            if (wipBatch.IsGenerateChild)
                throw new ValidationException("[{0}]已生成子批次号，请扫描子批次号".L10nFormat(barcode));
            var inputBatch = BatchController.GetInputBatch(barcode.Code, wipBatch.IsChild);
            if (inputBatch != null && inputBatch.BatchState == BatchState.In)
                throw new ReMoveInException("批次条码[{0}]已入站，入站工序[{1}]、工位[{2}]".L10nFormat(barcode.Code, inputBatch?.Process?.Name, inputBatch?.Station?.Name), inputBatch.WorkOrderId);
        }

        /// <summary>
        /// 生成子批次号
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <returns>子批次</returns>
        public virtual SubWipBatch GenerateChildBatch(double workOrderId)
        {
            var setting = BatchController.GetOrCreateBatchPrintSetting(workOrderId);
            ValidateSetting(setting);
            var workOrder = RF.GetById<WorkOrder>(workOrderId);
            if (workOrder == null)
                throw new EntityNotFoundException(typeof(WorkOrder), workOrderId);
            var barcodes = RT.Service.Resolve<NumberRuleController>().GenerateSegment(setting.NumberRule.Id, 1, workOrder);
            var barcode = new SubWipBatch()
            {
                WorkOrderId = workOrderId,
                BatchNo = barcodes.FirstOrDefault(),
                Qty = setting.Qty == null ? 0 : setting.Qty.Value,
                BoxesQty = setting.Qty == null ? 0 : setting.Qty.Value,
                PrintDate = DateTime.Now,
                BatchState = BatchState.Generated,
                IsChild = true,
            };
            barcode.GenerateId();
            return barcode;
        }

        /// <summary>
        /// 验证批次打印设置
        /// </summary>
        /// <param name="setting">批次打印设置</param>
        private void ValidateSetting(BatchPrintSetting setting)
        {
            if (!setting.NumberRuleId.HasValue)
                throw new ValidationException("批次打印设置未设置编码规则，请配置".L10N());
            if (setting.PrintControl && !setting.PrintTemplateId.HasValue)
                throw new ValidationException("批次打印设置未设置打印模板，请配置".L10N());
        }

        /// <summary>
        /// 获取载具解绑方式
        /// </summary>
        /// <returns>载具解绑方式</returns>
        UnbindMode GetUnbindMode()
        {
            var config = ConfigService.GetConfig(new ContainerUnbindConfig());
            if (config == null)
                throw new ValidationException("未找到载具解绑配置，请检查全局配置项".L10N());
            return config.UnbindMode;
        }

        /// <summary>
        /// 跳到指定站
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="toProcessId">目标工序</param>
        /// <param name="workcell">工作单元</param> 
        /// <exception cref="ValidationException">产品未上线，已下线、产品工艺路线不存在跳站工序</exception>
        /// <exception cref="EntityNotFoundException">工序不存在</exception>
        protected virtual void BatchGoto(CollectBarcode barcode, double toProcessId, Workcell workcell)
        {
            //var process = GetById<Process>(toProcessId);
            //if (process == null)
            //    throw new EntityNotFoundException(typeof(Process), toProcessId);

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var relation = BatchController.GetBatchRelation(barcode);
                if (relation == null)
                    throw new ValidationException("未找到[{0}]批次关系".L10nFormat(barcode.Code));
                if (relation.IsFinish)
                    throw new ValidationException("[{0}]已下线,不能跳站".L10nFormat(barcode));
                var product = RuntimeController.FindProduct(barcode);
                if (product == null)
                {
                    var version = FindLastBatchWipProductVersion(relation.Bid);
                    if (version == null)
                        throw new ValidationException("[{0}]未上线,不能跳站".L10nFormat(barcode));
                    product = RecoverProduct(version);
                }

                var fromProcessId = product.Routing.Current.ProcessId.Value;
                var toProcess = product.Routing.Processes.FirstOrDefault(p => p.Id == toProcessId);

                if (toProcess == null)
                {
                    throw new ValidationException("跳站失败,当前产品工艺路线没有工序[{0}]".L10nFormat(toProcessId));
                }
                if (!toProcess.ProcessId.HasValue)
                {
                    throw new ValidationException("跳站失败,当前产品工艺路线没有工序[{0}]".L10nFormat(toProcess.ProcessId));
                }

                product.Routing.Next.Clear();
                product.Routing.Next.Add(toProcess.Id);

                //保存采集运行时产品
                RuntimeController.Save(product);
                LogGoto(fromProcessId, toProcess.ProcessId.Value, workcell);
                tran.Complete();
            }
        }

        /// <summary>
        /// 获取转入信息
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<InputBatch> GetInputBatchById(List<double> ids)
        {
            return ids.SplitContains(tempIds =>
            {
                return Query<InputBatch>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
        }

        /// <summary>
        /// 获取当前工作单元的对应id转入批次
        /// </summary>
        /// <param name="id"></param>
        /// <param name="workcell"></param>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual InputBatch GetInputBatchById(double id, Workcell workcell, double woId)
        {
            return Query<InputBatch>().Where(p => p.Id == id && p.ResourceId == workcell.ResourceId && p.StationId == workcell.StationId && p.ProcessId == workcell.ProcessId && p.WorkOrderId == woId && p.RemainQty > 0).FirstOrDefault();
        }

        /// <summary>
        /// 获取当前工作单元的对应id转入批次
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="workcell"></param>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual EntityList<InputBatch> GetInputBatchById(List<double> ids, Workcell workcell, double woId)
        {
            return ids.SplitContains(id =>
            {
                return Query<InputBatch>().Where(p => id.Contains(p.Id) && p.ResourceId == workcell.ResourceId && p.StationId == workcell.StationId && p.ProcessId == workcell.ProcessId && p.WorkOrderId == woId && p.RemainQty > 0).ToList();

            });
        }

        /// <summary>
        /// 拆分合并保存信息
        /// </summary>
        /// <param name="batchWipProductVersions">父生产通用报表</param>
        /// <param name="childBatchVersion">子生产通用报表</param>
        /// <param name="childRelation">子批次关系</param>
        /// <param name="inputBatchs">转入批次</param>
        /// <param name="pWipBatch">原生成子批</param>
        /// <param name="childWipBatch">新生成子批</param>
        /// <param name="product">运行时</param>
        /// <param name="batchRelations">批次关系</param>
        /// <param name="batchSplitMergeRecords">拆分合并记录</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="defects">缺陷</param>
        /// <param name="ngQty">不良数量</param>
        public virtual void SplitMergeSaveInfo(EntityList<BatchWipProductVersion> batchWipProductVersions, BatchWipProductVersion childBatchVersion, BatchRelation childRelation, EntityList<InputBatch> inputBatchs, EntityList<WipBatch> pWipBatch, WipBatch childWipBatch, product product, EntityList<BatchRelation> batchRelations, EntityList<BatchSplitMergeRecord> batchSplitMergeRecords, Workcell workcell, EntityList<Defect> defects = null, decimal? ngQty = null)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(batchWipProductVersions);
                RF.Save(childBatchVersion);
                RF.Save(childRelation);
                RF.Save(inputBatchs);
                RF.Save(pWipBatch);
                RF.Save(childWipBatch);
                RT.Service.Resolve<RuntimeController>().Save(product);
                RF.Save(batchRelations);
                RF.Save(batchSplitMergeRecords);
                if (childWipBatch.BatchState == BatchState.In)
                {
                    var collectBarcode = new CollectBarcode { Code = childWipBatch.BatchNo, Type = BarcodeType.BatchBarocde };
                    RT.Service.Resolve<BatchAssemblyController>().MoveIn(collectBarcode, workcell, defects, ngQty);
                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 移除命令
        /// </summary>
        /// <param name="batchWipProductVersion"></param>
        /// <param name="wipBatch"></param>
        /// <param name="input"></param>
        /// <param name="workcell"></param>
        public virtual void RemoveInput(BatchWipProductVersion batchWipProductVersion, WipBatch wipBatch, InputBatch input, Workcell workcell)
        {
            // 更新生产通用报表和批次生成的报废数量(回溯)
            input.PersistenceStatus = PersistenceStatus.Deleted;

            batchWipProductVersion.ScrapQty -= input.ScrapQty;
            batchWipProductVersion.RemainQty += input.ScrapQty;

            wipBatch.ScrapQty -= input.ScrapQty;
            if (input.ScrapQty > 0)
            {
                wipBatch.EditQtyProcessCode = batchWipProductVersion.Process?.Code;
            }
            wipBatch.Qty += input.ScrapQty;
            wipBatch.BatchState = BatchState.Removed;
            // 班次
            var shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(workcell.ResourceId, DateTime.Now);
            if (shift == null)
            {
                throw new ValidationException("当前生产资源班次为空！".L10N());
            }
            BatchWipRecord batchWipRecord = new BatchWipRecord
            {
                BatchVersionId = batchWipProductVersion.Id,
                BatchNo = input.BatchNo,
                InOutType = PlugType.Remove,
                Qty = input.RemainQty,
                ShiftId = shift.Id,
                ResourceId = workcell.ResourceId,
                ProcessId = workcell.ProcessId,
                StationId = workcell.StationId,
                ResultType = ResultType.Pass,
            };

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(batchWipProductVersion);
                RF.Save(wipBatch);
                RF.Save(input);
                RF.Save(batchWipRecord);
                tran.Complete();
            }
        }

        /// <summary>
        /// 报废转入
        /// </summary>
        /// <param name="pBatchVersion"></param>
        /// <param name="pWipBatch"></param>
        /// <param name="input"></param>
        /// <param name="scrapQty"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void ScrapInput(BatchWipProductVersion pBatchVersion, WipBatch pWipBatch, InputBatch input, decimal scrapQty)
        {
            // 转入更新
            input.ScrapQty += scrapQty;
            input.RemainQty -= scrapQty;

            // 生产通用报表更新
            pBatchVersion.ScrapQty += scrapQty;
            pBatchVersion.RemainQty -= scrapQty;

            if (pWipBatch.ScrapQty == null)
            {
                pWipBatch.ScrapQty = 0;
            }
            pWipBatch.ScrapQty += scrapQty;
            if (scrapQty > 0)
            {
                pWipBatch.EditQtyProcessCode = pBatchVersion.Process?.Code;
            }
            pWipBatch.Qty -= scrapQty;

            if (input.RemainQty <= 0)
            {
                input.PersistenceStatus = PersistenceStatus.Deleted;
                pWipBatch.BatchState = BatchState.Removed;
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(pBatchVersion);
                RF.Save(pWipBatch);
                RF.Save(input);
                tran.Complete();
            }
        }

        /// <summary>
        /// 扫描转出匹配转入
        /// </summary>
        /// <param name="batchNo"></param>
        /// <param name="workcell"></param>
        /// <param name="workOrderId"></param>
        public virtual InputBatch MatchInputBatch(string batchNo, Workcell workcell, double workOrderId)
        {
            var input = Query<InputBatch>().Where(p => p.BatchNo == batchNo && p.ResourceId == workcell.ResourceId && p.ProcessId == workcell.ProcessId && p.StationId == workcell.StationId && p.WorkOrderId == workOrderId).FirstOrDefault();
            if (input == null)
            {
                throw new ValidationException("批次{0}未转入".L10nFormat(batchNo));
            }
            return input;
        }

        /// <summary>
        /// 工位批次列表新生成子批次(拆分)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="workcell"></param>
        /// <param name="splitQty"></param>
        /// <param name="type"></param>
        /// <param name="defects"></param>
        /// <param name="isNg"></param>
        public virtual void NewGenerateSplitInput(InputBatch input, Workcell workcell, decimal splitQty, BarcodeType type, EntityList<Defect> defects = null, bool isNg = false)
        {
            // 
            input = RT.Service.Resolve<WipController>().GetInputBatchById(input.Id, workcell, input.WorkOrderId);
            var process = RF.GetById<Process>(workcell.ProcessId, new EagerLoadOptions().LoadWithViewProperty());
            if (input == null)
            {
                throw new ValidationException("转入批次为空或已转出".L10N());
            }
            decimal ngQty = 0;
            if (isNg)
            {
                ngQty = splitQty;
            }
            // 验证生产通用版本
            var pBatchVersion = RT.Service.Resolve<BatchWipProductVersionController>().GetBatchWipProductByNo(input.BatchNo);
            var wo = RT.Service.Resolve<WorkOrderController>().GetWorkOrdersByWoIds(new List<double> { pBatchVersion.WorkOrderId }).FirstOrDefault();
            if (wo == null)
            {
                throw new ValidationException("批次产品工单不存在");
            }
            if (wo.IsPause == YesNo.Yes)
            {
                throw new ValidationException("批次产品工单已暂停，不允许生产");
            }
            if (wo.State == Core.WorkOrders.WorkOrderState.Finish || wo.State == Core.WorkOrders.WorkOrderState.Close || wo.State == Core.WorkOrders.WorkOrderState.CancelRelease)
            {
                throw new ValidationException("批次产品工单状态不为生产或发放，不允许生产");
            }
            if (pBatchVersion == null)
            {
                throw new ValidationException("批次产品不存在");
            }
            if (pBatchVersion.RemainQty <= 0)
            {
                throw new ValidationException("批次{0}剩余数量小于等于0，不能拆分".L10nFormat(pBatchVersion.BatchNo));
            }
            if (pBatchVersion.IsFinish)
            {
                throw new ValidationException("批次{0}已完工，不能拆分".L10nFormat(pBatchVersion.BatchNo));
            }
            if (pBatchVersion.RemainQty < splitQty)
            {
                throw new ValidationException("批次{0}剩余数量{1}小于拆分数量{2}，不能拆分".L10nFormat(pBatchVersion.BatchNo, pBatchVersion.RemainQty, splitQty));
            }

            // 父批数量减少，记录减少
            pBatchVersion.RemainQty -= splitQty;
            input.RemainQty -= splitQty;

            //生成wipBatch
            var pWipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatches(new List<string> { input.BatchNo }).FirstOrDefault();
            var config = RT.Service.Resolve<BatchManageController>().GetOrCreateBatchPrintSetting(input.WorkOrderId);
            if (config == null || config.NumberRuleId == 0 || config.NumberRuleId == null)
            {
                throw new ValidationException("未找到批次打印设置".L10N());
            }
            var childBarcode = RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.NumberRule.Id, 1).FirstOrDefault();
            var childWipBatch = JsonConvert.DeserializeObject<WipBatch>(JsonConvert.SerializeObject(pWipBatch));
            childWipBatch.GenerateId();
            childWipBatch.BatchList.Clear();
            childWipBatch.BatchNo = childBarcode;
            childWipBatch.RemainQty = childWipBatch.Qty = splitQty;
            childWipBatch.ScrapQty = null;
            childWipBatch.PersistenceStatus = PersistenceStatus.New;
            childWipBatch.RangeId = null;

            //创建批次关系
            var childRelation = RT.Service.Resolve<BatchManageController>().CreateSubWipBatchRelationNoSave(childWipBatch.BatchNo, childWipBatch.BatchNo, childWipBatch.Qty, childWipBatch.WorkOrderId, null, isNg);
            //复制拆分前的运行时
            var oldRuntimeProduct = RT.Service.Resolve<RuntimeController>().FindProduct(input.BatchNo, Core.Barcodes.BarcodeType.BatchBarocde);
            //克隆运行时
            var childRuntimeProduct = oldRuntimeProduct.Clone();
            childRuntimeProduct.Qty = splitQty;
            //创建一个PUID用于后面与拆分批次号关联
            var puid = Guid.NewGuid().ToString("N").ToUpper();
            //复制新生成的拆分批次号的Puid为新的Puid
            childRuntimeProduct.Puid = puid;
            //创建新条码的工艺路线布局
            RT.Service.Resolve<WipController>().SaveBatchRuntimeRouting(childWipBatch.WorkOrderId, childBarcode, pWipBatch.BatchNo);
            var runtimeController = RT.Service.Resolve<RuntimeController>();
            //拆分批次号与产品运行时Puid做关联
            runtimeController.MapPuid(new CollectBarcode() { Code = childBarcode, Type = BarcodeType.BatchBarocde }, puid);

            //复制原批次版本数据生成新的拆分批次
            BatchWipProductVersion childBatchVersion = CopyWipProductVersion(childBarcode, workcell, pBatchVersion, pWipBatch, splitQty);
            if (isNg)
            {
                childBatchVersion.DefectState = BatchWIP.Products.SplitAndMerge.Enums.QState.UnPass;
            }
            // 更新wipBatch
            pWipBatch.RemainQty -= splitQty;
            if (splitQty > 0)
            {
                pWipBatch.EditQtyProcessCode = process?.Code;
            }
            pWipBatch.Qty -= splitQty;

            // 更新batchRelaiton
            EntityList<BatchRelation> batchRelations = RT.Service.Resolve<BatchManageController>().GetBatchRelations(new List<CollectBarcode> { new CollectBarcode()
                        {
                            Code = pBatchVersion.BatchNo,
                            BarcodeType = BarcodeType.BatchBarocde,
                            Type = BarcodeType.BatchBarocde
                        }});
            foreach (var item in batchRelations)
            {
                item.RemainQty -= splitQty;
                item.Qty -= splitQty;
                item.PersistenceStatus = PersistenceStatus.Modified;
            }

            EntityList<BatchSplitMergeRecord> batchSplitMergeRecords = new EntityList<BatchSplitMergeRecord>();
            // 创建一条批次合并拆分记录
            var batchSplitMerge = new BatchSplitMergeRecord()
            {
                BatchOperationType = BatchOperationType.Split,
                InputBatchNo = input.BatchNo,
                InputQty = splitQty,
                OutputBatchNo = childBarcode,
                VersionId = pBatchVersion.Id,
                OutputQty = splitQty,
            };
            batchSplitMergeRecords.Add(batchSplitMerge);

            // 保存
            RT.Service.Resolve<WipController>().SplitMergeSaveInfo(new EntityList<BatchWipProductVersion> { pBatchVersion }, childBatchVersion, childRelation, new EntityList<InputBatch> { input }, new EntityList<WipBatch> { pWipBatch }, childWipBatch, childRuntimeProduct, batchRelations, batchSplitMergeRecords, workcell, defects, ngQty);

        }

        /// <summary>
        /// 复制旧的批次版本数据到新的合并批次号
        /// </summary>
        /// <param name="childBatchNo"></param>
        /// <param name="workcell"></param>
        /// <param name="pBatchVersion"></param>
        /// <param name="pWipBatch"></param>
        /// <param name="splitQty"></param>
        /// <returns>返回复制后的合并批次号生产版本记录</returns>
        private BatchWipProductVersion CopyWipProductVersion(string childBatchNo, Workcell workcell, BatchWipProductVersion pBatchVersion, WipBatch pWipBatch, decimal splitQty)
        {
            var mergeBarcodeWipProductVersion = JsonConvert.DeserializeObject<BatchWipProductVersion>(JsonConvert.SerializeObject(pBatchVersion));
            mergeBarcodeWipProductVersion.BatchNo = childBatchNo;
            mergeBarcodeWipProductVersion.CurrentProcessId = null;
            mergeBarcodeWipProductVersion.ResourceId = workcell.ResourceId;
            mergeBarcodeWipProductVersion.StationId = workcell.StationId;
            mergeBarcodeWipProductVersion.ScrapQty = 0;
            mergeBarcodeWipProductVersion.Qty = splitQty;
            mergeBarcodeWipProductVersion.RemainQty = splitQty;
            mergeBarcodeWipProductVersion.GenerateId();
            mergeBarcodeWipProductVersion.PersistenceStatus = PersistenceStatus.New;

            //更新缺陷ID指向
            mergeBarcodeWipProductVersion.DefectList.Clear();
            mergeBarcodeWipProductVersion.RepaireList.Clear();
            mergeBarcodeWipProductVersion.ProcessList.Clear();
            return mergeBarcodeWipProductVersion;
        }
    }
}