using SIE.Domain.Validation;
using SIE.Tech.Processs;
using SIE.Tech.Routings.Technologys;
using SIE.Wpf.Tech.Routings.Technologys.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace SIE.Wpf.Tech.Routings.Technologys.Controls
{
    /// <summary>
    /// Container.xaml 的交互逻辑
    /// </summary>
    public partial class Container : UserControl
    {
        /// <summary>
        /// 工艺是否只读
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// 设计容器接口
        /// </summary>
        public IContainer Model { get; private set; }

        /// <summary>
        /// 子元素集合
        /// </summary>
        public UIElementCollection Children { get { return cnsDesignerContainer.Children; } }

        /// <summary>
        /// 复制粘贴的Layoyut字符串
        /// </summary>
        private string CopyPasteXml { get; set; }

        /// <summary>
        /// 粘贴时，复制操作是否执行成功。
        /// </summary>
        private bool IsCopyFinished { get; set; }

        /// <summary>
        /// 画布
        /// </summary>
        Gridding GridLines { get; set; }

        /// <summary>
        /// 矩形选中框
        /// </summary>
        Rectangle tempRectangle;

        /// <summary>
        /// 规则
        /// </summary>
        public Rule TempRule { get; set; }

        /// <summary>
        /// 工艺流程版本复制选项
        /// </summary>
        public RoutingVersionCopyViewModel CopyViewModel { get; set; }

        /// <summary>
        /// 模型变更事件
        /// </summary>
        public event Action<IContainer> ModelChanged;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Container()
        {
            InitializeComponent();
            this.PreviewDrop += Container_PreviewDrop;
            InitModel(new ContainerModel());
            InitGridLines();
            _pathPoint = (Path)FindResource("path_Point");
            _storyboard = new Storyboard();
            cnsDesignerContainer.SizeChanged += delegate
            {
                GridLines.Update(cnsDesignerContainer.ActualWidth, cnsDesignerContainer.ActualHeight);
            };
        }

        /// <summary>
        /// 添加活动节点
        /// </summary>
        /// <param name="activity">活动节点（开始、结束、工序等）</param>
        void AddActivityAnimation(IActivity activity)
        {
            if (activity != null)
            {
                activity.SelectedEvent += Activity_SelectedEvent;
                activity.UnselectedEvent += Activity_UnselectedEvent;
            }
        }

        /// <summary>
        /// 活动节点取消选中事件
        /// </summary>
        /// <param name="obj">元素</param>
        private void Activity_UnselectedEvent(IElement obj)
        {
            _storyboard.Stop(this);
            foreach (var runPoint in runPoints.Where(runPoint => Children.Contains(runPoint)))
            {
                Children.Remove(runPoint);
            }

            runPoints.Clear();
        }

        /// <summary>
        /// 活动节点选中事件
        /// </summary>
        /// <param name="obj">元素</param>
        private void Activity_SelectedEvent(IElement obj)
        {
            if (Model.SelectElements.Count > 1)
            {
                Activity_UnselectedEvent(null);
                return;
            }

            InitStoryboard(obj as IActivity);
            _storyboard.Begin(this);
        }

        /// <summary>
        /// 初始化Model
        /// </summary>
        /// <param name="model">容器</param>
        void InitModel(IContainer model)
        {
            Model = model;
            this.DataContext = Model;
            ModelChanged?.Invoke(Model);
        }

        /// <summary>
        /// 初始化网格
        /// </summary>
        void InitGridLines()
        {
            GridLines = new Gridding(Model.Width, Model.Height);
            GridLines.DataContext = Model;
            var binding = new Binding("ShowGridLine");
            binding.Converter = new BooleanToVisibilityConverterExtension();
            GridLines.SetBinding(Gridding.VisibilityProperty, binding);
            Children.Add(GridLines);
            tempRectangle = new Rectangle();
            tempRectangle.Visibility = Visibility.Collapsed;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, 234, 213, 2);
            tempRectangle.Fill = brush;
            tempRectangle.Opacity = 0.2;

            brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, 0, 0, 0);
            tempRectangle.Stroke = brush;
            tempRectangle.StrokeMiterLimit = 2.0;
            tempRectangle.SetValue(Panel.ZIndexProperty, int.MaxValue);
            Children.Add(tempRectangle);
        }

        /// <summary>
        /// 添加流程
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        public void AddFlow_Click(object sender, RoutedEventArgs e)
        {
            var model = sender as IContainer;
            if (model != null)
            {
                Children.Clear();
                InitModel(model);
                InitGridLines();
                CreateNewWorkFlow();
            }
        }

        /// <summary>
        /// 粘贴工艺流程版本
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        public void PasteRoutingVersion_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(CopyPasteXml))
            {
                throw new ValidationException("粘贴失败，请先进行复制操作!".L10nFormat());
            }
            if (!IsCopyFinished)
            {
                LoadFromXmlString(CopyPasteXml, true);
                ProcessCopyRoutingVersion(CopyPasteXml);
            }

            IsCopyFinished = false;
            var modelParam = sender as IContainer;
            Model.RoutingId = modelParam.RoutingId;
            Model.RoutingVersionId = modelParam.RoutingVersionId;
        }

        /// <summary>
        /// 复制流程
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        public void Copylow_Click(object sender, RoutedEventArgs e)
        {
            LoadFromXmlString(sender.ToString(), true);
        }

        /// <summary>
        /// 复制工艺路线版本
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">参数</param>
        public void CopyRoutingVersion_Click(object sender, RoutedEventArgs e)
        {
            LoadFromXmlString(sender.ToString(), true);
            ProcessCopyRoutingVersion(sender.ToString());
        }

        /// <summary>
        /// 根据复制选项处理复制的工艺路线版本
        /// </summary>
        /// <param name="copyPasteXml">复制粘贴的xml字符串</param>
        private void ProcessCopyRoutingVersion(string copyPasteXml)
        {
            if (Model == null)
            {
                return;
            }
            IsCopyFinished = true;
            CopyPasteXml = copyPasteXml;
            var activitys = Model.Activitys;
            if (activitys != null && activitys.Count > 0)
            {
                foreach (var activityModel in activitys)
                {
                    if (!CopyViewModel.IsCopyBom)
                        activityModel.Bom.Clear();
                    if (!CopyViewModel.IsCopyActivityProperty)
                    {
                        activityModel.IsOptional = false;
                        activityModel.IsRepeat = false;
                        activityModel.CreateSku = false;
                        activityModel.IsCalculate = false;
                        activityModel.IsGenerateTask = false;
                        activityModel.IsRequirementTask = false;
                        activityModel.IsBuckleMaterial = false;
                        activityModel.IsPassRate = false;
                        activityModel.IsBinding = false;
                        activityModel.IsUnBinding = false;
                        activityModel.StartProcess = null;
                        activityModel.NormalVictory = null;
                        activityModel.RepairVictory = null;
                        activityModel.IsStricter = false;
                        activityModel.Overtime = null;
                        activityModel.MaxPassNum = null;
                        activityModel.IsNextMoveIn = false;
                    }
                }
            }
        }

        /// <summary>
        /// 编辑流程
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        public void EditFlow_Click(object sender, RoutedEventArgs e)
        {
            LoadFromXmlString(sender.ToString());
        }

        /// <summary>
        /// 创建一个默认新流程
        /// </summary>
        public void CreateNewWorkFlow()
        {
            var beginActivityModel = new ActivityModel();
            beginActivityModel.Type = ActivityType.Initial;
            beginActivityModel.Text = "开始";

            var rule = new RuleModel();
            rule.SourceActivityId = beginActivityModel.Id;
            beginActivityModel.Rules.Add(rule);

            var beginActivity = new Activity(this, beginActivityModel);
            beginActivityModel.SetPoint(new SIE.Tech.Routings.Technologys.Point(400, 126));
            AddActivityAnimation(beginActivityModel);
            Children.Add(beginActivity);
            Model.Rules.Add(rule);
            Model.AddChild(beginActivityModel);

            var endActivityModel = new ActivityModel();
            endActivityModel.Type = ActivityType.Completion;
            endActivityModel.Text = "结束";
            var endActivity = new Activity(this, endActivityModel);
            endActivityModel.SetPoint(new SIE.Tech.Routings.Technologys.Point(400, 500));
            Children.Add(endActivity);
            Model.AddChild(endActivityModel);
        }

        /// <summary>
        /// 加载xml到容器
        /// </summary>
        /// <param name="xml">xml</param>
        /// <param name="isCopying">复制命令触发时为true</param>
        public void LoadFromXmlString(string xml, bool isCopying = false)
        {
            Children.Clear();
            if (xml.IsNullOrWhiteSpace())
            {
                InitModel(new ContainerModel());
                return;
            }

            try
            {
                var containerModel = new ContainerModel();
                containerModel.Deserialize(xml);
                if (isCopying)
                {
                    containerModel.RoutingVersionId = 0;
                }
                InitModel(containerModel);
                foreach (var ruleModel in containerModel.Rules.Where(p => p.EndActivity != null))
                {
                    var rule = new Rule(this, ruleModel);
                    Children.Add(rule);
                }

                foreach (var activityModel in containerModel.Activitys)
                {
                    var activity = new Activity(this, activityModel);
                    AddActivityAnimation(activityModel);
                    Children.Add(activity);
                }
            }
            catch (Exception exc)
            {
                CRT.MessageService.ShowMessage(exc.Message, "打开流程失败".L10N());
                InitModel(new ContainerModel());
            }
            finally
            {
                InitGridLines();
            }
        }

        /// <summary>
        /// 对象拖动到容器时触发
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Container_PreviewDrop(object sender, DragEventArgs e)
        {
            if (Model.RoutingId > 0)
            {
                ActivityModel activityModel = e.Data.GetData(typeof(ActivityModel)) as ActivityModel;
                var processType = activityModel.ProcessType;
                if (Model.Activitys.FirstOrDefault(p => (p.ProcessType == ProcessType.BatchAssembly || p.ProcessType == ProcessType.BatchFix || p.ProcessType == ProcessType.BatchPacking || p.ProcessType == ProcessType.BatchPqc) && p.ProcessId > 0) != null
                    && (!(processType == ProcessType.BatchAssembly || processType == ProcessType.BatchFix || processType == ProcessType.BatchPacking || processType == ProcessType.BatchPqc)))
                {
                    CRT.MessageService.ShowMessage("只能添加批次类型工序".L10N());
                    return;
                }
                else if (Model.Activitys.FirstOrDefault(p => (p.ProcessType == ProcessType.Assembly || p.ProcessType == ProcessType.Fix || p.ProcessType == ProcessType.Packing || p.ProcessType == ProcessType.Pqc) && p.ProcessId > 0) != null
                            && (processType == ProcessType.BatchAssembly || processType == ProcessType.BatchFix || processType == ProcessType.BatchPacking || processType == ProcessType.BatchPqc))
                {
                    CRT.MessageService.ShowMessage("只能添加非批次类型工序".L10N());
                    return;
                }
                else
                {
                    //
                }

                var activity = new Activity(this, activityModel);
                AddActivityAnimation(activityModel);
                Children.Add(activity);
                Model.AddChild(activityModel);
                foreach (var rule in activityModel.Rules)
                {
                    Model.Rules.Add(rule);
                }

                var point = e.GetPosition(this);
                activityModel.SetPoint(new SIE.Tech.Routings.Technologys.Point(point.X + svContainer.HorizontalOffset, point.Y + svContainer.VerticalOffset));
            }
        }

        /// <summary>
        /// 鼠标位置
        /// </summary>
        System.Windows.Point mousePosition;

        /// <summary>
        /// 容器鼠标左键点击事件，显示临时矩形选中框
        /// 用于选中容器元素
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Container_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Model.SelectedElement(Model);
            mousePosition = e.GetPosition(sender as FrameworkElement);
            tempRectangle.Visibility = Visibility.Visible;
            TrackingMouseMove = true;
        }

        /// <summary>
        /// 是否跟踪鼠标移动
        /// </summary>
        public bool TrackingMouseMove { get; set; }

        /// <summary>
        /// 规则1矩形
        /// </summary>
        public Ellipse MoveEllipse1 { get; set; }
         

        /// <summary>
        /// 规则2矩形
        /// </summary>
        public Ellipse MoveEllipse2 { get; set; }

        /// <summary>
        /// 鼠标移动事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (TrackingMouseMove)
            {
                FrameworkElement element = sender as FrameworkElement;
                System.Windows.Point beginPoint = mousePosition;
                System.Windows.Point endPoint = e.GetPosition(element);

                if (endPoint.X >= beginPoint.X)
                {
                    if (endPoint.Y >= beginPoint.Y)
                    {
                        tempRectangle.SetValue(Canvas.TopProperty, beginPoint.Y);
                        tempRectangle.SetValue(Canvas.LeftProperty, beginPoint.X);
                    }
                    else
                    {
                        tempRectangle.SetValue(Canvas.TopProperty, endPoint.Y);
                        tempRectangle.SetValue(Canvas.LeftProperty, beginPoint.X);
                    }
                }
                else
                {
                    if (endPoint.Y >= beginPoint.Y)
                    {
                        tempRectangle.SetValue(Canvas.TopProperty, beginPoint.Y);
                        tempRectangle.SetValue(Canvas.LeftProperty, endPoint.X);
                    }
                    else
                    {
                        tempRectangle.SetValue(Canvas.TopProperty, endPoint.Y);
                        tempRectangle.SetValue(Canvas.LeftProperty, endPoint.X);
                    }
                }

                tempRectangle.Width = Math.Abs(endPoint.X - beginPoint.X);
                tempRectangle.Height = Math.Abs(endPoint.Y - beginPoint.Y);
            }
            else
            {
                if (TempRule != null)
                {
                    var point = e.GetPosition(TempRule);
                    var currentPoint = new SIE.Tech.Routings.Technologys.Point(point.X, point.Y);
                    TempRule.Model.EndPoint = currentPoint;

                    if (TempRule.Model.BeginActivity != null)
                    {
                        TempRule.Model.SetPointOfIntersection(currentPoint, TempRule.Model.BeginActivity.GetPoint(), RuleMoveType.Begin, TempRule.Model.BeginActivity);
                    }
                }
                else if (MoveEllipse1 != null)
                {
                    if (MoveEllipse1.Tag is IRule)
                    {
                        var rule = MoveEllipse1.Tag as IRule;
                        var point = e.GetPosition(this.cnsDesignerContainer);
                        rule.Point1 = new SIE.Tech.Routings.Technologys.Point(point.X, point.Y);
                    }
                }
                else if (MoveEllipse2 != null && MoveEllipse2.Tag is IRule)
                {
                    var rule = MoveEllipse2.Tag as IRule;
                    var point = e.GetPosition(this.cnsDesignerContainer);
                    rule.Point2 = new SIE.Tech.Routings.Technologys.Point(point.X, point.Y);
                }
                else
                {
                    //
                }
            }
        }

        /// <summary>
        /// 容器鼠标左键点击事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Container_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (TrackingMouseMove)
            {
                if (tempRectangle.Width > 0 && tempRectangle.Height > 0)
                {
                    double width = tempRectangle.Width;
                    double height = tempRectangle.Height;
                    double left = (double)tempRectangle.GetValue(Canvas.LeftProperty);
                    double top = (double)tempRectangle.GetValue(Canvas.TopProperty);
                    Model.SelectedElement(null);
                    foreach (var child in Model.Children)
                    {
                        if (child is IActivity)
                        {
                            var activity = child as IActivity;
                            if (left <= activity.Left && activity.Left <= left + width
                                && top <= activity.Top + 25 && activity.Top + 25 <= top + height
                                && left <= activity.Left + activity.Width && activity.Left + activity.Width <= left + width
                                && top <= activity.Top + activity.Height + 25 && activity.Top + activity.Height + 25 <= top + height)
                            {
                                Model.SelectedElement(activity, true, false);
                            }
                        }
                        else if (child is IRule)
                        {
                            var rule = child as IRule;
                            if (left <= rule.BeginLeft && rule.BeginLeft <= left + width
                                && top <= rule.BeginTop && rule.BeginTop <= top + height
                                && left <= rule.EndLeft && rule.EndLeft <= left + width
                                && top <= rule.EndTop && rule.EndTop <= top + height)
                            {
                                Model.SelectedElement(rule, true, false);
                            }
                        }
                        else
                        {
                            //
                        }
                    }
                }

                tempRectangle.Width = tempRectangle.Height = 0;
                tempRectangle.Visibility = Visibility.Collapsed;
                TrackingMouseMove = false;
            }
            else
            {
                if (TempRule != null)
                {
                    TempRule.ReleaseMouseCapture();
                    TempRule.SetEndActivity();
                    if (TempRule.Model.EndActivity != null)
                    {
                        TempRule.Model.EndActivity.Move();
                        TempRule.Model.BeginActivity.Rules.Remove(TempRule.Model);
                        TempRule.Model.BeginActivity.BeginRules.Add(TempRule.Model);
                        TempRule.Model.EndActivity.EndRules.Add(TempRule.Model);
                    }
                    else
                    {
                        Children.Remove(TempRule);
                        Model.RemoveChild(TempRule.Model);
                    }

                    TempRule = null;
                }
                else if (MoveEllipse1 != null)
                {
                    MoveEllipse1.ReleaseMouseCapture();
                    MoveEllipse1 = null;
                }
                else if (MoveEllipse2 != null)
                {
                    MoveEllipse2.ReleaseMouseCapture();
                    MoveEllipse2 = null;
                }
                else
                {
                    //
                }
            }
        }

        /// <summary>
        /// 运动点集合
        /// </summary>
        List<Grid> runPoints = new List<Grid>();

        /// <summary>
        /// 位置
        /// </summary>
        Path _pathPoint;

        /// <summary>
        /// 故事板
        /// </summary>
        private Storyboard _storyboard;

        /// <summary>
        /// 初始化故事板
        /// </summary>
        /// <param name="activity">活动节点</param>
        void InitStoryboard(IActivity activity)
        {
            if (activity == null || activity.BeginRules == null || activity.BeginRules.Count == 0)
                return;
            _storyboard.Children.Clear();
            foreach (var rule in activity.BeginRules)
            {
                ////颜色
                if (rule.StartPoint.X == rule.EndPoint.X && rule.StartPoint.Y == rule.EndPoint.Y)
                {
                    return;
                }
                Path particlePath = GetParticlePath(new System.Windows.Point(rule.StartPoint.X, rule.StartPoint.Y), new System.Windows.Point(rule.Point1.X, rule.Point1.Y), new System.Windows.Point(rule.Point2.X, rule.Point2.Y), new System.Windows.Point(rule.EndPoint.X, rule.EndPoint.Y), rule.Color);
                //// 跑动的点
                Grid grid = GetRunPoint(rule.Color);
                AddPointToStoryboard(grid, _storyboard, particlePath);
                Canvas.SetLeft(grid, -grid.Width / 2);
                Canvas.SetTop(grid, -grid.Height / 2);
                cnsDesignerContainer.Children.Add(grid);
                runPoints.Add(grid);
            }
        }

        #region 获取运动轨迹
        /// <summary>
        /// 获取运动轨迹
        /// </summary>
        /// <param name="startPoint">起点</param>
        /// <param name="point1">控制点</param>
        /// <param name="point2">终点</param>
        /// <param name="endPoint">结束位置</param>
        /// <param name="color">颜色:r,g,b</param>
        /// <returns>Path</returns>
        private Path GetParticlePath(System.Windows.Point startPoint, System.Windows.Point point1, System.Windows.Point point2, System.Windows.Point endPoint, string color)
        {
            Path path = new Path();
            PathGeometry pg = new PathGeometry();
            PathFigure pf = new PathFigure();
            pf.StartPoint = startPoint;
            BezierSegment bezierSegment = new BezierSegment();
            bezierSegment.Point1 = point1;
            bezierSegment.Point2 = point2;
            bezierSegment.Point3 = endPoint;
            pf.Segments.Add(bezierSegment);
            pg.Figures.Add(pf);
            path.Data = pg;
            path.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            path.Stretch = Stretch.None;
            return path;
        }
        #endregion

        /// <summary>
        /// 获取跑动的点
        /// </summary>
        /// <param name="color">颜色:r,g,b</param>
        /// <returns>Grid</returns>
        private Grid GetRunPoint(string color)
        {
            ////一个Grid里包含一个椭圆 一个Path 椭圆做阴影
            Grid grid = new Grid();
            grid.Width = 40;
            grid.Height = 15;
            grid.RenderTransformOrigin = new System.Windows.Point(0.5, 0.5);
            Path path = new Path();
            path.Data = _pathPoint.Data;
            path.Width = _pathPoint.Width;
            path.Height = _pathPoint.Height;
            LinearGradientBrush lgb = new LinearGradientBrush();
            lgb.StartPoint = new System.Windows.Point(0, 0);
            lgb.EndPoint = new System.Windows.Point(1, 0);
            var lgbFill = (Color)ColorConverter.ConvertFromString(color);
            lgbFill.A = 100;
            lgb.GradientStops.Add(new GradientStop(lgbFill, 0));
            lgb.GradientStops.Add(new GradientStop(Color.FromArgb(255, 255, 255, 255), 1));
            path.Fill = lgb;
            path.Stretch = Stretch.Fill;
            grid.Children.Add(path);
            grid.SetValue(Canvas.ZIndexProperty, int.MaxValue);
            return grid;
        }

        #region 将点加入到动画故事版
        /// <summary>
        /// 将点加入到动画故事版
        /// </summary>
        /// <param name="point">运动的点</param>
        /// <param name="sb">故事版</param>
        /// <param name="particlePath">运动轨迹</param>
        private void AddPointToStoryboard(Grid point, Storyboard sb, Path particlePath)
        {
            MatrixTransform matrix = new MatrixTransform();
            point.RenderTransform = matrix;
            string name = "m" + Guid.NewGuid().ToString().Replace("-", string.Empty);
            this.RegisterName(name, matrix);

            MatrixAnimationUsingPath matrixAnimation = new MatrixAnimationUsingPath();
            matrixAnimation.PathGeometry = particlePath.Data.GetFlattenedPathGeometry();
            matrixAnimation.Duration = new Duration(TimeSpan.FromSeconds(2));
            matrixAnimation.RepeatBehavior = RepeatBehavior.Forever;
            matrixAnimation.AutoReverse = false;
            matrixAnimation.IsOffsetCumulative = false;
            matrixAnimation.DoesRotateWithTangent = true; ////沿切线旋转

            Storyboard.SetTargetName(matrixAnimation, name);
            Storyboard.SetTargetProperty(matrixAnimation, new PropertyPath(MatrixTransform.MatrixProperty));
            sb.Children.Add(matrixAnimation);
        }
        #endregion

        /// <summary>
        /// Ctrl键是否按下
        /// </summary>
        public bool CtrlKeyIsPress
        {
            get { return Keyboard.Modifiers == ModifierKeys.Control; }
        }

        /// <summary>
        /// 键盘控制活动节点移动长度
        /// </summary>
        int _moveStepLenght
        {
            get
            {
                if (CtrlKeyIsPress)
                {
                    return 5;
                }
                return 1;
            }
        }

        /// <summary>
        /// 控件键盘触发事件
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void UserControl_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Down:
                    Model.MoveSelectItems(0, _moveStepLenght);
                    MoveDown();
                    e.Handled = true;
                    break;
                case Key.Up:
                    Model.MoveSelectItems(0, -_moveStepLenght);
                    MoveUp();
                    e.Handled = true;
                    break;
                case Key.Left:
                    Model.MoveSelectItems(-_moveStepLenght, 0);
                    MoveLeft();
                    e.Handled = true;
                    break;
                case Key.Right:
                    Model.MoveSelectItems(_moveStepLenght, 0);
                    MoveRight();
                    e.Handled = true;
                    break;
                case Key.A:
                    if (CtrlKeyIsPress)
                    {
                        foreach (var child in Model.Children)
                        {
                            Model.SelectedElement(child, true, false);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 向下移动滚动条一行
        /// </summary>
        void MoveDown()
        {
            var activitys = Model.SelectElements.OfType<IActivity>().Select(p => p ).ToList();
            if (activitys.Count > 0)
            {
                var height = svContainer.VerticalOffset + ActualHeight;
                var maxHeight = activitys.Max(p => p.Top + p.ContainerHeight);
                if (maxHeight > height)
                {
                    svContainer.LineDown();
                }
            }
        }

        /// <summary>
        /// 向上移动滚动条一行
        /// </summary>
        void MoveUp()
        {
            var activitys = Model.SelectElements.OfType<IActivity>().Select(p => p).ToList();
            if (activitys.Count > 0 && svContainer.VerticalOffset > activitys.Min(p => p.Top))
            {
                svContainer.LineUp();
            }
        }

        /// <summary>
        /// 向左移动滚动条一行
        /// </summary> 
        void MoveLeft()
        {
            var activitys = Model.SelectElements.OfType<IActivity>().Select(p => p).ToList();
            if (activitys.Count > 0 && svContainer.HorizontalOffset > activitys.Min(p => p.Left))
            {
                svContainer.LineLeft();
            }
        }

        /// <summary>
        /// 向右移动滚动条一行
        /// </summary>
        void MoveRight()
        {
            var activitys = Model.SelectElements.OfType<IActivity>().Select(p => p).ToList();
            if (activitys.Count > 0)
            {
                var width = svContainer.HorizontalOffset + ActualWidth;
                var maxWidth = activitys.Max(p => p.Left + p.Width);
                if (maxWidth > width)
                {
                    svContainer.LineRight();
                }
            }
        }

        /// <summary>
        /// 屏幕容器滚动条移动
        /// </summary>
        public void Scrollable()
        {
            MoveRight();
            MoveLeft();
            MoveDown();
            MoveUp();
        }
    }
}