using SIE.Domain.Validation;
using SIE.Tech.Processs;
using SIE.Utils;
using System;
using System.IO;
using System.Text;
using System.Xml.Linq;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
    /// 规则模型
    /// </summary>
    [Serializable]
    public class RuleModel : ChildElementModel, IRule
    {
        const double END_POINT_RADIUS = 4;
        const double BEGIN_POINT_RADIUS = 4;
        const double EXTENDED_LENGTH = 50;

        /// <summary>
        /// 构造函数，初始化颜色
        /// </summary>
        public RuleModel()
        {
            Color = "#FF33BCBA";
        }

        /// <summary>
        /// 规则属性变更
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == "StartPoint")
            {
                BeginLeft = StartPoint.X;
                BeginTop = StartPoint.Y;
                SetTextPonit();
            }

            if (propertyName == "EndPoint")
            {
                EndLeft = EndPoint.X;
                EndTop = EndPoint.Y;
                SetTextPonit();
            }

            if (propertyName == "Point1")
            {
                Left1 = Point1.X;
                Top1 = Point1.Y;
            }

            if (propertyName == "Point2")
            {
                Left2 = Point2.X;
                Top2 = Point2.Y;
            }

            if (propertyName == "Type" && BeginActivity != null)
            {
                OnActivityMove(BeginActivity);
                base.OnPropertyChanged("Self");
            }

            if (Type == RuleType.Line && BeginActivity != null && BeginActivity == EndActivity)
            {
                Type = RuleType.Curve;
            }

            if (propertyName == "Text")
            {
                if (Text == EnumViewModel.EnumToLabel(ResultTypeForDesign.Pass).L10N())
                {
                    Color = "#FF33A133";
                }
                else if (Text == EnumViewModel.EnumToLabel(ResultTypeForDesign.Fail).L10N())
                {
                    Color = "#FFEA4333";
                }
                else if (Text == EnumViewModel.EnumToLabel(ResultTypeForDesign.Any).L10N())
                {
                    Color = "#FFBB33FF";
                }
                else
                {
                    Color = "#FF33BCBA";
                }
            }

            ////if (propertyName == "Color")
            ////{
            ////}
        }

        /// <summary>
        /// 设置规则文本位置
        /// </summary>
        void SetTextPonit()
        {
            if (BeginActivity == EndActivity)
            {
                TextLeft = (Point1.X + Point2.X) / 2 - 10;
                TextTop = Point1.Y - 5;
                return;
            }

            TextLeft = (StartPoint.X + EndPoint.X) / 2;
            TextTop = (StartPoint.Y + EndPoint.Y) / 2;
        }

        /// <summary>
        /// 活动节点移动
        /// </summary>
        /// <param name="activity">活动节点</param>
        public void OnActivityMove(IActivity activity)
        {
            if (activity == null || (activity != EndActivity && activity != BeginActivity))
            {
                return;
            }

            if (EndActivity == activity)
            {
                SetPointOfIntersection(StartPoint, EndActivity.GetPoint(), RuleMoveType.End, EndActivity);
                if (BeginActivity != null)
                {
                    SetPointOfIntersection(EndActivity.GetPoint(), BeginActivity.GetPoint(), RuleMoveType.Begin, BeginActivity);
                }
            }
            else if (BeginActivity == activity)
            {
                SetPointOfIntersection(EndPoint, BeginActivity.GetPoint(), RuleMoveType.Begin, BeginActivity);
                if (EndActivity != null)
                {
                    SetPointOfIntersection(BeginActivity.GetPoint(), EndActivity.GetPoint(), RuleMoveType.End, EndActivity);
                }
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 设置交叉点
        /// </summary>
        /// <param name="beginPoint">开始点</param>
        /// <param name="endPoint">结束点</param>
        /// <param name="type">规则移动类型</param>
        /// <param name="activity">活动节点</param>
        public void SetPointOfIntersection(Point beginPoint, Point endPoint, RuleMoveType type, IActivity activity)
        {
            if (activity == null)
            {
                return;
            }
            ////起始点坐标和终点坐标之间的夹角（相对于Y轴坐标系）
            double angle = Math.Abs(Math.Atan((endPoint.X - beginPoint.X) / (endPoint.Y - beginPoint.Y)) * 180.0 / Math.PI);
            ////活动的长和宽之间的夹角（相对于Y轴坐标系）
            double angle2 = Math.Abs(Math.Atan(activity.Width / activity.Height) * 180.0 / Math.PI);
            if (activity.Type == ActivityType.Interaction)
            {
                SetInteractionPoint(beginPoint, endPoint, type, activity, angle, angle2);
            }
            else if (activity.Type == ActivityType.Initial || activity.Type == ActivityType.Completion)
            {
                SetPointCompletion(beginPoint, endPoint, type, activity, angle, angle2);
            }
            else
            {
                //
            }

            if (Type == RuleType.Line)
            {
                Point1 = Point2 = StartPoint;
            }
        }

        /// <summary>
        /// 设置活动类型为交互时的交互点
        /// </summary>
        /// <param name="beginPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="type"></param>
        /// <param name="activity"></param>
        /// <param name="angle"></param>
        /// <param name="angle2"></param>
        private void SetInteractionPoint(Point beginPoint, Point endPoint, RuleMoveType type, IActivity activity, double angle, double angle2)
        {
            #region
            Point point2 = new Point();
            Point point = new Point();
            if (Math.Abs(endPoint.X - beginPoint.X) <= activity.Width / 2 && Math.Abs(endPoint.Y - beginPoint.Y) <= activity.Height / 2)
            {
                point = endPoint;
            }
            else
            {
                ////半径
                double radio = activity.Height < activity.Width ? activity.Height / 2 : activity.Width / 2;

                if (angle <= angle2) ////起始点坐标在终点坐标的上方,或者下方
                {
                    if (endPoint.Y < beginPoint.Y) ////在上方
                    {
                        point.X = endPoint.X < beginPoint.X ? endPoint.X + Math.Tan(Math.PI * angle / 180.0) * radio :
                            endPoint.X - Math.Tan(Math.PI * angle / 180.0) * radio;

                        point.Y = endPoint.Y + activity.Height / 2;
                        point2 = new Point(point.X, point.Y + EXTENDED_LENGTH);
                    }
                    else ////在下方
                    {
                        point.X = endPoint.X < beginPoint.X ? endPoint.X + Math.Tan(Math.PI * angle / 180.0) * radio :
                            endPoint.X - Math.Tan(Math.PI * angle / 180.0) * radio;

                        point.Y = endPoint.Y - activity.Height / 2;
                        point2 = new Point(point.X, point.Y - EXTENDED_LENGTH);
                    }
                }
                else ////左方或者右方
                {
                    if (endPoint.X < beginPoint.X) ////在右方
                    {
                        point.X = endPoint.X + activity.Width / 2;
                        point.Y = endPoint.Y < beginPoint.Y ? endPoint.Y + Math.Tan(Math.PI * (90 - angle) / 180.0) * radio :
                            endPoint.Y - Math.Tan(Math.PI * (90 - angle) / 180.0) * radio;
                        point2 = new Point(point.X + EXTENDED_LENGTH, point.Y);
                    }
                    else ////在左方
                    {
                        point.X = endPoint.X - activity.Width / 2;
                        point.Y = endPoint.Y < beginPoint.Y ? endPoint.Y + Math.Tan(Math.PI * (90 - angle) / 180.0) * radio :
                            endPoint.Y - Math.Tan(Math.PI * (90 - angle) / 180.0) * radio;
                        point2 = new Point(point.X - EXTENDED_LENGTH, point.Y);
                    }
                }
            }

            if (BeginActivity != null && BeginActivity == EndActivity)
            {
                StartPoint = EndPoint = new Point(BeginActivity.Left + BeginActivity.Width / 2, BeginActivity.Top + (BeginActivity.ContainerHeight - BeginActivity.Height) / 2);
                var sideLength = Math.Sqrt((EXTENDED_LENGTH * EXTENDED_LENGTH) / 2);
                Point1 = new Point(StartPoint.X - sideLength, StartPoint.Y - sideLength);
                Point2 = new Point(StartPoint.X + sideLength, StartPoint.Y - sideLength);
            }
            else if (type == RuleMoveType.End)
            {
                point.X -= END_POINT_RADIUS;
                point.Y -= END_POINT_RADIUS;
                EndPoint = point;
                Point2 = point2;
            }
            else if (type == RuleMoveType.Begin)
            {
                point.X -= BEGIN_POINT_RADIUS;
                point.Y -= BEGIN_POINT_RADIUS;
                StartPoint = point;
                Point1 = point2;
            }
            else
            {
                //
            }
            #endregion

        }

        /// <summary>
        /// 设置活动类型为终态时的交互点
        /// </summary>
        /// <param name="beginPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="type"></param>
        /// <param name="activity"></param>
        /// <param name="angle"></param>
        /// <param name="angle2"></param>
        private void SetPointCompletion(Point beginPoint, Point endPoint, RuleMoveType type, IActivity activity, double angle, double angle2)
        {
            #region
            Point point2 = new Point();
            Point point = new Point();
            if (Math.Abs(endPoint.X - beginPoint.X) <= activity.Width / 2
                && Math.Abs(endPoint.Y - beginPoint.Y) <= activity.Height / 2)
            {
                point = endPoint;
            }
            else
            {
                double radial = (activity.Width < activity.Height ? activity.Width : activity.Height) / 2;
                double bc = Math.Sqrt((endPoint.X - beginPoint.X) * (endPoint.X - beginPoint.X) + (endPoint.Y - beginPoint.Y) * (endPoint.Y - beginPoint.Y));
                point.X = endPoint.X - (endPoint.X - beginPoint.X) * radial / bc;
                point.Y = endPoint.Y - (endPoint.Y - beginPoint.Y) * radial / bc;
                if (angle <= angle2) ////起始点坐标在终点坐标的上方,或者下方
                {
                    point2 = endPoint.Y < beginPoint.Y ? new Point(point.X, point.Y + EXTENDED_LENGTH) : new Point(point.X, point.Y - EXTENDED_LENGTH);
                }
                else
                {
                    point2 = endPoint.X < beginPoint.X ? new Point(point.X + EXTENDED_LENGTH, point.Y) : new Point(point.X - EXTENDED_LENGTH, point.Y);
                }
            }

            if (type == RuleMoveType.End)
            {
                point.X -= END_POINT_RADIUS;
                point.Y -= END_POINT_RADIUS;
                EndPoint = point;
                Point2 = point2;
            }
            else if (type == RuleMoveType.Begin)
            {
                point.X -= BEGIN_POINT_RADIUS;
                point.Y -= BEGIN_POINT_RADIUS;
                StartPoint = point;
                Point1 = point2;
            }
            else
            {
                // 
            }
            #endregion
        }

        /// <summary>
        /// 设置开始活动节点
        /// </summary>
        /// <param name="activity">活动节点</param>
        public void SetBeginActivity(IActivity activity)
        {
            if (BeginActivity != null)
            {
                BeginActivity.ActivityMove -= OnActivityMove;
            }

            BeginActivity = activity;
            BeginActivity.ActivityMove += OnActivityMove;
        }

        /// <summary>
        /// 设置结束活动节点
        /// </summary>
        /// <param name="activity">活动节点</param>
        public void SetEndActivity(IActivity activity)
        {
            if (EndActivity != null)
            {
                EndActivity.ActivityMove -= OnActivityMove;
            }

            EndActivity = activity;
            EndActivity.ActivityMove += OnActivityMove;
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        public override void Delete()
        {
            Type = RuleType.Line;
            if (BeginActivity != null && !BeginActivity.Rules.Contains(this))
            {
                BeginActivity.BeginRules.Remove(this);
                BeginActivity.Rules.Add(this);
                BeginActivity = null;
            }

            if (EndActivity != null)
            {
                EndActivity.EndRules.Remove(this);
                EndActivity = null;
            }
        }

        /// <summary>
        /// 序列化规则
        /// </summary>
        /// <returns>序列化xml</returns>
        public override string Serialize()
        {
            XElement el = new XElement("Rule");
            el.SetAttributeValue(nameof(Id), Id);
            el.SetAttributeValue(nameof(State), State);
            el.SetAttributeValue(nameof(IsSelected), IsSelected);
            el.SetAttributeValue(nameof(ZIndex), ZIndex);
            el.SetAttributeValue(nameof(SourceActivityId), SourceActivityId);
            el.SetAttributeValue(nameof(Text), Text);
            el.SetAttributeValue(nameof(ParameterId), ParameterId);
            el.SetAttributeValue(nameof(ParamResultType), ParamResultType);
            el.SetAttributeValue(nameof(Expression), Expression);
            el.SetAttributeValue(nameof(StartPoint), StartPoint);
            el.SetAttributeValue(nameof(Point1), Point1);
            el.SetAttributeValue(nameof(Point2), Point2);
            el.SetAttributeValue(nameof(EndPoint), EndPoint);
            el.SetAttributeValue("BeginActivityId", BeginActivity?.Id);
            el.SetAttributeValue("EndActivityId", EndActivity?.Id);
            el.SetAttributeValue(nameof(Type), Type);
            el.SetAttributeValue(nameof(BeginLeft), BeginLeft);
            el.SetAttributeValue(nameof(BeginTop), BeginTop);
            el.SetAttributeValue(nameof(EndLeft), EndLeft);
            el.SetAttributeValue(nameof(EndTop), EndTop);
            el.SetAttributeValue(nameof(TextLeft), TextLeft);
            el.SetAttributeValue(nameof(TextTop), TextTop);
            el.SetAttributeValue(nameof(Left1), Left1);
            el.SetAttributeValue(nameof(Left2), Left2);
            el.SetAttributeValue(nameof(Top1), Top1);
            el.SetAttributeValue(nameof(Top2), Top2);
            el.SetAttributeValue(nameof(Color), Color);
            return el.ToString();
        }

        /// <summary>
        /// 反序列化规则
        /// </summary>
        /// <param name="xml">序列化xml</param>
        /// <param name="isCopy"></param>
        public override void Deserialize(string xml, bool isCopy = false)
        {
            if (xml == null || xml.Length < 10)   //<Rule/>
            {
                return;
            }

            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            XElement xelement = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(bytes)));
            Id = xelement.GetAttributeValue(nameof(Id), string.Empty);
            if (isCopy)
            {
                CopyId = Id;
                Id = Guid.NewGuid().ToString();

            }


            State = (ElementState)EnumViewModel.LabelToEnum(xelement.GetAttributeValue(nameof(State), string.Empty), typeof(ElementState));
            Text = xelement.GetAttributeValue(nameof(Text), string.Empty);
            ParameterId = xelement.GetAttributeValue(nameof(ParameterId), 0d);
            ParamResultType = xelement.GetAttributeValue(nameof(ParamResultType), ResultTypeForDesign.Any);
            Expression = xelement.GetAttributeValue(nameof(Expression), string.Empty);
            var startPoint = xelement.GetAttributeValue(nameof(StartPoint), string.Empty);
            if (!startPoint.IsNullOrWhiteSpace() && startPoint.Split(',').Length > 1)
            {
                StartPoint = new Point(startPoint.Split(',')[0].ConvertTo<double>(), startPoint.Split(',')[1].ConvertTo<double>());
            }

            var point1 = xelement.GetAttributeValue(nameof(Point1), string.Empty);
            if (!point1.IsNullOrWhiteSpace() && point1.Split(',').Length > 1)
            {
                Point1 = new Point(point1.Split(',')[0].ConvertTo<double>(), point1.Split(',')[1].ConvertTo<double>());
            }

            var point2 = xelement.GetAttributeValue(nameof(Point2), string.Empty);
            if (!point2.IsNullOrWhiteSpace() && point2.Split(',').Length > 1)
            {
                Point2 = new Point(point2.Split(',')[0].ConvertTo<double>(), point2.Split(',')[1].ConvertTo<double>());
            }

            var endPoint = xelement.GetAttributeValue(nameof(EndPoint), string.Empty);
            if (!endPoint.IsNullOrWhiteSpace() && endPoint.Split(',').Length > 1)
            {
                EndPoint = new Point(endPoint.Split(',')[0].ConvertTo<double>(), endPoint.Split(',')[1].ConvertTo<double>());
            }

            var beginActivityId = xelement.GetAttributeValue("BeginActivityId", string.Empty);
            var endActivityId = xelement.GetAttributeValue("EndActivityId", string.Empty);
            Type = (RuleType)EnumViewModel.LabelToEnum(xelement.GetAttributeValue(nameof(Type), string.Empty), typeof(RuleType));
            BeginLeft = xelement.GetAttributeValue(nameof(BeginLeft), 0d);
            BeginTop = xelement.GetAttributeValue(nameof(BeginTop), 0d);
            EndLeft = xelement.GetAttributeValue(nameof(EndLeft), 0d);
            EndTop = xelement.GetAttributeValue(nameof(EndTop), 0d);
            TextLeft = xelement.GetAttributeValue(nameof(TextLeft), 0d);
            TextTop = xelement.GetAttributeValue(nameof(TextTop), 0d);
            Left1 = xelement.GetAttributeValue(nameof(Left1), 0d);
            Left2 = xelement.GetAttributeValue(nameof(Left2), 0d);
            Top1 = xelement.GetAttributeValue(nameof(Top1), 0d);
            Top2 = xelement.GetAttributeValue(nameof(Top2), 0d);
            Color = xelement.GetAttributeValue(nameof(Color), string.Empty);
            SourceActivityId = xelement.GetAttributeValue(nameof(SourceActivityId), string.Empty);
            if (!beginActivityId.IsNullOrWhiteSpace())
            {
                BeginActivity = new ActivityModel();
                BeginActivity.Id = beginActivityId;
                if (isCopy)
                {
                    BeginActivity.CopyId = beginActivityId;
                    BeginActivity.Id = Guid.NewGuid().ToString();
                }
            }

            if (!endActivityId.IsNullOrWhiteSpace())
            {
                EndActivity = new ActivityModel();
                EndActivity.Id = endActivityId;
                if (isCopy)
                {
                    EndActivity.CopyId = endActivityId;
                    EndActivity.Id = Guid.NewGuid().ToString();
                }
            }
        }

        /// <summary>
        /// 保存验证
        /// </summary>
        public override void ValidateSave()
        {
            if (BeginActivity == null || EndActivity == null)
            {
                throw new ValidationException("工艺流程的规则未绑定到活动工序".L10N());
            }
        }

        #region 属性
        /// <summary>
        /// 开始活动
        /// </summary>
        public IActivity BeginActivity
        {
            get { return GetProperty<IActivity>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 开始位置
        /// </summary>
        public Point StartPoint
        {
            get { return GetProperty<Point>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 结束活动
        /// </summary>
        public IActivity EndActivity
        {
            get { return GetProperty<IActivity>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 位置1
        /// </summary>
        public Point Point1
        {
            get { return GetProperty<Point>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 规则类型
        /// </summary>
        public RuleType Type
        {
            get { return GetProperty<RuleType>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 位置2
        /// </summary>
        public Point Point2
        {
            get { return GetProperty<Point>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 结束位置
        /// </summary>
        public Point EndPoint
        {
            get { return GetProperty<Point>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 规则起点距容器左侧长度
        /// </summary>
        public double BeginLeft
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 规则起点距容器顶部长度
        /// </summary>
        public double BeginTop
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 规则终点距容器左侧长度
        /// </summary>
        public double EndLeft
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 规则终点距容器顶部长度
        /// </summary>
        public double EndTop
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 规则名称距容器左侧长度
        /// </summary>
        public double TextLeft
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 规则名称距容器顶部长度
        /// </summary>
        public double TextTop
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工序参数Id
        /// </summary>
        public double ParameterId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 工序结果类型
        /// </summary>
        public ResultTypeForDesign ParamResultType
        {
            get { return GetProperty<ResultTypeForDesign>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 脚本
        /// </summary>
        public string Expression
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 源活动ID
        /// </summary>
        public string SourceActivityId
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 位置1距离容器左侧长度
        /// </summary>
        public double Left1
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 位置1距离容器顶部长度
        /// </summary>
        public double Top1
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 位置2距离容器左侧长度
        /// </summary>
        public double Left2
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 位置2距离容器顶部长度
        /// </summary>
        public double Top2
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 线条颜色
        /// </summary>
        public string Color
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
        #endregion
    }
}
