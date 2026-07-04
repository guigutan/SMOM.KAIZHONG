using Newtonsoft.Json;
using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Common.Print;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.BussMove.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ.BussMove
{
    public partial class MoveForm : Common.Forms.FormBase
    {

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


        public MoveForm(Form formMain)
        {
            InitializeComponent();

            this.FormMain = formMain;
            this.xpTitle1.AProcessType = ProcessType.Assembly;

            detailsNoBoderForm = new List<Form>();
            reportTaskList = new BindingList<ReportTask>();

            PanelInfo = new SubmitPanelInfo();

            //默认合格
            Qualified = false;

            var moveSetting = GetConfig();
            if (moveSetting != null)
            {
                InitDevicePort(moveSetting);
            }
        }
        /// <summary>
        /// 刷新配置项
        /// </summary>
        private void ReflashConfig()
        {
            var moveSetting = GetConfig();
            if (moveSetting != null)
            {
                CloseSerial();
                InitDevicePort(moveSetting);
            }
        }

        /// <summary>
        /// 串口读取条码
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
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        private MoveSetting GetConfig()
        {
            MoveSetting moveSetting = null;
            var setting = Settings.Default.MoveSetting;
            if (!string.IsNullOrEmpty(setting))
            {
                moveSetting = JsonConvert.DeserializeObject<MoveSetting>(setting);
            }
            return moveSetting;
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
                if (this.CurrentProcess != null && this.CurrentProcess.ParameterList.Any())
                {
                    this.HaveFailParameter = this.CurrentProcess.ParameterList.Exists(m => m.Type == Models.WIP.Entity.ResultTypeForDesign.Fail);
                    this.scanBracodeCtr1.ARightSwitchVisible = this.HaveFailParameter;
                }
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
            MoveCollect(barcode, workcell);
        }

        /// <summary>
        /// 过站采集
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        public void MoveCollect(string barcode, Workcell workcell)
        {
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

            this.Qualified = !this.scanBracodeCtr1.ARightSwitchChecked;

            var currentStep = Step.CurrentStep;
            var collectBarcode = new CollectBarcode { Code = barcode, Type = currentStep.BarcodeType };
            try
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
                    Step.Barcodes.Add(barcode);
                }
                if (!Step.NextStep())
                {
                    this.Collect(workcell, collectBarcode);
                }
                else
                {
                    var msg = string.Format("[{0}]扫描采集成功，请扫描[{1}]".L10N(), collectBarcode.Code, (Step.ProcessSteps.ToList()[Step.StepIndex]).BarcodeType.ToLabel());
                    SetTips(msg, true);
                    this.messageListCtr1.AddMessage(msg);
                }
                this.scanBracodeCtr1.ResetBarcode();
            }
            catch (Exception exc)
            {
                SetTips(exc.Message, false);
                this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
                this.scanBracodeCtr1.ResetBarcode();

            }
        }

        private void Collect(Workcell workcell, CollectBarcode collectBarcode)
        {
            var barcodes = Step.Barcodes.ToArray();
            try
            {
                ValidateCombinedCodeBinding();
                var collectData = new CollectData();

                //设置过站记录状态(Start=>MoveIn,Finish=>MoveOut)
                collectData.State = WipProductProcessState;

                //过站状态为【出站】，当前工序的工序参数有【失败】  当前过站结果有勾选【不合格】，则弹出不良录入的窗口
                if (WipProductProcessState == WipProductProcessState.Finish
                    && this.HaveFailParameter && !this.Qualified)
                {
                    if (!InputDefect())
                    {
                        //重新开始
                        string msg = "选择【不合格】没有录入缺陷，过站失败，请切换为【合格】或扫描条码后录入缺陷代码再过站".L10N();
                        this.ReStart();
                        SetTips(msg, false);
                        this.messageListCtr1.AddMessage(msg, Common.Controls.XPMessageType.Error);
                        return;
                    }
                    else
                    {
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
                            });
                        }
                    }
                }

                InitCombinedCodeInfo(collectData);
                WipService.Collect(barcodes, collectData, workcell);
                PrintBindingSn();


                //清除已选择的缺陷
                this.DefectItemList.Clear();

                this.collectionRecordsGridCtr1.AddRecord(collectBarcode, collectData.Result);

                PanelInfo.Clear();

                if (WipProductProcessState == WipProductProcessState.Finish)
                {
                    var msg = string.Format("[{0}]过站成功".L10N(), barcodes.LastOrDefault());
                    SetTips(msg, true);
                    this.messageListCtr1.AddMessage(msg);
                }
                else
                {
                    var msg = string.Format("[{0}]入站成功".L10N(), barcodes.LastOrDefault());
                    SetTips(msg, true);
                    this.messageListCtr1.AddMessage(msg);
                }

                PrintLabel(barcodes[0]);

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
                Step.Reset();
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
                    //回滚一步
                    Step.Roolback();
                    SetTips(exc.Message, false);
                    this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
                }
            }
        }

        /// <summary>
        /// 打印标签
        /// </summary>
        /// <param name="sn"></param>
        private void PrintLabel(string sn)
        {
            //获取过站采集的配置项
            var setting = Settings.Default.MoveSetting;
            MoveSetting moveSetting = null;
            if (!string.IsNullOrEmpty(setting))
            {
                moveSetting = JsonConvert.DeserializeObject<MoveSetting>(setting);
            }
            if (moveSetting == null)
            {
                return;
            }


            if (!moveSetting.IsPrintOutCode)
            {
                return;
            }
            if (string.IsNullOrEmpty(moveSetting.Printer))
            {
                throw new ValidationException("打印机不能为空".L10N());
            }
            try
            {
                var barcode = MoveService.GetBarcode(sn);
                var templateId = MoveService.GetBarcodeTemplateId(sn);
                if (templateId <= 0)
                {
                    throw new ValidationException("外标签打印模板不能为空".L10N());
                }
                List<object> barcodes = new List<object>();
                barcodes.Add(barcode);
                List<double> barcodeIds = new List<double>();
                barcodeIds.Add(barcode.Id);

                MoveService.Reprint(barcodeIds, "打印外标签");
                string spath = XpPrinter.Instance.DownloadTempalteWithFileName(templateId, LoginInfo.Instance.InvOrgId);
                XpPrinter.Instance.Print(spath, barcodes, moveSetting.Printer);
            }
            catch (Exception exc)
            {
                throw new ValidationException(exc.Message.L10N());
            }
        }

        /// <summary>
        /// 打印拼版码
        /// </summary>
        private void PrintBindingSn()
        {
            if (PanelInfo.BindingMode == BindingMode.Auto)
            {
                //获取过站采集的配置项
                var setting = Settings.Default.MoveSetting;
                MoveSetting moveSetting = null;
                if (!string.IsNullOrEmpty(setting))
                {
                    moveSetting = JsonConvert.DeserializeObject<MoveSetting>(setting);
                }
                if (moveSetting == null)
                {
                    return;
                }

                if (string.IsNullOrEmpty(moveSetting.Printer))
                {
                    throw new ValidationException("打印机不能为空".L10N());
                }
                PrintBindingSn(moveSetting, this.CurrentWorkOrder.Id);
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
                    this.messageListCtr1.AddMessage(exc.Message, Common.Controls.XPMessageType.Error);
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

        private void btnSetting_Click(object sender, EventArgs e)
        {
            MoveSettingForm moveSettingForm = new MoveSettingForm();
            DialogResult dialogResult = moveSettingForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
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
