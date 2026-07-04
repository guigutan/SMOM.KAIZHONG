using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.BarcodeProcesses
{
    /// <summary>
    /// 条码工序指派明细
    /// </summary>
    [RootEntity, Serializable]
    [Label("条码工序指派明细")]
    public class BarcodeProDetail : DataEntity
    {
        #region 条码 BarcodeProcess
        /// <summary>
        /// 条码Id
        /// </summary>
        [Label("条码")]
        public static readonly IRefIdProperty BarcodeProcessIdProperty =
            P<BarcodeProDetail>.RegisterRefId(e => e.BarcodeProcessId, ReferenceType.Normal);

        /// <summary>
        /// 条码Id
        /// </summary>
        public double BarcodeProcessId
        {
            get { return (double)this.GetRefId(BarcodeProcessIdProperty); }
            set { this.SetRefId(BarcodeProcessIdProperty, value); }
        }

        /// <summary>
        /// 条码
        /// </summary>
        public static readonly RefEntityProperty<BarcodeProcess> BarcodeProcessProperty =
            P<BarcodeProDetail>.RegisterRef(e => e.BarcodeProcess, BarcodeProcessIdProperty);

        /// <summary>
        /// 条码
        /// </summary>
        public BarcodeProcess BarcodeProcess
        {
            get { return this.GetRefEntity(BarcodeProcessProperty); }
            set { this.SetRefEntity(BarcodeProcessProperty, value); }
        }
        #endregion

        #region 顺序号 NumberIndex
        /// <summary>
        /// 顺序号
        /// </summary>
        [Label("顺序号")]
        public static readonly Property<int?> NumberIndexProperty = P<BarcodeProDetail>.Register(e => e.NumberIndex);

        /// <summary>
        /// 顺序号
        /// </summary>
        public int? NumberIndex
        {
            get { return this.GetProperty(NumberIndexProperty); }
            set { this.SetProperty(NumberIndexProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<BarcodeProDetail>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<BarcodeProDetail>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion


        #region 员工Ids EmployeeIds
        /// <summary>
        /// 员工Ids
        /// </summary>
        [MaxLength(2000)]
        [Label("员工Ids")]
        public static readonly Property<string> EmployeeIdsProperty = P<BarcodeProDetail>.Register(e => e.EmployeeIds);

        /// <summary>
        /// 员工Ids
        /// </summary>
        public string EmployeeIds
        {
            get { return this.GetProperty(EmployeeIdsProperty); }
            set { this.SetProperty(EmployeeIdsProperty, value); }
        }
        #endregion

        #region 员工名称 EmployeeJoinNames
        /// <summary>
        /// 员工名称
        /// </summary>
        [MaxLength(2000)]
        [Label("员工名称")]
        public static readonly Property<string> EmployeeJoinNamesProperty = P<BarcodeProDetail>.Register(e => e.EmployeeJoinNames);

        /// <summary>
        /// 员工名称
        /// </summary>
        public string EmployeeJoinNames
        {
            get { return this.GetProperty(EmployeeJoinNamesProperty); }
            set { this.SetProperty(EmployeeJoinNamesProperty, value); }
        }
        #endregion

        #region 是否过站检验 IsCheck
        /// <summary>
        /// 是否过站检验
        /// </summary>
        [Label("是否过站检验")]
        public static readonly Property<bool> IsCheckProperty = P<BarcodeProDetail>.Register(e => e.IsCheck);

        /// <summary>
        /// 是否过站检验
        /// </summary>
        public bool IsCheck
        {
            get { return this.GetProperty(IsCheckProperty); }
            set { this.SetProperty(IsCheckProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<BarcodeProDetail>.RegisterView(e => e.ProcessCode, p => p.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
            set { this.SetProperty(ProcessCodeProperty, value); }
        }
        #endregion


        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<BarcodeProDetail>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
            set { this.SetProperty(ProcessNameProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 实体配置
    /// </summary>
    public class BarcodeProDetailConfig : EntityConfig<BarcodeProDetail>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BC_BARCODE_DTL").MapAllProperties();
            Meta.Property(BarcodeProDetail.EmployeeIdsProperty).ColumnMeta.HasLength(4000);
            Meta.Property(BarcodeProDetail.EmployeeJoinNamesProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}
