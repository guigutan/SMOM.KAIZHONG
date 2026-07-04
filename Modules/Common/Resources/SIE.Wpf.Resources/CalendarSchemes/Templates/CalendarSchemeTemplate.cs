using SIE.MetaModel.View;
using SIE.Resources.CalendarSchemes;
using SIE.Wpf.Resources.CalendarSchemes.Layouts;

namespace SIE.Wpf.Resources.CalendarSchemes.Templates
{
    /// <summary>
    /// 日历方案Template
    /// </summary>
    class CalendarSchemeTemplate : ListUITemplate
    {
        /// <summary>
        /// 构造方法
        /// </summary>
        public CalendarSchemeTemplate() : base(typeof(CalendarScheme)) { }

        /// <summary>
        /// 获取当前模板的结构定义。
        /// 结构定义包括：块间的结构、布局、块对应的视图的扩展名。
        /// </summary>
        /// <returns>聚合块</returns>
        protected override AggtBlocks DefineBlocks()
        {
            var blocks = base.DefineBlocks();
            blocks.Layout = new LayoutMeta(typeof(ExDockLayout));
            return blocks;
        }
    }
}
