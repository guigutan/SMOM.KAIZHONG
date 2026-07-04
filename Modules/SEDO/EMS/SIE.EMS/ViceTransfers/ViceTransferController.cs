using SIE.Common.Configs;
using SIE.Common.Employees;
using SIE.Core.Common.Controllers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Enums;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.Equipments.Configs;
using SIE.Equipments.Enums;
using SIE.Equipments.WorkFlows;
using SIE.Fixtures.FixtureDemands;
using SIE.Fixtures.Fixtures.Accounts;
using SIE.Fixtures.InboundOrders;
using SIE.Fixtures.Models;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.EMS.ViceTransfers
{
    /// <summary>
    /// 副资产调拨控制器
    /// </summary>
    public class ViceTransferController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<ViceTransfer> Fetch(ViceTransferCriteria criteria)
        {
            var query = Query<ViceTransfer>();
            if (!criteria.TransferNo.IsNullOrEmpty())
            {
                query.Where(p => p.TransferNo.Contains(criteria.TransferNo));
            }
            if (criteria.ApprovalStatus.HasValue)
            {
                query.Where(p => p.ApprovalStatus == criteria.ApprovalStatus.Value);
            }
            if (criteria.QureyFactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.QureyFactoryId.Value);
            }
            //来源仓库和目标仓库除符合查询外，额外限制数据的两个仓库至少有一个是用户有权限的仓库才能查询
            if (criteria.OriginWareHouseId.HasValue && criteria.TargetWareHouseId.HasValue)
            {
                var whList = RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmployee(null, "");
                if (!whList.Any(m => m.Id == criteria.OriginWareHouseId) && !whList.Any(m => m.Id == criteria.TargetWareHouseId))
                {
                    throw new ValidationException("当前用户没有来源仓库和目标仓库的权限!".L10N());
                }
            }

            if (criteria.OriginWareHouseId.HasValue)
            {
                query.Where(p => p.WarehouseId == criteria.OriginWareHouseId.Value);
            }
            if (criteria.TargetWareHouseId.HasValue)
            {
                query.Where(p => p.TargetWarehouseId == criteria.TargetWareHouseId.Value);
            }
            if (criteria.TransferStatus.HasValue)
            {
                query.Where(p => p.TransferStatus == criteria.TransferStatus.Value);
            }
            if (criteria.ApplicantId.HasValue)
            {
                query.Where(p => p.ApplicantId == criteria.ApplicantId.Value);
            }
            if (criteria.ViceAssetObject.HasValue)
            {
                query.Where(p => p.ViceAssetObject == criteria.ViceAssetObject.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取副资产调拨的工治具需求
        /// </summary>
        /// <param name="viceTransferFixtureIds"></param>
        /// <returns></returns>
        private EntityList<ViceTransferFixture> GetViceTransferFixtures(List<double> viceTransferFixtureIds)
        {
            return Query<ViceTransferFixture>().Where(n => viceTransferFixtureIds.Contains(n.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 保存执行
        /// </summary>
        public virtual void ExecutSave(ViceTransfer model)
        {
            if (model.ViceAssetObject == Enums.ViceAssetObject.Fixture)
            {
                ExecutSaveFixture(model);
            }
            if (model.ViceAssetObject == Enums.ViceAssetObject.Spare)
            {
                ExecutSaveSpare(model);
            }
        }

        /// <summary>
        /// 获取工治具台账列表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList GetFixtureIDAccounts(ViceTransferFixtureDetail entity, PagingInfo pagingInfo, string keyword)
        {
            //工治具编码+质量状态+状态+仓库】获取工治具ID台账。
            //在线时，选择工治具ID后，带出车间和产线，获取序列号所在的最新的工治具需求单，获取需求单的【车间+产线】；在线时，不需要带出车间产线，为空
            var accountState = entity.FixtureStatus == FixtureStatus.InStorage ? FixtureAccountState.InStorage : FixtureAccountState.Online;

            if (entity.FixtureStatus == FixtureStatus.InStorage)
            {
                return Query<FixtureIDAccount>().Join<FixtureAccountStock>((x, y) => x.Id == y.FixtureAccountId && y.FixtureWarehouseId == entity.WarehouseId)
                     .Where(m => m.FixtureEncode.Code == entity.FixtureEncodeCode && m.QualityState == entity.FixtureQualityState && m.AccountState == accountState)
                     .WhereIf(!keyword.IsNullOrEmpty(), m => m.Code.Contains(keyword))
                     .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            else
            {
                return Query<FixtureIDAccount>().Where(m => m.FixtureEncode.Code == entity.FixtureEncodeCode
                           && m.QualityState == entity.FixtureQualityState && m.AccountState == FixtureAccountState.Online)
                         .WhereIf(!keyword.IsNullOrEmpty(), m => m.Code.Contains(keyword))
                         .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
        }

        #region 副资产调拨工治具执行

        /// <summary>
        /// 副资产调拨工治具执行
        /// </summary>
        private void ExecutSaveFixture(ViceTransfer model)
        {
            if (model == null)
            {
                throw new ValidationException("参数异常".L10N());
            }

            EntityList<ViceTransferFixture> viceTransferFixtureList = new EntityList<ViceTransferFixture>();
            EntityList<ViceTransferFixtureDetail> toSaveviceTransferFixtureDetailList = CheckExcuteFixtureDetail(model, viceTransferFixtureList);
            if (toSaveviceTransferFixtureDetailList.Any())//符合条件的数据
            {
                using (var tran = DB.AutonomousTransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(viceTransferFixtureList);

                    var IDOnLineSaveFixtureDetails = new EntityList<ViceTransferFixtureDetail>();
                    foreach (var saveItemDetail in toSaveviceTransferFixtureDetailList)
                    {
                        if (saveItemDetail.ManageMode == Fixtures.ManageMode.Code)
                        {
                            var codeAccount = Query<FixtureAccount>().Where(m => m.Code == saveItemDetail.FixtureEncodeCode).FirstOrDefault();
                            if (codeAccount == null)
                            {
                                throw new ValidationException("{0}工治具编码不存在编码台账".L10nFormat(saveItemDetail.FixtureEncodeCode));
                            }
                            UpdateTargetStorage(saveItemDetail, codeAccount);
                            //在库调拨时，库存明细：来源库位的合格数或不合格数（根据质量状态）和总数量减少，更新后的库存不能小于0，
                            //否则报错：工治具编码XXX、来源库位XXX、质量状态XXX库存不足（并刷新对应行的【来源库位库存数】）；
                            if (saveItemDetail.FixtureStatus == FixtureStatus.InStorage)
                            {
                                UpdateSouresInStorage(saveItemDetail, codeAccount);
                            }
                            if (saveItemDetail.FixtureStatus == FixtureStatus.OnLine)
                            {
                                //在线调拨时，更新工治具编码台账的在库数量增加，在线数量减少，更新后不能小于0，
                                SetOnLineDetail(model, saveItemDetail, codeAccount);
                            }
                        }

                        if (saveItemDetail.ManageMode == Fixtures.ManageMode.Number)
                        {
                            if (!saveItemDetail.FixtureIDAccountId.HasValue)//未填写
                            {
                                break;
                            }
                            //状态更新为【在库】，库存详情的库位改为目标库位；
                            var codeAccount = RF.GetById<FixtureAccount>(saveItemDetail.FixtureIDAccountId, new EagerLoadOptions().LoadWithViewProperty());
                            if (codeAccount == null)
                            {
                                throw new ValidationException("工治具ID不存在台账中".L10N());
                            }
                            codeAccount.AccountState = FixtureAccountState.InStorage;
                            RF.Save(codeAccount);
                            UpdateTargetStorage(saveItemDetail, codeAccount);

                            //副资产调拔,工治具执行完毕后,原仓库库存减少
                            if (saveItemDetail.FixtureStatus == FixtureStatus.InStorage)
                            {
                                UpdateSouresInStorage(saveItemDetail, codeAccount);
                            }

                            if (saveItemDetail.FixtureStatus == FixtureStatus.OnLine)
                            {
                                IDOnLineSaveFixtureDetails.Add(saveItemDetail);
                            }
                        }
                    }
                    var viceTransferDBFixtureList = Query<ViceTransferFixture>().Where(m => m.ViceTransferId == model.Id).ToList();
                    model.TransferStatus = viceTransferDBFixtureList.All(m => m.Qty == m.TransferQty) ? TransferStatus.Done : TransferStatus.Partial;
                    DB.Update<ViceTransfer>().Set(m => m.TransferStatus, model.TransferStatus).Where(m => m.Id == model.Id).Execute();
                    CreateOnlineTransferIDFixtureInbound(model, IDOnLineSaveFixtureDetails);
                    RF.Save(toSaveviceTransferFixtureDetailList);
                    tran.Complete();
                }
            }
        }

        /// <summary>
        /// 获取工治具副资产列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual EntityList<ViceTransferFixture> GetViceTransferFixtureList(double id)
        {
            var result = Query<ViceTransferFixture>().Where(p => p.ViceTransferId == id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (result.Any())
            {
                foreach (var detail in result)
                {
                    var qty = GetFixtureEncodeQty(detail.FixtureEncodeId, detail.WarehouseId, detail.FixtureQualityState);
                    detail.InStorageQty = qty.Item1;
                    detail.OnlineQty = qty.Item2;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取备件明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual EntityList<ViceTransferSparePart> GetViceTransferSparePartList(double id)
        {
            var result = Query<ViceTransferSparePart>().Where(p => p.ViceTransferId == id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (result.Any())
            {
                foreach (var detail in result)
                {
                    var wh = RF.GetById<Warehouse>(detail.WarehouseId);
                    var qty = GetSparePartQty(detail.SparePartId, wh.Code, detail.QualityStatus);
                    detail.WhInventory = qty;
                }
            }
            return result;
        }

        /// <summary>
        /// 校验检查执行工治具明细数据
        /// </summary>
        /// <param name="model"></param>
        /// <param name="viceTransferFixtureList"></param>
        /// <returns></returns>
        private EntityList<ViceTransferFixtureDetail> CheckExcuteFixtureDetail(ViceTransfer model, EntityList<ViceTransferFixture> viceTransferFixtureList)
        {
            var viceTransferFixtureDetailList = model.ViceTransferFixtureDetailList;
            var viceTransferFixtureIds = viceTransferFixtureDetailList.Select(m => m.ViceTransferFixtureId).ToList();
            var toSaveviceTransferFixtureDetailList = new EntityList<ViceTransferFixtureDetail>();
            if (viceTransferFixtureIds.Any())//校验
            {
                viceTransferFixtureList.AddRange(GetViceTransferFixtures(viceTransferFixtureIds));
                foreach (var item in viceTransferFixtureDetailList)
                {
                    var updateDemandItem = viceTransferFixtureList.FirstOrDefault(m => m.Id == item.ViceTransferFixtureId);
                    if (updateDemandItem != null)
                    {//处理前端值丢失
                        item.FixtureEncodeCode = updateDemandItem.FixtureEncodeCode;
                        item.ManageMode = updateDemandItem.ManageMode;
                        updateDemandItem.TransferQty += item.TransferQty;
                        if (updateDemandItem.TransferQty > updateDemandItem.Qty)
                        {
                            throw new ValidationException("调拨数量大于需求数量".L10N());
                        }
                    }

                    if (item.FixtureStatus == FixtureStatus.InStorage && item.SourceInventoryQty < item.TransferQty)
                    {
                        if (!item.StorageLocationId.HasValue)
                        {
                            throw new ValidationException("在库记录需要选择来源库位".L10N());
                        }

                        if (item.FixtureIDAccountId.HasValue)
                        {
                            throw new ValidationException("工治具编码{0}、来源库位{1}、序列号{2}库存数小于调拨数量"
                                .L10nFormat(item.FixtureEncodeCode, item.StorageLocation.Code, item.FixtureIDAccount.Code));
                        }
                        else
                        {
                            throw new ValidationException("工治具编码{0}、来源库位{1} 库存数小于调拨数量"
                                .L10nFormat(item.FixtureEncodeCode, item.StorageLocation.Code));
                        }
                    }
                    if (item.FixtureStatus == FixtureStatus.OnLine)
                    {
                        if (item.OnlineQty < item.TransferQty)
                        {
                            throw new ValidationException("工治具编码{0}、在线数小于调拨数量".L10nFormat(item.FixtureEncodeCode));
                        }
                    }

                    var sameItemDetail = GetSameFixtureTransferDetail(item);
                    if (sameItemDetail != null)
                    {
                        sameItemDetail.TransferQty += item.TransferQty;
                        sameItemDetail.PersistenceStatus = PersistenceStatus.Modified;
                        toSaveviceTransferFixtureDetailList.Add(sameItemDetail);
                    }
                    else
                    {
                        item.PersistenceStatus = PersistenceStatus.New;
                        toSaveviceTransferFixtureDetailList.Add(item);
                    }
                }
            }

            return toSaveviceTransferFixtureDetailList;
        }

        /// <summary>
        /// 在线调拨的所有工治具ID按【工治具编码+质量状态】分组，一组生成一个入库单
        /// </summary>
        /// <param name="model"></param>
        /// <param name="IDOnLineSaveFixtureDetails"></param>
        private static void CreateOnlineTransferIDFixtureInbound(ViceTransfer model, EntityList<ViceTransferFixtureDetail> IDOnLineSaveFixtureDetails)
        {
            //在线调拨的所有工治具ID按【工治具编码+质量状态】分组，一组生成一个入库单
            if (IDOnLineSaveFixtureDetails.Any())
            {
                var dicOnlineDetails = IDOnLineSaveFixtureDetails.GroupBy(p => new { p.FixtureEncodeCode, p.FixtureQualityState }).ToDictionary(p => p.Key, p => p.ToList());
                foreach (var key in dicOnlineDetails.Keys)
                {
                    var firstItem = dicOnlineDetails[key].FirstOrDefault();
                    if (firstItem != null)
                    {
                        var firstItemFixtureEncodeId = RF.GetById<ViceTransferFixture>(firstItem.ViceTransferFixtureId).FixtureEncodeId;
                        InboundOrder inboundOrder = new InboundOrder()
                        {
                            No = RT.Service.Resolve<CommonController>().GetNo<InboundOrder>("工治具入库单号"),
                            FixtureEncodeId = firstItemFixtureEncodeId,
                            InboundStatus = InboundStatus.Done,
                            InboundType = FixtureInboundType.Scene,
                            Qty = dicOnlineDetails[key].Count,
                            WarehouseId = firstItem.WarehouseId,
                            QualityState = firstItem.FixtureQualityState,
                            RelevantOrderNo = model.TransferNo
                        };
                        inboundOrder.GenerateId();
                        dicOnlineDetails[key].ForEach(item =>
                        {
                            inboundOrder.InboundOrderFixtureIdAccountList.Add(
                                new InboundOrderFixtureIdAccount()
                                {
                                    FixtureIDAccountId = item.FixtureIDAccountId.Value,
                                    InboundOrderId = inboundOrder.Id,
                                    Qty = item.TransferQty,
                                    StorageLocationId = item.StorageLocationId
                                }
                                );

                        });
                        RF.Save(inboundOrder);
                    }
                }

            }
        }

        /// <summary>
        /// 更新在线的工治具台账信息
        /// </summary>
        /// <param name="model"></param>
        /// <param name="saveItemDetil"></param>
        /// <param name="codeAccount"></param>
        private static void SetOnLineDetail(ViceTransfer model, ViceTransferFixtureDetail saveItemDetil, FixtureAccount codeAccount)
        {
            codeAccount.InStockQty += saveItemDetil.TransferQty;
            codeAccount.OnlineQty -= saveItemDetil.TransferQty;
            if (codeAccount.OnlineQty < 0)
            {
                throw new ValidationException("工治具编码{0}在线库存不足".L10N());
            }
            //在线调拨时，生成入库单
            InboundOrder inboundOrder = new InboundOrder()
            {
                No = RT.Service.Resolve<CommonController>().GetNo<InboundOrder>("工治具入库单号"),
                FixtureEncodeId = codeAccount.FixtureEncodeId,
                InboundStatus = InboundStatus.Done,
                InboundType = FixtureInboundType.Scene,
                Qty = saveItemDetil.TransferQty,
                WarehouseId = saveItemDetil.WarehouseId,
                QualityState = saveItemDetil.FixtureQualityState,
                RelevantOrderNo = model.TransferNo
            };
            inboundOrder.GenerateId();
            RF.Save(inboundOrder);
            RF.Save(codeAccount);
        }

        /// <summary>
        /// 更新目标库位库存
        /// </summary>
        /// <param name="saveItemDetil"></param>
        /// <param name="codeAccount"></param>

        private void UpdateTargetStorage(ViceTransferFixtureDetail saveItemDetil, FixtureAccount codeAccount)
        {
            var targetWarehouseId = saveItemDetil.TargetWarehouseId;
            var fixtureAccountStock = Query<FixtureAccountStock>()
                     .Where(m => m.FixtureWarehouseId == targetWarehouseId
                     && m.FixtureStorageLocationId == saveItemDetil.TargetId
                     && m.FixtureAccountId == codeAccount.Id).FirstOrDefault();
            //库存明细：目标库位的合格数或不合格数（根据质量状态）和总数量增加（不存在目标库位的数据，则插入一条）
            if (fixtureAccountStock == null)//不存在目标库位的数据，则插入一条
            {
                FixtureAccountStock newFixtureAccountStock = new FixtureAccountStock()
                {
                    FixtureWarehouseId = targetWarehouseId,
                    FixtureAccountId = codeAccount.Id,
                    NgQty = saveItemDetil.FixtureQualityState == FixtureQualityState.Ng ? saveItemDetil.TransferQty : 0,
                    PassQty = saveItemDetil.FixtureQualityState == FixtureQualityState.Pass ? saveItemDetil.TransferQty : 0,
                    TotalQty = saveItemDetil.TransferQty,
                    FixtureStorageLocationId = saveItemDetil.TargetId,

                };
                RF.Save(newFixtureAccountStock);
            }
            else
            {
                fixtureAccountStock.PassQty += (saveItemDetil.FixtureQualityState == FixtureQualityState.Pass ? saveItemDetil.TransferQty : 0);
                fixtureAccountStock.NgQty += (saveItemDetil.FixtureQualityState == FixtureQualityState.Ng ? saveItemDetil.TransferQty : 0);
                fixtureAccountStock.TotalQty += saveItemDetil.TransferQty;
                RF.Save(fixtureAccountStock);
            }
        }

        /// <summary>
        /// 更新来源库位的库存数据
        /// </summary>
        /// <param name="saveItemDetil"></param>
        /// <param name="codeAccount"></param>
        private void UpdateSouresInStorage(ViceTransferFixtureDetail saveItemDetil, FixtureAccount codeAccount)
        {
            var souceWarehouseId = saveItemDetil.WarehouseId;
            var fixtureAccountInStorageStock = Query<FixtureAccountStock>()
                     .Where(m => m.FixtureWarehouseId == souceWarehouseId
                     && m.FixtureStorageLocationId == saveItemDetil.StorageLocationId
                     && m.FixtureAccountId == codeAccount.Id).FirstOrDefault();
            if (fixtureAccountInStorageStock != null)
            {
                fixtureAccountInStorageStock.PassQty -= (saveItemDetil.FixtureQualityState == FixtureQualityState.Pass ? saveItemDetil.TransferQty : 0);
                fixtureAccountInStorageStock.NgQty -= (saveItemDetil.FixtureQualityState == FixtureQualityState.Ng ? saveItemDetil.TransferQty : 0);
                fixtureAccountInStorageStock.TotalQty -= saveItemDetil.TransferQty;
                if (fixtureAccountInStorageStock.PassQty < 0 || fixtureAccountInStorageStock.NgQty < 0 || fixtureAccountInStorageStock.TotalQty < 0)
                {
                    throw new ValidationException("工治具编码{0}、来源库位{1}、质量状态{2}库存不足".L10nFormat(saveItemDetil.FixtureEncodeCode,
                         saveItemDetil.StorageLocation.Code, saveItemDetil.FixtureQualityState.ToLabel()));
                }
                RF.Save(fixtureAccountInStorageStock);
            }
        }

        #endregion

        #region 副资产调拨备件执行
        /// <summary>
        /// 备件执行
        /// </summary>
        private void ExecutSaveSpare(ViceTransfer model)
        {
            var viceTransferSparePartDetailList = model.ViceTransferSparePartDetailList;
            var viceTransferSparePartIds = viceTransferSparePartDetailList.Select(m => m.ViceTransferSparePartId).ToList();

            if (viceTransferSparePartIds.Any())
            {

                var toSaveViceTransferSparePartDetails = new EntityList<ViceTransferSparePartDetail>();//带保存的明细数据
                using (var tran = DB.AutonomousTransactionScope(EmsEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(model.ViceTransferSparePartList);
                    foreach (var item in viceTransferSparePartDetailList)
                    {
                        ViceTransferSparePart updateDemandItem = CheckSparePartDetailInfos(toSaveViceTransferSparePartDetails, model.ViceTransferSparePartList, item);
                        switch (item.ControlMethod)
                        {
                            case ControlMethod.Batch:
                                if (!item.StoreSummaryLotId.HasValue)
                                {
                                    throw new ValidationException("需求行号{0}批次号必填".L10nFormat(item.LineNo));
                                }

                                UpdateStoreSummaryLotInfo(item, updateDemandItem);
                                break;
                            case ControlMethod.Sn:
                                if (!item.StoreSummaryDetailId.HasValue)
                                {
                                    throw new ValidationException("需求行号{0}序列号必填".L10nFormat(item.LineNo));
                                }
                                UpdateSNStroageInfo(item, updateDemandItem);
                                break;

                            case ControlMethod.ItemCode:
                                UpdateItemCodeStorageInfo(item, updateDemandItem);
                                break;
                            default:
                                break;

                        }
                    }
                    var dbSparePartList = Query<ViceTransferSparePart>().Where(m => m.ViceTransferId == model.Id).ToList();
                    model.TransferStatus = dbSparePartList.All(m => m.Qty == m.TransferQty) ? TransferStatus.Done : TransferStatus.Partial;
                    DB.Update<ViceTransfer>().Set(m => m.TransferStatus, model.TransferStatus).Where(m => m.Id == model.Id).Execute();
                    RF.Save(toSaveViceTransferSparePartDetails);
                    tran.Complete();
                }

            }
        }

        /// <summary>
        /// 检查副资产调拨备件明细信息
        /// </summary>
        /// <param name="toSaveViceTransferSparePartDetails"></param>
        /// <param name="viceTransferSparePartList"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private ViceTransferSparePart CheckSparePartDetailInfos(EntityList<ViceTransferSparePartDetail> toSaveViceTransferSparePartDetails, EntityList<ViceTransferSparePart> viceTransferSparePartList, ViceTransferSparePartDetail item)
        {
            if (item.SourceInventoryQty < item.TransferQty)
            {
                if (item.StoreSummaryLotId.HasValue)
                {
                    throw new ValidationException("备件编码{0}、来源库位{1}、批次号{2}库存数小于调拨数量"
                        .L10nFormat(item.SparePartCode, item.StorageLocation.Code, item.StoreSummaryLot.BatchNumber));
                }
                if (item.StoreSummaryDetailId.HasValue)
                {
                    throw new ValidationException("备件编码{0}、来源库位{1}、序列号{2}库存数小于调拨数量"
                        .L10nFormat(item.SparePartCode, item.StorageLocation.Code, item.StoreSummaryDetail.OrderNumberCode));
                }
                throw new ValidationException("备件编码{0}、来源库位{1}库存数小于调拨数量"
                        .L10nFormat(item.SparePartCode, item.StorageLocation.Code));
            }

            var updateDemandItem = viceTransferSparePartList.FirstOrDefault(m => m.Id == item.ViceTransferSparePartId);
            if (updateDemandItem != null)
            {
                item.ControlMethod = updateDemandItem.ControlMethod;
                updateDemandItem.TransferQty += item.TransferQty;
                if (updateDemandItem.TransferQty > updateDemandItem.Qty)
                {
                    throw new ValidationException("调拨数量大于需求数量".L10N());
                }
                RF.Save(updateDemandItem);
            }
            var sameItemDetail = GetSameSparePartDetail(item);
            if (sameItemDetail != null)
            {
                sameItemDetail.TransferQty += item.TransferQty;
                sameItemDetail.PersistenceStatus = PersistenceStatus.Modified;
                toSaveViceTransferSparePartDetails.Add(sameItemDetail);
            }
            else
            {
                item.PersistenceStatus = PersistenceStatus.New;
                toSaveViceTransferSparePartDetails.Add(item);
            }

            return updateDemandItem;
        }

        /// <summary>
        /// 更新序列号库存信息
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updateDemandItem"></param>
        private void UpdateSNStroageInfo(ViceTransferSparePartDetail item, ViceTransferSparePart updateDemandItem)
        {
            var storeSummaryDetailLocationInfo = Query<StoreSummaryDetail>().Join<StoreSummary>((x, y) => x.StoreSummaryId == y.Id
                                            && y.SparePartId == updateDemandItem.SparePartId)
                                           .Where(m => m.WarehouseId == item.WarehouseId && m.StorageLocationId == item.StorageLocationId && m.OrderNumberCode == item.StoreSummaryDetail.OrderNumberCode
                                           ).FirstOrDefault();
            if (storeSummaryDetailLocationInfo != null)
            {
                storeSummaryDetailLocationInfo.WarehouseId = item.TargetWarehouseId;
                storeSummaryDetailLocationInfo.StorageLocationId = item.TargetId;
                RF.Save(storeSummaryDetailLocationInfo);
            }
            else
            {
                throw new ValidationException("未能找到备件编码{0}、来源库位{1}、序列号{2}的库存信息"
                    .L10nFormat(item.SparePartCode, item.StorageLocation.Code, item.StoreSummaryDetail.OrderNumberCode));
            }
        }

        /// <summary>
        /// 更新备件批次号库存信息
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updateDemandItem"></param>
        private void UpdateStoreSummaryLotInfo(ViceTransferSparePartDetail item, ViceTransferSparePart updateDemandItem)
        {
            var storeSourceSummaryLocationInfo = Query<StoreSummaryLot>().Join<StoreSummary>((x, y) => x.StoreSummaryId == y.Id
                                 && y.SparePartId == updateDemandItem.SparePartId)
                                .Where(m => m.WarehouseId == item.WarehouseId && m.StorageLocationId == item.StorageLocationId && m.BatchNumber == item.StoreSummaryLot.BatchNumber
                                ).FirstOrDefault();
            if (storeSourceSummaryLocationInfo != null)
            {
                storeSourceSummaryLocationInfo.GoodNumber -= item.QualityStatus == QualityStatus.Good ? (int)item.TransferQty : 0;
                storeSourceSummaryLocationInfo.RotNumber -= item.QualityStatus == QualityStatus.Defective ? (int)item.TransferQty : 0;
                storeSourceSummaryLocationInfo.SumNumber -= (int)item.TransferQty;

                if (storeSourceSummaryLocationInfo.SumNumber < 0 || storeSourceSummaryLocationInfo.GoodNumber < 0 || storeSourceSummaryLocationInfo.RotNumber < 0)
                {
                    throw new ValidationException("备件编码{0}、来源库位{1}的库存不足".L10nFormat(item.SparePartCode, item.StorageLocation.Code));
                }
                RF.Save(storeSourceSummaryLocationInfo);
            }
            else
            {
                throw new ValidationException("未能找到备件编码{0}、来源库位{1}、批次号{2}的库存信息"
                    .L10nFormat(item.SparePartCode, item.StorageLocation.Code, item.StoreSummaryLot.BatchNumber));
            }

            var storeTargetSummaryLocationInfo = Query<StoreSummaryLot>()
                .Join<StoreSummary>((x, y) => x.StoreSummaryId == y.Id && y.SparePartId == updateDemandItem.SparePartId)
                .Where(m => m.WarehouseId == item.TargetWarehouseId && m.StorageLocationId == item.TargetId
                && m.BatchNumber == item.StoreSummaryLot.BatchNumber).FirstOrDefault();
            if (storeTargetSummaryLocationInfo != null)
            {
                storeTargetSummaryLocationInfo.GoodNumber += item.QualityStatus == QualityStatus.Good ? (int)item.TransferQty : 0;
                storeTargetSummaryLocationInfo.RotNumber += item.QualityStatus == QualityStatus.Defective ? (int)item.TransferQty : 0;
                storeTargetSummaryLocationInfo.SumNumber += (int)item.TransferQty;
                RF.Save(storeTargetSummaryLocationInfo);
            }
            else//没有目标库位的数据时，插入一条数据
            {
                var DBSparePartStoreSummary = Query<StoreSummary>().Where(m => m.SparePartId == updateDemandItem.SparePartId).FirstOrDefault();
                if (DBSparePartStoreSummary == null)
                {
                    throw new ValidationException("备件编码{0}未存在有库存信息".L10nFormat(updateDemandItem.SparePartCode));
                }
                var saveStoreSummaryLot = new StoreSummaryLot()
                {
                    StoreSummaryId = DBSparePartStoreSummary.Id,
                    GoodNumber = item.QualityStatus == QualityStatus.Good ? (int)item.TransferQty : 0,
                    RotNumber = item.QualityStatus == QualityStatus.Defective ? (int)item.TransferQty : 0,
                    SumNumber = (int)item.TransferQty,
                    StorageLocationId = item.TargetId,
                    WarehouseId = item.TargetWarehouseId,
                    BatchNumber = item.StoreSummaryLot.BatchNumber,
                    InboundDate = RF.Find<StoreSummaryLot>().GetDbTime()
                };
                RF.Save(saveStoreSummaryLot);

            }
        }


        /// <summary>
        /// 更新编码类库存信息
        /// </summary>
        /// <param name="item"></param>
        /// <param name="updateDemandItem"></param>
        private void UpdateItemCodeStorageInfo(ViceTransferSparePartDetail item, ViceTransferSparePart updateDemandItem)
        {
            var storeSourceSummaryLocationInfo = Query<StoreSummaryLocation>().Join<StoreSummary>((x, y) => x.StoreSummaryId == y.Id
                     && y.SparePartId == updateDemandItem.SparePartId)
                    .Where(m => m.WarehouseId == item.WarehouseId && m.StorageLocationId == item.StorageLocationId).FirstOrDefault();
            if (storeSourceSummaryLocationInfo != null)
            {//更新来源库位的总库存减少；根据【质量状态】，更新不良品数或可以库存减少
                storeSourceSummaryLocationInfo.GoodNumber -= item.QualityStatus == QualityStatus.Good ? (int)item.TransferQty : 0;
                storeSourceSummaryLocationInfo.RotNumber -= item.QualityStatus == QualityStatus.Defective ? (int)item.TransferQty : 0;
                storeSourceSummaryLocationInfo.SumNumber -= (int)item.TransferQty;

                if (storeSourceSummaryLocationInfo.SumNumber < 0 || storeSourceSummaryLocationInfo.GoodNumber < 0 || storeSourceSummaryLocationInfo.RotNumber < 0)
                {
                    if (item.StoreSummaryLotId.HasValue)
                    {
                        throw new ValidationException("备件编码{0}、来源库位{1}、批次号{2}库存不足"
                            .L10nFormat(item.SparePartCode, item.StorageLocation.Code, item.StoreSummaryLot.BatchNumber));
                    }
                    if (item.StoreSummaryDetailId.HasValue)
                    {
                        throw new ValidationException("备件编码{0}、来源库位{1}、序列号{2}库存不足"
                            .L10nFormat(item.SparePartCode, item.StorageLocation.Code, item.StoreSummaryDetail.OrderNumberCode));
                    }
                }
                RF.Save(storeSourceSummaryLocationInfo);
            }
            else
            {
                throw new ValidationException("未能找到备件编码{0}、来源库位{1}的库存信息"
                    .L10nFormat(item.SparePartCode, item.StorageLocation.Code));
            }
            //更新目标库位的总库存增加；根据【质量状态】，更新不良品数或可以库存增加
            var storeTargetSummaryLocationInfo = Query<StoreSummaryLocation>().Join<StoreSummary>((x, y) => x.StoreSummaryId == y.Id
           && y.SparePartId == updateDemandItem.SparePartId)
           .Where(m => m.WarehouseId == item.TargetWarehouseId && m.StorageLocationId == item.TargetId).FirstOrDefault();
            if (storeTargetSummaryLocationInfo != null)
            {
                storeTargetSummaryLocationInfo.GoodNumber += item.QualityStatus == QualityStatus.Good ? (int)item.TransferQty : 0;
                storeTargetSummaryLocationInfo.RotNumber += item.QualityStatus == QualityStatus.Defective ? (int)item.TransferQty : 0;
                storeTargetSummaryLocationInfo.SumNumber += (int)item.TransferQty;
                RF.Save(storeTargetSummaryLocationInfo);
            }
            else//没有目标库位的数据时，插入一条数据
            {
                var DBSparePartStoreSummary = Query<StoreSummary>().Where(m => m.SparePartId == updateDemandItem.SparePartId).FirstOrDefault();
                if (DBSparePartStoreSummary == null)
                {
                    throw new ValidationException("备件编码{0}未存在有库存信息".L10nFormat(updateDemandItem.SparePartCode));
                }
                var saveStoreSummaryLocation = new StoreSummaryLocation()
                {
                    StoreSummaryId = DBSparePartStoreSummary.Id,
                    GoodNumber = item.QualityStatus == QualityStatus.Good ? (int)item.TransferQty : 0,
                    RotNumber = item.QualityStatus == QualityStatus.Defective ? (int)item.TransferQty : 0,
                    SumNumber = (int)item.TransferQty,
                    StorageLocationId = item.TargetId,
                    WarehouseId = item.TargetWarehouseId,
                };
                RF.Save(saveStoreSummaryLocation);

            }
        }

        /// <summary>
        /// 获取相同的工治具调拨明细
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private ViceTransferFixtureDetail GetSameFixtureTransferDetail(ViceTransferFixtureDetail item)
        {
            return Query<ViceTransferFixtureDetail>().Where(m => m.ViceTransferFixtureId == item.ViceTransferFixtureId &&
               m.FixtureIDAccountId == item.FixtureIDAccountId && m.ViceTransferFixture.FixtureQualityState == item.FixtureQualityState
               && m.StorageLocationId == item.StorageLocationId).FirstOrDefault();
        }

        /// <summary>
        /// 根据【需求行号+批次号+序列号+质量状态+来源库位】获取数据到已有的【调拨明细】
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private ViceTransferSparePartDetail GetSameSparePartDetail(ViceTransferSparePartDetail item)
        {
            //根据【需求行号+批次号+序列号+质量状态+来源库位】到已有的【调拨明细】获取数据，能获取到时，
            return Query<ViceTransferSparePartDetail>().Where(m => m.ViceTransferSparePartId == item.ViceTransferSparePartId &&
               m.StoreSummaryLotId == item.StoreSummaryLotId && m.StoreSummaryDetailId == item.StoreSummaryDetailId
               && m.QualityStatus == item.QualityStatus
               && m.StorageLocationId == item.StorageLocationId).FirstOrDefault();
        }
        #endregion


        /// <summary>
        /// 获取工治具库存数量
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public virtual int GetFixtureStorageLocationQty(ViceTransferFixtureDetail detail)
        {
            var stock = Query<FixtureAccountStock>().Where(m => m.FixtureAccount.FixtureEncode.Code == detail.FixtureEncodeCode
            && m.FixtureStorageLocationId == detail.StorageLocationId).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            return detail.FixtureQualityState == FixtureQualityState.Ng ? stock.Sum(m => m.NgQty) : stock.Sum(m => m.PassQty);
        }

        /// <summary>
        /// 获取编码类工治具编码台账的在线数
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public virtual int GetFixtureEncodeOnlineQty(ViceTransferFixtureDetail detail)
        {
            var result = Query<FixtureCodeAccount>().Where(m => m.Code == detail.FixtureEncodeCode).FirstOrDefault();
            if (result != null)
            {
                return result.OnlineQty;
            }
            return 0;
        }

        /// <summary>
        /// 获取工治具ID库位
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public virtual Tuple<double?, string, string, string> GetFixtureIDAccountLocationInfo(ViceTransferFixtureDetail detail)
        {
            //获取序列号所在的最新的工治具需求单，获取需求单的【车间+产线】

            var lastFixtureDemand = Query<FixtureDemand>().Join<FixtureUnload>((x, y) => x.Id == y.FixtureDemandId && y.FixtureAccountId == detail.FixtureIDAccountId).OrderBy(m => m.CreateDate)
                .FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            var stock = Query<FixtureAccountStock>().Where(m => m.FixtureAccountId == detail.FixtureIDAccountId).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());

            return new Tuple<double?, string, string, string>(stock?.FixtureStorageLocationId, stock?.LocationCode,
                lastFixtureDemand?.WorkShopName, lastFixtureDemand?.ResourceName);

        }

        /// <summary>
        /// 管控方式为【批次】时，根据【批次号+质量状态+来源库位】获取可用库存或不良品数 
        /// </summary>
        /// <param name="detail"></param>
        /// <returns></returns>
        public virtual int GetWarehouseLotQty(ViceTransferSparePartDetail detail)
        {
            var storeSummary = Query<StoreSummary>().Where(m => m.SparePart.SparePartCode == detail.SparePartCode).FirstOrDefault();
            if (storeSummary == null)
            {
                return 0;
            }

            if (detail.ControlMethod == ControlMethod.Batch)
            {
                if (!detail.StoreSummaryLotId.HasValue)
                {
                    throw new ValidationException("获取来源库位库存数失败，批次不能为空，批次管控备件必须填写选择批次".L10N());
                }
                var queryList = Query<StoreSummaryLot>().Where(p => p.StoreSummaryId == storeSummary.Id && p.WarehouseId == detail.WarehouseId &&
                p.BatchNumber == detail.StoreSummaryLot.BatchNumber && p.StorageLocationId == detail.StorageLocationId).ToList();
                if (queryList.Any())
                {
                    return detail.QualityStatus == QualityStatus.Defective ? queryList.Sum(p => p.RotNumber) : queryList.Sum(p => p.GoodNumber);
                }
            }
            if (detail.ControlMethod == ControlMethod.ItemCode)
            {
                var summaryLocationList = Query<StoreSummaryLocation>().Where(p => p.StoreSummaryId == storeSummary.Id && p.WarehouseId == detail.WarehouseId
                && p.StorageLocationId == detail.StorageLocationId).ToList();
                if (summaryLocationList.Any())
                {
                    return detail.QualityStatus == QualityStatus.Defective ? summaryLocationList.Sum(p => p.RotNumber) : summaryLocationList.Sum(p => p.GoodNumber);
                }
            }
            if (detail.ControlMethod == ControlMethod.Sn)
            {
                if (!detail.StoreSummaryDetailId.HasValue)
                {
                    throw new ValidationException("获取来源库位库存数失败，序列号不能为空，序列号管控备件必须填写序列号".L10N());
                }
                var summaryDetailList = Query<StoreSummaryDetail>().Where(p => p.StoreSummaryId == storeSummary.Id && p.WarehouseId == detail.WarehouseId &&
                  p.OrderNumberCode == detail.StoreSummaryDetail.OrderNumberCode && p.StorageLocationId == detail.StorageLocationId).ToList();
                if (summaryDetailList.Any())
                {
                    return detail.QualityStatus == QualityStatus.Defective ? summaryDetailList.Sum(p => p.RotNumber) : summaryDetailList.Sum(p => p.GoodNumber);
                }

            }
            return 0;
        }

        /// <summary>
        /// 可选来源仓库下库存状态为【入库】且质量状态符合的序列号
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryDetail> GetStoreSummaryDetailList(ViceTransferSparePartDetail detail, string keyword, PagingInfo pagingInfo)
        {
            return Query<StoreSummaryDetail>()
                   .Where(m => m.WarehouseId == detail.WarehouseId && m.StoreStatus == OrdNumStoreStatus.In && m.StoreSummary.SparePart.SparePartCode == detail.SparePartCode)
                   .WhereIf(detail.QualityStatus == QualityStatus.Defective, m => m.OdNbStatus == OdNbStatus.NoGoodProduct)
                     .WhereIf(detail.QualityStatus == QualityStatus.Good, m => m.OdNbStatus == OdNbStatus.GoodProduct)
                  .WhereIf(!keyword.IsNullOrEmpty(), m => m.OrderNumberCode.Contains(keyword))
                  .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 选择来源仓库有库存的批次号
        /// </summary>
        /// <param name="detail"></param>
        /// <param name="keyword"></param>
        /// <param name="pagingInfo"></param>
        /// <returns></returns>
        public virtual EntityList<StoreSummaryLot> GetStoreSummaryLotList(ViceTransferSparePartDetail detail, string keyword, PagingInfo pagingInfo)
        {
            return Query<StoreSummaryLot>()
                   .Where(m => m.WarehouseId == detail.WarehouseId && m.StoreSummary.SparePart.SparePartCode == detail.SparePartCode)
                   .WhereIf(detail.QualityStatus == QualityStatus.Defective, m => m.RotNumber > 0)
                   .WhereIf(detail.QualityStatus == QualityStatus.Good, m => m.GoodNumber > 0)
                  .WhereIf(!keyword.IsNullOrEmpty(), m => m.BatchNumber.Contains(keyword))
                  .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取备件调拨明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual EntityList<ViceTransferSparePartDetail> GetViceTransferSparePartDetails(double id)
        {
            var res = new EntityList<ViceTransferSparePartDetail>();
            var result = new List<ViceTransferSparePartDetail>();
            var parentData = RF.GetById<ViceTransfer>(id, new EagerLoadOptions().LoadWith(ViceTransfer.ViceTransferFixtureListProperty));
            if (parentData == null)
            {
                return res;
            }
            var viceTransferSparePartList = Query<ViceTransferSparePart>().Where(m => m.ViceTransferId == parentData.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (!viceTransferSparePartList.Any())
            {
                return res;
            }
            //备件的管控方式为【物料编码】和【批次】的数据展示一条，管控方式为【序列号】的数据展示多条（需求数量 - 调拨数量）
            viceTransferSparePartList.ForEach(item =>
            {
                //获取申请清单中调拨数量小于需求数量的数据
                if (item.TransferQty < item.Qty)
                {
                    //备件的管控方式为【物料编码】和【批次】的数据展示一条，管控方式为【序列号】的数据展示多条（需求数量 - 调拨数量）
                    if (item.ControlMethod == SpareParts.Enums.ControlMethod.Batch || item.ControlMethod == SpareParts.Enums.ControlMethod.ItemCode)
                    {
                        result.Add(GetSparePartDetail(item));
                    }
                    if (item.ControlMethod == SpareParts.Enums.ControlMethod.Sn)
                    {
                        for (int i = 0; i < item.Qty - item.TransferQty; i++)
                        {
                            result.Add(GetSparePartDetail(item));
                        }
                    }
                }

            });
            if (result.Any())
            {
                res.AddRange(result.OrderBy(m => m.LineNo).ToList());
            }
            return res;
        }

        /// <summary>
        /// 获取备件调拨明细
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private ViceTransferSparePartDetail GetSparePartDetail(ViceTransferSparePart item)
        {
            if (item == null)
            {
                return null;
            }
            var result = new ViceTransferSparePartDetail()
            {
                ControlMethod = item.ControlMethod,
                ViceTransferSparePartId = item.Id,
                SparePartCode = item.SparePartCode,
                LineNo = item.LineNo,
                QualityStatus = item.QualityStatus,
                SparePartName = item.SparePartName,
                SparePartType = item.SparePartType,
                Specification = item.Specification,
                TransferQty = item.ControlMethod == ControlMethod.Sn ? 1 : 0,//管控方式为【序列号】时，不能编辑，默认为1
                UnitName = item.UnitName,
                ViceTransferId = item.ViceTransferId,
                WarehouseId = item.WarehouseId,
                TargetWarehouseId = item.TargetWarehouseId,
                SourceInventoryQty = item.ControlMethod == ControlMethod.Sn ? 1 : 0,//管控方式为【序列号】时，不能编辑，默认为1
            };
            //管控方式为其他时，默认为来源仓库下编码为【STAGE】的库位，下拉选择来源仓库下的库位
            if (item.ControlMethod != SpareParts.Enums.ControlMethod.Sn)
            {
                result.StorageLocation = RT.Service.Resolve<WarehouseController>().GetStageStorageLocation(item.WarehouseId);
            }
            //管控方式为【物料编码】时，根据【备件编码 + 质量状态 + 来源库位】获取可用库存或不良品数
            if (item.ControlMethod == ControlMethod.ItemCode)
            {
                var wh = RF.GetById<Warehouse>(item.WarehouseId);
                result.SourceInventoryQty = (int)GetSparePartQty(item.SparePartId, wh.Code, item.QualityStatus);
            }
            result.Target = RT.Service.Resolve<WarehouseController>().GetStageStorageLocation(item.TargetWarehouseId);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private ViceTransferFixtureDetail GetViceTransferFixtureDetail(ViceTransferFixture item)
        {
            var result = new ViceTransferFixtureDetail()
            {
                ManageMode = item.ManageMode,
                ViceTransferFixtureId = item.Id,
                ModelCode = item.ModelCode,
                ModelName = item.ModelName,
                FixtureEncodeCode = item.FixtureEncodeCode,
                LineNo = item.LineNo,
                FixtureQualityState = item.FixtureQualityState,
                FixtureTypeCode = item.FixtureTypeCode,
                UintName = item.UintName,
                TransferQty = item.ManageMode == Fixtures.ManageMode.Number ? 1 : 0,
                ViceTransferId = item.ViceTransferId,
                TargetWarehouseId = item.TargetWarehouseId,
                WarehouseId = item.WarehouseId,
                FixtureStatus = FixtureStatus.InStorage,
            };


            return result;
        }

        /// <summary>
        /// 获取工治具执行明细
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual EntityList<ViceTransferFixtureDetail> GetViceTransferFixtureDetails(double id)
        {
            var res = new EntityList<ViceTransferFixtureDetail>();
            var result = new List<ViceTransferFixtureDetail>();
            var parentData = RF.GetById<ViceTransfer>(id, new EagerLoadOptions().LoadWith(ViceTransfer.ViceTransferFixtureListProperty));
            if (parentData == null)
            {
                return res;
            }
            var viceTransferFixtureList = Query<ViceTransferFixture>().Where(m => m.ViceTransferId == parentData.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            if (!viceTransferFixtureList.Any())
            {
                return res;
            }
            viceTransferFixtureList.ForEach(item =>
            {

                //获取申请清单中调拨数量小于需求数量的数据
                if (item.TransferQty < item.Qty)
                {
                    var location = RT.Service.Resolve<WarehouseController>().GetStageStorageLocation(item.TargetWarehouseId);
                    //工治具的管理方式为【编码管控】的数据展示一条，管控方式为【ID】的数据展示多条（需求数量 - 调拨数量）
                    if (item.ManageMode == Fixtures.ManageMode.Code)
                    {
                        var resultItem = GetViceTransferFixtureDetail(item);
                        resultItem.Target = location;
                        result.Add(resultItem);
                    }
                    if (item.ManageMode == Fixtures.ManageMode.Number)
                    {

                        for (int i = 0; i < item.Qty - item.TransferQty; i++)
                        {
                            var resultDetail = GetViceTransferFixtureDetail(item);
                            resultDetail.OnlineQty = 0;
                            resultDetail.Target = location;
                            resultDetail.SourceInventoryQty = 1;
                            result.Add(resultDetail);
                        }

                    }
                }

            });
            if (result.Any())
            {
                res.AddRange(result.OrderBy(m => m.LineNo).ToList());
            }
            return res;
        }

        /// <summary>
        /// 获取备件库存数量
        /// </summary>
        /// <param name="sparePartId"></param>
        /// <param name="warehouseCode"></param>
        /// <param name="qualityStatus"></param>
        /// <returns></returns>
        public virtual decimal GetSparePartQty(double sparePartId, string warehouseCode, QualityStatus qualityStatus)
        {
            var storeSummary = Query<StoreSummary>().Where(m => m.SparePartId == sparePartId).FirstOrDefault();
            if (storeSummary == null)
            {
                return 0;
            }

            var storeSummaryWarehouse = RT.Service.Resolve<SparePartController>().GetStoreSummaryWarehouseList(null, null, storeSummary);
            if (storeSummaryWarehouse.Any())
            {
                return qualityStatus == QualityStatus.Defective ? storeSummaryWarehouse.Where(m => m.WarehouseCode == warehouseCode).Sum(P => P.RotNumber)
                    : storeSummaryWarehouse.Where(m => m.WarehouseCode == warehouseCode).Sum(P => P.GoodNumber);
            }
            return 0;

        }

        /// <summary>
        /// 获取工治具的在线数、在库数
        /// </summary>
        /// <param name="fixtureEncodeId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="fixtureQualityState"></param>
        /// <returns></returns>
        public virtual Tuple<decimal, decimal> GetFixtureEncodeQty(double fixtureEncodeId, double warehouseId, FixtureQualityState fixtureQualityState)
        {
            var fixEndcode = RF.GetById<FixtureEncode>(fixtureEncodeId, new EagerLoadOptions().LoadWithViewProperty());
            if (fixEndcode == null)
            {
                return new Tuple<decimal, decimal>(0, 0);
            }
            if (fixEndcode.ManageMode == Fixtures.ManageMode.Number)
            {
                var IDTotalQty = Query<FixtureAccountStock>()
                       .Where(p => p.FixtureWarehouseId == warehouseId && p.FixtureAccount.QualityState == fixtureQualityState
                       && p.FixtureAccount.AccountState == FixtureAccountState.InStorage
                       && p.FixtureAccount.FixtureEncodeId == fixtureEncodeId)
                       .Select(p => new
                       {
                           CanUseNum = p.TotalQty.SUM()//总数量
                       }).FirstOrDefault<decimal>();

                var IDOnlineQty = Query<FixtureAccount>()
                       .Where(p => p.QualityState == fixtureQualityState
                       && p.AccountState == FixtureAccountState.Online
                       && p.FixtureEncodeId == fixtureEncodeId)
                       .Select(p => new
                       {
                           CanUseNum = p.TotalQty.SUM()//总数量
                       }).FirstOrDefault<decimal>();

                return new Tuple<decimal, decimal>(IDTotalQty, IDOnlineQty);
            }

            if (fixEndcode.ManageMode == Fixtures.ManageMode.Code)
            {
                //管控方式为【编码】时，汇总工治具编码台账中该工治具编码的库存详情中主表仓库的合格数或不合格数（根据质量状态）
                var codeEncodeQty = Query<FixtureAccountStock>()
                    .Where(p => p.FixtureWarehouseId == warehouseId && p.FixtureAccount.FixtureEncodeId == fixtureEncodeId)
                    .Select(p => new
                    {
                        ScrapNum = p.NgQty.SUM(),//不合格数量
                        CanUseNum = p.PassQty.SUM()//合格数量
                    }).FirstOrDefault<FixtureEncode>();

                //管控方式为【编码】时，获取工治具编码台账中该工治具编码的【在线】字段
                var codeOnlieQty = Query<FixtureAccountStock>()
                   .Where(p => p.FixtureWarehouseId == warehouseId && p.FixtureAccount.FixtureEncodeId == fixtureEncodeId)
                   .Select(p => p.FixtureAccount.OnlineQty.SUM()).FirstOrDefault<decimal>();
                return new Tuple<decimal, decimal>(fixtureQualityState == FixtureQualityState.Ng ? codeEncodeQty.ScrapNum : codeEncodeQty.CanUseNum, codeOnlieQty);
            }
            return new Tuple<decimal, decimal>(0, 0);

        }

        /// <summary>
        /// 关闭
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="remark"></param>
        public virtual void Shutdown(List<double> selectedIds, string remark)
        {
            var demands = GetListViceTransferByIds(selectedIds);
            if (demands.Any(p => (p.ApprovalStatus != ApprovalStatus.Audited
            && (p.TransferStatus == TransferStatus.Close || p.TransferStatus == TransferStatus.Done))))
                throw new ValidationException("调拨状态为【未调拨】或【调拨中】且审核通过的数据才允许关闭！".L10N());
            demands.ForEach(item =>
            {
                item.TransferStatus = item.TransferStatus == TransferStatus.Partial ? TransferStatus.Done : TransferStatus.Close;
                item.CloseRemark = remark;
            });
            RF.Save(demands);
        }

        /// <summary>
        /// 撤回调拨
        /// </summary>
        /// <param name="selectedIds"></param>

        public virtual void CancelViceTransfers(List<double> selectedIds)
        {
            var transfers = GetListViceTransferByIds(selectedIds);
            if (transfers.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能操作".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                List<double> transfersIds = new List<double>();
                transfers.ForEach(p =>
                {
                    p.ApprovalStatus = ApprovalStatus.Draft;
                    transfersIds.Add(p.Id);
                });
                RF.Save(transfers);
                var now = RF.Find<ViceTransfer>().GetDbTime();
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(transfersIds, typeof(ViceTransfer).FullName, ApprovalResult.Retract, now, "");
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核副资产调拨
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="value"></param>
        /// <param name="remark"></param>
        public virtual void Approval(List<double> selectedIds, ApprovalResult value, string remark)
        {
            if (!selectedIds.Any())
            {
                throw new ValidationException("传参异常".L10N());
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                ApprovalInner(selectedIds, value, remark);
                trans.Complete();
            }
        }

        /// <summary>
        /// 审核副资产调拨
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <param name="value"></param>
        /// <param name="remark"></param>
        /// <param name="assetTransferList"></param>
        public virtual void ApprovalInner(List<double> selectedIds, ApprovalResult value, string remark, EntityList<ViceTransfer> assetTransferList = null)
        {
            if (assetTransferList == null)
            {
                assetTransferList = GetListViceTransferByIds(selectedIds);
                if (!assetTransferList.Any())
                {
                    throw new ValidationException("审核数据异常".L10N());
                }
            }
            //验证只有执行中的数据才能审核
            if (assetTransferList.Any(p => p.ApprovalStatus != ApprovalStatus.PendingReview))
            {
                throw new ValidationException("只有状态为【待审核】的数据才能审核".L10N());
            }
            var warehouseIds = assetTransferList.Select(m => m.WarehouseId).ToList();
            //校验用户有来源仓库的权限，否则报错：用户没有来源仓库权限，不能审核
            var whList = RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmployee(null, "");
            if (!whList.Any())
            {
                throw new ValidationException("用户没有来源仓库权限，不能审核".L10N());
            }
            foreach (var whId in warehouseIds)
            {
                if (whList.FirstOrDefault(m => m.Id == whId) == null)
                {
                    throw new ValidationException("用户没有来源仓库权限，不能审核".L10N());
                }
            }
            var status = value == ApprovalResult.Pass ? ApprovalStatus.Audited : ApprovalStatus.Reject;
            var now = RF.Find<ViceTransfer>().GetDbTime();
            var ids = new List<double>();
            assetTransferList.ForEach(item =>
            {
                item.ApprovalStatus = status;
                ids.Add(item.Id);
            });

            //保存成功之后添加审核记录
            RF.Save(assetTransferList);
            RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(ids, typeof(ViceTransfer).FullName, value, now, remark);
        }

        /// <summary>
        /// 提交副资产调拨
        /// </summary>
        /// <param name="selectedIds"></param>
        public virtual void Sumbit(List<double> selectedIds)
        {
            var approvalConfig = GetApprovalConfigValue();
            var now = RF.Find<ViceTransfer>().GetDbTime();
            var assetTransfers = GetListViceTransferByIds(selectedIds);
            if (assetTransfers.Any(p => p.ApprovalStatus != ApprovalStatus.Draft && p.ApprovalStatus != ApprovalStatus.Reject))
            {
                throw new ValidationException("只有状态为【待提交】、【驳回】的数据才能提交".L10N());
            }
            var recordIds = new List<double>();
            foreach (var item in assetTransfers)
            {
                item.ApprovalStatus = ApprovalStatus.PendingReview;
                recordIds.Add(item.Id);
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(assetTransfers);
                RT.Service.Resolve<WorkFlowRecordController>().CreateWorkFlowRecords(recordIds, typeof(ViceTransfer).FullName, ApprovalResult.Submit, now, "");
                if (approvalConfig != null && !approvalConfig.EnableAudit)
                {
                    ApprovalInner(selectedIds, ApprovalResult.Pass, "通过!".L10N(), assetTransfers);
                }
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存副资产调拨
        /// </summary>
        /// <param name="model"></param>
        public virtual void SaveViceTransfer(ViceTransfer model)
        {
            var whList = RT.Service.Resolve<WarehouseController>().GetAllWarehouseByEmployeeForEdo(null, "");
            if (!whList.Any(m => m.Id == model.WarehouseId) && !whList.Any(m => m.Id == model.TargetWarehouseId))
            {
                throw new ValidationException("当前用户没有来源仓库和目标仓库的权限，请修改".L10N());
            }
            if (model.TargetWarehouseId == model.WarehouseId)
            {
                throw new ValidationException("来源仓库和目标仓库不能相同，请修改".L10N());
            }

            RF.Save(model);
        }

        /// <summary>
        /// 获取新的副资产调拨
        /// </summary>
        /// <returns></returns>
        public virtual ViceTransfer GetNewViceTransfers()
        {
            var entity = new ViceTransfer();
            entity.TransferNo = RT.Service.Resolve<CommonController>().GetNo<ViceTransfer>("资产调拨");
            entity.TransferStatus = TransferStatus.NotYet;
            entity.ApprovalStatus = ApprovalStatus.Draft;
            entity.ApplyDate = DateTime.Now;
            entity.ApplicantId = RT.IdentityId;
            entity.ApplicantName = RF.GetById<Employee>(RT.IdentityId).Name;
            return entity;
        }

        /// <summary>
        /// 根据Ids获取副资产调拨
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public virtual EntityList<ViceTransfer> GetListViceTransferByIds(IList<double> idList)
        {
            return idList.SplitContains((ids) =>
            {
                return Query<ViceTransfer>().Where(p => ids.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });

        }

        /// <summary>
        /// 获取流程配置项
        /// </summary>
        /// <returns></returns>
        public virtual ApprovalConfigValue GetApprovalConfigValue()
        {
            var config = ConfigService.GetConfig(new ApprovalConfig(), typeof(ViceTransfer));
            if (config == null)
            {
                throw new ValidationException("未找到审批流程配置,请检查规则配置".L10N());
            }
            return config;
        }

    }
}
