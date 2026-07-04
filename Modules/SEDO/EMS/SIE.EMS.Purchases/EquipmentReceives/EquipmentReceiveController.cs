using SIE.Common.Configs;
using SIE.Core.Common.Controllers;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments;
using SIE.EMS.Purchases.Enums;
using SIE.EMS.Purchases.EquipmentReceives.Configs;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipmentCards;
using SIE.Equipments.EquipModels;
using SIE.EventMessages.EMS.EquipAccount;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.EMS.Purchases.EquipmentReceives
{
    /// <summary>
    /// 设备接收控制器
    /// </summary>
    public partial class EquipmentReceiveController : DomainController
    {
        /// <summary>
        /// 查询设备接收信息
        /// </summary>
        /// <param name="criteria">设备接收查询实体</param>
        /// <returns>设备接收信息</returns>
        public virtual EntityList<EquipmentReceive> CriteriaEquipmentReceives(EquipmentReceiveCriteria criteria)
        {
            var query = Query<EquipmentReceive>();
            if (criteria.FactoryId.HasValue)
            {
                query.Where(p => p.FactoryId == criteria.FactoryId.Value);
            }
            if (criteria.DepartmentId.HasValue)
            {
                query.Where(p => p.DepartmentId == criteria.DepartmentId.Value);
            }
            if (!criteria.PurchaseOrderNo.IsNullOrWhiteSpace())
            {
                query.Join<EquipmentReceiveDetail>("b", (a, b) => a.Id == b.EquipmentReceiveId)
                    .Join<EquipmentReceiveDetail, PurchaseOrder>((b, c) => b.PurchaseOrderId == c.Id && c.OrderNo.Contains(criteria.PurchaseOrderNo));
            }
            if (criteria.ReceiveType.HasValue)
            {
                query.Where(p => p.ReceiveType == criteria.ReceiveType.Value);
            }
            if (criteria.SupplierId.HasValue || criteria.CustomerId.HasValue)
            {
                query.Join<EquipmentReceiveDetail>("b1", (a, b1) => a.Id == b1.EquipmentReceiveId)
                    .WhereIf<EquipmentReceiveDetail>(criteria.SupplierId.HasValue, (a, b1) => b1.SupplierId == criteria.SupplierId)
                    .WhereIf<EquipmentReceiveDetail>(criteria.CustomerId.HasValue, (a, b1) => b1.CustomerId == criteria.CustomerId);
            }
            if (criteria.ReceiveBillStatus.HasValue)
            {
                query.Where(p => p.ReceiveBillStatus == criteria.ReceiveBillStatus.Value);
            }
            if (criteria.CreateDate.BeginValue.HasValue)
            {
                query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
            }
            if (criteria.CreateDate.EndValue.HasValue)
            {
                query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
            }
            return query.Distinct().OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id列表获取设备接收信息
        /// </summary>
        /// <param name="receiveIds">id列表</param>
        /// <returns>设备接收信息</returns>
        public virtual EntityList<EquipmentReceive> GetEquipmentReceivesByIds(List<double> receiveIds)
        {
            return receiveIds.SplitContains(ids => Query<EquipmentReceive>().Where(p => ids.Contains(p.Id)).ToList());
        }

        /// <summary>
        /// 根据id获取设备接收
        /// </summary>
        /// <param name="id">设备接收id</param>
        /// <returns>设备接收</returns>
        public virtual EquipmentReceive GetEquipmentReceiveById(double id)
        {
            return Query<EquipmentReceive>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备接收id列表获取接收明细列表
        /// </summary>
        /// <param name="receiveIds">设备接收id列表</param>
        /// <returns>接收明细列表</returns>
        public virtual EntityList<EquipmentReceiveDetail> GetDetailsByReceiveIds(List<double> receiveIds)
        {
            return Query<EquipmentReceiveDetail>().Where(p => receiveIds.Contains(p.EquipmentReceiveId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据设备接收id获取接收明细列表
        /// </summary>
        /// <param name="receiveId">设备接收id</param>
        /// <param name="pagingInfo">分页</param>
        /// <returns>接收明细列表</returns>
        public virtual EntityList<EquipmentReceiveDetail> GetDetailsByReceiveId(double receiveId, PagingInfo pagingInfo)
        {
            return Query<EquipmentReceiveDetail>().Where(p => receiveId == p.EquipmentReceiveId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据id获取设备接收明细
        /// </summary>
        /// <param name="id">设备接收明细id</param>
        /// <returns>设备接收明细</returns>
        public virtual EquipmentReceiveDetail GetEquipmentReceiveDetailById(double id)
        {
            return Query<EquipmentReceiveDetail>().Where(p => p.Id == id).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据采购订单行获取设备型号数据
        /// </summary>
        /// <param name="orderItemId">采购订单行</param>
        /// <returns>设备型号数据</returns>
        public virtual EquipModel GetEquipModelInfo(double orderItemId)
        {
            var orderItem = Query<PurchaseRequisitionItem>().Join<PurchaseOrderItem>((a, b) => a.Id == b.PurchaseRequisitionItemId && b.Id == orderItemId).FirstOrDefault();
            if (orderItem == null)
            {
                return null;
            }
            return Query<EquipModel>().Where(p => p.Code == orderItem.ObjectCode).FirstOrDefault();
        }

        /// <summary>
        /// 获取设备接收扫描信息
        /// </summary>
        /// <param name="receiveId">设备接收id</param>
        /// <returns>设备接收扫描信息</returns>
        public virtual ReceiveScanViewModel GetReceiveScanInfo(double receiveId)
        {
            var receive = GetEquipmentReceiveById(receiveId);
            var model = new ReceiveScanViewModel();
            model.ReceiveNo = receive.ReceiveNo;
            model.FactoryName = receive.FactoryName;
            model.DepartmentName = receive.DepartmentName;
            model.ReceiveType = receive.ReceiveType;
            return model;
        }

        /// <summary>
        /// 创建一个新的设备接收
        /// </summary>
        /// <returns>新的设备接收</returns>
        public virtual EquipmentReceive GetNewEquipmentReceive()
        {
            var entity = new EquipmentReceive();
            entity.ReceiveNo = RT.Service.Resolve<CommonController>().GetNo<EquipmentReceive>("设备接收");
            //创建时由待提交改为待接收
            entity.ReceiveBillStatus = ReceiveBillStatus.ToBeReceived;
            entity.ReceiveType = ReceiveType.Purchase;
            entity.AcceptanceType = AcceptanceType.Single;
            return entity;
        }

        /// <summary>
        /// 删除前校验最新状态
        /// </summary>
        /// <param name="ids">实体id</param>
        public virtual void DeleteEquipmentReceive(List<double> ids)
        {
            var entity = GetEquipmentReceivesByIds(ids);
            if (entity.Any(p => p.ReceiveBillStatus != ReceiveBillStatus.ToBeReceived))
            {
                throw new ValidationException("只有状态为【{0}】的数据才能删除"
                    .L10nFormat(ReceiveBillStatus.ToBeReceived.ToLabel()));
            }
            var allSnList = RT.Service.Resolve<EquipmentReceiveSnController>().GetReceiveSnList(ids);
            var equipCodes = allSnList.Select(p => p.EquipmentCode).Distinct().ToList();
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                DB.Delete<EquipmentReceive>().Where(p => ids.Contains(p.Id)).Execute();
                DB.Delete<EquipAccount>().Where(p => equipCodes.Contains(p.Code)).Execute();
                trans.Complete();
            }
        }

        /// <summary>
        /// 保存设备接收
        /// </summary>
        /// <param name="receive">设备接收</param>
        public virtual void SaveEquipmentReceive(EquipmentReceive receive)
        {
            if (receive == null)
            {
                throw new ValidationException("保存失败，数据异常".L10N());
            }

            if (receive.DetailList.Any(x => x.WorkshopId == null && x.WarehouseId == null))
            {
                throw new ValidationException("【接收仓库】和【接收车间】不能同时为空。".L10N());
            }

            if (receive.DetailList.Any(x => x.WorkshopId != null && x.WarehouseId != null))
            {
                throw new ValidationException("【接收仓库】和【接收车间】不能同时有值。".L10N());
            }

            if (receive.PersistenceStatus != PersistenceStatus.New)
            {
                var old = GetById<EquipmentReceive>(receive.Id);
                if (old == null)
                {
                    throw new ValidationException("保存失败，数据异常".L10N());
                }

                //待接收的单据才能修改
                if (old.ReceiveBillStatus != ReceiveBillStatus.ToBeReceived)
                {
                    throw new ValidationException("保存失败，状态为【{0}】的数据才能修改".L10nFormat(ReceiveBillStatus.ToBeReceived));
                }
            }

            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                RF.Save(receive);

                var details = GetDetailsByReceiveIds(new List<double> { receive.Id });

                if (details.Any(p => p.RecivedQty > 0))
                {
                    throw new ValidationException("接收明细存在【已接收数量】不为0时，不允许修改".L10N());
                }

                if (details.Any(p => p.Qty < p.RecivedQty))
                {
                    throw new ValidationException("接收数量不能小于已经接收的数量".L10N());
                }

                var orderItemIds = details.Where(p => p.PurchaseOrderItemId != null).Select(p => (double)p.PurchaseOrderItemId).Distinct().ToList();
                var orderItems = RT.Service.Resolve<PurchaseOrderController>().GetPurDetailsByIds(orderItemIds);

                if (receive.ReceiveType == ReceiveType.Purchase)
                {
                    var noGiveaways = details.Where(p => !p.Giveaway).ToList();//找到非赠品的数据
                    if (noGiveaways.Any(p => p.PurchaseOrderId == null || p.PurchaseOrderItemId == null))
                    {
                        throw new ValidationException("接收类型为采购接收且不是赠品时,采购单号和采购行号必输".L10N());
                    }
                    foreach (var noGiveaway in noGiveaways)
                    {
                        var orderItem = orderItems.FirstOrDefault(p => p.Id == noGiveaway.PurchaseOrderItemId);
                        if (orderItem == null)
                        {
                            throw new ValidationException("找不到id为:{0}的采购订单行".L10nFormat(noGiveaway.PurchaseOrderItemId));
                        }
                        var qty = GetNoGiveawaysQty(noGiveaway);
                        if (qty > orderItem.Qty + orderItem.RejectQty)//接收数量累计不能大于采购数量+拒收数量
                        {
                            throw new ValidationException("采购订单号:{0}，行号{1}接收数量大于采购数量".L10nFormat(noGiveaway.PurOrderNo, orderItem.LineNo));
                        }
                    }
                    if (details.Any(p => p.Giveaway && p.Price != 0))
                    {
                        throw new ValidationException("接收类型为【采购接收】且是赠品时，单价只能为0".L10N());
                    }
                }

                if ((receive.ReceiveType == ReceiveType.Customer || receive.ReceiveType == ReceiveType.Lease || receive.ReceiveType == ReceiveType.Other)
                   && details.Any(p => p.PurchaseOrderId != null || p.PurchaseOrderItemId != null))
                {
                    throw new ValidationException("采购单号和采购行号只能为空".L10N());
                }

                if (receive.ReceiveType == ReceiveType.Customer && details.Any(p => p.CustomerId == null))
                {
                    throw new ValidationException("接收类型为【客供接收】时，客户必输".L10N());
                }

                if (receive.ReceiveType == ReceiveType.Giveaway)
                {
                    if (details.Any(p => !p.Giveaway))
                    {
                        throw new ValidationException("接收类型为【赠品接收】时，明细必须是赠品".L10N());
                    }

                    if (details.Any(p => p.Price != 0))
                    {
                        throw new ValidationException("接收类型为【赠品接收】时，单价只能为0".L10N());
                    }

                    var warehouseIds = details.Where(p => p.WarehouseId != null)
                        .Select(p => p.WarehouseId.Value).Distinct().ToList();

                    var warehouseList = RT.Service.Resolve<WarehouseController>().GetWarehouses(warehouseIds);

                    if (warehouseList.Any(p => p.GetProperty(Warehouses.WarehouseExtension.IsZeroCostProperty) !=true))
                    {
                        throw new ValidationException("接收类型为【赠品接收】时，接收仓库只能是零成本仓".L10N());
                    }
                }

                //保存后再更新品种数和总数量
                receive.VarietyQuantity = details.Select(p => p.EquipModelId).Distinct().Count();
                receive.TotalQty = details.Sum(p => p.Qty);
                RF.Save(receive);
                trans.Complete();
            }
        }

        /// <summary>
        /// 获取接收明细的非赠品接收数量汇总
        /// </summary>
        /// <param name="noGiveaway">接收明细</param>
        /// <returns>接收数量汇总</returns>
        private int GetNoGiveawaysQty(EquipmentReceiveDetail noGiveaway)
        {
            return Query<EquipmentReceiveDetail>().Join<EquipmentReceive>((a, b) => a.EquipmentReceiveId == b.Id
                && b.ReceiveType == ReceiveType.Purchase)
                .Where(p => p.PurchaseOrderItemId == noGiveaway.PurchaseOrderItemId && !p.Giveaway).Select(p => p.Qty).ToList<int>().Sum();
        }

        /// <summary>
        /// 设备接收扫描-扫描设备编码
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">设备接收模型</param>
        /// <returns>扫描信息</returns>
        public virtual ReceiveExecuteInfo ScanEquipExecute(string sn, ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var snInfo = GetReceiveScanSnViewModel(model);
            var info = new ReceiveExecuteInfo();
            var equip = RT.Service.Resolve<EquipController>().GetEquipAccount(p => p.Code == sn);
            if (model.ReceiveType == ReceiveType.Outsourced)
            {
                if (equip == null || equip.UseState != AccountUseState.OutsourcedRepair || equip.EquipModelId != model.EquipModelId)
                {
                    info.Success = false;
                    info.Message = "请扫描委外维修的设备编码".L10N();
                    return info;
                }
                snInfo.OriginalSn = equip.OriginalSerialNumber;
            }
            else
            {
                if (equip != null)
                {
                    info.Success = false;
                    info.Message = "设备编码{0}已存在于设备台账中，请确认".L10nFormat(sn);
                    return info;
                }
                var card = Query<EquipmentCard>().Where(p => p.Code == sn).Count();
                if (card > 0)
                {
                    info.Success = false;
                    info.Message = "设备编码{0}已存在于设备立卡中，请确认".L10nFormat(sn);
                    return info;
                }
            }
            snInfo.EquipmentCode = sn;
            info.Message = "请扫描设备编码".L10N();
            info.Success = true;
            info.SnInfo = snInfo;
            return info;
        }

        /// <summary>
        /// 设备接收扫描-扫描原厂序列号
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">设备接收模型</param>
        /// <returns>扫描信息</returns>
        public virtual ReceiveExecuteInfo ScanSnExecute(string sn, ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var snInfo = GetReceiveScanSnViewModel(model);
            var info = new ReceiveExecuteInfo();
            var equip = RT.Service.Resolve<EquipController>().GetEquipAccount(p => p.OriginalSerialNumber == sn);
            if (model.ReceiveType == ReceiveType.Outsourced)
            {
                if (equip == null || equip.UseState != AccountUseState.OutsourcedRepair || equip.EquipModelId != model.EquipModelId)
                {
                    info.Success = false;
                    info.Message = "请扫描委外维修的原厂序列号".L10N();
                    return info;
                }
                snInfo.EquipmentCode = equip.Code;
            }
            else
            {
                if (equip != null)
                {
                    info.Success = false;
                    info.Message = "原厂序列号{0}已存在于设备台账中，请确认".L10nFormat(sn);
                    return info;
                }
                var card = Query<EquipmentCard>().Where(p => p.OriginalSerialNumber == sn).Count();
                if (card > 0)
                {
                    info.Success = false;
                    info.Message = "原厂序列号{0}已存在于设备立卡中，请确认".L10nFormat(sn);
                    return info;
                }
                snInfo.EquipmentCode = RT.Service.Resolve<EquipAccountController>().GetAccountNo();
                if (snInfo.EquipmentCode.IsNullOrWhiteSpace())
                {
                    info.Success = false;
                    info.Message = "请维护设备编码编码规则".L10N();
                    return info;
                }
            }
            snInfo.OriginalSn = sn;
            info.Message = "请扫描原厂序列号".L10N();
            info.Success = true;
            info.SnInfo = snInfo;
            return info;
        }

        /// <summary>
        /// 设备接收扫描-扫描设备编码+原厂序列号
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="model">设备接收模型</param>
        /// <returns>扫描信息</returns>
        public virtual ReceiveExecuteInfo ScanEquipAndSnExecute(string sn, ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var snInfo = GetReceiveScanSnViewModel(model);
            var info = new ReceiveExecuteInfo();
            if (model.FirstSn.IsNullOrWhiteSpace())
            {
                var card = Query<EquipmentCard>().Where(p => p.Code == sn).Count();
                if (card > 0)
                {
                    info.Success = false;
                    info.Message = "设备编码{0}已存在于设备立卡中，请确认".L10nFormat(sn);
                    return info;
                }
                var equip = RT.Service.Resolve<EquipController>().GetEquipAccount(p => p.Code == sn);
                if (equip != null)
                {
                    info.Success = false;
                    info.Message = "设备编码{0}已存在于设备台账中，请确认".L10nFormat(sn);
                    return info;
                }
                else
                {
                    info.Success = true;
                    info.IsFirstSn = true;
                    info.Message = "请扫描原厂序列号".L10N();
                    return info;
                }
            }
            else
            {
                var equip = RT.Service.Resolve<EquipController>().GetEquipAccount(p => p.OriginalSerialNumber == sn);
                if (equip != null)
                {
                    info.Success = false;
                    info.Message = "原厂序列号{0}已存在于设备台账中，请确认".L10nFormat(sn);
                    return info;
                }
                var card = Query<EquipmentCard>().Where(p => p.OriginalSerialNumber == sn).Count();
                if (card > 0)
                {
                    info.Success = false;
                    info.Message = "原厂序列号{0}已存在于设备立卡中，请确认".L10nFormat(sn);
                    return info;
                }
                snInfo.EquipmentCode = model.FirstSn;
                snInfo.OriginalSn = sn;
                info.Message = "请扫描设备编码".L10N();
            }
            info.Success = true;
            info.SnInfo = snInfo;
            return info;
        }

        /// <summary>
        /// 生成设备接收序列号模型
        /// </summary>
        /// <param name="model">设备接收模型</param>
        /// <returns>设备接收序列号模型</returns>
        private ReceiveScanSnViewModel GetReceiveScanSnViewModel(ReceiveScanViewModel model)
        {
            var receiveDetail = model.EquipmentReceiveDetail;
            if (receiveDetail == null)
            {
                throw new ValidationException("请选择接收明细后再扫码".L10N());
            }
            var snInfo = new ReceiveScanSnViewModel();
            snInfo.EquipmentReceiveDetailId = model.EquipmentReceiveDetailId;
            snInfo.ReceiveLineNo = receiveDetail.LineNo;
            snInfo.PurchaseOrderNo = receiveDetail.PurchaseOrder?.OrderNo;
            snInfo.OrderItemNo = receiveDetail.PurchaseOrderItem?.LineNo;
            snInfo.EquipModelCode = model.EquipModel?.Code;
            snInfo.EquipModelName = model.EquipModelName;
            snInfo.TechnicalNorm = model.EquipModel?.Specifications;
            return snInfo;
        }

        /// <summary>
        /// 生成设备接收序列号模型
        /// </summary>
        /// <param name="model">设备接收模型</param>
        /// <returns>设备接收序列号模型</returns>
        private ReceiveScanSnViewModel GetReceiveScanSnViewModel(EquipmentReceiveSn model)
        {
            var snInfo = new ReceiveScanSnViewModel();
            snInfo.Id = model.Id.ToString();
            snInfo.EquipmentReceiveDetailId = model.EquipmentReceiveDetailId;
            snInfo.ReceiveLineNo = model.ReceiveLineNo;
            snInfo.PurchaseOrderNo = model.PurchaseOrderNo;
            snInfo.OrderItemNo = model.OrderItemNo;
            snInfo.EquipmentCode = model.EquipmentCode;
            snInfo.EquipModelCode = model.EquipModelCode;
            snInfo.EquipModelName = model.EquipModelName;
            snInfo.OriginalSn = model.OriginalSn;
            snInfo.TechnicalNorm = model.EquipmentReceiveDetail.EquipModel?.Specifications;
            return snInfo;
        }

        /// <summary>
        /// 委外返厂确定
        /// </summary>
        /// <param name="model">设备接收模型</param>
        /// <returns>序列号信息</returns>
        public virtual ReceiveExecuteInfo Determine(ReceiveScanViewModel model)
        {
            var info = new ReceiveExecuteInfo();
            var equip = RT.Service.Resolve<EquipController>().GetEquipAccount(p => p.Id == model.EquipAccountId);
            if (equip == null)
            {
                throw new ValidationException("数据异常，找不到id为:{0}的设备".L10nFormat(model.EquipAccountId));
            }
            var snInfo = GetReceiveScanSnViewModel(model);
            snInfo.OriginalSn = equip.OriginalSerialNumber;
            snInfo.EquipmentCode = equip.Code;
            info.SnInfo = snInfo;
            return info;
        }

        /// <summary>
        /// 获取序列号列表信息
        /// </summary>
        /// <param name="ReceiveId"></param>
        /// <returns></returns>
        public virtual EntityList<ReceiveScanSnViewModel> GetEquipmentReceiveSnInfo(double ReceiveId, IList<OrderInfo> sortInfo, PagingInfo pagingInfo)
        {
            EntityList<ReceiveScanSnViewModel> models = new EntityList<ReceiveScanSnViewModel>();
            EntityList<EquipmentReceiveSn> EquipmentReceiveSnList = RT.Service.Resolve<EquipmentReceiveSnController>().GetReceiveSnInfo(ReceiveId, sortInfo, pagingInfo);
            foreach (var EquipmentReceiveSn in EquipmentReceiveSnList)
            {
                models.Add(GetReceiveScanSnViewModel(EquipmentReceiveSn));
            }
            return models;
        }


        /// <summary>
        /// 非委外返厂确定
        /// </summary>
        /// <param name="model">设备接收模型</param>
        /// <returns>序列号信息</returns>
        public virtual List<ReceiveScanSnViewModel> DetermineOnQty(ReceiveScanViewModel model)
        {
            if (model == null)
            {
                throw new ValidationException("数据异常".L10N());
            }
            var snInfo = GetReceiveScanSnViewModel(model);
            var infos = new List<ReceiveScanSnViewModel>();
            for (var i = 0; i < model.CurrentQty; i++)
            {
                var info = new ReceiveScanSnViewModel();
                info.EquipmentReceiveDetailId = model.EquipmentReceiveDetailId;
                info.ReceiveLineNo = snInfo.ReceiveLineNo;
                info.PurchaseOrderNo = snInfo.PurchaseOrderNo;
                info.OrderItemNo = snInfo.OrderItemNo;
                info.EquipModelCode = snInfo.EquipModelCode;
                info.EquipModelName = snInfo.EquipModelName;
                info.TechnicalNorm = snInfo.TechnicalNorm;
                info.EquipmentCode = RT.Service.Resolve<EquipAccountController>().GetAccountNo();
                infos.Add(info);
            }
            return infos;
        }

        /// <summary>
        /// 保存设备接收扫描
        /// </summary>
        /// <param name="model">设备接收模型</param>
        /// <param name="snList">序列号信息</param>
        public virtual void SaveReceiveScan(ReceiveScanViewModel model, List<ReceiveScanSnViewModel> snList)
        {
            if (model == null || snList == null || !snList.Any())
            {
                throw new ValidationException("数据异常，保存数据为空".L10N());
            }

            // 设备接收单
            var receive = GetById<EquipmentReceive>(model.EquipmentReceiveId);

            if (receive == null)
            {
                throw new ValidationException("数据异常，找不到id为：{0}的设备接收".L10nFormat(model.EquipmentReceiveId));
            }

            if (receive.ReceiveBillStatus == ReceiveBillStatus.Completed)
            {
                throw new ValidationException("设备接收单【{0}】的状态已完成，不能修改。".L10nFormat(model.ReceiveNo));
            }

            // 设备接收明细
            var details = GetDetailsByReceiveIds(new List<double> { receive.Id });
            // 是否写入设备立卡
            var enableEquipCard = RT.Service.Resolve<EquipAccountController>().GetUseCard();
            // 数据库时间
            var now = RF.Find<EquipmentReceive>().GetDbTime();

            // 接收序列号明细
            EntityList<EquipmentReceiveSn> saveSnList = new EntityList<EquipmentReceiveSn>();
            // 写入设备立卡
            EntityList<EquipmentCard> saveCardList = new EntityList<EquipmentCard>();
            // 写入设备台账
            EntityList<EquipAccount> saveAccountList = new EntityList<EquipAccount>();

            foreach (var sn in snList)
            {
                var receiveDetail = details.FirstOrDefault(p => p.Id == sn.EquipmentReceiveDetailId);

                if (receiveDetail == null)
                {
                    throw new ValidationException("数据异常，找不到id为：{0}的接收明细".L10nFormat(sn.EquipmentReceiveDetailId));
                }

                var receiveSn = new EquipmentReceiveSn();
                receiveSn.EquipmentReceiveDetailId = sn.EquipmentReceiveDetailId;
                receiveSn.EquipmentCode = sn.EquipmentCode;
                receiveSn.OriginalSn = sn.OriginalSn;
                receiveSn.IsOriginalEquipment = model.ReceiveType == ReceiveType.Outsourced;
                saveSnList.Add(receiveSn);

                receiveDetail.RecivedQty += 1;

                if (receiveDetail.RecivedQty > receiveDetail.Qty)
                {
                    throw new ValidationException("行号：{0}已接收数量不能大于接收数量".L10nFormat(receiveDetail.LineNo));
                }

                //不是【委外返厂】时写入设备立卡/设备台账表
                if (model.ReceiveType != ReceiveType.Outsourced)
                {
                    if (enableEquipCard)
                    {
                        GenerateEquipmentCard(receive, receiveDetail, sn, now, saveCardList);
                    }
                    else
                    {
                        GenerateEquipAccount(receive, receiveDetail, sn, now, saveAccountList);
                    }
                }
            }
            using (var trans = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
            {
                // 保存序列号明细
                if (saveSnList.Any())
                {
                    RF.BatchInsert(saveSnList);
                }
                // 新增设备立卡
                if (saveCardList.Any())
                {
                    RF.BatchInsert(saveCardList);
                }
                // 新增设备台账并同步到当前登陆人的设备与人员权限设备清单中
                if (saveAccountList.Any())
                {
                    RF.BatchInsert(saveAccountList);

                    RT.Service.Resolve<IEquipAccount>().SynDevicePur(saveAccountList.Select(p => p.Id).ToList());
                }
                
                // 保存接收明细
                RF.Save(details);

                if (!details.Any(x => x.RecivedQty < x.Qty))
                {
                    //全部接收时，更新接收单状态为待提交
                    receive.ReceiveBillStatus = ReceiveBillStatus.ToBeSubmitted;

                    RF.Save(receive);
                }

                trans.Complete();
            }
        }

        /// <summary>
        /// 写入设备立卡
        /// </summary>
        /// <param name="receive">接收</param>
        /// <param name="receiveDetail">接收明细</param>
        /// <param name="sn">序列号</param>
        /// <param name="now">当前时间</param>
        /// <param name="saveCardList">设备立卡保存列表</param>
        private void GenerateEquipmentCard(EquipmentReceive receive, EquipmentReceiveDetail receiveDetail, ReceiveScanSnViewModel sn, DateTime now, EntityList<EquipmentCard> saveCardList)
        {
            var equipCard = new EquipmentCard();
            equipCard.Code = sn.EquipmentCode;
            equipCard.NeedAcceptance = false;
            equipCard.OriginalSerialNumber = sn.OriginalSn;
            equipCard.PurchaseOrderNo = receiveDetail.PurOrderNo;
            equipCard.EnterDate = now;
            equipCard.IsCustomsSupervision = false;
            equipCard.IssAsset = false;
            equipCard.IsChange = true;
            equipCard.FactoryId = receive.FactoryId;
            equipCard.ApprovalStatus = ApprovalStatus.Draft;
            equipCard.EquipmentCardSource = GetEquipmentCardSource(receive.ReceiveType);
            equipCard.EquipModelId = receiveDetail.EquipModelId;
            equipCard.AccountState = AccountState.Running;
            equipCard.AccountUseState = AccountUseState.InIdle;
            equipCard.SupplierId = receiveDetail.SupplierId;
            if (receive.ReceiveType == ReceiveType.Lease)
            {
                equipCard.Proprietorship = Proprietorship.Lease;
                equipCard.PurchaseUnit = receiveDetail.SupplierName;
            }
            else if (receive.ReceiveType == ReceiveType.Customer)
            {
                equipCard.Proprietorship = Proprietorship.ByCustomer;
                equipCard.PurchaseUnit = receiveDetail.CustomerName;
            }
            else
            {
                equipCard.Proprietorship = Proprietorship.Own;
            }
            saveCardList.Add(equipCard);
        }

        /// <summary>
        /// 获取卡片来源类型
        /// </summary>
        /// <param name="receiveType">接收类型</param>
        /// <returns>卡片来源类型</returns>
        private EquipmentCardSource GetEquipmentCardSource(ReceiveType receiveType)
        {
            switch (receiveType)
            {
                case ReceiveType.Purchase:
                    return EquipmentCardSource.EquipmentReceive;
                case ReceiveType.Giveaway:
                    return EquipmentCardSource.Giveaway;
                case ReceiveType.Customer:
                    return EquipmentCardSource.Customer;
                case ReceiveType.Lease:
                    return EquipmentCardSource.Lease;
                case ReceiveType.Outsourced:
                    return EquipmentCardSource.EquipmentReceive;
                default:
                    return EquipmentCardSource.Other;
            }
        }

        /// <summary>
        /// 写入设备台账表
        /// </summary>
        /// <param name="receive">接收</param>
        /// <param name="receiveDetail">接收明细</param>
        /// <param name="sn">序列号</param>
        /// <param name="now">当前时间</param>
        /// <param name="saveAccountList">保存设备台账列表</param>
        private void GenerateEquipAccount(EquipmentReceive receive, EquipmentReceiveDetail receiveDetail, ReceiveScanSnViewModel sn, DateTime now, EntityList<EquipAccount> saveAccountList)
        {
            var equipAccount = new EquipAccount();
            equipAccount.FactoryId = receive.FactoryId;
            equipAccount.Code = sn.EquipmentCode;
            equipAccount.EquipModelId = receiveDetail.EquipModelId;
            equipAccount.OriginalSerialNumber = sn.OriginalSn;
            equipAccount.UseState = AccountUseState.ToAccepted;
            equipAccount.State = AccountState.Running;
            equipAccount.Frozen = YesNo.No;
            equipAccount.IsVirtual = YesNo.No;
            equipAccount.SupplierId = receiveDetail.SupplierId;
            equipAccount.PurchaseOrderNo = receiveDetail.PurOrderNo;
            equipAccount.EnterDate = now;
            if (receive.ReceiveType == ReceiveType.Lease)
            {
                equipAccount.Proprietorship = Proprietorship.Lease;
                equipAccount.PurchaseUnit = receiveDetail.SupplierName;
            }
            else if (receive.ReceiveType == ReceiveType.Customer)
            {
                equipAccount.Proprietorship = Proprietorship.ByCustomer;
                equipAccount.PurchaseUnit = receiveDetail.CustomerName;
            }
            else
            {
                equipAccount.Proprietorship = Proprietorship.Own;
            }
            saveAccountList.Add(equipAccount);
        }
    }
}
