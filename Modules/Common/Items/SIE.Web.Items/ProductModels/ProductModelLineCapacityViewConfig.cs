using SIE.Items;
using SIE.MetaModel.View;
using SIE.Web.Items.ProductModels.Commands;
using System.Collections.Generic;

namespace SIE.Web.Items
{
    /// <summary>
    /// 产线产能视图配置
    /// </summary>
    internal class ProductModelLineCapacityViewConfig : WebViewConfig<ProductModelLineCapacity>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
		protected override void ConfigView()
        {
            View.InlineEdit();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(LineCapacityAddCommand).FullName);
            View.UseCommands(WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.ExportXls);
            View.Property(p => p.Resource).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add(nameof(e.ResourceName), nameof(e.Resource.Name));
                m.DicLinkField = dic;
            }).HasLabel("资源");
            View.Property(p => p.ResourceName).Readonly();
            View.Property(p => p.WorkingHours).ShowInList(160).UseSpinEditor(e => e.MinValue = 0);
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
        }
    }
}
