using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Packages;
using SIE.Packages.Packages;
using SIE.Tech.Processs;
using System;
using System.Linq;

namespace SIE.MES.WorkOrders.WorkOrderPackageGenerators
{
    /// <summary>
    /// 工单包装规则验证器
    /// </summary>
    public static class WorkOrderPackageRuleValidator
    {
        /// <summary>
        /// 验证工单的包装规则
        /// </summary>
        /// <param name="workOrder">工单</param>
        /// <param name="packingUnits">包装单位列表</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ValidationException"></exception>
        public static void Validate(WorkOrder workOrder, EntityList<PackingUnit> packingUnits)
        {
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (workOrder.RoutingProcessList != null && workOrder.RoutingProcessList.Any()
                && workOrder.RoutingProcessList.Any(p => p.ProcessType == ProcessType.Packing || p.ProcessType == ProcessType.BatchPacking)
                && !workOrder.PackageRuleDetailList.Any())
            {
                throw new ValidationException("工单工艺路线存在包装工序，必须维护包装规则".L10N());
            }

            if (!workOrder.PackageRuleDetailList.Any())
            {
                return;
            }

            var firstPackageRuleDetail = workOrder.PackageRuleDetailList.OrderBy(x => x.GetIndex()).First();

            var firtPackUnit = packingUnits.FirstOrDefault(x => x.Id == firstPackageRuleDetail.PackageUnitId);
            if (firtPackUnit != null && !firtPackUnit.IsMasterUnit)
            {
                throw new ValidationException("主单位必须是第一个".L10N());
            }

            if (workOrder.PackageRuleDetailList.Count == 1)
            {
                throw new ValidationException("包装必须包含两个以上包装单位".L10N());
            }

            for (int i = 1; i < workOrder.PackageRuleDetailList.Count(); i++)
            {
                if (workOrder.PackageRuleDetailList[i].Qty < workOrder.PackageRuleDetailList[i - 1].Qty)
                {
                    throw new ValidationException("外包装数量不能小于内包装数量".L10N());
                }
            }

            //包装单位非主单位时，编码规则必须要有
            foreach (var packageRuleDetail in workOrder.PackageRuleDetailList)
            {
                if (packageRuleDetail.Qty <= 0)
                {
                    throw new ValidationException("产品数必须大于0".L10N());
                }

                if (packageRuleDetail.LevelQty <= 0)
                {
                    throw new ValidationException("包装数必须大于0".L10N());
                }

                PackingUnit packingUnit = null;
                if (packageRuleDetail.PackageUnitId != default)
                {
                    packingUnit = packingUnits.FirstOrDefault(x => x.Id == packageRuleDetail.PackageUnitId);

                    //包装单位非主单位时
                    //编码规则必须要有
                    //需打印的包装单位，包装规则必须要设置打印模板
                    if (packingUnit != null && !packingUnit.IsMasterUnit)
                    {
                        if (packageRuleDetail.NumberRuleId == null)
                        {
                            throw new ValidationException("包装单位[{0}]条码规则不能为空".L10nFormat(packingUnit.Code));
                        }

                        if (packageRuleDetail.IsPrint && packageRuleDetail.PrintTemplateId == null)
                        {
                            throw new ValidationException("包装单位[{0}]需要打印，必须设置打印模板".L10nFormat(packingUnit.Code));
                        }
                    }
                }

                if (workOrder.PackageRuleDetailList.Any(x => x.PackageUnitId == packageRuleDetail.PackageUnitId
                    && x.GetIndex() != packageRuleDetail.GetIndex()))
                {
                    if (packingUnit != null)
                    {
                        throw new ValidationException("工单已经存在包装单位[{0}]".L10nFormat(packingUnit.Code));
                    }
                    else
                    {
                        throw new ValidationException("工单存在相同的包装单位".L10N());
                    }
                }
            }
        }

        /// <summary>
        /// 验证工单的包装规则
        /// </summary>
        /// <param name="workOrder"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Validate(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                throw new ArgumentNullException(nameof(workOrder));
            }

            if (!workOrder.PackageRuleDetailList.Any())
            {
                return;
            }

            //获取包装单位
            var packageUnitIds = workOrder.PackageRuleDetailList.Select(x => x.PackageUnitId).Distinct().ToList();
            var packingUnits = AppRuntime.Service.Resolve<PackageController>().GetPackingUnits(packageUnitIds);

            Validate(workOrder, packingUnits);
        }
    }
}
