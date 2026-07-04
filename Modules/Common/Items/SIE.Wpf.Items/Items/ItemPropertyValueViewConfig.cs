using SIE.Items;
using SIE.Wpf.Command;

namespace SIE.Wpf.Items
{
    /// <summary>
    /// 物料属性值视图配置
    /// </summary>
    internal class ItemPropertyValueViewConfig : WPFViewConfig<ItemPropertyValue>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DisableEditing();
            View.ClearCommands();
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(Items.Commands.ItemPropertyValueAddCommand), typeof(ListDeleteCommand));
            View.FormEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.DefinitionName);
                View.Property(p => p.Value);
                View.Property(p => p.PropertyGroup);
            }
        }

        /// <summary>
        /// 默认表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.DefinitionName);
                View.Property(p => p.Value);
            }
        }

        /// <summary>
        /// 默认查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.DefinitionName);
                View.Property(p => p.Value);
            }
        }
    }
}
