using SIE.Domain;
using SIE.ObjectModel;

namespace SIE.Traces.Common
{
    /// <summary>
    /// 工序缺陷追溯
    /// </summary>
    [RootEntity, Serializable]
	//[CriteriaQuery]
	[Label("工序缺陷记录")]
	public partial class ProductDefectTraceViewModel : ViewModel
    {
        #region 工序 Process
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessProperty = P<ProductDefectTraceViewModel>.Register(e => e.Process);

        /// <summary>
        /// 工序
        /// </summary>
        public string Process
        {
            get { return GetProperty(ProcessProperty); }
            set { SetProperty(ProcessProperty, value); }
        }
        #endregion

        #region 缺陷编码 DefectCode
        /// <summary>
        /// 缺陷编码
        /// </summary>
        [Label("缺陷编码")]
        public static readonly Property<string> DefectCodeProperty = P<ProductDefectTraceViewModel>.Register(e => e.DefectCode);

        /// <summary>
        /// 缺陷编码
        /// </summary>
        public string DefectCode
        {
            get { return GetProperty(DefectCodeProperty); }
            set { SetProperty(DefectCodeProperty, value); }
        }
        #endregion

        #region 缺陷描述 DefectDescription
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescriptionProperty = P<ProductDefectTraceViewModel>.Register(e => e.DefectDescription);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDescription
        {
            get { return GetProperty(DefectDescriptionProperty); }
            set { SetProperty(DefectDescriptionProperty, value); }
        }
        #endregion

        #region 检验项描述 InspItemName
        /// <summary>
        /// 检验项描述
        /// </summary>
        [Label("检验项名称")]
        public static readonly Property<string> InspItemNameProperty = P<ProductDefectTraceViewModel>.Register(e => e.InspItemName);

        /// <summary>
        /// 检验项描述
        /// </summary>
        public string InspItemName
        {
            get { return GetProperty(InspItemNameProperty); }
            set { SetProperty(InspItemNameProperty, value); }
        }
        #endregion

        #region 板号 BoardNo
        /// <summary>
        /// 板号
        /// </summary>
        [Label("板号")]
        public static readonly Property<int?> BoardNoProperty = P<ProductDefectTraceViewModel>.Register(e => e.BoardNo);

        /// <summary>
        /// 板号
        /// </summary>
        public int? BoardNo
        {
            get { return this.GetProperty(BoardNoProperty); }
            set { this.SetProperty(BoardNoProperty, value); }
        }
        #endregion

        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> SnProperty = P<ProductDefectTraceViewModel>.Register(e => e.Sn);

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get { return GetProperty(SnProperty); }
            set { SetProperty(SnProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<ProductDefectTraceViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 缺陷位置 Location
        /// <summary>
        /// 缺陷位置
        /// </summary>
        [Label("缺陷位置")]
        public static readonly Property<string> LocationProperty = P<ProductDefectTraceViewModel>.Register(e => e.Location);

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string Location
        {
            get { return GetProperty(LocationProperty); }
            set { SetProperty(LocationProperty, value); }
        }
        #endregion

        #region 维修时间 FixedDate
        /// <summary>
        /// 维修时间
        /// </summary>
        [Label("维修时间")]
        public static readonly Property<DateTime?> FixedDateProperty = P<ProductDefectTraceViewModel>.Register(e => e.FixedDate);

        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime? FixedDate
        {
            get { return GetProperty(FixedDateProperty); }
            set { SetProperty(FixedDateProperty, value); }
        }
        #endregion

        #region 是否误判 IsMisjudgment
        /// <summary>
        /// 是否误判
        /// </summary>
        [Label("是否误判")]
        public static readonly Property<bool> IsMisjudgmentProperty = P<ProductDefectTraceViewModel>.Register(e => e.IsMisjudgment);

        /// <summary>
        /// 是否误判
        /// </summary>
        public bool IsMisjudgment
        {
            get { return this.GetProperty(IsMisjudgmentProperty); }
            set { this.SetProperty(IsMisjudgmentProperty, value); }
        }
        #endregion

        #region 维修人 FixedBy

        /// <summary>
        /// 维修人
        /// </summary>
        [Label("维修人")]
        public static readonly Property<string> FixedByProperty = P<ProductDefectTraceViewModel>.Register(e => e.FixedBy);

        /// <summary>
        /// 维修人
        /// </summary>
        public string FixedBy
        {
            get { return GetProperty(FixedByProperty); }
            set { SetProperty(FixedByProperty, value); }
        }
        #endregion

    }
}