using SIE.Core.Common;
using SIE.Core.Common.Models;
using SIE.Data.DbMigration;
using SIE.Items.Configs;
using SIE.Items.Items.Configs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20220608_000000_InitItemData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return ItemEntityDataProvider.ConnectionStringName; }
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

                //初始化 物料编码生成规则
                InitItemCodeNoConfig();

                //初始化 产品BOM版本生成规则
                InitProductBomVersionConfig();
            });
        }

        /// <summary>
        /// 初始化 物料编码生成规则
        /// </summary>
        private void InitItemCodeNoConfig()
        {
            List<InitNumberData> datas = new List<InitNumberData>();

            var initNumberData = new InitNumberData()
            {
                Code = "物料编码生成规则",
                RuleType = Common.NumberRules.RuleType.Common
            };

            initNumberData.Details.Add(new InitNumberDetailData()
            {
                DetailType = DetailType.Sequence,
                Length = 8,
            });

            datas.Add(initNumberData);

            var numberRule = RT.Service.Resolve<NumberRuleExtController>().InitNumberRule(datas).FirstOrDefault();

            if (numberRule != null && numberRule.Id != default)
            {
                ItemCodeNoConfigValue configValue = new ItemCodeNoConfigValue
                {
                    ItemCodeRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<Item, ItemCodeNoConfig, ItemCodeNoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 产品BOM版本生成规则
        /// </summary>
        private void InitProductBomVersionConfig()
        {
            List<InitNumberData> datas = new List<InitNumberData>();

            var initNumberData = new InitNumberData()
            {
                Code = "产品BOM版本生成规则",
                RuleType = Common.NumberRules.RuleType.Common
            };

            initNumberData.Details.Add(new InitNumberDetailData()
            {
                DetailType = DetailType.FixedValue,
                FixedValue = "V",
                Length = 1,
            });

            initNumberData.Details.Add(new InitNumberDetailData()
            {
                DetailType = DetailType.Sequence,
                Length = 2,
            });

            datas.Add(initNumberData);

            var numberRule = RT.Service.Resolve<NumberRuleExtController>().InitNumberRule(datas).FirstOrDefault();

            if (numberRule != null && numberRule.Id != default)
            {
                ProductBomVersionConfigValue configValue = new ProductBomVersionConfigValue
                {
                    VersionId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<ProductBom, ProductBomVersionConfig, ProductBomVersionConfigValue>(configValue);
            }
        }
    }
}
