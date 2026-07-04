using SIE.Common;
using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.Resources.WipResources;
using System;
using System.Linq;

namespace SIE.Resources.DbMigrations
{
    /// <summary>
    /// 初始化资源类型快码
    /// </summary>

    public class _20221123_000000_InitResourceType : ManualDbMigration
    {
        /// <summary>
        /// 数据库连接
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
            get { return "添加资源类型快码。".L10N(); }
        }

        /// <summary>
        /// 数据库升级类型
        /// </summary>
        public override ManualMigrationType Type
        {
            get { return ManualMigrationType.Data; }
        }

        /// <summary>
        /// 数据库降级
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

                if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == WipResource.ResourceTypeString) == null)
                {
                    RF.Save(new CatalogType()
                    {
                        Code = WipResource.ResourceTypeString,
                        Name = "资源类型".L10N(),
                        Description = "资源类型".L10N()
                    });
                }
            });
        }
    }
}
