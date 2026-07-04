using SIE.Common.Catalogs;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.MES.ProjectDesigns.ChildInfos;
using SIE.MES.Projects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DbMigrations
{
    /// <summary>
    /// 初始化项目参数
    /// </summary>
    public class _20250422_103000_initProParam : ManualDbMigration
    {
        /// <summary>
        /// 链接数据库
        /// </summary>
        public override string DbSetting
        {
            get { return MesCoreEntityDataProvider.ConnectionStringName; }
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
            get { return "添加初始数据。".L10N(); }
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
        protected override void Up()
        {
            this.RunCode(db =>
            {
                if (!RT.InvOrg.HasValue)
                {
                    RT.InvOrg = 1;
                }

                //初始化项目参数快码
                InitParamCategory();

                //初始化工序标准参数快码
                InitProcessStCatalogType();

                //初始化项目号需求设计文档快码
                InitDesignTreeAttachmentType();
            });
        }

        /// <summary>
        /// 初始化项目参数快码
        /// </summary>
        private void InitParamCategory()
        {
            if (RT.Service.Resolve<CatalogController>().GetCatalogType(ProjectParam.ProjectParamTypeCata) == null)
            {
                var catelogType = new CatalogType()
                {
                    Code = ProjectParam.ProjectParamTypeCata,
                    Name = "项目参数类型".L10N(),
                    Description = "项目参数类型".L10N(),
                };
                catelogType.CatalogList.Add(new Catalog() { Code = "工艺参数".L10N(), Name = "工艺参数".L10N(), Description = "工艺参数".L10N() });
                catelogType.CatalogList.Add(new Catalog() { Code = "其他参数".L10N(), Name = "其他参数".L10N(), Description = "其他参数".L10N() });
                RF.Save(catelogType);
            }
        }

        /// <summary>
        /// 初始化工序标准参数快码
        /// </summary>
        private void InitProcessStCatalogType()
        {
            if (RT.Service.Resolve<CatalogController>().GetCatalogType(ProcessStandardParam.ProcessStandardCata) == null)
            {
                var catelogType = new CatalogType()
                {
                    Code = ProcessStandardParam.ProcessStandardCata,
                    Name = "工序标准参数类型".L10N(),
                    Description = "工序标准参数类型".L10N(),
                };
                catelogType.CatalogList.Add(new Catalog() { Code = "普通".L10N(), Name = "普通".L10N(), Description = "普通".L10N() });
                catelogType.CatalogList.Add(new Catalog() { Code = "其他".L10N(), Name = "其他".L10N(), Description = "其他".L10N() });
                RF.Save(catelogType);
            }
        }

        /// <summary>
        /// 初始化项目号需求设计文档类型
        /// </summary>
        private void InitDesignTreeAttachmentType()
        {
            if (RT.Service.Resolve<CatalogController>().GetCatalogType(DesignTreeDocument.DesignTreeDocumentType) == null)
            {
                var catelogType = new CatalogType()
                {
                    Code = DesignTreeDocument.DesignTreeDocumentType,
                    Name = "项目号需求设计工艺资料文档类型".L10N(),
                    Description = "项目号需求设计工艺资料文档类型".L10N(),
                };
                catelogType.CatalogList.Add(new Catalog() { Code = "作业规格书".L10N(), Name = "作业规格书".L10N(), Description = "作业规格书".L10N() });
                catelogType.CatalogList.Add(new Catalog() { Code = "附件资料".L10N(), Name = "附件资料".L10N(), Description = "附件资料".L10N() });
                RF.Save(catelogType);
            }
        }
    }
}
