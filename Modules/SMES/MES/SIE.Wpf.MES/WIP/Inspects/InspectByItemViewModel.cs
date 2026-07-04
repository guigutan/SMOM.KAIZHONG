using SIE.Common;
using SIE.Common.Configs;
using SIE.Defects;
using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Inspects;
using SIE.MES.WIP.Models;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 检验项目采集ViewModel
    /// </summary>
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [RootEntity, Serializable]
    [Label("不良描述")]
    public class InspectByItemViewModel : DataCollectionViewModel<InspectByItemController>
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        public InspectByItemViewModel()
        {
            InitWorkstation(ProcessType.Pqc);
        }

        #region 缺陷信息 DefectItemList
        /// <summary> 
        /// 缺陷信息
        /// </summary>
        [Label("缺陷信息")]
        public static readonly ListProperty<EntityList<DefectItemViewModel>> DefectItemListProperty = P<InspectByItemViewModel>.RegisterList(e => e.DefectItemList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<DefectItemViewModel>()
        });

        /// <summary> 
        /// 缺陷信息
        /// </summary>
        public EntityList<DefectItemViewModel> DefectItemList
        {
            get { return this.GetLazyList(DefectItemListProperty); }
        }
        #endregion

        #region 机型检验项目 InspectionItemList
        /// <summary> 
        /// 机型检验项目
        /// </summary>
        [Label("机型检验项目")]
        public static readonly ListProperty<EntityList<InspectionItemViewModel>> InspectionItemListProperty = P<InspectByItemViewModel>.RegisterList(e => e.InspectionItemList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<InspectionItemViewModel>()
        });

        /// <summary> 
        /// 机型检验项目
        /// </summary>
        public EntityList<InspectionItemViewModel> InspectionItemList
        {
            get { return this.GetLazyList(InspectionItemListProperty); }
        }
        #endregion

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
                    if (!PanelInfo.NeetToBindingSn)
                        ShowTips("[{0}]关联产品条码完成，请提交".L10nFormat(Step.Barcodes.FirstOrDefault()));
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
                    {
                        Controller.ValidateBarcode(collectBarcode, workcell);
                    }

                    //当前不是入站操作时，载入产品机型或产品对应的检验项目
                    if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish)
                    {
                        InspectionItemList.Clear();
                        var inspectionItems = Controller.GetInspectionItems(WorkOrder.ProductId, workcell.ProcessId);
                        InspectionItemList.AddRange(inspectionItems.Select(p => new InspectionItemViewModel
                        {
                            ModelInspecitonItem = p,
                            PersistenceStatus = PersistenceStatus.Unchanged
                        }));
                    }

                    Step.Barcodes.Add(collectBarcode.Code);
                }

                if (!Step.NextStep())
                {
                    if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Start)
                    {
                        //入站（MoveIn）时不录入检验项目，直接提交
                        Submit();
                    }
                    else
                    {
                        ShowTips("[{0}]扫描成功,请提交".L10nFormat(collectBarcode));
                    }
                }
                else
                {
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
        /// 重置
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);
            DefectItemList.Clear();
            InspectionItemList.Clear();
        }

        /// <summary>
        /// 是否可提交验证
        /// </summary>
        /// <returns>返回是否可提交</returns>
        public virtual bool CanSubmit()
        {
            var isAllCheck = false;

            //是否已完成所有检验项
            isAllCheck = InspectionItemList.Any(p => p.IsNg) || InspectionItemList.All(p => p.IsNg || p.IsOk);

            return !Step.HasNextStep()
                && Workstation.EmployeeId.HasValue
                && Workstation.ProcessId.HasValue
                && Workstation.StationId.HasValue
                && Workstation.ResourceId.HasValue
                && isAllCheck;
        }

        /// <summary>
        /// 验证检验项目测试值
        /// </summary>
        /// <returns>检验项目是否合格</returns>
        private bool ValidateInspectionItem()
        {
            var submitInspectionItemList = InspectionItemList.Where(p => p.IsOk || p.IsNg);
            foreach (var item in submitInspectionItemList)
            {
                //如果检验项目标识为定量，则验证测试值
                if (item.ModelInspecitonItem.CheckTag == CheckTag.Quantitative && !item.InspectionValue.HasValue)
                {
                    ShowError("检验项[{0}]测试值不能为空".L10nFormat(item.ModelInspecitonItem.Name));
                    return false;
                }

                var defectCount = DefectItemList.Count(d => d.ModelInspectionItemId == item.ModelInspecitonItemId);
                ////如果存在不合格检验项目，则验证是否维护不良信息
                if (item.IsNg && defectCount == 0)
                {
                    ShowError("检验项[{0}]为不合格，请维护不良信息".L10nFormat(item.ModelInspecitonItem.Name));
                    return false;
                }

                //如果检验项目为合格，则验证是否维护不良信息（不合格改判合格情况下）
                if (item.IsOk && defectCount > 0)
                {
                    ShowError("检验项[{0}]为合格，不能维护不良信息".L10nFormat(item.ModelInspecitonItem.Name));
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 提交采集
        /// </summary>
        public virtual void Submit()
        {
            try
            {
                if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish
                    && !ValidateInspectionItem())
                {
                    //非入站，且检验项目未正确录入，则不能提交
                    return;
                }
                var submitInspectionItemList = InspectionItemList.Where(p => p.IsOk || p.IsNg);
                var collectData = new CollectData();

                //过站类型（Start => Move In, Finish => Move Out)
                collectData.State = WipProductProcessState;

                collectData.CollectBarcode = new CollectBarcode
                {
                    Code = Step.Barcodes.LastOrDefault(),
                    Type = Step.CurrentStep.BarcodeType
                };

                if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish)
                {
                    //当前不是入站（Move In）时，提交检验结果和缺陷
                    //检验结果存在不合格，则改工序为不合格
                    collectData.Result = submitInspectionItemList.Any(p => p.IsNg) ? ResultType.Fail : ResultType.Pass;

                    foreach (var defectItem in DefectItemList)
                    {
                        var defect = defectItem.Defect;
                        collectData.Defects.Add(new DefectData
                        {
                            DefectId = defect.Id,
                            DefectName = defect.Code,
                            CategoryId = defect.DefectCategoryId,
                            CategoryName = defect.DefectCategory?.Code,
                            Qty = 1,
                            InspectionItemId = defectItem.ModelInspectionItemId
                        });
                    }

                    foreach (var item in submitInspectionItemList)
                    {
                        collectData.InspectionItems.Add(new InspectionItem
                        {
                            InspectItemId = item.ModelInspecitonItem.Id,
                            InspectionValue = item.InspectionValue,
                            ItemResult = item.IsOk ? ResultType.Pass : ResultType.Fail,
                            Remarks = item.Remarks
                        });
                    }
                }

                Controller.Collect(Step.Barcodes.ToArray(), collectData, GetWorkcell());

                PrintBindingSn();

                AddDetail(new CollectBarcode
                {
                    Code = Step.Barcodes.LastOrDefault(),
                    Type = Step.CurrentStep.BarcodeType
                }, collectData.Result);

                var barcode = Step.Barcodes.LastOrDefault();

                Reset(ResetType.Success);

                if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish)
                {
                    ShowTips("[{0}]过站成功".L10nFormat(barcode));
                }
                else
                {
                    ShowTips("[{0}]入站成功".L10nFormat(barcode));
                }

                RefreshStatistics();
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
                    ShowError(exc);
                }
            }
        }
    }
}
