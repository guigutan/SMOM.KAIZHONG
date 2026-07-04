using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 上料采集单体条码
    /// </summary>
    [ChildEntity, Serializable]
    [Label("上料采集单体条码")]
    public partial class WipProductProcessReportSinglelabel : WipProductProcessSinglelabel
    {
        #region 生产采集记录 WipProductProcess
        /// <summary>
        /// 生产采集记录Id
        /// </summary>
        public static new readonly IRefIdProperty WipProductProcessIdProperty = P<WipProductProcessReportSinglelabel>.RegisterRefId(e => e.WipProductProcessId, ReferenceType.Parent);

        /// <summary>
        /// 生产采集记录Id
        /// </summary>
        public new double WipProductProcessId
        {
            get { return (double)GetRefId(WipProductProcessIdProperty); }
            set { SetRefId(WipProductProcessIdProperty, value); }
        }

        /// <summary>
        /// 生产采集记录
        /// </summary>
        public static new readonly RefEntityProperty<WipProductReportProcess> WipProductProcessProperty = P<WipProductProcessReportSinglelabel>.RegisterRef(e => e.WipProductProcess, WipProductProcessIdProperty);

        /// <summary>
        /// 生产采集记录
        /// </summary>
        public new WipProductReportProcess WipProductProcess
        {
            get { return GetRefEntity(WipProductProcessProperty); }
            set { SetRefEntity(WipProductProcessProperty, value); }
        }
        #endregion

    }
}
