using SIE.Common;
using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using System;

namespace SIE.Items.DbMigrations
{
    /// <summary>
    /// 初始化单位类型快码
    /// </summary>
    class _20180401_000000_InitUnitTypeData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return CommonEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加单位类型初始数据。".L10N(); }
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
                if (RT.Service.Resolve<CatalogController>().GetCatalogType(Unit.CatalogType) == null)
                {
                    var catelogType = new CatalogType()
                    {
                        Code = Unit.CatalogType,
                        Name = "单位类型".L10N(),
                        Description = "单位类型".L10N()
                    };
                    catelogType.CatalogList.Add(new Catalog() { Code = "0001", Name = "重量单位".L10N(), Description = "重量单位".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0002", Name = "体积单位".L10N(), Description = "体积单位".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0003", Name = "计数单位".L10N(), Description = "计数单位".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0004", Name = "金额单位".L10N(), Description = "金额单位".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0005", Name = "长度单位".L10N(), Description = "长度单位".L10N() });
                    catelogType.CatalogList.Add(new Catalog() { Code = "0006", Name = "时间单位".L10N(), Description = "时间单位".L10N() });
                    RF.Save(catelogType);
                }

                if (RT.Service.Resolve<CatalogController>().GetCatalogType("PRODUCTGRADE_TYPE") == null)
                {
                    var catelogType = new CatalogType()
                    {
                        Code = "PRODUCTGRADE_TYPE",
                        Name = "产品等级".L10N(),
                        Description = "产品等级".L10N()
                    };
                    RF.Save(catelogType);
                }
            });
        }
    }
}
