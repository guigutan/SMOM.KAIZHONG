using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Interfaces.ApsTasks
{
    /// <summary>
    /// 计划任务关联
    /// </summary>
    [RootEntity, Serializable]
    ////[CriteriaQuery]
    [Label("计划任务关联")]
    public partial class TaskUnion : DataEntity
    {
        #region 计划任务ID PlanTaskId
        /// <summary>
        /// 计划任务ID
        /// </summary>
        [Label("计划任务ID")]
        public static readonly Property<string> PlanTaskIdProperty = P<TaskUnion>.Register(e => e.PlanTaskId);

        /// <summary>
        /// 计划任务ID
        /// </summary>
        public string PlanTaskId
        {
            get { return GetProperty(PlanTaskIdProperty); }
            set { SetProperty(PlanTaskIdProperty, value); }
        }
        #endregion

        #region 计划单号 PlanNo
        /// <summary>
        /// 计划单号
        /// </summary>
        [Label("计划单号")]
        public static readonly Property<string> PlanNoProperty = P<TaskUnion>.Register(e => e.PlanNo);

        /// <summary>
        /// 计划单号
        /// </summary>
        public string PlanNo
        {
            get { return GetProperty(PlanNoProperty); }
            set { SetProperty(PlanNoProperty, value); }
        }
        #endregion

        #region 模具Id MouldId
        /// <summary>
        /// 模具Id
        /// </summary>
        [Label("模具Id")]
        public static readonly Property<double?> MouldIdProperty = P<TaskUnion>.Register(e => e.MouldId);

        /// <summary>
        /// 模具Id
        /// </summary>
        public double? MouldId
        {
            get { return GetProperty(MouldIdProperty); }
            set { SetProperty(MouldIdProperty, value); }
        }
        #endregion

        #region 模具条码Id MouldBarId
        /// <summary>
        /// 模具条码Id
        /// </summary>
        [Label("模具条码Id")]
        public static readonly Property<double?> MouldBarIdProperty = P<TaskUnion>.Register(e => e.MouldBarId);

        /// <summary>
        /// 模具条码Id
        /// </summary>
        public double? MouldBarId
        {
            get { return GetProperty(MouldBarIdProperty); }
            set { SetProperty(MouldBarIdProperty, value); }
        }
        #endregion

        #region 是否共模生产 IsSameMode
        /// <summary>
        /// 是否共模生产
        /// </summary>
        [Label("是否共模生产")]
        public static readonly Property<bool> IsSameModeProperty = P<TaskUnion>.Register(e => e.IsSameMode);

        /// <summary>
        /// 是否共模生产
        /// </summary>
        public bool IsSameMode
        {
            get { return GetProperty(IsSameModeProperty); }
            set { SetProperty(IsSameModeProperty, value); }
        }
        #endregion

        #region 车间Id WorkShopId
        /// <summary>
        /// 车间Id
        /// </summary>
        [Label("车间Id")]
        public static readonly Property<double> WorkShopIdProperty = P<TaskUnion>.Register(e => e.WorkShopId);

        /// <summary>
        /// 车间Id
        /// </summary>
        public double WorkShopId
        {
            get { return GetProperty(WorkShopIdProperty); }
            set { SetProperty(WorkShopIdProperty, value); }
        }
        #endregion

        #region 资源Id ResourceId
        /// <summary>
        /// 资源Id
        /// </summary>
        [Label("资源Id")]
        public static readonly Property<double> ResourceIdProperty = P<TaskUnion>.Register(e => e.ResourceId);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double ResourceId
        {
            get { return GetProperty(ResourceIdProperty); }
            set { SetProperty(ResourceIdProperty, value); }
        }
        #endregion

        #region 计划任务明细 DetailList
        /// <summary>
        /// 计划任务明细
        /// </summary>
        public static readonly ListProperty<EntityList<TaskUnionDetail>> DetailListProperty = P<TaskUnion>.RegisterList(e => e.DetailList);

        /// <summary>
        /// 计划任务明细
        /// </summary>
        public EntityList<TaskUnionDetail> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 计划任务关联实体配置类
    /// </summary>
    internal class TaskUnionConfig : EntityConfig<TaskUnion>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("TASK_UNION").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}