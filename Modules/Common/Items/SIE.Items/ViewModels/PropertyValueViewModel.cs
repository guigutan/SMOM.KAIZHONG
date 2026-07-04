using SIE.Domain;
using SIE.ManagedProperty;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Items.ViewModels
{
    /// <summary>
    /// 属性值ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("物料属性")]
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

        #region 属性名 DefinitionName
        /// <summary>
        /// 属性名
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> DefinitionNameProperty = P<PropertyValueViewModel>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

        /// <summary>
        /// 属性名
        /// </summary>
        public string DefinitionName
        {
            get { return this.GetProperty(DefinitionNameProperty); }
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
            if (e.Property == PropertyValueViewModel.DefinitionProperty)
            {
                if (e.NewValue != e.OldValue)
                {
                    Values.Clear();
                }
            }
        }
    }
}