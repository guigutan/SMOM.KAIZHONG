using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Items
{
    /// <summary>
    /// 辅助单位基类
    /// </summary>
    [Serializable]
    public class SecondUnitViewModel : ViewModel
    {
        #region 辅助单位 SecondUnit
        /// <summary>
        /// 辅助单位Id
        /// </summary>
        [Label("辅助单位")]
        public static readonly IRefIdProperty SecondUnitIdProperty =
            P<SecondUnitViewModel>.RegisterRefId(e => e.SecondUnitId, ReferenceType.Normal);

        /// <summary>
        /// 辅助单位Id
        /// </summary>
        public double SecondUnitId
        {
            get { return (double)this.GetRefId(SecondUnitIdProperty); }
            set { this.SetRefId(SecondUnitIdProperty, value); }
        }

        /// <summary>
        /// 辅助单位    
        /// </summary>
        public static readonly RefEntityProperty<Unit> SecondUnitProperty =
            P<SecondUnitViewModel>.RegisterRef(e => e.SecondUnit, SecondUnitIdProperty);

        /// <summary>
        /// 辅助单位
        /// </summary>
        public Unit SecondUnit
        {
            get { return this.GetRefEntity(SecondUnitProperty); }
            set { this.SetRefEntity(SecondUnitProperty, value); }
        }
        #endregion

        #region 分子 Numerator
        /// <summary>
        /// 分子
        /// </summary>
        [Label("分子")]
        public static readonly Property<decimal> NumeratorProperty = P<SecondUnitViewModel>.Register(e => e.Numerator);

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
        public static readonly Property<decimal> DenominatorProperty = P<SecondUnitViewModel>.Register(e => e.Denominator);

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
        public static readonly Property<decimal> ConvertFigreProperty = P<SecondUnitViewModel>.Register(e => e.ConvertFigre);

        /// <summary>
        /// 转换率
        /// </summary>
        public decimal ConvertFigre
        {
            get { return GetProperty(ConvertFigreProperty); }
            set { SetProperty(ConvertFigreProperty, value); }
        }
        #endregion

        #region 辅助单位名称 SecondUnitName
        /// <summary>
        /// 辅助单位名称
        /// </summary>
        [Label("辅助单位名称")]
        public static readonly Property<string> SecondUnitNameProperty = P<SecondUnitViewModel>.Register(e => e.SecondUnitName);

        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string SecondUnitName
        {
            get { return this.GetProperty(SecondUnitNameProperty); }
            set { this.SetProperty(SecondUnitNameProperty, value); }
        }
        #endregion

        #region 辅助单位精度 SecondPrecision
        /// <summary>
        /// 辅助单位精度
        /// </summary>
        [Label("属性名")]
        public static readonly Property<int> SecondPrecisionProperty = P<SecondUnitViewModel>.Register(e => e.SecondPrecision);

        /// <summary>
        /// 辅助单位精度
        /// </summary>
        public int SecondPrecision
        {
            get { return this.GetProperty(SecondPrecisionProperty); }
            set { this.SetProperty(SecondPrecisionProperty, value); }
        }
        #endregion

        #region 主单位精度 MainPrecision
        /// <summary>
        /// 主单位精度
        /// </summary>
        [Label("主单位精度")]
        public static readonly Property<int> MainPrecisionProperty = P<SecondUnitViewModel>.Register(e => e.MainPrecision);

        /// <summary>
        /// 主单位精度
        /// </summary>
        public int MainPrecision
        {
            get { return this.GetProperty(MainPrecisionProperty); }
            set { this.SetProperty(MainPrecisionProperty, value); }
        }
        #endregion

        #region 主单位取舍 MainTrade
        /// <summary>
        /// 主单位取舍
        /// </summary>
        [Label("主单位取舍")]
        public static readonly Property<TradeType> MainTradeProperty = P<SecondUnitViewModel>.Register(e => e.MainTrade);

        /// <summary>
        /// 主单位取舍
        /// </summary>
        public TradeType MainTrade
        {
            get { return this.GetProperty(MainTradeProperty); }
            set { this.SetProperty(MainTradeProperty, value); }
        }
        #endregion
         
        #region 辅助单位取舍 SecondTrade
        /// <summary>
        /// 辅助单位取舍
        /// </summary>
        [Label("辅助单位取舍")]
        public static readonly Property<TradeType> SecondTradeProperty = P<SecondUnitViewModel>.RegisterView(e => e.SecondTrade, p => p.SecondUnit.TradeType);

        /// <summary>
        /// 辅助单位取舍
        /// </summary>
        public TradeType SecondTrade
        {
            get { return this.GetProperty(SecondTradeProperty); }
            set { this.SetProperty(SecondTradeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 辅助单位基类
    /// </summary>
    [Serializable]
    public class SecondUnitData 
    {         
        /// <summary>
        /// 辅助单位Id
        /// </summary>
        public double SecondUnitId
        {
            get;
            set;
        }

        /// <summary>
        /// 分子
        /// </summary>
        public decimal Numerator
        {
            get;
            set;
        }

        /// <summary>
        /// 分母
        /// </summary>
        public decimal Denominator
        {
            get;
            set;
        }

        /// <summary>
        /// 转换率
        /// </summary>
        public decimal ConvertFigre
        {
            get;
            set;
        }

        /// <summary>
        /// 辅助单位名称
        /// </summary>
        public string SecondUnitName
        {
            get;
            set;
        }

        /// <summary>
        /// 辅助单位精度
        /// </summary>
        public int SecondPrecision
        {
            get;
            set;
        }

        /// <summary>
        /// 主单位精度
        /// </summary>
        public int MainPrecision
        {
            get;
            set;
        }
    }
}
