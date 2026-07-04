using SIE.Core.Common;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.InventoryTasks;
using SIE.EMS.SpareParts;
using SIE.Equipments.Enums;
using SIE.Items;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.InventoryPlans
{
    /// <summary>
    /// 备件盘点计划控制器
    /// </summary>
    public class InventoryPlanSparePartController : DomainController
    {
        /// <summary>
        /// 验证备件盘点范围内是否有可以盘点的备件
        /// </summary>
        /// <param name="plan">盘点计划</param>
        /// <exception cref="ValidationException"></exception>
        public virtual void ValidationInventoryScopeHasSparePartDetail(InventoryPlan plan)
        {
            List<double> sparePartIds = new List<double>();
            List<double> storageLocationIds = new List<double>();

            var inventoryPlanSparePart = ValidationInventoryScope(plan, sparePartIds, storageLocationIds);

            int count = 0;

            //管控方式为空或为序列号管控,取序列号管控的备件库存
            if (!inventoryPlanSparePart.ControlMethod.HasValue
                || inventoryPlanSparePart.ControlMethod.Value == SpareParts.Enums.ControlMethod.Sn)
            {
                count +=  GetItemCodeSparePartCount(inventoryPlanSparePart, sparePartIds, storageLocationIds);
            }

            //管控方式为空或为批次管控，取批次管控的备件库存
            if (!inventoryPlanSparePart.ControlMethod.HasValue
               || inventoryPlanSparePart.ControlMethod.Value == SpareParts.Enums.ControlMethod.Batch)
            {
                count += GetBatchSparePartCount(inventoryPlanSparePart, sparePartIds, storageLocationIds);
            }

            //管控方式为空或为编码管控，取批次管控的备件库存
            if (!inventoryPlanSparePart.ControlMethod.HasValue
               || inventoryPlanSparePart.ControlMethod.Value == SpareParts.Enums.ControlMethod.ItemCode)
            {
                count += GetSnSparePartCount(inventoryPlanSparePart, sparePartIds, storageLocationIds);
            }

            if (count<=0)
            {
                throw new ValidationException("备件盘点范围找不到备件库存资料".L10N());
            }
        }

        /// <summary>
        /// 生成备件盘点任务
        /// </summary>
        /// <param name="plan">盘点计划</param>
        public virtual void GenerateSparePartInventoryTask(InventoryPlan plan)
        {
            List<double> sparePartIds = new List<double>();
            List<double> storageLocationIds = new List<double>();

            var inventoryPlanSparePart = ValidationInventoryScope(plan, sparePartIds, storageLocationIds);

            Dictionary<double, List<InventoryTaskSparePart>> inventoryTaskSparePartsDictionary
                 = new Dictionary<double, List<InventoryTaskSparePart>>();

            EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails
                = new EntityList<InventoryTaskSparePartDetail>();

            //管控方式为空或为序列号管控,取序列号管控的备件库存
            if (!inventoryPlanSparePart.ControlMethod.HasValue
                || inventoryPlanSparePart.ControlMethod.Value == SpareParts.Enums.ControlMethod.Sn)
            {
                ProcessSnSparePart(inventoryPlanSparePart, sparePartIds, storageLocationIds, inventoryTaskSparePartsDictionary, inventoryTaskSparePartDetails);
            }

            //管控方式为空或为批次管控，取批次管控的备件库存
            if (!inventoryPlanSparePart.ControlMethod.HasValue
               || inventoryPlanSparePart.ControlMethod.Value == SpareParts.Enums.ControlMethod.Batch)
            {
                ProcessBatchSparePart(inventoryPlanSparePart, sparePartIds, storageLocationIds, inventoryTaskSparePartsDictionary, inventoryTaskSparePartDetails);
            }

            //管控方式为空或为编码管控，取批次管控的备件库存
            if (!inventoryPlanSparePart.ControlMethod.HasValue
               || inventoryPlanSparePart.ControlMethod.Value == SpareParts.Enums.ControlMethod.ItemCode)
            {
                ProcessItemCodeSparePart(inventoryPlanSparePart, sparePartIds, storageLocationIds, inventoryTaskSparePartsDictionary, inventoryTaskSparePartDetails);
            }

            if (!inventoryTaskSparePartsDictionary.Any())
            {
                throw new ValidationException("备件盘点范围找不到备件库存资料".L10N());
            }

            EntityList<InventoryTask> inventoryTasks = new EntityList<InventoryTask>();

            //备件盘点范围
            EntityList<InventoryTaskSparePartScope> inventoryTaskSparePartScopes
                = new EntityList<InventoryTaskSparePartScope>();

            CreateSparePartInventoryTasks(plan, inventoryPlanSparePart, inventoryTaskSparePartsDictionary, inventoryTaskSparePartDetails, inventoryTasks, inventoryTaskSparePartScopes);

            RF.Save(inventoryTasks);
            RF.Save(inventoryTaskSparePartScopes);
        }

        private InventoryPlanSparePart ValidationInventoryScope(InventoryPlan plan, List<double> sparePartIds, List<double> storageLocationIds)
        {
            if (!plan.ResponsibleId.HasValue)
            {
                throw new ValidationException("盘点责任人不能为空".L10N());
            }

            //获取盘点范围
            InventoryPlanSparePart inventoryPlanSparePart = Query<InventoryPlanSparePart>()
                .Where(x => x.InventoryPlanId == plan.Id)
                .FirstOrDefault();

            //获取备件清单
            var spareLists = RT.Service.Resolve<InventoryPlanController>().GetSpartLists(plan.Id);
            var spareListIds = spareLists.Select(p => p.SparePartId.Value).ToList();


            if (inventoryPlanSparePart !=null)
            {
                if (inventoryPlanSparePart.ItemCategoryId.HasValue)
                {
                    //获取备件
                    var querySparePart = Query<SparePart>();

                    // 根据根节点获取物料分类下所有子节点                
                    var itemCategoryIdList = RT.Service.Resolve<ItemController>()
                      .GetItemCateNodesByPtreeCode(inventoryPlanSparePart.ItemCategoryId.Value);

                    //同一个物料分类下面不会有太多子分类，所以这里用CreateContainsExpression而不用SplitContains
                    var exp = itemCategoryIdList.Cast<double?>().ToList()
                        .CreateContainsExpression<SparePart>("x", SparePart.ItemCategoryIdProperty.Name);
                    querySparePart.Where(exp);

                    sparePartIds.AddRange(querySparePart.Select(x => x.Id).ToList().Select(x => x.Id).Distinct().ToList());
                }
                else
                {
                    //获取备件
                    var querySparePartIds = Query<SparePart>().Select(x => x.Id).ToList().Select(x => x.Id).Distinct().ToList();
                    sparePartIds.AddRange(querySparePartIds);
                }
                //库位ID列表
                storageLocationIds.AddRange(GetStorageLocationIds(inventoryPlanSparePart));
            }

            if (spareListIds.Count>0)
            {
                sparePartIds.AddRange(spareListIds);
                var locCodes = spareLists.Select(x => x.SparePart.StorageArea).ToList();
                var locIds = RT.Service.Resolve<WarehouseController>().GetStorageLocationByCodes(locCodes).Select(p => p.Id).ToList();
                storageLocationIds.AddRange(locIds);
            }
            sparePartIds = sparePartIds.Distinct().ToList();
            storageLocationIds =storageLocationIds.Distinct().ToList();

            return inventoryPlanSparePart;
        }

        /// <summary>
        /// 创建备件盘点任务
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="inventoryPlanSparePart"></param>
        /// <param name="inventoryTaskSparePartsDictionary"></param>
        /// <param name="inventoryTaskSparePartDetails"></param>
        /// <param name="inventoryTasks"></param>
        /// <param name="inventoryTaskSparePartScopes"></param>
        private void CreateSparePartInventoryTasks(InventoryPlan plan, InventoryPlanSparePart inventoryPlanSparePart, Dictionary<double, List<InventoryTaskSparePart>> inventoryTaskSparePartsDictionary, EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails, EntityList<InventoryTask> inventoryTasks, EntityList<InventoryTaskSparePartScope> inventoryTaskSparePartScopes)
        {
            var whIds = inventoryTaskSparePartsDictionary.Keys.ToList();

            var warehouseEmployees = RT.Service.Resolve<WarehouseController>()
                .GetWarehouseEmployeesByWarehouseIds(whIds);

            var warehouseEmployeesDictionary =
                warehouseEmployees.GroupBy(x => x.WarehouseId).ToDictionary(x => x.Key, x => x.ToList());

            foreach (var warehouseId in inventoryTaskSparePartsDictionary.Keys)
            {
                var task = GetInventoryTask(plan);

                task.WarehouseId = warehouseId;

                //备件盘点任务汇总
                task.InventoryTaskSparePartList.AddRange(inventoryTaskSparePartsDictionary[warehouseId]);

                //备件盘点任务明细
                task.InventoryTaskSparePartDetailList.AddRange(
                    inventoryTaskSparePartDetails.Where(x => x.WarehouseId == warehouseId));

                InventoryTaskSparePartCounter inventoryTaskSparePartCounter = new InventoryTaskSparePartCounter()
                {
                    EmployeeId = plan.ResponsibleId.Value,
                    First = true,
                    Second = true,
                };

                task.InventoryTaskSparePartCounterList.Add(inventoryTaskSparePartCounter);

                //添加有该仓库权限的员工到初盘人当中
                if (warehouseEmployeesDictionary.ContainsKey(warehouseId))
                {
                    var employees = warehouseEmployeesDictionary[warehouseId];

                    foreach (var employeeId in employees.Select(x => x.EmployeeId))
                    {
                        //员工已经添加，不重复添加
                        if (task.InventoryTaskSparePartCounterList.Any(x => x.EmployeeId == employeeId))
                        {
                            continue;
                        }

                        InventoryTaskSparePartCounter counter = new InventoryTaskSparePartCounter()
                        {
                            EmployeeId = employeeId,
                            First = true,
                            Second = false
                        };

                        task.InventoryTaskSparePartCounterList.Add(counter);
                    }
                }

                inventoryTasks.Add(task);

                InventoryTaskSparePartScope inventoryTaskSparePartScope = new InventoryTaskSparePartScope()
                {
                    InventoryTask = task,
                    WarehouseId = warehouseId,
                    StorageAreas = inventoryPlanSparePart.StorageAreas,
                    StorageAreaIds = inventoryPlanSparePart.StorageAreaIds,
                    StorageLocations = inventoryPlanSparePart.StorageLocations,
                    StorageLocationIds = inventoryPlanSparePart.StorageLocationIds,
                    AssetsCategory = inventoryPlanSparePart.AssetsCategory,
                    PartType = inventoryPlanSparePart.PartType,
                    ControlMethod = inventoryPlanSparePart.ControlMethod,
                    SparePartId = inventoryPlanSparePart.SparePartId,
                    AssetOwnerId = inventoryPlanSparePart.AssetOwnerId,
                    IsFixAsset = inventoryPlanSparePart.IsFixAsset,
                    ItemCategoryId = inventoryPlanSparePart.ItemCategoryId,
                };

                inventoryTaskSparePartScopes.Add(inventoryTaskSparePartScope);
            }
        }

        /// <summary>
        /// 获取单个盘点任务
        /// </summary>
        /// <param name="plan"></param>
        /// <returns></returns>
        private InventoryTask GetInventoryTask(InventoryPlan plan)
        {
            var taskNo = RT.Service.Resolve<CommonController>().GetNo<InventoryTask>("盘点任务");
            var task = new InventoryTask();
            task.TaskNo = taskNo;
            task.FactoryId = plan.FactoryId;
            task.InventoryPlanId = plan.Id;
            task.Percentage = 0;
            task.ApprovalStatus = ApprovalStatus.Draft;
            task.InventoryTaskStatus = InventoryTaskStatus.NotBegin;
            task.InventoryType = plan.InventoryType;
            task.Remark = plan.Remark;
            task.PlanEndDate = plan.PlanEndDate;
            task.ResponsibleId = plan.ResponsibleId;
            task.InventoryExecuteType = plan.InventoryExecuteType;
            task.NeedPhoto = plan.NeedPhoto;
            task.PhotoFilePath = plan.PhotoFilePath;
            task.IsAsset = plan.IsAsset;
            task.ApplyDate = plan.ApplyDate;
            task.GenerateId();
            return task;
        }

        /// <summary>
        /// 获取要盘点的库位列表
        /// </summary>
        /// <param name="inventoryPlanSparePart"></param>
        /// <returns></returns>
        private List<double> GetStorageLocationIds(InventoryPlanSparePart inventoryPlanSparePart)
        {
            List<double> storageLocationIds = new List<double>();
            if (!inventoryPlanSparePart.StorageLocationIds.IsNullOrEmpty())
            {
                storageLocationIds.AddRange(inventoryPlanSparePart.StorageLocationIds.Split(';').Select(x => double.Parse(x)).Distinct());
            }
            else
            {
                if (!inventoryPlanSparePart.StorageAreaIds.IsNullOrEmpty())
                {
                    var storageAreaIds = inventoryPlanSparePart.StorageAreaIds.Split(';').Select(x => double.Parse(x)).Distinct().ToList();
                    var storageLocations = RT.Service.Resolve<WarehouseController>().GetStorageLocationList(storageAreaIds);
                    storageLocationIds.AddRange(storageLocations.Select(x => x.Id).Distinct().ToList());
                }
                else
                {
                    var warehouseIds = inventoryPlanSparePart.WarehouseIds.Split(';').Select(x => double.Parse(x)).Distinct().ToList();
                    var storageLocations = RT.Service.Resolve<WarehouseController>().GetStorageLocationByWhIds(warehouseIds);
                    storageLocationIds.AddRange(storageLocations.Select(x => x.Id).Distinct().ToList());
                }
            }

            return storageLocationIds;
        }

        /// <summary>
        /// 管控方式为空或为批次管控，取批次管控的备件库存
        /// </summary>
        /// <param name="inventoryPlanSparePart"></param>
        /// <param name="sparePartIds"></param>
        /// <param name="storageLocationIds"></param>
        private int GetItemCodeSparePartCount(InventoryPlanSparePart inventoryPlanSparePart, List<double> sparePartIds, List<double> storageLocationIds)
        {
            int count = 0;

            if (inventoryPlanSparePart.SparePartId.HasValue || sparePartIds == null || !sparePartIds.Any())
            {
                count = RT.Service.Resolve<SparePartController>().GetStoreSummaryLocationCountForInventory(
                    storageLocationIds, inventoryPlanSparePart.SparePartId, inventoryPlanSparePart.PartType);
            }
            else
            {
                count = RT.Service.Resolve<SparePartController>().GetStoreSummaryLocationCountForInventory(
                    storageLocationIds, inventoryPlanSparePart.PartType, sparePartIds);
            }

            return count;

        }

        /// <summary>
        /// 管控方式为空或为批次管控，取批次管控的备件库存
        /// </summary>
        /// <param name="inventoryPlanSparePart"></param>
        /// <param name="sparePartIds"></param>
        /// <param name="storageLocationIds"></param>
        /// <param name="inventoryTaskSparePartsDictionary"></param>
        /// <param name="inventoryTaskSparePartDetails"></param>
        private void ProcessItemCodeSparePart(InventoryPlanSparePart inventoryPlanSparePart, List<double> sparePartIds, List<double> storageLocationIds, Dictionary<double, List<InventoryTaskSparePart>> inventoryTaskSparePartsDictionary, EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails)
        {
            EntityList<StoreSummaryLocation> storeSummaryLocations;

            if (inventoryPlanSparePart.SparePartId.HasValue && (sparePartIds == null || !sparePartIds.Any()))
            {
                storeSummaryLocations = RT.Service.Resolve<SparePartController>().GetStoreSummaryLocationsForInventory(
                    storageLocationIds, inventoryPlanSparePart.SparePartId, inventoryPlanSparePart.PartType);
            }
            else
            {
                storeSummaryLocations = RT.Service.Resolve<SparePartController>().GetStoreSummaryLocationsForInventory(
                    storageLocationIds, inventoryPlanSparePart.PartType, sparePartIds);
            }

            var groupViews = storeSummaryLocations
                .GroupBy(x => new
                {
                    x.StoreSummary.SparePartId,
                    x.WarehouseId
                })
                .Select(x => new
                {
                    WarehouseId = x.Key.WarehouseId,
                    SparePartId = x.Key.SparePartId,
                    GoodNumber = x.Sum(y => y.GoodNumber),
                    RotNumber = x.Sum(y => y.RotNumber),
                    SumNumber = x.Sum(y => y.SumNumber),
                });

            foreach (var g in groupViews)
            {
                if (!inventoryTaskSparePartsDictionary.ContainsKey(g.WarehouseId))
                {
                    inventoryTaskSparePartsDictionary.Add(g.WarehouseId, new List<InventoryTaskSparePart>());
                }
                var inventoryTaskSpareParts = inventoryTaskSparePartsDictionary[g.WarehouseId];

                InventoryTaskSparePart inventoryTaskSparePart = new InventoryTaskSparePart()
                {
                    SparePartId = g.SparePartId,
                    GoodQty = g.GoodNumber,
                    NgQty = g.RotNumber,
                    Total = g.SumNumber
                };

                inventoryTaskSpareParts.Add(inventoryTaskSparePart);
            }

            foreach (var storeSummaryLocation in storeSummaryLocations)
            {
                InventoryTaskSparePartDetail inventoryTaskSparePartDetail = new InventoryTaskSparePartDetail()
                {
                    InventoryStatus = InventoryStatus.Not,
                    SparePartId = storeSummaryLocation.StoreSummary.SparePartId,
                    GoodQty = storeSummaryLocation.GoodNumber,
                    NgQty = storeSummaryLocation.RotNumber,
                    Total = storeSummaryLocation.SumNumber,
                    InventoryAssetSource = InventoryAssetSource.Account,
                    WarehouseId = storeSummaryLocation.WarehouseId,
                    StorageLocationId = storeSummaryLocation.StorageLocationId,
                };

                inventoryTaskSparePartDetails.Add(inventoryTaskSparePartDetail);
            }
        }

        /// <summary>
        /// 管控方式为空或为批次管控，取批次管控的备件库存
        /// </summary>
        /// <param name="inventoryPlanSparePart"></param>
        /// <param name="sparePartIds"></param>
        /// <param name="storageLocationIds"></param>
        private int GetBatchSparePartCount(InventoryPlanSparePart inventoryPlanSparePart, List<double> sparePartIds, List<double> storageLocationIds)
        {
            int count = 0;
            if (inventoryPlanSparePart.SparePartId.HasValue || sparePartIds == null || !sparePartIds.Any())
            {
                count = RT.Service.Resolve<SparePartController>().GetStoreSummaryLotCountForInventory(
                    storageLocationIds, inventoryPlanSparePart.SparePartId, inventoryPlanSparePart.PartType);
            }
            else
            {
                count = RT.Service.Resolve<SparePartController>().GetStoreSummaryLotCountForInventory(
                    storageLocationIds, inventoryPlanSparePart.PartType, sparePartIds);
            }
             
            return count;   
        }

        /// <summary>
        /// 管控方式为空或为批次管控，取批次管控的备件库存
        /// </summary>
        /// <param name="inventoryPlanSparePart"></param>
        /// <param name="sparePartIds"></param>
        /// <param name="storageLocationIds"></param>
        /// <param name="inventoryTaskSparePartsDictionary"></param>
        /// <param name="inventoryTaskSparePartDetails"></param>
        private void ProcessBatchSparePart(InventoryPlanSparePart inventoryPlanSparePart, List<double> sparePartIds, List<double> storageLocationIds, Dictionary<double, List<InventoryTaskSparePart>> inventoryTaskSparePartsDictionary, EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails)
        {
            EntityList<StoreSummaryLot> storeSummaryLots;

            if (inventoryPlanSparePart.SparePartId.HasValue && (sparePartIds == null || !sparePartIds.Any()))
            {
                storeSummaryLots = RT.Service.Resolve<SparePartController>().GetStoreSummaryLotsForInventory(
                    storageLocationIds, inventoryPlanSparePart.SparePartId, inventoryPlanSparePart.PartType);
            }
            else
            {
                storeSummaryLots = RT.Service.Resolve<SparePartController>().GetStoreSummaryLotsForInventory(
                    storageLocationIds, inventoryPlanSparePart.PartType, sparePartIds);
            }

            var groupViews = storeSummaryLots
                .GroupBy(x => new
                {
                    x.StoreSummary.SparePartId,
                    x.WarehouseId
                })
                .Select(x => new
                {
                    WarehouseId = x.Key.WarehouseId,
                    SparePartId = x.Key.SparePartId,
                    GoodNumber = x.Sum(y => y.GoodNumber),
                    RotNumber = x.Sum(y => y.RotNumber),
                    SumNumber = x.Sum(y => y.SumNumber),
                });

            foreach (var g in groupViews)
            {
                if (!inventoryTaskSparePartsDictionary.ContainsKey(g.WarehouseId))
                {
                    inventoryTaskSparePartsDictionary.Add(g.WarehouseId, new List<InventoryTaskSparePart>());
                }
                var inventoryTaskSpareParts = inventoryTaskSparePartsDictionary[g.WarehouseId];

                InventoryTaskSparePart inventoryTaskSparePart = new InventoryTaskSparePart()
                {
                    SparePartId = g.SparePartId,
                    GoodQty = g.GoodNumber,
                    NgQty = g.RotNumber,
                    Total = g.SumNumber
                };

                inventoryTaskSpareParts.Add(inventoryTaskSparePart);
            }

            foreach (var storeSummaryLot in storeSummaryLots)
            {
                InventoryTaskSparePartDetail inventoryTaskSparePartDetail = new InventoryTaskSparePartDetail()
                {
                    InventoryStatus = InventoryStatus.Not,
                    SparePartId = storeSummaryLot.StoreSummary.SparePartId,
                    LotNo = storeSummaryLot.BatchNumber,
                    StoreSummaryLotId = storeSummaryLot.Id,
                    GoodQty = storeSummaryLot.GoodNumber,
                    NgQty = storeSummaryLot.RotNumber,
                    Total = storeSummaryLot.SumNumber,
                    InventoryAssetSource = InventoryAssetSource.Account,
                    WarehouseId = storeSummaryLot.WarehouseId,
                    StorageLocationId = storeSummaryLot.StorageLocationId,
                };

                inventoryTaskSparePartDetails.Add(inventoryTaskSparePartDetail);
            }
        }

        /// <summary>
        /// 管控方式为空或为序列号管控,取序列号管控的备件库存
        /// </summary>
        /// <param name="inventoryPlanSparePart"></param>
        /// <param name="sparePartIds"></param>
        /// <param name="storageLocationIds"></param>
        private int GetSnSparePartCount(InventoryPlanSparePart inventoryPlanSparePart, List<double> sparePartIds, List<double> storageLocationIds)
        {
            int count = 0; 

            if (inventoryPlanSparePart.SparePartId.HasValue || sparePartIds == null || !sparePartIds.Any())
            {
                count = RT.Service.Resolve<SparePartController>().GetStoreSummaryDetailCountForInventory(storageLocationIds,
                    inventoryPlanSparePart.SparePartId, inventoryPlanSparePart.PartType, inventoryPlanSparePart.IsFixAsset,
                    inventoryPlanSparePart.AssetsCategory, inventoryPlanSparePart.AssetOwnerId);
            }
            else
            {
                count = RT.Service.Resolve<SparePartController>().GetStoreSummaryDetailCountForInventory(storageLocationIds,
                    inventoryPlanSparePart.PartType, sparePartIds, inventoryPlanSparePart.IsFixAsset,
                    inventoryPlanSparePart.AssetsCategory, inventoryPlanSparePart.AssetOwnerId);
            }

            return count;
        }

        /// <summary>
        /// 管控方式为空或为序列号管控,取序列号管控的备件库存
        /// </summary>
        /// <param name="inventoryPlanSparePart"></param>
        /// <param name="sparePartIds"></param>
        /// <param name="storageLocationIds"></param>
        /// <param name="inventoryTaskSparePartsDictionary"></param>
        /// <param name="inventoryTaskSparePartDetails"></param>
        private void ProcessSnSparePart(InventoryPlanSparePart inventoryPlanSparePart, List<double> sparePartIds, List<double> storageLocationIds, Dictionary<double, List<InventoryTaskSparePart>> inventoryTaskSparePartsDictionary, EntityList<InventoryTaskSparePartDetail> inventoryTaskSparePartDetails)
        {
            EntityList<StoreSummaryDetail> storeSummaryDetails;

            if (inventoryPlanSparePart.SparePartId.HasValue && (sparePartIds == null || !sparePartIds.Any()))
            {
                storeSummaryDetails = RT.Service.Resolve<SparePartController>().GetStoreSummaryDetailsForInventory(storageLocationIds,
                    inventoryPlanSparePart.SparePartId, inventoryPlanSparePart.PartType, inventoryPlanSparePart.IsFixAsset,
                    inventoryPlanSparePart.AssetsCategory, inventoryPlanSparePart.AssetOwnerId);
            }
            else
            {
                storeSummaryDetails = RT.Service.Resolve<SparePartController>().GetStoreSummaryDetailsForInventory(storageLocationIds,
                    inventoryPlanSparePart.PartType, sparePartIds, inventoryPlanSparePart.IsFixAsset,
                    inventoryPlanSparePart.AssetsCategory, inventoryPlanSparePart.AssetOwnerId);
            }

            var groupViews = storeSummaryDetails
                .GroupBy(x => new
                {
                    x.StoreSummary.SparePartId,
                    x.WarehouseId
                })
                .Select(x => new
                {
                    WarehouseId = x.Key.WarehouseId,
                    SparePartId = x.Key.SparePartId,
                    GoodNumber = x.Sum(y => (y.OdNbStatus == OdNbStatus.GoodProduct) ? 1 : 0),
                    RotNumber = x.Sum(y => (y.OdNbStatus == OdNbStatus.NoGoodProduct) ? 1 : 0),
                    SumNumber = x.Count(),
                });

            foreach (var g in groupViews)
            {
                if (!inventoryTaskSparePartsDictionary.ContainsKey(g.WarehouseId))
                {
                    inventoryTaskSparePartsDictionary.Add(g.WarehouseId, new List<InventoryTaskSparePart>());
                }
                var inventoryTaskSpareParts = inventoryTaskSparePartsDictionary[g.WarehouseId];

                InventoryTaskSparePart inventoryTaskSparePart = new InventoryTaskSparePart()
                {
                    SparePartId = g.SparePartId,
                    GoodQty = g.GoodNumber,
                    NgQty = g.RotNumber,
                    Total = g.SumNumber
                };

                inventoryTaskSpareParts.Add(inventoryTaskSparePart);
            }

            foreach (var storeSummaryDetail in storeSummaryDetails)
            {
                int good = 0;
                int ng = 0;
                switch (storeSummaryDetail.OdNbStatus)
                {
                    case OdNbStatus.NoGoodProduct:
                        {
                            good = 0;
                            ng = 1;
                        }
                        break;
                    case OdNbStatus.GoodProduct:
                        {
                            good = 1;
                            ng = 0;
                        }
                        break;
                    default:
                        break;
                }

                InventoryTaskSparePartDetail inventoryTaskSparePartDetail = new InventoryTaskSparePartDetail()
                {
                    InventoryStatus = InventoryStatus.Not,
                    SparePartId = storeSummaryDetail.StoreSummary.SparePartId,
                    Sn = storeSummaryDetail.OrderNumberCode,
                    StoreSummaryDetailId = storeSummaryDetail.Id,
                    GoodQty = good,
                    NgQty = ng,
                    Total = 1,
                    InventoryAssetSource = InventoryAssetSource.Account,
                    WarehouseId = storeSummaryDetail.WarehouseId,
                    StorageLocationId = storeSummaryDetail.StorageLocationId,
                };

                inventoryTaskSparePartDetails.Add(inventoryTaskSparePartDetail);
            }
        }
    }
}
