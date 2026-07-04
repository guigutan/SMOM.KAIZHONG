using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Stations
{
    /// <summary>
    /// LED屏幕基础数据
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("LED屏幕基础数据")]
    [DisplayMember(nameof(Code))]
    public partial class LED : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        [Required, NotDuplicate]
        public static readonly Property<string> CodeProperty = P<LED>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 控制卡编号 CardNo
        /// <summary>
        /// 控制卡编号,确定是哪块LED屏
        /// </summary>
        [Label("控制卡编号")]
        public static readonly Property<int> CardNoProperty = P<LED>.Register(e => e.CardNo);

        /// <summary>
        /// 控制卡编号,确定是哪块LED屏
        /// </summary>
        public int CardNo
        {
            get { return GetProperty(CardNoProperty); }
            set { SetProperty(CardNoProperty, value); }
        }
        #endregion

        #region 控制卡类型 CardType
        /// <summary>
        /// 控制卡类型
        /// </summary>
        [Label("控制卡类型")]
        public static readonly Property<string> CardTypeProperty = P<LED>.Register(e => e.CardType);

        /// <summary>
        /// 控制卡类型
        /// </summary>
        public string CardType
        {
            get { return GetProperty(CardTypeProperty); }
            set { SetProperty(CardTypeProperty, value); }
        }
        #endregion

        #region 控制卡通讯模式 CommunicationMode
        /// <summary>
        /// 控制卡通讯模式，串口通讯=0、网路通讯=1
        /// </summary>
        [MaxLength(128)]
        [Label("控制卡通讯模式")]
        public static readonly Property<string> CommunicationModeProperty = P<LED>.Register(e => e.CommunicationMode);

        /// <summary>
        /// 控制卡通讯模式，串口通讯=0、网路通讯=1
        /// </summary>
        public string CommunicationMode
        {
            get { return GetProperty(CommunicationModeProperty); }
            set { SetProperty(CommunicationModeProperty, value); }
        }
        #endregion

        #region LED屏幕的宽度 ScreemWidth
        /// <summary>
        /// LED屏幕的宽度，取值为：8的倍数
        /// </summary>
        [Label("LED屏幕的宽度")]
        public static readonly Property<int> ScreemWidthProperty = P<LED>.Register(e => e.ScreemWidth);

        /// <summary>
        /// LED屏幕的宽度，取值为：8的倍数
        /// </summary>
        public int ScreemWidth
        {
            get { return GetProperty(ScreemWidthProperty); }
            set { SetProperty(ScreemWidthProperty, value); }
        }
        #endregion

        #region LED屏幕的高度 ScreemHeight
        /// <summary>
        /// LED屏幕的高度，取值为：8的倍数
        /// </summary>
        [Label("LED屏幕的高度")]
        public static readonly Property<int> ScreemHeightProperty = P<LED>.Register(e => e.ScreemHeight);

        /// <summary>
        /// LED屏幕的高度，取值为：8的倍数
        /// </summary>
        public int ScreemHeight
        {
            get { return GetProperty(ScreemHeightProperty); }
            set { SetProperty(ScreemHeightProperty, value); }
        }
        #endregion

        #region 串口波特率 SerialBaud
        /// <summary>
        /// 串口波特率
        /// </summary>
        [Label("串口波特率")]
        public static readonly Property<int> SerialBaudProperty = P<LED>.Register(e => e.SerialBaud);

        /// <summary>
        /// 串口波特率
        /// </summary>
        public int SerialBaud
        {
            get { return GetProperty(SerialBaudProperty); }
            set { SetProperty(SerialBaudProperty, value); }
        }
        #endregion

        #region 串口号 SerialNum
        /// <summary>
        /// 串口号
        /// </summary>
        [Label("串口号")]
        public static readonly Property<int> SerialNumProperty = P<LED>.Register(e => e.SerialNum);

        /// <summary>
        /// 串口号
        /// </summary>
        public int SerialNum
        {
            get { return GetProperty(SerialNumProperty); }
            set { SetProperty(SerialNumProperty, value); }
        }
        #endregion

        #region 对应的IP地址 IpAddress
        /// <summary>
        /// LED屏对应的IP地址
        /// </summary>
        [MaxLength(32)]
        [Label("对应的IP地址")]
        public static readonly Property<string> IpAddressProperty = P<LED>.Register(e => e.IpAddress);

        /// <summary>
        /// LED屏对应的IP地址
        /// </summary>
        public string IpAddress
        {
            get { return GetProperty(IpAddressProperty); }
            set { SetProperty(IpAddressProperty, value); }
        }
        #endregion

        #region 对应的端口 NetPort
        /// <summary>
        /// LED屏对应的端口，必须为5005
        /// </summary>
        [Label("对应的端口")]
        public static readonly Property<int> NetPortProperty = P<LED>.Register(e => e.NetPort);

        /// <summary>
        /// LED屏对应的端口，必须为5005
        /// </summary>
        public int NetPort
        {
            get { return GetProperty(NetPortProperty); }
            set { SetProperty(NetPortProperty, value); }
        }
        #endregion

        #region 颜色类型 ColorStyle
        /// <summary>
        /// 显示屏颜色类型:0--单色屏，1--双色屏
        /// </summary>
        [Label("颜色类型")]
        public static readonly Property<int> ColorStyleProperty = P<LED>.Register(e => e.ColorStyle);

        /// <summary>
        /// 显示屏颜色类型:0--单色屏，1--双色屏
        /// </summary>
        public int ColorStyle
        {
            get { return GetProperty(ColorStyleProperty); }
            set { SetProperty(ColorStyleProperty, value); }
        }
        #endregion

        #region 默认显示的标题头 Title
        /// <summary>
        /// 默认显示的标题头
        /// </summary>
        [MaxLength(1024)]
        [Label("默认显示的标题头")]
        public static readonly Property<string> TitleProperty = P<LED>.Register(e => e.Title);

        /// <summary>
        /// 默认显示的标题头
        /// </summary>
        public string Title
        {
            get { return GetProperty(TitleProperty); }
            set { SetProperty(TitleProperty, value); }
        }
        #endregion

        #region 超时时间/毫秒 Timeout
        /// <summary>
        /// 超时时间，单位毫秒
        /// </summary>
        [Label("超时时间/毫秒")]
        public static readonly Property<int> TimeoutProperty = P<LED>.Register(e => e.Timeout);

        /// <summary>
        /// 超时时间，单位毫秒
        /// </summary>
        public int Timeout
        {
            get { return GetProperty(TimeoutProperty); }
            set { SetProperty(TimeoutProperty, value); }
        }
        #endregion

        #region 设备空闲时间默认显示的文本 DefaultText
        /// <summary>
        /// 设备空闲时间默认显示的文本
        /// </summary>
        [MaxLength(256)]
        [Label("设备空闲时间默认显示的文本")]
        public static readonly Property<string> DefaultTextProperty = P<LED>.Register(e => e.DefaultText);

        /// <summary>
        /// 设备空闲时间默认显示的文本
        /// </summary>
        public string DefaultText
        {
            get { return GetProperty(DefaultTextProperty); }
            set { SetProperty(DefaultTextProperty, value); }
        }
        #endregion

        #region 访问密码 Password
        /// <summary>
        /// 访问密码
        /// </summary>
        [MaxLength(32)]
        [Label("访问密码")]
        public static readonly Property<string> PasswordProperty = P<LED>.Register(e => e.Password);

        /// <summary>
        /// 访问密码
        /// </summary>
        public string Password
        {
            get { return GetProperty(PasswordProperty); }
            set { SetProperty(PasswordProperty, value); }
        }
        #endregion

        #region 备注 Note
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(128)]
        [Label("备注")]
        public static readonly Property<string> NoteProperty = P<LED>.Register(e => e.Note);

        /// <summary>
        /// 备注
        /// </summary>
        public string Note
        {
            get { return GetProperty(NoteProperty); }
            set { SetProperty(NoteProperty, value); }
        }
        #endregion

        #region LED屏幕显示的风格样式 ShowStype
        /// <summary>
        /// LED屏幕显示的风格样式Id
        /// </summary>
        [Label("屏幕风格样式")]
        public static readonly IRefIdProperty ShowStypeIdProperty = P<LED>.RegisterRefId(e => e.ShowStypeId, ReferenceType.Normal);

        /// <summary>
        /// LED屏幕显示的风格样式Id
        /// </summary>
        public double ShowStypeId
        {
            get { return (double)GetRefId(ShowStypeIdProperty); }
            set { SetRefId(ShowStypeIdProperty, value); }
        }

        /// <summary>
        /// LED屏幕显示的风格样式
        /// </summary>
        public static readonly RefEntityProperty<LEDShowStyle> ShowStypeProperty = P<LED>.RegisterRef(e => e.ShowStype, ShowStypeIdProperty);

        /// <summary>
        /// LED屏幕显示的风格样式
        /// </summary>
        public LEDShowStyle ShowStype
        {
            get { return GetRefEntity(ShowStypeProperty); }
            set { SetRefEntity(ShowStypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// LED屏幕基础数据 实体配置
    /// </summary>
    internal class LEDConfig : EntityConfig<LED>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WCS_LED").MapAllProperties();
            Meta.Property(LED.CommunicationModeProperty).ColumnMeta.HasLength(256);
            Meta.Property(LED.IpAddressProperty).ColumnMeta.HasLength(64);
            Meta.Property(LED.TitleProperty).ColumnMeta.HasLength(2048);
            Meta.Property(LED.DefaultTextProperty).ColumnMeta.HasLength(512);
            Meta.Property(LED.PasswordProperty).ColumnMeta.HasLength(64);
            Meta.Property(LED.NoteProperty).ColumnMeta.HasLength(256);
            Meta.EnablePhantoms();
            Meta.DisableDataSync();
        }
    }
}