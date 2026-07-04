using DevExpress.Xpf.CodeView;
using SIE.Common.Configs;
using SIE.Core.Barcodes;
using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Assemlys;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Runtime;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Wpf.MES.LoadItems;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.MES.WIP.Assemblys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.MES.BatchWIP.Assemblys
{
    /// <summary>
    /// 批次上料采集视图模型
    /// </summary>
    [RootEntity]
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [Label("批次上料采集")]
    public class BatchAssemblyViewModel : BatchDataCollectionViewModel<BatchAssemblyController>, ILoadableItem
    {
        /// <summary>
        /// 批次采集步骤
        /// </summary>
        internal new virtual BatchAssemblyCollectStep Step
        {
            get { return base.Step as BatchAssemblyCollectStep; }
            set { base.Step = value; }
        }

        /// <summary>g
        /// 当前转出的批次条码（转出缺料提示时用）
        /// </summary>
        CollectBarcode _outBarcode;

        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchAssemblyViewModel()
        {
            Step = new BatchAssemblyCollectStep(this);
            InitWorkstation(ProcessType.BatchAssembly);
        }

        #region IsLoadItem 是否上料
        /// <summary>
        /// 是否上料
        /// </summary>
        [Label("上料")]
        public static readonly Property<ScanMode> IsLoadItemProperty = P<BatchAssemblyViewModel>.Register(e => e.IsLoadItem, new PropertyMetadata<ScanMode>() { PropertyChangedCallBack = (o, e) => (o as BatchAssemblyViewModel).OnIsLoadItem(e) });

        /// <summary>
        /// 是否上料属性变更事件
        /// </summary>
        /// <param name="e">变更事件参数</param>
        private void OnIsLoadItem(ManagedPropertyChangedEventArgs e)
        {
            FocuseBarcode();
            try
            {
                if (e.OldValue == null)
                {
                    return;
                }

                if ((ScanMode)e.NewValue == ScanMode.LoadItem)
                {
                    var workcell = GetWorkcell();
                    var flag = OutputBatchList.Count > 0 ? CheckProcessBom(workcell, OutputBatchList.FirstOrDefault()) : CheckProcessBom(workcell);

                    if (flag)
                    {
                        ShowTips("请扫描标签".L10N());
                    }
                    else
                    {
                        ShowTips("上料数量不足，请继续扫描标签".L10N());
                    }
                }
                else if ((ScanMode)e.NewValue == ScanMode.Input)
                {
                    Step.IsLackItem = false;
                    ShowTips("请扫描{0}".L10nFormat(Step.InputCollectStep?.BarcodeType.ToLabel().L10N()));
                }
                else
                {
                    if ((ScanMode)e.NewValue == ScanMode.Output)
                    {
                        Step.IsLackItem = false;
                        ShowTips("请扫描{0}".L10nFormat(Step.OutputCollectStep?.BarcodeType.ToLabel().L10N()));
                    }
                }
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }



        /// <summary>
        /// 是否上料
        /// </summary>
        public ScanMode IsLoadItem
        {
            get { return this.GetProperty(IsLoadItemProperty); }
            set { this.SetProperty(IsLoadItemProperty, value); }
        }
        #endregion

        #region 装配清单 AssemblyDetail
        /// <summary>
        /// 装配清单
        /// </summary>
        public static readonly ListProperty<EntityList<AssemblyDetailViewModel>> AssemblyDetailProperty = P<BatchAssemblyViewModel>.RegisterList(e => e.AssemblyDetailList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<AssemblyDetailViewModel>()
        });

        /// <summary>
        /// 装配清单
        /// </summary>
        public EntityList<AssemblyDetailViewModel> AssemblyDetailList
        {
            get { return this.GetLazyList(AssemblyDetailProperty); }
        }
        #endregion

        #region 上料 LoadItemList 
        /// <summary>
        /// 上料
        /// </summary>
        public static readonly ListProperty<EntityList<LoadItem>> LoadItemListProperty = P<BatchAssemblyViewModel>.RegisterList(e => e.LoadItemList, new ListPropertyMeta
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

        #region 下料 UnloadItemList  
        /// <summary>
        /// 下料
        /// </summary>
        public static readonly ListProperty<EntityList<UnloadItem>> UnloadItemListProperty = P<BatchAssemblyViewModel>.RegisterList(e => e.UnloadItemList, new ListPropertyMeta
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

        #region 工位挪料 MoveItemList 
        /// <summary>
        /// 工位挪料
        /// </summary>
        public static readonly ListProperty<EntityList<MoveItem>> MoveItemListProperty = P<BatchAssemblyViewModel>.RegisterList(e => e.MoveItemList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<MoveItem>()
        });

        /// <summary>
        /// 工位挪料
        /// </summary>
        public EntityList<MoveItem> MoveItemList
        {
            get { return this.GetLazyList(MoveItemListProperty); }
        }
        #endregion 

        /// <summary>
        /// 计时一次
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            RefreshLoadItem();
        }

        /// <summary>
        /// 上料刷新计时器设置
        /// </summary>
        protected virtual void SetRefreshTimer()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            if (Workstation.Station != null)
            {
                var config = ConfigService.GetConfig(new RefreshLoadItemConfig(), typeof(BatchAssemblyViewModel), ResourceStation.Find(Workstation.Station));
                if (config.RefreshTime > 0)
                {
                    timer.Interval = TimeSpan.FromSeconds(config.RefreshTime); //设置刷新的间隔时间
                    timer.Start();
                }
            }
        }

        /// <summary>
        /// 初始化工位信息
        /// </summary>
        protected override void LoadWorkstationData()
        {
            base.LoadWorkstationData();
            RefreshLoadItem();
            RefreshUnoadItem();
            RefreshMoveItem();
            SetRefreshTimer();
        }

        /// <summary>
        /// 刷新上料
        /// </summary>
        public void RefreshLoadItem()
        {
            try
            {
                LoadItemList.Clear();
                var workcell = GetWorkcell();
                var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItemList(workcell.ResourceId, workcell.StationId);
                LoadItemList.MarkSaved();
                LoadItemList.AddRange(loadItems);

                AddAssemblyDetail();
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 刷新挪料
        /// </summary>
        public void RefreshMoveItem()
        {
            try
            {
                MoveItemList.Clear();
                var workcell = GetWorkcell();
                var moveItems = RT.Service.Resolve<LoadItemController>().GetMoveItemList(workcell.ResourceId, workcell.StationId);
                MoveItemList.AddRange(moveItems);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 刷新下料
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


        /// <summary>
        /// 加载
        /// </summary>
        public override void Onload()
        {
            base.Onload();
            if (timer != null)
            {
                timer.Tick += new EventHandler(Timer_Tick);
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void OnClose()
        {
            if (timer != null)
            {
                if (timer.IsEnabled)
                {
                    timer.Stop();
                }
                timer.Tick -= new EventHandler(Timer_Tick);
            }
            base.OnClose();
        }

        /// <summary>
        /// 重置采集信息
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);
            IsLoadItem = ScanMode.Input;
            _outBarcode = null;
            AddAssemblyDetail();
        }

        /// <summary>
        /// 条码扫完后处理逻辑
        /// </summary>
        /// <param name="e">属性变更参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (!Barcode.IsNotEmpty())
            {
                return;
            }

            try
            {
                var workcell = GetWorkcell();
                ClearInfos();
                if (IsReceiveContainer)
                {
                    RemoveInputBatch(base.InputBatch, Barcode);
                }
                else if (IsLoadItem == ScanMode.LoadItem && !Step.IsLackItem)
                {
                    ValidateLoadItem(Barcode, workcell);
                    AddAssemblyDetail();
                }
                else if (IsLoadItem == ScanMode.Input || (IsLoadItem == ScanMode.LoadItem && Step.IsLackItem))
                {
                    AssemblyCollect(Barcode, workcell);
                }
                else
                {
                    if (IsLoadItem == ScanMode.Output)
                    {
                        var collectStep = Step.OutputCollectStep;
                        GenerateOutputBatch(Barcode, collectStep.BarcodeType);
                    }
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

        #region 装配
        /// <summary>
        /// 装配采集
        /// </summary>
        /// <param name="barcode">编码</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void AssemblyCollect(string barcode, Workcell workcell)
        {
            //缺料时上料
            if (Step.IsLackItem)
            {
                ValidateLoadItem(barcode, workcell);

                //有转出批次是验证转出批次，无转出批次时验证全部批次
                var flag = OutputBatchList.Count > 0 ? CheckProcessBom(workcell, OutputBatchList.FirstOrDefault()) : CheckProcessBom(workcell);

                if (flag)
                {
                    IsLoadItem = ScanMode.Input;
                    if (OutputBatchList.Count > 0)
                    {
                        ShowTips("请执行转出操作".L10N());
                    }
                    else
                    {
                        ShowTips("请执行出站操作".L10N());
                    }
                }
                else
                {
                    ShowTips("上料数量不足，请继续扫描标签".L10N());
                }
            }
            //入站
            else
            {

                var collectStep = Step.InputCollectStep;
                CollectBarcode collectBarcode = new CollectBarcode { Code = Barcode, Type = collectStep.BarcodeType };

                //入站
                MoveIn(collectBarcode, workcell);

                var flag = OutputBatchList.Count > 0 ? CheckProcessBom(workcell, OutputBatchList.FirstOrDefault()) : CheckProcessBom(workcell);

                //验证是否够料，缺料时切换上料
                if (flag)
                {
                    ShowTips("[{0}:{1}]成功转入，请扫描{2}".L10nFormat(collectBarcode.Type.ToLabel().L10N(), Barcode, collectBarcode.Type.ToLabel().L10N()));
                }
                else
                {
                    IsLoadItem = ScanMode.LoadItem;
                    ShowTips("上料数量不足，请继续扫描标签".L10N());
                }
            }
        }

        /// <summary>
        /// 检查是否满足过站条件
        /// </summary>
        /// <param name="outputBatch">转出批次</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>bool</returns>
        public bool CheckProcessBom(Workcell workcell, OutputBatch outputBatch = null)
        {
            AddAssemblyDetail();
            var qty = InputBatchList.Sum(p => p.RemainQty);
            if (outputBatch != null)
            {
                qty = outputBatch.Qty;
            }

            if (Step.CheckInputProcessBom(workcell, qty))
            {
                return false;
            }

            return true;
        }
        #endregion

        #region 装配清单操作验证
        /// <summary>
        /// 添加装配明细
        /// </summary>
        private void AddAssemblyDetail()
        {
            try
            {
                ResetAssemblyDetail();
                ////匹配上料列表满足扣料的条码，添加到装配明细 
                MatchLoadItems(AssemblyDetailList);
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 匹配上料
        /// </summary>
        /// <param name="details">装配清单</param> 
        private bool MatchLoadItems(EntityList<AssemblyDetailViewModel> details)
        {
            if (details == null)
            {
                return false;
            }

            bool isLackItem = false;


            foreach (var assemblyDetail in details)
            {
                if (MatchLoadItems(assemblyDetail))
                {
                    isLackItem = true;
                }
            }
            return isLackItem;
        }

        /// <summary>
        /// 匹配上料
        /// </summary>
        /// <param name="assemblyDetail">装配件</param>
        /// <returns>大于0缺料</returns>
        private bool MatchLoadItems(AssemblyDetailViewModel assemblyDetail)
        {
            decimal lackQty = assemblyDetail.Qty;   //批次总需求数
            if (lackQty == 0m)
            {
                return false;
            }

            //2、主料匹配
            lackQty = MatchLoadItems(assemblyDetail.ItemId, assemblyDetail.ItemExtProp, lackQty, assemblyDetail);

            if (lackQty == 0)
            {
                return false;
            }

            //3、替代料匹配 
            foreach (var altItem in assemblyDetail.AltItemList)
            {
                lackQty = MatchLoadItems(altItem.ItemId, altItem.ItemExtProp, lackQty, assemblyDetail);
            }

            return lackQty > 0;
        }

        /// <summary>
        /// 匹配上料
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="itemExtPro">物料扩展属性</param>
        /// <param name="assemblyDetail">装配件</param>
        /// <param name="lackQty">缺料数量</param>
        /// <returns>剩余缺料数量</returns>
        private decimal MatchLoadItems(double itemId, string itemExtPro, decimal lackQty, AssemblyDetailViewModel assemblyDetail)
        {
            //过站时，按工序BOM的用量扣上料记录的数据，只扣上料记录中工单符合的物料标签。
            var loadItemList = LoadItemList.Where(x => WorkOrder != null
                    && x.WorkOrderId == WorkOrder.Id
                    && x.ItemExtProp == itemExtPro
                    && x.ItemId == itemId
                    && x.Qty > 0);

            if (loadItemList.Any())
            {
                foreach (LoadItem item in loadItemList)
                {
                    if (lackQty == 0m)
                    {
                        break;
                    }

                    var useQty = 0m;
                    decimal qty = item.Qty; //可用数量

                    if (qty >= lackQty)
                    {
                        useQty = lackQty;
                    }
                    else
                    {
                        useQty = qty;
                    }

                    assemblyDetail.ItemLabel = item.SourceCode;
                    assemblyDetail.RemainQty = qty - useQty;
                    lackQty -= useQty;
                }
            }

            return lackQty;
        }

        /// <summary>
        /// 重置装配清单
        /// </summary>
        private void ResetAssemblyDetail()
        {
            AssemblyDetailList.Clear();

            var workcell = GetWorkcell();
            if (workcell != null)
            {
                //转入批次数量
                var inputBatchQty = this.InputBatchList.Sum(p => p.RemainQty);

                var version = RT.Service.Resolve<WipProductVersionController>().GetWipProductVersion(Barcode);
                var product = RT.Service.Resolve<RuntimeController>().FindProduct(version?.Product?.Puid);
                if (version != null && product != null)
                {
                    var process = product.Routing.Processes.FirstOrDefault(p => p.ProcessId == workcell.ProcessId);
                    if (process != null)
                    {
                        process.Boms.Where(p => p.IsBuckleMaterial).ForEach(p =>
                        {
                            decimal totalQty = p.Qty * inputBatchQty;
                            var detail = new AssemblyDetailViewModel()
                            {
                                ItemId = p.ItemId,
                                DemandQty = p.Qty,
                                Qty = totalQty,
                                RemainQty = -totalQty,
                                ItemExtProp = p.ItemExtProp,
                                ItemExtPropName=p.ItemExtPropName
                            };

                            p.AltBom.ForEach(f =>
                            {
                                detail.AltItemList.Add(new AltItemViewModel
                                {
                                    ItemId = f.ItemId,
                                    ItemExtProp = f.ItemExtProp,
                                });
                            });

                            AssemblyDetailList.Add(detail);
                        });
                        return;
                    }
                }

                if (WorkOrder == null)
                {
                    return;
                }

                var boms = WorkOrder.ProcessBomList.Where(p => p.ProcessId == workcell.ProcessId);
                boms.Where(x => !x.IsAlternative).ForEach(processBom =>
                {
                    var strSingleQty = processBom.SingleQty.ToString();
                    var decimalNumUse = strSingleQty.Substring(strSingleQty.IndexOf('.') + 1).Length;
                    var decimalNum = processBom.Item.UnitPrecision==null? decimalNumUse : processBom.Item.UnitPrecision.Value;
                    //先遍历主料
                    decimal totalQty =Math.Round(processBom.SingleQty * inputBatchQty, decimalNum, MidpointRounding.AwayFromZero);
                    var detail = new AssemblyDetailViewModel()
                    {
                        ItemId = processBom.ItemId,
                        DemandQty = processBom.SingleQty,
                        Qty = totalQty,
                        RemainQty = -totalQty,
                        ItemExtProp = processBom.ItemExtProp,
                        ItemExtPropName=processBom.ItemExtPropName
                    };

                    //同替代组的替代料
                    boms.Where(x => x.IsAlternative && x.Alter == processBom.Alter).ForEach(f =>
                    {
                        detail.AltItemList.Add(new AltItemViewModel
                        {
                            ItemId = f.ItemId,
                            ItemExtProp = f.ItemExtProp,
                        });
                    });

                    AssemblyDetailList.Add(detail);
                });
            }
        }

        /////// <summary>
        /////// 添加装配物料标签
        /////// </summary>
        /////// <param name="barcode">条码</param>
        /////// <param name="itemId">物料Id</param>
        /////// <param name="qty">数量</param>
        /////// <param name="outQty">装配数量</param>
        /////// <returns>true表示不缺料，不允许再装配物料</returns>
        ////private void AddAssemblyLabel(string barcode, double itemId, decimal qty, decimal? outQty = null)
        ////{
        ////    foreach (var detail in AssemblyDetailList)
        ////    {
        ////        bool isMatch = false;
        ////        var validateQty = detail.Qty;
        ////        if (outQty != null)
        ////            validateQty = (decimal)outQty * detail.DemandQty;
        ////        if (detail.ItemId == itemId && validateQty - detail.RemainQty > 0)
        ////        {
        ////            detail.RemainQty += qty;
        ////            isMatch = true;
        ////        }

        ////        foreach (var altItem in detail.AltItemList)
        ////        {
        ////            if (altItem.ItemId == itemId && validateQty - detail.RemainQty > 0)
        ////            {
        ////                detail.RemainQty += qty;
        ////                isMatch = true;
        ////            }
        ////        }
        ////        if (isMatch)
        ////        {
        ////            var itemLabels = detail.ItemLabel.Split(';');
        ////            if (!itemLabels.Contains(barcode))
        ////                detail.ItemLabel = string.Join(";",new string[] { detail.ItemLabel, barcode });
        ////        }
        ////    }
        ////}

        #endregion

        #region 上料

        /// <summary>
        /// 上料验证
        /// </summary>
        /// <param name="barcode">编码</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void ValidateLoadItem(string barcode, Workcell workcell)
        {
            Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType = new Dictionary<LoadItemSourceType, bool>();
            //批次上料采集不能上料【单体条码】
            //dicLoadItemSourceType.Add(LoadItemSourceType.SN, true);

            LoadItemBarcodeInfo barcodeInfo = LoadItemHelper
                .GetLoadBarcodeInfo(barcode, workcell, dicLoadItemSourceType, WorkOrderId);

            //物料匹配 保存上料                 
            try
            {
                RT.Service.Resolve<LoadItemController>().NewLoadItem(barcodeInfo, workcell);

                //刷新上料
                RefreshLoadItem();
                ShowTips("[{0}]上料成功".L10nFormat(barcode));
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        #endregion

        /// <summary>
        /// 转出操作
        /// </summary>
        /// <param name="batch">转出批次</param>
        internal override void BatchOutput(OutputBatch batch)
        {
            try
            {
                GetWorkcell();
                EntityList<AssemblyDetailViewModel> details = new EntityList<AssemblyDetailViewModel>();
                details.AddRange(AssemblyDetailList);
                details.ForEach(p =>
                {
                    p.Qty = batch.Qty * p.DemandQty;
                    p.RemainQty = -batch.Qty * p.DemandQty;
                });
                if (MatchLoadItems(details))
                {
                    IsLoadItem = ScanMode.LoadItem;
                    FocuseBarcode();
                    throw new LackItemException("缺料，请补充上料！");
                }

                base.BatchOutput(batch);
            }
            catch (Exception exc)
            {
                if (exc is LackItemException)
                {
                    _outBarcode = new CollectBarcode { Type = batch.BarcodeType };
                    if (batch.BarcodeType == BarcodeType.ContainerNo)
                    {
                        _outBarcode.Code = batch.ContainerNo;
                    }
                    else
                    {
                        _outBarcode.Code = batch.SubBatchNo.IsNotEmpty() ? batch.SubBatchNo : batch.BatchNo;
                    }
                }

                throw;
            }

            _outBarcode = null;
            RefreshLoadItem();
        }

        /// <summary>
        /// 刷新转入批次
        /// </summary>
        /// <param name="outputBatch">转出批次</param>
        internal override void RefreshInputBatch(OutputBatch outputBatch = null)
        {
            base.RefreshInputBatch(outputBatch);
            AddAssemblyDetail();
        }

        /// <summary>
        /// 移除出站批次事件
        /// </summary>
        /// <param name="outputBatch"></param>
        internal override void RemoveOutBatch(OutputBatch outputBatch)
        {
            base.RemoveOutBatch(outputBatch);
            _outBarcode = null;
            AddAssemblyDetail();
        }

        /// <summary>
        /// 初始化采集数据
        /// </summary>
        /// <param name="collectData">采集数据</param>
        protected override void InitializedCollectData(CollectData collectData)
        {
            base.InitializedCollectData(collectData);

            if (_outBarcode != null)
            {
                collectData.CollectBarcode = _outBarcode;
            }

            collectData.LoadItems.AddRange(LoadItemList);
        }
    }

    /// <summary>
    /// 上料采集步骤
    /// </summary>
    public class BatchAssemblyCollectStep : BatchCollectStep
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vm">采集视图模型</param>
        public BatchAssemblyCollectStep(BatchAssemblyViewModel vm) : base(vm)
        {
        }

        /// <summary>
        /// 是否缺料
        /// </summary>
        public bool IsLackItem { get; set; }

        /// <summary>
        /// 入站检查BOM是否缺料
        /// </summary>
        /// <param name="worcell">工作单元</param>
        /// <param name="batchQty">验证批次数量</param>
        /// <returns>bool</returns>
        public bool CheckInputProcessBom(Workcell worcell, decimal batchQty)
        {
            var viewModel = this._viewModel as BatchAssemblyViewModel;

            if (viewModel == null || viewModel.AssemblyDetailList == null)
            {
                return true;
            }
            else
            {
                if (viewModel.AssemblyDetailList.Count <= 0)
                {
                    return false;
                }
            }

            if (viewModel.AssemblyDetailList.Any(p => p.RemainQty < 0))
            {
                IsLackItem = true;
                //return IsLackItem;
            }
            else
            {
                IsLackItem = false;
            }

            return IsLackItem;
        }
    }
}