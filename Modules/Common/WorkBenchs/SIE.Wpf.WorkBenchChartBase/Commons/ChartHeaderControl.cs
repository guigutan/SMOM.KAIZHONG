using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.Concerns;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 图标头控件
    /// </summary>
    public class ChartHeaderControl : UserControl
    {
        /// <summary>
        /// 新建命令委托
        /// </summary>
        public event Action<object, ExecutedRoutedEventArgs> NewAction;

        /// <summary>
        /// 新建命令
        /// </summary>
        public static RoutedUICommand New { get; set; } = new RoutedUICommand();

        /// <summary>
        /// 收藏命令
        /// </summary>
        public static RoutedUICommand Collect { get; set; } = new RoutedUICommand();
         
        /// <summary>
        /// 构造函数
        /// </summary>
        public ChartHeaderControl()
        {
            const string defpath = "pack://application:,,,/SIE.Wpf.WorkBenchChartBase;component/Themes/HeaderTemplate.xaml";
            this.Resources.MergedDictionaries.Add(new System.Windows.ResourceDictionary() { Source = new Uri(defpath) });
            this.CommandBindings.Add(new CommandBinding(New, NewCommandBinding_Executed));
            this.CommandBindings.Add(new CommandBinding(Collect, CollectCommandBinding_Executed));
            this.DataContextChanged += ChartHeaderControl_DataContextChanged;  
            this.Loaded += ChartHeaderControl_Loaded;
        }
        /// <summary>
        /// 获取关注状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChartHeaderControl_Loaded(object sender, RoutedEventArgs e)
        {
            var header = (this);
            var label = header.Template.FindName("lbTitle", this) as Label;
            var btnCollect = header.Template.FindName("btnCollect", this) as Button;
            if (label != null && label.Content != null && btnCollect != null)
            {
                var title = label.Content.ToString();
                var concernsInfo = RT.Service.Resolve<ConcernsController>().GetMyConcerns().FirstOrDefault(r => r.Name == title);
                if (concernsInfo != null)
                    btnCollect.Foreground = Brushes.Orange;
                else
                    btnCollect.Foreground = Brushes.Gray;
            }
        }
         

        private void ChartHeaderControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ChartHeaderControl_Loaded(null, null);
        }
 

        /// <summary>
        /// 新建命令执行方法
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">命令参数</param>
        protected virtual void NewCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            NewAction(sender, e);
        }

        /// <summary>
        /// 收藏命令执行方法
        /// </summary>
        /// <param name="sender">控件对象</param>
        /// <param name="e">命令参数</param>
        private void CollectCommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var btn = e.OriginalSource as Button;
            btn.Foreground = btn.Foreground  == Brushes.Orange ? Brushes.Gray: Brushes.Orange;
            btn.MoveFocus(new TraversalRequest(FocusNavigationDirection.Down));
            var lbTitle = ((btn.Parent as FrameworkElement).Parent as FrameworkElement).FindName("lbTitle") as Label;
            var title = lbTitle.Content.ToString();
            var concernsInfo = RT.Service.Resolve<ConcernsController>().GetMyConcerns().FirstOrDefault(r => r.Name == title);
            if(btn.Foreground == Brushes.Orange )
            {
                if(concernsInfo==null)
                {
                    concernsInfo = new ConcernsInfo();
                    concernsInfo.GenerateId();
                    concernsInfo.Name = title;
                    concernsInfo.Type = title;
                    RF.Save(concernsInfo);
                }
            }
            else
            {
                if(concernsInfo!=null)
                {
                    concernsInfo.PersistenceStatus = PersistenceStatus.Deleted;
                    RF.Save(concernsInfo);
                }
            }
        }
    }
}
