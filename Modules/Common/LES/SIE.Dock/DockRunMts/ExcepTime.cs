using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockRunMts
{
	/// <summary>
	/// 例外时段
	/// </summary>
	[ChildEntity, Serializable]
	[Label("例外时段")]
	public partial class ExcepTime : DataEntity
	{
        #region 例外类型 ExcepType
        /// <summary>
        /// 例外类型
        /// </summary>
        [Label("例外类型")]
        public static readonly Property<ExcepType> ExcepTypeProperty = P<ExcepTime>.Register(e => e.ExcepType);

        /// <summary>
        /// 例外类型
        /// </summary>
        public ExcepType ExcepType
        {
            get { return GetProperty(ExcepTypeProperty); }
            set { SetProperty(ExcepTypeProperty, value); }
        }
        #endregion

        #region 开始时间 BeginTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
		[Label("开始时间")]
		public static readonly Property<DateTime> BeginTimeProperty = P<ExcepTime>.Register(e => e.BeginTime);

		/// <summary>
		/// 开始时间
		/// </summary>
		public DateTime BeginTime
		{
			get { return GetProperty(BeginTimeProperty); }
			set { SetProperty(BeginTimeProperty, value); }
		}
		#endregion

		#region 结束时间 EndTime
		/// <summary>
		/// 结束时间
		/// </summary>
		[Required]
		[Label("结束时间")]
		public static readonly Property<DateTime> EndTimeProperty = P<ExcepTime>.Register(e => e.EndTime);

		/// <summary>
		/// 结束时间
		/// </summary>
		public DateTime EndTime
		{
			get { return GetProperty(EndTimeProperty); }
			set { SetProperty(EndTimeProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(2000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<ExcepTime>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 月台运行维护 DockRunMt
		/// <summary>
		/// 月台运行维护Id
		/// </summary>
		public static readonly IRefIdProperty DockRunMtIdProperty = P<ExcepTime>.RegisterRefId(e => e.DockRunMtId, ReferenceType.Parent);

		/// <summary>
		/// 月台运行维护Id
		/// </summary>
		public double DockRunMtId
		{
			get { return (double)GetRefId(DockRunMtIdProperty); }
			set { SetRefId(DockRunMtIdProperty, value); }
		}

		/// <summary>
		/// 月台运行维护
		/// </summary>
		public static readonly RefEntityProperty<DockRunMt> DockRunMtProperty = P<ExcepTime>.RegisterRef(e => e.DockRunMt, DockRunMtIdProperty);

		/// <summary>
		/// 月台运行维护
		/// </summary>
		public DockRunMt DockRunMt
		{
			get { return GetRefEntity(DockRunMtProperty); }
			set { SetRefEntity(DockRunMtProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// 例外时段 实体配置
	/// </summary>
	internal class ExcepTimeConfig : EntityConfig<ExcepTime>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EXCEP_TIME").MapAllProperties();
			Meta.Property(ExcepTime.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}