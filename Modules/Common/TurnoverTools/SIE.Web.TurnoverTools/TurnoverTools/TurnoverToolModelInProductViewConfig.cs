using SIE.TurnoverTools.TurnoverTools;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.Elec.MES.TurnoverTools
{
    /// <summary>
    /// 产品容量视图配置
    /// </summary>
    internal class TurnoverToolModelInProductViewConfig : WebViewConfig<TurnoverToolModelInProduct>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.Property(p => p.ProductId).ShowInList(width: 150).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.ProductId), nameof(e.Product.Id));
                keyValues.Add(nameof(e.ProductCode), nameof(e.Product.Code));
                keyValues.Add(nameof(e.ProductName), nameof(e.Product.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("物料编码");
            View.Property(p => p.ProductName).ShowInList(width: 150).HasLabel("物料名称").Readonly();
            View.Property(p => p.Capacity);
        }
    }
}