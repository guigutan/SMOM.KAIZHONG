using SIE.Common.Menus;
using SIE.EMS.AssetDisposals;
using SIE.EMS.AssetIssues;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.AssetReturns;
using SIE.EMS.AssetScraps;
using SIE.EMS.AssetTransfers;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Checks.Records;
using SIE.EMS.DevicePurs;
using SIE.EMS.Devices.Abnormals;
using SIE.EMS.EquipLends;
using SIE.EMS.Equipments.AlarmStates;
using SIE.EMS.Equipments.Boms;
using SIE.EMS.Equipments.RunningStates;
using SIE.EMS.Faults;
using SIE.EMS.FixedAssets.Accounts;
using SIE.EMS.IdleArchives;
using SIE.EMS.InspectionRules;
using SIE.EMS.InventoryBalances;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks;
using SIE.EMS.Lubrications;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.RunStandards;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.Tpms;
using SIE.EMS.ViceTransfers;
using System.Collections.Generic;

namespace SIE.Web.EMS.Menus
{
    /// <summary>
    /// 菜单初始化
    /// </summary>
    public class EMSMenuInit : IWebMenuInit
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <returns></returns>
        public List<MenuDto> GetMenuDtos()
        {
            var res = new List<MenuDto>();

            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备管理",
                Label = "设备与人员权限维护",
                EntityType = typeof(DevicePur)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备管理",
                Label = "设备BOM",
                EntityType = typeof(EquipBom)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备管理",
                Label = "检验规程",
                EntityType = typeof(InspectionRule)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备管理",
                Label = "故障类别",
                EntityType = typeof(EquipLargeFault)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备管理",
                Label = "设备异常维护",
                EntityType = typeof(DeviceAbnormal)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备管理",
                Label = "TPM检查评分项",
                EntityType = typeof(TpmWeekInspectScore)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备管理",
                Label = "TPM记录",
                EntityType = typeof(TpmRecord)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "固定资产台账",
                EntityType = typeof(FixedAssetsAccount)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "资产领用",
                EntityType = typeof(AssetRequisition)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "资产调拨",
                EntityType = typeof(AssetTransfer)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "副资产调拨",
                EntityType = typeof(ViceTransfer)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "资产发放",
                EntityType = typeof(AssetIssue)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "资产归还",
                EntityType = typeof(AssetReturn)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "闲置封存",
                EntityType = typeof(IdleArchive)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "资产报废",
                EntityType = typeof(AssetScrap)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "资产处置",
                EntityType = typeof(AssetDisposal)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "盘点计划",
                EntityType = typeof(InventoryPlan)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "盘点任务",
                EntityType = typeof(InventoryTask)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "盘点平账",
                EntityType = typeof(InventoryBalance)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.资产管理",
                Label = "设备借还管理",
                EntityType = typeof(EquipLendManage)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "EDO",
                Label = "运行管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "点检保养项目维护",
                EntityType = typeof(ProjectDetail)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "点检计划维护",
                EntityType = typeof(CheckPlanViewModel)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "设备点检记录",
                EntityType = typeof(CheckRecord)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "设备运行状态记录",
                EntityType = typeof(EquipRunningStateRecord)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "润滑记录",
                EntityType = typeof(Lubrication)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.运行管理",
                Label = "设备运行定标管理",
                EntityType = typeof(RunStandard)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "EDO",
                Label = "备件管理",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.备件管理",
                Label = "备件基础数据",
                EntityType = typeof(SparePart)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.备件管理",
                Label = "备件入库",
                EntityType = typeof(SparePartStore)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.备件管理",
                Label = "备件库存查询",
                EntityType = typeof(StoreSummary)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.备件管理",
                Label = "备件申请单",
                EntityType = typeof(SparePartApp)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.备件管理",
                Label = "备件出库单",
                EntityType = typeof(OutDepot)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.备件管理",
                Label = "备件交接单",
                EntityType = typeof(OutDepotHandover)
            });

            res.Add(new MenuDto()
            {
                TreeKey = "EDO",
                Label = "设备报表",
                IsLeafNode = false,
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备报表",
                Label = "报警统计",
                EntityType = typeof(AlarmCount)
            });
            res.Add(new MenuDto()
            {
                TreeKey = "EDO.设备报表",
                Label = "报警明细",
                EntityType = typeof(EquipAlarmRecord)
            });

            return res;
        }

    }
}
