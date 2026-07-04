using SIE.Domain;
using SIE.MES.Statistics.Entities;
using SIE.MES.Statistics.WIP;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using SIE.WorkBenchChartBase;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.Themes;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SIE.Wpf.MES.Workbench.CompletionRates
{
    /// <summary>
    /// 计划达成率 的交互逻辑
    /// </summary>
    [Category("目标管理")]
    [QuotaAttribute(_groupName, _title)]
    public partial class MonthPlanCompletionRateControl : ComponentItem
    {
        /// <summary>
        /// 指标分类
        /// </summary>
        private const string _groupName = "效率类";

        /// <summary>
        /// 指标名称
        /// </summary>
        private const string _title = "日均达成率";

        /// <summary>
        /// 完成率
        /// </summary>
        private double currentRate ;

        /// <summary>
        /// 指标率
        /// </summary>
        private double passRate = 0.80;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MonthPlanCompletionRateControl()
        {
            InitializeComponent();
            UseProperty<MonthPlanCompletionRateProperty>();
            MonthPlanCompletionRateControl owner = this;
            RT.EventBus.Subscribe<WorkShopChangedEvent>(owner, SubscribeHandle);
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public void LoadData()
        {
            double lastMonthReal = 80;
            double lastMonthTarget = 80;
            QuotaTargetDetail quotaTargetDetail = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaMonthTargetSetting(_groupName, _title, _dbTime.Value.Month, _workShopId);
            QuotaTargetDetail preQuotaTargetDetail = RT.Service.Resolve<QuotaTargetSettingController>().GetQuotaMonthTargetSetting(_groupName, _title, _dbTime.Value.Month - 1, _workShopId);
            var planWos = RT.Service.Resolve<WorkOrderController>().GetMonthWorkOrders(_workShopId, _dbTime.Value, 2);
            var effectIds = planWos.Select(p => p.Id).ToList();
            var finishWos = RT.Service.Resolve<WipStatisticsController>().GetMonthStatics(effectIds, _dbTime.Value, 2);
            ////取生产日期在当月，而且计划开始时间是当月的数据
            ////需求：当日完成总数：MES成品完工下线数量
            ////      当日计划总数：取计划开始时间在当前日期的工单计划数量之和           
            decimal currMonthTotalRate = GetTotalRate(_dbTime.Value, planWos, finishWos);
            var curMonth = DateTime.Parse(_dbTime.Value.Year + "-" + _dbTime.Value.Month + "-01");
            decimal lastMonthTotalRate = GetTotalRate(curMonth.AddDays(-1), planWos, finishWos);

            if (quotaTargetDetail != null)
            {
                // currentRate = double.Parse(quotaTargetDetail.Actual.ToString("F3"));
                currentRate = double.Parse(currMonthTotalRate.ToString("F3"));
                passRate = double.Parse(quotaTargetDetail.Target.ToString("F3"));
            }

            if (preQuotaTargetDetail != null)
            {
                // lastMonthReal = double.Parse(preQuotaTargetDetail.Actual.ToString("F3"));
                lastMonthReal = double.Parse(lastMonthTotalRate.ToString("F3"));
                lastMonthTarget = double.Parse(preQuotaTargetDetail.Target.ToString("F3"));
            }

            CompletionRatesEntity com = new CompletionRatesEntity();
            com.CurMonthReal = currentRate.ToString("0.#") + "%";
            com.CurMonthTarget = passRate.ToString("0.#") + "%";
            com.LastMonthReal = lastMonthReal.ToString("0.#") + "%";
            com.LastMonthTarget = lastMonthTarget.ToString("0.#") + "%";
            GridContent.DataContext = com;
            passRate = passRate / 100;
            currentRate = currentRate / 100;
            Refresh();
        }

        /// <summary>
        /// 获取月度每日达成率汇总
        /// </summary>
        /// <param name="dt">日期</param>
        /// <param name="planWos">计划数量工单集合</param>
        /// <param name="finishWos">完成数量数据集合</param>
        /// <returns>达成率</returns>
        private decimal GetTotalRate(DateTime dt, EntityList<WorkOrder> planWos, EntityList<WorkOrderStatistics> finishWos)
        {
            decimal totalRate = 0;
            for (int i = 0; i < dt.Day; i++)
            {
                var nowDate = dt.Date.AddDays(-i);
                ////当日完成总数
                var finishWoQty = finishWos.Where(p => p.CollectDate.Date == nowDate).Sum(p => p.QtyPass);
                if (finishWoQty == 0) continue;
                ////当日计划总数
                var planWoQty = planWos.Where(p => p.PlanBeginDate >= nowDate && p.PlanBeginDate < nowDate.AddDays(1)).Sum(p => p.PlanQty);
                if (planWoQty == 0) continue;
                totalRate += finishWoQty / planWoQty;
            }

            return totalRate;
        }

        /// <summary>
        /// 刷新仪表盘
        /// </summary>
        public override void Refresh()
        {
            string tooltips;
            tooltips = "本月目标：" + (passRate * 100).ToString("0.#") + "%\r\n";
            tooltips += "本月达成：" + (currentRate * 100).ToString("0.#") + "%\r\n";
            img.ToolTip = tooltips;
            laPercent.Content = (currentRate * 100).ToString("0.#") + "%";
            Draw(passRate, currentRate);
        }

        /// <summary>
        /// 画图获取点坐标
        /// </summary>
        /// <param name="centerPoint">中心点</param>
        /// <param name="sourcePoint">来源点</param>
        /// <param name="angle">角度</param>
        /// <returns>坐标点</returns>
        private Point GetOffsetPoint(Point centerPoint, Point sourcePoint, double angle)
        {
            double angleHude = -angle * Math.PI / 180;/*角度变成弧度*/
            Point offsetPoint = new Point(0, 0);
            offsetPoint.X = Convert.ToInt32((sourcePoint.X - centerPoint.X) * Math.Cos(angleHude) + (sourcePoint.Y - centerPoint.Y) * Math.Sin(angleHude) + centerPoint.X);
            offsetPoint.Y = Convert.ToInt32(-(sourcePoint.X - centerPoint.X) * Math.Sin(angleHude) + (sourcePoint.Y - centerPoint.Y) * Math.Cos(angleHude) + centerPoint.Y);
            return offsetPoint;
        }

        /// <summary>
        /// 画图
        /// </summary>
        /// <param name="pass_rate">目标率</param>
        /// <param name="rate">当前比率</param>
        private void Draw(double pass_rate, double rate)
        {
            var solidColorBrush = img.Background as SolidColorBrush;
            var underColor = solidColorBrush.Color;
            img.Children.Clear();
            var r = Math.Min(img.ActualWidth - 10, img.ActualHeight * 2 - 10) / 2;
            if (r < 20)
                return;
            var centerPoint = new Point(img.ActualWidth / 2, img.ActualHeight - 10);

            for (var i = 0; i <= 9; i++)
            {
                if (i < 2)
                    img.Children.Add(CreatePath(centerPoint, r, i * 18 + 1, (i + 1) * 18 - 1, Color.FromRgb(93, 183, 91), underColor));
                else if (i < 5)
                    img.Children.Add(CreatePath(centerPoint, r, i * 18 + 1, (i + 1) * 18 - 1, Color.FromRgb(255, 111, 0), underColor));
                else
                    img.Children.Add(CreatePath(centerPoint, r, i * 18 + 1, (i + 1) * 18 - 1, Colors.Red, underColor));
            }

            var round = CreatePath(centerPoint, r - 30, 0, 180, underColor, underColor);
            img.Children.Add(round);
            round.SetResourceReference(Path.FillProperty, new BrushesThemeKeyExtension { ResourceKey = BrushesThemeKeys.Background });
            double lineAngle = rate / (pass_rate / 0.8) * 180;
            lineAngle = Math.Min(181, lineAngle);
            Line line = new Line();
            Point startPoint = GetOffsetPoint(centerPoint, new Point(centerPoint.X, centerPoint.Y - 1), lineAngle);

            line.X1 = startPoint.X;
            line.Y1 = startPoint.Y;
            Point endPoint = GetOffsetPoint(centerPoint, new Point(centerPoint.X - r + 25, centerPoint.Y), lineAngle);
            line.X2 = endPoint.X;
            line.Y2 = endPoint.Y;
            line.Stroke = new SolidColorBrush(Colors.Gray);
            line.StrokeThickness = 2;
            img.Children.Add(line);

            line = new Line();
            startPoint = GetOffsetPoint(centerPoint, new Point(centerPoint.X, centerPoint.Y + 1), lineAngle);

            line.X1 = startPoint.X;
            line.Y1 = startPoint.Y;
            endPoint = GetOffsetPoint(centerPoint, new Point(centerPoint.X - r + 25, centerPoint.Y), lineAngle);
            line.X2 = endPoint.X;
            line.Y2 = endPoint.Y;
            line.Stroke = new SolidColorBrush(Colors.Gray);
            line.StrokeThickness = 2;
            img.Children.Add(line);

            Ellipse ellipse = new Ellipse();
            ellipse.Width = 9;
            ellipse.Height = 9;
            ellipse.Stroke = new SolidColorBrush(Colors.Gray);
            ellipse.SetValue(System.Windows.Controls.Canvas.LeftProperty, centerPoint.X - 4);
            ellipse.SetValue(System.Windows.Controls.Canvas.TopProperty, centerPoint.Y - 4);
            ellipse.Fill = new SolidColorBrush(Colors.Gray);
            img.Children.Add(ellipse);
        }

        /// <summary>
        /// 描线
        /// </summary>
        /// <param name="centerPoint">中心坐标点</param>
        /// <param name="r">半径</param>
        /// <param name="angle1">开始角度</param>
        /// <param name="angle2">结束角度</param>
        /// <param name="fillColor">填充颜色</param>
        /// <param name="borderColor">描边颜色</param>
        /// <returns>线</returns>
        private Path CreatePath(Point centerPoint, double r, double angle1, double angle2, Color fillColor, Color borderColor)
        {
            Point p1 = new Point(centerPoint.X - r, centerPoint.Y);
            Point p2 = new Point(centerPoint.X - r, centerPoint.Y);

            p1 = GetOffsetPoint(centerPoint, p1, angle1);
            p2 = GetOffsetPoint(centerPoint, p2, angle2);

            Path path = new Path();
            path.Stroke = new SolidColorBrush(borderColor);
            path.StrokeThickness = 1;
            path.Fill = new SolidColorBrush(fillColor);
            path.SetResourceReference(Path.StrokeProperty, new BrushesThemeKeyExtension { ResourceKey = BrushesThemeKeys.Background });

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

        /// <summary>
        /// 图大小改变事件
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void img_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Draw(passRate, currentRate);
        }

        #region 初始化车间、数据库时间
        /// <summary>
        /// 车间Id
        /// </summary>
        private double _workShopId;

        /// <summary>
        /// 订阅事件处理
        /// </summary>
        /// <param name="e">e</param>
        private void SubscribeHandle(WorkShopChangedEvent e)
        {
            _workShopId = e.WorkShopId;
            GetDbTime();
            if (_dbTime.HasValue)
                LoadData();
        }

        /// <summary>
        /// 数据库时间
        /// </summary>
        private DateTime? _dbTime;

        /// <summary>
        /// 获取数据库时间
        /// </summary>  
        private void GetDbTime()
        {
            var workShop = RF.GetById<Enterprise>(_workShopId);
            if (workShop == null) return;
            var repository = workShop.GetRepository() as EntityRepository;
            _dbTime = repository.GetDbTime();
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        protected override void OnClose()
        {
            RT.EventBus.Unsubscribe<WorkShopChangedEvent>(this);
            base.OnClose();
        }
        #endregion
    }

    /// <summary>
    /// 计划达成率实体和直通率实体
    /// </summary>
    public class CompletionRatesEntity : ObservableObject
    {
        /// <summary>
        /// 本月目标
        /// </summary>
        public string CurMonthTarget
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 本月实际
        /// </summary>
        public string CurMonthReal
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 上月目标
        /// </summary>
        public string LastMonthTarget
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 上月实际
        /// </summary>
        public string LastMonthReal
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }

    /// <summary>
    /// 计划达成率属性
    /// </summary>
    public class MonthPlanCompletionRateProperty : ComponentProperty<MonthPlanCompletionRateControl>
    {
    }
}