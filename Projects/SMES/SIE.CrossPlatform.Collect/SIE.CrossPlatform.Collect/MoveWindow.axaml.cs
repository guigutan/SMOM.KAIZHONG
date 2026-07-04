using Avalonia.Controls;
using SIE.CrossPlatform.Collect.Common.Controls;

namespace SIE.CrossPlatform.Collect
{
    public partial class MoveWindow : Window
    {
        public MoveWindow()
        {
            InitializeComponent();
            this.xpTitle.ATitle = this.Title = "¹ýÕ¾²É¼¯";
            this.WindowState = WindowState.Maximized;
            this.xpTitle.AType = EnumXPTitleType.WorkerCell;
        }

        private void Window_Loaded(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            this.xpTitle.FormMain = this.Owner as Window;
            this.Owner.Hide();
            this.Show();
        }
    }
}