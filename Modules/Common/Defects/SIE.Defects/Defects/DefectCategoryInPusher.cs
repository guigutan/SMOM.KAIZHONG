using SIE;
using SIE.Common.Sender;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Defects
{
	/// <summary>
	/// 缺陷分类与推送方式关系
	/// </summary>
	[ChildEntity, Serializable]
	//[CriteriaQuery]
	[Label("缺陷分类与推送方式关系")]
	public partial class DefectCategoryInPusher : DataEntity
	{
		#region 推送方式 Pusher
		/// <summary>
		/// 推送方式Id
		/// </summary>
		public static readonly IRefIdProperty PusherIdProperty = P<DefectCategoryInPusher>.RegisterRefId(e => e.PusherId, ReferenceType.Normal);

		/// <summary>
		/// 推送方式Id
		/// </summary>
		public double PusherId
		{
			get { return (double)GetRefId(PusherIdProperty); }
			set { SetRefId(PusherIdProperty, value); }
		}

		/// <summary>
		/// 推送方式
		/// </summary>
		public static readonly RefEntityProperty<Pusher> PusherProperty = P<DefectCategoryInPusher>.RegisterRef(e => e.Pusher, PusherIdProperty);

		/// <summary>
		/// 推送方式
		/// </summary>
		public Pusher Pusher
		{
			get { return GetRefEntity(PusherProperty); }
			set { SetRefEntity(PusherProperty, value); }
		}
		#endregion

		#region 预警邮件设置 DefectCategory
		/// <summary>
		/// 预警邮件设置Id
		/// </summary>
		public static readonly IRefIdProperty DefectCategoryIdProperty = P<DefectCategoryInPusher>.RegisterRefId(e => e.DefectCategoryId, ReferenceType.Parent);

		/// <summary>
		/// 预警邮件设置Id
		/// </summary>
		public double DefectCategoryId
		{
			get { return (double)GetRefId(DefectCategoryIdProperty); }
			set { SetRefId(DefectCategoryIdProperty, value); }
		}

		/// <summary>
		/// 预警邮件设置
		/// </summary>
		public static readonly RefEntityProperty<DefectCategory> DefectCategoryProperty = P<DefectCategoryInPusher>.RegisterRef(e => e.DefectCategory, DefectCategoryIdProperty);

		/// <summary>
		/// 预警邮件设置
		/// </summary>
		public DefectCategory DefectCategory
		{
			get { return GetRefEntity(DefectCategoryProperty); }
			set { SetRefEntity(DefectCategoryProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 缺陷分类与推送方式关系 实体配置
	/// </summary>
	internal class DefectCategoryInPusherConfig : EntityConfig<DefectCategoryInPusher>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("DEF_CATEGORY_PUSHER").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}