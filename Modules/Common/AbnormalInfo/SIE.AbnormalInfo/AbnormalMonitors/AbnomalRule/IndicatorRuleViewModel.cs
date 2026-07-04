using SIE.AbnormalInfo.Common;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
	/// <summary>
	/// 指标运算规则规则
	/// </summary>
	[RootEntity,Serializable]
	public partial class IndicatorRuleViewModel : ViewModel
	{

		#region 指标运算

		#region 指标名 IndicatorName
		/// <summary>
		/// 指标名
		/// </summary>
		[Label("指标名")]
		public static readonly Property<string> IndicatorNameProperty = P<IndicatorRuleViewModel>.Register(e => e.IndicatorName);

		/// <summary>
		/// 指标名
		/// </summary>
		public string IndicatorName
		{
			get { return GetProperty(IndicatorNameProperty); }
			set { SetProperty(IndicatorNameProperty, value); }
		}
		#endregion

		#region 指标运算 IndicatorOperation
		/// <summary>
		/// 指标运算
		/// </summary>
		[Label("指标运算")]
		public static readonly Property<string> IndicatorOperationProperty = P<IndicatorRuleViewModel>.Register(e => e.IndicatorOperation);

		/// <summary>
		/// 指标运算
		/// </summary>
		public string IndicatorOperation
		{
			get { return GetProperty(IndicatorOperationProperty); }
			set { SetProperty(IndicatorOperationProperty, value); }
		}
		#endregion

		#region 运算符 Operator
		/// <summary>
		/// 运算符
		/// </summary>
		[Label("")]
		public static readonly Property<Operator?> OperatorProperty = P<IndicatorRuleViewModel>.Register(e => e.Operator);

		/// <summary>
		/// 运算符
		/// </summary>
		public Operator? Operator
		{
			get { return GetProperty(OperatorProperty); }
			set { SetProperty(OperatorProperty, value); }
		}
		#endregion

		#region 值1 Value1
		/// <summary>
		/// 值1
		/// </summary>
		[Label("值1")]
		public static readonly Property<string> Value1Property = P<IndicatorRuleViewModel>.Register(e => e.Value1);

		/// <summary>
		/// 值1
		/// </summary>
		public string Value1
		{
			get { return GetProperty(Value1Property); }
			set { SetProperty(Value1Property, value); }
		}
		#endregion

		#region 值2 Value2
		/// <summary>
		/// 值2
		/// </summary>
		[Label("值2")]
		public static readonly Property<string> Value2Property = P<IndicatorRuleViewModel>.Register(e => e.Value2);

		/// <summary>
		/// 值2
		/// </summary>
		public string Value2
		{
			get { return GetProperty(Value2Property); }
			set { SetProperty(Value2Property, value); }
		}
		#endregion

		#region 单位 IndicatorUnit
		/// <summary>
		/// 单位
		/// </summary>
		[Label("单位")]
		public static readonly Property<IndicatorUnit> IndicatorUnitProperty = P<IndicatorRuleViewModel>.Register(e => e.IndicatorUnit);

		/// <summary>
		/// 单位
		/// </summary>
		public IndicatorUnit IndicatorUnit
		{
			get { return GetProperty(IndicatorUnitProperty); }
			set { SetProperty(IndicatorUnitProperty, value); }
		}
		#endregion

		#endregion

	}
}