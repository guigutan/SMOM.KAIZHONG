using Newtonsoft.Json;
using Resources.IconPacks;
using SIE.ComponentModel;
using SIE.WorkBenchChartBase.Commons;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.WorkBenchChartBase.Commons;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SIE.Wpf.WorkBenchChartBase.ChartGroups
{
    /// <summary>
    /// ChartGroup.xaml 的交互逻辑
    /// </summary>
    public partial class ChartGroup : ComponentItem
    {
        internal ChartGroupViewModel viewModel;
        ChartGroupProperty _property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ChartGroup()
        {
            InitializeComponent();
            _property = UseProperty<ChartGroupProperty>();
            viewModel = TrackableInterceptor.Create<ChartGroupViewModel>();
            viewModel.Id = Guid.NewGuid().ToString("N");

            DataContext = viewModel;
        }

        /// <summary>
        /// 运行后
        /// </summary>
        protected override void OnRun()
        {
            viewModel.TimeSpan = _property.TimeSpan;
            viewModel.Run();
        }

        /// <summary>
        /// 关闭后
        /// </summary>
        protected override void OnClose()
        {
            viewModel.Disposes();
            base.OnClose();
        }
    }

    /// <summary>
    /// 工作台控件的属性类
    /// </summary>
    public class ChartGroupProperty : ComponentProperty<ChartGroup>
    {
        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("标题"), Description("组件标题")]
        public virtual string Title
        {
            get { return Item.lblTitle.Text; }
            set { Item.lblTitle.Text = value; }
        }

        /// <summary>
        /// 产品模块
        /// </summary>
        [DisplayName("产品模块"), Description("产品模块，不指定时将加载所有模块")]
        public virtual ModuleFlag Module
        {
            get { return Item.viewModel.Module; }
            set { Item.viewModel.Module = value; }
        }

        /// <summary>
        /// 目标参数类型
        /// </summary>
        [DisplayName("目标参数类型"), Description("目标参数类型，不指定时将加载所有类型")]
        public virtual CategoryFlag ContentCategory
        {
            get { return Item.viewModel.ContentCategory; }
            set { Item.viewModel.ContentCategory = value; }
        }

        /// <summary>
        /// 刷新间隔
        /// </summary>
        [DisplayName("刷新间隔(分钟)"), Description("默认刷新时间为3分钟"), Category("自定义")]
        public double TimeSpan
        {
            get;
            set;
        }

        /// <summary>
        /// 当前图表容器的唯一标识
        /// </summary>
        [Browsable(false)]
        public string Id
        {
            get { return Item.viewModel.Id; }
            set { Item.viewModel.Id = value; }
        }
    }

    /// <summary>
    /// 图表信息的定义
    /// </summary>
    class ChartDefinition : TrackableBase
    {
        /// <summary>
        /// 图表对应的类型
        /// </summary>
        public virtual Type Type { get; set; }

        /// <summary>
        /// 图表的特性
        /// </summary>
        private ChartDefinitionAttribute definition;

        /// <summary>
        /// 图表的特性
        /// </summary>
        public virtual ChartDefinitionAttribute Definition
        {
            get
            {
                if (definition == null)
                    definition = Type.GetCustomAttribute<ChartDefinitionAttribute>();
                return definition;
            }
            set { definition = value; }
        }

        /// <summary>
        /// 是否被勾选
        /// </summary>
        public virtual bool IsSelected { get; set; }

        /// <summary>
        /// 图表的类型（柱形、折线、饼图等）
        /// </summary>
        public PackIconKind Icon { get; set; }
    }

    /// <summary>
    /// 图表对象的定义
    /// </summary>
    class ChartType
    {
        /// <summary>
        /// 图表类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 图表对象
        /// </summary>
        public object Instance { get; set; }
    }

    class ChartGroupViewModel : TrackableBase
    {
        /// <summary>
        /// 记录所有的图表控件
        /// </summary>
        List<ChartDefinition> charts;
        List<ChartDefinition> Charts
        {
            get
            {
                if (charts == null)
                {
                    charts = new List<ChartDefinition>();
                    var types = CRT.GetAllModules().SelectMany(p => p.Assembly.GetTypes());
                    string tempCategory = Enum.GetName(typeof(CategoryFlag), ContentCategory);
                    foreach (var type in types)
                    {
                        var definition = type.GetCustomAttribute<ChartDefinitionAttribute>();
                        if (definition != null && (Module == ModuleFlag.ALL || (definition.Module & Module) == Module)
                            && (ContentCategory == CategoryFlag.全部 || (tempCategory == definition.Category)))
                        {
                            var def = TrackableInterceptor.Create<ChartDefinition>();
                            def.PropertyChanged += Def_PropertyChanged;
                            def.Type = type;
                            if (type.IsSubclassOf(typeof(LineChart)))
                                def.Icon = PackIconKind.ChartLine;
                            else if (type.IsSubclassOf(typeof(PieChart)))
                                def.Icon = PackIconKind.ChartPie;
                            else if (type.IsSubclassOf(typeof(RadarChart)))
                                def.Icon = PackIconKind.ChartBubble;
                            else if (type.IsSubclassOf(typeof(BarChart)))
                                def.Icon = PackIconKind.ChartBar;
                            else
                                def.Icon = PackIconKind.Image;
                            def.Definition = definition;
                            charts.Add(def);
                        }
                    }
                }
                return charts;
            }
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        private void Def_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var def = sender as ChartDefinition;
            if (e.PropertyName == nameof(def.IsSelected))
            {
                if (def.IsSelected)
                    Selection.Add(def);
                else
                    Selection.Remove(def);
            }
        }

        /// <summary>
        /// 选择的图表集合
        /// </summary>
        List<ChartDefinition> Selection { get; } = new List<ChartDefinition>();

        /// <summary>
        /// 当前已选的图表集合
        /// </summary>
        List<ChartType> Selected { get; } = new List<ChartType>();

        /// <summary>
        /// 产品模块
        /// </summary>
        public ModuleFlag Module { get; set; }

        /// <summary>
        /// 图表类型
        /// </summary>
        public CategoryFlag ContentCategory { get; set; }

        /// <summary>
        /// 图表容器唯一标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// 刷新的时间间隔
        /// </summary>
        public double TimeSpan { get; set; }

        /// <summary>
        /// 第一个图表
        /// </summary>
        public virtual object Chart1 { get; set; }

        /// <summary>
        /// 第二个图表
        /// </summary>
        public virtual object Chart2 { get; set; }

        /// <summary>
        /// 第三个图表
        /// </summary>
        public virtual object Chart3 { get; set; }

        /// <summary>
        /// 前一个图表对象
        /// </summary>
        public virtual ICommand PrevCommand { get; }

        /// <summary>
        /// 下一个图表对象
        /// </summary>
        public virtual ICommand NextCommand { get; }

        /// <summary>
        /// 更多图表对象
        /// </summary>
        public virtual ICommand MoreCommand { get; }

        /// <summary>
        /// 记录第一个显示的图表控件的索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ChartGroupViewModel()
        {
            // 初始化点击事件
            PrevCommand = DevExpress.Mvvm.Native.DelegateCommandFactory.Create(Prev, CanPrev);
            NextCommand = DevExpress.Mvvm.Native.DelegateCommandFactory.Create(Next, CanNext);
            MoreCommand = DevExpress.Mvvm.Native.DelegateCommandFactory.Create(ShowMore);
        }

        /// <summary>
        /// 展示图表控件
        /// </summary>
        public void Run()
        {
            var layout = GetLayout();
            if (layout == null)
            {
                Selected.AddRange(Charts.Take(3).Select(p => new ChartType { Type = p.Type }));
                SaveLayout();
            }
            else
            {
                foreach (var type in layout.Selected)
                {
                    var t = Type.GetType(type);
                    if (t != null)
                        Selected.Add(new ChartType { Type = t });
                }
            }
            LoadCharts();
        }

        /// <summary>
        /// 更多的逻辑
        /// </summary>
        private void ShowMore()
        {
            TabControl tabControl = new TabControl();
            Charts.ForEach(p => p.IsSelected = false);
            Selection.Clear();
            foreach (var select in Selected)
            {
                var chart = Charts.FirstOrDefault(p => p.Type == select.Type);
                if (chart != null)
                    chart.IsSelected = true;
            }
            Charts.GroupBy(p => p.Definition.GroupName).ForEach(p =>
            {
                var info = new ChartInfo();
                info.DataContext = p;
                tabControl.Items.Add(
                    new TabItem
                    {
                        Margin = new Thickness(0, 0, 0, 0),
                        Header = p.Key.L10N(),
                        Content = info
                    });
            });
            var result = CRT.Workbench.ShowDialog("MoreDialog", tabControl, v =>
            {
                v.Title = "更多".L10N();
            });
            if (result == 0)
            {
                Selected.Clear();
                Selected.AddRange(Selection.Select(p => new ChartType { Type = p.Type }));
                LoadCharts();
                SaveLayout();
            }
        }

        /// <summary>
        /// 读取配置文件获取需要展示的图表控件
        /// </summary>
        /// <returns>返回展示的图表控件</returns>
        private ChartLayout GetLayout()
        {
            ChartLayout layout = null;
            var file = GetFileName();
            if (File.Exists(file))
            {
                try
                {
                    var json = File.ReadAllText(file);
                    layout = JsonConvert.DeserializeObject<ChartLayout>(json);
                }
                catch (Exception exc)
                {
                    RT.Logger.Error(exc.Message);
                }
            }
            return layout;
        }

        /// <summary>
        /// 获取配置文件的路径
        /// </summary>
        /// <returns></returns>
        private string GetFileName()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Customize");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            return Path.Combine(path, "ChartGroupLayout." + Id + ".json");
        }

        /// <summary>
        /// 保存需要展示的图表控件
        /// </summary>
        private void SaveLayout()
        {
            var file = GetFileName();
            using (var sw = new StreamWriter(file))
            {
                var layout = new ChartLayout();
                layout.Selected.AddRange(Selected.Select(p => p.Type.GetQualifiedName()));
                var json = JsonConvert.SerializeObject(layout);
                sw.Write(json);
            }
        }

        /// <summary>
        /// 加载图表控件
        /// </summary>
        public void LoadCharts()
        {
            if (PageIndex + 3 > Selected.Count)
                PageIndex = Selected.Count - 3;
            if (PageIndex < 0)
                PageIndex = 0;
            Disposes();

            Chart1 = LoadChart(PageIndex);
            Chart2 = LoadChart(PageIndex + 1);
            Chart3 = LoadChart(PageIndex + 2);
            SetTimerInterval(TimeSpan);
        }

        /// <summary>
        /// 根据索引加载图表控件对象
        /// </summary>
        /// <param name="index">展示图表索引</param>
        /// <returns>返回图表控件对象</returns>
        private object LoadChart(int index)
        {
            if (index >= 0 && Selected.Count > index)
            {
                var chartType = Selected[index];
                if (chartType.Instance == null)
                    chartType.Instance = Activator.CreateInstance(chartType.Type);
                return chartType.Instance;
            }
            return null;
        }

        /// <summary>
        /// 上一个
        /// </summary>
        private void Prev()
        {
            PageIndex--;
            LoadCharts();
        }

        /// <summary>
        /// 上一个是否可点击
        /// </summary>
        /// <returns>返回是否可点击</returns>
        private bool CanPrev()
        {
            return PageIndex > 0;
        }

        /// <summary>
        /// 下一个
        /// </summary>
        private void Next()
        {
            PageIndex++;
            LoadCharts();
        }

        /// <summary>
        /// 下一个是否可点击
        /// </summary>
        /// <returns>返回是否可点击</returns>
        private bool CanNext()
        {
            return PageIndex + 3 < Selected.Count;
        }

        /// <summary>
        /// 设置每一个图表控件刷新数据的时间
        /// </summary>
        /// <param name="timeSpan">刷新数据间隔</param>
        public void SetTimerInterval(double timeSpan)
        {
            Selected.Where(p => p.Instance != null).ForEach(p => ((IChart)p.Instance)?.SetTimerInterval(TimeSpan <= 0 ? 3 : TimeSpan));
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        public void Disposes()
        {
            Selected.Where(p => p.Instance != null).ForEach(p => ((IChart)p.Instance)?.Dispose());
        }
    }

    /// <summary>
    /// 图表
    /// </summary>
    class ChartLayout
    {
        public List<string> Selected { get; set; } = new List<string>();
    }
}
