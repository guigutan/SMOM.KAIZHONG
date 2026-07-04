using SIE.Defects;
using SIE.Domain;
using SIE.MES.Wip.Repairs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.WIP.Repairs
{
    /// <summary>
    /// 维修缺陷保存记录
    /// </summary>
    [ChildEntity, Serializable]
    [Label("维修缺陷保存记录")]
    public class RepairDefectRecord : DataEntity
    {
        #region 维修记录 RepairRecord
        /// <summary>
        /// 维修记录Id
        /// </summary>
        [Label("维修记录")]
        public static readonly IRefIdProperty RepairRecordIdProperty =
            P<RepairDefectRecord>.RegisterRefId(e => e.RepairRecordId, ReferenceType.Parent);

        /// <summary>
        /// 维修记录Id
        /// </summary>
        public double RepairRecordId
        {
            get { return (double)this.GetRefId(RepairRecordIdProperty); }
            set { this.SetRefId(RepairRecordIdProperty, value); }
        }

        /// <summary>
        /// 维修记录
        /// </summary>
        public static readonly RefEntityProperty<RepairMainRecord> RepairRecordProperty =
            P<RepairDefectRecord>.RegisterRef(e => e.RepairRecord, RepairRecordIdProperty);

        /// <summary>
        /// 维修记录
        /// </summary>
        public RepairMainRecord RepairRecord
        {
            get { return this.GetRefEntity(RepairRecordProperty); }
            set { this.SetRefEntity(RepairRecordProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<RepairDefectRecord>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double ProcessId
        {
            get { return (double)this.GetRefId(ProcessIdProperty); }
            set { this.SetRefId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<RepairDefectRecord>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 产品缺陷ID ProductDefectId
        /// <summary>
        /// 产品缺陷ID
        /// </summary>
        [Label("产品缺陷ID")]
        public static readonly Property<double?> ProductDefectIdProperty = P<RepairDefectRecord>.Register(e => e.ProductDefectId);

        /// <summary>
        /// 产品缺陷ID
        /// </summary>
        public double? ProductDefectId
        {
            get { return this.GetProperty(ProductDefectIdProperty); }
            set { this.SetProperty(ProductDefectIdProperty, value); }
        }
        #endregion

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty =
            P<RepairDefectRecord>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

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
            P<RepairDefectRecord>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 实际缺陷 ActualDefect
        /// <summary>
        /// 实际缺陷Id
        /// </summary>
        [Label("实际缺陷")]
        public static readonly IRefIdProperty ActualDefectIdProperty =
            P<RepairDefectRecord>.RegisterRefId(e => e.ActualDefectId, ReferenceType.Normal);

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
            P<RepairDefectRecord>.RegisterRef(e => e.ActualDefect, ActualDefectIdProperty);

        /// <summary>
        /// 实际缺陷
        /// </summary>
        public Defect ActualDefect
        {
            get { return this.GetRefEntity(ActualDefectProperty); }
            set { this.SetRefEntity(ActualDefectProperty, value); }
        }
        #endregion

        #region 维修方案 RepairSolution
        /// <summary>
        /// 维修方案Id
        /// </summary>
        [Label("维修方案")]
        public static readonly IRefIdProperty RepairSolutionIdProperty =
            P<RepairDefectRecord>.RegisterRefId(e => e.RepairSolutionId, ReferenceType.Normal);

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
            P<RepairDefectRecord>.RegisterRef(e => e.RepairSolution, RepairSolutionIdProperty);

        /// <summary>
        /// 维修方案
        /// </summary>
        public DefectRepairSolution RepairSolution
        {
            get { return this.GetRefEntity(RepairSolutionProperty); }
            set { this.SetRefEntity(RepairSolutionProperty, value); }
        }
        #endregion 

        #region 维修位置 RepairLocation
        /// <summary>
        /// 维修位置
        /// </summary>
        [Label("维修位置")]
        public static readonly Property<string> RepairLocationProperty = P<RepairDefectRecord>.Register(e => e.RepairLocation);

        /// <summary>
        /// 维修位置
        /// </summary>
        public string RepairLocation
        {
            get { return this.GetProperty(RepairLocationProperty); }
            set { this.SetProperty(RepairLocationProperty, value); }
        }
        #endregion

        #region 换料条码 ReloadBarcode
        /// <summary>
        /// 换料条码
        /// </summary>
        [Label("换料条码")]
        public static readonly Property<string> ReloadBarcodeProperty = P<RepairDefectRecord>.Register(e => e.ReloadBarcode);

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
        [MaxLength(2000)]
        public static readonly Property<string> RemarkProperty = P<RepairDefectRecord>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 是否新增 IsNewAdd
        /// <summary>
        /// 是否新增
        /// </summary>
        [Label("是否新增")]
        public static readonly Property<bool> IsNewAddProperty = P<RepairDefectRecord>.Register(e => e.IsNewAdd);

        /// <summary>
        /// 是否新增
        /// </summary>
        public bool IsNewAdd
        {
            get { return this.GetProperty(IsNewAddProperty); }
            set { this.SetProperty(IsNewAddProperty, value); }
        }
        #endregion

        #region 维修措施列表 MeasureList
        /// <summary>
        /// 维修措施列表
        /// </summary>
        [Label("维修措施列表")]
        public static readonly ListProperty<EntityList<RepairMeasureRecord>> MeasureListProperty = P<RepairDefectRecord>.RegisterList(e => e.MeasureList);

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public EntityList<RepairMeasureRecord> MeasureList
        {
            get { return this.GetLazyList(MeasureListProperty); }
        }
        #endregion

        #region 缺陷责任列表 ResponseList
        /// <summary>
        /// 缺陷责任列表
        /// </summary>
        [Label("缺陷责任列表")]
        public static readonly ListProperty<EntityList<RepairResponseRecord>> ResponseListProperty = P<RepairDefectRecord>.RegisterList(e => e.ResponseList);

        /// <summary>
        /// 缺陷责任列表
        /// </summary>
        public EntityList<RepairResponseRecord> ResponseList
        {
            get { return this.GetLazyList(ResponseListProperty); }
        }
        #endregion

        #region 视图属性
        #region 缺陷编码 DefectCode
        /// <summary>
        /// 缺陷编码
        /// </summary>
        [Label("缺陷编码")]
        public static readonly Property<string> DefectCodeProperty = P<RepairDefectRecord>.RegisterView(e => e.DefectCode, p => p.Defect.Code);

        /// <summary>
        /// 缺陷编码
        /// </summary>
        public string DefectCode
        {
            get { return this.GetProperty(DefectCodeProperty); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<RepairDefectRecord>.RegisterView(e => e.DefectDesc, p => p.Defect.Description);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get { return this.GetProperty(DefectDescProperty); }
        }
        #endregion

        #region 实际缺陷编码 ActualDefectCode
        /// <summary>
        /// 实际缺陷编码
        /// </summary>
        [Label("实际缺陷编码")]
        public static readonly Property<string> ActualDefectCodeProperty = P<RepairDefectRecord>.RegisterView(e => e.ActualDefectCode, p => p.ActualDefect.Code);

        /// <summary>
        /// 实际缺陷编码
        /// </summary>
        public string ActualDefectCode
        {
            get { return this.GetProperty(ActualDefectCodeProperty); }
        }
        #endregion

        #region 实际缺陷描述 ActualDefectDesc
        /// <summary>
        /// 实际缺陷描述
        /// </summary>
        [Label("实际缺陷描述")]
        public static readonly Property<string> ActualDefectDescProperty = P<RepairDefectRecord>.RegisterView(e => e.ActualDefectDesc, p => p.ActualDefect.Description);

        /// <summary>
        /// 实际缺陷描述
        /// </summary>
        public string ActualDefectDesc
        {
            get { return this.GetProperty(ActualDefectDescProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("属性名")]
        public static readonly Property<string> ProcessNameProperty = P<RepairDefectRecord>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 维修缺陷保存记录 实体配置
    /// </summary>
    internal class RepairDefectRecordConfig : EntityConfig<RepairDefectRecord>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_REPAIRE_DEF_RECORD").MapAllProperties();
            Meta.Property(RepairDefectRecord.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.DisablePhantoms();
        }
    }
}