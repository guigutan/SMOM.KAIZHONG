using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.MES.TeamManagement.OnLoans;
using System;

namespace SIE.MES.TeamManagement.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20220608_000000_InitTeamManagementData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return TeamManagementDataProvider.ConnectionStringName; }
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

                //初始化 班组借调单号生成规则
                InitWorkGroupOnLoanNoConfig();
            });
        }
        /// <summary>
        /// 初始化 班组借调单号生成规则
        /// </summary>
        private void InitWorkGroupOnLoanNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("班组借调单号生成规则", "JD",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<WorkGroupOnLoan, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}
