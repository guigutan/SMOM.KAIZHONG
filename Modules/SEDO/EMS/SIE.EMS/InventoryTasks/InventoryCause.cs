using SIE.Domain;
using SIE.EMS.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.EMS.InventoryTasks
{
    /// <summary>
    /// 盘点任务原因分析
    /// </summary>
    [ChildEntity, Serializable]
    [Label("盘点任务原因分析")]
    public partial class InventoryCause : DataEntity
    {
        #region 设备编码 EquipmentCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> EquipmentCodeProperty = P<InventoryCause>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return GetProperty(EquipmentCodeProperty); }
            set { SetProperty(EquipmentCodeProperty, value); }
        }
        #endregion

        #region 设备名称 EquipmentName
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> EquipmentNameProperty = P<InventoryCause>.Register(e => e.EquipmentName);

        /// <summary>
        /// 设备名称
        /// </summary>
        public string EquipmentName
        {
            get { return GetProperty(EquipmentNameProperty); }
            set { SetProperty(EquipmentNameProperty, value); }
        }
        #endregion

        #region 盘点结果 InventoryResult
        /// <summary>
        /// 盘点结果
        /// </summary>
        [Label("盘点结果")]
        public static readonly Property<InventoryResult> InventoryResultProperty = P<InventoryCause>.Register(e => e.InventoryResult);

        /// <summary>
        /// 盘点结果
        /// </summary>
        public InventoryResult InventoryResult
        {
            get { return this.GetProperty(InventoryResultProperty); }
            set { this.SetProperty(InventoryResultProperty, value); }
        }
        #endregion

        #region 原因分析 Cause
        /// <summary>
        /// 原因分析
        /// </summary>
        [MaxLength(1000)]
        [Label("原因分析")]
        public static readonly Property<string> CauseProperty = P<InventoryCause>.Register(e => e.Cause);

        /// <summary>
        /// 原因分析
        /// </summary>
        public string Cause
        {
            get { return GetProperty(CauseProperty); }
            set { SetProperty(CauseProperty, value); }
        }
        #endregion

        #region 改善措施 Improvements
        /// <summary>
        /// 改善措施
        /// </summary>
        [MaxLength(1000)]
        [Label("改善措施")]
        public static readonly Property<string> ImprovementsProperty = P<InventoryCause>.Register(e => e.Improvements);

        /// <summary>
        /// 改善措施
        /// </summary>
        public string Improvements
        {
            get { return GetProperty(ImprovementsProperty); }
            set { SetProperty(ImprovementsProperty, value); }
        }
        #endregion

        #region 保管人 Keeper
        /// <summary>
        /// 保管人Id
        /// </summary>
        [Label("保管人")]
        public static readonly IRefIdProperty KeeperIdProperty = P<InventoryCause>.RegisterRefId(e => e.KeeperId, ReferenceType.Normal);

        /// <summary>
        /// 保管人Id
        /// </summary>
        public double? KeeperId
        {
            get { return (double?)GetRefNullableId(KeeperIdProperty); }
            set { SetRefNullableId(KeeperIdProperty, value); }
        }

        /// <summary>
        /// 保管人
        /// </summary>
        public static readonly RefEntityProperty<Employee> KeeperProperty = P<InventoryCause>.RegisterRef(e => e.Keeper, KeeperIdProperty);

        /// <summary>
        /// 保管人
        /// </summary>
        public Employee Keeper
        {
            get { return GetRefEntity(KeeperProperty); }
            set { SetRefEntity(KeeperProperty, value); }
        }
        #endregion

        #region 改善责任人 Responsibe
        /// <summary>
        /// 改善责任人Id
        /// </summary>
        [Label("改善责任人")]
        public static readonly IRefIdProperty ResponsibeIdProperty = P<InventoryCause>.RegisterRefId(e => e.ResponsibeId, ReferenceType.Normal);

        /// <summary>
        /// 改善责任人Id
        /// </summary>
        public double? ResponsibeId
        {
            get { return (double?)GetRefNullableId(ResponsibeIdProperty); }
            set { SetRefNullableId(ResponsibeIdProperty, value); }
        }

        /// <summary>
        /// 改善责任人
        /// </summary>
        public static readonly RefEntityProperty<Employee> ResponsibeProperty = P<InventoryCause>.RegisterRef(e => e.Responsibe, ResponsibeIdProperty);

        /// <summary>
        /// 改善责任人
        /// </summary>
        public Employee Responsibe
        {
            get { return GetRefEntity(ResponsibeProperty); }
            set { SetRefEntity(ResponsibeProperty, value); }
        }
        #endregion

        #region 盘点任务 InventoryTask
        /// <summary>
        /// 盘点任务Id
        /// </summary>
        [Label("盘点任务")]
        public static readonly IRefIdProperty InventoryTaskIdProperty = P<InventoryCause>.RegisterRefId(e => e.InventoryTaskId, ReferenceType.Parent);

        /// <summary>
        /// 盘点任务Id
        /// </summary>
        public double InventoryTaskId
        {
            get { return (double)GetRefId(InventoryTaskIdProperty); }
            set { SetRefId(InventoryTaskIdProperty, value); }
        }

        /// <summary>
        /// 盘点任务
        /// </summary>
        public static readonly RefEntityProperty<InventoryTask> InventoryTaskProperty = P<InventoryCause>.RegisterRef(e => e.InventoryTask, InventoryTaskIdProperty);

        /// <summary>
        /// 盘点任务
        /// </summary>
        public InventoryTask InventoryTask
        {
            get { return GetRefEntity(InventoryTaskProperty); }
            set { SetRefEntity(InventoryTaskProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 盘点任务原因分析 实体配置
    /// </summary>
    internal class InventoryCauseConfig : EntityConfig<InventoryCause>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_INV_TASK_CAUSE").MapAllProperties();
            Meta.Property(InventoryCause.CauseProperty).ColumnMeta.HasLength(4000);
            Meta.Property(InventoryCause.ImprovementsProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}