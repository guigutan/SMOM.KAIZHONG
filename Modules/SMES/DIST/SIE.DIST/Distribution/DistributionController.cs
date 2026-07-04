using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Core.Logs;
using SIE.DIST.Distribution;
using SIE.DIST.Distribution.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.CallMaterials;
using SIE.EventMessages.IDistributions;
using SIE.Items;
using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using SIE.Packages.ItemLabels;
using SIE.Resources.WipResources;
using SIE.Utils;
using System;
using System.Linq;

namespace SIE.DIST
{
    /// <summary>
    /// 配送单控制器
    /// </summary>
    public partial class DistributionController : DomainController
    {
        /// <summary>
        /// 获取配送单列表
        /// </summary>
        /// <param name="criteria">配送单查询实体</param>
        /// <returns>配送单列表</returns>
        public virtual EntityList<DistributionBill> GetDistributionBills(DistributionBillCriteria criteria)
        {
            var query = Query<DistributionBill>();
            if (!criteria.ContainerNo.IsNullOrEmpty())
                query.Where(p => p.ContainerNo.Contains(criteria.ContainerNo));
            if (criteria.WorkOrderId.HasValue)
                query.Where(p => p.WorkOrderId == criteria.WorkOrderId);
            if (criteria.ItemId.HasValue)
                query.Where(p => p.ItemId == criteria.ItemId);
            return query.OrderBy(criteria.OrderInfoList).ToList(criteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 验证载具容器
        /// </summary>
        /// <param name="barcode">周转箱条码</param>
        /// <exception cref="ValidationException">周转箱不可用，当前状态非闲置</exception>
        public virtual void ValidateBarcode(string barcode)
        {
            if (barcode == null)
                throw new ArgumentNullException(nameof(barcode));
            var box = GetTurnoverBox(barcode);
            if (box.State != BoxState.Unused)
                throw new ValidationException("周转箱[{0}]不可用，当前状态为[{1}]，非[{2}]".L10nFormat(barcode, EnumViewModel.EnumToLabel(box.State).L10N(), EnumViewModel.EnumToLabel(BoxState.Unused).L10N()));
        }

        /// <summary>
        /// 验证箱号标签
        /// </summary>
        /// <param name="label">标签号</param>
        /// <param name="itemId">物料ID</param>
        ///  <returns>物料标签</returns>
        /// <exception cref="ValidationException">箱号标签不存在,请重新扫描箱号标签</exception>
        /// <exception cref="ValidationException">不允许配送，箱号物流状态为非已发料</exception>
        /// <exception cref="ValidationException">不允许配送，箱号标签可用数量等于0</exception>
        public virtual PackingLabel ValidatePackingLabel(string label, double itemId)
        {
            if (label.IsNullOrEmpty())
                throw new ValidationException("标签号不能为空".L10N());
            var packLabel = RT.Service.Resolve<PackingLabelController>().GetPackingLabel(label);
            if (packLabel == null)
                throw new ValidationException("标签[{0}]不存在,请重新扫描标签".L10nFormat(label));
            if (RT.Service.Resolve<IDistribution>().IsDistribution(label))
                throw new ValidationException("标签[{0}]已配送，不允许再配送".L10nFormat(label));
            if (packLabel.ItemId != itemId)
                throw new ValidationException("标签物料[{0}]与当前配送单配送物料[{1}]不匹配".L10nFormat(packLabel.Item?.Name, RF.GetById<Item>(itemId)?.Name));
            var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(label);
            if (itemLabel != null && itemLabel.SourceType == LabelSource.Distribution)
                throw new ValidationException("标签[{0}]已配送，不允许再配送".L10nFormat(label));
            if (itemLabel != null && itemLabel.SourceType == LabelSource.Receive)
                throw new ValidationException("标签[{0}]已接收，可直接上料，不需要配送".L10nFormat(label));
            //记录接口日志
            SaveGetPackingLabelLog(label);
            var callInfo = RT.Service.Resolve<ICallMaterial>().GetPackingLabel(label);
            if (callInfo != null && !callInfo.BillNo.IsNullOrEmpty())
                throw new ValidationException("不允许载具关联，叫料配送需PDA接收".L10nFormat(label));
            if (packLabel.Qty <= 0)
                throw new ValidationException("不允许配送，标签[{0}]可用数量等于0".L10nFormat(label));
            return packLabel;
        }

        /// <summary>
        /// 保存获取标签信息日志
        /// </summary>
        /// <param name="labelNo">标签号</param>
        private void SaveGetPackingLabelLog(string labelNo)
        {
            using (var tran = DB.AutonomousTransactionScope(DistEntityDataProvider.ConnectionStringName))
            {
                var inputValue = "标签号:{0}".L10nFormat(labelNo);
                var log = new InterfaceLog()
                {
                    Name = "ICallMaterial",
                    Method = "GetPackingLabel",
                    ControllerName = "DistributionController",
                    InputValue = inputValue,
                };

                RF.Save(log);
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存配送单
        /// </summary>
        /// <param name="goodsIssueId">发料单号</param>
        /// <param name="boxNo">载具号</param>
        /// <param name="lineId">配送产线</param>
        /// <param name="qty">配送数量</param>
        /// <param name="itemLabels">箱号标签</param>
        public virtual void SaveDistribution(double goodsIssueId, string boxNo, double lineId, decimal qty, string[] itemLabels = null)
        {
            if (boxNo == null)
                throw new ArgumentNullException(nameof(boxNo));
            GoodsIssue goodsIssue = RF.GetById<GoodsIssue>(goodsIssueId);
            TurnoverBox box = GetTurnoverBox(boxNo);
            CheckDistribution(goodsIssue, box, lineId, qty);  //验证参数
            using (var tran = DB.TransactionScope(DistEntityDataProvider.ConnectionStringName))
            {
                DistributionBill distribution = new DistributionBill();
                distribution.No = GetBillNo(typeof(DistributionBill));
                distribution.ContainerNo = boxNo;
                distribution.Qty = distribution.RemainderQty = distribution.OkQty = qty;
                distribution.BindingDate = DateTime.Now;
                distribution.BindingById = AppRuntime.IdentityId;
                distribution.WorkOrderId = goodsIssue.WorkOrderId;
                distribution.ResourceId = lineId;
                distribution.ItemId = goodsIssue.ItemId;
                distribution.State = DistributionState.DistributionIn;
                distribution.SourceId = goodsIssueId;
                distribution.SourceType = goodsIssue.GetType().Name;
                distribution.Shift = RT.Service.Resolve<WipResourceController>().GetWipResourceShift(distribution.ResourceId, distribution.BindingDate);
                RF.Save(distribution); //保存配送单

                //保存明细及属性(不扫描箱号直接保存发料属性)
                if (itemLabels != null && itemLabels.Length > 0)
                {
                    SaveDistributionBillDetail(goodsIssue, distribution, itemLabels);
                }
                else
                {
                    SaveGoodsIssuePropertyValue(goodsIssue, distribution);
                    goodsIssue.RemainderQty -= qty;
                    goodsIssue.DistributionQty += qty;
                }
                RF.Save(goodsIssue); //保存发料单
                box.State = BoxState.Inuse;
                RF.Save(box);
                tran.Complete();
            }
        }

        /// <summary>
        /// 保存配送明细
        /// </summary>
        /// <param name="goodsIssue">工单发料信息</param>
        /// <param name="distribution">配送单</param>
        /// <param name="itemLabels">物料条码集合</param>
        protected virtual void SaveDistributionBillDetail(GoodsIssue goodsIssue, DistributionBill distribution, string[] itemLabels)
        {
            decimal sumQty = 0m;
            foreach (string labelNo in itemLabels)
            {
                PackingLabel packingLabel = ValidatePackingLabel(labelNo, goodsIssue.ItemId);
                if (packingLabel == null)
                    throw new ValidationException("找不到标签[{0}]的信息".L10nFormat(labelNo));
                if (RT.Service.Resolve<IDistribution>().IsDistribution(labelNo))
                    throw new ValidationException("标签[{0}]已配送，不允许再配送".L10nFormat(labelNo));
                DistributionBillDetail detail = new DistributionBillDetail()
                {
                    ItemLabelNo = packingLabel.No,
                    OkQty = packingLabel.Qty,
                    Qty = packingLabel.Qty,
                    BillId = distribution.Id
                };
                SetDistributionBillDetailProperty(goodsIssue, detail);
                sumQty += packingLabel.Qty;
                RF.Save(detail);

                ////创建物料标签
                var item = packingLabel.Item;
                var factoryId = RT.Service.Resolve<EventMessages.MES.WorkOrders.IWorkOrderQuery>()
                    .GetWorkOrderFactoryId(distribution.WorkOrderId);

                RT.Service.Resolve<ItemLabelController>()
                    .CreateItemLabel(item, packingLabel.Qty, packingLabel.No, LabelSource.Distribution,
                    distribution.WorkOrderId, factoryId, itemExtProp: string.Empty, itemExtPropName: string.Empty);

                //标签属性取配送单物料属性
                SetItemLabelProperty(goodsIssue, packingLabel.No);
            }

            if (sumQty > goodsIssue.RemainderQty)
            {
                throw new ValidationException("发料单{0}库存不足配送数，请修改配送数量".L10nFormat(goodsIssue.SendNo));
            }
            else
            {
                goodsIssue.RemainderQty -= sumQty;
            }

            goodsIssue.DistributionQty += sumQty;
            RF.Save(goodsIssue);
        }

        /// <summary>
        /// 设置配送单明细属性值
        /// </summary>
        /// <param name="goodsIssue">配送单</param>
        /// <param name="detail">配送单明细</param>
        private void SetDistributionBillDetailProperty(GoodsIssue goodsIssue, DistributionBillDetail detail)
        {
            goodsIssue.PropertyValueList.ForEach(property =>
            {
                detail.PropertyList.Add(new DistributionBillDetailProperty()
                {
                    BillDetail = detail,
                    DefinitionId = property.DefinitionId,
                    Value = property.Value
                });
            });
        }

        /// <summary>
        /// 设置物料标签属性值
        /// </summary>
        /// <param name="goodsIssue">配送单</param>
        /// <param name="label">标签号</param>
        private void SetItemLabelProperty(GoodsIssue goodsIssue, string label)
        {
            var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(label);
            if (itemLabel != null)
            {
                goodsIssue.PropertyValueList.ForEach(property =>
                {
                    itemLabel.PropertyValueList.Add(new LabelPropertyValue()
                    {
                        ItemLabel = itemLabel,
                        DefinitionId = property.DefinitionId,
                        PropertyValue = property.Value,
                        PropertyName = property.DefinitionName
                    });
                });
                RF.Save(itemLabel);
            }
        }

        /// <summary>
        /// 获取工单发料属性值
        /// </summary>
        /// <param name="definitionId">物料属性Id</param>
        /// <param name="value">属性值</param>
        /// <param name="goodsIssueId">工单发料单ID</param>
        /// <returns>工单发料属性值</returns>
        public virtual GoodsIssuePropertyValue GetGoodIssuePropertyValue(double definitionId, string value, double goodsIssueId)
        {
            if (definitionId <= 0)
                throw new ArgumentNullException(nameof(definitionId));
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            return Query<GoodsIssuePropertyValue>().Where(p => p.DefinitionId == definitionId && p.Value == value && p.GoodsIssueId == goodsIssueId).FirstOrDefault();
        }

        /// <summary>
        /// 保存配送明细属性
        /// </summary>
        /// <param name="goodsIssue">工单发料信息</param>
        /// <param name="distribution">配送单信息</param>
        /// <param name="itemLabels">箱号列表</param>
        protected virtual void SavePropertyFromLabel(GoodsIssue goodsIssue, DistributionBill distribution, EntityList<ItemLabel> itemLabels)
        {
            if (itemLabels != null && itemLabels.Count > 0)
                CheckLabelProperty(itemLabels);
            else
                throw new ValidationException("箱号信息异常".L10N());
            EntityList<DistributionBillPropertyValue> distPropertyList = new EntityList<DistributionBillPropertyValue>();
            foreach (var labelPropertyValue in itemLabels.First().PropertyValueList)
            {
                DistributionBillPropertyValue distProperty = new DistributionBillPropertyValue()
                {
                    Value = labelPropertyValue.PropertyValue,
                    Definition = labelPropertyValue.Definition,
                    BillId = distribution.Id
                };
                distPropertyList.Add(distProperty);
            }

            if (distPropertyList.Count > 0)
                RF.Save(distPropertyList);
        }


        /// <summary>
        /// 根据发料记录保存属性
        /// </summary>
        /// <param name="goodsIssue">工单发料信息</param>
        /// <param name="distribution">配送单信息</param>
        protected virtual void SaveGoodsIssuePropertyValue(GoodsIssue goodsIssue, DistributionBill distribution)
        {
            if (goodsIssue.PropertyValueList.Count <= 0) return;
            EntityList<DistributionBillPropertyValue> distPropertyList = new EntityList<DistributionBillPropertyValue>();
            foreach (var goodsIssueProperty in goodsIssue.PropertyValueList)
            {
                DistributionBillPropertyValue distProperty = new DistributionBillPropertyValue()
                {
                    Value = goodsIssueProperty.Value,
                    Definition = goodsIssueProperty.Definition,
                    BillId = distribution.Id,
                    DefinitionId = goodsIssueProperty.DefinitionId,
                };
                distPropertyList.Add(distProperty);
            }

            if (distPropertyList.Count > 0)
                RF.Save(distPropertyList);
        }

        /// <summary>
        /// 验证配送数量及载具
        /// </summary>
        /// <param name="goodsIssue">发料单</param>
        /// <param name="container">周转箱</param>
        /// <param name="lineId">产线Id</param>
        /// <param name="qty">配送数量</param>
        /// <exception cref="ValidationException">产线不能为空</exception>
        /// <exception cref="ValidationException">配送数量必须大于0</exception>
        /// <exception cref="ValidationException">发料单库存不足配送数，请修改配送数量</exception>
        /// <exception cref="EntityNotFoundException">周转箱不可用，当前状态非闲置</exception>
        protected virtual void CheckDistribution(GoodsIssue goodsIssue, TurnoverBox container, double lineId, decimal qty)
        {
            if (goodsIssue == null)
                throw new ValidationException("发料单不能为空".L10N());
            if (container.State != BoxState.Unused)
                throw new ValidationException("周转箱[{0}]不可用，当前状态为[{1}]，非[{2}]".L10nFormat(container.Code, container.State, EnumViewModel.EnumToLabel(BoxState.Unused).L10N()));
            if (lineId <= 0)
                throw new ValidationException("产线不能为空".L10N());
            if (qty <= 0)
                throw new ValidationException("配送数量必须大于0".L10N());
            if (goodsIssue.RemainderQty < qty)
                throw new ValidationException("发料单{0}库存不足配送数，请修改配送数量".L10nFormat(goodsIssue.SendNo));
        }

        /// <summary>
        /// 验证箱号属性是否和第一个扫描的箱子属性保持一致
        /// </summary>
        /// <param name="labels">箱号列表</param>
        protected virtual void CheckLabelProperty(EntityList<ItemLabel> labels)
        {
            if (labels.Count == 1)
            {
                ItemLabel itemLabel = labels.First();
                foreach (var propertyValue in itemLabel.PropertyValueList)
                {
                    if (propertyValue.Definition == null)
                    {
                        throw new ValidationException("存在配送箱号，但箱号[{0}]配送物料属性[{1}]不存在".L10nFormat(itemLabel.Label, propertyValue.PropertyName));
                    }
                }
            }

            if (labels.Count > 1)
            {
                ItemLabel firstItemLabel = null;
                foreach (var itemLabel in labels)
                {
                    if (firstItemLabel == null)
                    {
                        firstItemLabel = itemLabel;
                        continue;
                    }

                    foreach (var propertyValue in itemLabel.PropertyValueList)
                    {
                        if (propertyValue.Definition == null || (!firstItemLabel.PropertyValueList.Any(p => p.DefinitionId == propertyValue.DefinitionId
                                    && p.PropertyValue.Trim() == propertyValue.PropertyValue.Trim())))
                        {
                            throw new ValidationException("存在多个配送箱号，但箱号[{0}]配送物料属性[{1}]不存在".L10nFormat(itemLabel.Label, propertyValue.PropertyName));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 根据载具条码得到工单发料配送中载具实体
        /// </summary>
        /// <param name="boxNo">配送周转箱号</param>
        /// <returns>配送单信息</returns>
        public virtual DistributionBill GetDistributionBill(string boxNo)
        {
            if (boxNo == null)
                throw new ArgumentNullException(nameof(boxNo));
            var box = GetTurnoverBox(boxNo);
            if (box.State != BoxState.Inuse)
                throw new ValidationException("配送周转箱非[{0}]状态,[{1}]当前状态为[{2}]".L10nFormat(BoxState.Inuse.ToLabel(), boxNo, box.State.ToLabel()));
            var query = Query<DistributionBill>();
            query.Where(p => p.ContainerNo == boxNo && p.State != DistributionState.LoadItem);
            return query.FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 配送周转箱是否存在
        /// </summary>
        /// <param name="code">周转箱编码</param>
        /// <returns>True:存在 False:不存在</returns>
        public virtual bool ExistsTurnoverBox(string code)
        {
            var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
            return RT.Service.Resolve<BoxController>().ExistsTurnoverBox(code, config.BoxType);
        }

        /// <summary>
        /// 获取配送周转箱
        /// </summary>
        /// <param name="boxNo">周转箱号</param>
        /// <returns>周转箱信息</returns>
        /// <exception cref="ValidationException">周转箱类型为配置</exception>
        /// <exception cref="ValidationException">周转箱不存在</exception>
        public virtual TurnoverBox GetTurnoverBox(string boxNo)
        {
            if (boxNo == null)
                throw new ArgumentNullException(nameof(boxNo));
            var config = ConfigService.GetConfig(new ProductTrunoverBoxTypeConfig());
            if (!config.BoxType.IsNotEmpty())
                throw new ValidationException("周转箱类型未配置，请配置".L10N());
            var box = RT.Service.Resolve<BoxController>().GetTurnoverBox(boxNo, config.BoxType);
            if (box == null)
                throw new ValidationException("周转箱不存在[{0}]".L10nFormat(boxNo));
            return box;
        }

        /// <summary>
        /// 根据发料单ID获取配送单信息
        /// </summary>
        /// <param name="goodsIssueId">发料单Id</param>
        /// <returns>配送单信息</returns>
        public virtual EntityList<DistributionBill> GetDistributionBillList(double goodsIssueId)
        {
            var q = Query<DistributionBill>();
            q.Where(p => p.SourceId == goodsIssueId);
            return q.OrderByDescending(p => p.BindingDate).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取配送单单号
        /// </summary>
        /// <param name="type">配置类型</param>
        /// <returns>配送单号</returns>
        public virtual string GetBillNo(Type type)
        {
            var config = ConfigService.GetConfig(new BillNoConfig(), type);
            if (config == null || config.NumberRule == null)
                throw new ValidationException("未找到配送单单号生成规则,请检查规则配置".L10N());
            return RT.Service.Resolve<NumberRuleController>()
                .GenerateSegment(config.NumberRuleId, 1)
                .FirstOrDefault();
        }

        /// <summary>
        /// 根据引用判断是否可以删除企业模型和设备模型
        /// </summary>
        /// <param name="id">来源Id</param>
        /// <param name="sourceType">来源类型</param>
        /// <returns>true,false</returns>
        public virtual bool IsHasUsedResourse(double id, SyncSourceType sourceType)
        {
            var res = RT.Service.Resolve<WipResourceController>().GetWipResource(id, sourceType);
            if (res == null) return true;
            return Query<DistributionBill>().Where(p => p.ResourceId == res.Id).FirstOrDefault() == null;
        }

        /// <summary>
        /// 判断配送单是否引用指定的生产资源
        /// </summary>
        /// <param name="wipResourceId">生产资源Id</param>
        /// <returns>bool: false--工单未引用生产资源；true--工单已引用生产资源</returns>
        public virtual bool DistributionBillHasUsedWipResource(double wipResourceId)
        {
            var distributionBill = Query<DistributionBill>().Where(x => x.ResourceId == wipResourceId && x.State != DistributionState.LoadItem).FirstOrDefault();
            if (distributionBill == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// 通过配送标签获取配送单
        /// </summary>
        /// <param name="label">配送标签</param>
        /// <returns>配送单</returns>
        public virtual DistributionBill GetDistributionBillByLabel(string label)
        {
            return Query<DistributionBill>()
                 .Join<DistributionBillDetail>((x, y) => x.Id == y.BillId && y.ItemLabelNo == label)
                 .Where(p => p.State != DistributionState.LoadItem).FirstOrDefault();
        }

        /// <summary>
        /// 获取配送单，bs需要做视图属性加载
        /// </summary>
        /// <param name="goodsIssueId">配送单ID</param>
        /// <returns>配送单</returns>
        public virtual GoodsIssue GetGoodsIssue(double goodsIssueId)
        {
            return RF.GetById<GoodsIssue>(goodsIssueId, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取配送管理（贪婪加载属性列表）
        /// </summary>
        /// <param name="goodsIssueId">配送管理Id</param>
        /// <returns>配送管理</returns>
        public virtual GoodsIssue GetGoodsIssueWithLoadData(double goodsIssueId)
        {
            return Query<GoodsIssue>().Where(p => p.Id == goodsIssueId).FirstOrDefault(new EagerLoadOptions().LoadWith(GoodsIssue.PropertyValueListProperty));
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="billId">配送单ID</param>
        /// <returns>属性值列表</returns>
        public virtual EntityList<DistributionBillPropertyValue> GetPropertyValueList(double billId)
        {
            return Query<DistributionBillPropertyValue>().Where(p => p.BillId == billId).ToList();
        }
    }
}
