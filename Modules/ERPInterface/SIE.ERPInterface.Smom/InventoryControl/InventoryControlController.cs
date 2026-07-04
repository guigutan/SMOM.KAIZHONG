using System;
using System.Collections.Generic;
using System.Linq;
using SIE.Domain;
using SIE.ERPInterface.Common.Datas.EbsData;
using SIE.ERPInterface.Ebs.Download.OnHands;
using SIE.ERPInterface.Smom.InventoryControl;
using SIE.ERPInterface.Smom.InventoryControl.datas;
using SIE.Inventory.Onhands;
using SIE.Items;
using SIE.Warehouses;

namespace SIE.ERPInterface.Common.InventoryControl
{
    /// <summary>
    /// 库存对照表控制器
    /// </summary>
    public class InventoryControlController:DomainController
    {
        #region 库存对照表
        /// <summary>
        /// 获取库存对照表设置
        /// </summary>
        /// <returns></returns>
        public virtual InventoryControlSetting GetInventoryControlSetting()
        {
            var employeeId = AppRuntime.IdentityId;
            var setting = GetInventoryControlSettingData(employeeId);
            if (setting == null)
            {
                //如果没有设置的信息
                setting = new InventoryControlSetting();
                setting.GenerateId();
                setting.IsItem = true;
                setting.IsLot = true;
                setting.IsWareHouse = true;
                setting.IsNgInv = true;
                setting.IsOkInv = true;
                setting.EbsToLot = EbsToLot.ToLotCode;
                setting.EbsToWarehouse = EbsToWareHouse.OneToOne;
                //setting.EmployeeId = employeeId;
                RF.Save(setting);
                return setting;
            }
            return setting;
        }

        /// <summary>
        /// 获取库存对照表设置
        /// </summary>
        /// <returns></returns>
        public virtual InventoryControlSetting GetInventoryControlSettingData(double employeeId)
        {
            var query = Query<InventoryControlSetting>();

            return query.Where(p => p.CreateBy == employeeId).ToList().FirstOrDefault();
        }

        /// <summary>
        /// 获取库存对照表列表数据
        /// </summary>
        /// <returns></returns>
        public virtual InventroyControlAllData GetInventoryControlViewModelData(InventoryControlViewModelCriteria criteria)
        {
            InventroyControlAllData result = new InventroyControlAllData();
            var orgId = AppRuntime.InvOrg;
            var orgName = "";
            List<string> ErpWareHouseCodes = new List<string>();
            var ErpWareHouse = AppRuntime.Service.Resolve<WarehouseController>().GetErpWareHouseByOrg(orgId.Value);
            if (!criteria.ErpWarehouseCode.IsNullOrEmpty())
            {
                //有值取erp仓库 没值取所有的ERP仓库
                ErpWareHouseCodes = criteria.ErpWarehouseCode.Split(",").Distinct().ToList();
                if (ErpWareHouseCodes.Count>0)
                {
                    orgName = ErpWareHouse[0].ErpOrgName;
                }
                
            }
            else
            {

                ErpWareHouseCodes = ErpWareHouse.Select(p => p.Code).Distinct().ToList();
                if (ErpWareHouseCodes.Count>0)
                {
                    orgName = ErpWareHouse[0].ErpOrgName;
                }
                
            }
            var allData = AppRuntime.Service.Resolve<EbsOnHandController>().Download(ErpWareHouseCodes, orgName);
            //根据查询条件过滤原始的数据 目前只有ERP批次号
            if (criteria.ErpLotCode.IsNotEmpty())
            {
                allData = allData.Where(p => p.Lot_Number == criteria.ErpLotCode).ToList();
            }
            if (criteria.ItemCode.IsNotEmpty())
            {
                allData = allData.Where(p => p.Item_Code == criteria.ItemCode).ToList();
            }
            var ErpWareCodes = allData.Select(p => p.Subinventory).Distinct().ToList();
            //根据ERP子库查找所有对应的仓库
            var wmsWareHouse = GetWmsWareHouseByErpWareHouse(ErpWareCodes);
            //获取当前设置
            var setting = GetInventoryControlSetting();
            //获取库存数据
            var onHandDatas = GetLotLpnOnhands(criteria, setting);
            //设置转换数据
            setOnHandData(onHandDatas, setting);
            if (onHandDatas.Count == 0 && allData.Count == 0)
            {
                return result;
            }
            //获取列表数据
            GetInventoryControlViewModels(onHandDatas, allData, wmsWareHouse, criteria, setting, result);

            return result;
        }

        /// <summary>
        /// 根据ERP子库获取对应的仓库
        /// </summary>
        /// <param name="ErpWareCodes">获取仓库</param>
        /// <returns></returns>
        private EntityList<ErpWarehouseDetail> GetWmsWareHouseByErpWareHouse(List<string> ErpWareCodes)
        {
            var query = Query<ErpWarehouseDetail>();
            query.Join<ErpWarehouse>((x, y) => x.ErpWarehouseId == y.Id && ErpWareCodes.Contains(y.Code));
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            //return result;
        }

        /// <summary>
        /// 查询库存
        /// </summary>
        /// <param name="criteria">库存对照表查询条件</param>
        /// <param name="Setting">库存对照表设置</param>
        /// <returns></returns>
        private EntityList<LotLpnOnhand> GetLotLpnOnhands(InventoryControlViewModelCriteria criteria, InventoryControlSetting Setting)
        {
            var query = Query<LotLpnOnhand>();
            if (criteria.ItemCode.IsNotEmpty())
            {
                query.Where(p => p.Item.Code.Contains(criteria.ItemCode));
            }
            if (criteria.ItemName.IsNotEmpty())
            {
                query.Where(p => p.Item.Name.Contains(criteria.ItemName));
            }
            if (Setting.IsNgInv == true)
            {
                //获取合格库存和不合格库存
                query.Where(p => p.State == OnhandState.Ok || p.State == OnhandState.Ng);
            }
            else
            {
                //只获取合格库存
                query.Where(p => p.State == OnhandState.Ok);
            }
            if (criteria.WarehouseCode.IsNotEmpty())
            {
                query.Where(p => criteria.WarehouseCode.Contains(p.Warehouse.Code));
            }
            if (criteria.IsShowZero == true)
            {
                query.Where(p => p.Qty > 0);
            }
            return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 设置列表数据
        /// </summary>
        /// <param name="lotLpnOnhands">批次和LPN库存</param>
        /// <param name="erpDatas">ERP库存</param>
        /// <param name="DicWareHouse">ERP子库字典</param>
        /// <param name="criteria">查询条件</param>
        /// <param name="Setting">设置</param>
        /// <param name="result">库存对照表数据</param>
        /// <returns></returns>
        private void GetInventoryControlViewModels(EntityList<LotLpnOnhand> lotLpnOnhands, List<ErpOnHandData> erpDatas, EntityList<ErpWarehouseDetail> DicWareHouse, InventoryControlViewModelCriteria criteria, InventoryControlSetting Setting, InventroyControlAllData result)
        {
            int LineNo = 1;
            if (Setting.EbsToWarehouse == EbsToWareHouse.NToOne)
            {
                if (Setting.IsLot == true)
                {
                    //展示WMS与ERP的库存
                    lotLpnOnhands.GroupBy(p => new { p.RealLotCode, p.ItemId }).ForEach(p =>
                    {
                        //var onHandItem = p.FirstOrDefault();
                        var List = p.ToList();
                        LineNo = SetInventoryControlItemData(erpDatas, DicWareHouse, criteria, Setting, result, LineNo, List);
                    });
                }
                else
                {
                    //展示WMS与ERP的库存
                    lotLpnOnhands.GroupBy(p => new { p.ItemId }).ForEach(p =>
                    {
                        //var onHandItem = p.FirstOrDefault();
                        var List = p.ToList();
                        LineNo = SetInventoryControlItemData(erpDatas, DicWareHouse, criteria, Setting, result, LineNo, List);
                    });
                }
            }
            else
            {
                //只有当仓库选择1:N
                //展示WMS与ERP的库存
                if (Setting.IsWareHouse == true && Setting.IsLot == true)
                {
                    lotLpnOnhands.GroupBy(p => new { p.WarehouseId, p.RealLotCode, p.ItemId }).ForEach(p =>
                    {
                        //var onHandItem = p.FirstOrDefault();
                        var List = p.ToList();
                        LineNo = SetInventoryControlItemData(erpDatas, DicWareHouse, criteria, Setting, result, LineNo, List);
                    });
                }
                else if (Setting.IsWareHouse == false && Setting.IsLot == true)
                {
                    ////展示WMS与ERP的库存
                    lotLpnOnhands.GroupBy(p => new { p.RealLotCode, p.ItemId }).ForEach(p =>
                    {
                        var List = p.ToList();
                        LineNo = SetInventoryControlItemData(erpDatas, DicWareHouse, criteria, Setting, result, LineNo, List);
                    });
                }
                else if (Setting.IsWareHouse == true && Setting.IsLot == false)
                {
                    ////展示WMS与ERP的库存
                    lotLpnOnhands.GroupBy(p => new { p.WarehouseId, p.ItemId }).ForEach(p =>
                    {
                        var List = p.ToList();
                        LineNo = SetInventoryControlItemData(erpDatas, DicWareHouse, criteria, Setting, result, LineNo, List);
                    });
                }
                else if (Setting.IsWareHouse == false && Setting.IsLot == false)
                {
                    //展示WMS与ERP的库存
                    lotLpnOnhands.GroupBy(p => new { p.ItemId }).ForEach(p =>
                    {
                        var List = p.ToList();
                        LineNo = SetInventoryControlItemData(erpDatas, DicWareHouse, criteria, Setting, result, LineNo, List);
                    });
                    //展示
                }
            }
            //筛选ERP库存有但是WMS没有的库存（ERP库存只有物料和批次的维度）
            if (Setting.IsLot == true)
            {
                erpDatas.GroupBy(p => new { p.Item_Code, p.Lot_Number }).ForEach(p =>
                {
                    var List = p.ToList();
                    LineNo = SetWmsNoHandData(List, Setting, result, lotLpnOnhands, LineNo, criteria);
                });
            }
            else
            {
                erpDatas.GroupBy(p => new { p.Item_Code }).ForEach(p =>
                {
                    var List = p.ToList();
                    LineNo = SetWmsNoHandData(List, Setting, result, lotLpnOnhands, LineNo, criteria);
                });
            }
            //重新排序
            result.ParentListData = result.ParentListData.OrderByDescending(p => p.WareHouseCode).ThenBy(p => p.ItemCode).ThenBy(p => p.ErpLotCode).ToList();
        }

        /// <summary>
        /// 设置ERP有但是wms没有的数据
        /// </summary>
        /// <param name="erpOnHandDatas">ERP库存数据</param>
        /// <param name="Setting">设置</param>
        /// <param name="result">列表数据</param>
        /// <param name="lotLpnOnhands">WMS库存数据</param>
        /// <param name="LineNo">行号</param>
        /// <returns></returns>
        private int SetWmsNoHandData(List<ErpOnHandData> erpOnHandDatas, InventoryControlSetting Setting, InventroyControlAllData result, EntityList<LotLpnOnhand> lotLpnOnhands, int LineNo, InventoryControlViewModelCriteria criteria)
        {
            InventoryControlViewModel InvViewModelItem = new InventoryControlViewModel();
            List<InventoryControlErpDetaiViewModel> ItemErpDetailList = new List<InventoryControlErpDetaiViewModel>();
            var ErpItem = erpOnHandDatas.FirstOrDefault();
            InvViewModelItem.ItemCode = ErpItem.Item_Code;
            InvViewModelItem.ErpLotCode = ErpItem.Lot_Number;
            List<LotLpnOnhand> wmsOnhandData = new List<LotLpnOnhand>();
            if (Setting.IsLot == true)
            {
                wmsOnhandData = lotLpnOnhands.Where(p => p.ItemCode == InvViewModelItem.ItemCode && p.RealLotCode == InvViewModelItem.ErpLotCode).ToList();
            }
            else
            {
                InvViewModelItem.ErpLotCode = "";
                wmsOnhandData = lotLpnOnhands.Where(p => p.ItemCode == InvViewModelItem.ItemCode).ToList();
            }
            if (wmsOnhandData.Count > 0)
            {
                return LineNo;
            }
            InvViewModelItem.ErpQty = erpOnHandDatas.Sum(f => f.Quantity);
            InvViewModelItem.DifferenceQty = InvViewModelItem.Qty - InvViewModelItem.ErpQty;

            var item = AppRuntime.Service.Resolve<ItemController>().GetItems(new List<string> { InvViewModelItem.ItemCode }).FirstOrDefault();
            if (item != null)
            {
                InvViewModelItem.ItemName = item.Name;
                InvViewModelItem.UnitCode = item.Unit.Name;
                InvViewModelItem.SpecificationModel = item.SpecificationModel;
            }
            int ErpdetailLineNo = 1;
            if (Setting.IsWareHouse == true)
            {
                //勾选了仓库
                erpOnHandDatas.GroupBy(p => p.Subinventory).ForEach(p =>
                {
                    var ErpwareHouseCode = p.Key;
                    var Erpqty = p.Sum(f => f.Quantity);
                    ItemErpDetailList.Add(SetErpInvDetail(ErpwareHouseCode, Erpqty, LineNo, ErpdetailLineNo));
                    ErpdetailLineNo++;
                });
            }
            InvViewModelItem.LineNo = LineNo;
            if (!criteria.ItemName.IsNullOrEmpty())
            {
                if (InvViewModelItem.ItemName == criteria.ItemName)
                {
                    result.ParentListData.Add(InvViewModelItem);
                    result.ErpListData.AddRange(ItemErpDetailList);
                }
            }
            else
            {
                result.ParentListData.Add(InvViewModelItem);
                result.ErpListData.AddRange(ItemErpDetailList);
            }
            LineNo++;
            return LineNo;
        }

        /// <summary>
        /// 获取每一行的数据
        /// </summary>
        /// <param name="erpDatas">ERP库存数据</param>
        /// <param name="DicWareHouse">ERP子库</param>
        /// <param name="criteria">查询条件</param>
        /// <param name="Setting">库存对照表设置</param>
        /// <param name="result">返回加过</param>
        /// <param name="LineNo">行号</param>
        /// <param name="p">wms库存数据</param>
        /// <returns></returns>
        private int SetInventoryControlItemData(List<ErpOnHandData> erpDatas, EntityList<ErpWarehouseDetail> DicWareHouse, InventoryControlViewModelCriteria criteria, InventoryControlSetting Setting, InventroyControlAllData result, int LineNo, List<LotLpnOnhand> p)
        {
            InventoryControlViewModel InvViewModelItem = new InventoryControlViewModel();
            List<InventoryControlDetailViewModel> ItemDetailList = new List<InventoryControlDetailViewModel>();
            List<InventoryControlErpDetaiViewModel> ItemErpDetailList = new List<InventoryControlErpDetaiViewModel>();
            var onHandItem = p.FirstOrDefault();
            List<string> ErpWareHouseCodes = new List<string>();
            var ErpWareHouse = DicWareHouse.Where(x => x.WarehouseId == onHandItem.WarehouseId).ToList();
            //InvViewModelItem.ItemId = onHandItem.ItemId;
            InvViewModelItem.ItemCode = onHandItem.ItemCode;
            InvViewModelItem.ItemName = onHandItem.ItemName;
            InvViewModelItem.WareHouseId = onHandItem.WarehouseId;
            InvViewModelItem.ErpLotCode = onHandItem.RealLotCode;
            InvViewModelItem.UnitCode = onHandItem.UnitName;
            InvViewModelItem.SpecificationModel = onHandItem.Item.SpecificationModel;
            if (Setting.EbsToWarehouse == EbsToWareHouse.OneToOne)
            {
                //一对一
                //单个子库
                InvViewModelItem.WareHouseCode = onHandItem.WarehouseCode;
                var ErpWareItem = ErpWareHouse.FirstOrDefault();
                if (ErpWareItem != null)
                {
                    ErpWareHouseCodes.Add(ErpWareItem.ErpWarehouseCode);
                    InvViewModelItem.ErpWareHouseCode = ErpWareItem.ErpWarehouseCode;
                }
            }
            if (Setting.EbsToWarehouse == EbsToWareHouse.OneToN)
            {
                //一对多
                //多个子库
                ErpWareHouseCodes = ErpWareHouse.Select(p => p.ErpWarehouseCode).Distinct().ToList();
            }
            InvViewModelItem.ItemId = onHandItem.ItemId;
            InvViewModelItem.LotCode = onHandItem.LotCode;
            if (criteria.ErpLotCode.IsNotEmpty() && InvViewModelItem.ErpLotCode != criteria.ErpLotCode)
            {
                return LineNo;
            }
            if (Setting.IsWareHouse != true || Setting.EbsToWarehouse == EbsToWareHouse.NToOne)
            {
                InvViewModelItem.WareHouseCode = "";
                InvViewModelItem.WareHouseId = 0;
                InvViewModelItem.ErpWareHouseCode = "";
                ErpWareHouseCodes.Clear();
            }
            if (Setting.IsLot != true)
            {
                InvViewModelItem.ErpLotCode = "";
                InvViewModelItem.LotCode = "";
            }
            var ErpLotCodes = p.Select(f => f.RealLotCode).Distinct().ToList();
            var newErpData = GetErpOnHands(ErpLotCodes, ErpWareHouseCodes, InvViewModelItem.ItemCode, erpDatas);
            //批次库存现有量
            if (Setting.EbsToWarehouse == EbsToWareHouse.NToOne)
            {
                InvViewModelItem.ErpWareHouseCode = newErpData.FirstOrDefault()?.Subinventory;
            }
            var nowQty = p.Sum(f => f.Qty);
            //物料的明细数
            //var AsnDetail = GetAsnDetailsRecQty(InvViewModelItem.WareHouseId, InvViewModelItem.ItemId, InvViewModelItem.LotCode);
            //var recQty = AsnDetail.Sum(f => f.ActualQty) - AsnDetail.Sum(f => f.GroundingQty);
            //InvViewModelItem.Qty = nowQty - recQty ?? 0;
            //InvViewModelItem.TemporaryQty = recQty ?? 0;
            InvViewModelItem.LineNo = LineNo;
            InvViewModelItem.ErpQty = newErpData.Sum(p => p.Quantity);
            InvViewModelItem.DifferenceQty = InvViewModelItem.Qty - InvViewModelItem.ErpQty;
            //不包含0库存
            if (criteria.IsShowZero == true && InvViewModelItem.Qty == 0 && InvViewModelItem.ErpQty == 0)
            {
                return LineNo;
            }
            //if (Setting.IsWareHouse == true && Setting.EbsToWarehouse != EbsToWareHouse.OneToOne)
            //{
            //    SetItemDetailList(Setting, newErpData, ItemDetailList, p, InvViewModelItem, LineNo, ItemErpDetailList, AsnDetail);
            //}
            if (criteria.IsShowDifferent == true)
            {
                //只显示差异库存
                if (InvViewModelItem.DifferenceQty != 0)
                {
                    result.ParentListData.Add(InvViewModelItem);
                    result.DetailListData.AddRange(ItemDetailList);
                    result.ErpListData.AddRange(ItemErpDetailList);
                    LineNo++;
                }
            }
            else
            {
                result.ParentListData.Add(InvViewModelItem);
                result.DetailListData.AddRange(ItemDetailList);
                result.ErpListData.AddRange(ItemErpDetailList);
                LineNo++;
            }
            return LineNo;
        }

        /// <summary>
        /// 设置明细数据
        /// </summary>
        /// <param name="Setting"></param>
        /// <param name="newErpData"></param>
        /// <param name="ItemDetailList"></param>
        /// <param name="p"></param>
        /// <param name="InvViewModelItem"></param>
        /// <param name="LineNo"></param>
        /// <param name="ItemErpDetailList"></param>
        //private void SetItemDetailList(InventoryControlSetting Setting, List<ErpOnHandData> newErpData, List<InventoryControlDetailViewModel> ItemDetailList, List<LotLpnOnhand> p, InventoryControlViewModel InvViewModelItem, int LineNo, List<InventoryControlErpDetaiViewModel> ItemErpDetailList, EntityList<AsnDetail> asnDetails)
        //{
        //    int wmsdetailLineNo = 1;
        //    int ErpdetailLineNo = 1;
        //    //赋值wms仓库数据
        //    p.GroupBy(p => new { p.WarehouseCode, p.WarehouseId }).ForEach(p =>
        //    {
        //        var wareHouseCode = p.Key.WarehouseCode;
        //        var qty = p.Sum(f => f.Qty);
        //        ItemDetailList.Add(SetInvDetailData(wareHouseCode, qty, LineNo, wmsdetailLineNo, asnDetails, InvViewModelItem.ItemId, p.Key.WarehouseId));
        //        wmsdetailLineNo++;
        //    });
        //    //赋值Erp子库数据
        //    newErpData.GroupBy(p => p.Subinventory).ForEach(p =>
        //    {
        //        var ErpwareHouseCode = p.Key;
        //        var Erpqty = p.Sum(f => f.Quantity);
        //        ItemErpDetailList.Add(SetErpInvDetail(ErpwareHouseCode, Erpqty, LineNo, ErpdetailLineNo));
        //        ErpdetailLineNo++;
        //    });
        //}

        /// <summary>
        /// 设置WMS库存数据
        /// </summary>
        /// <param name="warehouseCode">库存数据</param>
        /// <param name="qty">总数</param>
        /// <param name="LineNo">父行号</param>
        /// <param name="detailLineNo">明细行号</param>
        /// <returns></returns>
        //private InventoryControlDetailViewModel SetInvDetailData(string warehouseCode, decimal qty, int LineNo, int detailLineNo, EntityList<AsnDetail> asnDetails, double ItemId, double wareHouseId)
        //{
        //    var detail = new InventoryControlDetailViewModel();
        //    var asndtl = asnDetails.Where(f => f.ItemId == ItemId && wareHouseId == f.AsnWarehouseId).ToList();
        //    detail.Qty = qty;
        //    detail.WareHouseCode = warehouseCode;
        //    detail.ParentLineNo = LineNo;
        //    detail.LineNo = detailLineNo;
        //    detail.RecQty = asndtl.Sum(f => f.ActualQty) - asndtl.Sum(f => f.GroundingQty);
        //    detail.Qty = qty - detail.RecQty;
        //    detail.ErpQty = p.Quantity;
        //    detail.Qty = InvViewModelItem.Qty;
        //    detail.DifferenceQty = InvViewModelItem.DifferenceQty;
        //    detail.WareHouseCode = InvViewModelItem.WareHouseCode;
        //    detail.ErpWareHouseCode = p.Subinventory;
        //    detail.ParentLineNo = LineNo;
        //    detail.LineNo = detailLineNo;
        //    return detail;
        //}

        /// <summary>
        /// 设置ERP子库数据
        /// </summary>
        /// <param name="warehouseCode">ERP数据</param>
        /// <param name="qty">ERP数据</param>
        /// <param name="LineNo">父行号</param>
        /// <param name="detailLineNo">明细行号</param>
        /// <returns></returns>
        private InventoryControlErpDetaiViewModel SetErpInvDetail(string warehouseCode, decimal qty, int LineNo, int detailLineNo)
        {
            var detail = new InventoryControlErpDetaiViewModel();
            detail.ErpQty = qty;
            detail.ErpWareHouseCode = warehouseCode;
            detail.LineNo = detailLineNo;
            detail.ParentLineNo = LineNo;
            return detail;
        }

        /// <summary>
        /// 设置ERP批次号与库存的对照关系
        /// </summary>
        /// <param name="onHandDatas">库存数据</param>
        /// <param name="setting">设置</param>
        private void setOnHandData(EntityList<LotLpnOnhand> onHandDatas, InventoryControlSetting setting)
        {
            onHandDatas.ForEach(p =>
            {
                switch (setting.EbsToLot)
                {
                    case EbsToLot.ToLotCode:
                        p.RealLotCode = p.LotCode;
                        break;
                    case EbsToLot.ToProductionBatch:
                        p.RealLotCode = p.LotAtt04;
                        break;
                    case EbsToLot.ToLotAtt8:
                        p.RealLotCode = p.LotAtt08;
                        break;
                    case EbsToLot.ToLotAtt9:
                        p.RealLotCode = p.LotAtt09;
                        break;
                    case EbsToLot.ToLotAtt10:
                        p.RealLotCode = p.LotAtt10;
                        break;
                    case EbsToLot.ToLotAtt11:
                        p.RealLotCode = p.LotAtt11.ToString();
                        break;
                    case EbsToLot.ToLotAtt12:
                        p.RealLotCode = p.LotAtt12.ToString();
                        break;
                    default:
                        p.RealLotCode = p.LotCode;
                        break;

                }
            });
        }

        /// <summary>
        /// 过滤ERP库存数据
        /// </summary>
        /// <param name="LotCodes">批次号</param>
        /// <param name="ErpCodes">子库</param>
        /// <param name="ItemCode">物料</param>
        /// <param name="erpDatas">erp库存数据</param>
        /// <returns></returns>
        private List<ErpOnHandData> GetErpOnHands(List<string> LotCodes, List<string> ErpCodes, string ItemCode, List<ErpOnHandData> erpDatas)
        {
            if (erpDatas.Count == 0)
            {
                return erpDatas;
            }
            if (LotCodes.Count > 0)
            {
                erpDatas = erpDatas.Where(p => LotCodes.Contains(p.Lot_Number)).ToList();
            }
            if (ErpCodes.Count > 0)
            {
                erpDatas = erpDatas.Where(p => ErpCodes.Contains(p.Subinventory)).ToList();
            }
            if (ItemCode.IsNotEmpty())
            {
                erpDatas = erpDatas.Where(p => p.Item_Code == ItemCode).ToList();
            }
            return erpDatas;
        }

        /// <summary>
        /// 获取暂存数量
        /// </summary>
        /// <param name="wareHouseId">仓库ID</param>
        /// <param name="ItemId">物料ID</param>
        /// <param name="LotCode">批次号</param>
        /// <returns></returns>
        //private EntityList<AsnDetail> GetAsnDetailsRecQty(double wareHouseId, double ItemId, string LotCode)
        //{
        //    //decimal Qty = 0;
        //    List<AsnState> asnStates = new List<AsnState>()
        //    {
        //        AsnState.Collect,
        //        AsnState.PartCollect,
        //        AsnState.PartInspection,
        //        AsnState.Inspection,
        //        AsnState.PartQuaInspection,
        //        AsnState.QuaInspection,
        //        AsnState.PartGrounding,
        //    };
        //    var query = Query<AsnDetail>();
        //    query.Where(p => p.ItemId == ItemId && asnStates.Contains(p.AsnState));
        //    if (wareHouseId > 0)
        //    {
        //        query.Where(p => p.Asn.WarehouseId == wareHouseId);
        //    }
        //    //if (LotCode.IsNotEmpty())
        //    //{
        //    //    switch (Setting.EbsToLot)
        //    //    {
        //    //        case EbsToLot.ToLotCode:
        //    //            query.Where(p => p.Lot == LotCode);
        //    //            break;
        //    //        case EbsToLot.ToProductionBatch:
        //    //            query.Where(p => p.LotAtt04 == LotCode);
        //    //            break;
        //    //        case EbsToLot.ToLotAtt8:
        //    //            query.Where(p => p.LotAtt08 == LotCode);
        //    //            break;
        //    //        case EbsToLot.ToLotAtt9:
        //    //            query.Where(p => p.LotAtt09 == LotCode);
        //    //            break;
        //    //        case EbsToLot.ToLotAtt10:
        //    //            query.Where(p => p.LotAtt10 == LotCode);
        //    //            break;
        //    //        case EbsToLot.ToLotAtt11:
        //    //            query.Where(p => p.LotAtt11.ToString() == LotCode);
        //    //            break;
        //    //        case EbsToLot.ToLotAtt12:
        //    //            query.Where(p => p.LotAtt12.ToString() == LotCode);
        //    //            break;
        //    //        default:
        //    //            query.Where(p => p.Lot == LotCode);
        //    //            break;
        //    //    }
        //    //}
        //    if (LotCode.IsNotEmpty())
        //    {
        //        query.Where(p => p.Lot == LotCode);
        //    }

        //    var asnDetails = query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        //    return asnDetails;
        //}
        #endregion
    }
}
