using SIE.Common.Prints;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Packages.Boxs;
using System;

namespace SIE.Inventory.Piles
{
    /// <summary>
    /// 批量生成ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("批量生成")]
    public class BatchGeneratePileViewModel : ViewModel
    {
        #region 周转箱型号 TurnoverBoxModel
        /// <summary>
        /// 周转箱型号Id
        /// </summary>
        [Label("型号")]
        public static readonly IRefIdProperty TurnoverBoxModelIdProperty = P<BatchGeneratePileViewModel>.RegisterRefId(e => e.TurnoverBoxModelId, ReferenceType.Normal);

        /// <summary>
        /// 周转箱型号Id
        /// </summary>
        public double TurnoverBoxModelId
        {
            get { return (double)GetRefId(TurnoverBoxModelIdProperty); }
            set { SetRefId(TurnoverBoxModelIdProperty, value); }
        }

        /// <summary>
        /// 周转箱型号
        /// </summary>
        public static readonly RefEntityProperty<TurnoverBoxModel> TurnoverBoxModelProperty = P<BatchGeneratePileViewModel>.RegisterRef(e => e.TurnoverBoxModel, TurnoverBoxModelIdProperty);

        /// <summary>
        /// 周转箱型号
        /// </summary>
        public TurnoverBoxModel TurnoverBoxModel
        {
            get { return GetRefEntity(TurnoverBoxModelProperty); }
            set { SetRefEntity(TurnoverBoxModelProperty, value); }
        }
        #endregion

        #region 数量 GenerateQty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<int> GenerateQtyProperty = P<BatchGeneratePileViewModel>.Register(e => e.GenerateQty);

        /// <summary>
        /// 数量
        /// </summary>
        public int GenerateQty
        {
            get { return GetProperty(GenerateQtyProperty); }
            set { SetProperty(GenerateQtyProperty, value); }
        }
        #endregion

        #region 打印模板 Template
        /// <summary>
        /// 打印模板Id
        /// </summary>
        [Label("打印模板")]
        [Required]
        public static readonly IRefIdProperty TemplateIdProperty =
            P<BatchGeneratePileViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 打印模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double?)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty =
            P<BatchGeneratePileViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

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