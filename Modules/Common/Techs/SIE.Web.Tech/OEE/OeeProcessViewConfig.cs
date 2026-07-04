using SIE.MetaModel.View;
using SIE.Tech.OEE;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Tech.OEE
{
    public class OeeProcessViewConfig : WebViewConfig<OeeProcess>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(OeeProcess));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll).UseImportCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessId).Show().UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ProcessName), nameof(e.Process.Name));
                    keyValues.Add("ProcessId_Display", nameof(e.Process.Name)); ;
                    m.DicLinkField = keyValues;
                    m.BindDisplayField = nameof(OeeProcess.ProcessCode);
                    m.DisplayField = nameof(e.Process.Code);
                });
                View.Property(p => p.ProcessName).Show().Readonly();
            }
        }

        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Process.Code).Show().HasLabel("工序编码");
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessId).Show();
            }
        }
    }
}
