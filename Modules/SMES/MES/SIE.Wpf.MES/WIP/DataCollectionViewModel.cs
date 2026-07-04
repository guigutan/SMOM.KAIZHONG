using Newtonsoft.Json;
using SIE.Common;
using SIE.Common.CollectBarcodeConverters;
using SIE.Common.Configs;
using SIE.Core.Barcodes;
using SIE.Core.Items;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.Statistics.WIP;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.TaskExtensions;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Threading;
using SIE.Wpf.Common;
using SIE.Wpf.MES.Controls;
using SIE.Wpf.MES.Controls.Messager;
using SIE.Wpf.MES.Properties;
using SIE.Wpf.MES.WIP.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using MessagerControl = SIE.Wpf.MES.Controls.Messager.MessagerControl;

namespace SIE.Wpf.MES.WIP
{
    /// <summary>
    /// 数据采集泛型基类
    /// </summary>
    /// <typeparam name="T">泛型必须继承于WipController</typeparam>
    public class DataCollectionViewModel<T> : DataCollectionViewModel where T : WipController
    {
        /// <summary>
        /// 采集控制器，通过泛型参数确定控制器的类型
        /// </summary>
        protected virtual T Controller { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        protected DataCollectionViewModel()
        {
            Controller = RT.Service.Resolve<T>();
        }

        /// <summary>
        /// 验证:1.工艺路线。2.在制工单
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        /// <param name="workcell">工作单元</param>
        protected virtual ProductInfo Validate(CollectBarcode collectBarcode, Workcell workcell)
        {
            var product = Controller.Validate(collectBarcode, workcell);
            if (product.WorkOrderId != WorkOrderId && product.WorkOrderId != 0)
            {
                var wo = RF.GetById<WorkOrder>(product.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                if (WorkOrder != null)
                {
                    ShowError("工单已切换,由[{0}]切换到[{1}]".L10nFormat(WorkOrder.No, wo.No));
                }
                WorkOrder = wo;
                Controller.ChangeWipResourceWorkOrder(wo.Id, workcell);
                RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = product.WorkOrderId });
                UpdateWorkOrdeReportModel(wo.Id);
            }
            ValidateTaskReport(product.WorkOrderId, workcell);
            return product;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void Onload()
        {
            base.Onload();
            AsyncExecute(() => LoadWorkstationData());
        }

        /// <summary>
        /// 工作单元信息改变
        /// </summary>
        protected override void StationChanged(Station station)
        {
            base.StationChanged(station);
            LoadWorkstationData();
        }

        /// <summary>
        /// 初始化工位信息
        /// </summary>
        protected virtual void LoadWorkstationData()
        {
            try
            {
                var workcell = GetWorkcell();
                var wipLineWorkOrder = Controller.GetWipResourceWorkOrder(workcell);
                if (wipLineWorkOrder != null)
                {
                    var workOrder = RF.GetById<WorkOrder>(wipLineWorkOrder.WorkOrderId, new EagerLoadOptions().LoadWithViewProperty());
                    this.WorkOrder = workOrder;
                }
                else
                {
                    this.WorkOrder = null;
                }


                DefectList = new EntityList<Defect>();

                LoadDefectList(this.Workstation.Process);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }
    }

    /// <summary>
    /// 数据采集基类
    /// </summary>
    [RootEntity, Serializable]
    public class DataCollectionViewModel : WorkCellViewModel
    {
        /// <summary>
        /// 采集步骤
        /// </summary>
        protected virtual CollectStep Step { get; set; }

        #region 拼板码
        /// <summary>
        /// 拼板信息
        /// </summary>
        protected SubmitPanelInfo PanelInfo { get; set; }

        /// <summary>
        /// 拼板码绑定Sn配置项
        /// </summary>
        protected PanelBindingSnConfigValue PanelBindingSnConfigValue;

        /// <summary>
        /// 拼板码绑定SN是否自动生成
        /// </summary>
        protected bool IsConfigAuto { get { return PanelBindingSnConfigValue?.IsGenerateSn ?? false; } }

        /// <summary>
        /// 过站记录状态
        /// </summary>
        public WipProductProcessState WipProductProcessState { get; set; }

        /// <summary>
        /// 上一次的采集结果
        /// </summary>
        public ResultType? LastResultType { get; set; }

        /// <summary>
        /// 合并数据
        /// </summary>
        /// <param name="info">产品信息</param>
        protected virtual void MergeData(ProductInfo info)
        {
            if (info.BarcodeType == BarcodeType.CombinedCode)
            {
                PanelInfo.Clear();
                if (info.PanelInfo.IsBinding)
                    PanelInfo.BindingMode = IsConfigAuto ? SIE.MES.WIP.Models.BindingMode.Auto : SIE.MES.WIP.Models.BindingMode.Manual;
                PanelInfo.BarcodeType = info.BarcodeType;
                PanelInfo.PanelQty = info.PanelInfo.CanBindQty;
                PanelInfo.ForkPlateQty = info.PanelInfo.ForkPlateQty;
                PanelInfo.PanelCode = info.PanelInfo.PanelCode;
                PanelInfo.SnList.AddRange(info.PanelInfo.SnList);
            }


            WipProductProcessState = info.WipProductProcessState;
        }

        /// <summary>
        /// 验证采集提交
        /// </summary>
        protected virtual void ValidateCombinedCodeBinding()
        {
            if (PanelInfo.BarcodeType == BarcodeType.CombinedCode && PanelInfo.NeetToBindingSn)
            {
                throw new UnBindingSnException("拼板码绑定产品数未达到容量，请继续扫描第{2}个产品条码".L10nFormat(PanelInfo.SnList.Count + 1));
            }
        }

        /// <summary>
        /// 初始化拼板码信息
        /// </summary>
        /// <param name="collectData">采集数据</param>
        protected virtual void InitCombinedCodeInfo(CollectData collectData)
        {
            var bindingSns = PanelInfo.SnList.Select(p => new BindingSn() { Sn = p.Sn, Qty = p.Qty }).ToList();
            collectData.CombinedCode.BindingSns.AddRange(bindingSns);
            collectData.CombinedCode.AutoCreateAndBinding = IsConfigAuto;
            collectData.CombinedCode.ToBindingQty = PanelInfo.PanelQty - PanelInfo.ForkPlateQty;
            collectData.CollectBarcode.Type = PanelInfo.BarcodeType.HasValue ? PanelInfo.BarcodeType.Value : collectData.CollectBarcode.Type;
        }

        /// <summary>
        /// 打印拼板码绑定的自动生成的SN
        /// </summary>
        protected virtual void PrintBindingSn()
        {
            if (PanelInfo.BindingMode == SIE.MES.WIP.Models.BindingMode.Auto)
            {
                AsyncExecute(() =>
                {
                    try
                    {
                        var bacodes = RT.Service.Resolve<WipProductVersionController>().GetAndDeleteToBePrintedSnList(WorkOrder.Id, PanelInfo.PanelCode);
                        if (!bacodes.Any())
                            return;
                        var info = new BarcodePrintInfo()
                        {
                            WorkOrderId = WorkOrder.Id,
                            PrintTemplateId = WorkOrder?.Template?.LabelTemplateId ?? 0,
                            Printer = PanelBindingSnConfigValue.Printer,
                            TemplateType = WorkOrder?.Template?.LabelTemplate?.Type
                        };
                        BarcodePrintHelper.PrintBarcode(info, bacodes);
                    }
                    catch (Exception exc)
                    {
                        System.Diagnostics.Debug.Write(exc.Message);
                    }
                });
            }
        }

        /// <summary>
        /// 拼板码绑定配置
        /// </summary>
        /// <param name="station">工位</param>
        /// <returns>绑定SN配置</returns>
        protected virtual PanelBindingSnConfigValue GetPanelBindingSnConfig(Station station)
        {
            return ConfigService.GetConfig(new PanelBindingSnConfig(), GetType(), station);
        }
        #endregion

        /// <summary>
        /// 初始化采集步骤
        /// </summary>
        protected DataCollectionViewModel()
        {
            Step = new CollectStep(this);
            PanelInfo = new SubmitPanelInfo();
            ReportModel = -1;

            //默认合格
            Qualified = false;

            this.DefectItemList = new ObservableCollection<DefectItem>();
        }

        /// <summary>
        /// 加载
        /// </summary>
        public override void Onload()
        {
            Reset(ResetType.Init);
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void OnClose()
        {
            CloseSerial();
        }

        /// <summary>
        /// 重置界面数据
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            Error = null;
            DisplayBarCode = string.Empty;
            switch (resetType)
            {
                case ResetType.Init:
                    {
                        ShowTips("请扫描条码".L10N());
                    }
                    break;
                case ResetType.CollectRestart:
                    {
                        ShowTips("已重新开始，请扫描条码".L10N());
                    }
                    break;
                case ResetType.ChangeWorkStation:
                    {
                        ShowTips("已切换工作单元，请扫描条码".L10N());
                    }
                    break;
                case ResetType.Success:
                case ResetType.Error:
                case ResetType.None:
                    {
                        //成功后的初始化，重置时不提示信息，由业务功能自己写提示信息
                    }
                    break;
                default:
                    break;
            }

            FocuseBarcode();
            Step.Reset();
            PanelInfo.Clear();

            //清除已选择的缺陷
            this.DefectItemList.Clear();

            SwitchToEnglishMode();
        }

        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("扫描条码")]
        public static readonly Property<string> BarcodeProperty = P<DataCollectionViewModel>.Register(e => e.Barcode, new PropertyMetadata<string>
        {
            PropertyChangedCallBack = (s, e) => (s as DataCollectionViewModel).OnBarcodeChanged(e),
            CoerceGetValueCallBack = (s, v) => (s as DataCollectionViewModel).CoerceGetBarcode(v)
        });

        /// <summary>
        /// 转换条码内容
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>转换后的条码内容</returns>
        protected virtual string CoerceGetBarcode(string barcode)
        {
            if (barcode.IsNullOrEmpty()) return barcode;
            switch (BarcodeFormatConfig.Format)
            {
                case BarcodeFormat.None: return barcode;
                case BarcodeFormat.Upper: return barcode.ToUpper();
                case BarcodeFormat.Lower: return barcode.ToLower();
                default:
                    {
                        var regex = new Regex(BarcodeFormatConfig.Regex);
                        var matchs = regex.Matches(barcode);
                        if (matchs.Count > 0)
                            return matchs[0].Groups["code"].Value;
                        return barcode;
                    }
            }
        }

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }

        /// <summary>
        /// 条码扫完后处理逻辑
        /// </summary>
        /// <param name="e">属性变更参数</param>
        protected virtual void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
        }

        /// <summary>
        /// 条码格式化配置值
        /// </summary>
        CollectBarcodeConverterConfigValue _barcodeFormatConfig;

        /// <summary>
        /// 条码格式化配置值
        /// </summary>
        CollectBarcodeConverterConfigValue BarcodeFormatConfig
        {
            get { return _barcodeFormatConfig ?? (_barcodeFormatConfig = ConfigService.GetConfig(new CollectBarcodeConverterConfig())); }
        }

        #region 条码类型 BarcodeType
        /// <summary>
        /// 条码类型
        /// </summary>
        [Label("条码类型")]
        public static readonly Property<BarcodeType> BarcodeTypeProperty = P<DataCollectionViewModel>.Register(e => e.BarcodeType);

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType
        {
            get { return this.GetProperty(BarcodeTypeProperty); }
            set { this.SetProperty(BarcodeTypeProperty, value); }
        }
        #endregion
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<DataCollectionViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<DataCollectionViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 设备端口

        /// <summary>
        /// 获取端口类型
        /// </summary>
        /// <returns></returns>
        public virtual DevicePortConfigValue GetDevicePort()
        {
            if (Workstation.Station == null)
                return null;
            var devicePort = ConfigService.GetConfig(new DevicePortConfig(), GetType(), ResourceStation.Find(Workstation.Station));
            return devicePort;
        }
        /// <summary>
        /// 获取端口配置信息
        /// </summary>
        /// <returns></returns>
        public virtual SerialPortsConfigValue GetSerialPortsConfig()
        {
            if (Workstation.Station == null)
                return null;
            var serialPortsConfig = ConfigService.GetConfig(new SerialPortsConfig(), GetType(), ResourceStation.Find(Workstation.Station));
            return serialPortsConfig;
        }

        /// <summary>
        /// 初始化设备端口信息
        /// </summary>
        protected void InitDevicePort()
        {
            CloseSerial();
            //if (Workstation.Station != null)
            {
                Task.Run(new Action(() =>
                {
                    //初始化端口类型
                    //var devicePort = ConfigService.GetConfig(new DevicePortConfig(), GetType(), ResourceStation.Find(Workstation.Station));
                    var devicePort = GetDevicePort();
                    if (devicePort != null && devicePort.DevicePort == DevicePort.Serial)
                    {
                        OpenSerial();
                    }
                }).WithCurrentThreadContext());
            }
        }

        /// <summary>
        /// 串口列表
        /// </summary>
        List<System.IO.Ports.SerialPort> serials = new List<System.IO.Ports.SerialPort>();

        /// <summary>
        /// 关闭通信串口
        /// </summary>
        protected void CloseSerial()
        {
            foreach (var serial in serials)
                if (serial.IsOpen)
                    serial.Close();
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        void OpenSerial()
        {
            //初始化串口信息可配置多个串口
            //var serialPortsConfig = ConfigService.GetConfig(new SerialPortsConfig(), GetType(), ResourceStation.Find(Workstation.Station));
            var serialPortsConfig = GetSerialPortsConfig();
            if (serialPortsConfig == null)
                return;
            foreach (var s in serialPortsConfig.SerialPortList)
            {
                var serialPort = new System.IO.Ports.SerialPort();
                serials.Add(serialPort);
                serialPort.PortName = s.PortName.ToString();
                serialPort.BaudRate = s.BaudRate;
                serialPort.DataBits = 8;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.WriteTimeout = 1000;
                serialPort.ReadTimeout = 1000;
                serialPort.DataReceived += Serial_DataReceived;
                try
                {
                    serialPort.Open();
                    ShowError("打开串口[{0}]成功".L10nFormat(s.PortName));
                }
                catch (Exception exc)
                {
                    var error = "打开串口[{0}]失败:".L10nFormat(s.PortName) + exc.Message;
                    ShowError(error);
                    //CRT.MessageService.ShowMessage();
                }
            }
        }

        /// <summary>
        /// 串口数据接收
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            CRT.MainThread.InvokeIfRequired(() =>
            {
                try
                {
                    Thread.Sleep(100);
                    var se = (sender as System.IO.Ports.SerialPort);
                    var data = se.ReadExisting();
                    ReadBarcode(data.TrimEnd('\n', '\r', '\0').TrimStart('\0'));
                }
                catch (Exception exc)
                {
                    var error = "接收串口数据异常:".L10nFormat() + exc.Message;
                    ShowError(error);
                }
            });
        }

        /// <summary>
        /// 串口读取数据
        /// </summary>
        /// <param name="read">条码</param>
        protected virtual void ReadBarcode(string read)
        {
            Barcode = read;
        }

        #endregion

        #region 采集结果

        #region 采集结果 CollectDetailList
        /// <summary>
        /// 采集结果
        /// </summary>
        [Label("采集结果")]
        public static readonly ListProperty<CollectDetailViewModelList> CollectDetailListProperty = P<DataCollectionViewModel>.RegisterList(e => e.CollectDetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as DataCollectionViewModel).LoadProductCategoryList()
        });

        /// <summary>
        /// 采集结果
        /// </summary>
        public CollectDetailViewModelList CollectDetailList
        {
            get { return this.GetLazyList(CollectDetailListProperty); }
        }

        /// <summary>
        /// 加载采集结果
        /// </summary>
        /// <returns>采集结果列表</returns>
        private CollectDetailViewModelList LoadProductCategoryList()
        {
            return new CollectDetailViewModelList();
        }
        #endregion

        /// <summary>
        /// 添加采集结果记录
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="result">结果类型</param>
        protected virtual void AddDetail(CollectBarcode barcode, ResultType result = ResultType.Pass)
        {
            CollectDetailList.Add(new CollectDetailViewModel
            {
                Barcode = barcode.Code,
                BarcodeType = barcode.Type,
                CollectDate = DateTime.Now,
                Result = result
            });
        }

        #endregion

        #region 提示信息

        #region Tips 提示信息
        /// <summary>
        /// 提示信息
        /// </summary>
        [Label("提示信息")]
        public static readonly Property<string> TipsProperty = P<DataCollectionViewModel>.Register(e => e.Tips);

        /// <summary>
        /// 提示信息(不要直接修改值，调用ShowTips()方法）
        /// </summary>
        public string Tips
        {
            get { return this.GetProperty(TipsProperty); }
            set { this.SetProperty(TipsProperty, value); }
        }
        #endregion

        #region Error 错误信息
        /// <summary>
        /// 错误信息
        /// </summary>
        [Label("错误信息")]
        public static readonly Property<string> ErrorProperty = P<DataCollectionViewModel>.Register(e => e.Error);

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get { return this.GetProperty(ErrorProperty); }
            set { this.SetProperty(ErrorProperty, value); }
        }
        #endregion

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        public override void ShowError(string error)
        {
            if (error == null)
            {
                return;
            }

            Error = error.Replace("\r\n", string.Empty);

            AddMessageToHistory(Error, MessageType.Error);
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="tips">提示信息</param>
        public void ShowTips(string tips)
        {
            if (tips == null)
            {
                return;
            }

            Tips = tips.Replace("\r\n", string.Empty);

            AddMessageToHistory(Tips, MessageType.Normal);
        }

        /// <summary>
        /// 清空提示信息
        /// </summary>
        protected virtual void ClearInfos()
        {
            Error = null;
            Tips = null;
        }

        #endregion

        #region 统计信息

        #region 合格数量 QtyPass
        /// <summary>
        /// 合格数量
        /// </summary>
        [Label("当班采集数")]
        public static readonly Property<decimal> QtyPassProperty = P<DataCollectionViewModel>.Register(e => e.QtyPass);

        /// <summary>
        /// 合格数量
        /// </summary>
        public decimal QtyPass
        {
            get { return this.GetProperty(QtyPassProperty); }
            set { this.SetProperty(QtyPassProperty, value); }
        }
        #endregion

        #region 不合格数量 QtyFaild
        /// <summary>
        /// 不合格数量
        /// </summary>
        [Label("当班不良数")]
        public static readonly Property<decimal> QtyFaildProperty = P<DataCollectionViewModel>.Register(e => e.QtyFaild);

        /// <summary>
        /// 不合格数量
        /// </summary>
        public decimal QtyFaild
        {
            get { return this.GetProperty(QtyFaildProperty); }
            set { this.SetProperty(QtyFaildProperty, value); }
        }
        #endregion

        #region 工单完成数 QtyFinish
        /// <summary>
        /// 工单完成数
        /// </summary>
        [Label("工单完成数")]
        public static readonly Property<decimal> QtyFinishProperty = P<DataCollectionViewModel>.Register(e => e.QtyFinish);

        /// <summary>
        /// 工单完成数
        /// </summary>
        public decimal QtyFinish
        {
            get { return this.GetProperty(QtyFinishProperty); }
            set { this.SetProperty(QtyFinishProperty, value); }
        }
        #endregion

        /// <summary>
        /// 刷新统计数
        /// </summary>
        public virtual void RefreshStatistics()
        {
            Task.Run(() =>
            {
                Thread.Sleep(2 * 1000);   //线程休息2s等待预统计完成后再刷新，采集数刷新并非实时
                var info = new StatisticsQueryInfo()
                {
                    ResourceId = Workstation.ResourceId ?? 0,
                    ProcessId = Workstation.ProcessId ?? 0,
                    StationId = Workstation.StationId ?? 0,
                    OperatorId = Workstation.EmployeeId ?? 0
                };
                ShowWorkstationStatistics(GetStationCollected(info));
            });
        }

        /// <summary>
        /// 获取工位统计结果
        /// </summary>
        /// <param name="info">统计查询信息</param>
        /// <returns>工位统计结果</returns>
        protected virtual StationCollectedEvent GetStationCollected(StatisticsQueryInfo info)
        {
            return RT.Service.Resolve<WipStatisticsController>().GetStationCollected(info);
        }

        /// <summary>
        /// 显示工位统计信息
        /// </summary>
        /// <param name="e">工位统计参数</param>
        protected virtual void ShowWorkstationStatistics(StationCollectedEvent e)
        {
            if (e != null && e.StationId != 0 && e.StationId == Workstation.StationId)
            {
                CRT.MainThread.InvokeAsync(() =>
                {
                    QtyPass = e.QtyPass + e.QtyFailed;
                    QtyFaild = e.QtyFailed;
                });
            }
        }
        #endregion

        #region 报工任务 
        #region 任务列表 TaskList
        /// <summary>
        /// 任务列表
        /// </summary>
        [Label("任务列表")]
        public static readonly ListProperty<EntityList<ReportTaskViewModel>> TaskListProperty = P<DataCollectionViewModel>.RegisterList(e => e.TaskList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as DataCollectionViewModel).LoadTaskList()
        });

        /// <summary>
        /// 任务列表
        /// </summary>
        public EntityList<ReportTaskViewModel> TaskList
        {
            get { return this.GetLazyList(TaskListProperty); }
        }

        /// <summary>
        /// 任务列表
        /// </summary>
        /// <returns>任务列表</returns>
        private EntityList<ReportTaskViewModel> LoadTaskList()
        {
            return new EntityList<ReportTaskViewModel>();
        }
        #endregion

        #region 报工方式 ReportModel
        /// <summary>
        /// 报工方式 0手动、1自动
        /// </summary>
        [Label("报工方式")]
        public static readonly Property<int?> ReportModelProperty = P<DataCollectionViewModel>.Register(e => e.ReportModel);

        /// <summary>
        /// 报工方式
        /// </summary>
        public int? ReportModel
        {
            get { return this.GetProperty(ReportModelProperty); }
            set { this.SetProperty(ReportModelProperty, value); }
        }
        #endregion 

        /// <summary>
        /// 更新工单任务报工方式
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        protected virtual void UpdateWorkOrdeReportModel(double workOrderId)
        {
            ReportModel = RT.Service.Resolve<IWipTaskReport>().GetWorkOrdeReportModel(workOrderId);
        }

        /// <summary>
        /// 验证工单任务报工
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void ValidateTaskReport(double workOrderId, Workcell workcell)
        {
            using (Diagnostics.DebugTrace.Start("自动报工验证耗时：".L10N()))
            {
                if (ReportModel == -1)
                    UpdateWorkOrdeReportModel(workOrderId);
                if (ReportModel == null)
                    return;
                if (ReportModel == 1)
                    throw new ValidationException("不允许采集，当前条码所属工单任务单报工方式为手动报工".L10N());
                RT.Service.Resolve<IWipTaskReport>().ValidateAutoReport(workOrderId, workcell.EmployeeId, workcell.ProcessId);
            }
        }

        /// <summary>
        /// 刷新工单任务列表
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="retrospectType">追溯方式</param>
        /// <param name="lazyLoad">延迟加载，采集后才做报工，需报工后再刷新任务列表</param>
        protected void RefrshReportTasks(double employeeId, Core.Items.RetrospectType retrospectType, bool lazyLoad = true)
        {
            Task.Run(() =>
            {
                if (lazyLoad)
                    Thread.Sleep(2 * 1000);
                try
                {
                    var type = Workstation.Process?.Type;
                    if (employeeId <= 0 || type == ProcessType.BatchFix || type == ProcessType.Fix || type == ProcessType.Rework)
                        return;
                    var tasks = RT.Service.Resolve<IWipTaskReport>().GetReportTasks(employeeId, retrospectType, Workstation.ProcessId ?? 0);
                    EntityList<ReportTaskViewModel> result = new EntityList<ReportTaskViewModel>();
                    if (WorkOrder != null)
                    {
                        result.AddRange(tasks.Where(p => p.WorkOrderId == WorkOrder.Id));
                        result.AddRange(tasks.Where(p => p.WorkOrderId != WorkOrder.Id));
                    }
                    else
                        result.AddRange(tasks);
                    CRT.MainThread.InvokeAsync(() =>
                    {
                        TaskList.Clear();
                        TaskList.AddRange(result);
                        TaskList.MarkSaved();
                    });
                }
                catch (Exception exc)
                {
                    ShowError(exc);
                }
            });
        }

        /// <summary>
        /// 刷新工单任务列表
        /// </summary> 
        public virtual void RefrshReportTasks(Core.Items.RetrospectType retrospectType = Core.Items.RetrospectType.Single, bool lazyLoad = true)
        {
            RefrshReportTasks(Workstation.EmployeeId ?? 0, retrospectType, lazyLoad);
        }
        #endregion

        #region 视图属性 
        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<DataCollectionViewModel>.RegisterView(e => e.ProductCode, p => p.WorkOrder.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<DataCollectionViewModel>.RegisterView(e => e.ProductName, p => p.WorkOrder.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 工单数量 WorkOrderQty
        /// <summary>
        /// 工单数量
        /// </summary>
        [Label("工单数量")]
        public static readonly Property<decimal> WorkOrderQtyProperty = P<DataCollectionViewModel>.RegisterView(e => e.WorkOrderQty, p => p.WorkOrder.PlanQty);

        /// <summary>
        /// 工单数量
        /// </summary>
        public decimal WorkOrderQty
        {
            get { return this.GetProperty(WorkOrderQtyProperty); }
        }
        #endregion

        #region 产品型号 ProductModel
        /// <summary>
        /// 产品型号
        /// </summary>
        [Label("产品型号")]
        public static readonly Property<string> ProductModelProperty = P<DataCollectionViewModel>.RegisterView(e => e.ProductModel, p => p.WorkOrder.Product.Model.Code);

        /// <summary>
        /// 产品型号
        /// </summary>
        public string ProductModel
        {
            get { return this.GetProperty(ProductModelProperty); }
        }
        #endregion
        #endregion

        #region 显示条码 DisplayBarCode
        /// <summary>
        /// 显示条码
        /// </summary>
        [Label("显示条码")]
        public static readonly Property<string> DisplayBarCodeProperty = P<DataCollectionViewModel>.Register(e => e.DisplayBarCode);

        /// <summary>
        /// 显示条码
        /// </summary>
        public string DisplayBarCode
        {
            get { return this.GetProperty(DisplayBarCodeProperty); }
            set { this.SetProperty(DisplayBarCodeProperty, value); }
        }
        #endregion

        #region 合格 Qualified
        /// <summary>
        /// 合格
        /// </summary>
        [Label("合格")]
        public static readonly Property<bool> QualifiedProperty = P<DataCollectionViewModel>.Register(e => e.Qualified);

        /// <summary>
        /// 合格
        /// </summary>
        public bool Qualified
        {
            get { return this.GetProperty(QualifiedProperty); }
            set { this.SetProperty(QualifiedProperty, value); }
        }
        #endregion

        #region 存在失败工序参数 HaveFailParameter
        /// <summary>
        /// 存在失败工序参数
        /// </summary>
        [Label("存在失败工序参数")]
        public static readonly Property<bool> HaveFailParameterProperty = P<DataCollectionViewModel>.Register(e => e.HaveFailParameter);

        /// <summary>
        /// 存在失败工序参数
        /// </summary>
        public bool HaveFailParameter
        {
            get { return this.GetProperty(HaveFailParameterProperty); }
            set { this.SetProperty(HaveFailParameterProperty, value); }
        }
        #endregion

        #region 不良信息录入

        /// <summary>
        /// 缺陷代码列表
        /// </summary>
        public EntityList<Defect> DefectList
        {
            get;
            set;
        }

        /// <summary>
        /// 缺陷项目列表
        /// </summary>
        public ObservableCollection<DefectItem> DefectItemList { get; }

        /// <summary>
        /// 引出不良录入窗口
        /// </summary>
        protected bool InputDefect()
        {
            var editor = DefectControlFactory.CreateControl();
            editor.AllowMultiple = true;
            editor.AllowQty = false;

            //缺陷代码列表
            editor.Defects.AddRange(this.DefectList);

            editor.SelectedValue.Clear();

            //已选择的缺陷，在弹出框的已选择缺陷中加入
            this.DefectItemList.ForEach(e =>
            {
                var item = new DefectItem()
                {
                    Defect = e.Defect
                };

                editor.SelectedValue.Add(item);
            });

            var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
            {
                w.Title = "不良代码录入".L10N();
                w.Height = 500;
                w.Width = 750;
                w.MinHeight = 350;
                w.MinWidth = 400;
            });

            if (result == 0)
            {
                try
                {
                    if (editor.SelectedValue.Any())
                    {
                        //已选择的缺陷 
                        this.DefectItemList.Clear();
                        editor.SelectedValue
                            .Select(p => new DefectItem
                            {
                                Defect = p.Defect,
                                Qty = p.Qty,
                            }).ForEach(x => this.DefectItemList.Add(x));

                        return true;
                    }
                }
                catch (Exception exc)
                {
                    exc.Alert();
                }
            }

            return false;
        }

        /// <summary>
        /// 缺陷代码选择控件
        /// </summary>
        public DefectControl DefectControl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="process"></param>
        protected void LoadDefectList(Process process)
        {
            if (DefectList == null)
            {
                return;
            }

            //清除缺陷代码列表
            DefectList.Clear();

            if (process != null
                && (process.ParameterList.Any(x => x.Type == ResultTypeForDesign.Fail)
                //|| process.Type == ProcessType.Fqc
                || process.Type == ProcessType.Pqc
                || process.Type == ProcessType.BatchPqc || process.Type == ProcessType.Fix))
            {
                if (process.ParameterList.Any(x => x.Type == ResultTypeForDesign.Fail))
                {
                    this.HaveFailParameter = true;
                }

                //加载工序对应的缺陷代码列表
                try
                {
                    DefectList.AddRange(RT.Service.Resolve<ProcessController>().GetProcessDefects(process.Id));
                    if (DefectControl != null)
                    {
                        DefectControl.Defects.Clear();
                        DefectControl.Defects.AddRange(DefectList);
                    }
                }
                catch (Exception exc)
                {
                    ShowError(exc);
                }
            }
            else
            {
                this.HaveFailParameter = false;
            }
        }

        #endregion

        /// <summary>
        /// 切换工单
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="workOrderId">工单ID</param>
        public virtual void ChangeWorkStationWorkOrder(Workcell workcell, double workOrderId)
        {
            if (workOrderId != WorkOrderId)
            {
                var wo = RF.GetById<WorkOrder>(workOrderId, new EagerLoadOptions().LoadWithViewProperty());

                if (WorkOrder != null)
                {
                    ShowTips("工单已切换,由[{0}]切换到[{1}]".L10nFormat(WorkOrder.No, wo.No));
                }

                WorkOrder = wo;

                var controller = RT.Service.Resolve<WipController>();
                controller.ChangeWipResourceWorkOrder(wo.Id, workcell);
            }
        }

        #region 工作单元

        /// <summary>
        /// 员工变更事件
        /// </summary>
        /// <param name="employee">员工</param>
        protected override void EmployeeChanged(SIE.Resources.Employee employee)
        {
            base.EmployeeChanged(employee);
            const RetrospectType retrospectType = Core.Items.RetrospectType.Batch;
            RefrshReportTasks(employee.Id, retrospectType, false);
        }

        /// <summary>
        /// 工序变更事件
        /// </summary>
        /// <param name="process">工序</param>
        protected override void ProcessChanged(Process process)
        {
            base.ProcessChanged(process);

            //检查结果改成合格
            this.Qualified = true;

            LoadDefectList(process);
        }

        /// <summary>
        /// 工位变更
        /// </summary>
        /// <param name="station">工位</param>
        protected override void StationChanged(Station station)
        {
            base.StationChanged(station);

            if (station != null)
            {
                RefreshStatistics();
                PanelBindingSnConfigValue = GetPanelBindingSnConfig(station);

                //初始化设备端口信息
                InitDevicePort();
            }
        }

        #endregion

        /// <summary>
        /// 历史消息控件（在界面模板 XXXUITemplate 中初始化的）
        /// </summary>
        public MessagerControl MessagerControl { get; set; }

        /// <summary>
        /// 添加历史消息到消息列表
        /// </summary>
        /// <param name="message"></param>
        /// <param name="messageType"></param>
        private void AddMessageToHistory(string message, MessageType messageType)
        {
            if (MessagerControl != null)
            {
                MessagerControl.AddMessage(message, messageType);
            }
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

    /// <summary>
    /// 数据采集基类
    /// </summary>
    [RootEntity, Serializable]
    public class WorkCellViewModel : ViewModel, IFocusTrigger
    {
        #region IFocusTrigger

        /// <summary>
        /// 聚焦事件
        /// </summary>
        public event EventHandler Focused;

        /// <summary>
        /// 触发条码输入框获取焦点
        /// </summary>
        public void FocuseBarcode()
        {
            Focused?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        /// <summary>
        /// 异步执行操作
        /// </summary>
        /// <param name="action">执行内容</param>
        protected void AsyncExecute(Action action)
        {
            Task.Run(new Action(() =>
            {
                try
                {
                    CRT.MainThread.InvokeAsync(() =>
                    {
                        action();
                    });
                }
                catch (Exception exc)
                {
                    ShowError(exc);
                }
            }).WithCurrentThreadContext());
        }

        /// <summary>
        /// 加载
        /// </summary>
        public virtual void Onload()
        {
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void OnClose()
        {

        }

        /// <summary>
        /// 重置界面数据
        /// </summary>
        public virtual void Reset(ResetType resetType)
        {

        }

        #region 工作单元信息
        /// <summary>
        /// 工作站信息
        /// </summary>
        public Workstation _workstation;

        /// <summary>
        /// 工作站
        /// </summary>
        public Workstation Workstation
        {
            get
            {
                if (_workstation == null)
                {
                    _workstation = new Workstation(this);
                }

                return _workstation;
            }
        }
        /// <summary>
        /// B版初始化工作站信息
        /// </summary>
        /// <param name="processTypes"></param>
        public virtual void InitWorkstationB(params ProcessType[] processTypes)
        {
            Workstation.PropertyChanged += OnWorkstationPropertyChanged;

            Workstation.ProcessTypes.AddRange(processTypes); //设置工作站工序类型
            Workstation.EmployeeId = CRT.IdentityId;

            if (!LoadWorkstation() //如果工作站信息不存在，或者与上次登录用户的资源工序工位分配不一样，重新选择
                && WorkstationSelectorB.SelectOperation(Workstation))
            {
                //有切换工作单元，则将工作单元信息保存在本地配置文件中
                SaveWorkstation();
            }

            var broken = Workstation.Validate(ValidatorActions.None);

            if (broken.Count > 0)
            {
                ShowError(broken.ToString());
            }
        }


        /// <summary>
        /// 初始化工作站信息
        /// </summary>
        /// <param name="processTypes">工序类型数值</param>
        public virtual void InitWorkstation(params ProcessType[] processTypes)
        {
            Workstation.PropertyChanged += OnWorkstationPropertyChanged;

            Workstation.ProcessTypes.AddRange(processTypes); //设置工作站工序类型
            Workstation.EmployeeId = CRT.IdentityId;

            if (!LoadWorkstation() //如果工作站信息不存在，或者与上次登录用户的资源工序工位分配不一样，重新选择
                && WorkstationSelector.SelectOperation(Workstation))
            {
                //有切换工作单元，则将工作单元信息保存在本地配置文件中
                SaveWorkstation();
            }

            var broken = Workstation.Validate(ValidatorActions.None);

            if (broken.Count > 0)
            {
                ShowError(broken.ToString());
            }
        }

        /// <summary>
        /// 工作单元属性变更
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void OnWorkstationPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Workstation.Employee))
            {
                EmployeeChanged(Workstation.Employee);
            }

            if (e.PropertyName == nameof(Workstation.Resource))
            {
                ResourceChanged(Workstation.Resource);
            }

            if (e.PropertyName == nameof(Workstation.Process) && (Workstation.Station != null || Workstation.Process != null))
            {
                ProcessChanged(Workstation.Process);
            }

            if (e.PropertyName == nameof(Workstation.Station))
            {
                StationChanged(Workstation.Station);
            }
        }


        /// <summary>
        /// 保存工作单元信息
        /// </summary>
        public void SaveWorkstation()
        {
            if (Workstation == null)
            {
                throw new PlatformException("工作单元未初始化".L10N());
            }

            _workcell = new Workcell()
            {
                EmployeeId = Workstation.EmployeeId ?? 0,
                ResourceId = Workstation.ResourceId ?? 0,
                ProcessId = Workstation.ProcessId ?? 0,
                StationId = Workstation.StationId ?? 0,
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

            var key = GetType().GetQualifiedName();
            data[key] = _workcell;
            Settings.Default.Workcell = JsonConvert.SerializeObject(data);
            Settings.Default.Save();
        }

        /// <summary>
        /// 加载工作单元信息
        /// </summary>
        /// <returns>加载成功返回true，失败返回false</returns>
        bool LoadWorkstation()
        {
            var setting = Settings.Default.Workcell;
            if (setting.IsNotEmpty())
            {
                var workcells = JsonConvert.DeserializeObject<Dictionary<string, Workcell>>(setting);
                var key = GetType().GetQualifiedName();
                if (workcells.ContainsKey(key))   //匹配工作单元
                {
                    var workcell = workcells[key];

                    if (workcell.ResourceId == 0 || workcell.StationId == 0 || workcell.ProcessId == 0)
                    {
                        return false;
                    }

                    ////如果与上次登录用户的资源工序工位分配不一样，打开时需要重新选
                    if (!CheckUserWorkStation(CRT.IdentityId, workcell.ResourceId, workcell.ProcessId, workcell.StationId))
                    {
                        return false;
                    }

                    Workstation.EmployeeId = CRT.IdentityId;
                    Workstation.ResourceId = workcell.ResourceId;
                    Workstation.ProcessId = workcell.ProcessId;

                    //检查员工是否具有当前工序所需的技能
                    if (Workstation.ProcessId.HasValue
                        && !RT.Service.Resolve<ProcessController>().IsEmpHasProcessSkill(
                            Workstation.ProcessId.Value, Workstation.EmployeeId.Value))
                    {
                        return false;
                    }

                    Workstation.StationId = workcell.StationId;

                    EmployeeChanged(Workstation.Employee);
                    ResourceChanged(Workstation.Resource);
                    ProcessChanged(Workstation.Process);
                    StationChanged(Workstation.Station);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 加载工作单元信息
        /// </summary>
        /// <returns>加载成功返回true，失败返回false</returns>
        bool LoadRegionWorkstation()
        {
            var setting = Settings.Default.Workcell;
            if (setting.IsNotEmpty())
            {
                var workcells = JsonConvert.DeserializeObject<Dictionary<string, Workcell>>(setting);
                var key = GetType().GetQualifiedName();
                if (workcells.ContainsKey(key))   //匹配工作单元
                {
                    var workcell = workcells[key];

                    if (workcell.ResourceId == 0 || workcell.StationId == 0 || workcell.ProcessId == 0)
                    {
                        return false;
                    }

                    ////如果与上次登录用户的资源工序工位分配不一样，打开时需要重新选
                    if (!CheckUserWorkStation(CRT.IdentityId, workcell.ResourceId, workcell.ProcessId, workcell.StationId))
                    {
                        return false;
                    }

                    Workstation.EmployeeId = CRT.IdentityId;
                    Workstation.ResourceId = workcell.ResourceId;
                    Workstation.ProcessId = workcell.ProcessId;

                    //检查员工是否具有当前工序所需的技能
                    if (Workstation.ProcessId.HasValue
                        && !RT.Service.Resolve<ProcessController>().IsEmpHasProcessSkill(
                            Workstation.ProcessId.Value, Workstation.EmployeeId.Value))
                    {
                        return false;
                    }

                    Workstation.StationId = workcell.StationId;

                    EmployeeChanged(Workstation.Employee);
                    ResourceChanged(Workstation.Resource);
                    ProcessChanged(Workstation.Process);
                    StationChanged(Workstation.Station);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 根据用户查找是否匹配传入的工序，资源，工位
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="resourceId">产线ID</param>
        /// <param name="processId">工序ID</param>
        /// <param name="stationId">工位ID</param>
        /// <returns>匹配返回true，否则返回false</returns>
        bool CheckUserWorkStation(double userId, double resourceId, double processId, double stationId)
        {
            var ctlResource = RT.Service.Resolve<EmployeeController>();
            if (!ctlResource.UserHasResource(userId, resourceId))
            {
                return false;
            }

            var ctlProcess = RT.Service.Resolve<ProcessController>();

            if (!ctlProcess.EmployeeHasProcess(userId, processId))
            {
                return false;
            }

            var ctlStation = RT.Service.Resolve<StationController>();
            var station = ctlStation.GetStation(stationId);

            if (station == null)
            {
                return false;
            }

            if (station.ResourceId != resourceId)
            {
                return false;
            }

            var processIds = station.StationProcessList.Select(p => p.ProcessId).ToList();

            if (!processIds.Contains(processId))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获取采集单元信息
        /// </summary>
        /// <returns>工作单元</returns>
        public Workcell GetWorkcell()
        {
            if (_workcell == null)
            {
                var broken = Workstation.Validate(ValidatorActions.None);
                if (broken.Count > 0)
                    throw new ValidationException(broken.ToString());
                _workcell = new Workcell();
                _workcell.EmployeeId = Workstation.EmployeeId.Value;
                _workcell.ProcessId = Workstation.ProcessId.Value;
                _workcell.StationId = Workstation.StationId.Value;
                _workcell.ResourceId = Workstation.ResourceId.Value;
            }

            return _workcell;
        }

        /// <summary>
        /// 工作单元信息
        /// </summary>
        Workcell _workcell;

        /// <summary>
        /// 员工变更事件
        /// </summary>
        /// <param name="employee">员工</param>
        protected virtual void EmployeeChanged(SIE.Resources.Employee employee)
        {
        }

        /// <summary>
        /// 资源变更事件
        /// </summary>
        /// <param name="resource">资源</param>
        protected virtual void ResourceChanged(WipResource resource)
        {
        }

        /// <summary>
        /// 工序变更事件
        /// </summary>
        /// <param name="process">工序</param>
        protected virtual void ProcessChanged(Process process)
        {
        }

        /// <summary>
        /// 工位变更
        /// </summary>
        /// <param name="station">工位</param>
        protected virtual void StationChanged(Station station)
        {
        }
        #endregion

        #region 消息提示
        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        public virtual void ShowError(string error)
        {
            if (error == null)
            {
                return;
            }

            ClientRuntime.MessageService.ShowError(error.Replace("\r\n", string.Empty));
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="exc">异常</param>
        public virtual void ShowError(Exception exc)
        {
            var validationException = exc.GetBaseException() as ValidationException;
            if (validationException != null)
            {
                ShowError(DisplayHelper.Display(validationException.Message));
            }
            else
            {
                Extenstion.Alert(exc);
            }
        }
        #endregion
    }
}