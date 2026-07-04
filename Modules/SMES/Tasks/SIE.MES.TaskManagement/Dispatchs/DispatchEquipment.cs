using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工对象设备
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("派工对象设备")]
    public partial class DispatchEquipment : DataEntity
    {
        #region 设备 Equipment
        /// <summary>
        /// 设备Id
        /// </summary>
        [Label("设备")]
        public static readonly IRefIdProperty EquipmentIdProperty = P<DispatchEquipment>.RegisterRefId(e => e.EquipmentId, ReferenceType.Normal);

        /// <summary>
        /// 设备Id
        /// </summary>
        public double EquipmentId
        {
            get { return (double)GetRefId(EquipmentIdProperty); }
            set { SetRefId(EquipmentIdProperty, value); }
        }

        /// <summary>
        /// 设备
        /// </summary>
        public static readonly RefEntityProperty<EquipAccount> EquipmentProperty = P<DispatchEquipment>.RegisterRef(e => e.Equipment, EquipmentIdProperty);

        /// <summary>
        /// 设备
        /// </summary>
        public EquipAccount Equipment
        {
            get { return GetRefEntity(EquipmentProperty); }
            set { SetRefEntity(EquipmentProperty, value); }
        }
        #endregion

        #region 派工任务明细 TaskDetail
        /// <summary>
        /// 派工任务明细Id
        /// </summary>
        [Label("派工任务明细")]
        public static readonly IRefIdProperty TaskDetailIdProperty = P<DispatchEquipment>.RegisterRefId(e => e.TaskDetailId, ReferenceType.Parent);

        /// <summary>
        /// 派工任务明细Id
        /// </summary>
        public double TaskDetailId
        {
            get { return (double)GetRefId(TaskDetailIdProperty); }
            set { SetRefId(TaskDetailIdProperty, value); }
        }

        /// <summary>
        /// 派工任务明细
        /// </summary>
        public static readonly RefEntityProperty<DispatchTaskDetail> TaskDetailProperty = P<DispatchEquipment>.RegisterRef(e => e.TaskDetail, TaskDetailIdProperty);

        /// <summary>
        /// 派工任务明细
        /// </summary>
        public DispatchTaskDetail TaskDetail
        {
            get { return GetRefEntity(TaskDetailProperty); }
            set { SetRefEntity(TaskDetailProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 派工对象设备 实体配置
    /// </summary>
    internal class DispatchEquipmentConfig : EntityConfig<DispatchEquipment>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_DISP_EQUIP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}