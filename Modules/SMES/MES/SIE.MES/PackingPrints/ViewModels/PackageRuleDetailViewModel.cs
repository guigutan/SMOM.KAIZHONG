using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.PackingPrints.ViewModels
{
    /// <summary>
    /// 工单包装规则视图
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(PackageUnitName))]
    [Label("工单包装规则视图")]
    public class PackageRuleDetailViewModel : ViewModel
    {
        #region 包装单位名称 PackageUnitName
        /// <summary>
        /// 包装单位名称
        /// </summary>
        [Label("包装单位名称")]
        public static readonly Property<string> PackageUnitNameProperty = P<PackageRuleDetailViewModel>.Register(e => e.PackageUnitName);

        /// <summary>
        /// 包装单位名称
        /// </summary>
        public string PackageUnitName
        {
            get { return GetProperty(PackageUnitNameProperty); }
            set { SetProperty(PackageUnitNameProperty, value); }
        }
        #endregion

        #region 产品数 Qty
        /// <summary>
        /// 产品数
        /// </summary>
        [MinValue(0)]
        [Label("产品数")]
        public static readonly Property<decimal> QtyProperty = P<PackageRuleDetailViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 产品数
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 包装数 LevelQty
        /// <summary>
        /// 包装数
        /// </summary>
        [Label("包装数")]
        public static readonly Property<decimal> LevelQtyProperty = P<PackageRuleDetailViewModel>.Register(e => e.LevelQty);

        /// <summary>
        /// 包装数
        /// </summary>
        public decimal LevelQty
        {
            get { return this.GetProperty(LevelQtyProperty); }
            set { this.SetProperty(LevelQtyProperty, value); }
        }
        #endregion

        #region 编码规则 NumberRuleName
        /// <summary>
        /// 编码规则
        /// </summary>
        [Label("编码规则")]
        public static readonly Property<string> NumberRuleNameProperty = P<PackageRuleDetailViewModel>.Register(e => e.NumberRuleName);

        /// <summary>
        /// 编码规则
        /// </summary>
        public string NumberRuleName
        {
            get { return GetProperty(NumberRuleNameProperty); }
            set { SetProperty(NumberRuleNameProperty, value); }
        }
        #endregion

        #region 是否打印 IsPrint
        /// <summary>
        /// 是否打印
        /// </summary>
        [Label("是否打印")]
        public static readonly Property<bool> IsPrintProperty = P<PackageRuleDetailViewModel>.Register(e => e.IsPrint);

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool IsPrint
        {
            get { return this.GetProperty(IsPrintProperty); }
            set { this.SetProperty(IsPrintProperty, value); }
        }
        #endregion

        #region 打印模板 TemplateName
        /// <summary>
        /// 打印模板
        /// </summary>
        [Label("打印模板")]
        public static readonly Property<string> TemplateNameProperty = P<PackageRuleDetailViewModel>.Register(e => e.TemplateName);

        /// <summary>
        /// 打印模板
        /// </summary>
        public string TemplateName
        {
            get { return GetProperty(TemplateNameProperty); }
            set { SetProperty(TemplateNameProperty, value); }
        }
        #endregion
    }
}
