using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MES.LoadItems;
using SIE.MES.SingleLabels;
using SIE.MES.WIP;
using SIE.MES.WIP.Products;
using SIE.MES.WIP.Repairs;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Wpf.MES.LoadItems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.MES.WIP.Repairs
{
    /// <summary>
    ///  装配信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("装配信息")]
    public class ProductAssemblyDetailViewModel : ViewModel
    {
        /// <summary>
        /// 维修采集
        /// </summary>
        public RepairViewModel RepairViewModel { get; set; }

        #region 工作站信息(换料验证) Workcell
        /// <summary>
        /// 工作站信息
        /// </summary>
        [Label("工作站信息")]
        public static readonly Property<Workcell> WorkcellProperty = P<ProductAssemblyDetailViewModel>.Register(e => e.Workcell);

        /// <summary>
        /// 工作站信息
        /// </summary>
        public Workcell Workcell
        {
            get { return this.GetProperty(WorkcellProperty); }
            set { this.SetProperty(WorkcellProperty, value); }
        }
        #endregion

        #region 关键件 KeyItem
        /// <summary>
        /// 关键件
        /// </summary>
        [Label("关键件")]
        public static readonly Property<WipProductProcessKeyItem> KeyItemProperty = P<ProductAssemblyDetailViewModel>.Register(e => e.KeyItem);

        /// <summary>
        /// 关键件
        /// </summary>
        public WipProductProcessKeyItem KeyItem
        {
            get { return this.GetProperty(KeyItemProperty); }
            set { this.SetProperty(KeyItemProperty, value); }
        }
        #endregion   

        #region 待换料条码 Barcode
        /// <summary>
        /// 待换料条码
        /// </summary>
        [Label("待换料条码")]
        public static readonly Property<string> BarcodeProperty = P<ProductAssemblyDetailViewModel>.Register(e => e.Barcode, new PropertyMetadata<string>
        {
            PropertyChangedCallBack = (s, e) => (s as ProductAssemblyDetailViewModel).OnBarcodeChanged(e)
        });

        /// <summary>
        /// 待换料条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 换料数量 ChangeQty
        /// <summary>
        /// 换料数量
        /// </summary>
        [Label("换料数量")]
        public static readonly Property<decimal> ChangeQtyProperty
            = P<ProductAssemblyDetailViewModel>.Register(e => e.ChangeQty,
                new PropertyMetadata<decimal>()
                {
                    PropertyChangedCallBack = (s, e) => (s as ProductAssemblyDetailViewModel).OnChangeQtyChanged(e)
                });

        /// <summary>
        /// 换料数量
        /// </summary>
        public decimal ChangeQty
        {
            get { return this.GetProperty(ChangeQtyProperty); }
            set { this.SetProperty(ChangeQtyProperty, value); }
        }
        #endregion

        #region 换料列表 ChangeItemViewModelList 
        /// <summary>
        /// 换料列表  
        /// </summary>
        [Label("换料列表")]
        public static readonly ListProperty<EntityList<ChangeItemViewModel>> ChangeItemViewModelListProperty
            = P<ProductAssemblyDetailViewModel>.RegisterList(e => e.ChangeItemViewModelList, new ListPropertyMeta
            {
                HasManyType = HasManyType.Aggregation,
                DataProvider = e => new EntityList<ChangeItemViewModel>()
            });

        /// <summary> 
        /// 换料列表
        /// </summary>
        public EntityList<ChangeItemViewModel> ChangeItemViewModelList
        {
            get { return this.GetLazyList(ChangeItemViewModelListProperty); }
        }
        #endregion

        #region 是否换料 IsChangeSn
        /// <summary>
        /// 是否换料
        /// </summary>
        [Label("是否换料")]
        public static readonly Property<bool> IsChangeSnProperty = P<ProductAssemblyDetailViewModel>.Register(e => e.IsChangeSn);

        /// <summary>
        /// 是否换料
        /// </summary>
        public bool IsChangeSn
        {
            get { return this.GetProperty(IsChangeSnProperty); }
            set { this.SetProperty(IsChangeSnProperty, value); }
        }
        #endregion

        #region 换料后标签 ChangeBarcode
        /// <summary>
        /// 换料后标签
        /// </summary>
        [Label("换料后标签")]
        public static readonly Property<string> ChangeBarcodeProperty = P<ProductAssemblyDetailViewModel>.Register(e => e.ChangeBarcode);

        /// <summary>
        /// 换料后标签
        /// </summary>
        public string ChangeBarcode
        {
            get { return this.GetProperty(ChangeBarcodeProperty); }
            set { this.SetProperty(ChangeBarcodeProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单ID
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<ProductAssemblyDetailViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单ID
        /// </summary>
        public double? WorkOrderId
        {
            get { return (double?)this.GetRefNullableId(WorkOrderIdProperty); }
            set { this.SetRefNullableId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<ProductAssemblyDetailViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 总换料数量 TotalChangeQty
        /// <summary>
        /// 总换料数量
        /// </summary>
        [Label("总换料数量")]
        public static readonly Property<decimal> TotalChangeQtyProperty = P<ProductAssemblyDetailViewModel>.Register(e => e.TotalChangeQty);

        /// <summary>
        /// 总换料数量
        /// </summary>
        public decimal TotalChangeQty
        {
            get { return this.GetProperty(TotalChangeQtyProperty); }
            set { this.SetProperty(TotalChangeQtyProperty, value); }
        }
        #endregion

        #region 置换后处理 HandleMethod
        /// <summary>
        /// 置换后处理(默认为报废,如果选择正常下料，要进行下料，即更新物料标签表加上换料数量）
        /// </summary>
        [Label("置换后处理")]
        public static readonly Property<ChangeItemHandleMethod> HandleMethodProperty
            = P<ProductAssemblyDetailViewModel>.Register(e => e.HandleMethod);

        /// <summary>
        /// 置换后处理
        /// </summary>
        public ChangeItemHandleMethod HandleMethod
        {
            get { return this.GetProperty(HandleMethodProperty); }
            set { this.SetProperty(HandleMethodProperty, value); }
        }
        #endregion

        /// <summary>
        /// 换料条码
        /// </summary>
        string _changeSn;

        /// <summary>
        /// 换料数量变更事件
        /// </summary>
        /// <param name="e">参数</param>
        private void OnChangeQtyChanged(ManagedPropertyChangedEventArgs e)
        {
            if (_changeSn.IsNullOrEmpty())
            {
                return;
            }

            var changedItem = ChangeItemViewModelList.FirstOrDefault(p => p.ChangeSn == _changeSn);

            if (changedItem == null)
            {
                return;
            }

            if (ChangeQty > changedItem.LoadItemBarcodeInfo.Qty)
            {
                throw new ValidationException("换料数量不能大于条码可用数量".L10N());
            }

            IsOverLoadItem(_changeSn, changedItem.LoadItemBarcodeInfo.Qty);

            decimal orginalQty = changedItem.ChangeQty;

            changedItem.ChangeQty = ChangeQty;

            Barcode = null;

            //更新总换料数量
            TotalChangeQty = TotalChangeQty - orginalQty + ChangeQty;
        }

        /// <summary>
        /// 条码变更事件
        /// </summary>
        /// <param name="e">参数</param>
        protected void OnBarcodeChanged(ManagedPropertyChangedEventArgs e)
        {
            if (Barcode.IsNullOrEmpty())
            {
                return;
            }
            try
            {
                ValidationBarcode();

                LoadItemBarcodeInfo barcodeInfo = GetChangedBarcode(Barcode);

                ValidationItem(barcodeInfo);

                IsOverLoadItem(barcodeInfo.Barcode, 1);

                ChangeQty = 1;

                var item = new ChangeItemViewModel()
                {
                    ChangeSn = Barcode,
                    ChangeQty = 1,
                    LoadItemBarcodeInfo = barcodeInfo,
                    ProductAssemblyDetailViewModel = this,
                };

                item.MarkSaved();

                ChangeItemViewModelList.Add(item);

                NotifyPropertyChanged(ChangeItemViewModelListProperty);

                _changeSn = Barcode;

                Barcode = null;

                //更新总换料数量
                TotalChangeQty = TotalChangeQty + ChangeQty;
            }
            catch (Exception exc)
            {
                Barcode = null;
                exc.Alert();
            }
        }

        /// <summary>
        /// 验证条码
        /// </summary>
        private void ValidationBarcode()
        {
            if (Barcode == KeyItem.SourceCode)
            {
                throw new ValidationException("换料条码[{0}]与当前装配条码一致，不允许换料".L10nFormat(Barcode));
            }

            if (ChangeItemViewModelList.Any(p => p.ChangeSn == Barcode))
            {
                throw new ValidationException("条码[{0}]已存在换料列表".L10nFormat(Barcode));
            }
        }

        /// <summary>
        /// 获取换料条码
        /// </summary>
        /// <param name="sn">换料条码</param>
        /// <returns>换料条码信息</returns>
        private LoadItemBarcodeInfo GetChangedBarcode(string sn)
        {
            LoadItemBarcodeInfo barcodeInfo;
            //匹配上料列表
            var loadItems = RT.Service.Resolve<LoadItemController>().GetLoadItemList(Workcell.ResourceId, Workcell.StationId);
            var loadItem = loadItems.FirstOrDefault(p => p.SourceCode == sn);
            if (loadItem != null)
            {
                IsOverLoadItem(sn, loadItem.Qty);
                barcodeInfo = new LoadItemBarcodeInfo()
                {
                    Barcode = loadItem.SourceCode,
                    ItemId = loadItem.ItemId,
                    Qty = loadItem.Qty,
                    Type = loadItem.SourceType
                };

                barcodeInfo.ItemExtPropName = loadItem.ItemExtPropName;
                barcodeInfo.ItemExtProp = loadItem.ItemExtProp;
            }
            else
            {
                Dictionary<LoadItemSourceType, bool> dicLoadItemSourceType = new Dictionary<LoadItemSourceType, bool>();
                dicLoadItemSourceType.Add(LoadItemSourceType.SN, true);

                barcodeInfo = LoadItemHelper.GetLoadBarcodeInfo(sn, Workcell, dicLoadItemSourceType, WorkOrderId);
            }

            return barcodeInfo;
        }

        /// <summary>
        /// 判断换料是数量是否超过上料数
        /// </summary>
        /// <param name="sn">条码</param>
        /// <param name="loadQty">缺料数</param>
        public void IsOverLoadItem(string sn, decimal loadQty)
        {
            var changedList = RepairViewModel.DetailList.SelectMany(p => p.ChangeItemViewModelList);
            var qty = changedList.Where(p => p.ChangeSn == sn).Sum(p => p.ChangeQty);  //已添加换料数量
            if (loadQty - qty <= 0)
            {
                throw new ValidationException("条码{0}数量不足".L10nFormat(sn));
            }
        }

        /// <summary>
        /// 验证物料信息
        /// </summary>
        /// <param name="barcodeInfo">换料条码信息</param>
        private void ValidationItem(LoadItemBarcodeInfo barcodeInfo)
        {
            if (barcodeInfo.ItemId != KeyItem.ItemId)
            {
                throw new ValidationException("物料不匹配".L10N());
            }
            if (barcodeInfo.ItemExtProp != KeyItem.ItemExtProp)
            {
                throw new ValidationException("物料{0}扩展属性不匹配".L10nFormat(KeyItem.Item.Code));
            }
        }

        /// <summary>
        /// 重新计算总换料数量
        /// </summary>
        public void ComputeTotalChangeQty()
        {
            decimal totalChangeQty = 0;

            foreach (var changeItem in ChangeItemViewModelList)
            {
                totalChangeQty += changeItem.ChangeQty;
            }

            this.TotalChangeQty = totalChangeQty;
        }
    }

    /// <summary>
    /// 装配信息实体配置
    /// </summary>
    internal class ProductAssemblyDetailViewModelConfig : EntityConfig<ProductAssemblyDetailViewModel>
    {
        /// <summary>
        /// 添加验证规则
        /// </summary>
        /// <param name="rules">验证规则的声明器</param>
        protected override void AddValidations(IValidationDeclarer rules)
        {
            rules.Add((s, e) =>
            {
                var m = s as ProductAssemblyDetailViewModel;
                if (m.ChangeItemViewModelList.Any(p => p.ChangeQty <= 0))
                {
                    e.BrokenDescription = "换料数量必须大于0".L10N();
                }
                if (m.ChangeItemViewModelList.Any(p => p.ChangeQty > p.LoadItemBarcodeInfo.Qty))
                {
                    e.BrokenDescription = "换料数量不能大于条码可用数量".L10N();
                }
                if (m.ChangeItemViewModelList.Sum(p => p.ChangeQty) > m.KeyItem.Qty)
                {
                    e.BrokenDescription = "换料数量不能大于装配数量".L10N();
                }
            });
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.SupportTree();
        }
    }
}
