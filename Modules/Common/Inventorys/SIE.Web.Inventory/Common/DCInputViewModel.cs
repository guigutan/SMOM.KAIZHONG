using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.Inventory.Common
{
    /// <summary>
    /// DC输入ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("D/C输入")]
    public class DCInputViewModel : ViewModel
    {
        #region 格式 Format
        /// <summary>
        /// 格式
        /// </summary>
        [Label("格式")]
        public static readonly Property<string> FormatProperty = P<DCInputViewModel>.Register(e => e.Format);

        /// <summary>
        /// 格式
        /// </summary>
        public string Format
        {
            get { return this.GetProperty(FormatProperty); }
            set { this.SetProperty(FormatProperty, value); }
        }
        #endregion

        #region 年周 YearWeek
        /// <summary>
        /// 年周
        /// </summary>
        [Label("年周")]
        public static readonly Property<bool> YearWeekProperty = P<DCInputViewModel>.Register(e => e.YearWeek);

        /// <summary>
        /// 年周
        /// </summary>
        public bool YearWeek
        {
            get { return this.GetProperty(YearWeekProperty); }
            set { this.SetProperty(YearWeekProperty, value); }
        }
        #endregion

        #region 周年 WeekYear
        /// <summary>
        /// 周年
        /// </summary>
        [Label("周年")]
        public static readonly Property<bool> WeekYearProperty = P<DCInputViewModel>.Register(e => e.WeekYear);

        /// <summary>
        /// 周年
        /// </summary>
        public bool WeekYear
        {
            get { return this.GetProperty(WeekYearProperty); }
            set { this.SetProperty(WeekYearProperty, value); }
        }
        #endregion

        #region 年月日 YearMonthDay
        /// <summary>
        /// 年月日
        /// </summary>
        [Label("年月日")]
        public static readonly Property<bool> YearMonthDayProperty = P<DCInputViewModel>.Register(e => e.YearMonthDay);

        /// <summary>
        /// 年月日
        /// </summary>
        public bool YearMonthDay
        {
            get { return this.GetProperty(YearMonthDayProperty); }
            set { this.SetProperty(YearMonthDayProperty, value); }
        }
        #endregion

        #region 输入 Input
        /// <summary>
        /// 输入
        /// </summary>
        [Label("输入")]
        public static readonly Property<string> InputProperty = P<DCInputViewModel>.Register(e => e.Input);

        /// <summary>
        /// 输入
        /// </summary>
        public string Input
        {
            get { return this.GetProperty(InputProperty); }
            set { this.SetProperty(InputProperty, value); }
        }
        #endregion

        #region 转换 Transform
        /// <summary>
        /// 转换
        /// </summary>
        [Label("转换")]
        public static readonly Property<string> TransformProperty = P<DCInputViewModel>.Register(e => e.Transform);

        /// <summary>
        /// 转换
        /// </summary>
        public string Transform
        {
            get { return this.GetProperty(TransformProperty); }
            set { this.SetProperty(TransformProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 配置ViewModel
    /// </summary>
    internal class DCInputViewModelViewConfig : WebViewConfig<DCInputViewModel>
    {
        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            using (View.OrderProperties())
            {
                View.Property(p => p.Format).UseDisplayEditor(p => p.XType = "DCInputFormat").Show();
                View.Property(p => p.Input).Show();
                View.Property(p => p.Transform).UseDisplayEditor().Readonly().Show();
            }
        }
    }
}
