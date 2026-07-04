using SIE.Domain;
using SIE.MES.WIP.Products;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Report.WipProducts
{
    /// <summary>
    /// 生产采集记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("生产采集记录")]
    public partial class WipProductReportProcess : WipProductProcess
    {
        #region 采集记录 Version
        /// <summary>
        /// 采集记录Id
        /// </summary>
        [Label("采集记录")]
        public static new readonly IRefIdProperty VersionIdProperty = P<WipProductReportProcess>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 采集记录Id
        /// </summary>
        public new double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 采集记录
        /// </summary>
        public static new readonly RefEntityProperty<WipProductVersionReport> VersionProperty = P<WipProductReportProcess>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 采集记录
        /// </summary>
        public new WipProductVersionReport Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        #region 测试结果 TestResultList
        /// <summary>
        /// 测试结果
        /// </summary>
        [Label("测试结果")]
        public static new readonly ListProperty<EntityList<WipProductReportTestResult>> TestResultListProperty = P<WipProductReportProcess>.RegisterList(e => e.TestResultList);

        /// <summary>
        /// 测试结果
        /// </summary>
        public new EntityList<WipProductReportTestResult> TestResultList
        {
            get { return this.GetLazyList(TestResultListProperty); }
        }
        #endregion

        #region 关键件 KeyItemList
        /// <summary>
        /// 关键件
        /// </summary>
        [Label("关键件")]
        public static new readonly ListProperty<EntityList<WipProductReportProcessKeyItem>> KeyItemListProperty = P<WipProductReportProcess>.RegisterList(e => e.KeyItemList);

        /// <summary>
        /// 关键件
        /// </summary>
        public new EntityList<WipProductReportProcessKeyItem> KeyItemList
        {
            get { return this.GetLazyList(KeyItemListProperty); }
        }
        #endregion

        #region 上料采集单体条码 SinglelabelList
        /// <summary>
        /// 上料采集单体条码
        /// </summary>
        [Label("上料采集单体条码")]
        public static new readonly ListProperty<EntityList<WipProductProcessReportSinglelabel>> SinglelabelListProperty = P<WipProductReportProcess>.RegisterList(e => e.SinglelabelList);

        /// <summary>
        /// 上料采集单体条码
        /// </summary>
        public new EntityList<WipProductProcessReportSinglelabel> SinglelabelList
        {
            get { return this.GetLazyList(SinglelabelListProperty); }
        }
        #endregion

        #region 员工信息 EmployeeList
        /// <summary>
        /// 员工信息
        /// </summary>
        [Label("员工信息")]
        public static new readonly ListProperty<EntityList<WipProductProcessReportEmployee>> EmployeeListProperty = P<WipProductReportProcess>.RegisterList(e => e.EmployeeList);

        /// <summary>
        /// 员工信息
        /// </summary>
        public new EntityList<WipProductProcessReportEmployee> EmployeeList
        {
            get { return this.GetLazyList(EmployeeListProperty); }
        }
        #endregion

    }
}
