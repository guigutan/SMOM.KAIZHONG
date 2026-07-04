using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel.View;

namespace SIE.Web.Items
{
    /// <summary>
    /// 物料小类视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class ItemSmallCategoryViewConfig : WebViewConfig<ItemSmallCategory>
    {
        /// <summary>
        /// 添加工序BOM列表视图
        /// </summary>
        public static readonly string AddProcessBomView = "AddProcessBomView";

        #region 父级节点描述 Parents
        /// <summary>
        /// 父级节点描述
        /// </summary>
        public static readonly Property<string> ParentsProperty = P<ItemSmallCategory>.RegisterExtensionReadOnly("Parents", typeof(ItemSmallCategoryViewConfig),
            GetParents, ItemSmallCategory.TreePIdProperty);

        /// <summary>
        /// 父级节点描述
        /// </summary>
        /// <param name="me">ItemSmallCategory</param>
        /// <returns>string</returns>
        public static string GetParents(ItemSmallCategory me)
        {
            string parents = string.Empty;
            if (me.TreePId == null)
                return parents;
            var category = RF.Find<ItemCategory>().GetById(me.TreePId) as ItemCategory;
            parents = FindParent(category, string.Empty);
            return parents.TrimEnd('>').TrimEnd('-');
        }

        /// <summary>
        /// FindParent
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="parents">parents</param>
        /// <returns>string</returns>
        static string FindParent(ItemCategory category, string parents)
        {
            if (category == null)
                return parents;
            parents = category.Name + "->" + parents;
            return FindParent(RF.Find<ItemCategory>().GetById(category.TreePId) as ItemCategory, parents);
        }
        #endregion

        //#region 分类层级 LevelName
        ///// <summary>
        ///// 分类层级
        ///// </summary>
        //public static readonly Property<string> LevelNameProperty = P<ItemSmallCategory>.RegisterExtensionReadOnly("LevelName", typeof(ItemSmallCategoryViewConfig),
        //    GetLevelName, ItemSmallCategory.IdProperty);

        ///// <summary>
        ///// 分类层级
        ///// </summary>
        ///// <param name="me">ItemSmallCategory</param>
        ///// <returns>string</returns>
        //public static string GetLevelName(ItemSmallCategory me)
        //{
        //    return me.Level?.Name;
        //}
        //#endregion 

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.RemoveCommands(WebCommandNames.CustomizeUI);

            if (ViewGroup == AddProcessBomView)
                ConfigAddProcessBomView();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(ParentsProperty).HasLabel("父级关系");
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Level).Show(ShowInWhere.Hide);
            View.Property(p => p.LevelName).HasLabel("分类层级");
            View.Property(ParentsProperty).HasLabel("父级关系");
            View.Property(p => p.Type);
        }

        /// <summary>
        /// 分类添加工序Bom列表视图
        /// </summary>
        void ConfigAddProcessBomView()
        {
            View.ClearCommands();
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(ParentsProperty).HasLabel("父级关系").Show();
            View.AttachChildrenProperty(typeof(Item), (w) =>
            {
                var arg = w as ChildPagingDataArgs;
                var entity = arg.Parent as ItemSmallCategory;
                if (entity == null)
                    return new EntityList<Item>();

                var criteria = new ItemCriteria() { ItemCategoryId = entity.Id, PagingInfo = arg.PagingInfo, UpdateDate = new ObjectModel.DateRange { DateRangeType = ObjectModel.DateRangeType.All } };
                return RT.Service.Resolve<ItemController>().GetItems(criteria);
            }, ItemViewConfig.SmallCategoryView, true).Show(ChildShowInWhere.All);
        }
    }
}