using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace SIE.Wpf.MES.Wip.Controls
{
    /// <summary>
    /// 图片控件
    /// </summary>
    [TemplatePart(Name = ElementImageMain, Type = typeof(Image))]
    [TemplatePart(Name = ElementContentControlMain, Type = typeof(ContentControl))]
    public class ImageFrameCtrl : Control
    {
        /// <summary>
        /// 图片名称
        /// </summary>
        public const string ElementImageMain = "IMG1";

        /// <summary>
        /// 内容控件名称
        /// </summary>
        public const string ElementContentControlMain = "ContentControlMain";

        /// <summary>
        /// 图片
        /// </summary>
        Image _image;

        /// <summary>
        /// 内容控件
        /// </summary>
        ContentControl _contentControl;

        /// <summary>
        /// 构造函数
        /// </summary>
        static ImageFrameCtrl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ImageFrameCtrl), new FrameworkPropertyMetadata(typeof(ImageFrameCtrl)));
        }

        /// <summary>
        /// 是否鼠标按下
        /// </summary>
        private bool mouseDown;

        /// <summary>
        /// 鼠标位置
        /// </summary>
        private Point mouseXY;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ImageFrameCtrl()
        {
            Loaded += (s, e) =>
            {
                if (_contentControl != null)
                {
                    _contentControl.MouseLeftButtonDown -= ContentControl_MouseLeftButtonDown;
                    _contentControl.MouseLeftButtonUp -= ContentControl_MouseLeftButtonUp;
                    _contentControl.MouseMove -= ContentControl_MouseMove;
                    _contentControl.MouseWheel -= ContentControl_MouseWheel;
                }

                _image = GetTemplateChild(ElementImageMain) as Image;
                if (_image != null)
                {
                    _image.RenderTransform = CreateTransformGroup();
                }
                _contentControl = GetTemplateChild(ElementContentControlMain) as ContentControl;
                if (_contentControl != null)
                {
                    _contentControl.MouseLeftButtonDown += ContentControl_MouseLeftButtonDown;
                    _contentControl.MouseLeftButtonUp += ContentControl_MouseLeftButtonUp;
                    _contentControl.MouseMove += ContentControl_MouseMove;
                    _contentControl.MouseWheel += ContentControl_MouseWheel;
                }
            };
        }

        /// <summary>
        /// 创建平面转换组
        /// </summary>
        /// <returns>平面转换</returns>
        private Transform CreateTransformGroup()
        {
            TransformGroup transformGroup = new TransformGroup();
            ScaleTransform scaleTransform = new ScaleTransform();
            TranslateTransform translateTransform = new TranslateTransform();
            transformGroup.Children.Add(scaleTransform);
            transformGroup.Children.Add(translateTransform);
            return transformGroup;
        }

        #region 图片数据源 VideoImageSource
        /// <summary>
        /// 图片数据源
        /// </summary>
        public readonly static DependencyProperty VideoImageSourceProperty = DependencyProperty.Register(nameof(VideoImageSource), typeof(ImageSource), typeof(ImageFrameCtrl));

        /// <summary>
        /// 图片数据源
        /// </summary>
        public ImageSource VideoImageSource
        {
            get => (ImageSource)GetValue(VideoImageSourceProperty);
            set => SetValue(VideoImageSourceProperty, value);

        }
        #endregion

        #region Event 
        /// <summary>
        /// 鼠标按下时的事件，启用捕获鼠标位置并把坐标赋值给mouseXY.
        /// </summary>
        /// <param name="sender">内容控件</param>
        /// <param name="e">参数</param>
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
        /// 鼠标松开时的事件，停止捕获鼠标位置。
        /// </summary>
        /// <param name="sender">内容控件</param>
        /// <param name="e">参数</param>
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
        /// 鼠标移动时的事件，当鼠标按下并移动时发生Domousemove(img, e);函数
        /// </summary>
        /// <param name="sender">内容控件</param>
        /// <param name="e">参数</param>
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
        /// group.Children中的第二个是移动的函数
        /// 它根据X.Y的值来移动。并把当前鼠标位置赋值给mouseXY.
        /// </summary>
        /// <param name="img">内容控件</param>
        /// <param name="e">参数</param>
        private void Domousemove(ContentControl img, MouseEventArgs e)
        {
            if (e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            var group = _image.RenderTransform as TransformGroup;
            var transform = group.Children[1] as TranslateTransform;
            var scaleTransform = group.Children[0] as ScaleTransform;
            var position = e.GetPosition(img);
            transform.X -= (mouseXY.X - position.X);
            transform.Y -= (mouseXY.Y - position.Y);
            var transformArea = new Point(-img.ActualWidth * (scaleTransform.ScaleX - 1), -img.ActualHeight * (scaleTransform.ScaleY - 1));
            //往右与往下移动禁止超出
            if (mouseXY.X - position.X < 0 && transform.X > 0)
                transform.X = 0;
            if (mouseXY.Y - position.Y < 0 && transform.Y > 0)
                transform.Y = 0;
            // 往左与往上移动禁止超出
            if (mouseXY.X - position.X > 0 && transform.X < transformArea.X)
                transform.X = transformArea.X;
            if (mouseXY.Y - position.Y > 0 && transform.Y < transformArea.Y)
                transform.Y = transformArea.Y;

            mouseXY = position;

        }

        /// <summary>
        /// 鼠标滑轮事件，得到坐标，放缩函数和滑轮指数，由于滑轮值变化较大所以*0.001
        /// </summary>
        /// <param name="sender">内容控件</param>
        /// <param name="e">参数</param>
        private void ContentControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            var img = sender as ContentControl;
            if (img == null)
            {
                return;
            }
            var point = e.GetPosition(img);
            var group = _image.RenderTransform as TransformGroup;
            var delta = e.Delta * 0.001;
            DowheelZoom(group, point, delta);
        }

        /// <summary>
        /// Group.Children中的第一个是放缩函数。
        /// 如果ScaleX+滑轮指数小于0.1时就返回。
        /// var pointToContent = group.Inverse.Transform(point);
        /// 获取此变换的逆变换的值
        /// 使图片放缩后，放缩原点也随之变化。
        /// </summary>
        /// <param name="group">平面转换组</param>
        /// <param name="point">鼠标位置</param>
        /// <param name="delta">缩放比例</param>
        private void DowheelZoom(TransformGroup group, Point point, double delta)
        {
            var pointToContent = group.Inverse.Transform(point);
            var scaleTransform = group.Children[0] as ScaleTransform;
            if (scaleTransform.ScaleX + delta < 0.1) return;
            scaleTransform.ScaleX += delta;
            scaleTransform.ScaleY += delta;

            if (scaleTransform.ScaleX < 1)
                scaleTransform.ScaleX = 1;
            if (scaleTransform.ScaleY < 1)
                scaleTransform.ScaleY = 1;

            var transform1 = group.Children[1] as TranslateTransform;
            transform1.X = -1 * ((pointToContent.X * scaleTransform.ScaleX) - point.X);
            transform1.Y = -1 * ((pointToContent.Y * scaleTransform.ScaleY) - point.Y);
            if (transform1.X > 0)
                transform1.X = 0;
            if (transform1.Y > 0)
                transform1.Y = 0;


        }
        #endregion
    }
}
