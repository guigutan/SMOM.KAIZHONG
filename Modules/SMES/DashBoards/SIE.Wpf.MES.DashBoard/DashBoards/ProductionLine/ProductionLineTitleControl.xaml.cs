using SIE.Domain;
using SIE.MES.DashBoard.DashBoards.ProductionLine;
using SIE.Resources.WipResources;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.DashBoard.DashBoards.Editors;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SIE.Wpf.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// ProductionLineTitleControl.xaml 的交互逻辑
    /// </summary>
    [Category("生产")]
    public partial class ProductionLineTitleControl : ComponentItem
    {
        /// <summary>
        /// 车间名称
        /// </summary>
        private string _workShopName;

        /// <summary>
        /// 车间名称
        /// </summary>
        public string WorkShopName
        {
            get { return _workShopName; }
            set { _workShopName = value; }
        }

        /// <summary>
        /// 产线名字
        /// </summary>
        private string _lineName;

        /// <summary>
        /// 产线名字
        /// </summary>
        public string LineName
        {
            get { return _lineName; }
            set { _lineName = value; }
        }

        /// <summary>
        /// 标题名字
        /// </summary>
        private string _titleName;

        /// <summary>
        /// 标题名字
        /// </summary>
        public string Title
        {
            get { return _titleName; }
            set { _titleName = value; }
        }

        /// <summary>
        /// 时间刷新器
        /// </summary>
        DispatcherTimer _timer = new DispatcherTimer();

        /// <summary>
        /// 标题属性设置
        /// </summary>
        TitleItemProperty _property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ProductionLineTitleControl()
        {
            InitializeComponent();
            _property = this.UseProperty<TitleItemProperty>();
            ////_property = new TitleItemProperty();
            ////ComponentProperty = _property;
            ////ComponentProperty.Attach(this);

            _timer.Tick += new EventHandler(Timer_Tick);
            _timer.Interval = TimeSpan.FromSeconds(1); //设置刷新的间隔时间
            _timer.Start();
        }

        /// <summary>
        /// 运行时事件
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            BindingTitle();
        }

        /// <summary>
        /// 绑定标题
        /// </summary>
        public void BindingTitle()
        {
            var shopId = this._property.ShopAndLine.Shop;
            var lineIds = this._property.ShopAndLine.Lines?.Split(';');
            if (shopId != null)
            {
                var shop = RT.Service.Resolve<ProductionLineController>().GetEnterprisesById(shopId.Value);
                WorkShopName = shop?.Name;
                if (WorkShopName.IsNullOrEmpty())
                {
                    WorkShopName = "未配置车间";
                }
            }

            if (lineIds != null)
            {
                string tempLineId = lineIds[0];
                double lineId = 0;
                if (double.TryParse(tempLineId, out lineId) && lineId != 0)
                {
                    var lineEntity = RF.GetById<WipResource>(lineId);
                    LineName = lineEntity?.Name;
                    if (LineName.IsNullOrEmpty())
                    {
                        LineName = "未配置产线";
                    }
                }
            }
            Title = "{0} {1} 看板".L10nFormat(WorkShopName, LineName);
            boardTitle.Text = Title;
        }

        /// <summary>
        /// 异步刷新时间
        /// </summary>
        /// <param name="sender">时间对象</param>
        /// <param name="e">时间参数</param>
        void Timer_Tick(object sender, EventArgs e)
        {
            this.dateTime.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
        }
    }

    /// <summary>
    /// 标题区属性设置
    /// </summary>
    public class TitleItemProperty : ComponentProperty<ProductionLineTitleControl>
    {
        /// <summary>
        /// 标题内容
        /// </summary>
        [Category("标题设置"), DisplayName("标题内容"), Description("标题内容")]
        public string Content
        {
            get { return Item.boardTitle.Text; }
            set { Item.boardTitle.Text = value; }
        }

        /// <summary>
        /// 字体类型
        /// </summary>
        [Category("标题设置"), DisplayName("字体类型"), Description("字体类型")]
        public FontFamily FontFamily
        {
            get { return Item.boardTitle.FontFamily; }
            set { Item.boardTitle.FontFamily = value; }
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        [Category("标题设置"), DisplayName("字体大小"), Description("字体大小")]
        public double FontSize
        {
            get { return Item.boardTitle.FontSize; }
            set { Item.boardTitle.FontSize = value; }
        }

        /// <summary>
        /// 是否加粗
        /// </summary>
        [Category("标题设置"), DisplayName("是否加粗"), Description("是否加粗")]
        public FontWeight FontWeight
        {
            get { return Item.boardTitle.FontWeight; }
            set { Item.boardTitle.FontWeight = value; }
        }

        /// <summary>
        /// 字体颜色
        /// </summary>
        [Category("标题设置"), DisplayName("字体颜色"), Description("字体颜色")]
        public Color Foreground
        {
            get { return Item.boardTitle.Foreground is SolidColorBrush ? ((SolidColorBrush)Item.boardTitle.Foreground).Color : (Color)ColorConverter.ConvertFromString("#FFFFFF"); }
            set { Item.boardTitle.Foreground = new SolidColorBrush(value); }
        }

        /// <summary>
        /// 时间字体类型
        /// </summary>
        [Category("标题设置"), DisplayName("时间字体类型"), Description("时间字体类型")]
        public FontFamily TimeFontFamily
        {
            get { return Item.boardTitle.FontFamily; }
            set { Item.boardTitle.FontFamily = value; }
        }

        /// <summary>
        /// 时间字体大小
        /// </summary>
        [Category("标题设置"), DisplayName("时间字体大小"), Description("时间字体大小")]
        public double TimeFontSize
        {
            get { return Item.dateTime.FontSize; }
            set { Item.dateTime.FontSize = value; }
        }

        /// <summary>
        /// 时间字体加粗
        /// </summary>
        [Category("标题设置"), DisplayName("时间字体加粗"), Description("时间字体加粗")]
        public FontWeight TimeFontWeight
        {
            get { return Item.dateTime.FontWeight; }
            set { Item.dateTime.FontWeight = value; }
        }

        /// <summary>
        /// 时间字体颜色
        /// </summary>
        [Category("标题设置"), DisplayName("时间字体颜色"), Description("时间字体颜色")]
        public Color TimeForeground
        {
            get { return Item.dateTime.Foreground is SolidColorBrush ? ((SolidColorBrush)Item.dateTime.Foreground).Color : (Color)ColorConverter.ConvertFromString("#CCCCCC"); }
            set { Item.dateTime.Foreground = new SolidColorBrush(value); }
        }

        /// <summary>
        /// 车间产线联动对象
        /// </summary>
        ShopAndLine _shopAndLine = new ShopAndLine();

        /// <summary>
        /// 车间过滤
        /// </summary>
        [Category("数据过滤"), DisplayName("车间产线"), Description("车间产线"), PropertyEditor(typeof(DashBoardShopToLineLookupEdit))]
        public ShopAndLine ShopAndLine
        {
            get
            {
                return _shopAndLine;
            }

            set
            {
                _shopAndLine = value;
            }
        }

        /// <summary>
        /// 图片
        /// </summary>
        string _source;

        /// <summary>
        /// 图片
        /// </summary>
        [Category("图片"), DisplayName("选择Logo"), Description("选择Logo"), PropertyEditor(typeof(TitleImageEdit))]
        public string Source
        {
            get
            {
                return _source;
            }

            set
            {
                _source = value;
                LoadImage();
            }
        }

        /// <summary>
        /// 加载图片
        /// </summary>
        void LoadImage()
        {
            try
            {
                if (_source.IsNotEmpty())
                {
                    byte[] imageBytes = Convert.FromBase64String(_source);
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.StreamSource = new MemoryStream(imageBytes);
                    bitmap.EndInit();
                    Item.leftTitleImage.Source = bitmap;
                }
                else
                {
                    Item.leftTitleImage.Source = null;
                }
            }
            catch (Exception)
            {
                Item.leftTitleImage.Source = null;
            }
        }
    }
}
