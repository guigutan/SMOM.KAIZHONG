using SIE.Common.Catalogs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.EMS.EquipRepair;
using SIE.EMS.EquipRepair.EquipRepairs;
using SIE.EMS.EquipRepair.EquipRepairs.Configs;
using SIE.EMS.EquipRepair.ExperienceDepots;
using SIE.EMS.EquipRepair.PlanRepairs;
using SIE.EMS.Faults;
using SIE.EMS.Faults.Configs;
using System;
using System.Linq;

namespace SIE.EMS.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20220523_150300_InitEMSData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return EquipRepairEntityDataProvider.ConnectionStringName; }
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

                if (catalogTypeList.FirstOrDefault(p => p.Code == ExperienceDepot.expFaultReson) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = ExperienceDepot.expFaultReson,
                        Name = "故障原因".L10N(),
                        Description = "故障原因".L10N()
                    };
                    catalogType.CatalogList.Add(new Catalog() { Code = "GZ01", Name = "电压不稳定".L10N(), Description = "电压不稳定".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "GZ02", Name = "人为操作不当".L10N(), Description = "人为操作不当".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "GZ03", Name = "配件质量异常".L10N(), Description = "配件质量异常".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "GZ04", Name = "自然磨损".L10N(), Description = "自然磨损".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "GZ05", Name = "设备负荷运行".L10N(), Description = "设备负荷运行".L10N() });
                    RF.Save(catalogType);
                }

                //初始化故障类别单号生成规则
                InitEquipLargeFaultNoConfig();

                //初始化维修管理单号生成规则   
                InitEquipRepairNoConfig();

                //初始化设置设备维修管理是否工程评分确认为是
                InitIsEngineerConfirmConfigValue();

                //初始化设置设备维修管理是否交机确认为是
                InitIsHandoverConfirmConfigValue();

                //初始化 维修管理单号编码规则
                InitEquipRepairBillNoConfig();

                //初始化 维修经验库单号编码规则
                InitExperienceDepotNoConfig();

                //初始化 计划维修单号编码规则
                InitPlanRepairNoConfig();

            });
        }

        /// <summary>
        /// 初始化 备件入库单号生成规则
        /// </summary>
        private void InitEquipLargeFaultNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("设备故障类别生成规则", "GZ", SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                EquipLargeFaultCodeConfigValue configValue = new EquipLargeFaultCodeConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipLargeFault, EquipLargeFaultCodeConfig, EquipLargeFaultCodeConfigValue>(configValue);
            }

        }


        /// <summary>
        /// 初始化 备件入库单号生成规则
        /// </summary>
        private void InitEquipRepairNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("设备维修管理单号编码规则", "WX", SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                EquipRepairNoConfigValue configValue = new EquipRepairNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipRepairBill, EquipRepairNoConfig, EquipRepairNoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化设置设备维修管理是否工程评分确认为是
        /// </summary>
        private void InitIsEngineerConfirmConfigValue()
        {
            IsEngineerConfirmConfigValue isEngineerConfirmConfigValue = new IsEngineerConfirmConfigValue
            {
                IsEngineerConfirm = true
            };

            RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipRepairBill, IsEngineerConfirmConfig, IsEngineerConfirmConfigValue>(isEngineerConfirmConfigValue);
        }


        /// <summary>
        /// 初始化设置设备维修管理是否交机确认为是
        /// </summary>
        private void InitIsHandoverConfirmConfigValue()
        {
            IsHandoverConfirmConfigValue isHandoverConfirmConfigValue = new IsHandoverConfirmConfigValue
            {
                IsHandoverConfirm = true
            };

            RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipRepairBill, IsHandoverConfirmConfig, IsHandoverConfirmConfigValue>(isHandoverConfirmConfigValue);
        }


        /// <summary>
        /// 初始化 维修管理单号编码规则
        /// </summary>
        private void InitEquipRepairBillNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("维修管理单号编码规则", "WX", SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                EquipRepairNoConfigValue configValue = new EquipRepairNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipRepairBill, EquipRepairNoConfig, EquipRepairNoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 维修经验库单号编码规则
        /// </summary>
        private void InitExperienceDepotNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("维修经验库单号编码规则", "JYK", SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<ExperienceDepot, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 计划维修单号编码规则
        /// </summary>
        private void InitPlanRepairNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("计划维修单号编码规则", "OBM", SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<PlanRepair, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}