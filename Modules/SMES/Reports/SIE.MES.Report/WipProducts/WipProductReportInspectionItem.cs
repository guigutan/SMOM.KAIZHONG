using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 产品检验记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品检验记录")]
    public partial class WipProductReportInspectionItem : WipProductInspectionItem
    {
        #region 检验记录 Version
        /// <summary>
        /// 检验记录Id
        /// </summary>
        [Label("检验记录")]
        public static new readonly IRefIdProperty VersionIdProperty = P<WipProductReportInspectionItem>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 检验记录Id
        /// </summary>
        public new double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 检验记录
        /// </summary>
        public static new readonly RefEntityProperty<WipProductVersionReport> VersionProperty = P<WipProductReportInspectionItem>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 检验记录
        /// </summary>
        public new WipProductVersionReport Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion
    }
}
