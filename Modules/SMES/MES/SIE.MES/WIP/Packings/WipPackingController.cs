using SIE.Common.NumberRules;
using SIE.Common.Sort;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.Packings;
using SIE.Packages.Packings.Enums;
using SIE.Packages.Packings.Strategys;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.Packings
{
    /// <summary>
    /// 在制品包装控制器
    /// </summary>
    public partial class WipPackingController : WipController
    {
        /// <summary>
        /// 生产采集结束后
        /// </summary>
        /// <param name="wipProductProcess">生产采集</param>
        /// <param name="product">采集运行时产品模型, 记录产品在生产过程中的信息, 通过Puid产品全局ID关联生产信息</param>
        /// <param name="collectBarcodes">采集条码列表</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected override void OnWipProductProcessFinished(WipProductProcess wipProductProcess, product product,
            IList<CollectBarcode> collectBarcodes, CollectData collectData, Workcell workcell)
        {
            if (collectData == null)
            {
                throw new EntityNotFoundException(nameof(collectData));
            }

            //添加缺陷记录的方法移到基类WipController
            base.OnWipProductProcessFinished(wipProductProcess, product, collectBarcodes, collectData, workcell);

            if (collectData.PackingData.OuterPackingRelationId.HasValue)
            {
                JoinInSn(wipProductProcess, product, collectBarcodes, collectData, workcell);
            }
            else
            {
                CollectSn(wipProductProcess, product, collectBarcodes, collectData, workcell);
            }
        }

        #region 正常模式包装 
        /// <summary>
        /// 扫描SN，加入一个空的包装里
        /// </summary>
        /// <param name="wipProductProcess">生产采集记录</param>
        /// <param name="product">运行时产品模型</param>
        /// <param name="barcodes">条码集合</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>包装关系</returns>
        public virtual PackingRelation CollectSn(WipProductProcess wipProductProcess, product product,
            IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));
            if (workcell.EmployeeId <= 0)
                throw new ArgumentNullException(nameof(workcell.EmployeeId), "人员不能为空".L10N());
            if (workcell.ProcessId <= 0)
                throw new ArgumentNullException(nameof(workcell.ProcessId), "工序不能为空".L10N());
            if (workcell.StationId <= 0)
                throw new ArgumentNullException(nameof(workcell.StationId), "工位不能为空".L10N());
            if (barcodes == null)
                throw new ArgumentNullException(nameof(barcodes));
            if (!barcodes.Any())
                throw new ArgumentNullException(nameof(barcodes), "缺少条码".L10N());
            if (collectData?.PackingData == null)
                throw new ArgumentNullException(nameof(collectData));

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                WorkOrder workOrder = GetById<WorkOrder>(product.WorkOrderId);
                //验证工单包装规则
                ValidateWorkOrderPackageRule(workOrder);
                //获取主单位规则明细
                WorkOrderPackageRuleDetail masterUnitPackageRuleDetail = GetMasterUnitPackageRuleDetail(workOrder);
                //获取外包装规则明细
                WorkOrderPackageRuleDetail outerPackageRule = null;
                if (collectData.PackingData.DesignatedOuterPackingUnit == null)
                {
                    outerPackageRule = GetOuterPackageRuleDetail(masterUnitPackageRuleDetail, workOrder);
                }
                else
                {
                    outerPackageRule = GetPackageRuleDetail(collectData.PackingData.DesignatedOuterPackingUnit, workOrder);
                }
                //验证包装层级关系，工序是否有包装权限
                VaildationGreatethanInnerUnit(masterUnitPackageRuleDetail, outerPackageRule);
                VaildationPackingUnitHasProcess(workcell, outerPackageRule);
                ItemLabel itemLabel = GetItemLabel(barcodes.LastOrDefault().Code);
                itemLabel.Weight = collectData.PackingData.Weigh ?? 0;
                //验证物料标签
                ValidateItemLabel(itemLabel, masterUnitPackageRuleDetail.Qty);
                var emptyOuterPackage = RT.Service.Resolve<PackageController>().CreateEmptyPackage(workOrder.Id, outerPackageRule.PackageUnit, packingBy: workcell.EmployeeId, processId: workcell.ProcessId, stationId: workcell.StationId);
                //包装前事件
                OnCollectingSn(itemLabel, workOrder, masterUnitPackageRuleDetail, emptyOuterPackage, outerPackageRule, collectData, workcell);
                //包装
                RT.Service.Resolve<PackingRelationController>().AddItem(emptyOuterPackage, itemLabel);
                itemLabel = GetById<ItemLabel>(itemLabel.Id);
                emptyOuterPackage = GetById<PackingRelation>(emptyOuterPackage.Id);
                //包装后事件
                OnCollectedSn(itemLabel, workOrder, masterUnitPackageRuleDetail, emptyOuterPackage, outerPackageRule, collectData, workcell);
                trans.Complete();
                return emptyOuterPackage;
            }
        }

        /// <summary>
        /// 验证工单包装规则完整性
        /// </summary>
        /// <param name="workOrder">工单</param>
        void ValidateWorkOrderPackageRule(WorkOrder workOrder)
        {
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder));
            if (workOrder.PackageRuleDetailList.Count < 2)
                throw new ValidationException("工单[{0}]包装规则不完整".L10nFormat(workOrder.No));
        }

        /// <summary>
        /// 在指定工单里,验证加入的包装单位时，是否大于等于指定的外包装单位
        /// </summary>
        /// <param name="add">当前工单包装规则</param>
        /// <param name="outer">上一层级工单包装规则</param> 
        void VaildationGreatethanInnerUnit(WorkOrderPackageRuleDetail add, WorkOrderPackageRuleDetail outer)
        {
            if (SortExtension.GetIndex(add) >= SortExtension.GetIndex(outer))
                throw new ValidationException("不允许大包装放小包装".L10N());
        }

        /// <summary>
        /// 验证物料标签
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        /// <param name="qty">工单包装规则明细数量</param>
        private void ValidateItemLabel(ItemLabel itemLabel, decimal qty)
        {
            //验证物流状态
            if (itemLabel.Relation != null)
                throw new ValidationException("采集失败,条码:[{0}]已被[{1}{2}]包装。".L10nFormat(itemLabel.Label, itemLabel.Relation.PackageUnit.Name, itemLabel.Relation.PackageNo));
            if (itemLabel.Qty != qty)
                throw new ValidationException("主单位数量与标签数量不一致".L10nFormat(itemLabel.Label));
            if (itemLabel.Item.LowerWeight > itemLabel.Weight || itemLabel.Item.UpperWeight < itemLabel.Weight)
                throw new ValidationException("重量超出正常范围".L10N());
        }

        /// <summary>
        /// 包装后事件
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        /// <param name="workOrder">工单</param>
        /// <param name="innerPackageRule">内包装规则</param>
        /// <param name="outerPackage">包装关系</param>
        /// <param name="outerPackageRule">外包装规则</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        protected virtual void OnCollectedSn(ItemLabel itemLabel, WorkOrder workOrder, WorkOrderPackageRuleDetail innerPackageRule, PackingRelation outerPackage, WorkOrderPackageRuleDetail outerPackageRule, CollectData collectData, Workcell workcell)
        {
        }

        /// <summary>
        /// 包装前事件
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        /// <param name="workOrder">工单</param>
        /// <param name="innerPackageRule">内包装规则</param>
        /// <param name="outerPackage">包装关系</param>
        /// <param name="outerPackageRule">外包装规则</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元信息</param>
        protected virtual void OnCollectingSn(ItemLabel itemLabel, WorkOrder workOrder, WorkOrderPackageRuleDetail innerPackageRule, PackingRelation outerPackage, WorkOrderPackageRuleDetail outerPackageRule, CollectData collectData, Workcell workcell)
        {

        }
        #endregion

        #region 加入模式包装
        /// <summary>
        /// 扫描SN，加入到指定的外包装里
        /// </summary>
        /// <param name="wipProductProcess"></param>
        /// <param name="product"></param>
        /// <param name="barcodes"></param>
        /// <param name="collectData"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public virtual PackingRelation JoinInSn(WipProductProcess wipProductProcess, product product,
            IList<CollectBarcode> barcodes, CollectData collectData, Workcell workcell)
        {

            if (wipProductProcess == null)
            {
                throw new ArgumentNullException(nameof(wipProductProcess));
            }

            if (product == null)
            {
                throw new ArgumentNullException(nameof(product));
            }

            if (workcell == null)
            {
                throw new ArgumentNullException(nameof(workcell));
            }

            if (workcell.EmployeeId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(workcell.EmployeeId), "缺少人员".L10N());
            }

            if (workcell.ProcessId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(workcell.ProcessId), "工序".L10N());
            }

            if (workcell.StationId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(workcell.StationId), "缺少工位".L10N());
            }

            if (barcodes == null)
            {
                throw new ArgumentNullException(nameof(barcodes));
            }

            if (!barcodes.Any())
            {
                throw new ArgumentOutOfRangeException(nameof(barcodes), "缺少条码".L10N());
            }

            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var ruleValidMode = collectData.Context.GetValue(typeof(PackingRuleValidMode).Name, PackingRuleValidMode.Current);
                var packingController = RT.Service.Resolve<PackingController>();
                WorkOrder productWorkOrder = RF.GetById<WorkOrder>(product.WorkOrderId);
                //获取主单位
                WorkOrderPackageRuleDetail masterUnitPackageRuleDetail = GetMasterUnitPackageRuleDetail(productWorkOrder);
                //获取待加入的包装关系
                PackingRelation outerPackage = RT.Service.Resolve<PackingRelationController>().GetPackingRelation(collectData.PackingData.OuterPackingRelationId.Value);
                packingController.VaildationNotDoPacking(outerPackage);
                WorkOrder outerWorkOrder = RF.GetById<WorkOrder>(outerPackage.WorkOrderId); //外包装工单
                WorkOrderPackageRuleDetail outerPackageRule = GetPackageRuleDetail(outerPackage.PackageUnit, outerWorkOrder);
                VaildationPackingUnitHasProcess(workcell, outerPackageRule);
                VaildationSameProduct(outerWorkOrder, productWorkOrder);

                ItemLabel itemLabel = GetItemLabel(barcodes.LastOrDefault().Code);
                packingController.VaildationNotDoPacking(itemLabel);
                if (collectData.PackingData.DesignatedOuterPackingUnit == null)
                    VaildationJoinUnit(outerPackage, masterUnitPackageRuleDetail.PackageUnit, outerWorkOrder);
                VaildationRuleQtyIsSame(outerPackage, masterUnitPackageRuleDetail.PackageUnit, outerWorkOrder, productWorkOrder, ruleValidMode);
                VaildationJoinItemQty(itemLabel.Qty, outerPackage, outerWorkOrder);
                VaildationItemLabelQty(masterUnitPackageRuleDetail, itemLabel);
                itemLabel.Weight = collectData.PackingData.Weigh ?? 0;
                VaildationWeight(itemLabel);
                OnJoiningSn(itemLabel, productWorkOrder, masterUnitPackageRuleDetail, outerPackage, outerWorkOrder, outerPackageRule, collectData, workcell);

                RT.Service.Resolve<PackingRelationController>().AddItem(outerPackage, itemLabel);

                itemLabel = GetById<ItemLabel>(itemLabel.Id);
                outerPackage = GetById<PackingRelation>(outerPackage.Id);
                OnJoinedSn(itemLabel, productWorkOrder, masterUnitPackageRuleDetail, outerPackage, outerWorkOrder, outerPackageRule, collectData, workcell);
                trans.Complete();
                return outerPackage;
            }
        }

        /// <summary>
        /// 验证物料标签数量
        /// </summary>
        /// <param name="masterUnitPackageRuleDetail">工单包装规则</param>
        /// <param name="itemLabel">物料标签</param>
        protected virtual void VaildationItemLabelQty(WorkOrderPackageRuleDetail masterUnitPackageRuleDetail, ItemLabel itemLabel)
        {
            if (masterUnitPackageRuleDetail == null) throw new ArgumentNullException(nameof(masterUnitPackageRuleDetail));

            if (itemLabel == null) throw new ArgumentNullException(nameof(itemLabel));

            if (itemLabel.Qty != masterUnitPackageRuleDetail.Qty)
                throw new ValidationException("主单位数量与标签数量不一致".L10nFormat(itemLabel.Label));
        }

        /// <summary>
        /// 校验重量在标准范围内
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        void VaildationWeight(ItemLabel itemLabel)
        {
            if (itemLabel == null)
                throw new ArgumentNullException(nameof(itemLabel));
            if (itemLabel.Item.LowerWeight > itemLabel.Weight || itemLabel.Item.UpperWeight < itemLabel.Weight)
                throw new ValidationException("重量超出正常范围".L10N());
        }

        /// <summary>
        /// 加入物料标签后
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        /// <param name="itemLabelWorkOrder">工单</param>
        /// <param name="mastUnitPackageRule">工单包装规则</param>
        /// <param name="outerPackage">外部包装</param>
        /// <param name="outerWorkOrder">外部工单</param>
        /// <param name="outerPackageRule">外部包装规则</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void OnJoinedSn(ItemLabel itemLabel, WorkOrder itemLabelWorkOrder, WorkOrderPackageRuleDetail mastUnitPackageRule, PackingRelation outerPackage, WorkOrder outerWorkOrder, WorkOrderPackageRuleDetail outerPackageRule, CollectData collectData, Workcell workcell)
        {
        }

        /// <summary>
        /// 加入物料标签时
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        /// <param name="itemLabelWorkOrder">工单</param>
        /// <param name="mastUnitPackageRule">工单包装规则</param>
        /// <param name="outerPackage">外部包装</param>
        /// <param name="outerWorkOrder">外部工单</param>
        /// <param name="outerPackageRule">外部包装规则</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void OnJoiningSn(ItemLabel itemLabel, WorkOrder itemLabelWorkOrder, WorkOrderPackageRuleDetail mastUnitPackageRule, PackingRelation outerPackage, WorkOrder outerWorkOrder, WorkOrderPackageRuleDetail outerPackageRule, CollectData collectData, Workcell workcell)
        {
            ////TODO:后续逻辑扩展
        }
        #endregion

        #region 加入包装
        /// <summary>
        /// 扫描包装，加入到指定的外包装里
        /// </summary>
        /// <param name="innerPackageNo">内包装</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <remarks>
        /// <para>包含3个逻辑扩展点对采集过程的控制：</para>
        /// <para> * OnJoiningPackage 方法，验证已采集包装是否已经被包装</para>
        /// <para> * OnJoinedPackage 方法</para>
        /// <para> * GetWorkOrder方法 包装对应工单的查找逻辑</para>
        /// <para>支持:</para>
        /// <para>1.扫描外包装(空),加入一个包装(不空)</para>
        /// <para>2.扫描外包装(不空),加入一个包装(不空)</para>
        /// </remarks>
        /// <exception cref="ValidationException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>外包装关系</returns>
        public virtual PackingRelation JoinInPackage(string innerPackageNo, CollectData collectData, Workcell workcell)
        {
            /* 主干逻辑如下:包含3个逻辑扩展点
             * 获取内包装关系 和 获取外包装关系
             * 获取内包装工单 和 获取外包装工单
             * 获取内包装规则 和 获取外包装规则、外包装相应的内包装规
             * 验证是否相同产品
             * 验证包装单位
             * 验证包装规则数量是否一致
             * 验证外包装物料数量
             * 验证外包装包装数量
             * （可重写的扩展点） 执行OnJoiningPackage【验证已采集包装是否已经被包装】
             * 把内包装加入到外包装里
             * 获取最新外包装
             * （可重写的扩展点）执行OnJoinedPackage
             * 完成采集
             */
            if (innerPackageNo.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(innerPackageNo));
            if (workcell == null) throw new ArgumentNullException(nameof(workcell));
            if (workcell.EmployeeId <= 0) throw new ArgumentOutOfRangeException(nameof(workcell.EmployeeId), "缺少人员".L10N());
            if (workcell.ProcessId <= 0) throw new ArgumentOutOfRangeException(nameof(workcell.ProcessId), "工序".L10N());
            if (workcell.StationId <= 0) throw new ArgumentOutOfRangeException(nameof(workcell.StationId), "缺少工位".L10N());
            VaildationCollectData(collectData);
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var ruleValidMode = collectData.Context.GetValue(typeof(PackingRuleValidMode).Name, PackingRuleValidMode.Current);
                var packingController = RT.Service.Resolve<PackingController>();
                var relationContoller = RT.Service.Resolve<PackingRelationController>();
                PackingRelation innerPackage = relationContoller.GetBatchPackingRelation(innerPackageNo);
                //ValidationLogisticState(innerPackage);
                PackingRelation outerPackage = relationContoller.GetPackingRelation(collectData.PackingData.OuterPackingRelationId.Value);
                //ValidationLogisticState(outerPackage);
                packingController.VaildationNotDoPacking(outerPackage);
                packingController.VaildationNotInPackage(innerPackage);
                var innerWorkOrder = GetWorkOrder(innerPackage);
                var outerWorkOrder = GetWorkOrder(outerPackage, false) ?? innerWorkOrder;
                var innerPackageRule = GetPackageRuleDetail(innerPackage.PackageUnit, innerWorkOrder);
                var outerPackageRule = GetPackageRuleDetail(outerPackage.PackageUnit, outerWorkOrder);
                VaildationPackingUnitHasProcess(workcell, outerPackageRule);
                VaildationSameProduct(outerWorkOrder, innerWorkOrder);
                if (collectData.PackingData.DesignatedOuterPackingUnit == null)
                {
                    VaildationJoinUnit(outerPackage, innerPackageRule.PackageUnit, outerWorkOrder);
                    VaildationJoinPackageQty(outerPackage, outerWorkOrder);
                }

                VaildationRuleQtyIsSame(outerPackage, innerPackageRule.PackageUnit, outerWorkOrder, innerWorkOrder, ruleValidMode);

                VaildationGreatethanInnerUnit(innerPackageRule.PackageUnit, outerPackageRule.PackageUnit, outerWorkOrder);
                VaildationJoinItemQty(innerPackage.ItemQty, outerPackage, outerWorkOrder);
                OnJoiningPackage(innerPackage, innerWorkOrder, innerPackageRule, outerPackage, outerWorkOrder, outerPackageRule, collectData, workcell);
                relationContoller.AddPackage(outerPackage, innerPackage);
                innerPackage = GetById<PackingRelation>(innerPackage.Id);
                outerPackage = GetById<PackingRelation>(outerPackage.Id);
                OnJoinedPackage(innerPackage, innerWorkOrder, innerPackageRule, outerPackage, outerWorkOrder, outerPackageRule, collectData, workcell);
                trans.Complete();
                return outerPackage;
            }
        }

        /// <summary>
        /// 加入包装后
        /// </summary>
        /// <param name="innerPackage">内部包装</param>
        /// <param name="innerWorkOrder">内部工单</param>
        /// <param name="innerPackageRule">内部包装规则</param>
        /// <param name="outerPackage">外部包装</param>
        /// <param name="outerWorkOrder">外部工单</param>
        /// <param name="outerPackageRule">外部包装规则</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void OnJoinedPackage(PackingRelation innerPackage, WorkOrder innerWorkOrder, WorkOrderPackageRuleDetail innerPackageRule, PackingRelation outerPackage, WorkOrder outerWorkOrder, WorkOrderPackageRuleDetail outerPackageRule, CollectData collectData, Workcell workcell)
        {
        }

        /// <summary>
        /// 加入包装前
        /// </summary> 
        /// <param name="innerPackage">内部包装</param>
        /// <param name="innerWorkOrder">内部工单</param>
        /// <param name="innerPackageRule">内部包装规则</param>
        /// <param name="outerPackage">外部包装</param>
        /// <param name="outerWorkOrder">外部工单</param>
        /// <param name="outerPackageRule">外部包装规则</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void OnJoiningPackage(PackingRelation innerPackage, WorkOrder innerWorkOrder, WorkOrderPackageRuleDetail innerPackageRule, PackingRelation outerPackage, WorkOrder outerWorkOrder, WorkOrderPackageRuleDetail outerPackageRule, CollectData collectData, Workcell workcell)
        {
            MovePackageInSn(innerPackage, workcell);
        }

        /// <summary>
        /// 验证包装数量
        /// </summary>
        /// <param name="outerPackage">外包装关系</param>
        /// <param name="workOrder">工单</param>
        /// <exception cref="ValidationException"></exception>
        protected virtual void VaildationJoinPackageQty(PackingRelation outerPackage, WorkOrder workOrder)
        {
            if (outerPackage == null) throw new ArgumentNullException(nameof(outerPackage));
            if (workOrder == null) throw new ArgumentNullException(nameof(workOrder));
            GetInnerPackageRuleDetail(outerPackage.PackageUnit, workOrder);
            GetPackageRuleDetail(outerPackage.PackageUnit, workOrder);
        }

        /// <summary>
        /// 验证加入包装单位是否符合包装规则
        /// </summary>
        /// <param name="outerPackage">外包装关系</param>
        /// <param name="addPackingUnit">加入的包装单位</param>
        /// <param name="outerWorkOrder">工单</param>
        protected virtual void VaildationJoinUnit(PackingRelation outerPackage, PackingUnit addPackingUnit, WorkOrder outerWorkOrder)
        {
            if (outerPackage == null) throw new ArgumentNullException(nameof(outerPackage));
            if (addPackingUnit == null) throw new ArgumentNullException(nameof(addPackingUnit));
            if (outerWorkOrder == null) throw new ArgumentNullException(nameof(outerWorkOrder));
            WorkOrderPackageRuleDetail outerChildrenPackageRule = GetInnerPackageRuleDetail(outerPackage.PackageUnit, outerWorkOrder);
            if (addPackingUnit.Id != outerChildrenPackageRule.PackageUnitId)
                throw new ValidationException("包装[{0}{1}]只能放[{2}]".L10nFormat(outerPackage.PackageUnit.Name, outerPackage.PackageNo, outerChildrenPackageRule.PackageUnit.Name));
        }

        /// <summary>
        /// 验证采集数据
        /// </summary>
        /// <param name="collectData">采集数据</param>
        protected virtual void VaildationCollectData(CollectData collectData)
        {
            if (collectData?.PackingData?.OuterPackingRelationId == null)
                throw new ArgumentException(nameof(collectData), "包装采集数据不完整".L10N());
        }
        #endregion

        #region 单包装关系打包
        /// <summary>
        /// 单层级打包
        /// </summary>
        /// <param name="outerPackingRelation">外部包装关系</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="isCasecade">是否级联打包</param>
        /// <param name="isAtuoCreateBill">是否自动创建单据</param>
        /// <param name="listPackrelation">已打包的包装关系集合</param>
        /// <returns>打包后的最顶包装关系</returns>
        public virtual EntityList<PackingRelation> DoPacking(PackingRelation outerPackingRelation, Workcell workcell, bool isCasecade, bool isAtuoCreateBill = true, EntityList<PackingRelation> listPackrelation = null)
        {
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (outerPackingRelation == null)
                {
                    throw new ArgumentNullException(nameof(outerPackingRelation));
                }
                if (workcell == null)
                {
                    throw new ArgumentOutOfRangeException(nameof(workcell));
                }
                var workOrder = RF.GetById<WorkOrder>(outerPackingRelation.WorkOrderId);
                var ruleDetail = GetPackageRuleDetail(outerPackingRelation.PackageUnit, workOrder);
                if (!ruleDetail.NumberRuleId.HasValue)
                {
                    throw new ValidationException("工单[{0}],未配置[{1}]条码规则".L10nFormat(workOrder.No, ruleDetail.PackageUnit.Name));
                }

                var packingRelation = RT.Service.Resolve<PackingRelationController>().DoPacking(outerPackingRelation.Id, ruleDetail.NumberRuleId.Value, workcell.EmployeeId);
                Flag_IsProcessFinishIfProcessFinish(packingRelation, GetById<Process>(workcell.ProcessId), workOrder);
                if (listPackrelation == null)
                {
                    listPackrelation = new EntityList<PackingRelation>();
                }
                listPackrelation.Add(packingRelation);
                packingRelation = CasecadeDoPacking(packingRelation, workcell, workOrder, ruleDetail, isCasecade, listPackrelation) ?? packingRelation;

                trans.Complete();
                return listPackrelation;
            }
        }

        /// <summary>
        /// 级联打包
        /// </summary>
        /// <param name="outerPackingRelation">外部包装关系</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="workOrder">工单</param>
        /// <param name="ruleDetail">规则明细</param>
        /// <param name="isCasecade">是否级联打包</param>
        /// <param name="listPack">打包后的包装关系</param>
        /// <returns>包装关系</returns>
        private PackingRelation CasecadeDoPacking(PackingRelation outerPackingRelation, Workcell workcell, WorkOrder workOrder, WorkOrderPackageRuleDetail ruleDetail, bool isCasecade, EntityList<PackingRelation> listPack = null)
        {
            var process = GetById<Process>(outerPackingRelation.ProcessId);
            if (process == null)
                throw new EntityNotFoundException(typeof(Process), outerPackingRelation.ProcessId);
            if (!isCasecade || IsProcessTopPackageUnit(outerPackingRelation.PackageUnit, process, workOrder))
                return null;

            PackingRelation parentPackingRelation = AutoCollectPackge(outerPackingRelation, workcell, workOrder, ruleDetail);
            if (parentPackingRelation == null) return parentPackingRelation;
            var list = DoPacking(parentPackingRelation, workcell, isCasecade, true, listPack);
            return list[list.Count - 1];
        }

        /// <summary>
        /// 自动采集包装
        /// </summary>
        /// <param name="outerPackingRelation">外部包装关系</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="workOrder">工单</param>
        /// <param name="ruleDetail">规则明细</param>
        /// <returns>包装关系</returns>
        private PackingRelation AutoCollectPackge(PackingRelation outerPackingRelation, Workcell workcell, WorkOrder workOrder, WorkOrderPackageRuleDetail ruleDetail)
        {
            var packingRelationList = GetPackagesWithNotGreaterThanMaxRule(outerPackingRelation, workOrder, ruleDetail);
            PackingRelation parentPackingRelation = null;
            for (int i = 0; i < packingRelationList.Count; i++)
            {
                if (parentPackingRelation == null)
                {
                    parentPackingRelation = CollectPackageNo(packingRelationList[i].PackageNo, CollectData.Empty, workcell);
                }
                else
                {
                    var collectData = new CollectData();
                    collectData.PackingData.OuterPackingRelationId = parentPackingRelation.Id;
                    parentPackingRelation = JoinInPackage(packingRelationList[i].PackageNo, collectData, workcell);
                }
            }

            return parentPackingRelation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outerPackingRelation">外部包装关系</param>
        /// <param name="workOrder">工单</param>
        /// <param name="ruleDetail">规则明细</param>
        /// <returns>包装关系列表</returns>
        private IList<PackingRelation> GetPackagesWithNotGreaterThanMaxRule(PackingRelation outerPackingRelation, WorkOrder workOrder, WorkOrderPackageRuleDetail ruleDetail)
        {
            List<double> packingRelationList = new List<double>();
            using (var dba = DbAccesserFactory.Create(PackageEntityDataProvider.ConnectionStringName))
            {
                string sql = GenerateQuerySqlForCasecadPackage(outerPackingRelation, workOrder);
                using (System.Data.IDataReader dr = dba.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        packingRelationList.Add(dr.GetDecimal(0).ConvertTo<double>());
                    }
                }
            }
            List<double> casecadeRelation = new List<double>();
            var parentRule = GetOuterPackageRuleDetail(ruleDetail.PackageUnit, workOrder);
            var maxPackageQty = (parentRule.Qty / ruleDetail.Qty);
            if (packingRelationList.Count >= maxPackageQty)
            {
                casecadeRelation.AddRange(packingRelationList.Take((int)maxPackageQty));
            }

            if (packingRelationList.Count < maxPackageQty) return new List<PackingRelation>();

            var rs = Query<PackingRelation>().Where(f => casecadeRelation.ToList().Contains(f.Id)).ToList();
            return rs;
        }

        /// <summary>
        /// 为级联打包生成sql
        /// </summary>
        /// <param name="outerPackingRelation">外部包装关系</param>
        /// <param name="workOrder">工单</param>
        /// <returns>sql</returns>
        private string GenerateQuerySqlForCasecadPackage(PackingRelation outerPackingRelation, WorkOrder workOrder)
        {
            var pkgMeta = RF.Find<PackingRelation>().EntityMeta;
            var labelMeta = RF.Find<ItemLabel>().EntityMeta;
            var sql = @"SELECT T0.ID
                              FROM [PKG_RELATION] T0
                             WHERE EXISTS(SELECT 1
                                      FROM [ITEM_LABEL] T1,[PKG_RELATION] Y1
                                     WHERE Y1.[ROOT_ID] = T0.ID
                                       AND T1.[ITEM_ID] = {0}
                                       AND T1.[RELATION_ID] = Y1.ID
                                       AND T1.INV_ORG_ID = {3}
                                       AND Y1.INV_ORG_ID = {3}
                                       AND Y1.IS_PHANTOM = 0
                                       AND T1.IS_PHANTOM = 0)
                               AND T0.TREE_PID IS NULL
                               AND T0.[PACKAGE_NO] IS NOT NULL
                               AND T0.[PACKAGE_UNIT_ID] = {4}
                               AND T0.INV_ORG_ID = {3}
                               AND T0.IS_PHANTOM = 0
                               AND T0.[STATION_ID] = {1}
                               AND T0.[PROCESS_ID] = {2}
                               AND T0.[WORKORDER_ID] = {5}
                             ORDER BY T0.ID ASC"
            .Replace("[PKG_RELATION]", pkgMeta.TableMeta.TableName)
            .Replace("[ITEM_LABEL]", labelMeta.TableMeta.TableName)
            .Replace("[ROOT_ID]", pkgMeta.Property(PackingRelation.RootIdProperty).ColumnMeta.ColumnName)
            .Replace("[PACKAGE_NO]", pkgMeta.Property(PackingRelation.PackageNoProperty).ColumnMeta.ColumnName)
            .Replace("[PACKAGE_UNIT_ID]", pkgMeta.Property(PackingRelation.PackageUnitIdProperty).ColumnMeta.ColumnName)
            .Replace("[STATION_ID]", pkgMeta.Property(PackingRelation.StationIdProperty).ColumnMeta.ColumnName)
            .Replace("[PROCESS_ID]", pkgMeta.Property(PackingRelation.ProcessIdProperty).ColumnMeta.ColumnName)
            .Replace("[ITEM_ID]", labelMeta.Property(ItemLabel.ItemIdProperty).ColumnMeta.ColumnName)
            .Replace("[RELATION_ID]", labelMeta.Property(ItemLabel.RelationIdProperty).ColumnMeta.ColumnName)
            .Replace("[WORKORDER_ID]", pkgMeta.Property(PackingRelation.WorkOrderIdProperty).ColumnMeta.ColumnName);
            return string.Format(sql, workOrder.ProductId, outerPackingRelation.StationId, outerPackingRelation.ProcessId, AppRuntime.InvOrg, outerPackingRelation.PackageUnitId, workOrder.Id);
        }

        /// <summary>
        /// 判断工序包装是否达到最顶层级，达到最顶层级后将包装关系的是否工序完工设置为true
        /// </summary>
        /// <param name="outerPackage">外部包装</param>
        /// <param name="process">工序</param>
        /// <param name="workOrder">工单</param>
        private void Flag_IsProcessFinishIfProcessFinish(PackingRelation outerPackage, Process process, WorkOrder workOrder)
        {
            if (!IsProcessTopPackageUnit(outerPackage.PackageUnit, process, workOrder))
                return;
            if (outerPackage.PackageNo.IsNullOrEmpty()) return;
            if (DB.Update<PackingRelation>()
                     .Set(f => f.IsProcessFinish, true)
                     .Set(f => f.UpdateBy, RT.IdentityId)
                     .Set(f => f.UpdateDate, DateTime.Now)
                     .Where(f => f.Id == outerPackage.RootId).Execute() == 0)
                throw new ValidationException("级联打包失败,无法更新工序完工标志".L10N());
            outerPackage = GetById<PackingRelation>(outerPackage.Id);
        }

        /// <summary>
        /// 验证一个包装层级单位在一个工单里是否是工序权限配置里的最顶一层包装层级
        /// </summary>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="process">工序</param>
        /// <param name="workOrder">工单</param>
        /// <returns>bool</returns>
        [IgnoreProxy]
        public virtual bool IsProcessTopPackageUnit(PackingUnit packingUnit, Process process, WorkOrder workOrder)
        {
            if (packingUnit == null)
                throw new ArgumentNullException(nameof(packingUnit));
            if (process == null)
                throw new ArgumentNullException(nameof(process));
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder));
            var rules = workOrder.PackageRuleDetailList;
            var processInPackingUnits = rules.Where(f => !f.PackageUnit.IsMasterUnit).SelectMany(f => f.WorkOrderProcessPackingUnitList)
                .Where(f => f.ProcessId == process.Id).OrderBy(f => SortExtension.GetIndex(f.PackageRule)).ToArray();
            ////工单没有规则没有配置工序(不做限制),再验证最顶规则是不是一致
            if (!processInPackingUnits.Any() && GetTopOuterPackageRuleDetail(workOrder).PackageUnitId == packingUnit.Id)
                return true;

            ////只配置了一个单位的，肯定就是顶规则
            if (processInPackingUnits.Length == 1)
                return true;

            var unitHasProcess = processInPackingUnits.FirstOrDefault(f => f.PackageRule.PackageUnitId == packingUnit.Id);
            if (processInPackingUnits.Any() && unitHasProcess == null) throw new ValidationException("工单[{0}],工序[{1}]未找到配置包装[{2}]权限".L10nFormat(workOrder.No, process.Name, packingUnit.Name));

            ////判断上一级包装规则，如果没有是工单包装规则最顶，也属于工序包装权限中的最顶规则
            ////或者 有父包装规则，但是能匹配工序所关联的包装单位权限里最顶一个单位，那么它就是一个工序里的顶部规则
            ////因此，跨包装层级配置工序在这里不属于工序顶层包装
            var parentRule = GetOuterPackageRuleDetail(packingUnit, workOrder, false);
            if (parentRule == null || (!processInPackingUnits.Any(f => f.PackageRule.PackageUnitId == parentRule.PackageUnitId) && processInPackingUnits.Any()))
                return true;

            return false;
        }

        /// <summary>
        /// 扫描包装，加入一个空的包装里
        /// </summary>
        /// <param name="innerPackageNo">内包装</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        /// <remarks>
        /// <para>包含3个逻辑扩展点对采集过程的控制：</para>
        /// <para> * OnCollectingPackage 方法，验证已采集包装是否已经被包装</para>
        /// <para> * OnCollectedPackage 方法</para>
        /// <para> * GetWorkOrder方法 包装对应工单的查找逻辑</para>
        /// </remarks>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <exception cref="ArgumentException">无效参数异常</exception>
        /// <returns>外包装</returns>
        public virtual PackingRelation CollectPackageNo(string innerPackageNo, CollectData collectData, Workcell workcell)
        {
            /* 主干逻辑如下:包含3个逻辑扩展点
             * 获取内包装关系
             * （可重写的扩展点）获取工单
             * 获取采集的包装对应的包装规则
             * 获取外包装对应的包装规则
             * 获取最外层对应的包装规则
             * 创建一个符合采集包装单位相应的空外包装
             * （可重写的扩展点） 执行OnCollectingPackage【验证已采集包装是否已经被包装】
             * 把采集的包装加入到空的包装里面
             * 获取最新外包装
             * （可重写的扩展点）执行OnCollectedPackage
             * 完成采集
             */
            if (innerPackageNo.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(innerPackageNo));
            if (workcell == null)
                throw new ArgumentNullException(nameof(workcell));
            if (workcell.EmployeeId <= 0)
                throw new ArgumentOutOfRangeException(nameof(workcell.EmployeeId), "缺少人员".L10N());
            if (workcell.ProcessId <= 0)
                throw new ArgumentOutOfRangeException(nameof(workcell.ProcessId), "工序".L10N());
            if (workcell.StationId <= 0)
                throw new ArgumentOutOfRangeException(nameof(workcell.StationId), "缺少工位".L10N());
            if (collectData?.PackingData == null)
                throw new ArgumentNullException(nameof(collectData));
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                var packingController = RT.Service.Resolve<PackingController>();
                var relationContoller = RT.Service.Resolve<PackingRelationController>();
                PackingRelation innerPackage = relationContoller.GetBatchPackingRelation(innerPackageNo);

                //ValidationLogisticState(innerPackage);
                packingController.VaildationNotInPackage(innerPackage);
                var workOrder = GetWorkOrder(innerPackage);
                var innerPackageRule = GetPackageRuleDetail(innerPackage.PackageUnit, workOrder);
                WorkOrderPackageRuleDetail outerPackageRule = null;
                if (collectData.PackingData.DesignatedOuterPackingUnit != null)
                {
                    outerPackageRule = GetPackageRuleDetail(collectData.PackingData.DesignatedOuterPackingUnit, workOrder);
                    VaildationGreatethanInnerUnit(innerPackageRule.PackageUnit, collectData.PackingData.DesignatedOuterPackingUnit, workOrder);
                }
                else
                    outerPackageRule = GetOuterPackageRuleDetail(innerPackage.PackageUnit, workOrder);

                VaildationPackingUnitHasProcess(workcell, outerPackageRule);
                var topPackageRule = GetTopOuterPackageRuleDetail(workOrder);
                var emptyOuterPackage = RT.Service.Resolve<PackageController>().CreateEmptyPackage(workOrder.Id, outerPackageRule.PackageUnit, packingBy: workcell.EmployeeId, processId: workcell.ProcessId, stationId: workcell.StationId);
                OnCollectingPackage(innerPackage, innerPackageRule, emptyOuterPackage, outerPackageRule, topPackageRule, workOrder, collectData, workcell);
                relationContoller.AddPackage(emptyOuterPackage, innerPackage);
                emptyOuterPackage = GetById<PackingRelation>(emptyOuterPackage.Id);
                innerPackage = GetById<PackingRelation>(innerPackage.Id);
                OnCollectedPackage(innerPackage, innerPackageRule, emptyOuterPackage, outerPackageRule, topPackageRule, workOrder, collectData, workcell);
                trans.Complete();
                return emptyOuterPackage;
            }
        }

        /// <summary>
        /// 采集包装
        /// </summary>
        /// <param name="innerPackage">内部包装</param>
        /// <param name="innerPackageRule">内部包装规则</param>
        /// <param name="outerPackage">外部包装</param>
        /// <param name="outerPackageRule">外部包装规则</param>
        /// <param name="topPackageRule">顶层包装规则</param>
        /// <param name="workOrder">工单</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void OnCollectedPackage(PackingRelation innerPackage, WorkOrderPackageRuleDetail innerPackageRule, PackingRelation outerPackage, WorkOrderPackageRuleDetail outerPackageRule, WorkOrderPackageRuleDetail topPackageRule, WorkOrder workOrder, CollectData collectData, Workcell workcell)
        {
        }

        /// <summary>
        /// 加入包装
        /// </summary>
        /// <param name="innerPackage">内部包装</param>
        /// <param name="innerPackageRule">内部包装规则</param>
        /// <param name="outerPackage">外部包装</param>
        /// <param name="outerPackageRule">外部包装规则</param>
        /// <param name="topPackageRule">顶层包装规则</param>
        /// <param name="workOrder">工单</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void OnCollectingPackage(PackingRelation innerPackage, WorkOrderPackageRuleDetail innerPackageRule
            , PackingRelation outerPackage, WorkOrderPackageRuleDetail outerPackageRule
            , WorkOrderPackageRuleDetail topPackageRule
            , WorkOrder workOrder
             , CollectData collectData, Workcell workcell)
        {
            MovePackageInSn(innerPackage, workcell);
        }

        /// <summary>
        /// 包装过站
        /// </summary>
        /// <param name="movePackage">包装关系</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void MovePackageInSn(PackingRelation movePackage, Workcell workcell)
        {
            if (movePackage == null) throw new ArgumentNullException(nameof(movePackage));

            if (workcell == null) throw new ArgumentNullException(nameof(workcell));

            if (movePackage.ProcessId == workcell.ProcessId) return;

            var barcodes = GetRootPackingRelationItemLabel(movePackage);
            if (!barcodes.Any()) throw new ValidationException("找不到需要过站的条码".L10N());

            foreach (var barcode in barcodes)
            {
                RT.Service.Resolve<WipController>().Collect(new string[] { barcode }, CollectData.Empty, workcell);
            }
        }

        /// <summary>
        /// 根据指定的包装获取根包装所有产品条码
        /// </summary>
        /// <param name="innerPackage">内部包装</param>
        /// <returns>产品条码列表</returns>
        protected virtual IList<string> GetRootPackingRelationItemLabel(PackingRelation innerPackage)
        {
            if (innerPackage == null) throw new ArgumentNullException(nameof(innerPackage));
            return RT.Service.Resolve<ItemLabelController>().GetRootPackingRelationSN(innerPackage.RootId);
        }
        #endregion

        #region 多包装关系打包
        /// <summary>
        /// 多层级打包
        /// </summary>
        /// <param name="relations">包装关系列表</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="isCasecade">是否级联打包</param>
        /// <param name="isAtuoCreateBill">是否自动创建单据</param>
        /// <param name="listPackrelation">打包后的包装关系</param>
        /// <returns>上一包装层级</returns>
        public virtual EntityList<PackingRelation> DoMultLevelPacking(EntityList<PackingRelation> relations, Workcell workcell, bool isCasecade, bool isAtuoCreateBill = true, EntityList<PackingRelation> listPackrelation = null)
        {
            using (var trans = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
            {
                if (workcell == null)
                    throw new ArgumentOutOfRangeException(nameof(workcell));
                if (relations.Count <= 0)
                    throw new ValidationException("打包失败，选择打包包装数量必须大于0".L10N());
                var relation = relations.FirstOrDefault();
                var workOrder = RF.GetById<WorkOrder>(relation.WorkOrderId);
                //根据当前包装层级，获取上一层级包装规则
                var ruleDetail = GetOuterPackageRuleDetail(GetPackageRuleDetail(relation.PackageUnit, workOrder), workOrder);
                if (!ruleDetail.NumberRuleId.HasValue)
                    throw new ValidationException("工单[{0}],未配置[{1}]条码规则".L10nFormat(workOrder.No, ruleDetail.PackageUnit.Name));
                var process = GetById<Process>(workcell.ProcessId);
                ValidatePackingRelations(relations, ruleDetail);
                ValidateHasPackingUnit(ruleDetail.PackageUnit, process, workOrder);
                var packageNo = RT.Service.Resolve<NumberRuleController>().GenerateSegment(ruleDetail.NumberRuleId.Value, 1).FirstOrDefault();
                var emptyOuterPackage = RT.Service.Resolve<PackageController>().CreateEmptyPackage(workOrder.Id, ruleDetail.PackageUnitId, packageNo, packingBy: workcell.EmployeeId, processId: workcell.ProcessId, stationId: workcell.StationId);
                for (int i = 0; i < relations.Count; i++)
                {
                    if (i > 0)
                        emptyOuterPackage = RF.GetById<PackingRelation>(emptyOuterPackage.Id);
                    RT.Service.Resolve<PackingRelationController>().AddPackage(emptyOuterPackage, relations[i]);
                }
                Flag_IsProcessFinishIfProcessFinish(emptyOuterPackage, process, workOrder);
                if (listPackrelation == null) listPackrelation = new EntityList<PackingRelation>();
                listPackrelation.Add(emptyOuterPackage);
                emptyOuterPackage = CasecadeDoPacking(emptyOuterPackage, workcell, workOrder, ruleDetail, isCasecade, listPackrelation) ?? emptyOuterPackage;
                trans.Complete();
                return listPackrelation;
            }
        }

        /// <summary>
        /// 验证打包条件
        /// </summary>
        /// <param name="relations">包装关系列表</param>
        /// <param name="detail">外包装关系</param>
        void ValidatePackingRelations(EntityList<PackingRelation> relations, WorkOrderPackageRuleDetail detail)
        {
            if (relations.Count <= 0)
                throw new ValidationException("请选择包装".L10N());
            var relation = relations.FirstOrDefault();
            if (relations.Count == 1 && relation.PackageNo.IsNullOrEmpty())
                throw new ValidationException("打包失败，选择的包装未打包，不允许再打包".L10N());
            if (relations.Count > 1 && relations.Any(p => p.PackageNo.IsNullOrEmpty()) && !relations.Any(p => p.PackageUnitId != relation.PackageUnitId) && !relations.Any(p => p.WorkOrderId != relation.WorkOrderId))
                throw new ValidationException("打包失败，多个包装打包必须都是同一包装层级的已打包包装且工单一致".L10N());
            if (relations.Count > detail.LevelQty)
                throw new ValidationException("打包失败，外包装最大包装数为[{0}]，当前包装数为[{1}]".L10nFormat(detail.LevelQty, relations.Count));
        }

        /// <summary>
        /// 验证是否
        /// </summary>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="process">工序</param>
        /// <param name="workOrder">工单</param>
        void ValidateHasPackingUnit(PackingUnit packingUnit, Process process, WorkOrder workOrder)
        {
            if (packingUnit == null)
                throw new ValidationException("包装单位不能为空".L10N());
            if (process == null)
                throw new ValidationException("工序不能为空".L10N());
            if (workOrder == null)
                throw new ValidationException("工单不能为空".L10N());
            var rules = workOrder.PackageRuleDetailList;
            var processInPackingUnits = rules.Where(f => !f.PackageUnit.IsMasterUnit).SelectMany(f => f.WorkOrderProcessPackingUnitList)
                .Where(f => f.ProcessId == process.Id).OrderBy(f => SortExtension.GetIndex(f.PackageRule)).ToArray();
            var unitHasProcess = processInPackingUnits.FirstOrDefault(f => f.PackageRule.PackageUnitId == packingUnit.Id);
            if (processInPackingUnits.Any() && unitHasProcess == null)
                throw new ValidationException("工单[{0}],工序[{1}]未找到配置包装[{2}]权限".L10nFormat(workOrder.No, process.Name, packingUnit.Name));
        }
        #endregion 

        #region 包装流程控制事件 PackingStrategyEvent
        /// <summary>
        /// 包装物料数量是否装满
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="workOrder">工单</param>
        /// <returns>bool</returns>
        [IgnoreProxy]
        public virtual bool IsPackingRelationItemQtyFull(PackingRelation packingRelation, WorkOrder workOrder)
        {
            if (packingRelation == null) throw new ArgumentNullException(nameof(packingRelation));

            if (workOrder == null) throw new ArgumentNullException(nameof(workOrder));

            var outerPackageRuleDetail = GetPackageRuleDetail(packingRelation.PackageUnit, workOrder);
            return packingRelation.ItemQty == outerPackageRuleDetail.Qty;
        }

        /// <summary>
        /// 包装包装数量是否装满
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="workOrder">工单</param>
        /// <returns>bool</returns>
        public virtual bool IsPackingRelationPackageQtyFull(PackingRelation packingRelation, WorkOrder workOrder)
        {
            if (packingRelation == null)
                throw new ArgumentNullException(nameof(packingRelation));
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder));
            var outerPackageRuleDetail = GetPackageRuleDetail(packingRelation.PackageUnit, workOrder);
            var innerPackageDetail = GetInnerPackageRuleDetail(packingRelation.PackageUnit, workOrder);
            return packingRelation.PackedQty == (outerPackageRuleDetail.Qty / innerPackageDetail.Qty);
        }

        /// <summary>
        /// 是否满足整个包装规则
        /// </summary>
        /// <param name="relationId">包装关系ID</param>
        /// <param name="workOrder">工单</param>
        /// <returns>bool</returns>
        private bool IsPackageRuleFull(double relationId, WorkOrder workOrder)
        {
            if (workOrder == null) throw new ArgumentNullException(nameof(workOrder));
            var topRule = GetTopOuterPackageRuleDetail(workOrder);
            var topPkg = RF.GetById<PackingRelation>(relationId);
            if (topRule.PackageUnitId != topPkg.PackageUnitId || topRule.Qty != topPkg.ItemQty) return false;

            return true;
        }

        /// <summary>
        /// 穿件包装
        /// </summary>
        /// <param name="group"></param>
        /// <param name="scanModel"></param>
        /// <param name="insideBarcode"></param>
        /// <param name="relation"></param>
        /// <param name="workOrder"></param>
        /// <param name="unit"></param>
        /// <returns></returns>
        public virtual PackingStrategyEvent CreatePackingEvent(string group, ScanMode scanModel, string insideBarcode, PackingRelation relation, WorkOrder workOrder, PackingUnit unit)
        {
            var packingEvent = new PackingStrategyEvent();
            packingEvent.Group = group;
            packingEvent.StrategyType = scanModel == ScanMode.Normal ? ScanStrategyMode.ScanSingle : ScanStrategyMode.ScanOneJoinToMany;
            packingEvent.InsiderBarcode = insideBarcode.IsNotEmpty() ? new string[] { insideBarcode } : null;
            packingEvent.IsPackageItemFull = IsPackingRelationItemQtyFull(relation, workOrder);
            packingEvent.IsPackageQtyFull = unit == null && IsPackingRelationPackageQtyFull(relation, workOrder);
            packingEvent.IsPackageRuleFull = IsPackageRuleFull(relation.Id, workOrder);
            packingEvent.OuterPackingRelation = relation;
            return packingEvent;
        }
        #endregion
    }
}