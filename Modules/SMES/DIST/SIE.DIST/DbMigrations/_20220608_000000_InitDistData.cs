using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.DIST.Distribution.Configs;
using SIE.Packages.Boxs.Configs;
using System;

namespace SIE.DIST.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20220608_000000_InitDistData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return DistEntityDataProvider.ConnectionStringName; }
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

                //全局配置项:配送周转箱类型 初始值:配送周转箱
                //InitDistributionTurnoverBoxTypeConfig();

                //初始化 配送单退料单号生成规则
                InitReturnBillNoConfig();

                //初始化 配送单退料单标签生成规则
                InitBillLabelConfig();

                //初始化 配送单单号生成规则
                InitBillNoConfig();
            });
        }

        //全局配置项:配送周转箱类型 初始值:配送周转箱
        //private void InitDistributionTurnoverBoxTypeConfig()
        //{
        //    DistributionTurnoverBoxTypeConfigValue configValue = new DistributionTurnoverBoxTypeConfigValue();
        //    configValue.BoxType = "PS001";
        //    RT.Service.Resolve<ConfigExtController>()
        //        .InitGlobalConfig<DistributionTurnoverBoxTypeConfig, DistributionTurnoverBoxTypeConfigValue>(configValue);
        //}

        /// <summary>
        /// 初始化 配送单退料单号生成规则
        /// </summary>
        private void InitReturnBillNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("配送单退料单号生成规则", "TL",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                ReturnBillNoConfigValue configValue = new ReturnBillNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<DistributionBill, ReturnBillNoConfig, ReturnBillNoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 配送单退料单标签生成规则
        /// </summary>
        private void InitBillLabelConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("配送单退料单标签生成规则", "TLL",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                BillLabelConfigValue configValue = new BillLabelConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<DistributionBill, BillLabelConfig, BillLabelConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 配送单单号生成规则
        /// </summary>
        private void InitBillNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("配送单单号生成规则", "PS",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                BillNoConfigValue configValue = new BillNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<DistributionBill, BillNoConfig, BillNoConfigValue>(configValue);
            }
        }
    }
}
