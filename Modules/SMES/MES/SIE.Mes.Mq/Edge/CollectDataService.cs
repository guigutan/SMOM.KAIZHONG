using Newtonsoft.Json;
using SIE.Common;
using SIE.Common.Sort;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.EventMessages.Inspection;
using SIE.MES.Edge;
using SIE.MES.Edge.Models;
using SIE.MES.LoadItems;
using SIE.MES.PackingPrints;
using SIE.MES.SingleLabels;
using SIE.MES.WIP.PackRecombine.Relations;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Mes.Mq.Edge
{
    /// <summary>
    /// 采集服务
    /// </summary>
    public class CollectDataService : ICollectDataService
    {
        private readonly ICollectDataDao collectDataDao;
        private readonly IEdgeErrorMessageDao errorMessageDao;
        private readonly IEdgeWipDao wipDao;
        private readonly LoadItemService loadItemService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="collectDataDao"></param>
        /// <param name="errorMessageDao"></param>
        /// <param name="wipDao"></param>
        /// <param name="loadItemService"></param>
        public CollectDataService(ICollectDataDao collectDataDao,
            IEdgeErrorMessageDao errorMessageDao,
            IEdgeWipDao wipDao,
            LoadItemService loadItemService)
        {
            this.collectDataDao = collectDataDao;
            this.errorMessageDao = errorMessageDao;
            this.wipDao = wipDao;
            this.loadItemService = loadItemService;
        }
        /// <summary>
        /// 采集数据处理
        /// </summary>
        /// <param name="em"></param>
        public void CollectData(EdgeMessage em)
        {
            if (em == null)
            {
                RT.Logger.Error(("采集处理收到非法的消息：NULL，传送的消息请按照EdgeMessage类结构进行序列化").L10N());
                return;
            }

            EdgeCollectData data;
            RT.InvOrg = int.Parse(em.InvOrg);
            string body = em.Body.ToString();
            data = JsonConvert.DeserializeObject<EdgeCollectData>(body);

            // 根据采集用户ID解析生成当前身份
            double employeeId = double.Parse(data.EmployeeId);
            double? userId = collectDataDao.GetUserId(employeeId);
            if (RT.IdentityId != employeeId && userId != null)
            {
                RT.Principal = new DataPortal.DataPortalPrincipal(employeeId, userId.Value, "");
            }

            if (data.WorkOrderId.IsNullOrEmpty())
            {
                return;
            }
            //如果存在包装记录数据，则创建包装记录
            if (data.PackageList != null && data.PackageList.Any())
            {
                CreatePackingRelation(data);
                return;
            }

            WipProductVersion version;
            WipProductProcess pp;
            using (var tran = Domain.DB.TransactionScope(MesMqEntityDataProvider.ConnectionStringName))
            {
                lock (data)
                {
                    version = collectDataDao.GetWipProductVersion(data.Puid);
                    if (version == null)
                    {
                        var wipProduct = CreateWipProduct(data);
                        version = CreateWipProductVersion(wipProduct, data);
                    }
                }
                var pd = version.Product;
                pp = CreateWipProductProcess(data, version);
                if (data.MaterialLabels != null && data.MaterialLabels.Count > 0)
                {
                    CreateProcessKeyItem(data.MaterialLabels, pp, data);
                }
                if (data.UpdatedKeyMaterials != null && data.UpdatedKeyMaterials.Any())
                {
                    SetKeyMaterials(pp, data);
                }

                collectDataDao.SaveCollectData(pp, version, pd);
                data.Barcode = pp.Barcode;
                tran.Complete();
            }

            //采集完成后触发的逻辑
            ExecuteAfterMove(data, pp, version);
        }

        /// <summary>
        /// 保存异常消息
        /// </summary>
        /// <param name="em"></param>
        /// <param name="ex"></param>
        public void SaveErrorMessage(EdgeMessage em, Exception ex)
        {
            string body = em.Body?.ToString();
            errorMessageDao.CreateErrorMessage(em.Id, ex.Message, em.Name, body);
        }

        /// <summary>
        /// 维修后如换料则更新非当前工步的关键件
        /// </summary>
        /// <param name="wipProductProcess"></param>
        /// <param name="data"></param>
        private void SetKeyMaterials(WipProductProcess wipProductProcess, EdgeCollectData data)
        {
            var processList = wipProductProcess.Version.ProcessList;
            var currentProcessId = double.Parse(data.ProcessId);
            data.UpdatedKeyMaterials.ForEach(item =>
            {
                var processId = double.Parse(item.ProcessId);
                var records = processList.Where(m => m.ProcessId == processId && m.ProcessId != currentProcessId);
                records.ForEach(record =>
                {
                    foreach (var keyItem in record.KeyItemList)
                    {
                        if (keyItem.ItemId == double.Parse(item.ItemId) && keyItem.SourceCode == item.Barcode)
                        {
                            keyItem.PersistenceStatus = item.Qty == 0 ? Domain.PersistenceStatus.Deleted : Domain.PersistenceStatus.Modified;
                            keyItem.Qty = item.Qty;
                        }
                    }


                });

            });
        }

        /// <summary>
        /// 记录采集记录
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="version">生产产品版本</param>
        /// <returns>生产采集记录</returns>
        protected WipProductProcess CreateWipProductProcess(EdgeCollectData data, WipProductVersion version)
        {
            var currentTime = data.CreationDate;
            double resourceId = double.Parse(data.LineId);

            var shift = wipDao.GetShift(resourceId, currentTime);
            var process = RF.GetById<Process>(double.Parse(data.ProcessId));
            var collectStep = process.CollectStepList.LastOrDefault();

            WipProductProcess wipProductProcess = new WipProductProcess();
            wipProductProcess.GenerateId();
            wipProductProcess.ProcessId = double.Parse(data.ProcessId);
            wipProductProcess.ResourceId = double.Parse(data.LineId);
            wipProductProcess.Result = data.ResultType;
            wipProductProcess.StationId = double.Parse(data.StationId);
            wipProductProcess.OperateById = double.Parse(data.EmployeeId);
            wipProductProcess.OperateTime = currentTime;
            wipProductProcess.Shift = shift;
            wipProductProcess.Barcode = collectStep.BarcodeType == BarcodeType.KeyLabel ? data.ComBarcode : data.Barcode;
            wipProductProcess.State = data.State;
            wipProductProcess.Version = version;
            wipProductProcess.InInning = false;

            //SetVersionBarcode(ref version, data.BarcodeType, data.Barcode);
            version.Sn = version.Sn.IsNotEmpty() ? version.Sn : data.Barcode;
            version.KeyLabel = version.KeyLabel.IsNotEmpty() ? version.KeyLabel : data.ComBarcode;

            var wipProduct = version.Product;
            if (data.State == WipProductProcessState.Finish)
            {
                wipProduct.Result = data.ResultType;
                wipProduct.NgQty += data.NgQty;

                //生成产品检验记录
                if (data.InspectionItems != null && data.InspectionItems.Count > 0)
                {
                    AddInspectionItems(wipProductProcess, data, version);
                }

                //生成缺陷记录
                if (data.Defects != null && data.Defects.Count > 0)
                {
                    AddDefects(wipProductProcess, data, version);
                }

                //创建SKU
                if (data.CreateSku)
                {
                    var item = version.WorkOrder.Product;
                    RT.Service.Resolve<ItemLabelController>().CreateItemLabel(item, data.Qty - data.NgQty, wipProductProcess.Barcode,
                        LabelSource.Wip, version.WorkOrderId, version.WorkOrder.FactoryId, version.WorkOrder.ItemExtProp,
                        version.WorkOrder.ItemExtPropName, version.WorkOrder.ProjectMaintain?.Code);
                }

                //更新工单状态
                collectDataDao.UpdateWorkOrderState(version.WorkOrderId, Core.WorkOrders.WorkOrderState.Producing);
            }

            version.CurrentProcessId = wipProductProcess.Id;
            version.NowProcessId = wipProductProcess.ProcessId;
            if (data.NextProcessId.IsNotEmpty())
            {
                version.NextProcessId = double.Parse(data.NextProcessId);
            }

            // 对于已经完成生产的产品进行状态设置
            if (data.IsEnd && data.ResultType != ResultType.Fail)
            {
                version.Product.State = WipProductState.Finish;
                version.IsFinish = true;
                version.FinishDateTime = currentTime;
                version.NextProcessId = null;
                version.NextProcess = null;

                //更新工单的完工数量
                collectDataDao.UpdateWorkOrderQty(version.WorkOrderId, data.Qty - data.NgQty);
            }

            return wipProductProcess;
        }

        /// <summary>
        /// 过站完成后触发
        /// </summary>
        /// <param name="data">采集数据</param>
        /// <param name="wipProcess">生产产品采集记录</param>
        /// <param name="version">生产产品版本</param>
        protected void ExecuteAfterMove(EdgeCollectData data, WipProductProcess wipProcess, WipProductVersion version)
        {
            //采集结果不为失败则生成检验单据(首检、成品检)
            if (data.ResultType != ResultType.Fail)
            {
                var firstInspEvent = new FirstInspEvent()
                {
                    WorkOrderId = version.WorkOrderId,
                    ItemId = version.WorkOrder.ProductId,
                    ProcessId = double.Parse(data.ProcessId),
                    ResourceId = double.Parse(data.LineId),
                    StationId = double.Parse(data.StationId),
                    ShopId = version.WorkOrder.WorkShopId ?? 0,
                    EmployeeId = double.Parse(data.EmployeeId),
                    Barcode = wipProcess.Barcode,
                    CustomerId = version.WorkOrder.CustomerId,
                    CollectionDate = data.CreationDate,
                    IsEndProcess = data.Sign == Tech.Routings.RoutingProcessSign.End || (int)data.Sign == 6,//6代表即是首工序，也是末工序
                    IsStartProcess = data.Sign == Tech.Routings.RoutingProcessSign.Start || (int)data.Sign == 6
                };
                RT.Service.Resolve<IFirstInsp>().GenerateFirstInsp(firstInspEvent);

                var inspEvent = new ProductInspEvent()
                {
                    WorkOrderId = version.WorkOrderId,
                    ItemId = version.WorkOrder.ProductId,
                    ProcessId = double.Parse(data.ProcessId),
                    ResourceId = double.Parse(data.LineId),
                    StationId = double.Parse(data.StationId),
                    ShopId = version.WorkOrder.WorkShopId ?? 0,
                    EmployeeId = double.Parse(data.EmployeeId),
                    Barcode = data.Barcode,
                    OkQty = data.Qty,
                    CustomerId = version.WorkOrder.CustomerId,
                    CollectionDate = data.CreationDate,
                    IsEndProcess = data.Sign == Tech.Routings.RoutingProcessSign.End || (int)data.Sign == 6,
                    IsStartProcess = data.Sign == Tech.Routings.RoutingProcessSign.Start || (int)data.Sign == 6
                };

                RT.Service.Resolve<IProductInsp>().ProductInsp(inspEvent);

                if (data.IsEnd)
                {
                    //条码完工则生成待入库条码
                    var storageEvent = new ToStorageBarcodeEvent()
                    {
                        WorkOrderId = version.WorkOrderId,
                        Barcode = data.Barcode,
                        Qty = data.Qty - data.NgQty,
                        CollectionDate = data.CreationDate
                    };
                    RT.Service.Resolve<IToStorageBarcode>().ToStorageBarcode(storageEvent);
                }
            }
        }
        /// <summary>
        /// 添加缺陷记录
        /// </summary>
        /// <param name="wipProductProcess"></param>
        /// <param name="collectData"></param>
        /// <param name="version"></param>
        private void AddDefects(WipProductProcess wipProductProcess, EdgeCollectData collectData, WipProductVersion version)
        {
            foreach (var defect in collectData.Defects)
            {
                WipProductDefect wipProductDefect = null;

                if (!defect.IsFixed)//已维修不创建新的缺陷记录
                {
                    //构建产品缺陷记录
                    wipProductDefect = new WipProductDefect()
                    {
                        DefectId = defect.DefectId,
                        InspectionItemId = defect.InspectionItemId,
                        Location = defect.Location,
                        NgQty = defect.Qty.ConvertTo<decimal>(),
                        Remark = defect.Remark,
                        Result = collectData.ResultType,
                        StationId = double.Parse(collectData.StationId),
                        ProcessId = double.Parse(collectData.ProcessId),
                        ResourceId = double.Parse(collectData.LineId),
                        ShiftId = wipProductProcess.ShiftId,
                        IsMisjudgment = false,
                        IsFixed = defect.IsFixed,
                        VersionId = wipProductProcess.VersionId
                    };
                    wipProductDefect.GenerateId();
                    version.DefectList.Add(wipProductDefect);
                }
                else//维修时更新缺陷责任
                {
                    double? inspectionItemId = defect.InspectionItemId;
                    if (inspectionItemId == 0)
                        inspectionItemId = null;

                    wipProductDefect = version.DefectList.FirstOrDefault(m => m.ProcessId == defect.ProcessId.Value && m.DefectId == defect.DefectId && m.InspectionItemId == inspectionItemId);
                    if (wipProductDefect == null)
                        continue;
                    //构建缺陷责任
                    defect.DefectResponsibility.ForEach(responsibility =>
                    {
                        wipProductDefect.ResponsibilityList.Add(new WipDefectResponsibility { DefectResponsibilityId = responsibility.DefectResponsibilityId, WipProductDefectId = wipProductDefect.Id });
                    });
                    //维修措施
                    defect.DefectMeasure.ForEach(defectMeasure =>
                    {
                        wipProductDefect.MeasureList.Add(new WipDefectMeasure { RepairMeasureId = defectMeasure.RepairMeasureId, WipProductDefectId = wipProductDefect.Id });
                    });
                    wipProductDefect.FixedById = double.Parse(collectData.EmployeeId);
                    wipProductDefect.IsFixed = defect.IsFixed;
                    wipProductDefect.ShiftId = wipProductProcess.ShiftId;
                    wipProductDefect.FixedDate = collectData.CreationDate;
                    wipProductDefect.Remark = defect.Remark;
                }
            }
            //存在已维修 处理维修记录
            if (collectData.Defects.Any(m => m.IsFixed))
            {
                var wipProductRepair = new WipProductRepair()
                {
                    VersionId = wipProductProcess.VersionId,
                    StationId = double.Parse(collectData.StationId),
                    ProcessId = double.Parse(collectData.ProcessId),
                    RepairStart = collectData.CreationDate,
                    RepaireTime = collectData.CreationDate,
                    ShiftId = wipProductProcess.ShiftId,
                    ResourceId = double.Parse(collectData.LineId),
                    ReparieById = double.Parse(collectData.EmployeeId),

                };
                wipProductRepair.GenerateId();
                wipProductProcess.Version.DefectList.ForEach(m =>
                {
                    if (m.IsFixed)
                    {
                        wipProductRepair.WipProductRepairDefectList.Add(new WipProductRepairDefect() { WipProductDefectId = m.Id, WipProductRepairId = wipProductRepair.Id });
                    }
                });
                version.RepaireList.Add(wipProductRepair);
            }
        }

        /// <summary>
        /// 添加产品检验记录
        /// </summary>
        /// <param name="wipProductProcess"></param>
        /// <param name="collectData"></param>
        /// <param name="version"></param>
        private void AddInspectionItems(WipProductProcess wipProductProcess, EdgeCollectData collectData, WipProductVersion version)
        {
            foreach (var inspectionItem in collectData.InspectionItems)
            {
                //构建产品检验记录
                WipProductInspectionItem wipProductInspectionItem = new WipProductInspectionItem()
                {
                    InspectionItemId = inspectionItem.Id,
                    Name = inspectionItem.Name,
                    LimitLow = inspectionItem.CheckTag == Defects.InspectionItems.CheckTag.Qualitative ? null : inspectionItem.LimitLow,
                    LimitMax = inspectionItem.CheckTag == Defects.InspectionItems.CheckTag.Qualitative ? null : inspectionItem.LimitMax,
                    InspectionValue = inspectionItem.CheckTag == Defects.InspectionItems.CheckTag.Qualitative ? null : inspectionItem.Value,
                    Result = inspectionItem.Result == "合格" ? ResultType.Pass : ResultType.Fail,
                    Remarks = inspectionItem.Remark,
                    InspectById = RT.IdentityId,
                    StationId = double.Parse(collectData.StationId),
                    ProcessId = double.Parse(collectData.ProcessId),
                    ShiftId = wipProductProcess.ShiftId,
                    VersionId = wipProductProcess.VersionId
                };
                wipProductInspectionItem.GenerateId();
                version.InspectionItemList.Add(wipProductInspectionItem);
            }
        }

        /// <summary>
        /// 创建在制产品版本
        /// </summary>
        /// <param name="wipProduct">生产产品</param>
        /// <param name="data">采集条码</param>
        /// <param name="versionOfSource">来源运行时</param>
        /// <returns>生产产品版本</returns>
        protected virtual WipProductVersion CreateWipProductVersion(WipProduct wipProduct, EdgeCollectData data,
            WipProductVersion versionOfSource = null)
        {
            WipProductVersion version = new WipProductVersion()
            {
                ProductId = wipProduct.Id,
                WorkOrderId = double.Parse(data.WorkOrderId),
                Grade = ProductGrade.A,
            };
            version.GenerateId();

            //SetVersionBarcode(ref version, data.BarcodeType, data.Barcode);
            version.Sn = data.Barcode;
            version.KeyLabel = data.ComBarcode;

            wipProduct.VersionList.Add(version);

            if (versionOfSource != null)
            {
                version.NextProcessId = versionOfSource?.NextProcessId;
                version.CombinedCode = versionOfSource?.CombinedCode;
            }
            //collectDataDao.SaveWipProductVersion(version, wipProduct);
            wipProduct.CurrentVersion = version;
            return version;
        }

        /// <summary>
        /// 创建在制产品
        /// </summary>
        /// <param name="product">运行时产品</param>
        /// <returns>生产产品</returns>
        protected virtual WipProduct CreateWipProduct(EdgeCollectData product)
        {
            var wipProduct = new WipProduct()
            {
                BatchQty = product.Qty,
                Grade = ProductGrade.A,
                State = WipProductState.Producing,
                ItemId = double.Parse(product.ItemId),
                Puid = product.Puid,
                Result = ResultType.Pass,
                NgQty = product.NgQty
            };
            wipProduct.GenerateId();
            return wipProduct;
        }

        /// <summary>
        /// 创建关键物料数据
        /// </summary>
        /// <param name="materials"></param>
        /// <param name="wipProductProcess"></param>
        /// <param name="data"></param>
        protected void CreateProcessKeyItem(List<EdgeMaterial> materials, WipProductProcess wipProductProcess, EdgeCollectData data)
        {
            var loadItemCtr = RT.Service.Resolve<LoadItemController>();
            foreach (var material in materials)
            {
                WipProductProcessKeyItem wipProductProcessKeyItem;
                string barcode = material.Barcode;
                if (string.IsNullOrEmpty(material.SourceId) || material.SourceId == "0.0")
                {
                    var info = loadItemService.GetMaterialInfoByBarcode(barcode);

                    if (info.ItemId.IsNullOrEmpty() || info.ItemCode.IsNullOrEmpty())
                    {
                        wipProductProcessKeyItem = new WipProductProcessKeyItem
                        {
                            ItemId = double.Parse(material.ItemId),
                            SourceCode = material.Barcode,
                            SourceId = double.Parse(material.SourceId),
                            SourceType = LoadItemSourceType.ItemLabel,
                            Qty = material.Qty
                        };
                    }
                    wipProductProcessKeyItem = new WipProductProcessKeyItem
                    {
                        ItemId = double.Parse(info.ItemId),
                        SourceCode = info.Barcode,
                        SourceId = info.SourceId,
                        SourceType = info.Type,
                        Qty = material.Qty
                    };
                }
                else
                {
                    wipProductProcessKeyItem = new WipProductProcessKeyItem
                    {
                        ItemId = double.Parse(material.ItemId),
                        SourceCode = material.Barcode,
                        SourceId = double.Parse(material.SourceId),
                        SourceType = material.SourceType.Value,
                        Qty = material.Qty
                    };
                }

                //添加关键件信息
                var keyItemProcess = wipProductProcess.KeyItemList.FirstOrDefault(p => p.SourceCode == barcode);
                if (keyItemProcess != null)
                {
                    keyItemProcess.Qty += wipProductProcessKeyItem.Qty;
                }
                else
                {
                    wipProductProcess.KeyItemList.Add(wipProductProcessKeyItem);
                }

                if (wipProductProcessKeyItem.SourceType == LoadItemSourceType.SN)
                {
                    loadItemCtr.LoadEdgeItemLabel(barcode);
                }
            }
        }

        /// <summary>
        /// 创建包装记录
        /// </summary>
        /// <param name="data">采集数据</param>
        protected virtual void CreatePackingRelation(EdgeCollectData data)
        {
            using (var tran = Domain.DB.TransactionScope(MesMqEntityDataProvider.ConnectionStringName))
            {
                var codes = data.PackageList.Select(p => p.Code).ToList();
                var packRelations = Core.Common.DataProcessEx.SplitContains(codes, (tmpCodes) =>
                {
                    return DB.Query<PackingRelationQuery>().Where(p => tmpCodes.Contains(p.PackageNo)).ToList();
                });
                if (!data.IsRemove)
                {
                    foreach (var relation in packRelations)
                    {
                        relation.PersistenceStatus = PersistenceStatus.Deleted;
                        RF.Save(relation);
                    }

                    var relationList = new EntityList<PackingRelationQuery>();
                    var waitPackList = data.PackageList.Where(p => p.PackUnitName != "主单位").ToList();
                    var ruleList = RF.GetById<WorkOrder>(double.Parse(data.WorkOrderId)).PackageRuleDetailList;
                    foreach (var record in waitPackList)
                    {
                        var relation = new PackingRelationQuery();
                        relation.GenerateId();
                        relation.PackageNo = record.Code;
                        relation.PackageUnitId = record.PackUnitId;
                        relation.WorkOrderId = record.WorkOrderId;
                        relation.ProcessId = double.Parse(record.ProcessId);
                        relation.StationId = double.Parse(record.StationId);
                        relation.PackedDate = Convert.ToDateTime(record.PackedDate);
                        relation.FullPackedQty = ruleList.First(p => p.PackageUnitId == record.PackUnitId).LevelQty;
                        relationList.Add(relation);

                        //更新包装号明细的使用状态为已使用
                        DB.Update<PackingBarcode>().Set(p => p.IsUse, true).Where(p => p.Code == record.Code).Execute();
                    }
                    RF.Save(relationList);

                    //更新非主单位包装记录的包装数和产品数、父Id、根Id
                    foreach (var relation in relationList)
                    {
                        var record = data.PackageList.First(p => p.Code == relation.PackageNo);
                        if (record.PCode.IsNotEmpty())
                        {
                            relation.TreePId = relationList.First(p => p.PackageNo == record.PCode).Id;
                        }
                        relation.PackedQty = data.PackageList.Count(p => p.PCode == record.Code);
                        relation.ItemQty = record.ItemQty;

                        var top_record = data.PackageList.First(p => p.TreeId == record.TreeId && p.PCode == "");
                        relation.RootId = relationList.First(p => p.PackageNo == top_record.Code).Id;
                        relation.PersistenceStatus = PersistenceStatus.Modified;
                    }
                    RF.Save(relationList);

                    //主单位需创建SKU，并关联包装记录的Id
                    var mainUnitPackList = data.PackageList.Where(p => p.PackUnitName == "主单位").ToList();
                    var wo = RF.GetById<WorkOrder>(mainUnitPackList[0].WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                    var item = wo.Product;
                    foreach (var record in mainUnitPackList)
                    {
                        var label = RT.Service.Resolve<ItemLabelController>()
                            .CreateItemLabel(item, 1, record.Code, LabelSource.Wip, record.WorkOrderId, wo.FactoryId,
                            wo.ItemExtProp, wo.ItemExtPropName, wo.ProjectMaintainCode);

                        var p_relation = relationList.FirstOrDefault(p => p.PackageNo == record.PCode);
                        if (p_relation != null)
                        {
                            label.RelationId = p_relation.Id;
                            RF.Save(label);
                        }
                    }

                    //生成待入库条码
                    CreateTobeWarehouseNo(data, wo);
                }
                else
                {
                    var topRecord = data.PackageList.First(p => p.PCode == "");
                    var topRelation = packRelations.FirstOrDefault(p => p.PackageNo == topRecord.Code);
                    var treeId = topRelation != null ? topRelation.TreePId : RT.Service.Resolve<ItemLabelController>().GetItemLabel(topRecord.Code).RelationId;
                    //更新移除父节点的包装数
                    DB.Update<PackingRelationQuery>()
                        .Set(p => p.PackedQty, p => p.PackedQty - 1)
                        .Where(p => p.Id == treeId)
                        .Execute();

                    //更新移除父节点及其以上节点的产品数
                    UpdateParentNodeItemQty(treeId, topRecord.ItemQty);

                    if (topRecord.PackUnitName != "主单位")
                    {
                        //更新“移除节点”的父Id和根Id
                        DB.Update<PackingRelationQuery>()
                            .Set(p => p.TreePId, p => null)
                            .Set(p => p.RootId, topRelation.Id)
                            .Where(p => p.Id == topRelation.Id)
                            .Execute();

                        //更新“移除节点”子节点的根Id
                        UpdateSubNodeRootId(topRelation.Id, topRelation, packRelations);
                    }
                    else
                    {
                        //更新“移除节点”的包装关系Id（父Id）
                        DB.Update<ItemLabel>()
                            .Set(p => p.RelationId, p => null)
                            .Where(p => p.Label == topRecord.Code)
                            .Execute();
                    }

                }
                tran.Complete();
            }
        }

        /// <summary>
        /// 生成待入库条码
        /// </summary>
        /// <param name="data"></param>
        /// <param name="wo"></param>
        private static void CreateTobeWarehouseNo(EdgeCollectData data, WorkOrder wo)
        {
            var storeRule = wo.PackageRuleDetailList.Where(p => p.IsInStockLabel || p.PackageUnit.IsMasterUnit).OrderByDescending(p => p.IsInStockLabel).FirstOrDefault();
            var storeRuleIndex = SortExtension.GetIndex(storeRule);
            if (data.PackageList.Select(p => p.Index).Contains(storeRuleIndex))
            {
                var storePackRecordList = data.PackageList.Where(p => p.Index == storeRuleIndex).ToList();
                foreach (var storePackRecord in storePackRecordList)
                {
                    var storageEvent = new ToStorageBarcodeEvent()
                    {
                        WorkOrderId = wo.Id,
                        Barcode = storePackRecord.Code,
                        Qty = storePackRecord.ItemQty,
                        CollectionDate = Convert.ToDateTime(storePackRecord.PackedDate)
                    };
                    RT.Service.Resolve<IToStorageBarcode>().ToStorageBarcode(storageEvent);
                }
            }
        }

        private void UpdateParentNodeItemQty(double? treePId, decimal itemQty)
        {
            var p_relation = RF.GetById<PackingRelationQuery>(treePId);
            if (p_relation != null)
            {
                p_relation.ItemQty -= itemQty;
                RF.Save(p_relation);
                UpdateParentNodeItemQty(p_relation.TreePId, itemQty);
            }
        }

        private void UpdateSubNodeRootId(double rootId, PackingRelationQuery p_relation, IList<PackingRelationQuery> relationList)
        {
            var subRelations = relationList.Where(p => p.TreePId == p_relation.Id).ToList();
            foreach (var subRelation in subRelations)
            {
                subRelation.RootId = rootId;
                RF.Save(subRelation);
                UpdateSubNodeRootId(rootId, subRelation, relationList);
            }

        }
    }
}
