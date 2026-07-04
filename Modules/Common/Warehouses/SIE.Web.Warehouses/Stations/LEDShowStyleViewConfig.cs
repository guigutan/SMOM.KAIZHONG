using SIE.Warehouses.Stations;

namespace SIE.Web.Warehouses.Stations
{
    /// <summary>
    /// LED屏幕显示的风格样式视图配置
    /// </summary>
    internal class LEDShowStyleViewConfig : WebViewConfig<LEDShowStyle>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands().UseImportCommands();
            View.Property(p => p.Code);
            View.Property(p => p.FontAreaLx);
            View.Property(p => p.FontAreaLy);
            View.Property(p => p.FontAreaHigh);
            View.Property(p => p.FontAreaWide);
            View.Property(p => p.FontAreaFrameMode);
            View.Property(p => p.FontAreaFrameColor);
            View.Property(p => p.FontStyleFontName);
            View.Property(p => p.FontStyleSize);
            View.Property(p => p.FontStyleIsBold);
            View.Property(p => p.FontStyleIsItaic);
            View.Property(p => p.FontStyleIsUnderline);
            View.Property(p => p.FontStyleFontColor);
            View.Property(p => p.FontStyleRowSpace);
            View.Property(p => p.FontStyleAlignStyle);
            View.Property(p => p.FontStyleVAlignStyle);
            View.Property(p => p.MoveSetActionType);
            View.Property(p => p.MoveSetActionSpeed);
            View.Property(p => p.MoveSetIsBackgroundClear);
            View.Property(p => p.MoveSetHoldTime);
            View.Property(p => p.MoveSetClearSpeed);
            View.Property(p => p.MoveSetClearActionType);
            View.Property(p => p.MoveSetFrameTime);
        }

        /// <summary>
        /// 查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.FontAreaLx);
            View.Property(p => p.FontAreaLy);
            View.Property(p => p.FontAreaHigh);
            View.Property(p => p.FontAreaWide);
            View.Property(p => p.FontAreaFrameMode);
            View.Property(p => p.FontAreaFrameColor);
            View.Property(p => p.FontStyleFontName);
            View.Property(p => p.FontStyleSize);
            View.Property(p => p.FontStyleIsBold);
            View.Property(p => p.FontStyleIsItaic);
            View.Property(p => p.FontStyleIsUnderline);
            View.Property(p => p.FontStyleFontColor);
            View.Property(p => p.FontStyleRowSpace);
            View.Property(p => p.FontStyleAlignStyle);
            View.Property(p => p.FontStyleVAlignStyle);
            View.Property(p => p.MoveSetActionType);
            View.Property(p => p.MoveSetActionSpeed);
            View.Property(p => p.MoveSetIsBackgroundClear);
            View.Property(p => p.MoveSetHoldTime);
            View.Property(p => p.MoveSetClearSpeed);
            View.Property(p => p.MoveSetClearActionType);
            View.Property(p => p.MoveSetFrameTime);
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.FontAreaLx);
            View.Property(p => p.FontAreaLy);
            View.Property(p => p.FontAreaHigh);
            View.Property(p => p.FontAreaWide);
            View.Property(p => p.FontAreaFrameMode);
            View.Property(p => p.FontAreaFrameColor);
            View.Property(p => p.FontStyleFontName);
            View.Property(p => p.FontStyleSize);
            View.Property(p => p.FontStyleIsBold);
            View.Property(p => p.FontStyleIsItaic);
            View.Property(p => p.FontStyleIsUnderline);
            View.Property(p => p.FontStyleFontColor);
            View.Property(p => p.FontStyleRowSpace);
            View.Property(p => p.FontStyleAlignStyle);
            View.Property(p => p.FontStyleVAlignStyle);
            View.Property(p => p.MoveSetActionType);
            View.Property(p => p.MoveSetActionSpeed);
            View.Property(p => p.MoveSetIsBackgroundClear);
            View.Property(p => p.MoveSetHoldTime);
            View.Property(p => p.MoveSetClearSpeed);
            View.Property(p => p.MoveSetClearActionType);
            View.Property(p => p.MoveSetFrameTime);
        }
    }
}