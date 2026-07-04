using SIE.MES.Checker;
using SIE.Web.Common;
using SIE.Web.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Checker
{
    /// <summary>
    /// 检具维护实体查询
    /// </summary>
    public class CheckerUpholdCriterialViewConfig : WebViewConfig<CheckerUpholdCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.CheckerCode).ShowInList(width: 150);
                View.Property(p => p.CheckerName).ShowInList(width: 150);
                View.Property(p => p.CheckerType)
                .UseCatalogEditor(p =>
                {
                    p.CatalogType = CheckerUphold.TypeCatalogType; p.CatalogReloadData = true;
                }).UseListSetting(p => p.HelpInfo = "“来源快码Checker_TYPE");
                View.Property(p => p.Factory).ShowInList(width: 150).UseFactoryEditor();
                View.Property(p => p.DrawingNo).Show();
                View.Property(p => p.EffectiveDate).Show().UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.LastMonth;
                });
                //View.Property(p => p.Process).ShowInList(width: 150);
            }
        }
    }
}
