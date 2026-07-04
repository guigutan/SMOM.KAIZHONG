using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.ObjectModel;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.RunStandards
{
	/// <summary>
	/// 查询
	/// </summary>
	/// 
	[QueryEntity, Serializable]
	[Label("设备运行定标查询实体")]

	public class RunStandardCriteria : Criteria
    {

		#region 定标单号 No
		/// <summary>
		/// 定标单号
		/// </summary>
		[Label("定标单号")]
		public static readonly Property<string> NoProperty = P<RunStandardCriteria>.Register(e => e.No);

		/// <summary>
		/// 定标单号
		/// </summary>
		public string No
		{
			get { return GetProperty(NoProperty); }
			set { SetProperty(NoProperty, value); }
		}
		#endregion

		#region 定标名称 Name
		/// <summary>
		/// 定标名称
		/// </summary>
		[Label("定标名称")]
		public static readonly Property<string> NameProperty = P<RunStandardCriteria>.Register(e => e.Name);

		/// <summary>
		/// 定标名称
		/// </summary>
		public string Name
		{
			get { return GetProperty(NameProperty); }
			set { SetProperty(NameProperty, value); }
		}
		#endregion

		#region 设备型号 EquipModel
		/// <summary>
		/// 设备型号Id
		/// </summary>
		[Label("设备型号")]
		public static readonly IRefIdProperty EquipModelIdProperty = P<RunStandardCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

		/// <summary>
		/// 设备型号Id
		/// </summary>
		public double? EquipModelId
		{
			get { return (double?)GetRefNullableId(EquipModelIdProperty); }
			set { SetRefNullableId(EquipModelIdProperty, value); }
		}

		/// <summary>
		/// 设备型号
		/// </summary>
		public static readonly RefEntityProperty<EquipModel> EquipModelProperty = P<RunStandardCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

		/// <summary>
		/// 设备型号
		/// </summary>
		public EquipModel EquipModel
		{
			get { return GetRefEntity(EquipModelProperty); }
			set { SetRefEntity(EquipModelProperty, value); }
		}
        #endregion


        #region 制定人 Create
        /// <summary>
        /// 制定人Id
        /// </summary>
        [Label("制定人")]
        public static readonly IRefIdProperty CreateIdProperty =
            P<RunStandardCriteria>.RegisterRefId(e => e.CreateId, ReferenceType.Normal);

        /// <summary>
        /// 制定人Id
        /// </summary>
        public double? CreateId
        {
            get { return (double?)this.GetRefNullableId(CreateIdProperty); }
            set { this.SetRefNullableId(CreateIdProperty, value); }
        }

        /// <summary>
        /// 制定人
        /// </summary>
        public static readonly RefEntityProperty<Employee> CreateProperty =
            P<RunStandardCriteria>.RegisterRef(e => e.Create, CreateIdProperty);

        /// <summary>
        /// 制定人
        /// </summary>
        public Employee Create
        {
            get { return this.GetRefEntity(CreateProperty); }
            set { this.SetRefEntity(CreateProperty, value); }
        }
		#endregion

		#region 定标日期 CreateDate
		/// <summary>
		/// 定标日期
		/// </summary>
		[Label("定标日期")]
		public static readonly Property<DateRange> CreateDateProperty = P<RunStandardCriteria>.Register(e => e.CreateDate);

		/// <summary>
		/// 创建日期
		/// </summary>
		public DateRange CreateDate
		{
			get { return GetProperty(CreateDateProperty); }
			set { SetProperty(CreateDateProperty, value); }
		}
		#endregion

		/// <summary>
		/// 查询
		/// </summary>
		/// <returns></returns>
		protected override EntityList Fetch()
		{
			return RT.Service.Resolve<RunStandardsController>().Fetch(this);

		}
	}
}
