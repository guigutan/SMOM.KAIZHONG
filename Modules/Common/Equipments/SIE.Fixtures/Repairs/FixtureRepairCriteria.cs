using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Repairs
{
    /// <summary>
	/// 工治具报修查询体
	/// </summary>
	[QueryEntity, Serializable]
    [Label("工治具报修查询体")]
    public class FixtureRepairCriteria : Criteria
    {

        #region 维修单号 No
        /// <summary>
        /// 维修单号
        /// </summary>
        [Label("维修单号")]
        public static readonly Property<string> NoProperty = P<FixtureRepairCriteria>.Register(e => e.No);

        /// <summary>
        /// 维修单号
        /// </summary>
        public string No
        {
            get { return GetProperty(NoProperty); }
            set { SetProperty(NoProperty, value); }
        }
        #endregion

        #region 维修完成时间 RepairDate
        /// <summary>
        /// 维修完成时间
        /// </summary>
        [Label("维修完成时间")]
        public static readonly Property<DateRange> RepairDateProperty = P<FixtureRepairCriteria>.Register(e => e.RepairDate);

        /// <summary>
        /// 维修完成时间
        /// </summary>
        public DateRange RepairDate
        {
            get { return GetProperty(RepairDateProperty); }
            set { SetProperty(RepairDateProperty, value); }
        }
        #endregion

        #region 报修时间 ApplyDate
        /// <summary>
        /// 报修时间
        /// </summary>
        [Label("报修时间")]
        public static readonly Property<DateRange> ApplyDateProperty = P<FixtureRepairCriteria>.Register(e => e.ApplyDate);

        /// <summary>
        /// 报修时间
        /// </summary>
        public DateRange ApplyDate
        {
            get { return GetProperty(ApplyDateProperty); }
            set { SetProperty(ApplyDateProperty, value); }
        }
        #endregion

        #region 单据状态 RepairState
        /// <summary>
        /// 单据状态
        /// </summary>
        [Label("单据状态")]
        public static readonly Property<RepairState?> RepairStateProperty = P<FixtureRepairCriteria>.Register(e => e.RepairState);

        /// <summary>
        /// 单据状态
        /// </summary>
        public RepairState? RepairState
        {
            get { return GetProperty(RepairStateProperty); }
            set { SetProperty(RepairStateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<CoreFixtureController>().GetFixtureRepairList(this);
        }
    }
}
