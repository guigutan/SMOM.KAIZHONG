using SIE.Common.Configs;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Repairs;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Repairs;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Wpf.MES.LoadItems;
using SIE.Wpf.MES.WIP;
using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace SIE.Wpf.MES.BatchWIP.Repairs
{
    /// <summary>
    /// 维修采集
    /// </summary>
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [RootEntity, Serializable]
    [Label("批次维修采集")]
    public partial class BatchRepairViewModel : BatchDataCollectionViewModel<BatchRepairController>
    {
        /// <summary>
        /// 验证过的条码，防止验证通过后，提交前再修改条码
        /// </summary>
        public CollectBarcode SubmitBarcode { get; set; }

        /// <summary>
        /// 转出批次
        /// </summary>
        OutputBatch OutputBatch { get; set; }

        /// <summary>
        /// 批次入站时间
        /// </summary>
        DateTime? _inputDate;

        /// <summary>
        /// 维修采集视图模型，初始化工序类型
        /// </summary>
        public BatchRepairViewModel()
        {
            InitWorkstation(ProcessType.BatchFix);
        }

        #region DefectList 不良信息
        /// <summary>
        /// 不良信息
        /// </summary>
        [Label("不良信息")]
        public static readonly ListProperty<EntityList<BatchRepairDefectViewModel>> BatchRepairDefectListProperty = P<BatchRepairViewModel>.RegisterList(e => e.BatchRepairDefectList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<BatchRepairDefectViewModel>()
        });

        /// <summary> 
        /// 不良信息
        /// </summary>
        public EntityList<BatchRepairDefectViewModel> BatchRepairDefectList
        {
            get { return this.GetLazyList(BatchRepairDefectListProperty); }
        }
        #endregion

        /// <summary>
        /// 属性变更事件，重置显示信息及数据
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// 条码变更事件，采集条码
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty())
            {
                return;
            }

            ClearTipsInfos();
            var workcell = GetWorkcell();
            try
            {
                if (IsReceiveContainer)
                {
                    RemoveInputBatch(base.InputBatch, Barcode);
                }                
                else
                {
                    var collectStep = Step.InputCollectStep;
                    var collectBarcode = new CollectBarcode { Code = Barcode, Type = collectStep.BarcodeType };
                    MoveIn(collectBarcode, workcell); //验证
                    ShowTips("[{0}]扫描成功，请维修".L10nFormat(collectBarcode));
                }
            }
            catch (Exception exc)
            {
                ShowError(exc);
                if (IsReceiveContainer)
                    ShowTips("请扫描移除关联载具".L10N());
            }
            finally
            {
                Barcode = null;
            }
        }

        /// <summary>
        /// 加载缺陷
        /// </summary>
        /// <param name="workcell">工作单元</param>
        /// <param name="collectBarcode">采集条码</param>
        internal void LoadDefects(Workcell workcell, CollectBarcode collectBarcode)
        {
            BatchRepairDefectList.Clear();
            var defects = Controller.LoadBatchDefects(collectBarcode, workcell);
            var records = Controller.GetRepairRecords(defects.Select(p => p.Id).ToArray(), true);
            foreach (var defect in defects)
            {
                var repairDefect = new BatchRepairDefectViewModel() { WipProductDefect = defect, Remark = defect.Remark, Qty = defect.Qty };
                BatchRepairDefectList.Add(repairDefect);
                var recode = records.FirstOrDefault(p => p.ProductDefectId == defect.Id);
                if (recode == null) continue;
                repairDefect.ScrapQty = recode.ScrapQty;
                repairDefect.ScrapReason = recode.ScrapReason;
                repairDefect.Remark = recode.Remark;
                repairDefect.MeasureList.AddRange(recode.MeasureList.Select(p => p.Measure));
                repairDefect.ResponsibilityList.AddRange(recode.ResponsibilityList.Select(p => p.Responsibility));
            }
            var inputDate = records.FirstOrDefault()?.InputDate;
            _inputDate = inputDate.HasValue ? inputDate.Value : RF.Find<InputBatch>().GetDbTime();
            
            BatchRepairDefectList.MarkSaved();
        }

        /// <summary>
        /// 初始化转出批次
        /// </summary>
        /// <param name="collectBarcode">采集条码</param>
        /// <param name="batch">转入批次</param>
        /// <param name="containerNo">载具号</param>
        private void InitOutputBatch(CollectBarcode collectBarcode, InputBatch batch, string containerNo = "")
        {
            var relation = RT.Service.Resolve<BatchManageController>().GetBatchRelation(collectBarcode);
            if (relation == null)
                throw new ValidationException("未找到批次".L10N());
            OutputBatch = new OutputBatch()
            {
                Qty = batch.RemainQty - BatchRepairDefectList.Sum(p => p.ScrapQty),
                BatchNo = batch.BatchNo,
                ContainerNo = batch.ContainerNo,
                InputDate = batch.InputDate,
                OutputDate = DateTime.Now,
                IsNg = batch.NgQty > 0,
                BarcodeType = Step.OutputCollectStep.BarcodeType,
                WorkOrderId = batch.WorkOrderId,
                ScrapQty = batch.ScrapQty,
                NgQty = batch.NgQty,
            };

            OutputBatch.RelationBatchList.Add(new RelationBatch
            {
                InputBatchId = batch.Id,
                OutputBatchId = OutputBatch.Id,
                Qty = OutputBatch.Qty,
            });

            SubmitBarcode = collectBarcode;
        }

        /// <summary>
        /// 加载工作单元数据
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();            
        }

        /// <summary>
        /// 维修采集
        /// </summary>
        /// <param name="batch">需转出批次</param>
        /// <param name="uplineViewModel">上线工序</param>
        public virtual void Submit(InputBatch batch, BatchUplineViewModel uplineViewModel)
        {
            if (Step.OutputCollectStep == null)
            {
                throw new ValidationException("当前工序不存在转出步骤！".L10N());
            }
            var collectBarcode = new CollectBarcode { Type = BarcodeType.BatchBarocde, Code = batch.SubBatchNo.IsNotEmpty() ? batch.SubBatchNo : batch.BatchNo };
            InitOutputBatch(collectBarcode, batch, uplineViewModel.ContainerNo);

            var collectData = new CollectData() { CollectBarcode = SubmitBarcode, OutputBatch = OutputBatch };
            PrepareCollectData(collectData);

            if (uplineViewModel.UplineProcess == null)
            {
                throw new ValidationException("请选择转出工序！".L10N());
            }

            RT.Service.Resolve<BatchRepairController>().BatchCollect(collectData, GetWorkcell(),
                uplineViewModel.UplineProcess.RoutingProcessId);

            RefreshStatistics();
            RefreshBetail(PlugType.Out);
            SaveRepairRecord();
            var barcode = SubmitBarcode.Code;
            
            Reset(ResetType.Success);
            ShowTips("[{0}]维修完成，请扫描条码".L10nFormat(barcode));
        }

        /// <summary>
        /// 准备采集数据集
        /// </summary>
        /// <param name="collectData">采集数据</param>
        private void PrepareCollectData(CollectData collectData)
        {            
            collectData.RepairDefects.AddRange(BatchRepairDefectList.Select(p => new RepairDefect
            {
                IsFixed = true,
                ProductDefectId = p.WipProductDefectId,
                Remark = p.Remark,
                Responsiblities = p.ResponsibilityList,
                Measures = p.MeasureList,
                ScrapQty = p.ScrapQty,
                ScrapReason = p.ScrapReason
            }));
            collectData.ScrapQty = collectData.RepairDefects.Sum(p => p.ScrapQty);
            collectData.NgQty = collectData.OutputBatch.Qty;
            collectData.Context["InputDate"] = _inputDate;
        }

        /// <summary>
        /// 暂存维修记录
        /// </summary>
        public virtual void SaveRepairRecord()
        {
            EntityList<RepairRecord> records = new EntityList<RepairRecord>();
            BatchRepairDefectList.ForEach(p =>
            {
                var recode = new RepairRecord() { ProductDefectId = p.WipProductDefectId, IsBatch = true, ScrapQty = p.ScrapQty, ScrapReason = p.ScrapReason, Remark = p.Remark, InputDate = _inputDate.HasValue ? _inputDate.Value : RF.Find<RepairRecord>().GetDbTime() };
                p.MeasureList.ForEach(f => recode.MeasureList.Add(new MeasureRecord { Measure = f }));
                p.ResponsibilityList.ForEach(f => recode.ResponsibilityList.Add(new ResponsibilityRecord { Responsibility = f }));
                records.Add(recode);
            });
            RT.Service.Resolve<BatchRepairController>().SaveRepairRecord(records, BatchRepairDefectList.Select(p => p.WipProductDefectId).ToArray());
        }

        /// <summary>
        /// 批次已移除后操作
        /// </summary>
        /// <param name="inputBatch">已移除批次</param>
        protected override void InputBatchRemoved(InputBatch inputBatch)
        {
            base.InputBatchRemoved(inputBatch);
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);
            BatchRepairDefectList.Clear();            
            SubmitBarcode = null;
            OutputBatch = null;
            _inputDate = null;
        }

        /// <summary>
        /// 能否完成
        /// </summary>
        /// <returns>能提交返回true，否则返回false</returns>
        public bool CanSubmit()
        {
            return Workstation.EmployeeId.HasValue
                && Workstation.ProcessId.HasValue
                && Workstation.StationId.HasValue
                && Workstation.ResourceId.HasValue;
        }

        #region 报工任务
        /// <summary>
        /// 刷新工单任务列表
        /// </summary>
        /// <param name="retrospectType">追溯方式</param>
        /// <param name="lazyLoad">延迟加载，采集后才做报工，需报工后再刷新任务列表</param>
        public override void RefrshReportTasks(Core.Items.RetrospectType retrospectType = Core.Items.RetrospectType.Single, bool lazyLoad = true)
        {
            //方法重写
        }

        /// <summary>
        /// 更新工单任务报工方式
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        protected override void UpdateWorkOrdeReportModel(double workOrderId)
        {
            //方法重写
        }

        /// <summary>
        /// 验证工单任务报工
        /// </summary>
        /// <param name="workOrderId">工单Id</param>
        /// <param name="workcell">工作单元</param>
        protected override void ValidateTaskReport(double workOrderId, Workcell workcell)
        {
            //方法重写
        }
        #endregion
    }
}