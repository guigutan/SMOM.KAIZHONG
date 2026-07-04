using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.MES.WIP.Configs;
using System;

namespace SIE.MES.DbMigrations
{
    internal class _20220515_100100_InitChangeItemHandleMethod : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return MesCoreEntityDataProvider.ConnectionStringName; }
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

        protected override void Down()
        {
            throw new NotImplementedException();
        }

        // <summary>
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

                ChangeItemHandleMethodConfigValue configValue = new ChangeItemHandleMethodConfigValue
                {
                    HandleMethod = WIP.Repairs.ChangeItemHandleMethod.Scrap
                };
                const string entityType = "SIE.Wpf.MES.WIP.Repairs.RepairViewModel,SIE.Wpf.MES";
                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<ChangeItemHandleMethodConfig, ChangeItemHandleMethodConfigValue>(entityType, configValue);

                KeyComponentsReplacementConfigValue replacementConfigValue = new KeyComponentsReplacementConfigValue
                {
                    HandleMethod = WIP.Reworks.ReplaceItemHandleMethod.Scrap
                };
                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<KeyComponentsReplacementConfig, KeyComponentsReplacementConfigValue>(entityType, replacementConfigValue);
            });
        }
    }
}
