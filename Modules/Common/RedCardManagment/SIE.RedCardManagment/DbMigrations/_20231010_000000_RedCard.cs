using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.RedCardManagment.RedCardApplyBills;
using SIE.RedCardManagment.RedCards;
using System;

namespace SIE.RedCardManagment.DbMigrations
{
    /// <summary>
    /// 红牌管理数据库初始化
    /// </summary>
    internal class _20231011_000000_RedCard : ManualDbMigration
    {
        /// <summary>
        /// 数据库设置
        /// </summary>
        public override string DbSetting
        {
            get { return RedCardManagmentDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "红牌单据数据库初始化".L10N(); }
        }

        /// <summary>
        /// 手动升级的类型：数据
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 不支持 Down
        /// </summary>
        protected override void Down() { }

        /// <summary>
        /// 注入
        /// </summary>
        protected override void Up()
        {
            RunCode(db =>
            {
                ////由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                if (AppRuntime.InvOrg == null)
                    AppRuntime.InvOrg = 1;

                //初始化 红牌编码生成规则
                InitRedCardBillNoConfig();

                //初始化 红牌申请单编码生成规则
                InitRedCardApplyBillNoConfig();
            });
        }

        /// <summary>
        /// 初始化红牌编码规则和配置项
        /// </summary>
        private void InitRedCardBillNoConfig()
        {
            var numberRule = AppRuntime.Service.Resolve<NumberRuleExtController>().CreateFormNoNumberRule("红牌编码规则", "RC", Common.Algorithm.DateFormat.yyMMdd, 4);
            if (numberRule != null)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                AppRuntime.Service.Resolve<ConfigExtController>().InitModuleConfig<RedCard, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化红牌申请单编码规则和配置项
        /// </summary>
        private void InitRedCardApplyBillNoConfig()
        {
            var numberRule = AppRuntime.Service.Resolve<NumberRuleExtController>().CreateFormNoNumberRule("红牌申请单编码规则", "RCA", Common.Algorithm.DateFormat.yyMMdd, 4);
            if (numberRule != null)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                AppRuntime.Service.Resolve<ConfigExtController>().InitModuleConfig<RedCardApplyBill, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}
