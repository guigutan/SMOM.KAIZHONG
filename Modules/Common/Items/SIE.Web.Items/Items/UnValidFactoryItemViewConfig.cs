using DocumentFormat.OpenXml.Drawing.Charts;
using SIE.Items.Items;
using SIE.MetaModel.View;
using SIE.Web.Items.Items.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Items.Items
{
    public class UnValidFactoryItemViewConfig : WebViewConfig<UnValidFactoryItem>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(UnValidFactoryItem));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll, typeof(UnValidFactoryItemImportCommand).FullName, typeof(UnValidFactoryItemDLTemplateCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).Show().HasLabel("物料编码").UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = dic;
                });
                View.Property(p => p.ItemName).Show().Readonly();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).Show();
            }
        }

        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.Item.Code).HasLabel("物料编码").Show();
        }
    }
}
