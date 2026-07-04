using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Equipments.Accounts.ViewModels
{

    /// <summary>
    /// 条码打印
    /// </summary> 
    [Serializable, RootEntity]
    [Label("条码打印")]
    public class QRCodePrintCfgViewModel : ViewModel
    {

        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<QRCodePrintCfgViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
            P<QRCodePrintCfgViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

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
        public static readonly Property<double> TemplateNameIdProperty = P<QRCodePrintCfgViewModel>.Register(e => e.TemplateNameId);

        /// <summary>
        /// 打印模板ID
        /// </summary>
        public double TemplateNameId
        {
            get { return this.GetProperty(TemplateNameIdProperty); }
            set { this.SetProperty(TemplateNameIdProperty, value); }
        }
        #endregion

        #region 补打份数
        /// <summary>
        /// 补打份数
        /// </summary>
        [Label("补打份数")]
        [MinValue(1)]
        public static readonly Property<int> TimesProperty = P<QRCodePrintCfgViewModel>.Register(e => e.Times);

        /// <summary>
        /// 补打份数
        /// </summary>
        public int Times
        {
            get { return this.GetProperty(TimesProperty); }
            set { this.SetProperty(TimesProperty, value); }
        }
        #endregion
    }
    /// <summary>
    /// Lot打印返回结果
    /// </summary>
    [Serializable]
    public class QRCodePrintResult
    {
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrMsg { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Url { get; set; }
    }
}
