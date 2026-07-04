using SIE.Common;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.Statistics.WIP;
using SIE.MES.WIP;
using SIE.MES.WIP.Inspects;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace SIE.Wpf.MES.Workbench.Inspects
{
    /// <summary>
    /// 工作台检验采集视图模型
    /// </summary>
    public class WorkBenchInspectViewModel : DataCollectionViewModel<InspectController>
    {
        #region 缺陷录入 HasDefect
        /// <summary>
        /// 缺陷录入
        /// </summary>
        [Label("缺陷录入")]
        public static readonly Property<bool> HasDefectProperty = P<WorkBenchInspectViewModel>.Register(e => e.HasDefect, new PropertyMetadata<bool>
        {
            PropertyChangedCallBack = (s, e) => (s as WorkBenchInspectViewModel).OnHasDefectChanged(e),
        });

        private void OnHasDefectChanged(ManagedPropertyChangedEventArgs e)
        {
            ////TODO 测试 删除
            //Percent = 10;
            //Progress = "100/1000";
            //Error = null;
            //Tips = "dfsfs";
            //No = "WO20182033215";
        }

        /// <summary>
        /// 缺陷录入
        /// </summary>
        public bool HasDefect
        {
            get { return this.GetProperty(HasDefectProperty); }
            set { this.SetProperty(HasDefectProperty, value); }
        }
        #endregion

        #region 任务进度百分比 Percent
        /// <summary>
        /// 任务进度百分比
        /// </summary>
        [Label("任务进度百分比")]
        public static readonly Property<double> PercentProperty = P<WorkBenchInspectViewModel>.Register(e => e.Percent);

        /// <summary>
        /// 任务进度百分比
        /// </summary>
        public double Percent
        {
            get { return this.GetProperty(PercentProperty); }
            set { this.SetProperty(PercentProperty, value); }
        }
        #endregion

        #region 任务进度 Progress
        /// <summary>
        /// 任务进度
        /// </summary>
        [Label("任务进度")]
        public static readonly Property<string> ProgressProperty = P<WorkBenchInspectViewModel>.Register(e => e.Progress);

        /// <summary>
        /// 任务进度
        /// </summary>
        public string Progress
        {
            get { return this.GetProperty(ProgressProperty); }
            set { this.SetProperty(ProgressProperty, value); }
        }
        #endregion

        #region 工单号 No
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> NoProperty = P<WorkBenchInspectViewModel>.Register(e => e.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string No
        {
            get { return this.GetProperty(NoProperty); }
            set { this.SetProperty(NoProperty, value); }
        }
        #endregion

        #region 产品ID ProductId
        /// <summary>
        /// 产品ID
        /// </summary>
        [Label("产品ID")]
        public static readonly Property<double> ProductIdProperty = P<WorkBenchInspectViewModel>.Register(e => e.ProductId);

        /// <summary>
        /// 产品ID
        /// </summary>
        public double ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #region 缺陷控件
        WorkBenchDefectControl _control;
        WorkBenchDefectControl Control { get { return _control ?? (WorkBenchDefectControl)CreateDefectControl(); } }

        /// <summary>
        /// 创建缺陷控件
        /// </summary> 
        /// <returns>返回创建的UI</returns>
        FrameworkElement CreateDefectControl()
        {
            var ctl = WorkBenchDefectControlFactory.CreateControl();
            ctl.AllowMultiple = true;
            ctl.DataContext = this;
            ctl.SetBinding(WorkBenchDefectControl.SelectedValueProperty, new Binding("DefectItemList"));
            ctl.SetBinding(WorkBenchDefectControl.DefectsProperty, new Binding("DefectList"));
            ctl.Margin = new Thickness(-8);
            return ctl;
        }
        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkBenchInspectViewModel()
        {
            CreateDefectControl();
            Onload();
        }

        /// <summary>
        /// 条码变更事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (!Barcode.IsNotEmpty()) return;
            try
            {
                ClearInfos();
                var broken = Workstation.Validate(ValidatorActions.None);
                if (broken.Count > 0)
                    throw new ValidationException(broken.ToString());
                if (Barcode.ToUpper() == "RESET")
                {
                    Reset(ResetType.CollectRestart);
                    return;
                }
                if (!Step.HasNextStep())
                    Submit();
                var currentStep = Step.CurrentStep;
                var collectBarcode = new CollectBarcode { Code = Barcode, Type = currentStep.BarcodeType };
                var workcell = GetWorkcell();
                if (Step.StepIndex == 0)
                    Validate(collectBarcode, workcell);
                if (Step.StepIndex != 0)
                    Controller.ValidateBarcode(collectBarcode, workcell);
                Step.Barcodes.Add(collectBarcode.Code);
                if (!Step.NextStep())
                    Submit();
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

        protected override ProductInfo Validate(CollectBarcode collectBarcode, Workcell workcell)
        {
            var productInfo = base.Validate(collectBarcode, workcell);
            if (WorkOrder.ProductId != ProductId)
                ProductId = WorkOrder.ProductId;
            return productInfo;
        }

        /// <summary>
        /// 缺陷录入
        /// </summary>
        void DefectInput()
        {
            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), Control, w =>
             {
                 w.Title = "缺陷录入".L10N();
                 w.Commands.Clear();
                 w.Commands.Add("取消".L10N());
                 w.Commands.Add("提交".L10N());
                 w.Height = 450;
                 w.Width = 800;
                 w.Closing += (s, e) =>
                 {
                     if (w.Result == 1 && DefectItemList.Count == 0)
                     {
                         e.Cancel = true;
                         CRT.MessageService.ShowMessage("请录入缺陷".L10N(), "提示".L10N());
                     }
                 };

                 LoadWorkstationData();
             });
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
        /// 加载工作站的数据
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();
            try
            {
                var workcell = GetWorkcell();
                ResetDefects(workcell.ProcessId);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 重置工序缺陷信息
        /// </summary>
        /// <param name="processId">工序Id</param>
        public void ResetDefects(double? processId)
        {
            DefectList.Clear();
            if (processId.HasValue && processId != 0)
            {
                var process = RF.GetById<Process>(processId);
                DefectList.AddRange(process.DefectList.Select(p => p.Defect));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ValidateWorkstaion()
        {
            ClearInfos();
            var broken = Workstation.Validate(ValidatorActions.None);
            if (broken.Count > 0)
                ShowError(broken.ToString());
            else
            {                
                ShowTips("请扫描条码".L10N());
            }
        }

        public override void RefreshStatistics()
        {
            Task.Run(() =>
            {
                Thread.Sleep(2 * 1000);   //线程休息2s等待预统计完成后再刷新，采集数刷新并非实时
                var info = new ProcessStatisticsQuery()
                {
                    ResourceId = Workstation.ResourceId ?? 0,
                    ProcessId = Workstation.ProcessId ?? 0,
                    WorkOrderId = WorkOrderId ?? 0
                };
                ShowWorkProcessStatistics(RT.Service.Resolve<WipStatisticsController>().GetProcessCollected(info));
            });
        }

        protected void ShowWorkProcessStatistics(ProcessCollectedEvent e)
        {
            if (e.ProcessId == Workstation.ProcessId)
            {
                CRT.MainThread.InvokeAsync(() =>
                {
                    // 当班合格数：QtyPass   当班不良数：QtyFailed
                    if (WorkOrder == null) return;
                    var qty = e.QtyPass + e.QtyFailed;
                    Percent = double.Parse((qty).ToString()) / double.Parse(WorkOrder.PlanQty.ToString());
                    Progress = "{0}".FormatArgs((int)(qty)) + " / " + "{0}".FormatArgs(WorkOrder.PlanQty);
                });
            }
        }

        /// <summary>
        /// 执行提交逻辑
        /// </summary>
        void Submit()
        {
            if (HasDefect)
                DefectInput();
            try
            {
                var collectData = new CollectData();
                collectData.Result = DefectItemList.Count > 0 ? ResultType.Fail : ResultType.Pass;
                foreach (var defectItem in DefectItemList)
                {
                    var defect = defectItem.Defect;
                    collectData.Defects.Add(new DefectData { DefectId = defect.Id, DefectName = defect.Description, CategoryId = defect.DefectCategoryId, CategoryName = defect.DefectCategory?.Description, Qty = defectItem.Qty });
                }

                RT.Service.Resolve<InspectController>().Collect(Step.Barcodes.ToArray(), collectData, GetWorkcell());
                AddDetail(new CollectBarcode { Code = Step.Barcodes.LastOrDefault(), Type = Step.CurrentStep.BarcodeType }, collectData.Result);
                var barcode = Step.Barcodes.LastOrDefault();
                Reset(ResetType.Success);
                ShowTips("[{0}]过站成功".L10nFormat(barcode));
                RefreshStatistics();
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
        }
    }

}