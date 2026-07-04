using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.Wpf.Items;
using SIE.Wpf.Items.ViewModels;
using SIE.Wpf.MES.WorkOrders.Commands;

namespace SIE.Wpf.MES.WorkOrders
{
    /// <summary>
    /// 工单BOM替代料视图配置
    /// </summary>
    internal class WorkOrderBomPropertyValueViewConfig : WPFViewConfig<WorkOrderBomPropertyValue>
    {
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            using (View.OrderProperties())
            {
                //View.Property(p => p.Definition).Show(ShowInWhere.All).UseEditor(WPFEditorNames.EntityDropDown).UseDataSource((e, c) =>
                //{
                //    var workOrderValue = e as WorkOrderPropertyValue;
                //    return RT.Service.Resolve<ItemController>().GetItemPropertys(workOrderValue.WorkOrder.ProductId);
                //});
                //View.Property(p => p.Definition.Name).Show(ShowInWhere.All).Readonly();

                ////选择属性值
                //View.Property(p => p.Value).UseSelectionViewMeta(new SelectionViewMeta
                //{
                //    SelectionMode = MetaModel.EntitySelectionMode.Single,
                //    SelectionEntityType = typeof(ItemPropertyValue),
                //    SelectedValuePath = ItemPropertyValue.ValueProperty,
                //    DataSourceProvider = (e, c) =>
                //    {
                //        var workOrderPropertyValue = e as WorkOrderPropertyValue;
                //        return RT.Service.Resolve<ItemController>().GetItemPropertyValues(workOrderPropertyValue.DefinitionId);
                //    }
                //}).UseEditor(WPFEditorNames.EntityDropDown).Show(ShowInWhere.All);
            }
        }
    }

    /// <summary>
    /// 工单BOM属性值拓展视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class WorkOrderBomPropertyValueExtendViewConfig : WPFViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 工单BOM属性值拓展配置视图
        /// </summary>
        public const string WorkOrderBomPropertyValueExtendView = "WorkOrderBomPropertyValueExtendView";

        /// <summary>
        /// 工单BOM只读属性扩展视图
        /// </summary>
        public const string WorkOrderBomPropertyValueReadonlyView = "WorkOrderBomPropertyValueReadonlyView";

        /// <summary>
        /// 显示值
        /// </summary>
        public static readonly Property<string> DisplayValues1Property = P<PropertyValueViewModel>.RegisterExtensionReadOnly("DisplayValues1", typeof(WorkOrderBomPropertyValueExtendViewConfig),
            GetDisplayValues1, PropertyValueViewModel.DefinitionIdProperty);

        /// <summary>
        /// 获取显示值
        /// </summary>
        /// <param name="me">属性值ViewModel</param>
        /// <returns>显示值</returns>
        public static string GetDisplayValues1(PropertyValueViewModel me)
        {
            string result = string.Empty;
            foreach (var value in me.Values)
            {
                result += value + ";";
            }

            return result;
        }

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(WorkOrderBomPropertyValueExtendView, WorkOrderViewConfig.ReadonlyView);
            if (ViewGroup == WorkOrderBomPropertyValueReadonlyView)
            {
                ReadOnlyConfigView();
            }
            else if (ViewGroup == WorkOrderBomPropertyValueExtendView)
            {
                WorkOrderBomPropertyValueExtendConfigView();
            }
        }

        /// <summary>
        /// 工单BOM属性值拓展配置视图
        /// </summary>
        void WorkOrderBomPropertyValueExtendConfigView()
        {
            View.InlineEdit();
            View.UseCommands(typeof(AddWoBomPropertyValueCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete);
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).UseItemPropertyDefinitionEditor().Show(ShowInWhere.All);
                View.Property(DisplayValues1Property).UsePropertyValueEditor().HasLabel("值").Show(ShowInWhere.List).Readonly(false);
            }
        }

        /// <summary>
        /// 工单BOM属性值拓展配置只读视图
        /// </summary>
        void ReadOnlyConfigView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Definition).UseItemPropertyDefinitionEditor().Show(ShowInWhere.All);
                View.Property(DisplayValues1Property).UsePropertyValueEditor().HasLabel("值").Show(ShowInWhere.List);
            }
        }
    }
}
