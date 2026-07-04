using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.EventMessages.EMS.Purchases;
using SIE.Fixtures.FixtureRecords;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.Models;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Fixtures.InboundOrders
{
    /// <summary>
    /// 工治具入库控制器
    /// </summary>
    public class InboundOrderController : DomainController
    {
        /// <summary>
        /// 获取工治具入库数据
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<InboundOrder> GetInboundOrders(InboundOrderCriteria criteria)
        {
            var query = Query<InboundOrder>();
            if (criteria.InboundOrderNo.IsNotEmpty())
                query.Where(p => p.No.Contains(criteria.InboundOrderNo));

            if (criteria.ReceiptOrderNo.IsNotEmpty())
                query.Where(p => p.ReceiptOrderNo.Contains(criteria.ReceiptOrderNo));

            if (criteria.AcceptanceOrderNo.IsNotEmpty())
                query.Where(p => p.AcceptanceOrderNo.Contains(criteria.AcceptanceOrderNo));

            if (criteria.InboundStatus.HasValue)
                query.Where(p => p.InboundStatus == criteria.InboundStatus);
            if (criteria.InboundType.HasValue)
                query.Where(p => p.InboundType == criteria.InboundType);

            if (criteria.FixtureEncodeId.HasValue)
                query.Where(p => p.FixtureEncodeId == criteria.FixtureEncodeId);
            if (criteria.ManageMode.HasValue)
                query.Join<FixtureModel>((x, y) => x.FixtureEncode.FixtureModelId == y.Id && y.ManageMode == criteria.ManageMode);
            if (criteria.InboundTime != null)
            {
                if (criteria.InboundTime.BeginValue.HasValue)
                {
                    query.Where(x => x.InboundDate >= criteria.InboundTime.BeginValue);
                }
                if (criteria.InboundTime.EndValue.HasValue)
                {
                    query.Where(x => x.InboundDate <= criteria.InboundTime.EndValue);
                }
            }
            if (!criteria.PurchaseOrderNo.IsNullOrEmpty())
            {
                query.Exists<InboundOrderFixtureIdAccount>((x, y) => y.Where(m => m.InboundOrderId == x.Id && m.PoNo == criteria.PurchaseOrderNo));
                query.Exists<InboundOrderPurchase>((x, y) => y.Where(m => m.InboundOrderId == x.Id && m.PoNo == criteria.PurchaseOrderNo));
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());

        }

        /// <summary>
        /// 保存入库单
        /// </summary>
        /// <param name="model"></param>
        public virtual bool SaveInboundOrder(InboundOrder model)
        {
            if (model.InboundStatus != Equipments.Enums.InboundStatus.ToBe && model.InboundStatus != Equipments.Enums.InboundStatus.Doing)
            {
                throw new ValidationException("单据状态不为待入库，不能入库".L10N());
            }
            model.ScanedNum = model.FixtureEncode.FixtureModel.ManageMode == ManageMode.Code ? model.InboundOrderFixtureCodeAccountList.Count(m => m.StorageLocation != null) :
                model.InboundOrderFixtureIdAccountList.Count(m => m.StorageLocation != null);
            //要求必须填写库位才可以离开
            if (model.FixtureEncode.FixtureModel.ManageMode == ManageMode.Number)
            {
                if (model.InboundOrderFixtureIdAccountList.Any(m => m.StorageLocation == null))
                {
                    throw new ValidationException("存在明细行未填写库位信息!".L10N());
                }
                if (model.InboundOrderFixtureIdAccountList.Sum(m => m.Qty) > model.Qty)
                {
                    throw new ValidationException("明细行合计入库数不能大于本次入库数！".L10N());
                }
            }
            if (model.FixtureEncode.FixtureModel.ManageMode == ManageMode.Code)
            {
                if (model.InboundOrderFixtureCodeAccountList.Any(m => m.StorageLocation == null))
                {
                    throw new ValidationException("存在明细行未填写库位信息,请检查！".L10N());
                }
                if (model.InboundOrderFixtureCodeAccountList.Sum(m => m.Qty) > model.Qty)
                {
                    throw new ValidationException("明细行合计入库数不能大于本次入库数,请检查！".L10N());
                }
            }
            model.InboundStatus = InboundStatus.Doing;
            RF.Save(model);
            return true;

        }

        /// <summary>
        /// 提交工治具入库单
        /// </summary>
        /// <param name="model">工治具入库单</param>
        public virtual bool SubmitInboundOrder(InboundOrder model)
        {
            CheckSubmitInfo(model);
            EntityList<FixtureAccount> fixtureAccounts = new EntityList<FixtureAccount>();
            EntityList<FixtureAccountStock> fixtureAccountStocks = new EntityList<FixtureAccountStock>();
            var dbTime = RF.Find<FixtureAccount>().GetDbTime();

            List<PurchasesUpdateInfo> updateInfos = new List<PurchasesUpdateInfo>();
            EntityList<FixtureRecord> fixtureRecordList = new EntityList<FixtureRecord>();
            var storageLocations = RT.Service.Resolve<CoreFixtureController>().GetFixtureEncodeStorageLocationsByEncodeId(model.FixtureEncodeId);
            if (model.FixtureEncode.FixtureModel.ManageMode == ManageMode.Code)
            {
                UpdateFixtureStockByCode(model, fixtureAccounts, fixtureAccountStocks);

                foreach (var detail in model.InboundOrderPurchaseList)
                {
                    updateInfos.Add(new PurchasesUpdateInfo()
                    {
                        AccNo = model.AcceptanceOrderNo,
                        PoNo = detail.PoNo,
                        PoNoLineNo = !detail.PoLine.IsNullOrEmpty() ? int.Parse(detail.PoLine) : 0,
                        InboundQty = (int)detail.Qty,
                        RecNo = model.ReceiptOrderNo
                    });
                }
                foreach (var detail in model.InboundOrderFixtureCodeAccountList)
                {
                    if (storageLocations.Any() && storageLocations.FirstOrDefault(m => m.WarehouseId == model.WarehouseId && m.StorageLocationId == detail.StorageLocationId) == null)
                    {
                        throw new ValidationException("入库明细的【仓库和库位】须在工治具编码[{0}]存储位置列表中！".L10nFormat(model.FixtureEncode.Code));
                    }
                }
                CreateCodeFixtureRecord(model, fixtureRecordList);
            }
            else
            {
                var codeIds = model.InboundOrderFixtureIdAccountList.Select(m => m.FixtureIDAccountId).Distinct();

                if (!codeIds.Any())
                {
                    throw new ValidationException("存在明细行未填写工治具ID信息！".L10N());
                }
                if (model.InboundOrderFixtureIdAccountList.Sum(m => m.Qty) != model.Qty)
                {
                    throw new ValidationException("入库明细合计数量不等于入库数，请确认！".L10N());
                }


                fixtureAccounts = Query<FixtureAccount>().Where(p => codeIds.Contains(p.Id)).ToList();

                foreach (var fixtureAccount in fixtureAccounts)
                {
                    var stock = model.InboundOrderFixtureIdAccountList.FirstOrDefault(m => m.FixtureIDAccountId == fixtureAccount.Id);

                    if (stock != null)
                    {
                        fixtureAccount.AccountState = FixtureAccountState.InStorage;
                        //fixtureAccount.QualityState = model.QualityState; 2022/6/29 俊杰要求不更新质量状态
                        if (!fixtureAccount.InStorageDate.HasValue)
                        {
                            fixtureAccount.InStorageDate = dbTime;
                        }
                        fixtureAccountStocks.Add(new FixtureAccountStock
                        {
                            FixtureWarehouseId = model.WarehouseId.Value,
                            TotalQty = (int)stock.Qty,
                            FixtureAccountId = fixtureAccount.Id,
                            PassQty = (int)stock.Qty,
                            FixtureStorageLocationId = stock.StorageLocationId
                        });
                        //生成出入库明细
                        CreateIDFixtureRecords(model, fixtureRecordList, fixtureAccount, stock);
                    }
                }

                foreach (var detail in model.InboundOrderFixtureIdAccountList)
                {
                    if (storageLocations.Any() && storageLocations.FirstOrDefault(m => m.WarehouseId == model.WarehouseId && m.StorageLocationId == detail.StorageLocationId) == null)
                    {
                        throw new ValidationException("入库明细的【仓库和库位】须在工治具编码[{0}]的存储位置列表中！".L10nFormat(model.FixtureEncode.Code));
                    }
                    if (detail.MaintainTaskId.HasValue&&detail.MaintainTask.State != MaintainTasks.MaintainState.Finish)
                    {
                        throw new ValidationException("存在待保养的入库明细,请先保养完成再提交！".L10N());
                    }

                    updateInfos.Add(new PurchasesUpdateInfo()
                    {
                        AccNo = model.AcceptanceOrderNo,
                        PoNo = detail.PoNo,
                        PoNoLineNo = !detail.PoLineNo.IsNullOrEmpty() ? int.Parse(detail.PoLineNo) : 0,
                        InboundQty = (int)detail.Qty,
                        RecNo = model.ReceiptOrderNo
                    });

                }
            }

            model.InboundStatus = InboundStatus.Done;
            model.InboundDate = dbTime;

            using (var tran = DB.TransactionScope(KitFixturesEntityDataProvider.ConnectionStringName))
            {
                if (fixtureAccounts.Any())
                {
                    RF.Save(model);
                    RF.Save(fixtureAccounts);
                    RF.Save(fixtureAccountStocks);
                }
                if (fixtureRecordList.Any())
                {
                    RF.Save(fixtureRecordList);
                }
                if (updateInfos.Any() && model.InboundType == FixtureInboundType.Po)//回写采购订单的数据
                {
                    RT.Service.Resolve<IPurchases>().UpdatePurchasesInbound(updateInfos);
                }
                tran.Complete();
            }

            return true;
        }

        /// <summary>
        /// 检查提交参数
        /// </summary>
        /// <param name="model"></param>
        private void CheckSubmitInfo(InboundOrder model)
        {
            if (model == null)
            {
                throw new ValidationException("提交入库单失败，{0}信息为空！".L10nFormat(nameof(model)));
            }

            if (!model.WarehouseId.HasValue)
            {
                throw new ValidationException("主表数据请填写仓库！".L10N());
            }

            if (model.InboundOrderFixtureIdAccountList.Any(m => m.StorageLocation == null))
            {
                throw new ValidationException("存在明细行未填写库位信息！".L10N());
            }
            if (!model.QualityState.HasValue)
            {
                throw new ValidationException("入库单质量状态必填！".L10N());
            }
        }

        /// <summary>
        /// 生成出入库明细
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fixtureRecordList"></param>
        /// <param name="fixtureAccount"></param>
        /// <param name="stock"></param>
        private void CreateIDFixtureRecords(InboundOrder model, EntityList<FixtureRecord> fixtureRecordList, FixtureAccount fixtureAccount, InboundOrderFixtureIdAccount stock)
        {
            var record = new FixtureRecord();
            record.Code = model.No;
            record.Qty = 1;
            record.ApplyById = model.CreateBy;
            record.ApplyDate = model.CreateDate;
            record.ComplyById = model.CreateBy;
            record.ComplyDate = model.CreateDate;
            record.RecordType = RecordType.In;
            record.FixtureAccountId = fixtureAccount.Id;
            record.BusinessType = (BusinessType)(int)model.InboundType;
            record.FixtureWarehouseId = model.WarehouseId;
            record.FixtureStorageLocationId = stock.StorageLocationId;
            fixtureRecordList.Add(record);
        }

        /// <summary>
        /// 创建编码类入库记录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fixtureRecordList"></param>
        private void CreateCodeFixtureRecord(InboundOrder model, EntityList<FixtureRecord> fixtureRecordList)
        {
            var fixAccount = Query<FixtureAccount>().Where(m => m.Code == model.FixtureEncode.Code && m.FixtureEncodeId == model.FixtureEncodeId).FirstOrDefault();//获取编码类台账
                                                                                                                                                                   //按库位分组生成出入库明细
            var dics = model.InboundOrderFixtureCodeAccountList.GroupBy(m => m.StorageLocationId).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var detailKey in dics.Keys)
            {
                //生成出入库明细
                var record = new FixtureRecord();
                record.Code = model.No;
                record.Qty = (int)dics[detailKey].Sum(m => m.Qty);
                record.ApplyById = model.CreateBy;
                record.ApplyDate = model.CreateDate;
                record.ComplyById = model.CreateBy;
                record.ComplyDate = model.CreateDate;
                record.RecordType = RecordType.In;
                record.FixtureAccountId = fixAccount.Id;
                record.BusinessType = (BusinessType)(int)model.InboundType;
                record.FixtureWarehouseId = model.WarehouseId;
                record.FixtureStorageLocationId = detailKey;
                fixtureRecordList.Add(record);
            }
        }

        /// <summary>
        /// 按编码管理工治具按入库单更新库存信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="fixtureAccounts"></param>
        /// <param name="fixtureAccountStocks"></param>
        /// <exception cref="ValidationException"></exception>
        private void UpdateFixtureStockByCode(InboundOrder model, EntityList<FixtureAccount> fixtureAccounts, EntityList<FixtureAccountStock> fixtureAccountStocks)
        {
            if (model.InboundOrderFixtureCodeAccountList.Where(m => m.StorageLocation != null).Sum(p => p.Qty) != model.Qty)
            {
                throw new ValidationException("入库明细行数量总和需等于单据入库数！".L10N());
            }

            var fixtureAccount = Query<FixtureAccount>().Where(m => m.Code == model.FixtureEncode.Code && m.FixtureEncodeId == model.FixtureEncodeId).FirstOrDefault();


            if (fixtureAccount == null)
            {
                throw new ValidationException("填写工治具编码无对应工治具台账信息！".L10N());
            }
            else
            {
                //工治具入库（包含PDA和B/S）：【在库】增加，【待入库】减少
                fixtureAccount.InStockQty += (int)model.Qty;
                fixtureAccount.WaitShelfQty -= (int)model.Qty;
            }

            foreach (var item in model.InboundOrderFixtureCodeAccountList)
            {
                FixtureAccountStock fixtureAccountStock = fixtureAccountStocks
                    .FirstOrDefault(m => m.FixtureAccountId == fixtureAccount.Id
                    && m.FixtureWarehouseId == model.WarehouseId && m.FixtureStorageLocationId == item.StorageLocationId);

                if (fixtureAccountStock == null)
                {
                    fixtureAccountStock = Query<FixtureAccountStock>()
                        .Where(m => m.FixtureAccountId == fixtureAccount.Id
                            && m.FixtureWarehouseId == model.WarehouseId
                            && m.FixtureStorageLocationId == item.StorageLocationId)
                        .FirstOrDefault();
                    if (fixtureAccountStock != null)
                    {
                        fixtureAccountStocks.Add(fixtureAccountStock);
                    }
                }

                if (fixtureAccountStock != null)
                {
                    fixtureAccountStock.TotalQty += (int)item.Qty;
                    if (model.QualityState == FixtureQualityState.Pass)
                    {
                        fixtureAccountStock.PassQty += (int)item.Qty;
                    }
                    else
                    {
                        fixtureAccountStock.NgQty += (int)item.Qty;
                    }
                }
                else
                {
                    //新增工治具台账库存详情
                    fixtureAccountStock = new FixtureAccountStock()
                    {
                        PersistenceStatus = PersistenceStatus.New,
                        FixtureWarehouseId = model.WarehouseId.Value,
                        TotalQty = (int)item.Qty,
                        FixtureAccountId = fixtureAccount.Id,
                        //新增库存详情初始化总数为入库数，根据质量状态合格与不合格赋值为入库数
                        //PassQty = (int)item.Qty,
                        FixtureStorageLocationId = item.StorageLocationId
                    };
                    if (model.QualityState == FixtureQualityState.Pass)
                    {
                        fixtureAccountStock.PassQty += (int)item.Qty;
                    }
                    else
                    {
                        fixtureAccountStock.NgQty += (int)item.Qty;
                    }
                    fixtureAccountStocks.Add(fixtureAccountStock);
                }
            }

            fixtureAccounts.Add(fixtureAccount);
        }

        /// <summary>
        /// 根据入库单仓库获取库位
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual EntityList<StorageLocation> GetStorageLocationList(double Id, PagingInfo pagingInfo, string key)
        {
            //判断工治具编码是否是固定储位，且维护了固定储位，如果是择默认带出首行储位所选择的库位；反之则默认为空
            var parent = RF.GetById<InboundOrder>(Id);
            if (parent == null) { return new EntityList<StorageLocation>(); }
            return Query<StorageLocation>().Exists<InboundOrder>((x, y) =>y.Where(c=> x.WarehouseId == c.WarehouseId && x.WarehouseId == parent.WarehouseId))
                .WhereIf(!key.IsNullOrEmpty(), m => m.Code.Contains(key) || m.Name.Contains(key))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取默认库位
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public virtual StorageLocation GetDefaultLocation(double id)
        {
            var parent = RF.GetById<InboundOrder>(id);
            if (parent == null) { return null; }
            if (parent.FixtureEncode.FixtureModel.FixedStorage == YesNo.Yes)
            {
                return Query<StorageLocation>().Join<FixtureEncodeStorageLocation>((x, y) =>
                      x.Id == y.StorageLocationId && y.FixtureEncodeId == parent.FixtureEncodeId && x.WarehouseId == parent.WarehouseId).
                        FirstOrDefault();
            }
            else
            {
                return null;
            }
        }
    }
}
