using SIE.Core.Barcodes;
using SIE.Defects;
using SIE.Domain;
using SIE.MES.BatchWIP;
using SIE.MES.BatchWIP.Products;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using SIE.Wpf.MES.Controls;
using SIE.Wpf.MES.WIP;
using System;
using System.Collections.ObjectModel;

namespace SIE.Wpf.MES.BatchWIP.Inspects
{
    /// <summary>
    /// 批次检验不良集合
    /// </summary>
    [RootEntity, Serializable]
    [Label("批次检验不良集合")]
    public class BatchDefectiveSetViewModel : ViewModel
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchDefectiveSetViewModel()
        {
            DefectItemList = new ObservableCollection<DefectItem>();
            OutPutContainerNo = string.Empty;
        }

        /// <summary>
        /// 清楚数据
        /// </summary>
        public void Clear()
        {
            BatchWipPrdDefects?.Clear();
            DefectItemList.Clear();
            DefectList.Clear();
            OutPutContainerNo = string.Empty;
            Barcode = string.Empty;
            Qty = 0;
            NgQty = 0;
            RemainQty = 0;
            BatchDefectiveViewModels.Clear();
        }

        #region 生产批次 Barcode
        /// <summary>
        /// 生产批次
        /// </summary>
        [Label("生产批次")]
        public static readonly Property<string> BarcodeProperty = P<BatchDefectiveSetViewModel>.Register(e => e.Barcode);

        /// <summary>
        /// 生产批次
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 子批次号 ChildBarcode
        /// <summary>
        /// 子批次号
        /// </summary>
        [Label("子批次号")]
        public static readonly Property<string> ChildBarcodeProperty = P<BatchDefectiveSetViewModel>.Register(e => e.ChildBarcode);

        /// <summary>
        /// 子批次号
        /// </summary>
        public string ChildBarcode
        {
            get { return this.GetProperty(ChildBarcodeProperty); }
            set { this.SetProperty(ChildBarcodeProperty, value); }
        }
        #endregion

        #region 批次数量 Qty
        /// <summary>
        /// 批次数量
        /// </summary>
        [Label("批次数量")]
        public static readonly Property<decimal> QtyProperty = P<BatchDefectiveSetViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 批次数量
        /// </summary>
        public decimal Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 不良数量 NgQty
        /// <summary>
        /// 不良数量
        /// </summary>
        [Label("不良数量")]
        public static readonly Property<decimal> NgQtyProperty = P<BatchDefectiveSetViewModel>.Register(e => e.NgQty);

        /// <summary>
        /// 不良数量
        /// </summary>
        public decimal NgQty
        {
            get { return this.GetProperty(NgQtyProperty); }
            set { this.SetProperty(NgQtyProperty, value); }
        }
        #endregion

        #region 剩余数量 RemainQty
        /// <summary>
        /// 剩余数量
        /// </summary>
        [Label("剩余数量")]
        public static readonly Property<decimal> RemainQtyProperty = P<BatchDefectiveSetViewModel>.Register(e => e.RemainQty);

        /// <summary>
        /// 剩余数量
        /// </summary>
        public decimal RemainQty
        {
            get { return this.GetProperty(RemainQtyProperty); }
            set { this.SetProperty(RemainQtyProperty, value); }
        }
        #endregion

        #region 载具条码 OutPutContainerNo
        /// <summary>
        /// 载具条码
        /// </summary>
        [Label("载具条码")]
        public static readonly Property<string> OutPutContainerNoProperty = P<BatchDefectiveSetViewModel>.Register(e => e.OutPutContainerNo);

        /// <summary>
        /// 载具条码
        /// </summary>
        public string OutPutContainerNo
        {
            get { return this.GetProperty(OutPutContainerNoProperty); }
            set { this.SetProperty(OutPutContainerNoProperty, value); }
        }
        #endregion

        #region 当前入站批次信息 CurInputBatch
        /* /// <summary>
        /// 当前入站批次信息
        /// </summary>
        [Label("当前入站批次信息")]
        public static readonly Property<InputBatch> CurInputBatchProperty = P<BatchDefectiveSetViewModel>.Register(e => e.CurInputBatch);

        /// <summary>
        /// 当前入站批次信息
        /// </summary>
        public InputBatch CurInputBatch
        {
            get { return this.GetProperty(CurInputBatchProperty); }
            set { this.SetProperty(CurInputBatchProperty, value); }
        } */
        #endregion

        /// <summary>
        /// 缺陷项目列表
        /// </summary>
        public ObservableCollection<DefectItem> DefectItemList { get; set; }

        #region 缺陷代码列表 DefectList
        /// <summary>
        /// 缺陷代码列表
        /// </summary>
        [Label("缺陷代码列表")]
        public static readonly ListProperty<EntityList<Defect>> DefectListProperty = P<BatchDefectiveSetViewModel>.RegisterList(e => e.DefectList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<Defect>()
        });

        /// <summary>
        /// 缺陷代码列表
        /// </summary>
        public EntityList<Defect> DefectList
        {
            get { return this.GetLazyList(DefectListProperty); }
        }
        #endregion

        #region 不良信息 BatchDefectiveViewModels
        /// <summary>
        /// 批次检验不良明细
        /// </summary>
        [Label("不良明细")]
        public static readonly ListProperty<EntityList<BatchDefectiveViewModel>> BatchDefectiveVmsProperty = P<BatchDefectiveSetViewModel>.RegisterList(e => e.BatchDefectiveViewModels, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<BatchDefectiveViewModel>()
        });

        /// <summary>
        /// 批次检验不良明细
        /// </summary>
        public EntityList<BatchDefectiveViewModel> BatchDefectiveViewModels
        {
            get { return this.GetLazyList(BatchDefectiveVmsProperty); }
        }
        #endregion

        #region 工作站信息 Workstation
        /// <summary>
        /// 工作站信息
        /// </summary>
        [Label("工作站信息")]
        public static readonly Property<Workstation> WorkstationProperty = P<BatchDefectiveSetViewModel>.Register(e => e.Workstation);

        /// <summary>
        /// 工作站信息
        /// </summary>
        public Workstation Workstation
        {
            get { return this.GetProperty(WorkstationProperty); }
            set { this.SetProperty(WorkstationProperty, value); }
        }
        #endregion

        /// <summary>
        /// 转出批次列表
        /// </summary>
        public EntityList<OutputBatch> OutputBatchs { get; set; }

        /// <summary>
        /// 转入批次列表
        /// </summary>
        public EntityList<InputBatch> InputBatchs { get; set; }

        /* #region 转出批次列表 OutputBatchs
        /// <summary>
        /// 转出批次列表
        /// </summary>
        [Label("转出批次列表")]
        public static readonly ListProperty<EntityList<OutputBatch>> OutputBatchsProperty = P<BatchDefectiveSetViewModel>.RegisterList(e => e.OutputBatchs, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<OutputBatch>()
        });

        /// <summary>
        /// 转出批次列表
        /// </summary>
        public EntityList<OutputBatch> OutputBatchs
        {
            get { return this.GetLazyList(OutputBatchsProperty); }
        }
        #endregion */

        #region 工单 WorkOrder
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        public static readonly Property<WorkOrder> WorkOrderProperty = P<BatchDefectiveSetViewModel>.Register(e => e.WorkOrder);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetProperty(WorkOrderProperty); }
            set { this.SetProperty(WorkOrderProperty, value); }
        }
        #endregion

        #region 产品缺陷不良记录 BatchWipPrdDefects
        /// <summary>
        /// 产品缺陷不良记录
        /// </summary>
        [Label("产品缺陷不良记录")]
        public static readonly ListProperty<EntityList<BatchWipProductDefect>> BatchWipPrdDefectsProperty = P<BatchDefectiveSetViewModel>.RegisterList(e => e.BatchWipPrdDefects, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<BatchWipProductDefect>()
        });

        /// <summary>
        /// 产品缺陷不良记录
        /// </summary>
        public EntityList<BatchWipProductDefect> BatchWipPrdDefects
        {
            get { return this.GetLazyList(BatchWipPrdDefectsProperty); }
        }
        #endregion

        #region 条码类型 BarcodeType
        /// <summary>
        /// 条码类型
        /// </summary>
        [Required]
        [Label("条码类型")]
        public static readonly Property<BarcodeType> BarcodeTypeProperty = P<BatchDefectiveSetViewModel>.Register(e => e.BarcodeType);

        /// <summary>
        /// 条码类型
        /// </summary>
        public BarcodeType BarcodeType
        {
            get { return GetProperty(BarcodeTypeProperty); }
            set { SetProperty(BarcodeTypeProperty, value); }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        public BatchInspectViewModel batchInspectvm { get; set; }
    }
}
