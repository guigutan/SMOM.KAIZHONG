using SIE.Core.Labels;
using SIE.DIST;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 载具关联物料ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("载具关联")]
    [DisplayMember(nameof(GoodsIssueViewModel.Barcode))]
    public class GoodsIssueViewModel : ViewModel
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
            ItemLabelList = new List<string>();
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

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<GoodsIssueViewModel>.Register(e => e.ItemName);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
            set { this.SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<GoodsIssueViewModel>.Register(e => e.ItemCode);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
            set { this.SetProperty(ItemCodeProperty, value); }
        }
        #endregion

        #region 物料规格 ItemModel
        /// <summary>
        /// 物料规格
        /// </summary>
        [Label("物料规格")]
        public static readonly Property<string> ItemModelProperty = P<GoodsIssueViewModel>.Register(e => e.ItemModel);

        /// <summary>
        /// 物料规格
        /// </summary>
        public string ItemModel
        {
            get { return this.GetProperty(ItemModelProperty); }
            set { this.SetProperty(ItemModelProperty, value); }
        }
        #endregion

        #region 仓库发货数 GoodsQty
        /// <summary>
        /// 仓库发货数
        /// </summary>
        [Label("仓库发货数")]
        public static readonly Property<string> GoodsQtyProperty = P<GoodsIssueViewModel>.Register(e => e.GoodsQty);

        /// <summary>
        /// 仓库发货数
        /// </summary>
        public string GoodsQty
        {
            get { return this.GetProperty(GoodsQtyProperty); }
            set { this.SetProperty(GoodsQtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> RemainQtyProperty = P<GoodsIssueViewModel>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 累计配送数 DistributionQty
        /// <summary>
        /// 累计配送数
        /// </summary>
        [Label("累计配送数")]
        public static readonly Property<decimal> DistributionQtyProperty = P<GoodsIssueViewModel>.Register(e => e.DistributionQty);

        /// <summary>
        /// 累计配送数
        /// </summary>
        public decimal DistributionQty
        {
            get { return this.GetProperty(DistributionQtyProperty); }
            set { this.SetProperty(DistributionQtyProperty, value); }
        }
        #endregion

        #region 缺陷数量 DefectQty
        /// <summary>
        /// 缺陷数量
        /// </summary>
        [Label("缺陷数量")]
        public static readonly Property<decimal> DefectQtyProperty = P<GoodsIssueViewModel>.Register(e => e.DefectQty);

        /// <summary>
        /// 缺陷数量
        /// </summary>
        public decimal DefectQty
        {
            get { return this.GetProperty(DefectQtyProperty); }
            set { this.SetProperty(DefectQtyProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<GoodsIssueViewModel>.Register(e => e.UnitName);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
            set { this.SetProperty(UnitNameProperty, value); }
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

        #region BS
        #region 工单 WorkOrderNo
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<string> WorkOrderNoProperty = P<GoodsIssueViewModel>.RegisterView(e => e.WorkOrderNo, p => p.GoodsIssue.WorkOrder.No);

        /// <summary>
        /// 工单
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 属性列表 PropertyValueList
        /// <summary>
        /// 属性列表
        /// </summary>
        [Label("属性名")]
        public static readonly Property<EntityList<GoodsIssuePropertyValue>> PropertyValueListProperty = P<GoodsIssueViewModel>.RegisterView(e => e.PropertyValueList, p => p.GoodsIssue.PropertyValueList);

        /// <summary>
        /// 属性列表
        /// </summary>
        public EntityList<GoodsIssuePropertyValue> PropertyValueList
        {
            get { return this.GetProperty(PropertyValueListProperty); }
        }
        #endregion

        #region 物料ID ItemId
        /// <summary>
        /// 物料ID
        /// </summary>
        [Label("物料ID")]
        public static readonly Property<double> ItemIdProperty = P<GoodsIssueViewModel>.RegisterView(e => e.ItemId, p => p.GoodsIssue.ItemId);

        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get { return this.GetProperty(ItemIdProperty); }
        }
        #endregion

        #endregion

        #region 条码 Barcode 
        /// <summary>
        /// 条码
        /// </summary>
        [Label("扫描条码")]
        public static readonly Property<string> BarcodeProperty = P<GoodsIssueViewModel>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region TipsDisplay
        /// <summary>
        /// TipsDisplay
        /// </summary>       
        public static readonly Property<string> TipsDisplayProperty = P<GoodsIssueViewModel>.Register(e => e.TipsDisplay);

        /// <summary>
        /// TipsDisplay
        /// </summary>
        public string TipsDisplay
        {
            get { return this.GetProperty(TipsDisplayProperty); }
            set { this.SetProperty(TipsDisplayProperty, value); }
        }
        #endregion

        #region BarcodeDisplay
        /// <summary>
        /// BarcodeDisplay
        /// </summary>
        [Label("扫描条码")]
        public static readonly Property<string> BarcodeDisplayProperty = P<GoodsIssueViewModel>.Register(e => e.BarcodeDisplay);

        /// <summary>
        /// BarcodeDisplay
        /// </summary>
        public string BarcodeDisplay
        {
            get { return this.GetProperty(BarcodeDisplayProperty); }
            set { this.SetProperty(BarcodeDisplayProperty, value); }
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
        public List<string> ItemLabelList
        {
            get; set;
        }
    }

    /// <summary>
    /// 载具关联viewModel控制器
    /// </summary>
    public class GoodsIssueViewModelContrller
    {
        /// <summary>
        /// 条码扫完后处理逻辑
        /// </summary>
        /// <param name="model">实体</param>
        public virtual void OnBarcodeChanged(GoodsIssueViewModel model)
        {
            if (!model.Barcode.IsNotEmpty()) return;
            try
            {
                ////扫描周转箱或物料标签
                if (model.Barcode.ToUpper() != "OK")
                {
                    if (!model.TurnoverBox.IsNotEmpty()) ////验证载具容器
                    {
                        RT.Service.Resolve<DistributionController>().ValidateBarcode(model.Barcode);
                        model.TurnoverBox = model.Barcode;
                        if (model.RemainQty > 0)
                            model.Tips = "配送周转箱[{0}]扫描成功，请扫描箱号或提交".L10nFormat(model.TurnoverBox);
                        else
                            model.Tips = "工单 [{0}] 物料数量已全部配送完成".L10nFormat(model.WorkOrderNo);
                    }
                    else  ////验证箱号条码
                    {
                        var packingLabel = RT.Service.Resolve<DistributionController>().ValidatePackingLabel(model.Barcode, model.ItemId);
                        ConllectItemLabel(packingLabel, model);
                    }
                }
                else
                {
                    Submit(model);
                }
            }
            catch (Exception exc)
            {
                ShowError(exc, model);
            }
            finally
            {
                model.Barcode = null;
                model.QtyReadOnly = !(model.ItemLabelList == null || model.ItemLabelList.Count <= 0);
            }
        }

        /// <summary>
        /// 采集箱号标签
        /// </summary>
        /// <param name="packingLabel">物料标签</param>
        /// <param name="model">实体</param>
        private void ConllectItemLabel(PackingLabel packingLabel, GoodsIssueViewModel model)
        {
            if (model.ItemLabelList.Count == 0)
                model.Qty = 0;
            if (model.ItemLabelList.Contains(packingLabel.No))
            {
                model.Qty -= packingLabel.Qty;
                model.ItemLabelList.Remove(packingLabel.No);
                model.Tips = "箱号标签[{0}]移除成功，请继续扫描或扫[OK]提交".L10nFormat(packingLabel.No);
            }
            else
            {
                model.Qty += packingLabel.Qty;
                model.ItemLabelList.Add(packingLabel.No);
                model.Tips = "箱号标签[{0}]扫描成功，请继续扫描或扫[OK]提交".L10nFormat(packingLabel.No);
            }
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="exc">异常</param>
        /// <param name="model">实体</param>
        private void ShowError(Exception exc, GoodsIssueViewModel model)
        {
            var validationException = exc.GetBaseException() as ValidationException;
            if (validationException != null)
                model.Error = (DisplayHelper.Display(validationException.Message)).Replace("\r\n", string.Empty);
            else
                throw new ValidationException(exc.Message);
        }

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="model">实体</param>
        public virtual void Submit(GoodsIssueViewModel model)
        {
            if (string.IsNullOrEmpty(model.TurnoverBox))
            {
                model.Error = "周转箱条码为空，请扫描周转箱".L10N();
                return;
            }

            if (model.ResourceId == null)
            {
                model.Error = ("产线不能为空".L10N());
                return;
            }

            try
            {
                model.Error = string.Empty;
                RT.Service.Resolve<DistributionController>().SaveDistribution(model.GoodsIssueId, model.TurnoverBox, (double)model.ResourceId, model.Qty, model.ItemLabelList.ToArray());
                model.Tips = ("配送周转箱{0}配送成功".L10nFormat(model.TurnoverBox));
                model.DistributionQty += model.Qty;
                model.RemainQty -= model.Qty;
                model.GoodsIssue.MarkSaved();
            }
            catch (Exception exc)
            {
                ShowError(exc, model);
            }
        }
    }
}
