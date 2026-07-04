using SIE.Alert.AlertManages;
using SIE.Common.Catalogs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.Equipments.Abnormal;
using System;
using System.Linq;

namespace SIE.Equipments.DbMigrations
{
    /// <summary>
    /// 异常停线异常类型快码初始化
    /// </summary>
    public class _20180913_104140_InitAbnormalCauseData : ManualDbMigration
    {
        /// <summary>
        /// 数据库连接字符串名
        /// </summary>
        public override string DbSetting
        {
            get
            {
                return EquipmentEntityDataProvider.ConnectionStringName;
            }
        }

        /// <summary>
        /// 手动升级的类型：数据
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 添加 异常停线 库中的初始数据
        /// </summary>
        public override string Description
        {
            get { return "添加 异常停线 库中的初始数据".L10N(); }
        }

        /// <summary>
        /// 不支持 Down
        /// </summary>
        protected override void Down()
        {
        }

        /// <summary>
        /// 升级数据
        /// </summary>
        protected override void Up()
        {
            RunCode(db =>
            {
                //由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }

                if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == AbnormalCause.AbnormalTypeCatalog) == null)
                {
                    var catelogType = new CatalogType()
                    {
                        Code = AbnormalCause.AbnormalTypeCatalog,
                        Name = "异常类型".L10N(),
                        Description = "异常类型".L10N()
                    };

                    RF.Save(catelogType);
                }

                // 初始化 预警信息编码规则
                InitAlertManageCodeConfig();
            });
        }

        /// <summary>
        /// 初始化 预警信息编码规则
        /// </summary>
        private void InitAlertManageCodeConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("预警信息编码规则", "YJJ",
                SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>()
                    .InitModuleConfig<AlertManage, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}
