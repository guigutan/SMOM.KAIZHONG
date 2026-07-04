using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using System;

namespace SIE.MES.OnOffDutyA
{
    /// <summary>
    /// 上下岗录入查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("上下岗录入查询")]
    public partial class OnOffDutyRecrodsInputCriteria : Criteria
    {
        #region 员工号   StaffCode
        /// <summary>
        /// 员工编码
        /// </summary>
        [Label("员工号")]
        public static readonly Property<string> StaffCodeProperty = P<OnOffDutyRecrodsInputCriteria>.Register(e => e.StaffCode);

        /// <summary>
        /// 员工号
        /// </summary>
        public string StaffCode
        {
            get { return this.GetProperty(StaffCodeProperty); }
            set { this.SetProperty(StaffCodeProperty, value); }
        }
        #endregion

        #region 员工姓名   StaffName
        /// <summary>
        /// 员工姓名
        /// </summary>
        [Label("员工姓名")]
        public static readonly Property<string> StaffNameProperty = P<OnOffDutyRecrodsInputCriteria>.Register(e => e.StaffName);

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string StaffName
        {
            get { return this.GetProperty(StaffNameProperty); }
            set { this.SetProperty(StaffNameProperty, value); }
        }
        #endregion


        #region 员工组   StaffGroupName
        /// <summary>
        /// 员工组
        /// </summary>
        [Label("员工组")]
        public static readonly Property<string> StaffGroupNameProperty = P<OnOffDutyRecrodsInputCriteria>.Register(e => e.StaffGroupName);

        /// <summary>
        /// 员工组
        /// </summary>
        public string StaffGroupName
        {
            get { return this.GetProperty(StaffGroupNameProperty); }
            set { this.SetProperty(StaffGroupNameProperty, value); }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<OnOffDutyController>().FetchOnOffDutyRecrodsInput(this);
        }

    }
}