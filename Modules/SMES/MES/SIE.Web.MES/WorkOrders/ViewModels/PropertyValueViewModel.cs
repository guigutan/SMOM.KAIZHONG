using SIE.Common;
using SIE.Domain;
using SIE.Items;
using SIE.MES.WorkOrders;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Web.Items.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.MES.WorkOrders.ViewModels
{
    /// <summary>
    /// 工单工序BOM属性视图，物料属性值子视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class WoBomPropertyValueViewModelViewConfig : WebViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 工单工序BOM物料属性子视图，ViewGroup
        /// </summary>
        public const string WoBomPropertyValueViewModelListView = "WoBomPropertyValueViewModelListView";

        /// <summary>
        /// 工单工序BOM物料属性子视图，ViewGroup
        /// </summary>
        public const string WoBomPropertyValueReadOnlyView = "WoBomPropertyValueReadOnlyView";

        /// <summary>
        /// 工单工序BOM属性值拓展属性
        /// </summary>
        [Label("值")]
        public static readonly Property<string> WoBomValueProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("WoBomValue", typeof(WoBomPropertyValueViewModelViewConfig),
            GetValue, PropertyValueViewModel.ValuesProperty);

        /// <summary>
        /// 工单工序BOM属性值拓展属性
        /// </summary>
        /// <param name="me">PropertyValueViewModel</param>
        /// <returns>string</returns>
        public static string GetValue(PropertyValueViewModel me)
        {
            return string.Join(";", me.Values);
        }

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(WorkOrder));
            View.DeclareExtendViewGroup(new string[] { "WoBomPropertyValueViewModelListView", "WoBomPropertyValueReadOnlyView" });
            View.InlineEdit();
            if (ViewGroup == WoBomPropertyValueViewModelListView)
            {
                View.UseCommands("SIE.Web.MES.WorkOrders.Commands.AddWoBomPropertyValueCommand", WebCommandNames.Edit, "SIE.Web.MES.WorkOrders.Commands.WorkOrderDetailDelCommand");
                using (View.OrderProperties())
                {
                    View.Property(p => p.Definition).UseDataSource((source, pagingInfo, keyword) =>
                    {
                        var entity = source as PropertyValueViewModel;
                        if (entity != null)
                        {
                            var entitylist = RT.Service.Resolve<ItemController>().GetItemPropertys(entity.ItemId, "", null).
                            Select(m => m.Definition).Distinct((x, y) => x.Name == y.Name).AsEntityList();
                            return entitylist;
                        }
                        return new EntityList<PropertyValueViewModel>();
                    }).UsePagingLookUpEditor((m, e) =>
                    {
                        Dictionary<string, string> keyValues = new Dictionary<string, string>();
                        keyValues.Add(nameof(e.DefinitionName), nameof(e.DefinitionValue.Definition.Name));
                        m.BindDisplayField = PropertyValueViewModel.DefinitionNameProperty.Name;
                        m.DicLinkField = keyValues;
                        m.XType = "propertyCombox";
                    }).Show(ShowInWhere.All)
                    .UseListSetting(e => { e.HelpInfo = "显示当前物料下的物料属性"; });
                    View.Property(WoBomPropertyValueViewModelViewConfig.WoBomValueProperty).Readonly(false).UseTextButtonFieldEditor(p => { p.ExtendJsObj = "SIE.Web.Items.ViewModels.PropertyValueEditor"; p.Editable = false; p.IsReadonly = false; }).Show(ShowInWhere.List);
                }
            }
            if (ViewGroup == WoBomPropertyValueReadOnlyView)
            {
                using (View.OrderProperties())
                {
                    View.Property(p => p.DefinitionName).Readonly(true).Show(ShowInWhere.All);
                    View.Property(WoBomPropertyValueViewModelViewConfig.WoBomValueProperty).Readonly(true).Show(ShowInWhere.All);
                }
            }
        }
    }
}