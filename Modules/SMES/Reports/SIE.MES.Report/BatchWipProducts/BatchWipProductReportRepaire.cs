using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 产品维修记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品维修记录")]
    public partial class BatchWipProductReportRepaire : BatchWipProductRepaire
    {
        #region 版本 Version
        /// <summary>
        /// 版本Id
        /// </summary>
        [Label("版本")]
        public static new readonly IRefIdProperty VersionIdProperty = P<BatchWipProductReportRepaire>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 版本Id
        /// </summary>
        public new double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public static new readonly RefEntityProperty<BatchWipProductVersionReport> VersionProperty = P<BatchWipProductReportRepaire>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 版本
        /// </summary>
        public new BatchWipProductVersionReport Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion
    }
}
