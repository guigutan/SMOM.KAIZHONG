using SIE.Domain;
using SIE.Items;
using SIE.Items.ProductModels;
using SIE.MetaModel.View;
using SIE.Web.Items.ProductModels.Commands;

namespace SIE.Web.Items
{
    /// <summary>
    /// 产品机型视图配置
    /// </summary>
    class ProductModelViewConfig : WebViewConfig<ProductModel>
    {
        /// <summary>
        /// 与产品族列表关联的产品机型 ViewGroup字符串定义
        /// </summary>
        public const string ModelWithFamilyView = " ModelWithFamilyView";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ModelWithFamilyView);
            if (ViewGroup == ModelWithFamilyView)
            {
                ConfigModelWithFamilyView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.InlineEdit().UseDefaultCommands();
            View.UseCommands(typeof(ProductModelImportCommand).FullName);
            View.ReplaceCommands(WebCommandNames.Copy, "SIE.Web.Items.ProductModels.Commands.ProductModelCopyCommand");
            View.Property(p => p.Code).ShowInList(150).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name).ShowInList(150);
            View.Property(p => p.WorkingHours).ShowInList(130).UseSpinEditor(p => { p.MinValue = 0.01; p.MaxValue = 36000; });
            View.Property(p => p.SendingHours).ShowInList(130).UseSpinEditor(p => { p.MinValue = 0.01; p.MaxValue = 36000; });
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
            View.AssociateChildrenProperty(ProductModelLineCapacityDetailProperty.ProductModelLineCapacityListProperty, c =>
            {
                var arg = c as ChildPagingDataArgs;
                var productModel = arg.Parent as ProductModel;
                if (productModel == null)
                    return new EntityList<ProductModelLineCapacity>();
                return RT.Service.Resolve<ProductModelController>().GetProductModelLineCapacities(productModel.Id, arg.SortInfo, arg.PagingInfo);
            }).HasLabel("产线产能");
            View.ChildrenProperty(p => p.SkillList);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code); //编码
            View.Property(p => p.Name); //名称
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
            View.Property(p => p.ProductFamilyCode);
            View.Property(p => p.ProductFamilyName);
        }

        /// <summary>
        /// 与产品族列表关联的产品机型视图配置
        /// </summary>
        protected void ConfigModelWithFamilyView()
        {
            View.AssignAuthorize(typeof(ProductFamily));
            View.UseCommands("SIE.Web.Items.ProductModels.Commands.SelectProductModel", typeof(ProductModelDeleteCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).ShowInList(150).Readonly();
                View.Property(p => p.Name).ShowInList(150).Readonly();
                View.Property(p => p.WorkingHours).Readonly().ShowInList(130).UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 36000; });
                View.Property(p => p.SendingHours).Readonly().ShowInList(130).UseSpinEditor(p => { p.MinValue = 0; p.MaxValue = 36000; });
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.SkillList).Show(ChildShowInWhere.Hide);
            }
        }
    }
}