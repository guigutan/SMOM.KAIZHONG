using DevExpress.CodeParser;
using SIE.MES.Andon;
using SIE.MES.BlueLable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.BlueLable
{
    /// <summary>
    /// 蓝标查询
    /// </summary>
    public class BlueLableCriterialViewConfig : WebViewConfig<BlueLableCriterial>
    {
        /// <summary>
        /// 视图查询
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.BlueLableBox).ShowInList(width: 150);
                View.Property(p => p.Item).ShowInList(width: 150);
                View.Property(p => p.BatchNo).ShowInList(width: 150);
                View.Property(p => p.ProductionNo).ShowInList(width: 150);
                View.Property(p => p.CreateDate).Show().UseDateRangeEditor(p =>
                {
                    p.DateRangeType = ObjectModel.DateRangeType.LastMonth;
                });
                View.Property(p => p.ExternalIdent).Show();
            }
        }
    }
}
