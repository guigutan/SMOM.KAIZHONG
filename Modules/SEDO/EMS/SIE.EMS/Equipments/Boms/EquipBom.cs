using SIE;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EMS.Equipments.Models;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Boms
{
	/// <summary>
	/// 设备BOM
	/// </summary>
	[RootEntity, Serializable]
	[ConditionQueryType(typeof(EquipBomCriteria))]
	[Label("设备BOM")]
	public partial class EquipBom : DataEntity
	{
		#region 设备型号维护 EquipModel
		/// <summary>
		/// 设备型号维护Id
		/// </summary>
		[Label("设备型号编码")]
		public static readonly IRefIdProperty EquipModelIdProperty = P<EquipBom>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

		/// <summary>
		/// 设备型号维护Id
		/// </summary>
		public double EquipModelId
		{
			get { return (double)GetRefId(EquipModelIdProperty); }
			set { SetRefId(EquipModelIdProperty, value); }
		}

		/// <summary>
		/// 设备型号维护
		/// </summary>
		public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<EquipBom>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

		/// <summary>
		/// 设备型号维护
		/// </summary>
		public EquipModel EquipModel
		{
			get { return GetRefEntity(EquipModelProperty); }
			set { SetRefEntity(EquipModelProperty, value); }
		}
		#endregion

		#region 设备BOM明细列表 EquipBomDetailList
		///// <summary>
		///// 设备BOM明细列表
		///// </summary>
		//public static readonly ListProperty<EntityList<EquipBomDetail>> EquipBomDetailListProperty = P<EquipBom>.RegisterList(e => e.EquipBomDetailList);
		///// <summary>
		///// 设备BOM明细列表
		///// </summary>
		//public EntityList<EquipBomDetail> EquipBomDetailList
		//{
		//	get { return this.GetLazyList(EquipBomDetailListProperty); }
		//}
		#endregion

		#region 视图属性
		#region 设备型号名称 EquipModelName
		/// <summary>
		/// 设备型号名称
		/// </summary>
		[Label("设备型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipBom>.RegisterView(e => e.EquipModelName, p => p.EquipModel.Name);

		/// <summary>
		/// 设备型号名称
		/// </summary>
		public string EquipModelName
		{
            get { return this.GetProperty(EquipModelNameProperty); }
        }
		#endregion

		#region 设备类型 EquipTypeName
		/// <summary>
		/// 设备类型
		/// </summary>
		[Label("设备类型")]
        public static readonly Property<string> EquipTypeNameProperty = P<EquipBom>.RegisterView(e => e.EquipTypeName, p => p.EquipModel.EquipType.TypeName);

		/// <summary>
		/// 设备类型
		/// </summary>
		public string EquipTypeName
		{
            get { return this.GetProperty(EquipTypeNameProperty); }
        }
        #endregion
        #endregion
    }

	/// <summary>
	/// 设备BOM 实体配置
	/// </summary>
	internal class EquipBomConfig : EntityConfig<EquipBom>
	{
		/// <summary>
      	/// 配置元数据
    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_EQUIP_BOM").MapAllProperties();
			Meta.EnablePhantoms();
		}

		/// <summary>
		/// 校验规则
		/// </summary>
		/// <param name="rules">规则</param>
		protected override void AddValidations(IValidationDeclarer rules)
		{
			rules.AddRule(new HandlerRule()
			{
				Handler = (o, e) =>
				{
					var para = o.CastTo<EquipBom>();
					if (RT.Service.Resolve<EquipBomController>().VerifyEquipModelIsRepeat(para))
					{
						e.BrokenDescription = "【设备型号编码】不能重复，请确认！".L10N();
						return;
					}
				}
			}, new RuleMeta() { Scope = EntityStatusScopes.Add | EntityStatusScopes.Update });
		}
	}
}