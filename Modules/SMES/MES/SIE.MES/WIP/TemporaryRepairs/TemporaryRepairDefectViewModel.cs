using SIE.Defects;
using SIE.Defects.Measures;
using SIE.Domain;
using SIE.MES.Wip.Repairs;
using SIE.MES.WIP.Products;
using SIE.MES.WorkOrders;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 维修缺陷视图模型
    /// </summary>
    [RootEntity, Serializable]
    [Label("临时维修缺陷视图模型")]
    public class TemporaryRepairDefectViewModel : ViewModel
    {
        #region 工单 WorkOrder
        /// <summary>
        /// 工单Id
        /// </summary>
        [Label("工单")]
        public static readonly IRefIdProperty WorkOrderIdProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRefId(e => e.WorkOrderId, ReferenceType.Normal);

        /// <summary>
        /// 工单Id
        /// </summary>
        public double WorkOrderId
        {
            get { return (double)this.GetRefId(WorkOrderIdProperty); }
            set { this.SetRefId(WorkOrderIdProperty, value); }
        }

        /// <summary>
        /// 工单
        /// </summary>
        public static readonly RefEntityProperty<WorkOrder> WorkOrderProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRef(e => e.WorkOrder, WorkOrderIdProperty);

        /// <summary>
        /// 工单
        /// </summary>
        public WorkOrder WorkOrder
        {
            get { return this.GetRefEntity(WorkOrderProperty); }
            set { this.SetRefEntity(WorkOrderProperty, value); }
        }
        #endregion 

        #region 产品条码 Sn
        /// <summary>
        /// 产品条码
        /// </summary>
        [Label("产品条码")]
        public static readonly Property<string> SnProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.Sn);

        /// <summary>
        /// 产品条码
        /// </summary>
        public string Sn
        {
            get { return this.GetProperty(SnProperty); }
            set { this.SetProperty(SnProperty, value); }
        }
        #endregion

        #region 产品缺陷记录 WipProductDefect 
        /// <summary>
        /// 产品缺陷记录ID
        /// </summary>
        [Label("产品缺陷记录")]
        public static readonly IRefIdProperty WipProductDefectIdProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRefId(e => e.WipProductDefectId, ReferenceType.Normal);

        /// <summary>
        /// 产品缺陷记录ID
        /// </summary>
        public double WipProductDefectId
        {
            get { return (double)this.GetRefId(WipProductDefectIdProperty); }
            set { this.SetRefId(WipProductDefectIdProperty, value); }
        }

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductDefect> WipProductDefectProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRef(e => e.WipProductDefect, WipProductDefectIdProperty);

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public WipProductDefect WipProductDefect
        {
            get { return this.GetRefEntity(WipProductDefectProperty); }
            set { this.SetRefEntity(WipProductDefectProperty, value); }
        }
        #endregion

        #region 产品维修记录 WipProductRepair
        /// <summary>
        /// 产品维修记录Id
        /// </summary>
        [Label("产品维修记录")]
        public static readonly IRefIdProperty WipProductRepairIdProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRefId(e => e.WipProductRepairId, ReferenceType.Normal);

        /// <summary>
        /// 产品维修记录Id
        /// </summary>
        public double? WipProductRepairId
        {
            get { return (double?)this.GetRefNullableId(WipProductRepairIdProperty); }
            set { this.SetRefNullableId(WipProductRepairIdProperty, value); }
        }

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductRepair> WipProductRepairProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRef(e => e.WipProductRepair, WipProductRepairIdProperty);

        /// <summary>
        /// 产品维修记录
        /// </summary>
        public WipProductRepair WipProductRepair
        {
            get { return this.GetRefEntity(WipProductRepairProperty); }
            set { this.SetRefEntity(WipProductRepairProperty, value); }
        }
        #endregion 

        #region 工序ID ProcessId
        /// <summary>
        /// 工序ID
        /// </summary>
        [Label("工序ID")]
        public static readonly Property<double> ProcessIdProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.ProcessId);

        /// <summary>
        /// 工序ID
        /// </summary>
        public double ProcessId
        {
            get { return this.GetProperty(ProcessIdProperty); }
            set { this.SetProperty(ProcessIdProperty, value); }
        }
        #endregion 

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序")]
        public static readonly Property<string> ProcessNameProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.ProcessName);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion 

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double DefectId
        {
            get { return (double)this.GetRefId(DefectIdProperty); }
            set { this.SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 缺陷代码 DefectCode
        /// <summary>
        /// 缺陷代码
        /// </summary>
        [Label("缺陷代码")]
        public static readonly Property<string> DefectCodeProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.DefectCode);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public string DefectCode
        {
            get { return this.GetProperty(DefectCodeProperty); }
            set { this.SetProperty(DefectCodeProperty, value); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.DefectDesc);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get { return this.GetProperty(DefectDescProperty); }
            set { this.SetProperty(DefectDescProperty, value); }
        }
        #endregion

        #region 缺陷位置 DefectLocation
        /// <summary>
        /// 缺陷位置
        /// </summary>
        [Label("缺陷位置")]
        public static readonly Property<string> DefectLocationProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.DefectLocation);

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string DefectLocation
        {
            get { return this.GetProperty(DefectLocationProperty); }
            set { this.SetProperty(DefectLocationProperty, value); }
        }
        #endregion

        #region 维修措施编码 MeasureCode
        /// <summary>
        /// 维修措施编码
        /// </summary>
        [Label("维修措施编码")]
        public static readonly Property<string> MeasureCodeProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.MeasureCode);

        /// <summary>
        /// 维修措施编码
        /// </summary>
        public string MeasureCode
        {
            get { return this.GetProperty(MeasureCodeProperty); }
            set { this.SetProperty(MeasureCodeProperty, value); }
        }
        #endregion

        #region 维修措施描述 MeasureDesc
        /// <summary>
        /// 维修措施描述
        /// </summary>
        [Label("维修措施描述")]
        public static readonly Property<string> MeasureDescProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.MeasureDesc);

        /// <summary>
        /// 维修措施描述
        /// </summary>
        public string MeasureDesc
        {
            get { return this.GetProperty(MeasureDescProperty); }
            set { this.SetProperty(MeasureDescProperty, value); }
        }
        #endregion

        #region 缺陷责任 Responsibility
        /// <summary>
        /// 缺陷责任
        /// </summary>
        [Label("缺陷责任")]
        public static readonly Property<string> ResponsibilityProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.Responsibility);

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public string Responsibility
        {
            get { return this.GetProperty(ResponsibilityProperty); }
            set { this.SetProperty(ResponsibilityProperty, value); }
        }
        #endregion

        #region 维修位置 RepairLocation
        /// <summary>
        /// 维修位置 点位
        /// </summary>
        [Label("维修位置")]
        public static readonly Property<string> RepairLocationProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.RepairLocation);

        /// <summary>
        /// 维修位置
        /// </summary>
        public string RepairLocation
        {
            get { return this.GetProperty(RepairLocationProperty); }
            set { this.SetProperty(RepairLocationProperty, value); }
        }
        #endregion

        #region 实际缺陷 ActualDefect
        /// <summary>
        /// 实际缺陷Id
        /// </summary>
        [Label("实际缺陷")]
        public static readonly IRefIdProperty ActualDefectIdProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRefId(e => e.ActualDefectId, ReferenceType.Normal);

        /// <summary>
        /// 实际缺陷Id
        /// </summary>
        public double? ActualDefectId
        {
            get { return (double?)this.GetRefNullableId(ActualDefectIdProperty); }
            set { this.SetRefNullableId(ActualDefectIdProperty, value); }
        }

        /// <summary>
        /// 实际缺陷
        /// </summary>
        public static readonly RefEntityProperty<Defect> ActualDefectProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRef(e => e.ActualDefect, ActualDefectIdProperty);

        /// <summary>
        /// 实际缺陷
        /// </summary>
        public Defect ActualDefect
        {
            get { return this.GetRefEntity(ActualDefectProperty); }
            set { this.SetRefEntity(ActualDefectProperty, value); }
        }
        #endregion

        #region 实际缺陷编码 ActualDefectCode
        /// <summary>
        /// 实际缺陷编码
        /// </summary>
        [Label("实际缺陷编码")]
        public static readonly Property<string> ActualDefectCodeProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.ActualDefectCode);

        /// <summary>
        /// 实际缺陷编码
        /// </summary>
        public string ActualDefectCode
        {
            get { return this.GetProperty(ActualDefectCodeProperty); }
            set { this.SetProperty(ActualDefectCodeProperty, value); }
        }
        #endregion

        #region 实际缺陷描述 ActualDefectDesc
        /// <summary>
        /// 实际缺陷描述
        /// </summary>
        [Label("实际缺陷描述")]
        public static readonly Property<string> ActualDefectDescProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.ActualDefectDesc);

        /// <summary>
        /// 实际缺陷描述
        /// </summary>
        public string ActualDefectDesc
        {
            get { return this.GetProperty(ActualDefectDescProperty); }
            set { this.SetProperty(ActualDefectDescProperty, value); }
        }
        #endregion

        #region 换料条码 ReloadBarcode
        /// <summary>
        /// 换料条码
        /// </summary>
        [Label("换料条码")]
        public static readonly Property<string> ReloadBarcodeProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.ReloadBarcode);

        /// <summary>
        /// 换料条码
        /// </summary>
        public string ReloadBarcode
        {
            get { return this.GetProperty(ReloadBarcodeProperty); }
            set { this.SetProperty(ReloadBarcodeProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 维修员 MaintenanceMan
        /// <summary>
        /// 维修员Id
        /// </summary>
        [Label("维修员")]
        public static readonly IRefIdProperty MaintenanceManIdProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRefId(e => e.MaintenanceManId, ReferenceType.Normal);

        /// <summary>
        /// 维修员Id
        /// </summary>
        public double MaintenanceManId
        {
            get { return (double)this.GetRefId(MaintenanceManIdProperty); }
            set { this.SetRefId(MaintenanceManIdProperty, value); }
        }

        /// <summary>
        /// 维修员
        /// </summary>
        public static readonly RefEntityProperty<Employee> MaintenanceManProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRef(e => e.MaintenanceMan, MaintenanceManIdProperty);

        /// <summary>
        /// 维修员
        /// </summary>
        public Employee MaintenanceMan
        {
            get { return this.GetRefEntity(MaintenanceManProperty); }
            set { this.SetRefEntity(MaintenanceManProperty, value); }
        }
        #endregion

        #region 维修员 Maintenance
        /// <summary>
        /// 维修员
        /// </summary>
        [Label("维修员")]
        public static readonly Property<string> MaintenanceProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.Maintenance);

        /// <summary>
        /// 维修员
        /// </summary>
        public string Maintenance
        {
            get { return this.GetProperty(MaintenanceProperty); }
            set { this.SetProperty(MaintenanceProperty, value); }
        }
        #endregion

        #region 维修时间 RepairDate
        /// <summary>
        /// 维修时间
        /// </summary>
        [Label("维修时间")]
        public static readonly Property<DateTime?> RepairDateProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.RepairDate);

        /// <summary>
        /// 维修时间
        /// </summary>
        public DateTime? RepairDate
        {
            get { return this.GetProperty(RepairDateProperty); }
            set { this.SetProperty(RepairDateProperty, value); }
        }
        #endregion

        #region 是否新增 IsNewAdd
        /// <summary>
        /// 是否新增
        /// </summary>
        [Label("是否新增")]
        public static readonly Property<bool> IsNewAddProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.IsNewAdd, (s, e) =>
        {
            var vm = s as TemporaryRepairDefectViewModel;
            vm.ActualDefectEnable = !vm.IsNewAdd;
        });

        /// <summary>
        /// 是否新增
        /// </summary>
        public bool IsNewAdd
        {
            get { return this.GetProperty(IsNewAddProperty); }
            set { this.SetProperty(IsNewAddProperty, value); }
        }
        #endregion

        #region 实际缺陷只读 ActualDefectEnable
        /// <summary>
        /// 实际缺陷只读
        /// </summary>
        [Label("实际缺陷只读")]
        public static readonly Property<bool> ActualDefectEnableProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.ActualDefectEnable);

        /// <summary>
        /// 实际缺陷只读
        /// </summary>
        public bool ActualDefectEnable
        {
            get { return this.GetProperty(ActualDefectEnableProperty); }
            set { this.SetProperty(ActualDefectEnableProperty, value); }
        }
        #endregion

        #region 维修措施 MeasureList
        /// <summary>
        /// 维修措施
        /// </summary> 
        [Label("维修措施")]
        public static readonly ListProperty<EntityList<RepairMeasure>> MeasureListProperty = P<TemporaryRepairDefectViewModel>.RegisterList(e => e.MeasureList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<RepairMeasure>()
        });

        /// <summary>
        /// 维修措施
        /// </summary>
        public EntityList<RepairMeasure> MeasureList
        {
            get { return this.GetLazyList(MeasureListProperty); }
        }
        #endregion

        #region 缺陷责任 ResponsibilityList
        /// <summary>
        /// 缺陷责任
        /// </summary> 
        [Label("缺陷责任")]
        public static readonly ListProperty<EntityList<DefectResponsibility>> ResponsibilityListProperty = P<TemporaryRepairDefectViewModel>.RegisterList(e => e.ResponsibilityList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => new EntityList<DefectResponsibility>()
        });

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public EntityList<DefectResponsibility> ResponsibilityList
        {
            get { return this.GetLazyList(ResponsibilityListProperty); }
        }
        #endregion

        #region 报废数量 ScrapQty
        /// <summary>
        /// 报废数量
        /// </summary>
        [Label("报废数量")]
        public static readonly Property<decimal> ScrapQtyProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.ScrapQty);

        /// <summary>
        /// 报废数量
        /// </summary>
        public decimal ScrapQty
        {
            get { return this.GetProperty(ScrapQtyProperty); }
            set { this.SetProperty(ScrapQtyProperty, value); }
        }
        #endregion

        #region 报废原因 ScrapReason
        /// <summary>
        /// 报废原因
        /// </summary>
        [Label("报废原因")]
        public static readonly Property<string> ScrapReasonProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.ScrapReason);

        /// <summary>
        /// 报废原因
        /// </summary>
        public string ScrapReason
        {
            get { return this.GetProperty(ScrapReasonProperty); }
            set { this.SetProperty(ScrapReasonProperty, value); }
        }
        #endregion

        #region 是否修好 IsFixed
        /// <summary>
        /// 是否修好
        /// </summary>
        [Label("是否修好")]
        public static readonly Property<bool> IsFixedProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.IsFixed);

        /// <summary>
        /// 是否修好
        /// </summary>
        public bool IsFixed
        {
            get { return this.GetProperty(IsFixedProperty); }
            set { this.SetProperty(IsFixedProperty, value); }
        }
        #endregion

        #region 缺陷描述 DefectDescription
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescriptionProperty = P<TemporaryRepairDefectViewModel>.RegisterView(e => e.DefectDescription, p => p.WipProductDefect.Defect.Description);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDescription
        {
            get { return this.GetProperty(DefectDescriptionProperty); }
        }
        #endregion

        #region 维修录入
        #region 维修措施 MeasureBarcode
        /// <summary>
        /// 维修措施
        /// </summary>
        [Label("维修措施")]
        public static readonly Property<string> MeasureBarcodeProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.MeasureBarcode);

        /// <summary>
        /// 维修措施
        /// </summary>
        public string MeasureBarcode
        {
            get { return this.GetProperty(MeasureBarcodeProperty); }
            set { this.SetProperty(MeasureBarcodeProperty, value); }
        }
        #endregion

        #region 维修方案 RepairSolution
        /// <summary>
        /// 维修方案Id
        /// </summary>
        [Label("维修方案")]
        public static readonly IRefIdProperty RepairSolutionIdProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRefId(e => e.RepairSolutionId, ReferenceType.Normal);

        /// <summary>
        /// 维修方案Id
        /// </summary>
        public double? RepairSolutionId
        {
            get { return (double?)this.GetRefNullableId(RepairSolutionIdProperty); }
            set { this.SetRefNullableId(RepairSolutionIdProperty, value); }
        }

        /// <summary>
        /// 维修方案
        /// </summary>
        public static readonly RefEntityProperty<DefectRepairSolution> RepairSolutionProperty =
            P<TemporaryRepairDefectViewModel>.RegisterRef(e => e.RepairSolution, RepairSolutionIdProperty);

        /// <summary>
        /// 维修方案
        /// </summary>
        public DefectRepairSolution RepairSolution
        {
            get { return this.GetRefEntity(RepairSolutionProperty); }
            set { this.SetRefEntity(RepairSolutionProperty, value); }
        }
        #endregion

        #region 换料条码 ReloadSn
        /// <summary>
        /// 换料条码，扫描框输入
        /// </summary>
        [Label("换料条码")]
        public static readonly Property<string> ReloadSnProperty = P<TemporaryRepairDefectViewModel>.Register(e => e.ReloadSn);

        /// <summary>
        /// 换料条码
        /// </summary>
        public string ReloadSn
        {
            get { return this.GetProperty(ReloadSnProperty); }
            set { this.SetProperty(ReloadSnProperty, value); }
        }
        #endregion 

        #endregion
    }
}