using SIE.Items;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Items.Items.Commands;
using System.Collections.Generic;

namespace SIE.Web.Items.Items
{
    /// <summary>
    /// 转换单位视图配置
    /// </summary>
    public class ItemUnitViewConfig : WebViewConfig<ItemUnit>
    {
        /// <summary>
        /// 物料TAB页视图
        /// </summary>
        public const string TabView = "TabView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[] { TabView });
            View.AssignAuthorize(typeof(ItemUnit));
            if (ViewGroup == TabView)
            {
                ConfigTabView();
            }
        }

        ///<summary>
        /// 配置TAB视图
        /// </summary>
        private void ConfigTabView()
        {
            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands(typeof(AddItemUnitCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Unit).Show(ShowInWhere.All).UseDataSource((o, c, r) =>
                {
                    var criteria = o as ItemUnit;
                    return RT.Service.Resolve<ItemController>().GetUnitList(criteria.MainUnitId, r, c);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.UnitType), nameof(e.Unit.Type));
                    m.DicLinkField = keyValues;
                });
                View.Property(p => p.UnitType).Show(ShowInWhere.All).UseCatalogEditor(p => { p.CatalogReloadData = true; p.CatalogType = Unit.CatalogType; }).Readonly();
                View.Property(p => p.Numerator).Show(ShowInWhere.All).UseListSetting(e => { e.HelpInfo = "主单位数量×分子=辅助单位数量×分母"; });
                View.Property(p => p.Denominator).Show(ShowInWhere.All);
                View.Property(p => p.IsDefault).Readonly().Show(ShowInWhere.All);
            }
        }
    }
}
