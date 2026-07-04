using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Packages.QrCodeParseRules
{
    /// <summary>
    /// 二维码解析规则
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("二维码解析规则")]
	public partial class QrCodeParseRule : DataEntity, IStateEntity
	{
		#region 编码 Code
		/// <summary>
		/// 编码
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("编码")]
		public static readonly Property<string> CodeProperty = P<QrCodeParseRule>.Register(e => e.Code);

		/// <summary>
		/// 编码
		/// </summary>
		public string Code
		{
			get { return GetProperty(CodeProperty); }
			set { SetProperty(CodeProperty, value); }
		}
		#endregion

		#region 名称 Name
		/// <summary>
		/// 名称
		/// </summary>
		[Required]
		[NotDuplicate]
		[Label("名称")]
		public static readonly Property<string> NameProperty = P<QrCodeParseRule>.Register(e => e.Name);

		/// <summary>
		/// 名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 分隔符类型 SeparatorType
		/// <summary>
		/// 分隔符类型
		/// </summary>
		[Label("分隔符类型")]
		public static readonly Property<string> SeparatorTypeProperty = P<QrCodeParseRule>.Register(e => e.SeparatorType);

		/// <summary>
		/// 分隔符类型
		/// </summary>
		public string SeparatorType
		{
			get { return GetProperty(SeparatorTypeProperty); }
			set { SetProperty(SeparatorTypeProperty, value); }
		}
		#endregion

		#region 说明 Desc
		/// <summary>
		/// 说明
		/// </summary>
		[Label("说明")]
		public static readonly Property<string> DescProperty = P<QrCodeParseRule>.Register(e => e.Desc);

		/// <summary>
		/// 说明
		/// </summary>
		public string Desc
		{
			get { return GetProperty(DescProperty); }
			set { SetProperty(DescProperty, value); }
		}
		#endregion

		#region 截取方式 InterceptWay
		/// <summary>
		/// 截取方式
		/// </summary>
		[Label("截取方式")]
		public static readonly Property<InterceptWay> InterceptWayProperty = P<QrCodeParseRule>.Register(e => e.InterceptWay);

		/// <summary>
		/// 截取方式
		/// </summary>
		public InterceptWay InterceptWay
		{
			get { return GetProperty(InterceptWayProperty); }
			set { SetProperty(InterceptWayProperty, value); }
		}
		#endregion

		#region 二维码解析规则明细列表 QrCodeParseRuleDetailList
		/// <summary>
		/// 二维码解析规则明细列表
		/// </summary>
		public static readonly ListProperty<EntityList<QrCodeParseRuleDetail>> QrCodeParseRuleDetailListProperty = P<QrCodeParseRule>.RegisterList(e => e.QrCodeParseRuleDetailList);
		/// <summary>
		/// 二维码解析规则明细列表
		/// </summary>
		public EntityList<QrCodeParseRuleDetail> QrCodeParseRuleDetailList
		{
			get { return this.GetLazyList(QrCodeParseRuleDetailListProperty); }
		}
		#endregion

		#region 状态 State
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<State> StateProperty = P<QrCodeParseRule>.Register(e => e.State);
		/// <summary>
		/// 状态
		/// </summary>
		public State State
		{
			get { return this.GetProperty(StateProperty); }
			set { this.SetProperty(StateProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 二维码解析规则 实体配置
	/// </summary>
	internal class QrCodeParseRuleConfig : EntityConfig<QrCodeParseRule>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("QR_CODE_PARSE_RULE").MapAllProperties();
			Meta.EnablePhantoms();
		}
	}
}
