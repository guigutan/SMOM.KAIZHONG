using SIE.MES.ReworkLayoutVersions;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ReworkLayoutVersions
{
    public class ReworkInfoRecordViewConfig : WebViewConfig<ReworkInfoRecord>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ReworkInfoRecord));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.State).Show().Readonly();
                View.Property(p => p.Factory).Show().Readonly();
                View.Property(p => p.ItemCode).ShowInList(width: 150).Readonly();
                View.Property(p => p.ItemName).ShowInList(width: 150).Readonly();
                View.Property(p => p.ShortDescription).ShowInList(width: 150).Readonly();
                View.Property(p => p.Qty).Show().Readonly();
                View.Property(p => p.BeginDateTime).ShowInList(width: 150).Readonly();
                View.Property(p => p.EndDateTime).ShowInList(width: 150).Readonly();
                View.Property(p => p.Version).Show().Readonly();
                View.Property(p => p.IsUpload).Show().Readonly();
                View.Property(p => p.UniqueCode).Show().Readonly();
                View.Property(p => p.Department).Show().Readonly();
                View.Property(p => p.ProductOrder).Show().Readonly();
                View.Property(p => p.Identification).Show().Readonly();
                View.Property(p => p.Msg).Show().Readonly();
                View.ChildrenProperty(p => p.ReworkInfoRecordDtlList).Show(ChildShowInWhere.All);
            }
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.State).Show().UseEnumEditor(p => p.AllowBlank = true);
            View.Property(p => p.Factory).Show();
            View.Property(p => p.ItemId).Show();
            View.Property(p => p.BeginDateTime).UseDateRangeEditor().Show();
            View.Property(p => p.EndDateTime).UseDateRangeEditor().Show();
            View.Property(p => p.ReworkLayoutVersionId).Show();
            View.Property(p => p.UniqueCode).Show();
            View.Property(p => p.Department).Show();
            View.Property(p => p.ProductOrder).Show();
            View.Property(p => p.Msg).Show();
        }
    }
}
