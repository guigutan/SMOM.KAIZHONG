using SIE.Domain;
using SIE.Equipments.Abnormal;
using SIE.MES.DashBoard.DashBoards.ProductionLine;
using SIE.MES.WorkOrders;
using SIE.Resources.Enterprises;
using SIE.Wpf.Common.Diagram;
using SIE.Wpf.MES.DashBoard.DashBoards.Editors;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace SIE.Wpf.MES.DashBoard.DashBoards.ProductionLine
{
    /// <summary>
    /// WorkShopControl.xaml 的交互逻辑
    /// </summary>
    [Category("生产")]
    public partial class WorkShopControl : ComponentItem
    {
        #region 车间当日产线集合 WorkShopInfo        
        /// <summary>
        /// 车间当日产线集合
        /// </summary>
        private ObservableCollection<WorkShopInfo> _workShopInfo;

        /// <summary>
        /// 车间当日产线集合
        /// </summary>
        public ObservableCollection<WorkShopInfo> WorkShopInfo
        {
            get
            {
                if (_workShopInfo == null)
                {
                    _workShopInfo = new ObservableCollection<WorkShopInfo>();
                }

                return _workShopInfo;
            }
        }
        #endregion

        /// <summary>
        /// 时间刷新器
        /// </summary>
        DispatcherTimer timer = new DispatcherTimer();

        /// <summary>
        /// 数据时间刷新器
        /// </summary>
        DispatcherTimer dataTimer = new DispatcherTimer();

        /// <summary>
        /// 车间属性配置
        /// </summary>
        WorkShopControlProperty _property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkShopControl()
        {
            InitializeComponent();
            _property = this.UseProperty<WorkShopControlProperty>();

            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = TimeSpan.FromSeconds(1); //设置刷新的间隔时间
            timer.Start();
        }

        /// <summary>
        /// 运行后
        /// </summary>
        protected override void OnRun()
        {
            Refresh();
            Dispatch();
        }

        /// <summary>
        /// 运行时异步加载
        /// </summary>
        void Dispatch()
        {
            Dispatcher.BeginInvoke(new Action(() =>
            {
                dataTimer.Tick += DataTimer_Tick;
                if (_property.Interval > 0)
                {
                    dataTimer.Interval = TimeSpan.FromSeconds(_property.Interval); //设置刷新的间隔时间
                }
                else
                {
                    dataTimer.Interval = TimeSpan.FromSeconds(300);
                }

                dataTimer.Start();
            }));
        }

        /// <summary>
        /// 刷新当日计划工单
        /// </summary>
        public override void Refresh()
        {
            WorkShopInfo.Clear();

            var woList = RT.Service.Resolve<WorkOrderController>().GetCurrentDayLineList(_property.WorkShop);
            var resourceLst = woList.Select(p => p.Resource).Distinct((x, y) => x.Id == y.Id).ToList();
            foreach (var resource in resourceLst)
            {
                var workHours = RT.Service.Resolve<WorkShopController>().GetworkHours(resource.Id);
                var woResourceLst = woList.Where(p => p.ResourceId == resource.Id).ToList();
                var abnormalCauseLst = RT.Service.Resolve<AbnormalCauseController>().GetAbnormalCauseList(resource.Id);
                var ngQty = RT.Service.Resolve<WorkShopController>().GetNgQty(woResourceLst);
                decimal planQty = 0;
                decimal finishQty = 0;
                foreach (var woResource in woResourceLst)
                {
                    planQty += woResource.PlanQty;
                    finishQty += woResource.FinishQty;
                }

                WorkShopInfo workShopEntity = new WorkShopInfo();
                workShopEntity.Line = resource.Name;
                workShopEntity.PlanQty = decimal.Round(planQty, 2);
                workShopEntity.FinishQty = decimal.Round(finishQty, 2);
                if (finishQty == 0)
                {
                    workShopEntity.HourOutputQty = 0;
                    workShopEntity.ThroughRate = 0;
                    workShopEntity.State = "休息";
                }
                else
                {
                    if (workHours > 0)
                        workShopEntity.HourOutputQty = decimal.Round(finishQty / Convert.ToDecimal(workHours), 2);
                    workShopEntity.ThroughRate = decimal.Round((finishQty - ngQty) / finishQty, 4);
                    if (abnormalCauseLst != null && abnormalCauseLst.Count > 0)
                        workShopEntity.State = "停线";
                    else
                        workShopEntity.State = "正常";
                }

                WorkShopInfo.Add(workShopEntity);
            }
        }

        /// <summary>
        /// 超过计时器间隔时发生事件
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void DataTimer_Tick(object sender, EventArgs e)
        {
            Refresh();
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
    /// 组件属性
    /// </summary>
    public class WorkShopControlProperty : ComponentProperty<WorkShopControl>
    {
        /// <summary>
        /// 选择Logo
        /// </summary>
        string _source;

        /// <summary>
        /// 选择Logo
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

        /// <summary>
        /// 刷新时间(s)
        /// </summary>
        private int _interval = 300;

        /// <summary>
        /// 刷新时间(s)
        /// </summary>
        [Category("数据区"), DisplayName("刷新时间(s)"), Description("刷新时间(s)")]
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
        /// 车间
        /// </summary>
        [Category("数据区"), DisplayName("车间"), Description("车间"), TypeConverter(typeof(GetWorkShop))]
        public string WorkShop { get; set; }

        /// <summary>
        /// 标题内容
        /// </summary>
        [Category("标题设置"), DisplayName("标题"), Description("标题内容")]
        public string WorkShopTitle
        {
            get { return Item.workShopTitle.Text; }
            set { Item.workShopTitle.Text = value; }
        }

        /// <summary>
        /// 字体类型
        /// </summary>
        [Category("标题设置"), DisplayName("字体类型"), Description("字体类型")]
        public FontFamily FontFamily
        {
            get { return Item.workShopTitle.FontFamily; }
            set { Item.workShopTitle.FontFamily = value; }
        }

        /// <summary>
        /// 字体大小
        /// </summary>
        [Category("标题设置"), DisplayName("字体大小"), Description("字体大小")]
        public double FontSize
        {
            get { return Item.workShopTitle.FontSize; }
            set { Item.workShopTitle.FontSize = value; }
        }

        /// <summary>
        /// 是否加粗
        /// </summary>
        [Category("标题设置"), DisplayName("是否加粗"), Description("是否加粗")]
        public FontWeight FontWeight
        {
            get { return Item.workShopTitle.FontWeight; }
            set { Item.workShopTitle.FontWeight = value; }
        }
    }

    /// <summary>
    /// 获取车间信息
    /// </summary>
    /// <seealso cref="System.ComponentModel.StringConverter" />
    public class GetWorkShop : StringConverter
    {
        /// <summary>
        /// 使用指定的上下文返回此对象是否支持可以从列表中选取的标准值集。
        /// </summary>
        /// <param name="context">一个提供格式上下文的 <see cref="T:System.ComponentModel.ITypeDescriptorContext" />。</param>
        /// <returns>
        /// 如果应调用 <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> 来查找对象支持的一组公共值，则为 true；否则，为 false。
        /// </returns>
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        /// <summary>
        /// 当与格式上下文一起提供时，返回此类型转换器设计用于的数据类型的标准值集合。
        /// </summary>
        /// <param name="context">提供格式上下文的 <see cref="T:System.ComponentModel.ITypeDescriptorContext" />，可用来提取有关从中调用此转换器的环境的附加信息。此参数或其属性可以为 null。</param>
        /// <returns>
        /// 包含标准有效值集的 <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" />；如果数据类型不支持标准值集，则为 null。
        /// </returns>
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var workShopLst = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Shop);
            var workShopArray = workShopLst.Select(p => p.Name).ToArray();
            return new StandardValuesCollection(workShopArray);
        }

        /// <summary>
        /// 使用指定的上下文返回从 <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> 返回的标准值的集合是否为可能值的独占列表。
        /// </summary>
        /// <param name="context">一个提供格式上下文的 <see cref="T:System.ComponentModel.ITypeDescriptorContext" />。</param>
        /// <returns>
        /// 如果从 <see cref="M:System.ComponentModel.TypeConverter.GetStandardValues" /> 返回的 <see cref="T:System.ComponentModel.TypeConverter.StandardValuesCollection" /> 是可能值的穷举列表，则为 true；如果还可能有其他值，则为 false。
        /// </returns>
        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
