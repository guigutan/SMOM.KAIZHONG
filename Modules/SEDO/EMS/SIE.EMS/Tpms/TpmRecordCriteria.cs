using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using System;

namespace SIE.EMS.Tpms
{
    /// <summary>
    /// TPM记录查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("TPM记录查询实体")]
    public partial class TpmRecordCriteria : Criteria
    {
        #region 设备编码 EquipCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipCodeProperty = P<TpmRecordCriteria>.Register(e => e.EquipCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipCode
        {
            get { return GetProperty(EquipCodeProperty); }
            set { SetProperty(EquipCodeProperty, value); }
        }
        #endregion

        #region 机台号 MachineNo
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> MachineNoProperty = P<TpmRecordCriteria>.Register(e => e.MachineNo);

        /// <summary>
        /// 机台号
        /// </summary>
        public string MachineNo
        {
            get { return GetProperty(MachineNoProperty); }
            set { SetProperty(MachineNoProperty, value); }
        }
        #endregion

        #region 车间 Workshop
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间")]
        public static readonly IRefIdProperty WorkshopIdProperty = P<TpmRecordCriteria>.RegisterRefId(e => e.WorkshopId, ReferenceType.Normal);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double? WorkshopId
        {
            get { return (double?)GetRefNullableId(WorkshopIdProperty); }
            set { SetRefNullableId(WorkshopIdProperty, value); }
        }

        /// <summary>
        /// 车间
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> WorkshopProperty = P<TpmRecordCriteria>.RegisterRef(e => e.Workshop, WorkshopIdProperty);

        /// <summary>
        /// 车间
        /// </summary>
        public Enterprise Workshop
        {
            get { return GetRefEntity(WorkshopProperty); }
            set { SetRefEntity(WorkshopProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty = P<TpmRecordCriteria>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)GetRefNullableId(ProcessIdProperty); }
            set { SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty = P<TpmRecordCriteria>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<TpmRecordCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<TpmController>().GetTpmRecords(this);
        }
    }
}
