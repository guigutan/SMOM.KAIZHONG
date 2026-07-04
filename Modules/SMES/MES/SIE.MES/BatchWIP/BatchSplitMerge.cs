using SIE.Domain;
using SIE.MES.BatchWIP.Products;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP
{

    /// <summary>
    /// 批次拆分合并记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("批次拆分合并记录")]
    public class BatchSplitMergeRecord : DataEntity
    {

        #region 输入条码 InputBatchNo
        /// <summary>
        /// 输入条码
        /// </summary>
        [Label("输入条码")]
        public static readonly Property<string> InputBatchNoProperty = P<BatchSplitMergeRecord>.Register(e => e.InputBatchNo);

        /// <summary>
        /// 输入条码
        /// </summary>
        public string InputBatchNo
        {
            get { return GetProperty(InputBatchNoProperty); }
            set { SetProperty(InputBatchNoProperty, value); }
        }
        #endregion

        #region 输入数量 InputQty
        /// <summary>
        /// 输入数量
        /// </summary>
        [Label("输入数量")]
        public static readonly Property<decimal> InputQtyProperty = P<BatchSplitMergeRecord>.Register(e => e.InputQty);

        /// <summary>
        /// 输入数量
        /// </summary>
        public decimal InputQty
        {
            get { return GetProperty(InputQtyProperty); }
            set { SetProperty(InputQtyProperty, value); }
        }
        #endregion

        #region 操作类型 BatchOperationType
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<BatchOperationType> BatchOperationTypeProperty = P<BatchSplitMergeRecord>.Register(e => e.BatchOperationType);

        /// <summary>
        /// 操作类型
        /// </summary>
        public BatchOperationType BatchOperationType
        {
            get { return GetProperty(BatchOperationTypeProperty); }
            set { SetProperty(BatchOperationTypeProperty, value); }
        }
        #endregion

        #region 输出条码 BatchNo
        /// <summary>
        /// 输出条码
        /// </summary>
        [Label("输出条码")]
        public static readonly Property<string> OutputBatchNoProperty = P<BatchSplitMergeRecord>.Register(e => e.OutputBatchNo);

        /// <summary>
        /// 输出条码
        /// </summary>
        public string OutputBatchNo
        {
            get { return GetProperty(OutputBatchNoProperty); }
            set { SetProperty(OutputBatchNoProperty, value); }
        }
        #endregion

        #region 输出数量 OutputQty
        /// <summary>
        /// 输出数量
        /// </summary>
        [Label("输出数量")]
        public static readonly Property<decimal> OutputQtyProperty = P<BatchSplitMergeRecord>.Register(e => e.OutputQty);

        /// <summary>
        /// 输出数量
        /// </summary>
        public decimal OutputQty
        {
            get { return GetProperty(OutputQtyProperty); }
            set { SetProperty(OutputQtyProperty, value); }
        }
        #endregion

        #region 版本记录 Version
        /// <summary>
        /// 版本记录Id
        /// </summary>
        public static readonly IRefIdProperty VersionIdProperty = P<BatchSplitMergeRecord>.RegisterRefId(e => e.VersionId, ReferenceType.Parent);

        /// <summary>
        /// 版本记录Id
        /// </summary>
        public double VersionId
        {
            get { return (double)GetRefId(VersionIdProperty); }
            set { SetRefId(VersionIdProperty, value); }
        }

        /// <summary>
        /// 版本记录记录
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductVersion> VersionProperty = P<BatchSplitMergeRecord>.RegisterRef(e => e.Version, VersionIdProperty);

        /// <summary>
        /// 版本记录记录
        /// </summary>
        public BatchWipProductVersion Version
        {
            get { return GetRefEntity(VersionProperty); }
            set { SetRefEntity(VersionProperty, value); }
        }
        #endregion

        internal class BatchSplitMergeRecordConfig : EntityConfig<BatchSplitMergeRecord>
        {
            /// <summary>
            /// 对Meta属性的配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.MapTable("BATCH_SPLIT_MERGE_REC").MapAllProperties();
                Meta.EnablePhantoms();
            }
        }
    }

   /// <summary>
   /// 操作类型
   /// </summary>
    public enum BatchOperationType
    {
        /// <summary>
        /// 拆分
        /// </summary>
        [Label("拆分")]
        Split = 0,

        /// <summary>
        /// 合并
        /// </summary>
        [Label("合并")]
        Merge = 1,
    }

}
