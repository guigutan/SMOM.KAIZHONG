using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 物料基类，辅助单位
    /// </summary>
    [ChildEntity, Serializable]
    [Label("物料基类，辅助单位")]
    public partial class SecondUnitBase : DataEntity
    {
        #region 分子 Numerator
        /// <summary>
        /// 分子
        /// </summary>
        [Label("分子")]
        public static readonly Property<decimal> NumeratorProperty = P<SecondUnitBase>.Register(e => e.Numerator);

        /// <summary>
        /// 分子
        /// </summary>
        public decimal Numerator
        {
            get { return GetProperty(NumeratorProperty); }
            set { SetProperty(NumeratorProperty, value); }
        }
        #endregion

        #region 分母 Denominator
        /// <summary>
        /// 分母
        /// </summary>
        [Label("分母")]
        public static readonly Property<decimal> DenominatorProperty = P<SecondUnitBase>.Register(e => e.Denominator);

        /// <summary>
        /// 分母
        /// </summary>
        public decimal Denominator
        {
            get { return GetProperty(DenominatorProperty); }
            set { SetProperty(DenominatorProperty, value); }
        }
        #endregion

        #region 转换率 ConvertFigre
        /// <summary>
        /// 转换率
        /// </summary>
        [Label("转换率")]
        public static readonly Property<decimal> ConvertFigreProperty = P<SecondUnitBase>.Register(e => e.ConvertFigre);

        /// <summary>
        /// 转换率
        /// </summary>        
        public decimal ConvertFigre
        {
            get { return GetProperty(ConvertFigreProperty); }
            set { SetProperty(ConvertFigreProperty, value); }
        }
        #endregion

        #region 辅助单位 SecondUnit
        /// <summary>
        /// 辅助单位Id
        /// </summary>
        [Label("辅助单位")]
        public static readonly IRefIdProperty SecondUnitIdProperty =
            P<SecondUnitBase>.RegisterRefId(e => e.SecondUnitId, ReferenceType.Normal);

        /// <summary>
        /// 辅助单位Id
        /// </summary>
        public double? SecondUnitId
        {
            get { return (double?)this.GetRefNullableId(SecondUnitIdProperty); }
            set { this.SetRefNullableId(SecondUnitIdProperty, value); }
        }

        /// <summary>
        /// 辅助单位
        /// </summary>
        public static readonly RefEntityProperty<Unit> SecondUnitProperty =
            P<SecondUnitBase>.RegisterRef(e => e.SecondUnit, SecondUnitIdProperty);

        /// <summary>
        /// 辅助单位
        /// </summary>
        public Unit SecondUnit
        {
            get { return this.GetRefEntity(SecondUnitProperty); }
            set { this.SetRefEntity(SecondUnitProperty, value); }
        }
        #endregion

        #region 辅助单位名称 SecondUnitName
        /// <summary>
        /// 辅助单位名称
        /// </summary>
        [Label("辅助单位")]
        public static readonly Property<string> SecondUnitNameProperty = P<SecondUnitBase>.RegisterView(e => e.SecondUnitName, p => p.SecondUnit.Name);

        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string SecondUnitName
        {
            get { return this.GetProperty(SecondUnitNameProperty); }
            set { SetProperty(SecondUnitNameProperty, value); }
        }
        #endregion

        #region 辅助单位精度 SecondPrecision
        /// <summary>
        /// 辅助单位精度
        /// </summary>
        [Label("辅助单位精度")]
        public static readonly Property<int?> SecondPrecisionProperty = P<SecondUnitBase>.RegisterView(e => e.SecondPrecision, p => p.SecondUnit.Precision);

        /// <summary>
        /// 辅助单位精度
        /// </summary>
        public int? SecondPrecision
        {
            get { return this.GetProperty(SecondPrecisionProperty); }
            set { SetProperty(SecondPrecisionProperty, value); }
        }
        #endregion

        #region 辅助单位取舍 SecondTrade
        /// <summary>
        /// 辅助单位取舍
        /// </summary>
        [Label("辅助单位取舍")]
        public static readonly Property<TradeType> SecondTradeProperty = P<SecondUnitBase>.RegisterView(e => e.SecondTrade, p => p.SecondUnit.TradeType);

        /// <summary>
        /// 辅助单位取舍
        /// </summary>
        public TradeType SecondTrade
        {
            get { return this.GetProperty(SecondTradeProperty); }
            set { SetProperty(SecondTradeProperty, value); }
        }
        #endregion

    }
}
