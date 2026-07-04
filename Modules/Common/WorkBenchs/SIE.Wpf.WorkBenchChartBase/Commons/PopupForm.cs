using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.WorkBenchChartBase.Commons
{
    /// <summary>
    /// 弹框表单
    /// </summary>
    public class PopupForm:Window
    {
        /// <summary>
        /// Items
        /// </summary>
        public List<PopupItem> Items { get; set; } = new List<PopupItem>();

        /// <summary>
        /// 构造函数
        /// </summary>
        public PopupForm()
        {
            this.Loaded += OtherInfoForm_Loaded;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            this.Width = 930;
            this.Height = 750;
        }

        private void OtherInfoForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (Items.Count > 2)
            {
                this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            CreateItems();
        }

        private void CreateItems()
        {
            Grid main_content = new Grid();
            Grid grid = new Grid();
            this.Content = main_content;

            ScrollViewer content_view = new ScrollViewer();
            content_view.SetValue(Grid.RowProperty, 1);
            content_view.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            main_content.Children.Add(content_view);  

            content_view.Content = grid;
            for (var i=0;i<2;i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(650);
                grid.ColumnDefinitions.Add(col);
            }
             
            for(var i=0;i<Items.Count;i++)
            {
                if(i%2==0)
                {
                    RowDefinition row = new RowDefinition();
                    grid.RowDefinitions.Add(row);
                    row.Height = new GridLength(600);
                }
                grid.Children.Add(Items[i]);
                Items[i].SetValue(Grid.ColumnProperty, i % 2);
                Items[i].SetValue(Grid.RowProperty, i /2);
            }
        }
    }
}
