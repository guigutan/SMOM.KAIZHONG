using SIE.Domain;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.Warehouses;
using SIE.Equipments.Enums;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.SpareParts
{
    /// <summary>
    /// 备件调度控制器
    /// </summary>
    public class SpartPareReportJobController : DomainController
    {
        /// <summary>
        /// 异步调度生成备件库存统计数据
        /// </summary>
        public virtual void SyncSchedulingAutoStatistics()
        {
            var now = DateTime.Now.Date; // RF.Find<SparePart>().GetDbTime().Date;

            var isCreated = Query<SparePartMixReportRecord>().Where(m => m.SchedulingDate == now).FirstOrDefault() != null;
            if (isCreated)//已经生成过当天的数据
                return;

            EntityList<SparePartMixReportRecord> spartPartMixReportRecords = new EntityList<SparePartMixReportRecord>();

            var totalWareHouseList = GetStoreSummaryWarehouse();

            //按仓库分类所有仓库详情
            var totalDic = totalWareHouseList.GroupBy(m => m.WarehouseId).ToDictionary(p => p.Key, p => p.ToList());
            //按仓库统计每个仓库的库存情况
            List<StoreSummaryWarehouse> resultGroupByWhlist = new List<StoreSummaryWarehouse>();
            totalDic.ForEach(key =>
            {
                var sumItem = new StoreSummaryWarehouse()
                {
                    WarehouseId = key.Key,
                    RotNumber = key.Value.Sum(p => p.RotNumber),
                    GoodNumber = key.Value.Sum(p => p.GoodNumber),
                    SumNumber = key.Value.Sum(p => p.SumNumber),
                };
                resultGroupByWhlist.Add(sumItem);
            });

            //仓库剩余成本 仓库总库存数*成本
            Dictionary<double, decimal> warehouseCostDic = new Dictionary<double, decimal>();

            //取出所有的备件非0库存 
            var storeSummaryAllList = Query<StoreSummary>().Where(m => m.SumNumber > 0).ToList();
            //按备件编号分类统计库存情况
            var totalSparePartIdDic = totalWareHouseList.GroupBy(m => m.SpartpartId).ToDictionary(p => p.Key, p => p.ToList());
            //统计目前库存情况下非0成本仓的剩余库存金额
            SummaryWarehouseAmount(warehouseCostDic, storeSummaryAllList, totalSparePartIdDic);
            //按当前所有仓库创建须保存数据 后续按仓库更新其中数据
            CreateSaveRecordByWarehouseKeys(now, spartPartMixReportRecords, totalDic, resultGroupByWhlist, warehouseCostDic);

            //统计入库情况
            //取出当天所有已出库的出库明细
            SummaryInStorageInfo(now, spartPartMixReportRecords, resultGroupByWhlist, warehouseCostDic);
            //统计出库情况
            SummaryOutDepotInfo(now, spartPartMixReportRecords);
            if (spartPartMixReportRecords.Any())
            {
                RF.Save(spartPartMixReportRecords);
            }
        }

        /// <summary>
        /// 统计出库情况
        /// </summary>
        /// <param name="now"></param>
        /// <param name="spartPartMixReportRecords"></param>
        private void SummaryOutDepotInfo(DateTime now, EntityList<SparePartMixReportRecord> spartPartMixReportRecords)
        {
            var partOutDepotDetailList = Query<PartOutDepotDetail>().Where(m => m.OutboundStatus == OutboundStatus.Shipped && m.OutDepotDate > now && m.OutDepotDate < now.AddDays(1))
              .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //按仓库作为key统计
            var outDepotDetailAmountDic = new Dictionary<double, decimal>();
            var partOutDepotDetaiDic = partOutDepotDetailList.GroupBy(m => m.WarehouseId).ToDictionary(p => p.Key, p => p.ToList());
            partOutDepotDetailList.ForEach(partOutDepotDetail =>
            {
                var cost = partOutDepotDetail.OutDepotCount * partOutDepotDetail.UnitPrice;
                if (outDepotDetailAmountDic.ContainsKey(partOutDepotDetail.WarehouseId))//当前仓库已计算过则累计
                {
                    outDepotDetailAmountDic[partOutDepotDetail.WarehouseId] += (decimal)cost;

                }
                else
                {
                    outDepotDetailAmountDic.Add(partOutDepotDetail.WarehouseId, (decimal)cost);
                }

            });

            foreach (var key in partOutDepotDetaiDic.Keys)
            {
                var outDepotDetaiSumQty = partOutDepotDetaiDic[key].Sum(m => m.OutDepotCount);
                var exsitedResult = spartPartMixReportRecords.FirstOrDefault(m => m.WarehouseId == key);
                if (exsitedResult != null)
                {
                    exsitedResult.ExWarehouseAmount = outDepotDetailAmountDic.ContainsKey(key) ? outDepotDetailAmountDic[key] : 0;
                    exsitedResult.ExWarehouseQty = outDepotDetaiSumQty;
                }
            }
        }

        /// <summary>
        /// 统计入库情况
        /// </summary>
        /// <param name="now"></param>
        /// <param name="spartPartMixReportRecords"></param>
        /// <param name="resultGroupByWhlist"></param>
        /// <param name="warehouseCostDic"></param>
        private void SummaryInStorageInfo(DateTime now, EntityList<SparePartMixReportRecord> spartPartMixReportRecords, List<StoreSummaryWarehouse> resultGroupByWhlist, Dictionary<double, decimal> warehouseCostDic)
        {
            var inStorageList = Query<StoreDetail>().Where(m => m.InboundStatus == InboundStatus.Done && m.UpdateDate > now && m.UpdateDate < now.AddDays(1))
                           .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var receiptAmountDic = new Dictionary<double, decimal>();
            var storeDetailDic = inStorageList.GroupBy(m => m.WarehouseId).ToDictionary(p => p.Key, p => p.ToList());
            //统计各个仓库入库单入库的金额
            inStorageList.ForEach(storeDetail =>
            {

                var cost = storeDetail.Number * (storeDetail.SparePartUnitPrice.HasValue ? storeDetail.SparePartUnitPrice.Value : 0);
                if (receiptAmountDic.ContainsKey(storeDetail.WarehouseId))//当前仓库已计算过则累计
                {
                    receiptAmountDic[storeDetail.WarehouseId] += cost;

                }
                else
                {
                    receiptAmountDic.Add(storeDetail.WarehouseId, cost);
                }

            });
            foreach (var key in storeDetailDic.Keys)
            {
                var storeDetailQty = storeDetailDic[key].Sum(m => m.Number);
                var warehouseReceiptAmount = receiptAmountDic.ContainsKey(key) ? receiptAmountDic[key] : 0;

                var exsitedResult = spartPartMixReportRecords.FirstOrDefault(m => m.WarehouseId == key);
                if (exsitedResult != null)
                {
                    exsitedResult.ReceiptQty = storeDetailQty;
                    exsitedResult.ReceiptAmount = warehouseReceiptAmount;
                }
            }
        }


        /// <summary>
        /// 统计目前库存情况下非0成本仓的剩余库存金额
        /// </summary>
        /// <param name="warehouseCostDic"></param>
        /// <param name="storeSummaryAllList"></param>
        /// <param name="totalSparePartIdDic"></param>
        private void SummaryWarehouseAmount(Dictionary<double, decimal> warehouseCostDic, EntityList<StoreSummary> storeSummaryAllList, Dictionary<double, List<StoreSummaryWarehouse>> totalSparePartIdDic)
        {
            foreach (var itme in storeSummaryAllList)
            {
                if (totalSparePartIdDic.ContainsKey(itme.SparePartId))
                {
                    var avgCost = itme.AverageCost;
                    var warehouseSummaryList = totalSparePartIdDic[itme.SparePartId];
                    warehouseSummaryList.ForEach(it =>
                    {
                        if (!it.IsZeroCost)//非0成本仓才计算结余金额
                        {
                            if (warehouseCostDic.ContainsKey(it.WarehouseId))//当前仓库已计算过则累计
                            {
                                warehouseCostDic[it.WarehouseId] += it.SumNumber * avgCost;

                            }
                            else
                            {
                                warehouseCostDic.Add(it.WarehouseId, it.SumNumber * avgCost);
                            }
                        }
                    });
                }

            }
        }

        /// <summary>
        /// 按当前所有仓库创建须保存数据 后续按仓库更新其中数据
        /// </summary>
        /// <param name="now"></param>
        /// <param name="spartPartMixReportRecords"></param>
        /// <param name="totalDic"></param>
        private void CreateSaveRecordByWarehouseKeys(DateTime now, EntityList<SparePartMixReportRecord> spartPartMixReportRecords, Dictionary<double, List<StoreSummaryWarehouse>> totalDic
            , List<StoreSummaryWarehouse> resultGroupByWhlist, Dictionary<double, decimal> warehouseCostDic)
        {
            foreach (var key in totalDic.Keys)
            {
                var whSurplusQty = resultGroupByWhlist.FirstOrDefault(m => m.WarehouseId == key);
                var warehouseCost = warehouseCostDic.ContainsKey(key) ? warehouseCostDic[key] : 0;
                spartPartMixReportRecords.Add(new SparePartMixReportRecord()
                {
                    WarehouseId = key,
                    ReceiptQty = 0,
                    ReceiptAmount = 0,
                    ExWarehouseAmount=0,
                    ExWarehouseQty=0,
                    SchedulingDate = now,
                    SurplusQty = whSurplusQty != null ? whSurplusQty.SumNumber : 0,
                    SurplusAmount = warehouseCost,
                  
                });
            }
        }

        /// <summary>
        /// 获取目前的库存总数
        /// </summary>
        /// <returns></returns>

        private List<StoreSummaryWarehouse> GetStoreSummaryWarehouse()
        {
            List<StoreSummaryWarehouse> dblist = new List<StoreSummaryWarehouse>();
            //按备件编码管控
            var storeSummaryLocationQurey = Query<StoreSummaryLocation>().GroupBy(p => new
            {
                p.StoreSummary.SparePartId,
                p.WarehouseId,
                p.Warehouse.Code,
                p.Warehouse.Name,
                p.Warehouse.LibraryType
            }).Select(p => new
            {
                SpartpartId = p.StoreSummary.SparePartId,
                WarehouseId = p.WarehouseId,
                WarehouseCode = p.Warehouse.Code,
                WarehouseName = p.Warehouse.Name,
                LibraryType = p.Warehouse.LibraryType,
                RotNumber = p.RotNumber.SUM(),
                GoodNumber = p.GoodNumber.SUM(),
                SumNumber = p.SumNumber.SUM()
            });
            var storeSummaryLocation = storeSummaryLocationQurey.ToList<StoreSummaryWarehouse>();
            if (storeSummaryLocation.Any())
            {
                dblist.AddRange(storeSummaryLocation);
            }
            //按批次管控
            var storeSummaryLotQuery = Query<StoreSummaryLot>().GroupBy(p => new
            {
                p.StoreSummary.SparePartId,
                p.WarehouseId,
                p.Warehouse.Code,
                p.Warehouse.Name,
                p.Warehouse.LibraryType,
            }).Select(p => new
            {
                SpartpartId = p.StoreSummary.SparePartId,
                WarehouseId = p.WarehouseId,
                WarehouseCode = p.Warehouse.Code,
                WarehouseName = p.Warehouse.Name,
                LibraryType = p.Warehouse.LibraryType,
                RotNumber = p.RotNumber.SUM(),
                GoodNumber = p.GoodNumber.SUM(),
                SumNumber = p.SumNumber.SUM()
            });
            var storeSummaryLot = storeSummaryLotQuery.ToList<StoreSummaryWarehouse>(null);
            if (storeSummaryLot.Any())
            {
                dblist.AddRange(storeSummaryLot);

            }
            //按序列号管控
            var storeSummaryDetailQurey = Query<StoreSummaryDetail>().GroupBy(p => new
            {
                p.StoreSummary.SparePartId,
                p.WarehouseId,
                p.Warehouse.Code,
                p.Warehouse.Name,
                p.Warehouse.LibraryType,
            }).Select(p => new
            {
                SpartpartId = p.StoreSummary.SparePartId,
                WarehouseId = p.WarehouseId,
                WarehouseCode = p.Warehouse.Code,
                WarehouseName = p.Warehouse.Name,
                LibraryType = p.Warehouse.LibraryType,
                RotNumber = p.RotNumber.SUM(),
                GoodNumber = p.GoodNumber.SUM(),
                SumNumber = p.SumNumber.SUM()
            });
            var storeSummaryDetail = storeSummaryDetailQurey.ToList<StoreSummaryWarehouse>();
            if (storeSummaryDetail.Any())
            {
                dblist.AddRange(storeSummaryDetail);

            }
            if (dblist.Any())
            {
                var whList = dblist.Select(p => p.WarehouseCode).Distinct().SplitContains(tempCodes =>
                {
                    return Query<Warehouse>().Where(p => tempCodes.Contains(p.Code)).ToList();
                });

                dblist.ForEach(data =>
                {
                    data.IsZeroCost = whList.First(p => p.Code == data.WarehouseCode).GetIsZeroCost();
                });
            }


            return dblist;
        }
    }
}
