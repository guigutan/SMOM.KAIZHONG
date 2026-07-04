using SIE.Common.Catalogs;
using SIE.CSM.Customers;
using SIE.CSM.Suppliers;
using SIE.Data.DbMigration;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.CSM.DbMigrations
{
    /// <summary>
    /// 初始化快码
    /// </summary>
    public class _20180710_170001_InitSupplierType : ManualDbMigration
    {
        /// <summary>
        /// 数据库设置
        /// </summary>
        public override string DbSetting
        {
            get { return CsmEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加供应商、客户类型的快码".L10N(); }
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
                EntityList<CatalogType> catalogTypeList = RT.Service.Resolve<CatalogController>().GetCatalogTypeList();
                if (catalogTypeList.FirstOrDefault(p => p.Code == Supplier.CatalogAreaType) == null)
                {
                    Dictionary<string, string> AreaType = new Dictionary<string, string>
                    {
                        { "A01", "华南" },
                        { "A02", "华东" },
                        { "A03", "华中" },
                        { "A04", "华北" },
                        { "A05", "东北" },
                        { "A06", "西南" },
                        { "A07", "西北" },
                        { "A08", "国外" }
                    };
                    CatalogType CatalogAreaType = new CatalogType()
                    {
                        Code = Supplier.CatalogAreaType,
                        Name = "所在区域类型".L10N(),
                        Description = "所在区域类型".L10N()
                    };
                    AreaType.ForEach(p =>
                    {
                        CatalogAreaType.CatalogList.Add(new Catalog()
                        {
                            Code=p.Key,
                            Name=p.Value.L10N(),
                            Description=p.Value.L10N(),
                        });
                    });
                    RF.Save(CatalogAreaType);
                }

                if (catalogTypeList.FirstOrDefault(p => p.Code == SupplierAddress.CatalogAddressType) == null)
                {
                    Dictionary<string, string> AddressType = new Dictionary<string, string>
                    {
                        { "Add01", "收货地址" },
                        { "Add02", "退货地址" },
                        { "Add03", "注册地址" },
                        { "Add04", "发票地址" },
                        { "Add05", "发货地址" },
                        { "Add06", "办公地址" }
                    };
                    CatalogType CatalogAddressType = new CatalogType()
                    {
                        Code = SupplierAddress.CatalogAddressType,
                        Name = "地址类型".L10N(),
                        Description = "地址类型".L10N()
                    };
                    AddressType.ForEach(p =>
                    {
                        CatalogAddressType.CatalogList.Add(new Catalog()
                        {
                            Code = p.Key,
                            Name = p.Value.L10N(),
                            Description = p.Value.L10N(),
                        });
                    });
                    RF.Save(CatalogAddressType);
                }

                if (catalogTypeList.FirstOrDefault(p => p.Code == Customer.CatalogCustomerType) == null)
                {
                    Dictionary<string, string> CustomerType = new Dictionary<string, string>
                    {
                        { "C01", "合作伙伴" },
                        { "C02", "重点客户" },
                        { "C03", "普通客户" },
                        { "C04", "临时客户" },
                        { "C05", "潜在客户" },
                        { "C06", "断交客户" },
                        { "C07", "其他客户" }
                    };
                    CatalogType CatalogCustomerType = new CatalogType()
                    {
                        Code = Customer.CatalogCustomerType,
                        Name = "顾客类型".L10N(),
                        Description = "顾客类型".L10N()
                    };
                    CustomerType.ForEach(p =>
                    {
                        CatalogCustomerType.CatalogList.Add(new Catalog()
                        {
                            Code=p.Key,
                            Name=p.Value.L10N(),
                            Description=p.Value.L10N(),
                        });
                    });
                    RF.Save(CatalogCustomerType);
                }
            });
        }
    }
}
