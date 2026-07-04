using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.Packings;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Packages
{
    /// <summary>
    /// 包装采集控制器<c>PackingController</c>
    /// </summary>
    public partial class PackingController : DomainController
    {
        #region JoinInPackage
        /// <summary>
        /// 加入一个包装到指定包装里
        /// </summary>
        /// <param name="selectOuterPackingRelationId">选择外部包装关系</param>
        /// <param name="innerPackageNo">内部包装编号</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="packingUnit">包装单位</param>
        /// <returns>包装关系</returns>
        public virtual PackingRelation JoinInPackage(double selectOuterPackingRelationId, string innerPackageNo, Workcell workcell, PackingUnit packingUnit = null)
        {
            if (workcell == null) throw new ArgumentNullException(nameof(workcell));
            if (innerPackageNo.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(innerPackageNo));
            if (selectOuterPackingRelationId == 0) throw new ArgumentNullException(nameof(selectOuterPackingRelationId));
            using (var trans = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                CheckUserHasWarehouse(workcell);
                CheckWarehouseHasLocalStorageLocation(workcell);
                var relationController = RT.Service.Resolve<PackingRelationController>();
                PackingRelation outerPackage = relationController.GetPackingRelation(selectOuterPackingRelationId);
                PackingRelation innerPackage = relationController.GetBatchPackingRelation(innerPackageNo);
                ////TODO没有TreeChildrenList
                ////PackingRelation SourceParent = innerPackage.TreeParent as PackingRelation;
                ////PackageRule innerRule = GetPackageRule(innerPackage);
                ////PackageRuleDetail innerPackageRule = GetPackageRuleDetail(innerPackage.PackageUnit, innerRule);
                PackageRule rule = GetPackageRule(outerPackage);
                ////PackageRuleDetail outerPackageRule = GetPackageRuleDetail(outerPackage.PackageUnit, rule);
                VaildationNotDoPacking(outerPackage);
                VaildationNotInPackage(innerPackage);
                ////var outerItemLabel = GetItemLabel(outerPackage);
                var storageLocation = GetById<StorageLocation>(workcell.StorageLocationId);
                if (storageLocation == null) throw new EntityNotFoundException(typeof(StorageLocation), workcell.StorageLocationId);

                VaildationStaticLogisticState(outerPackage, innerPackage);
                VaildationSameProduct(innerPackage, outerPackage);
                if (packingUnit == null)
                {
                    VaildationJoinUnit(outerPackage, innerPackage.PackageUnit, rule);
                }

                relationController.AddPackage(outerPackage, innerPackage);
                trans.Complete();
                return RF.GetById<PackingRelation>(outerPackage.Id);
            }
        }
        #endregion

        #region CollectPackageNo
        /// <summary>
        /// 采集一个包装放进一个空的包装里
        /// </summary>
        /// <param name="innerPackageNo">内部包装编号</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="packingUnit">包装单位</param>
        /// <returns>包装关系</returns>
        public virtual PackingRelation CollectPackageNo(string innerPackageNo, Workcell workcell, PackingUnit packingUnit = null)
        {
            if (workcell == null) throw new ArgumentNullException(nameof(workcell));

            if (innerPackageNo.IsNullOrWhiteSpace()) throw new ArgumentNullException(nameof(innerPackageNo));

            using (var trans = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                var relationContoller = RT.Service.Resolve<PackingRelationController>();
                var innerPackage = relationContoller.GetBatchPackingRelation(innerPackageNo);
                CheckUserHasWarehouse(workcell);
                var rule = GetPackageRule(innerPackage);
                var innerPackageRuleDetail = GetPackageRuleDetail(innerPackage.PackageUnit, rule);
                VaildationNotBeDoPacking(innerPackage);
                PackageRuleDetail outerPackageRule = null;
                if (packingUnit == null)
                    outerPackageRule = GetParentPackageRuleDetail(innerPackageRuleDetail);
                else
                    outerPackageRule = GetPackageRuleDetail(packingUnit, rule);

                VaildationGreatethanInnerUnit(innerPackageRuleDetail.PackageUnit, outerPackageRule.PackageUnit, rule);
                var storageLocation = GetById<StorageLocation>(workcell.StorageLocationId);
                if (storageLocation == null) throw new EntityNotFoundException(typeof(StorageLocation), workcell.StorageLocationId);

                var outerPackage = RT.Service.Resolve<PackageController>().CreateEmptyPackage(innerPackage.WorkOrderId, outerPackageRule.PackageUnit, innerPackage.State);
                relationContoller.AddPackage(outerPackage, innerPackage);
                trans.Complete();
                return GetById<PackingRelation>(outerPackage.Id);
            }
        }
        #endregion

        /// <summary>
        /// 验证是否已被打包
        /// </summary>
        /// <param name="innerPackage">包装关系</param>
        public virtual void VaildationNotBeDoPacking(PackingRelation innerPackage)
        {
            if (RT.Service.Resolve<PackingRelationController>().IsNotBeDoPacking(innerPackage)) return;

            throw new ValidationException("采集失败,[{0}{1}]已被打包。".L10nFormat(innerPackage.PackageUnit.Name, innerPackage.PackageNo));
        }

        /// <summary>
        /// 验证是否已打包
        /// </summary>
        /// <param name="innerPackage">包装关系</param>
        public virtual void VaildationNotDoPacking(PackingRelation innerPackage)
        {
            if (RT.Service.Resolve<PackingRelationController>().IsNotDoPacking(innerPackage)) return;

            throw new ValidationException("采集失败,[{0}{1}]已打包。".L10nFormat(innerPackage.PackageUnit.Name, innerPackage.PackageNo));
        }

        /// <summary>
        /// 验证是否已被打包 
        /// </summary>
        /// <param name="itemLabel">物料标签</param>
        public virtual void VaildationNotDoPacking(ItemLabel itemLabel)
        {
            if (RT.Service.Resolve<ItemLabelController>().IsNotBeDoPacking(itemLabel)) return;

            throw new ValidationException("采集失败,条码:[{0}]已被[{1}{2}]包装。".L10nFormat(itemLabel.Label, itemLabel.Relation.PackageUnit.Name, itemLabel.Relation.PackageNo));
        }

        /// <summary>
        /// 验证是否在包装内
        /// </summary>
        /// <param name="innerPackage">包装关系</param>
        public virtual void VaildationNotInPackage(PackingRelation innerPackage)
        {
            if (RT.Service.Resolve<PackingRelationController>().IsNotInPackage(innerPackage)) return;

            throw new ValidationException("采集失败,条码:[{0}]已被包装。".L10nFormat(innerPackage.PackageNo));
        }

        /// <summary>
        /// 打包
        /// </summary>
        /// <param name="outerPackingRelation">内部包装关系</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="isCasecade">是否级联</param>
        /// <returns>包装关系</returns>
        public virtual PackingRelation DoPacking(PackingRelation outerPackingRelation, Workcell workcell, bool isCasecade)
        {
            if (outerPackingRelation == null) throw new ArgumentNullException(nameof(outerPackingRelation));

            if (workcell == null || !workcell.EmployeeId.HasValue || !workcell.WarehouseId.HasValue || !workcell.StorageAreaId.HasValue || !workcell.StorageLocationId.HasValue) throw new ArgumentNullException(nameof(workcell));

            using (var trans = DB.TransactionScope(PackageEntityDataProvider.ConnectionStringName))
            {
                var rule = RT.Service.Resolve<PackingController>().GetPackageRule(outerPackingRelation);
                var item = GetItemLabel(outerPackingRelation);
                var unitRule = RT.Service.Resolve<PackingController>().GetPackageRuleDetail(outerPackingRelation.PackageUnit, rule);
                if (unitRule == null) throw new ValidationException("条码[{0}]无法打包,无法在默认包装规则[{1}]中找到对应[{2}]规则".L10nFormat(outerPackingRelation.PackageNo, rule.Name, outerPackingRelation.PackageUnit.Name));

                if (!unitRule.NumberRuleId.HasValue) throw new ValidationException("[{0}]包装规则设置未配置条码规则".L10nFormat(outerPackingRelation.PackageUnit.Name));

                var packingRelation = RT.Service.Resolve<PackingRelationController>().DoPacking(outerPackingRelation.Id, unitRule.NumberRuleId.Value, workcell.EmployeeId.Value);
                packingRelation = CasecadeDoPacking(packingRelation, item, workcell, rule, unitRule, isCasecade) ?? packingRelation;
                trans.Complete();
                return packingRelation;
            }
        }

        /// <summary>
        /// 级联打包
        /// </summary>
        /// <param name="outerPackingRelation">包装关系(参数)</param>
        /// <param name="item">物料</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="rule">包装规则</param>
        /// <param name="ruleDetail">包装规则明细</param>
        /// <param name="isCasecade">是否级联</param>
        /// <returns>包装关系(返回)</returns>
        private PackingRelation CasecadeDoPacking(PackingRelation outerPackingRelation, ItemLabel item, Workcell workcell, PackageRule rule, PackageRuleDetail ruleDetail, bool isCasecade)
        {
            if (!isCasecade || IsTopPackageRule(outerPackingRelation.PackageUnitId, rule))
                return null;

            PackingRelation parentPackingRelation = AutoCollectPackge(outerPackingRelation, item, workcell, ruleDetail);
            if (parentPackingRelation == null) return parentPackingRelation;
            return DoPacking(parentPackingRelation, workcell, isCasecade);
        }

        /// <summary>
        /// 自动包装采集
        /// </summary>
        /// <param name="outerPackingRelation">包装关系(参数)</param>
        /// <param name="item">物料</param>
        /// <param name="workcell">工作单元</param>
        /// <param name="ruleDetail">包装规则明细</param>
        /// <returns>包装关系(返回)</returns>
        private PackingRelation AutoCollectPackge(PackingRelation outerPackingRelation, ItemLabel item, Workcell workcell, PackageRuleDetail ruleDetail)
        {
            var packingRelationList = GetPackagesWithNotGreaterThanMaxRule(outerPackingRelation, item, ruleDetail, workcell);
            PackingRelation parentPackingRelation = null;
            for (int i = 0; i < packingRelationList.Count; i++)
            {
                if (parentPackingRelation == null)
                {
                    parentPackingRelation = CollectPackageNo(packingRelationList[i].PackageNo, workcell);
                }
                else
                {
                    parentPackingRelation = JoinInPackage(parentPackingRelation.Id, packingRelationList[i].PackageNo, workcell);
                }
            }

            return parentPackingRelation;
        }

        /// <summary>
        /// 获取包装
        /// </summary>
        /// <param name="outerPackingRelation">包装关系(参数)</param>
        /// <param name="item">物料</param>
        /// <param name="ruleDetail">包装规则明细</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>包装关系( 返回)</returns>
        private IList<PackingRelation> GetPackagesWithNotGreaterThanMaxRule(PackingRelation outerPackingRelation, ItemLabel item, PackageRuleDetail ruleDetail, Workcell workcell)
        {
            List<double> packingRelationList = new List<double>();
            using (var dba = DbAccesserFactory.Create(PackageEntityDataProvider.ConnectionStringName))
            {
                string sql = GenerateQuerySqlForCasecadPackage(outerPackingRelation, item, workcell);
                using (System.Data.IDataReader dr = dba.ExecuteReader(sql))
                {
                    while (dr.Read())
                    {
                        packingRelationList.Add(dr.GetDecimal(0).ConvertTo<double>());
                    }
                }
            }

            var parentRule = GetParentPackageRuleDetail(ruleDetail);
            var maxPackageQty = (parentRule.Qty / ruleDetail.Qty);
            if (packingRelationList.Count > maxPackageQty)
            {
                var ex = new ValidationException("级联打包失败,存在[{0}]个未打包[{1}]包装".L10nFormat(packingRelationList.Count, outerPackingRelation.PackageUnit.Name));
                ex.Data.Add("NotPackingData", packingRelationList);
                throw ex;
            }

            if (packingRelationList.Count < maxPackageQty) return new EntityList<PackingRelation>();

            var rs = Query<PackingRelation>().Where(f => packingRelationList.ToList().Contains(f.Id)).ToList();
            return rs;
        }

        /// <summary>
        /// 为级联打包创建sql语句
        /// </summary>
        /// <param name="outerPackingRelation">包装关系</param>
        /// <param name="item">物料</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>sql语句</returns>
        private string GenerateQuerySqlForCasecadPackage(PackingRelation outerPackingRelation, ItemLabel item, Workcell workcell)
        {
            return string.Empty;
            ////var pkgMeta = RF.Find<PackingRelation>().EntityMeta;
            ////var labelMeta = RF.Find<ItemLabel>().EntityMeta;
            ////var storageMeta = RF.Find<LabelStorage>().EntityMeta;
            ////var state = item.LabelStorageList.FirstOrDefault(f => f.LocationId == workcell.StorageLocationId).State;

            ////var sql = @"SELECT T0.ID
            ////                  FROM [PKG_RELATION] T0
            ////                 WHERE EXISTS(SELECT 1
            ////                          FROM [ITEM_LABEL] T1,[PKG_RELATION] Y1
            ////                         WHERE Y1.[ROOT_ID] = T0.ID
            ////                           AND T1.[ITEM_ID] = {0}
            ////                           AND Y1.INV_ORG_ID = {3}
            ////                           AND Y1.IS_PHANTOM = 0
            ////                           AND T1.[RELATION_ID] = Y1.ID
            ////                           AND EXISTS(SELECT 1
            ////                                  FROM [ITEM_LABEL_STORAGE] T2
            ////                                 WHERE T2.[LABEL_ID] = T1.ID
            ////                                   AND T2.[LOCATION_ID] = {1}
            ////                                   AND T2.[ONHAND] > 0
            ////                                   AND T2.[STATE] = {2}
            ////                                   AND T2.INV_ORG_ID = {3}
            ////                                   AND T2.IS_PHANTOM = 0)
            ////                           AND T1.INV_ORG_ID = {3}
            ////                           AND T1.IS_PHANTOM = 0)
            ////                   AND T0.TREE_PID IS NULL
            ////                   AND T0.[PACKAGE_NO] IS NOT NULL
            ////                   AND T0.[PACKAGE_UNIT_ID] = {4}
            ////                   AND T0.INV_ORG_ID = {3}
            ////                   AND T0.IS_PHANTOM = 0
            ////                 ORDER BY T0.ID ASC"
            ////.Replace("[PKG_RELATION]", pkgMeta.TableMeta.TableName)
            ////.Replace("[ITEM_LABEL]", labelMeta.TableMeta.TableName)
            ////.Replace("[ITEM_LABEL_STORAGE]", storageMeta.TableMeta.TableName)
            ////.Replace("[ROOT_ID]", pkgMeta.Property(PackingRelation.RootIdProperty).ColumnMeta.ColumnName)
            ////.Replace("[PACKAGE_NO]", pkgMeta.Property(PackingRelation.PackageNoProperty).ColumnMeta.ColumnName)
            ////.Replace("[PACKAGE_UNIT_ID]", pkgMeta.Property(PackingRelation.PackageUnitIdProperty).ColumnMeta.ColumnName)
            ////.Replace("[ITEM_ID]", labelMeta.Property(ItemLabel.ItemIdProperty).ColumnMeta.ColumnName)
            ////.Replace("[RELATION_ID]", labelMeta.Property(ItemLabel.RelationIdProperty).ColumnMeta.ColumnName)
            ////.Replace("[LABEL_ID]", storageMeta.Property(LabelStorage.LabelIdProperty).ColumnMeta.ColumnName)
            ////.Replace("[LOCATION_ID]", storageMeta.Property(LabelStorage.LocationIdProperty).ColumnMeta.ColumnName)
            ////.Replace("[ONHAND]", storageMeta.Property(LabelStorage.OnhandProperty).ColumnMeta.ColumnName)
            ////.Replace("[STATE]", storageMeta.Property(LabelStorage.StateProperty).ColumnMeta.ColumnName);
            ////return string.Format(sql, item.ItemId, workcell.StorageLocationId, (int)state, AppRuntime.InvOrg, outerPackingRelation.PackageUnitId);
        }

        ///// <summary>
        ///// 为级联打包创建sql语句
        ///// </summary>
        ///// <param name="outerPackingRelation">包装关系</param>
        ///// <param name="item">物料</param>
        ///// <param name="workcell">工作单元</param>
        ///// <returns>sql语句</returns>
        ////private string GenerateQuerySqlForCasecadPackage(PackingRelation outerPackingRelation, ItemLabel item, Workcell workcell)
        ////{
        ////    var pkgMeta = RF.Find<PackingRelation>().EntityMeta;
        ////    var labelMeta = RF.Find<ItemLabel>().EntityMeta;
        ////    var storageMeta = RF.Find<LabelStorage>().EntityMeta;
        ////    var state = item.LabelStorageList.FirstOrDefault(f => f.LocationId == workcell.StorageLocationId).State;

        ////    var sql = @"SELECT T0.ID
        ////                      FROM [PKG_RELATION] T0
        ////                     WHERE EXISTS(SELECT 1
        ////                              FROM [ITEM_LABEL] T1,[PKG_RELATION] Y1
        ////                             WHERE Y1.[ROOT_ID] = T0.ID
        ////                               AND T1.[ITEM_ID] = {0}
        ////                               AND Y1.INV_ORG_ID = {3}
        ////                               AND Y1.IS_PHANTOM = 0
        ////                               AND T1.[RELATION_ID] = Y1.ID
        ////                               AND EXISTS(SELECT 1
        ////                                      FROM [ITEM_LABEL_STORAGE] T2
        ////                                     WHERE T2.[LABEL_ID] = T1.ID
        ////                                       AND T2.[LOCATION_ID] = {1}
        ////                                       AND T2.[ONHAND] > 0
        ////                                       AND T2.[STATE] = {2}
        ////                                       AND T2.INV_ORG_ID = {3}
        ////                                       AND T2.IS_PHANTOM = 0)
        ////                               AND T1.INV_ORG_ID = {3}
        ////                               AND T1.IS_PHANTOM = 0)
        ////                       AND T0.TREE_PID IS NULL
        ////                       AND T0.[PACKAGE_NO] IS NOT NULL
        ////                       AND T0.[PACKAGE_UNIT_ID] = {4}
        ////                       AND T0.INV_ORG_ID = {3}
        ////                       AND T0.IS_PHANTOM = 0
        ////                     ORDER BY T0.ID ASC"
        ////    .Replace("[PKG_RELATION]", pkgMeta.TableMeta.TableName)
        ////    .Replace("[ITEM_LABEL]", labelMeta.TableMeta.TableName)
        ////    .Replace("[ITEM_LABEL_STORAGE]", storageMeta.TableMeta.TableName)
        ////    .Replace("[ROOT_ID]", pkgMeta.Property(PackingRelation.RootIdProperty).ColumnMeta.ColumnName)
        ////    .Replace("[PACKAGE_NO]", pkgMeta.Property(PackingRelation.PackageNoProperty).ColumnMeta.ColumnName)
        ////    .Replace("[PACKAGE_UNIT_ID]", pkgMeta.Property(PackingRelation.PackageUnitIdProperty).ColumnMeta.ColumnName)
        ////    .Replace("[ITEM_ID]", labelMeta.Property(ItemLabel.ItemIdProperty).ColumnMeta.ColumnName)
        ////    .Replace("[RELATION_ID]", labelMeta.Property(ItemLabel.RelationIdProperty).ColumnMeta.ColumnName)
        ////    .Replace("[LABEL_ID]", storageMeta.Property(LabelStorage.LabelIdProperty).ColumnMeta.ColumnName)
        ////    .Replace("[LOCATION_ID]", storageMeta.Property(LabelStorage.LocationIdProperty).ColumnMeta.ColumnName)
        ////    .Replace("[ONHAND]", storageMeta.Property(LabelStorage.OnhandProperty).ColumnMeta.ColumnName)
        ////    .Replace("[STATE]", storageMeta.Property(LabelStorage.StateProperty).ColumnMeta.ColumnName);
        ////    return string.Format(sql, item.ItemId, workcell.StorageLocationId, (int)state, AppRuntime.InvOrg, outerPackingRelation.PackageUnitId);
        ////}

        /// <summary>
        /// 检查用于是否有仓库权限
        /// </summary>
        /// <param name="workcell">工作单元</param>
        protected virtual void CheckUserHasWarehouse(Workcell workcell)
        {
            if (workcell == null) throw new ArgumentNullException(nameof(workcell));
            ////var ctlResource = RT.Service.Resolve<WarehouseController>();
            //// 删除了仓库人员关系实体
            ////if (!ctlResource.UserHasWarehouse(workcell.UserId.Value, workcell.WarehouseId.Value))
            ////    throw new ValidationException("当前人员不属于仓库".L10N());
        }

        /// <summary>
        /// 检查仓库是否有本地货区
        /// </summary>
        /// <param name="workcell">工作单元</param>
        protected virtual void CheckWarehouseHasLocalStorageLocation(Workcell workcell)
        {
            if (workcell == null) throw new ArgumentNullException(nameof(workcell));

            if (GetById<StorageLocation>(workcell.StorageLocationId)?.Area?.WarehouseId != GetById<Warehouse>(workcell.WarehouseId)?.Id)
                throw new ValidationException("货位不存在".L10N());
        }

        /// <summary>
        /// 验证是否同一个产品
        /// </summary>
        /// <param name="outerPackingRelation">输出包装关系</param>
        /// <param name="innerPackingRelation">输入包装关系</param>
        protected virtual void VaildationSameProduct(PackingRelation outerPackingRelation, PackingRelation innerPackingRelation)
        {
            if (outerPackingRelation == null) throw new ArgumentNullException(nameof(outerPackingRelation));
            if (innerPackingRelation == null) throw new ArgumentNullException(nameof(innerPackingRelation));
            ////var ids = new double[] { outerPackingRelation.Id, innerPackingRelation.Id };
            if (GetItemLabel(outerPackingRelation).Item.Id != GetItemLabel(innerPackingRelation).Item.Id)
                throw new ValidationException("采集失败,物料不一致".L10N());
        }

        /// <summary>
        /// 验证物流状态是否相同
        /// </summary>
        /// <param name="outerPackingRelation">输出包装关系</param>
        /// <param name="innerPackingRelation">输入包装关系</param>
        protected virtual void VaildationStaticLogisticState(PackingRelation outerPackingRelation, PackingRelation innerPackingRelation)
        {
            if (innerPackingRelation == null) throw new ArgumentNullException(nameof(innerPackingRelation));
            if (outerPackingRelation == null) throw new ArgumentNullException(nameof(outerPackingRelation));
            if (innerPackingRelation.State != outerPackingRelation.State)
                throw new ValidationException("采集失败,状态不一致".L10N());
        }

        /// <summary>
        /// 是否顶层包装
        /// </summary>
        /// <param name="unitId">单位ID</param>
        /// <param name="rule">规则</param>
        /// <returns>bool</returns>
        [IgnoreProxy]
        public virtual bool IsTopPackageRule(double unitId, PackageRule rule)
        {
            if (unitId <= 0)
            {
                throw new ArgumentException(nameof(unitId), "{0} = {1}".FormatArgs(nameof(unitId), unitId));
            }

            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }

            return rule.PackageRuleDetailList.LastOrDefault().PackageUnitId == unitId;
        }
    }
}