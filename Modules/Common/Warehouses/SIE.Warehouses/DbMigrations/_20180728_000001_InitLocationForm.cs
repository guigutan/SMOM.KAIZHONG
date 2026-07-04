using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses.DbMigrations
{
    class _20180728_000001_InitLocationForm : ManualDbMigration
    {
        /// <summary>
        /// 数据库
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
            get { return "添加快码类型：库位形式".L10N(); }
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
                if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == StorageLocationInfo.LOCATIONFORM) == null)
                {
                    Dictionary<string, string> LocationForm = new Dictionary<string, string>
                    {
                        { "L01", "堆垛平地" },
                        { "L02", "多层货架" },
                        { "L03", "单层货架" },
                        { "L04", "容器设备" },
                        { "L05", "其他形态" }
                    };
                    CatalogType catalogType = new CatalogType()
                    {
                        Code = StorageLocationInfo.LOCATIONFORM,
                        Name = "库位形式".L10N(),
                        Description = "库位形式".L10N()
                    };
                    LocationForm.ForEach(p =>
                    {
                        catalogType.CatalogList.Add(new Catalog()
                        {
                            Code = p.Key,
                            Name = p.Value.L10N(),
                            Description = p.Value.L10N()
                        });
                    });
                    RF.Save(catalogType);
                }
            });
        }
    }
}
