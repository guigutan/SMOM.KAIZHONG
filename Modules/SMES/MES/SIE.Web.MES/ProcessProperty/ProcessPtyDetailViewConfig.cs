using SIE.MES.ProcessProperty;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.ProcessProperty
{
    public class ProcessPtyDetailViewConfig : WebViewConfig<ProcessPtyDetail>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save);
            using (View.OrderProperties())
            {
                View.Property(p => p.PrepareProjectId).Show().UsePagingLookUpEditor((x, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProName), nameof(e.PrepareProject.ProName));
                    keyValues.Add(nameof(e.ProCode), nameof(e.PrepareProject.ProCode));
                    keyValues.Add(nameof(e.ProType), nameof(e.PrepareProject.ProType));
                    keyValues.Add(nameof(e.ProDesc), nameof(e.PrepareProject.ProDesc));
                    x.DicLinkField = keyValues;
                });
                View.Property(p => p.ProName).Show().Readonly();
                View.Property(p => p.ProType).Show().Readonly();
                View.Property(p => p.ProDesc).Show().Readonly();
            }
        }
    }
}
