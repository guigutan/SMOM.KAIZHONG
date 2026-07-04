using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Barcodes;
using SIE.Defects;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Inspects;
using SIE.MES.WIP.Models;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Wpf.MES.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 检验采集ViewModel
    /// </summary>
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [EntityWithConfig(typeof(PanelBindingSnConfig))]
    [RootEntity, Serializable]
    [Label("检验采集")]
    public class InspectViewModel : DataCollectionViewModel<InspectController>
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        public InspectViewModel()
        {
            InitWorkstation(ProcessType.Pqc);
        }

        /// <summary>
        /// 条码扫描后处理逻辑
        /// </summary>
        /// <param name="e">属性变更参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (!Barcode.IsNotEmpty()) return;
            try
            {
                ClearInfos();
                if (QuickToSubmit(Barcode))
                    return;
                var currentStep = Step.CurrentStep;
                var collectBarcode = new CollectBarcode { Code = Barcode, Type = currentStep.BarcodeType };
                var workcell = GetWorkcell();
                if (PanelInfo.BindingMode == BindingMode.Manual && PanelInfo.NeetToBindingSn)
                {
                    var snQty = Controller.ValidateNewBarcode(Barcode, WorkOrderId ?? 0);
                    PanelInfo.SnList.Add(new SnData() { Sn = Barcode, Qty = snQty });
                }
                else
                {
                    if (Step.StepIndex == 0)
                    {
                        var info = Validate(collectBarcode, workcell);
                        MergeData(info);
                        DisplayBarCode = Barcode;
                    }

                    if (Step.StepIndex != 0)
                        Controller.ValidateBarcode(collectBarcode, workcell);

                    Step.Barcodes.Add(collectBarcode.Code);
                }
                if (!Step.NextStep())
                {
                    if (PanelInfo.BindingMode == BindingMode.Manual)
                    {
                        if (!PanelInfo.NeetToBindingSn)
                            ShowTips("[{0}]关联产品条码完成，请提交".L10nFormat(Step.Barcodes.FirstOrDefault()));
                        else
                            ShowTips("拼板码[{0}]可绑定{1}个产品，请扫描待绑定的第{2}个产品条码".L10nFormat(PanelInfo.PanelCode, PanelInfo.NeetToBindingQty, PanelInfo.SnList.Count + 1));
                    }
                    else
                    {
                        if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Start)
                        {
                            //move in
                            Submit();
                        }
                        else
                        {
                            ShowTips("[{0}]扫描成功,请提交".L10nFormat(collectBarcode));
                        }
                    }
                }
                else
                {
                    currentStep = Step.CurrentStep;
                    ShowTips("[{0}]扫描成功，请扫描[{1}]".L10nFormat(collectBarcode, currentStep.BarcodeType.ToLabel()));
                }
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
        /// 快速提交
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <returns>满足快速提交条件返回true，否则返回false</returns>
        private bool QuickToSubmit(string barcode)
        {
            if (!Step.HasNextStep())
            {
                if (PanelInfo.BarcodeType == BarcodeType.CombinedCode && PanelInfo.NeetToBindingSn)
                {
                    return false;
                }
                if (barcode == SIE.Barcodes.Barcode.SubmitCode)
                {
                    Submit();
                    return true;
                }
                else
                {
                    ShowError("上一条码未提交,扫[{0}]提交或者重新开始".L10nFormat(SIE.Barcodes.Barcode.SubmitCode));
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 重新设置界面的内容
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);

            DefectItemList.Clear();
        }

        /// <summary>
        /// 是否能提交
        /// </summary>
        /// <returns>返回是否能提交</returns>
        public virtual bool CanSubmit()
        {
            return !Step.HasNextStep()
                && Workstation.EmployeeId.HasValue
                && Workstation.ProcessId.HasValue
                && Workstation.StationId.HasValue
                && Workstation.ResourceId.HasValue;
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

                if (collectData.State == SIE.MES.WIP.Products.WipProductProcessState.Finish)
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
                            Qty = defectItem.Qty
                        });
                    }
                }

                InitCombinedCodeInfo(collectData);

                Controller.Collect(Step.Barcodes.ToArray(), collectData, GetWorkcell());

                PrintBindingSn();

                AddDetail(collectData.CollectBarcode, collectData.Result);

                Reset(ResetType.Success);

                if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish)
                {
                    ShowTips("[{0}]过站成功".L10nFormat(collectData.CollectBarcode));
                }
                else
                {
                    ShowTips("[{0}]入站成功".L10nFormat(collectData.CollectBarcode));
                }

                RefreshStatistics();

                RefrshReportTasks();
            }
            catch (Exception exc)
            {
                var baseExc = exc.GetBaseException();

                if (baseExc is UnBindingSnException)
                {
                    PanelInfo.BindingMode = BindingMode.Manual;
                    ShowTips("拼板码[{0}]可绑定{1}个产品，请扫描待绑定的第{2}个产品条码"
                        .L10nFormat(PanelInfo.PanelCode, PanelInfo.NeetToBindingQty, PanelInfo.SnList.Count + 1));
                }
                else
                {
                    ShowError(exc);
                }
            }
        }
    }
}
