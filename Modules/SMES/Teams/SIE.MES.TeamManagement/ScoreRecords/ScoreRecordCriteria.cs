using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分记录查询实体类
    /// </summary>
    [QueryEntity, Serializable]
    [Label("评分记录查询实体类")]
    public class ScoreRecordCriteria : Criteria
    {
        #region 发起时间 InitiateDate
        /// <summary>
        /// 发起时间
        /// </summary>
        [Label("发起时间")]
        public static readonly Property<DateTime?> InitiateDateProperty = P<ScoreRecordCriteria>.Register(e => e.InitiateDate);

        /// <summary>
        /// 发起时间
        /// </summary>
        public DateTime? InitiateDate
        {
            get { return GetProperty(InitiateDateProperty); }
            set { SetProperty(InitiateDateProperty, value); }
        }
        #endregion

        #region 发生时间 OccurDate
        /// <summary>
        /// 发生时间
        /// </summary>
        [Label("发生时间")]
        public static readonly Property<DateTime?> OccurDateProperty = P<ScoreRecordCriteria>.Register(e => e.OccurDate);

        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime? OccurDate
        {
            get { return GetProperty(OccurDateProperty); }
            set { SetProperty(OccurDateProperty, value); }
        }
        #endregion

        #region 是否失效 LoseEfficacy
        /*/// <summary>
        /// 是否失效
        /// </summary>
        [Label("是否失效")]
        public static readonly Property<bool?> LoseEfficacyProperty = P<ScoreRecordCriteria>.Register(e => e.LoseEfficacy);

        /// <summary>
        /// 是否失效
        /// </summary>
        public bool? LoseEfficacy
        {
            get { return GetProperty(LoseEfficacyProperty); }
            set { SetProperty(LoseEfficacyProperty, value); }
        }*/
        #endregion

        #region 班组成员编码 EmployeeCode
        /// <summary>
        /// 编组成员编码
        /// </summary>
        [Label("班组成员编码")]
        public static readonly Property<string> EmployeeCodeProperty = P<ScoreRecordCriteria>.Register(e => e.EmployeeCode);

        /// <summary>
        /// 编组成员编码
        /// </summary>
        public string EmployeeCode
        {
            get { return GetProperty(EmployeeCodeProperty); }
            set { SetProperty(EmployeeCodeProperty, value); }
        }
        #endregion

        #region 发起人编码 InitiaorCode
        /// <summary>
        /// 发起人编码
        /// </summary>
        [Label("发起人编码")]
        public static readonly Property<string> InitiaorCodeProperty = P<ScoreRecordCriteria>.Register(e => e.InitiaorCode);

        /// <summary>
        /// 发起人编码
        /// </summary>
        public string InitiaorCode
        {
            get { return GetProperty(InitiaorCodeProperty); }
            set { SetProperty(InitiaorCodeProperty, value); }
        }
        #endregion

        #region 评分项目编码 RatedItemCode
        /// <summary>
        /// 评分项目编码
        /// </summary>
        [Label("评分项目编码")]
        public static readonly Property<string> RatedItemCodeProperty = P<ScoreRecordCriteria>.Register(e => e.RatedItemCode);

        /// <summary>
        /// 评分项目编码
        /// </summary>
        public string RatedItemCode
        {
            get { return GetProperty(RatedItemCodeProperty); }
            set { SetProperty(RatedItemCodeProperty, value); }
        }
        #endregion

        #region 状态 ScoreState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("评分状态")]
        public static readonly Property<ScoreState?> ScoreStateProperty = P<ScoreRecordCriteria>.Register(e => e.ScoreState);

        /// <summary>
        /// 状态
        /// </summary>
        public ScoreState? ScoreState
        {
            get { return GetProperty(ScoreStateProperty); }
            set { SetProperty(ScoreStateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 获取数据集合
        /// </summary>
        /// <returns>评分记录集合</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ScoreRecordController>().GetScoreRecords(this);
        }
    }
}
