using DevExpress.Data.Extensions;
using NPOI.SS.Formula.Functions;
using SIE.Common.Configs;
using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Repairs;
using SIE.MES.WIP.Runtime;
using SIE.MES.WIP.TemporaryRepairs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Tech.Processs;
using SIE.Wpf.MES.Controls;
using SIE.Wpf.MES.LoadItems;
using SIE.Wpf.MES.WIP.Repairs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace SIE.Wpf.MES.WIP.TemporaryRepairs
{
    /// <summary>
    /// 临时维修采集
    /// </summary>
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [EntityWithConfig(typeof(ChangeItemHandleMethodConfig))]
    [RootEntity, Serializable]
    [Label("临时维修采集")]
    public partial class TemporaryRepairViewModel : DataCollectionViewModel<WipTemporaryRepairController>, ILoadableItem
    {
        /// <summary>
        /// 换料后原键件处理方式配置项的值
        /// </summary>
        private ChangeItemHandleMethod changeItemHandleMethod;

        /// <summary>
        /// 验证过的条码，防止验证通过后，提交前再修改条码
        /// </summary>
        public CollectBarcode SubmitBarcode { get; set; }

        /// <summary>
        /// 维修采集视图模型，初始化工序类型
        /// </summary>
        public TemporaryRepairViewModel()
        {
            InitWorkstation(ProcessType.Fix);
        }
        #region 维修条码 RepairBarcode
        /// <summary>
        /// 维修条码
        /// </summary>
        [Label("维修条码")]
        public static readonly Property<string> RepairBarcodeProperty = P<TemporaryRepairViewModel>.Register(e => e.RepairBarcode);

        /// <summary>
        /// 维修条码
        /// </summary>
        public string RepairBarcode
        {
            get { return this.GetProperty(RepairBarcodeProperty); }
            set { this.SetProperty(RepairBarcodeProperty, value); }
        }
        #endregion

        #region 模块KEY ModuleKey
        /// <summary>
        /// 模块KEY
        /// </summary>
        [Label("模块KEY")]
        public static readonly Property<string> ModuleKeyProperty = P<TemporaryRepairViewModel>.Register(e => e.ModuleKey);

        /// <summary>
        /// 模块KEY
        /// </summary>
        public string ModuleKey
        {
            get { return this.GetProperty(ModuleKeyProperty); }
            set { this.SetProperty(ModuleKeyProperty, value); }
        }
        #endregion 

        #region 不良记录 DefectsList
        /// <summary>
        /// 不良记录
        /// </summary>
        [Label("不良记录")]
        public static readonly ListProperty<EntityList<SIE.MES.WIP.Repairs.TemporaryRepairDefectViewModel>> DefectListProperty = P<TemporaryRepairViewModel>.RegisterList(e => e.DefectsList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as TemporaryRepairViewModel).LoadDefectsList()
        });

        /// <summary>
        /// 不良记录
        /// </summary>
        public EntityList<SIE.MES.WIP.Repairs.TemporaryRepairDefectViewModel> DefectsList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }/// <summary>
         /// 加载不良记录
         /// </summary>
        private EntityList<SIE.MES.WIP.Repairs.TemporaryRepairDefectViewModel> LoadDefectsList()
        {
            return new EntityList<SIE.MES.WIP.Repairs.TemporaryRepairDefectViewModel>();
        }
        #endregion

        #region 维修记录 RepairMainRecord
        /// <summary>
        /// 维修记录Id
        /// </summary>
        [Label("维修记录")]
        public static readonly IRefIdProperty RepairMainRecordIdProperty =
            P<TemporaryRepairViewModel>.RegisterRefId(e => e.RepairMainRecordId, ReferenceType.Normal);

        /// <summary>
        /// 维修记录Id
        /// </summary>
        public double? RepairMainRecordId
        {
            get { return (double?)this.GetRefNullableId(RepairMainRecordIdProperty); }
            set { this.SetRefNullableId(RepairMainRecordIdProperty, value); }
        }

        /// <summary>
        /// 维修记录
        /// </summary>
        public static readonly RefEntityProperty<RepairMainRecord> RepairMainRecordProperty =
            P<TemporaryRepairViewModel>.RegisterRef(e => e.RepairMainRecord, RepairMainRecordIdProperty);

        /// <summary>
        /// 维修记录
        /// </summary>
        public RepairMainRecord RepairMainRecord
        {
            get { return this.GetRefEntity(RepairMainRecordProperty); }
            set { this.SetRefEntity(RepairMainRecordProperty, value); }
        }
        #endregion

        /// <summary>
        /// 创建缺陷录入窗体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        protected virtual DefectControl CreateDefectControl(TemporaryRepairViewModel model)
        {
            var ctl = DefectControlFactory.CreateControl();
            ctl.AllowMultiple = true;
            ctl.DataContext = model;
            ctl.SetBinding(DefectControl.SelectedValueProperty, new Binding("DefectItemList"));
            model.DefectControl = ctl;
            ctl.Margin = new Thickness(-8);
            return ctl;
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public override void Onload()
        {
            base.Onload();

            //加载换料后原关键件条码处理方式的配置项
            var configValue = ConfigService.GetConfig(new ChangeItemHandleMethodConfig(), typeof(TemporaryRepairViewModel));
            if (configValue != null)
            {
                changeItemHandleMethod = configValue.HandleMethod;
            }
            else
            {
                changeItemHandleMethod = ChangeItemHandleMethod.Scrap;
            }
        }

        #region IsLoadItem 是否上料
        /// <summary>
        /// 是否上料
        /// </summary>
        [Label("是否上料")]
        public static readonly Property<bool> IsLoadItemProperty = P<TemporaryRepairViewModel>.Register(e => e.IsLoadItem, new PropertyMetadata<bool>() { PropertyChangedCallBack = (o, e) => (o as TemporaryRepairViewModel).OnIsLoadItem(e) });

        /// <summary>
        /// 是否上料值变更
        /// </summary>
        /// <param name="e">参数</param>
        private void OnIsLoadItem(ManagedPropertyChangedEventArgs e)
        {
            FocuseBarcode();
        }

        /// <summary>
        /// 是否上料
        /// </summary>
        public bool IsLoadItem
        {
            get { return this.GetProperty(IsLoadItemProperty); }
            set { this.SetProperty(IsLoadItemProperty, value); }
        }
        #endregion

        #region DefectList 不良信息
        /// <summary>
        /// 不良信息
        /// </summary>
        [Label("不良信息")]
        public static readonly ListProperty<EntityList<TemporaryRepairDefectViewModel>> RepairDefectListProperty = P<TemporaryRepairViewModel>.RegisterList(e => e.RepairDefectList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<TemporaryRepairDefectViewModel>()
        });

        /// <summary> 
        /// 不良信息
        /// </summary>
        public EntityList<TemporaryRepairDefectViewModel> RepairDefectList
        {
            get { return this.GetLazyList(RepairDefectListProperty); }
        }
        #endregion

        #region DetailList 装配明细
        /// <summary>
        /// 装配明细
        /// </summary>
        [Label("装配明细")]
        public static readonly ListProperty<EntityList<ProductAssemblyDetailViewModel>> DetailListProperty = P<TemporaryRepairViewModel>.RegisterList(e => e.DetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<ProductAssemblyDetailViewModel>()
        });

        /// <summary>
        /// 装配明细
        /// </summary>
        public EntityList<ProductAssemblyDetailViewModel> DetailList
        {
            get { return this.GetLazyList(DetailListProperty); }
        }
        #endregion

        #region LoadItemList 上料
        /// <summary>
        /// 上料
        /// </summary>
        [Label("上料")]
        public static readonly ListProperty<EntityList<LoadItem>> LoadItemListProperty = P<TemporaryRepairViewModel>.RegisterList(e => e.LoadItemList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<LoadItem>()
        });

        /// <summary>
        /// 上料
        /// </summary>
        public EntityList<LoadItem> LoadItemList
        {
            get { return this.GetLazyList(LoadItemListProperty); }
        }
        #endregion

        #region UnloadItemList 下料
        /// <summary>
        /// 下料列表
        /// </summary>
        [Label("下料")]
        public static readonly ListProperty<EntityList<UnloadItem>> UnloadItemListProperty = P<TemporaryRepairViewModel>.RegisterList(e => e.UnloadItemList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<UnloadItem>()
        });

        /// <summary> 
        /// 下料
        /// </summary>
        public EntityList<UnloadItem> UnloadItemList
        {
            get { return this.GetLazyList(UnloadItemListProperty); }
        }
        #endregion




        /// <summary>
        /// 属性变更事件，重置显示信息及数据
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == IsLoadItemProperty)
            {
                ShowTips(IsLoadItem ? "请扫描上料".L10N() : "请扫描条码".L10N());
                FocuseBarcode();
            }
        }

        /// <summary>
        /// 条码变更事件，采集条码
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty()) return;

            ClearInfos();

            var workcell = GetWorkcell();

            try
            {
                if (IsLoadItem)
                {
                    LoadItem(Barcode, workcell);
                }
                else
                {
                    Repair(workcell);
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
        /// 维修
        /// </summary>
        /// <param name="workcell"></param>
        private void Repair(Workcell workcell)
        {
            RepairMainRecord repairMainRecord = null;
            WipProductVersion version = null;


            if (SubmitBarcode != null)
            {
                ShowError("[{0}]未完成,请先完成或者重新开始".L10nFormat(SubmitBarcode));
                return;
            }
            if (PanelInfo.BindingMode == SIE.MES.WIP.Models.BindingMode.Manual && PanelInfo.NeetToBindingSn)
            {
                var snQty = Controller.ValidateNewBarcode(Barcode, WorkOrderId ?? 0);
                PanelInfo.SnList.Add(new SnData() { Sn = Barcode, Qty = snQty });
                if (!PanelInfo.NeetToBindingSn)
                    ShowTips("[{0}]关联产品条码完成，请维修".L10nFormat(SubmitBarcode));
            }
            else
            {
                var currentStep = Step.CurrentStep;
                var collectBarcode = new CollectBarcode() { Code = Barcode, Type = currentStep.BarcodeType };

                //验证
                var info = Validate(collectBarcode, workcell);
                MergeData(info);
                DisplayBarCode = Barcode;

                SubmitBarcode = collectBarcode;

                RepairDefectList.Clear();

                version = Controller.GetCurrentVersion(SubmitBarcode.Code);

                //设置过站记录状态(Start=>MoveIn,Finish=>MoveOut)
                var collectData = new CollectData();
                collectData.State = WipProductProcessState.Start;
                collectData.CollectBarcode = collectBarcode;

                var product = RT.Service.Resolve<RuntimeController>().FindProduct(collectBarcode);
                if (product == null || product.Routing == null || product.Routing.Current == null)
                {
                    throw new ValidationException("当前条码未完成任何工序采集，请至少完成一个工序采集再维修！".L10N());
                }

                var wipProductProcess = Controller.IsExsitedWipProductProcess(info.WorkOrderId, SubmitBarcode.Code, workcell.ProcessId, version.Id);
                repairMainRecord = Controller.GetRepairRecord(info.WorkOrderId, SubmitBarcode.Code, true);

                var currentTime = RF.Find<WipProductProcess>().GetDbTime();
                var shift = RT.Service.Resolve<WipResourceController>()
                    .GetWipResourceShift(workcell.ResourceId, currentTime);

                var currentProcess = RF.GetById<Process>(workcell.ProcessId);
                if (currentProcess.EnableMoveInControl == true
                && ((wipProductProcess != null && wipProductProcess.State == WipProductProcessState.Start)))
                {
                    info.WipProductProcessState = WipProductProcessState.Finish;
                }
                else
                {
                    version.IsPause = YesNo.Yes;
                    info.WipProductProcessState = WipProductProcessState.Start;
                    var wipProductRepair = Controller.GetExsitedWipProductRepair(wipProductProcess, workcell, version.Id);
                    if (wipProductRepair == null)
                    {
                        var wipProductRepairInfo = new WipProductRepair
                        {
                            ProcessId = workcell.ProcessId,
                            ReparieById = workcell.EmployeeId,
                            ResourceId = workcell.ResourceId,
                            RepairStart = DateTime.Now,
                            ShiftId = shift.Id,
                            StationId = workcell.StationId,
                            VersionId = version.Id,
                            PersistenceStatus = PersistenceStatus.New
                        };
                        RF.Save(wipProductRepairInfo);
                    }
                }
                if (currentProcess.EnableMoveInControl == true && wipProductProcess == null)
                {
                    info.WipProductProcessState = WipProductProcessState.Start;
                    version.IsPause = YesNo.Yes;
                    var wipProductRepair = Controller.GetExsitedWipProductRepair(wipProductProcess, workcell, version.Id);
                    if (wipProductRepair == null)
                    {
                        wipProductRepair = new WipProductRepair
                        {
                            ProcessId = workcell.ProcessId,
                            ReparieById = workcell.EmployeeId,
                            ResourceId = workcell.ResourceId,
                            RepairStart = DateTime.Now,
                            ShiftId = shift.Id,
                            StationId = workcell.StationId,
                            VersionId = version.Id,
                            PersistenceStatus = PersistenceStatus.New
                        };
                        RF.Save(wipProductRepair);
                    }
                }
                if (currentProcess.EnableMoveInControl != true)
                {
                    info.WipProductProcessState =  WipProductProcessState.Finish;
                }
                if (info.WipProductProcessState == WipProductProcessState.Finish && currentProcess.EnableMoveInControl != true)
                {
                    version.IsPause = YesNo.Yes;
                }

                RepairMainRecord = repairMainRecord;
                //设置过站状态
                WipProductProcessState = info.WipProductProcessState;

                //上一次采集的采集结果（通过/失败）
                LastResultType = info.LastResultType;
                if (WipProductProcessState == WipProductProcessState.Finish)
                {
                    version.IsPause = YesNo.Yes;
                    var defects = Controller.LoadDefects(Barcode, workcell);

                    defects.ForEach(f =>
                    {
                        var repairDefect = new TemporaryRepairDefectViewModel()
                        {
                            WipProductDefect = f,
                            ProcessId = f.ProcessId,
                            ActualDefectId = f.DefectId,
                            Remark = f.Remark,
                            DefectCode = f.DefectCode,
                            DefectDesc = f.DefectDesc,
                            DefectId = f.DefectId.Value,
                            IsFixed = f.IsFixed
                        };
                        repairDefect.MeasureList.AddRange(f.MeasureList.Select(p => p.RepairMeasure));
                        repairDefect.ResponsibilityList.AddRange(f.ResponsibilityList.Select(p => p.DefectResponsibility));
                        RepairDefectList.Add(repairDefect);
                    });

                    DetailList.MarkSaved();

                    LoadRepairDetailViewModel(Barcode, false);
                }

            }

            if (WipProductProcessState == WipProductProcessState.Start)
            {
                //入站（Move In)
                Submit(null);
            }
            else
            {
                ShowTips("[{0}]扫描成功，请维修".L10nFormat(SubmitBarcode));
            }

            if (repairMainRecord != null)
            {
                RF.Save(repairMainRecord);
            }
            RF.Save(version);//验证完成后保存挂起状态

        }

        /// <summary>
        /// 上料
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元信息</param>
        protected virtual void LoadItem(string barcode, Workcell workcell)
        {
            try
            {
                Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType = new Dictionary<LoadItemSourceType, bool>
                {
                    //批次上料采集不能上料【单体条码】
                    { LoadItemSourceType.SN, true }
                };

                var loadItemBarcodeInfo = LoadItemHelper.GetLoadBarcodeInfo(barcode, workcell, dicLoadItemSourceType, WorkOrderId);
                RT.Service.Resolve<LoadItemController>().NewLoadItem(loadItemBarcodeInfo, workcell, validateCurrentProcessBom: false);
                RefreshLoadItem();
                ShowTips("[{0}]上料成功".L10nFormat(barcode));
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 加载换料明细
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="isItem">是否半成品</param>
        private void LoadRepairDetailViewModel(string barcode, bool isItem)
        {
            var keyItems = RT.Service.Resolve<RepairController>().LoadkeyItems(barcode, isItem);
            foreach (var keyItem in keyItems)
            {
                var parent = DetailList.FirstOrDefault(p => p.KeyItem.SourceCode == barcode);
                if (parent == null)
                {
                    var detail = new ProductAssemblyDetailViewModel()
                    {
                        KeyItem = keyItem,
                        TreePId = parent?.Id,
                        HandleMethod = this.changeItemHandleMethod
                    };

                    detail.GenerateId();  ////不预生成ID，dev在构建树时报Duplicated primary key异常
                    DetailList.Add(detail);
                }

                if (keyItem.SourceType == LoadItemSourceType.SN) //半成品
                {
                    LoadRepairDetailViewModel(keyItem.SourceCode, true);
                }
            }
        }

        /// <summary>
        /// 加载工作单元数据
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();
            RefreshLoadItem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collectBarcode"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        protected override ProductInfo Validate(CollectBarcode collectBarcode, Workcell workcell)
        {
            var product = Controller.Validate(collectBarcode, workcell);
            if (product.WorkOrderId != WorkOrderId && product.WorkOrderId != 0)
            {
                var wo = RF.GetById<WorkOrder>(product.WorkOrderId);
                if (wo == null)
                {
                    ShowError("条码工单不存在或无工单权限".L10N());
                }
                if (WorkOrder != null)
                {
                    ShowError("工单已切换,由[{0}]切换到[{1}]".L10nFormat(WorkOrder.No, wo.No));
                }
                WorkOrder = wo;
                Controller.ChangeWipResourceWorkOrder(wo.Id, workcell);
                RT.EventBus.Publish(new ChangeWipResourceWorkOrderEvent { WorkOrderId = product.WorkOrderId });
                UpdateWorkOrdeReportModel(wo.Id);
            }

            ValidateTaskReport(product.WorkOrderId, workcell);
            return product;
        }

        /// <summary>
        /// 维修采集
        /// </summary>
        /// <param name="upRoutingProcessId">上线工序ID</param>
        public virtual void Submit(double? upRoutingProcessId = null)
        {
            try
            {
                var collectData = new CollectData();
                PrepareDefectData();
                //设置过站记录状态(Start=>MoveIn,Finish=>MoveOut)
                collectData.State = WipProductProcessState;

                var workcell = GetWorkcell();
                if (collectData.State == WipProductProcessState.Finish)
                {
                    PrepareCollectData(collectData, workcell);
                }
                Controller.Collect(SubmitBarcode.Code, collectData, workcell, upRoutingProcessId);

                PrintBindingSn();

                RefreshStatistics();

                AddDetail(SubmitBarcode);

                var barcode = SubmitBarcode.Code;

                RefreshLoadItem();
                var version = Controller.GetCurrentVersion(SubmitBarcode.Code);
                Reset(ResetType.Success);
                if (WipProductProcessState == WipProductProcessState.Finish)
                {
                    //维修完成解除暂停
                    if (version != null)
                    {
                        version.IsPause = YesNo.No;
                        RF.Save(version);
                    }

                    var newUnLoadItemCode = collectData.Context["UNLOADITEM_NEWITEMLABEL"]?.ToString();//获取维修下料序列号产生的新标签
                    if (newUnLoadItemCode.IsNullOrEmpty())
                    {
                        ShowTips("[{0}]维修完成，请扫描条码".L10nFormat(barcode));
                    }
                    else
                    {
                        ShowTips("换料下料过程产生新物料标签 {1}。[{0}]维修完成，请扫描条码".L10nFormat(barcode, newUnLoadItemCode.TrimEnd(',')));
                    }
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
                    PanelInfo.BindingMode = SIE.MES.WIP.Models.BindingMode.Manual;
                    ShowTips("拼板码[{0}]可绑定{1}个产品，请扫描待绑定的第{2}个产品条码".L10nFormat(PanelInfo.PanelCode, PanelInfo.NeetToBindingQty, PanelInfo.SnList.Count + 1));
                }
                else
                    throw;
            }
        }

        /// <summary>
        /// 准备采集数据集
        /// </summary>
        /// <param name="collectData">采集数据</param>
        /// <param name="workcell">工作单元</param>
        private void PrepareCollectData(CollectData collectData, Workcell workcell)
        {
            //将未上料的条码进行上料操作，再进行换料
            var loadItems = DetailList.SelectMany(p => p.ChangeItemViewModelList)
                .Where(p => !p.IsLoadItem);

            if (loadItems.Any())
            {
                foreach (var item in loadItems)
                {
                    try
                    {
                        //已经上过料不再上料操作
                        if (LoadItemList.FindIndex(m => m.SourceCode == item.LoadItemBarcodeInfo.Barcode) >= 0)
                        {
                            continue;
                        }
                        if (item.LoadItemBarcodeInfo.WipWorkOrderId == 0)
                        {
                            item.LoadItemBarcodeInfo.WipWorkOrderId = this.WorkOrderId.Value;
                        }
                        RT.Service.Resolve<LoadItemController>()
                            .NewLoadItem(item.LoadItemBarcodeInfo, workcell, validateCurrentProcessBom: false);

                    }
                    catch (Exception exc)
                    {
                        exc.Alert();
                    }
                }

                //刷新上料
                RefreshLoadItem();
            }

            foreach (var productAssemblyDetailViewModel in DetailList)
            {
                var useItem = new RepairUseItem()
                {
                    Sn = productAssemblyDetailViewModel.KeyItem.SourceCode,
                    SoureKeyItemId = productAssemblyDetailViewModel.KeyItem.Id,
                    HandleMethod = productAssemblyDetailViewModel.HandleMethod,
                    WorkOrderId = productAssemblyDetailViewModel.WorkOrderId,
                };

                productAssemblyDetailViewModel.ChangeItemViewModelList.ForEach(
                    e =>
                    {
                        var loadItem = LoadItemList.FirstOrDefault(p => p.SourceCode == e.ChangeSn);

                        if (loadItem == null)
                        {
                            throw new ValidationException("上料列表未找到条码{0}".L10nFormat(e.ChangeSn));
                        }

                        useItem.UserItems.Add(loadItem.Id, e.ChangeQty);
                    });

                collectData.RepairUseItems.Add(useItem);
            }

            collectData.RepairDefects.AddRange(RepairDefectList.Select(p => new RepairDefect
            {

                IsFixed = p.IsFixed,
                ProductDefectId = p.WipProductDefectId,
                Remark = p.Remark,
                Responsiblities = p.ResponsibilityList,
                Measures = p.MeasureList,
            }));

            //PrepareDefectData();
        }

        /// <summary>
        /// 暂存维修记录
        /// </summary>
        public virtual void SaveRepairRecord()
        {
            PrepareDefectData();
            var defects = new EntityList<WipProductDefect>();
            defects.AddRange(RepairDefectList.Select(p => p.WipProductDefect));
            RT.Service.Resolve<RepairController>().SaveRepairRecord(defects);
        }

        /// <summary>
        /// 准备缺陷责任，维修措施数据
        /// </summary>
        private void PrepareDefectData()
        {
            RepairDefectList.ForEach(e =>
            {
                e.WipProductDefect.ResponsibilityList.MarkSaved();
                e.WipProductDefect.MeasureList.MarkSaved();
                var responsibilities = new EntityList<DefectResponsibility>();
                responsibilities.AddRange(e.ResponsibilityList);
                var measures = new EntityList<RepairMeasure>();
                measures.AddRange(e.MeasureList);
                ////原有缺陷责任
                e.WipProductDefect.ResponsibilityList.ForEach(f =>
                {
                    var responsibility = responsibilities.FirstOrDefault(p => p.Id == f.DefectResponsibilityId);
                    if (responsibility == null)
                        f.PersistenceStatus = PersistenceStatus.Deleted;
                    responsibilities.Remove(responsibility);
                });
                foreach (var item in responsibilities)
                {
                    var responsibility = new WipDefectResponsibility() { DefectResponsibility = item, WipProductDefect = e.WipProductDefect, PersistenceStatus = PersistenceStatus.New };
                    e.WipProductDefect.ResponsibilityList.Add(responsibility);
                }

                e.WipProductDefect.MeasureList.ForEach(f =>
                {
                    var measure = measures.FirstOrDefault(p => p.Id == f.RepairMeasureId);
                    if (measure == null)
                        f.PersistenceStatus = PersistenceStatus.Deleted;
                    measures.Remove(measure);
                });
                foreach (var item in measures)
                {
                    var responsibility = new WipDefectMeasure() { RepairMeasure = item, WipProductDefect = e.WipProductDefect, PersistenceStatus = PersistenceStatus.New };
                    e.WipProductDefect.MeasureList.Add(responsibility);
                }

                e.WipProductDefect.IsFixed = e.IsFixed;
                e.WipProductDefect.Remark = e.Remark;
                e.IsNewAdd = false;
                if (e.WipProductDefect.Id <= 0)
                {
                    //e.IsFixed=true;
                    e.WipProductDefect.IsFixed = true;
                    RF.Save(e.WipProductDefect);
                    e.WipProductDefectId = e.WipProductDefect.Id;
                }
            });
        }

        /// <summary>
        /// 重新开始
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(ResetType.None);

            RepairDefectList.Clear();
            DetailList.Clear();
            ShowTips(IsLoadItem ? "请扫描上料".L10N() : "请扫描条码".L10N());
            SubmitBarcode = null;
            DefectItemList.Clear();
        }

        /// <summary>
        /// 能否完成
        /// </summary>
        /// <returns>能提交返回true，否则返回false</returns>
        public bool CanSubmit()
        {
            return SubmitBarcode != null
                && Workstation.EmployeeId.HasValue
                && Workstation.ProcessId.HasValue
                && Workstation.StationId.HasValue
                && Workstation.ResourceId.HasValue;
        }

        /// <summary>
        /// 刷新上料数据
        /// </summary>
        public virtual void RefreshLoadItem()
        {
            try
            {
                LoadItemList.Clear();
                var workcell = GetWorkcell();
                var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItemList(workcell.ResourceId, workcell.StationId);
                LoadItemList.MarkSaved();
                LoadItemList.AddRange(loadItems);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 刷新挪料数据
        /// </summary>
        public void RefreshMoveItem()
        {
            // 刷新挪料数据
        }

        /// <summary>
        /// 刷新下料数据
        /// </summary>
        public void RefreshUnoadItem()
        {
            try
            {
                UnloadItemList.Clear();
                var workcell = GetWorkcell();
                var unloadItems = RT.Service.Resolve<LoadItemController>().GetUnloadItemList(workcell.ProcessId, workcell.ResourceId, workcell.StationId);
                UnloadItemList.AddRange(unloadItems);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        #region 报工任务
        /// <summary>
        /// 刷新工单任务列表
        /// </summary>
        /// <param name="retrospectType"></param>
        /// <param name="lazyLoad"></param>
        public override void RefrshReportTasks(Core.Items.RetrospectType retrospectType = Core.Items.RetrospectType.Single, bool lazyLoad = true)
        {
            // 业务实现
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="workOrderId"></param>
        protected override void UpdateWorkOrdeReportModel(double workOrderId)
        {
            // 业务实现
        }
        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="workOrderId"></param>
        /// <param name="workcell"></param>
        protected override void ValidateTaskReport(double workOrderId, Workcell workcell)
        {
            // 业务实现
        }
        #endregion

        internal void RepairSubmit()
        {
            try
            {
                ClearInfos();
                ValidateRepairResult(false);
                var repairSubmit = new RepairSubmitViewModel(SubmitBarcode.Code, GetWorkcell());
                var template = new DetailsUITemplate(typeof(RepairSubmitViewModel), ViewConfig.DetailsView, ModuleKey);
                var ui = template.CreateUI();
                ui.MainView.Data = repairSubmit;
                SaveRepairResult();
                Controller.RepairSubmit(SubmitBarcode.Code, GetWorkcell(),
                    repairSubmit.OptionalPathViewModel.RoutingProcessId);

                ShowTips("提交成功".L10nFormat());

                //清空信息
                Barcode = null;
                SubmitBarcode = null;
                RepairDefectList.Clear();
                DefectList.Clear();
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }


        /// <summary>
        /// 验证维修结果
        /// </summary>
        /// <param name="isScrap"></param>
        /// <exception cref="ValidationException"></exception>
        private void ValidateRepairResult(bool isScrap)
        {
            if (string.IsNullOrEmpty(SubmitBarcode.Code))
                throw new ValidationException("暂未待维修条码，请扫描条码".L10N());
            if (isScrap)
                return;
            if (RepairDefectList.Any(p => p.Defect == null))
                throw new ValidationException("验证失败，缺陷不能为空".L10N());
            if (RepairDefectList.Any(p => p.MeasureList.Count == 0))
                throw new ValidationException("验证失败，维修措施不能为空".L10N());
            if (RepairDefectList.Any(p => p.ResponsibilityList.Count == 0))
                throw new ValidationException("验证失败，缺陷责任不能为空".L10N());

            if (this.RepairMainRecord == null)
            {
                throw new ValidationException("验证失败，当前维修记录为空".L10N());
            }
        }


        /// <summary>
        ///保存维修结果
        /// </summary>
        /// <exception cref="ValidationException"></exception>
        private void SaveRepairResult()
        {
            if (RepairMainRecord == null)
            {
                throw new ValidationException("暂未记录可提交".L10N());
            }

            foreach (var defect in RepairDefectList)
            {
                var defRecord = RepairMainRecord.RepairDefectRecordList
                    .FirstOrDefault(x => x.ProductDefectId == defect.WipProductDefectId);

                if (defRecord == null)
                {
                    defRecord = new RepairDefectRecord()
                    {
                        ProductDefectId = defect.WipProductDefectId
                    };

                    //defRecord.GenerateId();

                    RepairMainRecord.RepairDefectRecordList.Add(defRecord);
                }

                defRecord.ProcessId = defect.ProcessId;
                defRecord.Defect = defect.Defect;
                defRecord.ActualDefect = defect.ActualDefect;
                defRecord.RepairLocation = defect.RepairLocation;
                defRecord.RepairSolution = defect.RepairSolution;
                defRecord.ReloadBarcode = defect.ReloadBarcode;
                defRecord.Remark = defect.Remark;
                defRecord.IsNewAdd = defect.IsNewAdd;

                foreach (var measure in defect.MeasureList)
                {
                    if (!defRecord.MeasureList.Any(x => x.MeasureId == measure.Id))
                    {
                        defRecord.MeasureList.Add(new RepairMeasureRecord()
                        {
                            MeasureId = measure.Id
                        });
                    }
                }

                foreach (var responsibility in defect.ResponsibilityList)
                {
                    if (!defRecord.ResponseList.Any(x => x.ResponsibilityId == responsibility.Id))
                    {
                        defRecord.ResponseList.Add(new RepairResponseRecord()
                        {
                            ResponsibilityId = responsibility.Id
                        });
                    }
                }
            }

            if (RepairMainRecord.RepairDefectRecordList.Count == 0)
                throw new ValidationException("暂未记录可提交".L10N());

            //服务器保存后，更新本地对象
            RepairMainRecord = Controller.SaveRepairRecord(WorkOrder.Id, SubmitBarcode.Code, RepairMainRecord);

            RefeshDefectRecord(WorkOrder.Id, SubmitBarcode.Code);
        }

        private void RefeshDefectRecord(double workOrderId, string barcode)
        {
            var records = RT.Service.Resolve<WipProductVersionController>().GetDefectRecords(workOrderId, barcode);
            DefectsList.Clear();

            DefectsList.AddRange(records);
        }

        internal void AddDefect()
        {
            var editor = DefectControlFactory.CreateControl();
            editor.AllowMultiple = true;
            editor.AllowQty = false;

            //缺陷代码列表
            editor.Defects.AddRange(this.DefectList);

            editor.SelectedValue.Clear();

            //已选择的缺陷，在弹出框的已选择缺陷中加入 弹出窗不加载原有的
            //this.DefectItemList.ForEach(e =>
            //{
            //    var item = new Controls.DefectItem()
            //    {
            //        Defect = e.Defect
            //    };

            //    editor.SelectedValue.Add(item);
            //});

            var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), editor, w =>
            {
                w.Title = "不良代码录入".L10N();
                w.Height = 500;
                w.Width = 750;
                w.MinHeight = 350;
                w.MinWidth = 400;
            });

            if (result == 0)
            {
                try
                {
                    if (editor.SelectedValue.Any())
                    {
                        var workcell = this.GetWorkcell();
                        var currentTime = RF.Find<WipProductProcess>().GetDbTime();
                        var shift = RT.Service.Resolve<WipResourceController>()
                            .GetWipResourceShift(workcell.ResourceId, currentTime);
                        var workcellProcess = RF.GetById<Process>(workcell.ProcessId);

                        var version = Controller.GetCurrentVersion(SubmitBarcode.Code);
                        //已选择的缺陷 
                        this.DefectItemList.Clear();
                        editor.SelectedValue
                            .Select(p => new Controls.DefectItem
                            {
                                Defect = p.Defect,
                                Qty = p.Qty,
                            }).ForEach(f =>
                            {
                                this.DefectItemList.Add(f);
                                var repairDefect = new TemporaryRepairDefectViewModel()
                                {
                                    WipProductDefect = new WipProductDefect()
                                    {
                                        DefectId = f.Defect.Id,
                                        Defect = f.Defect,
                                        IsFixed = false,
                                        NgQty = (decimal)f.Qty,
                                        StationId = workcell.StationId,
                                        ProcessId = workcell.ProcessId,
                                        ResourceId = workcell.ResourceId,
                                        ShiftId = shift.Id,
                                        VersionId = version.Id
                                    },

                                    ActualDefectId = f.Defect.Id,
                                    DefectCode = f.Defect.Code,
                                    DefectDesc = f.Defect.Description,
                                    DefectId = f.Defect.Id,
                                    IsNewAdd = true,
                                    IsFixed = false
                                };
                                if (workcellProcess != null)
                                {
                                    repairDefect.ProcessId = workcellProcess.Id;
                                    repairDefect.ProcessName = workcellProcess.Name;
                                    repairDefect.IsNewAdd = true;
                                    repairDefect.WipProductDefect.Process = workcellProcess;
                                    repairDefect.GenerateId();
                                }
                                RepairDefectList.Add(repairDefect);

                            });
                    }
                }
                catch (Exception exc)
                {
                    exc.Alert();
                }
            }
        }
    }
}