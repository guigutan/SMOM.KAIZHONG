using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.TeamManagement.Vacancies
{
    /// <summary>
    /// 班组缺编查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("班组缺编查询")]
    public class WorkGroupVacancyCriteria : Criteria
    {
        #region 日期 ClockInDate
        /// <summary>
        /// 日期
        /// </summary>
        [Required]
        [Label("日期")]
        public static readonly Property<DateTime?> ClockInDateProperty = P<WorkGroupVacancyCriteria>.Register(e => e.ClockInDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? ClockInDate
        {
            get { return GetProperty(ClockInDateProperty); }
            set { SetProperty(ClockInDateProperty, value); }
        }
        #endregion

        #region 班组 WorkGroup
        /// <summary>
        /// 班组Id
        /// </summary>
        public static readonly IRefIdProperty WorkGroupIdProperty = P<WorkGroupVacancyCriteria>.RegisterRefId(e => e.WorkGroupId, ReferenceType.Normal);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double? WorkGroupId
        {
            get { return (double)GetRefId(WorkGroupIdProperty); }
            set { SetRefId(WorkGroupIdProperty, value); }
        }

        /// <summary>
        /// 班组
        /// </summary>
        public static readonly RefEntityProperty<WorkGroup> WorkGroupProperty = P<WorkGroupVacancyCriteria>.RegisterRef(e => e.WorkGroup, WorkGroupIdProperty);

        /// <summary>
        /// 班组
        /// </summary>
        public WorkGroup WorkGroup
        {
            get { return GetRefEntity(WorkGroupProperty); }
            set { SetRefEntity(WorkGroupProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取班组缺编信息
        /// </summary>
        /// <returns>班组缺编信息列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<VacancyController>().GetWorkGroupVacancy(this, PagingInfo);
        }
    }
}