using SIE.MES.Fixture;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.MES.Fixture.Commands;
using SIE.Web.MES.ItemChecker.Commands;
using SIE.Web.Resources;

namespace SIE.Web.MES.Fixture
{
    /// <summary>
    /// 工装维护视图配置
    /// </summary>
    public class FixtureUpholdViewConfig : WebViewConfig<FixtureUphold>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(FixtureUpholdSaveCommands).FullName);
            View.UseCommands(typeof(FixtureUpholdImportCommand).FullName, typeof(FixtureUpholdDLTemplateCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.FixtureCode).ShowInList(width: 150).HasLabel("工装唯一码");
                View.Property(p => p.FixtureName).ShowInList(width: 150).HasLabel("工装物料描述");
                View.Property(p => p.FixtureState)
                .UseCatalogEditor(p =>
                {
                    p.CatalogType = FixtureUphold.StateCatalogType; p.CatalogReloadData = true;
                }).UseListSetting(p => p.HelpInfo = "“来源快码FIXTURE_STATE");
                View.Property(p => p.FixtureType)
                .UseCatalogEditor(p =>
                {
                    p.CatalogType = FixtureUphold.TypeCatalogType; p.CatalogReloadData = true;
                }).UseListSetting(p => p.HelpInfo = "“来源快码FIXTURE_TYPE");
                View.Property(p => p.Factory).ShowInList(width: 150).UseFactoryEditor();
                View.Property(p => p.Drawn).ShowInList(width: 150);
                //View.Property(p => p.Process).ShowInList(width: 150);
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.FixtureCode).ShowInList(width: 150).HasLabel("工装唯一码"); 
            View.Property(p => p.FixtureName).ShowInList(width: 150).HasLabel("工装物料描述");
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.FixtureCode).Show().HasLabel("工装唯一码");
            View.Property(p => p.FixtureName).Show().HasLabel("工装物料描述");
            View.Property(p => p.FixtureState).Show();
            View.Property(p => p.FixtureType).Show();
            View.Property(p => p.FactoryCode).HasLabel("工厂编码");
            View.Property(p => p.Drawn).Show();
            //View.PropertyRef(p => p.Process.Code).HasLabel("工序编码");
        }
    }
}
