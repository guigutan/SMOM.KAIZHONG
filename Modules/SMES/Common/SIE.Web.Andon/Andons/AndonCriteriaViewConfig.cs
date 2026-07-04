using DocumentFormat.OpenXml.Wordprocessing;
using SIE.Andon.Andons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.Andons
{
    public class AndonCriteriaViewConfig : WebViewConfig<AndonCriteria>
    {
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.AndonType).UseDataSource((source, pagingInfo, keyword) =>
            {
                return RT.Service.Resolve<AndonController>().GetAndonTypes(pagingInfo, keyword);
            });
            View.Property(p => p.AndonClass);
            View.Property(p => p.State);
            View.Property(p => p.CreateTime).UseDateRangeEditor(p => p.DateRangeType = ObjectModel.DateRangeType.All);
        }
    }
}
