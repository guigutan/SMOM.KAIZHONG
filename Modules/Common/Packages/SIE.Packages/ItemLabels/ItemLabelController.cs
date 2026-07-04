using SIE.Core.Common;
using SIE.Core.WorkOrders;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Items;
using SIE.Packages.ItemLabels.Datas;
using SIE.Resources.Enterprises;
using SIE.Warehouses;
using SIE.Warehouses.ItemStockData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Packages.ItemLabels
{
    /// <summary>
    /// 物料标签
    /// </summary>
    public class ItemLabelController : DomainController
    {
        /// <summary>
        /// 根据ID获取物料标签
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public virtual EntityList<ItemLabel> GetItemLabelsByIds(List<double> ids)
        {
            var list = ids.SplitContains(temps =>
            {
                return Query<ItemLabel>().Where(p => temps.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return list;
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="criteria">物料标签查询实体</param>
        ///  <param name="isPaging">分页</param>
        /// <returns>物料标签</returns>
        public virtual EntityList<ItemLabel> GetItemLabels(ItemLabelCriteria criteria, bool isPaging = true)
        {
            using (Diagnostics.DebugTrace.Start("标签查询：".L10N()))
            {
                var query = Query<ItemLabel>();
                if (!criteria.Label.IsNullOrEmpty())
                    query.Where(p => p.Label.Contains(criteria.Label));
                //if (!criteria.WorkOrderNo.IsNullOrEmpty())
                //    query.Join<ItemLabelWorkOrder>((x, y) => x.Id == y.ItemLabelId).Where<ItemLabelWorkOrder>((x, y) => y.WorkOrder.No.Contains(criteria.WorkOrderNo));
                if (!criteria.WorkOrderNo.IsNullOrEmpty())
                    query.Where(p => p.WorkOrder.No.Contains(criteria.WorkOrderNo));
                if (criteria.ItemId.HasValue)
                    query.Where(p => p.ItemId == criteria.ItemId);
                if (criteria.ItemType.HasValue)
                    query.Where(p => p.Item.Type == criteria.ItemType);
                if (criteria.SourceType.HasValue)
                    query.Where(p => p.SourceType == criteria.SourceType);
                if (criteria.CreateDate.BeginValue != null)
                    query.Where(p => p.CreateDate >= criteria.CreateDate.BeginValue.Value);
                if (criteria.CreateDate.EndValue != null)
                    query.Where(p => p.CreateDate <= criteria.CreateDate.EndValue.Value);
                if (!criteria.ShowZero)
                    query.Where(p => p.Qty != 0 || p.NgQty != 0);
                if (!criteria.Lot.IsNullOrEmpty())
                    query.Where(p => p.Lot.Contains(criteria.Lot));
                if (!criteria.Exidv.IsNullOrEmpty())
                    query.Where(p => p.Exidv.Contains(criteria.Exidv));
                if (!criteria.Exidv2.IsNullOrEmpty())
                    query.Where(p => p.Exidv2.Contains(criteria.Exidv2));
                if (criteria.ItemLabelState.HasValue)
                {
                    query.Where(p => p.ItemLabelState == criteria.ItemLabelState);
                }
                if (!criteria.ShortDescription.IsNullOrEmpty())
                    query.Where(p => p.Item.ShortDescription.Contains(criteria.ShortDescription));
                if (!criteria.Licha.IsNullOrEmpty())
                    query.Where(p => p.Licha.Contains(criteria.Licha));
                if (!criteria.MrpController.IsNullOrEmpty())
                    query.Where(p => p.Item.MrpController.Contains(criteria.MrpController));

                if (isPaging)
                {
                    return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
                }
                else
                {
                    return query.ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                }
            }
        }

        /// <summary>
        /// 获取多个物料标签
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public virtual EntityList<ItemLabel> GetItemLabels(string label)
        {
            return Query<ItemLabel>().Where(p => p.Label == label)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="labels">标签号</param>
        /// <returns>物料标签</returns>
        public virtual EntityList<ItemLabel> GetItemLabels(List<string> labels)
        {
            var elo = new EagerLoadOptions().LoadWith(ItemLabel.RelationProperty);
            return labels.SplitContains((tempNos) =>
            {
                return Query<ItemLabel>().Where(p => tempNos.Contains(p.Label)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="labels">标签号</param>
        /// <returns>物料标签</returns>
        public virtual EntityList<ItemLabel> GetItemLabelsWithWorkOrder(List<string> labels)
        {
            var elo = new EagerLoadOptions().LoadWith(ItemLabel.WorkOrderProperty);
            return labels.SplitContains((tempNos) =>
            {
                return Query<ItemLabel>().Where(p => tempNos.Contains(p.Label)).ToList(null, elo);
            });
        }

        /// <summary>
        /// 按物料ID列表获取物料标签清单
        /// </summary>
        /// <param name="itemIds">物料Id列表</param>
        /// <returns>物料标签清单</returns>
        public virtual EntityList<ItemLabel> GetItemLabels(List<double> itemIds)
        {
            return itemIds.SplitContains(tempIds =>
            {
                return Query<ItemLabel>()
                    .Where(x => tempIds.Contains(x.ItemId) && x.Qty > 0)
                    .ToList();
            });
        }

        /// <summary>
        /// 按物料ID列表获取物料标签
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual EntityList<ItemLabel> GetReceiveItemLabels(List<double> itemIds)
        {
            return itemIds.SplitContains(tempIds =>
            {
                return Query<ItemLabel>()
                    .Where(x => tempIds.Contains(x.Id) && x.Qty > 0)
                    .ToList();
            });
        }

        /// <summary>
        /// 根据物料Id查询物料标签基本信息
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual List<ItemLabelBaseData> GetItemLabelBaseDatas(List<double> itemIds)
        {
            List<ItemLabelBaseData> itemLabelBaseDatas = new List<ItemLabelBaseData>();
            itemIds.SplitDataExecute(tempIds =>
            {
                var list = Query<ItemLabel>()
                    .Where(x => tempIds.Contains(x.ItemId) && x.Qty > 0)
                    .Select(p => new
                    {
                        p.Id,
                        p.ItemId,
                        p.ItemExtProp,
                        p.ItemExtPropName,
                        p.WarehouseId,
                        p.StorageLocationId,
                        p.IsSerialNumber,
                        p.Qty,
                    })
                    .ToList<ItemLabelBaseData>();
                itemLabelBaseDatas.AddRange(list);
            });
            return itemLabelBaseDatas;
        }

        /// <summary>
        /// 匹配物料标签查询扩展条件
        /// </summary>
        /// <param name="query">实体查询器</param>
        /// <param name="criteria">物料标签查询实体</param>
        protected virtual void MacthExtensionCrieriaCondition(IEntityQueryer<ItemLabel> query, ItemLabelCriteria criteria)
        {
        }

        /// <summary>
        /// 判断物料标签是否存在
        /// </summary>
        /// <param name="barcode">标签号</param>
        /// <returns>返回是否存在</returns>
        /// <exception cref="ArgumentNullException">ArgumentNullException</exception>
        public virtual bool Exists(string barcode)
        {
            if (barcode == null)
                throw new ArgumentNullException(nameof(barcode));
            return Query<ItemLabel>().Where(p => p.Label == barcode).Count() > 0;
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="label">标签号</param>
        /// <returns>物料标签</returns>
        /// <exception cref="ArgumentNullException">空异常</exception>
        public virtual ItemLabel GetItemLabel(string label)
        {
            return Query<ItemLabel>().Where(p => p.Label == label).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料标签(凯中)
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public virtual ItemLabel GetItemLabelKz(string label)
        {
            return Query<ItemLabel>().Where(p => p.Exidv == label || p.Exidv2 == label || p.Label == label).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
        /// <summary>
        /// 获取物料标签(凯中)
        /// </summary>
        /// <param name="labels"></param>
        /// <returns></returns>
        public virtual EntityList<ItemLabel> GetItemLabelKz(List<string> labels)
        {
            return Query<ItemLabel>().Where(p => labels.Contains(p.Exidv) || labels.Contains(p.Exidv2) || labels.Contains(p.Label)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="labelList">标签号</param>
        /// <returns>物料标签</returns>
        public virtual EntityList<ItemLabel> GetItemLabelDatas(List<string> labelList)
        {
            return labelList.SplitContains(nos =>
            {
                return Query<ItemLabel>().Where(p => nos.Contains(p.Label)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="labelList">标签号</param>
        /// <returns>物料标签</returns>
        public virtual EntityList<ItemLabel> GetItemLabelDatas(List<string> labelList, List<double?> locIds)
        {
            return labelList.SplitContains(nos =>
            {
                return Query<ItemLabel>().Where(p => nos.Contains(p.Label) && locIds.Contains(p.StorageLocationId)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
        }

        /// <summary>
        /// 获取物料标签(提升性能只查询业务使用到的Id,RemainQty,LabelStatus,Label)
        /// </summary>
        /// <param name="ids">ID集合</param>
        /// <returns>物料标签</returns>
        public virtual List<ItemLabelRemainQtyData> GetItemLabelDatas(List<double> ids)
        {
            if (ids == null || ids.Count == 0)
            {
                return new List<ItemLabelRemainQtyData>();
            }
            var itemLabelData = new List<ItemLabelRemainQtyData>();

            for (int i = 0; i < Math.Ceiling((double)ids.Count / 1000); i++)
            {
                var q = Query<ItemLabel>().Select(p => new { p.Id, p.Label, p.Qty }).Where(x => ids.Skip(i * 1000).Take(1000).Contains(x.Id));

                itemLabelData.AddRange(q.ToList<ItemLabelRemainQtyData>().ToList());
            }
            return itemLabelData;
        }

        /// <summary>
        /// 根据包装关系ID获取ItemLabel列表
        /// </summary>
        /// <param name="relationId">关系ID</param>
        /// <param name="eagerLoad">贪婪加载</param>
        /// <returns>ItemLabel列表</returns>
        public virtual EntityList<ItemLabel> GetItemLabelByRelationId(double relationId, EagerLoadOptions eagerLoad = null)
        {
            return Query<ItemLabel>().Where(f => f.RelationId == relationId).ToList(eagerLoad: eagerLoad);
        }

        /// <summary>
        /// 根据包装关系ID获取ItemLabel列表
        /// </summary>
        /// <param name="relationIds">关系ID</param>
        /// <returns>ItemLabel列表</returns>
        public virtual EntityList<ItemLabel> GetItemLabelByRelationId(List<double> relationIds)
        {
            var itemLabelList = new EntityList<ItemLabel>();
            for (int i = 0; i < Math.Ceiling((double)relationIds.Count / 1000); i++)
            {
                var query = Query<ItemLabel>().Where(f => f.RelationId > 0 && relationIds.Skip(i * 1000).Take(1000).Contains((double)f.RelationId));
                itemLabelList.AddRange(query.ToList(eagerLoad: new EagerLoadOptions().LoadWithViewProperty()));
            }
            return itemLabelList;
        }


        /// <summary>
        /// 获取物料基本标签
        /// </summary>
        /// <param name="nos">标签号</param>
        /// <returns>物料标签基本数据</returns>
        public virtual IList<ItemLabelBaseData> GetItemLabelBaseDatas(List<string> nos)
        {
            return DataProcessEx.SplitContains(nos, pNos =>
            {
                var query = Query<ItemLabel>();
                query.LeftJoin<PackingRelation>((l, r) => l.RelationId == r.Id);
                query.Select<PackingRelation>((l, r) => new { No = l.Label, PackageNo = r.PackageNo, Qty = l.Qty });
                query.Where(p => pNos.Contains(p.Label));
                return query.ToList<ItemLabelBaseData>();
            });
        }

        /// <summary>
        /// 创建物料标签
        /// </summary>
        /// <param name="item">物料</param>
        /// <param name="qty">标签数量</param>
        /// <param name="label">标签条码</param>        
        /// <param name="labelSource">条码来源类型</param>        
        /// <param name="woId">工单基础信息</param>
        /// <param name="factoryId">工厂Id</param>
        /// <param name="itemExtProp">扩展属性</param>
        /// <param name="itemExtPropName">扩展属性名称</param>
        /// <param name="projectNo">项目号</param>
        /// <returns>物料标签</returns>
        public virtual ItemLabel CreateItemLabel(Item item, decimal qty, string label, LabelSource labelSource,
            double? woId, double? factoryId, string itemExtProp, string itemExtPropName, string projectNo = null)
        {
            var itemLabel = GetItemLabel(label);

            //验证条码是否存在，存在则更新，不存在则新增
            if (itemLabel == null)
            {
                itemLabel = new ItemLabel();
            }

            itemLabel.Label = label;
            itemLabel.Qty = qty;
            if (itemLabel.InitialQty == null || itemLabel.InitialQty == 0)
                itemLabel.InitialQty = itemLabel.Qty;
            itemLabel.ItemId = item.Id;
            itemLabel.UnitId = item.UnitId;
            itemLabel.SourceType = labelSource;
            itemLabel.WorkOrderId = woId;
            itemLabel.FactoryId = factoryId;
            itemLabel.ItemExtProp = itemExtProp;
            itemLabel.ItemExtPropName = itemExtPropName;
            itemLabel.ProjectNo = projectNo.IsNullOrEmpty() ? "*" : projectNo;
            itemLabel.IsSerialNumber = RT.Service.Resolve<ItemStockBaseController>()
                .CheckItemIsSer(item.Id);

            RF.Save(itemLabel);
            return itemLabel;
        }

        /// <summary>
        /// 生成工单投入
        /// </summary>
        /// <param name="itemLabelId"></param>
        /// <param name="woId"></param>
        /// <param name="qty"></param>
        /// <returns></returns>
        public virtual ItemLabelWorkOrder CreateNewItemLabelWorkOrder(double itemLabelId, double woId, decimal qty)
        {
            ItemLabelWorkOrder itemLabelWorkOrder = new ItemLabelWorkOrder
            {
                ItemLabelId = itemLabelId,
                Qty = qty,
                WorkOrderId = woId,
            };
            return itemLabelWorkOrder;
        }

        /// <summary>
        /// 验证是否已被打包
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        /// <returns>bool</returns>
        public virtual bool IsNotBeDoPacking(ItemLabel itemLabel)
        {
            if (itemLabel == null) throw new ArgumentNullException(nameof(itemLabel));
            if (itemLabel.Relation == null) return true;
            return false;
        }

        /// <summary>
        /// 获取指定根包装的Item
        /// </summary>
        /// <param name="rootPackingRelationId">rootPackingRelationId</param>
        /// <param name="hasWorkOrder">hasWorkOrder</param>
        /// <param name="isAsc">isAsc</param>
        /// <returns>物料标签</returns>
        public virtual ItemLabel GetRootPackingRelationItemLabel(double rootPackingRelationId, bool hasWorkOrder = true, bool isAsc = true)
        {
            var q = Query<ItemLabel>().Exists<PackingRelation>((i, p) => p.Where(f => f.Id == i.RelationId && f.RootId == rootPackingRelationId));
            if (hasWorkOrder) q.Where(f => f.WorkOrderId > 0);
            if (isAsc) q.OrderBy(f => f.Id);
            else q.OrderByDescending(f => f.Id);
            return q.FirstOrDefault();
        }

        /// <summary>
        /// 获取ItemLabel根条码
        /// </summary>
        /// <param name="rootPackingRelationId">根节点</param>
        /// <returns>物料标签</returns>
        public virtual EntityList<ItemLabel> GetRootPackingRelationItemLabels(double rootPackingRelationId)
        {
            var q = Query<ItemLabel>().Exists<PackingRelation>((i, p) => p.Where(f => f.Id == i.RelationId && f.RootId == rootPackingRelationId));
            return q.ToList();
        }

        /// <summary>
        /// 获取指定根包装的SN
        /// </summary>
        /// <param name="rootPackingRelationId">包装关系id</param>
        /// <returns>SN列表</returns>
        public virtual IList<string> GetRootPackingRelationSN(double rootPackingRelationId)
        {
            return Query<ItemLabel>().Exists<PackingRelation>((i, p) => p.Where(f => f.Id == i.RelationId && f.RootId == rootPackingRelationId)).Select(f => f.Label).ToList<string>();
        }

        /// <summary>
        /// 判断标签是否已经接收
        /// 标签可以重复接收，前提是前一次的标签做了退料处理
        /// </summary>
        /// <returns>已接收返回true，未接收返回false</returns>
        public virtual bool IsReceived(string label)
        {
            return Query<ItemLabel>().Where(p => p.Label == label && p.Qty > 0).Count() > 0;
        }

        /// <summary>
        /// 更新物料标签状态
        /// </summary>
        /// <param name="itemLabelId">物料标签ID</param>
        /// <param name="remainQty">剩余数量</param>
        public virtual int BuckleRemainQty(double itemLabelId, decimal remainQty)
        {
            return DB.Update<ItemLabel>().Set(s => s.Qty, s => s.Qty - remainQty).Where(w => w.Id == itemLabelId).Execute();
        }

        /// <summary>
        /// 获取标签属性
        /// </summary>
        /// <param name="itemLabelId">标签Id</param>
        /// <returns>标签属性列表</returns>
        public virtual EntityList<LabelPropertyValue> GetLabelPropertyValueList(double itemLabelId)
        {
            return Query<LabelPropertyValue>().Where(p => p.ItemLabelId == itemLabelId).ToList();
        }



        /// <summary>
        /// 取最大子标签－流水号
        /// </summary>
        /// <returns></returns>
        public virtual int GetMaxChildeIndex(string ParentLabel, string TailChar = "-")
        {
            var childLabels = Query<ItemLabel>()
                                .Where(o => o.OriginalLabel == ParentLabel && o.Label.Contains(ParentLabel + TailChar + "%"))
                                .ToList();
            if (childLabels.Count == 0)
                return 0;
            ///以后后面不是个数字
            var matchChildLabels = childLabels.Where(O =>
                                                    O.Label.Split(TailChar[0]).Length == 2
                                                    && IsNumber(O.Label.Split(TailChar[0])[1]))
                                            .ToList();

            if (matchChildLabels.Count == 0)
                return 0;

            return matchChildLabels.Max(o => GetNumber(o.Label.Split(TailChar[0])[1]));
        }

        private bool IsNumber(string str)
        {
            int i = -1;
            bool isOk = int.TryParse(str, out i);
            return isOk;
        }

        private int GetNumber(string str)
        {
            int i = -1;
            int.TryParse(str, out i);
            return i;
        }

        /// <summary>
        /// 获取物料标签的扩展属性列表
        /// </summary>
        /// <param name="itemLabelIds">物料标签ID列表</param>
        /// <returns></returns>
        public virtual EntityList<LabelPropertyValue> GetItemLabelPropertyValues(List<double> itemLabelIds)
        {
            return itemLabelIds.SplitContains(tempIds =>
            {
                return Query<LabelPropertyValue>()
                    .Where(x => tempIds.Contains(x.ItemLabelId))
                    .ToList();
            });
        }

        /// <summary>
        /// 获取投入工单数据
        /// </summary>
        /// <param name="itemLabelIds">物料标签</param>
        /// <returns>投入工单数据</returns>
        public virtual EntityList<ItemLabelWorkOrder> GetItemLabelWorkOrders(List<double> itemLabelIds)
        {
            return itemLabelIds.SplitContains(tempIds =>
            {
                return Query<ItemLabelWorkOrder>()
                    .Where(x => tempIds.Contains(x.ItemLabelId))
                    .ToList();
            });
        }

        /// <summary>
        /// 获取投入工单数据
        /// </summary>
        /// <param name="labelNo">物料标签</param>
        /// <param name="locId">库位</param>
        /// <param name="itemExtPropName">物料扩展属性</param>
        /// <returns>投入工单数据</returns>
        public virtual EntityList<ItemLabelWorkOrder> GetItemLabelWorkOrders(string labelNo, string itemExtPropName, double locId)
        {
            return Query<ItemLabelWorkOrder>().Join<ItemLabel>((x, y) => x.ItemLabelId == y.Id)
               .Where<ItemLabel>((x, y) => y.StorageLocationId == locId && y.Label == labelNo && y.ItemExtPropName == itemExtPropName)
               .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取明细行
        /// </summary>
        /// <param name="itemlabelsIds"></param>
        /// <returns></returns>
        public virtual EntityList<ItemLabelWorkOrder> GetItemLabelsDetail(List<double> itemlabelsIds)
        {
            var itemLabelWorkOrderList = new EntityList<ItemLabelWorkOrder>();
            for (int i = 0; i < Math.Ceiling((double)itemlabelsIds.Count / 1000); i++)
            {
                var queryRsult = Query<ItemLabelWorkOrder>().Where(p => itemlabelsIds.Skip(i * 1000).Take(1000).Contains(p.ItemLabelId));
                itemLabelWorkOrderList.AddRange(queryRsult.ToList(null, new EagerLoadOptions().LoadWithViewProperty()));
            }
            return itemLabelWorkOrderList;
        }

        /// <summary>
        /// 获取指定仓库 库位不良的物料标签
        /// </summary>
        /// <param name="label"></param>
        /// <param name="whId"></param>
        /// <param name="locId"></param>
        /// <returns></returns>
        public virtual ItemLabel GetNgItemlable(string label, double whId, double locId)
        {
            return Query<ItemLabel>().Where(m => m.Label == label && m.WarehouseId == whId && m.StorageLocationId == locId && m.NgQty > 0).FirstOrDefault();
        }

        /// <summary>
        /// 获取存在的物料标签
        /// </summary>
        /// <param name="label"></param>
        /// <param name="whId"></param>
        /// <param name="locId"></param>
        /// <returns></returns>
        public virtual ItemLabel GetExsitedItemlable(string label, double whId, double locId)
        {
            return Query<ItemLabel>().Where(m => m.Label == label && m.WarehouseId == whId && m.StorageLocationId == locId).FirstOrDefault();
        }

        /// <summary>
        /// 获取物料标签投入
        /// </summary>
        /// <param name="labelId"></param>
        /// <param name="woId"></param>
        /// <returns></returns>
        public virtual ItemLabelWorkOrder GetExsitedItemWorkOrder(double labelId, double woId)
        {
            return Query<ItemLabelWorkOrder>().Where(p => p.ItemLabelId == labelId && p.WorkOrderId == woId).FirstOrDefault();
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="label">标签号</param>
        /// <returns>物料标签</returns>
        /// <exception cref="ArgumentNullException">空异常</exception>
        public virtual ItemLabel GetPackingItemLabel(string label)
        {
            return Query<ItemLabel>().Where(p => p.Label == label && p.SourceType == LabelSource.BatchWip).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据物料标签Id查询物料标签与物料基本信息
        /// </summary>
        /// <param name="itemIds"></param>
        /// <returns></returns>
        public virtual List<ItemLabel> GetItemLabelPrintDatas(List<double> itemIds)
        {
            List<ItemLabel> itemLabelsDatas = new List<ItemLabel>();
            itemIds.SplitDataExecute(tempIds =>
            {
                var list = Query<ItemLabel>()
                    .Where(x => tempIds.Contains(x.Id) && x.Qty > 0)
                     .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                itemLabelsDatas.AddRange(list);
            });
            return itemLabelsDatas;
        }

        #region 物料标签导入命令
        /// <summary>
        /// 根据物料编码获取物料
        /// </summary>
        /// <param name="itemCodeList"></param>
        /// <returns></returns>
        public virtual EntityList<Item> GetItemByCode(List<string> itemCodeList)
        {
            var itemList = itemCodeList.SplitContains(tempCodes =>
            {
                return Query<Item>()
                .Where(p => tempCodes.Contains(p.Code))
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return itemList;
        }

        /// <summary>
        /// 根据工厂编码判断工厂是否存在
        /// </summary>
        /// <param name="itemCodeList"></param>
        /// <returns></returns>
        public virtual EntityList<Enterprise> FactoryIsExists(List<string> itemCodeList)
        {
            var factoryList = itemCodeList.SplitContains(tempCodes =>
            {
                return Query<Enterprise>()
                .Join<EnterpriseLevel>((x, y) => x.LevelId == y.Id)
                .Where<EnterpriseLevel>((x, y) => itemCodeList.Contains(x.Code) && y.Type == EnterpriseType.Plant && x.InvOrgId == RT.InvOrg)
                .ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            factoryList.ForEach(i =>
            {
                i.TreePId = 0;
            });
            return factoryList;
        }

        /// <summary>
        /// 根据物料编码获取物料标签
        /// </summary>
        /// <param name="itemCodeList"></param>
        /// <returns></returns>
        public virtual EntityList<ItemLabel> GetItemLabelList(List<string> itemCodeList)
        {
            using (DataAuth.DataAuths.LoadAll())
            {
                var itemLabelList = itemCodeList.SplitContains(tempCodes =>
                {
                    return Query<ItemLabel>()
                    .Exists<Item>((x, y) => y.Where(p => p.Id == x.ItemId && tempCodes.Contains(p.Code))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
                });
                return itemLabelList;
            }

        }

        /// <summary>
        /// 根据物料编码获取库存
        /// </summary>
        /// <param name="itemCodeList"></param>
        /// <returns></returns>
        public virtual EntityList<ItemStockDataBase> GetItemStockDataBaseList(List<string> itemCodeList)
        {
            var stockBaseList = itemCodeList.SplitContains(tempCodes =>
            {
                return Query<ItemStockDataBase>()
                .Exists<Item>((x, y) => y.Where(p => p.Id == x.ItemId && tempCodes.Contains(p.Code))).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return stockBaseList;
        }

        /// <summary>
        /// 根据工单号获取不为取消的工单
        /// </summary>
        /// <param name="woNoList"></param>
        /// <returns></returns>
        public virtual EntityList<WorkOrder> GetWorkOrderList(List<string> woNoList)
        {
            var woOrderList = woNoList.SplitContains(tempNos =>
            {
                return Query<WorkOrder>()
                .Where(p => tempNos.Contains(p.No) && p.State != WorkOrderState.Close && p.State != WorkOrderState.Finish).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return woOrderList;
        }

        /// <summary>
        /// 根据供应商编码获取供应商信息
        /// </summary>
        /// <param name="supplierCodeList"></param>
        /// <returns></returns>
        public virtual EntityList<Supplier> GetSupplierList(List<string> supplierCodeList)
        {
            var supplierList = supplierCodeList.SplitContains(codes =>
            {
                return Query<Supplier>().Where(p => codes.Contains(p.Code)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            });
            return supplierList;
        }

        /// <summary>
        /// 验证序列号管控物料标签是否重复，序列号管控不可重复
        /// </summary>
        /// <param name="labelItemCodeList"></param>
        /// <param name="item"></param>
        /// <param name="stockBaseList">序列号库存列表</param>
        /// <param name="itemLabelList">物料标签表</param>
        /// <param name="importlabel"></param>
        /// <param name="isSerialNumber">序列号管控</param>
        /// <param name="isBatch">批次管控</param>
        /// <returns></returns>
        public virtual bool SerItemLabelRepeat(List<Tuple<string, string>> labelItemCodeList, Item item, EntityList<ItemStockDataBase> stockBaseList, EntityList<ItemLabel> itemLabelList, string importlabel, out bool? isSerialNumber, out bool? isBatch)
        {
            if (item != null)
            {
                // 重复返回true
                var isSer = stockBaseList.FirstOrDefault(p => p.ItemId == item.Id)?.IsSerialNumber;
                isBatch = stockBaseList.FirstOrDefault(p => p.ItemId == item.Id)?.IsBatch;
                isSerialNumber = isSer;
                var importLabels = labelItemCodeList.Where(p => item.Code == p.Item2 && p.Item1 == importlabel).Select(p => p.Item1).ToList();
                var baseLabel = itemLabelList.FirstOrDefault(p => p.ItemCode == item.Code && p.Label == importlabel);
                if (isSer != null && (bool)isSer && (importLabels.Count > 1 || baseLabel != null)) //序列号管控  导入列表重复(>1) 数据库重复(!=null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                isSerialNumber = null;
                isBatch = null;
                return false;
            }
        }



        /// <summary>
        /// 根据物料标签id获取物料标签
        /// </summary>
        /// <param name="itemLabelIds"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<ItemLabel> GetItemLabelByIds(List<double> itemLabelIds)
        {
            return itemLabelIds.SplitContains(tempIds =>
            {
                return Query<ItemLabel>().Where(p => tempIds.Contains(p.Id)).ToList();
            });
        }
        #endregion
    }
}