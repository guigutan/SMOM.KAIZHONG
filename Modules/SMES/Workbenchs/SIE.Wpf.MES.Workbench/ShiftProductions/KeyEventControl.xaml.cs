using DevExpress.Xpf.Core;
using SIE.Wpf.Common.Diagram;
using System;
using System.ComponentModel;
using System.Windows.Threading;

namespace SIE.Wpf.MES.Workbench.ShiftProductions
{
    /// <summary>
    /// KeyEventControl.xaml 的交互逻辑
    /// </summary>
    [Category("过程分析")]
    public partial class KeyEventControl : ComponentItem, IfaceKeyEvent
    {
        /// <summary>
        /// 计时器
        /// </summary>
        private DispatcherTimer timer;

        /// <summary>
        /// 配置属性(例如刷新时间)
        /// </summary>
        KeyEventProperty _property;

        /// <summary>
        /// 构造函数
        /// </summary>
        public KeyEventControl()
        {
            InitializeComponent();
            _property = UseProperty<KeyEventProperty>();
        }

        /// <summary>
        /// 组件关闭
        /// </summary>
        protected override void OnClose()
        {
            base.OnClose();
            timer?.Stop();
            timer = null;
        }

        /// <summary>
        /// 运行后处理方法
        /// </summary>
        protected override void OnRun()
        {
            base.OnRun();
            TimerIni(_property.TimeSpan <= 0 ? 3 : _property.TimeSpan);
            Refresh();
        }

        /// <summary>
        /// Timeer计时器初始化
        /// </summary>
        /// <param name="timeSpan">计时器轮询时间</param>
        private void TimerIni(double timeSpan)
        {
            if (timer == null)
                timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMinutes(timeSpan);
            timer.IsEnabled = true;
            timer.Start();
        }

        /// <summary>
        /// 计时器的处理方法
        /// </summary>
        /// <param name="sender">事件发送对象</param>
        /// <param name="e">事件参数</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            SIE.WorkBenchCommon.ExceptionHelper.CatchRemoteException(() =>
            {
                Dispatcher.BeginInvoke(new Action(() =>
                {
                    Refresh();
                }));
            });
        }

        /// <summary>
        /// 关键事项管控刷新方法
        /// </summary>
        public override void Refresh()
        {
            foreach (DXTabItem tabItem in tcItems.Items)
            {
                ((IfaceKeyEvent)tabItem.Content).Refresh();
            }
        }

        /// <summary>
        /// 关键事项管控属性
        /// </summary>
        public class KeyEventProperty : ComponentProperty<KeyEventControl>
        {
            /// <summary>
            /// 刷新时间（分钟）
            /// </summary>
            [DisplayName("刷新间隔(分钟)"), Description("默认刷新时间为3分钟"), Category("自定义")]
            public double TimeSpan { get; set; }
        }
    }
}
