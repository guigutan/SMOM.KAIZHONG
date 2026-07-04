using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using System;

namespace SIE.Items.DbMigrations
{
    /// <summary>
    /// 初始化物料快码"物料拓展属性快码组"
    /// </summary>
    internal class _20230316_000000_InitItemExtCata : ManualDbMigration
    {
        private const string desc = "物料拓展属性快码组";

        private const string extCatalogType = "ITEM_PROPERTY_EXTCATA";

        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting { 
            get { return ItemEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 手动升级的类型：数据
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description 
        {
            get { return desc.L10N(); }
        }

        /// <summary>
        /// 数据库回滚
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected override void Down()
        {
            
        }

        /// <summary>
        /// 数据库升级
        /// </summary>
        /// <exception cref="NotImplementedException"></exception>
        protected override void Up()
        {
            this.RunCode(db =>
            {
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }
                if (RT.Service.Resolve<CatalogController>().GetCatalogType(extCatalogType) == null)
                {
                    var catalogType = new CatalogType
                    {
                        Code = extCatalogType,
                        Name = desc.L10N(),
                        Description = desc.L10N(),
                    };
                    RF.Save(catalogType);
                }
            });
        }
    }
}
