using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WorkOrders;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.Packings.Enums;
using System;
using System.Linq;

namespace SIE.MES.WIP.Packings
{
    /// <summary>
    /// 包装采集控制器
    /// </summary>
    public partial class WipPackingController
    {
        #region 工单包装规则 WorkOrderPackageRuleDetail 
        /// <summary>
        /// 获取工单主单位包装规则明细
        /// </summary>
        /// <param name="workOrder">对应的工单</param>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <returns>包装关系相应的外包装工单包装规则明细</returns> 
        public virtual WorkOrderPackageRuleDetail GetMasterUnitPackageRuleDetail(WorkOrder workOrder)
        {
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder));
            var mastUnitRuleDetail = workOrder.PackageRuleDetailList.OrderBy(p => SortExtension.GetIndex(p)).FirstOrDefault();
            if (mastUnitRuleDetail == null || !mastUnitRuleDetail.PackageUnit.IsMasterUnit)
                throw new ValidationException("无法找到工单[{0}]的包装规则主单位,请确保主单位已经维护并且是第一个".L10nFormat(workOrder.No));
            return mastUnitRuleDetail;
        }

        /// <summary>
        /// 获取最外层工单包装规则明细
        /// </summary>
        /// <param name="workOrder">对应的工单</param>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <returns>包装关系相应的外包装工单包装规则明细</returns>
        [IgnoreProxy]
        public virtual WorkOrderPackageRuleDetail GetTopOuterPackageRuleDetail(WorkOrder workOrder)
        {
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder));
            var workOrderRules = GetWorkOrderPackageRule(workOrder);
            var topRuleDetail = workOrderRules.Last();
            return topRuleDetail;
        }

        /// <summary>
        /// 获取包装关系相应的内包装工单包装规则明细
        /// </summary>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="workOrder">对应的工单</param>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <exception cref="ArgumentException">无效参数异常</exception>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <returns>包装关系相应的外包装工单包装规则明细</returns>
        [IgnoreProxy]
        public virtual WorkOrderPackageRuleDetail GetInnerPackageRuleDetail(PackingUnit packingUnit, WorkOrder workOrder)
        {
            var rule = GetInnerPackageRuleDetailCore(packingUnit, workOrder);
            if (rule == null)
                throw new ValidationException("工单[{0}],[{1}]对应的包装规则无内包装".L10nFormat(workOrder.No, packingUnit.Name));
            return rule;
        }

        /// <summary>
        /// 获取内部包装规则明细
        /// </summary>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="workOrder">工单</param>
        /// <returns>工单包装规则明细</returns>
        private WorkOrderPackageRuleDetail GetInnerPackageRuleDetailCore(PackingUnit packingUnit, WorkOrder workOrder)
        {
            if (packingUnit == null)
                throw new ArgumentNullException(nameof(packingUnit));
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder));
            var workOrderPackRuleDetail = GetPackageRuleDetail(packingUnit, workOrder);
            var workOrderRules = GetWorkOrderPackageRule(workOrder);
            var index = workOrderRules.IndexOf(workOrderPackRuleDetail);
            var childrenRuleDetail = workOrderRules.ElementAtOrDefault(index - 1);
            return childrenRuleDetail;
        }

        /// <summary>
        /// 获取包装关系相应的工单包装规则明细
        /// </summary>
        /// <param name="packingUnit">包装关系</param>
        /// <param name="workOrder">对应的工单</param>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <exception cref="ArgumentException">无效参数异常</exception>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <returns>包装关系相应的工单包装规则明细</returns>
        [IgnoreProxy]
        public virtual WorkOrderPackageRuleDetail GetPackageRuleDetail(PackingUnit packingUnit, WorkOrder workOrder)
        {
            if (packingUnit == null)
                throw new ArgumentNullException(nameof(packingUnit));
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder));
            var workOrderRules = GetWorkOrderPackageRule(workOrder);
            var innerPackageRule = workOrderRules.FirstOrDefault(f => f.PackageUnitId == packingUnit.Id);
            if (innerPackageRule == null)
                throw new ValidationException("工单[{0}]不存在[{1}]对应的包装规则".L10nFormat(workOrder.No, RF.GetById<PackingUnit>(packingUnit.Id)?.Name));
            return innerPackageRule;
        }

        /// <summary>
        /// 获取上一层工单包装规则明细
        /// </summary>
        /// <param name="ruleDetail">当前包装规则</param>
        /// <param name="workOrder">工单</param>
        /// <param name="throwExceptionIfNotFind">是否抛未找到异常</param>
        /// <returns>工单包装规则明细</returns>
        WorkOrderPackageRuleDetail GetOuterPackageRuleDetail(WorkOrderPackageRuleDetail ruleDetail, WorkOrder workOrder, bool throwExceptionIfNotFind = true)
        {
            var parentRuleDetail = workOrder.PackageRuleDetailList.Where(p => SortExtension.GetIndex(p) > SortExtension.GetIndex(ruleDetail)).OrderBy(p => SortExtension.GetIndex(p)).FirstOrDefault();
            if (throwExceptionIfNotFind && parentRuleDetail == null)
                throw new ValidationException("工单[{0}],[{1}]对应的包装规则无外包装".L10nFormat(workOrder.No, ruleDetail.PackageUnit.Name));
            return parentRuleDetail;
        }

        /// <summary>
        /// 获取包装关系相应的外包装工单包装规则明细
        /// </summary>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="workOrder">对应的工单</param>
        /// <param name="throwExceptionIfNotFind">如果不存在 抛出异常</param>
        /// <exception cref="ArgumentNullException">空异常(包装单位)</exception>
        /// <exception cref="ArgumentNullException">空异常(工单)</exception>
        /// <exception cref="ArgumentException">无效参数异常</exception>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <returns>包装关系相应的外包装工单包装规则明细</returns>
        [IgnoreProxy]
        public virtual WorkOrderPackageRuleDetail GetOuterPackageRuleDetail(PackingUnit packingUnit, WorkOrder workOrder, bool throwExceptionIfNotFind = true)
        {
            if (packingUnit == null) throw new ArgumentNullException(nameof(packingUnit));

            if (workOrder == null) throw new ArgumentNullException(nameof(workOrder));

            var workOrderPackRuleDetail = GetPackageRuleDetail(packingUnit, workOrder);
            var workOrderRules = GetWorkOrderPackageRule(workOrder);
            var index = workOrderRules.IndexOf(workOrderPackRuleDetail);
            var parentRuleDetail = workOrderRules.ElementAtOrDefault(index + 1);
            if (throwExceptionIfNotFind && parentRuleDetail == null)
                throw new ValidationException("工单[{0}],[{1}]对应的包装规则无外包装".L10nFormat(workOrderRules.First().WorkOrder.No, workOrderPackRuleDetail.PackageUnit.Name));
            return parentRuleDetail;
        }

        /// <summary>
        /// 获取包装对应的工单包装规则明细
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <returns>工单包装规则明细</returns>
        [IgnoreProxy]
        protected virtual EntityList<WorkOrderPackageRuleDetail> GetWorkOrderPackageRule(WorkOrder workOrder)
        {
            if (workOrder == null)
                throw new ArgumentNullException(nameof(workOrder));
            if (workOrder.PackageRuleDetailList.Count < 2)
                throw new ValidationException("工单[{0}]包装规则不完整".L10nFormat(workOrder.No));
            return workOrder.PackageRuleDetailList;
        }

        /// <summary>
        /// 获取外包装层级的包装数
        /// </summary>
        /// <param name="packingUnit">内包装单位</param>
        /// <param name="workOrder">工单</param>
        /// <returns>包装数</returns>
        public virtual decimal GetOutPackingLevelQty(PackingUnit packingUnit, WorkOrder workOrder)
        {
            var ruleDetail = GetOuterPackageRuleDetail(GetPackageRuleDetail(packingUnit, workOrder), workOrder);
            if (!ruleDetail.NumberRuleId.HasValue)
                throw new ValidationException("工单[{0}],未配置[{1}]条码规则".L10nFormat(workOrder.No, ruleDetail.PackageUnit.Name));
            return ruleDetail.LevelQty;
        }
        #endregion

        /// <summary>
        /// 获取一个物料标签
        /// </summary>
        /// <param name="barcodes">条码</param>
        /// <returns>物料标签</returns>
        ItemLabel GetItemLabel(string barcodes)
        {
            ItemLabel itemLabel = RT.Service.Resolve<PackingController>().GetSKU(barcodes);
            if (itemLabel.SourceType != LabelSource.Wip)
                throw new ValidationException("[{0}]不能进行采集，[{2}]的标签才能进行采集".L10nFormat(barcodes, LabelSource.Wip.ToLabel()));
            return itemLabel;
        }

        /// <summary>
        /// 验证工序是否有包装权限
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="outerPackageRule">外部包装规则</param>
        protected virtual void VaildationPackingUnitHasProcess(Workcell workcell, WorkOrderPackageRuleDetail outerPackageRule)
        {
            if (outerPackageRule == null) throw new ArgumentNullException(nameof(outerPackageRule));
            if (workcell == null) throw new ArgumentNullException(nameof(workcell));
            if (!RT.Service.Resolve<WorkOrderController>().PackingUnitHasProcess(workcell.ProcessId, outerPackageRule))
                throw new ValidationException("未配置对应可操作包装权限".L10N());
        }

        /// <summary>
        /// 获取包装对应的工单信息
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="throwExcpetionIfNotFinded">如果不存在 抛出异常</param>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <returns>工单</returns>
        [IgnoreProxy]
        public virtual WorkOrder GetWorkOrder(PackingRelation packingRelation, bool throwExcpetionIfNotFinded = true)
        {
            var workOrder = RT.Service.Resolve<PackingController>().GetWorkOrder(packingRelation, throwExcpetionIfNotFinded);
            if (workOrder == null) return null;
            return GetById<WorkOrder>(workOrder.Id);
        }

        /// <summary>
        /// 在指定工单里,验证加入的包装单位时，是否大于等于指定的外包装单位
        /// </summary>
        /// <param name="addPackingUnit">包装关系</param>
        /// <param name="outerPackingUnit">外部包装单位</param>
        /// <param name="workOrder">工单</param>
        protected virtual void VaildationGreatethanInnerUnit(PackingUnit addPackingUnit, PackingUnit outerPackingUnit, WorkOrder workOrder)
        {
            if (addPackingUnit == null) throw new ArgumentNullException(nameof(addPackingUnit));

            if (outerPackingUnit == null) throw new ArgumentNullException(nameof(outerPackingUnit));

            if (workOrder == null) throw new ArgumentNullException(nameof(workOrder));

            var add = GetPackageRuleDetail(addPackingUnit, workOrder);
            var outer = GetPackageRuleDetail(outerPackingUnit, workOrder);
            if (SortExtension.GetIndex(add) >= SortExtension.GetIndex(outer))
                throw new ValidationException("不允许小包装放大包装".L10N());
        }

        /// <summary>
        /// 验证物料数量
        /// </summary>
        /// <param name="addItemQty">加入数量</param>
        /// <param name="outerPackage">外包装关系</param>
        /// <param name="workOrder">工单</param>
        /// <exception cref="ValidationException">产品满包装</exception>
        /// <exception cref="ArgumentNullException">外包装为空，工单为空，数据小于0</exception>
        protected virtual void VaildationJoinItemQty(decimal addItemQty, PackingRelation outerPackage, WorkOrder workOrder)
        {
            if (outerPackage == null) throw new ArgumentNullException(nameof(outerPackage));

            if (workOrder == null) throw new ArgumentNullException(nameof(workOrder));

            if (addItemQty <= 0) throw new ArgumentOutOfRangeException(nameof(addItemQty), "数量必须大于0".L10N());

            var outerPackageRule = GetPackageRuleDetail(outerPackage.PackageUnit, workOrder);
            if ((outerPackage.ItemQty + addItemQty) > outerPackageRule.Qty)
            {
                throw new ValidationException("采集失败,包装[{0}{1}]已满,[{2}]个产品".L10nFormat(outerPackage.PackageUnit.Name, outerPackage.PackageNo, outerPackage.ItemQty));
            }
        }

        /// <summary>
        /// 包装规格兼容性验证
        /// </summary>
        /// <param name="outerPackage">外包装</param>
        /// <param name="unit">包装单位</param>
        /// <param name="outerWorkOrder">外包装工单</param>
        /// <param name="innerWorkOrder">内包装工单</param>
        /// <param name="mode">验证方式</param>
        protected virtual void VaildationRuleQtyIsSame(PackingRelation outerPackage, PackingUnit unit, WorkOrder outerWorkOrder, WorkOrder innerWorkOrder, PackingRuleValidMode mode)
        {
            WorkOrderPackageRuleDetail outerRule = null, innerRule = null;
            if (mode == PackingRuleValidMode.None)
            {
                return;
            }
            if (mode == PackingRuleValidMode.Current || mode == PackingRuleValidMode.Child)
            {
                outerRule = GetPackageRuleDetail(unit, outerWorkOrder);
                innerRule = GetPackageRuleDetail(unit, innerWorkOrder);
                if (outerRule.Qty != innerRule.Qty)
                    throw new ValidationException("采集失败,包装[{0}{1}]只允许放包装规格[{2}个/{3}]".L10nFormat(outerPackage.PackageUnit?.Name, outerPackage.PackageNo, outerRule.Qty, outerRule.PackageUnit?.Name));
            }

            if (mode == PackingRuleValidMode.Child && !unit.IsMasterUnit)
            {
                PackingUnit currentUnit = null;
                while (outerRule != null)
                {
                    currentUnit = outerRule.PackageUnit;
                    outerRule = GetInnerPackageRuleDetailCore(currentUnit, outerWorkOrder);
                    Check.NotNull(outerRule, "外包装为空".L10N());
                    innerRule = GetInnerPackageRuleDetailCore(currentUnit, innerWorkOrder);
                    Check.NotNull(innerRule, "内包装为空".L10N());
                    if (outerRule.Qty != innerRule.Qty || innerRule.PackageUnitId != outerRule.PackageUnitId)
                    {
                        throw new ValidationException("采集失败,包装[{0}{1}]只允许放包装规格[{2}个/{3}]".L10nFormat(outerPackage.PackageUnit?.Name, outerPackage.PackageNo, outerRule.Qty, outerRule.PackageUnit?.Name));
                    }
                }
            }
        }

        /// <summary>
        /// 验证是否相同产品
        /// </summary>
        /// <param name="outerWorkOrder">外部工单</param>
        /// <param name="innerWorkOrder">内部工单</param>
        protected virtual void VaildationSameProduct(WorkOrder outerWorkOrder, WorkOrder innerWorkOrder)
        {
            if (innerWorkOrder == null)
                throw new ArgumentNullException(nameof(innerWorkOrder));
            if (outerWorkOrder == null)
                throw new ArgumentNullException(nameof(outerWorkOrder));
            if (outerWorkOrder.ProductId != innerWorkOrder.ProductId)
                throw new ValidationException("必须包装相同的物料,[({0}){1}]".L10nFormat(innerWorkOrder.Product.Code, innerWorkOrder.Product.Name));
        }

        /// <summary>
        /// 获取包装关系对应工单ID
        /// </summary>
        /// <param name="packingNo">包装号</param>
        /// <returns>工单ID</returns>
        public virtual double? GetRelationWorkOrderId(string packingNo)
        {
            return RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(packingNo, false)?.WorkOrderId;
        }
    }
}