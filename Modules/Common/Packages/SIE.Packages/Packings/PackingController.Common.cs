using SIE.Common.Sort;
using SIE.Core.WorkOrders;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using System;
using System.Linq;

namespace SIE.Packages
{
    /// <summary>
    /// 包装采集控制器<c>PackingController</c>
    /// </summary>
    public partial class PackingController
    {
        /// <summary>
        /// 获取指定单位的包装规则明细
        /// </summary>
        /// <param name="unit">包装单位</param>
        /// <param name="rule">包装规则</param>
        /// <returns>包装规则明细</returns>
        [IgnoreProxy]
        public virtual PackageRuleDetail GetPackageRuleDetail(PackingUnit unit, PackageRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (unit == null) throw new ArgumentNullException(nameof(unit));
            var packageRuleDetail = rule.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == unit.Id);
            if (packageRuleDetail == null)
                throw new ValidationException("当前物料不存在{0}包装规则".L10nFormat(unit.Name));
            return packageRuleDetail;
        }

        /// <summary>
        /// 获取父包装规则
        /// </summary>
        /// <param name="ruleDetail">包装规则明细(参数)</param>
        /// <returns>包装规则明细(返回)</returns>
        [IgnoreProxy]
        public virtual PackageRuleDetail GetParentPackageRuleDetail(PackageRuleDetail ruleDetail)
        {
            if (ruleDetail == null) throw new ArgumentNullException(nameof(ruleDetail));

            var parentRuleDtl = ruleDetail.PackageRule.PackageRuleDetailList.FirstOrDefault(f => SortExtension.GetIndex(f) > SortExtension.GetIndex(ruleDetail));
            if (parentRuleDtl == null)
                throw new ValidationException("[{0}]缺少外包装规则".L10nFormat(ruleDetail.PackageUnit.Name));

            return parentRuleDtl;
        }

        /// <summary>
        /// 获取主设备包装规则明细
        /// </summary>
        /// <param name="rule">包装规则</param>
        /// <returns>包装规则明细</returns>
        [IgnoreProxy]
        public virtual PackageRuleDetail GetMasterUnitPackageRuleDetail(PackageRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            var dtl = rule.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnit.IsMasterUnit);
            if (dtl == null)
                throw new ValidationException("当前物料包装规则未配置主单位规则".L10N());
            return dtl;
        }

        /// <summary>
        /// 根据条码确定包装关系，如果条码属于SN那么取SN外包装，如果条码是个包装则返回
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="isThrowExceptionIfNotFind">找不到异常</param>
        /// <returns>包装关系</returns>
        [IgnoreProxy]
        public virtual PackingRelation FindContactPackingRelationByBarcode(string barcode, bool isThrowExceptionIfNotFind = true)
        {
            if (barcode.IsNullOrWhiteSpace())
            {
                throw new ArgumentNullException(nameof(barcode));
            }

            return RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode)?.Relation
                ?? (isThrowExceptionIfNotFind ? RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcode) :
                RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcode, false));
        }

        /// <summary>
        /// 获取物料标签
        /// </summary>
        /// <param name="barcodes">编码</param>
        /// <returns>物料标签</returns>
        [IgnoreProxy]
        public virtual ItemLabel GetSKU(string barcodes)
        {
            if (barcodes.IsNullOrWhiteSpace())
                throw new ArgumentNullException(nameof(barcodes));
            ItemLabel itemLabel = null;
            var pkg = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcodes, false);
            if (pkg != null)
                itemLabel = RT.Service.Resolve<ItemLabelController>().GetRootPackingRelationItemLabel(pkg.RootId);
            if (itemLabel == null)
                itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcodes);
            if (itemLabel == null)
                throw new ValidationException("[{0}]未找到SKU".L10nFormat(barcodes));
            return itemLabel;
        }

        /// <summary>
        /// 获取对应包装物料信息
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <returns>物料标签</returns>
        [IgnoreProxy]
        public virtual ItemLabel GetItemLabel(PackingRelation packingRelation)
        {
            if (packingRelation == null) throw new ArgumentNullException(nameof(packingRelation));
            //// ItemLabel item = null;
            ////TODO没有TreeChildrenList
            ////if (!packingRelation.TreeChildrenList.Any())
            ////{
            ////    var criteria = new CommonQueryCriteria().Add(new PropertyMatch(ItemLabel.RelationIdProperty, BinaryOp.Equal, packingRelation.Id));
            ////    var itemLabel = RF.Find<ItemLabel>().GetFirstBy(criteria) as ItemLabel;
            ////    if (itemLabel == null || itemLabel.Item == null) throw new ValidationException("包装找不到物料信息");
            ////    return itemLabel;
            ////}
            ////var relation = packingRelation.TreeChildrenList.EachNode(f =>
            ////{
            ////    if (!f.TreeChildrenList.Any())
            ////    {
            ////        var leafPackageRelation = f as PackingRelation;

            ////        var criteria = new CommonQueryCriteria().Add(new PropertyMatch(ItemLabel.RelationIdProperty, BinaryOp.Equal, leafPackageRelation.Id));

            ////        var itemLabel = RF.Find<ItemLabel>().GetFirstBy(criteria) as ItemLabel;

            ////        if (itemLabel == null) return false;
            ////        item = itemLabel;
            ////        return true;
            ////    }
            ////    return false;
            ////});
            ////if (item == null)
            throw new ValidationException("包装找不到物料信息".L10nFormat(packingRelation.PackageUnit.Name));
            //// return item;
        }

        /// <summary>
        /// 获取包装规则
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <returns>包装规则</returns>
        [IgnoreProxy]
        public virtual PackageRule GetPackageRule(PackingRelation packingRelation)
        {
            if (packingRelation == null) throw new ArgumentNullException(nameof(packingRelation));
            ItemLabel itemLabel = GetItemLabel(packingRelation);
            var defautlRule = RT.Service.Resolve<PackageController>().GetDefaultItemPackageRuleByItemId(itemLabel.ItemId);
            if (defautlRule == null || defautlRule.PackageRule == null) throw new ValidationException("找不到默认的包装规则".L10N());

            return defautlRule.PackageRule;
        }

        /// <summary>
        /// 获取包装规则
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        /// <returns>包装规则</returns>
        [IgnoreProxy]
        public virtual PackageRule GetPackageRule(ItemLabel itemLabel)
        {
            if (itemLabel == null) throw new ArgumentNullException(nameof(itemLabel));
            if (itemLabel.Item == null) throw new ValidationException("物料标签缺失物料".L10N());
            return GetPackageRule(itemLabel.Item);
        }

        /// <summary>
        /// 获取包装规则
        /// </summary>
        /// <param name="item">物料</param>
        /// <returns>包装规则</returns>
        [IgnoreProxy]
        public virtual PackageRule GetPackageRule(Item item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            var defautlRule = RT.Service.Resolve<PackageController>().GetDefaultItemPackageRuleByItemId(item.Id);
            if (defautlRule == null || defautlRule.PackageRule == null) throw new ValidationException("找不到默认的包装规则".L10N());

            return defautlRule.PackageRule;
        }

        /// <summary>
        /// 验证单位
        /// </summary>
        /// <param name="addPackingUnit">包装单位</param>
        /// <param name="outerPackingUnit">输出包装单位</param>
        /// <param name="rule">包装规则</param>
        [IgnoreProxy]
        protected virtual void VaildationGreatethanInnerUnit(PackingUnit addPackingUnit, PackingUnit outerPackingUnit, PackageRule rule)
        {
            if (addPackingUnit == null) throw new ArgumentNullException(nameof(addPackingUnit));
            if (outerPackingUnit == null) throw new ArgumentNullException(nameof(outerPackingUnit));
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            var add = GetPackageRuleDetail(addPackingUnit, rule);
            var outer = GetPackageRuleDetail(outerPackingUnit, rule);
            if (SortExtension.GetIndex(add) >= SortExtension.GetIndex(outer))
                throw new ValidationException("不允许小包装放大包装".L10N());
        }

        /// <summary>
        /// 验证加入包装单位是否符合包装规则
        /// </summary>
        /// <param name="outerPackage">外包装关系</param>
        /// <param name="addPackingUnit">加入的包装单位</param>
        /// <param name="rule">包装规则</param>
        private void VaildationJoinUnit(PackingRelation outerPackage, PackingUnit addPackingUnit, PackageRule rule)
        {
            if (outerPackage == null) throw new ArgumentNullException(nameof(outerPackage));
            if (addPackingUnit == null) throw new ArgumentNullException(nameof(addPackingUnit));
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            PackageRuleDetail outerChildrenPackageRule = GetInnerPackageRuleDetail(outerPackage.PackageUnit, rule);

            if (addPackingUnit.Id != outerChildrenPackageRule.PackageUnitId)
                throw new ValidationException("包装[{0}{1}]只能放[{2}]".L10nFormat(outerPackage.PackageUnit.Name, outerPackage.PackageNo, outerChildrenPackageRule.PackageUnit.Name));
        }

        ///// <summary>
        ///// 验证物料数量
        ///// </summary>
        ///// <param name="addItemQty">加入数量</param>
        ///// <param name="outerPackage">外包装关系</param>
        ///// <param name="rule">外包装规则</param>
        ///// <exception cref="ValidationException">验证异常</exception>
        //private void VaildationJoinItemQty(decimal addItemQty, PackingRelation outerPackage, PackageRule rule)
        //{
        //    if (outerPackage == null)
        //        throw new ArgumentNullException(nameof(outerPackage));
        //    if (addItemQty == 0)
        //        throw new ArgumentOutOfRangeException(nameof(addItemQty), "加入数量0");
        //    if (rule == null)
        //        throw new ArgumentNullException(nameof(rule));

        //    var outerPackageRule = GetPackageRuleDetail(outerPackage.PackageUnit, rule);
        //    if ((outerPackage.ItemQty + addItemQty) > outerPackageRule.Qty)
        //    {
        //        throw new ValidationException("包装[{0}{1}]已满,[{2}]个产品".L10nFormat(outerPackage.PackageUnit.Name, outerPackage.PackageNo, outerPackage.ItemQty));
        //    }
        //}

        /// <summary>
        /// 获取包装关系相应的内包装工单包装规则明细
        /// </summary>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="rule">包装规则</param>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <exception cref="ArgumentException">参数异常</exception>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <returns>包装关系相应的外包装工单包装规则明细</returns>
        [IgnoreProxy]
        public virtual PackageRuleDetail GetInnerPackageRuleDetail(PackingUnit packingUnit, PackageRule rule)
        {
            var childrenRuleDetail = GetInnerPackageRuleDetailCore(packingUnit, rule);
            if (childrenRuleDetail == null)
                throw new ValidationException("[{0}]对应的包装规则无内包装".L10nFormat(packingUnit.Name));

            return childrenRuleDetail;
        }

        /// <summary>
        /// 获取包装规则明细
        /// </summary>
        /// <param name="packingUnit">包装单位</param>
        /// <param name="rule">包装规则</param>
        /// <returns>包装规则明细</returns>
        private PackageRuleDetail GetInnerPackageRuleDetailCore(PackingUnit packingUnit, PackageRule rule)
        {
            if (packingUnit == null) throw new ArgumentNullException(nameof(packingUnit));

            if (rule == null) throw new ArgumentNullException(nameof(rule));

            var outerPackRuleDetail = GetPackageRuleDetail(packingUnit, rule);
            var index = rule.PackageRuleDetailList.IndexOf(outerPackRuleDetail);
            var childrenRuleDetail = rule.PackageRuleDetailList.ElementAtOrDefault(index - 1);

            return childrenRuleDetail;
        }

        /// <summary>
        /// 包装物料数量是否装满
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="rule">b包装规则</param>
        /// <returns>bool</returns>
        [IgnoreProxy]
        public virtual bool IsPackingRelationItemQtyFull(PackingRelation packingRelation, PackageRule rule)
        {
            if (packingRelation == null) throw new ArgumentNullException(nameof(packingRelation));
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            var outerPackageRuleDetail = GetPackageRuleDetail(packingRelation.PackageUnit, rule);
            return packingRelation.ItemQty == outerPackageRuleDetail.Qty;
        }

        /// <summary>
        /// 包装包装数量是否装满
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="rule">包装规则</param>
        /// <returns>bool</returns>
        [IgnoreProxy]
        public virtual bool IsPackingRelationPackageQtyFull(PackingRelation packingRelation, PackageRule rule)
        {
            if (packingRelation == null) throw new ArgumentNullException(nameof(packingRelation));
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            var outerPackageRuleDetail = GetPackageRuleDetail(packingRelation.PackageUnit, rule);
            var innerPackageDetail = GetInnerPackageRuleDetail(packingRelation.PackageUnit, rule);
            return packingRelation.PackedQty == (outerPackageRuleDetail.Qty / innerPackageDetail.Qty);
        }

        /// <summary>
        /// 是否满足整个包装规则
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="rule">包装规则</param>
        /// <returns>bool</returns>
        [IgnoreProxy]
        public virtual bool IsPackageRuleFull(PackingRelation packingRelation, PackageRule rule)
        {
            if (packingRelation == null) throw new ArgumentNullException(nameof(packingRelation));
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            var topRule = GetTopPackageRuleDetail(rule);
            var topPkg = RF.GetById<PackingRelation>(packingRelation.RootId);
            if (topRule.PackageUnitId != topPkg.PackageUnitId || topRule.Qty != topPkg.ItemQty) return false;
            return true;
        }

        /// <summary>
        /// 获取最外层包装规则明细
        /// </summary>
        /// <param name="rule">包装规则</param>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <returns>包装关系相应的外包装工单包装规则明细</returns>
        [IgnoreProxy]
        public virtual PackageRuleDetail GetTopPackageRuleDetail(PackageRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));

            var topRuleDetail = rule.PackageRuleDetailList.Last();

            return topRuleDetail;
        }

        /// <summary>
        /// 获取包装对应的工单信息
        /// </summary>
        /// <param name="packingRelation">包装关系</param>
        /// <param name="throwExcpetionIfNotFinded">找不到异常</param>
        /// <exception cref="ValidationException">验证异常</exception>
        /// <exception cref="ArgumentNullException">空异常</exception>
        /// <returns>工单</returns>
        [IgnoreProxy]
        public virtual WorkOrder GetWorkOrder(PackingRelation packingRelation, bool throwExcpetionIfNotFinded = true)
        {
            var workOrder = RT.Service.Resolve<PackingRelationController>().GetWorkOrder(packingRelation);
            if (throwExcpetionIfNotFinded && workOrder == null)
                throw new ValidationException("找不到此包装[{0}]对应的工单信息".L10nFormat(packingRelation.PackageNo));
            return workOrder;
        }
    }
}
