using SIE.Domain;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.MES.Workbench.Fpy;
using SIE.ObjectModel;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.Workbench.Helper;
using SIE.Wpf.Themes;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SIE.Wpf.MES.Workbench.Fpy
{
    /// <summary>
    /// FpyControl.xaml 的交互逻辑
    /// </summary>
    [Category("高效作业")]
    public partial class FpyWorkBenchControl : ComponentItem
    {
        private const string V100 = "100";

        /// <summary>
        /// 实际比率
        /// </summary>
        private double currentRate;
        /// <summary>
        /// 预警值
        /// </summary>
        private double warning = 0.7;
        /// <summary>
        /// 目标值
        /// </summary>
        private decimal targeting = 0.9m;
        /// <summary>
        /// 线别ID
        /// </summary>
        private double _lineId;
        /// <summary>
        /// 班次ID
        /// </summary>
        private double _shiftId;


        FpyWorkBenchProperty _p;
        /// <summary>
        /// 构造函数
        /// </summary>
        public FpyWorkBenchControl()
        {
            InitializeComponent();
            var input = UseInput<FpyInput>();
            input.PropertyChanged += Input_PropertyChanged;
            UseProperty<FpyWorkBenchProperty>();
            _p = UseProperty<FpyWorkBenchProperty>();
        }

        /// <summary>
        ///  输入数据更改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Input_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var se = sender as FpyInput;
            if (e.PropertyName == nameof(se.LineID))
                _lineId = se.LineID;
            if (e.PropertyName == nameof(se.ShiftId))
                _shiftId = se.ShiftId;
            LoadData();
        }

        void LoadData()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                SetLineData();
            }));
        }

        #region 直通率柱状图信息集合 FpyLineInfos
        /// <summary>
        ///直通率柱状图信息集合
        /// </summary>
        private ObservableCollection<FpyWorkBenchEntity> _fpyLineInfos;

        /// <summary>
        /// 直通率柱状图信息集合
        /// </summary>
        public ObservableCollection<FpyWorkBenchEntity> FpyLineInfos
        {
            get
            {
                if (_fpyLineInfos == null)
                {
                    _fpyLineInfos = new ObservableCollection<FpyWorkBenchEntity>();
                }

                return _fpyLineInfos;
            }
        }
        #endregion

        /// <summary>
        /// 修改班次的开始时间、结束时间的日期为当前日期
        /// </summary>
        /// <param name="shiftEntity">实体</param>
        /// <param name="date">日期</param>
        /// <returns>班次实体</returns>
        public Shift SetShiftDateNowVaue(Shift shiftEntity, DateTime date)
        {
            var shiftSpan = (shiftEntity.EndTime.Date - shiftEntity.BeginTime.Date).TotalDays; //考虑跨天 
            var shiftBeginHour = shiftEntity.BeginTime.Hour;
            var shiftBeginMinute = shiftEntity.BeginTime.Minute;
            var shiftBeginSecond = shiftEntity.BeginTime.Second;
            shiftEntity.BeginTime = new DateTime(date.Year, date.Month, date.Day, shiftBeginHour, shiftBeginMinute, shiftBeginSecond); 

            var shiftEndHour = shiftEntity.EndTime.Hour;
            var shiftEndMinute = shiftEntity.EndTime.Minute;
            var shiftEndSecond = shiftEntity.EndTime.Second;
            shiftEntity.EndTime = new DateTime(date.Year, date.Month, date.Day, shiftEndHour, shiftEndMinute, shiftEndSecond);
            shiftEntity.EndTime = shiftEntity.EndTime.AddDays(shiftSpan);

            return shiftEntity;
        }

        /// <summary>
        /// 获取数据
        /// </summary>
        protected void SetLineData()
        {
            FpyLineInfos.Clear();
            laEnt.Content = string.Empty;
            gridEnt.ToolTip = string.Empty;
            currentRate = 0;

            if (_lineId == 0 || _shiftId == 0) return;
            var resource = RF.GetById<WipResource>(_lineId);
            if (resource == null)
            {
                return;
            }
            laEnt.Content = resource.Name;

            //改为获取数据库时间
            var repository = resource.GetRepository() as EntityRepository;
            var date = repository.GetDbTime();

            //获取指定产线指定班次对应的时间段所有工序的直通率（按工序分组汇总）
            var list = RT.Service.Resolve<ResourceShiftFpyController>().GetResourceShiftFpyList(_lineId, _shiftId, date.Date, date.Date);
            if (list.Count == 0)
                return;

            gridEnt.ToolTip = laEnt.Content;

            //计算逻辑 -工序直通率柱状图
            decimal totalRate = 1;
            var result = list.GroupBy(p => p.ProcessId);
            result.ForEach(e =>
            {
                var processList = e.ToList();
                var fpy = processList.Select(p => p.Fpy).ToArray();
                decimal rate = 1;
                fpy.ForEach(f => { rate = rate * f; });
                totalRate = totalRate * rate;
                FpyLineInfos.Add(new FpyWorkBenchEntity() { FpyRate = Math.Round(rate, 2) * 100, ProcessName = processList[0].ProcessName });
            });
            LineChart.Diagram.Series[0].DataSource = FpyLineInfos;

            //计算逻辑 -预警值/期望值
            var lineFpy = RT.Service.Resolve<FpySettingController>().GetLineFpySettingsByLineId(_lineId);
            if (lineFpy != null)
            {
                warning = (double)(lineFpy.Alarm / 100);
                targeting = lineFpy.Desired;
            }
            else
            {
                warning = 1;
                targeting = 100;
            }

            currentRate = (double)Math.Round(totalRate, 2);
            string tooltips = "";
            tooltips = "当前比率：" + (currentRate * 100) + "%\r\n预警值" + (warning) + "%\r\n目标值" + (targeting) + " % ";
            img.ToolTip = tooltips;
            Draw(currentRate);

        }

        #region 仪表盘
        /// <summary>
        /// 获取坐标
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="sourcePoint"></param>
        /// <param name="angle"></param>
        /// <returns></returns>
        private Point GetOffsetPoint(Point centerPoint, Point sourcePoint, double angle)
        {
            double angleHude = -angle * Math.PI / 180;/*角度变成弧度*/
            Point offsetPoint = new Point(0, 0);
            offsetPoint.X = ((sourcePoint.X - centerPoint.X) * Math.Cos(angleHude) + (sourcePoint.Y - centerPoint.Y) * Math.Sin(angleHude) + centerPoint.X);
            offsetPoint.Y = (-(sourcePoint.X - centerPoint.X) * Math.Sin(angleHude) + (sourcePoint.Y - centerPoint.Y) * Math.Cos(angleHude) + centerPoint.Y);
            return offsetPoint;
        }

        /// <summary>
        /// 画图
        /// </summary>
        /// <param name="curr_rate"></param>
        private void Draw(double curr_rate)
        {
            // curr_rate = 1;
            img.Children.Clear();
            var r = Math.Min(img.ActualWidth, img.ActualHeight) / 2;
            r = r - 10;
            if (r < 20)
                return;

            var centerPoint = new Point(img.ActualWidth / 2, img.ActualHeight / 2);
            img.Children.Add(CreatePath(centerPoint, r, -60, 0, Color.FromRgb(207, 207, 207)));
            img.Children.Add(CreatePath(centerPoint, r, 0, 60, Color.FromRgb(207, 207, 207)));
            img.Children.Add(CreatePath(centerPoint, r, 60, 120, Color.FromRgb(207, 207, 207)));
            img.Children.Add(CreatePath(centerPoint, r, 120, 180, Color.FromRgb(207, 207, 207)));
            img.Children.Add(CreatePath(centerPoint, r, 180, 240, Color.FromRgb(207, 207, 207)));
            for (int i = -60; i <= 180; i = i + 60)
            {
                var round = CreatePath(centerPoint, r, i, i + 60, Color.FromRgb(207, 207, 207));
                img.Children.Add(round);
            }
            laPercentVal.FontSize = 60 * r / 110;
            gridEnt.Width = 80 * r / 110;
            gridEnt.Height = 30 * r / 110;

            int l = laEnt.Content.ToString().Length;
            if (l <= 6)
            {
                laEnt.FontSize = 14 * r / 110;
            }
            else
            {
                laEnt.FontSize = 12 * r / 110; 
            }

            Color c;
            if (curr_rate > warning)
            {
                //大于预警值，显示绿色
                c = Color.FromRgb(92, 182, 94);
                laPercentVal.Foreground = new SolidColorBrush(c);
            }
            else
            {
                //显示红色
                c = Color.FromRgb(227, 48, 67);
            }
            laPercentVal.Foreground = new SolidColorBrush(c);
            laPercentFH.Foreground = new SolidColorBrush(c);
            laPercentVal.Content = curr_rate * 100;
            double endAngle = 0;

            if (curr_rate > 0)
            {
                endAngle = curr_rate * 300;//设置终点

                if (endAngle <= 60)
                {
                    //-60°到0°
                    img.Children.Add(CreatePath(centerPoint, r, -60, endAngle - 60, c));
                }
                else if (endAngle >= 240)
                {
                    //240°到300°
                    img.Children.Add(CreatePath(centerPoint, r, -60, 0, c));
                    img.Children.Add(CreatePath(centerPoint, r, 0, 60, c));
                    img.Children.Add(CreatePath(centerPoint, r, 60, 120, c));
                    img.Children.Add(CreatePath(centerPoint, r, 120, 180, c));
                    img.Children.Add(CreatePath(centerPoint, r, 180, endAngle - 60, c));
                }
                else if (endAngle > 60 && endAngle <= 120)
                {
                    img.Children.Add(CreatePath(centerPoint, r, -60, 0, c));
                    img.Children.Add(CreatePath(centerPoint, r, 0, endAngle - 60, c));
                }
                else if (endAngle > 120 && endAngle <= 180)
                {
                    img.Children.Add(CreatePath(centerPoint, r, -60, 0, c));
                    img.Children.Add(CreatePath(centerPoint, r, 0, 60, c));
                    img.Children.Add(CreatePath(centerPoint, r, 60, endAngle - 60, c));
                }
                else
                {
                    //180°到240°                 
                    img.Children.Add(CreatePath(centerPoint, r, -60, 0, c));
                    img.Children.Add(CreatePath(centerPoint, r, 0, 60, c));
                    img.Children.Add(CreatePath(centerPoint, r, 60, 120, c));
                    img.Children.Add(CreatePath(centerPoint, r, 120, endAngle - 60, c));

                }
            }
            //每个块的大小需跟灰色的一样，红绿色的也是
            for (int i = -60; i <= 240; i = i + 60)
            {
                //img.Children.Add(CreatePath(centerPoint, r - 20, i, i + 60, Color.FromRgb(254, 254, 255)));
                var round = CreatePath(centerPoint, r - 20, i, i + 60, Color.FromRgb(207, 207, 207));
                img.Children.Add(round);
                round.SetResourceReference(Path.FillProperty, new BrushesThemeKeyExtension { ResourceKey = BrushesThemeKeys.Background });
                round.SetResourceReference(Path.StrokeProperty, new BrushesThemeKeyExtension { ResourceKey = BrushesThemeKeys.Background });
            }
            //再覆盖一层把衔接位置的线去掉
            for (int i = -60; i <= 240; i = i + 60)
            {
                //img.Children.Add(CreatePath(centerPoint, r - 20, i, i + 60, Color.FromRgb(254, 254, 255)));
                var round = CreatePath(centerPoint, r - 20, i - 5, i + 5, Color.FromRgb(207, 207, 207));
                img.Children.Add(round);
                round.SetResourceReference(Path.FillProperty, new BrushesThemeKeyExtension { ResourceKey = BrushesThemeKeys.Background });
                round.SetResourceReference(Path.StrokeProperty, new BrushesThemeKeyExtension { ResourceKey = BrushesThemeKeys.Background });
            }
            CreateValue(centerPoint, r);
        }

        /// <summary>
        /// 画刻度值
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="r"></param>
        private void CreateValue(Point centerPoint, double r)
        {

            TextBlock txtScale = new TextBlock();
            txtScale.Text = "25";
            txtScale.FontSize = 20;
            Canvas.SetLeft(txtScale, centerPoint.X - r + 25);
            Canvas.SetTop(txtScale, centerPoint.Y - 20);
            this.img.Children.Add(txtScale);


            txtScale = new TextBlock();
            txtScale.Text = "50";
            txtScale.FontSize = 20;
            Canvas.SetLeft(txtScale, centerPoint.X - 10);
            Canvas.SetTop(txtScale, centerPoint.Y - r + 20);
            this.img.Children.Add(txtScale);


            txtScale = new TextBlock();
            txtScale.Text = "75";
            txtScale.FontSize = 20;
            Canvas.SetLeft(txtScale, (centerPoint.X + r - 50));
            Canvas.SetTop(txtScale, centerPoint.Y - 20);
            this.img.Children.Add(txtScale);

            txtScale = new TextBlock();
            txtScale.Text = "0";
            txtScale.FontSize = 20;

            Point startPoint = GetOffsetPoint(centerPoint, new Point(centerPoint.X - r + 30, centerPoint.Y - 20), -61);
            Canvas.SetLeft(txtScale, startPoint.X + 5);
            Canvas.SetTop(txtScale, startPoint.Y - 10);
            this.img.Children.Add(txtScale);

            txtScale = new TextBlock();
            txtScale.Text = V100;
            txtScale.FontSize = 20;
            startPoint = GetOffsetPoint(centerPoint, new Point((centerPoint.X + r - 30), centerPoint.Y - 20), 60);
            Canvas.SetLeft(txtScale, startPoint.X - 40);
            Canvas.SetTop(txtScale, startPoint.Y - 10);
            this.img.Children.Add(txtScale);




        }


        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="centerPoint"></param>
        /// <param name="r"></param>
        /// <param name="angle1"></param>
        /// <param name="angle2"></param>
        /// <param name="fillColor"></param>
        /// <returns></returns>
        private Path CreatePath(Point centerPoint, double r, double angle1, double angle2, Color fillColor)
        {
            Point p1 = new Point(centerPoint.X - r, centerPoint.Y);
            Point p2 = new Point(centerPoint.X - r, centerPoint.Y);

            p1 = GetOffsetPoint(centerPoint, p1, angle1);
            p2 = GetOffsetPoint(centerPoint, p2, angle2);

            Path path = new Path();
            path.Stroke = new SolidColorBrush(fillColor);
            path.StrokeThickness = 1;
            path.Fill = new SolidColorBrush(fillColor);
            //img.Children.Add(path);

            PathGeometry pathGeometry = new PathGeometry();
            path.Data = pathGeometry;

            PathFigure pathFigure = new PathFigure();
            pathFigure.StartPoint = new Point(centerPoint.X, centerPoint.Y);
            pathGeometry.Figures.Add(pathFigure);

            LineSegment lineSegment = new LineSegment();
            lineSegment.Point = new Point(p1.X, p1.Y);
            pathFigure.Segments.Add(lineSegment);

            ArcSegment arcSegment = new ArcSegment();
            arcSegment.Size = new Size(r, r);
            arcSegment.Point = new Point(p2.X, p2.Y);
            arcSegment.SweepDirection = SweepDirection.Clockwise;
            pathFigure.Segments.Add(arcSegment);

            lineSegment = new LineSegment();
            lineSegment.Point = new Point(centerPoint.X, centerPoint.Y);
            pathFigure.Segments.Add(lineSegment);


            return path;

        }

        private void img_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw(currentRate);
        }

        #endregion

        /// <summary>
        /// 仪表盘点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PieGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieGrid.Visibility = Visibility.Collapsed;
            LineChart.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 柱状图点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chartGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            PieGrid.Visibility = Visibility.Visible;
            LineChart.Visibility = Visibility.Collapsed;
        }

        private DispatcherTimer _timer;

        protected override void OnRun()
        {
            base.OnRun();
            TimerIni();
            LoadResourceShift();
        }
        void LoadResourceShift()
        {
            var resourceShift = SettingsHelper.GetResourceShift();
            _lineId = resourceShift.ResourceId;
            _shiftId = resourceShift.ShiftId;
            LoadData();
        }

        /// <summary>
        /// 关闭窗口事件
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            _timer?.Stop();
            _timer = null;
        }
        private DispatcherTimer timer;
        /// <summary>
        /// Timeer计时器初始化
        /// </summary>
        private void TimerIni()
        {
            if (timer == null)
                timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMinutes(_p.TimeSpan <= 0 ? 3 : _p.TimeSpan);
            timer.IsEnabled = true;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
            {
                LoadData();
            });
        }
    }

    /// <summary>
    /// 柱状图实体 
    /// </summary>
    public class FpyWorkBenchEntity : ObservableObject
    {
        /// <summary>
        /// 直通率
        /// </summary>
        public decimal FpyRate
        {
            get { return GetProperty<decimal>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class FpyInput : ComponentInput<FpyWorkBenchControl>
    {
        /// <summary>
        /// 产线ID
        /// </summary>
        [DisplayName("产线ID")]
        [Description("员工管理组件选择产线后输入")]
        public virtual double LineID { get; set; }

        /// <summary>
        /// 班次ID
        /// </summary>
        [DisplayName("班次ID")]
        [Description("员工管理组件选择班次后输入")]
        public virtual double ShiftId { get; set; }
    }

    /// <summary>
    /// 产线直通率属性
    /// </summary>
    public class FpyWorkBenchProperty : ComponentProperty<FpyWorkBenchControl>
    {
        /// <summary>
        /// 刷新时间（分钟）
        /// </summary>
        [DisplayName("刷新间隔(分钟)"), CategoryAttribute("自定义")]
        [Description("时间必须大于0,默认刷新时间为3分钟")]
        public double TimeSpan { get; set; }
    }
}
