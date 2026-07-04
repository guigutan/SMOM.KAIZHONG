using Newtonsoft.Json;
using SIE.XPCJ.BussRepairs.Properties;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.Models.WIP.Repairs;
using SIE.XPCJ.WIP.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ.BussRepairs
{
    public partial class RepairsForm : Common.Forms.FormBase
    {
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


        /// <summary>
        /// 验证过的条码，防止验证通过后，提交前再修改条码
        /// </summary>
        public CollectBarcode SubmitBarcode { get; set; }

        /// <summary>
        /// 换料后原键件处理方式配置项的值
        /// </summary>
        private ChangeItemHandleMethod changeItemHandleMethod;

        /// <summary>
        /// 上一次的采集结果
        /// </summary>
        public ResultType? LastResultType { get; set; }

        /// <summary>
        /// 存在可选路径
        /// </summary>
        public bool HasOptionalPath { get; set; }

        /// <summary>
        /// 维修缺陷
        /// </summary>

        private List<RepairDefectViewModel> RepairDefectList { get; set; } = new List<RepairDefectViewModel>();

        /// <summary>
        /// 换料明细
        /// </summary>
        private List<ProductAssemblyDetailViewModel> DetailList { get; set; } = new List<ProductAssemblyDetailViewModel>();

        public bool IsLoadItem
        {
            get
            {

                return !xpScanBarcode1.ALeftSwitchChecked;
            }
            set
            {
                xpScanBarcode1.ALeftSwitchChecked = value;
            }
        }


        public RepairsForm(Form formMain)
        {
            InitializeComponent();
            this.FormMain = formMain;
            this.xpTitle1.AProcessType = ProcessType.Fix;
            detailsNoBoderForm = new List<Form>();

            PanelInfo = new SubmitPanelInfo();

            //默认合格
            Qualified = false;

            var repairSetting = GetConfig();
            if (repairSetting != null)
            {
                InitDevicePort(repairSetting);
                this.changeItemHandleMethod = repairSetting.ChangeItemHandleMethod;
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
            var repairSetting = GetConfig();
            if (repairSetting != null)
            {
                this.changeItemHandleMethod = repairSetting.ChangeItemHandleMethod;
                CloseSerial();
                InitDevicePort(repairSetting);
            }

        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        private RepairsSetting GetConfig()
        {
            RepairsSetting repairsSetting = null;
            var setting = Settings.Default.RepairsSettiing;
            if (!string.IsNullOrEmpty(setting))
            {
                repairsSetting = JsonConvert.DeserializeObject<RepairsSetting>(setting);
            }
            return repairsSetting;
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

            this.xpScanBarcode1.ABarcode = "";
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
            //根据已选工作单元获取工序信息 如工作单元为空，则弹出工作单元选择
            if (this.xpTitle1.Workcell == null)
            {
                this.xpTitle1.ShowFormChangeWorkCell();
            }

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
                SetTips(this.IsLoadItem ? "请扫描上料的物料标签".L10N() : "请扫描条码".L10N(), false);
                this.messageListCtr1.AddMessage(this.IsLoadItem ? "请扫描上料的物料标签".L10N() : "请扫描条码".L10N(), Common.Controls.XPMessageType.Error);
                return;
            }

            this.xpScanBarcode1.ATips = "";
            var workcell = this.xpTitle1.Workcell;

            try
            {
                if (IsLoadItem)
                {
                    LoadItem(barcode, workcell);
                }
                else
                {
                    Repair(workcell, barcode);
                }
            }
            catch (Exception exc)
            {
                SetTips(exc.Message, false);
                this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
            }
            finally
            {
                this.xpScanBarcode1.ResetBarcode();
            }
        }

        /// <summary>
        /// 上料
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        protected virtual void LoadItem(string barcode, Workcell workcell)
        {
            try
            {
                Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType = new Dictionary<LoadItemSourceType, bool>
            {
                //批次上料采集不能上料【单体条码】
                { LoadItemSourceType.SN, true }
            };

                var loadItemBarcodeInfo = ProductAssemblyDetailViewModelHelper.GetLoadBarcodeInfo(barcode, workcell, dicLoadItemSourceType, this.CurrentWorkOrder.Id);
                WipService.NewLoadItem(loadItemBarcodeInfo, workcell, false);
                RefreshLoadItem();

                SetTips("[{0}]上料成功".L10nFormat(barcode), true);
                this.messageListCtr1.AddMessage("[{0}]上料成功".L10nFormat(barcode));
            }
            catch (Exception exc)
            {
                SetTips(exc.Message, false);
                this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
            }
        }

        /// <summary>
        /// 维修
        /// </summary>
        /// <param name="workcell"></param>
        /// <param name="barcode"></param>

        private void Repair(Workcell workcell, string barcode)
        {
            if (SubmitBarcode != null)
            {
                SetTips("[{0}]未完成,请先完成或者重新开始".L10nFormat(SubmitBarcode.Code), false);
                this.messageListCtr1.AddMessage("[{0}]未完成,请先完成或者重新开始".L10nFormat(SubmitBarcode.Code), Common.Controls.XPMessageType.Error);

                return;
            }
            var currentStep = Step.CurrentStep;
            var collectBarcode = new CollectBarcode() { Code = barcode, Type = currentStep.BarcodeType };


            var info = RepairsService.ValidateBarcode(collectBarcode, workcell, (CurrentWorkOrder == null || CurrentWorkOrder.Id <= 0) ? 0 : CurrentWorkOrder.Id);
            if (info.WorkOrderInfo != null && (CurrentWorkOrder == null || info.WorkOrderInfo.Id != CurrentWorkOrder.Id))
            {
                var msg = string.Format("工单已切换,由[{0}]切换到[{1}]".L10N(), CurrentWorkOrder?.No, info.WorkOrderInfo.No);
                SetTips(msg, true);
                this.messageListCtr1.AddMessage(msg);
                this.CurrentWorkOrder = info.WorkOrderInfo;
                this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.No;
                this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.ProductCode;
            }
            MergeData(info.ProductInfo);



            if (info.ProductInfo.Context.Contains("HasOptionalPath"))
            {
                HasOptionalPath = Convert.ToBoolean(info.ProductInfo.Context["HasOptionalPath"]);
            }
            else
            {

                HasOptionalPath = false;
            }
            this.xpButtonSubmit.Visible = HasOptionalPath;
            MergeData(info.ProductInfo);

            SubmitBarcode = collectBarcode;
            this.repairsListPanelListCtr1.ClearData();

            RepairsService.GetRepairRecord(info.ProductInfo.WorkOrderId, SubmitBarcode.Code);

            //设置过站状态
            WipProductProcessState = info.ProductInfo.WipProductProcessState;

            //上一次采集的采集结果（通过/失败）
            LastResultType = info.ProductInfo.LastResultType;
            if (WipProductProcessState == WipProductProcessState.Finish)
            {
                var repairDefectViewModels = RepairsService.GetLoadDefects(barcode, workcell);
                RepairDefectList.Clear();
                RepairDefectList.AddRange(repairDefectViewModels);
                this.repairsListPanelListCtr1.SetData(RepairDefectList);
                LoadRepairDetailViewModel(barcode, false);
                this.changeItemPanelListCtr1.SetData(DetailList);
            }

            if (WipProductProcessState == WipProductProcessState.Start)
            {
                //入站（Move In)
                Submit(workcell, collectBarcode, null);
            }
            else
            {
                SetTips("[{0}]扫描成功，请维修".L10nFormat(SubmitBarcode.Code), true);
                this.messageListCtr1.AddMessage("[{0}]扫描成功，请维修".L10nFormat(SubmitBarcode.Code));
            }

        }

        /// <summary>
        /// 加载换料明细
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="isItem">是否半成品</param>
        private void LoadRepairDetailViewModel(string barcode, bool isItem)
        {
            var keyItems = RepairsService.GetLoadkeyItems(barcode, isItem);
            foreach (var keyItem in keyItems)
            {
                var parent = DetailList.FirstOrDefault(p => p.KeyItem.SourceCode == barcode);
                if (parent == null)
                {
                    var productAssemblyDetailViewModel = new ProductAssemblyDetailViewModel()
                    {
                        KeyItem = keyItem,
                        TreePId = parent?.Id,
                        HandleMethod = this.changeItemHandleMethod,
                        Workcell = this.xpTitle1.Workcell,
                        WorkOrderId = this.CurrentWorkOrder.Id,
                        Id = Guid.NewGuid().ToString()
                    };

                    productAssemblyDetailViewModel.SourceCode = keyItem.SourceCode;
                    productAssemblyDetailViewModel.ProcessName = keyItem.ProcessName;
                    productAssemblyDetailViewModel.KeyItemItemCode = keyItem.ItemCode;
                    productAssemblyDetailViewModel.KeyItemItemName = keyItem.ItemName;
                    DetailList.Add(productAssemblyDetailViewModel);
                }

                if (keyItem.SourceType == LoadItemSourceType.SN) //半成品
                {
                    LoadRepairDetailViewModel(keyItem.SourceCode, true);
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
        /// 提交
        /// </summary>
        /// <param name="workcell"></param>
        /// <param name="collectBarcode"></param>
        private void Submit(Workcell workcell, CollectBarcode collectBarcode, double? upRoutingProcessId = null)
        {

            try
            {
                ValidateCombinedCodeBinding();

                var collectData = new CollectData();

                collectData.State = WipProductProcessState;

                collectData.CollectBarcode = collectBarcode;
                collectBarcode.Code = SubmitBarcode.Code;
                InitCombinedCodeInfo(collectData);

                if (collectData.State == WipProductProcessState.Finish)
                {
                    PrepareCollectData(collectData, workcell);
                    RepairsService.RepairCollect(SubmitBarcode.Code, collectData, workcell, upRoutingProcessId);
                }
                else
                {
                    string[] newbarcodes = new string[] { SubmitBarcode.Code };
                    RepairsService.Collect(newbarcodes, collectData, workcell);
                }

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

                if (WipProductProcessState == WipProductProcessState.Finish)
                {
                    SetTips("[{0}]维修完成，请扫描条码".L10nFormat(collectBarcode.Code), true);
                    this.messageListCtr1.AddMessage("[{0}]维修完成，请扫描条码".L10nFormat(collectBarcode.Code));
                }
                else
                {
                    SetTips("[{0}]入站成功".L10nFormat(collectBarcode.Code), true);
                    this.messageListCtr1.AddMessage("[{0}]入站成功".L10nFormat(collectBarcode.Code));
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
                    SetTips(exc.Message.L10N(), false);
                    this.messageListCtr1.AddMessage(exc.Message.L10N());
                }
            }
        }

        private void PrepareCollectData(CollectData collectData, Workcell workcell)
        {
            //DetailList 建议重新取值 因为后面的窗体有更改过引用
            var newDetailList = changeItemPanelListCtr1.GetNewProductAssemblyDetails();

            //将未上料的条码进行上料操作，再进行换料
            var loadItems = newDetailList.SelectMany(p => p.ChangeItemViewModelList)
                .Where(p => !p.IsLoadItem);

            if (loadItems.Any())
            {
                try
                {
                    foreach (var item in loadItems)
                    {

                        //已在上料功能上料的物料不再次上料
                        if (LoadItemList.FindIndex(m => m.SourceCode == item.LoadItemBarcodeInfo.Barcode) >= 0)
                        {
                            continue;
                        }
                        if (item.LoadItemBarcodeInfo.WipWorkOrderId == 0)
                        {
                            item.LoadItemBarcodeInfo.WipWorkOrderId = this.CurrentWorkOrder.Id;
                        }
                        WipService.NewLoadItem(item.LoadItemBarcodeInfo, workcell, false);
                    }
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.L10N());
                }
                //刷新上料
                RefreshLoadItem();
            }

            foreach (var productAssemblyDetailViewModel in newDetailList)
            {
                var useItem = new RepairUseItem()
                {
                    Sn = productAssemblyDetailViewModel.KeyItem.SourceCode,
                    SoureKeyItemId = productAssemblyDetailViewModel.KeyItem.Id,
                    HandleMethod = productAssemblyDetailViewModel.HandleMethod,
                    WorkOrderId = productAssemblyDetailViewModel.WorkOrderId,
                };

                productAssemblyDetailViewModel.ChangeItemViewModelList.ForEach(
                    e =>
                    {
                        var loadItem = LoadItemList.FirstOrDefault(p => p.SourceCode == e.ChangeSn);

                        if (loadItem == null)
                        {
                            throw new ValidationException("上料列表未找到条码{0}".L10nFormat(e.ChangeSn));
                        }

                        useItem.UserItems.Add(loadItem.Id, e.ChangeQty);
                    });

                collectData.RepairUseItems.Add(useItem);
            }

            collectData.RepairDefects.AddRange(RepairDefectList.Select(p => new RepairDefect
            {
                IsFixed = p.IsFixed,
                ProductDefectId = p.WipProductDefectId,
                Remark = p.Remark,
                Responsiblities = p.ResponsibilityList,
                Measures = p.MeasureList,
            }));

            PrepareDefectData();
        }

        /// <summary>
        /// 准备缺陷数据
        /// </summary>
        private void PrepareDefectData()
        {
            RepairDefectList.ForEach(e =>
            {
                var responsibilities = new List<DefectResponsibility>();
                responsibilities.AddRange(e.ResponsibilityList);
                var measures = new List<RepairMeasure>();
                measures.AddRange(e.MeasureList);
                ////原有缺陷责任
                e.WipProductDefect.ResponsibilityList.ForEach(f =>
                {
                    var responsibility = responsibilities.FirstOrDefault(p => p.Id == f.DefectResponsibilityId);
                    if (responsibility == null)
                    {
                        f.PersistenceStatus = PersistenceStatus.Deleted;
                    }
                    responsibilities.Remove(responsibility);
                });
                foreach (var item in responsibilities)
                {
                    var responsibility = new WipDefectResponsibility()
                    {
                        DefectResponsibilityId = item.Id,
                        WipProductDefectId = e.WipProductDefectId,
                        PersistenceStatus = PersistenceStatus.New
                    };
                    e.WipProductDefect.ResponsibilityList.Add(responsibility);
                }

                e.WipProductDefect.MeasureList.ForEach(f =>
                {
                    var measure = measures.FirstOrDefault(p => p.Id == f.RepairMeasureId);
                    if (measure == null)
                        f.PersistenceStatus = PersistenceStatus.Deleted;
                    measures.Remove(measure);
                });
                foreach (var item in measures)
                {
                    var responsibility = new WipDefectMeasure()
                    {
                        RepairMeasureId = item.Id,
                        WipProductDefectId = e.WipProductDefectId,
                        PersistenceStatus = PersistenceStatus.New
                    };
                    e.WipProductDefect.MeasureList.Add(responsibility);
                }

                e.WipProductDefect.IsFixed = e.IsFixed;
                e.WipProductDefect.Remark = e.Remark;
            });
        }


        /// <summary>
        /// 打印拼版码
        /// </summary>
        private void PrintBindingSn()
        {
            if (PanelInfo.BindingMode == BindingMode.Auto)
            {
                //获取上料采集的配置项
                var setting = Settings.Default.RepairsSettiing;
                RepairsSetting loadItemSetting = null;
                if (!string.IsNullOrEmpty(setting))
                {
                    loadItemSetting = JsonConvert.DeserializeObject<RepairsSetting>(setting);
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
                var message = string.Format("工单已切换,由[{0}]切换到[{1}]".L10N(), this.CurrentWorkOrder?.No, switchWorkOrderForm.WorkOrder.No);
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
        /// 是否可提交
        /// </summary>
        /// <returns></returns>
        public bool CanSubmit()
        {
            return SubmitBarcode != null
                && this.xpTitle1.Workcell.EmployeeId >= 0
                && this.xpTitle1.Workcell.ProcessId >= 0
                && this.xpTitle1.Workcell.StationId >= 0
                && this.xpTitle1.Workcell.ResourceId >= 0;
        }

        /// <summary>
        /// 是否可维修完成
        /// </summary>
        /// <returns></returns>
        internal bool CanRepairComplete()
        {
            bool canRepairComplete = false;

            if (!string.IsNullOrEmpty(SubmitBarcode.Code))
            {
                if (!this.HasOptionalPath)
                {
                    //有可选路径（子工艺路线）时，可以直接维修完成
                    canRepairComplete = true;
                }
                else
                {
                    //有可选路径（子工艺路线）时，必须走完子工艺路线，差且最后一站是通过，才能维修完成
                    if (LastResultType != null && LastResultType == ResultType.Pass)
                    {
                        canRepairComplete = true;
                    }
                    else
                    {
                        MessageBox.Show("维修工序存在可选路径（子工艺路线）时，必须走完子工艺路线，且最后一站是通过，才能维修完成".L10N());
                        return false;
                    }
                }
            }

            return canRepairComplete;
        }



        /// <summary>
        /// 重新开始
        /// </summary>
        private void ReStart()
        {
            this.SetTips("请扫描条码".L10N(), true);
            SubmitBarcode = null;
            this.ReflashProcessStep();
            this.messageListCtr1.AddMessage("请扫描条码".L10N(), Common.Controls.XPMessageType.Success);
            changeItemPanelListCtr1.Clear();
            this.repairsListPanelListCtr1.ClearData();
            this.xpScanBarcode1.ResetBarcode();
        }

        /// <summary>
        /// 一键下料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

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
            RepairsSettingForm repairsSettingForm = new RepairsSettingForm();
            if (repairsSettingForm.ShowDialog() == DialogResult.OK)
            {
                ReflashConfig();
            }
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
            }
        }

        private void xpTitle1_AExitClick(object sender, EventArgs e)
        {
            FormMain.Show();
            base.CloseSerial();
            this.Close();
        }

        private void xpScanBarcode1_ABarcodeChanged(object sender, EventArgs e)
        {
            if (this.xpTitle1.Workcell == null)
            {
                this.xpTitle1.ShowFormChangeWorkCell();
                return;
            }
            ScanChange(xpScanBarcode1.ABarcode);
        }

        private void xpButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (SaveRepairRecord())
                {
                    MessageBox.Show("保存成功".L10N());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void xpButtonComplete_Click(object sender, EventArgs e)
        {

            if (CanSubmit() && CanRepairComplete())
            {
                List<GotoProcessViewModel> processList = new List<GotoProcessViewModel>();
                try
                {
                    ValidateResponsibility();
                    processList = RepairsService.GetRepariRoutingProcessList(SubmitBarcode.Code, this.xpTitle1.Workcell);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                if (processList != null)
                {
                    var upline = new UplineViewModel(processList);
                    RepirsCompleteForm repirsCompleteForm = new RepirsCompleteForm(upline);
                    repirsCompleteForm.FunctionSubmit = (uplineModel) => { return Submit(uplineModel); };
                    repirsCompleteForm.ShowDialog();
                }
            }

        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="upline"></param>
        /// <returns></returns>
        public bool Submit(UplineViewModel upline)
        {
            try
            {
                if (upline.UplineProcess == null)
                {
                    MessageBox.Show("请选择上线工序".L10N());
                    return false;
                }
                var currentStep = Step.CurrentStep;
                var collectBarcode = new CollectBarcode() { Code = SubmitBarcode.Code, Type = currentStep.BarcodeType };
                this.Submit(this.xpTitle1.Workcell, collectBarcode, upline.UplineProcess.RoutingProcessId);
                return true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message.L10N());
                return false;
            }
        }

        /// <summary>
        /// 验证维修措施，缺陷责任
        /// </summary>
        /// <param name="model">维修采集视图模型</param>
        private void ValidateResponsibility()
        {
            if (this.RepairDefectList.Exists(p => p.MeasureList.Count == 0))
            {
                throw new ValidationException("缺陷[{0}]维修措施不能为空".L10nFormat(this.RepairDefectList.Find(p => p.MeasureList.Count == 0)?.WipProductDefect?.DefectCode));
            }

            if (this.RepairDefectList.Exists(p => p.ResponsibilityList.Count == 0))
            {
                throw new ValidationException("缺陷[{0}]缺陷责任不能为空".L10nFormat(this.RepairDefectList.Find(p => p.ResponsibilityList.Count == 0)?.WipProductDefect?.DefectCode));
            }
        }

        private void xpButtonSubmit_Click(object sender, EventArgs e)
        {
            if (this.CanSubmit() && this.HasOptionalPath)
            {
                try
                {
                    this.ShowRepairOptionalSubmit();
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.Message.L10N());
                }
            }
        }

        /// <summary>
        /// 验证维修结果
        /// </summary>
        /// <param name="isScrap"></param>
        private void ValidateRepairResult(bool isScrap)
        {
            if (string.IsNullOrEmpty(SubmitBarcode.Code))
                throw new ValidationException("暂未待维修条码，请扫描条码".L10N());
            if (isScrap)
                return;
            if (RepairDefectList.Exists(p => p.DefectId <= 0))
                throw new ValidationException("验证失败，缺陷不能为空".L10N());
            if (RepairDefectList.Exists(p => p.MeasureList.Count == 0))
                throw new ValidationException("验证失败，维修措施不能为空".L10N());
            if (RepairDefectList.Exists(p => p.ResponsibilityList.Count == 0))
                throw new ValidationException("验证失败，缺陷责任不能为空".L10N());

        }

        /// <summary>
        /// 保存
        /// </summary>
        private bool SaveRepairRecord()
        {
            PrepareDefectData();
            var defects = new List<WipProductDefect>();
            defects.AddRange(RepairDefectList.Select(p => p.WipProductDefect));
            if (!defects.Any())
            {
                this.SetTips("请扫描条码".L10N(), false);
                this.messageListCtr1.AddMessage("请扫描条码".L10N(), Common.Controls.XPMessageType.Error);
                return false;
            }
            RepairsService.SaveRepairRecord(defects);
            return true;
        }

        /// <summary>
        /// 可选工艺路线提交
        /// </summary>
        private void ShowRepairOptionalSubmit()
        {

            List<GotoProcessViewModel> processList = new List<GotoProcessViewModel>();
            try
            {
                ValidateRepairResult(false);
                processList = RepairsService.GetRepariOptionalPaths(SubmitBarcode.Code, this.xpTitle1.Workcell);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            if (processList != null)
            {
                var upline = new UplineViewModel(processList);
                RepirsCompleteForm repirsCompleteForm = new RepirsCompleteForm(upline);
                repirsCompleteForm.Text = "选择维修工艺路线".L10N();
                repirsCompleteForm.SetTitle("选择维修工艺路线".L10N());
                repirsCompleteForm.SetRoutingDisplay("可选路径".L10N());
                repirsCompleteForm.FunctionSubmit = (uplineModel) => { return RepairOptionalSubmit(uplineModel); };
                repirsCompleteForm.ShowDialog();
            }
        }

        private bool RepairOptionalSubmit(UplineViewModel upline)
        {
            if (upline.UplineProcess == null)
            {
                MessageBox.Show("请选择工艺路线".L10N());
                return false;
            }

            try
            {
                if (this.SaveRepairRecord())
                {
                    RepairsService.RepairOptionalSubmit(SubmitBarcode.Code, this.xpTitle1.Workcell, upline.UplineProcess.RoutingProcessId);
                    RepairDefectList.Clear();
                    DetailList.Clear();

                    this.SetTips("提交成功".L10N(), true);
                    SubmitBarcode = null;
                    this.ReflashProcessStep();
                    this.messageListCtr1.AddMessage("提交成功".L10N(), Common.Controls.XPMessageType.Success);
                    changeItemPanelListCtr1.Clear();
                    this.repairsListPanelListCtr1.ClearData();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.L10N());
                return false;
            }
        }

        private void xpWorkOrder1_AMoreInfoClick(object sender, EventArgs e)
        {
            XPFormWorkOrderDetail.ShowInfo(this.xpWorkOrder1.Parent, this.xpWorkOrder1, this.CurrentWorkOrder);
        }

        private void xpTabControlHeader1_ASelectIndexChanged(object sender, EventArgs e)
        {
            this.xpbtnOneUnloaditem.Visible = this.xpTabControlHeader1.ASelectedIndex == 2;
        }
    }
}
