using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.MES.BatchWIP.PackRecombine;
using System;

namespace SIE.Wpf.MES.WIP.PackRecombine
{
    /// <summary>
    /// 操作明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("操作明细")]
    public class OperationDetail : ViewModel
    {
        #region 条码 Barcode
        /// <summary>
        /// 条码
        /// </summary>
        [Label("包装条码")]
        public static readonly Property<string> BarcodeProperty = P<OperationDetail>.Register(e => e.Barcode);

        /// <summary>
        /// 条码
        /// </summary>
        public string Barcode
        {
            get { return this.GetProperty(BarcodeProperty); }
            set { this.SetProperty(BarcodeProperty, value); }
        }
        #endregion

        #region 包装单位 PackingUnit
        /// <summary>
        /// 包装单位
        /// </summary>
        [Label("包装单位")]
        public static readonly Property<string> PackingUnitProperty = P<OperationDetail>.Register(e => e.PackingUnit);

        /// <summary>
        /// 包装单位
        /// </summary>
        public string PackingUnit
        {
            get { return this.GetProperty(PackingUnitProperty); }
            set { this.SetProperty(PackingUnitProperty, value); }
        }
        #endregion

        #region 操作类型 Type
        /// <summary>
        /// 操作类型
        /// </summary>
        [Label("操作类型")]
        public static readonly Property<OperationType> TypeProperty = P<OperationDetail>.Register(e => e.Type);

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperationType Type
        {
            get { return this.GetProperty(TypeProperty); }
            set { this.SetProperty(TypeProperty, value); }
        }
        #endregion

        #region 操作时间 OpertationDate
        /// <summary>
        /// 操作时间
        /// </summary>
        [Label("操作时间")]
        public static readonly Property<DateTime> OpertationDateProperty = P<OperationDetail>.Register(e => e.OpertationDate);

        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime OpertationDate
        {
            get { return this.GetProperty(OpertationDateProperty); }
            set { this.SetProperty(OpertationDateProperty, value); }
        }
        #endregion

        #region 外层包装条码 OutBarcode
        /// <summary>
        /// 外层包装条码
        /// </summary>
        [Label("外层包装条码")]
        public static readonly Property<string> OutBarcodeProperty = P<OperationDetail>.Register(e => e.OutBarcode);

        /// <summary>
        /// 外层包装条码
        /// </summary>
        public string OutBarcode
        {
            get { return this.GetProperty(OutBarcodeProperty); }
            set { this.SetProperty(OutBarcodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 操作类型
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// 移除
        /// </summary>
        [Label("移除")]
        Remove,

        /// <summary>
        /// 加入
        /// </summary>
        [Label("加入")]
        JoinIn,

        /// <summary>
        /// 查询
        /// </summary>
        [Label("查询")]
        Search
    }

    /// <summary>
    /// 操作明细视图配置
    /// </summary>
    internal class OperationDetailViewConfig : WPFViewConfig<OperationDetail>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AssignAuthorize(typeof(BatchPackRecombineViewModel));
            View.ClearCommands();
            View.Property(p => p.Barcode).Readonly();
            View.Property(p => p.PackingUnit).Readonly();
            View.Property(p => p.OutBarcode).Readonly();
            View.Property(p => p.Type).Readonly();
            View.Property(p => p.OpertationDate).Readonly();
        }
    }
}