using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.WorkOrders;
using SIE.LES.Interfaces;
using SIE.LES.Interfaces.Datas;
using SIE.LES.RetreatItemManage.Configs;
using SIE.LES.RetreatItemManage.MaterialReturns;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.LES.RetreatItemManage
{
    /// <summary>
    /// 控制器
    /// </summary>
    public class MaterialReturnController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="criteria"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturn> Fetch(MaterialReturnCriteria criteria)
        {
            var qureyList = Query<MaterialReturn>();
            if (!criteria.NO.IsNullOrEmpty())
            {
                qureyList.Where(m => m.NO.Contains(criteria.NO));
            }
            if (criteria.ReturnType.HasValue)
            {
                qureyList.Where(m => m.ReturnType == criteria.ReturnType);
            }
            if (criteria.ReturnState.HasValue)
            {
                qureyList.Where(m => m.ReturnState == criteria.ReturnState);
            }
            if (criteria.ItemCode.IsNotEmpty())
            {
                qureyList.Where(m => m.Item.Code.Contains(criteria.ItemCode));
            }
            if (criteria.ItemName.IsNotEmpty())
            {
                qureyList.Where(m => m.Item.Name.Contains(criteria.ItemName));
            }
            if (criteria.Label.IsNotEmpty())
            {
                qureyList.Where(m => m.Label.Contains(criteria.Label));
            }
            if (criteria.BatchNO.IsNotEmpty())
            {
                qureyList.Where(m => m.BatchNO.Contains(criteria.BatchNO));
            }
            if (criteria.WorkOrder.IsNotEmpty())
            {
                qureyList.Where(m => m.WorkOrder.No.Contains(criteria.WorkOrder));
            }
            if (criteria.FactoryId.HasValue)
            {
                qureyList.Where(m => m.FactoryId == criteria.FactoryId);
            }
            if (criteria.WipResourceId.HasValue)
            {
                qureyList.Where(m => m.WipResourceId == criteria.WipResourceId);
            }
            if (criteria.EmployeeId.HasValue)
            {
                qureyList.Where(m => m.EmployeeId == criteria.EmployeeId);
            }
            if (criteria.SubmitDate.BeginValue.HasValue)
            {
                qureyList.Where(m => m.SubmitDate >= criteria.SubmitDate.BeginValue);
            }
            if (criteria.SubmitDate.EndValue.HasValue)
            {
                qureyList.Where(m => m.SubmitDate <= criteria.SubmitDate.EndValue);
            }

            return qureyList.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// ，明细提交
        /// </summary>
        /// <param name="entityCur"></param>
        public virtual void DetailSubmit(MaterialReturn entityCur)
        {
            if (entityCur.PersistenceStatus == PersistenceStatus.Modified || entityCur.PersistenceStatus == PersistenceStatus.New)
            {
                RT.Service.Resolve<MaterialReturnController>().ValideReturnMaterial(entityCur);
            }
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                entityCur.ReturnState = ReturnStates.Submitted;
                RF.Save(entityCur);
                HandleSubmit(new EntityList<MaterialReturn>() { entityCur });
                tran.Complete();
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="isMove">是否挪料</param>
        public virtual void Submit(List<double> ids, bool isMove = false)
        {
            var materialReturns = Query<MaterialReturn>().Where(m => ids.Contains(m.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            HandleSubmit(materialReturns, isMove);
        }

        /// <summary>
        /// 处理提交
        /// </summary>
        /// <param name="materialReturns"></param>
        /// <param name="isMove">是否挪料</param>
        public virtual void HandleSubmit(EntityList<MaterialReturn> materialReturns, bool isMove = false)
        {
            var isMust = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialMustReturnReason();
            var dbtime = RF.Find<MaterialReturn>().GetDbTime();
            EntityList<ItemLabel> itemLabels = new EntityList<ItemLabel>();
            EntityList<MaterialWithdrawalRecord> drawRecords = new EntityList<MaterialWithdrawalRecord>();

            var nowDate = RF.Find<MaterialWithdrawalRecord>().GetDbTime();
            var wipResourceIds = materialReturns.Where(p => p.WipResourceId != null).Select(p => (double)p.WipResourceId).ToList();
            var wipResources = RT.Service.Resolve<WipResourceController>().GetResourceTypeByResourceIds(wipResourceIds);
            using (var tran = DB.TransactionScope(LESEntityDataProvider.ConnectionStringName))
            {
                //单次单笔提交
                foreach (var res in materialReturns)
                {
                    if (isMust && res.ReturnReason.IsNullOrEmpty())
                    {
                        throw new ValidationException("退料原因必填！请填写".L10N());
                    }
                    if (res.Qty <= 0)
                    { throw new ValidationException("退料数量必须大于0！请修改".L10N()); }

                    RT.Service.Resolve<MaterialReturnController>().ValideReturnMaterial(res);
                    res.ReturnState = ReturnStates.Submitted;
                    res.SubmitDate = dbtime;
                    res.EmployeeId = RT.IdentityId;
                    //扣减标签数量、扣减标签关联工单
                    var label = RF.GetById<ItemLabel>(res.LabelId, new EagerLoadOptions().LoadWith(ItemLabel.WorkOrderListProperty));
                    if (label == null)
                    {
                        throw new ValidationException("物料标签不存在或当前账号没有物料标签对应工厂的权限！请检查".L10N());

                    }

                    if (res.ReturnType == ReturnTypes.Bad)//不良退料
                    {
                        label.NgQty -= res.Qty;
                    }
                    else
                    {
                        if (res.WorkOrderId.HasValue)
                        {
                            label.Qty -= res.Qty;
                            var workeOrderlation = label.WorkOrderList.FirstOrDefault(m => m.WorkOrderId == res.WorkOrderId);
                            if (workeOrderlation != null)
                            {
                                workeOrderlation.Qty -= res.Qty;//扣减标签关联工单
                                if (workeOrderlation.Qty == 0)//减到0 删除该行数据
                                {
                                    workeOrderlation.PersistenceStatus = PersistenceStatus.Deleted;
                                    RF.Save(workeOrderlation);
                                }
                            }
                        }
                        else
                        {
                            if (label.Qty >= res.Qty)
                            {
                                label.Qty -= res.Qty;
                            }
                        }

                    }

                    List<ReturnSnData> returnSnDatas = new List<ReturnSnData>();
                    returnSnDatas.Add(new ReturnSnData()
                    {
                        Sn = res.Label,
                        ItemId = res.ItemId,
                        ItemExtProp = label.ItemExtProp,
                        ItemExtPropName = label.ItemExtPropName,
                        LotCode = label.Lot,
                        Qty = res.Qty,
                        SourceStorageLocationId = res.ReturnWarehouseLocationId,
                        SourceWarehouseId = res.ReturnWarehouseId,
                        WoNo = res.WorkOrder != null ? res.WorkOrder.No : "",
                        SourceWarehouseCode = res.ReturnWarehouseId.HasValue ? res.ReturnWarehouse.Code : "",
                        IsFail = res.ReturnType == ReturnTypes.Bad
                    });

                    itemLabels.Add(label);
                    ////调用WMS接口------挪料不需要调用退料接口 2023.5.31 徐                  
                    if (!isMove)//非挪料需调用
                    {
                        RT.Service.Resolve<StockOrderController>().ReturnSnUpdate(returnSnDatas, res.NO);
                    }

                    // 生产退料记录
                    MaterialWithdrawalRecord curCallMaterialWithdrawal = new MaterialWithdrawalRecord();
                    curCallMaterialWithdrawal.ItemLabel = res.Label;
                    curCallMaterialWithdrawal.RemainQty = res.Qty;
                    curCallMaterialWithdrawal.WithdrawalQty = res.Qty;
                    curCallMaterialWithdrawal.BatchNo = res.BatchNO;
                    curCallMaterialWithdrawal.WithdrawalDate = nowDate;
                    curCallMaterialWithdrawal.WithdrawalById = RT.IdentityId;
                    curCallMaterialWithdrawal.ItemId = res.ItemId;
                    if (res.WipResourceId != 0)
                    {
                        curCallMaterialWithdrawal.ResourceId = res.WipResourceId;
                    }
                    if (res.WorkOrderId != 0)
                    {
                        curCallMaterialWithdrawal.WorkOrderId = res.WorkOrderId;
                    }
                    curCallMaterialWithdrawal.WorkShopId = wipResources.FirstOrDefault(p => p.Id == res.WipResourceId)?.WorkShopId;
                    drawRecords.Add(curCallMaterialWithdrawal);
                }
                RF.Save(materialReturns);
                RF.Save(itemLabels);
                RF.Save(drawRecords);
                tran.Complete();
            }
        }

        /// <summary>
        /// API提交生产退料(性能优化版20240320lyp)
        /// </summary>
        /// <param name="materialReturns">生产退料</param>
        public virtual void ApiHandleSubmit(EntityList<MaterialReturn> materialReturns)
        {
            // 工厂权限
            var factoryIds = Query<EmployeeEnterprise>().Where(y => y.EmployeeId == RT.IdentityId).Select(p => p.EnterpriseId).Distinct().ToList<double>().ToList();

            //校验仓库权限
            var warehouseIds = RT.Service.Resolve<WarehouseController>().GetAuthorityWarehouseId();

            //重新获取标签可用数量AlreadyQty
            var itemLabelIds = materialReturns.Select(p => p.LabelId).Distinct().ToList();
            var itemLabels = RT.Service.Resolve<ItemLabelController>().GetReceiveItemLabels(itemLabelIds);
            // 物料标签投入工单
            var itemLabelWos = RT.Service.Resolve<ItemLabelController>().GetItemLabelWorkOrders(itemLabelIds);

            var isMust = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialMustReturnReason();
            var dbtime = RF.Find<MaterialReturn>().GetDbTime();



            foreach (var materialReturn in materialReturns)
            {
                var itemLabel = itemLabels.FirstOrDefault(x => x.Id == materialReturn.LabelId);
                if (materialReturn.ReturnWarehouseId.HasValue)
                {
                    if (!factoryIds.Contains(materialReturn.FactoryId) && !warehouseIds.Contains(materialReturn.ReturnWarehouseId.Value))
                    {
                        throw new ValidationException("【{0}】退料保存失败，操作员工没有该标签的工厂权限或仓库权限，请检查".L10nFormat(materialReturn.Label));
                    }
                }
                if (materialReturn.ReturnType == ReturnTypes.Bad && materialReturn.Qty > materialReturn.BadQty)
                {
                    throw new ValidationException("【{0}】退料保存失败，退料数不能大于标签的不良数量".L10nFormat(materialReturn.Label));
                }
                if (materialReturn.ReturnType == ReturnTypes.Normal && itemLabel != null && materialReturn.Qty > itemLabel.Qty)
                {
                    if (materialReturn.WorkOrderId.HasValue)
                    {
                        throw new ValidationException("【{0}】退料保存失败，本次退料关联工单【{1}】的数量为{2},物料标签数量为{3}".L10nFormat(materialReturn.Label,
                            materialReturn.WorkOrder.No, materialReturn.AlreadyQty, itemLabel.Qty));
                    }
                    else
                    {
                        throw new ValidationException("【{0}】退料保存失败，本次退料剩余可用的数量为{1}".L10nFormat(materialReturn.Label, itemLabel.Qty));
                    }
                }
                if (isMust && materialReturn.ReturnReason.IsNullOrEmpty())
                {
                    throw new ValidationException("退料原因必填！请填写".L10N());
                }
                if (materialReturn.Qty <= 0)
                { throw new ValidationException("退料数量必须大于0！请修改".L10N()); }

                materialReturn.ReturnState = ReturnStates.Submitted;
                materialReturn.SubmitDate = dbtime;
                materialReturn.EmployeeId = RT.IdentityId;

                if (itemLabel == null)
                {
                    throw new ValidationException("物料标签不存在或当前账号没有物料标签对应工厂的权限！请检查".L10N());

                }

                if (materialReturn.ReturnType == ReturnTypes.Bad)//不良退料
                {
                    itemLabel.NgQty -= materialReturn.Qty;
                }
                else
                {
                    if (materialReturn.WorkOrderId.HasValue)
                    {
                        itemLabel.Qty -= materialReturn.Qty;
                        var workeOrderlation = itemLabelWos.FirstOrDefault(m => m.ItemLabelId == itemLabel.Id && m.WorkOrderId == materialReturn.WorkOrderId);
                        if (workeOrderlation != null)
                        {
                            workeOrderlation.Qty -= materialReturn.Qty;//扣减标签关联工单
                            if (workeOrderlation.Qty == 0)//减到0 删除该行数据
                            {
                                workeOrderlation.PersistenceStatus = PersistenceStatus.Deleted;
                            }
                        }
                    }
                    else
                    {
                        if (itemLabel.Qty >= materialReturn.Qty)
                        {
                            itemLabel.Qty -= materialReturn.Qty;
                        }
                    }

                }

                // 退料数据
                List<ReturnSnData> returnSnDatas = new List<ReturnSnData>();
                returnSnDatas.Add(new ReturnSnData()
                {
                    Sn = materialReturn.Label,
                    ItemId = materialReturn.ItemId,
                    ItemExtProp = itemLabel.ItemExtProp,
                    ItemExtPropName = itemLabel.ItemExtPropName,
                    LotCode = itemLabel.Lot,
                    Qty = materialReturn.Qty,
                    SourceStorageLocationId = materialReturn.ReturnWarehouseLocationId,
                    SourceWarehouseId = materialReturn.ReturnWarehouseId,
                    WoNo = materialReturn.WorkOrder != null ? materialReturn.WorkOrder.No : "",
                    SourceWarehouseCode = materialReturn.ReturnWarehouseId.HasValue ? materialReturn.ReturnWarehouse.Code : "",
                    IsFail = materialReturn.ReturnType == ReturnTypes.Bad
                });

                // 20240320 需和wms讨论多单情况
                ////调用WMS接口------挪料不需要调用退料接口 2023.5.31 徐                  
                //非挪料需调用
                RT.Service.Resolve<StockOrderController>().ReturnSnUpdate(returnSnDatas, materialReturn.NO);

            }

            // 保存物料标签投入工单
            RF.Save(itemLabelWos.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).AsEntityList());

            // 保存物料标签
            RF.Save(itemLabels.Where(p => p.PersistenceStatus != PersistenceStatus.Unchanged).AsEntityList());

            // 保存退料单信息
            RF.Save(materialReturns);
        }

        /// <summary>
        /// 在结果中查找
        /// </summary>
        /// <param name="materialReturnForSelectCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturn> MaterialReturnForSelectFetch(MaterialReturnForSelectCriteria materialReturnForSelectCriteria)
        {
            EntityList<MaterialReturn> res = new EntityList<MaterialReturn>();
            EntityList<MaterialReturn> materialReturns = RT.Service.Resolve<MaterialReturnController>().GetSearch(materialReturnForSelectCriteria.Sn);
            if (!materialReturnForSelectCriteria.WorkOrder.IsNullOrEmpty())
            {
                var records = materialReturns.Where(m => m.WorkOrderId.HasValue && m.WorkOrder.No == materialReturnForSelectCriteria.WorkOrder);
                if (records.Any())
                {
                    res.AddRange(records);
                }
            }
            if (materialReturnForSelectCriteria.FactoryId.HasValue)
            {
                if (res.Any())
                {
                    EntityList<MaterialReturn> result = new EntityList<MaterialReturn>();
                    var resultItems = res.TakeWhile(m => m.FactoryId == materialReturnForSelectCriteria.FactoryId).ToList();
                    result.AddRange(resultItems);
                    return result;
                }
                else
                {
                    var records = materialReturns.Where(m => m.FactoryId == materialReturnForSelectCriteria.FactoryId);
                    if (records.Any())
                    {
                        res.AddRange(records);
                    }
                }
            }
            if (!res.Any() && !materialReturnForSelectCriteria.FactoryId.HasValue && materialReturnForSelectCriteria.WorkOrder.IsNullOrEmpty())
            {
                return materialReturns;
            }
            return res;
        }




        /// <summary>
        /// 撤回
        /// </summary>
        /// <param name="ids"></param>
        public virtual void Redraw(List<double> ids)
        {
            DB.Update<MaterialReturn>().Set(p => p.ReturnState, ReturnStates.Revoked).Where(m => ids.Contains(m.Id)).Execute();
        }

        /// <summary>
        /// 获取单号
        /// </summary>
        /// <returns></returns>
        public virtual string GetReturnMaterialCode()
        {
            var config = ConfigService.GetConfig(new ReturnMaterialConfig(), typeof(MaterialReturn));
            if (config == null || config.ReturnMaterialCodeRule == null)
                throw new ValidationException("未找到生产退料单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.ReturnMaterialCodeRuleId, 1).FirstOrDefault();
        }

        /// <summary>
        /// 获取退料原因必填
        /// </summary>
        /// <returns></returns>
        public virtual bool GetReturnMaterialMustReturnReason()
        {
            var config = ConfigService.GetConfig(new ReturnMaterialConfig(), typeof(MaterialReturn));
            if (config == null || config.ReturnMaterialCodeRule == null)
                throw new ValidationException("未找到生产退料单号生成规则,请检查规则配置".L10N());
            return config.ReasonRequired;
        }

        /// <summary>
        /// 获取生产退联的默认退料原因
        /// </summary>
        /// <returns></returns>
        public virtual string GetReturnMaterialReasonDefault()
        {
            var config = ConfigService.GetConfig(new ReturnMaterialConfig(), typeof(MaterialReturn));
            if (config == null || config.ReturnMaterialCodeRule == null)
                throw new ValidationException("未找到生产退料配置项,请检查规则配置".L10N());
            return config.ReasonDefault;
        }




        /// <summary>
        /// 获取搜索的退料信息
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public virtual EntityList<ItemLabel> RetrunSearch(string keyWord)
        {
            return Query<ItemLabel>().Where(m => m.Label == keyWord).OrderBy(p => new { p.Label, p.WarehouseId, p.StorageLocationId, p.WorkOrderId, p.NgQty }).
                  ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        ///保存新增退料
        /// </summary>
        /// <param name="materialReturn"></param>

        public virtual void ValideReturnMaterial(MaterialReturn materialReturn)
        {
            if (materialReturn.Qty <= 0)
            {
                throw new ValidationException("退料数量必须大于0".L10N());
            }
            //校验工厂权限
            var factoryIds = GetAuthorityFactoryId();


            //校验仓库权限
            var warehouseIds = RT.Service.Resolve<WarehouseController>().GetAuthorityWarehouseId();

            //重新获取标签可用数量AlreadyQty

            var itemLabel = RF.GetById<ItemLabel>(materialReturn.LabelId);

            //if (!materialReturn.ReturnWarehouseId.HasValue)
            //{
            //    throw new ValidationException("【{0}】退料保存失败，退料仓库不能为空".L10nFormat(materialReturn.Label));
            //}
            //if (!materialReturn.ReturnWarehouseLocationId.HasValue)
            //{
            //    throw new ValidationException("【{0}】退料保存失败，退料库位不能为空".L10nFormat(materialReturn.Label));
            //}

            if (materialReturn.ReturnWarehouseId.HasValue)
            {
                if (!factoryIds.Contains(materialReturn.FactoryId) && !warehouseIds.Contains(materialReturn.ReturnWarehouseId.Value))
                {
                    throw new ValidationException("【{0}】退料保存失败，操作员工没有该标签的工厂权限或仓库权限，请检查".L10nFormat(materialReturn.Label));
                }
            }
            if (materialReturn.ReturnType == ReturnTypes.Bad && materialReturn.Qty > materialReturn.BadQty)
            {
                throw new ValidationException("【{0}】退料保存失败，退料数不能大于标签的不良数量".L10nFormat(materialReturn.Label));
            }
            if (materialReturn.ReturnType == ReturnTypes.Normal && itemLabel != null && materialReturn.Qty > itemLabel.Qty)
            {
                if (materialReturn.WorkOrderId.HasValue)
                {
                    throw new ValidationException("【{0}】退料保存失败，本次退料关联工单【{1}】的数量为{2},物料标签数量为{3}".L10nFormat(materialReturn.Label,
                        materialReturn.WorkOrder.No, materialReturn.AlreadyQty, itemLabel.Qty));
                }
                else
                {
                    throw new ValidationException("【{0}】退料保存失败，本次退料剩余可用的数量为{1}".L10nFormat(materialReturn.Label, itemLabel.Qty));
                }
            }

        }

        /// <summary>
        /// 获取当前用户所有工厂权限
        /// </summary>
        /// <returns></returns>
        public virtual List<double> GetAuthorityFactoryId()
        {
            return Query<EmployeeEnterprise>().Where(y => y.EmployeeId == RT.IdentityId).Select(p => p.EnterpriseId).Distinct().ToList<double>().ToList();
        }

        /// <summary>
        /// 获取查询的标签数据
        /// </summary>
        /// <param name="keyWord"></param>
        /// <returns></returns>
        public virtual EntityList<MaterialReturn> GetSearch(string keyWord)
        {
            EntityList<MaterialReturn> materialReturns = new EntityList<MaterialReturn>();
            var results = RT.Service.Resolve<MaterialReturnController>().RetrunSearch(keyWord);
            foreach (var result in results)
            {
                if (result.Qty == 0 && result.NgQty == 0)
                {
                    continue;
                }

                //存在可用数量大于不良数量这种情况如何生成数据

                if (result.WorkOrderList.Any())//存在关联工单
                {
                    foreach (var workOrderRes in result.WorkOrderList)
                    {
                        MaterialReturn materialReturnTemp = SetMaterialReturn(result, workOrderRes);
                        materialReturns.Add(materialReturnTemp);
                    }
                }
                else
                {

                    ItemLabelWorkOrder itemLabelWorkOrder = null;
                    if (result.WorkOrderId.HasValue)
                    {
                        itemLabelWorkOrder = new ItemLabelWorkOrder() { WorkOrderId = result.WorkOrderId.Value, WorkOrder = result.WorkOrder, ItemLabelId = result.Id };
                    }
                    if (result.Qty > 0 || (itemLabelWorkOrder != null && itemLabelWorkOrder.Qty > 0))//生成正常数据
                    {
                        MaterialReturn materialReturnTemp = SetMaterialReturn(result, itemLabelWorkOrder);
                        materialReturns.Add(materialReturnTemp);
                    }
                }
                if (result.NgQty > 0)//如果存在不良数量 则按不良再生一条退料单据
                {
                    MaterialReturn ngMaterialReturnTemp = SetMaterialReturn(result, null, true);//生成不良退料
                    materialReturns.Add(ngMaterialReturnTemp);
                }
            }
            return materialReturns;
        }

        /// <summary>
        /// 设置退料信息
        /// </summary>
        /// <param name="result"></param>
        /// <param name="workOrderRes"></param>
        /// <returns></returns>
        private MaterialReturn SetMaterialReturn(ItemLabel result, ItemLabelWorkOrder workOrderRes, bool isNg = false)
        {
            MaterialReturn materialReturnTemp = new MaterialReturn()
            {
                AlreadyQty = result.Qty,
                NO = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialCode(),
                Qty = result.Qty,
                BatchNO = result.Lot,
                Employee = RF.GetById<SIE.Resources.Employee>(RT.Identity.Id),
                ItemCode = result.ItemCode,
                ItemId = result.ItemId,
                EmployeeId = RT.Identity.Id,
                ItemName = result.ItemName,
                Item = result.Item,
                Label = result.Label,
                BadQty = result.NgQty,
                LabelId = result.Id,
                ReturnWarehouseId = result.WarehouseId,
                ReturnWarehouse = result.Warehouse,
                ReturnState = ReturnStates.TobeSubmitted,
                ReturnType = isNg ? (result.NgQty > 0 ? ReturnTypes.Bad : ReturnTypes.Normal) : ReturnTypes.Normal,
                ReturnWarehouseLocationId = result.StorageLocationId,
                ReturnWarehouseLocation = result.StorageLocation,
                ReturnReason = RT.Service.Resolve<MaterialReturnController>().GetReturnMaterialReasonDefault(),
                //获取资源
                IsSerial = result.IsSerialNumber,
                PersistenceStatus = PersistenceStatus.New,
                ItemExtProp = result.ItemExtProp,
                ItemExtPropName = result.ItemExtPropName

            };
            if (workOrderRes != null)
            {
                materialReturnTemp.WorkOrderId = workOrderRes.WorkOrderId;
                materialReturnTemp.WorkOrder = workOrderRes.WorkOrder;
                materialReturnTemp.Qty = workOrderRes.Qty;
                materialReturnTemp.AlreadyQty = workOrderRes.Qty;
                var info = RT.Service.Resolve<IWorkOrderQuery>().GetWorkOrderResource(workOrderRes.WorkOrderId);
                materialReturnTemp.WipResourceId = info.ResourceId;
                materialReturnTemp.WipResource = RF.GetById<WipResource>(info.ResourceId);
            }
            if (workOrderRes == null && isNg)//不良退料
            {
                materialReturnTemp.Qty = result.NgQty;
                materialReturnTemp.AlreadyQty = result.NgQty;
            }
            if (result.FactoryId.HasValue)
            {
                materialReturnTemp.Factory = result.Factory;
                materialReturnTemp.FactoryId = result.FactoryId.Value;
            }

            return materialReturnTemp;
        }
    }
}
