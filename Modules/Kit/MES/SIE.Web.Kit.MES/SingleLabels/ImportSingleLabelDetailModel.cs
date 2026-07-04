using SIE.Domain;
using SIE.ObjectModel;
using SIE.Kit.MES.SingleLabels;

namespace SIE.Web.Kit.Mes.SingleLabels
{
    /// <summary>
    /// 单体条码导入失败明细
    /// </summary>
    [RootEntity]
    [Label("导入失败数据")]
    public class ImportSingleLabelDetailModel : ViewModel
    {
        #region ErrorMessage 导入失败原因

        /// <summary>
        /// 导入失败原因
        /// </summary>
        [Label("导入失败原因")]
        public static readonly Property<string> ErrorMessageProperty = P<ImportSingleLabelDetailModel>.Register(e => e.ErrorMessage);

        /// <summary>
        /// 导入失败原因
        /// </summary>
        public string ErrorMessage
        {
            get { return this.GetProperty(ErrorMessageProperty); }
            set { this.SetProperty(ErrorMessageProperty, value); }
        }

        #endregion

        #region 条码 Sn
        /// <summary>
        /// 条码
        /// </summary>
        [Label("条码")]
        public static readonly Property<string> SnProperty = P<ImportSingleLabelDetailModel>.Register(e => e.Sn);

        /// <summary>
        /// 条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 批次标签 BatchCode
        /// <summary>
        /// 批次标签
        /// </summary>
        [Label("批次标签")]
        public static readonly Property<string> BatchCodeProperty = P<ImportSingleLabelDetailModel>.Register(e => e.BatchCode);

        /// <summary>
        /// 批次标签
        /// </summary>
        public string BatchCode
        {
            get { return this.GetProperty(BatchCodeProperty); }
            set { this.SetProperty(BatchCodeProperty, value); }
        }
        #endregion

        #region 打印日期 PrintDate
        /// <summary>
        /// 打印日期
        /// </summary>
        [Label("打印日期")]
        public static readonly Property<string> PrintDateProperty = P<ImportSingleLabelDetailModel>.Register(e => e.PrintDate);

        /// <summary>
        /// 打印日期
        /// </summary>
        public string PrintDate
        {
            get { return this.GetProperty(PrintDateProperty); }
            set { this.SetProperty(PrintDateProperty, value); }
        }
        #endregion

        #region 来源Id SourceId
        /// <summary>
        /// 来源Id
        /// </summary>
        [Label("来源Id")]
        public static readonly Property<string> SourceIdProperty = P<ImportSingleLabelDetailModel>.Register(e => e.SourceId);

        /// <summary>
        /// 来源Id
        /// </summary>
        public string SourceId
        {
            get { return this.GetProperty(SourceIdProperty); }
            set { this.SetProperty(SourceIdProperty, value); }
        }
        #endregion

        #region 来源号 SourceNo
        /// <summary>
        /// 来源号
        /// </summary>
        [Label("来源号")]
        public static readonly Property<string> SourceNoProperty = P<ImportSingleLabelDetailModel>.Register(e => e.SourceNo);

        /// <summary>
        /// 来源号
        /// </summary>
        public string SourceNo
        {
            get { return this.GetProperty(SourceNoProperty); }
            set { this.SetProperty(SourceNoProperty, value); }
        }
        #endregion

        #region 员工编码 Employee
        /// <summary>
        /// 员工编码
        /// </summary>
        [Label("员工编码")]
        public static readonly Property<string> EmployeeProperty = P<ImportSingleLabelDetailModel>.Register(e => e.Employee);

        /// <summary>
        /// 员工编码
        /// </summary>
        public string Employee
        {
            get { return this.GetProperty(EmployeeProperty); }
            set { this.SetProperty(EmployeeProperty, value); }
        }
        #endregion

        #region 条码来源类型 SourceType
        /// <summary>
        /// 条码来源类型
        /// </summary>
        [Label("条码来源类型")]
        public static readonly Property<string> SourceTypeProperty = P<ImportSingleLabelDetailModel>.Register(e => e.SourceType);

        /// <summary>
        /// 条码来源类型
        /// </summary>
        public string SourceType
        {
            get { return this.GetProperty(SourceTypeProperty); }
            set { this.SetProperty(SourceTypeProperty, value); }
        }
        #endregion

        #region 物料编码 Item
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemProperty = P<ImportSingleLabelDetailModel>.Register(e => e.Item);

        /// <summary>
        /// 物料
        /// </summary>
        public string Item
        {
            get { return this.GetProperty(ItemProperty); }
            set { this.SetProperty(ItemProperty, value); }
        }
        #endregion

        #region 标签状态 LabelState
        /// <summary>
        /// 标签状态
        /// </summary>
        [Label("标签状态")]
        public static readonly Property<string> LabelStateProperty = P<ImportSingleLabelDetailModel>.Register(e => e.LabelState);

        /// <summary>
        /// 标签状态
        /// </summary>
        public string LabelState
        {
            get { return this.GetProperty(LabelStateProperty); }
            set { this.SetProperty(LabelStateProperty, value); }
        }
        #endregion

        #region 供应商编码 Supplier
        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商编码")]
        public static readonly Property<string> SupplierProperty = P<ImportSingleLabelDetailModel>.Register(e => e.Supplier);

        /// <summary>
        /// 供应商
        /// </summary>
        public string Supplier
        {
            get { return this.GetProperty(SupplierProperty); }
            set { this.SetProperty(SupplierProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 产品分类检验标准导入失败明细视图配置
    /// </summary>
    internal class ImportSingleLabelDetailViewConfig : WPFViewConfig<ImportSingleLabelDetailModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(SingleLabel));
        }

        /// <summary>
        /// 配置列表试图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.ErrorMessage);
            View.Property(p => p.Sn);
            View.Property(p => p.BatchCode);
            View.Property(p => p.PrintDate);
            View.Property(p => p.SourceId);
            View.Property(p => p.SourceNo);
            View.Property(p => p.Employee);
            View.Property(p => p.SourceType);
            View.Property(p => p.Item);
            View.Property(p => p.LabelState);
            View.Property(p => p.Supplier);
        }
    }
}