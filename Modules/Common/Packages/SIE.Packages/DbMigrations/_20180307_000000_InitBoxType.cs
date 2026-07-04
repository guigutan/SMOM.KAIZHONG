using SIE.Common.Catalogs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using System;
using System.Linq;

namespace SIE.Packages.DbMigrations
{
    /// <summary>
    /// 初始化周转箱类型数据
    /// </summary>
    public class _20180307_000000_InitBoxType : ManualDbMigration
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public override string DbSetting
        {
            get { return PackageEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "初始化周转箱类型数据".L10N(); }
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
        /// 升级数据
        /// </summary>
        protected override void Up()
        {
            this.RunCode(db =>
            {
                //由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }

                EntityList<CatalogType> catalogTypeList = RT.Service.Resolve<CatalogController>().GetCatalogTypeList();

                if (catalogTypeList.FirstOrDefault(p => p.Code == TurnoverBox.BoxTypeCatalog) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = TurnoverBox.BoxTypeCatalog,
                        Name = "周转箱".L10N(),
                        Description = "周转箱".L10N()
                    };
                    RF.Save(catalogType);
                    CreateDetailsFastCode("SC001", "生产周转箱", catalogType);
                    CreateDetailsFastCode("PS001", "配送周转箱", catalogType);
                }

                //全局配置项:生产周转箱类型 初始值:生产周转箱
                InitProductTrunoverBoxTypeConfig();
            });
        }

        //全局配置项:生产周转箱类型 初始值:生产周转箱
        private void InitProductTrunoverBoxTypeConfig()
        {
            ProductTrunoverBoxTypeConfigValue configValue = new ProductTrunoverBoxTypeConfigValue();
            configValue.BoxType = "SC001";
            RT.Service.Resolve<ConfigExtController>()
                .InitGlobalConfig<ProductTrunoverBoxTypeConfig, ProductTrunoverBoxTypeConfigValue>(configValue);
        }

        /// <summary>
        /// 创建快码明细
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="catalogType"></param>
        private void CreateDetailsFastCode(string code, string name, CatalogType catalogType)
        {
            var catalog = new Catalog()
            {
                Code = code,
                Name = name.L10N(),
                CatalogTypeId = catalogType.Id
            };
            RF.Save(catalog);
        }
    }
}
