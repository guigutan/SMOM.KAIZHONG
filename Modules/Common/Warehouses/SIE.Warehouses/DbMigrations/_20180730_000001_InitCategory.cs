using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses.DbMigrations
{
    /// <summary>
    /// 初始化
    /// </summary>
    public class _20180730_000001_InitCategory : ManualDbMigration
    {
        /// <summary>
        /// 数据库设置
        /// </summary>
        public override string DbSetting
        {
            get { return WareHouseEntityDataProvider.ConnectionStringName; }
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
                if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == Warehouse.CatalogCategory) == null)
                {
                    Dictionary<string, string> WarehouseCatalog = new Dictionary<string, string>
                    {
                        { "W01", "原材料仓" },
                        { "W02", "半成品仓" },
                        { "W03", "成品仓" },
                        { "W04", "化学品仓" },
                        { "W05", "办公辅料仓" },
                        { "W06", "零配件仓" },
                        { "W07", "其他仓库" }
                    };
                    CatalogType catalogType = new CatalogType()
                    {
                        Code = Warehouse.CatalogCategory,
                        Name = "仓库分类".L10N(),
                        Description = "仓库分类".L10N()
                    };
                    WarehouseCatalog.ForEach(p =>
                    {
                        catalogType.CatalogList.Add(new Catalog()
                        {
                            Code=p.Key,
                            Name=p.Value.L10N(),
                            Description=p.Value.L10N()
                        });
                    });
                    RF.Save(catalogType);
                }
            });
        }
    }
}
