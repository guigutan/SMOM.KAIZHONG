using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 标签打印选择模板基类
    /// </summary>
    [RootEntity, Serializable]
    [Label("垛表打印模板")]
    public class PrintPileViewModel : ViewModel
    {
        #region 打印模板 LabelTemplate
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty LabelTemplateIdProperty =
            P<PrintPileViewModel>.RegisterRefId(e => e.LabelTemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? LabelTemplateId
        {
            get { return (double?)this.GetRefNullableId(LabelTemplateIdProperty); }
            set { this.SetRefNullableId(LabelTemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> LabelTemplateProperty =
            P<PrintPileViewModel>.RegisterRef(e => e.LabelTemplate, LabelTemplateIdProperty);

        /// <summary>
        /// 条码模板
        /// </summary>
        public PrintTemplate LabelTemplate
        {
            get { return this.GetRefEntity(LabelTemplateProperty); }
            set { this.SetRefEntity(LabelTemplateProperty, value); }
        }
        #endregion
    }
}
