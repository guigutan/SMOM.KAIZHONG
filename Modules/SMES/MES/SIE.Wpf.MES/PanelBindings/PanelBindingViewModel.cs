using SIE.Barcodes;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.PanelBindings;
using SIE.MES.WIP.Configs;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.MES.PanelBindings
{
    /// <summary>
    /// 工单条码绑定视图模型
    /// </summary>
    [RootEntity]
    [Label("工单条码绑定视图模型")]
    [EntityWithConfig(typeof(DevicePortConfig))]
    [EntityWithConfig(typeof(SerialPortsConfig))]
    public class PanelBindingViewModel : DataCollectionViewModel<PanelBindingController>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PanelBindingViewModel()
        {
            CurrentSnList = new List<SnViewModel>();
            DicSnItemList = new Dictionary<string, double>();
            UnBindingPanel = false;
            OperatorName = RF.GetById<Employee>(RT.IdentityId)?.Name;
        }

        #region 不绑定拼板码 UnBindingPanel
        /// <summary>
        /// 不绑定拼板码
        /// </summary>
        [Label("不绑定拼板码")]
        public static readonly Property<bool> UnBindingPanelProperty = P<PanelBindingViewModel>.Register(e => e.UnBindingPanel);

        /// <summary>
        /// 不绑定拼板码
        /// </summary>
        public bool UnBindingPanel
        {
            get { return this.GetProperty(UnBindingPanelProperty); }
            set { this.SetProperty(UnBindingPanelProperty, value); }
        }
        #endregion

        #region Skip数 ForkPlateQty
        /// <summary>
        /// 叉板数
        /// </summary>
        [Label("Skip数")]
        public static readonly Property<int> ForkPlateQtyProperty = P<PanelBindingViewModel>.Register(e => e.ForkPlateQty);

        /// <summary>
        /// Skip数
        /// </summary>
        public int ForkPlateQty
        {
            get { return this.GetProperty(ForkPlateQtyProperty); }
            set { this.SetProperty(ForkPlateQtyProperty, value); }
        }
        #endregion

        #region 等待板号输入 WaitBoardNo
        /// <summary>
        /// 等待板号输入
        /// </summary>
        [Label("等待板号输入")]
        public static readonly Property<bool> WaitBoardNoProperty = P<PanelBindingViewModel>.Register(e => e.WaitBoardNo);

        /// <summary>
        /// 等待板号输入
        /// </summary>
        public bool WaitBoardNo
        {
            get { return this.GetProperty(WaitBoardNoProperty); }
            set { this.SetProperty(WaitBoardNoProperty, value); }
        }
        #endregion 

        #region Skip板号 BoardNo
        /// <summary>
        /// 叉板板号
        /// </summary>
        [Label("Skip板号")]
        public static readonly Property<string> BoardNoProperty = P<PanelBindingViewModel>.Register(e => e.BoardNo);

        /// <summary>
        /// Skip板号
        /// </summary>
        public string BoardNo
        {
            get { return this.GetProperty(BoardNoProperty); }
            set { this.SetProperty(BoardNoProperty, value); }
        }
        #endregion 

        #region 拼板码列表 PanelList
        /// <summary>
        /// 拼板码列表
        /// </summary>
        [Label("拼板码列表")]
        public static readonly ListProperty<EntityList<PanelViewModel>> PanelListProperty = P<PanelBindingViewModel>.RegisterList(e => e.PanelList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as PanelBindingViewModel).LoadPanelList()
        });

        /// <summary>
        /// 拼板码列表
        /// </summary>
        public EntityList<PanelViewModel> PanelList
        {
            get { return this.GetLazyList(PanelListProperty); }
        }

        /// <summary>
        /// 加载拼板码列表
        /// </summary>
        private EntityList<PanelViewModel> LoadPanelList()
        {
            return new EntityList<PanelViewModel>();
        }
        #endregion

        #region SN条码列表 SnList
        /// <summary>
        /// SN条码列表
        /// </summary>
        [Label("条码列表")]
        public static readonly ListProperty<EntityList<SnViewModel>> SnListProperty = P<PanelBindingViewModel>.RegisterList(e => e.SnList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as PanelBindingViewModel).LoadSnList()
        });

        /// <summary>
        /// SN条码列表
        /// </summary>
        public EntityList<SnViewModel> SnList
        {
            get { return this.GetLazyList(SnListProperty); }
        }

        /// <summary>
        /// 加载SN条码列表
        /// </summary>
        private EntityList<SnViewModel> LoadSnList()
        {
            return new EntityList<SnViewModel>();
        }
        #endregion

        #region 电子行业工单 WorkOrder 
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单")]
        public new static readonly IRefIdProperty WorkOrderIdProperty =
            P<PanelBindingViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单ID
        /// </summary>
        public new double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public new static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<PanelBindingViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public new WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 当前正在扫描的拼板码 CurrentPanel
        /// <summary>
        /// 当前正在扫描的拼板码
        /// </summary>
        [Label("当前正在扫描的拼板码")]
        public static readonly Property<PanelViewModel> CurrentPanelProperty = P<PanelBindingViewModel>.Register(e => e.CurrentPanel);

        /// <summary>
        /// 当前正在扫描的拼板码
        /// </summary>
        public PanelViewModel CurrentPanel
        {
            get { return this.GetProperty(CurrentPanelProperty); }
            set { this.SetProperty(CurrentPanelProperty, value); }
        }
        #endregion

        #region 操作人名称 OperatorName
        /// <summary>
        /// 操作人名称
        /// </summary>
        [Label("操作人名称")]
        public static readonly Property<string> OperatorNameProperty = P<PanelBindingViewModel>.Register(e => e.OperatorName);

        /// <summary>
        /// 操作人名称
        /// </summary>
        public string OperatorName
        {
            get { return this.GetProperty(OperatorNameProperty); }
            set { this.SetProperty(OperatorNameProperty, value); }
        }
        #endregion

        #region 可绑定产品数量 CanBindQty
        /// <summary>
        /// 可绑定产品数量
        /// </summary>
        [Label("可绑定产品数量")]
        public static readonly Property<int?> CanBindQtyProperty = P<PanelBindingViewModel>.Register(e => e.CanBindQty);

        /// <summary>
        /// 可绑定产品数量
        /// </summary>
        public int? CanBindQty
        {
            get { return this.GetProperty(CanBindQtyProperty); }
            set { this.SetProperty(CanBindQtyProperty, value); }
        }
        #endregion

        #region 未绑定sn数 UnBindSnQty
        /// <summary>
        /// 未绑定sn数
        /// </summary>
        [Label("未绑定SN")]
        public static readonly Property<decimal?> UnBindSnQtyProperty = P<PanelBindingViewModel>.Register(e => e.UnBindSnQty);

        /// <summary>
        /// 未绑定sn数
        /// </summary>
        public decimal? UnBindSnQty
        {
            get { return this.GetProperty(UnBindSnQtyProperty); }
            set { this.SetProperty(UnBindSnQtyProperty, value); }
        }
        #endregion

        #region 未绑定拼板码数 UnBindPanelQty
        /// <summary>
        /// 未绑定拼板码数
        /// </summary>
        [Label("未绑定拼板码")]
        public static readonly Property<decimal?> UnBindPanelQtyProperty = P<PanelBindingViewModel>.Register(e => e.UnBindPanelQty);

        /// <summary>
        /// 未绑定拼板码数
        /// </summary>
        public decimal? UnBindPanelQty
        {
            get { return this.GetProperty(UnBindPanelQtyProperty); }
            set { this.SetProperty(UnBindPanelQtyProperty, value); }
        }
        #endregion

        #region 当前选择的拼板码 CurrentSelectPanel
        /// <summary>
        /// 当前选择的拼板码
        /// </summary>
        [Label("当前选择的拼板码")]
        public static readonly Property<string> CurrentSelectPanelProperty = P<PanelBindingViewModel>.Register(e => e.CurrentSelectPanel);

        /// <summary>
        /// 当前选择的拼板码
        /// </summary>
        public string CurrentSelectPanel
        {
            get { return this.GetProperty(CurrentSelectPanelProperty); }
            set { this.SetProperty(CurrentSelectPanelProperty, value); }
        }
        #endregion

        /// <summary>
        /// 当前扫描的条码列表
        /// </summary>
        public List<SnViewModel> CurrentSnList { get; set; }

        /// <summary>
        /// 绑定前的条码和条码对应工单的产品Id字典
        /// </summary>
        public Dictionary<string, double> DicSnItemList { get; set; }

        /// <summary>
        /// 条码变更
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty()) return;
            try
            {
                ClearInfos();
                if (IsWaitBoardNo())
                    return;
                if (UnBindingPanel)
                    NoPanelBinding(Barcode);
                else
                    ExistPanelBinding(Barcode);
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
        /// 验证输入
        /// </summary>
        /// <returns>等待输入返回true，否则返回false</returns>
        private bool IsWaitBoardNo()
        {
            if (WaitBoardNo)
            {
                ShowError("请录入Skip板号，多个板号用空格隔开".L10N());
                return true;
            }
            return false;
        }

        /// <summary>
        /// 有拼板码绑定
        /// </summary>
        /// <param name="barcode">扫描的条码</param>
        protected virtual void ExistPanelBinding(string barcode)
        {
            if (CurrentPanel == null)
            {
                var panel = Controller.GetPanel(barcode);

                //工单切换
                if (WorkOrder == null || (WorkOrder.Id != panel.WorkOrderId))
                {
                    WorkOrder = RF.GetById<WorkOrder>(panel.WorkOrderId);

                    SetWorkOrderData();
                }

                CurrentPanel = new PanelViewModel()
                {
                    Panel = panel.Code,
                    CanBindQty = CanBindQty ?? 0,
                    BindingQty = 0,
                };

                var sns = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodesByPanleCode(barcode);

                if (sns.Count > 0)
                {
                    DicSnItemList.Clear();

                    foreach (var sn in sns)
                    {
                        CurrentSnList.Add(new SnViewModel
                        {
                            Sn = sn.SN,
                            Qty = 1,
                            WorkOrderNo = sn.ChildWorkOrder?.No,
                            ProductCode = sn.ChildWorkOrder?.Product.Code,
                            ProducName = sn.ChildWorkOrder?.Product.Name,
                            IsBinding = YesNo.Yes
                        });

                        DicSnItemList.Add(sn.SN, sn.ChildWorkOrder.ProductId);
                    }

                    CurrentPanel.BindingQty = sns.Count;
                    CurrentPanel.BindingDate = sns.First().BindingDate;
                    CurrentPanel.OperatorName = sns.First().Operator.Name;

                    //存在绑定完成的记录，则设置当前拼板为绑定完成
                    if (sns.Any(x => x.IsBindComplete))
                    {
                        CurrentPanel.IsBindComplete = true;
                    }
                    else
                    {
                        CurrentPanel.IsBindComplete = false;
                    }
                }

                if (PanelList.Any(x => x.Panel == panel.Code))
                {
                    var existPanelViewModel = PanelList.FirstOrDefault(x => x.Panel == panel.Code);

                    existPanelViewModel.BindingQty = CurrentPanel.BindingQty;
                    existPanelViewModel.BindingDate = CurrentPanel.BindingDate;
                    existPanelViewModel.OperatorName = CurrentPanel.OperatorName;
                }
                else
                {
                    PanelList.Insert(0, CurrentPanel);
                }

                //绑定完成，不继续绑定，只带出之前绑定记录，以便做解除绑定操作
                if (CurrentPanel.IsBindComplete)
                {
                    var info = CanBindQty == null ? "" : (CanBindQty - ForkPlateQty).ToString() + "个";
                    ShowTips(UnBindingPanel ? "请扫描{0}SN条码进行绑定".L10nFormat(info) : "请扫描拼板码进行SN条码绑定".L10N());
                    CurrentPanel = null;
                    DicSnItemList.Clear();
                    CurrentSnList.Clear();
                }
                else
                {
                    ShowTips("拼板码采集成功！请扫描{0}个SN条码进行绑定".L10nFormat(CanBindQty - CurrentPanel.BindingQty - ForkPlateQty));
                }

                if (PanelList.Count > 10)
                    PanelList.RemoveAt(9);
            }
            else
                PanelBinding(barcode);
        }

        /// <summary>
        /// 无拼板码绑定
        /// </summary>
        /// <param name="barcode">扫描的条码</param>
        protected virtual void NoPanelBinding(string barcode)
        {
            if (CurrentPanel == null)
            {
                WorkOrder = Controller.GetElecWorkOrderBySn(barcode);

                if (WorkOrder.IsPanelWorkOrder)
                {
                    throw new ValidationException("数据错误，组合板工单不能打印生产条码。".L10N());
                }

                if (WorkOrder.PanelWorkOrderId.HasValue)
                {
                    throw new ValidationException("组合板工单的子产品不能使用无拼板码的方式绑定条码。".L10N());
                }

                SetWorkOrderData();

                CurrentPanel = new PanelViewModel()
                {
                    Panel = barcode,
                    CanBindQty = CanBindQty ?? 0,
                    BindingQty = 0
                };

                var sns = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodesByPanleCode(barcode);

                if (sns.Count > 0)
                {
                    DicSnItemList.Clear();

                    foreach (var sn in sns)
                    {
                        SnList.Add(new SnViewModel
                        {
                            Sn = sn.SN,
                            Qty = 1,
                            WorkOrderNo = sn.ChildWorkOrder.No,
                            ProductCode = sn.ChildWorkOrder.Product.Code,
                            ProducName = sn.ChildWorkOrder.Product.Name
                        });

                        DicSnItemList.Add(sn.SN, sn.ChildWorkOrder.ProductId);
                    }

                    CurrentPanel.BindingQty = sns.Count;
                    CurrentPanel.BindingDate = sns.First().BindingDate;
                    CurrentPanel.OperatorName = sns.First().Operator.Name;

                    //存在绑定完成的记录，则设置当前拼板为绑定完成
                    if (sns.Any(x => x.IsBindComplete))
                    {
                        CurrentPanel.IsBindComplete = true;
                    }
                    else
                    {
                        CurrentPanel.IsBindComplete = false;
                    }
                }

                if (PanelList.Any(x => x.Panel == barcode))
                {
                    var existPanelViewModel = PanelList.FirstOrDefault(x => x.Panel == barcode);

                    existPanelViewModel.BindingQty = CurrentPanel.BindingQty;
                    existPanelViewModel.BindingDate = CurrentPanel.BindingDate;
                    existPanelViewModel.OperatorName = CurrentPanel.OperatorName;
                }
                else
                {
                    PanelList.Insert(0, CurrentPanel);
                }

                //绑定完成，不继续绑定，只带出之前绑定记录，以便做解除绑定操作
                if (CurrentPanel.IsBindComplete)
                {
                    var info = CanBindQty == null ? "" : (CanBindQty - ForkPlateQty).ToString() + "个";
                    ShowTips(UnBindingPanel ? "请扫描{0}SN条码进行绑定".L10nFormat(info) : "请扫描拼板码进行SN条码绑定".L10N());
                    CurrentPanel = null;
                    DicSnItemList.Clear();
                    CurrentSnList.Clear();
                }

                if (PanelList.Count > 10)
                    PanelList.RemoveAt(9);
            }
            PanelBinding(barcode);
        }

        /// <summary>
        /// 扫描条码绑定
        /// </summary>
        /// <param name="barcode">扫描条码</param>
        private void PanelBinding(string barcode)
        {
            if (DicSnItemList.ContainsKey(barcode))
            {
                throw new ValidationException("条码：{0}已经扫描过了,请勿重新扫描！".L10nFormat(barcode));
            }

            DicSnItemList = Controller.ValidatePanelSn(barcode, WorkOrder, DicSnItemList);

            var workOrderOfSn = RT.Service.Resolve<WorkOrderController>().GetWorkOrderBySn(barcode);
            SnViewModel snViewModel = new SnViewModel
            {
                Sn = barcode,
                Qty = 1,
                WorkOrderNo = workOrderOfSn.No,
                ProductCode = workOrderOfSn.ProductCode,
                ProducName = workOrderOfSn.ProductName
            };

            if (CurrentSelectPanel == CurrentPanel.Panel)
            {
                SnList.Add(snViewModel);
            }

            CurrentSnList.Add(snViewModel);

            PanelList.Where(p => p.Panel == CurrentPanel.Panel).ForEach(p => p.BindingQty++);

            ShowTips("采集成功，请继续扫描SN条码".L10N());

            if (CurrentSnList.Count == (CurrentPanel.CanBindQty - ForkPlateQty))
            {
                PanelBinding();
            }
        }

        /// <summary>
        /// 条码绑定
        /// </summary>
        public void PanelBinding()
        {
            try
            {
                ClearInfos();
                List<int> boardNoList = ValidateBoardNo();

                //只传未绑定的条码
                var snNoList = CurrentSnList
                    .Where(x => x.IsBinding == YesNo.No)
                    .Select(x => x.Sn).ToList();

                Controller.PanelBindingSn(snNoList, boardNoList, CurrentPanel.Panel, ForkPlateQty, !UnBindingPanel, WorkOrder, CurrentPanel.CanBindQty);

                //绑定完成时，更新本地数据
                PanelList.Where(p => p.Panel == CurrentPanel.Panel)
                    .ForEach(p =>
                    {
                        p.BindingDate = DateTime.Now;
                        p.OperatorName = OperatorName;
                        p.IsBindComplete = true;
                    });

                ShowTips("绑定完成！".L10N());

                SetWorkOrderData();

                CurrentPanel = null;
                CurrentSnList.Clear();
                DicSnItemList.Clear();
                WaitBoardNo = false;
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }

        /// <summary>
        /// 验证叉板板号
        /// </summary>
        private List<int> ValidateBoardNo()
        {
            List<int> boardNoList = new List<int>();
            if (ForkPlateQty == 0)
                return boardNoList;
            if (BoardNo.IsNullOrEmpty())
            {
                WaitBoardNo = true;
                throw new ValidationException("请录入叉板板号，多个板号用空格隔开".L10N());
            }
            var boardNos = BoardNo.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            boardNos.ForEach(boardno =>
            {
                if (!int.TryParse(boardno, out int res))
                {
                    WaitBoardNo = true;
                    throw new ValidationException("叉板板号格式错误，格式：1  3".L10N());
                }
                boardNoList.Add(res);
            });
            if (ForkPlateQty != boardNoList.Count)
            {
                WaitBoardNo = true;
                throw new ValidationException("叉板板号数量必须与叉板数一致".L10N());
            }
            return boardNoList;
        }

        /// <summary>
        /// 移除拼板码
        /// </summary>
        /// <param name="panelViewModel">拼板码</param>
        public void RemovePanel(PanelViewModel panelViewModel)
        {
            if (!panelViewModel.IsBindComplete)
                ResetAssembly();
            else
            {
                Controller.RemoveBindingPanel(panelViewModel.Panel);
                SetWorkOrderData();
                ShowTips("移除绑定成功！".L10N());
            }
        }

        /// <summary>
        /// 移除Sn条码
        /// </summary>
        /// <param name="snCode">Sn条码</param>
        public void RemoveSn(string snCode)
        {
            if (DicSnItemList.ContainsKey(snCode))
            {
                //未提交绑定时，直接界面移除
                PanelList.Where(p => p.Panel == CurrentPanel.Panel).ForEach(p => p.BindingQty--);
                CurrentSnList.RemoveAll(x => x.Sn == snCode);
                RemovePanelData(true);

                //未提交时，增加处理SN与子产品物料ID的字典
                DicSnItemList.Remove(snCode);

                ShowTips("移除成功！".L10N());
                return;
            }

            //移除已提交数据中绑定的SN            
            var panelAndBarcode = Controller.RemoveBindingSn(snCode);

            PanelList.Where(p => p.Panel == panelAndBarcode.PanelCode).ForEach(p => p.BindingQty--);

            RemovePanelData(false);

            WorkOrder = panelAndBarcode.WorkOrder;

            SetWorkOrderData();
            ShowTips("移除成功！".L10N());
        }

        /// <summary>
        /// 界面移除拼板码数据
        /// </summary>
        /// <param name="isReset">是否重置采集信息</param>
        private void RemovePanelData(bool isReset)
        {
            if (PanelList.Any(p => p.BindingQty <= 0))
            {
                var removeModel = PanelList.FirstOrDefault(p => p.BindingQty <= 0);
                var removeAt = PanelList.IndexOf(removeModel);
                if (removeAt >= 0)
                    PanelList.RemoveAt(removeAt);
                if (isReset)
                    ResetAssembly();
            }
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="e">变更事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == UnBindingPanelProperty)
                ResetAssembly();
            else if (e.Property == ForkPlateQtyProperty)
                ForkPlateQtyChanged();
            else if (e.Property == CurrentSelectPanelProperty)
                CurrentSelectPanelChanged();
            if (e.Property == BoardNoProperty && WaitBoardNo && !BoardNo.IsNullOrEmpty())
            {
                PanelBinding();
            }
        }

        /// <summary>
        /// 选择的拼板码变更事件
        /// </summary>
        private void CurrentSelectPanelChanged()
        {
            SnList.Clear();
            if (CurrentSelectPanel == CurrentPanel?.Panel)
            {
                foreach (var snViewModel in CurrentSnList)
                {
                    SnList.Add(snViewModel);
                }
            }
            else
            {
                if (CurrentSelectPanel.IsNotEmpty())
                {
                    var sns = RT.Service.Resolve<PanelBindingController>().GetPanelAndBarcodesByPanleCode(CurrentSelectPanel);
                    foreach (var sn in sns)
                    {
                        SnList.Add(new SnViewModel
                        {
                            Sn = sn.SN,
                            Qty = 1,
                            WorkOrderNo = sn.ChildWorkOrder.No,
                            ProductCode = sn.ChildWorkOrder.Product.Code,
                            ProducName = sn.ChildWorkOrder.Product.Name
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 叉板数属性变更事件
        /// </summary>
        private void ForkPlateQtyChanged()
        {
            ResetAssembly();
            if (ForkPlateQty != 0)
            {
                if (CanBindQty.HasValue && (ForkPlateQty < CanBindQty))
                {
                    return;
                }

                var qty = ForkPlateQty;
                ForkPlateQty = 0;
                BoardNo = null;
                ShowError("当前输入叉板数为{0}，不可大于等于可绑定产品数量，叉板数已修正为0".L10nFormat(qty));
            }
            else
                BoardNo = null;
        }

        /// <summary>
        /// 界面刷新工单的数据
        /// </summary>
        private void SetWorkOrderData()
        {
            CanBindQty = WorkOrder.PanelQty;

            //组合板工单的待绑定子产品数量：工单拼板数 乘以 PCB物料属性明细中每个子产品的数量
            if (WorkOrder.IsPanelWorkOrder)
            {
                CanBindQty = RT.Service.Resolve<PanelBindingController>().GetPanelWorkOrderCanBindingQty(WorkOrder);
            }

            var info = RT.Service.Resolve<PanelBindingController>().GetWorkOrderPanleBindingInfo(WorkOrder.Id);
            UnBindPanelQty = info.UnBindingPanelQty;
            UnBindSnQty = info.UnBindingSnQty;
        }

        /// <summary>
        /// 重置采集信息
        /// </summary>
        public override void Reset(ResetType resetType)
        {
            base.Reset(resetType);
            ResetAssembly();
            ForkPlateQty = 0;
            BoardNo = null;
            WaitBoardNo = false;

            //清空SN与子产品物料ID的字典
            DicSnItemList.Clear();
        }

        /// <summary>
        /// 重置采集步骤
        /// </summary>
        private void ResetAssembly()
        {
            var info = CanBindQty == null ? "" : (CanBindQty - ForkPlateQty).ToString() + "个";
            ShowTips( UnBindingPanel ? "请扫描{0}SN条码进行绑定".L10nFormat(info) : "请扫描拼板码进行SN条码绑定".L10N());
            Error = string.Empty;
            CurrentPanel = null;
            if (PanelList.Any(p => p.BindingDate == null))
            {
                PanelList.RemoveAt(0);
            }

            CurrentSnList.Clear();
            SnList.Clear();
            DicSnItemList.Clear();

            if (PanelList.Any(x => x.Panel == CurrentSelectPanel))
            {
                string temp = CurrentSelectPanel;
                CurrentSelectPanel = string.Empty;
                CurrentSelectPanel = temp;
            }
        }
    }
}