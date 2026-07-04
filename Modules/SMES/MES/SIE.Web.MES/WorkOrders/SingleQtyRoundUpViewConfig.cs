using DevExpress.XtraRichEdit.Layout;
using SIE.EventMessages.WMS.StereoWarhouses.Datas;
using SIE.MES.WorkOrders._Routing_;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.MES.WorkOrders
{
    public class SingleQtyRoundUpViewConfig : WebViewConfig<SingleQtyRoundUp>
    {
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SingleQtyRoundUp));
        }

        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete, WebCommandNames.Edit, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll).UseImportCommands();
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).HasLabel("物料编码").Show().UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    keyValues.Add(nameof(e.Unit), nameof(e.Item.UnitCode));
                    keyValues.Add(nameof(e.ItemType), nameof(e.Item.Type));
                    keyValues.Add(nameof(e.ItemMtart), nameof(e.Item.Mtart));
                    keyValues.Add(nameof(e.ShortDescription), nameof(e.Item.ShortDescription));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.Unit).Show().Readonly();
                View.Property(p => p.ItemType).Show().Readonly();
                View.Property(p => p.ItemMtart).Show().Readonly();
                View.Property(p => p.ItemCategoryCode).Show().Readonly();
                View.Property(p => p.ItemCategoryName).Show().Readonly();
                View.Property(p => p.ShortDescription).Show().Readonly();
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.Item.Code).Show().HasLabel("物料编码");
            }
        }
    }
}
