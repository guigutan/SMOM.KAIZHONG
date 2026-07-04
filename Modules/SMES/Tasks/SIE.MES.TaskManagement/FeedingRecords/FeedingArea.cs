using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 供料区维护
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [DisplayMember(nameof(Name))]
    [Label("供料区维护")]
    public class FeedingArea : DataEntity,IStateEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FeedingArea()
        {
        }

        #region 供料区编码 Code
        /// <summary>
        /// 供料区编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("供料区编码")]
        public static readonly Property<string> CodeProperty = P<FeedingArea>.Register(e => e.Code);

        /// <summary>
        /// 供料区编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 供料区名称 Name
        /// <summary>
        /// 供料区名称
        /// </summary>
        [Required]
        [Label("供料区名称")]
        public static readonly Property<string> NameProperty = P<FeedingArea>.Register(e => e.Name);

        /// <summary>
        /// 供料区名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 供料区描述 Desc
        /// <summary>
        /// 供料区描述
        /// </summary>
        [Label("供料区描述")]
        public static readonly Property<string> DescProperty = P<FeedingArea>.Register(e => e.Desc);

        /// <summary>
        /// 供料区描述
        /// </summary>
        public string Desc
        {
            get { return this.GetProperty(DescProperty); }
            set { this.SetProperty(DescProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<FeedingArea>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 供料区与产线 ResourceList
        /// <summary>
        /// 供料区与产线 列表
        /// </summary>
        [Label("产线列表")]
        public static readonly ListProperty<EntityList<FeedingAreaReource>> ResourceListProperty = P<FeedingArea>.RegisterList(e => e.ResourceList);

        /// <summary>
        /// 供料区与产线 列表
        /// </summary>
        public EntityList<FeedingAreaReource> ResourceList
        {
            get { return this.GetLazyList(ResourceListProperty); }
        }
        #endregion

        #region 供料区与物料 ItemList
        /// <summary>
        /// 供料区与物料
        /// </summary>
        [Label("物料列表")]
        public static readonly ListProperty<EntityList<FeedingAreaItem>> ItemListProperty = P<FeedingArea>.RegisterList(e => e.ItemList);

        /// <summary>
        /// 供料区与物料
        /// </summary>
        public EntityList<FeedingAreaItem> ItemList
        {
            get { return this.GetLazyList(ItemListProperty); }
        }
        #endregion


        #region 视图属性


        #endregion
    }

    internal class FeedingAreaEntityConfig : EntityConfig<FeedingArea>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("FEEDING_AREA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
