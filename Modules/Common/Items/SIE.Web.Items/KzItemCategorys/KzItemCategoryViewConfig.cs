using DocumentFormat.OpenXml.Drawing.Charts;
using SIE.Items.KzItemCategorys;
using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Items.KzItemCategorys
{
    public class KzItemCategoryViewConfig : WebViewConfig<KzItemCategory>
    {
        protected override void ConfigView()
        {
            base.ConfigView();
            View.AssignAuthorize(typeof(KzItemCategory));
        }

        protected override void ConfigListView()
        {
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.Save, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll).UseImportCommands();
                View.Property(p => p.ItemId).Show().UsePagingLookUpEditor((m, e) => {
                    var dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.ItemName), nameof(e.Item.Name));
                    m.DicLinkField = dic;
                });
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.KzCategoryId).Show().HasLabel("工艺属性分类编码").UsePagingLookUpEditor((m, e) =>
                {
                    var dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.KzCategoryName), nameof(e.KzCategory.Name));
                    m.DicLinkField = dic;
                }); ;
                View.Property(p => p.KzCategoryName).Show().Readonly();
            }
        }

        protected override void ConfigQueryView()
        {
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).Show();
                View.Property(p => p.KzCategoryId).Show();
            }
        }

        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.Item.Code).Show().HasLabel("物料编码");
                View.PropertyRef(p => p.KzCategory.Code).Show().HasLabel("工艺属性分类编码");
            }
        }

        protected override void ConfigSelectionView()
        {
            View.UseClientOrder();
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemId).Show().Readonly();
                View.Property(p => p.ItemName).Show().Readonly();
                View.Property(p => p.KzCategoryCode).Show().Readonly();
                View.Property(p => p.KzCategoryName).Show().Readonly();
            }
        }
    }
}
