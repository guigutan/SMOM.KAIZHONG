using SIE.Andon.Andons.Configs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.DbMigrations
{
    internal class _20221213_171900_initAndonManage : ManualDbMigration
    {
        public override string DbSetting
        {
            get { return AndonEntityDataProvider.ConnectionStringName; }
        }
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }
        public override string Description
        {
            get { return "安灯管理事件编码初始化"; }
        }
        protected override void Down()
        {
            
        }

        protected override void Up()
        {
            var numberRule = RT.Service.Resolve<NumberRuleExtController>().CreateFormNoNumberRule("安灯管理事件编码规则", "Andon", SIE.Common.Algorithm.DateFormat.yyyyMMdd, 4);
            if (numberRule != null && numberRule.Id != default)
            {
                AndonManageCodeConfigValue configValue = new AndonManageCodeConfigValue();
                configValue.AndonManageCodeRuleId = numberRule.Id;
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<AndonManage, AndonManageCodeConfig, AndonManageCodeConfigValue>(configValue);
            }
        }
    }
}
