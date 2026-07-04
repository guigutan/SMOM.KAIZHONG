using SIE.Andon.Andons;
using SIE.MES.QTimes;
using SIE.MetaModel.View;
using SIE.Web.Andon.Andons.Commands;
using SIE.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons
{
    public class AndonSespViewConfig : WebViewConfig<AndonSesp>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SIE.Andon.Andons.Andon));
        }
        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //"SIE.Web.MES.QTimes.Commands.QTPushObjectEditCommand",
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete);
            View.UseCommands(typeof(AndonSeepImportCommand).FullName, typeof(AndonSeepDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonUpholdId).ShowInList(width: 150);
                View.Property(p => p.EmployeeId).ShowInList(width: 150);
                View.Property(p => p.AndonLevel)
                .UseCatalogEditor(p =>
                {
                    p.CatalogType = AndonSesp.LevelCatalogType; p.CatalogReloadData = true;
                }).UseListSetting(p => p.HelpInfo = "“来源快码ANDONSESP_LEVEL");
            }
        }

        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Andon.AndonCode).HasLabel("安灯维护编码");
            View.PropertyRef(p => p.AndonUphold.AndonCode).HasLabel("安灯区域编码");
            View.PropertyRef(p => p.Employee.Code).HasLabel("责任人编码");
            View.Property(p => p.AndonLevel)
            .UseCatalogEditor(p =>
            {
                p.CatalogType = AndonSesp.LevelCatalogType; p.CatalogReloadData = true;
            }).UseListSetting(p => p.HelpInfo = "“来源快码ANDONSESP_LEVEL").HasLabel("级别");
        }
    }
}
