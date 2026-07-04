using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses.Stations
{
	/// <summary>
	/// LED屏幕显示的风格样式
	/// </summary>
	[RootEntity, Serializable]
	[CriteriaQuery]
	[Label("LED屏幕显示的风格样式")]
	[DisplayMember(nameof(Code))]
	public partial class LEDShowStyle : DataEntity
	{
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
		[Required, NotDuplicate]
		public static readonly Property<string> CodeProperty = P<LEDShowStyle>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 字体区域：左上角X坐标 FontAreaLx
        /// <summary>
        /// 字体区域：左上角X坐标
        /// </summary>
        [Label("左上角X坐标")]
		public static readonly Property<int> FontAreaLxProperty = P<LEDShowStyle>.Register(e => e.FontAreaLx);

		/// <summary>
		/// 字体区域：左上角X坐标
		/// </summary>
		public int FontAreaLx
		{
			get { return GetProperty(FontAreaLxProperty); }
			set { SetProperty(FontAreaLxProperty, value); }
		}
		#endregion

		#region 字体区域：左上角Y坐标 FontAreaLy
		/// <summary>
		/// 字体区域：左上角Y坐标
		/// </summary>
		[Label("左上角Y坐标")]
		public static readonly Property<int> FontAreaLyProperty = P<LEDShowStyle>.Register(e => e.FontAreaLy);

		/// <summary>
		/// 字体区域：左上角Y坐标
		/// </summary>
		public int FontAreaLy
		{
			get { return GetProperty(FontAreaLyProperty); }
			set { SetProperty(FontAreaLyProperty, value); }
		}
		#endregion

		#region 字体区域：高度 FontAreaHigh
		/// <summary>
		/// 字体区域：高度
		/// </summary>
		[Label("高度")]
		public static readonly Property<int> FontAreaHighProperty = P<LEDShowStyle>.Register(e => e.FontAreaHigh);

		/// <summary>
		/// 字体区域：高度
		/// </summary>
		public int FontAreaHigh
		{
			get { return GetProperty(FontAreaHighProperty); }
			set { SetProperty(FontAreaHighProperty, value); }
		}
		#endregion

		#region 字体区域：宽度 FontAreaWide
		/// <summary>
		/// 字体区域：宽度
		/// </summary>
		[Label("宽度")]
		public static readonly Property<int> FontAreaWideProperty = P<LEDShowStyle>.Register(e => e.FontAreaWide);

		/// <summary>
		/// 字体区域：宽度
		/// </summary>
		public int FontAreaWide
		{
			get { return GetProperty(FontAreaWideProperty); }
			set { SetProperty(FontAreaWideProperty, value); }
		}
		#endregion

		#region 字体区域：边框模式 FontAreaFrameMode
		/// <summary>
		/// 字体区域：边框模式
		/// </summary>
		[Label("边框模式")]
		public static readonly Property<int> FontAreaFrameModeProperty = P<LEDShowStyle>.Register(e => e.FontAreaFrameMode);

		/// <summary>
		/// 字体区域：边框模式
		/// </summary>
		public int FontAreaFrameMode
		{
			get { return GetProperty(FontAreaFrameModeProperty); }
			set { SetProperty(FontAreaFrameModeProperty, value); }
		}
		#endregion

		#region 字体区域：边框颜色 FontAreaFrameColor
		/// <summary>
		/// 字体区域：边框颜色
		/// </summary>
		[Label("边框颜色")]
		public static readonly Property<int> FontAreaFrameColorProperty = P<LEDShowStyle>.Register(e => e.FontAreaFrameColor);

		/// <summary>
		/// 字体区域：边框颜色
		/// </summary>
		public int FontAreaFrameColor
		{
			get { return GetProperty(FontAreaFrameColorProperty); }
			set { SetProperty(FontAreaFrameColorProperty, value); }
		}
		#endregion

		#region 字体样式：字体名称 FontStyleFontName
		/// <summary>
		/// 字体样式：字体名称
		/// </summary>
		[Label("字体名称")]
		public static readonly Property<string> FontStyleFontNameProperty = P<LEDShowStyle>.Register(e => e.FontStyleFontName);

		/// <summary>
		/// 字体样式：字体名称
		/// </summary>
		public string FontStyleFontName
		{
			get { return GetProperty(FontStyleFontNameProperty); }
			set { SetProperty(FontStyleFontNameProperty, value); }
		}
		#endregion

		#region 字体样式：字体大小 FontStyleSize
		/// <summary>
		/// 字体样式：字体大小
		/// </summary>
		[Label("字体大小")]
		public static readonly Property<int> FontStyleSizeProperty = P<LEDShowStyle>.Register(e => e.FontStyleSize);

		/// <summary>
		/// 字体样式：字体大小
		/// </summary>
		public int FontStyleSize
		{
			get { return GetProperty(FontStyleSizeProperty); }
			set { SetProperty(FontStyleSizeProperty, value); }
		}
		#endregion

		#region 字体样式：是否加粗 FontStyleIsBold
		/// <summary>
		/// 字体样式：是否加粗
		/// </summary>
		[Label("是否加粗")]
		public static readonly Property<bool> FontStyleIsBoldProperty = P<LEDShowStyle>.Register(e => e.FontStyleIsBold);

		/// <summary>
		/// 字体样式：是否加粗
		/// </summary>
		public bool FontStyleIsBold
		{
			get { return GetProperty(FontStyleIsBoldProperty); }
			set { SetProperty(FontStyleIsBoldProperty, value); }
		}
		#endregion

		#region 字体样式：是否斜体 FontStyleIsItaic
		/// <summary>
		/// 字体样式：是否斜体
		/// </summary>
		[Label("是否斜体")]
		public static readonly Property<bool> FontStyleIsItaicProperty = P<LEDShowStyle>.Register(e => e.FontStyleIsItaic);

		/// <summary>
		/// 字体样式：是否斜体
		/// </summary>
		public bool FontStyleIsItaic
		{
			get { return GetProperty(FontStyleIsItaicProperty); }
			set { SetProperty(FontStyleIsItaicProperty, value); }
		}
		#endregion

		#region 字体样式：是否加下划线 FontStyleIsUnderline
		/// <summary>
		/// 字体样式：是否加下划线
		/// </summary>
		[Label("是否加下划线")]
		public static readonly Property<bool> FontStyleIsUnderlineProperty = P<LEDShowStyle>.Register(e => e.FontStyleIsUnderline);

		/// <summary>
		/// 字体样式：是否加下划线
		/// </summary>
		public bool FontStyleIsUnderline
		{
			get { return GetProperty(FontStyleIsUnderlineProperty); }
			set { SetProperty(FontStyleIsUnderlineProperty, value); }
		}
		#endregion

		#region 字体样式：字体颜色 FontStyleFontColor
		/// <summary>
		/// 字体样式：字体颜色
		/// </summary>
		[Label("字体颜色")]
		public static readonly Property<string> FontStyleFontColorProperty = P<LEDShowStyle>.Register(e => e.FontStyleFontColor);

		/// <summary>
		/// 字体样式：字体颜色
		/// </summary>
		public string FontStyleFontColor
		{
			get { return GetProperty(FontStyleFontColorProperty); }
			set { SetProperty(FontStyleFontColorProperty, value); }
		}
		#endregion

		#region 字体样式：行距 FontStyleRowSpace
		/// <summary>
		/// 字体样式：行距
		/// </summary>
		[Label("行距")]
		public static readonly Property<int> FontStyleRowSpaceProperty = P<LEDShowStyle>.Register(e => e.FontStyleRowSpace);

		/// <summary>
		/// 字体样式：行距
		/// </summary>
		public int FontStyleRowSpace
		{
			get { return GetProperty(FontStyleRowSpaceProperty); }
			set { SetProperty(FontStyleRowSpaceProperty, value); }
		}
		#endregion

		#region 字体样式：水平样式(0：左对齐，1：居中，2右对齐) FontStyleAlignStyle
		/// <summary>
		/// 字体样式：水平样式(0：左对齐，1：居中，2右对齐)
		/// </summary>
		[Label("水平样式")]
		public static readonly Property<int> FontStyleAlignStyleProperty = P<LEDShowStyle>.Register(e => e.FontStyleAlignStyle);

		/// <summary>
		/// 字体样式：水平样式(0：左对齐，1：居中，2右对齐)
		/// </summary>
		public int FontStyleAlignStyle
		{
			get { return GetProperty(FontStyleAlignStyleProperty); }
			set { SetProperty(FontStyleAlignStyleProperty, value); }
		}
		#endregion

		#region 字体样式：竖直样式(0：顶对齐，1：上下居中，2：底对齐) FontStyleVAlignStyle
		/// <summary>
		/// 字体样式：竖直样式(0：顶对齐，1：上下居中，2：底对齐)
		/// </summary>
		[Label("竖直样式")]
		public static readonly Property<int> FontStyleVAlignStyleProperty = P<LEDShowStyle>.Register(e => e.FontStyleVAlignStyle);

		/// <summary>
		/// 字体样式：竖直样式(0：顶对齐，1：上下居中，2：底对齐)
		/// </summary>
		public int FontStyleVAlignStyle
		{
			get { return GetProperty(FontStyleVAlignStyleProperty); }
			set { SetProperty(FontStyleVAlignStyleProperty, value); }
		}
		#endregion

		#region 动作样式：0到50，参见协议 MoveSetActionType
		/// <summary>
		/// 动作样式：0到50，参见协议
		/// </summary>
		[Label("动作样式")]
		public static readonly Property<int> MoveSetActionTypeProperty = P<LEDShowStyle>.Register(e => e.MoveSetActionType);

		/// <summary>
		/// 动作样式：0到50，参见协议
		/// </summary>
		public int MoveSetActionType
		{
			get { return GetProperty(MoveSetActionTypeProperty); }
			set { SetProperty(MoveSetActionTypeProperty, value); }
		}
		#endregion

		#region 动作速度，取值1到20 MoveSetActionSpeed
		/// <summary>
		/// 动作速度，取值1到20
		/// </summary>
		[Label("动作速度")]
		public static readonly Property<int> MoveSetActionSpeedProperty = P<LEDShowStyle>.Register(e => e.MoveSetActionSpeed);

		/// <summary>
		/// 动作速度，取值1到20
		/// </summary>
		public int MoveSetActionSpeed
		{
			get { return GetProperty(MoveSetActionSpeedProperty); }
			set { SetProperty(MoveSetActionSpeedProperty, value); }
		}
		#endregion

		#region 是否清除背景 MoveSetIsBackgroundClear
		/// <summary>
		/// 是否清除背景
		/// </summary>
		[Label("是否清除背景")]
		public static readonly Property<bool> MoveSetIsBackgroundClearProperty = P<LEDShowStyle>.Register(e => e.MoveSetIsBackgroundClear);

		/// <summary>
		/// 是否清除背景
		/// </summary>
		public bool MoveSetIsBackgroundClear
		{
			get { return GetProperty(MoveSetIsBackgroundClearProperty); }
			set { SetProperty(MoveSetIsBackgroundClearProperty, value); }
		}
		#endregion

		#region 停留时间，单位0.1S MoveSetHoldTime
		/// <summary>
		/// 停留时间，单位0.1S
		/// </summary>
		[Label("停留时间")]
		public static readonly Property<int> MoveSetHoldTimeProperty = P<LEDShowStyle>.Register(e => e.MoveSetHoldTime);

		/// <summary>
		/// 停留时间，单位0.1S
		/// </summary>
		public int MoveSetHoldTime
		{
			get { return GetProperty(MoveSetHoldTimeProperty); }
			set { SetProperty(MoveSetHoldTimeProperty, value); }
		}
		#endregion

		#region 清除速度 MoveSetClearSpeed
		/// <summary>
		/// 清除速度
		/// </summary>
		[Label("清除速度")]
		public static readonly Property<int> MoveSetClearSpeedProperty = P<LEDShowStyle>.Register(e => e.MoveSetClearSpeed);

		/// <summary>
		/// 清除速度
		/// </summary>
		public int MoveSetClearSpeed
		{
			get { return GetProperty(MoveSetClearSpeedProperty); }
			set { SetProperty(MoveSetClearSpeedProperty, value); }
		}
		#endregion

		#region 清除动作类型 MoveSetClearActionType
		/// <summary>
		/// 清除动作类型
		/// </summary>
		[Label("清除动作类型")]
		public static readonly Property<int> MoveSetClearActionTypeProperty = P<LEDShowStyle>.Register(e => e.MoveSetClearActionType);

		/// <summary>
		/// 清除动作类型
		/// </summary>
		public int MoveSetClearActionType
		{
			get { return GetProperty(MoveSetClearActionTypeProperty); }
			set { SetProperty(MoveSetClearActionTypeProperty, value); }
		}
		#endregion

		#region 每帧时间，取值20-200 MoveSetFrameTime
		/// <summary>
		/// 每帧时间，取值20-200
		/// </summary>
		[Label("每帧时间")]
		public static readonly Property<int> MoveSetFrameTimeProperty = P<LEDShowStyle>.Register(e => e.MoveSetFrameTime);

		/// <summary>
		/// 每帧时间，取值20-200
		/// </summary>
		public int MoveSetFrameTime
		{
			get { return GetProperty(MoveSetFrameTimeProperty); }
			set { SetProperty(MoveSetFrameTimeProperty, value); }
		}
		#endregion
	}

	/// <summary>
	/// LED屏幕显示的风格样式 实体配置
	/// </summary>
	internal class LEDShowStyleConfig : EntityConfig<LEDShowStyle>
	{
		/// <summary>
      	  	/// 配置元数据
    	    	/// </summary>
		protected override void ConfigMeta()
		{
			Meta.MapTable("WCS_LED_SHOW_STYPE").MapAllProperties();
			Meta.EnablePhantoms();
			Meta.DisableDataSync();
		}
	}
}