using SIE.Common.Catalogs;
using SIE.CSM.Suppliers;
using SIE.Data.DbMigration;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.CSM.DbMigrations
{
    /// <summary>
    /// 初始化
    /// </summary>
    public class _20180401_200001_InitCatalogType : ManualDbMigration
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
                Dictionary<string, string> SupplierList = new Dictionary<string, string>
                {
                    { "S01", "设备供应商" },
                    { "S02", "原材料供应商" },
                    { "S03", "办公用品供应商" },
                    { "S04", "服务供应商" },
                    { "S05", "其他类供应商" }
                };
                ////由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }
                ////获取快码组列表
                if (RT.Service.Resolve<CatalogController>().GetCatalogTypeList().FirstOrDefault(p => p.Code == Supplier.SupperType) == null)
                {
                    CatalogType catalogType = new CatalogType()
                    {
                        Code = Supplier.SupperType,
                        Name = "供应商类型".L10N(),
                        Description = "供应商类型".L10N()
                    };
                    SupplierList.ForEach(p =>
                    {
                        catalogType.CatalogList.Add(new Catalog()
                        {
                            Code = p.Key,
                            Name = p.Value.L10N(),
                            Description = p.Value.L10N(),
                        });
                    });
                    RF.Save(catalogType);
                }
            });
        }
    }
}