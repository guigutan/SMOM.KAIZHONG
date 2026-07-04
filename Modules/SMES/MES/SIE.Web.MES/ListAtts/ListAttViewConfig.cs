
using SIE.MES.ListAtts;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ListAtts
{
    internal class ListAttViewConfig : WebViewConfig<ListAtt>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            View.Property(p => p.DataId).Readonly();
            View.Property(p => p.EventTime).Readonly();
            View.Property(p => p.Pin).Readonly();
            View.Property(p => p.Name).Readonly();
            View.Property(p => p.LastName).Readonly();
            View.Property(p => p.DeptName).Readonly();
            View.Property(p => p.CardNo).Readonly();
            View.Property(p => p.DevSn).Readonly();
            View.Property(p => p.VerifyModeName).Readonly();
            View.Property(p => p.EventName).Readonly();
            View.Property(p => p.EventPointName).Readonly();
            View.Property(p => p.ReaderName).Readonly();
            View.Property(p => p.AccZone).Readonly();
            View.Property(p => p.DevName).Readonly();
            View.Property(p => p.LogId).Readonly();
            View.Property(p => p.AttPlace).Readonly();
            View.Property(p => p.Mark).Readonly();          
        }
    }
}
