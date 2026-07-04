using NPOI.SS.Formula.Functions;
using SIE.Common.Catalogs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.EMS.AssetDisposals;
using SIE.EMS.AssetIssues;
using SIE.EMS.AssetRequisitions;
using SIE.EMS.AssetReturns;
using SIE.EMS.AssetScraps;
using SIE.EMS.AssetTransfers;
using SIE.EMS.Checks.Configs;
using SIE.EMS.Checks.Plans.ViewModels;
using SIE.EMS.Common.Configs;
using SIE.EMS.FixedAssets.Accounts;
using SIE.EMS.FixedAssets.Accounts.Config;
using SIE.EMS.IdleArchives;
using SIE.EMS.InventoryPlans;
using SIE.EMS.InventoryTasks;
using SIE.EMS.Lubrications;
using SIE.EMS.Lubrications.Configs;
using SIE.EMS.Maintains.Configs;
using SIE.EMS.Maintains.Plans.ViewModels;
using SIE.EMS.RunStandards;
using SIE.EMS.SpareParts;
using SIE.EMS.SpareParts.Applys;
using SIE.EMS.SpareParts.Configs;
using SIE.EMS.SpareParts.OutDepotHandovers;
using SIE.EMS.SpareParts.OutDepots;
using SIE.EMS.ViceTransfers;
using SIE.Equipments.Configs;
using System;
using System.Linq;

namespace SIE.EMS.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20200106_000000_InitEMSData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return EmsEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加快码组初始数据。".L10N(); }
        }

        /// <summary>
        /// 手动升级的类型：数据
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 数据库回滚
        /// </summary>
        protected override void Down() { }

        /// <summary>
        /// 数据库升级
        /// </summary>
        protected override void Up()
        {
            this.RunCode(db =>
            {
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }

                EntityList<CatalogType> catalogTypeList = RT.Service.Resolve<CatalogController>().GetCatalogTypeList();

                if (catalogTypeList.FirstOrDefault(p => p.Code == InventoryPlan.InventoryTypeCatalog) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = InventoryPlan.InventoryTypeCatalog,
                        Name = "盘点类型".L10N(),
                        Description = "盘点类型".L10N()
                    };
                    catalogType.CatalogList.Add(new Catalog() { Code = "年终".L10N(), Name = "年终".L10N(), Description = "年终".L10N() });
                    RF.Save(catalogType);
                }

                if (catalogTypeList.FirstOrDefault(p => p.Code == AssetScrapEquipment.EquipScrapType) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = AssetScrapEquipment.EquipScrapType,
                        Name = "报废类型".L10N(),
                        Description = "报废类型".L10N()
                    };
                    catalogType.CatalogList.Add(new Catalog() { Code = "报废类型A".L10N(), Name = "报废类型A".L10N(), Description = "报废类型A".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "报废类型B".L10N(), Name = "报废类型B".L10N(), Description = "报废类型B".L10N() });
                    RF.Save(catalogType);
                }

                //初始化 备件入库单号编码规则
                InitSparePartStoreNoConfig();

                //初始化 备件入库是否计算平均成本为是
                InitIsComputeAvgCostConfigValue();

                //初始化 备件库存查询备件批次号和序列号编码规则
                InitSparePartBatchAndSnNoConfig();

                //初始化 备件申请单号编码规则
                InitSparePartApplyNoConfig();

                //初始化 备件出库单号编码规则
                InitOutDepotNoConfig();

                //初始化 备件交接单号编码规则
                InitOutDepotHandoverNoConfig();

                //全局配置项:是否生成备件交接单配置 初始值:是
                InitIsCreateHandoverBillConfig();

                //全局配置项:备件是否启用WMS管控配置 初始值:否
                InitIsWmsControlConfig();

                //初始化 设备运行定标管理单号编码规则
                InitRunStandardNoConfig();

                //初始化 保养计划维护单号编码规则
                InitMaintainPlanViewModelNoConfig();

                //初始化 点检计划维护单号编码规则
                InitCheckPlanViewModelNoConfig();

                //初始化 润滑记录单号编码规则
                InitLubricationNoConfig();

                //初始化 润滑记录计划规则
                InitLubricationPlanTypeConfig();

                //初始化 固定资产台账编码规则与折旧方式
                InitFixedAssetsAccountAndDepreciationWayNoConfig();

                //初始化 资产领用单号编码规则
                InitAssetRequisitionNoConfig();

                //初始化 资产调拨单号编码规则
                InitAssetTransferNoConfig();

                //初始化 副资产调拨单号编码规则
                InitViceTransferNoConfig();

                //初始化 资产发放单号编码规则
                InitAssetIssueNoConfig();

                //初始化 盘点计划单号编码规则
                InitInventoryPlanNoConfig();

                //初始化 闲置封存单号编码规则
                InitIdleArchiveNoConfig();

                //初始化 盘点任务单号编码规则
                InitInventoryTaskNoConfig();

                //初始化 资产归还单号编码规则
                InitAssetReturnNoConfig();

                //初始化 资产报废单号编码规则
                InitAssetScrapNoConfig();

                //初始化 资产处置单号编码规则
                InitAssetDisposalNoConfig();
            });
        }

        /// <summary>
        /// 设置备件入库的是否计算平均成本为 是
        /// </summary>
        private void InitIsComputeAvgCostConfigValue()
        {
            IsComputeAvgCostConfigValue isComputeAvgCostConfigValue = new IsComputeAvgCostConfigValue();
            isComputeAvgCostConfigValue.IsComputeAvgCost = true;

            RT.Service.Resolve<ConfigExtController>().InitModuleConfig<SparePartStore, IsComputeAvgCostConfig, IsComputeAvgCostConfigValue>(isComputeAvgCostConfigValue);
        }

        //全局配置项:是否生成备件交接单配置 初始值:是
        private void InitIsCreateHandoverBillConfig()
        {
            IsCreateHandoverBillConfigValue configValue = new IsCreateHandoverBillConfigValue
            {
                IsCreateHandoverBill = true
            };
            RT.Service.Resolve<ConfigExtController>().InitGlobalConfig<IsCreateHandoverBillConfig, IsCreateHandoverBillConfigValue>(configValue);
        }

        //全局配置项:备件是否启用WMS管控配置 初始值:否
        private void InitIsWmsControlConfig()
        {
            IsWmsControlConfigValue configValue = new IsWmsControlConfigValue
            {
                IsWmsControl = YesNo.No
            };
            RT.Service.Resolve<ConfigExtController>().InitGlobalConfig<IsWmsControlConfig, IsWmsControlConfigValue>(configValue);
        }

        /// <summary>
        /// 初始化 备件入库单号编码规则
        /// </summary>
        private void InitSparePartStoreNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("备件入库单号编码规则", "BR", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                SparePartStoreNoConfigValue configValue = new SparePartStoreNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<SparePartStore, SparePartStoreNoConfig, SparePartStoreNoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 备件批次号和序列号编码规则
        /// </summary>
        private void InitSparePartBatchAndSnNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var batchNumberRule = numberExtCtl.CreateFormNoNumberRule("备件批次号编码规则", "BL", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            var snNumberRule = numberExtCtl.CreateFormNoNumberRule("备件序列号编码规则", "BP", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (batchNumberRule != null && batchNumberRule.Id != default && snNumberRule != null && snNumberRule.Id != default)
            {
                BatchNumberConfigValue configValue = new BatchNumberConfigValue
                {
                    BatchNumberRuleId = batchNumberRule.Id,
                    SnNumberRuleId = snNumberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<StoreSummary, BatchNumberNoConfig, BatchNumberConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 备件申请单号编码规则
        /// </summary>
        private void InitSparePartApplyNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("备件申请单号编码规则", "BS", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<SparePartApp, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 备件出库单号编码规则
        /// </summary>
        private void InitOutDepotNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("备件出库单号编码规则", "SP", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<OutDepot, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 备件交接单号编码规则
        /// </summary>
        private void InitOutDepotHandoverNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("备件交接单号编码规则", "SPH", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<OutDepotHandover, NoConfig, NoConfigValue>(configValue);
            }
        }


        /// <summary>
        /// 初始化 设备运行定标管理单号编码规则
        /// </summary>
        private void InitRunStandardNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("设备运行定标管理单号编码规则", "SD", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<RunStandard, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 保养计划单号编码规则
        /// </summary>
        private void InitMaintainPlanViewModelNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("保养计划维护单号编码规则", "TPM", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                MaintainPlanNoConfigValue configValue = new MaintainPlanNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<MaintainPlanViewModel, MaintainPlanNoConfig, MaintainPlanNoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 点检计划维护单号编码规则
        /// </summary>
        private void InitCheckPlanViewModelNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("点检计划维护单号编码规则", "DJ", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                CheckPlanNoConfigValue configValue = new CheckPlanNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<CheckPlanViewModel, CheckPlanNoConfig, CheckPlanNoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 润滑记录单号编码规则
        /// </summary>
        private void InitLubricationNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("润滑记录单号编码规则", "RH", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<Lubrication, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 润滑记录计划规则
        /// </summary>
        private void InitLubricationPlanTypeConfig()
        {
            //计划规则
            PlanTypeConfigValue configValue = new PlanTypeConfigValue
            {
                PlanType = Enums.PlanType.BaseDate
            };
            RT.Service.Resolve<ConfigExtController>().InitModuleConfig<Lubrication, PlanTypeConfig, PlanTypeConfigValue>(configValue);
        }

        /// <summary>
        /// 初始化 固定资产台账编码规则与折旧方式
        /// </summary>
        private void InitFixedAssetsAccountAndDepreciationWayNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("固定资产台账单号编码规则", "EA", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                FixedAssetAccountConfigValue configValue = new FixedAssetAccountConfigValue
                {
                    NumberRuleId = numberRule.Id,
                    DepreciationWay = DepreciationWay.AverageAge
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<FixedAssetsAccount, FixedAssetAccountConfig, FixedAssetAccountConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 资产领用单号编码规则
        /// </summary>
        private void InitAssetRequisitionNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("资产领用单号编码规则", "EY", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<AssetRequisition, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 资产调拨单号编码规则
        /// </summary>
        private void InitAssetTransferNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("资产调拨单号编码规则", "ED", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<AssetTransfer, NoConfig, NoConfigValue>(configValue);
            }
        }


        /// <summary>
        /// 初始化 副资产调拨单号编码规则
        /// </summary>
        private void InitViceTransferNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("副资产调拨单号编码规则", "FED", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<ViceTransfer, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 资产发放单号编码规则
        /// </summary>
        private void InitAssetIssueNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("资产发放单号编码规则", "EF", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<AssetIssue, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 盘点计划单号编码规则
        /// </summary>
        private void InitInventoryPlanNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("盘点计划单号编码规则", "PP", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<InventoryPlan, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 闲置封存单号编码规则
        /// </summary>
        private void InitIdleArchiveNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("闲置封存单号编码规则", "ZF", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<IdleArchive, NoConfig, NoConfigValue>(configValue);
            }
        }


        /// <summary>
        /// 初始化 盘点任务单号编码规则
        /// </summary>
        private void InitInventoryTaskNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("盘点任务单号编码规则", "PPN", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<InventoryTask, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 资产归还单号编码规则
        /// </summary>
        private void InitAssetReturnNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("资产归还单号编码规则", "EG", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<AssetReturn, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 资产报废单号编码规则
        /// </summary>
        private void InitAssetScrapNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("资产报废单号编码规则", "EB", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<AssetScrap, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 资产处置单号编码规则
        /// </summary>
        private void InitAssetDisposalNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("资产处置单号编码规则", "EC", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<AssetDisposal, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}