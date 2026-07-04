using SIE.Barcodes;
using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.Barcodes.Barcodes.ViewModels
{
    /// <summary>
    /// 条码补打 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("条码补打")]
    public class ReprintInfo : ViewModel
    {
        #region 原因 Reason
        /// <summary>
        /// 原因
        /// </summary>
        [Label("原因")]
        [Required]
        public static readonly Property<string> ReasonProperty = P<ReprintInfo>.Register(e => e.Reason);

        /// <summary>
        /// 原因
        /// </summary>
        public string Reason
        {
            get { return this.GetProperty(ReasonProperty); }
            set { this.SetProperty(ReasonProperty, value); }
        }
        #endregion

        #region 补打份数
        /// <summary>
        /// 补打份数
        /// </summary>
        [Label("补打份数")]
        [MinValue(1)]
        public static readonly Property<int> TimesProperty = P<ReprintInfo>.Register(e => e.Times);

        /// <summary>
        /// 补打份数
        /// </summary>
        public int Times
        {
            get { return this.GetProperty(TimesProperty); }
            set { this.SetProperty(TimesProperty, value); }
        }
        #endregion

        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<ReprintInfo>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double TemplateId
        {
            get { return (double)this.GetRefId(TemplateIdProperty); }
            set { this.SetRefId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty =
            P<ReprintInfo>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 打印模板ID
        /// <summary>
        /// 打印模板ID
        /// </summary>
        [Label("打印模板ID")]
        public static readonly Property<double> TemplateNameIdProperty = P<ReprintInfo>.Register(e => e.TemplateNameId);

        /// <summary>
        /// 打印模板ID
        /// </summary>
        public double TemplateNameId
        {
            get { return this.GetProperty(TemplateNameIdProperty); }
            set { this.SetProperty(TemplateNameIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 条码补打 视图
    /// </summary>
    internal class ReprintInfoViewConfig : WebViewConfig<ReprintInfo>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(BarcodeReprint));
            View.ClearCommands();
            View.HasDetailColumnsCount(2);
            View.Property(p => p.Times).UseSpinEditor(p => p.MinValue = 1);
            View.Property(p => p.Template).UseDataSource((e, p, r) =>
            {
                return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(typeof(SIE.Barcodes.Printables.BarcodePrintable).GetQualifiedName(), p, r);
            });
            View.Property(p => p.Reason).UseMemoEditor().ShowInDetail(rowSpan: 3, columnSpan: 2);
        }
    }
}
