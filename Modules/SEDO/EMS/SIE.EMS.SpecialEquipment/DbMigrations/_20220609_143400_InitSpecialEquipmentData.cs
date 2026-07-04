using SIE.Common.Catalogs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.EMS.SpecialEquipment.RegularInspections;
using System;
using System.Linq;

namespace SIE.EMS.SpecialEquipment.DbMigrations
{
    /// <summary>
    /// 升级数据库加载数据
    /// </summary>
    public class _20220609_143400_InitSpecialEquipmentData : ManualDbMigration
    {
        /// <summary>
        /// 数据库设置
        /// </summary>
        public override string DbSetting
        {
            get { return EntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加快码".L10N(); }
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
            this.RunCode(db =>
            {
                ////由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }
                // 初始化 特种设备定检单号编码规则
                InitRegularInspectionNoConfig();
            });
        }

        /// <summary>
        /// 初始化 特种设备定检单号编码规则
        /// </summary>
        private void InitRegularInspectionNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("特种设备定检单号编码规则", "TDJ", SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<RegularInspection, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}
