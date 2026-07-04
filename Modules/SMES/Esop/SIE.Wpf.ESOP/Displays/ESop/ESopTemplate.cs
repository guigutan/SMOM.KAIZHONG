using SIE.Dashboard.Definitions;
using SIE.Dashboard.Modules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop.Configs;
using SIE.ESop.Displays;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MetaModel;
using SIE.Packages.Packings;
using SIE.Wpf.Dashboard;
using SIE.Wpf.Dashboard.Templates;
using SIE.Wpf.ESop.Displays;
using SIE.Wpf.ESop.Editors;
using SIE.Wpf.ESOP.ESOPFactory;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.Workbench;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace SIE.Wpf.ESop
{
    /// <summary>
    /// ESOP模板
    /// </summary>
#pragma warning disable S2931 // Classes with "IDisposable" members should implement "IDisposable"
    public class ESopTemplate : DashboardTemplate<ESopViewModel>
#pragma warning restore S2931 // Classes with "IDisposable" members should implement "IDisposable"
    {
        /// <summary>
        /// 当前窗口
        /// </summary>
        private static Window Window { get; set; }

        private const string ENABLE_PROCESS = "ESop.EnableProces";

        /// <summary>
        /// 
        /// </summary>
        private DisplayPoint displayerPoint { get; set; }
        /// <summary>
        /// 元数据关键字
        /// </summary>
        private string MetaKey = "";

        /// <summary>
        /// 选择命令
        /// </summary>
        private readonly SelectOperationCommand cmdSelect = new SelectOperationCommand();
        /// <summary>
        /// 最小化通知图标
        /// </summary>
        private NotifyIcon notifyIcon;

        ///// <summary>
        ///// 当前ESop显示的控件
        ///// </summary>
        //private ShowControl _showControl;

        private FactoryShowControl _showControl;

        /// <summary>
        /// 创建UI后触发
        /// </summary>
        /// <param name="ui">UI控件对象</param>
        protected override void OnUIGenerated(ControlResult ui)
        {
            base.OnUIGenerated(ui);
            var grid = ui.Control as NormalLayout;
            _showControl = new FactoryShowControl();
            grid.Children.Add(_showControl);
            ui.Control.Loaded -= Control_Loaded;
            ui.Control.Loaded += Control_Loaded;
            CommandBinding(ui);
            ui.MainView.DataChanged -= MainView_DataChanged;
            ui.MainView.DataChanged += MainView_DataChanged;
            ui.Control.Loaded += (s, e) =>
            {
                ui.Control.Focusable = true;
                ui.Control.Focus();
            };
            ui.MainView.Closed += (o, e1) =>
            {
                ui.Control.Loaded -= Control_Loaded;
                (ui.MainView.Data as ESopViewModel).OnClose();
                DisableProcess();
                ReleaseNotifyIcon();
                if (Window != null)
                {
                    Window.StateChanged -= Window_StateChanged;
                    Window = null;
                }

                if (_showControl != null)
                {
                    _showControl.Dispose();
                    _showControl = null;
                }
            };
        }

        /// <summary>
        /// 设置命令
        /// </summary>
        /// <param name="ui">控件结果</param>
        private void CommandBinding(ControlResult ui)
        {
            var config = SIE.Common.Configs.ConfigService.GetConfig(new DisplayConfig(), typeof(DisplayPoint));
            object enableProces = null;
            SIE.Context.AppContext.Items.TryGetValue(ENABLE_PROCESS, out enableProces);
            if (!((bool?)enableProces ?? false))
            {
                ui.Control.InputBindings.Add(new KeyBinding
                {
                    Modifiers = (System.Windows.Input.ModifierKeys)config.ModifierKeys,
                    Key = (System.Windows.Input.Key)config.Key,
                    Command = cmdSelect,
                    CommandParameter = ui.MainView
                });
            }

            ui.Control.InputBindings.Add(new KeyBinding
            {
                Modifiers = System.Windows.Input.ModifierKeys.Alt,
                Key = System.Windows.Input.Key.L,
                Command = new DeBugCommand(),
                CommandParameter = ui.MainView
            });
        }

        /// <summary>
        /// 当前页签关闭时触发
        /// </summary>
        /// <param name="sender">当前页签</param>
        /// <param name="e">事件参数</param>
        private void Page_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Window != null)
            {
                Window.StateChanged -= Window_StateChanged;
            }

            ReleaseNotifyIcon();
            DisableProcess();
            if (_showControl != null)
            {
                _showControl.Dispose();
                _showControl = null;
            }
        }

        /// <summary>
        /// 释放最小化图标
        /// </summary>
        private void ReleaseNotifyIcon()
        {
            if (notifyIcon != null)
            {
                notifyIcon.MouseDoubleClick -= OnNotifyIconDoubleClick;
                notifyIcon.Dispose();
                notifyIcon = null;
            }
        }


        /// <summary>
        /// 设置ESOP快捷键可用
        /// </summary>
        private void DisableProcess()
        {
            object enableProces1;
            SIE.Context.AppContext.Items.TryGetValue(ENABLE_PROCESS, out enableProces1);
            if (((bool?)enableProces1 ?? false))
            {
                RT.EventBus.Unsubscribe<SIE.MES.WIP.Workcell>(this);
                SIE.Context.AppContext.Items[ENABLE_PROCESS] = false;
            }
        }

        /// <summary>
        /// 主视图数据变更时触发
        /// </summary>
        /// <param name="sender">主视图</param>
        /// <param name="e">事件参数</param>
        private void MainView_DataChanged(object sender, EventArgs e)
        {
            var view = sender as LogicalView;
            var data = view.Data as ESopViewModel;
            _showControl.PlayEditor = data;

            object enableProces = null;
            SIE.Context.AppContext.Items.TryGetValue(ENABLE_PROCESS, out enableProces);
            if (((bool?)enableProces ?? false))
            {
                object esopWorkstation = null;
                SIE.Context.AppContext.Items.TryGetValue("ESop.Workstation", out esopWorkstation);
                var workcell = (esopWorkstation as Workstation);
                if (workcell == null)
                {
                    throw new ValidationException("工作区丢失".L10N());
                }

                if (workcell.ProcessId == 0 || workcell.ResourceId == 0)
                {
                    CRT.MessageService.ShowMessage("工序与产线,未找到匹配的显示点".L10N());
                }

                displayerPoint = RT.Service.Resolve<DisplayPointController>().GetDisplayPointList(
                                       new DisplayPointCriteria { ResourceId = workcell.ResourceId, ProcessId = workcell.ProcessId }).FirstOrDefault();
                if (displayerPoint == null)
                {
                    CRT.MessageService.ShowMessage("工序与产线,未找到匹配的显示点".L10N());
                }

                data.SetWorkstation(workcell, displayerPoint);
                RT.EventBus.Subscribe<SIE.MES.WIP.Workcell>(this, w =>
                {
                    if (w.ProcessId == 0 || w.ResourceId == 0)
                    {
                        return;
                    }

                    data.SetWorkstation(workcell, RT.Service.Resolve<DisplayPointController>().GetDisplayPointList(
                            new DisplayPointCriteria { ResourceId = w.ResourceId, ProcessId = w.ProcessId }).FirstOrDefault());
                });
            }
            else
            {
                view.Control.Dispatcher.BeginInvoke(new Action(() =>
                {
                    var displayId = data.InitWorkstation();
                    if (displayId != null)
                    {
                        displayerPoint = RF.GetById<DisplayPoint>(displayId);
                        if (Window != null)
                        {
                            var screenNum = (displayerPoint.PlayScreenNum == null || !displayerPoint.PlayScreenNum.HasValue) ? 0 : displayerPoint.PlayScreenNum.Value;
                            var left = ESopWorkstationSelector.SetWindowIndex(screenNum, Window);
                            var meta = WPFModuleMetaExt.GetModuleMeta(cmdSelect.player) as WPFParameterModuleMeta;
                            if (cmdSelect.player != null && meta != null && meta.IsFullScreen)
                            {
                                cmdSelect.player.ShiftFullScreen((int)left);
                                if (meta.IsFullScreen)//如果是全屏 由于框架会执行一次全屏，所以导致此处需要还原全屏再执行全屏
                                {
                                    cmdSelect.player.ShiftFullScreen((int)left);
                                }
                            }
                            Window.Left = left;
                        }
                    }
                }));
            }
            _showControl.DataContext = data;
            System.Windows.Data.Binding binding = new System.Windows.Data.Binding()
            {
                Mode = System.Windows.Data.BindingMode.OneWay,
                Path = new PropertyPath("CurrentPlayDocument", data),
            };
            _showControl.SetBinding(FactoryShowControl.CurrentPlayDocumentProperty, binding);
            _showControl.Play();

        }

        /// <summary>
        /// 控件加载
        /// </summary>
        /// <param name="sender">当前控件</param>
        /// <param name="e">路由事件参数</param>
        private void Control_Loaded(object sender, RoutedEventArgs e)
        {
            Window = Util.TryFindParent<Window>(sender as FrameworkElement);
            Window.StateChanged -= Window_StateChanged;
            Window.StateChanged += Window_StateChanged;
            Window_Loaded(sender, e);
            //设置命令的窗体
            cmdSelect.SourceWindow = Window;
            CreateNotifyIcon();
            if (Window is WpfWorkbench)
            {
                DocumentPage page = Util.TryFindParent<DocumentPage>(sender as FrameworkElement);
                if (page != null)
                {
                    page.Closing -= Page_Closing;
                    page.Closing += Page_Closing;
                }
            }
        }

        #region 初始化窗口以及按钮事件
        /// <summary>
        /// 窗口加载
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var player = Window.GetLogicalChild<DashboardPlayerControl>();
            if (player == null)
            {
                player = Util.TryFindParent<DashboardPlayerControl>(sender as FrameworkElement);
            }
            cmdSelect.player = player;

            var meta = WPFModuleMetaExt.GetModuleMeta(player) as WPFParameterModuleMeta;
            MetaKey = meta.Key;
            _showControl.PlayEditor.MetaKey = this.MetaKey;
            var btnPrevious = player.FindName("btnPrevious") as System.Windows.Controls.Button;//上一页
            var btnNext = player.FindName("btnNext") as System.Windows.Controls.Button;//下一页
            var btnPause = player.FindName("btnPause") as System.Windows.Controls.Button;//暂停
            var btnPlay = player.FindName("btnPlay") as System.Windows.Controls.Button;//播放
            var btnMagnifyAdd = player.FindName("MagnifyAdd") as System.Windows.Controls.Button;//放大
            var btnMagnifyMinus = player.FindName("MagnifyMinus") as System.Windows.Controls.Button;//缩小
            var btnActualSize = player.FindName("ActualSize") as System.Windows.Controls.Button;//1:1
            var btnConfigItem = player.FindName("btnConfigItem") as System.Windows.Controls.Button;
            if (btnConfigItem != null)
            {//隐藏配置项按钮
                btnConfigItem.Visibility = Visibility.Collapsed;
            }

            btnPrevious.Click -= BtnPrevious_Click;
            btnPrevious.Click += BtnPrevious_Click;
            btnNext.Click -= BtnNext_Click;
            btnNext.Click += BtnNext_Click;
            btnPause.Click -= BtnPause_Click;
            btnPause.Click += BtnPause_Click;
            btnPlay.Click -= BtnPlay_Click;
            btnPlay.Click += BtnPlay_Click;
            btnMagnifyAdd.Click -= player.MagnifyAdd_Click;
            btnMagnifyAdd.Click += MagnifyAdd_Click;

            btnActualSize.Click -= BtnActualSize_Click;
            btnActualSize.Click += BtnActualSize_Click;
            btnMagnifyMinus.Click -= player.MagnifyMinus_Click;
            btnMagnifyMinus.Click += MagnifyMinus_Click;
        }



        private void BtnActualSize_Click(object sender, RoutedEventArgs e)
        {
            if (_showControl != null)
            {
                _showControl.ActualSize();
            }
        }

        /// <summary>
        /// 开始播放
        /// </summary>
        /// <param name="sender">播放按钮</param>
        /// <param name="e">路由事件参数</param>
        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (_showControl != null)
            {
                _showControl.Play();
            }
        }

        /// <summary>
        /// 停止播放
        /// </summary>
        /// <param name="sender">停止按钮</param>
        /// <param name="e">路由事件参数</param>
        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (_showControl != null)
            {
                _showControl.Stop();
            }
        }

        /// <summary>
        /// 播放下一个
        /// </summary>
        /// <param name="sender">下一个按钮</param>
        /// <param name="e">路由事件参数</param>
        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (_showControl != null)
            {
                _showControl.Stop();
                _showControl.Next();
            }
        }

        /// <summary>
        /// 播放上一个
        /// </summary>
        /// <param name="sender">上一个按钮</param>
        /// <param name="e">路由事件参数</param>
        private void BtnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (_showControl != null)
            {
                _showControl.Stop();
                _showControl.Previous();
            }
        }

        #region 缩放        
        /// <summary>
        /// 放大 25%.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MagnifyAdd_Click(object sender, RoutedEventArgs e)
        {
            if (_showControl != null)
            {
                _showControl.MagnifyAdd();
            }
        }

        /// <summary>
        /// 缩小 25%.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/> instance containing the event data.</param>
        private void MagnifyMinus_Click(object sender, RoutedEventArgs e)
        {
            if (_showControl != null)
            {
                _showControl.MagnifyMinus();
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// 创建最小化图标
        /// </summary>
        private void CreateNotifyIcon()
        {
            if (notifyIcon == null)
            {
                this.notifyIcon = new NotifyIcon();
                this.notifyIcon.BalloonTipText = "最小化隐藏作业指导书".L10N(); //设置程序启动时显示的文本

                this.notifyIcon.Text = "电子作业指导书".L10N(); //最小化到托盘时，鼠标点击时显示的文本
                var uri = new Uri("pack://application:,,,/SIE.Wpf.ESop;component/Images/esop.ico", UriKind.RelativeOrAbsolute);
                this.notifyIcon.Icon = new System.Drawing.Icon(System.Windows.Application.GetResourceStream(uri).Stream); //程序图标

                this.notifyIcon.Visible = true;
                notifyIcon.MouseDoubleClick -= OnNotifyIconDoubleClick;
                notifyIcon.MouseDoubleClick += OnNotifyIconDoubleClick;
                this.notifyIcon.ShowBalloonTip(500);
            }
        }

        /// <summary>
        /// 双击通知图标的时候触发
        /// </summary>
        /// <param name="sender">通知图标</param>
        /// <param name="e">事件参数</param>
        private void OnNotifyIconDoubleClick(object sender, EventArgs e)
        {
            if (Window != null)
            {
                Window.Show();
                Window.WindowState = WindowState.Normal;
            }
        }

        /// <summary>
        /// 窗口状态变更时触发
        /// </summary>
        /// <param name="sender">当前窗口</param>
        /// <param name="e">事件参数</param>
        private void Window_StateChanged(object sender, EventArgs e)
        {
            WindowState ws = Window.WindowState;
            if (ws == WindowState.Minimized)
            {
                Window.Hide();
            }
        }
    }

    #region ESOP快捷命令
    /// <summary>
    /// 测试命令
    /// </summary>
    public class DeBugCommand : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current
        ///     state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed,this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed,this object can be set to null.</param>
        public void Execute(object parameter)
        {
            var vm = ((parameter as LogicalView).Data as ESopViewModel);
            var wk = vm.Workstation;
            WipResourceWorkOrder wipLineWorkOrder = null;
            if (!wk.StationId.HasValue)
            {
                wipLineWorkOrder = RT.Service.Resolve<WipController>().GetWipResourceWorkOrder(wk.ResourceId.Value);
            }
            else
            {
                wipLineWorkOrder = RT.Service.Resolve<WipController>().GetWipResourceWorkOrder(new SIE.MES.WIP.Workcell
                {
                    ProcessId = vm.ProcessId ?? 0,
                    StationId = wk.StationId ?? 0,
                    ResourceId = wk.ResourceId ?? 0,
                    EmployeeId = wk.UserId ?? 0
                });
            }

            CRT.MessageService.ShowMessage(
                "产线ID:{0}\r\n产线编码:{1}\r\n产线名称:{2}\r\n显示点:{3}\r\n显示物料ID:{4}\r\n显示物料编码:{5}\r\n显示物料名称:{6}\r\n据库记录的在产物料Id:{7}\r\n数据库记录的在产物料编码:{8}\r\n据库记录的在产物料名称:{9}\r\n文档集:{10}\r\n文档:{11}".L10nFormat(wk?.Resource.Id, wk?.Resource.Code, wk?.Resource.Name, wk?.DisplayPoint.Name, vm.ItemId, vm.Item?.Code, vm.Item?.Name, wipLineWorkOrder?.WorkOrder?.ProductId, wipLineWorkOrder?.WorkOrder?.Product.Code, wipLineWorkOrder?.WorkOrder?.Product.Name, vm.CurrentPlayDocument?.DocumentCollection?.Name, vm.CurrentPlayDocument?.Name));
        }
    }

    /// <summary>
    /// 选择工作单元命令
    /// </summary>
    public class SelectOperationCommand : ICommand
    {
        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should execute.
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current
        ///     state.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed,this object can be set to null.</param>
        /// <returns>true if this command can be executed; otherwise, false.</returns>
        public bool CanExecute(object parameter)
        {
            return true;
        }

        /// <summary>
        /// 面板控件
        /// </summary>
        public DashboardPlayerControl player { get; set; }

        /// <summary>
        /// 来自窗口
        /// </summary>
        public Window SourceWindow { get; set; }
        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">Data used by the command. If the command does not require data to be passed,this object can be set to null.</param>
        public void Execute(object parameter)
        {
            ESopWorkstationSelector.SelectOperation(((parameter as LogicalView).Data as ESopViewModel).Workstation, SourceWindow, player);
        }
    }
    #endregion 
}
