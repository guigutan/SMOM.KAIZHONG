using Newtonsoft.Json;
using SIE.XPCJ.BussInspects.Properties;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ.BussInspects
{
    public partial class InspectsForm : Common.Forms.FormBase
    {
        /// <summary>
        /// 完成标识，当扫码输入OK时，表示完成扫码
        /// </summary>
        public static readonly string SubmitCode = "OK";


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


        private const string scanMsg = "请扫描条码";


        public InspectsForm(Form formMain)
        {
            InitializeComponent();
            this.FormMain = formMain;
            this.xpTitle1.AProcessType = ProcessType.Pqc;

            detailsNoBoderForm = new List<Form>();
            reportTaskList = new BindingList<ReportTask>();

            PanelInfo = new SubmitPanelInfo();

            //默认合格
            Qualified = false;


            var inspectsSetting = GetConfig();
            if (inspectsSetting != null)
            {
                InitDevicePort(inspectsSetting);
            }

            this.scanBracodeCtr1.ARightSwitchVisible = false;
        }
        /// <summary>
        /// 刷新配置项
        /// </summary>
        private void ReflashConfig()
        {
            var inspectsSetting = GetConfig();
            if (inspectsSetting != null)
            {
                CloseSerial();
                InitDevicePort(inspectsSetting);
            }
        }


        /// <summary>
        /// 重写父类读取串口条码
        /// </summary>
        /// <param name="read"></param>
        public override void ReadBarcode(string read)
        {
            this.Invoke(new Action(() =>
            {
                if (!string.IsNullOrEmpty(read))
                {
                    this.scanBracodeCtr1.SetBarcode(read);
                }
            }));

        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        private ConfigSettingBase GetConfig()
        {
            ConfigSettingBase inspectsSetting = null;
            var setting = Settings.Default.InspectsSetting;
            if (!string.IsNullOrEmpty(setting))
            {
                inspectsSetting = JsonConvert.DeserializeObject<ConfigSettingBase>(setting);
            }
            return inspectsSetting;
        }

        private void InspectsForm_Load(object sender, EventArgs e)
        {

            if (CurrentWorkOrder == null)
            {
                //异步加载数据
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

            this.scanBracodeCtr1.ResetBarcode();
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
                var defects = WipService.GetProcessDefects(CurrentProcess.Id);
                this.defectSelectCtr1.ClearAllDefect();
                if (defects.Any())
                {
                    this.defectSelectCtr1.InittalizeData(defects);
                }
                this.scanBracodeCtr1.ARightSwitchVisible = false;
                this.DefectItemList.Clear();
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
                SetTips(scanMsg.L10N(), false);
                this.messageListCtr1.AddMessage(scanMsg.L10N(), Common.Controls.XPMessageType.Error);
                return;
            }

            this.scanBracodeCtr1.ATips = "";
            var workcell = this.xpTitle1.Workcell;
            OnBarcodeChanged(barcode, workcell);
        }

        /// <summary>
        /// 快速提交
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        private bool QuickToSubmit(string barcode)
        {
            if (!Step.HasNextStep())
            {
                if (PanelInfo.BarcodeType == BarcodeType.CombinedCode && PanelInfo.NeetToBindingSn)
                {
                    return false;
                }
                if (barcode == SubmitCode)
                {
                    Submit();
                    return true;
                }
                else
                {
                    var erroMsg = "上一条码未提交,扫[{0}]提交或者重新开始".L10nFormat(SubmitCode);
                    SetTips(erroMsg, false);
                    this.messageListCtr1.AddMessage(erroMsg, Common.Controls.XPMessageType.Error);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 执行提交逻辑
        /// </summary>
        public virtual void Submit()
        {
            try
            {
                //验证组合板采集提交
                ValidateCombinedCodeBinding();

                var collectData = new CollectData();

                //设置过站记录状态(Start => MoveIn,Finish => MoveOut)
                collectData.State = WipProductProcessState;

                collectData.CollectBarcode = new CollectBarcode
                {
                    Code = Step.Barcodes.LastOrDefault(),
                    Type = Step.CurrentStep.BarcodeType
                };

                if (collectData.State == WipProductProcessState.Finish)
                {
                    DefectItemList = this.defectSelectCtr1.GetDefectResult();
                    //入站时，不记录检验结果和缺陷数据，出站（MoveOut）时才记录
                    collectData.Result = DefectItemList.Count > 0 ? ResultType.Fail : ResultType.Pass;

                    foreach (var defectItem in DefectItemList)
                    {
                        var defect = defectItem.Defect;
                        collectData.Defects.Add(new DefectData
                        {
                            DefectId = defect.Id,
                            DefectName = defect.Description,
                            CategoryId = defect.DefectCategoryId,
                            CategoryName = defect.DefectCategory?.Description,
                            Qty = defectItem.Qty
                        });
                    }
                }

                InitCombinedCodeInfo(collectData);

                var workcell = this.xpTitle1.Workcell;

                WipService.Collect(Step.Barcodes.ToArray(), collectData, workcell);
                PrintBindingSn();

                this.collectionRecordsGridCtr1.AddRecord(collectData.CollectBarcode, collectData.Result);

                //清除已选择的缺陷
                this.DefectItemList.Clear();
                this.defectSelectCtr1.ClearDefect();
                PanelInfo.Clear();

                if (WipProductProcessState == WipProductProcessState.Finish)
                {
                    var msg = string.Format("[{0}]过站成功".L10N(), collectData.CollectBarcode.Code);
                    SetTips(msg.L10N(), true);
                    this.messageListCtr1.AddMessage(msg.L10N());
                }
                else
                {
                    var msg = string.Format("[{0}]入站成功".L10N(), collectData.CollectBarcode.Code);
                    SetTips(msg.L10N(), true);
                    this.messageListCtr1.AddMessage(msg.L10N());
                }

                this.ReflashProcessStep();
                new Task(() =>
                {
                    this.Invoke(new Action(() =>
                    {
                        //在于后端过站更新采集数是异步的 过站完成后采集数不一定更新完
                        Thread.Sleep(300);
                        GetCollectionQty();
                    }));
                }).Start();
                RefrshReportTasks();
                this.scanBracodeCtr1.ResetBarcode();
            }
            catch (Exception exc)
            {
                var baseExc = exc.GetBaseException();

                if (baseExc is UnBindingSnException)
                {
                    PanelInfo.BindingMode = BindingMode.Manual;
                    var msg = string.Format("拼板码[{0}]可绑定{1}个产品，请扫描待绑定的第{2}个产品条码".L10N(), PanelInfo.PanelCode, PanelInfo.NeetToBindingQty, PanelInfo.SnList.Count + 1);
                    SetTips(msg, false);
                    this.messageListCtr1.AddMessage(msg, Common.Controls.XPMessageType.Error);
                }
                else
                {
                    SetTips(exc.Message, false);
                    this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
                }
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
                var setting = Settings.Default.InspectsSetting;
                InspectsSetting inspectsSetting = null;
                if (!string.IsNullOrEmpty(setting))
                {
                    inspectsSetting = JsonConvert.DeserializeObject<InspectsSetting>(setting);
                }
                if (inspectsSetting == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(inspectsSetting.Printer))
                {
                    throw new ValidationException("打印机不能为空".L10N());
                }
                PrintBindingSn(inspectsSetting, this.CurrentWorkOrder.Id);
            }

        }
        /// <summary>
        /// 条码变更
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        protected void OnBarcodeChanged(string barcode, Workcell workcell)
        {
            if (string.IsNullOrEmpty(barcode)) return;
            string erroMsg = "";
            if (this.xpTitle1.Workcell.ProcessId == 0)
            {
                erroMsg = "工序不能为空".L10N();
            }
            if (this.xpTitle1.Workcell.ResourceId == 0)
            {
                erroMsg += "资源不能为空".L10N();
            }
            if (this.xpTitle1.Workcell.StationId == 0)
            {
                erroMsg += "工位不能为空".L10N();
            }
            if (!string.IsNullOrEmpty(erroMsg))
            {
                SetTips(erroMsg, false);
                this.messageListCtr1.AddMessage(erroMsg, Common.Controls.XPMessageType.Error);
                this.scanBracodeCtr1.ResetBarcode();
                return;
            }
            try
            {
                if (QuickToSubmit(barcode))
                    return;
                var currentStep = Step.CurrentStep;
                var collectBarcode = new CollectBarcode { Code = barcode, Type = currentStep.BarcodeType };

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
                            var msg = string.Format("工单已切换,由[{0}]切换到[{1}]".L10N(), CurrentWorkOrder?.No, info.WorkOrderInfo.No);
                            this.scanBracodeCtr1.ATips = msg;
                            SetTips(msg, true);
                            this.messageListCtr1.AddMessage(msg);
                            this.CurrentWorkOrder = info.WorkOrderInfo;
                            this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.No;
                            this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.ProductCode;
                        }
                        MergeData(info.ProductInfo);
                    }

                    if (Step.StepIndex != 0)
                    {
                        WipService.ValidateBarcode(collectBarcode, workcell);
                    }

                    Step.Barcodes.Add(collectBarcode.Code);
                }
                if (!Step.NextStep())
                {
                    if (PanelInfo.BindingMode == BindingMode.Manual)
                    {
                        if (!PanelInfo.NeetToBindingSn)
                        {
                            SetTips("[{0}]关联产品条码完成，请提交".L10nFormat(Step.Barcodes.FirstOrDefault()), true);
                            this.messageListCtr1.AddMessage("[{0}]关联产品条码完成，请提交".L10nFormat(Step.Barcodes.FirstOrDefault()));
                        }
                        else
                        {
                            var flaseTips = "拼板码[{0}]可绑定{1}个产品，请扫描待绑定的第{2}个产品条码".L10nFormat(PanelInfo.PanelCode, PanelInfo.NeetToBindingQty, PanelInfo.SnList.Count + 1);
                            SetTips(flaseTips, false);
                            this.messageListCtr1.AddMessage(flaseTips, Common.Controls.XPMessageType.Error);
                        }
                    }
                    else
                    {
                        if (WipProductProcessState == WipProductProcessState.Start)
                        {
                            //move in
                            Submit();
                        }
                        else
                        {
                            SetTips("[{0}]扫描成功,请提交".L10nFormat(collectBarcode.Code), true);
                            this.messageListCtr1.AddMessage("[{0}]扫描成功,请提交".L10nFormat(collectBarcode.Code));
                        }
                    }
                }
                else
                {
                    currentStep = Step.CurrentStep;
                    SetTips("[{0}]扫描成功，请扫描[{1}]".L10nFormat(collectBarcode, currentStep.BarcodeType.ToLabel()), true);
                    this.messageListCtr1.AddMessage("[{0}]扫描成功，请扫描[{1}]".L10nFormat(collectBarcode, currentStep.BarcodeType.ToLabel()));
                }
            }
            catch (Exception exc)
            {
                var excErroMsg = exc.Message;
                SetTips(excErroMsg, false);
                this.messageListCtr1.AddMessage(excErroMsg, Common.Controls.XPMessageType.Error);
            }
            finally
            {
                this.scanBracodeCtr1.ResetBarcode();
            }
        }

        /// <summary>
        /// 刷新工单任务列表
        /// </summary> 
        public void RefrshReportTasks(RetrospectType retrospectType = RetrospectType.Single, bool lazyLoad = true)
        {
            RefrshReportTasks(this.xpTitle1.Workcell.EmployeeId <= 0 ? 0 : this.xpTitle1.Workcell.EmployeeId, retrospectType, lazyLoad);
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
                    this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
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
                throw new UnBindingSnException(string.Format("拼板码绑定产品数未达到容量，请继续扫描第{0}个产品条码".L10N(), PanelInfo.SnList.Count + 1));
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
                this.xpWorkOrder1.ATextBox4Text = qtyPass.Item2.ToString("0");
            }


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
                var message = string.Format("工单已切换,由[{0}]切换到[{1}]".L10N(), this.CurrentWorkOrder?.No, switchWorkOrderForm.WorkOrder.No);
                this.CurrentWorkOrder = switchWorkOrderForm.WorkOrder;
                this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder.No;
                this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder.ProductCode;
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
            this.scanBracodeCtr1.ATips = tips;
            this.scanBracodeCtr1.ATipsColor = oprateState ? Color.FromArgb(0, 203, 106) : Color.FromArgb(255, 0, 0);
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
            this.SetTips(scanMsg.L10N(), true);
            this.ReflashProcessStep();
            this.messageListCtr1.AddMessage(scanMsg.L10N(), Common.Controls.XPMessageType.Success);
            this.scanBracodeCtr1.ResetBarcode();
        }

        /// <summary>
        /// 是否可提交
        /// </summary>
        /// <returns></returns>
        public virtual bool CanSubmit()
        {
            var workcell = this.xpTitle1.Workcell;
            if (Step == null || workcell == null)
            {
                return false;
            }
            return !Step.HasNextStep()
                && workcell.EmployeeId > 0
                && workcell.ProcessId > 0
                && workcell.StationId > 0
                && workcell.ResourceId > 0;
        }
        private void xpSubmit_Click(object sender, EventArgs e)
        {
            if (CanSubmit())
            {
                Submit();
            }
        }

        /// <summary>
        /// 配置项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            InspectsSettingForm inspectsSettingForm = new InspectsSettingForm();
            var result = inspectsSettingForm.ShowDialog();
            if (result == DialogResult.OK)
            {
                ReflashConfig();
            }
        }

        private void scanBracodeCtr1_ABarCodeChanged(object sender, EventArgs e)
        {
            if (this.xpTitle1.Workcell == null)
            {
                this.xpTitle1.ShowFormChangeWorkCell();
                return;
            }
            ScanChange(scanBracodeCtr1.ABarcode);
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
            base.CloseSerial();
            FormMain.Show();
            this.Close();
        }

        private void xpWorkOrder1_AMoreInfoClick(object sender, EventArgs e)
        {
            XPFormWorkOrderDetail.ShowInfo(this.xpWorkOrder1.Parent, this.xpWorkOrder1, this.CurrentWorkOrder);
        }
    }
}
