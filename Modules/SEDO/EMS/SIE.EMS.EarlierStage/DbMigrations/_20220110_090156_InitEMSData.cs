using SIE.Common.Catalogs;
using SIE.Common.Configs.CommonConfigs;
using SIE.Core.Common;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.EMS.EarlierStage.Budgets;
using SIE.EMS.EarlierStage.Projects;
using System;
using System.Linq;

namespace SIE.EMS.EarlierStage.DbMigrations
{
    /// <summary>
    /// 初始化快码
    /// </summary>
    public class _20220110_090156_InitEMSData : ManualDbMigration
    {
        /// <summary>
        /// 数据库
        /// </summary>
        public override string DbSetting
        {
            get { return EmsEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "添加快码组初始数据。".L10N(); }
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
                if (catalogTypeList.FirstOrDefault(p => p.Code == Budget.InvestClassify) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = Budget.InvestClassify,
                        Name = "投资分类".L10N(),
                        Description = "投资分类".L10N()
                    };
                    catalogType.CatalogList.Add(new Catalog() { Code = "IC0001", Name = "扩能项目".L10N(), Description = "扩能项目".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "IC0002", Name = "技改项目".L10N(), Description = "技改项目".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "IC0003", Name = "研发测试项目".L10N(), Description = "研发测试项目".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "IC0004", Name = "基建工程项目".L10N(), Description = "基建工程项目".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "IC0005", Name = "信息技术项目".L10N(), Description = "信息技术项目".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "IC0006", Name = "全新基地建设项目".L10N(), Description = "全新基地建设项目".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "IC0007", Name = "年度大修".L10N(), Description = "年度大修".L10N() });
                    RF.Save(catalogType);
                }
                if (catalogTypeList.FirstOrDefault(p => p.Code == Budget.BudgetClassify) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = Budget.BudgetClassify,
                        Name = "预算分类".L10N(),
                        Description = "预算分类".L10N()
                    };
                    catalogType.CatalogList.Add(new Catalog() { Code = "YS001", Name = "设备更新".L10N(), Description = "设备更新".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "YS002", Name = "设备技改".L10N(), Description = "设备技改".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "YS003", Name = "设备新增".L10N(), Description = "设备新增".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "YS004", Name = "备件采购".L10N(), Description = "备件采购".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "YS005", Name = "设备大修".L10N(), Description = "设备大修".L10N() });
                    RF.Save(catalogType);
                }
                if (catalogTypeList.FirstOrDefault(p => p.Code == Project.ProjectClassify) == null)
                {
                    var catalogType = new CatalogType()
                    {
                        Code = Project.ProjectClassify,
                        Name = "项目类别".L10N(),
                        Description = "项目类别".L10N()
                    };
                    catalogType.CatalogList.Add(new Catalog() { Code = "设备新增".L10N(), Name = "设备新增".L10N(), Description = "设备新增".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "车间升级".L10N(), Name = "车间升级".L10N(), Description = "车间升级".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "技改升级".L10N(), Name = "技改升级".L10N(), Description = "技改升级".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "基建".L10N(), Name = "基建".L10N(), Description = "基建".L10N() });
                    catalogType.CatalogList.Add(new Catalog() { Code = "备件采购".L10N(), Name = "备件采购".L10N(), Description = "备件采购".L10N() });
                    RF.Save(catalogType);
                }

                // 初始化 项目管理单号编码规则
                InitProjectNoConfig();

                // 初始化 项目变更单号编码规则
                InitProjectChangeNoConfig();

                // 初始化 预算管理单号编码规则
                InitBudgetNoConfig();

            });
        }

        /// <summary>
        /// 初始化 项目管理单号编码规则
        /// </summary>
        private void InitProjectNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("项目管理单号编码规则", "XM", SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<Project, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 项目变更单号编码规则
        /// </summary>
        private void InitProjectChangeNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("项目变更单号编码规则", "SBG", SIE.Common.Algorithm.DateFormat.yyMMdd, 2);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<ProjectChange, NoConfig, NoConfigValue>(configValue);
            }
        }

        /// <summary>
        /// 初始化 预算管理单号编码规则
        /// </summary>
        private void InitBudgetNoConfig()
        {
            var numberExtCtl = RT.Service.Resolve<NumberRuleExtController>();
            var numberRule = numberExtCtl.CreateFormNoNumberRule("预算管理单号编码规则", "YS", SIE.Common.Algorithm.DateFormat.yyMMdd, 3);

            if (numberRule != null && numberRule.Id != default)
            {
                NoConfigValue configValue = new NoConfigValue
                {
                    NumberRuleId = numberRule.Id
                };
                RT.Service.Resolve<ConfigExtController>().InitModuleConfig<Budget, NoConfig, NoConfigValue>(configValue);
            }
        }
    }
}
