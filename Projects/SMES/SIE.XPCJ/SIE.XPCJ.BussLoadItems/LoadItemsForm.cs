using Newtonsoft.Json;
using SIE.XPCJ.BussLoadItems.Properties;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Threading;

namespace SIE.XPCJ.BussLoadItems
{
    public partial class LoadItemsForm : Common.Forms.FormBase
    {
        /// <summary>
        /// 计时器
        /// </summary>
        DispatcherTimer timer = new DispatcherTimer();


        /// <summary>
        /// 报工任务
        /// </summary>
        private BindingList<ReportTask> reportTaskList;

        /// <summary>
        /// 当前在制工单
        /// </summary>
        private WorkOrder CurrentWorkOrder { get; set; }

        /// <summary>
        /// 缺陷项目
        /// </summary>
        public List<DefectItem> DefectItemList { get; set; }

        /// <summary>
        /// 上料明细
        /// </summary>
        public List<LoadItem> LoadItemList { get; set; } = new List<LoadItem>();

        /// <summary>
        /// 下料明细
        /// </summary>
        public List<UnloadItem> UnloadItemList { get; set; } = new List<UnloadItem>();
        public bool IsLoadItem
        {
            get
            {

                return xpScanBarcode1.ALeftSwitchChecked;
            }
            set
            {
                xpScanBarcode1.ALeftSwitchChecked = value;
            }
        }
        public BindingList<AssemblyDetailViewModel> AssemblyDetailList { get; set; } = new BindingList<AssemblyDetailViewModel>();

        /// <summary>
        /// 上料字典集合
        /// </summary>
        public Dictionary<string, LoadItemBarcodeInfo> AssemblyItemsDictionary { get; set; } = new Dictionary<string, LoadItemBarcodeInfo>();



        public LoadItemsForm(Form formMain)
        {
            InitializeComponent();
            this.FormMain = formMain;
            xpTitle1.AProcessType = ProcessType.Assembly;

            detailsNoBoderForm = new List<Form>();
            reportTaskList = new BindingList<ReportTask>();

            PanelInfo = new SubmitPanelInfo();

            //默认合格
            Qualified = false;


            var loadItemSetting = GetConfig();
            if (loadItemSetting != null)
            {
                InitDevicePort(loadItemSetting);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="read"></param>
        public override void ReadBarcode(string read)
        {
            this.Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(read))
                {
                    this.xpScanBarcode1.SetBarcode(read);
                }
            }));
        }

        /// <summary>
        /// 刷新配置项
        /// </summary>
        private void ReflashConfig()
        {
            var loadItemSetting = GetConfig();
            if (loadItemSetting != null)
            {
                CloseSerial();
                InitDevicePort(loadItemSetting);
            }
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        private LoadItemSetting GetConfig()
        {
            LoadItemSetting loadItemSetting = null;
            var setting = Settings.Default.LoadSettiing;
            if (!string.IsNullOrEmpty(setting))
            {
                loadItemSetting = JsonConvert.DeserializeObject<LoadItemSetting>(setting);
            }
            return loadItemSetting;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (this.xpTitle1.Workcell != null)
            {
                RefreshLoadItem();
            }
        }

        protected void SetRefreshTimer()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            LoadItemSetting loadItemSetting = GetConfig();
            if (loadItemSetting != null && !string.IsNullOrEmpty(loadItemSetting.ReflashTime))
            {
                var refreshTime = int.Parse(loadItemSetting.ReflashTime);
                if (refreshTime > 0)
                {
                    timer.Interval = TimeSpan.FromSeconds(refreshTime); //设置刷新的间隔时间
                    timer.Start();
                }
            }
        }

        /// <summary>
        /// 刷新统计数
        /// </summary>
        public void RefreshStatistics()
        {
            new Task(() =>
            {
                this.BeginInvoke(new Action(() => GetCollectionQty()));
            }).Start();
        }

        /// <summary>
        /// SN是否已采集
        /// </summary>
        /// <returns></returns>
        private bool IsSnCollected()
        {
            return this.Step != null && this.Step.Barcodes != null && this.Step.Barcodes.Any();
        }


        private void MoveForm_Load(object sender, EventArgs e)
        {
            if (CurrentWorkOrder == null)
            {
                new Task(() =>
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        if (this.xpTitle1.Workcell != null)
                        {
                            CurrentWorkOrder = WipService.GetWipResourceWorkOrder(this.xpTitle1.Workcell);
                            this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.No;
                            this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.ProductCode;
                        }
                    }));
                }).Start();
            }

            new Task(() =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    GetCollectionQty();
                    ReflashProcessStep();
                }));
            }).Start();

            this.xpScanBarcode1.ResetBarcode();
            this.loadItemPanelListCtr1.ReflashLoadItemListAction = (d, s) =>
             {
                 if (string.IsNullOrEmpty(s))
                 {
                     this.SetTips("条码【{0}】正常下料成功".L10nFormat(d.SourceCode), true);
                     this.messageListCtr1.AddMessage("条码【{0}】正常下料成功".L10nFormat(d.SourceCode));
                     this.RefreshLoadItem();
                     this.RefreshUnoadItem();
                 }
                 else
                 {
                     this.SetTips(s.L10N(), false);
                     this.messageListCtr1.AddMessage(s.L10N(), Common.Controls.XPMessageType.Error);
                 }
             };
            this.loadItemPanelListCtr1.ReflashUnLoadItemListAction = (d, s) =>
            {
                if (string.IsNullOrEmpty(s))
                {
                    this.SetTips("条码【{0}】不良下料成功".L10nFormat(d.SourceCode), true);
                    this.messageListCtr1.AddMessage("条码【{0}】不良下料成功".L10nFormat(d.SourceCode));
                    this.RefreshLoadItem();
                    this.RefreshUnoadItem();
                }
                else
                {
                    this.SetTips(s.L10N(), false);
                    this.messageListCtr1.AddMessage(s.L10N(), Common.Controls.XPMessageType.Error);
                }
            };
            timer.Tick += new EventHandler(Timer_Tick);
            SetRefreshTimer();
            this.assemblyListGridCtr1.dataGridView1.DataSource = AssemblyDetailList;
            //根据已选工作单元获取工序信息 如工作单元为空，则弹出工作单元选择
            if (this.xpTitle1.Workcell == null)
            {
                this.xpTitle1.ShowFormChangeWorkCell();
            }

        }

        private void scanBracodeCtr1_ABarCodeChanged(object sender, EventArgs e)
        {

            if (this.xpTitle1.Workcell == null)
            {
                this.xpTitle1.ShowFormChangeWorkCell();
                return;
            }
            ScanChange(this.xpScanBarcode1.ABarcode);
        }

        /// <summary>
        /// 刷新当前工序和采集步骤
        /// </summary>
        public void ReflashProcessStep()
        {
            if (this.xpTitle1.Workcell != null)
            {
                this.DefectItemList = new List<DefectItem>();
                this.CurrentProcess = WipService.GetProcessInfo(this.xpTitle1.Workcell.ProcessId);
                Step = new CollectStep(this.xpTitle1.Workcell, this.CurrentProcess);
                if (this.CurrentProcess != null && this.CurrentProcess.ParameterList.Any())
                {
                    this.HaveFailParameter = this.CurrentProcess.ParameterList.Exists(m => m.Type == Models.WIP.Entity.ResultTypeForDesign.Fail);
                    this.xpScanBarcode1.ARightSwitchVisible = this.HaveFailParameter;
                }
                this.DefectItemList.Clear();
                this.RefreshLoadItem();
                this.RefreshUnoadItem();
            }
        }

        /// <summary>
        /// 扫描变更
        /// </summary>
        /// <param name="barcode"></param>
        public void ScanChange(string barcode)
        {
            if (string.IsNullOrEmpty(barcode))
            {
                SetTips(this.IsLoadItem ? "请扫描上料的物料标签".L10N() : "请扫描条码".L10N(), true);
                this.messageListCtr1.AddMessage(this.IsLoadItem ? "请扫描上料的物料标签".L10N() : "请扫描条码".L10N(), Common.Controls.XPMessageType.Success);
                return;
            }

            this.xpScanBarcode1.ATips = "";
            var workcell = this.xpTitle1.Workcell;

            try
            {
                if (IsLoadItem)
                {
                    this.xpScanBarcode1.ATips = "";

                    //验证并上料
                    ValidateLoadItem(barcode, workcell);
                }
                else
                {

                    if (IsSnCollected()
                        && Step.Barcodes.First() != barcode)
                    {
                        var msg = "生产条码【{0}】未完成装配采集，请继续或点击【重新开始】进行新条码的装配采集！".L10nFormat(this.Step.Barcodes.First());
                        SetTips(msg, false);
                        this.messageListCtr1.AddMessage(msg, Common.Controls.XPMessageType.Error);
                        return;
                    }

                    ///ClearInfos();
                    this.Qualified = !this.xpScanBarcode1.ARightSwitchChecked;
                    AssemblyCollect(barcode, workcell);
                }
            }
            catch (Exception exc)
            {
                var baseExc = exc.GetBaseException();
                if (baseExc is UnBindingSnException)
                {
                    PanelInfo.BindingMode = BindingMode.Manual;
                    var msg = "拼板码[{0}]可绑定{1}个产品，请扫描待绑定的第{2}个产品条码".L10nFormat(PanelInfo.PanelCode, PanelInfo.NeetToBindingQty, PanelInfo.SnList.Count + 1);
                    SetTips(msg, false);
                    this.messageListCtr1.AddMessage(msg, Common.Controls.XPMessageType.Error);
                }
                else
                {
                    SetTips(exc.Message, false);
                    this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
                }
            }
            finally
            {
                this.xpScanBarcode1.ResetBarcode();
            }




        }

        /// <summary>
        /// 验证上料
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        protected void ValidateLoadItem(string barcode, Workcell workcell)
        {
            Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType = new Dictionary<LoadItemSourceType, bool>
                 {
                  { LoadItemSourceType.SN, true }
                  };
            LoadItemBarcodeInfo barcodeInfo = LoadItemHelper
                .GetLoadBarcodeInfo(barcode, workcell, dicLoadItemSourceType, this.CurrentWorkOrder != null ? this.CurrentWorkOrder.Id : 0);

            LocalAddLoadItem(barcode, workcell, barcodeInfo);
        }

        /// <summary>
        /// 本地上料
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="barcodeInfo"></param>
        private void LocalAddLoadItem(string barcode, Workcell workcell, LoadItemBarcodeInfo barcodeInfo)
        {
            if (IsSnCollected()) // 装配采集
            {
                /*注释：如果上料条码为生产半成品上料则只能上料一次*/
                if (AssemblyItemsDictionary.ContainsKey(barcode) && barcodeInfo.Type == LoadItemSourceType.SN)
                {
                    throw new ValidationException("SN条码{0}已上料，请扫描其他条码".L10nFormat(barcode));
                }

                if (!AssemblyDetailList.Any(p => p.ItemId == barcodeInfo.ItemId)
                    && !AssemblyDetailList.SelectMany(x => x.AltItemList).Any(p => p.ItemId == barcodeInfo.ItemId))
                {
                    var item = WipService.GetItemInfo(barcodeInfo.ItemId);

                    throw new ValidationException("物料[{0}]非装配需求物料，请扫描其他物料条码".L10nFormat(item.Code));
                }

                //扫描SN后再上料，匹配条码是否满足装配条件
                if (ValidationAssemblyProperty(CurrentWorkOrder, barcodeInfo.ItemId, barcodeInfo.ItemExtProp))
                {
                    AddAssemblyLabel(barcodeInfo, barcodeInfo.ItemId, barcodeInfo.ItemExtProp, barcodeInfo.Qty);

                    //SN不为空，直接扣料过站
                    AssemblyCollect(this.Step.Barcodes.First(), this.xpTitle1.Workcell);
                }
            }
            else // 提前上料
            {
                //单体、半成品不提前上料
                if (barcodeInfo.Type == LoadItemSourceType.SN)
                {
                    throw new ValidationException("半成品[{0}]不能提前上料".L10nFormat(barcodeInfo.Barcode));
                }

                //物料匹配 保存上料                 
                try
                {
                    WipService.NewLoadItem(barcodeInfo, workcell, true);
                    //刷新上料
                    RefreshLoadItem();

                    SetTips("[{0}]上料成功".L10nFormat(barcode), true);
                }
                catch (Exception exc)
                {
                    SetTips(exc.Message, false);
                    this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
                }
            }
        }
        /// <summary>
        /// 刷新上料
        /// </summary>
        public void RefreshLoadItem()
        {
            try
            {
                LoadItemList.Clear();
                var workcell = this.xpTitle1.Workcell;
                var loadItems = WipService.GetLoadItemList(workcell.ResourceId, workcell.StationId);
                loadItems.ForEach(m =>
                {
                    LoadItemList.Add(m);
                });
                this.loadItemPanelListCtr1.SetData(LoadItemList);
            }
            catch (Exception exc)
            {
                SetTips(exc.Message, false);
                this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
            }
        }
        /// <summary>
        /// 装配采集
        /// </summary>
        /// <param name="barcode">编码</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void AssemblyCollect(string barcode, Workcell workcell)
        {
            var currentStep = Step.CurrentStep;
            var collectBarcode = new CollectBarcode { Code = barcode, Type = currentStep.BarcodeType };
            collectBarcode.AssemblyItems = AssemblyItemsDictionary;

            if (!IsSnCollected())
            {
                if (PanelInfo.BindingMode == BindingMode.Manual && PanelInfo.NeetToBindingSn)
                {

                    var snQty = WipService.ValidateNewBarcode(barcode, (CurrentWorkOrder == null || CurrentWorkOrder.Id <= 0) ? 0 : CurrentWorkOrder.Id);
                    PanelInfo.SnList.Add(new SnData() { Sn = barcode, Qty = snQty });
                }
                else
                {
                    if (Step.StepIndex == 0)
                    {
                        var info = WipService.ValidateBarcode(collectBarcode, workcell, (CurrentWorkOrder == null || CurrentWorkOrder.Id <= 0) ? 0 : CurrentWorkOrder.Id);
                        if (info.WorkOrderInfo != null && (CurrentWorkOrder == null || info.WorkOrderInfo.Id != CurrentWorkOrder.Id))
                        {
                            var msg = "工单已切换,由[{0}]切换到[{1}]".L10nFormat(CurrentWorkOrder?.No, info.WorkOrderInfo.No);
                            SetTips(msg, true);
                            this.messageListCtr1.AddMessage(msg);
                            this.CurrentWorkOrder = info.WorkOrderInfo;
                            this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.No;
                            this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.ProductCode;
                        }
                        MergeData(info.ProductInfo);
                    }
                    else
                    {
                        WipService.ValidateBarcode(collectBarcode, workcell);
                    }

                    if (Step.Barcodes.Contains(collectBarcode.Code))
                    {
                        throw new ValidationException("条码[{0}]重复采集".L10nFormat(collectBarcode.Code));
                    }

                    Step.Barcodes.Add(collectBarcode.Code);
                }
            }

            if (WipProductProcessState == WipProductProcessState.Finish
                && IsLackItem(barcode, workcell))
            {
                //当前过站状态非Start=> Move In，且验证上料不满足时，返回不提交
                return;
            }

            ////最后一步采集
            if (!Step.NextStep())
            {
                Submit(workcell, collectBarcode);
            }
            else
            {
                string barcodeType = Step.ProcessSteps.FirstOrDefault(p => p.Step == (Step.StepIndex + 1)).BarcodeType.ToLabel();
                SetTips("[{0}]扫描采集成功，请扫描[{1}]".L10nFormat(collectBarcode, barcodeType), true);
                this.messageListCtr1.AddMessage("[{0}]扫描采集成功，请扫描[{1}]".L10nFormat(collectBarcode, barcodeType));
            }
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="workcell"></param>
        /// <param name="collectBarcode"></param>
        private void Submit(Workcell workcell, CollectBarcode collectBarcode)
        {
            var barcodes = Step.Barcodes.ToArray();

            try
            {
                ValidateCombinedCodeBinding();

                var collectData = new CollectData();

                //过站状态为【出站】，当前工序的工序参数有【失败】  当前过站结果有勾选【不合格】，则弹出不良录入的窗口
                if (WipProductProcessState == WipProductProcessState.Finish
                    && this.HaveFailParameter && !this.Qualified)
                {
                    if (!InputDefect())
                    {
                        //重新开始
                        SetTips("选择【不合格】没有录入缺陷，过站失败，请切换为【合格】或扫描条码后录入缺陷代码再过站".L10N(), false);
                        this.messageListCtr1.AddMessage("选择【不合格】没有录入缺陷，过站失败，请切换为【合格】或扫描条码后录入缺陷代码再过站".L10N(), Common.Controls.XPMessageType.Warn);
                        return;
                    }
                    else
                    {
                        //入站时，不记录检验结果和缺陷数据，出站（MoveOut）时才记录
                        collectData.Result = DefectItemList.Count > 0 ? ResultType.Fail : ResultType.Pass;
                        collectData.Defects.AddRange(from defectItem in DefectItemList
                                                     let defect = defectItem.Defect
                                                     select new DefectData
                                                     {
                                                         DefectId = defect.Id,
                                                         DefectName = defect.Description,
                                                         CategoryId = defect.DefectCategoryId,
                                                         CategoryName = defect.DefectCategory?.Description,
                                                     });
                    }
                }

                collectData.State = WipProductProcessState;
                collectData.CollectBarcode = collectBarcode;
                collectBarcode.Code = barcodes[0];
                InitCombinedCodeInfo(collectData);

                WipService.Collect(barcodes, collectData, workcell);
                PrintBindingSn();

                this.collectionRecordsGridCtr1.AddRecord(collectBarcode, collectData.Result);

                this.ReStart();
                new Task(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        //在于后端过站更新采集数是异步的 过站完成后采集数不一定更新完
                        Thread.Sleep(200);
                        GetCollectionQty();
                    }));
                }).Start();
                RefreshLoadItem();

                RefrshReportTasks();

                if (WipProductProcessState == WipProductProcessState.Finish)
                {
                    SetTips("[{0}]过站成功，请扫描条码".L10nFormat(collectBarcode.Code), true);
                    this.messageListCtr1.AddMessage("[{0}]过站成功，请扫描条码".L10nFormat(collectBarcode.Code));
                }
                else
                {
                    SetTips("[{0}]入站成功，请扫描条码".L10nFormat(collectBarcode.Code), true);
                    this.messageListCtr1.AddMessage("[{0}]入站成功，请扫描条码".L10nFormat(collectBarcode.Code));
                }
            }
            catch (Exception exc)
            {
                var baseExc = exc.GetBaseException();
                if (baseExc is UnBindingSnException)
                {
                    PanelInfo.BindingMode = BindingMode.Manual;
                    SetTips("拼板码[{0}]可绑定{1}个产品，请扫描待绑定的第{2}个产品条码".L10nFormat(PanelInfo.PanelCode, PanelInfo.NeetToBindingQty, PanelInfo.SnList.Count + 1), false);
                }
                else
                {
                    //当前是上料模式时，不回滚
                    if (!IsLoadItem)
                    {
                        //回滚一步
                        Step.Roolback();
                    }
                    SetTips(exc.Message.L10N(), false);
                    this.messageListCtr1.AddMessage(exc.Message.L10N());
                }
                AddAssemblyDetail(barcodes[0]);
            }
        }


        /// <summary>
        /// 打印拼版码
        /// </summary>
        private void PrintBindingSn()
        {
            if (PanelInfo.BindingMode == BindingMode.Auto)
            {
                //获取上料采集的配置项
                var setting = Settings.Default.LoadSettiing;
                LoadItemSetting loadItemSetting = null;
                if (!string.IsNullOrEmpty(setting))
                {
                    loadItemSetting = JsonConvert.DeserializeObject<LoadItemSetting>(setting);
                }
                if (loadItemSetting == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(loadItemSetting.Printer))
                {
                    throw new ValidationException("打印机不能为空".L10N());
                }
                PrintBindingSn(loadItemSetting, this.CurrentWorkOrder.Id);
            }

        }
        /// <summary>
        /// 重置装配明细
        /// </summary>
        private void ResetAssemblyDetail(string barcode)
        {
            AssemblyDetailList.Clear();
            AssemblyItemsDictionary.Clear();

            var workcell = this.xpTitle1.Workcell;
            if (workcell == null)
            {
                return;
            }

            //查找【生产采集运行时产品】
            var product = LoadItemSevice.FindProduct(barcode, Step.CurrentStep.BarcodeType);
            if (product != null)
            {
                var workOrderBoms = LoadItemSevice.GetWorkOrderBom(CurrentWorkOrder.Id);
                //查找【生产采集运行时产品】后工序列表中与当前选择工序匹配的【运行时工序】
                var process = LoadItemSevice.GetProductRoutingGetNext(barcode, Step.CurrentStep.BarcodeType, workcell.ProcessId);

                if (process != null)
                {
                    var isBuckleMaterialBoms = process.Boms.Where(p => p.IsBuckleMaterial);
                    foreach (var p in isBuckleMaterialBoms)
                    {//在工序BOM存在且在工单BOM中为反冲物料的不出现在装备清单里面
                        var index = workOrderBoms.FindIndex(m => m.ItemId == p.ItemId && m.IsRecoilItem);
                        if (index >= 0)
                        {
                            continue;
                        }
                        var itemInfo = WipService.GetItemInfo(p.ItemId);
                        var detail = new AssemblyDetailViewModel()
                        {
                            ItemId = p.ItemId,
                            DemandQty = p.Qty,
                            ItemExtProp = p.ItemExtProp,
                            ItemExtPropName = p.ItemExtPropName,
                            AlterGroup = p.AlterGroup,
                            ItemCode = itemInfo.Code,
                            ItemName = itemInfo.Name
                        };

                        p.AltBom.ForEach(f =>
                        {
                            var altItemInfo = WipService.GetItemInfo(f.ItemId);
                            detail.AltItemList.Add(new AltItemViewModel
                            {
                                ItemId = f.ItemId,
                                ItemExtProp = f.ItemExtProp,
                                AlterGroup = f.AlterGroup,
                                ItemCode = altItemInfo.Code,
                                ItemName = altItemInfo.Name
                            });
                        });

                        AssemblyDetailList.Add(detail);
                    }
                    return;
                }
            }

            if (CurrentWorkOrder == null)
            {
                return;
            }

            //直接用工单的工序BOM
            var boms = LoadItemSevice.GetWorkerProcessBomList(CurrentWorkOrder.Id).Where(p => p.ProcessId == workcell.ProcessId && !p.IsAlternative).ToList();

            boms.ForEach(processBom =>
            {
                //主料
                var detail = new AssemblyDetailViewModel()
                {
                    ItemId = processBom.ItemId,
                    DemandQty = processBom.SingleQty,
                    ItemExtProp = processBom.ItemExtProp,
                    ItemExtPropName = processBom.ItemExtPropName,
                };

                AssemblyDetailList.Add(detail);

                //替代料
                if (!string.IsNullOrEmpty(processBom.Alter))
                {
                    var processBomsOfAlter = LoadItemSevice.GetWorkerProcessBomList(CurrentWorkOrder.Id)
                        .Where(x => x.ProcessId == workcell.ProcessId && x.IsAlternative && x.Alter == processBom.Alter)
                        .ToList();

                    processBomsOfAlter.ForEach(f =>
                    {
                        detail.AltItemList.Add(new AltItemViewModel
                        {
                            ItemId = f.ItemId,
                            ItemExtProp = f.ItemExtProp,
                        });
                    });
                }
            });

            //组合板，需求量要乘以拼板数
            if (PanelInfo.BarcodeType == BarcodeType.CombinedCode)
            {
                foreach (var detail in AssemblyDetailList)
                {
                    detail.Qty *= (PanelInfo.PanelQty - PanelInfo.ForkPlateQty);
                }
                //AssemblyDetailList.ForEach(dtl => dtl.Qty *= (PanelInfo.PanelQty - PanelInfo.ForkPlateQty));
            }
        }


        /// <summary>
        /// 添加装配明细
        /// </summary>
        private void AddAssemblyDetail(string barcode)
        {
            ResetAssemblyDetail(barcode);

            RefreshLoadItem();

            string alterGroup = string.Empty;

            foreach (var detail in AssemblyDetailList)
            {
                //已经满足
                if (detail.DemandQty - detail.Qty <= 0)
                {
                    continue;
                }

                if (string.IsNullOrEmpty(alterGroup) || alterGroup == detail.AlterGroup)
                {
                    //匹配上料列表满足扣料的条码，添加到装配明细                
                    foreach (var loadItemEntity in LoadItemList.Where(p => p.WorkOrderId == CurrentWorkOrder.Id
                            && p.ItemId == detail.ItemId
                            && p.ItemExtProp == detail.ItemExtProp
                            && p.Qty > 0))
                    {

                        var lackQty = detail.DemandQty - detail.Qty;
                        if (loadItemEntity.Qty >= lackQty)
                        {
                            detail.Qty = detail.DemandQty;
                            loadItemEntity.Qty = loadItemEntity.Qty - lackQty;
                        }
                        else
                        {
                            detail.Qty += loadItemEntity.Qty;
                            loadItemEntity.Qty = 0;
                        }

                        detail.ItemLabel = string.Join(";", new string[] { detail.ItemLabel, loadItemEntity.SourceCode });

                        if (!string.IsNullOrEmpty(detail.AlterGroup))
                        {
                            alterGroup = detail.AlterGroup;
                        }

                        //已经满足
                        if (detail.DemandQty - detail.Qty <= 0)
                        {
                            break;
                        }
                    }
                }

                //已经满足
                if (detail.DemandQty - detail.Qty <= 0)
                {
                    continue;
                }

                alterGroup = UseAltItem(alterGroup, detail);
            }
        }

        /// <summary>
        /// 使用替代料
        /// </summary>
        /// <param name="alterGroup">替代组分组</param>
        /// <param name="detail">主料的物料需求</param>
        /// <returns>替代组分组</returns>
        private string UseAltItem(string alterGroup, AssemblyDetailViewModel detail)
        {
            foreach (var alt in detail.AltItemList)
            {
                if (string.IsNullOrEmpty(alterGroup) || alterGroup == alt.AlterGroup)
                {
                    //匹配上料列表满足扣料的条码，添加到装配明细                
                    foreach (var loadItemEntity in LoadItemList.Where(p => p.WorkOrderId == CurrentWorkOrder.Id
                        && p.ItemId == alt.ItemId
                        && p.ItemExtProp == alt.ItemExtProp
                        && p.Qty > 0))
                    {
                        var lackQty = detail.DemandQty - detail.Qty;
                        if (loadItemEntity.Qty >= lackQty)
                        {
                            detail.Qty = detail.DemandQty;
                            loadItemEntity.Qty = loadItemEntity.Qty - lackQty;
                        }
                        else
                        {
                            detail.Qty += loadItemEntity.Qty;
                            loadItemEntity.Qty = 0;
                        }

                        detail.ItemLabel = string.Join(";", new string[] { detail.ItemLabel, loadItemEntity.SourceCode });

                        if (!string.IsNullOrEmpty(alt.AlterGroup))
                        {
                            alterGroup = alt.AlterGroup;
                        }

                        //已经满足
                        if (detail.DemandQty - detail.Qty <= 0)
                        {
                            break;
                        }
                    }
                }

                //已经满足
                if (detail.DemandQty - detail.Qty <= 0)
                {
                    break;
                }
            }

            return alterGroup;
        }

        /// <summary>
        /// 是否缺料
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public bool IsLackItem(string barcode, Workcell workcell)
        {
            if (!AssemblyDetailList.Any() && Step.Barcodes.Any())
            {
                try
                {
                    LoadItemSevice.ValidateProcessBomApi(new CollectBarcode(barcode, Step.ProcessSteps.First().BarcodeType), workcell);
                    return false;
                }
                catch (Exception exc)
                {
                    if (exc.GetBaseException() is LackItemException)
                    {
                        var baseExc = exc.GetBaseException();

                        this.IsLoadItem = true;  //切换上料 会把SN清空                        

                        SetTips(((baseExc as LackItemException).Message).L10N(), false);
                        this.messageListCtr1.AddMessage(((baseExc as LackItemException).Message).L10N());

                    }
                    else
                    {
                        SetTips(exc.Message.L10N(), false);
                        this.messageListCtr1.AddMessage(exc.Message.L10N());
                    }

                    //加载产品条码在该工序的物料需求
                    AddAssemblyDetail(barcode);

                    return true;
                }
            }
            else
            {
                //验证装配清单是否满足过站
                if (AssemblyDetailList.Any(p => p.ItemLabel == null || p.DemandQty - p.Qty > 0))
                {
                    this.IsLoadItem = true;  //切换上料 会把SN清空   
                    SetTips("请扫描上料的物料标签".L10N(), false);
                    this.messageListCtr1.AddMessage("请扫描上料的物料标签".L10N(), Common.Controls.XPMessageType.Error);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///验证装配属性
        /// </summary>
        /// <param name="workOrder"></param>
        /// <param name="itemId"></param>
        /// <param name="itemExtProp"></param>
        /// <returns></returns>
        private bool ValidationAssemblyProperty(WorkOrder workOrder, double itemId, string itemExtProp)
        {
            var woProcessBomList = LoadItemSevice.GetWorkerProcessBomList(workOrder.Id);
            var workcell = this.xpTitle1.Workcell;
            if (woProcessBomList.Exists(p => p.ItemId == itemId && p.ProcessId == workcell.ProcessId && p.ItemExtProp == itemExtProp))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 输入缺陷
        /// </summary>
        /// <returns></returns>
        public bool InputDefect()
        {
            if (CurrentProcess != null
        && (CurrentProcess.ParameterList.Exists(x => x.Type == ResultTypeForDesign.Fail)
        || CurrentProcess.Type == ProcessType.Pqc
        || CurrentProcess.Type == ProcessType.BatchPqc || CurrentProcess.Type == ProcessType.Fix))
            {
                if (CurrentProcess.ParameterList.Exists(x => x.Type == ResultTypeForDesign.Fail))
                {
                    this.HaveFailParameter = true;
                }

                //加载工序对应的缺陷代码列表
                try
                {
                    XPFormSelectDefect defectSelectForm = new XPFormSelectDefect();
                    var defects = WipService.GetProcessDefects(CurrentProcess.Id);
                    if (!defects.Any())
                    {
                        SetTips("当前工序未配置缺陷，请先配置缺陷".L10N(), false);
                        this.messageListCtr1.AddMessage("当前工序未配置缺陷，请先配置缺陷".L10N());
                        return false;
                    }
                    defectSelectForm.DefectListData.AddRange(defects);
                    defectSelectForm.CurrentDefectList = this.DefectItemList;//当前选中缺陷

                    var result = defectSelectForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        this.DefectItemList = defectSelectForm.CurrentDefectList;
                        return true;
                    }
                    return false;
                }
                catch (Exception exc)
                {
                    SetTips(exc.Message, false);
                    this.messageListCtr1.AddMessage(exc.Message);
                }
            }
            else
            {
                this.HaveFailParameter = false;
            }
            return false;
        }


        /// <summary>
        /// 刷新工单任务列表
        /// </summary> 
        public void RefrshReportTasks(RetrospectType retrospectType = RetrospectType.Single, bool lazyLoad = true)
        {
            RefrshReportTasks(this.xpTitle1.Workcell.EmployeeId <= 0 ? 0 : this.xpTitle1.Workcell.EmployeeId, retrospectType, lazyLoad);
        }

        /// <summary>
        /// 添加装配标签
        /// </summary>
        /// <param name="barcodeInfo"></param>
        /// <param name="itemId"></param>
        /// <param name="itemExtProp"></param>
        /// <param name="qty"></param>
        private void AddAssemblyLabel(LoadItemBarcodeInfo barcodeInfo, double itemId, string itemExtProp, decimal qty)
        {
            bool useBarcode = false;
            var barcode = !string.IsNullOrEmpty(barcodeInfo.BillNo) ? barcodeInfo.BillNo : barcodeInfo.Barcode;
            decimal remainQty = qty;

            var tempDetailList = new List<AssemblyDetailViewModel>();
            foreach (var detail in AssemblyDetailList)
            {
                if ((detail.ItemId == itemId && detail.ItemExtProp == itemExtProp
                        || detail.AltItemList.FirstOrDefault(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp) != null)
                    && (detail.DemandQty - detail.Qty) > 0)
                {
                    var lackQty = detail.DemandQty - detail.Qty;

                    if (remainQty >= lackQty)
                    {
                        detail.Qty = detail.DemandQty;
                        detail.RemainQty = remainQty - lackQty;
                    }
                    else
                    {
                        detail.Qty += remainQty;
                        detail.RemainQty = 0m;
                    }

                    if (AssemblyItemsDictionary.ContainsKey(barcode.Trim()))
                    {
                        var exsitedBarcodeInfo = AssemblyItemsDictionary[barcode.Trim()];
                        exsitedBarcodeInfo.Qty = detail.Qty;
                    }
                    else
                    {
                        detail.ItemLabel = string.Join(";", new string[] { detail.ItemLabel, barcode });
                        AssemblyItemsDictionary.Add(barcode.Trim(), barcodeInfo);
                    }
                    useBarcode = true;
                }
                tempDetailList.Add(detail);
            }
            if (AssemblyDetailList.Any())
            {//重新更新列表
                AssemblyDetailList.Clear();
                tempDetailList.ForEach(it => { AssemblyDetailList.Add(it); });
            }

            if (!useBarcode)
            {
                var item = WipService.GetItemInfo(itemId);
                throw new ValidationException("物料[{0}]已满足装配，请扫描其他物料条码".L10nFormat(item.Code));
            }
            //this.det
        }
        /// <summary>
        /// 刷新下料列表
        /// </summary>
        public void RefreshUnoadItem()
        {
            try
            {
                UnloadItemList.Clear();
                var workcell = this.xpTitle1.Workcell;
                var unloadItems = WipService.GetUnloadItemList(workcell.ProcessId, workcell.ResourceId, workcell.StationId);
                UnloadItemList.AddRange(unloadItems.OrderBy(m => m.CreateDate));
                this.unloadItemsListCtr1.SetData(UnloadItemList);
            }
            catch (Exception exc)
            {
                SetTips(exc.Message, false);
                this.messageListCtr1.AddMessage(exc.Message);
            }
        }



        /// <summary>
        /// 刷新工单任务列表
        /// </summary>
        /// <param name="employeeId">员工Id</param>
        /// <param name="retrospectType">追溯方式</param>
        /// <param name="lazyLoad">延迟加载，采集后才做报工，需报工后再刷新任务列表</param>
        protected void RefrshReportTasks(double employeeId, RetrospectType retrospectType, bool lazyLoad = true)
        {
            new Task(() =>
            {
                if (lazyLoad)
                    Thread.Sleep(2 * 1000);
                try
                {
                    var type = this.CurrentProcess?.Type;
                    if (employeeId <= 0 || type == ProcessType.BatchFix || type == ProcessType.Fix || type == ProcessType.Rework)
                        return;
                    var tasks = WipService.GetReportTasks(employeeId, retrospectType, this.CurrentProcess?.Id);
                    this.reportTaskList.Clear();
                    foreach (var task in tasks)
                    {
                        if (CurrentWorkOrder != null && task.WorkOrderId == CurrentWorkOrder.Id)
                        {
                            this.reportTaskList.Insert(0, task);
                        }
                        else
                        {
                            this.reportTaskList.Add(task);
                        }
                    }
                    SetTaskInfoCtr();
                }
                catch (Exception exc)
                {
                    SetTips(exc.Message, false);
                    this.messageListCtr1.AddMessage(exc.Message);
                }
            }).Start();
        }


        /// <summary>
        /// 验证拼版采集
        /// </summary>
        protected virtual void ValidateCombinedCodeBinding()
        {
            if (PanelInfo.BarcodeType == BarcodeType.CombinedCode && PanelInfo.NeetToBindingSn)
            {
                throw new UnBindingSnException("拼板码绑定产品数未达到容量，请继续扫描第{0}个产品条码".L10nFormat(PanelInfo.SnList.Count + 1));
            }
        }

        /// <summary>
        /// 获取当班采集数 当前工位采集数
        /// </summary>
        public void GetCollectionQty()
        {
            //当班 当前工位 采集数获取
            if (this.xpTitle1.Workcell != null && this.xpTitle1.Workcell.ProcessId != 0 &&
                 this.xpTitle1.Workcell.ResourceId != 0 && this.xpTitle1.Workcell.StationId != 0
                )
            {
                var qtyPass = WipService.GetQtyPassAndFailed(new StatisticsQueryInfo()
                {
                    OperatorId = this.xpTitle1.Workcell.EmployeeId,
                    ProcessId = this.xpTitle1.Workcell.ProcessId,
                    ResourceId = this.xpTitle1.Workcell.ResourceId,
                    StationId = this.xpTitle1.Workcell.StationId
                });

                this.xpWorkOrder1.ATextBox3Text = qtyPass.Item1.ToString("0");
            }
        }


        /// <summary>
        /// 显示装配列表
        /// </summary>
        private void SetAssembleListDisplay()
        {
            xpbtnOneUnloaditem.Visible = false;
            this.loadItemPanelListCtr1.Visible = false;
            this.unloadItemsListCtr1.Visible = false;
            this.assemblyListGridCtr1.Visible = true;
            this.collectionRecordsGridCtr1.Visible = false;
            this.messageListCtr1.Visible = false;
            tasksListCtr1.Visible = false;
        }


        /// <summary>
        /// 设置任务列表
        /// </summary>
        private void SetTaskInfoCtr()
        {
            this.BeginInvoke(new Action(() => this.tasksListCtr1.SetData(reportTaskList)));
        }

        /// <summary>
        /// 切换工单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSwitchWo_Click(object sender, EventArgs e)
        {
            XPFormSwitchWorkOrder switchWorkOrderForm = new XPFormSwitchWorkOrder();
            switchWorkOrderForm.CurrentWo = this.CurrentWorkOrder;
            switchWorkOrderForm.Workcell = this.xpTitle1.Workcell;
            var result = switchWorkOrderForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                var message = string.Format("工单已切换,由[{0}]切换到[{1}]", this.CurrentWorkOrder?.No, switchWorkOrderForm.WorkOrder.No);
                this.CurrentWorkOrder = switchWorkOrderForm.WorkOrder;
                this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.No;
                this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.ProductCode;
                if (switchWorkOrderForm.WorkOrder != null)
                {
                    this.messageListCtr1.AddMessage(message);
                    SetTips(message, true);
                }
            }
        }

        /// <summary>
        /// 设置提示
        /// </summary>
        /// <param name="tips"></param>
        /// <param name="oprateState">操作成功</param>
        private void SetTips(string tips, bool oprateState)
        {
            this.xpScanBarcode1.ATips = tips;
            this.xpScanBarcode1.ATipsColor = oprateState ? Color.FromArgb(0, 203, 106) : Color.FromArgb(255, 0, 0);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            ReStart();
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        private void ReStart()
        {
            this.SetTips("请扫描条码".L10N(), true);
            this.ReflashProcessStep();
            this.messageListCtr1.AddMessage("请扫描条码".L10N(), Common.Controls.XPMessageType.Success);
            this.AssemblyDetailList.Clear();
            this.xpScanBarcode1.ResetBarcode();
        }

        private void xpButton2_Click_1(object sender, EventArgs e)
        {
            var workcell = this.xpTitle1.Workcell;
            var loadItems = WipService.GetLoadItemList(workcell.ResourceId, workcell.StationId);

            var source = new List<LoadItemViewModel>();
            loadItems.ForEach(it =>
            {
                source.Add(new LoadItemViewModel()
                {
                    Id = it.Id,
                    CreateDate = it.CreateDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    ItemCode = it.ItemCode,
                    ItemName = it.ItemName,
                    LoadQty = it.LoadQty,
                    Qty = it.Qty,
                    SourceCode = it.SourceCode,
                    SourceType = it.SourceType.ToLabel()

                });
            });

            OneKeyUnLoadItemForm oneKeyUnLoadItemForm = new OneKeyUnLoadItemForm();
            oneKeyUnLoadItemForm.SetDataSource(source);
            var result = oneKeyUnLoadItemForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (!oneKeyUnLoadItemForm.SelectIds.Any())
                {
                    this.SetTips("一键下料失败，请至少选择一条上料记录".L10N(), false);
                    this.messageListCtr1.AddMessage("一键下料失败，请至少选择一条上料记录".L10N(), Common.Controls.XPMessageType.Error);
                    return;
                }
                try
                {
                    WipService.UnloadAllItem(oneKeyUnLoadItemForm.SelectIds);
                    this.SetTips("一键下料成功".L10N(), true);
                    this.messageListCtr1.AddMessage("一键下料成功".L10N());
                    this.RefreshLoadItem();
                    this.RefreshUnoadItem();
                }
                catch (Exception ex)
                {
                    this.SetTips(ex.Message.L10N(), false);
                    this.messageListCtr1.AddMessage(ex.Message.L10N(), Common.Controls.XPMessageType.Error);
                }
            }

        }

        private void btnSetting_Click(object sender, EventArgs e)
        {
            LoadItemSettingForm loadItemSettingForm = new LoadItemSettingForm();
            DialogResult dialogResult = loadItemSettingForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                ReflashConfig();
                SetRefreshTimer();
            }
        }

        private void xpTitle1_AExitClick(object sender, EventArgs e)
        {
            FormMain.Show();
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            timer.Tick -= new EventHandler(Timer_Tick);
            base.CloseSerial();
            this.Close();
        }

        private void xpTitle1_AWorkCellChanged(object sender, EventArgs e)
        {
            if (this.xpTitle1.Workcell != null)
            {
                this.ReflashProcessStep();
                CurrentWorkOrder = WipService.GetWipResourceWorkOrder(this.xpTitle1.Workcell);
                this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.No;
                this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.ProductCode;
                this.GetCollectionQty();
                SetRefreshTimer();
            }
        }

        private void xpWorkOrder1_AMoreInfoClick(object sender, EventArgs e)
        {
            XPFormWorkOrderDetail.ShowInfo(this.xpWorkOrder1.Parent, this.xpWorkOrder1, this.CurrentWorkOrder);
        }

        private void xpTabControlHeader1_ASelectIndexChanged(object sender, EventArgs e)
        {
            this.xpScanBarcode1.FocusTextBox();
            this.xpbtnOneUnloaditem.Visible = false;
            if (xpTabControlHeader1.ASelectedIndex == 1)
            {
                this.xpbtnOneUnloaditem.Visible = true;
            }
        }
    }
}
