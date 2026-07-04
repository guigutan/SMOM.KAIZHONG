using SIE.MES.Fixture;
using SIE.Web.Common;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Fixture
{
    /// <summary>
    /// 工装维护实体查询
    /// </summary>
    public class FixtureUpholdCriterialViewConfig :WebViewConfig<FixtureUpholdCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
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
    }
}
