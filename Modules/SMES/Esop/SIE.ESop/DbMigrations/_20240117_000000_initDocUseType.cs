using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.ESop.EngDocuments;
using System;

namespace SIE.ESop.DbMigrations
{
    /// <summary>
    /// 工程文件使用类型快码
    /// </summary>
    internal class _20240117_000000_initDocUseType : ManualDbMigration
    {
        public override string DbSetting
        {
            get { return ESopEntityDataProvider.ConnectionStringName; }
        }

        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        public override string Description
        {
            get { return "添加工程文件使用类型快码".L10N(); }
        }

        protected override void Down()
        {
            
        }

        protected override void Up()
        {
            this.RunCode(db =>
            {
                AppRuntime.InvOrg ??= 1;
                if (RT.Service.Resolve<CatalogController>().GetCatalogType(EngDocumentDetail.DocTypeCatalogType) == null)
                {
                    var catalogtype = new CatalogType()
                    {
                        Code = EngDocumentDetail.DocTypeCatalogType,
                        Name = "工程文件使用类型".L10N(),
                        Description = "工程文件使用类型".L10N(),
                    };
                    catalogtype.CatalogList.Add(new Catalog() { Code = "ESOP", Name = "ESOP", Description = "ESOP" });
                    catalogtype.CatalogList.Add(new Catalog() { Code = "图纸".L10N(), Name = "图纸".L10N(), Description = "图纸".L10N() });
                    RF.Save(catalogtype);
                }
            });
        }
    }
}
