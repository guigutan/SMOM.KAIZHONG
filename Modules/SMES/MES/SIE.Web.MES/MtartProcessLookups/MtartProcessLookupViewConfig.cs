using SIE.MES.MtartProcessLookups;
using SIE.MetaModel.View;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.MtartProcessLookups
{
    public class MtartProcessLookupViewConfig : WebViewConfig<MtartProcessLookup>
    {
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls).UseImportCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.ProcessId).Show().UsePagingLookUpEditor((x, y) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(y.ProcessName), nameof(y.Process.Name));
                    dic.Add("ProcessId_Display", nameof(y.Process.Code));
                    x.DisplayField = nameof(y.Process.Code);
                    x.BindDisplayField = nameof(MtartProcessLookup.ProcessCode);
                    x.DicLinkField = dic;
                }).HasLabel("工序编码");
                View.Property(p => p.ProcessName).Show().Readonly();
                View.Property(p => p.KzCategoryId).Show().HasLabel("工艺属性分类编码").UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.KzCategoryName), nameof(e.KzCategory.Name));
                    m.DicLinkField = dic;
                });
                View.Property(p => p.KzCategoryName).Show();
                //View.Property(p => p.Mtart).Show();
                //View.Property(p => p.Dispo).Show();
                View.Property(p => p.ItemCategoryId).Show().UsePagingLookUpEditor((x, y) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(y.ItemCategoryName), nameof(y.ItemCategory.Name));
                    x.DicLinkField = dic;
                });
                View.Property(p => p.ItemCategoryName).Show().Readonly();
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.Process.Code).Show().HasLabel("工序编码");
                //View.Property(p => p.Mtart).Show();
                //View.Property(p => p.Dispo).Show();
                View.PropertyRef(p => p.ItemCategory.Code).Show().HasLabel("分类编码");
            }
        }
    }
}
