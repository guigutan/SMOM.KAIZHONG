using NPOI.SS.Formula.Functions;
using SIE.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.LES.LinesideWarehouses;
using SIE.MES.LoadItems;
using SIE.MES.Wip.Repairs;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.Packages.ItemLabels;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 维修采集控制器
    /// </summary>
    public class RepairController : WipController
    {
        /// <summary>
        /// 维修换料
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="repairUseItems">维修数据</param>
        protected virtual string UseLoadItem(WipProductProcess wipProductProcess, ICollection<RepairUseItem> repairUseItems)
        {
            if (repairUseItems == null)
            {
                throw new ArgumentNullException(nameof(repairUseItems));
            }

            EntityList<WipProductProcessKeyItem> keyItems = new EntityList<WipProductProcessKeyItem>();
            var keyItemList = wipProductProcess.Version.ProcessList.SelectMany(p => p.KeyItemList); //产品所有关键件

            if (keyItemList.Any())
            {
                keyItems.AddRange(keyItemList);
            }

            StringBuilder newItemLabelCode = new StringBuilder();
            foreach (var repairUseItem in repairUseItems)
            {
                repairUseItem.UserItems.ForEach(e =>
                {//备注：e.key:新上料的记录的Id e.Value：上料实际数量 
                    var loadItem = GetById<LoadItem>(e.Key);
                    if (loadItem == null)
                    {
                        throw new EntityNotFoundException(typeof(LoadItem), e.Key);
                    }

                    if (loadItem.Qty < e.Value)
                    {
                        throw new ValidationException("缺料,物料[{0}]剩余数量[{1}],用量为[{2}]"
                            .L10nFormat(loadItem.SourceCode, loadItem.Qty, e.Value));
                    }

                    loadItem.Qty -= e.Value;

                    AddKeyItem(wipProductProcess, loadItem, e.Value);

                    RF.Save(loadItem);
                });

                var newItemLabel = RemoveKeyItem(keyItems, repairUseItem, repairUseItem.UserItems.Sum(p => p.Value));
                if (!newItemLabel.IsNullOrEmpty())
                {
                    newItemLabelCode.Append(newItemLabel + ",");//如果产生了新标签则累计复制
                }

            }
            return newItemLabelCode.ToString();
        }

        /// <summary>
        /// 移除关键件，全部换料时删除关键件
        /// </summary>        
        /// <param name="keyItems">产品生产关键件列表</param>
        /// <param name="repairUseItem">换料记录</param>        
        /// <param name="qty">换料数量</param>
        private string RemoveKeyItem(EntityList<WipProductProcessKeyItem> keyItems,
            RepairUseItem repairUseItem, decimal qty)
        {

            string newItemLabelCode = "";//新物料标签
            if (qty == 0)
            {
                return newItemLabelCode;
            }

            var keyItem = keyItems.FirstOrDefault(p => p.Id == repairUseItem.SoureKeyItemId) ?? throw new ValidationException("关键件{0}不存在"
                    .L10nFormat(repairUseItem.Sn));
            if (keyItem.Qty < qty)
            {
                throw new ValidationException("条码{0}装配数量{1}小于换料数量{2}"
                    .L10nFormat(repairUseItem.Sn, keyItem.Qty, qty));
            }

            if (repairUseItem.HandleMethod == ChangeItemHandleMethod.Recycle|| repairUseItem.HandleMethod == ChangeItemHandleMethod.NGRecycle)//置换后正常下料
            {
                var itemLabel = Query<ItemLabel>()
                    .Where(x => x.Id == keyItem.SourceId)
                    .FirstOrDefault();

                if (itemLabel != null)
                {
                    var unloadItem = new UnloadItem
                    {
                        LoadItemQty = qty,
                        Qty = qty,
                        RemainderQty = qty,
                        Shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(keyItem.Process.ResourceId, DateTime.Now),
                        State = UnloadState.UnConfirm,
                        StationId = keyItem.Process.StationId,
                        ResourceId = keyItem.Process.ResourceId,
                        SourceCode = keyItem.SourceCode,
                        SourceId = keyItem.SourceId,
                        SourceType = keyItem.SourceType,
                        Item = keyItem.Item,
                        WorkOrderId = keyItem.Process.Version.WorkOrderId,

                    };
                    unloadItem.ItemExtProp = keyItem.ItemExtProp;
                    unloadItem.ItemExtPropName = keyItem.ItemExtPropName;
                    RF.Save(unloadItem);

                    double? warehouseId = itemLabel.WarehouseId;
                    double? storageLocationId = itemLabel.StorageLocationId;
                    if (!warehouseId.HasValue || !storageLocationId.HasValue)//如果物料标签不存在仓库或库位 则取当前生产资源的线边仓作为退料后的仓库和库位
                    {
                        //如果获取不到当前资源的线边仓则提示要求维护线边仓
                        var linesideWarehouse = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouse(keyItem.Process.ResourceId);
                        if (linesideWarehouse == null)
                        {
                            throw new ValidationException("下料失败，原因:物料标签【{0}】的仓库或库位为空且当前产线【{1}】未维护产线线边仓,请先维护产线线边仓".L10nFormat(itemLabel.Label, keyItem.Process.ResourceName));
                        }
                        warehouseId = linesideWarehouse.WarehouseId;
                        storageLocationId = linesideWarehouse.StorageLocationId;
                    }
                    newItemLabelCode = RepairReWorkUnloadItem(keyItem.SourceId, qty, warehouseId.Value, storageLocationId.Value, repairUseItem.HandleMethod == ChangeItemHandleMethod.NGRecycle, keyItem.Process.Version.WorkOrderId);

                }
            }
            keyItem.Qty -= qty;
            if (keyItem.Qty == 0)
            {
                DB.Delete<WipProductProcessKeyItem>()
                .Where(x => x.Id == keyItem.Id)
                    .Execute();
            }
            return newItemLabelCode;
        }

        /// <summary>
        /// 采集完成，子类重写实现特殊逻辑
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">运行时产品</param>
        /// <param name="collectBarcodes">采集条码集合</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        protected override void OnWipProductProcessFinished(WipProductProcess wipProductProcess, product product,
            IList<CollectBarcode> collectBarcodes, CollectData collectData, Workcell workcell)
        {
            if (collectData == null)
            {
                throw new EntityNotFoundException(nameof(collectData));
            }

            base.OnWipProductProcessFinished(wipProductProcess, product, collectBarcodes, collectData, workcell);

            var newItemLables = UseLoadItem(wipProductProcess, collectData.RepairUseItems);
            //将新的物料标签传递到外部 提示用户
            collectData.Context["UNLOADITEM_NEWITEMLABEL"] = newItemLables;
            var wipProductRepairId = AddRepair(wipProductProcess, collectData, workcell);

            AddRepairReplaceRecord(wipProductRepairId, wipProductProcess, collectData.RepairUseItems);

        }

        /// <summary>
        /// 增加换料记录
        /// </summary>
        /// <param name="wipProductRepairId">维修记录Id</param>
        /// <param name="wipProductProcess"></param>
        /// <param name="repairUseItems"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void AddRepairReplaceRecord(double wipProductRepairId, WipProductProcess wipProductProcess, List<RepairUseItem> repairUseItems)
        {
            if (repairUseItems == null)
            {
                throw new ArgumentNullException(nameof(repairUseItems));
            }

            EntityList<WipProductProcessKeyItem> keyItems = new EntityList<WipProductProcessKeyItem>();
            var keyItemList = wipProductProcess.Version.ProcessList.SelectMany(p => p.KeyItemList); //产品所有关键件

            if (keyItemList.Any())
            {
                keyItems.AddRange(keyItemList);
            }
            EntityList<WipProductRepairReplaceRecord> wipProductRepairReplaceRecords = new EntityList<WipProductRepairReplaceRecord>();
            foreach (var repairUseItem in repairUseItems)
            {
                repairUseItem.UserItems.ForEach(e =>
                {//备注：e.key:新上料的记录的Id e.Value：上料实际数量 
                    var loadItem = GetById<LoadItem>(e.Key);
                    if (loadItem == null)
                    {
                        throw new EntityNotFoundException(typeof(LoadItem), e.Key);
                    }

                    var keyItem = keyItems.FirstOrDefault(p => p.Id == repairUseItem.SoureKeyItemId) ?? throw new ValidationException("关键件{0}不存在"
                            .L10nFormat(repairUseItem.Sn));
                    if (e.Value > 0)//换料大于的才记录
                    {
                        WipProductRepairReplaceRecord wipProductRepairReplaceRecord = new WipProductRepairReplaceRecord();

                        wipProductRepairReplaceRecord.NewLabeNo = loadItem.SourceCode;
                        wipProductRepairReplaceRecord.LabelNo = repairUseItem.Sn;
                        wipProductRepairReplaceRecord.Qty = e.Value;
                        wipProductRepairReplaceRecord.Item = loadItem.Item;
                        wipProductRepairReplaceRecord.WipProductRepairId = wipProductRepairId;
                        wipProductRepairReplaceRecords.Add(wipProductRepairReplaceRecord);
                    }
                });
            }
            if (wipProductRepairReplaceRecords.Any())
            {
                RF.Save(wipProductRepairReplaceRecords);
            }
        }

        /// <summary>
        /// 添加产品维修记录
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        protected virtual double AddRepair(WipProductProcess wipProductProcess, CollectData collectData, Workcell workcell)
        {
            if (wipProductProcess is null)
            {
                throw new ArgumentNullException(nameof(wipProductProcess));
            }

            if (collectData is null)
            {
                throw new ArgumentNullException(nameof(collectData));
            }

            if (workcell is null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            var version = wipProductProcess.Version;


            //尝试查找已存在的维修开始记录

            WipProductRepair wipProductRepair = GetExsitedWipProductRepair(wipProductProcess, workcell, version.Id);
            if (wipProductRepair == null)
            {
                wipProductRepair = new WipProductRepair
                {
                    ProcessId = workcell.ProcessId,
                    ReparieById = workcell.EmployeeId,
                    ResourceId = workcell.ResourceId,
                    RepaireTime = DateTime.Now,
                    RepairStart = DateTime.Now,
                    ShiftId = wipProductProcess.ShiftId,
                    StationId = workcell.StationId,
                    VersionId = wipProductProcess.VersionId
                };
            }
            else
            {
                wipProductRepair.RepaireTime = DateTime.Now;
            }

            foreach (var repairDefect in collectData.RepairDefects)
            {
                var defect = version.DefectList.FirstOrDefault(p => p.Id == repairDefect.ProductDefectId);
                if (defect != null)
                {
                    repairDefect.Responsiblities.ForEach(e =>
                    {
                        if (!defect.ResponsibilityList.Any(p => p.DefectResponsibilityId == e.Id))
                        {
                            var responsibility = new WipDefectResponsibility()
                            {
                                DefectResponsibility = e,
                                WipProductDefect = defect
                            };

                            defect.ResponsibilityList.Add(responsibility);
                        }
                    });

                    repairDefect.Measures.ForEach(e =>
                    {
                        if (!defect.MeasureList.Any(p => p.RepairMeasureId == e.Id))
                        {
                            var measure = new WipDefectMeasure()
                            {
                                RepairMeasure = e,
                                WipProductDefect = defect
                            };

                            defect.MeasureList.Add(measure);
                        }
                    });

                    defect.Remark = repairDefect.Remark;
                    defect.IsFixed = true;

                    defect.FixedById = workcell.EmployeeId;
                    defect.FixedDate = DateTime.Now;

                    RF.Save(defect);
                }

                if (!repairDefect.IsFixed&& repairDefect.ProductDefectId.HasValue&&repairDefect.ProductDefectId.Value > 0)
                {
                    var wipProductRepairDefect = new WipProductRepairDefect
                    {
                        WipProductDefectId = repairDefect.ProductDefectId.Value,
                    };

                    wipProductRepair.WipProductRepairDefectList.Add(wipProductRepairDefect);
                }
            }

            RF.Save(wipProductRepair);
            return wipProductRepair.Id;
        }

        /// <summary>
        /// 获取已开始的维修记录
        /// </summary>
        /// <param name="wipProductProcess"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public virtual WipProductRepair GetExsitedWipProductRepair(WipProductProcess wipProductProcess, Workcell workcell, double versionId)
        {
            var vId = wipProductProcess == null ? versionId : wipProductProcess.VersionId;
            return Query<WipProductRepair>().Where(p => p.ProcessId == workcell.ProcessId && p.ResourceId == workcell.ResourceId
                         && p.StationId == workcell.StationId && p.VersionId == vId && p.RepaireTime == null
                         ).OrderBy(p => p.RepairStart).FirstOrDefault();
        }

        /// <summary>
        /// 获取工艺路线中的工序列表
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元信息</param>        
        /// <returns>工艺路线工序列表</returns>
        public virtual List<GotoProcessViewModel> GetRoutingProcessList(string barcode, Workcell workcell)
        {
            if (barcode == null)
                throw new ArgumentNullException(nameof(barcode));
            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));

            var barcodeType = RuntimeController.FindMapBarcodeType(barcode);

            if (barcodeType == null)
                throw new ValidationException("找不到条码[{0}]类型，可能条码已下线".L10nFormat(barcode));

            var collectBarcode = new CollectBarcode(barcode, barcodeType.Value);

            var product = ValidateProduct(collectBarcode, workcell);

            List<GotoProcessViewModel> lists = new List<GotoProcessViewModel>();

            //子工艺路线中的工序不要列出选择,非工序组及工序组下的工序
            var processes = product.Routing.Processes
                .Where(x => x.ParentNodeId.IsNullOrEmpty() && x.Type != ProcessType.Fix && string.IsNullOrEmpty(x.GroupId))
                .ToList();

            var processOfCurrent = product.Routing.GetNext()
                .FirstOrDefault(x => x.ProcessId == workcell.ProcessId);
            if (processOfCurrent == null)
            {
                return lists;
            }

            var groupProcesses = product.Routing.Processes.Where(x => x.IsGroup == true).ToList();
            foreach (var groupProcess in groupProcesses)
            {
                //工序组下有工序没有通过，则只能去未通过的工序
                if (product.Routing.Processes
                    .Any(x => !x.IsPass && x.IsGroup != true && x.GroupId == groupProcess.GroupId))
                {
                    processes.AddRange(product.Routing.Processes
                        .Where(x => !x.IsPass && x.IsGroup != true && x.GroupId == groupProcess.GroupId));
                }
                else
                {
                    processes.AddRange(product.Routing.Processes
                        .Where(x => x.IsGroup != true && x.GroupId == groupProcess.GroupId));
                }
            }

            StringBuilder stringBuilderPathDesc = new StringBuilder();

            foreach (var process in processes)
            {
                stringBuilderPathDesc.Clear();

                GotoProcessViewModel optionalPathViewModel = new GotoProcessViewModel()
                {
                    Id = Guid.NewGuid().ToString("N").ToUpper(),
                    RoutingProcessId = process.Id,
                    PathName = process.Name
                };

                stringBuilderPathDesc.Append(process.Name);

                BuilderPathDescription(stringBuilderPathDesc, product.Routing.Processes, process,
                    processOfCurrent.Id);

                optionalPathViewModel.PathDescription = stringBuilderPathDesc.ToString();

                lists.Add(optionalPathViewModel);
            }

            //默认上线工序
            List<double> nextProcessIds;
            var currentProcess = product.Routing.Processes.FirstOrDefault(x => workcell.ProcessId == x.ProcessId);
            if (currentProcess != null && currentProcess.Next.TryGetValue(ResultType.Pass, out nextProcessIds))
            {
                var next = product.Routing.Processes.FirstOrDefault(q => q.Id == nextProcessIds[0]);

                if (next != null)
                {
                    var defultViewModel = lists.FirstOrDefault(x => x.RoutingProcessId == next.Id);
                    if (defultViewModel != null)
                    {
                        defultViewModel.IsDefault = true;
                    }
                }
            }

            return lists;
        }

        private void BuilderPathDescription(StringBuilder stringBuilderPathDesc, List<process> processes,
            process currentProcess, double sourceProcessId)
        {
            if (currentProcess.Next.ContainsKey(ResultType.Pass))
            {
                var nextIds = currentProcess.Next[ResultType.Pass];
                if (nextIds.Any())
                {
                    var nextId = nextIds[0];
                    if (nextId != sourceProcessId)
                    {
                        var nextProcess = processes.FirstOrDefault(x => x.Id == nextId);
                        if (nextProcess != null && string.IsNullOrEmpty(nextProcess.ParentNodeId))
                        {
                            stringBuilderPathDesc.Append(" --> {0}".L10nFormat(nextProcess.Name));
                            BuilderPathDescription(stringBuilderPathDesc, processes, nextProcess, sourceProcessId);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 维修采集
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        /// <param name="uplineProcess">上线工序</param>
        public virtual void Collect(string barcode, CollectData collectData, Workcell workcell, double? uplineProcess = null)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            if (collectData == null)
            {
                throw new ArgumentNullException(nameof(collectData));
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                Move(new[] { barcode }, collectData, workcell);
                if (uplineProcess.HasValue)
                {
                    var collectBarcode = GetBarcode(barcode);
                    Goto(collectBarcode, uplineProcess.Value, workcell);
                }

                tran.Complete();
            }
        }

        /// <summary>
        /// 验证采集步骤
        /// </summary>
        /// <param name="barcodes">条码数组</param>
        /// <param name="steps">采集步骤数组</param>
        /// <param name="collectBarcode">采集步骤（前端传入）</param>
        /// <param name="noValidateStep">不验证采集步骤</param>
        /// <returns>采集条码集合</returns>
        /// <exception cref="ValidationException">条码与采集步骤不一致、条码为空</exception>
        protected override IList<CollectBarcode> ValidateCollectStep(string[] barcodes, ProcessCollectStep[] steps, CollectBarcode collectBarcode, bool noValidateStep)
        {
            //不按工序的采集步骤，只要是产品关联过的条码都可以扫描进行维修
            var collectBarcodes = new List<CollectBarcode>();
            if (barcodes != null)
            {
                foreach (var barcode in barcodes)
                {
                    collectBarcodes.Add(GetBarcode(barcode));
                }
            }
            return collectBarcodes;
        }

        /// <summary>
        /// 获取采集条码，根据条码从关联表查找条码类型
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>采集条码</returns>
        /// <exception cref="ValidationException">条码不存在</exception>
        public virtual CollectBarcode GetBarcode(string barcode)
        {
            var barcodeType = RuntimeController.FindMapBarcodeType(barcode);
            if (barcodeType == null)
                throw new ValidationException("找不到条码[{0}]类型，可能条码已下线".L10nFormat(barcode));
            return new CollectBarcode(barcode, barcodeType.Value);
        }

        /// <summary>
        /// 验证工艺路线并加载产品未维修的缺陷
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元信息</param>
        /// <returns>产品缺陷记录列表</returns>
        public virtual EntityList<WipProductDefect> LoadDefects(string barcode, Workcell workcell)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            var collectBarcode = GetBarcode(barcode);
            var version = FindLastWipProductVersion(collectBarcode);
            return version.DefectList.Where(p => !p.IsMisjudgment).AsEntityList();
        }

        /// <summary>
        /// 加载缺陷包含视图属性
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual EntityList<WipProductDefect> LoadDefectsWithViewProperty(string barcode, Workcell workcell)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            var collectBarcode = GetBarcode(barcode);
            var version = FindLastWipProductVersion(collectBarcode);
            var defectListIds = version.DefectList.Where(m => !m.IsMisjudgment).Select(m => m.Id).ToList();
            if (defectListIds.Any())
            {
                return Query<WipProductDefect>().Where(m => defectListIds.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWith(WipProductDefect.MeasureListProperty)
                    .LoadWith(WipProductDefect.ResponsibilityListProperty).LoadWithViewProperty());
            }
            return new EntityList<WipProductDefect>();
        }

        /// <summary>
        /// 验证工艺路线并加载产品未维修的缺陷
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="isItem">是否半成品</param>
        /// <returns>产品生产关键件列表</returns>
        public virtual EntityList<WipProductProcessKeyItem> LoadkeyItems(string barcode, bool isItem)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            WipProductVersion version;
            if (!isItem)
            {
                var collectBarcode = GetBarcode(barcode);
                version = FindLastWipProductVersion(collectBarcode);
            }
            else
            {
                version = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersion(barcode);
            }
            var keyItems = version?.ProcessList?.SelectMany(p => p.KeyItemList);
            var result = new EntityList<WipProductProcessKeyItem>();
            ////获取产品的所有关键件
            if (keyItems.Any())
            {
                result.AddRange(keyItems);
            }
            return result;
        }

        /// <summary>
        /// 保存维修记录
        /// </summary>
        /// <param name="defects">产品缺陷记录列表</param>
        public virtual void SaveRepairRecord(EntityList<WipProductDefect> defects)
        {
            RF.Save(defects);
        }

        /// <summary>
        /// 保存缺陷维修记录
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <param name="toSaveRecord">缺陷维修记录集合</param>
        /// <exception cref="ValidationException">产品缺陷为空、工单为空、工序为空、产品条码为空</exception>
        public virtual RepairMainRecord SaveRepairRecord(double workOrderId, string sn, RepairMainRecord toSaveRecord)
        {
            if (toSaveRecord.RepairDefectRecordList.Any(p => p.ProductDefectId == 0 && !p.IsNewAdd))
                throw new ValidationException("验证失败，产品缺陷为空".L10N());

            if (toSaveRecord.RepairDefectRecordList.Any(p => p.ProcessId == 0))
                throw new ValidationException("验证失败，工序为空".L10N());

            //DeleteRepairDefectRecords(workOrderId, sn);

            RF.Save(toSaveRecord);

            return toSaveRecord;
        }

        /// <summary>
        /// 保存维修记录
        /// </summary>
        /// <param name="records">维修记录</param>
        /// <param name="defectIds">缺陷ID集合</param>
        public virtual void SaveRepairRecord(EntityList<RepairRecord> records, double[] defectIds)
        {
            var oldRecords = GetRepairRecords(defectIds, true);
            List<RepairRecord> temRecords = new List<RepairRecord>();
            temRecords.AddRange(oldRecords);
            foreach (RepairRecord newRecode in records)
            {
                //匹配旧的记录，存在则数据覆盖，不存在则新增
                var oldRecord = oldRecords.FirstOrDefault(p => p.ProductDefectId == newRecode.ProductDefectId);
                if (oldRecord == null)
                {
                    RF.Save(newRecode); //旧记录为空，则记录为新增
                }
                else
                {
                    oldRecord.ScrapReason = newRecode.ScrapReason;
                    oldRecord.ScrapQty = newRecode.ScrapQty;
                    oldRecord.Remark = newRecode.Remark;
                    oldRecord.MeasureList.Clear();
                    oldRecord.MeasureList.AddRange(newRecode.MeasureList);
                    oldRecord.ResponsibilityList.Clear();
                    oldRecord.ResponsibilityList.AddRange(newRecode.ResponsibilityList);
                    RF.Save(oldRecords);
                    temRecords.Remove(oldRecord);
                }
            }
            ////删除的记录
            if (temRecords.Count > 0)
            {
                temRecords.ForEach(e => e.PersistenceStatus = PersistenceStatus.Deleted);
                RF.Save(temRecords.AsEntityList());
            }
        }

        /// <summary>
        /// 获取暂存维修记录
        /// </summary>
        /// <param name="defectId">缺陷ID</param>
        /// <param name="isBatch">是否批次维修</param>
        /// <returns>暂存维修记录列表</returns>
        public virtual EntityList<RepairRecord> GetRepairRecords(double defectId, bool isBatch)
        {
            return Query<RepairRecord>().Where(p => p.ProductDefectId == defectId && p.IsBatch == isBatch).ToList();
        }

        /// <summary>
        /// 获取暂存维修记录
        /// </summary>
        /// <param name="defectIds">缺陷ID集合</param>
        /// <param name="isBatch">是否批次维修</param>
        /// <returns>暂存维修记录列表</returns>
        public virtual EntityList<RepairRecord> GetRepairRecords(double[] defectIds, bool isBatch)
        {
            return Query<RepairRecord>().Where(p => defectIds.Contains(p.ProductDefectId) && p.IsBatch == isBatch).ToList();
        }

        /// <summary>
        /// 获取产品维修次数
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <returns>维修次数</returns>
        public virtual int GetSnRepairTimes(double workOrderId, string sn)
        {
            return Query<WipProductProcess>()
               .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.Sn == sn && y.WorkOrderId == workOrderId)
               .Join<WipProductVersion, WipProduct>((v, p) => v.Id == p.CurrentVersionId)
               .Where(p => p.State == WipProductProcessState.Finish)
               .Where(p => p.Process.Type == ProcessType.Fix || p.Process.Type == ProcessType.BatchFix)
               .Count();
        }

        /// <summary>
        /// 获取产品缺陷位置/点位
        /// </summary>
        /// <param name="workOrderId">工单ID</param>
        /// <param name="sn">产品条码</param>
        /// <returns>缺陷位置/点位</returns>
        public virtual IList<string> GetWipProductDefectLocations(double workOrderId, string sn)
        {
            return Query<WipProductDefect>()
               .Join<WipProductVersion>((x, y) => x.VersionId == y.Id && y.Sn == sn && y.WorkOrderId == workOrderId)
               .Join<WipProductVersion, WipProduct>((v, p) => v.Id == p.CurrentVersionId)
               .Where(p => p.Location != null && !p.IsMisjudgment)
               .Select(p => p.Location)
               .ToList<string>();
        }

        /// <summary>
        /// 获取可选路径
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ValidationException"></exception>
        public virtual List<GotoChildProcessViewModel> GetOptionalPaths(string barcode, Workcell workcell)
        {
            if (barcode == null)
                throw new ArgumentNullException(nameof(barcode));
            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));

            List<GotoChildProcessViewModel> lists = new List<GotoChildProcessViewModel>();

            var barcodeType = RuntimeController.FindMapBarcodeType(barcode);
            if (barcodeType == null)
                throw new ValidationException("找不到条码[{0}]类型，可能条码已下线".L10nFormat(barcode));
            var collectBarcode = new CollectBarcode(barcode, barcodeType.Value);

            var product = RuntimeController.FindProduct(collectBarcode);

            var currentProcess = product.Routing.GetNext()
                .FirstOrDefault(x => x.ProcessId == workcell.ProcessId);
            if (currentProcess == null)
            {
                return lists;
            }

            //不存在可选路径
            if (!currentProcess.Next.ContainsKey(ResultType.Optional))
            {
                return lists;
            }

            StringBuilder stringBuilderPathDesc = new StringBuilder();

            var optionalProcessIds = currentProcess.Next[ResultType.Optional];
            foreach (var optionalProcessId in optionalProcessIds)
            {
                stringBuilderPathDesc.Clear();

                var process = product.Routing.Processes.FirstOrDefault(x => x.Id == optionalProcessId);
                if (process == null)
                {
                    continue;
                }
                GotoChildProcessViewModel optionalPathViewModel = new GotoChildProcessViewModel()
                {
                    Id = Guid.NewGuid().ToString("N").ToUpper(),
                    RoutingProcessId = process.Id
                };

                if (currentProcess.OptionalPathDictionary.ContainsKey(optionalProcessId))
                {

                    optionalPathViewModel.PathName
                        = currentProcess.OptionalPathDictionary[optionalProcessId];
                }

                var firstProcess = product.Routing.Processes.FirstOrDefault(x => x.Id == optionalProcessId);
                stringBuilderPathDesc.Append(firstProcess.Name);

                BuilderPathDescriptionByProcessId(stringBuilderPathDesc, product.Routing.Processes, firstProcess,
                    currentProcess.Id);

                optionalPathViewModel.PathDescription = stringBuilderPathDesc.ToString();

                lists.Add(optionalPathViewModel);
            }


            return lists;
        }

        /// <summary>
        /// 构建工艺子路线描述
        /// </summary>
        /// <param name="stringBuilderPathDesc"></param>
        /// <param name="processes"></param>
        /// <param name="currentProcess"></param>
        /// <param name="sourceProcessId"></param>
        private void BuilderPathDescriptionByProcessId(StringBuilder stringBuilderPathDesc, List<process> processes,
            process currentProcess, double sourceProcessId)
        {
            if (currentProcess.Next.ContainsKey(ResultType.Pass))
            {
                var nextIds = currentProcess.Next[ResultType.Pass];
                if (nextIds.Any())
                {
                    var nextId = nextIds[0];
                    if (nextId != sourceProcessId)
                    {
                        var nextProcess = processes.FirstOrDefault(x => x.Id == nextId);
                        if (nextProcess != null && !string.IsNullOrEmpty(nextProcess.ParentNodeId))
                        {
                            stringBuilderPathDesc.Append(" --> {0}".L10nFormat(nextProcess.Name));
                            BuilderPathDescriptionByProcessId(stringBuilderPathDesc, processes, nextProcess, sourceProcessId);
                        }
                    }
                }
            }

        }

        /// <summary>
        /// 维修提交
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="gotoRoutingProcessId"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual void RepairSubmit(string barcode, Workcell workcell, double gotoRoutingProcessId)
        {
            if (barcode == null)
            {
                throw new ArgumentNullException(nameof(barcode));
            }
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))

            {
                var collectBarcode = GetBarcode(barcode);
                Goto(collectBarcode, gotoRoutingProcessId, workcell);
                tran.Complete();
            }
        }

        /// <summary>
        /// 重写验证完成
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="product"></param>
        /// <param name="result"></param>
        protected override void OnValidateFinish(CollectBarcode barcode, Workcell workcell, product product, ProductInfo result)
        {
            base.OnValidateFinish(barcode, workcell, product, result);

            //是否有可选路径存在
            var currentProcess = product.Routing.GetNext()
                .FirstOrDefault(x => x.ProcessId == workcell.ProcessId);

            if (currentProcess != null && currentProcess.Next.ContainsKey(ResultType.Optional))
            {
                result.Context["HasOptionalPath"] = true;
            }
            else
            {
                result.Context["HasOptionalPath"] = false;
            }
        }

        /// <summary>
        /// 获取维修记录
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="sn"></param>
        /// <param name="createRepairRecord"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual RepairMainRecord GetRepairRecord(double workOrderId, string sn, bool createRepairRecord = false)
        {
            if (workOrderId == 0)
            {
                throw new ValidationException("工单不能空".L10N());
            }

            if (string.IsNullOrEmpty(sn))
            {
                throw new ValidationException("产品条码不能空".L10N());
            }

            RepairMainRecord repairMainRecord = Query<RepairMainRecord>()
                .Where(p => p.WorkOrderId == workOrderId && p.Sn == sn)
                .FirstOrDefault();

            if (repairMainRecord == null && createRepairRecord)
            {
                repairMainRecord = new RepairMainRecord()
                {
                    RepairStart = RF.Find<RepairMainRecord>().GetDbTime(),
                    WorkOrderId = workOrderId,
                    Sn = sn,
                    PersistenceStatus = PersistenceStatus.New
                };
                RF.Save(repairMainRecord);
            }

            return repairMainRecord;
        }

    }
}