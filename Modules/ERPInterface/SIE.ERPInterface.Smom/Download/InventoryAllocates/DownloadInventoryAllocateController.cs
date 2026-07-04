using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.Inventory.Commom;
using SIE.Inventory.Transactions;
using SIE.Items;
using SIE.Warehouses;


namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 下载库存调拨控制器
    /// </summary>
    public class DownloadInventoryAllocateController : DomainController
    {
        /// <summary>
        /// 从API下载企业模型到业务表
        /// </summary>
        /// <param name="allocateDatas"></param>
        /// /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadAllocateToBusiness(List<InventoryAllocateData> allocateDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<InventoryAllocateData>(
                allocateDatas,
                p => this.SaveInventoryAllocates(p),
                JobType.Allocate,
                invOrg);
        }

        /// <summary>
        /// 保存库存调拨数据
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public virtual List<ErpErrorData> SaveInventoryAllocates(List<InventoryAllocateData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;
            if (datas.Any(f => (OrderType)f.OrderType != OrderType.DirectAllocate && (OrderType)f.OrderType != OrderType.TwoAllocate))
                return errors;
            //EntityList<InventoryAllocate> inventoryAllocates = new EntityList<InventoryAllocate>();
            List<string> whCodes = new List<string>();
            whCodes.AddRange(datas.Select(a => a.SourceWHCode).Distinct());
            whCodes.AddRange(datas.Select(a => a.TargetWHCode).Distinct());
            var whdic = RT.Service.Resolve<Warehouses.WarehouseController>().GetWarehouseList(whCodes.Distinct().ToList()).ToDictionary(p => p.Code, p => p.Id);
            if (whdic.Count == 0)
                return errors;
            var locCodes = datas.SelectMany(a => a.RequireList).Select(a => a.AppointLoc).Distinct().ToList();
            var pickToCodes = datas.Where(a => a.PickToCode.IsNotEmpty()).Select(a => a.PickToCode).Distinct().ToList();
            pickToCodes.AddRange(locCodes.Where(a => a != ""));
            var query = Query<StorageLocation>()
                .Join<StorageArea>((a, b) => a.AreaId == b.Id && !b.IsFrozen)
                .Join<Warehouse>((a, b) => a.WarehouseId == b.Id && !b.IsFrozen);
            query.Where(p => p.State == State.Enable && !p.IsFrozen);
            query.Where(p => pickToCodes.Contains(p.Code) || p.Code == "PICKTO");
            var locs = query.ToList();
            var itemCodes = datas.SelectMany(a => a.RequireList).Select(a => a.ItemCode).Distinct().ToList();
            var itemdic = RT.Service.Resolve<ItemController>().GetItems(itemCodes).ToDictionary(a => a.Code, a => a.Id);
            var lotCodes = datas.SelectMany(a => a.RequireList).Select(a => a.LotCode).Distinct().ToList();
            var lotdic = RT.Service.Resolve<LotController>().GetLots(lotCodes).ToDictionary(a => a.Code, a => a.Id);
            try
            {
                datas.GroupBy(p => p.OrderType).ForEach(p =>
                {
                    var orderType = (OrderType)p.Key;
                    double tranId = 0;
                    Transaction transaction = RT.Service.Resolve<TransactionController>().GetTransactions(orderType).FirstOrDefault();
                    if (transaction == null)
                    {
                        throw new ValidationException("请维护单据小类[{0}]".L10nFormat(orderType.ToLabel()));
                    }
                    p.ToList().ForEach(a =>
                    {
                        try
                        {
                            if (a.No.IsNullOrEmpty())
                                throw new ValidationException("单号不能为空".L10N());
                            if (a.RequireList.Count == 0)
                                return;
                            //InventoryAllocate allocate = new InventoryAllocate()
                            //{
                            //    OrderType = orderType,
                            //    AllocateState = AllocateState.Create,
                            //    No = a.No,
                            //    TransDate = a.TransDate,
                            //    SourceNo = a.SourceNo,
                            //    ShipperCode = a.ShipperCode.IsNotEmpty() ? a.ShipperCode : "*",
                            //    TransactionId = tranId,
                            //};
                            double souWhId = 0;
                            if (!whdic.TryGetValue(a.SourceWHCode, out souWhId))
                                throw new ValidationException("仓库{0}不存在".L10nFormat(a.SourceWHCode));
                            double tarWhId = 0;
                            if (!whdic.TryGetValue(a.TargetWHCode, out tarWhId))
                                throw new ValidationException("仓库{0}不存在".L10nFormat(a.TargetWHCode));
                            //allocate.TargetWHId = tarWhId;
                            //allocate.SourceWHId = souWhId;
                            var pickto = locs.FirstOrDefault(b => b.Code == "PICKTO");
                            //allocate.PickToId = pickto.Id;
                            if (a.PickToCode.IsNotEmpty())
                            {
                                var pick = locs.FirstOrDefault(b => b.Code == a.PickToCode);
                                if (pick != null)
                                {
                                    if (pick.WarehouseId != souWhId)
                                        throw new ValidationException("在途库位{0}不是仓库{1}的库位".L10nFormat(a.PickToCode, a.SourceWHCode));
                                    //allocate.PickToId = pick.Id;
                                }
                            }
                            int i = 1;
                            //a.RequireList.ForEach(b =>
                            //{
                            //    if (!itemdic.ContainsKey(b.ItemCode))
                            //    {
                            //        throw new ValidationException("物料{0}不存在".L10nFormat(b.ItemCode)); ;
                            //    }
                            //    if (b.LotCode.IsNotEmpty() && !lotdic.ContainsKey(b.LotCode))
                            //    {
                            //        throw new ValidationException("批次{0}不存在".L10nFormat(b.LotCode)); ;
                            //    }

                            //    var itemid = itemdic.GetValue<double>(b.ItemCode);
                            //    Requirement requirement = new Requirement()
                            //    {
                            //        ItemId = itemid,
                            //        RequireQty = b.Qty,
                            //        LineNo = i,
                            //        LotId = lotdic.GetValue<double?>(b.LotCode, null),
                            //        AllocateState = AllocateState.RequireCreate,
                            //        AppointTaskNo = b.AppointTaskNo,
                            //        AppointLpn = b.AppointLPN,
                            //        Project = b.AppointProjectNo,
                            //    };
                            //    if (requirement.LotId.HasValue)
                            //    {
                            //        requirement.LotAtt01 = b.LotAtt01;
                            //        requirement.LotAtt02 = b.LotAtt02;
                            //        requirement.LotAtt03 = b.LotAtt03;
                            //        requirement.LotAtt04 = b.LotAtt04;
                            //    }
                            //    if (b.AppointLoc.IsNotEmpty())
                            //    {
                            //        var loc = locs.FirstOrDefault(c => c.Code == b.AppointLoc);
                            //        if (loc?.WarehouseId != souWhId)
                            //            throw new ValidationException("指定库位{0}不是仓库{1}的库位".L10nFormat(b.AppointLoc, a.SourceWHCode));
                            //        requirement.AppointLocationId = loc.Id;
                            //    }
                            //    i++;
                            //    var assignRule = RT.Service.Resolve<StrategyCommonController>().GetEnabelAssignRuleData(requirement.ItemId, souWhId, orderType, null);
                            //    if (assignRule == null)
                            //        throw new ValidationException("未配置默认分配规则".L10N());
                            //    requirement.AssignRuleId = assignRule.Id;
                            //    var turnOverRule = RT.Service.Resolve<StrategyCommonController>().GetEnabelTurnOverRuleData(requirement.ItemId, souWhId, orderType, null);
                            //    if (turnOverRule == null) throw new ValidationException("未配置默认周转规则".L10N());
                            //    requirement.TurnOverRuleId = turnOverRule.Id;
                            //    allocate.RequirementList.Add(requirement);
                            //});
                            //inventoryAllocates.Add(allocate);
                        }
                        catch (Exception ex)
                        {
                            errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = a.No });
                        }
                    });

                });
                //RF.Save(inventoryAllocates);
            }
            catch (Exception ex)
            {
                errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = "" });
            }

            return errors;
        }
    }
}
