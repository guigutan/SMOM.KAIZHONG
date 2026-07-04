using SIE.Common.Catalogs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.EMS.MeteringEquipment.Calibrations;
using System;
using System.Linq;

namespace SIE.EMS.MeteringEquipment.DbMigrations
{
    /// <summary>
    /// 升级数据库
    /// </summary>
    public class _20200429_000001_InitCatalogType : ManualDbMigration
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
                ////获取快码组列表
                if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == CalibrationEquipment.PrecisionClassType) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = CalibrationEquipment.PrecisionClassType,
                        Name = "精度级别类型".L10N(),
                        Description = "精度级别类型".L10N()
                    };
                    RF.Save(catalogType);
                    CreateDetailsFastCode("10", "I", catalogType);
                    CreateDetailsFastCode("20", "II", catalogType);
                    CreateDetailsFastCode("30", "III", catalogType);
                }

                // 初始化 计量设备定检单号编码规则
                InitCalibrationNoConfig();



            });
        }

        /// <summary>
        /// 创建快码-订单来源
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="catalogType"></param>
        private void CreateDetailsFastCode(string code, string name, CatalogType catalogType)
        {
            var catalog = new Catalog()
            {
                Code = code,
                Name = name,
                CatalogTypeId = catalogType.Id
            };
            RF.Save(catalog);
        }

        /// <summary>
        /// 初始化 计量设备定检单号编码规则
        /// </summary>
        private void InitCalibrationNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("计量设备定检单号编码规则", "JLD", SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<Calibration, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}
