using SIE.Domain;
using SIE.Items;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.TaskConfigs
{
    /// <summary>
	/// 任务单生成配置项
	/// </summary>
	[RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("任务单生成配置项")]
    public partial class TaskConfig : DataEntity
    {
        #region 按照工序生成任务单 ByProcess
        /// <summary>
        /// 按照工序生成任务单
        /// </summary>
        [Label("按照工序生成任务单")]
        public static readonly Property<bool> ByProcessProperty = P<TaskConfig>.Register(e => e.ByProcess);

        /// <summary>
        /// 按照工序生成任务单
        /// </summary>
        public bool ByProcess
        {
            get { return GetProperty(ByProcessProperty); }
            set { SetProperty(ByProcessProperty, value); }
        }
        #endregion

        #region 按照规格件生成任务单 BySpecification
        /// <summary>
        /// 按照规格件生成任务单
        /// </summary>
        [Label("按照规格件生成任务单")]
        public static readonly Property<bool> BySpecificationProperty = P<TaskConfig>.Register(e => e.BySpecification);

        /// <summary>
        /// 按照规格件生成任务单
        /// </summary>
        public bool BySpecification
        {
            get { return GetProperty(BySpecificationProperty); }
            set { SetProperty(BySpecificationProperty, value); }
        }
        #endregion

        #region 按照固定数量生成任务单 ByQty
        /// <summary>
        /// 按照固定数量生成任务单
        /// </summary>
        [Label("按照固定数量生成任务单")]
        public static readonly Property<bool> ByQtyProperty = P<TaskConfig>.Register(e => e.ByQty);

        /// <summary>
        /// 按照固定数量生成任务单
        /// </summary>
        public bool ByQty
        {
            get { return GetProperty(ByQtyProperty); }
            set { SetProperty(ByQtyProperty, value); }
        }
        #endregion

        #region 是否生成虚拟件任务 ByVirtualPart
        /// <summary>
        /// 是否生成虚拟件任务
        /// </summary>
        [Label("是否生成虚拟件任务")]
        public static readonly Property<bool> ByVirtualPartProperty = P<TaskConfig>.Register(e => e.ByVirtualPart);

        /// <summary>
        /// 是否生成虚拟件任务
        /// </summary>
        public bool ByVirtualPart
        {
            get { return GetProperty(ByVirtualPartProperty); }
            set { SetProperty(ByVirtualPartProperty, value); }
        }
        #endregion

        #region 固定数量 Qty
        /// <summary>
        /// 固定数量
        /// </summary>
        [Label("固定数量")]
        public static readonly Property<decimal> QtyProperty = P<TaskConfig>.Register(e => e.Qty);

        /// <summary>
        /// 固定数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 产品族 ProductFamily
        /// <summary>
        /// 产品族Id
        /// </summary>
        public static readonly IRefIdProperty ProductFamilyIdProperty = P<TaskConfig>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

        /// <summary>
        /// 产品族Id
        /// </summary>
        public double ProductFamilyId
        {
            get { return (double)GetRefId(ProductFamilyIdProperty); }
            set { SetRefId(ProductFamilyIdProperty, value); }
        }

        /// <summary>
        /// 产品族
        /// </summary>
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty = P<TaskConfig>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

        /// <summary>
        /// 产品族
        /// </summary>
        public ProductFamily ProductFamily
        {
            get { return GetRefEntity(ProductFamilyProperty); }
            set { SetRefEntity(ProductFamilyProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 任务单生成配置项 实体配置
    /// </summary>
    internal class TaskConfigConfig : EntityConfig<TaskConfig>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TM_TASK_CONFIG").MapAllProperties();
            Meta.Property(TaskConfig.ProductFamilyIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }

    /// <summary>
    /// 任务单生成配置信息
    /// </summary>
    [Serializable]
    public class TaskConfigInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskConfigInfo()
        {
            Family = 0;
        }

        /// <summary>
        /// 按照工序生成任务单
        /// </summary>
        public bool ByProcess { get; set; }

        /// <summary>
        /// 按照规格件生成任务单
        /// </summary>
        public bool BySpecification { get; set; }

        /// <summary>
        /// 按照固定数量生成任务单
        /// </summary>
        public bool ByQty { get; set; }

        /// <summary>
        /// 是否生成虚拟件任务
        /// </summary>
        public bool ByVirtualPart { get; set; }

        /// <summary>
        /// 固定数量
        /// </summary>
        public decimal Qty { get; set; }

        /// <summary>
        /// 控制界面控件禁用
        /// </summary>
        public int Family { get; set; }
    }

    /// <summary>
    /// 产品族任务单生成规则设置
    /// </summary>
    [Serializable]
    public class FamilyTaskConfig
    {
        /// <summary>
        /// 产品族ID
        /// </summary>
        public double FamilyId { get; set; }

        /// <summary>
        /// 任务单生成配置信息
        /// </summary>
        public TaskConfigInfo Config { get; set; } = new TaskConfigInfo();
    }
}
