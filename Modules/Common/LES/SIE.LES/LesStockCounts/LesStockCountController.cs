using SIE.Common.Configs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Common.NumberRules;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.WMS;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Inventory.Task;
using SIE.Inventory.TransactionProcessing;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.Items.Items;
using SIE.LES.LesStockCounts.Datas;
using SIE.LES.LesStockCounts.ViewModels;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 线边仓盘点控制器
    /// </summary>
    public partial class LesStockCountController : DomainController
    {
        /// <summary>
        /// 获取盘点单
        /// </summary>
        /// <param name="criteria">查询实体</param>
        /// <returns>盘点单集合</returns>
        public virtual EntityList<LesStockCount> GetLesStockCounts(LesStockCountCriteria criteria)
        {
            var query = Query<LesStockCount>();
            if (!criteria.No.IsNullOrEmpty())
            {
                query.Where(p => p.No.Contains(criteria.No));
            }

            if (!criteria.SourceBillNo.IsNullOrEmpty())
            {
                query.Where(p => p.SourceBillNo.Contains(criteria.SourceBillNo));
            }
            if (!string.IsNullOrEmpty(criteria.State))
            {
                var criteriaState = new List<int>();
                criteria.State.Split(',').ForEach(state =>
                {
                    criteriaState.Add(int.Parse(state));
                });
                query.Where(p => criteriaState.Contains((int)p.State));
            }
            if (criteria.LesStockCountResult.HasValue)
            {
                query.Where(p => p.LesStockCountResult == criteria.LesStockCountResult);
            }

            //创建时间
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }

            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }

            //创建人
            if (criteria.CreateById.HasValue)
            {
                query.Where(p => p.CreateBy == criteria.CreateById.Value);
            }
            List<double?> empWareIds = new List<double?>();

            if (criteria.WarehouseId.HasValue && criteria.WarehouseId > 0)
            {
                empWareIds.Clear();
                empWareIds.Add(criteria.WarehouseId.Value);
            }
            else
            {
                var empWarehouseIds = RT.Service.Resolve<WarehouseController>().GetEmployeeWarehouseIds().ToList();
                empWarehouseIds.ForEach(p =>
                {
                    empWareIds.Add(p);
                });
            }

            if (criteria.ItemId > 0 || criteria.WarehouseId > 0)
            {
                query.Exists<LesStockCountDetail>((x, y) => y.Where(p => p.LesStockCountId == x.Id && empWareIds.Contains(p.WarehouseId))
                    .WhereIf(criteria.ItemId > 0, p => p.ItemId == criteria.ItemId)
                    .WhereIf(criteria.WarehouseId > 0, p => p.WarehouseId == criteria.WarehouseId));
            }
            if (criteria.CountDimension.HasValue)
            {
                query.Exists<LesStockCountRange>((x, y) => y.Where(p => p.LesStockCountId == x.Id && p.CountDimension == criteria.CountDimension));
            }

            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            query.OrderBy(criteria.OrderInfoList);
            return query.ToList(criteria.PagingInfo, elo);
        }

        /// <summary>
        /// 获取盘点单
        /// </summary>
        /// <param name="ids">查询实体</param>
        /// <returns>盘点单集合</returns>
        public virtual EntityList<LesStockCount> GetLesStockCounts(List<double> ids)
        {
            return ids.SplitContains(pIds =>
            {
                return Query<LesStockCount>().Where(p => pIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 创建盘点单号
        /// </summary>
        /// <returns>盘点单号</returns>
        public virtual string GetCountNo()
        {
            var config = ConfigService.GetConfig<NoConfigValue>(new NoConfig(), typeof(LesStockCount));
            if (config == null || config.BacodeRule == null)
            {
                throw new ValidationException("未找到盘点单号生成规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.BacodeRule.Id, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取物料标签信息
        /// </summary>
        /// <param name="label">标签号</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="factoryId">工厂ID</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        public virtual void GetItemLabel(string label, double itemId, double warehouseId, string itemExtProp)
        {
            var itemLabel = Query<ItemLabel>().Where(p => p.Label == label).ToList();
            if (itemLabel.Any())
            {
                //if (!itemLabel.Any(p => p.ItemId == itemId && p.WarehouseId == warehouseId && p.FactoryId == factoryId && p.ItemExtProp == itemExtProp && p.StorageLocationId == storageLocationId))
                //{
                //    throw new ValidationException("未找到与（物料+物料扩展属性+仓库+库位+工厂）一致的物料标签[{0}]数据,请检查输入的物料标签".L10nFormat(label));
                //}
                if (itemLabel.Any(p => p.IsSerialNumber == true && (p.Qty > 0 || p.NgQty > 0)))
                {
                    throw new ValidationException("物料标签[{0}]为序列号标签，且可用数或不良数大于0,无法新增盘盈!".L10nFormat(label));
                }
            }
            else
            {
                throw new ValidationException("未能从物料标签信息中找到物料标签为[{0}]的数据,请检查输入的物料标签".L10nFormat(label));
            }
        }

        /// <summary>
        /// 关闭盘点单
        /// </summary>
        /// <param name="lesStockCountId">盘点单ID</param>
        /// <returns>盘点单</returns>
        public virtual LesStockCount CloseLesStockCount(double lesStockCountId)
        {
            var bill = RF.GetById<LesStockCount>(lesStockCountId);
            if (bill.State != LesCountState.Audit && bill.State != LesCountState.PartCount && bill.State != LesCountState.FinishCount)
            {
                throw new ValidationException("盘点单[{0}]单据状态为[{1}]，无法关闭".L10nFormat(bill.No, bill.State.ToLabel()));
            }
            var billDtls = bill.LesStockCountDetailList.Where(p => p.State != LesCountState.Close);
            if (billDtls.Any(p => p.State != LesCountState.Audit && p.State != LesCountState.FinishCount))
            {
                throw new ValidationException("盘点单[{0}]明细行状态不是审批或已盘点".L10nFormat(bill.No));
            }
            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                foreach (var dtl in billDtls)
                {
                    dtl.State = LesCountState.Close;
                }
                bill.State = LesCountState.Close;
                RF.Save(bill);
                trans.Complete();
            }
            return bill;
        }

        /// <summary>
        /// 根据盘点单ID查找盘点范围
        /// </summary>
        /// <param name="stockCountId"></param>
        /// <returns></returns>
        public virtual LesStockCountRange GetLesStockCountRange(double stockCountId)
        {
            EagerLoadOptions elo = new EagerLoadOptions();
            elo.LoadWithViewProperty();
            var query = Query<LesStockCountRange>().Where(p => p.LesStockCountId == stockCountId);
            return query.FirstOrDefault(elo);
        }

        /// <summary>
        /// 关闭盘点明细
        /// </summary>
        /// <param name="billId">单据Id</param>
        /// <param name="dtlIdList">明细ID</param>
        public virtual LesStockCount CloseLesStockCountDtl(double billId, List<double> dtlIdList)
        {
            var bill = RF.GetById<LesStockCount>(billId);
            var dtlList = bill.LesStockCountDetailList.Where(f => dtlIdList.Contains(f.Id));
            if (!dtlList.Any())
            {
                throw new ValidationException("明细行数为0".L10N());
            }
            if (dtlList.Any(p => p.State != LesCountState.Audit && p.State != LesCountState.FinishCount))
            {
                throw new ValidationException("明细行状态不是审批或已盘点".L10N());
            }
            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                foreach (var dtl in bill.LesStockCountDetailList.Where(p => dtlIdList.Contains(p.Id)))
                {
                    dtl.State = LesCountState.Close;
                }
                UpdateLesCountState(bill);
                RF.Save(bill);
                trans.Complete();
                return bill;
            }
        }

        /// <summary>
        /// 根据明细Id列表获取盘点明细
        /// </summary>
        /// <param name="idList">明细Id列表</param>
        /// <returns>返回盘点明细</returns>
        public virtual EntityList<LesStockCountDetail> GetLesStockCountDetailList(List<double> idList)
        {
            return idList.SplitContains(ids =>
            {
                return Query<LesStockCountDetail>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 根据盘点单Id获取盘点明细
        /// </summary>
        /// <param name="stockCountId">盘点单Id</param>
        /// <returns>返回盘点明细</returns>
        public virtual EntityList<LesStockCountDetail> GetStockCountDetail(double stockCountId)
        {
            var query = Query<LesStockCountDetail>().Where(p => p.LesStockCountId == stockCountId);
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 验证盘点范围
        /// </summary>
        /// <param name="range">范围</param>
        /// <exception cref="ValidationException">验证数据</exception>
        private void ValiateRangeData(LesStockCountRange range)
        {
            if (range == null)
            {
                throw new ValidationException("单据盘点范围未维护".L10N());
            }
            if (range.Warehouses.IsNullOrEmpty())
            {
                throw new ValidationException("仓库不能为空".L10N());
            }
        }

        /// <summary>
        /// 库位+标签细度校验是否重复
        /// </summary>
        /// <param name="bill">线边仓盘点单据</param>
        private void LocationStockCountDetailComparerFunc(LesStockCount bill)
        {
            //根据仓库+标签号+库位做汇总
            var sameDtlCount = bill.LesStockCountDetailList.GroupBy(p => new LesStockCountDetail
            {
                ItemId = p.ItemId,
                WarehouseId = p.WarehouseId,
                StorageLocationId = p.StorageLocationId,
                LabelNo = p.LabelNo,
                LotId = null,
                OnhandState = p.OnhandState,
                ItemExtProp = p.ItemExtProp
            }, (m, f) => new { Dtl = m, LineNo = f.Min(p => p.LineNo), Count = f.Count() }, new LesStockCountDetailComparer());

            var sameIsNewCount = bill.LesStockCountDetailList.Where(p => p.IsNewInventory).GroupBy(p => new LesStockCountDetail
            {
                ItemId = p.ItemId,
                WarehouseId = p.WarehouseId,
                StorageLocationId = p.StorageLocationId,
                LabelNo = p.LabelNo,
                LotId = null,
                OnhandState = p.OnhandState,
                ItemExtProp = p.ItemExtProp
            }, (m, f) => new { Dtl = m, LineNo = f.Min(p => p.LineNo), Count = f.Count() }, new LesStockCountDetailComparer());

            if (sameDtlCount.Count(p => p.Count > 1) - sameIsNewCount.Count(p => p.Count > 1) != 0)
            {
                //var stockDetail = sameDtlCount.Where(p => p.Count > 1).OrderBy(p => p.LineNo).Select(p => p.Dtl).FirstOrDefault();
                var LineNo = sameDtlCount.Where(p => p.Count > 1).OrderBy(p => p.LineNo).Select(p => p.LineNo).FirstOrDefault();
                var stockDetail = bill.LesStockCountDetailList.FirstOrDefault(p => p.LineNo == LineNo);
                setCompareErrorMsg(stockDetail);
            }
        }

        /// <summary>
        /// 保存盘点单
        /// </summary>
        /// <param name="bill">盘点单</param>
        public virtual LesStockCount SaveLesStockCount(LesStockCount bill)
        {
            if (bill == null)
            {
                throw new ArgumentNullException(nameof(bill));
            }

            var range = bill.LesStockCountRangeList.FirstOrDefault();
            //验证盘点范围数据正确性
            ValiateRangeData(range);

            //验证盘点明细行数据正确性
            ValiateCountDtl(bill, range);

            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                if (bill.State == LesCountState.Audit || bill.State == LesCountState.PartCount || bill.State == LesCountState.FinishCount)
                {
                    Dictionary<double, LesStockCountDetail> dtlDic = new Dictionary<double, LesStockCountDetail>();
                    var lotids = bill.LesStockCountDetailList.Where(p => (p.State == LesCountState.Audit || p.State == LesCountState.FinishCount) && p.LotId.HasValue).Select(p => p.LotId.Value).Distinct().ToList();
                    var lots = RT.Service.Resolve<LotController>().GetLots(lotids);
                    //更新状态为审核的明细
                    bill.LesStockCountDetailList.Where(p => p.State == LesCountState.Audit || p.State == LesCountState.FinishCount).ForEach(p =>
                    {
                        if (!p.IsNewInventory && p.State == LesCountState.Audit && p.ActualCountQty.HasValue && range.IsDynamicOnhand)
                        {
                            string lotCode = "";
                            if (p.LotId.HasValue)
                            {
                                var lot = lots.FirstOrDefault(f => f.Id == p.LotId.Value);
                                if (lot != null)
                                {
                                    lotCode = lot.Code;
                                }
                            }
                            var nowQty = GetStockCountDetailQty(p.ItemId, p.WarehouseId, p.LabelNo, p.ItemExtPropName, lotCode, p.StorageLocationId, p.OnhandState, p.FactoryId);
                            p.Qty = nowQty;
                        }
                        if (p.State == LesCountState.Audit && p.ActualCountQty.HasValue)
                        {
                            p.DiffCountQty = p.ActualCountQty - p.Qty;
                            p.CountById = RT.IdentityId;
                            p.CountDate = DateTime.Now;
                            if (p.DiffCountQty != 0)
                            {
                                p.LesStockCountDetailResult = LesStockCountDetailResult.Abnormal;
                            }
                            else
                            {
                                p.LesStockCountDetailResult = LesStockCountDetailResult.Normal;
                            }
                            UpdateCountDtlState(p);
                            dtlDic.Add(p.Id, p);
                        }
                        if (p.State == LesCountState.FinishCount && !p.ActualCountQty.HasValue)
                        {
                            p.DiffCountQty = null;
                            p.CountById = null;
                            p.CountDate = null;
                            UpdateCountDtlState(p);
                        }
                    });
                    // 5、不能有参数完全一致的两条新增盘盈数据
                    NewInventoryStockCountDetailComparerFunc(bill);

                    //更新单据状态
                    UpdateLesCountState(bill);
                }
                RF.Save(bill);
                trans.Complete();
            }
            return bill;
        }

        /// <summary>
        /// 根据明细获取库存
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="labelNo">标签号</param>
        /// <param name="ItemExtPropName">物料扩展属性</param>
        /// <param name="LotCode">批次号</param>
        /// <param name="StorageLocationId">库位</param>
        /// <param name="onhandState">库存状态</param>
        /// <param name="factoryId"></param>
        /// <returns></returns>
        private decimal GetStockCountDetailQty(double itemId, double warehouseId, string labelNo, string ItemExtPropName, string LotCode, double? StorageLocationId, OnhandState? onhandState, double? factoryId)
        {
            decimal NowQty = 0;
            var query = Query<ItemLabel>().Where(p => p.ItemId == itemId && p.WarehouseId == warehouseId);
            if (!labelNo.IsNullOrEmpty())
            {
                query.Where(p => p.Label == labelNo);
            }
            if (!ItemExtPropName.IsNullOrEmpty())
            {
                query.Where(p => p.ItemExtPropName == ItemExtPropName);
            }
            if (StorageLocationId.HasValue)
            {
                query.Where(p => p.StorageLocationId == StorageLocationId);
            }
            if (!LotCode.IsNullOrEmpty())
            {
                query.Where(p => p.Lot == LotCode);
            }
            if (factoryId.HasValue)
            {
                query.Where(p => p.FactoryId == factoryId);
            }
            if (onhandState.HasValue && onhandState == OnhandState.Ok)
            {
                NowQty = query.ToList(null, null).Sum(p => p.Qty);
            }
            if (onhandState.HasValue && onhandState == OnhandState.Ng)
            {
                NowQty = query.ToList(null, null).Sum(p => p.NgQty);
            }
            return NowQty;
        }
        /// <summary>
        /// 标签细度校验是否重复
        /// </summary>
        /// <param name="bill">线边仓盘点单据</param>
        private void LabelStockCountDetailComparerFunc(LesStockCount bill)
        {
            //根据仓库 + 标签号做汇总
            var sameDtlCount = bill.LesStockCountDetailList.GroupBy(p => new LesStockCountDetail
            {
                ItemId = p.ItemId,
                WarehouseId = p.WarehouseId,
                StorageLocationId = null,
                LabelNo = p.LabelNo,
                LotId = null,
                OnhandState = p.OnhandState,
                ItemExtProp = p.ItemExtProp
            }, (m, f) => new { Dtl = m, LineNo = f.Min(p => p.LineNo), Count = f.Count() }, new LesStockCountDetailComparer());

            var sameIsNewCount = bill.LesStockCountDetailList.Where(p => p.IsNewInventory).GroupBy(p => new LesStockCountDetail
            {
                ItemId = p.ItemId,
                WarehouseId = p.WarehouseId,
                StorageLocationId = null,
                LabelNo = p.LabelNo,
                LotId = null,
                OnhandState = p.OnhandState,
                ItemExtProp = p.ItemExtProp
            }, (m, f) => new { Dtl = m, LineNo = f.Min(p => p.LineNo), Count = f.Count() }, new LesStockCountDetailComparer());

            if (sameDtlCount.Count(p => p.Count > 1) - sameIsNewCount.Count(p => p.Count > 1) != 0)
            {
                var LineNo = sameDtlCount.Where(p => p.Count > 1).OrderBy(p => p.LineNo).Select(p => p.LineNo).FirstOrDefault();
                var stockDetail = bill.LesStockCountDetailList.FirstOrDefault(p => p.LineNo == LineNo);
                setCompareErrorMsg(stockDetail);
            }
        }

        /// <summary>
        /// 物料细度校验是否重复
        /// </summary>
        /// <param name="bill">线边仓盘点单据</param>
        private void ItemStockCountDetailComparerFunc(LesStockCount bill)
        {
            //根据仓库+物料编码+物料扩展属性做汇总
            var sameDtlCount = bill.LesStockCountDetailList.GroupBy(p => new LesStockCountDetail
            {
                ItemId = p.ItemId,
                WarehouseId = p.WarehouseId,
                StorageLocationId = null,
                LabelNo = null,
                LotId = null,
                OnhandState = p.OnhandState,
                ItemExtProp = p.ItemExtProp
            }, (m, f) => new { Dtl = m, LineNo = f.Min(p => p.LineNo), Count = f.Count() }, new LesStockCountDetailComparer());

            var sameIsNewCount = bill.LesStockCountDetailList.Where(p => p.IsNewInventory).GroupBy(p => new LesStockCountDetail
            {
                ItemId = p.ItemId,
                WarehouseId = p.WarehouseId,
                StorageLocationId = null,
                LabelNo = null,
                LotId = null,
                OnhandState = p.OnhandState,
                ItemExtProp = p.ItemExtProp
            }, (m, f) => new { Dtl = m, LineNo = f.Min(p => p.LineNo), Count = f.Count() }, new LesStockCountDetailComparer());

            if (sameDtlCount.Count(p => p.Count > 1) - sameIsNewCount.Count(p => p.Count > 1) != 0)
            {
                //var noList = sameDtlCount.Where(p => p.Count > 1).Select(p => p.LineNo).OrderBy(p => p);
                //throw new ValidationException("新增盘盈数据存在物料盘点细度的相同明细行,行号[{0}]".L10nFormat(string.Join(",", noList)));
                var LineNo = sameDtlCount.Where(p => p.Count > 1).OrderBy(p => p.LineNo).Select(p => p.LineNo).FirstOrDefault();
                var stockDetail = bill.LesStockCountDetailList.FirstOrDefault(p => p.LineNo == LineNo);
                setCompareErrorMsg(stockDetail);
            }
        }

        /// <summary>
        /// 更新盘点明细状态
        /// </summary>
        /// <param name="dtl">盘点单明细</param>
        public virtual void UpdateCountDtlState(LesStockCountDetail dtl)
        {
            if (dtl == null)
            {
                return;
            }
            if (dtl.State != LesCountState.Close && !dtl.ActualCountQty.HasValue)
            {
                dtl.State = LesCountState.Audit;
            }
            if (dtl.ActualCountQty.HasValue && dtl.State != LesCountState.Close)
            {
                dtl.State = LesCountState.FinishCount;
            }
        }

        /// <summary>
        /// 新库存盘点校验是否重复
        /// </summary>
        /// <param name="bill">库存盘点单据</param>
        private void NewInventoryStockCountDetailComparerFunc(LesStockCount bill)
        {
            var sameDtlCount = bill.LesStockCountDetailList.Where(p => p.IsNewInventory).GroupBy(p => new LesStockCountDetail
            {
                ItemId = p.ItemId,
                WarehouseId = p.WarehouseId,
                StorageLocationId = p.StorageLocationId,
                LotId = p.LotId,
                OnhandState = p.OnhandState,
                ItemExtProp = p.ItemExtProp,
                LabelNo = p.LabelNo,
            }, (m, f) => new { Dtl = m, LineNo = f.Min(p => p.LineNo), Count = f.Count() }, new LesStockCountDetailComparer());

            if (sameDtlCount.Any(p => p.Count > 1))
            {
                var LineNo = sameDtlCount.Where(p => p.Count > 1).OrderBy(p => p.LineNo).Select(p => p.LineNo).FirstOrDefault();
                var stockDetail = bill.LesStockCountDetailList.FirstOrDefault(p => p.LineNo == LineNo);
                setCompareErrorMsg(stockDetail);
                //var noList = sameDtlCount.Where(p => p.Count > 1).Select(p => p.LineNo).OrderBy(p => p);

            }
        }

        /// <summary>
        /// 新增盘盈重复明细报错
        /// </summary>
        /// <param name="stockDetail"></param>
        /// <exception cref="ValidationException"></exception>
        private void setCompareErrorMsg(LesStockCountDetail stockDetail)
        {
            if (stockDetail == null)
            {
                return;
            }
            var errorMsg = "新增盘盈数据存在相同的明细行,物料:[{0}],库存状态:[{2}]".L10nFormat(stockDetail.Item.Code, stockDetail.ItemExtPropName, stockDetail.OnhandState.ToLabel());
            if (stockDetail.LotId.HasValue)
            {
                errorMsg += ",批次:[{0}]".L10nFormat(stockDetail.Lot.Code);
            }
            if (!stockDetail.ItemExtPropName.IsNullOrEmpty())
            {
                errorMsg += ",扩展属性:[{0}]".L10nFormat(stockDetail.ItemExtPropName);
            }
            if (!stockDetail.LabelNo.IsNullOrEmpty())
            {
                errorMsg += ",标签:[{0}]".L10nFormat(stockDetail.LabelNo);
            }
            if (stockDetail.FactoryId.HasValue)
            {
                errorMsg += ",工厂:[{0}]".L10nFormat(stockDetail.Factory.Code);
            }
            if (stockDetail.StorageLocationId.HasValue)
            {
                errorMsg += ",库位:[{0}]".L10nFormat(stockDetail.StorageLocation.Code);
            }
            throw new ValidationException(errorMsg);
        }
        /// <summary>
        /// 更新盘点单、明细状态
        /// </summary>
        /// <param name="bill">盘点单</param>
        public virtual void UpdateLesCountState(LesStockCount bill)
        {
            if (bill.LesStockCountDetailList.All(p => p.State == LesCountState.Close))
            {
                bill.State = LesCountState.Close;
            }
            else
            {
                if (!bill.LesStockCountDetailList.All(p => p.State == LesCountState.Audit || p.State == LesCountState.Close))
                {
                    if (bill.LesStockCountDetailList.All(p => p.State == LesCountState.FinishCount || p.State == LesCountState.Audit || p.State == LesCountState.Close))
                    {
                        bill.State = LesCountState.PartCount;
                    }
                    if (bill.LesStockCountDetailList.All(p => p.State == LesCountState.FinishCount || p.State == LesCountState.Close))
                    {
                        bill.State = LesCountState.FinishCount;
                    }
                    if (bill.LesStockCountDetailList.All(p => p.State == LesCountState.Finished || p.State == LesCountState.Close))
                    {
                        bill.State = LesCountState.Finished;
                    }
                }
                else
                {
                    bill.State = LesCountState.Audit;
                }
            }
        }

        /// <summary>
        /// 验证盘点明细数据
        /// </summary>
        /// <param name="bill">单据</param>
        /// <param name="range">盘点范围</param>
        private void ValiateCountDtl(LesStockCount bill, LesStockCountRange range)
        {
            if (bill.LesStockCountDetailList.Count > 0)
            {
                switch (range.CountDimension)
                {
                    case CountDimension.Label:
                        LabelStockCountDetailComparerFunc(bill);
                        break;
                    case CountDimension.Lot:
                        LotStockCountDetailComparerFunc(bill);
                        break;
                    case CountDimension.Location:
                        LocationStockCountDetailComparerFunc(bill);
                        break;
                    case CountDimension.Item:
                        ItemStockCountDetailComparerFunc(bill);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 标签细度校验是否重复
        /// </summary>
        /// <param name="bill"></param>
        private void LotStockCountDetailComparerFunc(LesStockCount bill)
        {
            //根据仓库+物料+物料扩展属性+批次做汇总
            var sameDtlCount = bill.LesStockCountDetailList.GroupBy(p => new LesStockCountDetail
            {
                ItemId = p.ItemId,
                WarehouseId = p.WarehouseId,
                StorageLocationId = null,
                LotId = p.LotId,
                OnhandState = p.OnhandState,
                ItemExtProp = p.ItemExtProp,
                LabelNo = string.Empty,
            }, (m, f) => new { Dtl = m, LineNo = f.Max(p => p.LineNo), Count = f.Count() }, new LesStockCountDetailComparer());

            var sameIsNewCount = bill.LesStockCountDetailList.Where(p => p.IsNewInventory).GroupBy(p => new LesStockCountDetail
            {
                ItemId = p.ItemId,
                WarehouseId = p.WarehouseId,
                StorageLocationId = null,
                LotId = p.LotId,
                OnhandState = p.OnhandState,
                ItemExtProp = p.ItemExtProp,
                LabelNo = string.Empty,
            }, (m, f) => new { Dtl = m, LineNo = f.Max(p => p.LineNo), Count = f.Count() }, new LesStockCountDetailComparer());

            if (sameDtlCount.Count(p => p.Count > 1) - sameIsNewCount.Count(p => p.Count > 1) != 0)
            {

                throw new ValidationException("新增盘盈数据存在批次盘点细度的相同明细行".L10N());
            }
        }

        /// <summary>
        /// 获取盘点单状态
        /// </summary>
        /// <param name="id">盘点单ID</param>
        /// <returns>盘点单状态</returns>
        public virtual LesCountState GetLesCountState(double id)
        {
            var query = DB.Query<LesStockCount>().Select(p => p.State).Where(p => p.Id == id).ToList<decimal>().FirstOrDefault();
            return (LesCountState)query;
        }

        #region 审核
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="stockCountId">盘点单ID</param>
        public virtual LesStockCount AuditLesStockCount(double stockCountId)
        {
            var bill = RF.GetById<LesStockCount>(stockCountId, new EagerLoadOptions().LoadWithViewProperty());
            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                AuditSotckCount(bill);
                trans.Complete();
            }
            return bill;
        }

        /// <summary>
        /// 审批库存盘点单
        /// </summary>
        /// <param name="bill">单据</param>
        private void AuditSotckCount(LesStockCount bill)
        {
            //验证数据
            ValidateData(bill);

            var range = bill.LesStockCountRangeList.FirstOrDefault();

            //创建盘点明细
            var dtlList = CreateStockCountDtl(bill, range);
            bill.LesStockCountDetailList.AddRange(dtlList);

            //更新单据、单据明细状态
            bill.State = LesCountState.Audit;
            bill.AuditById = RT.IdentityId;
            bill.AuditDate = DateTime.Now;
            int lineNo = 1;
            bill.LesStockCountDetailList.ForEach(p =>
            {
                p.LineNo = lineNo.ToString();
                p.State = LesCountState.Audit;
                lineNo++;
            });
            RF.Save(bill);
        }

        /// <summary>
        /// 根据盘点范围创建盘点明细
        /// </summary>
        /// <param name="range">盘点范围</param>
        /// <param name="bill">单据</param>
        /// <returns>盘点明细</returns>
        private List<LesStockCountDetail> CreateStockCountDtl(LesStockCount bill, LesStockCountRange range)
        {
            var dtlList = new List<LesStockCountDetail>();

            //获取盘点范围内的库存记录
            var ItemLabelonhands = GetItemLabelOnhands(range);
            if (ItemLabelonhands.Count > 0)
            {
                //根据库存记录分组创建盘点明细
                dtlList = GroupByOnhands(range, ItemLabelonhands, bill);
            }

            if (dtlList.Count == 0)
            {
                throw new ValidationException("当前盘点范围没有可盘点的库存明细!".L10N());
            }
            return dtlList.OrderBy(p => p.WarehouseId).ThenBy(p => p.StorageLocationId)
                .ThenBy(p => p.ItemId).ThenBy(p => p.LotId).ToList();
        }

        /// <summary>
        /// 获取盘点范围内库存记录
        /// </summary>
        /// <param name="range">盘点范围</param>
        /// <returns>库存记录</returns>
        private EntityList<ItemLabel> GetItemLabelOnhands(LesStockCountRange range)
        {
            var query = Query<ItemLabel>().Where(p => p.Qty > 0 || p.NgQty > 0);
            if (!range.Warehouses.IsNullOrEmpty())
            {
                var whCodeList = range.Warehouses.Split(';').ToList();
                query.Where(p => whCodeList.Contains(p.Warehouse.Code));
            }
            if (!range.Items.IsNullOrEmpty())
            {
                var itemCodeList = range.Items.Split(';').ToList();
                query.Join<Item>("a1", (x, y) => x.ItemId == y.Id && itemCodeList.Contains(y.Code));
            }
            if (!range.ItemCategorys.IsNullOrEmpty())
            {
                var list = range.ItemCategorys.Split(';').ToList();
                var idList = RT.Service.Resolve<ItemController>().GetItemCateNodesByPtreeCode(list);
                query.Exists<ItemCategoryRelation>((x, y) => y.Where(f => f.ItemId == x.ItemId && f.Type == CategoryType.Item && idList.Contains(f.ItemCategory.Id)));
            }
            if (range.ConsumeMode.HasValue)
            {
                if (!range.Items.IsNullOrEmpty())
                {
                    query.Where<Item>((d, a) => d.ItemId == a.Id && a.ConsumeMode == range.ConsumeMode);
                }
                else
                {
                    query.Join<Item>("b1", (d, a) => d.ItemId == a.Id && a.ConsumeMode == range.ConsumeMode);
                }
            }
            //if (range.CountDimension == CountDimension.Label || range.CountDimension == CountDimension.Location)
            //{
            //    if (!range.Items.IsNullOrEmpty())
            //    {
            //        query.Where<Item>((x, y) => x.ItemId == y.Id && y.IsLabel == true);
            //    }
            //    else
            //    {
            //        query.Join<Item>((x, y) => x.ItemId == y.Id && y.IsLabel == true);
            //    }
            //}
            return query.OrderBy(p => p.WarehouseId).OrderBy(p => p.ItemId).ToList();
        }

        /// <summary>
        /// 根据库存记录分组创建盘点明细
        /// </summary>
        /// <param name="range">盘点范围</param>
        /// <param name="itemLabelList">物料标签库存记录</param>
        /// <param name="lesStockCount">线边仓盘点单</param>
        /// <returns>盘点明细</returns>
        private List<LesStockCountDetail> GroupByOnhands(LesStockCountRange range, EntityList<ItemLabel> itemLabelList, LesStockCount lesStockCount)
        {
            List<LesStockCountDetail> countDtlList = new List<LesStockCountDetail>();
            List<StockCountOnHandDatas> stockCountOnHandDatas;
            var tempOnhandList = itemLabelList.ToList();
            var LotNos = tempOnhandList.Select(p => p.Lot).Distinct().ToList();
            var Lots = RT.Service.Resolve<LotController>().GetLots(LotNos);
            switch (range.CountDimension)
            {
                case CountDimension.Location:
                    //根据仓库+标签号+库位做汇总(郭锐说的，不合并工厂)
                    stockCountOnHandDatas = tempOnhandList.GroupBy(p => new { p.WarehouseId, p.Label, p.StorageLocationId, p.ItemId, p.ItemExtProp, p.ItemExtPropName, p.Lot, p.FactoryId }).Select(t => new StockCountOnHandDatas
                    {
                        LabelNo = t.Key.Label,
                        FactoryId = t.Key.FactoryId,
                        WarehouseId = t.Key.WarehouseId,
                        StorageLocationId = t.Key.StorageLocationId,
                        ItemId = t.Key.ItemId,
                        Qty = t.Sum(s => s.Qty),
                        NgQty = t.Sum(s => s.NgQty),
                        LotCode = t.Key.Lot,
                        ItemExtProp = t.Key.ItemExtProp,
                        ItemExtPropName = t.Key.ItemExtPropName,
                    }).ToList();
                    break;
                case CountDimension.Lot:
                    //批次：根据仓库+物料+物料扩展属性+批次做汇总  盘点细度为批次的不写标签号
                    stockCountOnHandDatas = tempOnhandList.GroupBy(p => new { p.WarehouseId, p.ItemId, p.Lot, p.ItemExtProp, p.ItemExtPropName }).Select(t => new StockCountOnHandDatas
                    {
                        LabelNo = String.Empty,
                        WarehouseId = t.Key.WarehouseId,
                        //FactoryId = t.Key.FactoryId,
                        StorageLocationId = 0,
                        ItemId = t.Key.ItemId,
                        Qty = t.Sum(s => s.Qty),
                        NgQty = t.Sum(s => s.NgQty),
                        LotCode = t.Key.Lot,
                        ItemExtProp = t.Key.ItemExtProp,
                        ItemExtPropName = t.Key.ItemExtPropName,
                    }).ToList();
                    break;
                case CountDimension.Label:
                    //标签：根据仓库+标签号做汇总
                    stockCountOnHandDatas = tempOnhandList.GroupBy(p => new { p.WarehouseId, p.Label, p.ItemId, p.ItemExtProp, p.ItemExtPropName, p.Lot }).Select(t => new StockCountOnHandDatas
                    {
                        LabelNo = t.Key.Label,
                        WarehouseId = t.Key.WarehouseId,
                        //FactoryId = t.Key.FactoryId,
                        StorageLocationId = 0,
                        ItemId = t.Key.ItemId,
                        Qty = t.Sum(s => s.Qty),
                        NgQty = t.Sum(s => s.NgQty),
                        LotCode = t.Key.Lot,
                        ItemExtProp = t.Key.ItemExtProp,
                        ItemExtPropName = t.Key.ItemExtPropName,
                    }).ToList();
                    break;
                case CountDimension.Item:
                    //根据物料编码+物料扩展属性做汇总  盘点细度为物料的不写标签号和批次号
                    stockCountOnHandDatas = tempOnhandList.GroupBy(p => new { p.ItemId, p.ItemExtProp, p.ItemExtPropName, p.WarehouseId }).Select(t => new StockCountOnHandDatas
                    {

                        LabelNo = String.Empty,
                        WarehouseId = t.Key.WarehouseId,
                        //FactoryId = t.Key.FactoryId,
                        StorageLocationId = 0,
                        ItemId = t.Key.ItemId,
                        Qty = t.Sum(s => s.Qty),
                        NgQty = t.Sum(s => s.NgQty),
                        LotCode = string.Empty,
                        ItemExtProp = t.Key.ItemExtProp,
                        ItemExtPropName = t.Key.ItemExtPropName,
                    }).ToList();
                    break;
                default:
                    stockCountOnHandDatas = tempOnhandList.GroupBy(p => new { p.WarehouseId, p.ItemId, p.Lot, p.ItemExtProp, p.ItemExtPropName }).Select(t => new StockCountOnHandDatas
                    {
                        LabelNo = String.Empty,
                        WarehouseId = t.Key.WarehouseId,
                        //FactoryId = t.Key.FactoryId,
                        StorageLocationId = 0,
                        ItemId = t.Key.ItemId,
                        Qty = t.Sum(s => s.Qty),
                        NgQty = t.Sum(s => s.NgQty),
                        LotCode = t.Key.Lot,
                        ItemExtProp = t.Key.ItemExtProp,
                        ItemExtPropName = t.Key.ItemExtPropName,
                    }).ToList();
                    break;
            }
            GetStockCountDetailByCount(countDtlList, stockCountOnHandDatas, range, Lots, lesStockCount);
            return countDtlList;
        }

        /// <summary>
        /// 根据分组信息创建盘点单明细
        /// </summary>
        /// <param name="countDtlList">盘点单明细</param>
        /// <param name="stockCountOnHandDatas">分组信息</param>
        /// <param name="range">盘点范围</param>
        /// <param name="lots">批次信息</param>
        /// <param name="lesStockCount">盘点单</param>
        /// <returns></returns>
        public virtual void GetStockCountDetailByCount(List<LesStockCountDetail> countDtlList, List<StockCountOnHandDatas> stockCountOnHandDatas, LesStockCountRange range, EntityList<Lot> lots, LesStockCount lesStockCount)
        {

            if (stockCountOnHandDatas.Count == 0)
            {
                return;
            }
            foreach (var item in stockCountOnHandDatas)
            {
                if (item.Qty == 0 && item.NgQty == 0)
                {
                    continue;
                }
                double lotId = 0;
                if (!item.LotCode.IsNullOrEmpty())
                {
                    var lotItem = lots.FirstOrDefault(f => f.Code == item.LotCode && f.ItemId == item.ItemId && f.ItemExtProp == item.ItemExtProp);
                    if (lotItem != null)
                    {
                        lotId = lotItem.Id;
                    }

                }
                if (item.Qty > 0)
                {
                    //合格的盘点明细
                    var normalHands = new LesStockCountDetail()
                    {
                        ItemId = item.ItemId,
                        FactoryId = item.FactoryId,
                        WarehouseId = item.WarehouseId.Value,
                        ItemExtProp = item.ItemExtProp,
                        ItemExtPropName = item.ItemExtPropName,
                        StorageLocationId = item.StorageLocationId,
                        LabelNo = item.LabelNo,
                        OnhandState = OnhandState.Ok,
                        LotId = lotId,
                        CountDimension = range.CountDimension,
                    };
                    if (range.IsDynamicOnhand)
                    {
                        normalHands.Qty = null;
                    }
                    else
                    {
                        normalHands.Qty = item.Qty;
                    }
                    normalHands.GenerateId();
                    normalHands.LesStockCountId = lesStockCount.Id;
                    countDtlList.Add(normalHands);

                }
                if (item.NgQty > 0)
                {
                    //不合格的盘点明细
                    var ngHands = new LesStockCountDetail()
                    {
                        ItemId = item.ItemId,
                        FactoryId = item.FactoryId,
                        WarehouseId = item.WarehouseId.Value,
                        ItemExtProp = item.ItemExtProp,
                        ItemExtPropName = item.ItemExtPropName,
                        StorageLocationId = item.StorageLocationId,
                        LabelNo = item.LabelNo,
                        OnhandState = OnhandState.Ng,
                        LotId = lotId,
                        CountDimension = range.CountDimension,
                    };
                    if (range.IsDynamicOnhand)
                    {
                        ngHands.Qty = null;
                    }
                    else
                    {
                        ngHands.Qty = item.NgQty;
                    }
                    ngHands.GenerateId();
                    ngHands.LesStockCountId = lesStockCount.Id;
                    countDtlList.Add(ngHands);
                }
            }
        }
        /// <summary>
        /// 验证盘点单
        /// </summary>
        /// <param name="bill">盘点单</param>
        private void ValidateData(LesStockCount bill)
        {
            if (bill == null)
            {
                throw new ValidationException("单据不存在".L10N());
            }
            if (bill.State != LesCountState.Create)
            {
                throw new ValidationException("单据不是创建状态，无法审核".L10N());
            }
            if (bill.State == LesCountState.Close || bill.State == LesCountState.Finished)
            {
                throw new ValidationException("单据状态为关闭或者已完工，无法审核".L10N());
            }
            if (bill.LesStockCountRangeList.Count == 0)
            {
                throw new ValidationException("单据盘点范围未维护".L10N());
            }
        }
        #endregion

        #region 完工
        /// <summary>
        /// 完工盘点单(差异数==0)
        /// </summary>
        /// <param name="stockCountId">盘点单Id</param>
        public virtual bool? FinishedStockCount(double stockCountId)
        {
            var bill = RF.GetById<LesStockCount>(stockCountId);
            var dtlList = bill.LesStockCountDetailList;
            if (!dtlList.Any(p => p.State == LesCountState.FinishCount))
            {
                throw new ValidationException("不存在状态为已盘点的明细行".L10N());
            }
            bool? isDiff = null;

            if (dtlList.Where(p => p.State == LesCountState.FinishCount).All(p => p.LesStockCountDetailResult == LesStockCountDetailResult.Normal))
            {
                bill.LesStockCountResult = LesStockCountResult.Normal;
                bill.State = LesCountState.Finished;
                dtlList.ForEach(p => p.State = LesCountState.Finished);
                RF.Save(bill);
            }
            else
            {
                isDiff = bill.LesStockCountRangeList.Any(a => a.CountDimension == CountDimension.Location) && dtlList.Any(a => a.IsAllowAdjust == true);
            }
            return isDiff;
        }

        /// <summary>
        /// 完工盘点单(盘点明细的差异数!=0)
        /// </summary>
        /// <param name="LesstockCountId">盘点单Id</param>
        /// <param name="finishType">差异处理方式</param>        
        public virtual List<DiffAdjustViewModel> FinishedLesStockCount(double LesstockCountId, LesStockCountResult finishType)
        {
            List<DiffAdjustViewModel> rst = new List<DiffAdjustViewModel>();
            var bill = RF.GetById<LesStockCount>(LesstockCountId);
            var countDtlList = bill.LesStockCountDetailList;
            if (!countDtlList.Any(p => p.State == LesCountState.FinishCount))
            {
                throw new ValidationException("不存在状态为已盘点的明细行".L10N());
            }
            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                //已盘点并且存在差异的明细              
                var dtlList = countDtlList.Where(p => p.State == LesCountState.FinishCount && p.LesStockCountDetailResult == LesStockCountDetailResult.Abnormal).ToList();
                if (dtlList.Any(p => p.AnalysisResult == null))
                    throw new ValidationException("盘点结果为异常的盘点明细分析结果不能为空!".L10N());

                //差异复核
                if (finishType == LesStockCountResult.DiffReview)
                {
                    bill.LesStockCountResult = LesStockCountResult.DiffReview;
                    CreateDiffReviewCount(bill, dtlList);
                }

                //差异调账
                if (finishType == LesStockCountResult.DiffAdjust)
                {
                    rst = HandleDiffAdjust(bill, dtlList);

                }
                if (finishType == LesStockCountResult.NotDeal)
                {
                    bill.LesStockCountResult = LesStockCountResult.NotDeal;
                }
                if (rst == null || rst.Count == 0)
                {
                    //更新单据状态
                    countDtlList.Where(p => p.State == LesCountState.FinishCount).ForEach(p =>
                    {
                        p.State = LesCountState.Finished;
                    });

                    UpdateLesCountState(bill);
                    RF.Save(bill);
                }
                trans.Complete();
                return rst;
            }
        }

        /// <summary>
        /// 处理差异调账逻辑
        /// </summary>
        /// <param name="bill">单据</param>
        /// <param name="dtlList">明细</param>       
        public virtual List<DiffAdjustViewModel> HandleDiffAdjust(LesStockCount bill, List<LesStockCountDetail> dtlList)
        {
            List<DiffAdjustViewModel> rst = new List<DiffAdjustViewModel>();
            if (dtlList.Any(f => f.LabelNo.IsNullOrEmpty()))
                throw new ValidationException("明细的标签不能为空".L10N());

            ValidateNewLabels(dtlList);

            List<double> hasHandle = new List<double>();
            var labelNos = dtlList.Select(a => a.LabelNo).ToList();
            List<double?> locIds = new List<double?>();
            dtlList.Select(a => a.StorageLocationId).Distinct().ForEach(a =>
            {
                locIds.Add(a);
            });
            var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelDatas(labelNos, locIds);

            //需求暂时还没想到要如何实现
            if (dtlList.Any(f => f.IsAllowAdjust == true))
            {
                var itemIds = dtlList.Where(f => f.IsAllowAdjust == true).Select(f => f.ItemId).ToList();
                var items = RT.Service.Resolve<ItemController>().GetItemList(itemIds);
                var pushItemId = items.Where(f => f.ConsumeMode == ConsumeMode.Push).Select(f => f.Id).ToList();
                var needToDtls = dtlList.Where(f => f.IsAllowAdjust == true && pushItemId.Contains(f.ItemId));

                if (needToDtls.Any())
                {
                    needToDtls.GroupBy(p => new { p.LabelNo, p.WarehouseId, p.ItemId, p.ItemExtProp, p.ItemExtPropName, p.StorageLocationId, p.LotId }).ForEach(p =>
                     {
                         if (p.Count() > 1)
                             throw new ValidationException("存在盘点明细一样的数据".L10N());
                         var dtl = p.First();
                         var label = labels.FirstOrDefault(a => a.Label == dtl.LabelNo && a.StorageLocationId == dtl.StorageLocationId);
                         if (label == null && !dtl.IsNewInventory)
                         {
                             throw new ValidationException("物料标签表盘亏数量大于标签数量[标签{0}库位{1}]".L10nFormat(dtl.LabelNo, dtl.StorageLocation.Code));
                         }

                         var onhands = RT.Service.Resolve<InvOnhandController>().GetLotLpnOnhands(p.Key.WarehouseId, null, p.Key.StorageLocationId, p.Key.ItemId, p.Key.LotId
                              , "", "", "", "", false, OnhandState.Ok, p.Key.ItemExtProp);
                         if (onhands.Any())
                         {//包含合格的推式物料
                             hasHandle.AddRange(p.Select(f => f.Id));
                             var onhand = onhands.First();
                             DiffAdjustViewModel diffAdjustViewModel = new DiffAdjustViewModel()
                             {
                                 StorageLocationId = p.Key.StorageLocationId.Value,
                                 LabelNo = dtl.LabelNo,
                                 AvaiableQty = label.Qty,
                                 DtlId = dtl.Id,
                                 ItemCode = onhand.ItemCode,
                                 ItemExtPropName = p.Key.ItemExtPropName,
                                 ItemId = p.Key.ItemId,
                                 ItemName = onhand.ItemName,
                                 Factory = dtl.Factory?.Name,
                                 Qty = dtl.DiffCountQty.Value,
                                 StorageLocation = onhand.StorageLocationCode,
                                 WarehouseId = dtl.WarehouseId,
                                 WarehouseName = onhand.WarehouseCode,
                             };
                             rst.Add(diffAdjustViewModel);
                         }
                     });
                }
            }
            if (!hasHandle.Any())
            {//没有包含合格的推式物料
                using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
                {
                    bill.LesStockCountResult = LesStockCountResult.DiffAdjust;
                    UpdateOnhand(bill, dtlList, labels);
                    trans.Complete();
                }
            }

            return rst;
        }

        /// <summary>
        /// 验证新增盘盈的数据
        /// </summary>      
        private void ValidateNewLabels(List<LesStockCountDetail> dtlList)
        {
            var itemIds = dtlList.Where(f => f.IsNewInventory).Select(f => f.ItemId).Distinct().ToList();
            if (itemIds.Any())
            {
                var itemStocks = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds);
                var serItems = itemStocks.Where(f => f.IsSerialNumber == true).Select(f => f.ItemId).ToList();
                var serLabels = dtlList.Where(f => f.IsNewInventory && serItems.Contains(f.ItemId)).Select(f => f.LabelNo).ToList();
                serLabels.ForEach(f =>
                {
                    if (dtlList.Count(a => a.LabelNo == f) > 1)
                        throw new ValidationException("明细序列号标签重复[{0}]".L10nFormat(f));
                });
                var packLabelNos = RT.Service.Resolve<IPackingLabelSn>().GetPackingLabelSnDatas(serLabels);

                dtlList.Where(f => f.IsNewInventory && serItems.Contains(f.ItemId)).GroupBy(p => p.StorageLocationId).ForEach(f =>
                {
                    f.ForEach(a =>
                    {
                        if (packLabelNos.Any(b => b.Sn == a.LabelNo && b.StorageLocationId != f.Key && b.SnState != 30))
                        {
                            throw new ValidationException("标签{0}在序列状态表状态不是出库".L10nFormat(a.LabelNo));
                        }
                        if (packLabelNos.Any(b => b.Sn == a.LabelNo && (b.ItemId != a.ItemId || b.ItemExtProp != a.ItemExtProp)))
                        {
                            throw new ValidationException("标签{0}在序列状态表状态已存在但不是当前物料、扩展属性".L10nFormat(a.LabelNo));
                        }
                    });
                });
            }
        }

        /// <summary>
        /// 更新库存
        /// </summary>       
        private void UpdateOnhand(LesStockCount bill, List<LesStockCountDetail> adjustDtl, EntityList<ItemLabel> labels)
        {
            List<InvCollectData> datas = new List<InvCollectData>();
            var employeeCode = RT.Service.Resolve<EmployeeController>().GetEmployeeCodeById(RT.IdentityId);
            var itemIds = adjustDtl.Select(f => f.ItemId).ToList();
            var itemStocksDic = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds).ToDictionary(p => p.ItemId, p => p.IsSerialNumber == true);
            adjustDtl.Where(f => f.OnhandState == OnhandState.Ok).GroupBy(p => new { p.ItemId, p.LotId, p.ItemExtProp, p.ItemExtPropName, p.StorageLocationId }).ForEach(f =>
               {
                   f.ForEach(p =>
                   {
                       itemStocksDic.TryGetValue(p.ItemId, out bool isSer);
                       var label = labels.FirstOrDefault(a => a.Label == p.LabelNo && a.StorageLocationId == p.StorageLocationId);
                       if (label != null)
                       {
                           label.Qty += p.DiffCountQty.Value;
                           if (label.Lot == "")
                               label.Lot = "LotDefault";
                           if (label.Qty < 0)
                               throw new ValidationException("物料标签表盘亏数量大于标签数量[标签{0}库位{1}]".L10nFormat(p.LabelNo, p.StorageLocation.Code));
                       }
                       else if (p.IsNewInventory)
                       {
                           ItemLabel itemLabel = new ItemLabel()
                           {
                               FactoryId = p.FactoryId,
                               ItemId = p.ItemId,
                               ItemExtProp = p.ItemExtProp,
                               ItemExtPropName = p.ItemExtPropName,
                               Label = p.LabelNo,
                               Lot = p.Lot.Code,
                               IsSerialNumber = isSer,
                               StorageLocationId = p.StorageLocationId,
                               WarehouseId = p.WarehouseId,
                               Qty = p.DiffCountQty.Value,
                               InitialQty = p.DiffCountQty.Value,
                               SourceType = LabelSource.Count,
                           };
                           RF.Save(itemLabel);
                           labels.Add(itemLabel);
                       }
                       else
                       {
                           throw new ValidationException("没找到物料标签表数据[标签{0}库位{1}]".L10nFormat(p.LabelNo, p.StorageLocation.Code));
                       }
                   });
                   if (f.Sum(a => a.DiffCountQty.Value) != 0)
                   {//同一个库存物料没有表更数量则不需要调整
                       f.ForEach(p =>
                       {
                           AddInvCollectData(bill, p, datas, employeeCode);
                       });
                   }
               });
            adjustDtl.Where(f => f.OnhandState == OnhandState.Ng).GroupBy(p => new { p.ItemId, p.LotId, p.ItemExtProp, p.ItemExtPropName, p.StorageLocationId }).ForEach(f =>
            {
                f.ForEach(p =>
                {
                    var label = labels.FirstOrDefault(a => a.Label == p.LabelNo && a.StorageLocationId == p.StorageLocationId);
                    if (label != null)
                    {
                        label.NgQty += p.DiffCountQty.Value;
                        if (label.NgQty < 0)
                            throw new ValidationException("物料标签表盘亏数量大于标签不良数量[标签{0}库位{1}]".L10nFormat(p.LabelNo, p.StorageLocation.Code));
                    }
                    else
                    {
                        throw new ValidationException("没找到物料标签表数据[标签{0}库位{1}]".L10nFormat(p.LabelNo, p.StorageLocation.Code));
                    }
                });
                if (f.Sum(a => a.DiffCountQty.Value) != 0)
                {//同一个库存物料没有表更数量则不需要调整
                    f.ForEach(p =>
                    {
                        AddInvCollectData(bill, p, datas, employeeCode);
                    });
                }
            });
            AddNewLabels(adjustDtl);
            ////执行库存操作
            if (datas.Count > 0)
            {
                RT.Service.Resolve<InvTransactionController>().InvTransaction(datas);
            }
            RF.Save(labels);
        }

        /// <summary>
        /// 添加新标签
        /// </summary>        
        private void AddNewLabels(List<LesStockCountDetail> adjustDtl)
        {
            List<PackingLabelSnData> snDatas = new List<PackingLabelSnData>();
            var itemIds = adjustDtl.Select(f => f.ItemId).Distinct().ToList();
            var itemStocks = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds);
            var serItems = itemStocks.Where(f => f.IsSerialNumber == true).Select(f => f.ItemId).ToList();
            adjustDtl.ForEach(f =>
            {
                PackingLabelSnData item = new PackingLabelSnData()
                {
                    ItemExtProp = f.ItemExtProp,
                    ItemExtPropName = f.ItemExtPropName,
                    ItemId = f.ItemId,
                    LotId = f.LotId,
                    OnhandState = (int)f.OnhandState,
                    Sn = f.LabelNo,
                    SnState = 20,
                    StorageLocationId = f.StorageLocationId,
                    WarehouseId = f.WarehouseId,
                    FactoryId = f.FactoryId,
                    Qty = f.ActualCountQty.Value,
                    IsSer = serItems.Contains(f.ItemId),
                };
                snDatas.Add(item);
            });
            RT.Service.Resolve<IPackingLabelSn>().CreatePackingLabelSn(snDatas);
        }

        /// <summary>
        /// 保存差异调账数据
        /// </summary>
        /// <param name="billId">差异调账</param>
        /// <param name="adjustWorkOrderViewModels">投入工单</param>
        public virtual void SaveDiffAdjust(double billId, List<AdjustWorkOrderViewModel> adjustWorkOrderViewModels)
        {
            var dtlIds = adjustWorkOrderViewModels.Select(f => f.DtlId).Distinct().ToList();
            var bill = RF.GetById<LesStockCount>(billId);
            var dtls = bill.LesStockCountDetailList.Where(f => f.State == LesCountState.FinishCount && f.LesStockCountDetailResult == LesStockCountDetailResult.Abnormal);
            ValidateNewLabels(dtls.ToList());
            var labelNos = dtls.Select(a => a.LabelNo).ToList();
            List<double?> locIds = new List<double?>();
            dtls.Where(f => f.IsAllowAdjust == true && f.LabelNo.IsNotEmpty()).Select(a => a.StorageLocationId).Distinct().ForEach(a =>
            {
                locIds.Add(a);
            });
            var labels = RT.Service.Resolve<ItemLabelController>().GetItemLabelDatas(labelNos, locIds);
            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                UpdateOnhand(bill, dtls.ToList(), labels);
                var labelWos = RT.Service.Resolve<ItemLabelController>().GetItemLabelWorkOrders(labels.Select(f => f.Id).ToList());
                dtls.Where(f => dtlIds.Contains(f.Id)).ForEach(f =>
                {
                    var label = labels.FirstOrDefault(a => a.Label == f.LabelNo && a.StorageLocationId == f.StorageLocationId);
                    if (label == null)
                        throw new ValidationException("物料标签找不到标签[{0}]，库位{1}的数据".L10nFormat(f.LabelNo, f.StorageLocation.Code));
                    adjustWorkOrderViewModels.Where(a => a.DtlId == f.Id).ForEach(a =>
                        {
                            var wo = labelWos.FirstOrDefault(b => b.ItemLabelId == label.Id && b.WorkOrderId == a.WorkOrderId);
                            SetWoRecord(f, a, wo);
                            if (wo != null)
                            {
                                wo.Qty += a.DiffQty;
                                if (wo.Qty < 0)
                                    throw new ValidationException("标签XXX盘亏数量大于投入工单数量，请处理后再执行完工".L10N());
                                if (wo.Qty == 0)
                                    wo.PersistenceStatus = PersistenceStatus.Deleted;
                            }
                            else
                            {
                                wo = new ItemLabelWorkOrder()
                                {
                                    WorkOrderId = a.WorkOrderId,
                                    ItemLabelId = label.Id,
                                    Qty = a.DiffQty,
                                };
                            }
                            RF.Save(wo);

                        });
                });
                bill.LesStockCountDetailList.Where(p => p.State == LesCountState.FinishCount).ForEach(p =>
                {
                    p.State = LesCountState.Finished;
                });
                UpdateLesCountState(bill);
                bill.LesStockCountResult = LesStockCountResult.DiffAdjust;
                RF.Save(bill);
                trans.Complete();
            }
        }

        /// <summary>
        /// 生成调账记录
        /// </summary>
        /// <param name="dtl">明细</param>
        /// <param name="a">工单</param>
        /// <param name="wo">工单</param>
        private void SetWoRecord(LesStockCountDetail dtl, AdjustWorkOrderViewModel a, ItemLabelWorkOrder wo)
        {
            LesStockCountWorkOrder lesStockCountWork = new LesStockCountWorkOrder()
            {
                LesStockCountId = dtl.LesStockCountId,
                LineNo = dtl.LineNo,
                WorkOrderNo = a.WorkOrder.No,
                Qty = wo == null ? 0 : wo.Qty,
                DiffCountQty = a.DiffQty,
            };
            lesStockCountWork.ActualCountQty = lesStockCountWork.Qty + lesStockCountWork.DiffCountQty;
            RF.Save(lesStockCountWork);
        }

        #region 更新库存
        /// <summary>
        /// 更新库存操作
        /// </summary>
        /// <param name="invAdjust">库存调整对象</param>
        /// <param name="invdtl">库存调整明细</param>
        /// <param name="datas">事务数据</param>
        /// <param name="empCode">员工号</param>
        /// <returns>事务数据</returns>
        private void AddInvCollectData(LesStockCount invAdjust, LesStockCountDetail invdtl, List<InvCollectData> datas, string empCode = null)
        {
            InvCollectData data = new InvCollectData();
            StockTrans stockTrans = new StockTrans();
            if (invdtl.DiffCountQty.Value < 0)  ////负调整,更新库存分配数(减去分配数)
            {
                stockTrans.FromLpn = "*";
                stockTrans.FromLocationId = invdtl.StorageLocationId.Value;
                stockTrans.FromOnhandState = invdtl.OnhandState;
                data.isFromAllottedQty = false;
                data.isToAllottedQty = false;
            }
            else                            ////正调整,更新库存可用数(增加可用数)
            {
                stockTrans.ToLpn = "*";
                stockTrans.ToLocationId = invdtl.StorageLocationId.Value;
                stockTrans.ToOnhandState = invdtl.OnhandState;
                data.isFromAllottedQty = false;
                data.isToAllottedQty = false;
            }

            stockTrans.Qty = Math.Abs(invdtl.DiffCountQty.Value);

            //获取交易数据
            var baseTransactionData = GetTransData(invAdjust, invdtl, empCode);

            data.baseTransactionData = baseTransactionData;
            data.item = invdtl.Item;
            if (invdtl.Lot != null)
                data.lot = invdtl.Lot;
            else
                data.lot = RT.Service.Resolve<LotController>().GetLotDefault();
            data.stockTrans = stockTrans;
            data.isValidateState = false;
            if (invdtl.ItemExtProp.IsNotEmpty())
            {
                data.ItemExtProp = invdtl.ItemExtProp;
                data.ItemExtPropName = invdtl.ItemExtPropName;
            }
            datas.Add(data);
        }

        /// <summary>
        /// 获取交易相关数据
        /// </summary>
        /// <param name="invAdjust">库存盘点对象</param>
        /// <param name="invdtl">库存盘点明细</param>
        /// <param name="empCode">员工号</param>
        /// <returns>交易相关数据</returns>
        public virtual BaseTransactionData GetTransData(LesStockCount invAdjust, LesStockCountDetail invdtl, string empCode)
        {
            var trantion = RT.Service.Resolve<TransactionController>().GetDefaultTransactions(invAdjust.OrderType);
            BaseTransactionData baseTransactionData = new BaseTransactionData()
            {
                StorerCode = "*",
                BillNo = invAdjust.No,
                BillId = invAdjust.Id,
                BillDetailNo = invdtl.LineNo,
                BillDetailId = invdtl.Id,
                ProjectNo = "*",
                TaskNo = "*",
                OrderType = invAdjust.OrderType,
                TransactionId = trantion.Id,
                TransactionType = TransactionType.adjust,
                EmployeeNo = empCode,
                TaskLevel = Inventory.Task.TaskLevel.Middle
            };

            return baseTransactionData;
        }
        #endregion

        /// <summary>
        /// 创建差异复核
        /// </summary>
        /// <param name="bill">盘点单</param>
        /// <param name="dtlList">已盘点明细</param>
        private void CreateDiffReviewCount(LesStockCount bill, List<LesStockCountDetail> dtlList)
        {
            var diffBill = new LesStockCount();
            diffBill.No = GetCountNo();
            diffBill.OrderType = OrderType.DifferenceCount;
            diffBill.State = LesCountState.Audit;
            diffBill.SourceBillNo = bill.No;
            diffBill.AuditById = RT.IdentityId;
            diffBill.AuditDate = DateTime.Now;
            diffBill.SourceType = SourceType.STORCKCOUNT;
            var range = new LesStockCountRange();
            range.Clone(bill.LesStockCountRangeList.FirstOrDefault(), new CloneOptions(CloneActions.NormalProperties));
            range.CountDimension = CountDimension.Location;
            diffBill.LesStockCountRangeList.Add(range);

            //分析所有已盘点的明细行
            HandleStockDetailData(diffBill, dtlList);

            if (diffBill.LesStockCountDetailList.Count > 0)
            {
                int lineNo = 1;
                foreach (var dtl in diffBill.LesStockCountDetailList)
                {
                    dtl.LineNo = lineNo.ToString();
                    dtl.State = LesCountState.Audit;
                    lineNo++;
                }
                UpdateLesCountState(diffBill);
                RF.Save(diffBill);
            }
            else
            {
                throw new ValidationException("盘点范围内没有可生成的明细".L10N());
            }
        }

        /// <summary>
        /// 分析所有已盘点的明细行
        /// </summary>
        /// <param name="diffBill">调整单据</param>
        /// <param name="dtlList">盘点明细行</param>
        private void HandleStockDetailData(LesStockCount diffBill, List<LesStockCountDetail> dtlList)
        {
            var range = diffBill.LesStockCountRangeList.FirstOrDefault();

            var itemIds = diffBill.LesStockCountDetailList.Select(x => x.ItemId).Distinct().ToList();
            var itemStocks = RT.Service.Resolve<ItemStockBaseController>().GetItemStockDataBases(itemIds, new EagerLoadOptions().LoadWithViewProperty());
            var serItems = itemStocks.Where(f => f.IsSerialNumber == true).Select(a => a.ItemId).ToList();
            foreach (var dtl in dtlList)
            {
                if (dtl.LesStockCountDetailResult.HasValue && dtl.LesStockCountDetailResult == LesStockCountDetailResult.Abnormal && !dtl.IsNewInventory)
                {
                    var ItemList = GetItemLabelByDiffReview(dtl.WarehouseId, dtl.StorageLocationId, dtl.LotCode, dtl.ItemExtProp, dtl.OnhandState, dtl.LabelNo, dtl.ItemId, serItems);
                    if (ItemList.Count > 0)
                    {
                        //根据库存记录分组创建盘点明细
                        var stockDetail = GroupByOnhands(range, ItemList, diffBill);
                        diffBill.LesStockCountDetailList.AddRange(stockDetail);
                    }
                    else
                    {
                        throw new ValidationException("盘点范围内没有可生成的明细或需复核物料不是标签管理物料无法生成标签+库位细度的盘点明细".L10N());
                    }
                }

                if (dtl.IsNewInventory)
                {
                    diffBill.LesStockCountDetailList.Add(new LesStockCountDetail()
                    {
                        WarehouseId = dtl.WarehouseId,
                        StorageLocationId = dtl.StorageLocationId,
                        FactoryId = dtl.FactoryId,
                        Qty = dtl.Qty,
                        LabelNo = dtl.LabelNo,
                        LotId = dtl.LotId,
                        OnhandState = dtl.OnhandState,
                        State = LesCountState.Audit,
                        ItemExtProp = dtl.ItemExtProp,
                        ItemExtPropName = dtl.ItemExtPropName,
                        ItemId = dtl.ItemId
                    });
                }
            }
        }

        /// <summary>
        /// 差异复核获取物料标签数据
        /// </summary>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="LocId">库位ID</param>
        /// <param name="LotCode">批次号</param>
        /// <param name="ItemExtProp">物料扩展属性</param>
        /// <param name="onhandState">库存状态</param>
        /// <param name="labelNo">标签号码</param>
        /// <param name="ItemId">物料ID</param>
        /// <param name="serItems">序列号物料集合</param>
        /// <returns></returns>
        public virtual EntityList<ItemLabel> GetItemLabelByDiffReview(double warehouseId, double? LocId, string LotCode, string ItemExtProp, OnhandState? onhandState, string labelNo, double ItemId, List<double> serItems)
        {
            var query = Query<ItemLabel>().Where(p => p.WarehouseId == warehouseId && p.ItemId == ItemId);
            if (LocId.HasValue)
            {
                query.Where(p => p.StorageLocationId == LocId);
            }
            if (!LotCode.IsNullOrEmpty())
            {
                query.Where(p => p.Lot == LotCode);
            }
            if (!ItemExtProp.IsNullOrEmpty())
            {
                query.Where(p => p.ItemExtProp == ItemExtProp);
            }
            if (!labelNo.IsNullOrEmpty())
            {
                query.Where(p => p.Label == labelNo);
            }
            if (onhandState == OnhandState.Ok)
            {
                query.Where(p => p.Qty > 0);
            }
            if (onhandState == OnhandState.Ng)
            {
                query.Where(p => p.NgQty > 0);
            }

            return query.OrderBy(p => p.WarehouseId).ToList();
        }

        /// <summary>
        /// 获取调账工单数据
        /// </summary>
        /// <param name="countDtlId">调整明细Id</param>
        /// <returns></returns>        
        public virtual List<AdjustWorkOrderViewModel> GetAdjustWorkOrderViewModels(double countDtlId)
        {
            List<AdjustWorkOrderViewModel> rst = new List<AdjustWorkOrderViewModel>();

            var countDtl = RF.GetById<LesStockCountDetail>(countDtlId);
            if (countDtl == null)
                throw new ValidationException("明细已经不存在".L10N());
            var wos = RT.Service.Resolve<ItemLabelController>().GetItemLabelWorkOrders(countDtl.LabelNo, countDtl.ItemExtPropName, countDtl.StorageLocationId.Value);
            wos.ForEach(p =>
            {
                AdjustWorkOrderViewModel item = new AdjustWorkOrderViewModel()
                {
                    IsAuto = true,
                    WorkOrderId = p.WorkOrderId,
                    DiffQty = 0 - p.Qty,
                    Qty = p.Qty,
                    DtlId = countDtlId,
                    WorkOrderNo = p.WorkOrderNo
                };
                rst.Add(item);
            });

            return rst;
        }

        #endregion

        #region 接口私有方法
        /// <summary>
        /// 获取盘点单列表数据
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="pageNum">页数</param>
        /// <param name="pageSize">页码</param>
        /// <param name="deliveryTime">审核时间</param>
        /// <returns></returns>
        public virtual EntityList<LesStockCount> GetLesStockCountsList(string keyword, int pageNum, int pageSize, string deliveryTime)
        {
            var query = Query<LesStockCount>().Where(p => p.State == LesCountState.Audit || p.State == LesCountState.PartCount || p.State == LesCountState.FinishCount);
            if (!keyword.IsNullOrEmpty())
            {
                keyword = "%" + keyword + "%";
                query.Where(p => p.No.Contains(keyword));
            }
            if (!deliveryTime.IsNullOrEmpty())
            {
                DateTime date;
                if (!DateTime.TryParse(deliveryTime, out date))
                    throw new ValidationException("日期格式不正确!".L10N());
                else
                    query.Where(p => p.AuditDate >= date.Date && p.AuditDate < date.Date.AddDays(1));
            }
            return query.ToList(new PagingInfo(pageNum, pageSize), new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据盘点ID获取对应的盘点明细
        /// </summary>
        /// <param name="stockId">盘点单ID</param>
        /// <param name="keyword">仓库名称/编码</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<LesStockCountDetail> GetStockDetailByStockId(double stockId, string keyword, EagerLoadOptions elo)
        {
            var query = Query<LesStockCountDetail>().Where(p => p.LesStockCountId == stockId);
            if (!keyword.IsNullOrEmpty())
            {
                query.Where(p => p.Warehouse.Code == keyword || p.Warehouse.Name == keyword);
            }
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 根据条件获取对应的盘点明细
        /// </summary>
        /// <param name="stockId">盘点单ID</param>
        /// <param name="warehouseId">仓库ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="State">行状态</param>
        /// <param name="locId">库位ID</param>
        /// <param name="lotId">批次ID</param>
        /// <param name="ItemExtPropName">物料扩展属性</param>
        /// <param name="factoryId">工厂ID</param>
        /// <param name="labelNo">标签</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual EntityList<LesStockCountDetail> GetStockDetailByAllParams(double warehouseId, double stockId, double? itemId, LesCountState? State, double? locId, double? lotId, string ItemExtPropName, double? factoryId, string labelNo, EagerLoadOptions elo = null)
        {
            var query = Query<LesStockCountDetail>().Where(p => p.LesStockCountId == stockId && p.WarehouseId == warehouseId);
            if (itemId.HasValue)
            {
                query.Where(p => p.ItemId == itemId);
            }
            if (State.HasValue)
            {
                query.Where(p => p.State == State);
            }
            if (locId.HasValue)
            {
                query.Where(p => p.StorageLocationId == locId);
            }
            if (lotId.HasValue)
            {
                query.Where(p => p.LotId == lotId);
            }
            if (!ItemExtPropName.IsNullOrEmpty())
            {
                query.Where(p => p.ItemExtPropName == ItemExtPropName);
            }
            if (factoryId.HasValue)
            {
                query.Where(p => p.FactoryId == factoryId);
            }
            if (!labelNo.IsNullOrEmpty())
            {
                query.Where(p => p.LabelNo == labelNo);
            }
            return query.ToList(null, elo);
        }

        /// <summary>
        /// 通过盘点明细ID集合(最多只有2个ID)获取盘点明细
        /// </summary>
        /// <param name="ids">盘底ID集合（最多只有2个）</param>
        /// <param name="elo"></param>
        /// <returns></returns>
        public virtual EntityList<LesStockCountDetail> GetStockDetailByIds(List<double> ids, EagerLoadOptions elo = null)
        {
            return Query<LesStockCountDetail>().Where(p => ids.Contains(p.Id)).ToList(null, elo);
        }
        #endregion

        /// <summary>
        /// 校验物料是否在盘点范围内
        /// </summary>
        /// <param name="range">盘点范围</param>
        /// <param name="ItemId">物料ID</param>
        /// <returns>物料记录</returns>
        private EntityList<Item> CheckItemRange(LesStockCountRange range, double ItemId)
        {
            var query = Query<Item>().Where(p => p.Id == ItemId);
            if (!range.Items.IsNullOrEmpty())
            {
                var itemCodeList = range.Items.Split(';').ToList();
                query.Where(p => itemCodeList.Contains(p.Code));
            }
            if (!range.ItemCategorys.IsNullOrEmpty())
            {
                var list = range.ItemCategorys.Split(';').ToList();
                var idList = RT.Service.Resolve<ItemController>().GetItemCateNodesByPtreeCode(list);
                query.Exists<ItemCategoryRelation>((x, y) => y.Where(f => f.ItemId == x.Id && f.Type == CategoryType.Item && idList.Contains(f.ItemCategory.Id)));
            }
            if (range.ConsumeMode.HasValue)
            {
                query.Where(p => p.ConsumeMode == range.ConsumeMode);
            }
            return query.OrderBy(p => p.Id).ToList();
        }

        /// <summary>
        /// 通过盘点ID查找盘点范围
        /// </summary>
        /// <param name="stockId">盘点单ID</param>
        /// <param name="elo">贪婪加载</param>
        /// <returns></returns>
        public virtual LesStockCountRange GetCountRangesByStockId(double stockId, EagerLoadOptions elo = null)
        {
            return Query<LesStockCountRange>().Where(p => p.LesStockCountId == stockId).ToList(null, elo).FirstOrDefault();
        }
    }
}
