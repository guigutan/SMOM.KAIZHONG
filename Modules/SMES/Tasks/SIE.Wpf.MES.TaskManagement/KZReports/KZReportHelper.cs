using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP;
using SIE.Resources.WipResources;
using SIE.Threading;
using SIE.Wpf.Andon;
using SIE.Wpf.Andon.Controls;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.TaskManagement.KZReports.Controls;
using SIE.Wpf.MES.TaskManagement.Properties;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Task = System.Threading.Tasks.Task;

namespace SIE.Wpf.MES.TaskManagement.KZReports
{
    /// <summary>
    /// KZ报工帮助类
    /// </summary>
    public class KZReportHelper
    {
        KZTaskReportViewModelBase modelBase;
        KZTaskReportViewModel model;
        public ReportMainControl reportMainControl; //报工主界面
        public UserLoginControl userLoginControl;   //用户登录切换控件
        public ResourceListControl resourceListControl; //资源列表控件
        public TaskListControl taskListControl; //任务单列表控件
        public ViewTaskListControl viewTaskListControl; //任务单列表控件
        public TaskQueueListControl taskQueueListControl; //任务生产队列列表控件
        public TouchInputControl touchInputControl; //数字键盘控件
        public ReportManualControl reportManualControl; //手工报工
        public ScanReportControl scanReportControl;     //扫码报工
        public PreparationNav preparationNavControl;    //开工准备
        public PreparationEquip preparationEquipControl;    //工装/检具
        public PreparationModel preparationModelControl;    //模具
        public LabelPrintControl labelPrintControl; //标签打印
        public FeedingTaskListControl feedingTaskListControl;   //上料任务单
        public FeedingScanControl feedingScanControl;       //上料扫描标签
        public DeductionListControl deductionListControl;   //下料标签信息
        public WeighingControl weighingControl;         //称重
        public SWRecordScanControl sWRecordControl;            //余料称重
        public ReportMulitStationControl reportMulitStationControl; //多工位报工
        public ResourceMulitSelectListControl resourceMulitSelectListControl; //资源列表控件(可多选,待实现)
        public ReportProcessControl reportProcessControl; //过程工序数采报工控件
        public AndonButtonControl andonButtonControl;   //安灯管理
        public FeedingAreaScanControl feedingAreaScanControl;   //区域上料

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        //public KZReportHelper(KZTaskReportViewModel _model)
        //{
        //    model = _model;
        //    modelBase = _model;
        //}
        /// <summary>
        /// 构造函数
        /// </summary>
        public KZReportHelper(KZTaskReportViewModelBase _modelbase)
        {
            modelBase = _modelbase;
        }

        string GetScreen()
        {
            return $"({(int)SystemParameters.PrimaryScreenWidth}*{(int)SystemParameters.PrimaryScreenHeight})";
        }

        /// <summary>
        /// 设置窗口样式
        /// </summary>
        /// <param name="w"></param>
        /// <param name="title"></param>
        void SetWindowStyle(View.Workbench.IDialogContent w, string title = "")
        {
            w.WindowState = System.Windows.WindowState.Maximized;
            w.WindowStyle = System.Windows.WindowStyle.ToolWindow;
            w.Width = 1024; w.MinWidth = 1024;
            w.Height = 768; w.MinHeight = 768;
            w.Title = $"{title} {GetScreen()}";
            w.Commands.Clear();
        }

        /// <summary>
        /// 跳转称重
        /// </summary>
        public void ShowWeighing()
        {
            if (modelBase.DispatchTask == null || modelBase.DispatchTaskId == null)
            {
                CRT.MessageService.ShowWarning("请先选择任务单!".L10N());
                return;
            }
            modelBase.StopIOTReportTimer();
            if (modelBase.ReportEmployee == null)
            {
                ShowUserLogin();
                return;
            }
            if (weighingControl == null)
            {
                weighingControl = new WeighingControl(modelBase);
            }
            CRT.Workbench.ShowDialog("weighingControl", weighingControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "称重";
            });
        }

        /// <summary>
        /// 报工人登录
        /// </summary>
        public void ShowUserLogin(Action completedAction = null)
        {
            if (modelBase.ReportEmployee != null)
                return;
            //if (userLoginControl == null)
            userLoginControl = new UserLoginControl(modelBase);

            CRT.Workbench.ShowDialog("userLoginControl", userLoginControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "报员工登录";
                w.Commands.Clear();
                w.Closed += (s, e) =>
                {
                    modelBase.ReportEmployee = userLoginControl.Employee;
                    if (modelBase.ReportEmployee == null)
                        CRT.Workbench.CloseCurrent();
                    else
                    {
                        completedAction?.Invoke();
                        //ShowReportMain();
                    }
                };
            });
        }


        /// <summary>
        /// 资源列表
        /// </summary>
        public void ShowSelectResourceList(Action completedAction = null)
        {
            if (modelBase.ReportEmployee == null)
            {
                ShowUserLogin();
                return;
            }

            //if (resourceListControl == null)
            {
                resourceListControl = new ResourceListControl(modelBase);
            }

            modelBase.StopIOTReportTimer();
            CRT.Workbench.ShowDialog("resourceListControl", resourceListControl, w =>
            {
                SetWindowStyle(w);
                w.Title = "报工资源";
                w.Closed += (s, e) =>
                {
                    modelBase.ResourceId = resourceListControl.ResouceId;
                    SaveWorkstation(resourceListControl.ResouceId);
                    if (modelBase.Resource == null)
                    {
                        modelBase.Process = null;
                        if (modelBase.Workstation != null)
                        {
                            modelBase.Workstation.Resource = null;
                            modelBase.Workstation.Process = null;
                            modelBase.Workstation.Station = null;
                        }
                        //CRT.Workbench.CloseCurrent();
                    }
                    else
                    {
                        completedAction?.Invoke();
                        //ShowReportMain();
                    }
                };
            });
        }

        /// <summary>
        /// 显示区域上料
        /// </summary>
        /// <param name="vm"></param>
        public void ShowFeedingAreaScan()
        {
            feedingAreaScanControl = new FeedingAreaScanControl();

            CRT.Workbench.ShowDialog("feedingAreaScanControl", feedingAreaScanControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "区域上料";
            });
        }

        /// <summary>
        /// 资源列表(多选)
        /// </summary>
        public void ShowMulitSelectResourceList(KZTaskReportViewModel vm, Action completedAction = null)
        {
            if (modelBase.ReportEmployee == null)
            {
                ShowUserLogin();
                return;
            }
            resourceMulitSelectListControl = new ResourceMulitSelectListControl(modelBase);

            //modelBase.StopIOTReportTimer();
            CRT.Workbench.ShowDialog("resourceMulitSelectListControl", resourceMulitSelectListControl, w =>
            {
                SetWindowStyle(w);
                w.Title = "选择资源工位";
                w.Closed += (s, e) =>
                {
                    var resourceId = resourceMulitSelectListControl.ResouceId;
                    if (resourceId == 0)
                        CRT.Workbench.CloseCurrent();
                    else
                    {
                        if (vm != null)
                            vm.ResourceId = resourceId;
                        else
                        {
                            var vm1 = (modelBase as KZTaskReportMultiStationViewModel).ReportViewModelList.FirstOrDefault(p => p.Resource == null);
                            if (vm1 != null)
                                vm1.ResourceId = resourceId;
                            else
                                CRT.MessageService.ShowWarningFormatted("所有生产工位都已经分配资源");
                        }
                        completedAction?.Invoke();
                    }
                };
            });
        }

        /// <summary>
        /// 显示余料称重信息
        /// </summary>
        public void ShowSWRecordScan()
        {
            modelBase.StopIOTReportTimer();
            if (sWRecordControl == null)
            {
                sWRecordControl = new SWRecordScanControl(modelBase);
            }
            CRT.Workbench.ShowDialog("sWRecordControl", sWRecordControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "余料称重"; ;
            });
            sWRecordControl = null;
        }

        /// <summary>
        /// 显示下料标签信息
        /// </summary>
        public void ShowDeductionList()
        {
            modelBase.StopIOTReportTimer();

            if (deductionListControl == null)
            {
                deductionListControl = new DeductionListControl(modelBase);

            }
            CRT.Workbench.ShowDialog("deductionListControl", deductionListControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "下料"; ;
            });
            deductionListControl = null;
        }

        /// <summary>
        /// 显示上料任务单
        /// </summary>
        public void ShowFeedingTaskList()
        {
            modelBase.StopIOTReportTimer();
            if (modelBase.ReportEmployee == null)
            {
                ShowUserLogin();
                return;
            }
            if (feedingTaskListControl == null)
            {
                feedingTaskListControl = new FeedingTaskListControl(modelBase);
            }
            CRT.Workbench.ShowDialog("feedingTaskListControl", feedingTaskListControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "上料任务";
            });
        }

        /// <summary>
        /// 上料
        /// </summary>
        /// <param name="taskId"></param>
        public void ShowFeedingScan(double taskId)
        {
            feedingScanControl = new FeedingScanControl(modelBase, taskId);

            feedingScanControl.Loaded += (s, e) =>
            {
                modelBase.FeedingDispatchTaskList.Clear();
                Task.Run(new Action(() =>
                {
                    modelBase.LoadScanFeeding(taskId);
                }).WithCurrentThreadContext());
            };

            modelBase.StopIOTReportTimer();
            CRT.Workbench.ShowDialog("feedingScanControl", feedingScanControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "扫描标签";
            });
        }

        /// <summary>
        /// 任务列表
        /// </summary>
        public void ShowSelectTaskList(bool isManualReport = false)
        {
            if (modelBase.ReportEmployee == null)
            {
                ShowUserLogin();
                return;
            }

            //if (taskListControl == null)
            {
                taskListControl = new TaskListControl(modelBase);
            }
            modelBase.StopIOTReportTimer();
            CRT.Workbench.ShowDialog("taskListControl", taskListControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "任务列表";
                w.Closed += (s, e) =>
                {

                };
            });
        }

        public void ShowViewAndon(Workstation workstation)
        {
            AndonManageViewModel model = new AndonManageViewModel(false);
            model._workstation = workstation;
            model.Workstation.Resource = workstation.Resource;

            model.KZWorkstation.Resource = workstation.Resource;

            var template = new AndonManageUITemplate(model);
            var ui = template.CreateUI();
            CRT.Workbench.ShowDialog(ui, w =>
            {
                SetWindowStyle(w);
                w.Title = "安灯管理";
                w.Closed += (s, e) =>
                {
                    model.OnClose();
                };

                model.GetAndons();
                model.GetAndonManageList();
                //将按钮给他隐藏调
                ui.MainView.Commands.ForEach(p => p.IsVisible = false);
                //将切换工作单元的按钮给他们去掉，防止他们点击
                var button = template.kZWorkstationDetailLogicalView?.PropertyEditors?.LastOrDefault()?.Control;
                if (button != null)
                    button.Visibility = Visibility.Hidden;
            });
        }

        /// <summary>
        /// 任务列表
        /// </summary>
        public void ShowViewTaskList(double? resourceId, double? processId, bool isVisible = false)
        {
            viewTaskListControl = new ViewTaskListControl(modelBase, isVisible);
            viewTaskListControl.ResourceId = resourceId;
            viewTaskListControl.ProcessId = processId;

            modelBase.StopIOTReportTimer();
            CRT.Workbench.ShowDialog("viewTaskListControl", viewTaskListControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "任务列表";
                w.Closed += (s, e) =>
                {

                };
            });
        }

        /// <summary>
        /// 任务队列列表
        /// </summary>
        public void ShowTaskQueueList()
        {
            //if (taskQueueListControl == null)
            {
                taskQueueListControl = new TaskQueueListControl(modelBase);
            }
            modelBase.StopIOTReportTimer();
            CRT.Workbench.ShowDialog("taskQueueListControl", taskQueueListControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "生产队列清单";
                w.Closed += (s, e) =>
                {

                };
            });
        }

        /// <summary>
        /// 生产报工
        /// </summary>
        public void ShowReportMain()
        {
            if (modelBase.ReportEmployee == null)
            {
                ShowUserLogin(new Action(() => ShowReportMain()));
                return;
            }
            //加载资源
            LoadWorkstation();
            if (modelBase.Resource == null)
            {
                ShowSelectResourceList(new Action(() => ShowReportMain()));
                return;
            }
            //if (reportMainControl != null)
            //    return;

            if (reportMainControl == null)
                reportMainControl = new ReportMainControl(modelBase);
            CRT.Workbench.ShowDialog("reportMainControl", reportMainControl, w =>
            {
                var title = "生产报工".L10N();
                if (modelBase.IotMode != IotMode.Normal)
                    title = "生产报工 ({0})".L10nFormat(modelBase.IotMode.ToLabel());

                SetWindowStyle(w, title);
                w.Closed += (s, e) =>
                {
                    CRT.Workbench.CloseCurrent();
                };
            });
        }

        /// <summary>
        /// 加载资源
        /// </summary>
        void LoadWorkstation()
        {
            //除了多工位之外的功能
            if (modelBase.IotMode == IotMode.MultiStation)
                return;
            if (modelBase.Resource != null)
                return;
            var setting = Settings.Default.Workcell;
            if (setting.IsNotEmpty())
            {
                var workcells = JsonConvert.DeserializeObject<Dictionary<string, Workcell>>(setting);
                var key = GetType().GetQualifiedName() + modelBase.IotMode.ToString();
                if (workcells.ContainsKey(key))   //匹配工作单元
                {
                    var workcell = workcells[key];
                    if (workcell != null && workcell.ResourceId > 0)
                        modelBase.Resource = RF.GetById<WipResource>(workcell.ResourceId);
                }
            }
        }

        /// <summary>
        /// 保存资源
        /// </summary>
        public void SaveWorkstation(double resourceId)
        {
            //除了多工位之外的功能
            if (modelBase.IotMode == IotMode.MultiStation)
                return;
            var workcell = new Workcell()
            {
                ResourceId = resourceId
            };
            var setting = Settings.Default.Workcell;
            Dictionary<string, Workcell> data = null;
            if (setting.IsNotEmpty())
            {
                data = JsonConvert.DeserializeObject<Dictionary<string, Workcell>>(setting);
            }
            if (data == null)
            {
                data = new Dictionary<string, Workcell>();
            }
            var key = GetType().GetQualifiedName() + modelBase.IotMode.ToString();
            data[key] = workcell;
            Settings.Default.Workcell = JsonConvert.SerializeObject(data);
            Settings.Default.Save();
        }

        /// <summary>
        /// 生产报工
        /// </summary>
        public void ShowReportDetail(KZTaskReportViewModelBase vm)
        {
            if (vm.ReportEmployee == null)
            {
                throw new ValidationException("报工员还未登录");
            }
            if (vm.Resource == null)
            {
                throw new ValidationException("请先分配资源工位");
            }

            var reportMainControl1 = new ReportMainControl(vm);
            CRT.Workbench.ShowDialog("reportMainControl1", reportMainControl1, w =>
            {
                var title = "生产报工".L10N();
                if (vm.IotMode != IotMode.Normal)
                    title = "生产报工 ({0})".L10nFormat(vm.IotMode.ToLabel());

                SetWindowStyle(w, title);
                w.Closed += (s, e) =>
                {
                    //CRT.Workbench.CloseCurrent();
                };
            });
        }
        /// <summary>
        /// 生产报工 (多工位)
        /// </summary>
        public void ShowReportMulitStation()
        {
            if (modelBase.ReportEmployee == null)
            {
                ShowUserLogin(new Action(() => ShowReportMulitStation()));
                return;
            }
            var vm = modelBase as KZTaskReportMultiStationViewModel;
            if (vm != null && vm.ReportViewModelList.All(p => p.Resource == null))
            {
                ShowMulitSelectResourceList(null, null);
                //return;
            }
            vm.ReportViewModelList.ForEach(p => p.ReportEmployee = modelBase.ReportEmployee);

            if (reportMulitStationControl == null)
                reportMulitStationControl = new ReportMulitStationControl(modelBase);
            CRT.Workbench.ShowDialog("reportMulitStationControl", reportMulitStationControl, w =>
            {
                var title = "生产报工".L10N();
                if (modelBase.IotMode != IotMode.Normal)
                    title = "生产报工 ({0})".L10nFormat(modelBase.IotMode.ToLabel());

                SetWindowStyle(w, title);
                w.Closed += (s, e) =>
                {
                    CRT.Workbench.CloseCurrent();
                };
            });
        }

        /// <summary>
        /// 生产报工 (过程数采)
        /// </summary>
        public void ShowReportProcessControl()
        {

            if (modelBase.ReportEmployee == null)
            {
                ShowUserLogin(new Action(() => ShowReportProcessControl()));
                return;
            }
            //加载资源
            LoadWorkstation();
            if (modelBase.Resource == null)
            {
                ShowSelectResourceList(new Action(() => ShowReportProcessControl()));
                return;
            }

            if (reportMulitStationControl == null)
                reportProcessControl = new ReportProcessControl(modelBase);
            CRT.Workbench.ShowDialog("reportProcessControl", reportProcessControl, w =>
            {
                var title = "生产报工".L10N();
                if (modelBase.IotMode != IotMode.Normal)
                    title = "生产报工 ({0})".L10nFormat(modelBase.IotMode.ToLabel());

                SetWindowStyle(w, title);
                w.Closed += (s, e) =>
                {
                    CRT.Workbench.CloseCurrent();
                };
            });
        }

        /// <summary>
        /// 手工报工
        /// </summary>
        public void ShowReportManual()
        {
            modelBase.StopIOTReportTimer();
            //modelBase.DispatchTask = null;
            modelBase.OkQty = 0;
            modelBase.SuspectQty = 0;
            modelBase.IsReportManual = true;
            if (reportManualControl == null)
                reportManualControl = new ReportManualControl(modelBase);
            CRT.Workbench.ShowDialog("reportManualControl", reportManualControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "手工报工";
                w.Closed += (s, e) =>
                {
                    modelBase.OkQty = 0;
                    modelBase.SuspectQty = 0;
                    modelBase.IsReportManual = false;
                };
            });
        }
        /// <summary>
        /// 扫码报工
        /// </summary>
        public void ShowScanReport()
        {
            modelBase.StopIOTReportTimer();
            //if (scanReportControl == null)
            scanReportControl = new ScanReportControl(modelBase);
            CRT.Workbench.ShowDialog("scanReportControl", scanReportControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "扫码报工";
                w.Closed += (s, e) =>
                {
                };
            });
        }

        /// <summary>
        /// 开机准备
        /// </summary>
        public void ShowPreparationNav()
        {
            if (modelBase.DispatchTask == null || modelBase.DispatchTaskId == null)
            {
                CRT.MessageService.ShowWarning("任务单不能为空!".L10N());
                return;
            }
            modelBase.StopIOTReportTimer();
            if (preparationNavControl == null)
                preparationNavControl = new PreparationNav(modelBase);
            CRT.Workbench.ShowDialog("preparationNavControl", preparationNavControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "开机准备";
                w.Closed += (s, e) =>
                {

                };
            });
        }

        /// <summary>
        /// 工装/检具
        /// </summary>
        public void ShowPreparationEquip()
        {
            modelBase.StopIOTReportTimer();
            if (preparationEquipControl == null)
                preparationEquipControl = new PreparationEquip(modelBase);
            CRT.Workbench.ShowDialog("preparationEquipControl", preparationEquipControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "工装/检具准备";
                w.Closed += (s, e) =>
                {

                };
            });
        }

        /// <summary>
        /// 上模
        /// </summary>
        public void ShowPreparationModel()
        {
            modelBase.StopIOTReportTimer();
            if (preparationModelControl == null)
                preparationModelControl = new PreparationModel(modelBase);
            CRT.Workbench.ShowDialog("preparationModelControl", preparationModelControl, w =>
            {
                SetWindowStyle(w);

                w.Title = "上模";
                w.Closed += (s, e) =>
                {

                };
            });
        }

        /// <summary>
        /// 标签打印
        /// </summary>
        public void ShowLabelPrintControl(bool autoPrint)
        {
            modelBase.StopIOTReportTimer();
            //if (labelPrintControl == null)
            labelPrintControl = new LabelPrintControl(modelBase, autoPrint);
            CRT.Workbench.ShowDialog("labelPrintControl", labelPrintControl, w =>
            {
                //SetWindowStyle(w);
                w.Width = 700;
                w.Height = 600;
                w.Commands.Clear();
                w.Title = "标签打印";
                w.Closed += (s, e) =>
                {

                };
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        public void ShowCalculatorEditor(Property<decimal> property)
        {
            var editor = new Calculator();
            editor.Value = (double)modelBase.GetProperty(property);
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
            {
                w.Title = "数量录入".L10N();
                w.Height = 400;
                w.Width = 400;
                w.Closing += (a, b) =>
                {
                    if (w.Result == 0)
                    {
                        if (editor.HasError)
                        {
                            b.Cancel = true;
                            return;
                        }
                        modelBase.SetProperty(property, editor.Value);
                    }
                };
            });
        }

        /// <summary>
        /// 显示数字键盘
        /// </summary>
        public void ShowCalculatorEditor(Func<decimal, decimal> func, decimal value)
        {
            var editor = new Calculator();
            editor.Value = (double)value;
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
            {
                w.Title = "数量录入".L10N();
                w.Height = 400;
                w.Width = 400;
                w.Closing += (a, b) =>
                {
                    if (w.Result == 0)
                    {
                        if (editor.HasError)
                        {
                            b.Cancel = true;
                            return;
                        }
                        func?.Invoke((decimal)editor.Value);
                    }
                };
            });
        }


        /// <summary>
        /// 显示数字键盘
        /// </summary>
        public void ShowNumberInput(Property<decimal> property)
        {
            //if (touchInputCtl == null)
            touchInputControl = new TouchInputControl(modelBase, modelBase.GetProperty(property));

            var dialogId = CRT.Workbench.ShowDialog("TouchInputControl", touchInputControl, w =>
            {
                //SetWindowStyle(w);
                w.WindowStyle = System.Windows.WindowStyle.None;
                w.Width = 500;
                w.Height = 430;
                //w.Title = "数量录入";
                w.Commands.Clear();
                w.Closed += (s, e) =>
                {
                    if (touchInputControl.Value.IsNullOrEmpty())
                        touchInputControl.Value = "0";
                    modelBase.SetProperty(property, decimal.Parse(touchInputControl.Value));
                    Keyboard.ClearFocus();
                };
            });

        }

        /// <summary>
        /// 显示数字键盘
        /// </summary>
        public void ShowNumberInput(Func<decimal, decimal> func, decimal value)
        {
            //if (touchInputCtl == null)
            touchInputControl = new TouchInputControl(value);

            var dialogId = CRT.Workbench.ShowDialog("TouchInputControl", touchInputControl, w =>
            {
                //SetWindowStyle(w);
                w.WindowStyle = System.Windows.WindowStyle.None;
                w.Width = 500;
                w.Height = 430;
                //w.Title = "数量录入";
                w.Commands.Clear();
                w.Closed += (s, e) =>
                {
                    if (touchInputControl.Value.IsNullOrEmpty())
                        touchInputControl.Value = "0";

                    Keyboard.ClearFocus();
                    func?.Invoke(decimal.Parse(touchInputControl.Value));
                };
            });

        }

        /// <summary>
        /// 切换到英文输入法
        /// </summary>
        public void SwitchToEnglishMode()
        {
            if (InputLanguageManager.Current.CurrentInputLanguage.Name.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                return;
            // 切换到英文输入法
            foreach (CultureInfo item in InputLanguageManager.Current.AvailableInputLanguages)
            {
                if (item.Name.StartsWith("en", StringComparison.OrdinalIgnoreCase))
                {
                    InputLanguageManager.Current.CurrentInputLanguage = item;
                    break;
                }
            }
        }
    }
}
