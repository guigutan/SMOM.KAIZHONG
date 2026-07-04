using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.TurnoverTools.TurnoverTools
{
    /// <summary>
    /// 周转工具
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("周转工具")]
    [DisplayMember(nameof(TurnoverTool.Code))]
    public partial class TurnoverTool : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<TurnoverTool>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<TurnoverTool>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 周转工具类型 ToolType
        /// <summary>
        /// 周转工具类型
        /// </summary>
        [Required]
        [Label("周转工具类型")]
        public static readonly Property<string> ToolTypeProperty = P<TurnoverTool>.Register(e => e.ToolType);

        /// <summary>
        /// 周转工具类型
        /// </summary>
        public string ToolType
        {
            get { return GetProperty(ToolTypeProperty); }
            set { SetProperty(ToolTypeProperty, value); }
        }
        #endregion

        #region 周转工具型号 Model
        /// <summary>
        /// 周转工具型号Id
        /// </summary>
        public static readonly IRefIdProperty ModelIdProperty = P<TurnoverTool>.RegisterRefId(e => e.ModelId, ReferenceType.Normal);

        /// <summary>
        /// 周转工具型号Id
        /// </summary>
        public double ModelId
        {
            get { return (double)GetRefId(ModelIdProperty); }
            set { SetRefId(ModelIdProperty, value); }
        }

        /// <summary>
        /// 周转工具型号
        /// </summary>
        public static readonly RefEntityProperty<TurnoverToolModel> ModelProperty = P<TurnoverTool>.RegisterRef(e => e.Model, ModelIdProperty);

        /// <summary>
        /// 周转工具型号
        /// </summary>
        public TurnoverToolModel Model
        {
            get { return GetRefEntity(ModelProperty); }
            set { SetRefEntity(ModelProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("周转工具状态")]
        public static readonly Property<TurnoverToolState> StateProperty = P<TurnoverTool>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public TurnoverToolState State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 注册视图

        #region 周转工具型号编码 ModelCode
        /// <summary>
        /// 周转工具型号编码
        /// </summary>
        [Label("周转工具型号")]
        public static readonly Property<string> ModelCodeProperty = P<TurnoverTool>.RegisterView(e => e.ModelCode, p => p.Model.Code);

        /// <summary>
        /// 周转工具型号编码
        /// </summary>
        public string ModelCode
        {
            get { return this.GetProperty(ModelCodeProperty); }
        }
        #endregion

        #region 周转工具型号名称 ModelName
        /// <summary>
        /// 周转工具型号名称
        /// </summary>
        [Label("周转工具型号名称")]
        public static readonly Property<string> ModelNameProperty = P<TurnoverTool>.RegisterView(e => e.ModelName, p => p.Model.Name);

        /// <summary>
        /// 周转工具型号名称
        /// </summary>
        public string ModelName
        {
            get { return this.GetProperty(ModelNameProperty); }
        }
        #endregion

        #region 周转工具型号-专用容器 ModelIsDedicated
        /// <summary>
        /// 专用容器
        /// </summary>
        [Label("专用容器")]
        public static readonly Property<bool> ModelIsDedicatedProperty = P<TurnoverTool>.RegisterView(e => e.ModelIsDedicated, p => p.Model.IsDedicated);

        /// <summary>
        /// 专用容器
        /// </summary>
        public bool ModelIsDedicated
        {
            get { return this.GetProperty(ModelIsDedicatedProperty); }
        }
        #endregion

        #region 周转工具型号-容量 ModelCapacity
        /// <summary>
        /// 容量
        /// </summary>
        [Label("容量")]
        public static readonly Property<int> ModelCapacityProperty = P<TurnoverTool>.RegisterView(e => e.ModelCapacity, p => p.Model.DefaultCapacity);

        /// <summary>
        /// 容量
        /// </summary>
        public int ModelCapacity
        {
            get { return this.GetProperty(ModelCapacityProperty); }
        }
        #endregion

        #region 周转工具型号-客户 ModelCustomerId
        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        public static readonly Property<double?> ModelCustomerIdProperty = P<TurnoverTool>.RegisterView(e => e.ModelCustomerId, p => p.Model.CustomerId);

        /// <summary>
        /// 客户
        /// </summary>
        public double? ModelCustomerId
        {
            get { return this.GetProperty(ModelCustomerIdProperty); }
        }
        #endregion

        #region 周转工具型号-客户名称 ModelCustomerName
        /// <summary>
        /// 客户名称
        /// </summary>
        [Label("客户名称")]
        public static readonly Property<string> ModelCustomerNameProperty = P<TurnoverTool>.RegisterView(e => e.ModelCustomerName, p => p.Model.Customer.Name);

        /// <summary>
        /// 客户名称
        /// </summary>
        public string ModelCustomerName
        {
            get { return this.GetProperty(ModelCustomerNameProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 周转工具 实体配置
    /// </summary>
    internal class TurnoverToolConfig : EntityConfig<TurnoverTool>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_TURN_TOOL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}