using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.MES.TaskManagement.TaskConfigs
{
    /// <summary>
    /// 任务单生成配置项ViewModel
    /// </summary>
    [RootEntity]
    [Label("任务单生成配置项ViewModel")]
    public class TaskConfigViewModel : ViewModel
    {
        #region 按照工序生成任务单 ByProcess
        /// <summary>
        /// 按照工序生成任务单
        /// </summary>
        [Label("按照工序生成任务单")]
        public static readonly Property<bool> ByProcessProperty = P<TaskConfigViewModel>.Register(e => e.ByProcess);

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
        public static readonly Property<bool> BySpecificationProperty = P<TaskConfigViewModel>.Register(e => e.BySpecification);

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
        public static readonly Property<bool> ByQtyProperty = P<TaskConfigViewModel>.Register(e => e.ByQty);

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
        public static readonly Property<bool> ByVirtualPartProperty = P<TaskConfigViewModel>.Register(e => e.ByVirtualPart);

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
        public static readonly Property<decimal> QtyProperty = P<TaskConfigViewModel>.Register(e => e.Qty);

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
        public static readonly IRefIdProperty ProductFamilyIdProperty =
            P<TaskConfigViewModel>.RegisterRefId(e => e.ProductFamilyId, ReferenceType.Normal);

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
        public static readonly RefEntityProperty<ProductFamily> ProductFamilyProperty =
            P<TaskConfigViewModel>.RegisterRef(e => e.ProductFamily, ProductFamilyIdProperty);

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
    /// 任务单生成配置项扩展属性
    /// </summary>
    [CompiledPropertyDeclarer]
    public class TaskConfigExDetailProperty
    {
        #region TaskConfig TaskConfig (任务单生成配置项)
        /// <summary>
        /// 任务单生成配置项 扩展属性。
        /// </summary>
        public static readonly Property<TaskConfigViewModel> TaskConfigViewModelProperty =
            P<ProductFamily>.RegisterExtension<TaskConfigViewModel>("TaskConfigViewModel", typeof(TaskConfigExDetailProperty));

        /// <summary>
        /// 获取 任务单生成配置项 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        public static TaskConfigViewModel GetTaskConfigViewModel(ProductFamily me)
        {
            return me.GetProperty(TaskConfigViewModelProperty);
        }

        /// <summary>
        /// 设置 任务单生成配置项 属性的值。
        /// </summary>
        /// <param name="me">要设置扩展属性值的对象。</param>
        /// <param name="value">设置的值。</param>
        public static void SetItemPlanEx(ProductFamily me, TaskConfigViewModel value)
        {
            me.SetProperty(TaskConfigViewModelProperty, value);
        }
        #endregion
#pragma warning disable S1144 
        /// <summary>
        /// 产品族任务单生成配置项 实体配置
        /// </summary>
        internal class TaskConfigViewModelPropertyConfig : EntityConfig<ProductFamily>
        {
            /// <summary>
            /// 属性元数据配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(TaskConfigExDetailProperty.TaskConfigViewModelProperty).DontMapColumn();
            }
        }
#pragma warning restore S1144 
    }
}
