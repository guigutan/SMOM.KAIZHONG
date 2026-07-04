using Microsoft.Scripting.Utils;
using SIE.Core.Items;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages;
using SIE.EventMessages.Common.WorkOrders;
using SIE.LES.LinesideWarehouses;
using SIE.MES.LoadItems;
using SIE.MES.LoadItems.Enum;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Packages.ItemLabels;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 
    /// </summary>
    public class BackflushMaterialExecutor
    {
        /// <summary>
        /// 扣料需求
        /// </summary>
        private readonly EntityList<WoCostItem> deductItems;

        /// <summary>
        /// 替代料
        /// </summary>
        private readonly Dictionary<string, List<WorkOrderBom>> bomAlternativesDictionary;

        /// <summary>
        /// 线边仓
        /// </summary>
        private readonly EntityList<LinesideWarehouse> linesideWarehouses;
        /// <summary>
        /// 物料标签
        /// </summary>
        private readonly EntityList<ItemLabel> itemLabels;

        /// <summary>
        /// 物料标签字典
        /// </summary>
        private readonly Dictionary<double, List<ItemLabel>> itemLabelsDictionary;

        /// <summary>
        /// 数据库时间
        /// </summary>
        private readonly DateTime dbDateTime;

        /// <summary>
        /// 工单Id
        /// </summary>
        private readonly List<double> workOrderIds;

        /// <summary>
        /// 工单
        /// </summary>
        private readonly EntityList<WorkOrderMove> workOrderMoves;

        /// <summary>
        /// 任务单Id
        /// </summary>
        private double? DispatchTaskId;

        /// <summary>
        /// 员工
        /// </summary>
        private Employee employee;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deductItems">扣料需求</param>
        /// <exception cref="ValidationException"></exception>
        public BackflushMaterialExecutor(EntityList<WoCostItem> deductItems)
        {
            if (deductItems == null)
            {
                throw new ValidationException("物料倒扣记录找不到".L10N());
            }

            if (deductItems.Any(x => x.State != WoCostItemState.ToSubmit && x.State != WoCostItemState.FailSubmit))
            {
                throw new ValidationException("扣料状态是【未扣料】或【扣料失败】的记录才允许进行扣料".L10N());
            }

            this.deductItems = deductItems;

            var workOrderBomIds = deductItems.Where(x => x.WorkOrderBomId.HasValue).Select(x => x.WorkOrderBomId.Value).Distinct().ToList();

            //工单BOM的替代料
            var bomAlternatives = RT.Service.Resolve<WorkOrderBomController>().GetWorkOrderBomAlternatives(workOrderBomIds);

            bomAlternativesDictionary = bomAlternatives.GroupBy(x => x.Alter).ToDictionary(k => k.Key, v => v.ToList());

            //获取产线线边仓维护信息
            linesideWarehouses = RF.GetAll<LinesideWarehouse>();

            //物料标签
            var itemIds = deductItems.Select(x => x.ItemId).ToList();
            itemIds.AddRange(bomAlternatives.Select(x => x.ItemId));
            itemIds = itemIds.Distinct().ToList();
            itemLabels = RT.Service.Resolve<ItemLabelController>().GetItemLabels(itemIds);
            itemLabelsDictionary = itemLabels.GroupBy(g => g.ItemId).ToDictionary(k => k.Key, v => v.ToList());

            //	以下扣料失败的场景的不影响过站，生成一条扣料失败记录
            //	资源没维护仓库和库位
            //	库位的标签库存不够，需要全部扣完
            dbDateTime = RF.Find<WoCostItem>().GetDbTime();
            employee = RF.GetById<Employee>(RT.IdentityId);

            workOrderIds = deductItems.Select(x => x.WorkOrderId).Distinct().ToList();
            workOrderMoves = workOrderIds.SplitContains(tempIds =>
            {
                return DB.Query<WorkOrderMove>().Where(x => tempIds.Contains(x.Id)).ToList();
            });
        }

        /// <summary>
        /// 空构造
        /// </summary>
        public BackflushMaterialExecutor()
        {
        }
        /// <summary>
        /// 替代分组
        /// </summary>
        private string alterGroup { get; set; }

        public void InputDispatchTaskId(double DispatchTaskId)
        {
            this.DispatchTaskId = DispatchTaskId;
        }

        /// <summary>
        /// 执行扣料逻辑
        /// </summary>
        /// <returns></returns>
        public WoCostItemDeductResult ExecuteDeductItems()
        {
            var result = new WoCostItemDeductResult();

            //获取单号
            BackflushMaterialHelper.BatchSetCostNos(deductItems);

            foreach (var deductItem in deductItems)
            {
                if (deductItem.State == WoCostItemState.Submitted)
                    continue;

                var wo = workOrderMoves.FirstOrDefault(x => x.Id == deductItem.WorkOrderId);
                LinesideWarehouse lineWh = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouse(wo.WorkShopId, wo.ResourceId, linesideWarehouses);

                try
                {
                    using (var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
                    {
                        if (lineWh == null)
                            throw new ValidationException("工单[{0}]未维护产线线边仓信息,无法执行倒扣料".L10nFormat(wo?.No));

                        //扣除物料标签
                        var labeDatas = DeductItemLabels(deductItem, lineWh);

                        //WMS接口
                        UpdateWmsOnhand(deductItem, labeDatas);

                        //保存扣料状态
                        deductItem.State = WoCostItemState.Submitted;
                        deductItem.FailMsg = string.Empty;
                        if (!deductItem.SubmiterId.HasValue)    //补扣时,不更新提交人,提交时间
                        {
                            deductItem.SubmiterId = RT.IdentityId;
                            deductItem.SubmitTime = dbDateTime;
                        }
                        RF.Save(deductItem);                                  
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    deductItem.State = WoCostItemState.FailSubmit;
                    deductItem.FailMsg = ex.Message;
                    DB.Update<WoCostItem>()
                        .Set(p => p.CostNo, deductItem.CostNo)
                        .Set(p => p.State, deductItem.State)
                        .Set(p => p.FailMsg, deductItem.FailMsg).Where(p => p.Id == deductItem.Id).Execute();
                }
            }

            //更新工单BOM
            deductItems.GroupBy(p => p.WorkOrderId).ForEach(g =>
            {
                var woId = g.Key;
                var itemIds = g.Select(p => p.ItemId).Distinct().ToList();
                RT.Service.Resolve<IWorkOrderUpdate>().UpdateWoBomQty(woId, itemIds);
            });

            result.SuccessCount += deductItems.Count(x => x.State == WoCostItemState.Submitted);
            result.FailCount += deductItems.Count(x => x.State == WoCostItemState.FailSubmit);
            return result;
        }

        /// <summary>
        /// 更新WMS库存
        /// </summary>
        /// <param name="deductItem"></param>
        /// <param name="mesLabelDatas"></param>
        void UpdateWmsOnhand(WoCostItem deductItem, List<MesLabelData> mesLabelDatas)
        {
            if (mesLabelDatas.Count == 0)
                return;
            var wo = workOrderMoves.FirstOrDefault(x => x.Id == deductItem.WorkOrderId);
            if (employee == null)
                employee = RF.GetById<Employee>(deductItem.SubmiterId ?? RT.IdentityId);
            try
            {
                MesUpdateOnhandData mesUpdateOnhandData = new MesUpdateOnhandData()
                {
                    // 操作类型 0-上料 1-下料 2-倒扣非工序BOM物料 3-接收 ,只有下料和接收是增加库位库存、其他都是减库存
                    OpType = 2
                };
                mesUpdateOnhandData.WoNo = wo?.No;
                mesUpdateOnhandData.EnterpriseId = wo?.WorkShopId;
                mesUpdateOnhandData.WoId = wo?.Id;
                mesUpdateOnhandData.EmpCode = employee?.Code;
                mesUpdateOnhandData.LabelDatas.AddRange(mesLabelDatas);

                RT.Service.Resolve<ILotLpnOnhand>().MesUpdateOnhand(mesUpdateOnhandData);
            }
            catch (Exception ex)
            {
                var errorMsg = ex.GetBaseException().Message;
                throw new ValidationException("调用WMS接口失败: {0}".L10nFormat(errorMsg));
            }
        }
        /// <summary>
        /// 生成WMS事务标签数据
        /// </summary>
        /// <param name="itemLabel"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        MesLabelData NewMesLabelData(ItemLabel itemLabel, decimal qty)
        {

            var data = new MesLabelData
            {
                LabelNo = itemLabel.Label,
                IsFail = false,
                WarehouseId = itemLabel.WarehouseId ?? 0,
                StorageLocationId = itemLabel.StorageLocationId,
                //数量为扣料数量
                Qty = qty,
                ItemExtProp = itemLabel.ItemExtProp,
                ItemExtPropName = itemLabel.ItemExtPropName,
                ProjectNo = itemLabel.ProjectNo,
                ItemId = itemLabel.ItemId,
                LotCode = itemLabel.Lot,
                WorkOrderId = itemLabel.WorkOrderId,
                WorkOrderNo = itemLabel.WorkOrderNo
            };

            return data;
        }

        /// <summary>
        /// 扣除物料标签
        /// </summary>
        /// <param name="deductItem"></param>
        /// <param name="linesideWarehouse"></param>
        List<MesLabelData> DeductItemLabels(WoCostItem deductItem, LinesideWarehouse linesideWarehouse)
        {
            var mesLabelDatas = new List<MesLabelData>();
            decimal reqQty = deductItem.Qty;

            if (itemLabelsDictionary.ContainsKey(deductItem.ItemId) && IsAlterGroupEmptyOrEqual(deductItem.AlterGroup))
            {
                var itemLablelsOfItem = itemLabelsDictionary[deductItem.ItemId];

                mesLabelDatas.AddRange(DeductMaterial(linesideWarehouse, deductItem, itemLablelsOfItem, reqQty));
            }

            //替代料
            if (reqQty > 0 && bomAlternativesDictionary.ContainsKey(deductItem.Alter))
            {
                var alternatives = bomAlternativesDictionary[deductItem.Alter];

                foreach (var alternative in alternatives)
                {
                    if (itemLabelsDictionary.ContainsKey(alternative.ItemId) && IsAlterGroupEmptyOrEqual(alternative.AlterGroup))
                    {
                        var itemLablelsOfItem = itemLabelsDictionary[alternative.ItemId];

                        mesLabelDatas.AddRange(DeductMaterial(linesideWarehouse, deductItem, itemLablelsOfItem, reqQty));
                    }
                }
            }
            if (mesLabelDatas.Count == 0)
                throw new ValidationException("物料不足".L10N());

            return mesLabelDatas;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="alterGroup"></param>
        /// <returns></returns>
        private bool IsAlterGroupEmptyOrEqual(string alterGroup)
        {
            bool canUse = true;

            if (alterGroup.IsNullOrEmpty())
            {
                return canUse;
            }

            if (this.alterGroup.IsNullOrEmpty())
            {
                this.alterGroup = alterGroup;
            }
            else
            {
                if (this.alterGroup != alterGroup)
                {
                    //替代分组不相同不能使用
                    canUse = false;
                }
            }
            return canUse;
        }

        /// <summary>
        /// 扣料
        /// </summary>		
        /// <param name="linesideWarehouse">线边仓库配置</param>                
        /// <param name="deductItem">扣料需求</param>
        /// <param name="itemLablelsOfItem">可用的物料标签</param> 
        /// <param name="requireQty">扣料需求数量</param>
        /// <returns>剩余需求量</returns>
        private List<MesLabelData> DeductMaterial(LinesideWarehouse linesideWarehouse, WoCostItem deductItem, List<ItemLabel> itemLablelsOfItem, decimal requireQty)
        {
            var mesLabelDatas = new List<MesLabelData>();
            List<ItemLabel> itemLabelsCanUse;
            if (deductItem.ConsumeType == Items.ConsumeMode.Push)
            {
                //	推式物料：根据工单和扣料的物料获取状态为【已接收】的物料标签，去除剩余数量为0的数据，按接收时间顺序扣减数量，扣到为0时扣下一个； 
                if (deductItem.ProjectNo.IsNullOrEmpty() || deductItem.ProjectNo.Equals("*"))
                {
                    itemLabelsCanUse = itemLablelsOfItem
                        .Where(x => x.Qty > 0 && x.ItemExtProp == deductItem.ItemExtProp && (x.ProjectNo == "*" || x.ProjectNo.IsNullOrEmpty()))
                        .OrderBy(p => p.CreateDate).ToList();
                }
                else
                    itemLabelsCanUse = itemLablelsOfItem
                        .Where(x => x.Qty > 0 && x.ItemExtProp == deductItem.ItemExtProp && x.ProjectNo == deductItem.ProjectNo)
                        .OrderBy(p => p.CreateDate).ToList();

            }
            else
            {
                //	拉式物料：获取资源的扣料库位，获取该库位中该物料的物料标签，去除剩余数量为0的数据，按时间顺序进行扣减，扣到为0时扣下一个；
                if (deductItem.ProjectNo.IsNullOrEmpty() || deductItem.ProjectNo.Equals("*"))
                {
                    itemLabelsCanUse = itemLablelsOfItem
                        .Where(x => x.StorageLocationId == linesideWarehouse.StorageLocationId && x.Qty > 0 && x.ItemExtProp == deductItem.ItemExtProp && (x.ProjectNo == "*" || x.ProjectNo.IsNullOrEmpty()))
                        .OrderBy(p => p.CreateDate).ToList();
                }
                else
                    itemLabelsCanUse = itemLablelsOfItem
                        .Where(x => x.StorageLocationId == linesideWarehouse.StorageLocationId && x.Qty > 0 && x.ItemExtProp == deductItem.ItemExtProp && x.ProjectNo == deductItem.ProjectNo)
                        .OrderBy(p => p.CreateDate).ToList();
            }

            if (itemLabelsCanUse.Sum(p => p.Qty) < requireQty)
                throw new ValidationException("物料不足".L10N());

            foreach (var itemLabel in itemLabelsCanUse)
            {
                //当前标签能扣料数量
                decimal deductQty;  //标签扣料数
                if (requireQty <= 0)
                    break;

                if (itemLabel.Qty >= requireQty)
                {
                    //标签数满足需求数
                    deductQty = requireQty;

                    deductItem.CostItemLabel = itemLabel;
                    deductItem.ItemExtProp = itemLabel.ItemExtProp;
                    deductItem.ItemExtPropName = itemLabel.ItemExtPropName;
                    deductItem.ProjectNo = itemLabel.ProjectNo;
                    deductItem.State = WoCostItemState.Submitted;
                    deductItem.FailMsg = string.Empty;
                    deductItem.WarehouseId = itemLabel.WarehouseId;
                    deductItem.StorageId = itemLabel.StorageLocationId;
                    if (!deductItem.SubmiterId.HasValue)
                    {
                        deductItem.SubmiterId = RT.IdentityId;
                        deductItem.SubmitTime = dbDateTime;
                    }
                }
                else
                {
                    //标签数小于需求数,需要拆分新耗用记录
                    deductQty = itemLabel.Qty;
                    deductItem.Qty -= deductQty;

                    WoCostItem deductItemNew = CreateNewDeductItem(deductItem, itemLabel, deductQty);
                    BackflushMaterialHelper.BatchSetCostNos(new EntityList<WoCostItem>() { deductItemNew });
                    RF.Save(deductItemNew);
                    deductItems.Add(deductItemNew);
                }
                itemLabel.Qty -= deductQty;
                requireQty -= deductQty;

                if (itemLabel.StorageLocationId > 0)
                    mesLabelDatas.Add(NewMesLabelData(itemLabel, deductQty));

                DB.Update<ItemLabel>().Set(p => p.Qty, p => p.Qty - deductQty).Where(p => p.Id == itemLabel.Id).Execute();
            }
            return mesLabelDatas;
        }

        /// <summary>
        /// 创建耗用记录
        /// </summary>
        /// <param name="deductItem"></param>
        /// <param name="itemLabel"></param>
        /// <param name="deductCount"></param>
        /// <returns></returns>
        private static WoCostItem CreateNewDeductItem(WoCostItem deductItem, ItemLabel itemLabel, decimal deductCount)
        {
            var deductItemNew = new WoCostItem
            {
                RecordType = deductItem.RecordType,
                BatchNo = deductItem.BatchNo,
                BarCode = deductItem.BarCode,
                Qty = deductCount,
                CostItemLabel = itemLabel,
                ItemExtProp = itemLabel.ItemExtProp,
                ItemExtPropName = itemLabel.ItemExtPropName,
                ProjectNo = itemLabel.ProjectNo,
                ConsumeType = deductItem.ConsumeType,
                StationId = deductItem.StationId,
                WarehouseId = itemLabel.WarehouseId,
                StorageId = itemLabel.StorageLocationId,
                State = WoCostItemState.Submitted,
                ItemId = deductItem.ItemId,
                ProcessId = deductItem.ProcessId,
                WorkOrderId = deductItem.WorkOrderId,
                FactoryId = deductItem.FactoryId,
                WipResourceId = deductItem.WipResourceId,
                AlternativeItemId = deductItem.AlternativeItemId,
                WorkOrderBomId = deductItem.WorkOrderBomId,
                Submiter = deductItem.Submiter,
                SubmitTime = deductItem.SubmitTime,
                Alter = deductItem.Alter,
                AlterGroup = deductItem.AlterGroup,
            };

            return deductItemNew;
        }


        /// <summary>
        /// 创建倒扣料
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="factoryId"></param>
        /// <param name="workOrderId"></param>
        /// <param name="productQty"></param>
        /// <param name="workOrderBoms"></param>
        /// <param name="retrospectType"></param>
        /// <param name="submitTime"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public virtual EntityList<WoCostItem> CreateDeductItems(string barcode, double? resourceId,
            double? processId, double? stationId, double factoryId, double workOrderId, decimal productQty,
            IList<WorkOrderBom> workOrderBoms, RetrospectType retrospectType, DateTime? submitTime = null)
        {

            if (workOrderBoms is null)
            {
                throw new ArgumentNullException(nameof(workOrderBoms));
            }

            EntityList<WoCostItem> deductItems = new EntityList<WoCostItem>();

            foreach (var workOrderBom in workOrderBoms)
            {
                var deductItem = new WoCostItem
                {
                    Qty = workOrderBom.SingleQty * productQty,
                    CostItemLabel = null,
                    State = WoCostItemState.ToSubmit,
                    RecordType = WoCostItemType.DeductItem,
                    ItemId = workOrderBom.ItemId,
                    ItemExtProp = workOrderBom.ItemExtProp,
                    ItemExtPropName = workOrderBom.ItemExtPropName,
                    ProjectNo = workOrderBom.ProjectMaintainCode,
                    WipResourceId = resourceId,
                    ProcessId = processId,
                    WorkOrderId = workOrderId,
                    ConsumeType = workOrderBom.ItemConsumeMode,
                    WorkOrderBomId = workOrderBom.Id,
                    StationId = stationId,
                    FactoryId = factoryId,
                    Alter = workOrderBom.Alter,
                    AlterGroup = workOrderBom.AlterGroup,
                    SubmiterId = RT.IdentityId,
                    SubmitTime = submitTime ?? DateTime.Now
                };


                if (retrospectType == RetrospectType.Single)
                {
                    deductItem.BarCode = barcode;
                }
                else
                {
                    deductItem.BatchNo = barcode;
                }

                deductItems.Add(deductItem);
            }

            BackflushMaterialHelper.BatchSetCostNos(deductItems);

            return deductItems;
        }

        /// <summary>
        /// 创建倒扣工单耗用单
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="resourceId"></param>
        /// <param name="processId"></param>
        /// <param name="stationId"></param>
        /// <param name="product"></param>
        /// <param name="isFinsh">是否产品完工</param>
        /// <exception cref="ValidationException"></exception>
        public virtual EntityList<WoCostItem> CreateDeductItems(string barcode, double? resourceId,
            double? processId, double? stationId, product product, bool isFinsh)
        {
            EntityList<WoCostItem> deductItems = new EntityList<WoCostItem>();
            var wo = product.WorkOrderMove;
            if (wo == null)
                throw new ValidationException("倒扣料执行失败，生产采集运行时产品【product】的工单为空！".L10N());

            //未维护产线线边仓信息时,不执行倒扣料
            LinesideWarehouse lineWh = RT.Service.Resolve<LinesideWarehouseController>().GetLinesideWarehouse(wo.WorkShopId, wo.ResourceId);
            if (lineWh == null)
                return deductItems;

            if (!wo.FactoryId.HasValue)
                throw new ValidationException("倒扣料执行失败，工单【{0}】的工厂为空！".L10nFormat(wo.No));

            var workOrderBoms = DB.Query<WorkOrderBom>().Where(x => x.WorkOrderId == product.WorkOrderId && x.IsRecoilItem && !x.IsAlternative).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var recoilBoms = new EntityList<WorkOrderBom>();
            if (isFinsh)
            {
                //获取工单BOM且不在工序BOM中的物料进行扣料
                var itemIds = product.Routing.Processes.SelectMany(x => x.Boms).Select(x => x.ItemId).ToList();
                var boms = workOrderBoms.Where(x => !itemIds.Contains(x.ItemId));
                recoilBoms.AddRange(boms);
            }
            else
            {
                //BOM物料既存在工单BOM又存在工序BOM中且是反冲物料,则过站时不触发齐套检,但要生成当前工序的工单耗用单
                var itemIds = product.Routing.Current.Boms.Select(x => x.ItemId).ToList();
                var boms = workOrderBoms.Where(x => itemIds.Contains(x.ItemId));
                recoilBoms.AddRange(boms);
            }

            if (!recoilBoms.Any())
            {
                //没有需要倒扣物料，直接返回
                return deductItems;
            }

            var retrospectType = DB.Query<ItemBatchRule>().Where(p => p.ItemId == product.ItemId).FirstOrDefault().RetrospectType;

            deductItems.AddRange(CreateDeductItems(barcode, resourceId, processId, stationId, wo.FactoryId ?? 0, product.WorkOrderId, product.Qty, recoilBoms, retrospectType));

            RF.BatchInsert(deductItems);
            deductItems.MarkSaved();
            return deductItems;
        }

    }
}
