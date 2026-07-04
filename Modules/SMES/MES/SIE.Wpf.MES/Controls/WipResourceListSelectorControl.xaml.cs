
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SIE.Wpf.MES.Controls
{
    /// <summary>
    /// WipResourceListSelectorControl.xaml 的交互逻辑
    /// </summary>
    public partial class WipResourceListSelectorControl : UserControl
    {
        /// <summary>
        /// WipResourceListSelectorControl.xaml.cs
        /// </summary>
        public WipResourceListSelectorControl()
        {
            InitializeComponent();
            LeftResources = new ObservableCollection<BWipResource>();
            RightResources = new ObservableCollection<BWipResource>();
            DataContext = this; // 绑定自己
        }


        /// <summary>
        /// 员工ID
        /// </summary>
        public double EmployeeId { get; set; }

        /// <summary>
        /// 员工姓名
        /// </summary>
        public string EmployeeName { get; set; }

        /// <summary>
        /// 是否属于上岗
        /// </summary>
        public bool IsOnDuty { get; set; }

        /// <summary>
        /// 上下岗类型名称
        /// </summary>
        public string DutyStypeName { get; set; }

        /// <summary>
        /// 窗体标题
        /// </summary>
        public string TileName { get; set; }

        /// <summary>
        /// 左边列表数据源（您拥有的资源）
        /// </summary>
        public ObservableCollection<BWipResource> LeftResources { get; set; }
      


        /// <summary>
        /// 右边列表数据源（已选择的资源）
        /// </summary>
        public ObservableCollection<BWipResource> RightResources { get; set; }
      



        #region 箭头按钮移动逻辑

        /// <summary>
        /// 单个右移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveRight_Click(object sender, RoutedEventArgs e)
        {
            if (LeftList.SelectedItem is BWipResource item)
            {
                LeftResources.Remove(item);
                RightResources.Add(item);
            }
        }
        /// <summary>
        /// 全部右移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveAllRight_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in LeftResources.ToArray())
            {
                RightResources.Add(item);
            }
            LeftResources.Clear();
        }





        /// <summary>
        ///  单个左移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (RightList.SelectedItem is BWipResource item)
            {
                RightResources.Remove(item);
                LeftResources.Add(item);
            }
        }

        /// <summary>
        /// 全部左移
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveAllLeft_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in RightResources.ToArray())
            {
                LeftResources.Add(item);
            }
            RightResources.Clear();
        }


        #endregion





        /// <summary>
        /// 辅助方法：获取父容器
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="child"></param>
        /// <returns></returns>
        public static T GetVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            while (parent != null)
            {
                if (parent is T t) return t;
                parent = VisualTreeHelper.GetParent(parent);
            }
            return null;
        }



        /// <summary>
        /// 对话框结果
        /// </summary>
        public bool Result { get; set; } = false;

        /// <summary>
        /// 确定按钮 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            //var window = this.GetVisualParent<Window>();
            //if (window != null)
            //{               
            //    this.Result = true;
            //    window.Close();
            //}

            var window = GetVisualParent<Window>(this);
            if (window != null)
            {
                Result = true;
                window.DialogResult = true;
                window.Close();
            }

        }

        /// <summary>
        /// 关闭按钮 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //var window = this.GetVisualParent<Window>();
            //if (window != null)
            //{                
            //    this.Result = false;
            //    window.DialogResult = false;
            //    window.Close();
            //}

            var window = GetVisualParent<Window>(this);
            if (window != null)
            {
                Result = false;
                window.DialogResult = false;
                window.Close();
            }

        }



    }


    /// <summary>
    /// B版WipResource
    /// </summary>
    public class BWipResource
    {
        /// <summary>
        /// 资源ID
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 在岗状态( 在岗中... /  空字符 )
        /// </summary>
        public string OnDutyStatusName { get; set; }

    }



}
