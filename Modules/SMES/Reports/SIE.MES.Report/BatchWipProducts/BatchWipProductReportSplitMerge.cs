using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WIP;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Report.BatchWipProducts
{
    /// <summary>
    /// 批次拆分合并记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次拆分合并记录")]
    public partial class BatchWipProductReportSplitMerge : BatchSplitMergeRecord
    {
        #region 版本 Version
        /// <summary>
        /// 版本Id
        /// </summary>
        [Label("版本")]
        public static new readonly IRefIdProperty VersionIdProperty = P<BatchWipProductReportSplitMerge>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

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
        public static new readonly RefEntityProperty<BatchWipProductVersionReport> VersionProperty = P<BatchWipProductReportSplitMerge>.RegisterRef(e => e.Version, VersionIdProperty);

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
