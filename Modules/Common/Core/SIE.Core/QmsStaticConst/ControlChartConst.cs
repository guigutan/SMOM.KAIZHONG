using SIE;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.QmsStaticConst
{
    /// <summary>
    /// 控制图参数
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("控制图参数")]
    public partial class ControlChartConst :StaticConstValueBase

    {
        #region 样本数量 SampleQty
        /// <summary>
        /// 样本数量
        /// </summary>
        [Label("n")]
        [Required]
        public static readonly Property<int> SampleQtyProperty = P<ControlChartConst>.Register(e => e.SampleQty);

        /// <summary>
        /// 样本数量
        /// </summary>
        public int SampleQty
        {
            get { return GetProperty(SampleQtyProperty); }
            set { SetProperty(SampleQtyProperty, value); }
        }
        #endregion

        #region A A
        /// <summary>
        /// A
        /// </summary>
        [Label("A")]
        public static readonly Property<double?> AProperty = P<ControlChartConst>.Register(e => e.A);

        /// <summary>
        /// A
        /// </summary>
        public double? A
        {
            get { return GetProperty(AProperty); }
            set { SetProperty(AProperty, value); }
        }
        #endregion

        #region A2 A2
        /// <summary>
        /// A2
        /// </summary>
        [Label("A₂")]
        public static readonly Property<double?> A2Property = P<ControlChartConst>.Register(e => e.A2);

        /// <summary>
        /// A2
        /// </summary>
        public double? A2
        {
            get { return GetProperty(A2Property); }
            set { SetProperty(A2Property, value); }
        }
        #endregion

        #region A3 A3
        /// <summary>
        /// A3
        /// </summary>
        [Label("A₃")]
        public static readonly Property<double?> A3Property = P<ControlChartConst>.Register(e => e.A3);

        /// <summary>
        /// A3
        /// </summary>
        public double? A3
        {
            get { return GetProperty(A3Property); }
            set { SetProperty(A3Property, value); }
        }
        #endregion

        #region B3 B3
        /// <summary>
        /// B3
        /// </summary>
        [Label("B₃")]
        public static readonly Property<double?> B3Property = P<ControlChartConst>.Register(e => e.B3);

        /// <summary>
        /// B3
        /// </summary>
        public double? B3
        {
            get { return GetProperty(B3Property); }
            set { SetProperty(B3Property, value); }
        }
        #endregion

        #region B4 B4
        /// <summary>
        /// B4
        /// </summary>
        [Label("B₄")]
        public static readonly Property<double?> B4Property = P<ControlChartConst>.Register(e => e.B4);

        /// <summary>
        /// B4
        /// </summary>
        public double? B4
        {
            get { return GetProperty(B4Property); }
            set { SetProperty(B4Property, value); }
        }
        #endregion

        #region B5 B5
        /// <summary>
        /// B5
        /// </summary>
        [Label("B₅")]
        public static readonly Property<double?> B5Property = P<ControlChartConst>.Register(e => e.B5);

        /// <summary>
        /// B5
        /// </summary>
        public double? B5
        {
            get { return GetProperty(B5Property); }
            set { SetProperty(B5Property, value); }
        }
        #endregion

        #region B6 B6
        /// <summary>
        /// B6
        /// </summary>
        [Label("B₆")]
        public static readonly Property<double?> B6Property = P<ControlChartConst>.Register(e => e.B6);

        /// <summary>
        /// B6
        /// </summary>
        public double? B6
        {
            get { return GetProperty(B6Property); }
            set { SetProperty(B6Property, value); }
        }
        #endregion

        #region C4 C4
        /// <summary>
        /// C4
        /// </summary>
        [Label("C₄")]
        public static readonly Property<double?> C4Property = P<ControlChartConst>.Register(e => e.C4);

        /// <summary>
        /// C4
        /// </summary>
        public double? C4
        {
            get { return GetProperty(C4Property); }
            set { SetProperty(C4Property, value); }
        }
        #endregion       

        #region D1 D1
        /// <summary>
        /// D1
        /// </summary>
        [Label("D₁")]
        public static readonly Property<double?> D1Property = P<ControlChartConst>.Register(e => e.D1);

        /// <summary>
        /// D1
        /// </summary>
        public double? D1
        {
            get { return GetProperty(D1Property); }
            set { SetProperty(D1Property, value); }
        }
        #endregion

        #region D2 D2
        /// <summary>
        /// D2
        /// </summary>
        [Label("D₂")]
        public static readonly Property<double?> D2Property = P<ControlChartConst>.Register(e => e.D2);

        /// <summary>
        /// D2
        /// </summary>
        public double? D2
        {
            get { return GetProperty(D2Property); }
            set { SetProperty(D2Property, value); }
        }
        #endregion

        #region D3 D3
        /// <summary>
        /// D3
        /// </summary>
        [Label("D₃")]
        public static readonly Property<double?> D3Property = P<ControlChartConst>.Register(e => e.D3);

        /// <summary>
        /// D3
        /// </summary>
        public double? D3
        {
            get { return GetProperty(D3Property); }
            set { SetProperty(D3Property, value); }
        }
        #endregion

        #region D4 D4
        /// <summary>
        /// D4
        /// </summary>
        [Label("D₄")]
        public static readonly Property<double?> D4Property = P<ControlChartConst>.Register(e => e.D4);

        /// <summary>
        /// D4
        /// </summary>
        public double? D4
        {
            get { return GetProperty(D4Property); }
            set { SetProperty(D4Property, value); }
        }
        #endregion

        #region D2Nd D2Nd
        /// <summary>
        /// D2Nd
        /// </summary>
        [Label("d₂")]
        public static readonly Property<double?> D2NdProperty = P<ControlChartConst>.Register(e => e.D2Nd);

        /// <summary>
        /// D2Nd
        /// </summary>
        public double? D2Nd
        {
            get { return GetProperty(D2NdProperty); }
            set { SetProperty(D2NdProperty, value); }
        }
        #endregion

        #region D3Nd D3Nd
        /// <summary>
        /// D3Nd
        /// </summary>
        [Label("d₃")]
        public static readonly Property<double?> D3NdProperty = P<ControlChartConst>.Register(e => e.D3Nd);

        /// <summary>
        /// D3Nd
        /// </summary>
        public double? D3Nd
        {
            get { return GetProperty(D3NdProperty); }
            set { SetProperty(D3NdProperty, value); }
        }
        #endregion

        #region D4Nd D4Nd
        /// <summary>
        /// D4Nd
        /// </summary>
        [Label("d₄")]
        public static readonly Property<double?> D4NdProperty = P<ControlChartConst>.Register(e => e.D4Nd);

        /// <summary>
        /// D4
        /// </summary>
        public double? D4Nd
        {
            get { return GetProperty(D4NdProperty); }
            set { SetProperty(D4NdProperty, value); }
        }
        #endregion

        #region E2 E2
        /// <summary>
        /// E2
        /// </summary>
        [Label("E₂")]
        public static readonly Property<double?> E2Property = P<ControlChartConst>.Register(e => e.E2);

        /// <summary>
        /// E2
        /// </summary>
        public double? E2
        {
            get { return GetProperty(E2Property); }
            set { SetProperty(E2Property, value); }
        }
        #endregion

        #region MeA2 MeA2
        /// <summary>
        /// MEA2
        /// </summary>
        [Label("MeA₂")]
        public static readonly Property<double?> MeA2Property = P<ControlChartConst>.Register(e => e.MeA2);

        /// <summary>
        /// MEA2
        /// </summary>
        public double? MeA2
        {
            get { return GetProperty(MeA2Property); }
            set { SetProperty(MeA2Property, value); }
        }
        #endregion

        #region 控制图参数集合 MsaConst
        /// <summary>
        /// 控制图参数集合Id
        /// </summary>
        public static readonly IRefIdProperty MsaConstIdProperty = P<ControlChartConst>.RegisterRefId(e => e.MsaConstId, ReferenceType.Parent);

        /// <summary>
        /// 控制图参数集合Id
        /// </summary>
        public double MsaConstId
        {
            get { return (double)GetRefId(MsaConstIdProperty); }
            set { SetRefId(MsaConstIdProperty, value); }
        }

        /// <summary>
        /// 控制图参数集合
        /// </summary>
        public static readonly RefEntityProperty<StaticConst> MsaConstProperty = P<ControlChartConst>.RegisterRef(e => e.MsaConst, MsaConstIdProperty);

        /// <summary>
        /// 控制图参数集合
        /// </summary>
        public StaticConst MsaConst
        {
            get { return GetRefEntity(MsaConstProperty); }
            set { SetRefEntity(MsaConstProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 控制图参数 实体配置
    /// </summary>
    internal class ControlChartConstConfig : EntityConfig<ControlChartConst>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MSA_CONST_CTRL_CHT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}