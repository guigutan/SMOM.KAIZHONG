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
	public partial class MaterialReturnForSelectCriteria : Criteria
	{
        #region 查询条码 Sn
        /// <summary>
        /// 查询条码
        /// </summary>
        [Label("查询条码")]
        public static readonly Property<string> SnProperty = P<MaterialReturnForSelectCriteria>.Register(e => e.Sn);

        /// <summary>
        /// 查询条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 关联工单 WorkOrder	
        /// <summary>
        /// 关联工单
        /// </summary>
        [Label("关联工单")]
        public static readonly Property<string> WorkOrderProperty = P<MaterialReturnForSelectCriteria>.Register(e => e.WorkOrder);

		/// <summary>
		/// 关联工单
		/// </summary>
		public string WorkOrder
		{
            get { return this.GetProperty(WorkOrderProperty); }
            set { this.SetProperty(WorkOrderProperty, value); }
        }
        #endregion

		#region 工厂 Factory
		/// <summary>
		/// 工厂Id
		/// </summary>
		[Label("工厂")]
		public static readonly IRefIdProperty FactoryIdProperty = P<MaterialReturnForSelectCriteria>.RegisterRefId(e => e.FactoryId, ReferenceType.Normal);

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
		public static readonly RefEntityProperty<Enterprise> FactoryProperty = P<MaterialReturnForSelectCriteria>.RegisterRef(e => e.Factory, FactoryIdProperty);

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
			return RT.Service.Resolve<MaterialReturnController>().MaterialReturnForSelectFetch(this);
		}
	}
}
