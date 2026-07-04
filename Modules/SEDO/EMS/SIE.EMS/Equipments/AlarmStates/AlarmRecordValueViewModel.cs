using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.AlarmStates
{
    /// <summary>
    /// 设备报警记录值
    /// </summary>
    [RootEntity, Serializable]	
	[Label("设备报警记录值")]
	public partial class AlarmRecordValueViewModel : ViewModel
	{
		#region TAG全称 FullTagName
		/// <summary>
		/// TAG全称
		/// </summary>
		[Label("TAG全称")]
		public static readonly Property<string> FullTagNameProperty = P<AlarmRecordValueViewModel>.Register(e => e.FullTagName);

		/// <summary>
		/// TAG全称
		/// </summary>
		public string FullTagName
		{
			get { return GetProperty(FullTagNameProperty); }
			set { SetProperty(FullTagNameProperty, value); }
		}
		#endregion

		#region 报警值  AlarmValue
		/// <summary>
		/// 报警值
		/// </summary>
		[Required]
		[Label("报警值")]
		public static readonly Property<double> AlarmValueProperty = P<AlarmRecordValueViewModel>.Register(e => e.AlarmValue);

		/// <summary>
		/// 报警值
		/// </summary>
		public double AlarmValue
		{
			get { return GetProperty(AlarmValueProperty); }
			set { SetProperty(AlarmValueProperty, value); }
		}
		#endregion

		#region 恢复值  RecoveryValue
		/// <summary>
		/// 恢复值
		/// </summary>
		[Label("恢复值")]
		public static readonly Property<double?> RecoveryValueProperty = P<AlarmRecordValueViewModel>.Register(e => e.RecoveryValue);

		/// <summary>
		/// 恢复值
		/// </summary>
		public double? RecoveryValue
		{
			get { return GetProperty(RecoveryValueProperty); }
			set { SetProperty(RecoveryValueProperty, value); }
		}
		#endregion

		#region MDC变量名 MDCVariableName
		/// <summary>
		/// MDC变量名
		/// </summary>
		[Label("MDC变量名")]
		public static readonly Property<string> MDCVariableNameProperty = P<AlarmRecordValueViewModel>.Register(e => e.MDCVariableName);

		/// <summary>
		/// MDC变量名
		/// </summary>
		public string MDCVariableName
		{
			get { return GetProperty(MDCVariableNameProperty); }
			set { SetProperty(MDCVariableNameProperty, value); }
		}
		#endregion

		#region 参数编码 PararCode
		/// <summary>
		/// 参数编码
		/// </summary>
		[Label("参数编码")]
		[Required]
		public static readonly Property<string> PararCodeProperty = P<AlarmRecordValueViewModel>.Register(e => e.PararCode);

		/// <summary>
		/// 参数编码
		/// </summary>
		public string PararCode
		{
			get { return GetProperty(PararCodeProperty); }
			set { SetProperty(PararCodeProperty, value); }
		}
		#endregion

		#region 参数名称 ParaName
		/// <summary>
		/// 参数名称
		/// </summary>
		[Label("参数名称")]
		public static readonly Property<string> ParaNameProperty = P<AlarmRecordValueViewModel>.Register(e => e.ParaName);

		/// <summary>
		/// 参数名称
		/// </summary>
		public string ParaName
		{
			get { return GetProperty(ParaNameProperty); }
			set { SetProperty(ParaNameProperty, value); }
		}
        #endregion

        #region 曲线 IsShowInChart
        /// <summary>
        /// 曲线
        /// </summary>
        [Label("曲线")]
        public static readonly Property<bool> IsShowInChartProperty = P<AlarmRecordValueViewModel>.Register(e => e.IsShowInChart);

        /// <summary>
        /// 曲线
        /// </summary>
        public bool IsShowInChart
        {
            get { return this.GetProperty(IsShowInChartProperty); }
            set { this.SetProperty(IsShowInChartProperty, value); }
        }
        #endregion

    }

}