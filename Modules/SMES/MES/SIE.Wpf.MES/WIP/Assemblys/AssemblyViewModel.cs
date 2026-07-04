using DevExpress.Data.Extensions;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Core.Barcodes;
using SIE.Defects;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.EventMessages.MES.Barcodes;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Assemblys;
using SIE.MES.WIP.Configs;
using SIE.MES.WIP.Models;
using SIE.MES.WIP.Runtime;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Wpf.MES.LoadItems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace SIE.Wpf.MES.WIP.Assemblys
{
    /// <summary>
    /// 上料采集
    /// </summary>
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    [EntityWithConfig(typeof(RefreshLoadItemConfig))]
    //[EntityWithConfig(typeof(StationCallMaterialConfig))]
    [EntityWithConfig(typeof(PanelBindingSnConfig))]
    [RootEntity, Serializable]
    [Label("上料采集")]
    public partial class AssemblyViewModel : DataCollectionViewModel<AssemblyController>, ILoadableItem
    {
        /// <summary>
        /// 装配ViewModel
        /// </summary>
        public AssemblyViewModel()
        {
            InitWorkstation(ProcessType.Assembly);
            AssemblyItemsDictionary = new Dictionary<string, LoadItemBarcodeInfo>();
        }

        #region IsLoadItem 是否上料
        /// <summary>
        /// 是否上料
        /// </summary>
        public static readonly Property<bool> IsLoadItemProperty = P<AssemblyViewModel>.Register(e => e.IsLoadItem, new PropertyMetadata<bool>() { PropertyChangedCallBack = (o, e) => (o as AssemblyViewModel).OnIsLoadItem(e) });

        /// <summary>
        /// 是否上料属性变更事件
        /// </summary>
        /// <param name="e">变更事件参数</param>
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

        /// <summary>
        /// 条码扫描后处理逻辑
        /// </summary>
        /// <param name="e">属性变更事件参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (!Barcode.IsNotEmpty())
            {
                return;
            }

            try
            {
                var workcell = GetWorkcell();
                if (IsLoadItem)
                {
                    ClearInfos();

                    //验证并上料
                    ValidateLoadItem(Barcode, workcell);
                }
                else
                {

                    if (IsSnCollected()
                        && Step.Barcodes.First() != Barcode)
                    {
                        ShowError("生产条码【{0}】未完成装配采集，请继续或点击【重新开始】进行新条码的装配采集！"
                            .L10nFormat(this.Step.Barcodes.First()));

                        return;
                    }

                    ClearInfos();

                    AssemblyCollect(Barcode, workcell);
                }
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
            finally
            {
                Barcode = null;
            }
        }

        /// <summary>
        /// SN已经采集
        /// </summary>
        /// <returns></returns>
        private bool IsSnCollected()
        {
            return this.Step != null && this.Step.Barcodes != null && this.Step.Barcodes.Any();
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">变更事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == AssemblyViewModel.IsLoadItemProperty && e.OldValue != null)
            {
                //上料、采集切换时重置
                ResetTips();
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
        /// 加载数据
        /// </summary>
        public override void Onload()
        {
            timer.Tick += new EventHandler(Timer_Tick);

            base.Onload();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
            if (timer.IsEnabled)
            {
                timer.Stop();
            }
            timer.Tick -= new EventHandler(Timer_Tick);
        }

        /// <summary>
        /// 重置采集信息
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);

            AssemblyItemsDictionary.Clear();
            AssemblyDetailList.Clear();
            IsLoadItem = false;
            DisplayBarCode = string.Empty;
        }

        /// <summary>
        /// 重置提示信息
        /// </summary>
        private void ResetTips()
        {
            ShowTips(IsLoadItem ? "请扫描上料条码".L10N() : "请扫描生产条码".L10N());
        }

        #region 装配清单上料挪料下料
        /// <summary>
        /// 计时器
        /// </summary>
        DispatcherTimer timer = new DispatcherTimer();

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
                var config = ConfigService.GetConfig(new RefreshLoadItemConfig(), typeof(AssemblyViewModel), ResourceStation.Find(Workstation.Station));
                if (config.RefreshTime > 0)
                {
                    timer.Interval = TimeSpan.FromSeconds(config.RefreshTime); //设置刷新的间隔时间
                    timer.Start();
                }
            }
        }

        #region 装配清单 AssemblyDetail
        /// <summary>
        /// 装配清单
        /// </summary>
        public static readonly ListProperty<EntityList<AssemblyDetailViewModel>> AssemblyDetailProperty = P<AssemblyViewModel>.RegisterList(e => e.AssemblyDetailList, new ListPropertyMeta
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

        #region LoadItemList 上料
        /// <summary>
        /// 上料
        /// </summary>
        public static readonly ListProperty<EntityList<LoadItem>> LoadItemListProperty = P<AssemblyViewModel>.RegisterList(e => e.LoadItemList, new ListPropertyMeta
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
        /// 下料
        /// </summary>
        public static readonly ListProperty<EntityList<UnloadItem>> UnloadItemListProperty = P<AssemblyViewModel>.RegisterList(e => e.UnloadItemList, new ListPropertyMeta
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

        #region MoveItemList 工位挪料
        /// <summary>
        /// 工位挪料
        /// </summary>
        public static readonly ListProperty<EntityList<MoveItem>> MoveItemListProperty = P<AssemblyViewModel>.RegisterList(e => e.MoveItemList, new ListPropertyMeta
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
        /// 上料字典集合
        /// </summary>
        public Dictionary<string, LoadItemBarcodeInfo> AssemblyItemsDictionary { get; set; }

        /// <summary>
        /// 上料验证
        /// </summary>
        /// <param name="barcode">编码</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void ValidateLoadItem(string barcode, Workcell workcell)
        {
            Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType = new Dictionary<LoadItemSourceType, bool>
            {
                { LoadItemSourceType.SN, true }
            };

            LoadItemBarcodeInfo barcodeInfo = LoadItemHelper
                .GetLoadBarcodeInfo(barcode, workcell, dicLoadItemSourceType, WorkOrderId);
            barcodeInfo.DisplayBarCode = DisplayBarCode;
            LocalAddLoadItem(barcode, workcell, barcodeInfo);
        }

        /// <summary>
        /// 本地上料
        /// </summary>
        /// <param name="barcode"></param>
        /// <param name="workcell"></param>
        /// <param name="barcodeInfo"></param>
        /// <exception cref="ValidationException"></exception>

        private void LocalAddLoadItem(string barcode, Workcell workcell, SIE.MES.LoadItems.LoadItemBarcodeInfo barcodeInfo)
        {
            if (IsSnCollected()) // 装配采集
            {
                /*注释：如果上料条码为生产半成品上料则只能上料一次*/
                if (AssemblyItemsDictionary.ContainsKey(barcode) && barcodeInfo.Type == LoadItemSourceType.SN)
                {
                    throw new ValidationException("SN条码{0}已上料，请扫描其他条码"
                        .L10nFormat(barcode));
                }

                if (!AssemblyDetailList.Any(p => p.ItemId == barcodeInfo.ItemId)
                    && !AssemblyDetailList.SelectMany(x => x.AltItemList).Any(p => p.ItemId == barcodeInfo.ItemId))
                {
                    throw new ValidationException("物料[{0}]非装配需求物料，请扫描其他物料条码"
                        .L10nFormat(RF.GetById<Item>(barcodeInfo.ItemId)?.Code));
                }

                //校验项目号
                var woProjectNo = WorkOrder.ProjectMaintainCode.IsNullOrEmpty() ? "*" : WorkOrder?.ProjectMaintainCode;
                var projectNo = barcodeInfo.ProjectNo.IsNullOrEmpty() ? "*" : barcodeInfo.ProjectNo;
                if (projectNo != woProjectNo)
                {
                    throw new ValidationException("标签[{0}]项目号[{1}]与工单项目号[{2}]不一致，请扫描其他物料条码".L10nFormat(barcodeInfo.Label, projectNo, woProjectNo));
                }
                //扫描SN后再上料，匹配条码是否满足装配条件
                if (ValidationAssemblyProperty(barcodeInfo.ItemId, barcodeInfo.ItemExtProp))
                {
                    AddAssemblyLabel(barcodeInfo, barcodeInfo.ItemId, barcodeInfo.ItemExtProp, barcodeInfo.Qty);

                    //SN不为空，直接扣料过站
                    AssemblyCollect(this.Step.Barcodes.First(), GetWorkcell());
                }
            }
            else // 提前上料
            {
                //单体、半成品不提前上料
                if (barcodeInfo.Type == LoadItemSourceType.SN)
                {
                    throw new ValidationException("半成品[{0}]不能提前上料".L10nFormat(barcodeInfo.Barcode));
                }

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
        }


        /// <summary>
        /// 添加装配标签
        /// </summary>
        /// <param name="barcodeInfo">上料信息</param>
        /// <param name="itemId">物料Id</param>
        /// <param name="itemExtProp">物料扩展属性</param>
        /// <param name="qty">数量</param>
        /// <returns>false表示不缺料，不允许再装配物料</returns>
        private void AddAssemblyLabel(LoadItemBarcodeInfo barcodeInfo, double itemId, string itemExtProp, decimal qty)
        {
            bool useBarcode = false;
            var barcode = barcodeInfo.BillNo.IsNotEmpty() ? barcodeInfo.BillNo : barcodeInfo.Barcode;
            decimal remainQty = qty;
            foreach (var detail in AssemblyDetailList)
            {
                if ((detail.ItemId == itemId && detail.ItemExtProp == itemExtProp
                        || detail.AltItemList.FirstOrDefault(p => p.ItemId == itemId && p.ItemExtProp == itemExtProp) != null)
                    && (detail.DemandQty - detail.Qty) > 0)
                {
                    var lackQty = detail.DemandQty - detail.Qty;

                    if (remainQty >= lackQty)
                    {
                        detail.Qty = detail.DemandQty;
                        detail.RemainQty = remainQty - lackQty;
                    }
                    else
                    {
                        detail.Qty += remainQty;
                        detail.RemainQty = 0m;
                    }

                    if (AssemblyItemsDictionary.ContainsKey(barcode.Trim()))
                    {
                        var exsitedBarcodeInfo = AssemblyItemsDictionary[barcode.Trim()];
                        exsitedBarcodeInfo.Qty = detail.Qty;
                    }
                    else
                    {
                        detail.ItemLabel = string.Join(";", new string[] { detail.ItemLabel, barcode });
                        AssemblyItemsDictionary.Add(barcode.Trim(), barcodeInfo);
                    }
                    useBarcode = true;
                }
            }

            if (!useBarcode)
            {
                throw new ValidationException("物料[{0}]已满足装配，请扫描其他物料条码"
                            .L10nFormat(RF.GetById<Item>(itemId)?.Code));
            }
        }

        /// <summary>
        /// 验证装配标签是否满足扣料条件，满足扣料条件添加到装配清单,true满足
        /// </summary>
        /// <param name="itemId">物料ID</param>
        /// <param name="itemExtProp">物料属性集合</param>
        /// <returns>bool</returns>
        private bool ValidationAssemblyProperty(double itemId, string itemExtProp)
        {
            var workcell = GetWorkcell();
            if (WorkOrder.ProcessBomList
                     .Any(p => p.ItemId == itemId && p.ProcessId == workcell.ProcessId && p.ItemExtProp == itemExtProp))
            {
                return true;
            }
            else
            {
                return false;
            }
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
                LoadItemList.AddRange(loadItems);
                LoadItemList.MarkSaved();
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
        #endregion

        /// <summary>
        /// 装配采集
        /// </summary>
        /// <param name="barcode">编码</param>
        /// <param name="workcell">工作单元</param>
        protected virtual void AssemblyCollect(string barcode, Workcell workcell)
        {
            var currentStep = Step.CurrentStep;
            var collectBarcode = new CollectBarcode { Code = barcode, Type = currentStep.BarcodeType };
            collectBarcode.AssemblyItems = AssemblyItemsDictionary;

            if (!IsSnCollected())
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
                        DisplayBarCode = Barcode;
                    }
                    else
                    {
                        Controller.ValidateBarcode(collectBarcode, workcell);
                    }

                    if (Step.Barcodes.Contains(collectBarcode.Code))
                    {
                        throw new ValidationException("条码[{0}]重复采集".L10nFormat(collectBarcode.Code));
                    }

                    Step.Barcodes.Add(collectBarcode.Code);
                }
            }

            if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish
                && IsLackItem(barcode, workcell))
            {
                //当前过站状态非Start=> Move In，且验证上料不满足时，返回不提交
                if (!this.IsLoadItem)
                {
                    this.IsLoadItem = true;
                }
                return;
            }

            ////最后一步采集
            if (!Step.NextStep())
            {
                Submit(workcell, collectBarcode);
            }
            else
            {
                ShowTips("[{0}]扫描采集成功，请扫描[{1}]".L10nFormat(collectBarcode, Step.ProcessSteps.FirstOrDefault(p => p.Step == (Step.StepIndex + 1)).BarcodeType.ToLabel()));
            }
        }

        private void Submit(Workcell workcell, CollectBarcode collectBarcode)
        {
            var barcodes = Step.Barcodes.ToArray();

            try
            {
                ValidateCombinedCodeBinding();

                var collectData = new CollectData();

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
                        collectData.Defects.AddRange(from defectItem in DefectItemList
                                                     let defect = defectItem.Defect
                                                     select new DefectData
                                                     {
                                                         DefectId = defect.Id,
                                                         DefectName = defect.Description,
                                                         CategoryId = defect.DefectCategoryId,
                                                         CategoryName = defect.DefectCategory?.Description,
                                                     });
                    }
                }

                collectData.State = WipProductProcessState;
                collectData.CollectBarcode = collectBarcode;
                collectBarcode.Code = barcodes[0];
                InitCombinedCodeInfo(collectData);

                Controller.Collect(barcodes, collectData, workcell);

                PrintBindingSn();

                AddDetail(collectBarcode, collectData.Result);

                RefreshStatistics();

                this.Reset(resetType: ResetType.Success);

                RefreshLoadItem();

                RefrshReportTasks();

                if (WipProductProcessState == SIE.MES.WIP.Products.WipProductProcessState.Finish)
                {
                    ShowTips("[{0}]过站成功，请扫描条码".L10nFormat(collectBarcode));
                }
                else
                {
                    ShowTips("[{0}]入站成功，请扫描条码".L10nFormat(collectBarcode));
                }

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
                    //当前是上料模式时，不回滚
                    if (!IsLoadItem)
                    {
                        //回滚一步
                        Step.Roolback();
                    }

                    ShowError(exc);
                }
            }
            finally
            {
                AddAssemblyDetail();
            }
        }

        /// <summary>
        /// 检查是否满足过站条件
        /// </summary>
        /// <param name="barcode">条码</param>
        /// <param name="workcell">工作单元</param>
        /// <returns>true--未满足，false--满足</returns>
        public bool IsLackItem(string barcode, Workcell workcell)
        {
            if (!AssemblyDetailList.Any() && Step.Barcodes.Any())
            {
                try
                {
                    RT.Service.Resolve<AssemblyController>()
                        .ValidateProcessBom(new CollectBarcode(barcode, Step.ProcessSteps.First().BarcodeType), workcell);

                    return false;
                }
                catch (Exception exc)
                {
                    if (exc.GetBaseException() is LackItemException)
                    {
                        var baseExc = exc.GetBaseException();

                        this.IsLoadItem = true;  //切换上料 会把SN清空                        

                        this.ShowError((baseExc as LackItemException).Message);

                        this.ShowTips("请扫描上料的物料标签".L10N());
                    }
                    else
                    {
                        exc.Alert();
                    }

                    //加载产品条码在该工序的物料需求
                    AddAssemblyDetail();

                    return true;
                }
            }
            else
            {
                //验证装配清单是否满足过站
                if (AssemblyDetailList.Any(p => p.ItemLabel == null || p.DemandQty - p.Qty > 0))
                {
                    ShowTips("请扫描上料的物料标签".L10N());
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 添加装配明细
        /// </summary>
        private void AddAssemblyDetail()
        {
            ResetAssemblyDetail();

            RefreshLoadItem();

            string alterGroup = string.Empty;

            foreach (var detail in AssemblyDetailList)
            {
                //已经满足
                if (detail.DemandQty - detail.Qty <= 0)
                {
                    continue;
                }

                if (alterGroup.IsNullOrEmpty() || alterGroup == detail.AlterGroup)
                {
                    //匹配上料列表满足扣料的条码，添加到装配明细                
                    foreach (var loadItemEntity in LoadItemList.Where(p => p.WorkOrderId == WorkOrderId
                            && p.ItemId == detail.ItemId
                            && p.ItemExtProp == detail.ItemExtProp
                            && p.Qty > 0))
                    {

                        var lackQty = detail.DemandQty - detail.Qty;
                        if (loadItemEntity.Qty >= lackQty)
                        {
                            detail.Qty = detail.DemandQty;
                            //loadItemEntity.Qty = loadItemEntity.Qty - lackQty; //预扣不扣减上料明显剩余数 和数据库保持一致
                        } 
                        else
                        {
                            detail.Qty += loadItemEntity.Qty;
                            //loadItemEntity.Qty = 0;
                        }

                        detail.ItemLabel = string.Join(";", new string[] { detail.ItemLabel, loadItemEntity.SourceCode });

                        if (!detail.AlterGroup.IsNullOrEmpty())
                        {
                            alterGroup = detail.AlterGroup;
                        }

                        //已经满足
                        if (detail.DemandQty - detail.Qty <= 0)
                        {
                            break;
                        }
                    }
                }

                //已经满足
                if (detail.DemandQty - detail.Qty <= 0)
                {
                    continue;
                }

                alterGroup = UseAltItem(alterGroup, detail);
            }
        }

        /// <summary>
        /// 使用替代料
        /// </summary>
        /// <param name="alterGroup">替代组分组</param>
        /// <param name="detail">主料的物料需求</param>
        /// <returns>替代组分组</returns>
        private string UseAltItem(string alterGroup, AssemblyDetailViewModel detail)
        {
            foreach (var alt in detail.AltItemList)
            {
                if (alterGroup.IsNullOrEmpty() || alterGroup == alt.AlterGroup)
                {
                    //匹配上料列表满足扣料的条码，添加到装配明细                
                    foreach (var loadItemEntity in LoadItemList.Where(p => p.WorkOrderId == WorkOrderId
                        && p.ItemId == alt.ItemId
                        && p.ItemExtProp == alt.ItemExtProp
                        && p.Qty > 0))
                    {
                        var lackQty = detail.DemandQty - detail.Qty;
                        if (loadItemEntity.Qty >= lackQty)
                        {
                            detail.Qty = detail.DemandQty;
                            //loadItemEntity.Qty = loadItemEntity.Qty - lackQty;
                        }
                        else
                        {
                            detail.Qty += loadItemEntity.Qty;
                            //loadItemEntity.Qty = 0;
                        }

                        detail.ItemLabel = string.Join(";", new string[] { detail.ItemLabel, loadItemEntity.SourceCode });

                        if (!alt.AlterGroup.IsNullOrEmpty())
                        {
                            alterGroup = alt.AlterGroup;
                        }

                        //已经满足
                        if (detail.DemandQty - detail.Qty <= 0)
                        {
                            break;
                        }
                    }
                }

                //已经满足
                if (detail.DemandQty - detail.Qty <= 0)
                {
                    break;
                }
            }

            return alterGroup;
        }


        /// <summary>
        /// 重置装配清单
        /// </summary>
        private void ResetAssemblyDetail()
        {
            AssemblyDetailList.Clear();
            AssemblyItemsDictionary.Clear();

            var workcell = GetWorkcell();
            if (workcell == null)
            {
                return;
            }

            //查找【生产采集运行时产品】
            var product = RT.Service.Resolve<RuntimeController>().FindProduct(Barcode, Step.CurrentStep.BarcodeType);
            if (product != null)
            {
                var workOrderBoms = Controller.GetWorkOrderBom(product);
                //查找【生产采集运行时产品】后工序列表中与当前选择工序匹配的【运行时工序】
                var process = product.Routing.GetNext()
                    .FirstOrDefault(p => p.ProcessId == workcell.ProcessId);

                if (process != null)
                {
                    var isBuckleMaterialBoms = process.Boms.Where(p => p.IsBuckleMaterial);
                    foreach (var p in isBuckleMaterialBoms)
                    {   //在工序BOM存在且在工单BOM中为反冲物料的不出现在装备清单里面
                        var index = workOrderBoms.FindIndex(m => m.ItemId == p.ItemId && m.IsRecoilItem);
                        if (index >= 0)
                        {
                            continue;
                        }

                        var detail = new AssemblyDetailViewModel()
                        {
                            ItemId = p.ItemId,
                            DemandQty = p.Qty,
                            ItemExtProp = p.ItemExtProp,
                            ItemExtPropName = p.ItemExtPropName,
                            AlterGroup = p.AlterGroup
                        };

                        p.AltBom.ForEach(f =>
                        {
                            detail.AltItemList.Add(new AltItemViewModel
                            {
                                ItemId = f.ItemId,
                                ItemExtProp = f.ItemExtProp,
                                AlterGroup = f.AlterGroup
                            });
                        });

                        AssemblyDetailList.Add(detail);
                    }
                    return;
                }
            }

            if (WorkOrder == null)
            {
                return;
            }

            //直接用工单的工序BOM
            var processBoms = WorkOrder.ProcessBomList
                .Where(p => p.ProcessId == workcell.ProcessId && !p.IsAlternative).ToList();
            var woBoms = WorkOrder.BomList.ToList();
            foreach (var processBom in processBoms)
            {
                //在工序BOM存在且在工单BOM中为反冲物料的不出现在装备清单里面
                var index = woBoms.FindIndex(m => m.ItemId == processBom.ItemId && m.IsRecoilItem);
                if (index >= 0)
                {
                    continue;
                }

                //主料
                var detail = new AssemblyDetailViewModel()
                {
                    ItemId = processBom.ItemId,
                    DemandQty = processBom.SingleQty,
                    ItemExtProp = processBom.ItemExtProp,
                    ItemExtPropName = processBom.ItemExtPropName,
                };

                AssemblyDetailList.Add(detail);

                //替代料
                if (!processBom.Alter.IsNullOrEmpty())
                {
                    var processBomsOfAlter = WorkOrder.ProcessBomList
                        .Where(x => x.ProcessId == workcell.ProcessId && x.IsAlternative && x.Alter == processBom.Alter)
                        .ToList();

                    processBomsOfAlter.ForEach(f =>
                    {
                        detail.AltItemList.Add(new AltItemViewModel
                        {
                            ItemId = f.ItemId,
                            ItemExtProp = f.ItemExtProp,
                        });
                    });
                }
            }

            //组合板，需求量要乘以拼板数
            if (PanelInfo.BarcodeType == BarcodeType.CombinedCode)
            {
                AssemblyDetailList.ForEach(dtl => dtl.Qty *= (PanelInfo.PanelQty - PanelInfo.ForkPlateQty));
            }
        }
    }
}