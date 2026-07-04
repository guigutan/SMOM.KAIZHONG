using DevExpress.Xpf.Editors;
using SIE.Barcodes.WipBatchs;
using SIE.Common.Configs;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Core.Prints;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Dispatchs;
using SIE.KZ.Print;
using SIE.Logging;
using SIE.ManagedProperty;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MES.TaskManagement.PreStartupSetupRecords;
using SIE.MES.TaskManagement.ProcessPrepareRecords;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Pressure;
using SIE.MES.WIP.Pressure.Configs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Wpf.Common.Prints;
using SIE.Wpf.Controls.WaitProgress;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Threading;

namespace SIE.Wpf.MES.WIP.Pressure
{
    /// <summary>
    /// 耐压采集
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(WipPressureSnConfig))]
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [Label("耐压采集")]
    public class PressureViewModel : KZDataCollectionViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PressureViewModel()
        {
            InitWorkstation();

            Printer = SIE.Common.Properties.Settings.Default.PrinterName;
        }

        /// <summary>
        /// SN明细列表视图
        /// </summary>
        public LogicalView SnDetailListView { get; set; }


        #region 生产批次 WipBatch
        /// <summary>
        /// 生产批次Id
        /// </summary>
        [Label("生产批次")]
        public static readonly IRefIdProperty WipBatchIdProperty =
            P<PressureViewModel>.RegisterRefId(e => e.WipBatchId, ReferenceType.Normal);

        /// <summary>
        /// 生产批次Id
        /// </summary>
        public double? WipBatchId
        {
            get { return (double?)this.GetRefNullableId(WipBatchIdProperty); }
            set { this.SetRefNullableId(WipBatchIdProperty, value); }
        }

        /// <summary>
        /// 生产批次
        /// </summary>
        public static readonly RefEntityProperty<WipBatch> WipBatchProperty =
            P<PressureViewModel>.RegisterRef(e => e.WipBatch, WipBatchIdProperty);

        /// <summary>
        /// 生产批次
        /// </summary>
        public WipBatch WipBatch
        {
            get { return this.GetRefEntity(WipBatchProperty); }
            set { this.SetRefEntity(WipBatchProperty, value); }
        }
        #endregion

        #region 生产资源 Resource
        /// <summary>
        /// 生产资源Id
        /// </summary>
        [Label("生产资源")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<PressureViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 生产资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 生产资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<PressureViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 生产资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 耐压测试批次 WipPressure
        /// <summary>
        /// 耐压测试批次Id
        /// </summary>
        [Label("耐压测试批次")]
        public static readonly IRefIdProperty WipPressureIdProperty =
            P<PressureViewModel>.RegisterRefId(e => e.WipPressureId, ReferenceType.Normal);

        /// <summary>
        /// 耐压测试批次Id
        /// </summary>
        public double? WipPressureId
        {
            get { return (double?)this.GetRefNullableId(WipPressureIdProperty); }
            set { this.SetRefNullableId(WipPressureIdProperty, value); }
        }

        /// <summary>
        /// 耐压测试批次
        /// </summary>
        public static readonly RefEntityProperty<WipPressure> WipPressureProperty =
            P<PressureViewModel>.RegisterRef(e => e.WipPressure, WipPressureIdProperty);

        /// <summary>
        /// 耐压测试批次
        /// </summary>
        public WipPressure WipPressure
        {
            get { return this.GetRefEntity(WipPressureProperty); }
            set { this.SetRefEntity(WipPressureProperty, value); }
        }
        #endregion

        #region 条码规则 NumberRule 
        /// <summary>
        /// 条码规则ID
        /// </summary>
        [Label("条码规则")]
        public static readonly IRefIdProperty NumberRuleIdProperty = P<PressureViewModel>.RegisterRefId(e => e.NumberRuleId, ReferenceType.Normal);

        /// <summary>
        /// 条码规则ID
        /// </summary>
        public double? NumberRuleId
        {
            get { return (double?)this.GetRefNullableId(NumberRuleIdProperty); }
            set { this.SetRefNullableId(NumberRuleIdProperty, value); }
        }

        /// <summary>
        /// 条码规则
        /// </summary>
        public static readonly RefEntityProperty<NumberRule> NumberRuleProperty = P<PressureViewModel>.RegisterRef(e => e.NumberRule, NumberRuleIdProperty);

        /// <summary>
        /// 条码规则
        /// </summary>
        public NumberRule NumberRule
        {
            get { return this.GetRefEntity(NumberRuleProperty); }
            set { this.SetRefEntity(NumberRuleProperty, value); }
        }
        #endregion

        #region 打印机 Printer 
        /// <summary>
        /// 打印机
        /// </summary>
        [Label("打印机")]
        public static readonly Property<string> PrinterProperty = P<PressureViewModel>.Register(e => e.Printer);

        /// <summary>
        /// 打印机
        /// </summary>
        public string Printer
        {
            get { return this.GetProperty(PrinterProperty); }
            set { this.SetProperty(PrinterProperty, value); }
        }
        #endregion

        #region 模板 Template 
        /// <summary>
        /// 模板Id
        /// </summary>
        [Label("打印模板")]
        public static readonly IRefIdProperty TemplateIdProperty = P<PressureViewModel>.RegisterRefId(e => e.TemplateId, ReferenceType.Normal);

        /// <summary>
        /// 模板Id
        /// </summary>
        public double? TemplateId
        {
            get { return (double)this.GetRefNullableId(TemplateIdProperty); }
            set { this.SetRefNullableId(TemplateIdProperty, value); }
        }

        /// <summary>
        /// 模板
        /// </summary>
        public static readonly RefEntityProperty<PrintTemplate> TemplateProperty = P<PressureViewModel>.RegisterRef(e => e.Template, TemplateIdProperty);

        /// <summary>
        /// 模板
        /// </summary>
        public PrintTemplate Template
        {
            get { return this.GetRefEntity(TemplateProperty); }
            set { this.SetRefEntity(TemplateProperty, value); }
        }
        #endregion

        #region 打印设置 PrinterSettingTpl
        /// <summary>
        /// 打印设置Id
        /// </summary>
        [Label("打印设置")]
        public static readonly IRefIdProperty PrinterSettingTplIdProperty =
            P<PressureViewModel>.RegisterRefId(e => e.PrinterSettingTplId, ReferenceType.Normal);

        /// <summary>
        /// 打印设置Id
        /// </summary>
        public double? PrinterSettingTplId
        {
            get { return (double?)this.GetRefNullableId(PrinterSettingTplIdProperty); }
            set { this.SetRefNullableId(PrinterSettingTplIdProperty, value); }
        }

        /// <summary>
        /// 打印设置
        /// </summary>
        public static readonly RefEntityProperty<PrinterSettingTpl> PrinterSettingTplProperty =
            P<PressureViewModel>.RegisterRef(e => e.PrinterSettingTpl, PrinterSettingTplIdProperty);

        /// <summary>
        /// 打印设置
        /// </summary>
        public PrinterSettingTpl PrinterSettingTpl
        {
            get { return this.GetRefEntity(PrinterSettingTplProperty); }
            set { this.SetRefEntity(PrinterSettingTplProperty, value); }
        }
        #endregion


        #region 超打验证码 VerifyCode
        /// <summary>
        /// 超打验证码
        /// </summary>
        [Label("超打验证码")]
        public static readonly Property<string> VerifyCodeProperty = P<PressureViewModel>.Register(e => e.VerifyCode);

        /// <summary>
        /// 超打验证码
        /// </summary>
        public string VerifyCode
        {
            get { return this.GetProperty(VerifyCodeProperty); }
            set { this.SetProperty(VerifyCodeProperty, value); }
        }
        #endregion


        #region SN明细 SnDetailList
        /// <summary>
        /// SN明细
        /// </summary>
        [Label("SN明细")]
        public static readonly ListProperty<EntityList<WipPressureSn>> SnDetailListProperty = P<PressureViewModel>.RegisterList(e => e.SnDetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as PressureViewModel).LoadSnDetailList()
        });

        /// <summary>
        /// SN明细
        /// </summary>
        public EntityList<WipPressureSn> SnDetailList
        {
            get { return this.GetLazyList(SnDetailListProperty); }
        }

        /// <summary>
        /// 
        /// </summary>
        private EntityList<WipPressureSn> LoadSnDetailList()
        {
            return new EntityList<WipPressureSn>();
        }
        #endregion

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="resetType"></param>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);

            Resource = Workstation.Resource;
            WipBatch = null;
            WorkOrder = null;
            SnDetailList.Clear();
            //var config = ConfigService.GetConfig(new WipPressureSnConfig(), typeof(PressureViewModel));
            //NumberRule = config.BacodeRule;
            NumberRule = null;
            Template = null;
            PrinterSettingTpl = null;
            if (Printer.IsNullOrEmpty())
                Printer = SIE.Common.Properties.Settings.Default.PrinterName;
            if (Resource == null)
                ShowTips("请扫描资源编码");
            else if (WipBatch == null)
                ShowTips("请扫描工序标签");

            InitDevicePort();

        }

        /// <summary>
        /// 清空
        /// </summary>
        protected override void ClearInfos()
        {
            base.ClearInfos();

        }

        /// <summary>
        /// 采集验证
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        protected bool ValidateCollect()
        {
            //if (NumberRule == null)
            //{
            //    ShowError("请配置SN编码规则"); return false;
            //}

            if (Printer.IsNullOrEmpty())
            {
                ShowError("请选择打印机"); return false;
            }

            return true;
        }
        /// <summary>
        /// 条码扫描后处理逻辑
        /// </summary>
        /// <param name="e">1</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty())
            {
                return;
            }
            try
            {
                using (var timeWacther = new TimeWacther("耐压测试过站"))
                {
                    timeWacther.StartMark("条码是：{0}".FormatArgs(Barcode));
                    ClearInfos();
                    timeWacther.Mark("ClearInfos");
                    if (!ValidateCollect())
                        return;
                    //扫描资源
                    if (Resource == null)
                    {
                        Resource = RT.Service.Resolve<WipResourceController>().GetWipResourceByCode(Barcode);
                        if (Resource == null)
                            ShowError("请扫描正确的资源编码".L10nFormat(Barcode));
                        else
                            ShowTips("请扫描工序标签");
                        Workstation.Resource = Resource;
                        timeWacther.Mark("GetWipResourceByCode");
                        return;
                    }
                    //扫描工序标签
                    if (WipBatch == null)
                    {
                        ScanWipBatch(Barcode);
                        timeWacther.Mark("ScanWipBatch");
                        return;
                    }

                    //扫描或者监听测试数据, 生成SN数据
                    var snList = RT.Service.Resolve<WipPressureController>().GenerateWipPressureSns(Barcode, WipPressure, NumberRule.Id, Resource);
                    timeWacther.Mark("GenerateWipPressureSns");
                    if (snList.Count > 0)
                    {
                        ShowTips("已生成SN[{0}]".L10nFormat(snList.Select(p => p.Sn).Concat(",")));
                        snList.ForEach(sn =>
                        {
                            //刷新SN列表数据
                            SnDetailList.Insert(0, sn);
                            SnDetailListView.Current = sn;
                        });
                        timeWacther.Mark("调用打印 PrintSn 开始");
                        //打印SN标签
                        PrintSn(snList, timeWacther);
                        timeWacther.Mark("调用打印 PrintSn 结束");
                        if (WipPressure != null && WipBatch != null)
                        {
                            if (!IsCanContinue(WipPressure))
                            {
                                timeWacther.Mark("IsCanContinue");
                                return;
                            }
                        }
                    }
                    else
                    {
                        ShowError("测试数据[{0}]解析失败".L10nFormat(Barcode));
                    }

                    ShowTips("请进行耐压工序测试");
                }
            }
            catch (Exception exc)
            {
                ShowError(exc.GetExceptionMessage());
            }
            finally
            {
                Barcode = null;
            }
        }

        /// <summary>
        /// 扫描工序标签
        /// </summary>
        /// <param name="barcode"></param>
        protected void ScanWipBatch(string barcode)
        {
            var wipBatch = RT.Service.Resolve<WipBatchController>().GetWipBatch(barcode);
            if (wipBatch == null)
            {
                ShowError("请扫描正确的工序标签".L10nFormat(barcode));
                return;
            }

            WorkOrder = RF.GetById<WorkOrder>(wipBatch.WorkOrderId);
            var wipPressureTpl = RT.Service.Resolve<WipPressureController>().GetWipPressurePrintTemplate(WorkOrder.ProductId);
            NumberRule = wipPressureTpl?.NumberRule;
            Template = wipPressureTpl?.PrintTemplate;
            PrinterSettingTpl = RT.Service.Resolve<PrinterSettingController>().GetPrinterSettingTpl(RT.IdentityId, wipBatch.ProductCode);
            if (NumberRule == null || Template == null)
            {
                ShowError("产品[{0}]还未维护对应的SN编码规则与打印模板,请检查".L10nFormat(WorkOrder.Product?.Code));
                return;
            }

            var wipPressure = RT.Service.Resolve<WipPressureController>().GetWipPressure(wipBatch.BatchNo);
            if (wipPressure != null)
            {
                WipBatch = wipBatch;
                if (!IsCanContinue(wipPressure))
                    return;
            }
            else
            {
                //校验前置工序报工
                var task = RT.Service.Resolve<ITaskReportKZ>().ValidatePrepareProcessHasReport(wipBatch.BatchNo, "电性能测试,耐压测试");

                if (task != null && task is DispatchTask dispatchTask)
                {
                    //校验产前准备
                    RT.Service.Resolve<ProcessPrepareRecordsController>().ValidateProcessPrepare(dispatchTask);
                    //校验开机准备
                    RT.Service.Resolve<PreStartupSetupRecordsController>().ValidateStartupSetupPrepare(dispatchTask);
                }
            }

            WipPressure = wipPressure ?? GetOrCreateWipPressure(wipBatch);
            WipBatch = wipBatch;

            SnDetailList.Clear();
            SnDetailList.AddRange(WipPressure.WipPressureSnList.OrderByDescending(p => p.TestTime));

            ShowTips("请进行耐压工序测试");
        }

        /// <summary>
        /// 创建或获取批次数据
        /// </summary>
        /// <param name="wipBatch"></param>
        /// <returns></returns>
        protected WipPressure GetOrCreateWipPressure(WipBatch wipBatch)
        {
            var wipPressure = RT.Service.Resolve<WipPressureController>().GetWipPressure(wipBatch.BatchNo);
            if (wipPressure == null)
            {
                wipPressure = new WipPressure()
                {
                    WorkOrder = WorkOrder,
                    Resource = Resource,
                    BatchNo = wipBatch.BatchNo,
                    Product = WorkOrder?.Product,
                    Qty = wipBatch.Qty,
                    OriginalQty = wipBatch.Qty
                };
                RF.Save(wipPressure);
            }

            return wipPressure;
        }

        /// <summary>
        /// 是否可以继续测试
        /// </summary>
        /// <returns></returns>
        protected bool IsCanContinue(WipPressure wipPressure)
        {
            var snQty = RT.Service.Resolve<WipPressureController>().GetWipPressureSnQty(wipPressure.Id);    //已采集数量
            if (snQty < wipPressure.OriginalQty)
                return true;

            var overQty = RT.Service.Resolve<WipPressureController>().GetMaxPrintCount(wipPressure.OriginalQty);    //最大超打数量
            if (snQty >= overQty)
            {
                ShowError("工序标签[{0}]已达到最大可测试数量,请扫描其他工序标签".L10nFormat(WipBatch.BatchNo));
                WipBatch = null;
                return false;
            }

            if (wipPressure.IsAllowOver)
                return true;

            if (CRT.MessageService.AskQuestion("工序标签[{0}]已完成所有的测试数量, 是否需要继续测试".L10nFormat(WipBatch.BatchNo), "确认"))
            {
                VerifyCode = null;
                var template = new DetailsUITemplate(typeof(PressureViewModel), PressureViewModelViewConfig.VerifyCodeView);
                var ui = template.CreateUI();
                ui.MainView.Data = this;
                var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
                {
                    w.Title = "超打验证码".L10N();
                    w.Height = 200;
                    w.Width = 400;
                    w.Closing += (s, e) =>
                    {
                        if (w.Result == 0)
                        {
                            if (RT.Service.Resolve<WipPressureController>().VerifyCode(wipPressure, VerifyCode))
                            {
                                wipPressure.IsAllowOver = true;
                                Error = null;
                            }
                            else
                            {
                                CRT.MessageService.ShowError("验证码不正确!");
                                e.Cancel = true;
                            }
                        }
                    };
                });

                if (result == 0 && wipPressure.IsAllowOver == true)
                    return true;

            }

            ShowError("工序标签[{0}]已完成所有的测试数量,请扫描其他工序标签".L10nFormat(WipBatch.BatchNo));
            WipBatch = null;
            return false;
        }

        /// <summary>
        /// 打印SN标签
        /// </summary>
        /// <param name="snList"></param>
        public virtual void PrintSn(EntityList<WipPressureSn> snList, TimeWacther timeWacther = null)
        {
            ClearInfos();
            if (Template == null)
            {
                ShowError("当前产品未配置打印模板"); return;
            }
            if (Printer.IsNullOrEmpty())
            {
                ShowError("请选择打印机"); return;
            }
            if (snList.Count == 0)
                return;
            var snStr = snList.Select(p => p.Sn).Concat(",");
            if (snList.Count > 3)
                snStr = snList.Take(3).Select(p => p.Sn).Concat(",") + "...";
            ShowTips("正在打印SN[{0}]...".L10nFormat(snStr));

            Exception exception = null;
            var win = new WaitDialog();
            win.Width = 300;
            win.WindowStyle = System.Windows.WindowStyle.None;
            win.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            //win.Topmost = true;
            win.Owner = CRT.Workbench.MainWindow as System.Windows.Window;
            win.GetLogicalChild<ProgressBarEdit>().StyleSettings = new ProgressBarMarqueeStyleSettings();
            win.ShowInTaskbar = false;
            win.Text = "正在打印...".L10N();
            ThreadPool.GetMinThreads(out int workerMin, out int ioMin);
            ThreadPool.GetMaxThreads(out int workerMax, out int ioMax);
            timeWacther?.Mark("开始准备进入异步线程，条码：{0},Min: {1}, Max: {2}".FormatArgs(snStr, workerMin, workerMax));
            ThreadPool.QueueUserWorkItem(oo =>
            {
                try
                {
                    timeWacther?.Mark($"开始异步线程打印");
                    if (PrinterSettingTpl != null)
                    {
                        ////客户料码数据
                        //var itemCustomer = RT.Service.Resolve<ItemCusotmerDataController>().GetItemCusotmerData(WipPressure.ProductId.Value, batchNo: WipPressure.BatchNo, lineCode: Resource?.Code);
                        ////GDI+打印
                        //var snList = new List<string>() { sn.Sn };
                        //PrintSn(snList, itemCustomer?.ProjectName);

                        //模板转图片打印
                        var template = RF.GetById<PrintTemplate>(Template.Id);
                        IPrintable printable = new WipPressureSnPrintable();
                        var report = HostReportFactory.Current.GetReportByExtension(Template.Type);
                        var datas = new List<byte[]>();
                        foreach (var sn in snList)
                        {
                            timeWacther?.Mark("模板转换图片，条码：{0} 开始".FormatArgs(sn));
                            var bytes = report.ExportToImage(printable, template.Content, Printer, () =>
                            {
                                return new List<WipPressureSn>() { sn };
                            },
                            PrinterSettingTpl.Resolution
                            );
                            datas.Add(bytes);
                            timeWacther?.Mark("模板转换图片，条码：{0} 结束".FormatArgs(sn));
                        }
                        PrintBytes(datas);
                        timeWacther?.Mark("图片打印到打印机 结束");
                        //模板打印
                        //report.Print(printable, template.Content, Printer, () =>
                        //{
                        //    return new List<WipPressureSn>() { sn };
                        //}, () => { }, 1,
                        //PrinterSettingTpl.MarginsLeft,
                        //PrinterSettingTpl.MarginsTop,
                        //PrinterSettingTpl.MarginsRight,
                        //PrinterSettingTpl.MarginsBottom
                        //);
                    }
                    else
                    {
                        timeWacther?.Mark("标品模板打印 准备");
                        //标品模板打印
                        var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(TemplateId.Value);
                        IPrintable printable = new WipPressureSnPrintable();
                        var report = ReportFactory.Current.GetReportByExtension(Template.Type);
                        timeWacther?.Mark("标品模板打印 开始");
                        report.Print(printable, filePath, Printer, () =>
                        {
                            return snList;
                        }, () => { });
                        timeWacther?.Mark("标品模板打印 结束");
                    }
                }
                catch (Exception exc)
                {
                    exception = exc;
                }

                Action ac = () => win.DialogResult = true;
                win.Dispatcher.BeginInvoke(ac);
            });

            win.ShowDialog();
            if (exception != null)
            {
                ShowError(exception.GetExceptionMessage());
                exception.Alert();
            }
            else
                ShowTips("SN打印完成".L10nFormat());

        }

        /// <summary>
        /// 字节图像打印
        /// </summary>
        /// <param name="imageBytes"></param>
        public virtual void PrintBytes(List<byte[]> imageBytes)
        {
            if (PrinterSettingTpl == null)
            {
                ShowError("当前产品未配置打印模板"); return;
            }
            if (Printer.IsNullOrEmpty())
            {
                ShowError("请选择打印机"); return;
            }
            if (imageBytes.Count == 0)
            {
                ShowError("打印数据为空,请检查"); return;
            }

            var setting = PrinterSettingTpl;

            using (PrintDocument printDoc = new PrintDocument())
            {
                printDoc.PrinterSettings.Copies = printDoc.PrinterSettings.Copies;
                //printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", setting.PageWidth, setting.PageHeight); //纸张大小  像素=(毫米数/25.4)*显示器DPI
                //printDoc.DefaultPageSettings.Margins = new Margins(setting.MarginsLeft, setting.MarginsRight, setting.MarginsTop, setting.MarginsBottom);

                printDoc.PrintPage += (s, e) =>
                {
                    if (imageBytes.Count > 0)
                    {
                        var imageByte = imageBytes[0];
                        imageBytes.RemoveAt(0);

                        e.Graphics.Clear(Color.White);

                        Brush brush = new SolidBrush(Color.Black);
                        //绘制模版图片
                        using (MemoryStream ms = new MemoryStream(imageByte))
                        {
                            Image image = Image.FromStream(ms);
                            //e.Graphics.DrawImage(image, setting.MarginsLeft, setting.MarginsTop, setting.PageWidth, setting.PageHeight);
                            Rectangle destRect = new Rectangle(setting.MarginsLeft, setting.MarginsTop, setting.PageWidth, setting.PageHeight);
                            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                            e.Graphics.DrawImage(image, destRect);

                        }
                        e.HasMorePages = imageBytes.Count > 0;
                    }
                    else
                    {
                        e.HasMorePages = false;
                    }
                };

                printDoc.PrinterSettings.PrinterName = Printer;//打印机名称
                printDoc.DocumentName = "标签打印";//文档名称

                printDoc.Print(); //打印               

            }
        }

        /// <summary>
        /// 系统驱动打印
        /// </summary>
        /// <param name="snList">二维码字符</param>
        /// <param name="projectName">项目规格</param>
        public virtual void PrintSn(List<string> snList, string projectName)
        {
            if (PrinterSettingTpl == null)
            {
                ShowError("当前产品未配置打印模板"); return;
            }
            if (Printer.IsNullOrEmpty())
            {
                ShowError("请选择打印机"); return;
            }
            if (!snList.Any())
            {
                ShowError("打印数据为空,请检查"); return;
            }

            var PrintSettingInfo = PrinterSettingTpl;

            using (PrintDocument printDoc = new PrintDocument())
            {
                printDoc.PrinterSettings.Copies = printDoc.PrinterSettings.Copies;
                printDoc.DefaultPageSettings.PaperSize = new PaperSize("Custom", PrintSettingInfo.PageWidth, PrintSettingInfo.PageHeight); //纸张大小  像素=(毫米数/25.4)*显示器DPI
                printDoc.DefaultPageSettings.Margins = new Margins(PrintSettingInfo.MarginsLeft, PrintSettingInfo.MarginsRight, PrintSettingInfo.MarginsTop, PrintSettingInfo.MarginsBottom);
                //printDoc.DefaultPageSettings.Margins = new Margins(20,20,20,20);
                //PrintSettingInfo.QrcodeX = 10;
                //PrintSettingInfo.QrcodeY = 5;
                printDoc.PrintPage += (s, e) =>
                {
                    if (snList.Count > 0)
                    {
                        var sn = snList[0];
                        snList.RemoveAt(0);

                        e.Graphics.Clear(Color.White);

                        Brush brush = new SolidBrush(Color.Black);
                        //二维码图片
                        System.Drawing.Image img = GenerateQRCode(sn, 2, 0);
                        e.Graphics.DrawImage(img, PrintSettingInfo.QrcodeX, PrintSettingInfo.QrcodeY, PrintSettingInfo.QrcodeWidth, PrintSettingInfo.QrcodeHeight);

                        //二维码字符                      
                        Font font_BydCodeStr = new Font(PrintSettingInfo.CodeStrFontName, (float)PrintSettingInfo.CodeStrFontSize, PrintSettingInfo.CodeStrFontBold ? FontStyle.Bold : FontStyle.Regular);
                        List<string> listcode = SplitByLength(sn, PrintSettingInfo.CodeStrLineSize);
                        for (int i = 0; i < listcode.Count; i++)
                        {
                            e.Graphics.DrawString(listcode[i], font_BydCodeStr, brush, new Point(PrintSettingInfo.CodeStrX, PrintSettingInfo.CodeStrY + (PrintSettingInfo.CodeStrLineHeight * i)));
                        }

                        //项目名称（产品名称）==示例图中的P94
                        Font font_ProjectName = new Font(PrintSettingInfo.CodeStrFontName, (float)PrintSettingInfo.ProjectFontSize, PrintSettingInfo.ProjectFontBold ? FontStyle.Bold : FontStyle.Regular);
                        e.Graphics.DrawString(projectName, font_ProjectName, brush, new System.Drawing.Point(PrintSettingInfo.ProjectNameX, PrintSettingInfo.ProjectNameY));

                        e.HasMorePages = snList.Count > 0;
                    }
                    else
                    {
                        e.HasMorePages = false;
                    }

                };

                printDoc.PrinterSettings.PrinterName = Printer;//打印机名称
                printDoc.DocumentName = "标签打印";//文档名称

                printDoc.Print(); //打印               

            }
        }

        /// <summary>
        /// 生成二维码图像
        /// </summary>
        /// <param name="text"></param>
        /// <param name="qrCodeScale"></param>
        /// <param name="qrCodeVersion"></param>
        /// <returns></returns>
        public virtual Image GenerateQRCode(string text, int qrCodeScale = 2, int qrCodeVersion = 0)
        {
            return null;
            //var qrCode = new QRCodeEncoder()
            //{
            //    QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE,
            //    QRCodeScale = qrCodeScale,
            //    QRCodeVersion = qrCodeVersion,
            //    QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M
            //};
            //var image = qrCode.Encode(text, Encoding.UTF8);
            //return image;
        }

        /// <summary>
        /// 将字符串按指定长度拆分成多个子字符串
        /// </summary>
        /// <param name="input">输入的原始字符串</param>
        /// <param name="splitLength">拆分的长度</param>
        /// <returns>拆分后的字符串集合</returns>
        /// <exception cref="ArgumentOutOfRangeException">当拆分长度小于或等于0时抛出</exception>
        public virtual List<string> SplitByLength(string input, int splitLength)
        {
            // 创建结果集合
            List<string> result = new List<string>();

            // 处理空字符串情况
            if (string.IsNullOrEmpty(input))
            {
                return result;
            }

            // 验证拆分长度的有效性
            if (splitLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(splitLength), "拆分长度必须大于0");
            }

            // 计算需要拆分的次数
            int totalParts = (int)Math.Ceiling((double)input.Length / splitLength);

            // 拆分字符串
            for (int i = 0; i < totalParts; i++)
            {
                // 计算当前子字符串的起始索引
                int startIndex = i * splitLength;

                // 计算当前子字符串的长度（最后一段可能不足指定长度）
                int currentLength = Math.Min(splitLength, input.Length - startIndex);

                // 截取子字符串并添加到结果集合
                result.Add(input.Substring(startIndex, currentLength));
            }

            return result;
        }
    }
}
