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
using SIE.EMS.Equipments.Units;
using SIE.EMS.Faults;
using SIE.EMS.FixedAssets.Accounts;
using SIE.EMS.IdleArchives;
using SIE.EMS.InspectionRules;
using SIE.EMS.InventoryBalances;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks;
using SIE.EMS.Logs;
using SIE.EMS.Lubrications;
using SIE.EMS.MainenanceProjects;
using SIE.EMS.RunStandards;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.Tpms;
using SIE.EMS.ViceTransfers;
using SIE.MetaModel;
using SIE.Modules;
using SIE.Web.EMS;
using SIE.Web.EMS.InventoryBalances;
using System;

[assembly: Module(typeof(Module))]
namespace SIE.Web.EMS
{
    /// <summary>
    /// 模块配置
    /// </summary>
    public class Module : UIModule
    {
        /// <summary>
        /// 初始化模块
        /// </summary>
        /// <param name="app">应用程序</param>
        public override void Initialize(IApp app)
        {
            app.ModuleOperations += App_ModuleOperations;
        }

        /// <summary>
        /// 模块定义
        /// </summary>
        /// <param name="sender">应用程序</param>
        /// <param name="e">参数</param>
        private void App_ModuleOperations(object sender, EventArgs e)
        {
            CommonModel.Modules.AddModules(new WebModuleMeta()
            {
                EntityType = typeof(EquipUnit),
                Label = "设备单元维护"
            }, new WebModuleMeta()
            {
                EntityType = typeof(ProjectDetail),
                Label = "点检保养项目维护"
            }, new WebModuleMeta()
            {
                EntityType = typeof(EquipLargeFault),
                Label = "故障类别"
            },  new WebModuleMeta()
            {
                EntityType = typeof(TpmWeekInspectScore),
                Label = "TPM检查评分项"
            }, new WebModuleMeta()
            {
                EntityType = typeof(CheckPlanViewModel),
                Label = "点检计划维护",
            }, new WebModuleMeta()
            {
                EntityType = typeof(CheckRecord),
                Label = "设备点检记录"
            }, new WebModuleMeta()
            {
                EntityType = typeof(TpmRecord),
                Label = "TPM记录"
            }, new WebModuleMeta()
            {
                EntityType = typeof(DevicePur),
                Label = "设备与人员权限维护"
            }, new WebModuleMeta()
            {
                EntityType = typeof(SparePart),
                Label = "备件基础数据"
            }, new WebModuleMeta()
            {
                EntityType = typeof(DeviceAbnormal),
                Label = "设备异常维护"
            }, new WebModuleMeta()
            {
                EntityType = typeof(EquipBom),
                Label = "设备BOM"
            }, new WebModuleMeta()
            {
                EntityType = typeof(SparePartStore),
                Label = "备件入库"
            }, new WebModuleMeta()
            {
                EntityType = typeof(StoreSummary),
                Label = "备件库存查询"
            }, new WebModuleMeta()
            {
                EntityType = typeof(SparePartApp),
                Label = "备件申请单"
            }, new WebModuleMeta()
            {
                EntityType = typeof(OutDepot),
                Label = "备件出库单"
            }, new WebModuleMeta()
            {
                EntityType = typeof(OutDepotHandover),
                Label = "备件交接单"
            }, new WebModuleMeta()
            {
                EntityType = typeof(EquipRunningStateRecord),
                Label = "设备运行状态记录"
            }, new WebModuleMeta()
            {
                EntityType = typeof(AlarmCount),
                Label = "报警统计"
            }, new WebModuleMeta()
            {
                EntityType = typeof(EquipAlarmRecord),
                Label = "报警明细",
                UIGenerator = "SIE.Web.EMS.Equipments.AlarmStates.Layouts.AlarmDetailUIGenerator"

            }, new WebModuleMeta()
            {
                EntityType = typeof(FixedAssetsAccount),
                Label = "固定资产台账"
            }, new WebModuleMeta()
            {
                EntityType = typeof(Lubrication),
                Label = "润滑记录"
            }, new WebModuleMeta()
            {
                EntityType = typeof(AssetRequisition),
                Label = "资产领用"
            }, new WebModuleMeta()
            {
                EntityType = typeof(AssetTransfer),
                Label = "资产调拨"
            }, new WebModuleMeta()
            {
                EntityType = typeof(ViceTransfer),
                Label = "副资产调拨"
            }, new WebModuleMeta()
            {
                EntityType = typeof(InspectionRule),
                Label = "检验规程"
            }, new WebModuleMeta()
            {
                EntityType = typeof(InventoryPlan),
                Label = "盘点计划"
            }, new WebModuleMeta()
            {
                EntityType = typeof(InventoryTask),
                Label = "盘点任务"
            }, new WebModuleMeta()
            {
                EntityType = typeof(InventoryBalance),
                Label = "盘点平账",
                ViewGroup = InventoryBalanceViewConfig.BalanceView
            }, new WebModuleMeta()
            {
                EntityType = typeof(IdleArchive),
                Label = "闲置封存"
            }
            , new WebModuleMeta()
            {
                EntityType = typeof(RunStandard),
                Label = "设备运行定标管理"
            }, new WebModuleMeta()
            {
                EntityType = typeof(AssetIssue),
                Label = "资产发放"
            }, new WebModuleMeta()
            {
                EntityType = typeof(AssetReturn),
                Label = "资产归还"
            }, new WebModuleMeta()
            {
                EntityType = typeof(AssetScrap),
                Label = "资产报废"
            }, new WebModuleMeta()
            {
                EntityType = typeof(AssetDisposal),
                Label = "资产处置"
            }, new WebModuleMeta()
            {
                EntityType = typeof(EdoOutlineUploadLog),
                Label = "离线数据上传记录"
            }, new WebModuleMeta()
            {
                EntityType = typeof(EquipLendManage),
                Label = "设备借还管理"
            }
            );
        }
    }
}