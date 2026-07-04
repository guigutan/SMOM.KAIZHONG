using DevExpress.Xpf.Layout.Core;
using SIE.Domain;
using SIE.View;
using SIE.View.Workbench;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SIE.Wpf.MES.TouchScreenHomepage
{
    /// <summary>
    /// AndonButtonControl.xaml 的交互逻辑
    /// </summary>
    public partial class MenuControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public MenuControl()
        {
            InitializeComponent();

            MenuList = new ObservableCollection<SIE.Rbac.Menus.Menu>();
            ctlMenu.ItemsSource = MenuList;
            allScrollViewer.Background = new ImageBrush
            {
                ImageSource = new BitmapImage(new Uri("pack://application:,,,/SIE.Wpf.MES;component/Resources/Homepage.png"))
            };

        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual ObservableCollection<SIE.Rbac.Menus.Menu> MenuList { get; }

        /// <summary>
        /// 菜单列表
        /// </summary>
        public EntityList<SIE.Rbac.Menus.Menu> menus
        {
            get { return (EntityList<SIE.Rbac.Menus.Menu>)GetValue(MenusProperty); }
            set { SetValue(MenusProperty, value); }
        }

        /// <summary>
        /// 菜单列表依赖属性
        /// </summary>
        public static readonly DependencyProperty MenusProperty =
            DependencyProperty.Register("Menus", typeof(EntityList<SIE.Rbac.Menus.Menu>), typeof(MenuControl), new PropertyMetadata(new EntityList<SIE.Rbac.Menus.Menu>(),
                (s, e) => { ((MenuControl)s).OnValueChanged(e); }));

        /// <summary>
        /// 值变更
        /// </summary>
        /// <param name="e">e</param>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldvalue = e.OldValue as EntityList<SIE.Rbac.Menus.Menu>;
            if (oldvalue != null)
            {
                oldvalue.CollectionChanged -= Menus_CollectionChanged;
            }

            var newvalue = e.NewValue as EntityList<SIE.Rbac.Menus.Menu>;
            if (newvalue != null)
            {
                Clear();
                newvalue.CollectionChanged += Menus_CollectionChanged;
            }
        }
        private void Menus_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (Rbac.Menus.Menu menu in e.NewItems)
                {
                    this.MenuList.Add(menu);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (Rbac.Menus.Menu menu in e.OldItems)
                {
                    this.MenuList.Remove(menu);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Clear();
                if (e.NewItems != null)
                {
                    foreach (Rbac.Menus.Menu menu in e.NewItems)
                    {
                        this.MenuList.Add(menu);
                    }
                }
            }
        }
        /// <summary>
        /// 清空内存数据
        /// </summary>
        void Clear()
        {
            MenuList.Clear();
        }


        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            var tagString = btn.Tag.ToString();
            if (tagString.IsNullOrEmpty())
            {
                return;
            }

            var clickMenu = this.MenuList.FirstOrDefault(m => m.Id.ToString() == tagString);
            //打开菜单
            if (clickMenu != null)
            {
                CRT.Service.Resolve<IMenuService>().OpenModule(clickMenu.ModuleKey);
            }
        }
    }
}