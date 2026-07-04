using SIE.DIST;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Packages.ItemLabels;
using SIE.Resources.WipResources;
using SIE.Wpf.Common;
using System;
using System.Collections.Generic;

namespace SIE.Wpf.DIST
{
    /// <summary>
    /// 载具关联物料ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("载具关联")]
    [DisplayMember(nameof(GoodsIssueViewModel.Barcode))]
    public class GoodsIssueViewModel : ViewModel, IFocusTrigger
    {
        /// <summary>
        /// 配送控制器
        /// </summary>
        DistributionController _controller { get; } = RT.Service.Resolve<DistributionController>();

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public GoodsIssueViewModel()
        {
            Qty = 0;
        }
        #endregion

        #region 扫描框聚焦
        /// <summary>
        /// 控件聚焦事件
        /// </summary>
        public event EventHandler Focused;

        /// <summary>
        /// 触发条码输入框获取焦点
        /// </summary>
        public void FocuseBarcode()
        {
            Focused?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region 发货记录 GoodsIssue
        /// <summary>
        /// 发货记录Id
        /// </summary>
        public static readonly IRefIdProperty GoodsIssueIdProperty =
            P<GoodsIssueViewModel>.RegisterRefId(e => e.GoodsIssueId, ReferenceType.Normal);

        /// <summary>
        /// 发货记录Id
        /// </summary>
        public double GoodsIssueId
        {
            get { return (double)this.GetRefId(GoodsIssueIdProperty); }
            set { this.SetRefId(GoodsIssueIdProperty, value); }
        }

        /// <summary>
        /// 发货记录
        /// </summary>
        public static readonly RefEntityProperty<GoodsIssue> GoodsIssueProperty =
            P<GoodsIssueViewModel>.RegisterRef(e => e.GoodsIssue, GoodsIssueIdProperty);

        /// <summary>
        /// 发货记录
        /// </summary>
        public GoodsIssue GoodsIssue
        {
            get { return this.GetRefEntity(GoodsIssueProperty); }
            set { this.SetRefEntity(GoodsIssueProperty, value); }
        }
        #endregion

        #region 配送明细 BillList
        /// <summary> 
        /// 配送明细
        /// </summary>
        public static readonly ListProperty<EntityList<DistributionBill>> BillListProperty = P<GoodsIssueViewModel>.RegisterList(e => e.BillList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<DistributionBill>()
        });

        /// <summary>
        /// 配送明细
        /// </summary>
        public EntityList<DistributionBill> BillList
        {
            get { return this.GetLazyList(BillListProperty); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("配送资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<GoodsIssueViewModel>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)GetRefNullableId(ResourceIdProperty); }
            set { SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<GoodsIssueViewModel>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return GetRefEntity(ResourceProperty); }
            set { SetRefEntity(ResourceProperty, value); }
        }

        #endregion

        #region 配送数量 Qty
        /// <summary>
        /// 配送数量
        /// </summary>
        [Label("配送数量"), MinValue(0)]
        public static readonly Property<decimal> QtyProperty = P<GoodsIssueViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 配送数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 周转箱 TurnoverBox
        /// <summary>
        /// 周转箱
        /// </summary>
        public static readonly Property<string> TurnoverBoxProperty = P<GoodsIssueViewModel>.Register(e => e.TurnoverBox);

        /// <summary>
        /// 周转箱
        /// </summary>
        public string TurnoverBox
        {
            get { return this.GetProperty(TurnoverBoxProperty); }
            set { this.SetProperty(TurnoverBoxProperty, value); }
        }
        #endregion

        #region 提示 Tips

        /// <summary>
        /// 提示信息
        /// </summary>
        public static readonly Property<string> TipsProperty = P<GoodsIssueViewModel>.Register(e => e.Tips);

        /// <summary>
        /// 提示信息
        /// </summary>
        public string Tips
        {
            get { return this.GetProperty(TipsProperty); }
            set { this.SetProperty(TipsProperty, value); }
        }

        #endregion

        #region 错误 Error

        /// <summary>
        /// 错误信息
        /// </summary>
        public static readonly Property<string> ErrorProperty = P<GoodsIssueViewModel>.Register(e => e.Error);

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get { return this.GetProperty(ErrorProperty); }
            set { this.SetProperty(ErrorProperty, value); }
        }

        #endregion

        #region 条码 Barcode 
        /// <summary>
        /// 条码
        /// </summary>
        [Label("扫描条码")]
        public static readonly Property<string> BarcodeProperty = P<GoodsIssueViewModel>.Register(e => e.Barcode, new PropertyMetadata<string>
        {
            PropertyChangedCallBack = (s, e) =>
            {
                (s as GoodsIssueViewModel).OnBarcodeChanged(s, e);
            }
        });

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 配送数量只读 QtyReadOnly
        /// <summary>
        /// 配送数量只读
        /// </summary>
        public static readonly Property<bool> QtyReadOnlyProperty = P<GoodsIssueViewModel>.Register(e => e.QtyReadOnly);

        /// <summary>
        /// 配送数量只读
        /// </summary> 
        public bool QtyReadOnly
        {
            get { return this.GetProperty(QtyReadOnlyProperty); }
            set { this.SetProperty(QtyReadOnlyProperty, value); }
        }
        #endregion 

        /// <summary>
        /// 箱号标签
        /// </summary>
        List<string> itemLabelList = new List<string>();

        #region 重新开始
        /// <summary>
        /// 重新开始(所有数据清空)
        /// </summary>
        public void Restart()
        {
            InitDistributionData();
            Resource = null;
            if (GoodsIssue.RemainderQty > 0)
                Tips = "请扫描配送周转箱";
            else
                Tips = "工单 [{0}] 物料数量已全部配送完成".L10nFormat(GoodsIssue.WorkOrder.No);
            Error = null;
        }

        /// <summary>
        /// 数据初始化（保留配送产线数据，用于继续配送）
        /// </summary>
        public void InitDistributionData()
        {
            Barcode = string.Empty;
            Qty = 0m;
            TurnoverBox = null;
            itemLabelList.Clear();
            LoadBills();
            FocuseBarcode();
        }

        /// <summary>
        /// 加载配送明细
        /// </summary>
        private void LoadBills()
        {
            BillList.Clear();
            BillList.AddRange(_controller.GetDistributionBillList(GoodsIssue.Id));
        }
        #endregion

        /// <summary>
        /// 显示提示信息
        /// </summary>
        /// <param name="tips">提示信息</param>
        protected void ShowTips(string tips)
        {
            Tips = tips.ToString().Replace("\r\n", string.Empty);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="error">错误信息</param>
        protected void ShowError(string error)
        {
            Error = error.ToString().Replace("\r\n", string.Empty);
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="exc">异常</param>
        protected void ShowError(Exception exc)
        {
            var validationException = exc.GetBaseException() as ValidationException;
            if (validationException != null)
                ShowError(DisplayHelper.Display(validationException.Message));
            else
                Extenstion.Alert(exc);
        }

        /// <summary>
        /// 条码扫完后处理逻辑
        /// </summary>
        /// <param name="s">托管对象</param>
        /// <param name="e">参数</param>
        protected virtual void OnBarcodeChanged(ManagedPropertyObject s, ManagedPropertyChangedEventArgs e)
        {
            if (!Barcode.IsNotEmpty()) return;
            try
            {
                ClearInfos();
                ////扫描周转箱或物料标签
                if (Barcode.ToUpper() != "OK")
                {
                    if (!TurnoverBox.IsNotEmpty()) ////验证载具容器
                    {
                        _controller.ValidateBarcode(Barcode);
                        TurnoverBox = Barcode;
                        if (GoodsIssue.RemainderQty > 0)
                            ShowTips("配送周转箱[{0}]扫描成功，请扫描箱号或提交".L10nFormat(TurnoverBox));
                        else
                            ShowTips("工单 [{0}] 物料数量已全部配送完成".L10nFormat(GoodsIssue.WorkOrder.No));
                    }
                    else  ////验证箱号条码
                    {
                        var packingLabel = _controller.ValidatePackingLabel(Barcode, GoodsIssue.ItemId);
                        ConllectItemLabel(packingLabel);
                    }
                }
                else
                {
                    Submit();
                }
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
            finally
            {
                Barcode = null;
                QtyReadOnly = itemLabelList.Count > 0;
            }
        }

        /// <summary>
        /// 采集箱号标签
        /// </summary>
        /// <param name="packingLabel">物料标签</param>
        private void ConllectItemLabel(PackingLabel packingLabel)
        {
            if (itemLabelList.Count == 0)
                Qty = 0;
            if (itemLabelList.Contains(packingLabel.No))
            {
                Qty -= packingLabel.Qty;
                itemLabelList.Remove(packingLabel.No);
                ShowTips("箱号标签[{0}]移除成功，请继续扫描或扫[OK]提交".L10nFormat(packingLabel.No));
            }
            else
            {
                Qty += packingLabel.Qty;
                itemLabelList.Add(packingLabel.No);
                ShowTips("箱号标签[{0}]扫描成功，请继续扫描或扫[OK]提交".L10nFormat(packingLabel.No));
            }
        }

        /// <summary>
        /// 清空其实信息
        /// </summary>
        private void ClearInfos()
        {
            Error = null;
            Tips = null;
        }

        /// <summary>
        /// 提交
        /// </summary>
        public virtual void Submit()
        {
            if (TurnoverBox == null)
            {
                ShowError("周转箱条码为空，请扫描周转箱".L10N());
                return;
            }

            if (ResourceId == null)
            {
                ShowError("产线不能为空".L10N());
                return;
            }

            try
            {
                Error = null;
                _controller.SaveDistribution(GoodsIssueId, TurnoverBox, (double)ResourceId, Qty, itemLabelList.ToArray());
                ShowTips("配送周转箱{0}配送成功".L10nFormat(TurnoverBox));
                GoodsIssue.DistributionQty += Qty;
                GoodsIssue.RemainderQty -= Qty;
                InitDistributionData();
                GoodsIssue.MarkSaved();
            }
            catch (Exception exc)
            {
                ShowError(exc);
            }
        }
    }
}
