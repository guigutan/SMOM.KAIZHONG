using SIE.Barcodes;
using SIE.Domain;
using SIE.MES.WorkOrders;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.PackingPrints
{
    /// <summary>
    /// 包装号
    /// </summary>
    [RootEntity, Serializable]
    [Label("包装号")]
    public partial class PackingBarcode : DataEntity
    {
        #region 包装号 Code
        /// <summary>
        /// 包装号
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("包装号")]
        public static readonly Property<string> CodeProperty = P<PackingBarcode>.Register(e => e.Code);

        /// <summary>
        /// 包装号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 打印日期 PrintDate
        /// <summary>
        /// 打印日期
        /// </summary>
        [Label("打印日期")]
        public static readonly Property<DateTime> PrintDateProperty = P<PackingBarcode>.Register(e => e.PrintDate);

        /// <summary>
        /// 打印日期
        /// </summary>
        public DateTime PrintDate
        {
            get { return GetProperty(PrintDateProperty); }
            set { SetProperty(PrintDateProperty, value); }
        }
        #endregion

        #region 是否使用 IsUse
        /// <summary>
        /// 是否使用
        /// </summary>
        [Label("是否使用")]
        public static readonly Property<bool> IsUseProperty = P<PackingBarcode>.Register(e => e.IsUse);

        /// <summary>
        /// 是否使用
        /// </summary>
        public bool IsUse
        {
            get { return GetProperty(IsUseProperty); }
            set { SetProperty(IsUseProperty, value); }
        }
        #endregion

        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        public static readonly IRefIdProperty WorkOrderIdProperty = P<PackingBarcode>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)GetRefId(WorkOrderIdProperty); }
            set { SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty = P<PackingBarcode>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return GetRefEntity(WorkOrderProperty); }
            set { SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion

        #region 工单包装规则 WorkOrderPackageRuleDetail
        /// <summary>
        ///工单包装规则Id
        /// </summary>
        public static readonly IRefIdProperty PackageRuleDetailIdProperty = P<PackingBarcode>.RegisterRefId(e => e.PackageRuleDetailId, ReferenceType.Normal);

        /// <summary>
        /// 工单包装规则Id
        /// </summary>
        public double PackageRuleDetailId
        {
            get { return (double)GetRefId(PackageRuleDetailIdProperty); }
            set { SetRefId(PackageRuleDetailIdProperty, value); }
        }

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public static readonly RefEntityProperty<WorkOrderPackageRuleDetail> PackageRuleDetailProperty = P<PackingBarcode>.RegisterRef(e => e.PackageRuleDetail, PackageRuleDetailIdProperty);

        /// <summary>
        /// 工单包装规则
        /// </summary>
        public WorkOrderPackageRuleDetail PackageRuleDetail
        {
            get { return GetRefEntity(PackageRuleDetailProperty); }
            set { SetRefEntity(PackageRuleDetailProperty, value); }
        }
        #endregion

        #region 打印次数 PrintTimes
        /// <summary>
        /// 打印次数
        /// </summary>
        [MinValue(0)]
        [Label("打印次数")]
        public static readonly Property<int> PrintTimesProperty = P<PackingBarcode>.Register(e => e.PrintTimes);

        /// <summary>
        /// 打印次数
        /// </summary>
        public int PrintTimes
        {
            get { return GetProperty(PrintTimesProperty); }
            set { SetProperty(PrintTimesProperty, value); }
        }
        #endregion

        #region 打印状态 State
        /// <summary>
        /// 打印状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<BarcodeState> StateProperty = P<PackingBarcode>.Register(e => e.PrintedState);

        /// <summary>
        /// 打印状态
        /// </summary>
        public BarcodeState PrintedState
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion  

        #region 打印人 PrintBy
        /// <summary>
        /// 打印人Id
        /// </summary>
        [Label("打印人")]
        public static readonly IRefIdProperty PrintByIdProperty = P<PackingBarcode>.RegisterRefId(e => e.PrintById, ReferenceType.Normal);

        /// <summary>
        /// 打印人Id
        /// </summary>
        public double PrintById
        {
            get { return (double)GetRefId(PrintByIdProperty); }
            set { SetRefId(PrintByIdProperty, value); }
        }

        /// <summary>
        /// 打印人
        /// </summary>
        public static readonly RefEntityProperty<Employee> PrintByProperty = P<PackingBarcode>.RegisterRef(e => e.PrintBy, PrintByIdProperty);

        /// <summary>
        /// 打印人
        /// </summary>
        public Employee PrintBy
        {
            get { return GetRefEntity(PrintByProperty); }
            set { SetRefEntity(PrintByProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 包装单位ID PackageUnitId
        /// <summary>
        /// 包装单位ID
        /// </summary>
        [Label("包装单位ID")]
        public static readonly Property<double> PackageUnitIdProperty
            = P<PackingBarcode>.RegisterView(e => e.PackageUnitId, p => p.PackageRuleDetail.PackageUnitId);

        /// <summary>
        /// 包装单位ID
        /// </summary>
        public double PackageUnitId
        {
            get { return this.GetProperty(PackageUnitIdProperty); }
        }
        #endregion


        #region 包装单位 PackageUnitName
        /// <summary>
        /// 包装单位
        /// </summary>
        [Label("包装单位")]
        public static readonly Property<string> PackageUnitNameProperty = P<PackingBarcode>.RegisterView(e => e.PackageUnitName, p => p.PackageRuleDetail.PackageUnit.Name);

        /// <summary>
        /// 包装单位
        /// </summary>
        public string PackageUnitName
        {
            get { return this.GetProperty(PackageUnitNameProperty); }
        }
        #endregion


        #region 打印人 PrintByName
        /// <summary>
        /// 打印人
        /// </summary>
        [Label("打印人")]
        public static readonly Property<string> PrintByNameProperty = P<PackingBarcode>.RegisterView(e => e.PrintByName, p => p.PrintBy.Name);

        /// <summary>
        /// 包装单位
        /// </summary>
        public string PrintByName
        {
            get { return this.GetProperty(PrintByNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 包装号 实体配置
    /// </summary>
    internal class PackingBarcodeConfig : EntityConfig<PackingBarcode>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_PACKING_BARCODE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
