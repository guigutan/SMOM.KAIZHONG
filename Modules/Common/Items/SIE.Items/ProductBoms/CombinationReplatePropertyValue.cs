using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 组合替代属性
    /// </summary>
    [ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("组合替代属性")]
	public partial class CombinationReplatePropertyValue : DataEntity
	{
		#region 属性值 Value
		/// <summary>
		/// 属性值
		/// </summary>
		[Label("属性值")]
		public static readonly Property<string> ValueProperty = P<CombinationReplatePropertyValue>.Register(e => e.Value);

		/// <summary>
		/// 属性值
		/// </summary>
		public string Value
		{
			get { return GetProperty(ValueProperty); }
			set { SetProperty(ValueProperty, value); }
		}
		#endregion

		#region 物料属性定义 Definition
		/// <summary>
		/// 物料属性定义Id
		/// </summary>
		[Required]
		[Label("物料属性定义")]
		public static readonly IRefIdProperty DefinitionIdProperty = P<CombinationReplatePropertyValue>.RegisterRefId(e => e.DefinitionId, ReferenceType.Normal);

		/// <summary>
		/// 物料属性定义Id
		/// </summary>
		public double DefinitionId
		{
			get { return (double)GetRefId(DefinitionIdProperty); }
			set { SetRefId(DefinitionIdProperty, value); }
		}

		/// <summary>
		/// 物料属性定义
		/// </summary>
		[Label("物料属性定义")]
		public static readonly RefEntityProperty<ItemPropertyDefinition> DefinitionProperty = P<CombinationReplatePropertyValue>.RegisterRef(e => e.Definition, DefinitionIdProperty);

		/// <summary>
		/// 物料属性定义
		/// </summary>
		public ItemPropertyDefinition Definition
		{
			get { return GetRefEntity(DefinitionProperty); }
			set { SetRefEntity(DefinitionProperty, value); }
		}
		#endregion

		#region 属性值 DefinitionName
		/// <summary>
		/// 属性值
		/// </summary>
		[Label("属性值")]
		public static readonly Property<string> DefinitionNameProperty = P<CombinationReplatePropertyValue>.RegisterView(e => e.DefinitionName, p => p.Definition.Name);

		/// <summary>
		/// 属性值
		/// </summary>
		public string DefinitionName
		{
			get { return this.GetProperty(DefinitionNameProperty); }
		}
		#endregion

		#region 属性组 PropertyGroup
		/// <summary>
		/// 属性组
		/// </summary>
		[Label("属性组")]
		public static readonly Property<string> PropertyGroupProperty = P<CombinationReplatePropertyValue>.Register(e => e.PropertyGroup);

		/// <summary>
		/// 属性组
		/// </summary>
		public string PropertyGroup
		{
			get { return GetProperty(PropertyGroupProperty); }
			set { SetProperty(PropertyGroupProperty, value); }
		}
		#endregion

		#region 组合替代属性值列表 CombinationReplate
		/// <summary>
		/// 组合替代属性值列表Id
		/// </summary>
		public static readonly IRefIdProperty CombinationReplateIdProperty = P<CombinationReplatePropertyValue>.RegisterRefId(e => e.CombinationReplateId, ReferenceType.Parent);

		/// <summary>
		/// 组合替代属性值列表Id
		/// </summary>
		public double CombinationReplateId
		{
			get { return (double)GetRefId(CombinationReplateIdProperty); }
			set { SetRefId(CombinationReplateIdProperty, value); }
		}

		/// <summary>
		/// 组合替代属性值列表
		/// </summary>
		public static readonly RefEntityProperty<CombinationReplate> CombinationReplateProperty = P<CombinationReplatePropertyValue>.RegisterRef(e => e.CombinationReplate, CombinationReplateIdProperty);

		/// <summary>
		/// 组合替代属性值列表
		/// </summary>
		public CombinationReplate CombinationReplate
		{
			get { return GetRefEntity(CombinationReplateProperty); }
			set { SetRefEntity(CombinationReplateProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 组合替代属性 实体配置
	/// </summary>
	internal class CombinationReplatePropertyValueConfig : EntityConfig<CombinationReplatePropertyValue>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("ITEM_COM_REPLATE_VAL").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}