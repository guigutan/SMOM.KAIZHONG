using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.Fixtures.Projects;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Models
{
    /// <summary>
	/// 工治具型号保养项目
	/// </summary>
	[ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("工治具型号保养项目")]
    public partial class FixtureModelMaintainProject : DataEntity
    {
        #region 入库保养 InStorageMaintain
        /// <summary>
        /// 入库保养
        /// </summary>
        [Label("入库保养")]
        public static readonly Property<bool> InStorageMaintainProperty = P<FixtureModelMaintainProject>.Register(e => e.InStorageMaintain);

        /// <summary>
        /// 入库保养
        /// </summary>
        public bool InStorageMaintain
        {
            get { return GetProperty(InStorageMaintainProperty); }
            set { SetProperty(InStorageMaintainProperty, value); }
        }
        #endregion

        #region 常规保养 CommonMaintain
        /// <summary>
        /// 常规保养
        /// </summary>
        [Label("常规保养")]
        public static readonly Property<bool> CommonMaintainProperty = P<FixtureModelMaintainProject>.Register(e => e.CommonMaintain);

        /// <summary>
        /// 常规保养
        /// </summary>
        public bool CommonMaintain
        {
            get { return GetProperty(CommonMaintainProperty); }
            set { SetProperty(CommonMaintainProperty, value); }
        }
        #endregion

        #region 上线定期保养 OnlineMaintain
        /// <summary>
        /// 上线定期保养
        /// </summary>
        [Label("上线定期保养")]
        public static readonly Property<bool> OnlineMaintainProperty = P<FixtureModelMaintainProject>.Register(e => e.OnlineMaintain);

        /// <summary>
        /// 上线定期保养
        /// </summary>
        public bool OnlineMaintain
        {
            get { return GetProperty(OnlineMaintainProperty); }
            set { SetProperty(OnlineMaintainProperty, value); }
        }
        #endregion

        #region 出库保养 ToStorageMaintain
        /// <summary>
        /// 出库保养
        /// </summary>
        [Label("出库保养")]
        public static readonly Property<bool> ToStorageMaintainProperty = P<FixtureModelMaintainProject>.Register(e => e.ToStorageMaintain);

        /// <summary>
        /// 出库保养
        /// </summary>
        public bool ToStorageMaintain
        {
            get { return GetProperty(ToStorageMaintainProperty); }
            set { SetProperty(ToStorageMaintainProperty, value); }
        }
        #endregion

        #region 保养项目 MaintainProject
        /// <summary>
        /// 保养项目Id
        /// </summary>
        public static readonly IRefIdProperty MaintainProjectIdProperty = P<FixtureModelMaintainProject>.RegisterRefId(e => e.MaintainProjectId, ReferenceType.Normal);

        /// <summary>
        /// 保养项目Id
        /// </summary>
        public double MaintainProjectId
        {
            get { return (double)GetRefId(MaintainProjectIdProperty); }
            set { SetRefId(MaintainProjectIdProperty, value); }
        }

        /// <summary>
        /// 保养项目
        /// </summary>
        public static readonly RefEntityProperty<MaintainProject> MaintainProjectProperty = P<FixtureModelMaintainProject>.RegisterRef(e => e.MaintainProject, MaintainProjectIdProperty);

        /// <summary>
        /// 保养项目
        /// </summary>
        public MaintainProject MaintainProject
        {
            get { return GetRefEntity(MaintainProjectProperty); }
            set { SetRefEntity(MaintainProjectProperty, value); }
        }
        #endregion


        #region 验收项目 AcceptanceItems
        /// <summary>
        /// 验收项目
        /// </summary>
        [Label("验收项目")]
        public static readonly Property<bool> AcceptanceItemsProperty = P<FixtureModelMaintainProject>.Register(e => e.AcceptanceItems);

        /// <summary>
        /// 验收项目
        /// </summary>
        public bool AcceptanceItems
        {
            get { return this.GetProperty(AcceptanceItemsProperty); }
            set { this.SetProperty(AcceptanceItemsProperty, value); }
        }
        #endregion


        #region 保养项目列表 FixtureModel
        /// <summary>
        /// 保养项目列表Id
        /// </summary>
        public static readonly IRefIdProperty FixtureModelIdProperty = P<FixtureModelMaintainProject>.RegisterRefId(e => e.FixtureModelId, ReferenceType.Parent);

        /// <summary>
        /// 保养项目列表Id
        /// </summary>
        public double FixtureModelId
        {
            get { return (double)GetRefId(FixtureModelIdProperty); }
            set { SetRefId(FixtureModelIdProperty, value); }
        }

        /// <summary>
        /// 保养项目列表
        /// </summary>
        public static readonly RefEntityProperty<FixtureModel> FixtureModelProperty = P<FixtureModelMaintainProject>.RegisterRef(e => e.FixtureModel, FixtureModelIdProperty);

        /// <summary>
        /// 保养项目列表
        /// </summary>
        public FixtureModel FixtureModel
        {
            get { return GetRefEntity(FixtureModelProperty); }
            set { SetRefEntity(FixtureModelProperty, value); }
        }
        #endregion

        #region 检验标识 CheckTag
        /// <summary>
        /// 检验标识
        /// </summary>
        [Label("检验标识")]
        public static readonly Property<CheckTag> CheckTagProperty = P<FixtureModelMaintainProject>.Register(e => e.CheckTag);

        /// <summary>
        /// 检验标识
        /// </summary>
        public CheckTag CheckTag
        {
            get { return this.GetProperty(CheckTagProperty); }
            set { this.SetProperty(CheckTagProperty, value); }
        }
        #endregion



        #region 视图属性
        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<FixtureModelMaintainProject>.RegisterView(e => e.ProjectName, p => p.MaintainProject.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
            set { this.SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 耗材 Consumable
        /// <summary>
        /// 耗材
        /// </summary>
        [Label("耗材")]
        public static readonly Property<string> ConsumableProperty = P<FixtureModelMaintainProject>.RegisterView(e => e.Consumable, p => p.MaintainProject.Consumable);

        /// <summary>
        /// 耗材
        /// </summary>
        public string Consumable
        {
            get { return this.GetProperty(ConsumableProperty); }
            set { this.SetProperty(ConsumableProperty, value); }
        }
        #endregion

        #region 耗材用量 ConsumableQty
        /// <summary>
        /// 耗材用量
        /// </summary>
        [Label("耗材用量")]
        public static readonly Property<decimal> ConsumableQtyProperty = P<FixtureModelMaintainProject>.RegisterView(e => e.ConsumableQty, p => p.MaintainProject.ConsumableQty);

        /// <summary>
        /// 耗材用量
        /// </summary>
        public decimal ConsumableQty
        {
            get { return this.GetProperty(ConsumableQtyProperty); }
            set { this.SetProperty(ConsumableQtyProperty, value); }
        }
        #endregion

        #region 方法 Method
        /// <summary>
        /// 方法
        /// </summary>
        [Label("方法")]
        public static readonly Property<string> MethodProperty = P<FixtureModelMaintainProject>.RegisterView(e => e.Method, p => p.MaintainProject.Method);

        /// <summary>
        /// 方法
        /// </summary>
        public string Method
        {
            get { return this.GetProperty(MethodProperty); }
            set { this.SetProperty(MethodProperty, value); }
        }
        #endregion

        #region 工具 Tool
        /// <summary>
        /// 工具
        /// </summary>
        [Label("工具")]
        public static readonly Property<string> ToolProperty = P<FixtureModelMaintainProject>.RegisterView(e => e.Tool, p => p.MaintainProject.Tool);

        /// <summary>
        /// 工具
        /// </summary>
        public string Tool
        {
            get { return this.GetProperty(ToolProperty); }
            set { this.SetProperty(ToolProperty, value); }
        }
        #endregion

        #region 检测合格最小值 MinValue
        /// <summary>
        /// 检测合格最小值
        /// </summary>
        [Label("检测合格最小值")]
        public static readonly Property<decimal?> MinValueProperty = P<FixtureModelMaintainProject>.Register(e => e.MinValue);

        /// <summary>
        /// 检测合格最小值
        /// </summary>
        public decimal? MinValue
        {
            get { return this.GetProperty(MinValueProperty); }
            set { this.SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 检测合格最大值 MaxValue
        /// <summary>
        /// 检测合格最大值
        /// </summary>
        [Label("检测合格最大值")]
        public static readonly Property<decimal?> MaxValueProperty = P<FixtureModelMaintainProject>.Register(e => e.MaxValue);

        /// <summary>
        /// 检测合格最大值
        /// </summary>
        public decimal? MaxValue
        {
            get { return this.GetProperty(MaxValueProperty); }
            set { this.SetProperty(MaxValueProperty, value); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 工治具型号保养项目 实体配置
    /// </summary>
    internal class FixtureModelMaintainProjectConfig : EntityConfig<FixtureModelMaintainProject>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXMODEL_MAINTAIN_PRJ").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
