using SIE.Common.Catalogs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.EMS.Purchases.EquipmentAcceptances;
using SIE.EMS.Purchases.EquipmentInbounds;
using SIE.EMS.Purchases.EquipmentReceives;
using SIE.EMS.Purchases.EquipmentSetups;
using SIE.EMS.Purchases.FixtureAcceptances;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.EMS.Purchases.FixtureReceives.Configs;
using SIE.EMS.Purchases.PaymentPlans;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.EMS.Purchases.PurchaseRequisitions;
using SIE.EMS.Purchases.SparePartAcceptances;
using SIE.EMS.Purchases.SparePartReceives;
using System;
using System.Linq;

namespace SIE.EMS.Purchases.DbMigrations
{
    /// <summary>
    /// 初始化快码
    /// </summary>
    public class _20220221_165123_InitEMSData : ManualDbMigration
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
        /// 注入
        /// </summary>
        protected override void Up()
        {
            this.RunCode(db =>
            {
                ////由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }

                ////获取快码组列表
                EntityList<CatalogType> catalogTypeList = RT.Service.Resolve<CatalogController>().GetCatalogTypeList();
                if (catalogTypeList.FirstOrDefault(p => p.Code == PurchaseOrder.PurchaseClassify) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = PurchaseOrder.PurchaseClassify,
                        Name = "采购分类".L10N(),
                        Description = "采购分类".L10N()
                    };
                    catalogType.CatalogList.Add(new Catalog() { Code = "采购分类1-1".L10N(), Name = "采购分类1-1".L10N(), Description = "采购分类1-1".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "CGFL2-1", Name = "CGFL2-1", Description = "CGFL2-1" });
                    RF.Save(catalogType);
                }
                if (catalogTypeList.FirstOrDefault(p => p.Code == PaymentTerms.PhaseCatalog) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = PaymentTerms.PhaseCatalog,
                        Name = "付款阶段".L10N(),
                        Description = "付款阶段".L10N()
                    };
                    catalogType.CatalogList.Add(new Catalog() { Code = "前期".L10N(), Name = "前期".L10N(), Description = "前期".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "中期".L10N(), Name = "中期".L10N(), Description = "中期".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "后期".L10N(), Name = "后期".L10N(), Description = "后期".L10N() });
                    RF.Save(catalogType);
                }
                if (catalogTypeList.FirstOrDefault(p => p.Code == PaymentTerms.PaymentMethodCatalog) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = PaymentTerms.PaymentMethodCatalog,
                        Name = "付款方式".L10N(),
                        Description = "付款方式".L10N()
                    };
                    catalogType.CatalogList.Add(new Catalog() { Code = "现金".L10N(), Name = "现金".L10N(), Description = "现金".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "银行卡".L10N(), Name = "银行卡".L10N(), Description = "银行卡".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "SWIFT", Name = "SWIFT", Description = "SWIFT" });
                    RF.Save(catalogType);
                }

                //初始化 备件接收单号编码规则
                InitSparePartReceiveNoConfig();

                //初始化 备件验收单号编码规则
                InitSparePartAcceptanceNoConfig();

                //初始化 安装调试单号编码规则
                InitEquipmentSetupNoConfig();

                //初始化 工治具接收单号和序列号编码规则
                InitFixtureReceiveAndSnNoConfig();

                //初始化 工治具验收单号编码规则
                InitFixtureAcceptanceNoConfig();

                //初始化 采购申请单号编码规则
                InitPurchaseRequisitionNoConfig();

                //初始化 采购订单单号编码规则
                InitPurchaseOrderNoConfig();

                //初始化 付款计划单号编码规则
                InitPaymentPlanNoConfig();

                //初始化 设备接收单号编码规则
                InitEquipmentReceiveNoConfig();

                //初始化 设备开箱验收单号编码规则
                InitEquipmentAcceptanceNoConfig();

                //初始化 设备入库单号编码规则
                InitEquipmentInboundNoConfig();

            });
        }

        /// <summary>
        /// 初始化 备件接收单号编码规则
        /// </summary>
        private void InitSparePartReceiveNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("备件接收单号编码规则", "SPJ", SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<SparePartReceive, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 备件验收单号编码规则
        /// </summary>
        private void InitSparePartAcceptanceNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("备件验收单号编码规则", "BY", SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<SparePartAcceptance, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 安装调试单号编码规则
        /// </summary>
        private void InitEquipmentSetupNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("安装调试单号编码规则", "AZ", SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipmentSetup, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 工治具接收单号和序列号编码规则
        /// </summary>
        private void InitFixtureReceiveAndSnNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("工治具接收单号编码规则", "GJ", SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            var snNumberRule = numberExtCtl.CreateFormNoNumberRule("工治具接收单列号编码规则", "GN", SIE.Common.Algorithm.DateFormat.yyMMdd, 5);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue numberconfigValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<FixtureReceive, NoConfig, NoConfigValue>(numberconfigValue);
            }

            if (snNumberRule != null && snNumberRule.Id != default)
            {
                ReceiveSnNoConfigValue snNumberconfigValue = new ReceiveSnNoConfigValue
                {
                    NumberRuleId = snNumberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<FixtureReceive, ReceiveSnNoConfig, ReceiveSnNoConfigValue>(snNumberconfigValue);
            }
        }

        /// <summary>
        /// 初始化 工治具验收单号编码规则
        /// </summary>
        private void InitFixtureAcceptanceNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("工治具验收单号编码规则", "GY", SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<FixtureAcceptance, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 采购申请单号编码规则
        /// </summary>
        private void InitPurchaseRequisitionNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("采购申请单号编码规则", "SPR", SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<PurchaseRequisition, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 采购订单单号编码规则
        /// </summary>
        private void InitPurchaseOrderNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("采购订单单号编码规则", "SPO", SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<PurchaseOrder, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 付款计划单号编码规则
        /// </summary>
        private void InitPaymentPlanNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("付款计划单号编码规则", "SF", SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<PaymentPlan, NoConfig, NoConfigValue>(configValue);
            }
        }


        /// <summary>
        /// 初始化 设备接收单号编码规则
        /// </summary>
        private void InitEquipmentReceiveNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("设备接收单号编码规则", "MJ", SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipmentReceive, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 设备开箱验收单号编码规则
        /// </summary>
        private void InitEquipmentAcceptanceNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("设备开箱验收单号编码规则", "SY", SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipmentAcceptance, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 设备入库单号编码规则
        /// </summary>
        private void InitEquipmentInboundNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("设备入库单号编码规则", "SR", SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipmentInbound, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}
