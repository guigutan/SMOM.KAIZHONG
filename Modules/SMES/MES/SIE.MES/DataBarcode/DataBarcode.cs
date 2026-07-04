using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DataBarcode
{
    /// <summary>
    /// 
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("数据条码化")]
    public partial class DataBarcode : DataEntity
    {
        #region 条码化类型 BarcodeType
        /// <summary>
        /// 条码化类型
        /// </summary>
        [Label("条码化类型")]
        public static readonly Property<string> BarcodeTypeProperty = P<DataBarcode>.Register(e => e.BarcodeType);

        /// <summary>
        /// 条码化类型
        /// </summary>
        public string BarcodeType
        {
            get { return GetProperty(BarcodeTypeProperty); }
            set { SetProperty(BarcodeTypeProperty, value); }
        }
        #endregion

        #region 条码化工厂 BarcodeSite
        /// <summary>
        /// 条码化工厂
        /// </summary>
        [Label("条码化工厂")]
        public static readonly Property<string> BarcodeSiteProperty = P<DataBarcode>.Register(e => e.BarcodeSite);

        /// <summary>
        /// 条码化工厂
        /// </summary>
        public string BarcodeSite
        {
            get { return GetProperty(BarcodeSiteProperty); }
            set { SetProperty(BarcodeSiteProperty, value); }
        }
        #endregion

        #region 条码化参数1 BarcodeParam1bo
        /// <summary>
        /// 条码化参数1
        /// </summary>
        [Label("条码化参数1")]
        public static readonly Property<string> BarcodeParam1Property = P<DataBarcode>.Register(e => e.BarcodeParam1);

        /// <summary>
        /// 条码化参数1
        /// </summary>
        public string BarcodeParam1
        {
            get { return GetProperty(BarcodeParam1Property); }
            set { SetProperty(BarcodeParam1Property, value); }
        }
        #endregion

        #region 条码化参数2 BarcodeParam2
        /// <summary>
        /// 条码化参数2
        /// </summary>
        [Label("条码化参数2")]
        public static readonly Property<string> BarcodeParam2Property = P<DataBarcode>.Register(e => e.BarcodeParam2);

        /// <summary>
        /// 条码化参数2
        /// </summary>
        public string BarcodeParam2
        {
            get { return GetProperty(BarcodeParam2Property); }
            set { SetProperty(BarcodeParam2Property, value); }
        }
        #endregion

        #region 条码化参数3 BarcodeParam3
        /// <summary>
        /// 条码化参数3
        /// </summary>
        [Label("条码化参数3")]
        public static readonly Property<string> BarcodeParam3Property = P<DataBarcode>.Register(e => e.BarcodeParam3);

        /// <summary>
        /// 条码化参数3
        /// </summary>
        public string BarcodeParam3
        {
            get { return GetProperty(BarcodeParam3Property); }
            set { SetProperty(BarcodeParam3Property, value); }
        }
        #endregion

        #region 条码化参数4 BarcodeParam4
        /// <summary>
        /// 条码化参数4
        /// </summary>
        [Label("条码化参数4")]
        public static readonly Property<string> BarcodeParam4Property = P<DataBarcode>.Register(e => e.BarcodeParam4);

        /// <summary>
        /// 条码化参数4
        /// </summary>
        public string BarcodeParam4
        {
            get { return GetProperty(BarcodeParam4Property); }
            set { SetProperty(BarcodeParam4Property, value); }
        }
        #endregion

        #region 条码化参数5 BarcodeParam5
        /// <summary>
        /// 条码化参数5
        /// </summary>
        [Label("条码化参数5")]
        public static readonly Property<string> BarcodeParam5Property = P<DataBarcode>.Register(e => e.BarcodeParam5);

        /// <summary>
        /// 条码化参数5
        /// </summary>
        public string BarcodeParam5
        {
            get { return GetProperty(BarcodeParam5Property); }
            set { SetProperty(BarcodeParam5Property, value); }
        }
        #endregion

        #region 条码化参数6 BarcodeParam6
        /// <summary>
        /// 条码化参数6
        /// </summary>
        [Label("条码化参数6")]
        public static readonly Property<string> BarcodeParam6Property = P<DataBarcode>.Register(e => e.BarcodeParam6);

        /// <summary>
        /// 条码化参数6
        /// </summary>
        public string BarcodeParam6
        {
            get { return GetProperty(BarcodeParam6Property); }
            set { SetProperty(BarcodeParam6Property, value); }
        }
        #endregion

        #region 条码化参数7 BarcodeParam7
        /// <summary>
        /// 条码化参数7
        /// </summary>
        [Label("条码化参数7")]
        public static readonly Property<string> BarcodeParam7Property = P<DataBarcode>.Register(e => e.BarcodeParam7);

        /// <summary>
        /// 条码化参数7
        /// </summary>
        public string BarcodeParam7
        {
            get { return GetProperty(BarcodeParam7Property); }
            set { SetProperty(BarcodeParam7Property, value); }
        }
        #endregion

        #region 条码化参数8 BarcodeParam8
        /// <summary>
        /// 条码化参数8
        /// </summary>
        [Label("条码化参数8")]
        public static readonly Property<string> BarcodeParam8Property = P<DataBarcode>.Register(e => e.BarcodeParam8);

        /// <summary>
        /// 条码化参数8
        /// </summary>
        public string BarcodeParam8
        {
            get { return GetProperty(BarcodeParam8Property); }
            set { SetProperty(BarcodeParam8Property, value); }
        }
        #endregion

        #region 条码化参数9 BarcodeParam9
        /// <summary>
        /// 条码化参数9
        /// </summary>
        [Label("条码化参数9")]
        public static readonly Property<string> BarcodeParam9Property = P<DataBarcode>.Register(e => e.BarcodeParam9);

        /// <summary>
        /// 条码化参数9
        /// </summary>
        public string BarcodeParam9
        {
            get { return GetProperty(BarcodeParam9Property); }
            set { SetProperty(BarcodeParam9Property, value); }
        }
        #endregion

        #region 条码化参数10 BarcodeParam10
        /// <summary>
        /// 条码化参数10
        /// </summary>
        [Label("条码化参数10")]
        public static readonly Property<string> BarcodeParam10Property = P<DataBarcode>.Register(e => e.BarcodeParam10);

        /// <summary>
        /// 条码化参数10
        /// </summary>
        public string BarcodeParam10
        {
            get { return GetProperty(BarcodeParam10Property); }
            set { SetProperty(BarcodeParam10Property, value); }
        }
        #endregion

        #region 条码化参数11 BarcodeParam11
        /// <summary>
        /// 条码化参数11
        /// </summary>
        [Label("条码化参数11")]
        public static readonly Property<string> BarcodeParam11Property = P<DataBarcode>.Register(e => e.BarcodeParam11);

        /// <summary>
        /// 条码化参数11
        /// </summary>
        public string BarcodeParam11
        {
            get { return GetProperty(BarcodeParam11Property); }
            set { SetProperty(BarcodeParam11Property, value); }
        }
        #endregion

        #region 条码化参数12 BarcodeParam12
        /// <summary>
        /// 条码化参数12
        /// </summary>
        [Label("条码化参数12")]
        public static readonly Property<string> BarcodeParam12Property = P<DataBarcode>.Register(e => e.BarcodeParam12);

        /// <summary>
        /// 条码化参数12
        /// </summary>
        public string BarcodeParam12
        {
            get { return GetProperty(BarcodeParam12Property); }
            set { SetProperty(BarcodeParam12Property, value); }
        }
        #endregion

        #region 条码化参数13 BarcodeParam13
        /// <summary>
        /// 条码化参数13
        /// </summary>
        [Label("条码化参数13")]
        public static readonly Property<string> BarcodeParam13Property = P<DataBarcode>.Register(e => e.BarcodeParam13);

        /// <summary>
        /// 条码化参数13
        /// </summary>
        public string BarcodeParam13
        {
            get { return GetProperty(BarcodeParam13Property); }
            set { SetProperty(BarcodeParam13Property, value); }
        }
        #endregion

        #region 条码化参数14 BarcodeParam14
        /// <summary>
        /// 条码化参数14
        /// </summary>
        [Label("条码化参数14")]
        public static readonly Property<string> BarcodeParam14Property = P<DataBarcode>.Register(e => e.BarcodeParam14);

        /// <summary>
        /// 条码化参数14
        /// </summary>
        public string BarcodeParam14
        {
            get { return GetProperty(BarcodeParam14Property); }
            set { SetProperty(BarcodeParam14Property, value); }
        }
        #endregion

        #region 条码化参数15 BarcodeParam15
        /// <summary>
        /// 条码化参数15
        /// </summary>
        [Label("条码化参数15")]
        public static readonly Property<string> BarcodeParam15Property = P<DataBarcode>.Register(e => e.BarcodeParam15);

        /// <summary>
        /// 条码化参数15
        /// </summary>
        public string BarcodeParam15
        {
            get { return GetProperty(BarcodeParam15Property); }
            set { SetProperty(BarcodeParam15Property, value); }
        }
        #endregion

        #region 条码化参数16 BarcodeParam16
        /// <summary>
        /// 条码化参数16
        /// </summary>
        [Label("条码化参数16")]
        public static readonly Property<string> BarcodeParam16Property = P<DataBarcode>.Register(e => e.BarcodeParam16);

        /// <summary>
        /// 条码化参数16
        /// </summary>
        public string BarcodeParam16
        {
            get { return GetProperty(BarcodeParam16Property); }
            set { SetProperty(BarcodeParam16Property, value); }
        }
        #endregion

        #region 条码化参数17 BarcodeParam17
        /// <summary>
        /// 条码化参数17
        /// </summary>
        [Label("条码化参数17")]
        public static readonly Property<string> BarcodeParam17Property = P<DataBarcode>.Register(e => e.BarcodeParam17);

        /// <summary>
        /// 条码化参数17
        /// </summary>
        public string BarcodeParam17
        {
            get { return GetProperty(BarcodeParam17Property); }
            set { SetProperty(BarcodeParam17Property, value); }
        }
        #endregion

        #region 条码化参数18 BarcodeParam18
        /// <summary>
        /// 条码化参数18
        /// </summary>
        [Label("条码化参数18")]
        public static readonly Property<string> BarcodeParam18Property = P<DataBarcode>.Register(e => e.BarcodeParam18);

        /// <summary>
        /// 条码化参数18
        /// </summary>
        public string BarcodeParam18
        {
            get { return GetProperty(BarcodeParam18Property); }
            set { SetProperty(BarcodeParam18Property, value); }
        }
        #endregion

        #region 条码化参数19 BarcodeParam19
        /// <summary>
        /// 条码化参数19
        /// </summary>
        [Label("条码化参数19")]
        public static readonly Property<string> BarcodeParam19Property = P<DataBarcode>.Register(e => e.BarcodeParam19);

        /// <summary>
        /// 条码化参数19
        /// </summary>
        public string BarcodeParam19
        {
            get { return GetProperty(BarcodeParam19Property); }
            set { SetProperty(BarcodeParam19Property, value); }
        }
        #endregion
    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class DataBarcodeConfig : EntityConfig<DataBarcode>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("DATA_BARCODE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
