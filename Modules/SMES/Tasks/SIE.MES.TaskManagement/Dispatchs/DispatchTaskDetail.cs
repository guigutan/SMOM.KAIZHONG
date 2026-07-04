using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 派工任务明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("派工任务明细")]
    public partial class DispatchTaskDetail : DataEntity
    {
        #region 对象名称（员工ID|班组ID|员工组ID) AdoId
        /// <summary>
        /// 对象名称（员工ID|班组ID|员工组ID)
        /// </summary>
        [Required]
        [Label("对象名称（员工ID|班组ID|员工组ID)")]
        public static readonly Property<double> AdoIdProperty = P<DispatchTaskDetail>.Register(e => e.AdoId);

        /// <summary>
        /// 对象名称（员工ID|班组ID|员工组ID)
        /// </summary>
        public double AdoId
        {
            get { return GetProperty(AdoIdProperty); }
            set { SetProperty(AdoIdProperty, value); }
        }
        #endregion

        #region 对象名称 AdoName
        /// <summary>
        /// 对象名称
        /// </summary>
        [Required]
        [Label("对象名称")]
        public static readonly Property<string> AdoNameProperty = P<DispatchTaskDetail>.Register(e => e.AdoName);

        /// <summary>
        /// 对象名称
        /// </summary>
        public string AdoName
        {
            get { return GetProperty(AdoNameProperty); }
            set { SetProperty(AdoNameProperty, value); }
        }
        #endregion

        #region 工序技能匹配度 ProcessMatchDegree
        /// <summary>
        /// 工序技能匹配度
        /// </summary>
        [Label("工序技能匹配度")]
        public static readonly Property<decimal> ProcessMatchDegreeProperty = P<DispatchTaskDetail>.Register(e => e.ProcessMatchDegree);

        /// <summary>
        /// 工序技能匹配度
        /// </summary>
        public decimal ProcessMatchDegree
        {
            get { return GetProperty(ProcessMatchDegreeProperty); }
            set { SetProperty(ProcessMatchDegreeProperty, value); }
        }
        #endregion

        #region 对象类型 AdoType
        /// <summary>
        /// 对象类型
        /// </summary>
        [Label("对象类型")]
        public static readonly Property<AdoType> AdoTypeProperty = P<DispatchTaskDetail>.Register(e => e.AdoType);

        /// <summary>
        /// 对象类型
        /// </summary>
        public AdoType AdoType
        {
            get { return GetProperty(AdoTypeProperty); }
            set { SetProperty(AdoTypeProperty, value); }
        }
        #endregion

        #region 员工类型 AdoGroup(只针对员工对象，区分员工是否属于员工组或班组)
        /// <summary>
        /// 员工类型
        /// </summary>
        [Label("员工类型")]
        public static readonly Property<AdoGroup?> AdoGroupProperty = P<DispatchTaskDetail>.Register(e => e.AdoGroup);

        /// <summary>
        /// 员工类型
        /// </summary>
        public AdoGroup? AdoGroup
        {
            get { return GetProperty(AdoGroupProperty); }
            set { SetProperty(AdoGroupProperty, value); }
        }
        #endregion

        #region 派工任务 DispatchTask
        /// <summary>
        /// 派工任务Id
        /// </summary>
        [Label("派工任务")]
        public static readonly IRefIdProperty DispatchTaskIdProperty = P<DispatchTaskDetail>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Parent);

        /// <summary>
        /// 派工任务Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)GetRefId(DispatchTaskIdProperty); }
            set { SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工任务
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty = P<DispatchTaskDetail>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工任务
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return GetRefEntity(DispatchTaskProperty); }
            set { SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 指定设备列表 Equipments
        /// <summary>
        /// 指定设备列表
        /// </summary>
        public static readonly ListProperty<EntityList<DispatchEquipment>> EquipmentsProperty = P<DispatchTaskDetail>.RegisterList(e => e.Equipments);

        /// <summary>
        /// 指定设备列表
        /// </summary>
        public EntityList<DispatchEquipment> Equipments
        {
            get { return this.GetLazyList(EquipmentsProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 派工任务明细 实体配置
    /// </summary>
    internal class DispatchTaskDetailConfig : EntityConfig<DispatchTaskDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_DISP_TASK_DTL").MapAllProperties();
            Meta.Property(DispatchTaskDetail.DispatchTaskIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}