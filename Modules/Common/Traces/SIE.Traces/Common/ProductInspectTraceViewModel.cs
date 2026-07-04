using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Traces.Common
{
    /// <summary>
    /// 工序检验追溯
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("工序检验记录")]
	public partial class ProductInspectTraceViewModel : ViewModel
    {
        #region 项目名称 Name
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> NameProperty = P<ProductInspectTraceViewModel>.Register(e => e.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 规范下限 LimitLow
        /// <summary>
        /// 规范下限
        /// </summary>
        [Label("规范下限")]
        public static readonly Property<decimal?> LimitLowProperty = P<ProductInspectTraceViewModel>.Register(e => e.LimitLow);

        /// <summary>
        /// 规范下限
        /// </summary>
        public decimal? LimitLow
        {
            get { return GetProperty(LimitLowProperty); }
            set { SetProperty(LimitLowProperty, value); }
        }
        #endregion

        #region 规范上限 LimitMax
        /// <summary>
        /// 规范上限
        /// </summary>
        [Label("规范上限")]
        public static readonly Property<decimal?> LimitMaxProperty = P<ProductInspectTraceViewModel>.Register(e => e.LimitMax);

        /// <summary>
        /// 规范上限
        /// </summary>
        public decimal? LimitMax
        {
            get { return GetProperty(LimitMaxProperty); }
            set { SetProperty(LimitMaxProperty, value); }
        }
        #endregion

        #region 测试值 InspectionValue
        /// <summary>
        /// 测试值
        /// </summary>
        [Label("测试值")]
        public static readonly Property<decimal?> InspectionValueProperty = P<ProductInspectTraceViewModel>.Register(e => e.InspectionValue);

        /// <summary>
        /// 测试值
        /// </summary>
        public decimal? InspectionValue
        {
            get { return GetProperty(InspectionValueProperty); }
            set { SetProperty(InspectionValueProperty, value); }
        }
        #endregion

        #region 备注 Remarks
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarksProperty = P<ProductInspectTraceViewModel>.Register(e => e.Remarks);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remarks
        {
            get { return GetProperty(RemarksProperty); }
            set { SetProperty(RemarksProperty, value); }
        }
        #endregion

        #region 检验结果 Result
        /// <summary>
        /// 检验结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<string> ResultProperty = P<ProductInspectTraceViewModel>.Register(e => e.Result);

        /// <summary>
        /// 检验结果
        /// </summary>
        public string Result
        {
            get { return GetProperty(ResultProperty); }
            set { SetProperty(ResultProperty, value); }
        }
        #endregion
	}
}