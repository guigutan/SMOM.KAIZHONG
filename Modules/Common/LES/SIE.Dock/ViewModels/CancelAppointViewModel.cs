using SIE.Domain;
using SIE.ObjectModel;
using SIE.Warehouses;
using System;

namespace SIE.Dock.ViewModels
{
    /// <summary>
    /// 取消预约
    /// </summary>
    [Serializable, RootEntity]
    [Label("取消预约")]
    public class CancelAppointViewModel : ViewModel
    {
        #region 操作类型 Type
        /// <summary>
        /// 操作类型(Type=0是预约取消，Type=1是排队取消)
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<int> TypeProperty = P<CancelAppointViewModel>.Register(e => e.Type);

        /// <summary>
        /// 操作类型
        /// </summary>
        public int Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 取消原因 CancelReason
        /// <summary>
        /// 取消原因Id
        /// </summary>
        [Label("取消原因")]
        public static readonly IRefIdProperty CancelReasonIdProperty =
            P<CancelAppointViewModel>.RegisterRefId(e => e.CancelReasonId, ReferenceType.Normal);

        /// <summary>
        /// 取消原因Id
        /// </summary>
        public double? CancelReasonId
        {
            get { return (double?)this.GetRefNullableId(CancelReasonIdProperty); }
            set { this.SetRefNullableId(CancelReasonIdProperty, value); }
        }

        /// <summary>
        /// 取消原因
        /// </summary>
        public static readonly RefEntityProperty<Reason> CancelReasonProperty =
            P<CancelAppointViewModel>.RegisterRef(e => e.CancelReason, CancelReasonIdProperty);

        /// <summary>
        /// 取消原因
        /// </summary>
        public Reason CancelReason
        {
            get { return this.GetRefEntity(CancelReasonProperty); }
            set { this.SetRefEntity(CancelReasonProperty, value); }
        }
        #endregion

        #region 原因名称 ReasonName
        /// <summary>
        /// 原因名称
        /// </summary>
        [Label("原因名称")]
        public static readonly Property<string> ReasonNameProperty = P<CancelAppointViewModel>.RegisterView(e => e.ReasonName, p => p.CancelReason.Name);

        /// <summary>
        /// 原因名称
        /// </summary>
        public string ReasonName
        {
            get { return this.GetProperty(ReasonNameProperty); }
        }
        #endregion

        #region 原因描述 ReasonDesc
        /// <summary>
        /// 原因描述
        /// </summary>
        [Label("原因描述")]
        public static readonly Property<string> ReasonDescProperty = P<CancelAppointViewModel>.Register(e => e.ReasonDesc);

        /// <summary>
        /// 原因描述
        /// </summary>
        public string ReasonDesc
        {
            get { return this.GetProperty(ReasonDescProperty); }
            set { this.SetProperty(ReasonDescProperty, value); }
        }
        #endregion
    }
}