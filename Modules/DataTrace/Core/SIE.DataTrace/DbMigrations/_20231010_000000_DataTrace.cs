using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.DataTrace.TraceMainDatas;
using System;

namespace SIE.DataTrace.DbMigrations
{
    /// <summary>
    /// 数据追溯数据库初始化
    /// </summary>
    internal class _20231011_000000_DataTrace : ManualDbMigration
    {
        /// <summary>
        /// 数据库设置
        /// </summary>
        public override string DbSetting
        {
            get { return DataTraceEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "数据追溯数据库初始化".L10N(); }
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
                AppRuntime.InvOrg ??= 1;

                //初始化 追溯主数据编码生成规则
                InitDataTraceMainDataBillNoConfig();
            });
        }

        /// <summary>
        /// 初始化追溯主数据编码规则和配置项
        /// </summary>
        private void InitDataTraceMainDataBillNoConfig()
        {
            var numberRule = AppRuntime.Service.Resolve<NumberRuleExtController>().CreateFormNoNumberRule("追溯主数据单号编码规则", "DTMD", Common.Algorithm.DateFormat.yyMMdd, 4);
            if (numberRule != null)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                AppRuntime.Service.Resolve<ConfigExtController>().InitModuleConfig<TraceMainData, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}
