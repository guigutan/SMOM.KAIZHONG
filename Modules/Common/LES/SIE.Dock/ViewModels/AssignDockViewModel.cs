using SIE.Dock.DockAppoints;
using SIE.Dock.DockMaintains;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.ViewModels
{
    /// <summary>
    /// 分配月台
    /// </summary>
    [Serializable, RootEntity]
    [Label("分配月台")]
    public class AssignDockViewModel : ViewModel
    {
        #region 园区ID YardId
        /// <summary>
        /// 园区ID
        /// </summary>
        [Label("园区ID")]
        public static readonly Property<double> YardIdProperty = P<AssignDockViewModel>.Register(e => e.YardId);

        /// <summary>
        /// 园区ID
        /// </summary>
        public double YardId
        {
            get { return this.GetProperty(YardIdProperty); }
            set { this.SetProperty(YardIdProperty, value); }
        }
        #endregion

        #region 预约类型 AppointType
        /// <summary>
        /// 预约类型
        /// </summary>
        [Label("预约类型")]
        public static readonly Property<AppointType> AppointTypeProperty = P<AssignDockViewModel>.Register(e => e.AppointType);

        /// <summary>
        /// 预约类型
        /// </summary>
        public AppointType AppointType
        {
            get { return this.GetProperty(AppointTypeProperty); }
            set { this.SetProperty(AppointTypeProperty, value); }
        }
        #endregion

        #region 月台排队Id DockQueueId
        /// <summary>
        /// 月台排队Id
        /// </summary>
        [Label("月台排队Id")]
        public static readonly Property<double> DockQueueIdProperty = P<AssignDockViewModel>.Register(e => e.DockQueueId);

        /// <summary>
        /// 月台排队Id
        /// </summary>
        public double DockQueueId
        {
            get { return this.GetProperty(DockQueueIdProperty); }
            set { this.SetProperty(DockQueueIdProperty, value); }
        }
        #endregion

        #region 是否立即分配 IsAtOnceAssign
        /// <summary>
        /// 是否立即分配
        /// </summary>
        [Label("是否立即分配")]
        public static readonly Property<bool> IsAtOnceAssignProperty = P<AssignDockViewModel>.Register(e => e.IsAtOnceAssign);

        /// <summary>
        /// 是否立即分配
        /// </summary>
        public bool IsAtOnceAssign
        {
            get { return this.GetProperty(IsAtOnceAssignProperty); }
            set { this.SetProperty(IsAtOnceAssignProperty, value); }
        }
        #endregion

        #region 月台 DockMaintain
        /// <summary>
        /// 月台Id
        /// </summary>
        [Label("月台")]
        public static readonly IRefIdProperty DockMaintainIdProperty =
            P<AssignDockViewModel>.RegisterRefId(e => e.DockMaintainId, ReferenceType.Normal);

        /// <summary>
        /// 月台Id
        /// </summary>
        public double? DockMaintainId
        {
            get { return (double?)this.GetRefNullableId(DockMaintainIdProperty); }
            set { this.SetRefNullableId(DockMaintainIdProperty, value); }
        }

        /// <summary>
        /// 月台
        /// </summary>
        public static readonly RefEntityProperty<DockMaintain> DockMaintainProperty =
            P<AssignDockViewModel>.RegisterRef(e => e.DockMaintain, DockMaintainIdProperty);

        /// <summary>
        /// 月台
        /// </summary>
        public DockMaintain DockMaintain
        {
            get { return this.GetRefEntity(DockMaintainProperty); }
            set { this.SetRefEntity(DockMaintainProperty, value); }
        }
        #endregion
    }
}