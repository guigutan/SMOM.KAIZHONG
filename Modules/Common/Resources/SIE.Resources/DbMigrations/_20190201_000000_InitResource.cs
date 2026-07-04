using SIE.Common;
using SIE.Common.Organizations;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.Utils;
using System;

namespace SIE.Resources.DbMigrations
{
    /// <summary>
    /// 初始化数据库
    /// </summary>
    class _20190201_000000_InitResource : ManualDbMigration
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
            get { return "添加组织层级中的初始数据。".L10N(); }
        }

        /// <summary>
        /// 手动升级类型
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

                //由于本类没有支持 Down 操作，所以这里面的 Up 需要防止重入。
                var list = RT.Service.Resolve<OrganizationController>().GetOrganizationByType(OrganizationType.Group);
                if (list.Count == 0)
                {
                    var level = new OrganizationLevel
                    {
                        Type = OrganizationType.Group
                    };
                    level.Code = level.Type.ToString();
                    level.Name = EnumViewModel.EnumToLabel(level.Type).L10N();
                    level.IsByHand = YesNo.No;
                    level.InvOrgId = 0;
                    RF.Save(level);

                    var org = new Organization
                    {
                        Level = level,
                        Name = "集团".L10N(),
                        Code = "Group",
                        IsByHand = YesNo.No,
                        InvOrgId = 0
                    };
                    RF.Save(org);
                }
            });
        }
    }
}
