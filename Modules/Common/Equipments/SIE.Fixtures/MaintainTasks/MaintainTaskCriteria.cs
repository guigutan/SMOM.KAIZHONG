using SIE.Domain;
using SIE.Fixtures;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.MaintainTasks
{
    /// <summary>
	/// 保养任务查询体
	/// </summary>
	[QueryEntity, Serializable]
    [Label("保养任务查询实体")]
    public class MaintainTaskCriteria : Criteria
    {
        #region 保养任务编号 No
        /// <summary>
        /// 保养任务编号
        /// </summary>
        [Label("保养任务编号")]
        public static readonly Property<string> NoProperty = P<MaintainTaskCriteria>.Register(e => e.No);

        /// <summary>
        /// 保养任务编号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 关联单号 RelatedNo
        /// <summary>
        /// 关联单号
        /// </summary>
        [Label("关联单号")]
        public static readonly Property<string> RelatedNoProperty = P<MaintainTaskCriteria>.Register(e => e.RelatedNo);

        /// <summary>
        /// 关联单号
        /// </summary>
        public string RelatedNo
        {
            get { return this.GetProperty(RelatedNoProperty); }
            set { this.SetProperty(RelatedNoProperty, value); }
        }
        #endregion

        #region 保养触发条件 MaintainType
        /// <summary>
        /// 保养触发条件
        /// </summary>
        [Label("保养触发条件")]
        public static readonly Property<MaintainType?> MaintainTypeProperty = P<MaintainTaskCriteria>.Register(e => e.MaintainType);

        /// <summary>
        /// 保养触发条件
        /// </summary>
        public MaintainType? MaintainType
        {
            get { return GetProperty(MaintainTypeProperty); }
            set { SetProperty(MaintainTypeProperty, value); }
        }
        #endregion

        #region 工治具ID IdCode
        /// <summary>
        /// 工治具ID
        /// </summary>
        [Label("工治具ID")]
        public static readonly Property<string> IdCodeProperty = P<MaintainTaskCriteria>.Register(e => e.IdCode);

        /// <summary>
        /// 工治具ID
        /// </summary>
        public string IdCode
        {
            get { return this.GetProperty(IdCodeProperty); }
            set { this.SetProperty(IdCodeProperty, value); }
        }
        #endregion

        #region 工治具编码 EncodeCode
        /// <summary>
        /// 工治具编码
        /// </summary>
        [Label("工治具编码")]
        public static readonly Property<string> EncodeCodeProperty = P<MaintainTaskCriteria>.Register(e => e.EncodeCode);

        /// <summary>
        /// 工治具编码
        /// </summary>
        public string EncodeCode
        {
            get { return this.GetProperty(EncodeCodeProperty); }
            set { this.SetProperty(EncodeCodeProperty, value); }
        }
        #endregion


        #region 工治具类型 FixtureType
        /// <summary>
        /// 工治具类型Id
        /// </summary>
        [Label("工治具类型")]
        public static readonly IRefIdProperty FixtureTypeIdProperty =
            P<MaintainTaskCriteria>.RegisterRefId(e => e.FixtureTypeId, ReferenceType.Normal);

        /// <summary>
        /// 工治具类型Id
        /// </summary>
        public double? FixtureTypeId
        {
            get { return (double?)this.GetRefNullableId(FixtureTypeIdProperty); }
            set { this.SetRefNullableId(FixtureTypeIdProperty, value); }
        }

        /// <summary>
        /// 工治具类型
        /// </summary>
        public static readonly RefEntityProperty<SIE.Fixtures.FixtureTypes.FixtureType> FixtureTypeProperty =
            P<MaintainTaskCriteria>.RegisterRef(e => e.FixtureType, FixtureTypeIdProperty);

        /// <summary>
        /// 工治具类型
        /// </summary>
        public SIE.Fixtures.FixtureTypes.FixtureType FixtureType
        {
            get { return this.GetRefEntity(FixtureTypeProperty); }
            set { this.SetRefEntity(FixtureTypeProperty, value); }
        }
        #endregion

        #region 保养状态 State
        /// <summary>
        /// 保养状态
        /// </summary>
        [Label("保养状态")]
        public static readonly Property<MaintainState?> StateProperty = P<MaintainTaskCriteria>.Register(e => e.State);

        /// <summary>
        /// 保养状态
        /// </summary>
        public MaintainState? State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 保养单创建时间 ApplyDate
        /// <summary>
        /// 保养单创建时间
        /// </summary>
        [Label("保养单创建时间")]
        public static readonly Property<DateRange> ApplyDateProperty = P<MaintainTaskCriteria>.Register(e => e.ApplyDate);

        /// <summary>
        /// 保养单创建时间
        /// </summary>
        public DateRange ApplyDate
        {
            get { return GetProperty(ApplyDateProperty); }
            set { SetProperty(ApplyDateProperty, value); }
        }
        #endregion

        #region 保养完成时间 FinishDate
        /// <summary>
        /// 保养完成时间
        /// </summary>
        [Label("保养完成时间")]
        public static readonly Property<DateRange> FinishDateProperty = P<MaintainTaskCriteria>.Register(e => e.FinishDate);

        /// <summary>
        /// 保养完成时间
        /// </summary>
        public DateRange FinishDate
        {
            get { return GetProperty(FinishDateProperty); }
            set { SetProperty(FinishDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<MaintainTaskController>().QueryMaintainTaskList(this);
        }
    }
}
