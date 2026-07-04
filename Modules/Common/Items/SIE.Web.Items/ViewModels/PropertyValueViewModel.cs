using SIE.Common;
using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using SIE.ObjectModel;
using SIE.Web.Items.ProductBoms.Commands;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.ViewModels
{
    /// <summary>
    /// 属性值ViewModel
    /// </summary>
    [Serializable]
    [RootEntity]
    [Label("产品属性")]
    public class PropertyValueViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PropertyValueViewModel()
        {
            Values = new List<string>();
        }

        #region 物料属性定义 Definition
        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        [Label("属性")]
        public static readonly IRefIdProperty DefinitionIdProperty = P<PropertyValueViewModel>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 物料属性定义Id
        /// </summary>
        public double DefinitionId
        {
            get { return (double)this.GetRefId(DefinitionIdProperty); }
            set { this.SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<PropertyValueViewModel>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 物料属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion


        #region 物料属性值 DefinitionValue
        /// <summary>
        /// 物料属性值Id
        /// </summary>
        [Label("物料属性值")]
        public static readonly IRefIdProperty DefinitionValueIdProperty =
            P<PropertyValueViewModel>.RegisterRefId(e => e.DefinitionValueId, ReferenceType.Normal);

        /// <summary>
        /// 物料属性值Id
        /// </summary>
        public double DefinitionValueId
        {
            get { return (double)this.GetRefId(DefinitionValueIdProperty); }
            set { this.SetRefId(DefinitionValueIdProperty, value); }
        }

        /// <summary>
        /// 物料属性值
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyValue> DefinitionValueProperty =
            P<PropertyValueViewModel>.RegisterRef(e => e.DefinitionValue, DefinitionValueIdProperty);

        /// <summary>
        /// 物料属性值
        /// </summary>
        public ItemPropertyValue DefinitionValue
        {
            get { return this.GetRefEntity(DefinitionValueProperty); }
            set { this.SetRefEntity(DefinitionValueProperty, value); }
        }
        #endregion

        #region 属性值 DefinitionName
        /// <summary>
        /// 属性值
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> DefinitionNameProperty = P<PropertyValueViewModel>.RegisterView(e => e.DefinitionName, p => p.DefinitionValue.Definition.Name);

        /// <summary>
        /// 属性值
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
            set { this.SetProperty(DefinitionNameProperty, value); }
        }
        #endregion


        #region 属性值列表 Values 
        /// <summary>
        /// 属性值列表
        /// </summary>
        [Label("属性值列表")]
        public static readonly Property<List<string>> ValuesProperty = P<PropertyValueViewModel>.Register(e => e.Values);

        /// <summary>
        /// 属性值列表
        /// </summary>
        public List<string> Values
        {
            get { return this.GetProperty(ValuesProperty); }
            set { this.SetProperty(ValuesProperty, value); }
        }
        #endregion

        #region 属性值 Value
        /// <summary>
        /// 属性值列表
        /// </summary>
        [Label("属性值")]
        public static readonly Property<string> ValueProperty = P<PropertyValueViewModel>.Register(e => e.Value);

        /// <summary>
        /// 属性值
        /// </summary>
        public string Value
        {
            get { return this.GetProperty(ValueProperty); }
            set { this.SetProperty(ValueProperty, value); }
        }
        #endregion

        #region 父类型 Type
        /// <summary>
        /// 父类型
        /// </summary>
        [Label("父类型")]
        public static readonly Property<Type> TypeProperty = P<PropertyValueViewModel>.Register(e => e.Type);

        /// <summary>
        /// 父类型
        /// </summary>
        public Type Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 父ID ParentId
        /// <summary>
        /// 父ID
        /// </summary>
        [Label("父ID")]
        public static readonly Property<double> ParentIdProperty = P<PropertyValueViewModel>.Register(e => e.ParentId);

        /// <summary>
        /// 父ID
        /// </summary>
        public double ParentId
        {
            get { return this.GetProperty(ParentIdProperty); }
            set { this.SetProperty(ParentIdProperty, value); }
        }
        #endregion

        #region 物料ID ItemId
        /// <summary>
        /// 物料ID
        /// </summary>
        [Label("物料ID")]
        public static readonly Property<double> ItemIdProperty = P<PropertyValueViewModel>.Register(e => e.ItemId);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
            set { this.SetProperty(ItemIdProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重置数据方法
        /// </summary>
        public void ResetData()
        {
            NotifyPropertyChanged(DefinitionIdProperty);
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">属性变更事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == PropertyValueViewModel.DefinitionProperty && e.NewValue != e.OldValue)
            {
                Values.Clear();
            }
        }
    }

    ////[ManagedProperty.CompiledPropertyDeclarer]
    ////internal class PropertyValueViewModelViewConfig : WebViewConfig<PropertyValueViewModel>
    ////{
    ////    public static readonly Property<string> ValueProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("Value", typeof(PropertyValueViewModelViewConfig),
    ////        GetValue, PropertyValueViewModel.ValuesProperty);
    ////    public static string GetValue(PropertyValueViewModel me)
    ////    {
    ////        return string.Join(";", me.Values);
    ////    }

    ////    protected override void ConfigView()
    ////    {
    ////        View.InlineEdit();
    ////        View.UseCommands(typeof(ItemPropertyValueAddCommand), WPFCommandNames.ListEdit);
    ////        using (View.OrderProperties())
    ////        {
    ////            View.Property(p => p.Definition).Show(ShowInWhere.All).Readonly();
    ////            View.Property(p => p.Definition.Name).Show(ShowInWhere.All);
    ////            View.Property(p => p.Value).Show(ShowInWhere.Detail);
    ////            View.Property(PropertyValueViewModelViewConfig.ValueProperty).UseEditor("ItemPropertyEditor").Show(ShowInWhere.List);
    ////        }
    ////    }
    ////}

    /// <summary>
    /// 产品Bom明细视图子视图，产品Bom孙视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BomDetailPropertyValueViewModelViewConfig : WebViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 拓展ViewGroup字符串定义
        /// </summary>
        internal const string BomDetailPropertyValueViewModelView = "BomDetailPropertyValueViewModelViewConfig";

        #region 属性值 Value
        /// <summary>
        /// 属性值字符串
        /// </summary>
        internal static readonly Property<string> BomDetailValueProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("BomDetailValue", typeof(BomDetailPropertyValueViewModelViewConfig),
            GetValue, PropertyValueViewModel.ValuesProperty);

        /// <summary>
        /// 属性值字符串
        /// </summary>
        /// <param name="me">PropertyValueViewModel</param>
        /// <returns>string</returns>
        internal static string GetValue(PropertyValueViewModel me)
        {
            return string.Join(";", me.Values);
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(BomDetailPropertyValueViewModelView);
            if (ViewGroup == BomDetailPropertyValueViewModelView)
            {
                View.InlineEdit();

                using (View.OrderProperties())
                {
                    View.Property(p => p.Definition).UsePagingLookUpEditor().Show(ShowInWhere.All);
                    View.Property(BomDetailPropertyValueViewModelViewConfig.BomDetailValueProperty)/*.UseEditor(PropertyValueEditor.EditorName)*/.HasLabel("值").Show(ShowInWhere.List).Readonly(false);
                }
            }
        }
    }

    /// <summary>
    /// 物料子视图，物料属性视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ItemPropertyValueViewModelViewConfig : WebViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 自定义ViewGroup
        /// </summary>
        internal const string ItemPropertyValueViewModelView = "ItemPropertyValueViewModelViewConfig";

        /// <summary>
        /// 物料属性值
        /// </summary>
        internal static readonly Property<string> ItemValueProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("ItemValue", typeof(ItemPropertyValueViewModelViewConfig), GetValue, PropertyValueViewModel.ValuesProperty);

        /// <summary>
        /// 物料属性值
        /// </summary>
        /// <param name="me">属性值模板</param>
        /// <returns>string</returns>
        internal static string GetValue(PropertyValueViewModel me)
        {
            return string.Join(";", me.Values);
        }

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(ItemPropertyValueViewModelView);

            if (ViewGroup == ItemPropertyValueViewModelView)
            {
                View.DisableEditing();
                using (View.OrderProperties())
                {
                    View.Property(p => p.DefinitionName).Show(ShowInWhere.All);
                    View.Property(p => p.Value).Show(ShowInWhere.Detail);
                    View.Property(ItemPropertyValueViewModelViewConfig.ItemValueProperty).HasLabel("值").Show(ShowInWhere.List);
                }
            }
        }
    }

    /// <summary>
    /// 产品Bom视图，物料属性值子视图
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class BomPropertyValueViewModelViewConfig : WebViewConfig<PropertyValueViewModel>
    {
        /// <summary>
        /// 产品Bom物料属性子视图，ViewGroup
        /// </summary>
        public const string BomPropertyValueViewModelListView = "BomPropertyValueViewModelListView";

        /// <summary>
        /// 产品Bom属性值拓展属性
        /// </summary>
        [Label("值")]
        public static readonly Property<string> BomValueProperty = P<PropertyValueViewModel>.RegisterExtensionReadOnly("BomValue", typeof(BomPropertyValueViewModelViewConfig),
            GetValue, PropertyValueViewModel.ValuesProperty);

        /// <summary>
        /// 产品Bom属性值拓展属性
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
            View.DeclareExtendViewGroup(new string[] { "BomPropertyValueViewModelListView", "GoodsIssuePropertyValueView", "WoOrderPropertyValueView" });
            View.InlineEdit();
            if (ViewGroup == BomPropertyValueViewModelListView)
            {
                View.UseCommands(typeof(BomPropertyValueAddCommand).FullName, WebCommandNames.Edit, WebCommandNames.Delete, typeof(BomPropertyValuesSaveCommand).FullName);
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
                         else
                         {
                             return null;
                         }
                     }).UsePagingLookUpEditor(p => p.XType = "propertyCombox").UseListSetting(e => { e.HelpInfo = "根据物料显示物料属性值"; }).Show(ShowInWhere.All);
                    View.Property(BomPropertyValueViewModelViewConfig.BomValueProperty)
                        .Readonly(false).UseTextButtonFieldEditor(p =>
                        {
                            p.ExtendJsObj = "SIE.Web.Items.ViewModels.PropertyValueEditor";
                            p.Editable = false; p.IsReadonly = false;
                        }).Show(ShowInWhere.List);
                }
            }

        }
    }
}