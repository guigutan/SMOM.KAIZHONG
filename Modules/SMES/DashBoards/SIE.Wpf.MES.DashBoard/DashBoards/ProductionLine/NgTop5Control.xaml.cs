using DevExpress.Xpf.Charts;
using SIE.Domain;
using SIE.MES.DashBoard.DashBoards.ProductionLine;
using SIE.Resources.ShiftTypes;
using SIE.Resources.WipResources;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.DashBoard.DashBoards.Editors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;

namespace SIE.Wpf.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// NgTop5Control.xaml 的交互逻辑
    /// </summary>
    [Category("生产")]
    public partial class NgTop5Control : ComponentItem
    {
        /// <summary>
        /// 产线id
        /// </summary>
        double _lineId;

        /// <summary>
        /// 当班班次
        /// </summary>
        Shift _shift;

        /// <summary>
        /// 当前时间
        /// </summary>
        DateTime _currentTime = DateTime.Now;

        /// <summary>
        /// 数据刷新器
        /// </summary>
        DispatcherTimer _timer;

        /// <summary>
        /// 数据区属性
        /// </summary>
        NgTop5ControlProperty _property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public NgTop5Control()
        {
            InitializeComponent();
            BindingPieChart();
            _timer = new DispatcherTimer();
            _property = this.UseProperty<NgTop5ControlProperty>();
        }

        /// <summary>
        /// 运行时事件
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            Dispatch();
            BindingPieChart();
        }

        /// <summary>
        /// 运行时异步加载
        /// </summary>
        void Dispatch()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                _timer.Tick += Timer_Tick;
                if (_property.Interval > 0)
                {
                    _timer.Interval = TimeSpan.FromSeconds(_property.Interval); //设置刷新的间隔时间
                }
                else
                {
                    _timer.Interval = TimeSpan.FromSeconds(60);
                }

                _timer.Start();
            }));
        }

        /// <summary>
        /// 设置时间间隔器动作
        /// </summary>
        /// <param name="sender">源对象</param>
        /// <param name="e">源参数</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            BindingPieChart();
        }

        /// <summary>
        /// 获取产线id
        /// </summary>
        /// <returns>产线id</returns>
        public double GetLineId()
        {
            double lineId = 0;
            if (_property == null
                || _property.ShopAndLine == null
                || _property.ShopAndLine.Lines.IsNullOrEmpty())
            {
                return lineId;
            }

            var lineIds = _property.ShopAndLine.Lines?.Split(';');
            if (lineIds != null)
            {
                string lId = lineIds[0];
                double.TryParse(lId, out lineId);
            }

            return lineId;
        }

        /// <summary>
        /// 数据校验
        /// </summary>
        /// <returns>验证结果</returns>
        public bool Validation()
        {
            _lineId = GetLineId();
            if (_lineId == 0)
            {
                return false;
            }

            _currentTime = RF.Find<Shift>().GetDbTime();


            var shiftType = RT.Service.Resolve<WipResourceController>().GetWipResourceShiftType(_lineId, _currentTime);
            if (shiftType == null)
            {
                //班制为空
                return false;
            }

            _shift = RT.Service.Resolve<ProductionLineController>().GetRealShiftByShiftTypeId(_currentTime, shiftType.Id);
            if (_shift == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 绑定饼图
        /// </summary>
        public void BindingPieChart()
        {
            var xyDiagram = (SimpleDiagram2D)ngPieChart.Diagram;
            List<SeriesPoint> ngPoints = new List<SeriesPoint>();
            var debrisSeries = (xyDiagram.Series.FirstOrDefault() as PieSeries2D);
            debrisSeries.Points.Clear();
            debrisSeries.SweepDirection = PieSweepDirection.Counterclockwise;
            debrisSeries.Rotation = 90;
            if (!Validation())
            {
                return;
            }

            Dictionary<string, double> defecDic = new Dictionary<string, double> { };

            DateTime shiftBeginTime = _shift.BeginTime;
            DateTime shiftEndTime = _shift.EndTime;
            var wipProductDefectList = RT.Service.Resolve<ProductionLineController>().GetWipProductDefectList(_lineId, shiftBeginTime, shiftEndTime);
            if (wipProductDefectList == null || !wipProductDefectList.Any())
            {
                return;
            }

            foreach (var item in wipProductDefectList)
            {
                if (item != null
                    && item.Defect != null
                    && item.Defect.DefectCategory != null
                    && !item.Defect.DefectCategory.Description.IsNullOrEmpty())
                {
                    string defectCate = item.Defect.DefectCategory.Description;
                    if (defecDic.ContainsKey(defectCate))
                    {
                        double ngQty = defecDic[defectCate];
                        ngQty += (double)item.NgQty;
                        defecDic[defectCate] = ngQty;
                    }
                    else
                    {
                        defecDic.Add(defectCate, (double)item.NgQty);
                    }
                }
            }

            var orderList = defecDic.OrderByDescending(p => p.Value).Take(7);
            int i = 1;
            foreach (var dic in orderList)
            {
                ngPoints.Add(new SeriesPoint() { Argument = dic.Key, Value = dic.Value, Brush = GetLineColor(i) });
                i++;
            }

            debrisSeries.Points.AddRange(ngPoints);
            ngPieChart.UpdateData();
        }

        /// <summary>
        /// 根据不理top值获取颜色
        /// </summary>
        /// <param name="num">数值</param>
        /// <returns>颜色</returns>
        private SolidColorBrush GetLineColor(int num)
        {
            var solidColorBrush = new SolidColorBrush();
            Color color;

            switch (num)
            {
                case 1:
                    color = (Color)ColorConverter.ConvertFromString("#166CF7");
                    break;

                case 2:
                    color = (Color)ColorConverter.ConvertFromString("#18C106");
                    break;

                case 3:
                    color = (Color)ColorConverter.ConvertFromString("#F9E600");
                    break;

                case 4:
                    color = (Color)ColorConverter.ConvertFromString("#FF6700");
                    break;

                case 5:
                    color = (Color)ColorConverter.ConvertFromString("#FF2746");
                    break;
                default:
                    color = (Color)ColorConverter.ConvertFromString("#FFFFFF");
                    break;
            }

            solidColorBrush.Color = color;
            return solidColorBrush;
        }
    }

    /// <summary>
    /// 图表属性值设置
    /// </summary>
    public class NgTop5ControlProperty : ComponentProperty<NgTop5Control>
    {
        /// <summary>
        /// 刷新频率(秒)
        /// </summary>
        private int _interval = 300;

        /// <summary>
        /// 刷新频率
        /// </summary>
        [Category("数据过滤"), DisplayName("刷新频率(秒)"), Description("刷新频率(秒)")]
        public int Interval
        {
            get
            {
                return _interval;
            }

            set
            {
                if (value < 0)
                {
                    _interval = 300;
                }
                else
                {
                    _interval = value;
                }
            }
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
    }
}
