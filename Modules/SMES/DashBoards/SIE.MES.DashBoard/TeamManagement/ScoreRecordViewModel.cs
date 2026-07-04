using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.DashBoard.TeamManagement
{
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(ScoreRecordVMCriteria))]
    [Label("评分统计表")]
    public class ScoreRecordViewModel : ViewModel
    {
        #region 班组 WorkGroupName
        /// <summary>
        /// 班组
        /// </summary>
        [Label("班组")]
        public static readonly Property<string> WorkGroupNameProperty = P<ScoreRecordViewModel>.Register(e => e.WorkGroupName);

        /// <summary>
        /// 班组
        /// </summary>
        public string WorkGroupName
        {
            get { return this.GetProperty(WorkGroupNameProperty); }
            set { this.SetProperty(WorkGroupNameProperty, value); }
        }
        #endregion

        #region 员工 EmpName
        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        public static readonly Property<string> EmpNameProperty = P<ScoreRecordViewModel>.Register(e => e.EmpName);

        /// <summary>
        /// 员工
        /// </summary>
        public string EmpName
        {
            get { return this.GetProperty(EmpNameProperty); }
            set { this.SetProperty(EmpNameProperty, value); }
        }
        #endregion

        #region 日期格式串 OccurDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期格式串")]
        public static readonly Property<string> OccurDateProperty = P<ScoreRecordViewModel>.Register(e => e.OccurDate);

        /// <summary>
        /// 日期
        /// </summary>
        public string OccurDate
        {
            get { return this.GetProperty(OccurDateProperty); }
            set { this.SetProperty(OccurDateProperty, value); }
        }
        #endregion

        #region 项目分值 Score
        /// <summary>
        /// 项目分值
        /// </summary>
        [Label("项目分值")]
        public static readonly Property<decimal> ScoreProperty = P<ScoreRecordViewModel>.Register(e => e.Score);

        /// <summary>
        /// 项目分值
        /// </summary>
        public decimal Score
        {
            get { return GetProperty(ScoreProperty); }
            set { SetProperty(ScoreProperty, value); }
        }
        #endregion

        #region 员工Id EmpId
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工Id")]
        public static readonly Property<double> EmpIdProperty = P<ScoreRecordViewModel>.Register(e => e.EmpId);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double EmpId
        {
            get { return this.GetProperty(EmpIdProperty); }
            set { this.SetProperty(EmpIdProperty, value); }
        }
        #endregion

        #region 班组Id WorkgroupId
        /// <summary>
        /// 班组Id
        /// </summary>
        [Label("班组Id")]
        public static readonly Property<double> WorkgroupIdProperty = P<ScoreRecordViewModel>.Register(e => e.WorkgroupId);

        /// <summary>
        /// 班组Id
        /// </summary>
        public double WorkgroupId
        {
            get { return this.GetProperty(WorkgroupIdProperty); }
            set { this.SetProperty(WorkgroupIdProperty, value); }
        }
        #endregion

        #region 日期 ActualDate
        /// <summary>
        /// 日期
        /// </summary>
        [Label("日期")]
        public static readonly Property<DateTime?> ActualDateProperty = P<ScoreRecordViewModel>.Register(e => e.ActualDate);

        /// <summary>
        /// 日期
        /// </summary>
        public DateTime? ActualDate
        {
            get { return this.GetProperty(ActualDateProperty); }
            set { this.SetProperty(ActualDateProperty, value); }
        }
        #endregion

    }
}
