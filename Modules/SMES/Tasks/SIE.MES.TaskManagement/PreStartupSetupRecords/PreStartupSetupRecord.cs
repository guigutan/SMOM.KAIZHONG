using SIE.Common.Configs;
using SIE.Domain;
using SIE.MES.Common;
using SIE.MES.ItemEquipAccount;
using SIE.MES.TaskManagement.Configs;
using SIE.MES.TaskManagement.Dispatchs;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.PreStartupSetupRecords
{
    /// <summary>
    /// 开机准备记录查询
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(PreStartupSetupRecordConfig))]
    [ConditionQueryType(typeof(PreStartupSetupRecordCriteria))]
    [Label("开机准备记录查询")]
    public class PreStartupSetupRecord : DataEntity
    {
        #region 派工单 DispatchTask
        /// <summary>
        /// 派工单Id
        /// </summary>
        [Label("派工单")]
        public static readonly IRefIdProperty DispatchTaskIdProperty =
            P<PreStartupSetupRecord>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Normal);

        /// <summary>
        /// 派工单Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)this.GetRefId(DispatchTaskIdProperty); }
            set { this.SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工单
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty =
            P<PreStartupSetupRecord>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工单
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return this.GetRefEntity(DispatchTaskProperty); }
            set { this.SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 编码 ToolCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> ToolCodeProperty = P<PreStartupSetupRecord>.Register(e => e.ToolCode);

        /// <summary>
        /// 编码
        /// </summary>
        public string ToolCode
        {
            get { return this.GetProperty(ToolCodeProperty); }
            set { this.SetProperty(ToolCodeProperty, value); }
        }
        #endregion

        #region 名称 ToolName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> ToolNameProperty = P<PreStartupSetupRecord>.Register(e => e.ToolName);

        /// <summary>
        /// 名称
        /// </summary>
        public string ToolName
        {
            get { return this.GetProperty(ToolNameProperty); }
            set { this.SetProperty(ToolNameProperty, value); }
        }
        #endregion

        #region 状态 ToolState
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<string> ToolStateProperty = P<PreStartupSetupRecord>.Register(e => e.ToolState);

        /// <summary>
        /// 状态
        /// </summary>
        public string ToolState
        {
            get { return this.GetProperty(ToolStateProperty); }
            set { this.SetProperty(ToolStateProperty, value); }
        }
        #endregion

        #region 图号 DrawingNo
        /// <summary>
        /// 图号
        /// </summary>
        [Label("图号")]
        public static readonly Property<string> DrawingNoProperty = P<PreStartupSetupRecord>.Register(e => e.DrawingNo);

        /// <summary>
        /// 图号
        /// </summary>
        public string DrawingNo
        {
            get { return this.GetProperty(DrawingNoProperty); }
            set { this.SetProperty(DrawingNoProperty, value); }
        }
        #endregion

        #region 唯一码 UniqueCode
        /// <summary>
        /// 唯一码
        /// </summary>
        [Label("唯一码")]
        public static readonly Property<string> UniqueCodeProperty = P<PreStartupSetupRecord>.Register(e => e.UniqueCode);

        /// <summary>
        /// 唯一码
        /// </summary>
        public string UniqueCode
        {
            get { return this.GetProperty(UniqueCodeProperty); }
            set { this.SetProperty(UniqueCodeProperty, value); }
        }
        #endregion

        #region 类型 CheckerFixtureType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<CheckerFixtureType> CheckerFixtureTypeProperty = P<PreStartupSetupRecord>.Register(e => e.CheckerFixtureType);

        /// <summary>
        /// 类型
        /// </summary>
        public CheckerFixtureType CheckerFixtureType
        {
            get { return this.GetProperty(CheckerFixtureTypeProperty); }
            set { this.SetProperty(CheckerFixtureTypeProperty, value); }
        }
        #endregion

        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal?> QtyProperty = P<PreStartupSetupRecord>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal? Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 工单号 WorkOrderNo
        /// <summary>
        /// 工单号
        /// </summary>
        [Label("工单号")]
        public static readonly Property<string> WorkOrderNoProperty = P<PreStartupSetupRecord>.RegisterView(e => e.WorkOrderNo, p => p.DispatchTask.WorkOrder.No);

        /// <summary>
        /// 工单号
        /// </summary>
        public string WorkOrderNo
        {
            get { return this.GetProperty(WorkOrderNoProperty); }
        }
        #endregion

        #region 派工单 TaskNo
        /// <summary>
        /// 派工单
        /// </summary>
        [Label("派工单")]
        public static readonly Property<string> TaskNoProperty = P<PreStartupSetupRecord>.RegisterView(e => e.TaskNo, p => p.DispatchTask.No);

        /// <summary>
        /// 派工单
        /// </summary>
        public string TaskNo
        {
            get { return this.GetProperty(TaskNoProperty); }
        }
        #endregion

        #region 工序编码 ProcessCode
        /// <summary>
        /// 工序编码编码
        /// </summary>
        [Label("工序编码")]
        public static readonly Property<string> ProcessCodeProperty = P<PreStartupSetupRecord>.RegisterView(e => e.ProcessCode, p => p.DispatchTask.Process.Code);

        /// <summary>
        /// 工序编码
        /// </summary>
        public string ProcessCode
        {
            get { return this.GetProperty(ProcessCodeProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<PreStartupSetupRecord>.RegisterView(e => e.ProcessName, p => p.DispatchTask.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 机台号 ResourceCode
        /// <summary>
        /// 机台号
        /// </summary>
        [Label("机台号")]
        public static readonly Property<string> ResourceCodeProperty = P<PreStartupSetupRecord>.RegisterView(e => e.ResourceCode, p => p.DispatchTask.Resource.Code);

        /// <summary>
        /// 机台号
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }
        #endregion

        #region 机台名称 ResourceName
        /// <summary>
        /// 机台名称
        /// </summary>
        [Label("机台名称")]
        public static readonly Property<string> ResourceNameProperty = P<PreStartupSetupRecord>.RegisterView(e => e.ResourceName, p => p.DispatchTask.Resource.Name);

        /// <summary>
        /// 机台名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }
        #endregion

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<PreStartupSetupRecord>.RegisterView(e => e.ItemCode, p => p.DispatchTask.Product.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return this.GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<PreStartupSetupRecord>.RegisterView(e => e.ItemName, p => p.DispatchTask.Product.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return this.GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<PreStartupSetupRecord>.RegisterView(e => e.ShortDescription, p => p.DispatchTask.Product.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }
        #endregion

        #endregion
    }

    internal class PreStartupSetupRecordEntityConfig : EntityConfig<PreStartupSetupRecord>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("PRE_START_SETUP_REC").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
