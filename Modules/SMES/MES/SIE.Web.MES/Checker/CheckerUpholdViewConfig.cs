using SIE.MES.Checker;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.MES.Checker.Commands;
using SIE.Web.MES.ItemChecker.Commands;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Checker
{
    /// <summary>
    /// 工装维护视图配置
    /// </summary>
    public class CheckerUpholdViewConfig : WebViewConfig<CheckerUphold>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, typeof(CheckerUpholdSaveCommands).FullName);
            View.UseCommands(typeof(CheckerUpholdImportCommand).FullName, typeof(CheckerUpholdDLTemplateCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);

            string CommName = typeof(CheckerUpholdImportEffectiveDateCommand).FullName; //SIE.Web.MES.Checker.Commands.CheckerUpholdImportEffectiveDateCommand
            View.UseCommands(CommName);
            using (View.OrderProperties())
            {
                View.Property(p => p.CheckerCode).ShowInList(width: 150);
                View.Property(p => p.CheckerName).ShowInList(width: 150);
                View.Property(p => p.EffectiveDate).ShowInList(width: 150);
                View.Property(p => p.CheckerType)
                .UseCatalogEditor(p =>
                {
                    p.CatalogType = CheckerUphold.TypeCatalogType; p.CatalogReloadData = true;
                }).UseListSetting(p => p.HelpInfo = "“来源快码Checker_TYPE");
                View.Property(p => p.Factory).ShowInList(width: 150).UseFactoryEditor();
                View.Property(p => p.DrawingNo).ShowInList(width: 150);
                //View.Property(p => p.Process).ShowInList(width: 150);
            }
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.CheckerCode).ShowInList(width: 150);
            View.Property(p => p.CheckerName).ShowInList(width: 150);
            View.Property(p => p.DrawingNo).ShowInList(width: 150);
        }

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.CheckerCode).Show();
            View.Property(p => p.CheckerName).Show();
            View.Property(p => p.EffectiveDate).Show();
            View.Property(p => p.CheckerType).Show();
            View.Property(p => p.FactoryCode).HasLabel("工厂编码");
            View.Property(p => p.DrawingNo).Show();
            //View.PropertyRef(p => p.Process.Code).HasLabel("工序编码");
        }
    }
}
