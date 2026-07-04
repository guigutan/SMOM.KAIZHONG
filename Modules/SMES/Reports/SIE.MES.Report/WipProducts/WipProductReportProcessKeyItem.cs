using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 产品生产关键件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品生产关键件")]
    public partial class WipProductReportProcessKeyItem : WipProductProcessKeyItem
    {
        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static new readonly IRefIdProperty ProcessIdProperty = P<WipProductReportProcessKeyItem>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 工序
        /// </summary>
        public new double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static new readonly RefEntityProperty<WipProductReportProcess> ProcessProperty = P<WipProductReportProcessKeyItem>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public new WipProductReportProcess Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion
    }
}
