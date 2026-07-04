using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Dock.ViewModels
{
    /// <summary>
    /// 月台预约打印
    /// </summary>
    [Serializable, RootEntity]
    [Label("月台预约打印")]
    public class DockAppointPrintViewModel : ViewModel
    {
        #region 打印模板 PrintTemplate
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty PrintTemplateIdProperty =
            P<DockAppointPrintViewModel>.RegisterRefId(e => e.PrintTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? PrintTemplateId
        {
            get { return (double?)this.GetRefNullableId(PrintTemplateIdProperty); }
            set { this.SetRefNullableId(PrintTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> PrintTemplateProperty =
            P<DockAppointPrintViewModel>.RegisterRef(e => e.PrintTemplate, PrintTemplateIdProperty);

        /// <summary>
        /// 打印模板
        /// </summary>
        public PrintTemplate PrintTemplate
        {
            get { return this.GetRefEntity(PrintTemplateProperty); }
            set { this.SetRefEntity(PrintTemplateProperty, value); }
        }
        #endregion
    }
}
