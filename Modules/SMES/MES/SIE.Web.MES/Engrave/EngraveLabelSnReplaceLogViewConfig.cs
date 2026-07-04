using SIE.MES.Engrave;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.Engrave
{
    public class EngraveLabelSnReplaceLogViewConfig : WebViewConfig<EngraveLabelSnReplaceLog>
    {

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.OldSn).Show().Readonly();
                View.Property(p => p.NewSn).Show().Readonly();
                View.Property(p => p.ProductCode).Show().Readonly();
                View.Property(p => p.ProductName).Show().Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
                View.Property(p => p.BatchNo).Show().Readonly();
                View.Property(p => p.BlueLabel).Show().Readonly();
            }
        }
    }
}
