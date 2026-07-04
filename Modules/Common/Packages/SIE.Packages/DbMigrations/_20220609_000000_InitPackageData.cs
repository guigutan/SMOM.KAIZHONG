using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Packages.ItemLabels;
using SIE.Packages.ItemLabels.Configs;
using System;

namespace SIE.Packages.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20220609_000000_InitPackageData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return PackageEntityDataProvider.ConnectionStringName; }
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

                //初始化 物料标签号生成规则
                InitItemLabelNoConfig();
            });
        }

        /// <summary>
        /// 初始化 物料标签号生成规则
        /// </summary>
        private void InitItemLabelNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("物料标签号生成规则", "WLB",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 4);

            if (numberRule != null && numberRule.Id != default)
            {
                ItemLabelNoConfigValue configValue = new ItemLabelNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<ItemLabel, ItemLabelNoConfig, ItemLabelNoConfigValue>(configValue);
            }
        }
    }
}
