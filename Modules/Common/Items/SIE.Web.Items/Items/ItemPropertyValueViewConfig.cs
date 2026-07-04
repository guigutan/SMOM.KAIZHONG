using SIE.Items;
using SIE.Web.Items.Items.Commands;

namespace SIE.Web.Items
{
    /// <summary>
    /// 物料属性值视图配置
    /// </summary>
    internal class ItemPropertyValueViewConfig : WebViewConfig<ItemPropertyValue>
    {
        internal const string workorderView = "WorkOrderPropertyValueExtendView";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DisableEditing();
            View.ClearCommands();
            if (ViewGroup == workorderView)
                WorkorderSelectionView();
            if (ViewGroup == "SelectList")
                ItemPropertyValueView();
        }

        /// <summary>
        /// 默认表格视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            ////View.AddBehavior("SIE.Web.Items.Items.Behaviors.ItemPropertyValueBehavior");
            View.UseGridSelectionModel();
            View.EnableEditing().FormEdit();
            View.UseCommands(typeof(ItemPropertyValueAddCommand).FullName, typeof(ItemPropertyValueDeleteCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition);
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

        /// <summary>
        /// 下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Value);
            View.Property(p => p.DefinitionName).HasLabel("属性");
        }

        /// <summary>
        /// 工单下拉视图
        /// </summary>
        void WorkorderSelectionView()
        {
            View.Property(p => p.Value);
        }

        /// <summary>
        /// 扩展属性 弹出框 界面 
        /// </summary>
        void ItemPropertyValueView()
        {
            View.UseGridSelectionModel();
            using (View.OrderProperties())
            {
                View.Property(p => p.DefinitionName).HasLabel("物料属性名称").Show(); 
                View.Property(p => p.Value).Show();
                View.Property(p => p.PropertyGroup).Show(); 
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}
