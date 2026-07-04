using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.SO;
using SIE.SO.SaleOrders;

namespace SIE.Pcb.SO.DbMigrations
{
    /// <summary>
    /// 添加销售订单快码的初始数据
    /// </summary>
    public class _20210513_000001_InitDefectData : ManualDbMigration
    {
        /// <summary>
        /// 数据库连接字符串名
        /// </summary>
        public override string DbSetting
        {
            get { return SOEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 添加 Defect 库中的初始数据
        /// </summary>
        public override string Description
        {
            get { return "添加销售订单快码的初始数据。"; }
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

                //检查是否有行业类型(销售明细单)
                if (RT.Service.Resolve<CatalogController>().GetCatalogType(SaleOrderDetail.INDUSTRYTYPE) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = SaleOrderDetail.INDUSTRYTYPE,
                        Name = "行业类型",
                        Description = "行业类型"
                    };
                    RF.Save(catalogType);
                    CreateSalesDetailsFastCode("Aerospace", "航空航天", catalogType);
                    CreateSalesDetailsFastCode("DataCenter", "数据中心", catalogType);
                    CreateSalesDetailsFastCode("MedicalCare", "医疗", catalogType);
                    CreateSalesDetailsFastCode("AutomotiveElectronics", "汽车电子", catalogType);
                    CreateSalesDetailsFastCode("ConsumerElectronics", "消费电子", catalogType);
                    CreateSalesDetailsFastCode("OpticalCommunication", "光通信", catalogType);
                }

                //检查是否有订单类型(销售明细单)
                if (RT.Service.Resolve<CatalogController>().GetCatalogType(SaleOrderDetail.ORDERTYPE) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = SaleOrderDetail.ORDERTYPE,
                        Name = "订单类型",
                        Description = "订单类型"
                    };
                    RF.Save(catalogType);
                    CreateSalesDetailsFastCode("Ordinary", "普通", catalogType);
                    CreateSalesDetailsFastCode("ExpressDelivery", "快件", catalogType);
                    CreateSalesDetailsFastCode("Develop", "研发", catalogType);
                    CreateSalesDetailsFastCode("DevelopExpress", "研发快件", catalogType);
                }

                //检查是否有产品类型(销售明细单)
                if (RT.Service.Resolve<CatalogController>().GetCatalogType(SaleOrderDetail.PRODUCTTYPE) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = SaleOrderDetail.PRODUCTTYPE,
                        Name = "产品类型",
                        Description = "产品类型"
                    };
                    RF.Save(catalogType);
                    CreateSalesDetailsFastCode("POFV", "POFV", catalogType);
                    CreateSalesDetailsFastCode("Sheet", "薄板", catalogType);
                    CreateSalesDetailsFastCode("RigidPCB", "刚挠结合板", catalogType);
                    CreateSalesDetailsFastCode("MetalPlate", "金属基板", catalogType);
                    CreateSalesDetailsFastCode("GoldenFinger", "金手指", catalogType);
                }

                //检查是否有产品等级(销售明细单)
                if (RT.Service.Resolve<CatalogController>().GetCatalogType(SaleOrderDetail.PRODUCTLEVEL) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = SaleOrderDetail.PRODUCTLEVEL,
                        Name = "产品等级",
                        Description = "产品等级"
                    };
                    RF.Save(catalogType);
                    CreateSalesDetailsFastCode("OneHDIPlate", "一阶HDI板", catalogType);
                    CreateSalesDetailsFastCode("TwoHDIPlate", "二阶HDI板", catalogType);
                    CreateSalesDetailsFastCode("ThreeHDIPlate", "三阶HDI板", catalogType);
                    CreateSalesDetailsFastCode("FourHDIPlate", "四阶HDI板", catalogType);
                    CreateSalesDetailsFastCode("AutoHDIPlate", "任意阶HDI板", catalogType);
                    CreateSalesDetailsFastCode("AluminumPlate", "铝基板", catalogType);
                    CreateSalesDetailsFastCode("ThroughHolePlate", "通孔板", catalogType);
                    CreateSalesDetailsFastCode("SoftPlate", "软板", catalogType);
                    CreateSalesDetailsFastCode("HardSoftPlate", "软硬板", catalogType);
                }
            });
        }
        /// <summary>
        /// 创建快码-订单来源
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
