using SIE.Common.Configs;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Logging;
using SIE.ManagedProperty;
using SIE.MES.WIP;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Packings;
using SIE.MES.WIP.Packings.Configs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Packages.ItemLabels;
using SIE.Packages.Packages;
using SIE.Packages.Packings;
using SIE.Packages.Packings.Enums;
using SIE.Packages.Packings.Strategys;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Threading;
using SIE.Utils;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.Properties;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Packings
{
    /// <summary>
    /// 包装ViewModel
    /// </summary>
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [EntityWithConfig(typeof(WeightSerialProtsConfig))]
    [EntityWithConfig(typeof(WipPackingConfig))]
    [EntityWithConfig(typeof(WipPackingBillConfig))]
    [RootEntity, Serializable]
    [Label("包装采集")]
    public partial class PackingViewModel : DataCollectionViewModel<WipPackingController>
    {
        ILog logger = Logging.LogManager.GetLogger("wip");
        /// <summary>
        /// 构造方法
        /// </summary>
        public PackingViewModel()
        {
            InitWorkstation(ProcessType.Packing);
            Printer = Settings.Default.PackingVMPrinter;
            _helper = new PackingHelper(this);

            //初始化重量信息
            WeightInfo = new WeightInfo();
        }

        /// <summary>
        /// 包装帮助类
        /// </summary>
        PackingHelper _helper;

        /// <summary>
        /// 包装帮助类
        /// </summary>
        public PackingHelper Helper { get { return _helper; } }

        /// <summary>
        /// 采集配置项
        /// </summary>
        WipPackingConfigValue _config;

        /// <summary>
        /// 内部编码
        /// </summary>
        private string InsideBarcode { get; set; }

        /// <summary>
        /// 输出包装关系
        /// </summary>
        public PackingRelation OuterPackingRelation { get; set; }

        /// <summary>
        /// 是否物料标签
        /// </summary>
        private bool IsSn
        {
            get
            {
                //多包装情况下，后包装工序采集上一次包装关系，不采集条码，此时为false
                return Step.Barcodes.Any();
            }
        }

        #region 重量 Weight
        /// <summary>
        /// 重量
        /// </summary>
        [Label("重量")]
        public static readonly Property<WeightInfo> WeighProperty = P<PackingViewModel>.Register(e => e.WeightInfo);

        /// <summary>
        /// 重量
        /// </summary>
        public WeightInfo WeightInfo
        {
            get { return this.GetProperty(WeighProperty); }
            set { this.SetProperty(WeighProperty, value); }
        }

        /// <summary>
        /// 重量上限
        /// </summary>
        public static readonly Property<decimal?> UpperLimitProperty = P<PackingViewModel>.RegisterReadOnly(
                e => e.UpperLimit, e => e.WorkOrder.Product.UpperWeight, WorkOrderProperty);

        /// <summary>
        /// 重量上限
        /// </summary>
        public decimal? UpperLimit
        {
            get { return this.GetProperty(UpperLimitProperty); }
        }

        /// <summary>
        /// 重量下限
        /// </summary>
        public static readonly Property<decimal?> LowerLimitProperty = P<PackingViewModel>.RegisterReadOnly(
            e => e.LowerLimit, e => e.WorkOrder.Product.LowerWeight, PackingViewModel.WorkOrderProperty);

        /// <summary>
        /// 重量下限
        /// </summary>
        public decimal? LowerLimit
        {
            get { return this.GetProperty(LowerLimitProperty); }
        }
        #endregion

        #region 包装关系 PackingRelationList 
        /// <summary>
        /// 包装关系列表
        /// </summary>
        [Label("包装关系")]
        public static readonly ListProperty<EntityList<PackingRelation>> PackingRelationListProperty = P<PackingViewModel>.RegisterList(e => e.PackingRelationList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<PackingRelation>()
        });

        /// <summary>
        /// 包装关系列表
        /// </summary>
        public EntityList<PackingRelation> PackingRelationList
        {
            get { return this.GetLazyList(PackingRelationListProperty); }
        }
        #endregion

        #region 产品条码 ItemLabelList 
        /// <summary>
        /// 产品条码列表
        /// </summary>
        [Label("产品条码")]
        public static readonly ListProperty<EntityList<SIE.Packages.ItemLabels.ItemLabel>> ItemLabelListProperty = P<PackingViewModel>.RegisterList(e => e.ItemLabelList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<SIE.Packages.ItemLabels.ItemLabel>()
        });

        /// <summary>
        /// 产品条码列表
        /// </summary>
        public EntityList<SIE.Packages.ItemLabels.ItemLabel> ItemLabelList
        {
            get { return this.GetLazyList(ItemLabelListProperty); }
        }
        #endregion

        #region 控制设置
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("默认打印机")]
        public static readonly Property<string> PrinterProperty = P<PackingViewModel>.Register(e => e.Printer, new PropertyMetadata<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as PackingViewModel).OnPrinterChanged(o, e)
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
        /// <param name="o">托管属性对象</param>
        /// <param name="e">托管属性变更事件参数</param>
        void OnPrinterChanged(ManagedPropertyObject o, ManagedPropertyChangedEventArgs e)
        {
            Settings.Default.PackingVMPrinter = Printer;
            Settings.Default.Save();
            FocuseBarcode();
        }

        /// <summary>
        /// 是否自动打印
        /// </summary>
        [Label("自动打印标签")]
        public static readonly Property<bool> IsAutoPrintLabelProperty = P<PackingViewModel>.Register(e => e.IsAutoPrintLabel, new PropertyMetadata<bool>() { PropertyChangedCallBack = (o, e) => (o as PackingViewModel).FocuseBarcode() });

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
        public static readonly Property<AutoDoPackingMode> AutoPackingModeProperty = P<PackingViewModel>.Register(e => e.AutoDoPackingMode, new PropertyMetadata<AutoDoPackingMode>() { PropertyChangedCallBack = (o, e) => (o as PackingViewModel).OnPackingModelChange() });

        /// <summary>
        /// 自动打包属性变更回调方法
        /// </summary>
        private void OnPackingModelChange()
        {
            FocuseBarcode();
            ResetOuterRelation();
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

        #region 扫描方式 ScanMode 
        /// <summary>
        /// 扫描方式
        /// </summary>
        [Label("扫描方式")]
        public static readonly Property<ScanMode> ScanModeProperty = P<PackingViewModel>.Register(e => e.ScanMode, new PropertyMetadata<ScanMode>() { PropertyChangedCallBack = (o, e) => (o as PackingViewModel).FocuseBarcode() });

        /// <summary>
        /// 扫描方式
        /// </summary>
        public ScanMode ScanMode
        {
            get { return this.GetProperty(ScanModeProperty); }
            set { this.SetProperty(ScanModeProperty, value); }
        }
        #endregion

        #region 设备端口 
        /// <summary>
        /// 初始化设备端口信息
        /// </summary>
        private void InitDevicePort()
        {
            CloseWeightSerial();
            if (Workstation.Station != null)
            {
                OpenSerial();
            }
        }

        /// <summary>
        /// 通信串口
        /// </summary>
        Dictionary<object, string> serialsRegex = new Dictionary<object, string>();

        /// <summary>
        /// 关闭通信串口
        /// </summary>
        void CloseWeightSerial()
        {
            foreach (System.IO.Ports.SerialPort serial in serialsRegex.Keys)
                if (serial.IsOpen)
                    serial.Close();
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        void OpenSerial()
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
                Regex regex = new Regex(serialsRegex[sender], RegexOptions.IgnoreCase);
                var result = regex.Match(source);
                if (result != null)
                    ReadWeight(Convert.ToDecimal(result.Value));
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
        #endregion

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
                    var workcell = GetWorkcell();
                    if (IsNeedToStep())
                    {
                        var currentStep = Step.CurrentStep;
                        var collectBarcode = new CollectBarcode { Code = Barcode, Type = currentStep.BarcodeType };
                        if (OuterPackingRelation != null || ScanMode != ScanMode.Join)
                            Validate(collectBarcode, workcell);
                        Step.Barcodes.Add(collectBarcode.Code);
                        var collectData = new CollectData() { CollectBarcode = collectBarcode };
                        if (!Step.NextStep())
                            Collect(workcell, collectData);
                        else
                            ShowTips("[条码:{0}]扫描成功，请扫描[{1}]".L10nFormat(collectBarcode, Step.ProcessSteps.FirstOrDefault(p => p.Step == (Step.StepIndex + 1)).BarcodeType.ToLabel()));//currentStep.BarcodeType.ToLabel()));
                    }
                    else
                    {
                        ValidateWipWorkOrder(Barcode);
                        Collect(workcell, new CollectData());
                    }
                }
                catch (Exception exc)
                {
                    Step.Reset();
                    ShowError(exc.GetBaseException().Message);
                }
                finally
                {
                    ResetPacking();
                }
            }
        }

        /// <summary>
        /// 切换工单
        /// </summary>
        /// <param name="packingNo">包装关系</param>
        private void ValidateWipWorkOrder(string packingNo)
        {
            double? workOrderId = RT.Service.Resolve<WipPackingController>().GetRelationWorkOrderId(packingNo);
            if (workOrderId.HasValue && workOrderId.Value != WorkOrderId)
            {
                var workcell = GetWorkcell();
                var wo = RF.GetById<WorkOrder>(workOrderId.Value);
                if (WorkOrder != null)
                    ShowError("工单已切换,由[{0}]切换到[{1}]".L10nFormat(WorkOrder.No, wo.No));
                WorkOrder = wo;
                Controller.ChangeWipResourceWorkOrder(wo.Id, workcell);
                RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = workOrderId.Value });
            }
        }

        /// <summary>
        /// 采集
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="collectData">采集数据</param>
        private void Collect(SIE.MES.WIP.Workcell workcell, CollectData collectData)
        {
            collectData.PackingData.OuterPackingRelationId = OuterPackingRelation?.Id;
            if (ScanMode == ScanMode.Normal)
            {
                NormalPacking(Barcode, collectData, workcell);
            }
            else if (ScanMode == ScanMode.Join)
            {
                JoinPacking(Barcode, collectData, workcell);
            }
            else
            {
                //
            }
            RefreshStatistics();
            RefrshReportTasks();
            Step.Reset();
        }

        #region 正常包装
        /// <summary>
        /// 正常包装
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        private void NormalPacking(string barcode, CollectData collectData, SIE.MES.WIP.Workcell workcell)
        {
            try
            {
                InsideBarcode = barcode;
                ContinuityControl(barcode);
                if (!collectData.Context.Contains(typeof(PackingRuleValidMode).Name))
                {
                    collectData.Context.Add(typeof(PackingRuleValidMode).Name, _config.PackingRuleValidMode);
                }
                else
                {
                    collectData.Context[typeof(PackingRuleValidMode).Name] = _config.PackingRuleValidMode;
                }
                if (IsSn)
                {
                    AddSn(barcode, collectData, workcell);
                }
                else
                {
                    AddPackage(barcode, collectData, workcell);
                }
                OnSucess(collectData.PackingData.DesignatedOuterPackingUnit);
            }
            catch (Exception ex)
            {
                OnFaile(barcode, ex);
                throw;
            }
        }

        /// <summary>
        /// 采集成功
        /// </summary>
        /// <param name="unit">包装单位</param>
        private void OnSucess(PackingUnit unit)
        {
            var packingEvent = Controller.CreatePackingEvent("MesPacking", ScanMode, InsideBarcode, OuterPackingRelation, WorkOrder, unit);
            RT.EventBus.Publish(packingEvent);
        }

        /// <summary>
        /// 采集失败
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="ex">异常信息</param>
        private void OnFaile(string barcode, Exception ex)
        {
            var packingEvent = CreateFaileEvent(barcode, ex.GetBaseException().Message);
            ShowError(packingEvent.Error);
            Step.Reset();
        }

        /// <summary>
        /// 采集失败消息
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="message">错误信息</param>
        /// <returns>失败消息</returns>
        PackingStrategyEvent CreateFaileEvent(string barcode, string message)
        {
            var packingEvent = new PackingStrategyEvent();
            packingEvent.StrategyType = ScanMode == ScanMode.Normal ? ScanStrategyMode.ScanSingle : ScanStrategyMode.ScanOneJoinToMany;
            packingEvent.InsiderBarcode = this.InsideBarcode.IsNotEmpty() ? new string[] { this.InsideBarcode } : null;
            packingEvent.OuterPackingRelation = OuterPackingRelation;
            packingEvent.Error = message;
            return packingEvent;
        }

        /// <summary>
        /// 添加包装
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="collectData">采集条码</param>
        /// <param name="workcell">工作单元</param>
        private void AddPackage(string barcode, CollectData collectData, SIE.MES.WIP.Workcell workcell)
        {
            OuterPackingRelation = OuterPackingRelation == null ?
                Controller.CollectPackageNo(barcode, collectData, workcell)
                : Controller.JoinInPackage(barcode, collectData, workcell);
        }

        /// <summary>
        /// 添加单体
        /// </summary>
        /// <param name="barcode">采集条码</param>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        private void AddSn(string barcode, CollectData collectData, SIE.MES.WIP.Workcell workcell)
        {
            collectData.PackingData.OuterPackingRelationId = OuterPackingRelation?.Id;
            Controller.Collect(Step.Barcodes.ToArray(), collectData, workcell);
            var label = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
            OuterPackingRelation = label.Relation;
        }

        /// <summary>
        /// 连续控制
        /// </summary>
        /// <param name="barcode">条码</param>
        void ContinuityControl(string barcode)
        {
            if (OuterPackingRelation == null || !_config.IsContinuityControl)
            {
                return;
            }
            var existPackage = RT.Service.Resolve<PackingController>().FindContactPackingRelationByBarcode(barcode, false);

            if ((existPackage != null && existPackage.PackageUnitId != ChildPackageRuleLevel.PackageUnitId)
                || (IsSn && !ChildPackageRuleLevel.PackageUnit.IsMasterUnit))
            {
                OuterPackingRelation = null;
            }
        }
        #endregion

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
                if (IsChildNode(barcode))
                {
                    ReadPackageCode(barcode, collectData.PackingData.DesignatedOuterPackingUnit);
                }
                else
                {
                    ReadBulkCode(barcode, collectData, workcell);
                }
            }
            catch (Exception ex)
            {
                OnFaile(barcode, ex);
                throw;
            }
        }

        /// <summary>
        /// 外包装条码
        /// </summary>  
        /// <param name="barcode">条码</param>
        /// <param name="unit">包装单位</param>
        void ReadPackageCode(string barcode, PackingUnit unit)
        {
            SetParentNode(barcode);
            OnSucess(unit);
        }

        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="barcode">编码</param>
        protected virtual void SetParentNode(string barcode)
        {
            var innerPackageRelation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcode, false);
            PackingRelation _outerPackingRelation = null;
            if (innerPackageRelation != null)
            {
                _outerPackingRelation = innerPackageRelation;
            }
            else
            {
                var itemLabel = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
                if (itemLabel?.Relation != null)
                {
                    _outerPackingRelation = itemLabel.Relation;
                }
            }

            if (_outerPackingRelation == null)
            {
                throw new ValidationException("系统无此条码[{0}]记录".L10nFormat(barcode));
            }

            if (_outerPackingRelation.RootId > 0)
            {
                OuterPackingRelation = RF.GetById<PackingRelation>(_outerPackingRelation.RootId);
            }
            else
            {
                while (_outerPackingRelation.GetTreePId() != null)
                {
                    if (_outerPackingRelation.GetTreePId() != null)
                        _outerPackingRelation = RF.GetById<PackingRelation>(_outerPackingRelation.TreePId);
                }

                OuterPackingRelation = _outerPackingRelation;
            }
        }

        /// <summary>
        /// 读取扩展编码
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="collectData">采集结果</param>
        /// <param name="workcell">工作单元</param>
        void ReadBulkCode(string barcode, CollectData collectData, SIE.MES.WIP.Workcell workcell)
        {
            SetBrotherNode(barcode);
            InsideBarcode = barcode;
            if (!collectData.Context.Contains(typeof(PackingRuleValidMode).Name))
            {
                collectData.Context.Add(typeof(PackingRuleValidMode).Name, _config.PackingRuleValidMode);
            }
            else
            {
                collectData.Context[typeof(PackingRuleValidMode).Name] = _config.PackingRuleValidMode;
            }
            if (!IsSn)
            {
                OuterPackingRelation = Controller.JoinInPackage(barcode, collectData, workcell);
            }
            else
            {
                Controller.Collect(Step.Barcodes.ToArray(), collectData, workcell);
                var label = RT.Service.Resolve<ItemLabelController>().GetItemLabel(barcode);
                OuterPackingRelation = label.Relation;
            }

            OnSucess(collectData.PackingData.DesignatedOuterPackingUnit);
        }

        /// <summary>
        /// 设置兄弟节点
        /// </summary>
        /// <param name="barcode">条码</param>
        private void SetBrotherNode(string barcode)
        {
            if (IsBrotherNode(barcode) && OuterPackingRelation.GetTreePId() != null)
            {
                OuterPackingRelation = RF.GetById<PackingRelation>(OuterPackingRelation.TreePId);
            }
        }

        /// <summary>
        /// 是否兄弟结点
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>bool</returns>
        private bool IsBrotherNode(string barcode)
        {
            var innerPackageRelation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcode, false);
            return OuterPackingRelation.PackageUnitId == innerPackageRelation?.PackageUnitId;
        }

        /// <summary>
        /// 是否子节点
        /// </summary>
        /// <param name="barcode">编码</param>
        /// <returns>bool</returns>
        bool IsChildNode(string barcode)
        {
            if (OuterPackingRelation == null)
            {
                return true;
            }

            if (!_config.IsContinuityControl)
            {
                return false;
            }

            var innerPackageRelation = RT.Service.Resolve<PackingRelationController>().GetBatchPackingRelation(barcode, false);
            if (innerPackageRelation != null && ChildPackageRuleLevel.PackageUnitId != innerPackageRelation.PackageUnitId)
            {
                return true;
            }

            return false;
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
                return WorkOrder.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == OuterPackingRelation?.PackageUnitId);
            }
        }

        /// <summary>
        /// 子包装规则层级
        /// </summary>
        protected virtual WorkOrderPackageRuleDetail ChildPackageRuleLevel
        {
            get
            {
                if (WorkOrder == null)
                {
                    return null;
                }
                return WorkOrder.PackageRuleDetailList.LastOrDefault(f => SortExtension.GetIndex(f) < SortExtension.GetIndex(CurrentPackageRuleLevel));
            }
        }

        /// <summary>
        /// 包装关系
        /// </summary>
        public EntityList<PackingRelation> PackingRelations { get; internal set; }

        /// <summary>
        /// 是否需要采集步骤
        /// 1、正常模式下
        /// </summary>
        /// <returns>返回true需要，false不需要</returns>
        bool IsNeedToStep()
        {
            return !RT.Service.Resolve<PackingRelationController>().IsPackingRelation(Barcode);
        }

        /// <summary>
        /// 工作站变更
        /// </summary>
        protected override void StationChanged(Station station)
        {
            base.StationChanged(station);

            InitDevicePort();
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e">托管属性变更事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == ScanModeProperty)
            {
                ShowError("已切换扫描模式:{0}".L10nFormat(EnumViewModel.EnumToLabel(ScanMode).L10N()));
                if (ScanMode != ScanMode.Normal)
                {
                    OuterPackingRelation = null;
                }
                ShowTips(ScanMode == ScanMode.Normal ? GetNormalTips() : GetJoinTips());
            }
            else if (e.Property == WorkOrderProperty && WorkOrder != null)
            {
                PackageRuleDetailList.Clear();
                if (WorkOrder.PackageRuleDetailList.Count > 0)
                {
                    PackageRuleDetailList.AddRange(WorkOrder.PackageRuleDetailList);
                    PackageRuleDetailList.MarkSaved();
                }
                FocuseBarcode();
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public static readonly ListProperty<EntityList<WorkOrderPackageRuleDetail>> PackageRuleDetailListProperty = P<PackingViewModel>.RegisterList(e => e.PackageRuleDetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as PackingViewModel).LoadPackageRuleDetailList()
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

        /// <summary>
        /// 显示提示信息
        /// </summary>
        public void ShowTips()
        {
            ShowTips(ScanMode == ScanMode.Normal ? GetNormalTips() : GetJoinTips());
        }

        /// <summary>
        /// 显示正常包装提示信息
        /// </summary>
        /// <returns>提示信息</returns>
        string GetNormalTips()
        {
            if (CurrentPackageRuleLevel != null && ChildPackageRuleLevel.PackageUnit.IsMasterUnit && this.IsSn)
            {
                return "[条码:{0}]采集成功,请继续扫描产品条码".L10nFormat(Step.Barcodes.LastOrDefault());
            }
            else if (CurrentPackageRuleLevel != null && !ChildPackageRuleLevel.PackageUnit.IsMasterUnit)
            {
                return "[条码:{0}]采集成功,请继续扫描[{1}]条码".L10nFormat(InsideBarcode, ChildPackageRuleLevel.PackageUnit.Name);
            }
            else
            {
                //
            }

            return "请扫描条码".L10N();
        }

        /// <summary>
        /// 显示加入包装提示信息
        /// </summary>
        /// <returns>提示信息</returns>
        string GetJoinTips()
        {
            if (OuterPackingRelation == null)
            {
                return "请先扫描同一包装里的条码,或者双击目标包装,进行加入扫码。".L10N();
            }
            else if (CurrentPackageRuleLevel != null && !ChildPackageRuleLevel.PackageUnit.IsMasterUnit && OuterPackingRelation.PackageNo.IsNullOrWhiteSpace())
            {
                return "包装[{0}]识别成功,请扫描要加入[{0}]包装条码。".L10nFormat(ChildPackageRuleLevel.PackageUnit.Name);
            }
            else if (CurrentPackageRuleLevel != null && ChildPackageRuleLevel.PackageUnit.IsMasterUnit && OuterPackingRelation.PackageNo.IsNullOrWhiteSpace())
            {
                return "包装[{0}]识别成功,请扫描要加入的产品条码。".L10nFormat(OuterPackingRelation.PackageUnit.Name);
            }
            else if (OuterPackingRelation.PackageNo.IsNotEmpty())
            {
                return "包装[{0}{1}]已加入至包装清单,仅可查看。".L10nFormat(OuterPackingRelation.PackageUnit.Name, OuterPackingRelation.PackageNo);
            }
            else
            {
                return "请重置".L10N();
            }
        }

        /// <summary>
        /// 加载工作站数据
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();
            if (Workstation.Station != null)
            {
                _config = ConfigService.GetConfig(new WipPackingConfig(), this.GetType(), ResourceStation.Find(Workstation.Station));
                IsAutoPrintLabel = _config.IsAutoPrintPackageLabel;
                AutoDoPackingMode = _config.AutoDoPackingMode;
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            ScanMode = ScanMode.Normal;
            ResetOuterRelation();
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            ReloadPackingRelation();
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            base.Reset(resetType);
        }

        /// <summary>
        /// 重置包装
        /// </summary>
        public void ResetPacking()
        {
            InsideBarcode = string.Empty;
            Barcode = string.Empty;
        }

        /// <summary>
        /// 重置外包装
        /// </summary>
        public void ResetOuterRelation()
        {
            OuterPackingRelation = null;
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
            PackingRelationList.Clear();
            ItemLabelList.Clear();
            PackingRelationList.AddRange(unpackage);
            PackingRelationList.MarkSaved();
        }

        async Task<EntityList<PackingRelation>> FindWorkPackingRelationByStation()
        {
            return await Task.Run(new Func<EntityList<PackingRelation>>(() =>
            RT.Service.Resolve<PackingRelationController>()
            .FindWorkPackingRelationByStation(Workstation.ProcessId, Workstation.StationId))
                .WithCurrentThreadContext()).ConfigureAwait(true);
        }

        /// <summary>
        /// 重置采集步骤
        /// </summary>
        public void ResetStep()
        {
            Step.Reset();
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void Onload()
        {
            base.Onload();
        }

        /// <summary>
        /// 关闭时
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
            CloseWeightSerial();
        }
    }
}