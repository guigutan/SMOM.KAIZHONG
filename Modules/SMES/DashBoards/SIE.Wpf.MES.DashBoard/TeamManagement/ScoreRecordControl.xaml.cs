using SIE.Domain;
using SIE.MES.TeamManagement.ScoreRecords;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.DashBoard.Editors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SIE.Wpf.MES.DashBoard.TeamManagement
{
    /// <summary>
    /// DisplayBillDataControl.xaml 的交互逻辑
    /// </summary>
    [Category("生产")]
    public partial class ScoreRecordControl : ComponentItem
    {
        /// <summary>
        /// 全部数据刷新时间控制器
        /// </summary>
        DispatcherTimer allDataRefreshTimer;

        /// <summary>
        /// 明细数据换页时间控制器
        /// </summary>
        DispatcherTimer detailTimer;

        /// <summary>
        /// 列表数据换页时间控制器
        /// </summary>
        DispatcherTimer listTimer;

        double widthRate = 1;
        double heightRate = 1;

        /// <summary>
        /// 明细的索引
        /// </summary>
        int _nowIndex_Detail ;

        /// <summary>
        /// 列表索引
        /// </summary>      
        int _nowIndex_List ;

        /// <summary>
        /// 列表一页显示数量
        /// </summary>
        int _listCount = 8;

        /// <summary>
        /// 数据库时间
        /// </summary>
        DateTime _dbtime;

        /// <summary>
        /// 用户显示区设置的属性
        /// </summary>
        ScoreRecordControlProperty _property;

        /// <summary>
        /// 数据列表8
        /// </summary>
        private ObservableCollection<ScoreRecordEntity> _listDatas;

        /// <summary>
        /// 明细数据4
        /// </summary>
        private ObservableCollection<AddScoreRecordEntity> _detailDatas;

        /// <summary>
        /// 有效人员集合
        /// </summary>
        EntityList<EmployeeResource> _empList;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ScoreRecordControl()
        {
            allDataRefreshTimer = new DispatcherTimer();
            listTimer = new DispatcherTimer();
            detailTimer = new DispatcherTimer();
            InitializeComponent();
            _property = this.UseProperty<ScoreRecordControlProperty>();
        }

        /// <summary>
        /// 第一次加载控制
        /// </summary>
        bool isFirst = true;

        /// <summary>
        /// 运行时事件
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            if (isFirst)
            {
                ResetAllItemWidthHeight();
                _listCount = (int)Math.Floor(displayGrid.ActualHeight / 80) - 1;
                if (_listCount < 0)
                {
                    _listCount = 1;//列表显示数量不能为负数
                }
                isFirst = false;
            }

            if (!GetShopAndLineData())
            {
                return;
            }
            _dbtime = RF.Find<ScoreRecord>().GetDbTime().Date;
            SetMonthWeekRange(_dbtime);
            InitAllData();
            Dispatch();
        }

        /// <summary>
        /// 运行时异步加载
        /// </summary>
        void Dispatch()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                allDataRefreshTimer.Tick += AllDataRefreshTimer_Tick;
                detailTimer.Tick += DetailTimer_Tick;
                listTimer.Tick += ListTimer_Tick;

                if (_property.RefreshItv != null)
                {
                    SetInterval(allDataRefreshTimer, _property.RefreshItv);
                }
                else
                {
                    allDataRefreshTimer.Interval = TimeSpan.FromSeconds(300);
                }

                if (_property.ListDataChangeItv != null)
                {
                    SetInterval(listTimer, _property.ListDataChangeItv);
                }
                else
                {
                    listTimer.Interval = TimeSpan.FromSeconds(10);
                }

                if (_property.DetailDataChangeItv != null)
                {
                    SetInterval(detailTimer, _property.DetailDataChangeItv);
                }
                else
                {
                    detailTimer.Interval = TimeSpan.FromSeconds(5);
                }

                allDataRefreshTimer.Start();
                detailTimer.Start();
                listTimer.Start();
            }));
        }

        /// <summary>
        /// 列表数据时间触发翻页
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void ListTimer_Tick(object sender, EventArgs e)
        {
            ShowListData();
        }

        /// <summary>
        /// 明细数据时间触发翻页
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void DetailTimer_Tick(object sender, EventArgs e)
        {
            ShowDetailData();
        }

        /// <summary>
        /// 全部数据刷新触发
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void AllDataRefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshAllData();
        }

        /// <summary>
        /// 刷新所有数据
        /// </summary>
        private void RefreshAllData()
        {
            _nowIndex_List = 0;
            _nowIndex_Detail = 0;
            listTimer.Stop();
            detailTimer.Stop();
            allDataRefreshTimer.Stop();
            InitAllData();
            listTimer.Start();
            detailTimer.Start();
            allDataRefreshTimer.Start();
        }

        /// <summary>
        /// 初始化所有数据
        /// </summary>
        private void InitAllData()
        {
            SetListData();
            SetDetailData();
            ShowListData();
            ShowDetailData();
        }

        #region 数据加载

        #region 列表数据

        /// <summary>
        /// 设置列表数据和前三名数据
        /// </summary>
        private void SetListData()
        {
            _listDatas = new ObservableCollection<ScoreRecordEntity>();
            var line = _property.ShopAndLine.Line;
            var workshop = _property.ShopAndLine.Shop;

            _empList = new EntityList<EmployeeResource>();
            if (line == null && workshop == null)
            {
                return;
            }
            else
            {
                if (line.HasValue)
                {
                    _empList = RT.Service.Resolve<EmployeeController>().GetEmployeeResourcesByResId(line.Value);
                }
                else
                {
                    _empList = RT.Service.Resolve<EmployeeController>().GetEmployeeResourcesByShopId(workshop.Value);
                }
            }

            if (_empList.Count == 0)
            {
                return;
            }
            DateTime startMonth = _dbtime.AddDays(1 - _dbtime.Day).Date;
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1).Date;
            if (endMonth > _dbtime)
            {
                endMonth = _dbtime.Date;
            }
            DateRange drMonth = new DateRange()
            {
                BeginValue = startMonth,
                EndValue = endMonth
            };
            var records = RT.Service.Resolve<ScoreRecordController>().GetScoreRecords(drMonth, _empList.Select(p => p.EmployeeId).ToList());
            SetBillData(records, _dbtime);
            SetTop3Data();
        }

        /// <summary>
        /// 设置主列表的数据
        /// </summary>
        /// <param name="records">评分记录</param>
        /// <param name="now">时间</param>
        private void SetBillData(EntityList<ScoreRecord> records, DateTime now)
        {
            DateRange dr1;
            dicWeekRange.TryGetValue(1, out dr1);
            DateRange dr2;
            dicWeekRange.TryGetValue(2, out dr2);
            DateRange dr3;
            dicWeekRange.TryGetValue(3, out dr3);
            DateRange dr4;
            dicWeekRange.TryGetValue(4, out dr4);
            DateRange dr5;
            dicWeekRange.TryGetValue(5, out dr5);
            List<DateRange> drs = new List<DateRange>() { dr1, dr2, dr3, dr4, dr5 };
            records.GroupBy(p => p.EmployeeId).ForEach(e =>
            {
                var emp = records.FirstOrDefault(f => f.EmployeeId == e.Key);
                List<string> strs = new List<string>();
                ScoreRecordEntity score = new ScoreRecordEntity();
                var curList = e.ToList();
                score.EmployeeName = emp.Employee.Name;
                score.EmployeeId = emp.EmployeeId;
                score.WorkGroupName = emp.Employee.WorkGroup.Name;
                foreach (var dr in drs)
                {
                    if (dr != null && dr.BeginValue <= now)
                    {
                        var sumScore = curList.Where(p => p.OccurDate >= dr.BeginValue.Value && p.OccurDate < dr.EndValue.Value.AddDays(1)).Sum(p => p.Score);
                        if (sumScore > 0)
                        {
                            strs.Add("+" + sumScore);
                        }
                        else
                        {
                            strs.Add(sumScore.ToString());
                        }
                    }
                    else
                    {
                        strs.Add("--");
                    }
                }

                score.FirstWeek = strs[0];
                score.SecondWeek = strs[1];
                score.ThirdWeek = strs[2];
                score.FourthWeek = strs[3];
                score.FifthWeek = strs[4];
                score.MonthScore = curList.Sum(p => p.Score) + 100;
                if (score.MonthScore >= 90)
                {
                    //优
                    score.ScorePic = @"../Images/youxiu.png";
                }
                else if (score.MonthScore < 90 && score.MonthScore >= 80)
                {
                    //良
                    score.ScorePic = @"../Images/lianghao.png";
                }
                else if (score.MonthScore < 80 && score.MonthScore >= 60)
                {
                    //合格
                    score.ScorePic = @"../Images/jige.png";
                }
                else
                {
                    //不良
                    score.ScorePic = @"../Images/buliang.png";
                }
                score.cellHeight = 80 * heightRate;
                score.cellFontSize = 29 * widthRate;
                score.headFontSize = 30 * widthRate;
                score.imgHeight = 41 * heightRate;
                score.imgWidth = 84 * widthRate;
                score.imgRowWidth = 200 * widthRate;
                score.headHeight = 80 * heightRate;
                _listDatas.Add(score);
            });
            int s = 1;
            _listDatas.OrderByDescending(p => p.MonthScore).Take(_property.DataCount).ForEach(p =>
            {
                p.RowNum = s;
                s++;
            });
        }

        /// <summary>
        /// 设置前三名数据
        /// </summary>       
        private void SetTop3Data()
        {
            var top3empIds = _listDatas.OrderByDescending(p => p.MonthScore).Take(3).Select(p => p.EmployeeId).ToList();
            for (int i = 0; i < top3empIds.Count; i++)
            {
                var emp = _empList.Where(p => p.EmployeeId == top3empIds[i]).Select(p => p.Employee).FirstOrDefault();
                var grid = top3Grid.Children[i + 1] as System.Windows.Controls.Grid;
                var stack = grid.Children[1] as System.Windows.Controls.StackPanel;
                var img = stack.Children[0] as System.Windows.Controls.Image;
                var la = stack.Children[1] as System.Windows.Controls.Label;
                la.Content = emp.Name;
                if (emp.Photo != null && emp.Photo.Length > 0)
                {
                    //保存的二进制数据转换成图像失败，则显示默认的图
                    try
                    {
                        img.Source = GetImageSource(emp.Photo);
                    }
                    catch (Exception exc)
                    {
                        RT.Logger.Error(exc.Message);
                    }
                }

                grid.Visibility = System.Windows.Visibility.Visible;
            }

            var g = top3Grid.Children[1] as System.Windows.Controls.Grid;
            g.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// 展示列表数据
        /// </summary>
        private void ShowListData()
        {
            if (_listDatas == null || _listDatas.Count == 0)
            {
                return;
            }
            int cc = _listCount;
            var rst = _listDatas.OrderBy(p => p.RowNum).Skip(_nowIndex_List * cc).Take(cc).ToList();
            displayGrid.DataContext = rst;
            if (rst.Any())
            {
                if (rst[0].RowNum > _listDatas.Count - cc)
                {
                    //最后一组回归0
                    _nowIndex_List = 0;
                }
                else
                {
                    _nowIndex_List++;
                }
            }
        }
        #endregion

        #region 明细数据
        /// <summary>
        /// 设置明细的数据
        /// </summary>
        private void SetDetailData()
        {
            DateRange dr = new DateRange() { BeginValue = _dbtime, EndValue = _dbtime };
            var list = RT.Service.Resolve<ScoreRecordController>().GetScoreRecordsForDashBoardDetail(dr, _empList.Select(p => p.EmployeeId).ToList());
            int i = 1;
            _detailDatas = new ObservableCollection<AddScoreRecordEntity>();
            foreach (var item in list.OrderByDescending(p => p.OccurDate))
            {
                AddScoreRecordEntity a = new AddScoreRecordEntity();
                a.RowNum = i;
                string score = item.Score > 0 ? "+" + item.Score : item.Score.ToString();
                a.DetailStr = score + "  " + item.Employee.Name + "【" + item.Employee.WorkGroup.Name + "】调优建议被采纳";
                _detailDatas.Add(a);
                i++;
            }
        }

        /// <summary>
        /// 时间触发显示员工加分项数据
        /// </summary>
        private void ShowDetailData()
        {
            if (_detailDatas.Count == 0)
            {
                return;
            }
            var rst = _detailDatas.Skip(_nowIndex_Detail * 4).Take(4).OrderBy(p => p.RowNum).ToList();
            for (int i = 0; i < 4; i++)
            {
                var stackPanel = bottomGrid.Children[i] as System.Windows.Controls.StackPanel;
                var la = stackPanel.Children[1] as System.Windows.Controls.Label;
                var img = stackPanel.Children[0] as System.Windows.Controls.Image;
                if (i + 1 > rst.Count)
                {
                    la.Content = string.Empty;
                }
                else
                {
                    la.Content = rst[i].DetailStr;
                    img.Visibility = System.Windows.Visibility.Visible;
                }
            }

            if (rst[0].RowNum > _detailDatas.Count - 4)
            {
                //最后一组回归0
                _nowIndex_Detail = 0;
            }
            else
            {
                _nowIndex_Detail++;
            }
        }
        #endregion

        #endregion

        /// <summary>
        /// 设置间隔时间
        /// </summary>
        /// <param name="timer">事件触发器</param>
        /// <param name="timeInv">时间间隔类</param>
        void SetInterval(DispatcherTimer timer, TimeInterval timeInv)
        {
            switch (timeInv.TimePart)
            {
                case TimePart.Hours:
                    timer.Interval = TimeSpan.FromHours(timeInv.TimeValue); //设置滚动的间隔时间
                    break;
                case TimePart.Minutes:
                    timer.Interval = TimeSpan.FromMinutes(timeInv.TimeValue); //设置滚动的间隔时间
                    break;
                case TimePart.Seconds:
                    timer.Interval = TimeSpan.FromSeconds(timeInv.TimeValue); //设置滚动的间隔时间
                    break;
                case TimePart.Days:
                    timer.Interval = TimeSpan.FromDays(timeInv.TimeValue); //设置滚动的间隔时间
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 运行时刷新事件
        /// </summary>
        public override void Refresh()
        {
            base.Refresh();
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        protected override void OnClose()
        {
            allDataRefreshTimer.Stop();
            listTimer.Stop();
            detailTimer.Stop();
            base.OnClose();
        }

        /// <summary>
        /// 当月每周的日期范围
        /// </summary>
        private Dictionary<int, DateRange> dicWeekRange = new Dictionary<int, DateRange>();

        /// <summary>
        /// 设置当月每周的日期范围
        /// </summary>
        /// <param name="dt">时间</param>
        private void SetMonthWeekRange(DateTime dt)
        {
            DateTime startMonth = dt.AddDays(1 - dt.Day).Date;  //本月月初  
            DateTime endMonth = startMonth.AddMonths(1).AddDays(-1).Date;  //本月月末  
            int startOfWeek = Convert.ToInt32(startMonth.DayOfWeek.ToString("d"));
            if (startOfWeek == 0)
            {
                startOfWeek = 7;
            }
            int i = 1;
            while (startMonth < endMonth)
            {
                DateTime weekEndDate = startMonth.AddDays(7 - startOfWeek);
                if (weekEndDate > endMonth)
                {
                    weekEndDate = endMonth;
                }
                dicWeekRange.Add(i, new DateRange()
                {
                    BeginValue = startMonth,
                    EndValue = weekEndDate
                });
                startMonth = weekEndDate.AddDays(1);
                startOfWeek = 1;
                i++;
            }
        }

        /// <summary>
        /// 二进制转换成image的source
        /// </summary>
        /// <param name="bte">二进制</param>
        /// <returns>BitmapImage</returns>
        private BitmapImage GetImageSource(byte[] bte)
        {
            BitmapImage bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = new MemoryStream(bte);
            bmp.EndInit();
            return bmp;
        }

        /// <summary>
        /// 获取车间产线数据
        /// </summary>
        /// <returns>bool 获取成功</returns>
        private bool GetShopAndLineData()
        {
            if (!_property.ShopAndLine.Line.HasValue && !_property.ShopAndLine.Shop.HasValue)
            {
                laTitle.Content = "请先配置车间或产线-绩效评分看板";
                return false;
            }

            var shop = RF.GetById<Enterprise>(_property.ShopAndLine.Shop);
            string str = shop.Name;

            if (_property.ShopAndLine.Line.HasValue)
            {
                var line = RF.GetById<WipResource>(_property.ShopAndLine.Line);
                str += " - " + line.Name;
            }

            str += " - 绩效评分看板";
            laTitle.Content = str;
            if (str.Length > 15)
            {
                laTitle.FontSize = laTitle.FontSize - 2 * heightRate;
                if (str.Length > 20)
                {
                    laTitle.Width = laTitle.Width + 35 * widthRate * (str.Length - 20);
                }
                if (laTitle.Width > 1000 * widthRate)
                {
                    laTitle.Width = 1000 * widthRate;
                    laTitle.FontSize = laTitle.FontSize - 5 * heightRate;
                }
            }

            return true;
        }

        /// <summary>
        /// 根据设定的宽高重设定看板各控件的宽高
        /// </summary>
        private void ResetAllItemWidthHeight()
        {
            var winHeight = this.ActualHeight;
            var winWidth = this.ActualWidth;
            widthRate = winWidth / 1920;
            heightRate = winHeight / 1080;
            OutestGird.RowDefinitions[0].Height = new GridLength(100 * heightRate);
            laTitle.Width = laTitle.Width * widthRate;
            laTitle.Height = laTitle.Height * heightRate;
            laTitle.FontSize = laTitle.FontSize * heightRate;
            var latop3 = top3Grid.Children[0] as System.Windows.Controls.Label;
            latop3.FontSize = latop3.FontSize * widthRate;
            top3Grid.RowDefinitions[0].Height = new GridLength(95 * heightRate);
            for (int i = 0; i < 3; i++)
            {
                var grid = top3Grid.Children[i + 1] as System.Windows.Controls.Grid;
                var img0 = grid.Children[0] as System.Windows.Controls.Image;
                var stack = grid.Children[1] as System.Windows.Controls.StackPanel;
                var img = stack.Children[0] as System.Windows.Controls.Image;
                var la = stack.Children[1] as System.Windows.Controls.Label;
                img0.Width = img0.Width * widthRate;
                img0.Height = img0.Height * heightRate;
                img.Width = img.Width * widthRate;
                img.Height = img.Height * heightRate;
                la.FontSize = la.FontSize * heightRate;
            }
            //设置数据明细的宽高
            for (int i = 0; i < 4; i++)
            {
                var stackPanel = bottomGrid.Children[i] as System.Windows.Controls.StackPanel;
                var la = stackPanel.Children[1] as System.Windows.Controls.Label;
                var img = stackPanel.Children[0] as System.Windows.Controls.Image;
                la.FontSize = la.FontSize * heightRate;
                img.Width = img.Width * widthRate;
                img.Height = img.Height * heightRate;
                stackPanel.Orientation = System.Windows.Controls.Orientation.Horizontal;
                stackPanel.Margin = new Thickness(30 * widthRate, 0, 0, 0);
            }
            //设置表头的样式
            Style newStyle = new Style();
            newStyle.TargetType = typeof(DataGridColumnHeader);
            newStyle.Setters.Add(new Setter(DataGridColumnHeader.FontSizeProperty, 30 * heightRate));
            newStyle.Setters.Add(new Setter(DataGridColumnHeader.HeightProperty, 80 * heightRate));
            //FromArgb(透明度,red,green,blue)
            newStyle.Setters.Add(new Setter(DataGridColumnHeader.BackgroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(0, 4, 254, 252))));
            newStyle.Setters.Add(new Setter(DataGridColumnHeader.ForegroundProperty, new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 4, 254, 252))));
            newStyle.Setters.Add(new Setter(DataGridColumnHeader.HorizontalContentAlignmentProperty, HorizontalAlignment.Center));
            newStyle.Setters.Add(new Setter(DataGridColumnHeader.VerticalContentAlignmentProperty, VerticalAlignment.Center));
            displayGrid.ColumnHeaderStyle = newStyle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void displayGrid_LayoutUpdated(object sender, EventArgs e)
        {
            // empty
        }
    }

    /// <summary>
    /// 数据表格属性值设置
    /// </summary>
    public class ScoreRecordControlProperty : ComponentProperty<ScoreRecordControl>
    {

        /// <summary>
        /// 滚动间隔时间
        /// </summary>
        [Category("表格设置"), DisplayName("全部数据刷新间隔"), Description("全部数据刷新间隔"), PropertyEditor(typeof(DashBoardTimeIntervalEdit))]
        public TimeInterval RefreshItv { get; set; } = new TimeInterval() { TimePart = TimePart.Minutes, TimeValue = 1 };

        /// <summary>
        /// 滚动间隔时间
        /// </summary>
        [Category("表格设置"), DisplayName("列表数据切换间隔"), Description("列表数据切换间隔"), PropertyEditor(typeof(DashBoardTimeIntervalEdit))]
        public TimeInterval ListDataChangeItv { get; set; } = new TimeInterval() { TimePart = TimePart.Seconds, TimeValue = 10 };

        /// <summary>
        /// 滚动间隔时间
        /// </summary>
        [Category("表格设置"), DisplayName("明细数据切换间隔"), Description("明细数据切换间隔"), PropertyEditor(typeof(DashBoardTimeIntervalEdit))]
        public TimeInterval DetailDataChangeItv { get; set; } = new TimeInterval() { TimePart = TimePart.Seconds, TimeValue = 5 };

        /// <summary>
        /// 列表人数数量
        /// </summary>
        [Category("数据过滤"), DisplayName("列表人数数量"), Description("列表人数数量")]
        public int DataCount { get; set; } = 100;

        /// <summary>
        /// 车间过滤
        /// </summary>
        [Category("数据过滤"), DisplayName("车间产线"), Description("车间产线"), PropertyEditor(typeof(DashBoardShopToLineLookupEdit))]

        public ShopAndLine ShopAndLine { get; set; } = new ShopAndLine();
    }
}
