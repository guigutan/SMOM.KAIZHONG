using SIE.Core.Items;
using SIE.KZ.Base.SmomControl;
using SIE.MetaModel.View;
using SIE.OSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Item = SIE.Core.Items.Item;

namespace SIE.Web.KZ.Base.SmomControl
{
    public class SyncItemLabelViewConfig : WebViewConfig<SyncItemLabel>
    {
        protected override void ConfigView()
        {
            base.ConfigView();
        }


        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll).UseImportCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).Show().UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = dic;
                    m.SearchFieldList.Add(Item.CodeProperty.Name);
                    m.SearchFieldList.Add(Item.NameProperty.Name);
                });
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.SourceFactory).Show();
                View.Property(p => p.ToFactory).Show();
            }
        }

        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).Show();
                View.Property(p => p.SourceFactory).Show();
                View.Property(p => p.ToFactory).Show();
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.Item.Code).Show().HasLabel("物料编码");
                View.Property(p => p.SourceFactory).Show().HasLabel("来源工厂");
                View.Property(p => p.ToFactory).Show().HasLabel("目标工厂");
            }
        }
    }
}
