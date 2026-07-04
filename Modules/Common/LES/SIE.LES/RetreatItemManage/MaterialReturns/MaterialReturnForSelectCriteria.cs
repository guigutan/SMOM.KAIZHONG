using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;

namespace SIE.LES.RetreatItemManage.MaterialReturns
{
	/// <summary>
	/// 退料实体
	/// </summary>
	[QueryEntity, Serializable]
	public partial class MaterialReturnCriteria:Criteria
	{
		#region NO
		/// <summary>
		/// 
		/// </summary>
		[Label("退料单号")]
		public static readonly Property<string> NOProperty = P<MaterialReturnCriteria>.Register(e => e.NO);

		/// <summary>
		/// 
		/// </summary>
		public string NO
		{
			get { return GetProperty(NOProperty); }
			set { SetProperty(NOProperty, value); }
		}
		#endregion

		#region 退料状态
		/// <summary>
		/// 状态
		/// </summary>
		[Label("状态")]
		public static readonly Property<ReturnStates?> ReturnStateProperty = P<MaterialReturnCriteria>.Register(e => e.ReturnState);

		/// <summary>
		/// 状态
		/// </summary>
		public ReturnStates? ReturnState
		{
			get { return GetProperty(ReturnStateProperty); }
			set { SetProperty(ReturnStateProperty, value); }
		}
		#endregion

		#region 退料类型
		/// <summary>
		/// 退料类型
		/// </summary>
		[Label("退料类型")]
		public static readonly Property<ReturnTypes?> ReturnTypeProperty = P<MaterialReturnCriteria>.Register(e => e.ReturnType);

		/// <summary>
		/// 退料类型
		/// </summary>
		public ReturnTypes? ReturnType
		{
			get { return GetProperty(ReturnTypeProperty); }
			set { SetProperty(ReturnTypeProperty, value); }
		}
		#endregion

		#region 标签号
		/// <summary>
		/// 标签号
		/// </summary>
		[Label("标签号")]
		public static readonly Property<string> LabelProperty = P<MaterialReturnCriteria>.Register(e => e.Label);

		/// <summary>
		/// 标签号
		/// </summary>
		public string Label
		{
			get { return GetProperty(LabelProperty); }
			set { SetProperty(LabelProperty, value); }
		}
		#endregion

		#region 批次号
		/// <summary>
		/// 批次号
		/// </summary>
		[Label("批次号")]
		public static readonly Property<string> BatchNOProperty = P<MaterialReturnCriteria>.Register(e => e.BatchNO);

		/// <summary>
		/// 批次号
		/// </summary>
		public string BatchNO
		{
			get { return GetProperty(BatchNOProperty); }
			set { SetProperty(BatchNOProperty, value); }
		}
		#endregion

		#region 关联工单 WorkOrder	
		/// <summary>
		/// 关联工单
		/// </summary>
		[Label("关联工单")]
        public static readonly Property<string> WorkOrderProperty = P<MaterialReturnCriteria>.Register(e => e.WorkOrder);

		/// <summary>
		/// 关联工单
		/// </summary>
		public string WorkOrder
		{
            get { return this.GetProperty(WorkOrderProperty); }
            set { this.SetProperty(WorkOrderProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<MaterialReturnCriteria>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<MaterialReturnCriteria>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 提交人 Employee
        /// <summary>
        /// 提交人Id
        /// </summary>
        public static readonly IRefIdProperty EmployeeIdProperty = P<MaterialReturnCriteria>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

		/// <summary>
		/// 提交人Id
		/// </summary>
		public double? EmployeeId
		{
			get { return (double?)GetRefNullableId(EmployeeIdProperty); }
			set { SetRefNullableId(EmployeeIdProperty, value); }
		}

		/// <summary>
		/// 提交人
		/// </summary>
		public static readonly RefEntityProperty<Employee> EmployeeProperty = P<MaterialReturnCriteria>.RegisterRef(e => e.Employee, EmployeeIdProperty);

		/// <summary>
		/// 提交人
		/// </summary>
		public Employee Employee
		{
			get { return GetRefEntity(EmployeeProperty); }
			set { SetRefEntity(EmployeeProperty, value); }
		}
		#endregion

		#region 生产资源 WipResource
		/// <summary>
		/// 生产资源Id
		/// </summary>
		public static readonly IRefIdProperty WipResourceIdProperty = P<MaterialReturnCriteria>.RegisterRefId(e => e.WipResourceId, ReferenceType.Normal);

		/// <summary>
		/// 生产资源Id
		/// </summary>
		public double? WipResourceId
		{
			get { return (double?)GetRefNullableId(WipResourceIdProperty); }
			set { SetRefNullableId(WipResourceIdProperty, value); }
		}

		/// <summary>
		/// 生产资源
		/// </summary>
		public static readonly RefEntityProperty<WipResource> WipResourceProperty = P<MaterialReturnCriteria>.RegisterRef(e => e.WipResource, WipResourceIdProperty);

		/// <summary>
		/// 生产资源
		/// </summary>
		public WipResource WipResource
		{
			get { return GetRefEntity(WipResourceProperty); }
			set { SetRefEntity(WipResourceProperty, value); }
		}
		#endregion

		#region 提交时间 SubmitDate
		/// <summary>
		/// 提交时间
		/// </summary>
		[Label("提交时间")]
        public static readonly Property<DateRange> SubmitDateProperty = P<MaterialReturnCriteria>.Register(e => e.SubmitDate);

		/// <summary>
		/// 提交时间
		/// </summary>
		public DateRange SubmitDate
		{
            get { return this.GetProperty(SubmitDateProperty); }
            set { this.SetProperty(SubmitDateProperty, value); }
        }
		#endregion

		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<MaterialReturnCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

		/// <summary>
		/// 工厂Id
		/// </summary>
		public double? FactoryId
		{
			get { return (double?)GetRefNullableId(FactoryIdProperty); }
			set { SetRefNullableId(FactoryIdProperty, value); }
		}

		/// <summary>
		/// 工厂
		/// </summary>
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<MaterialReturnCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

		/// <summary>
		/// 工厂
		/// </summary>
		public Enterprise Factory
		{
			get { return GetRefEntity(FactoryProperty); }
			set { SetRefEntity(FactoryProperty, value); }
		}
		#endregion

		/// <summary>
		/// 重写此方法实现查询
		/// </summary>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<MaterialReturnController>().Fetch(this);
		}
	}
}
