using SIE.Barcodes;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Common.Prints;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Moves;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Tech.Stations;
using SIE.Threading;
using SIE.Wpf.Common.Prints;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SIE.Wpf.MES.WIP.Moves
{
    /// <summary>
    /// 过站采集,特殊的装配采集,使用装配采集的控制器,要求工序BOM为空
    /// </summary>
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [EntityWithConfig(typeof(PrintLabelConfig))]
    [EntityWithConfig(typeof(PanelBindingSnConfig))]
    [RootEntity]
    [Label("过站采集")]
    public class MoveViewModel : DataCollectionViewModel<AssemblyController>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MoveViewModel()
        {
            InitWorkstation(ProcessType.Assembly);
        }

        /// <summary>
        /// 条码采集
        /// </summary>
        /// <param name="sn">条码</param>
        private void PrintLabel(string sn)
        {
            var workcell = GetWorkcell();
            var station = RF.GetById<Station>(workcell.StationId);
            var print = ConfigService.GetConfig(new PrintLabelConfig(), typeof(MoveViewModel), station);
            if (!print.IsPrint)
            {
                return;
            }
            if (print.Printer.IsNullOrEmpty())
            {
                throw new ValidationException("打印机不能为空".L10N());
            }
            try
            {
                var barcode = RT.Service.Resolve<BarcodeController>().GetBarcode(sn);
                var template = barcode.WorkOrder.Template?.PackingTemplate;
                if (template == null)
                {
                    throw new ValidationException("外标签打印模板不能为空".L10N());
                }
                EntityList<Barcode> barcodes = new EntityList<Barcode>();
                barcodes.Add(barcode);
                RT.Service.Resolve<BarcodeController>().Reprint(barcodes, BarcodeLogType.OutBox, "打印外标签", 1);
                var filePath = RT.Service.Resolve<PrintsController>().DownloadPrintTemplate(template.Id);
                var printable = new BarcodePrintable();
                var report = ReportFactory.Current.GetReportByExtension(template.Type);
#pragma warning disable CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
                DoPrint(print, barcodes, filePath, printable, report);
#pragma warning restore CS4014 // 由于此调用不会等待，因此在调用完成前将继续执行当前方法
            }
            catch (Exception exc)
            {
                ShowError(exc.Message);
            }
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="print"></param>
        /// <param name="barcodes"></param>
        /// <param name="filePath"></param>
        /// <param name="printable"></param>
        /// <param name="report"></param>
        /// <returns></returns>
        private async Task DoPrint(PrintLabelConfigValue print, EntityList<Barcode> barcodes, string filePath, BarcodePrintable printable, IReport report)
        {
            await Task.Run(new Action(() =>
            {
                report.Print(printable, filePath, print.Printer, () =>
                {
                    return barcodes;
                }, () =>
                {
                });
            }).WithCurrentThreadContext()).ConfigureAwait(true);
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
                ClearInfos();

                var workcell = GetWorkcell();

                MoveCollect(Barcode, workcell);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
            finally
            {
                Barcode = null;
            }
        }

        /// <summary>
        /// 过站采集
        /// </summary>
        /// <param name="barcode">1</param>
        /// <param name="workcell">2</param>
        protected virtual void MoveCollect(string barcode, Workcell workcell)
        {
            var currentStep = Step.CurrentStep;
            var collectBarcode = new CollectBarcode { Code = barcode, Type = currentStep.BarcodeType };
            try
            {
                if (PanelInfo.BindingMode == BindingMode.Manual && PanelInfo.NeetToBindingSn)
                {
                    var snQty = Controller.ValidateNewBarcode(barcode, WorkOrderId ?? 0);
                    PanelInfo.SnList.Add(new SnData() { Sn = barcode, Qty = snQty });
                }
                else
                {
                    if (Step.StepIndex == 0)
                    {
                        var info = Validate(collectBarcode, workcell);
                        MergeData(info);
                    }
                    if (Step.StepIndex != 0)
                    {
                        Controller.ValidateBarcode(collectBarcode, workcell);
                    }
                    Step.Barcodes.Add(barcode);
                }
                if (!Step.NextStep())
                {
                    Collect(workcell, collectBarcode);
                }
                else
                {
                    ShowTips("[{0}]扫描采集成功，请扫描[{1}]".L10nFormat(collectBarcode, (Step.ProcessSteps.ToList()[Step.StepIndex]).BarcodeType.ToLabel()));
                }
            }
            catch (Exception exc)
            {
                ShowError(exc);
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
                if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish
                    && this.HaveFailParameter && !this.Qualified)
                {
                    if (!InputDefect())
                    {
                        //重新开始
                        this.Reset(resetType: ResetType.Error);

                        ShowError("选择【不合格】没有录入缺陷，过站失败，请切换为【合格】或扫描条码后录入缺陷代码再过站".L10N());

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
                Controller.Collect(barcodes, collectData, workcell);
                PrintBindingSn();
                RefreshStatistics();

                //清除已选择的缺陷
                this.DefectItemList.Clear();

                AddDetail(collectBarcode, collectData.Result);

                PanelInfo.Clear();

                if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish)
                {
                    ShowTips("[{0}]过站成功".L10nFormat(new CollectBarcode
                    {
                        Code = barcodes.LastOrDefault(),//barcodes[0],
                        Type = Step.CurrentStep.BarcodeType
                    }));
                }
                else
                {
                    ShowTips("[{0}]入站成功".L10nFormat(new CollectBarcode
                    {
                        Code = barcodes.LastOrDefault(),//barcodes[0],
                        Type = Step.CurrentStep.BarcodeType
                    }));
                }

                PrintLabel(barcodes[0]);
                RefrshReportTasks();
                Step.Reset();
            }
            catch (Exception exc)
            {
                var baseExc = exc.GetBaseException();
                if (baseExc is UnBindingSnException)
                {
                    PanelInfo.BindingMode = BindingMode.Manual;
                    ShowTips("拼板码[{0}]可绑定{1}个产品，请扫描待绑定的第{2}个产品条码".L10nFormat(PanelInfo.PanelCode, PanelInfo.NeetToBindingQty, PanelInfo.SnList.Count + 1));
                }
                else
                {
                    //回滚一步
                    Step.Roolback();
                    ShowError(exc);
                }
            }
        }
    }
}
