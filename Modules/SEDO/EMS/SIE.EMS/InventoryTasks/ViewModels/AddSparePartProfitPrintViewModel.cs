using SIE.Common.Prints;
using SIE.Domain;
using SIE.EMS.SpareParts.Enums;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.InventoryTasks.ViewModels
{
    /// <summary>
    /// 序列号打印 ViewModel
    /// </summary> 
    [Serializable, RootEntity]
    [Label("序列号打印")]
    public class AddSparePartProfitPrintViewModel : ViewModel
    {
        #region 备件管控方式 ControlMethod
        /// <summary>
        /// 备件管控方式
        /// </summary>
        [Label("备件管控方式")]
        public static readonly Property<ControlMethod> ControlMethodProperty = P<AddSparePartProfitPrintViewModel>.Register(e => e.ControlMethod);

        /// <summary>
        /// 备件管控方式
        /// </summary>
        public ControlMethod ControlMethod
        {
            get { return this.GetProperty(ControlMethodProperty); }
            set { this.SetProperty(ControlMethodProperty, value); }
        }
        #endregion

        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<AddSparePartProfitPrintViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

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
            P<AddSparePartProfitPrintViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

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
