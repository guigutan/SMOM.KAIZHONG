using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.TaskManagement.Dispatchs
{
    /// <summary>
    /// 任务工序BOM
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("任务工序BOM")]
    public partial class TaskProcessBom : DataEntity
    {
        #region 物料 Item
        /// <summary>
        /// 物料Id
        /// </summary>
        [Label("物料")]
        public static readonly IRefIdProperty ItemIdProperty = P<TaskProcessBom>.RegisterRefId(e => e.ItemId, ReferenceType.Normal);

        /// <summary>
        /// 物料Id
        /// </summary>
        public double ItemId
        {
            get { return (double)GetRefId(ItemIdProperty); }
            set { SetRefId(ItemIdProperty, value); }
        }

        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        public static readonly RefEntityProperty<Item> ItemProperty = P<TaskProcessBom>.RegisterRef(e => e.Item, ItemIdProperty);

        /// <summary>
        /// 物料
        /// </summary>
        public Item Item
        {
            get { return GetRefEntity(ItemProperty); }
            set { SetRefEntity(ItemProperty, value); }
        }
        #endregion

        #region 单机定额 Qty
        /// <summary>
        /// 单机定额
        /// </summary>
        [Label("单机定额")]
        public static readonly Property<decimal> QtyProperty = P<TaskProcessBom>.Register(e => e.Qty);

        /// <summary>
        /// 单机定额
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 使用数量 UseQty
        /// <summary>
        /// 使用数量
        /// </summary>
        [Label("使用数量")]
        public static readonly Property<decimal> UseQtyProperty = P<TaskProcessBom>.Register(e => e.UseQty);

        /// <summary>
        /// 使用数量
        /// </summary>
        public decimal UseQty
        {
            get { return GetProperty(UseQtyProperty); }
            set { SetProperty(UseQtyProperty, value); }
        }
        #endregion

        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<TaskProcessBom>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

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
            P<TaskProcessBom>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 派工任务 DispatchTask
        /// <summary>
        /// 派工任务Id
        /// </summary>
        [Label("派工任务")]
        public static readonly IRefIdProperty DispatchTaskIdProperty = P<TaskProcessBom>.RegisterRefId(e => e.DispatchTaskId, ReferenceType.Parent);

        /// <summary>
        /// 派工任务Id
        /// </summary>
        public double DispatchTaskId
        {
            get { return (double)GetRefId(DispatchTaskIdProperty); }
            set { SetRefId(DispatchTaskIdProperty, value); }
        }

        /// <summary>
        /// 派工任务
        /// </summary>
        public static readonly RefEntityProperty<DispatchTask> DispatchTaskProperty = P<TaskProcessBom>.RegisterRef(e => e.DispatchTask, DispatchTaskIdProperty);

        /// <summary>
        /// 派工任务
        /// </summary>
        public DispatchTask DispatchTask
        {
            get { return GetRefEntity(DispatchTaskProperty); }
            set { SetRefEntity(DispatchTaskProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 物料编码 ItemCode
        /// <summary>
        /// 物料编码
        /// </summary>
        [Label("物料编码")]
        public static readonly Property<string> ItemCodeProperty = P<TaskProcessBom>.RegisterView(e => e.ItemCode, e => e.Item.Code);

        /// <summary>
        /// 物料编码
        /// </summary>
        public string ItemCode
        {
            get { return GetProperty(ItemCodeProperty); }
        }
        #endregion

        #region 物料名称 ItemName
        /// <summary>
        /// 物料名称
        /// </summary>
        [Label("物料名称")]
        public static readonly Property<string> ItemNameProperty = P<TaskProcessBom>.RegisterView(e => e.ItemName, e => e.Item.Name);

        /// <summary>
        /// 物料名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<TaskProcessBom>.RegisterView(e => e.UnitName, e => e.Item.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return GetProperty(UnitNameProperty); }
        }
        #endregion

        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<TaskProcessBom>.RegisterView(e => e.ProcessName, e => e.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return GetProperty(ProcessNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 任务工序BOM 实体配置
    /// </summary>
    internal class TaskProcessBomConfig : EntityConfig<TaskProcessBom>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_TASK_PRO_BOM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}