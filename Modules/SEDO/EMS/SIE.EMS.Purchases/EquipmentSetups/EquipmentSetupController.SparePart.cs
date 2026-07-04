using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Purchases.EquipmentSetups.Configs;
using SIE.EMS.Purchases.EquipmentSetups.ViewModels;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Enums;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.SpareParts.OutDepots.Details;
using SIE.EMS.SpareParts.OutDepots.Enums;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.EquipmentSetups
{
    /// <summary>
    /// 安装调试控制器-备件使用
    /// </summary>
    public partial class EquipmentSetupController : DomainController
    {
        /// <summary>
        /// 获取备件使用是否必须关联出库单
        /// </summary>
        /// <returns>备件使用是否必须关联出库单</returns>
        public virtual bool GetRelationOutDepot()
        {
            var config = ConfigService.GetConfig(new RelationOutDepotConfig(), typeof(EquipmentSetup));
            if (config == null)
            {
                throw new ValidationException("未找到备件使用是否必须关联出库单配置,请检查规则配置".L10N());
            }
            return config.Relation;
        }

        /// <summary>
        /// 根据安装调试id列表获取安装调试备件使用
        /// </summary>
        /// <param name="setupIds">安装调试id列表</param>
        /// <returns>安装调试备件使用</returns>
        public virtual EntityList<SetupSparePart> GetSparePartsBySetupIds(List<double> setupIds)
        {
            return setupIds.SplitContains(ids => Query<SetupSparePart>().Where(p => ids.Contains(p.EquipmentSetupId)).ToList());
        }

        /// <summary>
        /// 获取安装调试备件使用
        /// </summary>
        /// <param name="setup">安装调试</param>
        /// <returns>安装调试备件使用</returns>
        public virtual EntityList<SetupSparePart> GetSetupSpareParts(EquipmentSetup setup)
        {
            var list = new EntityList<SetupSparePart>();
            //①关联的出库单的出库明细数据
            var outDepotIds = Query<OutDepot>().Where(p => p.SourceNo == setup.SetupNo && p.OutDepotType == OutDepotType.Setup).Select(p => p.Id).ToList<double>();
            var outDepotDetails = Query<PartOutDepotDetail>().Where(p => outDepotIds.Contains(p.OutDepotId)
            && p.OutboundStatus == OutboundStatus.Shipped && (p.OutDepotCount - p.UseCount - p.ReturnQty) > 0).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var outDepotDetail in outDepotDetails)
            {
                var setupSparePart = new SetupSparePart();
                setupSparePart.EquipmentSetupId = setup.Id;
                setupSparePart.UseQty = outDepotDetail.UseCount;
                setupSparePart.LotNo = outDepotDetail.BatchNo;
                setupSparePart.Sn = outDepotDetail.SeriaNo;
                setupSparePart.SparePartId = outDepotDetail.SparePartId;
                setupSparePart.SparePartName = outDepotDetail.SparePartName;
                setupSparePart.PartOutDepotDetailId = outDepotDetail.Id;
                setupSparePart.PartOutDepotDetailLineNo = outDepotDetail.LineNo;
                setupSparePart.OutDepotNo = outDepotDetail.OutDepotNoView;
                setupSparePart.UnitId = outDepotDetail.SparePartUnitId;
                setupSparePart.UnitName = outDepotDetail.SparePartUnitName;
                setupSparePart.SparePartCode = outDepotDetail.SparePartCode;
                setupSparePart.Specification = outDepotDetail.SpecificationView;
                setupSparePart.ControlMethod = outDepotDetail.ControlMethod;
                setupSparePart.OutDepotCount = outDepotDetail.OutDepotCount;
                setupSparePart.UseCount = outDepotDetail.UseCount;
                setupSparePart.ReturnQty = outDepotDetail.ReturnQty;
                setupSparePart.SurplusQty = outDepotDetail.OutDepotCount - outDepotDetail.UseCount - outDepotDetail.ReturnQty;
                setupSparePart.ExtValues["PartOutDepotDetailId_Display"] = outDepotDetail.LineNo;
                setupSparePart.ExtValues["SparePartId_Display"] = outDepotDetail.SparePartCode;
                setupSparePart.ExtValues["UnitId_Display"] = outDepotDetail.SparePartUnitName;
                setupSparePart.IsOutDepotInfo = true;
                list.Add(setupSparePart);
            }
            //②备件使用子表数据
            var spareParts = Query<SetupSparePart>().Where(p => p.EquipmentSetupId == setup.Id).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var sparePart in spareParts)
            {
                sparePart.SurplusQty = sparePart.OutDepotCount - sparePart.UseCount - sparePart.ReturnQty;
                list.Add(sparePart);
            }
            return list;
        }

        /// <summary>
        /// 获取【质量状态】为【良品】且【状态】不为【待出库】的出库单明细
        /// </summary>
        /// <param name="pagingInfo">分页</param>
        /// <param name="keyword">单号</param>
        /// <returns>出库单明细</returns>
        public virtual EntityList<PartOutDepotDetail> GetPartOutDepotDetails(PagingInfo pagingInfo, string keyword)
        {
            return Query<PartOutDepotDetail>().Exists<OutDepot>((a, b) => b.Where(p => p.Id == a.OutDepotId
                && p.QualityStatus == QualityStatus.Good && p.OutDepotState != OutDepotState.Ing)
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.No.Contains(keyword)))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据出库单获取备件编码
        /// </summary>
        /// <param name="outDepotDetailId">出库单</param>
        /// <param name="keyword">关键字</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>备件编码</returns>
        public virtual EntityList<SparePart> GetOutDepotSpareParts(double? outDepotDetailId, string keyword, PagingInfo pagingInfo)
        {
            var query = Query<SparePart>();
            if (!keyword.IsNullOrWhiteSpace())
            {
                query.Where(p => p.SparePartCode.Contains(keyword));
            }
            if (outDepotDetailId.HasValue)
            {
                query.Exists<PartOutDepotDetail>((a, b) => b.Where(p => p.SparePartId == a.Id && p.Id == outDepotDetailId.Value
                && p.OutboundStatus == OutboundStatus.Shipped && (p.OutDepotCount - p.UseCount - p.ReturnQty) > 0));
            }
            return query.ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取批次号信息
        /// </summary>
        /// <param name="outDepotDetailId">出库单</param>
        /// <param name="sparePartId">备件</param>
        /// <param name="keyword">条码</param>
        /// <returns>批次号</returns>
        public virtual EntityList<SetupLotSnInfo> GetLotInfos(double? outDepotDetailId, double sparePartId, string keyword)
        {
            var list = new EntityList<SetupLotSnInfo>();
            var sns = new List<string>();
            //出库单选择时，可选出库单的出库明细中备件编码符合且【使用数量+退回数】小于【出库数量】的批次
            if (outDepotDetailId != null)
            {
                var lots = Query<StoreSummaryLot>().Exists<PartOutDepotDetail>((a, b) => b.Where(p => p.BatchNoRefId == a.Id
                && p.Id == outDepotDetailId.Value && p.SparePartId == sparePartId && p.UseCount + p.ReturnQty < p.OutDepotCount))
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.BatchNumber.Contains(keyword))
                .Select(p => p.BatchNumber).Distinct().ToList<string>();
                sns.AddRange(lots);
            }
            else
            {
                //出库单未选择时，可选备件编码符合的批次（不限制状态，可用数量等）
                var lots = Query<StoreSummaryLot>().Exists<StoreSummary>((a, b) => b.Where(p => p.Id == a.StoreSummaryId && p.SparePartId == sparePartId))
                    .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.BatchNumber.Contains(keyword))
                    .Select(p => p.BatchNumber).Distinct().ToList<string>();
                sns.AddRange(lots);
            }
            foreach (var sn in sns)
            {
                if (sn.IsNullOrWhiteSpace())
                {
                    continue;
                }
                var info = new SetupLotSnInfo();
                info.Id = sn;
                info.Value = sn;
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 获取序列号信息
        /// </summary>
        /// <param name="outDepotDetailId">出库单</param>
        /// <param name="sparePartId">备件</param>
        /// <param name="keyword">条码</param>
        /// <returns>序列号</returns>
        public virtual EntityList<SetupLotSnInfo> GetSnInfos(double? outDepotDetailId, double sparePartId, string keyword)
        {
            var list = new EntityList<SetupLotSnInfo>();
            var sns = new List<StoreSummaryDetail>();
            //出库单选择时，可选出库单的出库明细中备件编码符合且【使用数量+退回数】小于【出库数量】的的序列号
            if (outDepotDetailId != null)
            {
                var codes = Query<StoreSummaryDetail>().Exists<PartOutDepotDetail>((a, b) => b.Where(p => p.SeriaNoRefId == a.Id
                && p.Id == outDepotDetailId.Value && p.SparePartId == sparePartId && p.UseCount + p.ReturnQty < p.OutDepotCount))
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.OrderNumberCode.Contains(keyword)).ToList();
                sns.AddRange(codes);
            }
            else
            {
                //出库单未选择时，可选备件编码符合状态为【出库】的序列号
                var codes = Query<StoreSummaryDetail>().Exists<StoreSummary>((a, b) => b.Where(p => p.Id == a.StoreSummaryId && p.SparePartId == sparePartId
                && a.StoreStatus == OrdNumStoreStatus.Out))
                .WhereIf(!keyword.IsNullOrWhiteSpace(), p => p.OrderNumberCode.Contains(keyword)).ToList();
                sns.AddRange(codes);
            }
            foreach (var sn in sns)
            {
                if (sn.OrderNumberCode.IsNullOrWhiteSpace())
                {
                    continue;
                }
                var info = new SetupLotSnInfo();
                info.Id = sn.OrderNumberCode;
                info.Value = sn.OrderNumberCode;
                info.Qty = 1;
                list.Add(info);
            }
            return list;
        }

        /// <summary>
        /// 验证备件使用
        /// </summary>
        /// <param name="setupSparePartList">备件使用信息</param>
        /// <param name="relation">是否备件使用必须关联出库单</param>
        /// <param name="newSetupSparePart">新增的备件使用</param>
        private void CheckSetupSparePartList(EntityList<SetupSparePart> setupSparePartList, bool relation, List<SetupSparePart> newSetupSparePart)
        {
            var setupSpareParts = setupSparePartList.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged);
            if (relation && setupSpareParts.Any(p => p.PartOutDepotDetailId == null))
            {
                throw new ValidationException("备件使用必须关联出库单".L10N());
            }
            var sparePartIds = setupSparePartList.Where(p => p.SparePartId != null).Select(p => p.SparePartId).ToList();
            var spareParts = Query<SparePart>().Where(p => sparePartIds.Contains(p.Id)).ToList();
            var equipAccountIds = setupSparePartList.Where(p => p.EquipAccountId > 0).Select(p => p.EquipAccountId).ToList();

            var equipAccounts = equipAccountIds.SplitContains(tempIds =>
                {
                    return Query<EquipAccount>().Where(p => tempIds.Contains(p.Id)).ToList();
                });

            foreach (var setupSparePart in setupSpareParts)
            {
                if (setupSparePart.PartOutDepotDetailId.HasValue && setupSparePart.SparePartId == null)
                {
                    throw new ValidationException("出库单不为空时：备件编码必输".L10N());
                }
                if (setupSparePart.SparePartId == null)
                {
                    if (setupSparePart.SparePartName.IsNullOrWhiteSpace())
                    {
                        throw new ValidationException("备件编码为空时，备件名称必输".L10N());
                    }
                    if (!setupSparePart.LotNo.IsNullOrWhiteSpace() || !setupSparePart.Sn.IsNullOrWhiteSpace())
                    {
                        throw new ValidationException("备件编码为空时，批次号和序列号只能为空".L10N());
                    }
                }
                if (setupSparePart.EquipAccountId > 0 && setupSparePart.SparePartId != null)
                {
                    var sparePart = spareParts.FirstOrDefault(p => p.Id == setupSparePart.SparePartId);
                    if (sparePart == null)
                    {
                        throw new ValidationException("数据异常，找不到备件数据".L10N());
                    }
                    var equipAccount = equipAccounts.FirstOrDefault(p => p.Id == setupSparePart.EquipAccountId);
                    if (equipAccount == null)
                    {
                        throw new ValidationException("数据异常，找不到设备数据".L10N());
                    }
                    //当备件是专用备件时，校验设备的类型和型号是否符合
                    if (sparePart.SpartType == SparePartType.Special && sparePart.SpartEquipModelId != equipAccount.EquipModelId)
                    {
                        throw new ValidationException("备件{0}为专用备件,不符合设备{1}".L10nFormat(sparePart.SparePartCode, equipAccount.Code));
                    }
                }
                //手动新增的数据保存至备件使用表
                if (setupSparePart.PersistenceStatus == PersistenceStatus.New && setupSparePart.PartOutDepotDetailId.HasValue)
                {
                    newSetupSparePart.Add(setupSparePart);
                }
            }
            var outDepotInfos = setupSpareParts.Where(p => p.IsOutDepotInfo && p.Id == 0).ToList();
            foreach (var outDepotInfo in outDepotInfos)
            {
                //设备编码和使用数量都未填写的数据不执行保存
                if (outDepotInfo.EquipAccountId == 0 && outDepotInfo.UseQty <= 0)
                {
                    outDepotInfo.PersistenceStatus = PersistenceStatus.Unchanged;
                    continue;
                }
                //设备编码和使用数量只填写一个时报错
                if (outDepotInfo.EquipAccountId == 0 || outDepotInfo.UseQty <= 0)
                {
                    throw new ValidationException("请填写设备编码和使用数量".L10N());
                }
                //设备编码和使用数量都不为空时，数据保存至备件使用子表
                outDepotInfo.PersistenceStatus = PersistenceStatus.New;
                newSetupSparePart.Add(outDepotInfo);
            }
        }

        /// <summary>
        /// 更新出库明细的【使用数量】为原来的值加上本条数据的【使用数量】
        /// </summary>
        /// <param name="newSetupSparePart">新增的备件使用</param>
        private void UpdateAddUseCount(List<SetupSparePart> newSetupSparePart)
        {
            if (newSetupSparePart.Any())
            {
                var outDepotDetailIds = newSetupSparePart.Select(p => p.PartOutDepotDetailId).ToList();
                var outDepotDetails = Query<PartOutDepotDetail>().Where(p => outDepotDetailIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                foreach (var item in newSetupSparePart)
                {
                    var outDepotDetail = outDepotDetails.FirstOrDefault(p => p.Id == item.PartOutDepotDetailId);
                    if (outDepotDetail == null)
                    {
                        throw new ValidationException("数据异常，找不到出库单明细".L10N());
                    }
                    outDepotDetail.UseCount += item.UseQty;
                    //校验不能大于【拣货数-退货数】
                    if (outDepotDetail.UseCount > outDepotDetail.OutDepotCount - outDepotDetail.ReturnQty)
                    {
                        var str = "出库单{0},备件{1}".L10nFormat(outDepotDetail.OutDepotNoView, outDepotDetail.SparePartCode);
                        if (!item.LotNo.IsNullOrWhiteSpace())
                        {
                            str += ",批次{0}".L10nFormat(item.LotNo);
                        }
                        if (!item.Sn.IsNullOrWhiteSpace())
                        {
                            str += ",序列号{0}".L10nFormat(item.Sn);
                        }
                        throw new ValidationException(str + ",使用数量大于可使用数量".L10N());
                    }
                }
                RF.Save(outDepotDetails);
            }
        }

        /// <summary>
        /// 更新出库明细的【使用数量】
        /// </summary>
        /// <param name="setupList">安装调试数据</param>
        /// <param name="oldSpareParts">原备件使用数据</param>
        private void UpdateUseCount(EntityList<EquipmentSetup> setupList, EntityList<SetupSparePart> oldSpareParts)
        {
            var editIds = new List<double>();
            foreach (var setup in setupList)
            {
                //已保存至备件使用子表的数据，修改时
                var sparePartList = setup.SetupSparePartList.Where(p => p.PersistenceStatus == PersistenceStatus.Modified);
                foreach (var newItem in sparePartList)
                {
                    var oldSparePart = oldSpareParts.FirstOrDefault(p => p.Id == newItem.Id);
                    if (oldSparePart == null)
                    {
                        throw new ValidationException("数据异常，找不到备件使用数据".L10N());
                    }
                    //如果出库明细修改了，则更新原出库明细的使用数量和更新新出库明细的使用数量；
                    if (oldSparePart.PartOutDepotDetailId != newItem.PartOutDepotDetailId)
                    {
                        if (oldSparePart.PartOutDepotDetailId.HasValue)
                        {
                            DB.Update<PartOutDepotDetail>().Set(p => p.UseCount, p => p.UseCount - oldSparePart.UseQty).Where(p => p.Id == oldSparePart.PartOutDepotDetailId).Execute();
                            editIds.Add(oldSparePart.PartOutDepotDetailId.Value);
                        }
                        if (newItem.PartOutDepotDetailId.HasValue)
                        {
                            DB.Update<PartOutDepotDetail>().Set(p => p.UseCount, p => p.UseCount + newItem.UseQty).Where(p => p.Id == newItem.PartOutDepotDetailId).Execute();
                            editIds.Add(newItem.PartOutDepotDetailId.Value);
                        }
                    }
                    else
                    {
                        //出库明细未修改，使用数量修改时，更新出库明细的使用数量（增加或减少差值）
                        if (newItem.PartOutDepotDetailId.HasValue)
                        {
                            var qty = oldSparePart.UseQty - newItem.UseQty;
                            if (qty < 0)
                            {
                                qty = -qty;
                                DB.Update<PartOutDepotDetail>().Set(p => p.UseCount, p => p.UseCount + qty).Where(p => p.Id == newItem.PartOutDepotDetailId).Execute();
                            }
                            else
                            {
                                DB.Update<PartOutDepotDetail>().Set(p => p.UseCount, p => p.UseCount - qty).Where(p => p.Id == newItem.PartOutDepotDetailId).Execute();
                            }
                            editIds.Add(newItem.PartOutDepotDetailId.Value);
                        }
                    }
                }
                //当出库单为空时，直接删除数据；出库单不为空时，额外更新对应出库明细的使用数量为【原来的值-删除数据的使用数量】
                foreach (var item in setup.SetupSparePartList.DeletedList)
                {
                    var deleted = item as SetupSparePart;
                    if (deleted != null && deleted.Id > 0 && deleted.PartOutDepotDetailId.HasValue)
                    {
                        var oldSparePart = oldSpareParts.FirstOrDefault(p => p.Id == deleted.Id);
                        if (oldSparePart == null)
                        {
                            throw new ValidationException("数据异常，找不到备件使用数据".L10N());
                        }
                        DB.Update<PartOutDepotDetail>().Set(p => p.UseCount, p => p.UseCount - oldSparePart.UseQty).Where(p => p.Id == oldSparePart.PartOutDepotDetailId).Execute();
                        editIds.Add(oldSparePart.PartOutDepotDetailId.Value);
                    }
                }
            }
            var outDepotDetails = Query<PartOutDepotDetail>().Where(p => editIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var outDepotDetail in outDepotDetails)
            {
                //校验不能大于【拣货数-退货数】
                if (outDepotDetail.UseCount > outDepotDetail.OutDepotCount - outDepotDetail.ReturnQty)
                {
                    var str = "出库单{0},备件{1}".L10nFormat(outDepotDetail.OutDepotNoView, outDepotDetail.SparePartCode);
                    if (!outDepotDetail.BatchNo.IsNullOrWhiteSpace())
                    {
                        str += ",批次{0}".L10nFormat(outDepotDetail.BatchNo);
                    }
                    if (!outDepotDetail.SeriaNo.IsNullOrWhiteSpace())
                    {
                        str += ",序列号{0}".L10nFormat(outDepotDetail.SeriaNo);
                    }
                    throw new ValidationException(str + ",使用数量大于可使用数量".L10N());
                }
            }
        }

        /// <summary>
        /// 根据备件+仓库查询可用库存数
        /// </summary>
        /// <param name="sparePartId">备件</param>
        /// <param name="warehouseId">仓库</param>
        /// <returns>可用库存数</returns>
        public virtual decimal GetWarehouseQty(double sparePartId, double warehouseId)
        {
            var storeSummary = Query<StoreSummary>().Where(p => p.SparePartId == sparePartId).FirstOrDefault();
            if (storeSummary == null)
            {
                return 0;
            }
            else
            {
                var sparePart = GetById<SparePart>(sparePartId);
                if (sparePart == null)
                {
                    throw new ValidationException("数据异常".L10N());
                }
                if (sparePart.ControlMethod == ControlMethod.ItemCode)
                {
                    var q = Query<StoreSummaryLocation>().Where(p => p.StoreSummaryId == storeSummary.Id && p.WarehouseId == warehouseId);
                    q.Select(p => new { GoodNumber = p.GoodNumber.SUM() });
                    return q.FirstOrDefault<decimal>();
                }
                else if (sparePart.ControlMethod == ControlMethod.Batch)
                {
                    var q = Query<StoreSummaryLot>().Where(p => p.StoreSummaryId == storeSummary.Id && p.WarehouseId == warehouseId);
                    q.Select(p => new { GoodNumber = p.GoodNumber.SUM() });
                    return q.FirstOrDefault<decimal>();
                }
                else
                {
                    var q = Query<StoreSummaryDetail>().Where(p => p.StoreSummaryId == storeSummary.Id && p.WarehouseId == warehouseId);
                    q.Select(p => new { GoodNumber = p.GoodNumber.SUM() });
                    return q.FirstOrDefault<decimal>();
                }
            }
        }
    }
}
