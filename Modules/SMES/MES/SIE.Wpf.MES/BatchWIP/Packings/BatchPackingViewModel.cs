using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.BatchPackings;
using SIE.MES.WIP;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Packings;
using SIE.MES.WIP.Packings.Configs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Packages;
using SIE.Packages.Packings.Enums;
using SIE.Packages.Printables;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Utils;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.MES.Editors;
using SIE.Wpf.MES.Properties;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text.RegularExpressions;

namespace SIE.Wpf.MES.BatchWIP.Packings
{
    /// <summary>
    /// 批次包装ViewModel
    /// </summary>
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [EntityWithConfig(typeof(WeightSerialProtsConfig))]
    [EntityWithConfig(typeof(WipPackingConfig))]
    [EntityWithConfig(typeof(WipPackingBillConfig))]
    [RootEntity, Serializable]
    [Label("批次包装采集")]
    public class BatchPackingViewModel : BatchDataCollectionViewModel<BatchWipPackingController>
    {
        /// <summary>
        /// 
        /// </summary>
        public BatchPackingViewModel()
        {
            InitWorkstation(ProcessType.BatchPacking);
            Printer = Settings.Default.PackingVMPrinter;

            //初始化重量信息
            WeightInfo = new WeightInfo();
        }

        /// <summary>
        /// 配置项值
        /// </summary>
        WipPackingConfigValue _configValue;

        /// <summary>
        /// 输出包装关系
        /// </summary>
        public BatchPackingRelation OuterBatchPackingRelation { get; set; }

        #region 重量 Weight
        /// <summary>
        /// 重量
        /// </summary>
        [Label("重量")]
        public static readonly Property<WeightInfo> WeightInfoProperty = P<BatchPackingViewModel>.Register(e => e.WeightInfo);

        /// <summary>
        /// 重量
        /// </summary>
        public WeightInfo WeightInfo
        {
            get { return this.GetProperty(WeightInfoProperty); }
            set { this.SetProperty(WeightInfoProperty, value); }
        }

        /// <summary>
        /// 重量上限
        /// </summary>
        public static readonly Property<decimal?> UpperLimitProperty = P<BatchPackingViewModel>.RegisterReadOnly(
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
        public static readonly Property<decimal?> LowerLimitProperty = P<BatchPackingViewModel>.RegisterReadOnly(
            e => e.LowerLimit, e => e.WorkOrder.Product.LowerWeight, WorkOrderProperty);

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
        public static readonly ListProperty<EntityList<BatchPackingRelation>> PackingRelationListProperty = P<BatchPackingViewModel>.RegisterList(e => e.PackingRelationList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<BatchPackingRelation>()
        });

        /// <summary>
        /// 包装关系列表
        /// </summary>
        public EntityList<BatchPackingRelation> PackingRelationList
        {
            get { return this.GetLazyList(PackingRelationListProperty); }
        }
        #endregion

        #region 控制设置
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("默认打印机")]
        public static readonly Property<string> PrinterProperty = P<BatchPackingViewModel>.Register(e => e.Printer, new PropertyMetadata<string>()
        {
            PropertyChangedCallBack = (o, e) => (o as BatchPackingViewModel).OnPrinterChanged(o, e)
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
        public static readonly Property<bool> IsAutoPrintLabelProperty = P<BatchPackingViewModel>.Register(e => e.IsAutoPrintLabel, new PropertyMetadata<bool>() { PropertyChangedCallBack = (o, e) => (o as BatchPackingViewModel).FocuseBarcode() });

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
        public static readonly Property<AutoDoPackingMode> AutoPackingModeProperty
            = P<BatchPackingViewModel>.Register(e => e.AutoDoPackingMode, new PropertyMetadata<AutoDoPackingMode>()
            {
                PropertyChangedCallBack = (o, e) => (o as BatchPackingViewModel).OnPackingModelChange(e)
            });

        private void OnPackingModelChange(ManagedPropertyChangedEventArgs e)
        {
            if (e.OldValue == null)
            {
                return;
            }

            FocuseBarcode();
            ResetOuterRelation();
            ShowTips();
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
        public static readonly Property<ScanMode> ScanModeProperty = P<BatchPackingViewModel>.Register(e => e.ScanMode, new PropertyMetadata<ScanMode>() { PropertyChangedCallBack = (o, e) => (o as BatchPackingViewModel).FocuseBarcode() });

        /// <summary>
        /// 扫描方式
        /// </summary>
        public ScanMode ScanMode
        {
            get { return this.GetProperty(ScanModeProperty); }
            set { this.SetProperty(ScanModeProperty, value); }
        }
        #endregion

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
                InitDevicePort();
            }
        }

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
                //var source = (sender as System.IO.Ports.SerialPort).ReadLine().TrimEnd('\n', '\r', '\0').TrimStart('\0');
                //Regex regex = new Regex(serialsRegex[sender], RegexOptions.IgnoreCase);
                //var result = regex.Match(source);
                //if (result != null)
                //    ReadWeight(Convert.ToDecimal(result.Value));

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
        #endregion

        #region 重置
        /// <summary>
        /// 重置
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);
            ResetOuterRelation();
            ReloadPackingRelation();
        }

        /// <summary>
        /// 重置加入包装选择
        /// </summary>
        public void ResetOuterRelation()
        {
            OuterBatchPackingRelation = null;
            Barcode = string.Empty;
            //ResetPacking();
        }

        ///// <summary>
        ///// 重置包装动作
        ///// </summary>
        //public void ResetPacking()
        //{
        //InsideBarcode = string.Empty;
        //Barcode = string.Empty;
        //}

        /// <summary>
        /// 重新加载工位工序未完成的包装关系
        /// </summary>
        public void ReloadPackingRelation()
        {
            if (Workstation.ProcessId == null || Workstation.StationId == null) return;
            var unpackage = Controller.FindWorkPackingRelationByStation(Workstation.ProcessId.Value, Workstation.StationId.Value, WorkOrderId);
            PackingRelationList.Clear();
            PackingRelationList.AddRange(unpackage);
            PackingRelationList.MarkSaved();

            if (OuterBatchPackingRelation != null)
                OuterBatchPackingRelation = PackingRelationList.FirstOrDefault(p => p.Id == OuterBatchPackingRelation.Id);
        }

        /// <summary>
        /// 刷新进站批次
        /// </summary>
        internal override void RefreshInputBatch(OutputBatch outputBatch = null)
        {
            try
            {
                if (WorkOrder == null) return;
                InputBatchList.Clear();
                var workcell = GetWorkcell();
                var inputBatchs = RT.Service.Resolve<BatchManageController>().GetInputBatchs(workcell.ResourceId, workcell.ProcessId, workcell.StationId, WorkOrder.Id);
                inputBatchs.ForEach(p => p.SplitQty = p.RemainQty);
                InputBatchList.MarkSaved();
                InputBatchList.AddRange(inputBatchs);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }
        #endregion

        /// <summary>
        /// 工单包装主单位规则明细
        /// </summary>
        WorkOrderPackageRuleDetail _masterPackageRuleLevel;

        /// <summary>
        /// 工单包装主单位规则明细
        /// </summary>
        public virtual WorkOrderPackageRuleDetail MasterPackageRuleLevel
        {
            get
            {
                if (WorkOrder == null) return null;
                if (_masterPackageRuleLevel == null)
                    _masterPackageRuleLevel = WorkOrder.PackageRuleDetailList.OrderBy(p => p.GetIndex()).FirstOrDefault();
                return _masterPackageRuleLevel;
            }
        }

        /// <summary>
        /// 当前选择加入包装层级
        /// </summary>
        public virtual WorkOrderPackageRuleDetail CurrentPackageRuleLevel
        {
            get
            {
                if (WorkOrder == null) return null;
                return WorkOrder.PackageRuleDetailList.FirstOrDefault(f => f.PackageUnitId == OuterBatchPackingRelation?.PackageUnitId);
            }
        }

        /// <summary>
        /// 当前选择加入包装子包装规则层级
        /// </summary>
        public virtual WorkOrderPackageRuleDetail ChildPackageRuleLevel
        {
            get
            {
                if (WorkOrder == null) return null;
                return WorkOrder.PackageRuleDetailList.LastOrDefault(f => SortExtension.GetIndex(f) < SortExtension.GetIndex(CurrentPackageRuleLevel));
            }
        }

        /// <summary>
        /// 属性变更
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == ScanModeProperty)
            {
                ShowError("已切换扫描模式:{0}".L10nFormat(EnumViewModel.EnumToLabel(ScanMode).L10N()));
                ResetOuterRelation();
                ShowTips();
            }
            else if (e.Property == WorkOrderProperty && WorkOrder != null)
            {
                PackageRuleDetailList.Clear();
                _masterPackageRuleLevel = null;
                if (WorkOrder.PackageRuleDetailList.Count > 0)
                {
                    PackageRuleDetailList.AddRange(WorkOrder.PackageRuleDetailList);
                    PackageRuleDetailList.MarkSaved();
                }

                RefreshInputBatch();
                ResetOuterRelation();
                ReloadPackingRelation();
                ////FocuseBarcode();
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public static readonly ListProperty<EntityList<WorkOrderPackageRuleDetail>> PackageRuleDetailListProperty = P<BatchPackingViewModel>.RegisterList(e => e.PackageRuleDetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as BatchPackingViewModel).LoadPackageRuleDetailList()
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
        /// 条码变更事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnBarcodeChanged(e);
            if (Barcode.IsNullOrEmpty()) return;


            try
            {
                ClearInfos();

                if (IsAutoPrintLabel && Printer.IsNullOrEmpty())
                {
                    throw new ValidationException("自动打印标签，请先选择打印机！".L10N());
                }

                var workcell = GetWorkcell();
                if (IsReceiveContainer)
                    RemoveInputBatch(base.InputBatch, Barcode);
                else if (ScanMode == ScanMode.Normal)
                {
                    //正常
                    NormalPacking(workcell);
                }
                else
                {
                    //先设置待加入包装
                    if (OuterBatchPackingRelation == null)
                    {
                        OuterBatchPackingRelation = Controller.GetBatchPkgRelationByNo(Barcode);
                        ShowTips();

                        //加入自动打包
                        if (OuterBatchPackingRelation != null)
                        {
                            JoinAutoPacking(OuterBatchPackingRelation, workcell);
                        }

                        return;
                    }

                    //加入手动扫码
                    //if (AutoDoPackingMode == AutoDoPackingMode.Scan)
                    //{
                    //    JoinScanPacking(workcell);
                    //}
                }
            }
            catch (Exception exc)
            {
                ShowError(exc);
                if (IsReceiveContainer)
                    ShowTips("请扫描移除关联载具".L10N());
            }
            finally
            {
                Barcode = null;
            }
        }

        #region 功能操作
        /// <summary>
        /// 正常进站扫描
        /// </summary>
        /// <param name="workcell">工作单元</param>
        private new void InputBatch(Workcell workcell)
        {
            var collectStep = Step.InputCollectStep;
            var collectBarcode = new CollectBarcode { Code = Barcode, Type = collectStep.BarcodeType };

            MoveIn(collectBarcode, workcell);
            RefreshInputBatch();
            ShowTips("[{0}:{1}]成功转入，请扫描{2}".L10nFormat(collectBarcode.Type.ToLabel().L10N(), Barcode, collectBarcode.Type.ToLabel().L10N()));
        }

        /// <summary>
        /// 正常打包
        /// </summary>
        internal void NormalPacking(Workcell workcell)
        {
            InputBatch(workcell);
            if (AutoDoPackingMode == AutoDoPackingMode.AutoPacking)
            {
                Controller.AutoBatchPacking(WorkOrderId.Value, workcell);
                RefreshStatistics();
                RefreshInputBatch();
                ReloadPackingRelation();
                ShowTips("自动打包成功。".L10nFormat());
            }
            else if (AutoDoPackingMode == AutoDoPackingMode.AutoCasePacking)
            {
                Controller.AutoBatchPacking(WorkOrderId.Value, workcell);
                RefreshStatistics();
                Controller.DoPkgPacking(WorkOrder, workcell);
                RefreshInputBatch();
                ReloadPackingRelation();
                ShowTips("自动级联打包成功。".L10nFormat());
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 加入（自动打包、级联打包）
        /// </summary>
        /// <param name="rela">目标包装</param>
        /// <param name="workcell">工作单元</param>
        internal void JoinAutoPacking(BatchPackingRelation rela, Workcell workcell)
        {
            try
            {
                ClearInfos();
                if (rela.PackageUnit.IsMasterUnit)
                {
                    RT.Service.Resolve<BatchWipPackingController>().AutoJoinWithIuputBatch(
                        WorkOrderId.Value,
                        OuterBatchPackingRelation,
                        CurrentPackageRuleLevel,
                        GetWorkcell());
                }
                else
                {
                    RT.Service.Resolve<BatchWipPackingController>().AutoJoinWithReletion(
                        WorkOrderId.Value,
                        OuterBatchPackingRelation,
                        CurrentPackageRuleLevel,
                        ChildPackageRuleLevel,
                        GetWorkcell());
                }

                ShowTips("包装[{0}]加入自动打包成功。".L10nFormat(OuterBatchPackingRelation.PackageNo));

                //级联打包
                if (AutoDoPackingMode == AutoDoPackingMode.AutoCasePacking)
                {
                    RT.Service.Resolve<BatchWipPackingController>().DoPkgPacking(WorkOrder, workcell);
                    ShowTips("包装[{0}]加入自动级联打包成功。".L10nFormat(OuterBatchPackingRelation.PackageNo));
                }

                RefreshStatistics();
                ResetOuterRelation();
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }

            RefreshInputBatch();
            ReloadPackingRelation();
        }
        #endregion

        #region Tips
        /// <summary>
        /// 显示提示信息
        /// </summary>
        public void ShowTips()
        {
            ShowTips(ScanMode == ScanMode.Normal ? GetNormalTips() : GetJoinTips());
        }

        /// <summary>
        /// 正常模式提示信息
        /// </summary>
        /// <returns>提示信息</returns>
        string GetNormalTips()
        {
            return "请扫描{0}".L10nFormat(Step?.InputCollectStep?.BarcodeType.ToLabel());
        }

        /// <summary>
        /// 加入模式提示信息
        /// </summary>
        /// <returns>提示信息</returns>
        string GetJoinTips()
        {
            if (OuterBatchPackingRelation == null)
                return "请先扫描同一包装里的条码,或者双击目标包装,进行加入扫码。".L10N();
            if (OuterBatchPackingRelation.TreePId != null)
            {
                var tips = "包装[{0}]识别失败,包装被打包，切换包装后进行加入扫码。".L10nFormat(OuterBatchPackingRelation.PackageUnit.Name);
                OuterBatchPackingRelation = null;
                return tips;
            }
            else if (CurrentPackageRuleLevel != null && OuterBatchPackingRelation.PackageUnit.IsMasterUnit && CurrentPackageRuleLevel.Qty <= OuterBatchPackingRelation.ItemQty)
            {
                var tips = "包装[{0}]识别失败,包装已满，切换包装后进行加入扫码。".L10nFormat(OuterBatchPackingRelation.PackageUnit.Name);
                OuterBatchPackingRelation = null;
                return tips;
            }
            else if (CurrentPackageRuleLevel != null && !OuterBatchPackingRelation.PackageUnit.IsMasterUnit && CurrentPackageRuleLevel.LevelQty <= OuterBatchPackingRelation.PackedQty)
            {
                var tips = "包装[{0}]识别失败,包装已满，切换包装后进行加入扫码。".L10nFormat(OuterBatchPackingRelation.PackageUnit.Name);
                OuterBatchPackingRelation = null;
                return tips;
            }
            else if (CurrentPackageRuleLevel != null && ChildPackageRuleLevel != null)
            {
                return "包装[{0}]识别成功,请扫描要加入[{1}]包装条码。".L10nFormat(CurrentPackageRuleLevel.PackageUnit.Name, ChildPackageRuleLevel.PackageUnit.Name);
            }
            else if (CurrentPackageRuleLevel != null && ChildPackageRuleLevel == null)
            {
                return "包装[{0}]识别成功,请扫描要加入的批次条码。".L10nFormat(OuterBatchPackingRelation.PackageUnit.Name);
            }
            else
            {
                return "请重置".L10N();
            }
        }
        #endregion

        /// <summary>
        /// 去除工序变更时，生成子批和打印命令隐藏事件总线
        /// </summary>
        protected override void ProcessChanged(Process process)
        {
            // 去除工序变更时，生成子批和打印命令隐藏事件总线
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        public override void Onload()
        {
            base.Onload();
            RT.RemotingEventBus.Subscribe<PackingEvent>(this, PrintPackageBarcodes);
        }

        /// <summary>
        /// 视图关闭事件
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
        }

        /// <summary>
        /// 自动打印
        /// </summary>
        /// <param name="packingEvent">批次打包事件</param>
        private void PrintPackageBarcodes(PackingEvent packingEvent)
        {
            var task = CRT.MainThread.InvokeAsync(() =>
            {
                try
                {
                    double stationId = packingEvent.StationId;
                    if (stationId != GetWorkcell().StationId)
                        return;

                    var relaIds = packingEvent.RelationIdList;
                    if (!IsAutoPrintLabel || Printer.IsNullOrEmpty() || relaIds == null || relaIds.Length == 0)
                        return;

                    var packingRelations = RT.Service.Resolve<BatchWipPackingController>().GetBatchPkgRelationByIds(relaIds);
                    if (packingRelations.Count == 0)
                        return;

                    var pkgUint = packingRelations.FirstOrDefault().PackageUnit;
                    var rule = PackageRuleDetailList.First(p => p.PackageUnitId == pkgUint?.Id);
                    if (rule == null)
                        throw new ValidationException("工单[{0}]不存在[{1}]对应的包装层级".L10nFormat(WorkOrder.No, pkgUint?.Name));
                    if (rule.IsPrint)
                    {
                        if (rule.PrintTemplateId == null)
                            throw new ValidationException("工单[{0}]对应的[{1}]包装层级不存在打印模板".L10nFormat(WorkOrder.No, pkgUint?.Name));

                        var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(rule.PrintTemplateId.Value);
                        var printable = new BatchPackingRelationPrintable();
                        var report = ReportFactory.Current.GetReportByExtension(rule.PrintTemplate.Type);

                        report.Print(printable, filePath, this.Printer, () =>
                        {
                            return packingRelations;
                        }, () =>
                        {
                            RT.Service.Resolve<BatchWipPackingController>().SaveRelasAfterPrintSuccessfully(packingRelations.Select(p => p.Id).ToArray());
                        });
                    }
                }
                catch (Exception exc)
                {
                    ShowError(exc);
                }
                finally
                {
                    ReloadPackingRelation();
                }
            });

            task.Wait();
        }

        /// <summary>
        /// 加载工作站数据
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();
            if (Workstation.Station != null)
            {
                _configValue = ConfigService.GetConfig(new WipPackingConfig(), this.GetType(), ResourceStation.Find(Workstation.Station));
                IsAutoPrintLabel = _configValue.IsAutoPrintPackageLabel;
                AutoDoPackingMode = _configValue.AutoDoPackingMode;
            }
        }
    }
}
