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
using SIE.XPCJ.BussNewPackage.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIE.XPCJ.BussNewPackage
{
    public partial class NewPackageForm : Common.Forms.FormBase
    {

        /// <summary>
        /// 打印机
        /// </summary>
        private string Printer = "";

        /// <summary>
        /// 打印方式
        /// </summary>
        private PrintMode PrintMode = PrintMode.Online;

        /// <summary>
        /// 是否输入标签号
        /// </summary>
        private bool IsNeedPackageNo;

        /// <summary>
        /// 工单
        /// </summary>
        private WorkOrder CurrentWorkOrder;

        /// <summary>
        /// 条码明细
        /// </summary>
        private List<XPPackageSnRecord> packageSnRecords = new List<XPPackageSnRecord>();

        /// <summary>
        /// 包装规则
        /// </summary>
        private BindingList<XPWorkOrderPackageRuleDetail> packageRules = new BindingList<XPWorkOrderPackageRuleDetail>();

        /// <summary>
        /// 包装关系
        /// </summary>
        private List<XPPackingRelation> packingRelations = new List<XPPackingRelation>();

        /// <summary>
        /// 待扫描包装号
        /// </summary>
        private Queue<string> advanceBarcodeQueue = new Queue<string>();

        /// <summary>
        /// 待扫描包装号对应单位
        /// </summary>
        private Queue<XPPackingUnit> AdvancePackingUnit = new Queue<XPPackingUnit>();

        private List<TreeGridModel> treeGridDataSource = new List<TreeGridModel>();


        public NewPackageForm(Form formMain)
        {
            InitializeComponent();
            this.xpTitle1.FormMain = formMain;
            this.xpTitle1.AProcessType = ProcessType.Packing;

            this.xpDataGridViewRule.AutoGenerateColumns = false;
            this.xpDataGridViewRule.DataSource = this.packageRules;

            this.InitTreeGrid();
        }

        /// <summary>
        /// 初始化树型Grid
        /// </summary>
        private void InitTreeGrid()
        {
            int lastColunWidth = Global.ScreenWidth - 800 - 80;
            this.treeGrid1.RowType = typeof(UCDataGridViewTreeRow);
            this.treeGrid1.IsShowChildRowCheckBox = false;
            List<DataGridViewColumnEntity> lstCulumns = new List<DataGridViewColumnEntity>();
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Sn", HeadText = "条码号".L10N(), HeadTextAlign = ContentAlignment.MiddleCenter, Width = 200, WidthType = SizeType.Absolute, TextAlign = ContentAlignment.MiddleLeft });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "PackageUnitName", HeadText = "包装单位".L10N(), HeadTextAlign = ContentAlignment.MiddleCenter, Width = 200, WidthType = SizeType.Absolute, TextAlign = ContentAlignment.MiddleLeft });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "WoNo", HeadText = "工单号".L10N(), HeadTextAlign = ContentAlignment.MiddleCenter, Width = 200, WidthType = SizeType.Absolute, TextAlign = ContentAlignment.MiddleLeft });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "WoSn", HeadText = "工单条码号".L10N(), HeadTextAlign = ContentAlignment.MiddleCenter, Width = lastColunWidth, WidthType = SizeType.Absolute, TextAlign = ContentAlignment.MiddleLeft });
            lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "ProductName", HeadText = "产品".L10N(), HeadTextAlign = ContentAlignment.MiddleCenter, Width = 200, WidthType = SizeType.Absolute, TextAlign = ContentAlignment.MiddleLeft });
            this.treeGrid1.Columns = lstCulumns;
            this.treeGrid1.IsShowCheckBox = true;
            this.treeGrid1.DataSource = treeGridDataSource;
        }

        /// <summary>
        /// 重置树型Grid数据源
        /// </summary>
        private void ResetTreeGridDataSource()
        {
            TreeGridModel.GenTreeGridDataSource(this.treeGridDataSource, this.packingRelations, this.packageSnRecords, this.packageRules);
            this.treeGrid1.ReloadSource();
            return;
        }

        private string CurrentBarcode = "";

        private void NewPackageForm_Load(object sender, EventArgs e)
        {
            //获取配置项目
            var setting = GetConfig();
            if (setting != null)
            {
                this.Printer = setting.Printer;
                this.PrintMode = PrintMode.Online;// setting.PrintMode;
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

            //刷新当前工序和采集步骤
            new Task(() =>
            {
                this.BeginInvoke(new Action(() =>
                {
                    ReflashProcessStep();
                }));
            }).Start();

            this.xpScanBarcode1.ResetBarcode();

            //根据已选工作单元获取工序信息 如工作单元为空，则弹出工作单元选择
            if (this.xpTitle1.Workcell == null)
            {
                this.xpTitle1.ShowFormChangeWorkCell();
            }
        }

        /// <summary>
        /// 刷新当前工序和采集步骤
        /// </summary>
        private void ReflashProcessStep()
        {
            if (this.xpTitle1.Workcell != null)
            {
                base.CurrentProcess = WipService.GetProcessInfo(this.xpTitle1.Workcell.ProcessId);
                base.Step = new CollectStep(this.xpTitle1.Workcell, this.CurrentProcess);
            }
        }

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

            XPApiResultNewPackage result = NewPackingService.GetCurrentInfo(workcell);
            this.PrintMode = result.PrintMode;
            result.IsChangeOrder = this.CurrentWorkOrder != null && result.WorkOrder != null && this.CurrentWorkOrder.Id != result.WorkOrder.Id;
            this.ResetCurrentWorkOrder(result);
        }

        /// <summary>
        /// 工作单元切换事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpTitle1_AWorkCellChanged(object sender, EventArgs e)
        {
            this.GetCurrentInfo(this.xpTitle1.Workcell);
            this.ReflashProcessStep();
        }


        private void ClearInfos()
        {
            this.xpScanBarcode1.ATips = "请扫描条码".L10N();
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
                ClearInfos();
                bool isPass = true;
                if (PrintMode == PrintMode.InAdvance)
                {
                    isPass = IsNeedPackageNo ? AdvanceInputPackageNo(xpScanBarcode1.ABarcode) : AdvanceInputBarcode(xpScanBarcode1.ABarcode);
                }
                else
                {
                    CurrentBarcode = xpScanBarcode1.ABarcode;
                }
                if (isPass)
                {
                    PackingCollect(this.CurrentBarcode);
                }
            }
            catch (Exception exc)
            {
                this.xpScanBarcode1.ABarcode = "";
                this.xpScanBarcode1.ShowError(exc.Message);
                this.xpMessageList1.AddMessage(exc.Message);
            }
        }

        /// <summary>
        /// 验证是否需要提前输入
        /// </summary>
        /// <returns></returns>
        protected bool AdvanceInputPackageNo(string packageNo)
        {
            var curRuleUnit = AdvancePackingUnit.Peek() ?? throw new ValidationException("单位异常！".L10N());
            NewPackingService.AdvanceInputPackageNo(packageNo, curRuleUnit.Id, curRuleUnit.Name, CurrentWorkOrder.Id);

            advanceBarcodeQueue.Enqueue(packageNo);
            AdvancePackingUnit.Dequeue();
            if (AdvancePackingUnit.Any())
            {
                IsNeedPackageNo = true;
                this.xpScanBarcode1.ShowTips("请扫描[{0}]包装条码".L10nFormat(AdvancePackingUnit.Peek().Name));
                this.xpScanBarcode1.ABarcode = "";
                return false;
            }
            else
            {
                IsNeedPackageNo = false;
                return true;
            }
        }

        /// <summary>
        /// 验证输入条码，预算包装层级
        /// </summary>
        /// <param name="barcode"></param>
        /// <returns></returns>
        protected bool AdvanceInputBarcode(string barcode)
        {
            XPApiResultNewPackage result = NewPackingService.AdvanceInputBarcode(this.CurrentWorkOrder, barcode, Step.CurrentStep.BarcodeType, this.xpTitle1.Workcell);

            if (result.AdvancePackingUnits != null)
            {
                foreach (XPPackingUnit packingUnit in result.AdvancePackingUnits)
                    this.AdvancePackingUnit.Enqueue(packingUnit);
            }
            if (!string.IsNullOrEmpty(result.CurrentBarcode))
            {
                this.CurrentBarcode = result.CurrentBarcode;
            }

            IsNeedPackageNo = false;

            if (this.AdvancePackingUnit.Any())
            {
                this.ResetCurrentWorkOrder(result, false);
                IsNeedPackageNo = true;
                this.xpScanBarcode1.ShowTips("请扫描[{0}]包装条码".L10nFormat(this.AdvancePackingUnit.Peek().Name));
                this.xpScanBarcode1.ABarcode = "";
                return false;
            }
            else
            {
                this.ResetCurrentWorkOrder(result, false);
            }

            return true;
        }

        /// <summary>
        /// 切换工单
        /// </summary>
        /// <param name="result"></param>
        private void ResetCurrentWorkOrder(XPApiResultNewPackage result, bool isClearLocalInfo = true)
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
                this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.ProductCode;
                this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.No;
                this.xpWorkOrder1.ATextBox3Text = this.CurrentWorkOrder?.ProductName;

                //切换包装规则
                this.packageRules.Clear();
                if (result.PackageRules != null)
                {
                    foreach (XPWorkOrderPackageRuleDetail rule in result.PackageRules)
                    {
                        this.packageRules.Add(rule);
                    }
                }

                //切换条码明细
                this.packageSnRecords.Clear();
                if (result.PackageSnRecords != null)
                    this.packageSnRecords.AddRange(result.PackageSnRecords);


                //切换包装关系
                this.packingRelations.Clear();
                if (result.AllRelations != null)
                    this.packingRelations.AddRange(result.AllRelations);

                //刷新条码明细数据显示
                this.ResetTreeGridDataSource();

                return;
            }

            if (!result.IsChangeOrder)
            {
                if (isClearLocalInfo)
                {
                    //切换条码明细
                    this.packageSnRecords.Clear();
                    if (result.PackageSnRecords != null)
                    {
                        this.packageSnRecords.AddRange(result.PackageSnRecords);
                    }

                    //切换包装关系
                    this.packingRelations.Clear();
                    if (result.AllRelations != null)
                    {
                        this.packingRelations.AddRange(result.AllRelations);
                    }
                }

                //刷新条码明细数据显示
                this.ResetTreeGridDataSource();
            }
        }

        /// <summary>
        /// 包装采集
        /// </summary>
        protected void PackingCollect(string barcode)
        {
            if (string.IsNullOrEmpty(Printer))
                throw new ValidationException("打印机不能为空".L10N());

            var result = NewPackingService.PackingCollect(barcode, this.CurrentWorkOrder, this.xpTitle1.Workcell, (int)this.PrintMode, this.advanceBarcodeQueue);
            this.ResetCurrentWorkOrder(result);

            //执行打印
            if (!string.IsNullOrEmpty(result.Sns) && result.PrintRelations != null && result.PrintRelations.Count > 0)
            {
                this.Print(result.PrintRelations);
            }

            this.xpCollectionRecordsGrid1.AddRecord(barcode, BarcodeType.SN);

            // 包装号预输入
            advanceBarcodeQueue.Clear();
            AdvancePackingUnit.Clear();
            string tip = "【{0}:{1}】采集成功".L10nFormat("生产条码".L10N(), barcode);
            this.xpMessageList1.AddMessage(tip);
            this.xpScanBarcode1.ShowTips(tip);
            this.xpScanBarcode1.ResetBarcode();
        }

        /// <summary>
        /// 打印包装
        /// </summary>
        /// <param name="printRelations"></param>
        /// <param name="invOrg"></param>
        /// <exception cref="ValidationException"></exception>
        public void Print(List<XPPackingRelation> printRelations)
        {
            foreach (XPPackingRelation pkg in printRelations.OrderBy(p => p.ItemQty))
            {
                //获取包装SN
                var rule = this.packageRules.FirstOrDefault(f => f.PackageUnitId == pkg.PackageUnitId);
                if (rule.PrintTemplateId == null || rule.PrintTemplateId == 0)
                {
                    throw new ValidationException("找不到对应的包装规则【{0}】的【打印模板】".L10nFormat(rule.PackageUnitName));
                }


                string spath = XpPrinter.Instance.DownloadTempalteWithFileName(rule.PrintTemplateId.Value, LoginInfo.Instance.InvOrgId);
                List<object> printDatas = new List<object>();
                pkg.Customer = this.CurrentWorkOrder.CustomerName;
                pkg.ItemCode = this.CurrentWorkOrder.ProductCode;
                pkg.ItemName = this.CurrentWorkOrder.ProductName;
                printDatas.Add(pkg);
                XpPrinter.Instance.Print(spath, printDatas, this.Printer);

                NewPackingService.UpdateRelationStatePrinted(pkg.Id);

            }
        }

        /// <summary>
        /// 重新加载工位工序未完成的包装关系
        /// </summary>
        public void ReloadPackingRelation()
        {
            if (this.xpTitle1.Workcell == null)
                return;

            var result = NewPackingService.ReloadPackingRelation(this.xpTitle1.Workcell);
            this.packageSnRecords.Clear();
            foreach (XPPackageSnRecord r in result.PackageSnRecords)
                this.packageSnRecords.Add(r);

            this.packingRelations.Clear();
            if (result.AllRelations != null)
                this.packingRelations.AddRange(result.AllRelations);

            //刷新条码明细数据显示
            this.ResetTreeGridDataSource();
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, EventArgs e)
        {
            ReloadPackingRelation();

            //this.packageSnRecords.Clear();
            //this.packingRelations.Clear();

            ////刷新条码明细数据显示
            //this.treeGridDataSource.Clear();
            //this.treeGrid1.ReloadSource();
            this.xpButtonPackage.Enabled = false;
            this.xpButtonPackage.ForeColor = this.xpButtonPackage.Enabled ? System.Drawing.Color.White : System.Drawing.Color.Silver;
            this.CurrentBarcode = "";
            if (this.treeGrid1.HeaderCheckBox != null)
                this.treeGrid1.HeaderCheckBox.Checked = false;
            this.IsNeedPackageNo = false;
            this.AdvancePackingUnit.Clear();
            this.advanceBarcodeQueue.Clear();
            this.xpScanBarcode1.ResetBarcode();
            this.xpScanBarcode1.ShowTips("已重新开始，请扫描条码".L10N());
            this.xpMessageList1.AddMessage("已重新开始，请扫描条码".L10N());
        }

        private void btnESOP_Click(object sender, EventArgs e)
        {
            //string s = System.IO.File.ReadAllText("ddd.txt");
            //this.treeGridDataSource = JsonConvert.DeserializeObject<List<TreeGridModel>>(s);
            //this.treeGrid1.DataSource = this.treeGridDataSource;
            //this.treeGrid1.ReloadSource();
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
                this.xpWorkOrder1.ATextBox1Text = this.CurrentWorkOrder?.ProductCode;
                this.xpWorkOrder1.ATextBox2Text = this.CurrentWorkOrder?.No;
                this.xpWorkOrder1.ATextBox3Text = this.CurrentWorkOrder?.ProductName;
                if (switchWorkOrderForm.WorkOrder != null)
                {
                    this.xpMessageList1.AddMessage(message);
                    this.xpScanBarcode1.ShowWaring(message);
                    this.xpScanBarcode1.ResetBarcode();
                }
            }
        }

        /// <summary>
        /// 获取配置项
        /// </summary>
        /// <returns></returns>
        private NewPackageSetting GetConfig()
        {
            NewPackageSetting setting = null;
            var s = Settings.Default.NewPackageSetting;
            if (!string.IsNullOrEmpty(s))
            {
                setting = JsonConvert.DeserializeObject<NewPackageSetting>(s);
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
            NewPackageSettingForm settingForm = new NewPackageSettingForm();
            DialogResult dialogResult = settingForm.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var setting = GetConfig();
                if (setting != null)
                {
                    this.Printer = setting.Printer;
                    //this.PrintMode = setting.PrintMode;
                    CloseSerial();
                    InitDevicePort(setting);
                }
            }
        }

        /// <summary>
        /// 格式化包装规则数字显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpDataGridViewRule_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null) // 替换 YOUR_COLUMN_INDEX 为你要格式化的列的索引
            {
                if (e.Value is decimal decimalValue)
                {
                    e.Value = decimalValue.ToString("0.##"); // 格式化为不带小数点和小数点后的零的字符串
                    e.FormattingApplied = true;
                }
            }
        }

        /// <summary>
        /// 手动打包事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void xpButtonPackage_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.Printer))
            {
                this.xpScanBarcode1.ShowError("打印机不允许为空".L10N());
                return;
            }

            if (this.treeGrid1.SelectRows.Count <= 0)
            {
                this.xpScanBarcode1.ShowError("请选择要打包的条码明细".L10N());
                return;
            }

            List<XPPackageSnRecord> records = new List<XPPackageSnRecord>();
            foreach (IDataGridViewRow row in this.treeGrid1.SelectRows)
            {
                records.Add(this.treeGridDataSource[row.RowIndex].PackageSnRecord);
            }

            var curRule = this.packageRules.FirstOrDefault(p => p.PackageUnitId == records.FirstOrDefault().PackageUnitId);
            bool isNext = false;
            XPWorkOrderPackageRuleDetail nextRule = null;
            for (int i = 0; i < this.packageRules.Count; i++)
            {
                if (isNext)
                {
                    nextRule = this.packageRules[i];
                    break;
                }
                if (curRule.Id == this.packageRules[i].Id)
                    isNext = true;
            }
            if (nextRule == null)
            {
                this.xpScanBarcode1.ShowError("找不到包装单位[{0}]对应的下一层级".L10nFormat(curRule.PackageUnitName));
                return;
            }

            if (records.Count > nextRule.LevelQty)
            {
                this.xpScanBarcode1.ShowError("最多选择[{0}]进行打包，当前选择的数量[{1}]".L10nFormat(nextRule.LevelQty, records.Count));
                return;
            }

            if (nextRule.LevelQty > records.Count)
            {
                if (MessageBox.Show("外包装最大包装数为[{0}]，当前选择包装数为[{1}]，是否生成未满层级包装？".L10nFormat(nextRule.LevelQty, records.Count), "赛意SMOM".L10N(), MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            try
            {
                var result = NewPackingService.PackageMuanual(this.xpTitle1.Workcell, records);

                if (!string.IsNullOrEmpty(result.MuanualPackageNo) && result.PrintRelations != null && result.PrintRelations.Count > 0)
                {
                    this.Print(result.PrintRelations); //打印
                }

                //刷新条码明细
                this.packageSnRecords.Clear();
                if (result.PackageSnRecords != null)
                {
                    foreach (XPPackageSnRecord record in result.PackageSnRecords)
                        this.packageSnRecords.Add(record);
                }

                //切换包装关系
                this.packingRelations.Clear();
                if (result.AllRelations != null)
                    this.packingRelations.AddRange(result.AllRelations);

                this.ResetTreeGridDataSource();

                foreach (UCDataGridViewTreeRow row in this.treeGrid1.Rows)
                    row.IsChecked = false;

                this.xpScanBarcode1.ShowTips("手动打包成功，生成包装号【{0}】".L10nFormat(result.MuanualPackageNo));
            }
            catch (Exception ex)
            {
                this.xpScanBarcode1.ShowError(ex.Message);
                this.xpMessageList1.AddMessage(ex.Message);
            }
        }

        private void treeGrid1_ARowCheckedChanged(object sender, DataGridViewEventArgs e)
        {
            List<XPPackageSnRecord> records = new List<XPPackageSnRecord>();
            foreach (IDataGridViewRow row in this.treeGrid1.SelectRows)
            {
                records.Add(this.treeGridDataSource[row.RowIndex].PackageSnRecord);
            }

            this.xpButtonPackage.Enabled = CanExcute(records);// records.Count > 0 && records.Select(p => p.WoNo).Distinct().Count() <= 1;
            this.xpButtonPackage.ForeColor = this.xpButtonPackage.Enabled ? System.Drawing.Color.White : System.Drawing.Color.Silver;
        }

        /// <summary>
        /// 判断是否可以打包
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private bool CanExcute(List<XPPackageSnRecord> records)
        {
            if (records.Count > 1)
            {
                var relation = records.FirstOrDefault();
                var result = !records.Exists(p => p.PackageUnitId != relation.PackageUnitId) && !records.Exists(p => p.WorkOrderId != relation.WorkOrderId);
                return result;
            }

            return records.Count == 1;

        }

        private void xpWorkOrder1_AMoreInfoClick(object sender, EventArgs e)
        {
            XPFormWorkOrderDetail.ShowInfo(this.xpWorkOrder1.Parent, this.xpWorkOrder1, this.CurrentWorkOrder);
        }

        private void xpTabControlHeader1_ASelectIndexChanged(object sender, EventArgs e)
        {
            this.xpScanBarcode1.FocusTextBox();
        }
    }
}
