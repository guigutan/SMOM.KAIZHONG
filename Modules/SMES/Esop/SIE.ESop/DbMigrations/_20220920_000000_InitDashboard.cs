using SIE.Common.Catalogs;
using SIE.Core.Common;
using SIE.Dashboard.Definitions;
using SIE.Dashboard.Modules;
using SIE.Data.DbMigration;
using SIE.Domain;
using SIE.Packages.Boxs;
using SIE.Packages.Boxs.Configs;
using System;
using System.Linq;

namespace SIE.ESop.DbMigrations
{
    /// <summary>
    /// 初始化周转箱类型数据
    /// </summary>
    public class _20220920_000000_InitDashboard : ManualDbMigration
    {
        /// <summary>
        /// 数据库连接
        /// </summary>
        public override string DbSetting
        {
            get { return ESopEntityDataProvider.ConnectionStringName; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public override string Description
        {
            get { return "初始化ESOP控制台菜单".L10N(); }
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

                if (!RF.GetAll<DashboardDefinition>().Any(p => p.Code == "ESOP"))
                {
                    var dashboardDefinition = new DashboardDefinition()
                    {
                        Code = "ESOP",
                        Name = "ESOP",
                        LayoutScale = 1,
                        Type = DashboardType.Dashboard
                    };
                    var dashboardSettings = new DashboardSettings()
                    {
                        UITemplate = "SIE.Wpf.ESop.ESopTemplate,SIE.Wpf.ESOP",
                        Intervals = 0,
                        LayoutScale = 1,
                    };
                    dashboardDefinition.DashboardSettingsList.Add(dashboardSettings);
                    RF.Save(dashboardDefinition);

                    var dashboardModule = new DashboardModule()
                    {
                        DashboardDefinitionId = dashboardDefinition.Id,
                        IsFullScreen = true,
                        KeyLabel = "ESOP",
                        Label = "ESOP",
                    };
                    RF.Save(dashboardModule);
                }
            });
        }
    }
}
