using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Inventory.Commom;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.LES.LesStockCounts.Datas;
using SIE.Packages.ItemLabels;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.LES.LesStockCounts
{
    /// <summary>
    /// 线边仓盘点API接口
    /// </summary>
    public partial class LesStockCountController
    {
        private readonly string lesStockBillNotExsit = "盘点单不存在!".L10N();
        #region 公共
        /// <summary>
        /// 线边仓盘点扫描识别
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="scanLoc">扫描的内容</param>
        /// <param name="id">盘点单ID</param>
        /// <param name="isNew">是否新增盘盈</param>
        /// <returns></returns>
        [ApiService("线边仓盘点:扫描识别")]
        [return: ApiReturn("线边仓盘点:扫描识别-返回扫描的各类参数等")]
        public virtual ScanAutoData GetStockScanData([ApiParameter("仓库ID")] double wareHouseId, [ApiParameter("扫描内容")] string scanLoc, [ApiParameter("盘点单ID")] double id, [ApiParameter("是否新增盘盈")] bool isNew)
        {
            var rst = new ScanAutoData();
            var stockRange = GetCountRangesByStockId(id);
            if (stockRange == null)
            {
                throw new ValidationException("扫描的内容有误!".L10N());
            }
            rst.ItemCode = "";
            rst.ItemName = "";
            rst.ItemExtPropName = "";
            rst.ItemExtProp = "";
            var loc = RT.Service.Resolve<WarehouseController>().GetStorageLocation(wareHouseId, scanLoc);
            // 扫描类型 0-库位 1-物料 2-批次 3-标签
            if (loc != null)
            {
                if (loc.State == State.Disable)
                {
                    throw new ValidationException("库位已禁用".L10N());
                }
                if (loc.IsFrozen)
                {
                    throw new ValidationException("库位已冻结".L10N());
                }
                rst.LocCode = loc.Code;
                rst.LocId = loc.Id;
                rst.Type = 0;
                return rst;
            }
            ////查物料
            if (stockRange.CountDimension == CountDimension.Item)
            {
                var item = RT.Service.Resolve<ItemController>().GetItems(new List<string> { scanLoc }).FirstOrDefault();
                if (item != null)
                {
                    var itemId = item.Id;
                    if (isNew)
                    {
                        rst.Type = 1;
                        rst.ItemId = itemId;
                        rst.ItemCode = item.Code;
                        rst.ItemName = item.Name;
                        return rst;
                    }
                    else
                    {
                        var stockDetails = GetStockDetailByAllParams(wareHouseId, id, itemId, null, null, null, "", null, "");
                        if (stockDetails.Count > 0)
                        {
                            var itemExtPropList = stockDetails.Where(p => !p.ItemExtPropName.IsNullOrEmpty()).Select(p => p.ItemExtPropName).Distinct().ToList();
                            rst.Type = 1;
                            rst.ItemId = itemId;
                            rst.ItemCode = item.Code;
                            rst.ItemExtPropList = itemExtPropList;
                            rst.ItemName = item.Name;
                            if (itemExtPropList.Count == 1)
                            {
                                rst.ItemExtPropName = itemExtPropList[0];
                            }
                            return rst;
                        }
                    }

                }
            }

            //查标签(标签或标签+库位会出现物料当标签或批次当标签的情况 先前置查询不然会识别为物料或者批次)
            var ItemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(scanLoc);
            if (ItemLabels.Count > 0)
            {
                var ItemLabelItem = ItemLabels.FirstOrDefault();
                if (ItemLabelItem == null)
                {
                    throw new ValidationException("扫描的内容有误!".L10N());
                }
                var lotItem = RT.Service.Resolve<LotController>().GetLot(ItemLabelItem.Lot, ItemLabelItem.ItemId, ItemLabelItem.ItemExtProp);
                rst.Type = 3;
                rst.ItemId = ItemLabelItem.ItemId;
                rst.LotId = lotItem?.Id;
                rst.LotCode = lotItem?.Code;
                rst.ItemCode = ItemLabelItem.Item.Code;
                rst.ItemName = ItemLabelItem.Item.Name;
                rst.ItemExtProp = ItemLabelItem.ItemExtProp;
                rst.ItemExtPropName = ItemLabelItem.ItemExtPropName;
                rst.ItemExtPropList = ItemLabels.Where(p => !p.ItemExtPropName.IsNullOrEmpty()).Select(p => p.ItemExtPropName).Distinct().ToList();
                rst.LabelNo = ItemLabelItem.Label;
                return rst;
            }
            throw new ValidationException("扫描的内容有误!".L10N());
        }
        #endregion

        #region 线边仓盘点-盘点
        /// <summary>
        /// 获取线边仓盘点单
        /// </summary>
        /// <param name="keyword">盘点单号</param>
        /// <param name="deliveryTime">交货日期(只包含年月日)</param>
        /// <param name="pageNumber">第几页（从1开始）</param>
        /// <param name="pageSize">每页多少行数据</param>
        /// <returns>获取线边仓盘点单</returns>
        [ApiService("线边仓盘点:获取线边仓盘点单")]
        [return: ApiReturn("线边仓盘点:获取线边仓盘点单")]
        public virtual List<StockCountList> GetLesStockList([ApiParameter("关键字")] string keyword, [ApiParameter("交货日期")] string deliveryTime, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            var rst = new List<StockCountList>();
            var StockList = GetLesStockCountsList(keyword, pageNumber, pageSize, deliveryTime);
            StockList.ForEach(p =>
            {
                var CountRange = p.LesStockCountRangeList.FirstOrDefault();
                if (CountRange != null)
                {
                    var stockItem = new StockCountList()
                    {
                        BillId = p.Id,
                        BillNo = p.No,
                        AuditTime = p.AuditDate,
                        OrderType = p.OrderType,
                        OrderTypeStr = p.OrderType.ToLabel(),
                        CountDimension = CountRange.CountDimension,
                        IsBlindCount = CountRange.IsBlindCount,
                        IsDynamicOnhand = CountRange.IsDynamicOnhand,
                        AuditTimeStr = p.AuditDate?.ToString("MM-dd HH:mm"),
                    };
                    rst.Add(stockItem);
                }
            });
            return rst;
        }

        /// <summary>
        /// 获取盘点单下的仓库分组数据
        /// </summary>
        /// <param name="Id">盘点单ID</param>
        /// <param name="keyword">仓库编码</param>
        /// <param name="deliveryTime">交货日期(只包含年月日)</param>
        /// <param name="pageNumber">第几页（从1开始）</param>
        /// <param name="pageSize">每页多少行数据</param>
        /// <returns>获取未收货ASN列表，不包含已经码盘的ASN单</returns>
        [ApiService("线边仓盘点:获取盘点单下的仓库分组数据")]
        [return: ApiReturn("盘点单下的仓库分组数据")]
        public virtual List<StockWareHouseList> GetLesStockByWareHouse([ApiParameter("盘点单ID")] double Id, [ApiParameter("关键字")] string keyword, [ApiParameter("交货日期")] string deliveryTime, [ApiParameter("页号")] int pageNumber, [ApiParameter("页码")] int pageSize)
        {
            var rst = new List<StockWareHouseList>();
            var stockDetail = GetStockDetailByStockId(Id, keyword, new EagerLoadOptions().LoadWithViewProperty());
            if (stockDetail.Count == 0)
            {
                return rst;
            }
            if (keyword.IsNullOrEmpty())
            {
                rst = stockDetail.GroupBy(p => new { p.WarehouseId, p.WareHouseCode, p.WareHouseName }).Select(t => new StockWareHouseList()
                {
                    WareHouseCode = t.Key.WareHouseCode,
                    WareHouseName = t.Key.WareHouseName,
                    WareHouseId = t.Key.WarehouseId,
                    FinishQty = t.Count(a => a.State == LesCountState.FinishCount),
                    UnFinishQty = t.Count(a => a.State == LesCountState.Audit),
                }).ToList();
            }
            else
            {
                //如果有查询条件 那么查出来的仓库明细分组数据只会有一条
                var stockDetailItem = stockDetail.FirstOrDefault();
                if (stockDetailItem == null)
                {
                    return rst;
                }
                var stockWareItem = new StockWareHouseList()
                {
                    WareHouseCode = stockDetailItem.WareHouseCode,
                    WareHouseName = stockDetailItem.WareHouseName,
                    WareHouseId = stockDetailItem.WarehouseId,
                    FinishQty = stockDetail.Count(p => p.State == LesCountState.FinishCount),
                    UnFinishQty = stockDetail.Count(p => p.State == LesCountState.Audit),
                };
                rst.Add(stockWareItem);
            }
            return rst;
        }

        /// <summary>
        /// 获取盘点单明细(加载、跳过、点击任务列表获取盘点单明细)
        /// </summary>
        /// <param name="id">盘点单ID</param>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="lineNo">行号</param>
        /// <param name="detailID">盘点单明细ID</param>
        /// <returns></returns>
        [ApiService("线边仓盘点:获取盘点单下的指定仓库明细分组数据")]
        [return: ApiReturn("盘点单下的指定仓库明细分组数据")]
        public virtual StockDetailData GetStockDetailData([ApiParameter("盘点单ID")] double id, [ApiParameter("仓库ID")] double wareHouseId, [ApiParameter("行号")] string lineNo = "", [ApiParameter("明细ID")] double detailID = 0)
        {
            var rst = new StockDetailData();
            var stockDetails = GetStockDetailByAllParams(wareHouseId, id, null, LesCountState.Audit, null, null, "", null, "", new EagerLoadOptions().LoadWithViewProperty());
            if (stockDetails.Count == 0)
            {
                return rst;
            }

            var stockDetailData = stockDetailDataByLineNo(stockDetails, lineNo);
            if (stockDetailData == null)
            {
                return rst;
            }
            rst.Id = id;
            rst.WareHouseId = stockDetailData.WarehouseId;
            rst.WareHouseCode = stockDetailData.WareHouseCode;
            rst.ItemExtProp = stockDetailData.ItemExtProp;
            rst.ItemExtPropName = stockDetailData.ItemExtPropName;
            rst.LineNo = stockDetailData.LineNo;
            rst.LotId = stockDetailData.LotId;
            rst.LocId = stockDetailData.StorageLocationId;
            rst.DetailIds = new List<double>();
            rst.ItemId = stockDetailData.ItemId;
            rst.ItemCode = stockDetailData.ItemCode;
            rst.ItemName = stockDetailData.ItemName;
            rst.FactroyId = stockDetailData.FactoryId;
            rst.FactoryName = stockDetailData.FactoryName;
            rst.FactoryCode = stockDetailData.FactoryCode;
            rst.LabelNo = stockDetailData.LabelNo;
            var stockDetailDatas = GetStockDetailByAllParams(wareHouseId, id, stockDetailData.ItemId, LesCountState.Audit, stockDetailData.StorageLocationId, stockDetailData.LotId, stockDetailData.ItemExtPropName, stockDetailData.FactoryId, stockDetailData.LabelNo);
            stockDetailDatas.ForEach(p =>
            {
                rst.DetailIds.Add(p.Id);
                if (p.OnhandState == OnhandState.Ok)
                {
                    rst.HasOK = true;
                    rst.OkQty = p.Qty;
                }
                if (p.OnhandState == OnhandState.Ng)
                {
                    rst.HasNg = true;
                    rst.NgQty = p.Qty;
                }
            });

            return rst;
        }

        /// <summary>
        /// 根据行号获取盘点明细
        /// </summary>
        /// <param name="stockDetails"></param>
        /// <param name="LineNo"></param>
        /// <returns></returns>
        private LesStockCountDetail stockDetailDataByLineNo(EntityList<LesStockCountDetail> stockDetails, string LineNo)
        {
            var stockDetailList = stockDetails.OrderBy(p => Convert.ToInt32(p.LineNo)).ToList();
            if (LineNo.IsNullOrEmpty())
            {
                //如果行号是空的取第一个
                return stockDetailList.FirstOrDefault();
            }
            else
            {

                var data = stockDetailList.Where(p => Convert.ToInt32(p.LineNo) > Convert.ToInt32(LineNo)).OrderBy(p => Convert.ToInt32(p.LineNo)).ToList();
                if (data.Count > 0)
                {
                    var nowStockDetailItem = stockDetailList.FirstOrDefault(p => p.LineNo == LineNo);
                    if (nowStockDetailItem == null)
                    {
                        throw new ValidationException("行号[{0}]对应的盘点明细不存在".L10nFormat(LineNo));
                    }
                    //不一样的盘点明细指仓库/物料/物料扩展属性/批次/库位/工厂/标签存在不一样的明细
                    return data.FirstOrDefault(p => p.WarehouseId != nowStockDetailItem.WarehouseId || p.ItemId != nowStockDetailItem.ItemId || p.ItemExtPropName != nowStockDetailItem.ItemExtPropName || p.LotId != nowStockDetailItem.LotId || p.StorageLocationId != nowStockDetailItem.StorageLocationId || p.FactoryId != nowStockDetailItem.FactoryId || p.LabelNo != nowStockDetailItem.LabelNo);
                }
                else
                {
                    return stockDetailList.FirstOrDefault();
                }
            }
        }
        /// <summary>
        /// 根据参数获取盘点明细
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="id">盘点单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <param name="lotId">批次ID</param>
        /// <param name="locId">库位ID</param>
        /// <param name="labelNo">标签条码</param>
        /// <param name="isNew">是否新增盘盈</param>
        /// <returns></returns>
        [ApiService("线边仓盘点:根据参数获取盘点明细")]
        [return: ApiReturn("根据参数获取盘点明细")]
        public virtual StockDetailData GetStockDetailByParams([ApiParameter("仓库ID")] double wareHouseId, [ApiParameter("盘点单ID")] double id, [ApiParameter("物料ID")] double? itemId, [ApiParameter("物料扩展属性")] string itemExtPropName, [ApiParameter("批次ID")] double? lotId, [ApiParameter("库位ID")] double? locId, [ApiParameter("标签条码")] string labelNo, [ApiParameter("是否新增盘盈")] bool isNew)
        {
            var rst = new StockDetailData();
            LesCountState? state = null;
            if (!isNew)
            {
                state = LesCountState.Audit;
            }
            var stockDetails = GetStockDetailByAllParams(wareHouseId, id, itemId, state, locId, lotId, itemExtPropName, null, labelNo, new EagerLoadOptions().LoadWithViewProperty());
            if (stockDetails.Count == 0)
            {
                return rst;
            }
            var stockDetail = stockDetails.FirstOrDefault();
            if (stockDetail == null)
            {
                return rst;
            }
            rst.Id = id;
            rst.WareHouseId = stockDetail.WarehouseId;
            rst.WareHouseCode = stockDetail.WareHouseCode;
            rst.ItemExtProp = stockDetail.ItemExtProp;
            rst.ItemExtPropName = stockDetail.ItemExtPropName;
            rst.LineNo = stockDetail.LineNo;
            rst.LotId = stockDetail.LotId;
            rst.LocId = stockDetail.StorageLocationId;
            rst.ItemId = stockDetail.ItemId;
            rst.ItemCode = stockDetail.ItemCode;
            rst.ItemName = stockDetail.ItemName;
            rst.DetailIds = new List<double>();
            rst.FactroyId = stockDetail.FactoryId;
            rst.FactoryName = stockDetail.FactoryName;
            rst.FactoryCode = stockDetail.FactoryCode;
            rst.LabelNo = stockDetail.LabelNo;
            var stockDetailDatas = GetStockDetailByAllParams(wareHouseId, id, stockDetail.ItemId, state, stockDetail.StorageLocationId, stockDetail.LotId, stockDetail.ItemExtPropName, stockDetail.FactoryId, stockDetail.LabelNo);
            stockDetailDatas.ForEach(p =>
            {
                rst.DetailIds.Add(p.Id);
                if (p.OnhandState == OnhandState.Ok)
                {
                    rst.HasOK = true;
                    rst.OkQty = p.Qty;
                }
                if (p.OnhandState == OnhandState.Ng)
                {
                    rst.HasNg = true;
                    rst.NgQty = p.Qty;
                }
            });
            return rst;
        }

        /// <summary>
        /// 线边仓盘点-普通盘点提交
        /// </summary>
        /// <param name="submitData"></param>
        [ApiService("线边仓盘点:普通盘点提交")]
        [return: ApiReturn("void方法")]
        public virtual void SubmitLesStockCountDetail(StockDetailData submitData)
        {
            var LesStockCount = RF.GetById<LesStockCount>(submitData.Id, new EagerLoadOptions().LoadWithViewProperty());
            var range = LesStockCount.LesStockCountRangeList.FirstOrDefault();
            if (range == null)
            {
                throw new ValidationException("盘点单下盘点范围不存在!".L10N());
            }
            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                LesStockCount.LesStockCountDetailList.Where(p => submitData.DetailIds.Contains(p.Id)).ForEach(p =>
                {

                    if (p.State != LesCountState.Audit)
                    {
                        throw new ValidationException("当前盘点明细状态不是待盘点!".L10N());
                    }
                    if (p.OnhandState == OnhandState.Ok)
                    {
                        p.ActualCountQty = submitData.OkQty;
                    }
                    else if (p.OnhandState == OnhandState.Ng)
                    {
                        p.ActualCountQty = submitData.NgQty;
                    }
                    if (range.IsDynamicOnhand)
                    {
                        var nowQty = GetStockCountDetailQty(p.ItemId, p.WarehouseId, p.LabelNo, p.ItemExtPropName, p.LotCode, p.StorageLocationId, p.OnhandState, p.FactoryId);
                        p.Qty = nowQty;
                    }
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
                    p.State = LesCountState.FinishCount;
                });
                UpdateLesCountState(LesStockCount);
                RF.Save(LesStockCount);
                trans.Complete();
            }

        }
        /// <summary>
        /// 获取盘点单下指定仓库的明细分组
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="id">盘点单ID</param>
        /// <returns></returns>
        [ApiService("线边仓盘点:获取盘点单下指定仓库的明细分组")]
        [return: ApiReturn("获取盘点单下指定仓库的明细分组")]
        public virtual StockGroupData GetStockDetailGroups([ApiParameter("仓库ID")] double wareHouseId, [ApiParameter("盘点单ID")] double id)
        {
            var rst = new StockGroupData();
            var LesStockCount = RF.GetById<LesStockCount>(id);
            if (LesStockCount == null)
            {
                throw new ValidationException(lesStockBillNotExsit);
            }
            var stockDetailDatas = GetStockDetailByAllParams(wareHouseId, id, null, null, null, null, "", null, "", new EagerLoadOptions().LoadWithViewProperty());
            var CountList = stockDetailDatas.Where(p => p.State == LesCountState.Audit).ToList();
            var UnCountList = stockDetailDatas.Where(p => p.State == LesCountState.FinishCount).ToList();
            rst.CountList.AddRange(CountList);
            rst.FinCountList.AddRange(UnCountList);
            return rst;
        }

        /// <summary>
        /// 校验盘点单状态
        /// </summary>
        /// <param name="id">盘点单ID</param>
        /// <returns></returns>
        [ApiService("线边仓盘点:校验盘点单状态")]
        [return: ApiReturn("校验盘点单状态")]
        public virtual bool CheckStockStatus([ApiParameter("盘点单ID")] double id)
        {
            var LesStockCount = RF.GetById<LesStockCount>(id);
            if (LesStockCount == null)
            {
                throw new ValidationException(lesStockBillNotExsit);
            }
            if (LesStockCount.State != LesCountState.Audit && LesStockCount.State != LesCountState.PartCount && LesStockCount.State != LesCountState.FinishCount)
            {
                throw new ValidationException("盘点单状态不是待盘点、部分盘点、已盘点状态!请刷新数据!".L10N());
            }
            return true;
        }
        #endregion

        #region 线边仓盘点-新增盘盈
        /// <summary>
        /// 判断物料是否在盘点范围内
        /// </summary>
        /// <param name="id">盘点单ID</param>
        /// <param name="itemId">物料ID</param>
        /// <param name="ItemCode">物料编码</param>
        /// <returns></returns>
        [ApiService("线边仓盘点:判断物料是否在盘点范围内")]
        [return: ApiReturn("判断物料是否在盘点范围内")]
        public virtual bool CheckStockRangeByItem([ApiParameter("盘点单ID")] double id, [ApiParameter("物料ID")] double itemId, [ApiParameter("物料编码")] string ItemCode)
        {
            var LesStockCount = RF.GetById<LesStockCount>(id, new EagerLoadOptions().LoadWithViewProperty());
            if (LesStockCount == null)
            {
                throw new ValidationException(lesStockBillNotExsit);
            }
            var range = LesStockCount.LesStockCountRangeList.FirstOrDefault();
            if (range == null)
            {
                throw new ValidationException("盘点范围不存在!".L10N());
            }
            var items = CheckItemRange(range, itemId);
            if (items.Count == 0)
            {
                throw new ValidationException("物料不在盘点单盘点范围内".L10nFormat(ItemCode));
            }
            return true;
        }


        /// <summary>
        /// 线边仓盘点-新增盘盈提交
        /// </summary>
        /// <param name="submitData"></param>
        /// <exception cref="ValidationException"></exception>
        [ApiService("线边仓盘点:新增盘盈提交")]
        [return: ApiReturn("新增盘盈提交")]
        public virtual void SubmitNewStockDetail([ApiParameter("新增盘盈明细")] StockDetailData submitData)
        {
            var LesStockCount = RF.GetById<LesStockCount>(submitData.Id, new EagerLoadOptions().LoadWithViewProperty());
            if (LesStockCount == null)
            {
                throw new ValidationException(lesStockBillNotExsit);
            }
            var range = LesStockCount.LesStockCountRangeList.FirstOrDefault();
            if (range == null)
            {
                throw new ValidationException("盘点单下盘点范围不存在!".L10N());
            }
            if (range.CountDimension == CountDimension.Label || range.CountDimension == CountDimension.Location)
            {
                var ItemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabels(submitData.LabelNo).FirstOrDefault();
                if (ItemLabel == null)
                {
                    throw new ValidationException("标签在物料标签不存在!".L10N());
                }
                if (ItemLabel.IsSerialNumber == true && (ItemLabel.Qty != 0 || ItemLabel.NgQty != 0))
                {
                    throw new ValidationException("当前序列号在物料标签表中数量不为0!".L10N());
                }
            }
            var lesDetail = LesStockCount.LesStockCountDetailList.OrderByDescending(p => Convert.ToInt32(p.LineNo)).FirstOrDefault();

            if (lesDetail == null)
            {
                throw new ValidationException("盘点明细不存在!".L10N());
            }
            int maxLineNo = Convert.ToInt32(lesDetail.LineNo);
            //HasOK/HasNg为true说明该明细存在库存状态为合格/不合格的盘点明细 所以取反 说明是新增盘盈
            if (!submitData.HasOK && submitData.OkQty > 0)
            {
                //合格的新增盘盈
                var okDetail = GetNewStockCountDetail(submitData.Id, submitData, range);
                maxLineNo++;
                okDetail.LineNo = maxLineNo.ToString();
                okDetail.OnhandState = OnhandState.Ok;
                okDetail.DiffCountQty = submitData.OkQty;
                okDetail.ActualCountQty = submitData.OkQty;
                LesStockCount.LesStockCountDetailList.Add(okDetail);
            }
            if (!submitData.HasNg && submitData.NgQty > 0)
            {
                //不合格的新增盘盈
                var ngDetail = GetNewStockCountDetail(submitData.Id, submitData, range);
                maxLineNo++;
                ngDetail.LineNo = maxLineNo.ToString();
                ngDetail.OnhandState = OnhandState.Ng;
                ngDetail.DiffCountQty = submitData.NgQty;
                ngDetail.ActualCountQty = submitData.NgQty;
                LesStockCount.LesStockCountDetailList.Add(ngDetail);
            }
            SortLesStockDetail(LesStockCount);
            ValiateCountDtl(LesStockCount, range);
            NewInventoryStockCountDetailComparerFunc(LesStockCount);
            using (var trans = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                RF.Save(LesStockCount);
                trans.Complete();
            }
        }

        /// <summary>
        /// 校验标签能否扫描(如果扫描的标签对应的物料启用了序列号管理，则此标签在物料库存表中不能存在可用数量或不良品数不为0的数据)
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="scanNo">标签条码</param>
        [ApiService("线边仓盘点:新增盘盈校验标签能否扫描")]
        [return: ApiReturn("新增盘盈-校验标签能否扫描")]
        public virtual bool CheckItemLabel([ApiParameter("物料ID")] double itemId, [ApiParameter("标签条码")] string scanNo)
        {
            var ItemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(scanNo);
            if (ItemLabel == null)
            {
                throw new ValidationException("标签[{0}]在物料标签表中不存在!".L10nFormat(scanNo));
            }
            var isSer = RT.Service.Resolve<ItemStockBaseController>().CheckItemIsSer(itemId);
            if (isSer && (ItemLabel.Qty > 0 || ItemLabel.NgQty > 0))
            {
                throw new ValidationException("标签[{0}]在物料标签表中存在可用数或不良数大于0的标签数据!".L10nFormat(scanNo));
            }
            return true;
        }

        /// <summary>
        /// 获取新的盘点明细
        /// </summary>
        /// <param name="stockId">盘点单ID</param>
        /// <param name="submitData">提交的数据</param>
        /// <param name="range">盘点范围</param>
        /// <returns></returns>
        private LesStockCountDetail GetNewStockCountDetail(double stockId, StockDetailData submitData, LesStockCountRange range)
        {
            var rst = new LesStockCountDetail();
            rst.GenerateId();
            rst.ItemId = submitData.ItemId;
            rst.ItemExtProp = submitData.ItemExtProp;
            rst.ItemExtPropName = submitData.ItemExtPropName;
            rst.FactoryId = submitData.FactroyId;
            rst.LotId = submitData.LotId;
            rst.StorageLocationId = submitData.LocId;
            rst.IsNewInventory = true;
            rst.LesStockCountId = stockId;
            rst.State = LesCountState.FinishCount;
            rst.WarehouseId = submitData.WareHouseId;
            rst.LesStockCountDetailResult = LesStockCountDetailResult.Abnormal;
            rst.Qty = 0;
            rst.CountById = RT.IdentityId;
            rst.CountDate = DateTime.Now;
            rst.CountDimension = range.CountDimension;
            rst.LabelNo = submitData.LabelNo ?? "";
            return rst;
        }

        /// <summary>
        /// 对盘点明细的物料扩展属性进行重新排序 因为可能会出现物料扩展属性一样但是顺序不一样导致无法校验重复的情况
        /// </summary>
        private void SortLesStockDetail(LesStockCount lesStockCount)
        {
            lesStockCount.LesStockCountDetailList.ForEach(p =>
            {
                if (!p.ItemExtProp.IsNullOrEmpty())
                {
                    var ItemExtPropList = p.ItemExtProp.Split(';').OrderBy(x => x).ToList();
                    var ItemExtpropNameList = p.ItemExtPropName.Split(';').OrderBy(x => x).ToList();
                    p.ItemExtProp = String.Join(";", ItemExtPropList);
                    p.ItemExtPropName = String.Join(";", ItemExtpropNameList);
                }
            });
        }
        #endregion


    }
}
