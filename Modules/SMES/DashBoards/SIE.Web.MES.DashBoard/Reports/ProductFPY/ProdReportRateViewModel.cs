using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Web.MES.DashBoard.Reports.ProductFPY
{
    /// <summary>
    /// 下钻报表统一返回数据模型
    /// </summary>
    public class ReportDataModel : ViewModel
    {
        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<ReportDataModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #region 良品值 PassQty
        /// <summary>
        /// 良品值
        /// </summary>
        [Label("良品值")]
        public static readonly Property<decimal> PassQtyProperty = P<ReportDataModel>.Register(e => e.PassQty);

        /// <summary>
        /// 良品值
        /// </summary>
        public decimal PassQty
        {
            get { return this.GetProperty(PassQtyProperty); }
            set { this.SetProperty(PassQtyProperty, value); }
        }
        #endregion

        #region 不良品值 FailedQty
        /// <summary>
        /// 不良品值
        /// </summary>
        [Label("不良品值")]
        public static readonly Property<decimal> FailedQtyProperty = P<ReportDataModel>.Register(e => e.FailedQty);

        /// <summary>
        /// 不良品值
        /// </summary>
        public decimal FailedQty
        {
            get { return this.GetProperty(FailedQtyProperty); }
            set { this.SetProperty(FailedQtyProperty, value); }
        }
        #endregion

        #region 良品通过率 MatchValue
        /// <summary>
        /// 良品通过率
        /// </summary>
        [Label("良品通过率")]
        public static readonly Property<decimal> MatchValueProperty = P<ReportDataModel>.Register(e => e.MatchValue);

        /// <summary>
        /// 良品通过率
        /// </summary>
        public decimal MatchValue
        {
            get { return this.GetProperty(MatchValueProperty); }
            set { this.SetProperty(MatchValueProperty, value); }
        }
        #endregion
    }
}
