using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.DbMigrations
{
    internal class _20221201_083800_initAndon : ManualDbMigration
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
            get { return "添加安灯维护优先级快码"; }
        }

        protected override void Down()
        {
            
        }

        protected override void Up()
        {
            this.RunCode(db =>
            {
                if (AppRuntime.InvOrg == null)
                {
                    AppRuntime.InvOrg = 1;
                }
                if (RT.Service.Resolve<CatalogController>().GetCatalogType(Andon.PriorityCatalogType) == null)
                {
                    var catalogtype = new CatalogType()
                    {
                        Code = Andon.PriorityCatalogType,
                        Name = "安灯维护优先级",
                        Description = "安灯维护优先级",
                    };
                    catalogtype.CatalogList.Add(new Catalog() { Code = "高", Name = "高", Description = "高"});
                    catalogtype.CatalogList.Add(new Catalog() { Code = "中", Name = "中", Description = "中"});
                    catalogtype.CatalogList.Add(new Catalog() { Code = "低", Name = "低", Description = "低"});
                    RF.Save(catalogtype);
                }
            });
        }
    }
}
