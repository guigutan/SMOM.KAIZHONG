using SIE.Domain;
using SIE.Items;
using SIE.Items.Items;
using SIE.Web.Items.Items.Commands;
using System.Collections.Generic;

namespace SIE.Web.Items.Items
{
    /// <summary>
	/// 产品等级视图配置
	/// </summary>
	public class ProductGradeViewConfig : WebViewConfig<ProductGrade>
    {
        /// <summary>
        /// 物料维护产品等级ViewGroup
        /// </summary>
        internal const string ItemProductGradeView = "ItemProductGradeView";

        /// <summary>
        /// 选择按钮弹出的产品等级ViewGroup
        /// </summary>
        internal const string ButtonSelectView = "ButtonSelectViewConfig";

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ItemProductGradeView, ButtonSelectView);
            if (ViewGroup == ItemProductGradeView)
            {
                ItemProductGradeTabView();
            }
            else if (ViewGroup == ButtonSelectView)
            {
                ButtonSelectPopView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.FormEdit();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Describe);
            View.Property(p => p.Item).HasLabel("物料");
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Describe);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code).Show(ShowInWhere.All);
            View.Property(p => p.Name).Show(ShowInWhere.All);
        }

        /// <summary>
        /// 物料维护--产品等级视图
        /// </summary>
        protected void ItemProductGradeTabView()
        {
            View.InlineEdit();
            View.ClearCommands();
            View.UseCommands(typeof(ProductGradeLookupCommand).FullName, typeof(ProductGradeDeleteCommand).FullName);
            View.Property(p => p.Code).Show(ShowInWhere.All);
            View.Property(p => p.Name).Show(ShowInWhere.All);
            View.Property(p => p.Describe).Show(ShowInWhere.All);
            View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 选择按钮弹出的产品等级视图
        /// </summary>
        protected void ButtonSelectPopView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show(ShowInWhere.All);
                View.Property(p => p.Name).Show(ShowInWhere.All);
                View.Property(p => p.Describe).Show(ShowInWhere.All);
                View.Property(p => p.UpdateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateBy).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            }
        }
    }

    /// <summary>
    /// 物料维护--产品等级页签视图类
    /// </summary>
    internal class ProductGradeExtViewConfig : WebViewConfig<Item>
    {
        /// <summary>
        /// 物料扩展视图--产品等级列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.AttachChildrenProperty(typeof(ProductGrade), (e) =>
            {
                var curItem = e.Parent as Item;
                var argsPaging = e as ChildPagingDataArgs;
                var productGrades = new EntityList<ProductGrade>();
                if (curItem != null)
                {
                    productGrades = RT.Service.Resolve<ItemController>().GetProductGrades(curItem.Id, argsPaging?.PagingInfo, (List<OrderInfo>)argsPaging?.SortInfo);
                }

                return productGrades;
            }, ProductGradeViewConfig.ItemProductGradeView, true).OrderNo = 50;
        }
    }
}
