using SIE.APS;
using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.Kit.APS.ProductLocations;

namespace SIE.Kit.APS.DbMigrations
{
    /// <summary>
    /// 添加产品定位的快码初始数据
    /// </summary>
    public class _20210526_000011_InitCatalogType : ManualDbMigration
    {
        /// <summary>
        /// 数据库连接字符串名
        /// </summary>
        public override string DbSetting
        {
            get { return KitAPSEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 添加 Defect 库中的初始数据
        /// </summary>
        public override string Description
        {
            get { return "添加产品定位的快码初始数据。"; }
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

                //获取快码组列表    

                //检查是否有分类值
                if (RT.Service.Resolve<CatalogController>().GetCatalogType(ProductLocation.CLASSIFICATIONVALUE) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = ProductLocation.CLASSIFICATIONVALUE,
                        Name = "分类值",
                        Description = "分类值"
                    };
                    RF.Save(catalogType);
                    CreateSalesDetailsFastCode("AutomotiveElectronics", "汽车电子", catalogType);
                    CreateSalesDetailsFastCode("ConsumerElectronics", "消费电子", catalogType);
                    CreateSalesDetailsFastCode("OpticalCommunication", "光通信", catalogType);
                }

            });
        }
        /// <summary>
        /// 创建快码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="catalogType"></param>
        private void CreateSalesDetailsFastCode(string code, string name, CatalogType catalogType)
        {
            var catalog = new Catalog()
            {
                Code = code,
                Name = name,
                CatalogTypeId = catalogType.Id
            };
            RF.Save(catalog);
        }
    }
}
