using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 序列号打印 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("序列号打印")]
    public class ReceiveSnPrintViewModel : ViewModel
    {
        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<ReceiveSnPrintViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
            P<ReceiveSnPrintViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion
    }
}
