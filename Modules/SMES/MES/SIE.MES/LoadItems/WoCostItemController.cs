using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.Common.WorkOrders;
using SIE.EventMessages.MES.LoadItems;
using SIE.EventMessages.MES.LoadItems.Models;
using SIE.Items;
using SIE.MES.LoadItems.Configs;
using SIE.MES.LoadItems.Enum;
using SIE.MES.LoadItems.Models;
using SIE.MES.WIP;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.LoadItems
{
    /// <summary>
    /// 扣料控制器
    /// </summary>
    public class WoCostItemController : DomainController, IWoCostItem
    {
        private const string woCostMsgError = "工单耗用量数据异常！";
        #region 查询扣料数据
        /// <summary>
        /// 查询扣料数据
        /// </summary>
        /// <param name="woCostItemCriterial"></param>
        /// <returns></returns>
        public virtual EntityList<WoCostItem> CriterialDeductItem(WoCostItemCriterial woCostItemCriterial)
        {
            if (woCostItemCriterial == null)
            {
                throw new ValidationException("工单耗用量查询实体异常！".L10N());
            }
            var query = Query<WoCostItem>();
            if (woCostItemCriterial.CostNo.IsNotEmpty())
            {
                query.Where(p => p.CostNo.Contains(woCostItemCriterial.CostNo));
            }
            if (woCostItemCriterial.RecordType.HasValue)
            {
                query.Where(p => p.RecordType == woCostItemCriterial.RecordType.Value);
            }
            if (woCostItemCriterial.State.HasValue)
            {
                query.Where(p => p.State == woCostItemCriterial.State.Value);
            }
            if (woCostItemCriterial.WoNo.IsNotEmpty())
            {
                query.Where(p => p.WorkOrder.No.Contains(woCostItemCriterial.WoNo));
            }
            if (woCostItemCriterial.ProductName.IsNotEmpty())
            {
                query.Where(p => p.ProductName.Contains(woCostItemCriterial.ProductName));
            }
            if (woCostItemCriterial.CostItemCode.IsNotEmpty())
            {
                query.Where(p => p.Item.Code.Contains(woCostItemCriterial.CostItemCode));
            }
            if (woCostItemCriterial.CostItemName.IsNotEmpty())
            {
                query.Where(p => p.Item.Name.Contains(woCostItemCriterial.CostItemName));
            }
            if (woCostItemCriterial.Label.IsNotEmpty())
            {
                query.Where(p => p.CostItemLabel.Label.Contains(woCostItemCriterial.Label));
            }
            if (woCostItemCriterial.Lot.IsNotEmpty())
            {
                query.Where(p => p.CostItemLabel.Lot.Contains(woCostItemCriterial.Lot));
            }
            if (woCostItemCriterial.BarCode.IsNotEmpty())
            {
                query.Where(p => p.BarCode.Contains(woCostItemCriterial.BarCode));
            }
            if (woCostItemCriterial.BatchNo.IsNotEmpty())
            {
                query.Where(p => p.BatchNo.Contains(woCostItemCriterial.BatchNo));
            }
            if (woCostItemCriterial.FactoryId != 0 && woCostItemCriterial.FactoryId != null)
            {
                query.Where(p => p.FactoryId == woCostItemCriterial.FactoryId.Value);
            }
            if (woCostItemCriterial.WipResourceId != 0 && woCostItemCriterial.WipResourceId != null)
            {
                query.Where(p => p.WipResourceId == woCostItemCriterial.WipResourceId.Value);
            }
            if (woCostItemCriterial.SubmiterId != 0 && woCostItemCriterial.SubmiterId != null)
            {
                query.Where(p => p.SubmiterId == woCostItemCriterial.SubmiterId.Value);
            }
            if (woCostItemCriterial.SubmitTime.BeginValue.HasValue)
            {
                query.Where(p => p.SubmitTime >= woCostItemCriterial.SubmitTime.BeginValue.Value);
            }
            if (woCostItemCriterial.SubmitTime.EndValue.HasValue)
            {
                query.Where(p => p.SubmitTime <= woCostItemCriterial.SubmitTime.EndValue.Value);
            }
            query.Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EnterpriseId == x.FactoryId && p.EmployeeId == RT.IdentityId));
            return query.OrderBy(woCostItemCriterial.OrderInfoList).ToList(woCostItemCriterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }
        #endregion

        #region 根据资源获取工单
        /// <summary>
        /// 根据资源获取工单
        /// </summary>
        /// <param name="woCostItem"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<WorkOrder> GetWorkOrders(WoCostItem woCostItem, PagingInfo pagingInfo, string keyword)
        {
            if (woCostItem == null)
            {
                throw new ValidationException(woCostMsgError.L10N());
            }

            var woOrderList = Query<WorkOrder>()
                .WhereIf(keyword.IsNotEmpty(), p => p.No.Contains(keyword))
                .WhereIf(woCostItem.WipResourceId != 0 && woCostItem.WipResourceId != null, p => p.ResourceId == woCostItem.WipResourceId).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return woOrderList;
        }
        #endregion

        #region 根据单据类型获取耗用物料
        /// <summary>
        /// 根据单据类型获取耗用物料
        /// </summary>
        /// <param name="woCostItem"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<Item> GetItemsByType(WoCostItem woCostItem, PagingInfo pagingInfo, string keyword)
        {
            if (woCostItem == null)
            {
                throw new ValidationException(woCostMsgError.L10N());
            }
            if (woCostItem.RecordType == Enum.WoCostItemType.WoCost)
            {
                if (woCostItem.WorkOrderId == 0)
                {
                    throw new ValidationException("当单据类型为工单消耗时，仅能选择工单BOM的物料！请先维护工单数据".L10N());
                }
                else
                {
                    var query = Query<Item>()
                        .Exists<WorkOrderBom>((x, y) => y.Where(p => p.WorkOrderId == woCostItem.WorkOrderId && p.ItemId == x.Id))
                        .WhereIf(keyword.IsNotEmpty(), p => p.Name.Contains(keyword) || p.Code.Contains(keyword))
                        .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                    return query;
                }
            }
            else
            {
                var query = Query<Item>()
                    .Where(p => p.State == State.Enable)
                    .WhereIf(keyword.IsNotEmpty(), p => p.Name.Contains(keyword) || p.Code.Contains(keyword))
                    .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                return query;
            }
        }
        #endregion

        #region 根据物料获取其物料标签
        /// <summary>
        /// 根据物料获取其物料标签
        /// </summary>
        /// <param name="woCostItem"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<ItemLabel> GetItemLabels(WoCostItem woCostItem, PagingInfo pagingInfo, string keyword)
        {
            if (woCostItem == null)
            {
                throw new ValidationException(woCostMsgError.L10N());
            }
            if (woCostItem.ItemId == 0)
            {
                throw new ValidationException("请先维护消耗物料！".L10N());
            }
            var query = Query<ItemLabel>()
                .WhereIf(keyword.IsNotEmpty(), p => p.Label.Contains(keyword))
                .Where(p => p.ItemId == woCostItem.ItemId && p.ItemExtProp == woCostItem.ItemExtProp).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取工单对应的工序
        /// <summary>
        /// 获取工单对应的工序
        /// </summary>
        /// <param name="woCostItem"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Process> GetProcesses(WoCostItem woCostItem, PagingInfo pagingInfo, string keyword)
        {
            if (woCostItem == null)
            {
                throw new ValidationException(woCostMsgError.L10N());
            }
            if (woCostItem.WorkOrderId == 0)
            {
                throw new ValidationException("仅可选择工单下对应的工序，请先维护工单数据！".L10N());
            }
            var query = Query<Process>()
                .Exists<WorkOrderRoutingProcess>((x, y) => y.Where(p => p.ProcessId == x.Id && p.WorkOrderId == woCostItem.WorkOrderId))
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        #region 获取权限工厂下的生产资源
        /// <summary>
        /// 获取权限工厂下的生产资源
        /// </summary>
        /// <param name="woCostItem"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> GetWipResources(WoCostItem woCostItem, PagingInfo pagingInfo, string keyword)
        {
            if (woCostItem == null)
            {
                throw new ValidationException(woCostMsgError.L10N());
            }
            if (woCostItem.FactoryId == 0)
            {
                throw new ValidationException("请先维护工厂数据！".L10N());
            }
            var query = Query<WipResource>()
                .WhereIf(keyword.IsNotEmpty(), p => p.Name.Contains(keyword) || p.Code.Contains(keyword))
                .Where(p => p.FactoryId == woCostItem.FactoryId)
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;
        }
        #endregion

        /// <summary>
        /// 获取员工工厂权限下的工厂信息(查询用)
        /// </summary>
        /// <param name="woCostItemCriterial"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> QueryEnterprises(WoCostItemCriterial woCostItemCriterial, PagingInfo pagingInfo, string keyword)
        {
            if (woCostItemCriterial == null)
            {
                throw new ValidationException("工单耗用量查询实体数据异常！".L10N());
            }
            var query = Query<Enterprise>().Where(p => p.Level.Type == EnterpriseType.Plant)
                .Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EnterpriseId == x.Id && p.EmployeeId == RT.IdentityId))
                .WhereIf(keyword.IsNotEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            query.ForEach(i =>
            {
                i.TreePId = null;
            });
            return query;
        }

        /// <summary>
        /// 根据工厂获取资源信息(查询用)
        /// </summary>
        /// <param name="woCostItemCriterial"></param>
        /// <param name="pagingInfo"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public virtual EntityList<WipResource> QueryWipResources(WoCostItemCriterial woCostItemCriterial, PagingInfo pagingInfo, string keyword)
        {
            if (woCostItemCriterial == null)
            {
                throw new ValidationException("工单耗用量查询实体数据异常！".L10N());
            }
            var Factorys = Query<Enterprise>()
                .Exists<EmployeeEnterprise>((x, y) => y.Where(p => p.EnterpriseId == x.Id && p.EmployeeId == RT.IdentityId))
                .Select(p => p.Id).ToList();
            var FactoryIds = Factorys.Select(p => p.Id).Distinct().ToList();
            var query = Query<WipResource>()
                .WhereIf(woCostItemCriterial.FactoryId != 0, p => p.FactoryId == woCostItemCriterial.FactoryId)
                .WhereIf(woCostItemCriterial.FactoryId == 0, p => FactoryIds.Contains(p.FactoryId.Value))
                .ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return query;

        }

        /// <summary>
        /// 获取工单耗用单编码
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetCostNoRule(int qty)
        {
            var config = ConfigService.GetConfig(new WoCostItemNoConfig(), typeof(WoCostItem));
            if (config == null || config.CostNoRule == null)
            {
                throw new ValidationException("未找到工单耗用单编码规则,请检查规则配置".L10N());
            }
            return RT.Service.Resolve<NumberRuleController>().GenerateSegment(config.CostNoRule, qty);
        }

        #region 补扣命令
        /// <summary>
        /// 补扣命令
        /// </summary>
        /// <param name="deductItemIds"></param>
        public virtual WoCostItemDeductResult SupDeduct(List<double> deductItemIds)
        {
            return RT.Service.Resolve<BackflushMaterialController>().ReBackflushMaterial(deductItemIds);
        }
        #endregion

        #region 提交命令
        /// <summary>
        /// 提交命令
        /// </summary>
        /// <param name="woCostItem"></param>
        public virtual void SubmitCost(WoCostItem woCostItem)
        {
            if (woCostItem == null)
            {
                throw new ValidationException("工单耗用单数据异常!".L10N());
            }
            if (woCostItem.CostItemLabelId == 0 || woCostItem.CostItemLabelId == null)
            {
                throw new ValidationException("该耗用物料未维护物料标签！".L10N());
            }
            var itemLabel = RF.GetById<ItemLabel>(woCostItem.CostItemLabelId);
            if (woCostItem.Qty <= 0)
            {
                throw new ValidationException("提交数量必须大于0!".L10N());
            }

            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                //更新物料标签数量
                if (itemLabel.Qty < woCostItem.Qty)
                {
                    throw new ValidationException("物料标签可用数量小于提交数量！".L10N());
                }
                else
                {
                    itemLabel.Qty -= woCostItem.Qty;
                }
                //状态变更为已提交
                woCostItem.State = Enum.WoCostItemState.Submitted;
                woCostItem.SubmiterId = RT.IdentityId;
                woCostItem.SubmitTime = RF.Find<WoCostItem>().GetDbTime();
                //调用wsm接口减少线边仓库存
                UpdateWmsOnhand(woCostItem);
                RF.Save(itemLabel);
                RF.Save(woCostItem);

                //更新工单BOM
                RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woCostItem.WorkOrderId, new List<double>() { woCostItem.ItemId });

                tran.Complete();
            }
        }
        #endregion

        private void UpdateWmsOnhand(WoCostItem woCostItem)
        {
            MesUpdateOnhandData mesUpdateOnhandData = new MesUpdateOnhandData()
            {
                WoNo = woCostItem.WorkOrder.No,
                WoId = woCostItem.WorkOrderId,
                OpType = 0,
                EmpCode = RT.Identity.Name,
                EnterpriseId = woCostItem.WorkOrder.WorkShopId,
                TranstypeName = "工单耗用",
            };
            mesUpdateOnhandData.LabelDatas.Add(new MesLabelData
            {
                LabelNo = woCostItem.CostItemLabel.Label,
                IsFail = false,
                StorageLocationId = woCostItem.CostItemLabel.StorageLocationId,
                Qty = woCostItem.Qty,
                ItemExtProp = woCostItem.ItemExtProp,
                ItemExtPropName = woCostItem.ItemExtPropName,
                ProjectNo = woCostItem.ProjectNo,
                ItemId = woCostItem.ItemId,
                LotCode = woCostItem.CostItemLabel.Lot,
                WorkOrderNo = woCostItem.WorkOrder.No,
                WorkOrderId = woCostItem.WorkOrderId
            });
            RT.Service.Resolve<ILotLpnOnhand>().MesUpdateOnhand(mesUpdateOnhandData);
        }

        /// <summary>
        /// 强制关闭命令
        /// </summary>
        /// <param name="deductItemId"></param>
        public virtual void Close(double deductItemId)
        {
            using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                DB.Update<WoCostItem>()
                    .Set(x => x.State, Enum.WoCostItemState.Close)
                    .Where(x => x.Id == deductItemId)
                    .Execute();

                //更新工单BOM
                var woCostItem = RF.GetById<WoCostItem>(deductItemId);
                RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woCostItem.WorkOrderId, new List<double>() { woCostItem.ItemId });

                tran.Complete();
            }
        }

        /// <summary>
        /// 根据工单id获取对应的工厂资源信息
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        public virtual FactoryInfo GetFactoryInfo(double workOrderId)
        {
            var woOrder = RF.GetById<WorkOrder>(workOrderId, new EagerLoadOptions().LoadWithViewProperty());
            if (woOrder == null)
            {
                return new FactoryInfo();
            }
            FactoryInfo factoryInfo = new FactoryInfo
            {
                FactoryId = woOrder.FactoryId,
                FactoryName = woOrder.Factory?.Name,
                WipResourceId = woOrder.ResourceId,
                WipResourceName = woOrder.Resource?.Name,
            };
            return factoryInfo;
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="itemLabelIds"></param>
        /// <returns></returns>
        public virtual EntityList<ItemLabel> GetItemLabelByIds(List<double?> itemLabelIds)
        {
            var itemLableList = itemLabelIds.SplitContains(tempIds =>
            {
                return Query<ItemLabel>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
            return itemLableList;
        }

        /// <summary>
        /// 根据工单ids获取耗用单
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual EntityList<WoCostItem> GetWoCostItemByWoIds(List<double> woIds)
        {
            return woIds.SplitContains(tempIds =>
            {
                return Query<WoCostItem>().Where(p => tempIds.Contains(p.WorkOrderId) && p.State == LoadItems.Enum.WoCostItemState.Submitted).ToList();
            });
        }

        /// <summary>
        /// 根据工单ids获取耗用单基础数据
        /// </summary>
        /// <param name="woIds"></param>
        /// <returns></returns>
        public virtual List<WoCostItemBaseData> GetBaseWoCostItemByWoIds(List<double> woIds)
        {
            List<WoCostItemBaseData> woCostItemBaseDatas = new List<WoCostItemBaseData>();
            woIds.SplitDataExecute(tempIds =>
            {
                var list = Query<WoCostItem>()
                .Where(p => tempIds.Contains(p.WorkOrderId) && p.State == LoadItems.Enum.WoCostItemState.Submitted)
                .Select(p => new
                {
                    p.Id,
                    p.WorkOrderId,
                    p.ItemId,
                    p.ItemExtProp,
                    p.ItemExtPropName,
                    p.Qty,
                })
                .ToList<WoCostItemBaseData>();
                woCostItemBaseDatas.AddRange(list);
            });
            return woCostItemBaseDatas;
        }

        /// <summary>
        /// 根据工单id获取耗用单
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual EntityList<WoCostItem> GetWoCostItemByWoId(double woId)
        {
            return Query<WoCostItem>().Where(p => p.WorkOrderId == woId && p.State == LoadItems.Enum.WoCostItemState.Submitted).ToList();
        }

        /// <summary>
        /// 根据工单id获取耗用单基础数据
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual List<WoCostItemBaseData> GetBaseWoCostItemByWoId(double woId)
        {
            List<WoCostItemBaseData> woCostItemBaseDatas = new List<WoCostItemBaseData>();
            var list = Query<WoCostItem>()
                .Where(p => p.WorkOrderId == woId && p.State == LoadItems.Enum.WoCostItemState.Submitted)
                .Select(p => new
                {
                    p.Id,
                    p.WorkOrderId,
                    p.ItemId,
                    p.ItemExtProp,
                    p.ItemExtPropName,
                    p.Qty,
                })
                .ToList<WoCostItemBaseData>();
            woCostItemBaseDatas.AddRange(list);
            return woCostItemBaseDatas;
        }

        /// <summary>
        /// 查询工单物料耗用数据
        /// </summary>
        /// <param name="WoNo"></param>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public List<WoCostItemInfo> GetWoCostItemDatas(string WoNo, List<double> itemIds = null)
        {
            var st = new List<WoCostItemState>() { WoCostItemState.ToSubmit, WoCostItemState.Submitted, WoCostItemState.FailSubmit };
            List<WoCostItemInfo> woCostItemBaseDatas = new List<WoCostItemInfo>();
            var list = Query<WoCostItem>()
                .Where(p => p.WorkOrder.No == WoNo && st.Contains(p.State))
                .WhereIf(itemIds != null, p => itemIds.Contains(p.ItemId))
                .Select(p => new
                {
                    p.CostNo,
                    p.WorkOrderId,
                    p.State,
                    p.ItemId,
                    p.ItemExtProp,
                    p.ItemExtPropName,
                    p.Qty,
                })
                .ToList<WoCostItemInfo>();
            woCostItemBaseDatas.AddRange(list);
            return woCostItemBaseDatas;
        }
    }
}
