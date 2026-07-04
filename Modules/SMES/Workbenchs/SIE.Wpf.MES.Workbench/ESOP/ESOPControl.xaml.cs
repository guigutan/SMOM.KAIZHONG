using SIE.Wpf.Common.Diagram;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SIE.Wpf.MES.Workbench.ESOP
{
    /// <summary>
    /// ESOPControl.xaml 的交互逻辑
    /// </summary>
    [Category("在线文档")]
    public partial class ESOPControl : ComponentItem
    {
        /// <summary>
        /// ESOP组件通信输入
        /// </summary>
        private ESOPInput _input;

        /// <summary>
        /// 鼠标左键是否按下
        /// </summary>
        private bool mouseDown;

        /// <summary>
        /// 坐标点
        /// </summary>
        private Point mouseXY;

        /// <summary>
        /// 最小缩放倍数
        /// </summary>
        private double min = 1.0;

        /// <summary>
        /// 最大缩放倍数
        /// </summary>
        private double max = 3.0;

        /// <summary>
        /// 产品Id
        /// </summary>
        //private double _productId;

        /// <summary>
        /// ESOP构造函数
        /// </summary>
        public ESOPControl()
        {
            InitializeComponent();
            _input = UseInput<ESOPInput>();
            _input.PropertyChanged += ESOPInput_PropertyChanged;
            UseProperty<ESOPProperty>();
        }

        /// <summary>
        /// 组件通信的属性变更事件
        /// </summary>
        /// <param name="sender">属性变更事件的发送对象</param>
        /// <param name="e">事件参数</param>
        private void ESOPInput_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            GetComponentData();
            ShowESOP();
        }

        /// <summary>
        /// OnRun方法
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            GetComponentData();
            ShowESOP();
        }

        /// <summary>
        /// 获取组件通信的数据--产品Id
        /// </summary>
        private void GetComponentData()
        {
            //_productId = _input.ProductId;
        }

        /// <summary>
        /// 获取产品对应的ESOP图片
        /// </summary>
        private void ShowESOP()
        {
            SetViewSize();
            IMG.Source = GetESOPImage(); //new BitmapImage(new Uri(_esopImgPath, UriKind.Relative));
        }

        /// <summary>
        /// 获取产品对应的ESOP图片
        /// </summary>
        /// <returns>图片资源</returns>
        private BitmapImage GetESOPImage()
        {
            const BitmapImage bmg = null;
            //var esop = RT.Service.Resolve<ProductESOPController>().GetProductESOP(_productId);
            //if (esop != null)
            //{
            //    bmg = ByteToBitmapImage(esop.ESOP);
            //}

            return bmg;
        }

        /// <summary>
        /// SizeChanged事件
        /// </summary>
        /// <param name="sender">SizeChanged事件的发送对象</param>
        /// <param name="e">事件参数</param>
        private void ESOP_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SetViewSize();
        }

        /// <summary>
        /// DragEnter事件
        /// </summary>
        /// <param name="sender">DragEnter事件的发送对象</param>
        /// <param name="e">事件参数</param>
        private void ESOP_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effects = DragDropEffects.Link; //WinForm中为e.Effect = DragDropEffects.Link
            else e.Effects = DragDropEffects.None; //WinFrom中为e.Effect = DragDropEffects.None
        }

        /// <summary>
        /// Drop事件的发送对象
        /// </summary>
        /// <param name="sender">Drop事件的</param>
        /// <param name="e">事件参数</param>
        private void ESOP_Drop(object sender, DragEventArgs e)
        {
            string filename = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            this.IMG.Source = new BitmapImage(new Uri(filename));
        }

        /// <summary>
        /// 设置图片视图宽度、高度
        /// </summary>
        private void SetViewSize()
        {
            mainScrollv.Width = this.ActualWidth;
            mainScrollv.Height = this.ActualHeight;
        }

        /// <summary>
        /// 鼠标左键按下的处理事件
        /// </summary>
        /// <param name="sender">左键按下事件的发送对象</param>
        /// <param name="e">事件参数</param>
        private void ContentControl_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var img = sender as ContentControl;
            if (img == null)
            {
                return;
            }

            img.CaptureMouse();
            mouseDown = true;
            mouseXY = e.GetPosition(img);
        }

        /// <summary>
        /// 鼠标左键释放的处理事件
        /// </summary>
        /// <param name="sender">鼠标左键释放事件的发送对象</param>
        /// <param name="e">事件参数</param>
        private void ContentControl_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var img = sender as ContentControl;
            if (img == null)
            {
                return;
            }

            img.ReleaseMouseCapture();
            mouseDown = false;
        }

        /// <summary>
        /// 鼠标再控件上方移动时的处理事件
        /// </summary>
        /// <param name="sender">鼠标移动处理事件发送对象</param>
        /// <param name="e">事件参数</param>
        private void ContentControl_MouseMove(object sender, MouseEventArgs e)
        {
            var img = sender as ContentControl;
            if (img == null)
            {
                return;
            }

            if (mouseDown)
            {
                Domousemove(img, e);
            }
        }

        /// <summary>
        /// 鼠标移动后的XY坐标
        /// </summary>
        /// <param name="img">图片的坐标</param>
        /// <param name="e">鼠标事件参数</param>
        private void Domousemove(ContentControl img, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var group = IMG.FindResource("TfGroup") as TransformGroup;
            var transform = group.Children[1] as TranslateTransform;
            var position = e.GetPosition(img);
            transform.X -= mouseXY.X - position.X;
            transform.Y -= mouseXY.Y - position.Y;
            mouseXY = position;
        }

        /// <summary>
        /// 旋转鼠标轮时的处理事件
        /// </summary>
        /// <param name="sender">处理事件的发送对象</param>
        /// <param name="e">事件参数</param>
        private void ContentControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var img = sender as ContentControl;
            if (img == null)
            {
                return;
            }

            var point = e.GetPosition(img);
            var group = IMG.FindResource("TfGroup") as TransformGroup;
            var delta = e.Delta * 0.001;
            DowheelZoom(group, point, delta);
        }

        /// <summary>
        /// 旋转鼠标轮后的XY坐标
        /// </summary>
        /// <param name="group">旋转鼠标轮后的变换组</param>
        /// <param name="point">坐标点</param>
        /// <param name="delta">XY坐标偏移量</param>
        private void DowheelZoom(TransformGroup group, Point point, double delta)
        {
            var pointToContent = group.Inverse.Transform(point);
            var transform = group.Children[0] as ScaleTransform;
            if (transform.ScaleX + delta < min) return;
            if (transform.ScaleX + delta > max) return;
            transform.ScaleX += delta;
            transform.ScaleY += delta;
            var transform1 = group.Children[1] as TranslateTransform;
            transform1.X = -1 * ((pointToContent.X * transform.ScaleX) - point.X);
            transform1.Y = -1 * ((pointToContent.Y * transform.ScaleY) - point.Y);
        }

        /*/// <summary>
        /// 打开图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OpenImg_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Files (*.png)|*.png|Files(*.jpg)|*.jpg";
            if (dialog.ShowDialog() == true)
            {
                //MessageBox.Show(dialog.FileName);
                this.IMG.Source = new BitmapImage(new Uri(dialog.FileName));
            }
        }*/

        /*/// <summary>
        /// 最小缩放倍数变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMinSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.min = double.Parse(txtMinSize.Text);
        }

        /// <summary>
        /// 最大缩放倍数变更事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMaxSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.max = double.Parse(txtMaxSize.Text);
        }*/
    }

    /// <summary>
    /// ESOP属性
    /// </summary>
    public class ESOPProperty : ComponentProperty<ESOPControl>
    {
    }
}
