using SIE.Defects.InspectionItems;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Fixtures.Projects
{
    /// <summary>
    /// 工治具保养项目
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [DisplayMember(nameof(Name))]
    [Label("工治具保养项目")]
    public partial class MaintainProject : DataEntity
    {
        #region 项目名称 Name
        /// <summary>
        /// 项目名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("项目名称")]
        public static readonly Property<string> NameProperty = P<MaintainProject>.Register(e => e.Name);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 耗材 Consumable
        /// <summary>
        /// 耗材
        /// </summary>
        [Label("耗材")]
        public static readonly Property<string> ConsumableProperty = P<MaintainProject>.Register(e => e.Consumable);

        /// <summary>
        /// 耗材
        /// </summary>
        public string Consumable
        {
            get { return GetProperty(ConsumableProperty); }
            set { SetProperty(ConsumableProperty, value); }
        }
        #endregion

        #region 耗材用量 ConsumableQty
        /// <summary>
        /// 耗材用量
        /// </summary>
        [Label("耗材用量")]
        public static readonly Property<decimal> ConsumableQtyProperty = P<MaintainProject>.Register(e => e.ConsumableQty);

        /// <summary>
        /// 耗材用量
        /// </summary>
        public decimal ConsumableQty
        {
            get { return GetProperty(ConsumableQtyProperty); }
            set { SetProperty(ConsumableQtyProperty, value); }
        }
        #endregion

        #region 方法 Method
        /// <summary>
        /// 方法
        /// </summary>
        [Label("方法")]
        public static readonly Property<string> MethodProperty = P<MaintainProject>.Register(e => e.Method);

        /// <summary>
        /// 方法
        /// </summary>
        public string Method
        {
            get { return GetProperty(MethodProperty); }
            set { SetProperty(MethodProperty, value); }
        }
        #endregion

        #region 工具 Tool
        /// <summary>
        /// 工具
        /// </summary>
        [Label("工具")]
        public static readonly Property<string> ToolProperty = P<MaintainProject>.Register(e => e.Tool);

        /// <summary>
        /// 工具
        /// </summary>
        public string Tool
        {
            get { return GetProperty(ToolProperty); }
            set { SetProperty(ToolProperty, value); }
        }
        #endregion

        #region 检测合格最小值 MinValue
        /// <summary>
        /// 检测合格最小值
        /// </summary>
        [Label("检测合格最小值")]
        public static readonly Property<decimal?> MinValueProperty = P<MaintainProject>.Register(e => e.MinValue);

        /// <summary>
        /// 检测合格最小值
        /// </summary>
        public decimal? MinValue
        {
            get { return GetProperty(MinValueProperty); }
            set { SetProperty(MinValueProperty, value); }
        }
        #endregion

        #region 检测合格最大值 MaxValue
        /// <summary>
        /// 检测合格最大值
        /// </summary>
        [Label("检测合格最大值")]
        public static readonly Property<decimal?> MaxValueProperty = P<MaintainProject>.Register(e => e.MaxValue);

        /// <summary>
        /// 检测合格最大值
        /// </summary>
        public decimal? MaxValue
        {
            get { return GetProperty(MaxValueProperty); }
            set { SetProperty(MaxValueProperty, value); }
        }
        #endregion

        #region 检验标识 CheckTag
        /// <summary>
        /// 检验标识
        /// </summary>
        [Label("检验标识")]
        public static readonly Property<CheckTag> CheckTagProperty = P<MaintainProject>.Register(e => e.CheckTag);

        /// <summary>
        /// 检验标识
        /// </summary>
        public CheckTag CheckTag
        {
            get { return GetProperty(CheckTagProperty); }
            set { SetProperty(CheckTagProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 保养项目 实体配置
    /// </summary>
    internal class MaintainProjectConfig : EntityConfig<MaintainProject>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_FIXTURE_MAINTAIN_PRJ").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}