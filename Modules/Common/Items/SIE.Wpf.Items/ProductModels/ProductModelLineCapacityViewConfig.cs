using SIE.Items;
using SIE.Wpf.Items.ProductModels.Commands;
using SIE.Wpf.Resources;

namespace SIE.WPF.Items
{
    /// <summary>
    /// 产线产能视图配置
    /// </summary>
    internal class ProductModelLineCapacityViewConfig : WPFViewConfig<ProductModelLineCapacity>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
		protected override void ConfigView()
        {
            View.InlineEdit().UseDefaultBehaviors();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(AddProductModelCapacityCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, WPFCommandNames.Export);

            using (View.OrderProperties())
            {
                View.Property(p => p.Resource).UseEnterpriseEquipmentResourceEditor();
                View.Property(p => p.Resource.Name).HasLabel("资源名称");
                View.Property(p => p.WorkingHours).UseSpinEditor(p => { p.MinValue = (decimal)0.01; p.MaxValue = 36000; });
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
