using SIE.Common.Catalogs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items.ViewModels
{
    /// <summary>
    /// 物料属性 模板
    /// </summary>
    [RootEntity, Serializable]
    public class ItemPropertyViewModel : ViewModel
    {
        /// <summary>
        /// 无参构造函数
        /// </summary>
        public ItemPropertyViewModel()
        {
            Values = new List<string>();
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="propertyValue">物料属性值</param>
        public ItemPropertyViewModel(ItemPropertyValue propertyValue)
        {
            PropertyValue = propertyValue;
            Definition = propertyValue.Definition;
            if (Definition?.PropertyType == ItemPropertyType.Catalog)
                Catalog = Definition.CatalogType.CatalogList.FirstOrDefault(p => p.Name == PropertyValue.Value);
            if (Definition?.PropertyType == ItemPropertyType.Number)
                NumberValue = Convert.ToInt32(PropertyValue.Value);
            if (Definition?.PropertyType == ItemPropertyType.Text)
                Value = PropertyValue.Value;
        }

        /// <summary>
        /// 根据输入属性类型自动识别ItemPropertyViewModel中的哪个字段需要复制
        /// </summary>
        public void AcceptChanges()
        {
            if (Definition == null)
                throw new ValidationException("属性不能为空".L10N());
            PropertyValue.Definition = Definition;
            PropertyValue.Name = Definition.Name;
            if (IsCatalog)
                PropertyValue.Value = Catalog.Name;
            if (IsNumber)
                PropertyValue.Value = NumberValue.ToString();
            if (IsString)
                PropertyValue.Value = Value;
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "DefinitionId")
            {
                NotifyPropertyChanged(IsCatalogProperty);
                NotifyPropertyChanged(IsStringProperty);
                NotifyPropertyChanged(IsNumberProperty);
            }
        }

        #region  PropertyValue
        /// <summary>
        /// 物料属性值
        /// </summary>
        [Label("物料属性值")]
        public static readonly Property<ItemPropertyValue> PropertyValueProperty = P<ItemPropertyViewModel>.Register(e => e.PropertyValue);

        /// <summary>
        /// 物料属性值
        /// </summary>
        public ItemPropertyValue PropertyValue
        {
            get { return this.GetProperty(PropertyValueProperty); }
            set { this.SetProperty(PropertyValueProperty, value); }
        }
        #endregion

        #region 属性定义 Definition
        /// <summary>
        /// 属性定义ID
        /// </summary>
        [Label("属性定义")]
        public static readonly IRefIdProperty DefinitionIdProperty = P<ItemPropertyViewModel>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

        /// <summary>
        /// 属性定义ID
        /// </summary>
        public double DefinitionId
        {
            get { return (double)this.GetRefId(DefinitionIdProperty); }
            set { this.SetRefId(DefinitionIdProperty, value); }
        }

        /// <summary>
        /// 属性定义
        /// </summary>
        public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty =
            P<ItemPropertyViewModel>.RegisterRef(e => e.Definition, DefinitionIdProperty);

        /// <summary>
        /// 属性定义
        /// </summary>
        public ItemPropertyDefinition Definition
        {
            get { return this.GetRefEntity(DefinitionProperty); }
            set { this.SetRefEntity(DefinitionProperty, value); }
        }
        #endregion

        #region 名称 DefinitionName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> DefinitionNameProperty = P<ItemPropertyViewModel>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
        }
        #endregion


        #region Item 物料
        /// <summary>
        /// 物料ID
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<ItemPropertyViewModel>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get { return (double)this.GetRefId(ItemIdProperty); }
            set { this.SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        public static readonly RefEntityProperty<Item> ItemProperty = P<ItemPropertyViewModel>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return this.GetRefEntity(ItemProperty); }
            set { this.SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region IsCatalog 是否快码类型
        /// <summary>
        /// 是否快码类型
        /// </summary>
        [Label("是否快码类型")]
        public static readonly Property<bool> IsCatalogProperty = P<ItemPropertyViewModel>.RegisterReadOnly(
            e => e.IsCatalog, e => e.GetIsCatalog(), DefinitionIdProperty);

        /// <summary>
        /// 是否快码类型
        /// </summary>
        public bool IsCatalog
        {
            get { return this.GetProperty(IsCatalogProperty); }
        }

        /// <summary>
        /// 获取是否快码类型
        /// </summary>
        /// <returns>bool</returns>
        private bool GetIsCatalog()
        {
            return Definition?.PropertyType == ItemPropertyType.Catalog;
        }
        #endregion

        #region 是否文本
        /// <summary>
        /// 是否文本
        /// </summary>
        [Label("是否文本")]
        public static readonly Property<bool> IsStringProperty = P<ItemPropertyViewModel>.RegisterReadOnly(
             e => e.IsString, e => e.GetIsString(), DefinitionIdProperty);

        /// <summary>
        /// 是否文本
        /// </summary>
        public bool IsString
        {
            get { return this.GetProperty(IsStringProperty); }
        }

        /// <summary>
        /// 获取是否文本
        /// </summary>
        /// <returns>bool</returns>
        private bool GetIsString()
        {
            return Definition?.PropertyType == ItemPropertyType.Text;
        }
        #endregion

        #region 是否数字
        /// <summary>
        /// 是否数字
        /// </summary>
        [Label("是否数字")]
        public static readonly Property<bool> IsNumberProperty = P<ItemPropertyViewModel>.RegisterReadOnly(
             e => e.IsNumber, e => e.GetIsNumber(), DefinitionIdProperty);

        /// <summary>
        /// 是否数字
        /// </summary>
        public bool IsNumber
        {
            get { return this.GetProperty(IsNumberProperty); }
        }

        /// <summary>
        /// 获取是否数字
        /// </summary>
        /// <returns>bool</returns>
        private bool GetIsNumber()
        {
            return Definition?.PropertyType == ItemPropertyType.Number;
        }
        #endregion

        #region ItemPropertyValue 物料属性值/字符串值
        /// <summary>
        /// 物料属性值
        /// </summary>
        [Label("物料属性值")]
        public static readonly Property<string> ItemPropertyValueProperty = P<ItemPropertyViewModel>.Register(e => e.Value);

        /// <summary>
        /// 物料属性值
        /// </summary>
        public string Value
        {
            get { return this.GetProperty(ItemPropertyValueProperty); }
            set { this.SetProperty(ItemPropertyValueProperty, value); }
        }

        #endregion

        #region 数字值
        /// <summary>
        /// 数字值
        /// </summary>
        [MinValue(0)]
        [Label("数字值")]
        public static readonly Property<int> NumberValueProperty = P<ItemPropertyViewModel>.Register(e => e.NumberValue);

        /// <summary>
        /// 数字值
        /// </summary>
        public int NumberValue
        {
            get { return this.GetProperty(NumberValueProperty); }
            set { this.SetProperty(NumberValueProperty, value); }
        }
        #endregion

        #region 属性组
        /// <summary>
        /// 属性组
        /// </summary>
        [Label("属性组")]
        public static readonly Property<string> PropertyGroupProperty = P<ItemPropertyViewModel>.Register(e => e.PropertyGroup);

        /// <summary>
        /// 属性组
        /// </summary>
        public string PropertyGroup
        {
            get { return this.GetProperty(PropertyGroupProperty); }
            set { this.SetProperty(PropertyGroupProperty, value); }
        }
        #endregion

        #region ItemPropertyDefinitionList 物料属性定义列表
        /// <summary>
        /// 物料属性定义 列表
        /// </summary>
        [Label("物料属性定义列表")]
        public static readonly Property<EntityList<ItemPropertyDefinition>> ItemPropertyDefinitionListProperty = P<ItemPropertyViewModel>.RegisterReadOnly(e => e.ItemPropertyDefinitionList, e => e.GetItemPropertyDefinitionList());

        /// <summary>
        /// 物料属性定义 列表
        /// </summary>
        public EntityList<ItemPropertyDefinition> ItemPropertyDefinitionList
        {
            get { return this.GetProperty(ItemPropertyDefinitionListProperty); }
        }

        /// <summary>
        /// 物料属性定义 列表
        /// </summary>
        /// <returns>物料属性定义列表</returns>
        private EntityList<ItemPropertyDefinition> GetItemPropertyDefinitionList()
        {
            var list = new EntityList<ItemPropertyDefinition>();
            ////list.AddRange(Item.PropertyValueList.Select(p => p.Definition));
            return list;
        }
        #endregion

        #region Catalog 快码 快码值
        /// <summary>
        /// 快码 快码值
        /// </summary>
        [Label("快码值")]
        public static readonly IRefIdProperty CatalogIdProperty = P<ItemPropertyViewModel>.RegisterRefId(e => e.CatalogId, ReferenceType.Normal);

        /// <summary>
        /// 快码 快码值
        /// </summary>
        public double CatalogId
        {
            get { return (double)this.GetRefId(CatalogIdProperty); }
            set { this.SetRefId(CatalogIdProperty, value); }
        }

        /// <summary>
        /// 快码
        /// </summary>
        public static readonly RefEntityProperty<Catalog> CatalogProperty =
            P<ItemPropertyViewModel>.RegisterRef(e => e.Catalog, CatalogIdProperty);

        /// <summary>
        /// 快码
        /// </summary>
        public Catalog Catalog
        {
            get { return this.GetRefEntity(CatalogProperty); }
            set { this.SetRefEntity(CatalogProperty, value); }
        }
        #endregion

        #region 值 列表
        /// <summary>
        /// 属性值列表
        /// </summary>
        [Label("属性值列表")]
        public static readonly Property<List<string>> ValuesProperty = P<ItemPropertyViewModel>.Register(e => e.Values);

        /// <summary>
        /// 属性值列表
        /// </summary>
        public List<string> Values
        {
            get { return this.GetProperty(ValuesProperty); }
            set { this.SetProperty(ValuesProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 物料属性 ViewModel 实体配置
    /// </summary>
    internal class ItemPropertyViewModelConfig : EntityConfig<ItemPropertyViewModel>
    {
        /// <summary>
        /// 重写增加验证方法
        /// </summary>
        /// <param name="rules">规则</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add((o, e) =>
            {
                ItemPropertyViewModel itempropertyViewModel = o as ItemPropertyViewModel;
                if (itempropertyViewModel.IsCatalog && itempropertyViewModel.Catalog == null)
                {
                    e.BrokenDescription = "属性值不能为空".L10N();
                }
            });
            base.AddValidations(rules);
        }
    }
}
