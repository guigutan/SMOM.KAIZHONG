using SIE.Domain;
using SIE.Packages;
using SIE.Packages.Packages;
using SIE.Tech.Processs;
using System.Linq;

namespace SIE.MES.WorkOrders.WorkOrderPackageGenerators
{
    /// <summary>
    /// 工单工序与包装单位的关系生成器
    /// </summary>
    public class WoProcessPackingUnitGenerator
    {
        /// <summary>
        /// 工序列表
        /// </summary>
        private readonly EntityList<Process> processs;

        /// <summary>
        /// 包装单位
        /// </summary>
        private readonly EntityList<PackingUnit> packingUnits;

        /// <summary>
        /// 工序对应包装单位
        /// </summary>
        private readonly EntityList<ProcessPackingUnit> processPackingUnitList;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="_processs"></param>
        public WoProcessPackingUnitGenerator(EntityList<Process> _processs)
        {
            processs = _processs;
            var processIds = processs.Select(x => x.Id).Distinct().ToList();
            processPackingUnitList = RT.Service.Resolve<ProcessController>()
                .GetProcessPackingUnits(processIds);

            var packageUnitIds = processPackingUnitList.Select(x => x.PackageUnitId).Distinct().ToList();

            if (packageUnitIds.Any())
            {
                packingUnits = RT.Service.Resolve<PackageController>().GetPackingUnits(packageUnitIds);
            }
            else
            {
                packingUnits = new EntityList<PackingUnit>();
            }
        }

        /// <summary>
        /// 生成工单工序与包装单位的关系
        /// </summary>
        /// <param name="workOrder">工单</param>
        public void GenerateWorkOrderProcessPackingUnit(WorkOrder workOrder)
        {
            if (workOrder == null)
            {
                return;
            }

            if (!workOrder.RoutingProcessList.Any(f => f.ProcessType == ProcessType.Packing
                || f.ProcessType == ProcessType.BatchPacking))
            {
                return;
            }

            foreach (var packageRuleDetail in workOrder.PackageRuleDetailList)
            {
                var packageUnit = packingUnits.FirstOrDefault(x => x.Id == packageRuleDetail.PackageUnitId);

                if (packageUnit == null || packageUnit.IsMasterUnit)
                {
                    continue;
                }

                packageRuleDetail.WorkOrderProcessPackingUnitList.Clear();

                foreach (var processId in workOrder.RoutingProcessList
                    .Where(f => f.ProcessType == ProcessType.Packing || f.ProcessType == ProcessType.BatchPacking)
                    .Select(x => x.ProcessId))
                {
                    if (!processId.HasValue)
                    {
                        continue;
                    }

                    var procss = GetProcess(processId.Value);
                    if (procss == null ||
                        !PackingUnitHasProcess(packageRuleDetail.PackageUnitId, processId.Value))
                    {
                        continue;
                    }

                    var ruleUnit = new WorkOrderProcessPackingUnit
                    {
                        ProcessId = procss.Id,
                        //此时包装规则明细的ID还没有生成
                        PackageRule = packageRuleDetail
                    };

                    ruleUnit.ExtValues["ProcessId_Display"] = procss.Name;
                    //改成后面批量生成Id
                    //<code>ruleUnit.GenerateId();</code>
                    packageRuleDetail.WorkOrderProcessPackingUnitList.Add(ruleUnit);
                }
            }
        }

        /// <summary>
        /// 判断工序是否有绑定包装单位
        /// </summary>
        /// <param name="packingUnitId">包装单位ID</param>
        /// <param name="processId">工序Id</param>
        /// <returns>bool</returns>
        private bool PackingUnitHasProcess(double packingUnitId, double processId)
        {
            return processPackingUnitList.Any(f => f.PackageUnitId == packingUnitId && f.ProcessId == processId);
        }


        /// <summary>
        /// 获取工序
        /// </summary>
        /// <param name="processId"></param>
        /// <returns></returns>
        private Process GetProcess(double processId)
        {
            return processs.FirstOrDefault(x => x.Id == processId);
        }
    }
}
