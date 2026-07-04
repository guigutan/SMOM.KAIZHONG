using SIE.Domain;
using SIE.EMS.Enums;
using SIE.EMS.SpareParts;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.InventoryTasks
{
	/// <summary>
	/// 备件盘点差异
	/// </summary>
	[RootEntity, Serializable]	
	[Label("备件盘点差异")]
	public partial class InventoryTaskSparePartDiff : DataEntity
	{
		#region 序列号 Sn
		/// <summary>
		/// 序列号
		/// </summary>
		[Label("序列号")]
		public static readonly Property<string> SnProperty = P<InventoryTaskSparePartDiff>.Register(e => e.Sn);

		/// <summary>
		/// 序列号
		/// </summary>
		public string Sn
		{
			get { return GetProperty(SnProperty); }
			set { SetProperty(SnProperty, value); }
		}
		#endregion

		#region 总数 Total
		/// <summary>
		/// 总数
		/// </summary>
		[Label("总数")]
		public static readonly Property<int> TotalProperty = P<InventoryTaskSparePartDiff>.Register(e => e.Total);

		/// <summary>
		/// 总数
		/// </summary>
		public int Total
		{
			get { return GetProperty(TotalProperty); }
			set { SetProperty(TotalProperty, value); }
		}
		#endregion

		#region 实盘总数 ActualTotal
		/// <summary>
		/// 实盘总数
		/// </summary>
		[Label("实盘总数")]
		public static readonly Property<int> ActualTotalProperty = P<InventoryTaskSparePartDiff>.Register(e => e.ActualTotal);

		/// <summary>
		/// 实盘总数
		/// </summary>
		public int ActualTotal
		{
			get { return GetProperty(ActualTotalProperty); }
			set { SetProperty(ActualTotalProperty, value); }
		}
		#endregion

		#region 差异数量 Diff
		/// <summary>
		/// 差异数量
		/// </summary>
		[Label("差异数量")]
		public static readonly Property<int> DiffProperty = P<InventoryTaskSparePartDiff>.Register(e => e.Diff);

		/// <summary>
		/// 差异数量
		/// </summary>
		public int Diff
		{
			get { return GetProperty(DiffProperty); }
			set { SetProperty(DiffProperty, value); }
		}
		#endregion

		#region 备注 Remark
		/// <summary>
		/// 备注
		/// </summary>
		[Label("备注")]
		[MaxLength(1000)]
		public static readonly Property<string> RemarkProperty = P<InventoryTaskSparePartDiff>.Register(e => e.Remark);

		/// <summary>
		/// 备注
		/// </summary>
		public string Remark
		{
			get { return GetProperty(RemarkProperty); }
			set { SetProperty(RemarkProperty, value); }
		}
		#endregion

		#region 备件基础数据 SparePart
		/// <summary>
		/// 备件基础数据Id
		/// </summary>
		[Label("备件基础数据")]
		public static readonly IRefIdProperty SparePartIdProperty = P<InventoryTaskSparePartDiff>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

		/// <summary>
		/// 备件基础数据Id
		/// </summary>
		public double SparePartId
		{
			get { return (double)GetRefId(SparePartIdProperty); }
			set { SetRefId(SparePartIdProperty, value); }
		}

		/// <summary>
		/// 备件基础数据
		/// </summary>
		public static readonly RefEntityProperty<SparePart> SparePartProperty = P<InventoryTaskSparePartDiff>.RegisterRef(e => e.SparePart, SparePartIdProperty);

		/// <summary>
		/// 备件基础数据
		/// </summary>
		public SparePart SparePart
		{
			get { return GetRefEntity(SparePartProperty); }
			set { SetRefEntity(SparePartProperty, value); }
		}
		#endregion

		#region 盘点结果 InventoryResult
		/// <summary>
		/// 盘点结果
		/// </summary>
		[Label("盘点结果")]
		public static readonly Property<InventoryResult> InventoryResultProperty = P<InventoryTaskSparePartDiff>.Register(e => e.InventoryResult);

		/// <summary>
		/// 盘点结果
		/// </summary>
		public InventoryResult InventoryResult
		{
			get { return GetProperty(InventoryResultProperty); }
			set { SetProperty(InventoryResultProperty, value); }
		}
        #endregion

        #region 盘点任务 InventoryTask
        /// <summary>
        /// 盘点任务Id
        /// </summary>
        [Label("盘点任务")]
        public static readonly IRefIdProperty InventoryTaskIdProperty =
            P<InventoryTaskSparePartDiff>.RegisterRefId(e => e.InventoryTaskId, ReferenceType.Parent);

        /// <summary>
        /// 盘点任务Id
        /// </summary>
        public double InventoryTaskId
        {
            get { return (double)this.GetRefId(InventoryTaskIdProperty); }
            set { this.SetRefId(InventoryTaskIdProperty, value); }
        }

        /// <summary>
        /// 盘点任务
        /// </summary>
        public static readonly RefEntityProperty<InventoryTask> InventoryTaskProperty =
            P<InventoryTaskSparePartDiff>.RegisterRef(e => e.InventoryTask, InventoryTaskIdProperty);

        /// <summary>
        /// 盘点任务
        /// </summary>
        public InventoryTask InventoryTask
        {
            get { return this.GetRefEntity(InventoryTaskProperty); }
            set { this.SetRefEntity(InventoryTaskProperty, value); }
        }
		#endregion

		#region 视图属性
		#region 备件名称 SparePartName
		/// <summary>
		/// 备件名称
		/// </summary>
		[Label("备件名称")]
		public static readonly Property<string> SparePartNameProperty = P<InventoryTaskSparePartDiff>.RegisterView(e => e.SparePartName, p => p.SparePart.SparePartName);

		/// <summary>
		/// 备件名称
		/// </summary>
		public string SparePartName
		{
			get { return this.GetProperty(SparePartNameProperty); }
		}
        #endregion

        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<InventoryTaskSparePartDiff>.RegisterView(e => e.ApprovalStatus, p => p.InventoryTask.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion


        #endregion
    }

	/// <summary>
	///  实体配置
	/// </summary>
	internal class InventoryTaskSparePartDiffConfig : EntityConfig<InventoryTaskSparePartDiff>
	{
		/// <summary>
		/// 配置元数据
		/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("EMS_INV_TASK_SP_DIFF").MapAllProperties();
			Meta.Property(InventoryTaskSparePartDiff.RemarkProperty).ColumnMeta.HasLength(4000);
			Meta.EnablePhantoms();
		}
	}
}