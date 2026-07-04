using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 产品测试结果
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品测试结果")]
    public partial class WipProductReportTestResult : WipProductTestResult
    {
        #region 采集记录 Process
        /// <summary>
        /// 采集记录Id
        /// </summary>
        public static new readonly IRefIdProperty ProcessIdProperty = P<WipProductReportTestResult>.RegisterRefId(e => e.ProcessId, ReferenceType.Parent);

        /// <summary>
        /// 采集记录Id
        /// </summary>
        public new double ProcessId
        {
            get { return (double)GetRefId(ProcessIdProperty); }
            set { SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 采集记录
        /// </summary>
        public static new readonly RefEntityProperty<WipProductReportProcess> ProcessProperty = P<WipProductReportTestResult>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 采集记录
        /// </summary>
        public new WipProductReportProcess Process
        {
            get { return GetRefEntity(ProcessProperty); }
            set { SetRefEntity(ProcessProperty, value); }
        }
        #endregion
    }
}
