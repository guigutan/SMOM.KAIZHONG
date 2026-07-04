using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Core.Labels;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Logging;
using SIE.ManagedProperty;
using SIE.MES.PackingPrints;
using SIE.MES.WIP;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.NewPackages;
using SIE.MES.WIP.Packings;
using SIE.MES.WIP.Packings.Configs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.Packings.Enums;
using SIE.Packages.Printables;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Tech.Stations.Configs;
using SIE.Threading;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.Properties;
using SIE.Wpf.MES.WIP.NewPackages;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 直接包装采集
    /// </summary>
    //[EntityWithConfig(typeof(DirectWeightConfig))]
    [EntityWithConfig(typeof(DirectDevicePortConfig))]
    [EntityWithConfig(typeof(DirectSerialPortsConfig))]
    [EntityWithConfig(typeof(DirectWeightSerialProtsConfig))]
    //[EntityWithConfig(typeof(DirectWipPackingConfig))]
    //[EntityWithConfig(typeof(DirectWipPackingBillConfig))]
    //[EntityWithConfig(typeof(DirectPackingPrintModeConfig))]
    [RootEntity]
    [Label("直接包装采集")]
    public class DirectPackingViewModel : DataCollectionViewModel<DirectPackingController>
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        readonly ILog logger = LogManager.GetLogger("wip");

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
        /// 当前条码的上一层包装单位
        /// </summary>
        protected PackingUnit OuterPackingUnit { get; set; }

        /// <summary>
        /// 当前条码
        /// </summary>
        protected string CollectBarcode { get; set; }

        /// <summary>
        /// 是否需要过站
        /// </summary>
        private bool needMove { get; set; }

        #region 称重
        #region 重量信息 Weight
        /// <summary>
        /// 重量信息
        /// </summary>
        [Label("重量信息")]
        public static readonly Property<WeightInfo> WeightInfoProperty = P<DirectPackingViewModel>.Register(e => e.WeightInfo);

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
        [Label("重量上限")]
        public static readonly Property<decimal?> UpperLimitProperty = P<DirectPackingViewModel>.RegisterReadOnly(e => e.UpperLimit, e => e.WorkOrder.Product.UpperWeight, WorkOrderProperty);

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
        public static readonly Property<decimal?> LowerLimitProperty = P<DirectPackingViewModel>.RegisterReadOnly(
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
        public static readonly Property<bool> IsWeightProperty = P<DirectPackingViewModel>.Register(e => e.IsWeight);

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
                var weightConfig = ConfigService.GetConfig(new DirectWeightConfig(), typeof(Station), Workstation.Station);
                IsWeight = (weightConfig != null) && weightConfig.IsWeight;
                if (IsWeight)
                    OpenWeightSerial();
            });
        }
        #endregion

        #region 控制设置 Printer
        /// <summary>
        /// 控制设置
        /// </summary>
        [Label("默认打印机")]
        public static readonly Property<string> PrinterProperty = P<DirectPackingViewModel>.Register(e => e.Printer, new PropertyMetadata<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as DirectPackingViewModel).OnPrinterChanged()
        });

        /// <summary>
        /// 控制设置
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
            SIE.Wpf.MES.Properties.Settings.Default.PackingVMPrinter = Printer;
            SIE.Wpf.MES.Properties.Settings.Default.Save();
            FocuseBarcode();
        }

        #region 是否自动打印 IsAutoPrintLabel
        /// <summary>
        /// 是否自动打印
        /// </summary>
        [Label("自动打印标签")]
        public static readonly Property<bool> IsAutoPrintLabelProperty = P<DirectPackingViewModel>.Register(e => e.IsAutoPrintLabel, new PropertyMetadata<bool>()
        {
            PropertyChangedCallBack = (o, e) => (o as DirectPackingViewModel).FocuseBarcode()
        });

        /// <summary>
        /// 是否自动打印
        /// </summary>
        public bool IsAutoPrintLabel
        {
            get { return this.GetProperty(IsAutoPrintLabelProperty); }
            set { this.SetProperty(IsAutoPrintLabelProperty, value); }
        }
        #endregion

        #endregion

        #region 打印方式 PrintMode
        /// <summary>
        /// 打印方式
        /// </summary>
        [Label("打印方式")]
        public static readonly Property<PrintMode> PrintModeProperty = P<DirectPackingViewModel>.Register(e => e.PrintMode);

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
        public static readonly Property<ScanMode> ScanModeProperty = P<DirectPackingViewModel>.Register(e => e.ScanMode, new PropertyMetadata<ScanMode>() { PropertyChangedCallBack = (o, e) => (o as DirectPackingViewModel).FocuseBarcode() });

        /// <summary>
        /// 扫描方式
        /// </summary>
        public ScanMode ScanMode
        {
            get { return this.GetProperty(ScanModeProperty); }
            set { this.SetProperty(ScanModeProperty, value); }
        }
        #endregion

        #region 自动打包模式 AutoDoPackingMode
        /// <summary>
        /// 自动打包模式
        /// </summary>
        [Label("自动打包模式")]
        public static readonly Property<DirectAutoDoPackingMode> AutoDoPackingModeProperty = P<DirectPackingViewModel>.Register(e => e.AutoDoPackingMode);

        /// <summary>
        /// 自动打包模式
        /// </summary>
        public DirectAutoDoPackingMode AutoDoPackingMode
        {
            get { return this.GetProperty(AutoDoPackingModeProperty); }
            set { this.SetProperty(AutoDoPackingModeProperty, value); }
        }
        #endregion

        #region 包装单位 PackingUnit
        /// <summary>
        /// 包装单位Id
        /// </summary>
        [Label("包装单位")]
        public static readonly IRefIdProperty PackingUnitIdProperty =
            P<DirectPackingViewModel>.RegisterRefId(e => e.PackingUnitId, ReferenceType.Normal);

        /// <summary>
        /// 包装单位Id
        /// </summary>
        public double? PackingUnitId
        {
            get { return (double?)this.GetRefNullableId(PackingUnitIdProperty); }
            set { this.SetRefNullableId(PackingUnitIdProperty, value); }
        }

        /// <summary>
        /// 包装单位
        /// </summary>
        public static readonly RefEntityProperty<PackingUnit> PackingUnitProperty =
            P<DirectPackingViewModel>.RegisterRef(e => e.PackingUnit, PackingUnitIdProperty);

        /// <summary>
        /// 包装单位
        /// </summary>
        public PackingUnit PackingUnit
        {
            get { return this.GetRefEntity(PackingUnitProperty); }
            set { this.SetRefEntity(PackingUnitProperty, value); }
        }
        #endregion

        #region 是否扫描包装号 IsPackage
        /// <summary>
        /// 是否包装号
        /// </summary>
        [Label("是否扫描包装号")]
        public static readonly Property<bool> IsPackageProperty = P<DirectPackingViewModel>.Register(e => e.NeedScanPackageNo);

        /// <summary>
        /// 是否扫描包装号
        /// </summary>
        public bool NeedScanPackageNo
        {
            get { return this.GetProperty(IsPackageProperty); }
            set { this.SetProperty(IsPackageProperty, value); }
        }
        #endregion 

        #region 条码明细
        /// <summary>
        /// 条码明细
        /// </summary>
        public static readonly ListProperty<EntityList<DirectPackageSnRecord>> PackageSnRecordListProperty = P<DirectPackingViewModel>.RegisterList(e => e.PackageSnRecordList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as DirectPackingViewModel).LoadPackageSnRecordList()
        });

        /// <summary>
        /// 条码明细
        /// </summary>
        public EntityList<DirectPackageSnRecord> PackageSnRecordList
        {
            get { return this.GetLazyList(PackageSnRecordListProperty); }
        }

        /// <summary>
        /// 条码明细
        /// </summary>
        /// <returns>条码明细</returns>
        private EntityList<DirectPackageSnRecord> LoadPackageSnRecordList()
        {
            return new EntityList<DirectPackageSnRecord>();
        }
        #endregion

        #region 包装层级 PackLevel
        /// <summary>
        /// 包装层级
        /// </summary>
        [Label("包装层级")]
        public static readonly Property<string> PackLevelProperty = P<DirectPackingViewModel>.Register(e => e.PackLevel);

        /// <summary>
        /// 包装层级
        /// </summary>
        public string PackLevel
        {
            get { return this.GetProperty(PackLevelProperty); }
            set { this.SetProperty(PackLevelProperty, value); }
        }
        #endregion

        #region 包装规则 PackageRuleDetailList
        /// <summary>
        /// 包装规则
        /// </summary>
        public static readonly ListProperty<EntityList<WorkOrderPackageRuleDetail>> PackageRuleDetailListProperty = P<DirectPackingViewModel>.RegisterList(e => e.PackageRuleDetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as DirectPackingViewModel).LoadPackageRuleDetailList()
        });

        /// <summary>
        /// 包装规则
        /// </summary>
        public EntityList<WorkOrderPackageRuleDetail> PackageRuleDetailList
        {
            get { return this.GetLazyList(PackageRuleDetailListProperty); }
        }

        private EntityList<WorkOrderPackageRuleDetail> LoadPackageRuleDetailList()
        {
            return new EntityList<WorkOrderPackageRuleDetail>();
        }
        #endregion

        #region 包装关系列表 PackingRelations
        /// <summary>
        /// 包装关系列表
        /// </summary>
        [Label("包装关系列表")]
        public static readonly ListProperty<EntityList<PackingRelation>> PackingRelationsProperty = P<DirectPackingViewModel>.RegisterList(e => e.PackingRelations, new ListPropertyMeta
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
            var serialPortsConfig = ConfigService.GetConfig(new DirectWeightSerialProtsConfig(), GetType(), ResourceStation.Find(Workstation.Station));
            foreach (var s in serialPortsConfig.DirectWeightSerialPortList)
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
            {
                if (serial.IsOpen)
                {
                    serial.Close();
                }
            }
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public DirectPackingViewModel()
        {
            PrintMode = PrintMode.Online;
            InitWorkstation(ProcessType.Packing);
            Printer = Settings.Default.PackingVMPrinter;

            IsAutoPrintLabel = true;

            //初始化重量信息
            WeightInfo = new WeightInfo();

            ToScanPackageUnits = new Queue<PackingUnit>();
        }

        /// <summary>
        /// 加载工作站数据
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();
            IsAutoPrintLabel = true;
            AutoDoPackingMode = DirectAutoDoPackingMode.AutoPacking;

            if (Workstation.Station != null)
            {
                //加载工位对应的配置项
                var _config = ConfigService.GetConfig(new DirectWipPackingConfig(), typeof(Station), Workstation.Station);
                if (_config != null)
                {
                    IsAutoPrintLabel = _config.IsAutoPrintPackageLabel;
                    AutoDoPackingMode = _config.AutoDoPackingMode;
                }
                //加载产线对应的配置项
                var cfg = ConfigService.GetConfig(new DirectPackingPrintModeConfig(), typeof(Station), Workstation.Station);

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
            ReloadDirectPackingSnRelation();
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
        public void ReloadDirectPackingSnRelation()
        {
            if (Workstation.Process == null || Workstation.Station == null)
            {
                return;
            }
            var records = FindPackageSnRecords();
            PackageSnRecordList.Clear();
            PackageSnRecordList.AddRange(records);
            PackageSnRecordList.MarkSaved();
        }

        /// <summary>
        /// 加载工位未满包装的包装关系
        /// </summary>
        /// <returns>包装关系列表</returns>
        private EntityList<DirectPackageSnRecord> FindPackageSnRecords()
        {
            return RT.Service.Resolve<DirectPackingController>()
                .GetPackageSnRecords(Workstation.ResourceId.Value, Workstation.ProcessId.Value, Workstation.StationId.Value);

        }

        /// <summary>
        /// 初始化包装号打印模式
        /// </summary>
        private void InitPrintModeConfig(Station station)
        {
            AsyncExecute(() =>
            {
                var config = ConfigService.GetConfig(new DirectPackingPrintModeConfig(), typeof(Station), station);
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
            
        }

        /// <summary>
        /// 工位变更
        /// </summary>
        /// <param name="station">工位</param>
        protected override void StationChanged(Station station)
        {
            base.StationChanged(station);
            InitPrintModeConfig(station);
            CloseWeightSerial();
            if (station != null)
            {
                InitWeightConfig();
            }
            ReloadDirectPackingSnRelation();
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
            else if (e.Property == PackingUnitIdProperty && e.OldValue != null && Convert.ToDouble(e.OldValue)!= 0)
            {
                CurrentPackingRelation = null;
                CurrentPackingUnit = null;
                CollectBarcode = null;
                ScannedPackageNos.Clear();
                ToScanPackageUnits.Clear();
                FocuseBarcode();
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
            {
                return;
            }
            using (PerformenceWatcher.Start(logger, "OnBarcodeChanged"))
            {
                try
                {
                    ClearInfos();
                    bool isPass = false;
                    isPass = NeedScanPackageNo ? ValidatePackageNo(Barcode, CurrentPackingRelation)
                        : ValidateBarcode(Barcode, CurrentPackingRelation);

                    if (isPass)
                    {
                        PackingCollect();
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
            var workOrder = RF.GetById<WorkOrderMove>(productInfo.WorkOrderId);
            //工单的包装规则列表
            List<WorkOrderPackageRuleDetail> woRuleDtls
                = RT.Service.Resolve<DirectPackingController>().GetWorkOrderPackageRuleDetails(workOrder.Id, new EagerLoadOptions().LoadWithViewProperty());
            
            if (currentPackingRelation != null
                && productInfo.WorkOrderId != currentPackingRelation.WorkOrderId)
            {
                ShowError("条码[{0}]的工单与当前采集的包装的工单不相同".L10nFormat(barcode));
                return false;
            }
            CollectBarcode = barcode;

            CurrentPackingUnit = productInfo.Context["PackingUnit"] as PackingUnit;
            OuterPackingUnit = productInfo.Context["OuterPackingUnit"] as PackingUnit;
            ToScanPackageUnits = productInfo.Context["ToScanPackageUnits"] as Queue<PackingUnit>;
            var upLevelPackageRuleDetail = RT.Service.Resolve<DirectPackingController>().GetUpLevelPackingRule(woRuleDtls, workOrder.No, CurrentPackingUnit);
            if (upLevelPackageRuleDetail != null && PackingUnit != null && upLevelPackageRuleDetail.PackageUnitId != PackingUnit.Id)
            {
                ShowError("条码[{0}]的上层包装单位层级与当前采集所选包装单位层级的不同".L10nFormat(barcode));
                return false;
            }
            //是否需要过站
            needMove = false;

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
        /// 验证指定包装号
        /// </summary>
        /// <param name="packageNo">包装号</param>
        /// <param name="currentPackingRelation"></param>
        protected virtual bool ValidatePackageNo(string packageNo, PackingRelation currentPackingRelation)
        {
            // 验证包装号
            // 增加验证包装号是否要采集的包装单位
            RT.Service.Resolve<PackingBarcodeController>()
                .ValidatePackingBarcode(packageNo, ToScanPackageUnits.Peek(), currentPackingRelation);

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
            collectData.PackingData.PackingMode = Packages.Packings.Enums.AutoDoPackingMode.AutoPacking;
            collectData.PackingData.PrintMode = PrintMode;
            collectData.PackingData.ScannedPackageNos = ScannedPackageNos;
            collectData.PackingData.CurrentPackingUnit = CurrentPackingUnit;

            //处理包装关系            
            CurrentPackingRelation = Controller.PackingCollect(CollectBarcode, collectData, workcell, needMove);
            // 打印
            if (CurrentPackingRelation.PackageNo.IsNotEmpty())
            {
                var printRelations = RT.Service.Resolve<PackingRelationController>().GetPackingRelations(CurrentPackingRelation.PackageNo.Split(',').ToList());
                var invOrg = RT.InvOrg.Value;
                Task.Run(new Action(() =>
                {
                    Print(printRelations, invOrg);
                }).WithCurrentThreadContext());
            }

            //刷新
            ReloadDirectPackingSnRelation();
            //提示词
            var packingEvent = Controller.CreatePackingEvent("MesPacking", ScanMode, CollectBarcode, CurrentPackingRelation, WorkOrder, collectData.PackingData.CurrentPackingUnit);
            ShowTips(CollectBarcode, packingEvent.OuterPackingRelation);
            // 不进行级联处理
            if (CurrentPackingRelation.IsFullPackage)
            {
                CurrentPackingRelation = null;
                ScannedPackageNos.Clear();
            }
            // 上一包装层级
            PackingUnit = OuterPackingUnit;
            CollectBarcode = null;
            CurrentPackingUnit = null;
        }

        private readonly object _locker = new object();

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="report"></param>
        /// <param name="filePath"></param>
        /// <param name="printer"></param>
        /// <param name="relation"></param>
        private void DoPrint(IReport report, string filePath, string printer, PackingRelation relation)
        {
            var printable = new PackingRelationPrintable();
            report.Print(printable, filePath, printer, () =>
            {
                return new PackingRelation[] { relation };
            }, () =>
            {
            });
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printRelations"></param>
        /// <param name="invOrg"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Print(EntityList<PackingRelation> printRelations, int invOrg)
        {
            if (!RT.InvOrg.HasValue)
            {
                RT.InvOrg = invOrg;
            }

            var wo = RF.GetById<WorkOrder>(printRelations.FirstOrDefault().WorkOrderId);
            if (wo == null)
            {
                throw new ValidationException("找不到此包装[{0}]对应的工单信息".L10nFormat(printRelations.FirstOrDefault().PackageNo));
            }

            foreach (PackingRelation pkg in printRelations.OrderBy(p => p.ItemQty))
            {
                //获取包装SN
                var rule = wo.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == pkg.PackageUnit.Id);
                if (rule.PrintTemplateId == null || rule.PrintTemplateId == 0)
                {
                    throw new ValidationException("找不到对应的包装规则【{0}】的【打印模板】".L10nFormat(rule.PackageUnit.Name));
                }
                var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(rule.PrintTemplateId.Value);
                var report = ReportFactory.Current.GetReportByExtension(rule.PrintTemplate.Type);
                pkg.Customer = wo.Customer?.Name;
                pkg.ItemCode = wo.Product?.Code;
                pkg.ItemName = wo.Product?.Name;
                lock (_locker)
                {
                    DoPrint(report, filePath, Printer, pkg);
                }
                RT.Service.Resolve<PackingRelationController>().UpdateRelationState(pkg.Id, LogisticState.Printed);

            }
        }
    }
}
