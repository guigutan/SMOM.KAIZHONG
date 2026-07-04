using SIE.Domain;
using SIE.Domain.Query;
using SIE.EMS.Maintains.Plans;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Maintains.Projects
{
    /// <summary>
    /// 保养项目视图
    /// </summary>
    [RootEntity, Serializable]
    [Label("保养项目视图")]
    public partial class MaintainProjectView : Entity<double>
    {
        #region 保养计划ID PlanId
        /// <summary>
        /// 保养计划ID
        /// </summary>
        [Label("保养计划ID")]
        public static readonly Property<double> PlanIdProperty = P<MaintainProjectView>.Register(e => e.PlanId);

        /// <summary>
        /// 保养计划ID
        /// </summary>
        public double PlanId
        {
            get { return GetProperty(PlanIdProperty); }
            set { SetProperty(PlanIdProperty, value); }
        }
        #endregion

        #region 设备台账ID EquipAccountId
        /// <summary>
        /// 设备台账ID
        /// </summary>
        [Label("设备台账ID")]
        public static readonly Property<double> EquipAccountIdProperty = P<MaintainProjectView>.Register(e => e.EquipAccountId);

        /// <summary>
        /// 设备台账ID
        /// </summary>
        public double EquipAccountId
        {
            get { return GetProperty(EquipAccountIdProperty); }
            set { SetProperty(EquipAccountIdProperty, value); }
        }
        #endregion

        #region 保养开始日期 BeginDate
        /// <summary>
        /// 保养开始日期
        /// </summary>
        [Label("保养开始日期")]
        public static readonly Property<DateTime> BeginDateProperty = P<MaintainProjectView>.Register(e => e.BeginDate);

        /// <summary>
        /// 保养开始日期
        /// </summary>
        public DateTime BeginDate
        {
            get { return GetProperty(BeginDateProperty); }
            set { SetProperty(BeginDateProperty, value); }
        }
        #endregion

        #region 周期 ProjectCycle
        /// <summary>
        /// 周期
        /// </summary>
        [Label("周期")]
        public static readonly Property<decimal> ProjectCycleProperty = P<MaintainProjectView>.Register(e => e.ProjectCycle);

        /// <summary>
        /// 周期
        /// </summary>
        public decimal ProjectCycle
        {
            get { return GetProperty(ProjectCycleProperty); }
            set { SetProperty(ProjectCycleProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 单元 实体配置
    /// </summary>
    internal class MaintainProjectViewViewConfig : EntityConfig<MaintainProjectView>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<MaintainProject>()
                 .Join<MaintainPlan>((pj, pl) => pj.MaintainPlanId == pl.Id)
                 .Select<MaintainPlan>((pj, pl) => new
                 {
                     ID = pl.Id,
                     PLAN_ID = pl.Id,
                     EQUIP_ACCOUNT_ID = pl.EquipAccountId,
                     BEGIN_DATE = pl.PlanBeginDate,                     
                 })
                 .GroupBy<MaintainPlan>((pj, pl) => new
                 {
                     pl.Id,
                     pl.EquipAccountId,
                     pl.PlanBeginDate,                     
                 }).ToQuery();
            Meta.MapView(view).MapAllProperties();
            Meta.DisablePhantoms();
        }
    }
}