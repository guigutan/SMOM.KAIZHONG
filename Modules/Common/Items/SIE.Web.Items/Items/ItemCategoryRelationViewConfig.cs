using SIE.Domain;
using SIE.Items;
using SIE.MetaModel.View;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.Items
{
    /// <summary>
    /// 物料与分类关系表 配置视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemCategoryRelationViewConfig : WebViewConfig<ItemCategoryRelation>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("质量分类");
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit();
            View.UseCommands(WebCommandNames.Edit);
            View.Property(p => p.Type).UseEnumEditor().Readonly();
            View.Property(p => p.ItemCategory).UseDataSource((e, c, r) =>
            {
                var itemCateRel = e as ItemCategoryRelation;
                if (itemCateRel != null)
                {
                    var list = RT.Service.Resolve<ItemController>().GetItemSmallCategory(itemCateRel.Type, itemCateRel.Item.Type, r, c);
                    list.ForEach(p => p.TreePId = null);

                    return list;
                }
                else
                {
                    return new EntityList<ItemCategory>();
                }
            }).UsePagingLookUpEditor((m, r) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(r.ItemCategoryName), nameof(r.ItemCategory.Name));
                m.DicLinkField = keyValues;
            }).HasLabel("分类编码").Readonly(p => p.PersistenceStatus == PersistenceStatus.New)
            .UseListSetting(e => { e.HelpInfo = "显示选择分类型的物料小类,新增才可编辑"; });
            View.Property(p => p.ItemCategoryName).HasLabel("分类名称").Readonly();
        }
    }
}
