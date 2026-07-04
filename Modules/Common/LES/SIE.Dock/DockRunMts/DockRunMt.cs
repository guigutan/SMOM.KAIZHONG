using SIE;
using SIE.Dock.DockMaintains;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.DockRunMts
{
	/// <summary>
	/// 月台运行维护
	/// </summary>
	[RootEntity, Serializable]
    [ConditionQueryType(typeof(DockRunMtCriteria))]
	[Label("月台运行维护")]
	public partial class DockRunMt : DataEntity
	{
        #region 月台维护 DockMaintain
        /// <summary>
        /// 月台维护Id
        /// </summary>      
		[Label("月台编码")]
		public static readonly IRefIdProperty DockMaintainIdProperty = P<DockRunMt>.RegisterRefId(e => e.DockMaintainId, ReferenceType.Normal);

        /// <summary>
        /// 月台维护Id
        /// </summary>
        public double DockMaintainId
        {
            get { return (double)GetRefId(DockMaintainIdProperty); }
            set { SetRefId(DockMaintainIdProperty, value); }
        }

        /// <summary>
        /// 月台维护
        /// </summary>
        public static readonly RefEntityProperty<DockMaintain> DockMaintainProperty = P<DockRunMt>.RegisterRef(e => e.DockMaintain, DockMaintainIdProperty);

        /// <summary>
        /// 月台维护
        /// </summary>
        public DockMaintain DockMaintain
        {
            get { return GetRefEntity(DockMaintainProperty); }
            set { SetRefEntity(DockMaintainProperty, value); }
        }
		#endregion

		#region 月台编码 DockMaintainCode
		/// <summary>
		/// 月台编码
		/// </summary>
		[Label("月台编码")]
		public static readonly Property<string> DockMaintainCodeProperty = P<DockRunMt>.RegisterView(e => e.DockMaintainCode, p => p.DockMaintain.Code);

		/// <summary>
		/// 月台编码
		/// </summary>
		public string DockMaintainCode
		{
			get { return this.GetProperty(DockMaintainCodeProperty); }
		}
		#endregion

		#region 月台名称 DockMaintainName
		/// <summary>
		/// 月台名称
		/// </summary>
		[Label("月台名称")]
		public static readonly Property<string> DockMaintainNameProperty = P<DockRunMt>.RegisterView(e => e.DockMaintainName, p => p.DockMaintain.Name);

		/// <summary>
		/// 月台名称
		/// </summary>
		public string DockMaintainName
        {
			get { return this.GetProperty(DockMaintainNameProperty); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[MaxLength(2000)]
		[Label("备注")]
		public static readonly Property<string> RemarkProperty = P<DockRunMt>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 工作时段列表 WorkTimeList
		/// <summary>
		/// 工作时段列表
		/// </summary>
		public static readonly ListProperty<EntityList<WorkTime>> WorkTimeListProperty = P<DockRunMt>.RegisterList(e => e.WorkTimeList);
		/// <summary>
		/// 工作时段列表
		/// </summary>
		public EntityList<WorkTime> WorkTimeList
		{
			get { return this.GetLazyList(WorkTimeListProperty); }
		}
		#endregion

		#region 例外时段列表 ExcepTimeList
		/// <summary>
		/// 例外时段列表
		/// </summary>
		public static readonly ListProperty<EntityList<ExcepTime>> ExcepTimeListProperty = P<DockRunMt>.RegisterList(e => e.ExcepTimeList);
		/// <summary>
		/// 例外时段列表
		/// </summary>
		public EntityList<ExcepTime> ExcepTimeList
		{
			get { return this.GetLazyList(ExcepTimeListProperty); }
		}
		#endregion

	}

	/// <summary>
	/// 月台运行维护 实体配置
	/// </summary>
	internal class DockRunMtConfig : EntityConfig<DockRunMt>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("DOCK_RUNMT").MapAllProperties();
			Meta.Property(DockRunMt.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}