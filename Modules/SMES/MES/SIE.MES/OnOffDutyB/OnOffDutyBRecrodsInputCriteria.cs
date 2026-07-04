using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.OnOffDutyB
{
    /// <summary>
    /// B上下岗录入查询员工
    /// </summary>
    [QueryEntity, Serializable]
    [Label("B上下岗录入查询员工")]
    public partial class OnOffDutyBRecrodsInputCriteria : Criteria
    {
        #region 员工号   StaffCode
        /// <summary>
        /// 员工编码
        /// </summary>
        [Label("员工号")]
        public static readonly Property<string> StaffCodeProperty = P<OnOffDutyBRecrodsInputCriteria>.Register(e => e.StaffCode);

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
        public static readonly Property<string> StaffNameProperty = P<OnOffDutyBRecrodsInputCriteria>.Register(e => e.StaffName);

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
        public static readonly Property<string> StaffGroupNameProperty = P<OnOffDutyBRecrodsInputCriteria>.Register(e => e.StaffGroupName);

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
        /// B执行上下岗录入查询员工
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<OnOffDutyBController>().FetchOnOffDutyRecrodsInput(this);
        }


    }




}
