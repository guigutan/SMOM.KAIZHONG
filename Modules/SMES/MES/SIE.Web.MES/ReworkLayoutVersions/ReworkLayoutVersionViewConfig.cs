using DocumentFormat.OpenXml.Presentation;
using SIE.MES.ReworkLayoutVersions;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ReworkLayoutVersions
{
    public class ReworkLayoutVersionViewConfig : WebViewConfig<ReworkLayoutVersion>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ReworkLayoutVersion));
            base.ConfigView();
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.Version).Show().Readonly();
                View.Property(p => p.Desc).Show().Readonly();
                View.Property(p => p.ItemCode).Show().Readonly();
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
                View.Property(p => p.Factory).Show().Readonly();
                View.Property(p => p.BeginDateTime).Show().Readonly();
                View.Property(p => p.EndDateTime).Show().Readonly();
                View.Property(p => p.EffBeginDateTime).Show().Readonly();
                View.Property(p => p.EffEndDateTime).Show().Readonly();
                View.Property(p => p.Type).Show().Readonly();
                View.Property(p => p.Group).Show().Readonly();
                View.Property(p => p.Counter).Show().Readonly();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Version).Show();
                View.Property(p => p.Desc).Show();
                View.Property(p => p.Item).Show();
                View.Property(p => p.Factory).Show();
                View.Property(p => p.BeginDateTime).Show().UseDateRangeEditor();
                View.Property(p => p.EndDateTime).Show().UseDateRangeEditor();
                View.Property(p => p.EffBeginDateTime).Show().UseDateRangeEditor();
                View.Property(p => p.EffEndDateTime).Show().UseDateRangeEditor();
                View.Property(p => p.Type).Show();
                View.Property(p => p.Group).Show();
                View.Property(p => p.Counter).Show();
            }
        }

        protected override void ConfigSelectionView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.Version).Show().Readonly();
                View.Property(p => p.Desc).Show().Readonly();
                View.Property(p => p.ItemCode).Show().Readonly();
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
                View.Property(p => p.Factory).Show().Readonly();
                View.Property(p => p.BeginDateTime).Show().Readonly();
                View.Property(p => p.EndDateTime).Show().Readonly();
                View.Property(p => p.EffBeginDateTime).Show().Readonly();
                View.Property(p => p.EffEndDateTime).Show().Readonly();
                View.Property(p => p.Type).Show().Readonly();
                View.Property(p => p.Group).Show().Readonly();
                View.Property(p => p.Counter).Show().Readonly();
            }
        }
    }
}
