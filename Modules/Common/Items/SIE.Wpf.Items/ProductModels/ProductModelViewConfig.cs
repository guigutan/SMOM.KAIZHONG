using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductModels;
using SIE.Wpf.Items.ProductModels.Commands;
using System;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 产品机型视图配置
    /// </summary>
    class ProductModelViewConfig : WPFViewConfig<ProductModel>
    {
        /// <summary>
        /// 与产品族列表关联的产品机型 ViewGroup字符串定义
        /// </summary>
        public const string FamilyCategoryView = " FamilyCategoryView";

        /// <summary>
        /// 选择按钮弹出的产品机型  ViewGroup字符串定义
        /// </summary>
        public const string ButtonSelectView = "ButtonSelectViewConfig";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(FamilyCategoryView, ButtonSelectView);
            if (ViewGroup == FamilyCategoryView)
            {
                ConfigFamilyCategoryView();
            }
            else if (ViewGroup == ButtonSelectView)
            {
                ConfigButtonSelectView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit()
                .UseDefaultBehaviors()
                .UseDefaultCommands();
            View.UseCommands(typeof(ProductModelExcleImportCommand));
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.WorkingHours).UseSpinEditor(p => { p.MinValue = (Decimal)0.01; p.MaxValue = 36000; });
            View.Property(p => p.SendingHours).UseSpinEditor(p => { p.MinValue = (Decimal)0.01; p.MaxValue = 36000; });
            View.AssociateChildrenProperty(ProductModelLineCapacityDetailProperty.ProductModelLineCapacityListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var productModel = arg.Parent as ProductModel;
                if (productModel == null)
                    return new EntityList<ProductModelLineCapacity>();
                return RT.Service.Resolve<ProductModelController>().GetProductModelLineCapacities(productModel.Id, arg.SortInfo, arg.PagingInfo);
            }).HasLabel("产线产能");
            View.ChildrenProperty(p => p.SkillList).Show(ChildShowInWhere.Hide);
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
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.WorkingHours);
            View.Property(p => p.SendingHours);
            View.Property(p => p.ProductFamily).HasLabel("产品族");
        }

        /// <summary>
        /// 与产品族列表关联的产品机型视图配置
        /// </summary>
        protected void ConfigFamilyCategoryView()
        {
            View.UseCommands(typeof(ProductModelLookUpCommand), typeof(ProductModelDeleteCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList();
                View.Property(p => p.Name).ShowInList();
                View.Property(p => p.WorkingHours).ShowInList().UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 36000; });
                View.Property(p => p.SendingHours).ShowInList().UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 36000; });
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.SkillList).Show(ChildShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 选择按钮弹出的产品机型视图配置
        /// </summary>
        protected void ConfigButtonSelectView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList();
                View.Property(p => p.Name).ShowInList();
                View.Property(p => p.WorkingHours).ShowInList().UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 36000; });
                View.Property(p => p.SendingHours).ShowInList().UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 36000; });
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.SkillList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}