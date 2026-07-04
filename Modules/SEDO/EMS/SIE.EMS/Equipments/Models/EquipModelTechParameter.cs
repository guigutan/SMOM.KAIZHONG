using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Models
{
    /// <summary>
    /// 技术参数
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("技术参数")]
	public partial class EquipModelTechParameter : DataEntity
	{
		#region 参数名称 ParameterName
		/// <summary>
		/// 参数名称
		/// </summary>
		[Label("参数名称")]
		[Required]
		public static readonly Property<string> ParameterNameProperty = P<EquipModelTechParameter>.Register(e => e.ParameterName);

		/// <summary>
		/// 参数名称
		/// </summary>
		public string ParameterName
		{
			get { return GetProperty(ParameterNameProperty); }
			set { SetProperty(ParameterNameProperty, value); }
		}
		#endregion

		#region 参数内容 ParameterValue
		/// <summary>
		/// 参数内容
		/// </summary>
		[Label("参数内容")]
		[Required]
		public static readonly Property<string> ParameterValueProperty = P<EquipModelTechParameter>.Register(e => e.ParameterValue);

		/// <summary>
		/// 参数内容
		/// </summary>
		public string ParameterValue
		{
			get { return GetProperty(ParameterValueProperty); }
			set { SetProperty(ParameterValueProperty, value); }
		}
		#endregion

		#region 技术参数列表 EquipModel
		/// <summary>
		/// 技术参数列表Id
		/// </summary>
		public static readonly IRefIdProperty EquipModelIdProperty = P<EquipModelTechParameter>.RegisterRefId(e => e.EquipModelId, ReferenceType.Parent);

		/// <summary>
		/// 技术参数列表Id
		/// </summary>
		public double EquipModelId
		{
			get { return (double)GetRefId(EquipModelIdProperty); }
			set { SetRefId(EquipModelIdProperty, value); }
		}

		/// <summary>
		/// 技术参数列表
		/// </summary>
		public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipModelTechParameter>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

		/// <summary>
		/// 技术参数列表
		/// </summary>
		public EquipModel EquipModel
		{
			get { return GetRefEntity(EquipModelProperty); }
			set { SetRefEntity(EquipModelProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 技术参数 实体配置
	/// </summary>
	internal class EquipModelTechParameterConfig : EntityConfig<EquipModelTechParameter>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_MODEL_TECH_PARA").MapAllProperties();
			Meta.EnablePhantoms();
			Meta.EnableSort();
		}
	}

}
