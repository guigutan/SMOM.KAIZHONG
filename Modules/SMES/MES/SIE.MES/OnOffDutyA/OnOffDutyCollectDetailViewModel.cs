using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Specialized;

namespace SIE.MES.OnOffDutyA
{
    /// <summary>
    /// 采集明细
    /// </summary>
    [RootEntity, Serializable]
    public class OnOffDutyCollectDetailViewModel : ViewModel
    {
        #region 员工号 StaffNO
        /// <summary>
        /// 员工号
        /// </summary>
        [Label("员工号")]
        public static readonly Property<string> StaffNOProperty = P<OnOffDutyCollectDetailViewModel>.Register(e => e.StaffNO);

        /// <summary>
        /// 员工号
        /// </summary>
        public string StaffNO
        {
            get { return this.GetProperty(StaffNOProperty); }
            set { this.SetProperty(StaffNOProperty, value); }
        }
        #endregion

        #region 员工名 StaffName
        /// <summary>
        /// 员工名
        /// </summary>
        [Label("员工名")]
        public static readonly Property<string> StaffNameProperty = P<OnOffDutyCollectDetailViewModel>.Register(e => e.StaffName);

        /// <summary>
        /// 员工名
        /// </summary>
        public string StaffName
        {
            get { return this.GetProperty(StaffNameProperty); }
            set { this.SetProperty(StaffNameProperty, value); }
        }
        #endregion

        #region 上下岗类型 OnOffDutyType
        /// <summary>
        /// 上下岗类型
        /// </summary>
        [Label("上下岗类型")]
        public static readonly Property<OnOffDutyType> OnOffDutyTypeProperty = P<OnOffDutyCollectDetailViewModel>.Register(e => e.OnOffDutyType);

        /// <summary>
        /// 上下岗类型
        /// </summary>
        public OnOffDutyType OnOffDutyType
        {
            get { return this.GetProperty(OnOffDutyTypeProperty); }
            set { this.SetProperty(OnOffDutyTypeProperty, value); }
        }
        #endregion     


        #region 创建时间 CollectDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateTime> CollectDateProperty = P<OnOffDutyCollectDetailViewModel>.Register(e => e.CollectDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CollectDate
        {
            get { return this.GetProperty(CollectDateProperty); }
            set { this.SetProperty(CollectDateProperty, value); }
        }
        #endregion

        #region 上下岗时间 InputDate
        /// <summary>
        /// 上下岗时间
        /// </summary>
        [Label("上下岗时间")]
        public static readonly Property<DateTime> InputDateProperty = P<OnOffDutyCollectDetailViewModel>.Register(e => e.InputDate);

        /// <summary>
        /// 上下岗时间
        /// </summary>
        public DateTime InputDate
        {
            get { return this.GetProperty(InputDateProperty); }
            set { this.SetProperty(InputDateProperty, value); }
        }
        #endregion

        #region 创建用户 CollectUseName
        /// <summary>
        /// 创建用户
        /// </summary>
        [Label("创建用户")]
        public static readonly Property<string> CollectUseNameProperty = P<OnOffDutyCollectDetailViewModel>.Register(e => e.CollectUseName);

        /// <summary>
        /// 创建用户
        /// </summary>
        public string CollectUseName
        {
            get { return this.GetProperty(CollectUseNameProperty); }
            set { this.SetProperty(CollectUseNameProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 采集明细集合
    /// </summary>
    public class OnOffDutyCollectDetailViewModelList : EntityList<OnOffDutyCollectDetailViewModel>
    {
        /// <summary>
        /// 集合变更通知
        /// </summary>
        /// <param name="e">集合变更参数</param>
        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            base.OnCollectionChanged(e);
        }

        /// <summary>
        /// 插入采集明细
        /// </summary>
        /// <param name="index">插入位置</param>
        /// <param name="item">采集明细对象</param>
        protected override void InsertItem(int index, object item)
        {
            base.InsertItem(0, item);
            if (Count > 20)
                RemoveAt(Count - 1);
        }
    }
}