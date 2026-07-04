using Newtonsoft.Json;
using SIE.XPCJ.Common.ApiCall;
using SIE.XPCJ.Common.Controls.TreeGrid;
using SIE.XPCJ.Common.Extensions;
using SIE.XPCJ.Common.Forms;
using SIE.XPCJ.Common.Print;
using SIE.XPCJ.Common.Services;
using SIE.XPCJ.Common.Settings;
using SIE.XPCJ.Models;
using SIE.XPCJ.Models.ConfigsSetting;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.Exceptions;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.Models.WIP.Packing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using SIE.XPCJ.BussRework.Properties;

namespace SIE.XPCJ.BussRework
{
    public partial class ReworkForm : Common.Forms.FormBase
    {

        /// <summary>
        /// 完成标识，当扫码输入OK时，表示完成扫码
        /// </summary>
        public static readonly string SubmitCode = "OK";
        /// <summary>
        /// 置换后处理方式
        /// </summary>
        private ReplaceItemHandleMethod ReplaceItemHandleMethod = ReplaceItemHandleMethod.Scrap;

        /// <summary>
        /// 工单
        /// </summary>
        private WorkOrder CurrentWorkOrder;

        /// <summary>
        /// 采集步骤
        /// </summary>
        private XPReworkStep ReworkStep;

        private List<XPWipProductProcessKeyItem> ListKeyItems = new List<XPWipProductProcessKeyItem>();

        bool selectedBlankingWay = false;

        XPLinesideWarehouse selectedWarehouse = null;

        public ReworkForm(Form formMain)
        {
            InitializeComponent();
            this.xpTitle1.FormMain = formMain;
            this.xpTitle1.AProcessType = ProcessType.Rework;

            this.xpButtonSubmit.Visible = false;
            this.checkBoxAll.Visible = false;
            this.xpScanBarcode1.ShowTips("请扫描返工工单条码".L10N());
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

        private void ReworkForm_Load(object sender, EventArgs e)
        {
            this.xpCardListPanel1.ACard = this.keyItemCard1;
            this.xpCardListPanel1.DataSource = KeyItemDataObj.GenByXPWipProductProcessKeyItem(this.ListKeyItems, this.xpScanBarcode1.ALeftSwitchCheckedIndex == 1);

            //获取配置项目
            var setting = GetConfig();
            if (setting != null)
            {
                this.ReplaceItemHandleMethod = setting.ReplaceItemHandleMethod;
                CloseSerial();
                InitDevicePort(setting);
            }

            //获取当前工单信息
            if (this.CurrentWorkOrder == null)
            {
                new Task(() =>
                {
                    this.BeginInvoke(new Action(() =>
                    {
                        GetCurrentInfo(this.xpTitle1.Workcell);
                    }));
                }).Start();
            }

            this.xpScanBarcode1.ResetBarcode();

            //根据已选工作单元获取工序信息 如工作单元为空，则弹出工作单元选择
            if (this.xpTitle1.Workcell == null)
            {
                this.xpTitle1.ShowFormChangeWorkCell();
            }
        }

        ///// <summary>
        ///// 刷新当前工序和采集步骤
        ///// </summary>
        //private void ReflashProcessStep()
        //{
        //    if (this.xpTitle1.Workcell != null)
        //    {
        //        base.CurrentProcess = WipService.GetProcessInfo(this.xpTitle1.Workcell.ProcessId);
        //        base.Step = new CollectStep(this.xpTitle1.Workcell, this.CurrentProcess);
        //    }
        //}

        /// <summary>
        /// 根据Workcell获取当前工作单元上的工单
        /// </summary>
        /// <param name="workcell"></param>
        private void GetCurrentInfo(Workcell workcell)
        {
            if (workcell == null)
                return;

            //记录旧的工单
            WorkOrder preWorkOrder = this.CurrentWorkOrder;

            XPApiResultRework result = ReworkService.GetCurrentInfo(workcell);
            result.IsChangeOrder = this.CurrentWorkOrder != null && result.WorkOrder != null && this.CurrentWorkOrder.Id != result.WorkOrder.Id;

            this.ResetCurrentWorkOrder(result);

            this.ReworkStep = result.Step;
            this.GetCollectionQty(); //获取当班和工序采集数
        }

        /// <summary>
        /// 工作单元切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpTitle1_AWorkCellChanged(object sender, EventArgs e)
        {
            this.GetCurrentInfo(this.xpTitle1.Workcell);
        }


        /// <summary>
        /// 扫码事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpScanBarcode1_ABarcodeChanged(object sender, EventArgs e)
        {
            if (this.xpTitle1.Workcell == null)
            {
                this.xpScanBarcode1.ShowError("请选择工作单元".L10N());
                return;
            }

            if (string.IsNullOrEmpty(xpScanBarcode1.ABarcode))
                return;

            try
            {
                //校验工单 切换工单
                if (ReworkStep.StepIndex == 0 && xpScanBarcode1.ALeftSwitchCheckedIndex == 1)//第一步要判断当前工单是否需要切换
                {
                    var currentStep = ReworkStep.CurrentStep;
                    var collectBarcode = new CollectBarcode { Code = xpScanBarcode1.ABarcode, Type = currentStep.BarcodeType };
                    var workcell = this.xpTitle1.Workcell;
                    var info = WipService.ValidateBarcode(collectBarcode, workcell, (CurrentWorkOrder == null || CurrentWorkOrder.Id <= 0) ? 0 : CurrentWorkOrder.Id);
                    if (info.WorkOrderInfo != null && (CurrentWorkOrder == null || info.WorkOrderInfo.Id != CurrentWorkOrder.Id))
                    {
                        var msg = string.Format("工单已切换,由[{0}]切换到[{1}]".L10N(), CurrentWorkOrder?.No, info.WorkOrderInfo.No);
                        this.xpScanBarcode1.ShowTips(msg);
                        this.xpMessageList1.AddMessage(msg, Common.Controls.XPMessageType.Success);
                        this.CurrentWorkOrder = info.WorkOrderInfo;
                        this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.No;
                        this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.ProductCode;
                    }
                }

                if (xpScanBarcode1.ALeftSwitchCheckedIndex == 0)//条码置换
                {
                    PermuteAssemblyCollect();
                }
                else if (xpScanBarcode1.ALeftSwitchCheckedIndex == 1) //解绑关键件
                {
                    PermuteUnboundAssemblyCollect();
                }
                else if (xpScanBarcode1.ALeftSwitchCheckedIndex == 2) //(ReworkOperate == ReworkOperate.Unbound)
                {
                    KeyItemUnbound();
                }

               

            }
            catch (Exception exc)
            {
                this.xpScanBarcode1.ShowError(exc.Message);
                this.xpMessageList1.AddMessage(exc.Message);
            }
            this.xpScanBarcode1.ABarcode = "";
        }

        /// <summary>
        /// 条码置换
        /// </summary>
        private void PermuteAssemblyCollect()
        {
            var result = ReworkService.PermuteAssemblyCollect(xpScanBarcode1.ABarcode, this.CurrentWorkOrder == null ? 0 : this.CurrentWorkOrder.Id,
                this.xpTitle1.Workcell, this.ReworkStep, ListKeyItems.Select(p => p.Id).ToList(),
                ListKeyItems.Where(p => p.IsUnbound).Select(p => p.Id).ToList());
            if (result.ResultType == ResultType.Pass)
                this.xpCollectionRecordsGrid1.AddRecord(result.CollectBarcode, result.ResultType);
            if (result.IsChangeOrder)
                ResetCurrentWorkOrder(result);
            this.xpMessageList1.AddMessage(result.Tips);
            this.ResetStep(result);
        }


        private bool PermuteUnboundSubmitCheck()
        {

            bool checkFlag = true;
            if (!ReworkStep.HasNextStep())
            {
                checkFlag = false;
                if (xpScanBarcode1.ABarcode ==SubmitCode)
                {
                    Submit();
                }
                else
                {
                    this.xpScanBarcode1.ShowTips("上一条码未提交,扫[{0}]提交或者重新开始".L10nFormat(SubmitCode));
                    this.xpMessageList1.AddMessage("上一条码未提交,扫[{0}]提交或者重新开始".L10nFormat(SubmitCode));
                }
            }

            return checkFlag;
        }

        /// <summary>
        /// 条码置换解绑关健件采集
        /// </summary>
        private void PermuteUnboundAssemblyCollect()
        {
            if (!PermuteUnboundSubmitCheck())
            {
                return;
            }

            selectedBlankingWay = false;
            selectedWarehouse = null;
            ReplaceItemHandleMethod? replaceItemHandleMethod = null;
            if (!ReworkStep.HasNextStep() && this.ListKeyItems.Count > 0 && ListKeyItems.Where(p => p.IsUnbound).Select(p => p.Id).ToList().Count > 0)
            {
                List<XPLinesideWarehouse> listWarehouse = ReworkService.GetWarehouse(this.ListKeyItems[0].ResourceId);
                ReworkKeyItemComfrimForm frm = new ReworkKeyItemComfrimForm(this.ReplaceItemHandleMethod, listWarehouse);
                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                selectedBlankingWay = frm.SelectedBlankingWay;
                selectedWarehouse = frm.SelectedWarehouse;
                replaceItemHandleMethod = frm.m_ReplaceItemHandleMethod;
            }
            var result = ReworkService.PermuteUnboundAssemblyCollect(xpScanBarcode1.ABarcode, this.CurrentWorkOrder == null ? 0 : this.CurrentWorkOrder.Id,
                this.xpTitle1.Workcell, this.ReworkStep,
                selectedBlankingWay, selectedWarehouse, ListKeyItems.Select(p => p.Id).ToList(),
                ListKeyItems.Where(p => p.IsUnbound).Select(p => p.Id).ToList(), replaceItemHandleMethod);
            if (result.ResultType == ResultType.Pass)
                this.xpCollectionRecordsGrid1.AddRecord(result.CollectBarcode, result.ResultType);
            this.xpMessageList1.AddMessage(result.Tips);
            this.ResetStep(result);
        }

        /// <summary>
        /// 关健件解绑
        /// </summary>
        private void KeyItemUnbound()
        {
            selectedBlankingWay = false;
            selectedWarehouse = null;

            XPWipProductProcessKeyItem curKeyItem = ReworkService.PreKeyItemUnbound(xpScanBarcode1.ABarcode);
            if (curKeyItem == null)
            {
                this.xpScanBarcode1.ShowError("未找到关键键[{0}]".L10nFormat(xpScanBarcode1.ABarcode));
                return;
            }
            this.ListKeyItems.Add(curKeyItem);
            ReplaceItemHandleMethod? replaceItemHandleMethod = null;
            List<XPLinesideWarehouse> listWarehouse = ReworkService.GetWarehouse(curKeyItem.ResourceId);
            ReworkKeyItemComfrimForm frm = new ReworkKeyItemComfrimForm(this.ReplaceItemHandleMethod, listWarehouse);
            if (frm.ShowDialog() != DialogResult.OK)
                return;

            selectedBlankingWay = frm.SelectedBlankingWay;
            selectedWarehouse = frm.SelectedWarehouse;
            replaceItemHandleMethod = frm.m_ReplaceItemHandleMethod;

            List<double> listKeyItemId = new List<double>();
            listKeyItemId.Add(curKeyItem.Id);

            var result = ReworkService.KeyItemUnbound(xpScanBarcode1.ABarcode, this.CurrentWorkOrder == null ? 0 : this.CurrentWorkOrder.Id,
                this.xpTitle1.Workcell, this.ReworkStep,
                selectedBlankingWay, selectedWarehouse, listKeyItemId, listKeyItemId, replaceItemHandleMethod);
            this.xpCollectionRecordsGrid1.AddRecord(result.CollectBarcode, result.ResultType);
            this.xpMessageList1.AddMessage(result.Tips);
            this.ResetStep(result);
        }

        /// <summary>
        /// 切换工单
        /// </summary>
        /// <param name="result"></param>
        private void ResetCurrentWorkOrder(XPApiResultRework result)
        {
            if (result.IsChangeOrder)
            {
                string tips = "工单已切换,由[{0}]切换到[{1}]".L10nFormat(this.CurrentWorkOrder.No, result.WorkOrder.No);
                this.xpScanBarcode1.ShowWaring(tips);
                this.xpMessageList1.AddMessage(tips);
            }

            //切换工单
            if (this.CurrentWorkOrder == null || result.IsChangeOrder)
            {
                this.CurrentWorkOrder = result.WorkOrder;
                this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.No;
                this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.ProductCode;

                return;
            }
        }


        private void ResetStep(XPApiResultRework result)
        {
            this.ListKeyItems.Clear();
            this.xpScanBarcode1.ABarcode = "";
            this.ReworkStep = result.Step;
            if (result.WipKeyItems != null)
            {
                foreach (var keyItem in result.WipKeyItems)
                {
                    if (this.ListKeyItems.Find(p => p.Id == keyItem.Id) == null)
                        this.ListKeyItems.Add(keyItem);
                }
            }
            this.xpCardListPanel1.DataSource = KeyItemDataObj.GenByXPWipProductProcessKeyItem(this.ListKeyItems, this.xpScanBarcode1.ALeftSwitchCheckedIndex == 1);
            if (!string.IsNullOrEmpty(result.Tips))
                this.xpScanBarcode1.ShowTips(result.Tips);

            this.xpButtonSubmit.Visible = this.xpScanBarcode1.ALeftSwitchCheckedIndex == 1;
            this.xpButtonSubmit.Enabled = !this.ReworkStep.HasNextStep();
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        private void ResetStart()
        {
            this.xpScanBarcode1.ResetBarcode();
            this.xpButtonSubmit.Visible = this.xpScanBarcode1.ALeftSwitchCheckedIndex == 1;
            this.checkBoxAll.Visible = this.xpScanBarcode1.ALeftSwitchCheckedIndex == 1;
            this.xpButtonSubmit.Enabled = false;

            if (this.xpScanBarcode1.ALeftSwitchCheckedIndex == 0|| this.xpScanBarcode1.ALeftSwitchCheckedIndex == 1)
            {
                this.xpScanBarcode1.ShowTips("请扫描返工工单条码".L10N());
                this.xpMessageList1.AddMessage("请扫描返工工单条码".L10N());
            }
            else if (this.xpScanBarcode1.ALeftSwitchCheckedIndex == 2)
            {
                this.xpScanBarcode1.ShowTips("请扫描关键件条码".L10N());
                this.xpMessageList1.AddMessage("请扫描关键件条码".L10N());
            }

            this.ListKeyItems.Clear();
            this.xpCardListPanel1.DataSource = KeyItemDataObj.GenByXPWipProductProcessKeyItem(this.ListKeyItems, this.xpScanBarcode1.ALeftSwitchCheckedIndex == 1);
            this.selectedBlankingWay = false;
            this.selectedWarehouse = null;
            this.ReworkStep?.Reset();
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            this.ResetStart();
        }

        private void btnESOP_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 打开切换工单页面
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
                    this.xpScanBarcode1.ShowWaring(message);
                    this.xpScanBarcode1.ResetBarcode();
                    this.xpMessageList1.AddMessage(message);
                }
            }
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        private ReworkSetting GetConfig()
        {
            ReworkSetting setting = null;
            var s = Settings.Default.ReworkSetting;
            if (!string.IsNullOrEmpty(s))
            {
                setting = JsonConvert.DeserializeObject<ReworkSetting>(s);
            }
            return setting;
        }

        /// <summary>
        /// 打开配置项页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetting_Click(object sender, EventArgs e)
        {
            ReworkSettingForm settingForm = new ReworkSettingForm();
            DialogResult dialogResult = settingForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var setting = GetConfig();
                if (setting != null)
                {
                    this.ReplaceItemHandleMethod = setting.ReplaceItemHandleMethod;
                    CloseSerial();
                    InitDevicePort(setting);
                }
            }
        }


        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpButtonSubmit_Click(object sender, EventArgs e)
        {
            Submit();
        }

        /// <summary>
        /// 提交
        /// </summary>
        private void Submit()
        {
            ReplaceItemHandleMethod? replaceItemHandleMethod = null;
            if (this.ListKeyItems.Count > 0 && ListKeyItems.Where(p => p.IsUnbound).Select(p => p.Id).ToList().Count > 0)
            {
                List<XPLinesideWarehouse> listWarehouse = ReworkService.GetWarehouse(this.ListKeyItems[0].ResourceId);
                ReworkKeyItemComfrimForm frm = new ReworkKeyItemComfrimForm(this.ReplaceItemHandleMethod, listWarehouse);
                if (frm.ShowDialog() != DialogResult.OK)
                    return;

                selectedBlankingWay = frm.SelectedBlankingWay;
                selectedWarehouse = frm.SelectedWarehouse;
                replaceItemHandleMethod = frm.m_ReplaceItemHandleMethod;
            }

            try
            {
                var result = ReworkService.SubmitPermuteUnboundAssemblyCollect(this.CurrentWorkOrder.Id, this.xpTitle1.Workcell, this.ReworkStep,
                            selectedBlankingWay, selectedWarehouse, ListKeyItems.Select(p => p.Id).ToList(),
                            ListKeyItems.Where(p => p.IsUnbound).Select(p => p.Id).ToList(), replaceItemHandleMethod);
                this.xpMessageList1.AddMessage(result.Tips);
                this.ResetStep(result);

                selectedBlankingWay = false;
                selectedWarehouse = null;
            }
            catch (Exception exc)
            {
                this.xpScanBarcode1.ShowError(exc.Message);
                this.xpMessageList1.AddMessage(exc.Message);
            }
        }

        private void xpScanBarcode1_ALeftSwitchCheckedChanged(object sender, EventArgs e)
        {
            this.ResetStart();
        }

        private void xpWorkOrder1_AMoreInfoClick(object sender, EventArgs e)
        {
            XPFormWorkOrderDetail.ShowInfo(this.xpWorkOrder1.Parent, this.xpWorkOrder1, this.CurrentWorkOrder);
        }

        private void checkBoxAll_CheckedChanged(object sender, EventArgs e)
        {
            foreach (XPWipProductProcessKeyItem keyItem in ListKeyItems)
            {
                keyItem.IsUnbound = checkBoxAll.Checked;
            }
            this.xpCardListPanel1.DataSource = KeyItemDataObj.GenByXPWipProductProcessKeyItem(this.ListKeyItems, this.xpScanBarcode1.ALeftSwitchCheckedIndex == 1);
        }

        private void xpTabControlHeader1_ASelectIndexChanged(object sender, EventArgs e)
        {
            this.xpScanBarcode1.FocusTextBox();
        }
    }
}
