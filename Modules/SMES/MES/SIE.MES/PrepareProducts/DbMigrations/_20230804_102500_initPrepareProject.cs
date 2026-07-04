using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.MES.PrepareProducts.Configs;

namespace SIE.MES.PrepareProducts.DbMigrations
{
    internal class _20230804_102500_initPrepareProject : ManualDbMigration
    {
        public override string DbSetting
        {
            get { return MesCoreEntityDataProvider.ConnectionStringName; }
        }

        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        public override string Description
        {
            get { return "产前准备项目维护编码初始化"; }
        }

        protected override void Down()
        {
            
        }

        protected override void Up()
        {
            var numberRule = RT.Service.Resolve<NumberRuleExtController>().CreateFormNoNumberRule("产前准备项目维护编码规则", "PRO", SIE.Common.Algorithm.DateFormat.yyyyMMdd, 4);
            if (numberRule != null && numberRule.Id != default)
            {
                PrepareProjectCodeConfigValue prepareProjectCodeConfigValue = new PrepareProjectCodeConfigValue();
                prepareProjectCodeConfigValue.ProCodeRuleId = numberRule.Id;
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<PrepareProject, PrepareProjectCodeConfig, PrepareProjectCodeConfigValue>(prepareProjectCodeConfigValue);
            }
        }
    }
}
