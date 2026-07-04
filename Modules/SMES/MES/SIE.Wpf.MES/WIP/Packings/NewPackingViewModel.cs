using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Logging;
using SIE.ManagedProperty;
using SIE.MES.PackingPrints;
using SIE.MES.WIP;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Packings;
using SIE.MES.WIP.Packings.Configs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.Packings.Enums;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Tech.Stations.Configs;
using SIE.Threading;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.Properties;
using SIE.Wpf.MES.WIP.Packings.Commands;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 包装采集
    /// </summary>
    [EntityWithConfig(typeof(WeightConfig))]
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [EntityWithConfig(typeof(WeightSerialProtsConfig))]
    [EntityWithConfig(typeof(WipPackingConfig))]
    [EntityWithConfig(typeof(WipPackingBillConfig))]
    //[EntityWithConfig(typeof(PackingPrintModeConfig))]
    [RootEntity]
    [Label("包装采集")]
    public partial class NewPackingViewModel : DataCollectionViewModel<NewWipPackingController>
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        readonly ILog logger = LogManager.GetLogger("wip");

        /// <summary>
        /// 包装帮助类
        /// </summary>
        readonly NewPackingHelper _helper;

        /// <summary>
        /// 包装帮助类
        /// </summary>
        public NewPackingHelper Helper { get { return _helper; } }

        /// <summary>
        /// 当前处理的包装关系
        /// </summary>
        public PackingRelation CurrentPackingRelation { get; set; }

        /// <summary>
        /// 手动指定包装号
        /// </summary>
        protected Queue<string> ScannedPackageNos { get; } = new Queue<string>();

        /// <summary>
        /// 内部编码
        /// </summary>
        private string InsideBarcode { get; set; }

        /// <summary>
        /// 待扫描包装号
        /// </summary>
        private Queue<PackingUnit> ToScanPackageUnits { get; set; }

        /// <summary>
        /// 当前条码对应的包装单位
        /// </summary>
        protected PackingUnit CurrentPackingUnit { get; set; }

        /// <summary>
        /// 当前条码
        /// </summary>
        protected string CollectBarcode { get; set; }

        /// <summary>
        /// 是否需要过站
        /// </summary>
        private bool needMove { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public NewPackingViewModel()
        {
            PrintMode = PrintMode.Online;
            InitWorkstation(ProcessType.Packing);
            Printer = Settings.Default.PackingVMPrinter;

            IsAutoPrintLabel = true;
            _helper = new NewPackingHelper(this);

            //初始化重量信息
            WeightInfo = new WeightInfo();

            ToScanPackageUnits = new Queue<PackingUnit>();
        }

        #region 称重
        #region 重量信息 Weight
        /// <summary>
        /// 重量信息
        /// </summary>
        [Label("重量信息")]
        public static readonly Property<WeightInfo> WeightInfoProperty = P<NewPackingViewModel>.Register(e => e.WeightInfo);

        /// <summary>
        /// 重量信息
        /// </summary>
        public WeightInfo WeightInfo
        {
            get { return this.GetProperty(WeightInfoProperty); }
            set { this.SetProperty(WeightInfoProperty, value); }
        }
        #endregion

        #region 重量上限 UpperLimit
        /// <summary>
        /// 重量上限
        /// </summary>
        public static readonly Property<decimal?> UpperLimitProperty = P<NewPackingViewModel>.RegisterReadOnly(
                e => e.UpperLimit, e => e.WorkOrder.Product.UpperWeight, WorkOrderProperty);

        /// <summary>
        /// 重量上限
        /// </summary>
        public decimal? UpperLimit
        {
            get { return this.GetProperty(UpperLimitProperty); }
        }
        #endregion

        #region 重量下限 LowerLimit
        /// <summary>
        /// 重量下限
        /// </summary>
        public static readonly Property<decimal?> LowerLimitProperty = P<NewPackingViewModel>.RegisterReadOnly(
            e => e.LowerLimit, e => e.WorkOrder.Product.LowerWeight, WorkOrderProperty);

        /// <summary>
        /// 重量下限
        /// </summary>
        public decimal? LowerLimit
        {
            get { return this.GetProperty(LowerLimitProperty); }
        }
        #endregion

        #region 是否称重 IsWeight
        /// <summary>
        /// 是否称重
        /// </summary>
        [Label("是否称重")]
        public static readonly Property<bool> IsWeightProperty = P<NewPackingViewModel>.Register(e => e.IsWeight);

        /// <summary>
        /// 是否称重
        /// </summary>
        public bool IsWeight
        {
            get { return this.GetProperty(IsWeightProperty); }
            set { this.SetProperty(IsWeightProperty, value); }
        }
        #endregion 

        /// <summary>
        /// 初始化称重配置
        /// </summary>
        private void InitWeightConfig()
        {
            AsyncExecute(() =>
            {
                var weightConfig = ConfigService.GetConfig(new WeightConfig(), GetType(), ResourceStation.Find(Workstation.Station));
                IsWeight = (weightConfig != null) && weightConfig.IsWeight;
                if (IsWeight)
                    OpenWeightSerial();
            });
        }
        #endregion

        #region 控制设置
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("默认打印机")]
        public static readonly Property<string> PrinterProperty = P<NewPackingViewModel>.Register(e => e.Printer, new PropertyMetadata<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as NewPackingViewModel).OnPrinterChanged()
        });

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }

        /// <summary>
        /// 打印机变更事件
        /// </summary>
        void OnPrinterChanged()
        {
            Settings.Default.PackingVMPrinter = Printer;
            Settings.Default.Save();
            FocuseBarcode();
        }

        /// <summary>
        /// 是否自动打印
        /// </summary>
        [Label("自动打印标签")]
        public static readonly Property<bool> IsAutoPrintLabelProperty = P<NewPackingViewModel>.Register(e => e.IsAutoPrintLabel, new PropertyMetadata<bool>() { PropertyChangedCallBack = (o, e) => (o as NewPackingViewModel).FocuseBarcode() });

        /// <summary>
        /// 是否自动打印
        /// </summary>
        public bool IsAutoPrintLabel
        {
            get { return this.GetProperty(IsAutoPrintLabelProperty); }
            set { this.SetProperty(IsAutoPrintLabelProperty, value); }
        }

        /// <summary>
        /// 自动打包方式
        /// </summary>
        [Label("自动打包模式")]
        public static readonly Property<AutoDoPackingMode> AutoPackingModeProperty = P<NewPackingViewModel>.Register(e => e.AutoDoPackingMode, new PropertyMetadata<AutoDoPackingMode>() { PropertyChangedCallBack = (o, e) => (o as NewPackingViewModel).OnPackingModelChange() });

        /// <summary>
        /// 自动打包属性变更回调方法
        /// </summary>
        private void OnPackingModelChange()
        {
            FocuseBarcode();
            //ResetOuterRelation();
        }

        /// <summary>
        /// 自动打包方式
        /// </summary>
        public AutoDoPackingMode AutoDoPackingMode
        {
            get { return this.GetProperty(AutoPackingModeProperty); }
            set { this.SetProperty(AutoPackingModeProperty, value); }
        }
        #endregion


        #region 打印方式 PrintMode
        /// <summary>
        /// 打印方式
        /// </summary>
        [Label("打印方式")]
        public static readonly Property<PrintMode> PrintModeProperty = P<NewPackingViewModel>.Register(e => e.PrintMode);

        /// <summary>
        /// 打印方式
        /// </summary>
        public PrintMode PrintMode
        {
            get { return this.GetProperty(PrintModeProperty); }
            set { this.SetProperty(PrintModeProperty, value); }
        }
        #endregion

        #region 扫描方式 ScanMode 
        /// <summary>
        /// 扫描方式
        /// </summary>
        [Label("扫描方式")]
        public static readonly Property<ScanMode> ScanModeProperty = P<NewPackingViewModel>.Register(e => e.ScanMode, new PropertyMetadata<ScanMode>() { PropertyChangedCallBack = (o, e) => (o as NewPackingViewModel).FocuseBarcode() });

        /// <summary>
        /// 扫描方式
        /// </summary>
        public ScanMode ScanMode
        {
            get { return this.GetProperty(ScanModeProperty); }
            set { this.SetProperty(ScanModeProperty, value); }
        }
        #endregion

        #region 是否扫描包装号 IsPackage
        /// <summary>
        /// 是否包装号
        /// </summary>
        [Label("是否扫描包装号")]
        public static readonly Property<bool> IsPackageProperty = P<NewPackingViewModel>.Register(e => e.NeedScanPackageNo);

        /// <summary>
        /// 是否扫描包装号
        /// </summary>
        public bool NeedScanPackageNo
        {
            get { return this.GetProperty(IsPackageProperty); }
            set { this.SetProperty(IsPackageProperty, value); }
        }
        #endregion 

        #region 包装关系列表 PackingRelations 
        /// <summary>
        /// 包装关系列表
        /// </summary>
        [Label("包装关系")]
        public static readonly ListProperty<EntityList<PackingRelation>> PackingRelationsProperty = P<NewPackingViewModel>.RegisterList(e => e.PackingRelations, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<PackingRelation>()
        });

        /// <summary>
        /// 包装关系列表
        /// </summary>
        public EntityList<PackingRelation> PackingRelations
        {
            get { return this.GetLazyList(PackingRelationsProperty); }
        }
        #endregion

        #region 产品条码列表 ItemLabelList 
        /// <summary>
        /// 产品条码列表
        /// </summary>
        [Label("产品条码")]
        public static readonly ListProperty<EntityList<ItemLabel>> ItemLabelListProperty = P<NewPackingViewModel>.RegisterList(e => e.ItemLabelList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<ItemLabel>()
        });

        /// <summary>
        /// 产品条码列表
        /// </summary>
        public EntityList<ItemLabel> ItemLabelList
        {
            get { return this.GetLazyList(ItemLabelListProperty); }
        }
        #endregion

        #region 工单包装规则列表 
        /// <summary>
        /// 工单包装规则
        /// </summary>
        public static readonly ListProperty<EntityList<WorkOrderPackageRuleDetail>> PackageRuleDetailListProperty = P<NewPackingViewModel>.RegisterList(e => e.PackageRuleDetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as NewPackingViewModel).LoadPackageRuleDetailList()
        });

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public EntityList<WorkOrderPackageRuleDetail> PackageRuleDetailList
        {
            get { return this.GetLazyList(PackageRuleDetailListProperty); }
        }

        /// <summary>
        /// 创建工单包装规则列表
        /// </summary>
        /// <returns>返回新的工单包装规则</returns>
        private EntityList<WorkOrderPackageRuleDetail> LoadPackageRuleDetailList()
        {
            return new EntityList<WorkOrderPackageRuleDetail>();
        }
        #endregion

        #region 称重设备
        /// <summary>
        /// 通信串口
        /// </summary>
        readonly Dictionary<object, string> serialsRegex = new Dictionary<object, string>();

        /// <summary>
        /// 打开串口
        /// </summary>
        void OpenWeightSerial()
        {
            //初始化串口信息可配置多个串口
            var serialPortsConfig = ConfigService.GetConfig(new WeightSerialProtsConfig(), GetType(), ResourceStation.Find(Workstation.Station));
            foreach (var s in serialPortsConfig.SerialPortList)
            {
                var serialPort = new System.IO.Ports.SerialPort();
                serialsRegex.Add(serialPort, s.Regular);
                serialPort.PortName = s.PortName.ToString();
                serialPort.BaudRate = s.BaudRate;
                serialPort.DataReceived += Serial_DataReceived;
                try
                {
                    serialPort.Open();
                }
                catch (Exception exc)
                {
                    CRT.MessageService.ShowError("打开串口[{0}]失败:".L10nFormat(s.PortName) + exc.Message);
                }
            }
        }

        /// <summary>
        /// 串口数据保存后
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">串口数据保存后事件参数</param>
        private void Serial_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                var source = (sender as System.IO.Ports.SerialPort).ReadLine().TrimEnd('\n', '\r', '\0').TrimStart('\0');

                decimal inputVal = 0M;

                if (serialsRegex.ContainsKey(sender) && !string.IsNullOrEmpty(serialsRegex[sender]))
                {
                    Regex regex = new Regex(serialsRegex[sender], RegexOptions.IgnoreCase);
                    var result = regex.Match(source);

                    if (result != null)
                    {
                        decimal.TryParse(result.Value, out inputVal);
                    }
                }
                else
                {
                    decimal.TryParse(source, out inputVal);
                }

                ReadWeight(inputVal);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 串口读取数据
        /// </summary>
        /// <param name="weight">重量</param>
        private void ReadWeight(decimal weight)
        {
            WeightInfo = new WeightInfo()
            {
                Weight = weight,
                LowerLimit = WorkOrder?.Product?.LowerWeight,
                UpperLimit = WorkOrder?.Product?.UpperWeight,
            };
        }

        /// <summary>
        /// 关闭通信串口
        /// </summary>
        void CloseWeightSerial()
        {
            foreach (System.IO.Ports.SerialPort serial in serialsRegex.Keys)
                if (serial.IsOpen)
                    serial.Close();
        }
        #endregion


        /// <summary>
        /// 加载工作站数据
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();

            IsAutoPrintLabel = true;
            AutoDoPackingMode = AutoDoPackingMode.AutoCasePacking;

            if (Workstation.Station != null)
            {
                //加载工位对应的配置项
                var _config = ConfigService.GetConfig(new WipPackingConfig(), this.GetType(),
                    ResourceStation.Find(Workstation.Station));

                if (_config != null)
                {
                    IsAutoPrintLabel = _config.IsAutoPrintPackageLabel;
                    AutoDoPackingMode = _config.AutoDoPackingMode;
                }
            }

            if (Workstation.Resource != null)
            {
                //加载产线对应的配置项
                var cfg = ConfigService.GetConfig(new NewPackingPrintModeConfig(),typeof(Station), Workstation.Station);

                if (cfg != null && cfg.PrintMode == PrintMode.InAdvance)
                {
                    IsAutoPrintLabel = false;
                }
            }
        }

        /// <summary>
        /// 关闭时
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
            CloseWeightSerial();
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        public void ShowTips(string collectBarcode, PackingRelation outerPackingRelation)
        {
            if (outerPackingRelation != null && !outerPackingRelation.PackageNo.IsNullOrEmpty())
            {
                ShowTips("[条码:{0}]采集成功,生成【{1}:{2}】 请继续扫描产品条码"
                    .L10nFormat(collectBarcode, outerPackingRelation.PackageUnitName, outerPackingRelation.PackageNo));
            }
            else
            {
                ShowTips("[条码:{0}]采集成功,请继续扫描产品条码".L10nFormat(collectBarcode));
            }
        }

        /// <summary>
        /// 显示加入提示信息
        /// </summary>
        public void ShowJoinTips(PackingRelation packingRelation)
        {
            ShowTips("包装:【{0}】识别成功,请扫描要加入的产品条码".L10nFormat(packingRelation.PackageUnitName));
        }

        /// <summary>
        /// 重置采集步骤
        /// </summary>
        public void ResetStep()
        {
            Step.Reset();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);
            CurrentPackingRelation = null;
            CurrentPackingUnit = null;
            CollectBarcode = null;
            NeedScanPackageNo = false;
            ScannedPackageNos.Clear();
            ToScanPackageUnits.Clear();
            ReloadPackingRelation();
        }

        /// <summary>
        /// 重置包装
        /// </summary>
        public void ResetPacking()
        {
            CurrentPackingUnit = null;
            CollectBarcode = null;
            ScannedPackageNos.Clear();
            ToScanPackageUnits.Clear();
            Barcode = string.Empty;
            InsideBarcode = string.Empty;
        }

        /// <summary>
        /// 重置外包装
        /// </summary>
        public void ResetOuterRelation()
        {
            CurrentPackingRelation = null;
            ResetPacking();
        }

        /// <summary>
        /// 重新加载工位工序未完成的包装关系
        /// </summary>
        public async Task ReloadPackingRelation()
        {
            if (Workstation.Process == null || Workstation.Station == null)
            {
                return;
            }
            var unpackage = await FindWorkPackingRelationByStation().ConfigureAwait(true);

            PackingRelations.Clear();
            ItemLabelList.Clear();
            PackingRelations.AddRange(unpackage);
            PackingRelations.MarkSaved();
        }

        /// <summary>
        /// 加载工位未满包装的包装关系
        /// </summary>
        /// <returns>包装关系列表</returns>
        async Task<EntityList<PackingRelation>> FindWorkPackingRelationByStation()
        {
            return await Task.Run(new Func<EntityList<PackingRelation>>(() =>
            RT.Service.Resolve<PackingRelationController>()
                .FindWorkPackingRelationByStation(Workstation.ProcessId, Workstation.StationId))
                .WithCurrentThreadContext()).ConfigureAwait(true);
        }

        /// <summary>
        /// 初始化包装号打印模式
        /// </summary>
        private void InitPrintModeConfig(WipResource resource)
        {
            AsyncExecute(() =>
            {
                var config = ConfigService.GetConfig(new NewPackingPrintModeConfig(), typeof(Station), Workstation.Station);
                PrintMode = config == null ? PrintMode.Online : config.PrintMode;
            });
        }

        /// <summary>
        /// 资源变更事件
        /// </summary>
        /// <param name="resource">资源</param>
        protected override void ResourceChanged(WipResource resource)
        {
            base.ResourceChanged(resource);
            InitPrintModeConfig(resource);
        }

        /// <summary>
        /// 工位变更
        /// </summary>
        /// <param name="station">工位</param>
        protected override void StationChanged(Station station)
        {
            base.StationChanged(station);
            CloseWeightSerial();
            if (station != null)
            {
                InitWeightConfig();
            }
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            ReloadPackingRelation();
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e">托管属性变更事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == WorkOrderProperty)
            {
                LoadPackageRuleDtl();
                FocuseBarcode();

            }
            else if (e.Property == ScanModeProperty && ScanMode == ScanMode.Join)
            {
                ResetOuterRelation();
                ShowTips("已切换扫描模式:{0},请先扫描同一包装里的条码,进行加入扫码".L10nFormat(ScanMode.ToLabel()));
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 加载工单包装规则明细
        /// </summary>
        private void LoadPackageRuleDtl()
        {
            AsyncExecute(() =>
            {
                PackageRuleDetailList.Clear();
                if (WorkOrder != null && WorkOrder.PackageRuleDetailList.Count > 0)
                {
                    PackageRuleDetailList.AddRange(WorkOrder.PackageRuleDetailList);
                    PackageRuleDetailList.MarkSaved();
                }
            });
        }

        /// <summary>
        /// 条码变更事件
        /// </summary>
        /// <param name="e">托管属性变更事件参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty())
                return;
            using (PerformenceWatcher.Start(logger, "OnBarcodeChanged"))
            {
                try
                {
                    ClearInfos();
                    if (ScanMode != ScanMode.Join)
                    {
                        bool isPass = false;
                        isPass = NeedScanPackageNo ? ValidatePackageNo(Barcode, CurrentPackingRelation)
                            : ValidateBarcode(Barcode, CurrentPackingRelation);

                        if (isPass)
                        {
                            PackingCollect();
                        }
                    }
                    else
                    {
                        var workcell = GetWorkcell();
                        var currentStep = Step.CurrentStep;
                        var collectBarcode = new CollectBarcode { Code = Barcode, Type = currentStep.BarcodeType };
                        var collectData = new CollectData() { CollectBarcode = collectBarcode };

                        //找到条码的最外层，没有包装完成的包装关系
                        collectData.PackingData.OuterPackingRelationId = CurrentPackingRelation?.Id;
                        JoinPacking(Barcode, collectData, workcell);
                        RefreshStatistics();
                        RefrshReportTasks();
                        Step.Reset();

                        //切换回 正常 包装模式
                        ScanMode = ScanMode.Normal;
                    }
                }
                catch (Exception exc)
                {
                    ShowError(exc);
                }
                finally
                {
                    Barcode = string.Empty;
                }
            }
        }

        /// <summary>
        /// 验证条码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="currentPackingRelation"></param>
        protected bool ValidateBarcode(string barcode, PackingRelation currentPackingRelation)
        {
            var workcell = GetWorkcell();
            var collectBarcode = new CollectBarcode { Code = barcode, Type = Step.CurrentStep.BarcodeType };
            collectBarcode.Context["PrintMode"] = PrintMode;
            collectBarcode.Context["OuterPackingRelation"] = CurrentPackingRelation;
            collectBarcode.Context["AutoDoPackingMode"] = AutoDoPackingMode;
            var productInfo = Validate(collectBarcode, workcell);

            if (currentPackingRelation != null
                && productInfo.WorkOrderId != currentPackingRelation.WorkOrderId)
            {
                ShowError("条码[{0}]的工单与当前采集的包装的工单不相同".L10nFormat(barcode));
                return false;
            }

            CollectBarcode = barcode;

            CurrentPackingUnit = productInfo.Context["PackingUnit"] as PackingUnit;
            ToScanPackageUnits = productInfo.Context["ToScanPackageUnits"] as Queue<PackingUnit>;

            //是否需要过站
            needMove = (bool)productInfo.Context["NeedMove"];

            NeedScanPackageNo = false;

            if (ToScanPackageUnits.Any())
            {
                NeedScanPackageNo = true;
                ShowTips("请扫描[{0}]包装条码".L10nFormat(ToScanPackageUnits.Peek().Name));
                return false;
            }

            return true;
        }

        /// <summary>
        /// 包装采集
        /// </summary>
        protected void PackingCollect()
        {
            var workcell = GetWorkcell();

            var collectData = new CollectData()
            {
                CollectBarcode = new CollectBarcode()
                {
                    Code = CollectBarcode,
                    Type = Step.CurrentStep.BarcodeType
                }
            };

            collectData.PackingData.OuterPackingRelationId = CurrentPackingRelation?.Id;
            collectData.PackingData.PackingMode = AutoDoPackingMode;
            collectData.PackingData.PrintMode = PrintMode;
            collectData.PackingData.ScannedPackageNos = ScannedPackageNos;
            collectData.PackingData.CurrentPackingUnit = CurrentPackingUnit;

            //过站 有处理包装关系            
            CurrentPackingRelation = Controller.PackingCollect(CollectBarcode, collectData, workcell, needMove);

            OnSucess(collectData.PackingData.CurrentPackingUnit);

            CollectBarcode = null;
            CurrentPackingUnit = null;
            RefreshStatistics();
            RefrshReportTasks();
        }

        /// <summary>
        /// 验证指定包装号
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <param name="currentPackingRelation"></param>
        protected virtual bool ValidatePackageNo(string packageNo, PackingRelation currentPackingRelation)
        {
            // 验证包装号
            // 增加验证包装号是否要采集的包装单位
            RT.Service.Resolve<PackingBarcodeController>()
                .ValidatePackingBarcode(packageNo, ToScanPackageUnits.Peek(),  currentPackingRelation);

            ScannedPackageNos.Enqueue(packageNo);

            ToScanPackageUnits.Dequeue();

            if (ToScanPackageUnits.Any())
            {
                ShowTips("请扫描[{0}]包装条码".L10nFormat(ToScanPackageUnits.Peek().Name));
                return false;
            }
            else
            {
                NeedScanPackageNo = false;
                return true;
            }
        }

        #region 加入包装

        /// <summary>
        /// 加入包装
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        private void JoinPacking(string barcode, CollectData collectData, SIE.MES.WIP.Workcell workcell)
        {
            try
            {
                var curRelation = RT.Service.Resolve<PackingRelationController>().GetPackingRelation(barcode, false);

                if (curRelation == null)
                {
                    var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
                    if (itemLabel?.Relation != null)
                    {
                        curRelation = itemLabel.Relation;
                    }
                }

                if (curRelation == null)
                {
                    throw new ValidationException("系统无此条码[{0}]包装记录".L10nFormat(barcode));
                }

                // 获取最外层的外包装关系
                PackingRelation _outPackRelation = null;
                if (curRelation.RootId > 0)
                {
                    _outPackRelation = RF.GetById<PackingRelation>(curRelation.RootId);
                }
                else
                {
                    //这里为兼容原来没有写入 RootId 的包装关系，递归查找最外层的包装关系

                    int count = 0;

                    // 循环上钻获取最外层关系
                    while (curRelation.GetTreePId() != null)
                    {
                        if (curRelation.GetTreePId() != null)
                        {
                            curRelation = RF.GetById<PackingRelation>(curRelation.TreePId);
                        }

                        count++;
                        if (count > 20)
                        {
                            throw new ValidationException("此条码[{0}]包装关系异常，请联系管理员处理。".L10nFormat(barcode));
                        }
                    }

                    _outPackRelation = curRelation;
                }

                if (_outPackRelation == null)
                {
                    throw new ValidationException("系统无此条码[{0}]包装记录".L10nFormat(barcode));
                }

                if (_outPackRelation.IsPacked)
                {
                    throw new ValidationException("加入包装失败，{0}[{1}]已打包完成，无法加入操作。"
                        .L10nFormat(curRelation.PackageUnitName, barcode));
                }

                CurrentPackingRelation = _outPackRelation;

                SetParentNode(barcode, curRelation);

                CollectBarcode = barcode;

                OnSucess(collectData.PackingData.DesignatedOuterPackingUnit);

                CollectBarcode = null;
            }
            catch (Exception ex)
            {
                OnFaile(barcode, ex);
                throw;
            }
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="barcode">编码</param>
        /// <param name="packRelation">层级包装关系</param>
        protected virtual void SetParentNode(string barcode, PackingRelation packRelation)
        {

        }

        #endregion

        /// <summary>
        /// 工单包装规则明细
        /// </summary>
        public virtual WorkOrderPackageRuleDetail CurrentPackageRuleLevel
        {
            get
            {
                if (WorkOrder == null)
                {
                    return null;
                }
                return WorkOrder.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == CurrentPackingRelation?.PackageUnitId);
            }
        }

        /// <summary>
        /// 采集成功
        /// </summary>
        /// <param name="unit">包装单位</param>
        private void OnSucess(PackingUnit unit)
        {
            var packingEvent = Controller.CreatePackingEvent("MesPacking", ScanMode, CollectBarcode, CurrentPackingRelation, WorkOrder, unit);
            RT.EventBus.Publish(packingEvent);
        }

        /// <summary>
        /// 采集失败
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="ex">异常信息</param>
        private void OnFaile(string barcode, Exception ex)
        {
            string msg = string.Format("[{0}]采集失败:{1}", barcode, ex.Message);
            ShowError(msg);
            Step.Reset();
        }
    }

    /// <summary>
    /// 包装采集视图配置
    /// </summary>
    public class NewPackingViewModelViewConfig : WPFViewConfig<NewPackingViewModel>
    {
        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(NewPackingViewModel));
            View.AssignAuthorize(typeof(JoinPackingViewModel));
            View.UseCommands(typeof(NewPackingResetCommand));
            View.UseDetail(columnCount: 6);
            using (View.DeclareGroup(string.Empty))
            {
                View.Property(p => p.Tips).UseEditor(TipsEditor.EditorName).ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
                View.Property(p => p.Error).UseEditor(ErrorEditor.EditorName).ShowInDetail(columnSpan: 6, height: 0, hideLabel: true);
            }
            using (View.DeclareGroup("扫描信息".L10N(), detailColumnCount: 8))
            {
                View.Property(p => p.Barcode).UseEditor(BarcodeEditor.EditorName).ShowInDetail(columnSpan: 5, height: 0, hideLabel: true);
                View.Property(p => p.WeightInfo).UseEditor(WeighEditor.EditorName).ShowInDetail(columnSpan: 1, height: 0, hideLabel: true);
                View.Property(p => p.ScanMode).UseEditor(EnumButtonEditor.EditorName).ShowInDetail(columnSpan: 2, height: 0, hideLabel: true);
            }
            using (View.DeclareGroup("工单信息".L10N(), detailColumnCount: 5, collapsable: true))
            {
                View.Property(p => p.WorkOrder.No).UseEditor(WorkOrderEditor.EditorName).HasLabel("工单号").ShowInDetail();
                View.Property(p => p.WorkOrder.Product.Name).HasLabel("产品名称").ShowInDetail();
                View.Property(p => p.WorkOrder.Product.Code).HasLabel("产品编码").ShowInDetail();
                View.Property(p => p.WorkOrder.Product.Model.Name).HasLabel("产品型号").ShowInDetail();
                View.Property(p => p.QtyPass).UseHighlightEditor("#000000", "#############################").HasLabel("当班采集数").ShowInDetail().Readonly(true);
                View.Property(p => p.Printer).HasLabel("指定打印机").UseEditor(PrinterEditor.EditorName)//.Show(ShowInWhere.All)
                    .Visibility(NewPackingViewModel.IsAutoPrintLabelProperty).ShowInDetail(columnSpan: 2);
            }
            //using (View.DeclareGroup("控制选项".L10N(), detailColumnCount: 6))
            //{
            //    //View.Property(p => p.AutoDoPackingMode).ShowInDetail(columnSpan: 4, height: 0, hideLabel: true).UseEditor(EnumButtonEditor.EditorName);
            //    //View.Property(p => p.IsAutoPrintLabel).ShowInDetail(height: 0, hideLabel: true).UseEditor(BoolToggleButtonEditor.EditorName);
            //}
            View.ChildrenProperty(p => p.PackingRelations).Show(ChildShowInWhere.Detail).HasLabel("包装关系").Readonly().UseViewGroup(PackingRelationViewConfig.NewPackingView);
            View.ChildrenProperty(p => p.ItemLabelList).Show(ChildShowInWhere.Detail).HasLabel("产品条码").Readonly().UseViewGroup(ItemLabelViewConfig.PackingView);
            View.ChildrenProperty(p => p.PackageRuleDetailList).Show(ChildShowInWhere.Detail).HasLabel("包装规则").Readonly().UseViewGroup(PackageRuleDetailViewConfig.PackingView);
        }
    }
}