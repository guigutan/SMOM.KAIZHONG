using SIE.Barcodes.WipBatchs;
using SIE.Common;
using SIE.Common.NumberRules;
using SIE.Common.Sort;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP;
using SIE.MES.WIP.Packings;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.BatchWIP.BatchPackings
{
    /// <summary>
    /// 批次包装采集控制器
    /// </summary>
    public class BatchWipPackingController : WipPackingController
    {
        private const string BATCH_PACKED = "BatchPacked";
        #region 正常
        #region 打包（主单位）
        /// <summary>
        /// 自动打包
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="workcell">工作单元</param>
        public virtual void AutoBatchPacking(double workOrderId, Workcell workcell)
        {
            var inputBatchList = RT.Service.Resolve<BatchManageController>().GetInputBatchs(workcell.ResourceId, workcell.ProcessId, workcell.StationId, workOrderId);
            inputBatchList.ForEach(p => p.SplitQty = p.RemainQty);

            ////var batchPackingRelas = new EntityList<BatchPackingRelation>();
            var batchPackingRelas = new List<EntityList<BatchPackingRelation>>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                inputBatchList.GroupBy(p => p.WipBatchId).ForEach(p =>
                {
                    var batchList = new EntityList<InputBatch>();
                    batchList.AddRange(p.ToList());
                    batchPackingRelas.Add(DoBatchPacking(batchList, workcell));
                });

                tran.Complete();
            }

            //发送消息队列，通知自动打印条码
            //发送事件总线，成品入库
            if (batchPackingRelas.Count > 0)
            {
                foreach (var relas in batchPackingRelas)
                {
                    if (relas.Count > 0)
                    {
                        RT.EventBus.Publish(new Packages.Packings.DoPackingEvent(Packages.Packings.DoPackingAction.Packed, BATCH_PACKED, relas.ToArray()));
                        RT.RemotingEventBus.Publish(new PackingEvent(workcell.StationId, relas.Select(p => p.Id).ToArray()));
                    }
                }
            }
        }

        /// <summary>
        /// 打包操作（主单位打包操作）
        /// </summary>
        /// <param name="inputBatchList">待打包的批次列表</param>
        /// <param name="workCell">生产单元</param>
        /// <param name="isfullBox">是否满箱包装</param>
        /// <param name="sendEvent">是否发送入库和打印消息</param>
        /// <returns>批次包装关系列表</returns>
        public virtual EntityList<BatchPackingRelation> DoBatchPacking(EntityList<InputBatch> inputBatchList, Workcell workCell, bool isfullBox = true, bool sendEvent = false)
        {
            ValidatePacking(inputBatchList, workCell);

            //当前打包的工单
            WorkOrder workOrder = inputBatchList.FirstOrDefault()?.WorkOrder;
            if (workOrder == null)
            {
                throw new ValidationException("无法找到批次对应工单，请确保数据是否被修改！".L10N());
            }
            if (workOrder.PackageRuleDetailList == null || workOrder.PackageRuleDetailList.Count == 0)
            {
                throw new ValidationException("工单[{0}]没有设置包装规则关系,请设置后重新打开批次包装执行操作！".L10nFormat(workOrder.No));
            }
            var masterUnit = workOrder.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).FirstOrDefault();
            if (masterUnit == null || masterUnit.PackageUnit == null || !masterUnit.PackageUnit.IsMasterUnit)
            {
                throw new ValidationException("未维护工单[{0}]包装规则主单位,请确保主单位已经维护并且是第一个！".L10nFormat(workOrder.No));
            }

            var numberRuleId = masterUnit.NumberRuleId;
            if (!numberRuleId.HasValue)
            {
                throw new ValidationException("工单[{0}]未维护主单位条码规则，请维护！".L10nFormat(workOrder.No));
            }
            if (masterUnit.NumberRule == null || masterUnit.NumberRule.DetailList.Count <= 0)
            {
                throw new ValidationException("工单[{0}]主单位条码规则未维护规则明细，请维护！".L10nFormat(workOrder.No));
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                EntityList<SubWipBatch> subWipBatchList = new EntityList<SubWipBatch>();
                EntityList<BatchPackingRelation> batchPkgRelations = new EntityList<BatchPackingRelation>();

                var packingQty = inputBatchList.Sum(p => p.SplitQty);
                var packageCount = 0M;
                if (isfullBox)
                {
                    packageCount = (int)(packingQty / masterUnit.Qty);
                }
                else
                {
                    packageCount = packingQty / masterUnit.Qty;
                }

                for (int i = 0; i < packageCount; i++)
                {
                    var subBatch = GenerateSubBatch(inputBatchList, workOrder, masterUnit, numberRuleId);
                    subWipBatchList.Add(subBatch);

                    var batchPackingRelation = GenerateBatchPackingRelation(masterUnit, workOrder, subBatch, workCell, subBatch.Qty);
                    batchPkgRelations.Add(batchPackingRelation);
                    AutoMoveOut(workCell, subBatch, batchPackingRelation.Id);
                }

                inputBatchList.ForEach(p => p.SplitQty = 0);
                RF.Save(inputBatchList);
                RF.Save(subWipBatchList);
                RF.Save(batchPkgRelations);
                tran.Complete();

                //发送消息队列，通知自动打印条码
                //发送事件总线，成品入库
                if (sendEvent && batchPkgRelations.Count > 0)
                {
                    RT.EventBus.Publish(new Packages.Packings.DoPackingEvent(Packages.Packings.DoPackingAction.Packed, BATCH_PACKED, batchPkgRelations.ToArray()));
                    RT.RemotingEventBus.Publish(new PackingEvent(workCell.StationId, batchPkgRelations.Select(p => p.Id).ToArray()));
                }

                return batchPkgRelations;
            }
        }

        /// <summary>
        /// 打包验证
        /// </summary>
        /// <param name="inputBatchList">待打包批次</param>
        /// <param name="workCell">生产单元</param>
        private void ValidatePacking(EntityList<InputBatch> inputBatchList, Workcell workCell)
        {
            if (inputBatchList.Count == 0)
                return;

            if (workCell.ResourceId <= 0)
                throw new ArgumentNullException(nameof(workCell.ResourceId), "请指定一个合法的资源,resourceId = {0}".L10nFormat(workCell.ResourceId));
            if (workCell.ProcessId <= 0)
                throw new ArgumentNullException(nameof(workCell.ProcessId), "请指定一个合法的工序,processId = {0}".L10nFormat(workCell.ProcessId));
            if (workCell.StationId <= 0)
                throw new ArgumentNullException(nameof(workCell.StationId), "请指定一个合法的工位,stationId = {0}".L10nFormat(workCell.StationId));

            if (inputBatchList.Any(p => p.RemainQty < p.SplitQty))
                throw new ValidationException("拆分数量必须小于等于剩余数量！".L10N());
            if (inputBatchList.Sum(p => p.SplitQty) <= 0)
                throw new ValidationException("包装数量不能等于0，请修改拆分数量！".L10N());
            if (inputBatchList.GroupBy(p => p.WipBatchId).Count() > 1)
                throw new ValidationException("不同生产批次的子批次之间不能一起打包！".L10N());
            if (inputBatchList.GroupBy(p => p.WorkOrderId).Count() > 1)
                throw new ValidationException("不同生产工单的子批次之间不能一起打包！".L10N());
            var wipBatchNo = inputBatchList.FirstOrDefault().WipBatch.BatchNo;
            if (RT.Service.Resolve<BatchManageController>().IsWipBatchRelationStoped(wipBatchNo, BarcodeType.BatchBarocde))
                throw new ValidationException("[{0}]产品已暂停，不能继续生产".L10nFormat(wipBatchNo));
        }

        /// <summary>
        /// 创建子批（数量为默认为主单位数量），并记录批次关系
        /// </summary>
        /// <param name="inputBatchList">待打包批次</param>
        /// <param name="workOrder">工单</param>
        /// <param name="masterUnit">主单位</param>
        /// <param name="numberRuleId">条码规则</param>
        /// <returns>生产子批次</returns>
        private SubWipBatch GenerateSubBatch(EntityList<InputBatch> inputBatchList, WorkOrder workOrder, WorkOrderPackageRuleDetail masterUnit, double? numberRuleId)
        {
            SubWipBatch wipBatch = new SubWipBatch
            {
                BatchNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(numberRuleId.Value, 1, workOrder).FirstOrDefault(),
                WorkOrderId = workOrder.Id,
                WipBatchId = inputBatchList.FirstOrDefault().WipBatchId,
                BoxesQty = masterUnit.Qty,
                IsChild = true,
                IsGenerateChild = false,
                BatchState = BatchState.Generated,
            };

            wipBatch.GenerateId();

            //拆合批关系
            EntityList<BatchRelation> relationBactches = new EntityList<BatchRelation>();

            //当前包装产品数
            decimal qty = 0M;
            foreach (var inputBatch in inputBatchList.Where(p => p.SplitQty > 0).OrderBy(p => p.InputDate))
            {
                //满包装缺产品数
                var lackQtyInBox = masterUnit.Qty - qty;
                if (lackQtyInBox == 0)
                    break;

                //拆分数量
                decimal splitQty = 0;

                if (lackQtyInBox >= inputBatch.SplitQty)
                {
                    splitQty = inputBatch.SplitQty;

                    qty += splitQty;
                    inputBatch.SplitQty = 0;
                    inputBatch.RemainQty -= splitQty;
                }
                else
                {
                    splitQty = lackQtyInBox;

                    qty = masterUnit.Qty;
                    inputBatch.RemainQty -= splitQty;
                    inputBatch.SplitQty = inputBatch.SplitQty <= inputBatch.RemainQty ? inputBatch.SplitQty : inputBatch.RemainQty;
                }

                relationBactches.Add(new BatchRelation
                {
                    Bid = wipBatch.BatchNo,
                    Pid = inputBatch.SubBatchNo.IsNullOrEmpty() ? inputBatch.BatchNo : inputBatch.SubBatchNo,
                    WipBatch = inputBatch.BatchNo,
                    WorkOrderId = workOrder.Id,
                    Qty = splitQty,
                    RemainQty = inputBatch.RemainQty,
                });
            }

            wipBatch.Qty = qty;

            if (relationBactches.Count > 1)
            {
                relationBactches.ForEach(p => p.BatchSource = BatchSource.Merge);
            }
            else
            {
                relationBactches.ForEach(p => p.BatchSource = BatchSource.Split);
            }

            RF.Save(relationBactches);

            var brokenList = wipBatch.Validate(SubWipBatch.BatchNoProperty, ValidatorActions.StopOnFirstBroken);
            if (brokenList.Count > 0)
            {
                var rule = brokenList.FirstOrDefault().Rule.ValidationRule as DynamicEntityValidationRule;
                if (rule.EntityRule is DynamicNotDuplicateRule)
                {
                    throw new ValidationException("主单位的编码规则配置了和其他编码规则相同的规则明细，请修改对应的规则明细！".L10N());
                }
                else
                {
                    throw new ValidationException(brokenList.ToString("；", MetaModel.RuleLevel.Error));
                }
            }

            RF.Save(wipBatch);
            return wipBatch;
        }

        /// <summary>
        /// 自动转出
        /// </summary>
        /// <param name="workCell">工作单元</param>
        /// <param name="subBatch">子批</param>
        /// <param name="relationId">包装关系Id</param>
        private void AutoMoveOut(Workcell workCell, SubWipBatch subBatch, double relationId)
        {
            //记录过站数据
            var collectData = InitCollectData(subBatch, workCell);
            BatchMoveOut(collectData, workCell);
            var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(subBatch.BatchNo);
            if (itemLabel != null)
            {
                itemLabel.RelationId = relationId;
                RF.Save(itemLabel);
            }
        }

        /// <summary>
        /// 初始化采集数据
        /// </summary>
        /// <param name="subBatch">新生成子批次</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>采集数据</returns>
        private CollectData InitCollectData(SubWipBatch subBatch, Workcell workcell)
        {
            var outputBatch = new OutputBatch()
            {
                BatchNo = subBatch.WipBatch.BatchNo,
                SubBatchNo = subBatch.BatchNo,
                Qty = subBatch.Qty,
                WorkOrderId = subBatch.WorkOrderId,
                SubWipBatch = subBatch,
                IsGenerateBatch = true,
                BarcodeType = BarcodeType.BatchBarocde
            };

            var inputBatches = RT.Service.Resolve<BatchManageController>().GetCurrentInputBatchs(workcell.ResourceId, workcell.ProcessId, workcell.StationId, subBatch.WorkOrderId);

            var batchRelas = RT.Service.Resolve<BatchManageController>().GetBatchRelations(subBatch.BatchNo, BarcodeType.BatchBarocde);
            batchRelas.ForEach(p =>
            {
                var inputBatch = inputBatches.FirstOrDefault(q => q.BatchNo == p.Pid || q.SubBatchNo == p.Pid);
                outputBatch.RelationBatchList.Add(new RelationBatch() { InputBatch = inputBatch, Qty = p.Qty });
            });

            var collectedBarcode = new CollectBarcode
            {
                Code = outputBatch.SubBatchNo,
                Type = BarcodeType.BatchBarocde
            };

            var collectedData = new CollectData
            {
                CollectBarcode = collectedBarcode,
                OutputBatch = outputBatch,
                Result = ResultType.Pass,
                State = WIP.Products.WipProductProcessState.Finish
            };

            return collectedData;
        }
        #endregion

        #region 级联打包
        /// <summary>
        /// 级联打包 
        /// </summary>
        /// <param name="workOrder">当前工单</param>
        /// <param name="workCell">工作单元</param>
        public virtual void DoPkgPacking(WorkOrder workOrder, Workcell workCell)
        {
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder), "工单不能为空！".L10N());
            if (workOrder.PackageRuleDetailList == null || workOrder.PackageRuleDetailList.Count == 0)
                throw new ValidationException("工单[{0}]没有设置包装规则关系,请设置后重新打开批次包装执行操作！".L10nFormat(workOrder.No));
            var relationList = FindWorkPackingRelationByStation(workCell.ProcessId, workCell.StationId, workOrder.Id, false);
            if (relationList == null || relationList.Count <= 0)
                return;
            if (relationList.Any(p => p.WorkOrderId != relationList.FirstOrDefault()?.WorkOrderId))
                throw new ValidationException("待打包的包装的所属工单必须一致！".L10N());
            if (relationList.Any(p => p.ParentNo.IsNotEmpty()))
                throw new ValidationException("待打包的包装的父条码必须为空！".L10N());

            //Key:包装单位Id， Value:包装关系列表
            var relaDics = new Dictionary<double, List<BatchPackingRelation>>();
            relationList.GroupBy(p => p.PackageUnitId).OrderBy(p => SortExtension.GetIndex(workOrder.PackageRuleDetailList.FirstOrDefault(q => q.PackageUnitId == p.Key))).ForEach(p =>
            {
                relaDics[p.Key] = p.ToList();
            });

            var ruleDics = workOrder.PackageRuleDetailList.OrderBy(p => p.GetIndex()).ToDictionary(p => p.PackageUnitId);
            var newBatchRelas = new List<EntityList<BatchPackingRelation>>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                foreach (var dic in ruleDics)
                {
                    //若无父包装层级时，跳出循环
                    var parentRuleUnit = ruleDics.FirstOrDefault(t => t.Value.GetIndex() > dic.Value.GetIndex());
                    if (parentRuleUnit.Value == null)
                    {
                        break;
                    }
                    if (!relaDics.ContainsKey(dic.Key))
                    {
                        continue;
                    }

                    var rela = relaDics[dic.Key];

                    rela.GroupBy(q => q.PackingBatch).ForEach(q =>
                    {
                        EntityList<BatchPackingRelation> pkgRelas = new EntityList<BatchPackingRelation>();
                        pkgRelas.AddRange(q.OrderBy(p => p.CreateDate));
                        var parentRelas = DoPkgPackingByPackageUnit(pkgRelas, workCell);
                        newBatchRelas.Add(parentRelas);

                        if (relaDics.ContainsKey(parentRuleUnit.Key))
                        {
                            relaDics[parentRuleUnit.Key].AddRange(parentRelas);
                        }
                        else
                        {
                            relaDics[parentRuleUnit.Key] = parentRelas.ToList();
                        }
                    });
                }

                tran.Complete();
            }

            //发送消息队列，通知自动打印条码
            //发送事件总线，成品入库
            if (newBatchRelas.Count > 0)
            {
                foreach (var relas in newBatchRelas)
                {
                    if (relas.Count > 0)
                    {
                        RT.EventBus.Publish(new Packages.Packings.DoPackingEvent(Packages.Packings.DoPackingAction.Packed, BATCH_PACKED, relas.ToArray()));
                        RT.RemotingEventBus.Publish(new PackingEvent(workCell.StationId, relas.Select(p => p.Id).ToArray()));
                    }
                }
            }
        }

        /// <summary>
        /// 按包装单位打包 
        /// </summary>
        /// <param name="relationList">包装关系列表</param>
        /// <param name="workCell">工作单元</param>
        /// <param name="isfullBox">是否满箱包装</param>
        /// <param name="sendEvent">是否发送打印和出库消息</param>
        /// <returns>批次包装关系列表</returns>
        public virtual EntityList<BatchPackingRelation> DoPkgPackingByPackageUnit(EntityList<BatchPackingRelation> relationList, Workcell workCell, bool isfullBox = true, bool sendEvent = false)
        {
            ValidatePkgPacking(relationList, workCell);

            WorkOrder workOrder = RF.GetById<WorkOrder>(relationList.FirstOrDefault()?.WorkOrderId);
            if (workOrder == null)
                throw new ValidationException("包装对应的工单不存在，请确认后再打包！".L10N());

            WorkOrderPackageRuleDetail currRuleUnit = workOrder.PackageRuleDetailList.FirstOrDefault(p => p.PackageUnitId == relationList.FirstOrDefault()?.PackageUnitId);
            if (currRuleUnit == null)
                throw new ValidationException("包装对应的包装规则[{0}]未在工单维护，请确认后再打包！".L10nFormat(relationList.FirstOrDefault().PackageUnit.Name));

            WorkOrderPackageRuleDetail parentRuleUnit = workOrder.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).FirstOrDefault(p => SortExtension.GetIndex(p) > SortExtension.GetIndex(currRuleUnit));

            if (parentRuleUnit == null)
                throw new ValidationException("包装对应的包装层级已是最外层包装！".L10N());

            SubWipBatch subBatch = RF.GetById<SubWipBatch>(relationList.FirstOrDefault()?.PackingBatch);
            WipBatch midBatch = subBatch.IsChild ? subBatch.WipBatch : subBatch;

            //已打包的包装集合
            EntityList<BatchPackingRelation> pkgRelations = new EntityList<BatchPackingRelation>();
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //若是满箱包装时，取满箱包装整数；
                var pkgsCount = 0M;
                if (isfullBox)
                {
                    pkgsCount = (int)(relationList.Count / parentRuleUnit.LevelQty);
                }
                else
                {
                    pkgsCount = relationList.Count / parentRuleUnit.Qty;
                }

                //int pkgsCount = relationList.Count / (int)parentRuleUnit.LevelQty;
                for (int i = 0; i < pkgsCount; i++)
                {
                    var joinRelas = relationList.Skip((int)parentRuleUnit.LevelQty * i).Take((int)parentRuleUnit.LevelQty);
                    var qty = joinRelas.Sum(p => p.ItemQty);
                    var parentPkg = GenerateBatchPackingRelation(parentRuleUnit, workOrder, midBatch, workCell, qty);
                    joinRelas.ForEach(p =>
                    {
                        p.ParentNo = parentPkg.PackageNo;
                        p.TreePId = parentPkg.Id;
                    });

                    //尾数包装成未满的
                    if (pkgsCount - i < 1 && pkgsCount - i > 0)
                        parentPkg.PackedQty = relationList.Count % parentRuleUnit.LevelQty;

                    pkgRelations.Add(parentPkg);
                }

                RF.Save(pkgRelations);
                RF.Save(relationList);
                tran.Complete();
            }

            //发送消息队列，通知自动打印条码
            //发送事件总线，成品入库
            if (sendEvent && pkgRelations.Count > 0)
            {
                RT.EventBus.Publish(new Packages.Packings.DoPackingEvent(Packages.Packings.DoPackingAction.Packed, BATCH_PACKED, pkgRelations.ToArray()));
                RT.RemotingEventBus.Publish(new PackingEvent(workCell.StationId, pkgRelations.Select(p => p.Id).ToArray()));
            }

            return pkgRelations;
        }

        /// <summary>
        /// 验证级联打包
        /// </summary>
        /// <param name="relationList">包装关系列表</param>
        /// <param name="workCell">工作单元</param>
        private void ValidatePkgPacking(EntityList<BatchPackingRelation> relationList, Workcell workCell)
        {
            if (relationList.Count == 0)
                return;

            if (workCell.ResourceId <= 0)
                throw new ArgumentNullException(nameof(workCell.ResourceId), "请指定一个合法的资源,resourceId = {0}".L10nFormat(workCell.ResourceId));
            if (workCell.ProcessId <= 0)
                throw new ArgumentNullException(nameof(workCell.ProcessId), "请指定一个合法的工序,processId = {0}".L10nFormat(workCell.ProcessId));
            if (workCell.StationId <= 0)
                throw new ArgumentNullException(nameof(workCell.StationId), "请指定一个合法的工位,stationId = {0}".L10nFormat(workCell.StationId));

            if (relationList.Any(p => p.PackingBatch != relationList.FirstOrDefault()?.PackingBatch))
                throw new ValidationException("待打包的包装的生产批次必须一致！".L10N());
            if (relationList.Any(p => p.WorkOrderId != relationList.FirstOrDefault()?.WorkOrderId))
                throw new ValidationException("待打包的包装的所属工单必须一致！".L10N());
            if (relationList.Any(p => p.PackageUnitId != relationList.FirstOrDefault()?.PackageUnitId))
                throw new ValidationException("待打包的包装的包装单位必须一致！".L10N());
            if (relationList.Any(p => p.TreePId != null))
                throw new ValidationException("待打包的包装的父条码必须为空！".L10N());
        }
        #endregion

        /// <summary>
        /// 创建批次包装关系
        /// </summary>
        /// <param name="packageRuleUnit">主单位</param>
        /// <param name="workOrder">工单</param>
        /// <param name="batch">关联批次（主单位时则关联子批）</param>
        /// <param name="workCell">工作单元</param>
        /// <param name="qty">包装数量</param>
        /// <returns>批次包装关系</returns>
        private BatchPackingRelation GenerateBatchPackingRelation(WorkOrderPackageRuleDetail packageRuleUnit, WorkOrder workOrder, WipBatch batch, Workcell workCell, decimal qty)
        {
            if (!packageRuleUnit.NumberRuleId.HasValue)
                throw new ValidationException("工单[{0}]的[{1}]包装规则层级，未维护条码规则".L10nFormat(workOrder.No, packageRuleUnit.PackageUnit.Name));

            BatchPackingRelation packageRelation = new BatchPackingRelation
            {
                PackageNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(packageRuleUnit.NumberRuleId.Value, 1).FirstOrDefault(),
                WorkOrderId = workOrder.Id,
                PackageUnitId = packageRuleUnit.PackageUnitId,
                PackedQty = packageRuleUnit.LevelQty,
                ItemQty = qty,
                PackingBy = workCell.EmployeeId <= 0 ? RT.IdentityId : workCell.EmployeeId,
                State = LogisticState.UnPrinted,
                PackedDate = RF.Find<BatchPackingRelation>().GetDbTime(),
                ProcessId = workCell.ProcessId,
                StationId = workCell.StationId
            };

            if (batch.IsChild)
            {
                //packageRelation.PackageNo = batch.BatchNo;
                packageRelation.PackingBatch = (batch as SubWipBatch).WipBatchId.Value;
                packageRelation.BatchNo = (batch as SubWipBatch).WipBatch.BatchNo;
                packageRelation.Batch = (batch as SubWipBatch).WipBatch.BatchNo;
                packageRelation.ChildBatch = batch.Id;
                packageRelation.ChildBatchNo = batch.BatchNo;
            }
            else
            {
                //packageRelation.PackageNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(packageRuleUnit.NumberRuleId.Value, 1).FirstOrDefault();
                packageRelation.PackingBatch = batch.Id;
                packageRelation.BatchNo = batch.BatchNo;
                packageRelation.Batch = batch.BatchNo;
                packageRelation.ChildBatch = batch.Id;
                packageRelation.ChildBatchNo = batch.BatchNo;
            }

            packageRelation.GenerateId();
            packageRelation.RootId = packageRelation.Id;

            return packageRelation;
        }
        #endregion

        #region 加入
        /// <summary>
        /// 加入 - 条码（手动扫描）
        /// </summary>
        /// <param name="barcode">扫描条码</param>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="rela">目标包装</param>
        /// <param name="currentRuleDetail">当前层级</param>
        /// <param name="childRuleDetail">待加入包装层级</param>
        /// <param name="workcell">工作单元</param>
        public virtual void JoinWithBarcode(string barcode, double workOrderId, BatchPackingRelation rela, WorkOrderPackageRuleDetail currentRuleDetail, WorkOrderPackageRuleDetail childRuleDetail, Workcell workcell)
        {
            if (rela != null && rela.PackageUnit.IsMasterUnit)
            {
                var inputBatchList = RT.Service.Resolve<BatchManageController>().GetInputBatchs(workcell.ResourceId, workcell.ProcessId, workcell.StationId, workOrderId);
                if (inputBatchList.Count <= 0)
                {
                    throw new ValidationException("当前工序工位不存在进站批次列表".L10N());
                }
                var inputBatch = inputBatchList.FirstOrDefault(p => p.BatchNo == barcode || p.SubBatchNo == barcode);
                if (inputBatch == null)
                {
                    throw new ValidationException("当前工序工位不存在批次号为[{0}]的进站批次".L10nFormat(barcode));
                }
                inputBatch.SplitQty = inputBatch.RemainQty;

                var joinBatches = new EntityList<InputBatch> { inputBatch };
                JoinWithIuputBatch(joinBatches, rela, currentRuleDetail);
            }
            else
            {
                var joinRela = GetBatchPkgRelationByNo(barcode);
                if (joinRela == null)
                {
                    throw new ValidationException("当前工序工位不存在包装条码为[{0}]的包装".L10nFormat(barcode));
                }
                var joinRelas = new EntityList<BatchPackingRelation> { joinRela };
                JoinWithReletion(joinRelas, rela, currentRuleDetail, childRuleDetail);
            }
        }

        /// <summary>
        /// 加入 - 批次条码（手动）
        /// </summary>
        /// <param name="joinBatches">待加入批次</param>
        /// <param name="rela">目标包装</param>
        /// <param name="currentRuleDetail">目标包装层级</param>
        public virtual void JoinWithIuputBatch(EntityList<InputBatch> joinBatches, BatchPackingRelation rela, WorkOrderPackageRuleDetail currentRuleDetail/*, Workcell workcell*/)
        {
            if (rela.TreePId != null)
            {
                throw new ValidationException("目标包装:[{0}]已被打包".L10nFormat(rela.PackageNo));
            }
            if (!rela.PackageUnit.IsMasterUnit)
            {
                throw new ValidationException("目标包装:[{0}]的包装单位必须是主单位".L10nFormat(rela.PackageNo));
            }
            if (joinBatches.Any(p => p.WipBatchId != rela.PackingBatch))
            {
                throw new ValidationException("加入批次产品必须属于同一个生产批:[{0}]".L10nFormat(rela.BatchNo));
            }
            if (joinBatches.Any(p => p.WorkOrderId != rela.WorkOrderId))
            {
                throw new ValidationException("加入批次产品必须属于同一个工单".L10N());
            }
            if (currentRuleDetail == null || rela.ItemQty >= currentRuleDetail.Qty)
            {
                throw new ValidationException("目标包装:[{0}]已满".L10nFormat(rela.PackageNo));
            }

            //缺料数量
            var lackedQty = currentRuleDetail.Qty - rela.ItemQty;

            //目标主单位关联的子批次
            var subWipBatch = RF.GetById<SubWipBatch>(rela.ChildBatch);

            //批次关系
            var relationBactches = new EntityList<BatchRelation>();

            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                foreach (var inputBatch in joinBatches.OrderBy(p => p.InputDate))
                {
                    if (inputBatch.SplitQty <= 0)
                    {
                        continue;
                    }
                    else if (lackedQty == 0)
                    {
                        break;
                    }
                    else
                    {
                        //
                    }

                    decimal joinQty = 0;
                    if (inputBatch.SplitQty >= lackedQty)
                    {
                        joinQty = lackedQty;
                        inputBatch.RemainQty -= joinQty;
                        inputBatch.SplitQty = inputBatch.SplitQty <= inputBatch.RemainQty ? inputBatch.SplitQty : inputBatch.RemainQty;
                        lackedQty = 0;
                    }
                    else
                    {
                        joinQty = inputBatch.SplitQty;
                        inputBatch.SplitQty = 0;
                        inputBatch.RemainQty -= joinQty;
                        lackedQty -= joinQty;
                    }

                    rela.ItemQty += joinQty;
                    subWipBatch.Qty += joinQty;
                    relationBactches.Add(new BatchRelation
                    {
                        Bid = subWipBatch.BatchNo,
                        Pid = inputBatch.SubBatchNo,
                        WipBatch = inputBatch.BatchNo,
                        WorkOrderId = inputBatch.WorkOrderId,
                        Qty = joinQty,
                        RemainQty = inputBatch.RemainQty,
                    });
                }

                joinBatches.ForEach(p => p.SplitQty = 0);
                RF.Save(joinBatches);
                RF.Save(subWipBatch);
                RF.Save(rela);
                RF.Save(relationBactches);

                tran.Complete();
            }
        }

        /// <summary>
        /// 加入 - 包装关系（手动）
        /// </summary>
        /// <param name="joinRelas">待加入包装</param>
        /// <param name="rela">目标包装</param>
        /// <param name="currentRuleDetail">当前目标包装层级</param>
        /// <param name="childRuleDetail">待加入包装层级</param>
        /// <returns>批次包装关系</returns>
        public virtual BatchPackingRelation JoinWithReletion(EntityList<BatchPackingRelation> joinRelas, BatchPackingRelation rela, WorkOrderPackageRuleDetail currentRuleDetail, WorkOrderPackageRuleDetail childRuleDetail /*, Workcell workcell*/)
        {
            if (rela.TreePId != null)
                throw new ValidationException("目标包装:[{0}]已被打包".L10nFormat(rela.PackageNo));
            if (rela.PackedQty >= currentRuleDetail.LevelQty)
                throw new ValidationException("目标包装:[{0}]已满".L10nFormat(rela.PackageNo));
            if (rela.PackageUnitId != currentRuleDetail.PackageUnitId)
                throw new ValidationException("目标包装:[{0}]的包装单位必须是[{1}]".L10nFormat(rela.PackageNo, currentRuleDetail.PackageUnit.Name));
            if (joinRelas.Any(p => p.PackageUnitId != childRuleDetail.PackageUnitId))
                throw new ValidationException("加入包装的包装单位必须是[{0}]".L10nFormat(childRuleDetail.PackageUnit.Name));
            if (joinRelas.Any(p => p.TreePId != null))
                throw new ValidationException("加入包装存在已被打包的".L10N());
            if (joinRelas.Any(p => p.PackingBatch != rela.PackingBatch))
                throw new ValidationException("加入包装必须属于同一个生产批次:[{0}]".L10nFormat(rela.BatchNo));
            if (joinRelas.Any(p => p.WorkOrderId != rela.WorkOrderId))
                throw new ValidationException("加入包装必须属于同一个工单".L10N());

            //缺料数量
            var lackedQty = currentRuleDetail.LevelQty - rela.PackedQty;
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                foreach (var joinRela in joinRelas)
                {
                    if (lackedQty <= 0)
                    {
                        break;
                    }

                    joinRela.TreePId = rela.Id;
                    joinRela.ParentNo = rela.PackageNo;
                    lackedQty -= 1;
                    rela.PackedQty += 1;
                    rela.ItemQty += joinRela.ItemQty;
                }

                RF.Save(joinRelas);
                RF.Save(rela);
                tran.Complete();
            }

            return rela;
        }

        /// <summary>
        /// 加入 - 批次条码（自动打包）
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="rela">待加入包装</param>
        /// <param name="currentRuleDetail">当前目标包装规则</param>
        /// <param name="workcell">工作单元</param>
        public virtual void AutoJoinWithIuputBatch(double workOrderId, BatchPackingRelation rela, WorkOrderPackageRuleDetail currentRuleDetail, Workcell workcell)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var joinList = new EntityList<InputBatch>();
                joinList.AddRange(RT.Service.Resolve<BatchManageController>().GetInputBatchs(workcell.ResourceId, workcell.ProcessId, workcell.StationId, workOrderId).Where(p => p.WipBatchId == rela.PackingBatch));
                joinList.ForEach(p => p.SplitQty = p.RemainQty);
                JoinWithIuputBatch(joinList, rela, currentRuleDetail);

                tran.Complete();
            }
        }

        /// <summary>
        /// 加入 - 包装关系（自动打包）
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="rela">待加入包装</param>
        /// <param name="currentRuleDetail">当前目标包装规则</param>
        /// <param name="childRuleDetail">待加入包装规则</param>
        /// <param name="workcell">工作单元</param>
        public virtual void AutoJoinWithReletion(double workOrderId, BatchPackingRelation rela, WorkOrderPackageRuleDetail currentRuleDetail, WorkOrderPackageRuleDetail childRuleDetail, Workcell workcell)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var joinList = new EntityList<BatchPackingRelation>();
                joinList.AddRange(FindWorkPackingRelationByStation(workcell.ProcessId, workcell.StationId, workOrderId, childRuleDetail.PackageUnitId, false).Where(p => p.PackingBatch == rela.PackingBatch));
                JoinWithReletion(joinList, rela, currentRuleDetail, childRuleDetail);

                tran.Complete();
            }
        }

        #endregion

        /// <summary>
        /// 根据工位、工序、工单查找主单位
        /// </summary>
        /// <param name="process">工序Id</param>
        /// <param name="station">工位Id</param>
        /// <param name="workOrder">工单Id</param>
        /// <param name="hasParent">是否存在父</param>
        /// <returns>包装关系</returns>
        public virtual EntityList<BatchPackingRelation> FindWorkPackingRelationByStation(double process, double station, double? workOrder = null, bool? hasParent = null)
        {
            var query = Query<BatchPackingRelation>().Where(p => p.StationId == station && p.ProcessId == process);
            if (workOrder.HasValue)
            {
                query.Where(p => p.WorkOrderId == workOrder.Value);
            }
            if (hasParent == true)
            {
                query.Where(p => p.TreePId != null);
            }
            else if (hasParent == false)
            {
                query.Where(p => p.TreePId == null);
            }
            else
            {
                //
            }
            return query.OrderByDescending(p => p.CreateDate).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据工位、工序、工单、包装单位查找包装关系
        /// </summary>
        /// <param name="process">工序Id</param>
        /// <param name="station">工位Id</param>
        /// <param name="workOrder">工单Id</param>
        /// <param name="packageUnitId">包装单位</param>
        /// <param name="hasParent">是否存在父层级包装</param>
        /// <returns>包装关系</returns>
        public virtual EntityList<BatchPackingRelation> FindWorkPackingRelationByStation(double process, double station, double workOrder, double packageUnitId, bool? hasParent = null)
        {
            var query = Query<BatchPackingRelation>().Where(p => p.StationId == station && p.ProcessId == process && p.WorkOrderId == workOrder && p.PackageUnitId == packageUnitId);
            if (hasParent == true)
            {
                query.Where(p => p.TreePId != null);
            }
            else if (hasParent == false)
            {
                query.Where(p => p.TreePId == null);
            }
            else
            {
                //
            }
            return query.OrderByDescending(p => p.CreateDate).ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据包装号查询批次包装
        /// </summary>
        /// <param name="sn">包装号</param>
        /// <returns>批次包装</returns>
        public virtual BatchPackingRelation GetBatchPkgRelationByNo(string sn)
        {
            return Query<BatchPackingRelation>().Where(p => p.PackageNo == sn).FirstOrDefault();
        }

        /// <summary>
        /// 根据Id集合查询批次包装
        /// </summary>
        /// <param name="relaIds">包装关系Id集合</param>
        /// <returns>批次包装列表</returns>
        public virtual EntityList<BatchPackingRelation> GetBatchPkgRelationByIds(double[] relaIds)
        {
            var relaList = new EntityList<BatchPackingRelation>();
            for (int i = 0; i < relaIds.Length / 1000m; i++)
            {
                relaList.AddRange(Query<BatchPackingRelation>().Where(p => relaIds.Skip(i * 1000).Take(1000).Contains(p.Id)).ToList());
            }

            return relaList;
        }

        /// <summary>
        /// 打印后更新数据状态为已打印
        /// </summary>
        /// <param name="relaIds">包装关系Id集合</param>
        public virtual void SaveRelasAfterPrintSuccessfully(double[] relaIds)
        {
            using (var tran = DB.AutonomousTransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                for (int i = 0; i < relaIds.Length / 1000m; i++)
                {
                    DB.Update<BatchPackingRelation>().Set(p => p.State, LogisticState.Printed).Where(p => relaIds.Skip(i * 1000).Take(1000).Contains(p.Id)).Execute();
                }

                tran.Complete();
            }
        }
    }
}
