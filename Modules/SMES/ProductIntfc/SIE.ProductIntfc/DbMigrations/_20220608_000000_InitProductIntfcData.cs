using SIE.Common.Catalogs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.ProductIntfc.Configs;
using SIE.ProductIntfc.FirstInsps;
using SIE.ProductIntfc.InspLogs;
using SIE.ProductIntfc.ProductInsps;
using SIE.ProductIntfc.ProductStorages;
using System;
using System.Linq;

namespace SIE.ProductIntfc.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20220608_000000_InitProductIntfcData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return ProductIntfcEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加初始数据。".L10N(); }
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

                //初始化 成品报检单单号生成规则
                InitWorkGroupOnLoanNoConfig();

                //初始化 成品入库单号生成规则
                InitProductStorageConfig();

                //初始化 首检单号生成规则
                InitFirstInspNoConfig();

                // 初始化成品报检手动审核处理方式快码
                InitInspExamineType();
            });
        }
        /// <summary>
        /// 初始化 成品报检单单号生成规则
        /// </summary>
        private void InitWorkGroupOnLoanNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("成品报检单单号生成规则", "CP",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<ProductInsp, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 成品入库单号生成规则
        /// </summary>
        private void InitProductStorageConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("成品入库单号生成规则", "RK",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                ProductStorageConfigValue configValue = new ProductStorageConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<StorageWorkOrder, ProductStorageConfig, ProductStorageConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 首检单号生成规则
        /// </summary>
        private void InitFirstInspNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("首检单号生成规则", "MSJ",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                FirstInspNoConfigValue configValue = new FirstInspNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<FirstInsp, FirstInspNoConfig, FirstInspNoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化成品报检手动审核处理方式快码
        /// </summary>
        private void InitInspExamineType()
        {
            ////获取快码组列表
            if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == InspLog.ProcessModeCataStr) == null)
            {
                CatalogType catalogType = new CatalogType
                {
                    Code = InspLog.ProcessModeCataStr,
                    Name = "成品报检处理方式快码",
                    Description = "成品报检处理方式快码"
                };
                catalogType.CatalogList.Add(new Catalog { Code = "返工", Name = "返工", Description = "返工" });
                catalogType.CatalogList.Add(new Catalog { Code = "作废", Name = "作废", Description = "作废" });
                RF.Save(catalogType);
            }
        }
    }
}
