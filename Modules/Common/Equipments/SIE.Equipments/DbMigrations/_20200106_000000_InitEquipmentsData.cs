using SIE.Common.Catalogs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.Equipments.Configs;
using SIE.Equipments.EquipAccounts;
using SIE.Equipments.EquipModels;
using SIE.Equipments.EquipTypes;
using System;
using System.Linq;

namespace SIE.Equipments.DbMigrations
{
    /// <summary>
    /// 数据库升级
    /// </summary>
    public class _20200106_000000_InitEquipmentsData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return EquipmentEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加快码组初始数据。".L10N(); }
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
                if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == EquipType.EquipTypeCatalogType) == null)
                {
                    var catelogType = new CatalogType()
                    {
                        Code = EquipType.EquipTypeCatalogType,
                        Name = "设备类别".L10N(),
                        Description = "EMS设备类别".L10N()
                    };
                    catelogType.CatalogList.Add(new Catalog() { Code = "0001", Name = "生产设备".L10N(), Description = "生产设备".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0002", Name = "辅助设备".L10N(), Description = "辅助设备".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0003", Name = "办公设备".L10N(), Description = "办公设备".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0004", Name = "公用设备".L10N(), Description = "公用设备".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0005", Name = "物流设备".L10N(), Description = "物流设备".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0006", Name = "IT设备".L10N(), Description = "IT设备".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0007", Name = "特种设备".L10N(), Description = "特种设备".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0008", Name = "计量设备".L10N(), Description = "计量设备".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0009", Name = "模具设备".L10N(), Description = "模具设备".L10N() });
                    RF.Save(catelogType);
                }

                if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == EquipAccount.EquipAccountUseLevel) == null)
                {
                    var catelogType = new CatalogType()
                    {
                        Code = EquipAccount.EquipAccountUseLevel,
                        Name = "ABC分类".L10N(),
                        Description = "ABC分类".L10N()
                    };
                    catelogType.CatalogList.Add(new Catalog() { Code = "A", Name = "A", Description = "A" });
                    catelogType.CatalogList.Add(new Catalog() { Code = "B", Name = "B", Description = "B" });
                    catelogType.CatalogList.Add(new Catalog() { Code = "C", Name = "C", Description = "C" });
                    RF.Save(catelogType);
                }


                //初始化设备型号特种设备和计量设备的类别
                InitEquipModelSpecialAndMetering();

                //初始化 设备台账维护设备台账编码规则
                InitEquipAccountNoConfig();
            });
        }

        /// <summary>
        /// 初始化设备型号特种设备和计量设备的类别
        /// </summary>
        private static void InitEquipModelSpecialAndMetering()
        {
            Catalog specialCatalog = RT.Service.Resolve<CatalogController>().GetCatalog(EquipType.EquipTypeCatalogType, "0007");
            Catalog meteringCatalog = RT.Service.Resolve<CatalogController>().GetCatalog(EquipType.EquipTypeCatalogType, "0008");

            //初始化设备型号特种设备与计量设备类型

            EquipModelEquipmentCategoryConfigValue equipModelEquipmentCategoryConfigValue = new EquipModelEquipmentCategoryConfigValue();

            if (specialCatalog != null && meteringCatalog != null)
            {
                equipModelEquipmentCategoryConfigValue.SpecialIds = specialCatalog.Id.ToString();
                equipModelEquipmentCategoryConfigValue.SpecialCategoryName = specialCatalog.Name;

                equipModelEquipmentCategoryConfigValue.EquipmentMeteringIds = meteringCatalog.Id.ToString();
                equipModelEquipmentCategoryConfigValue.EquipmentMeteringName = meteringCatalog.Name;

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipModel, EquipModelEquipmentCategoryConfig, EquipModelEquipmentCategoryConfigValue>(equipModelEquipmentCategoryConfigValue);
            }
        }


        /// <summary>
        /// 初始化 设备台账维护设备台账编码规则
        /// </summary>
        private void InitEquipAccountNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("设备台账编码规则", "MC", SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                AccountNoConfigValue configValue = new AccountNoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };

                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<EquipAccount, AccountNoConfig, AccountNoConfigValue>(configValue);
            }
        }
    }
}