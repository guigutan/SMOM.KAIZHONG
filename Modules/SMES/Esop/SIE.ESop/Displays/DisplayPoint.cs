using SIE.Common.Configs;
using SIE.Domain;
using SIE.ESop.Configs;
using SIE.ESop.Displays.Configs;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.ESop.Displays
{
    /// <summary>
    /// 显示点
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(DisplayPointCriteria))]
    [EntityWithConfig(typeof(DisplayConfig))]
    [EntityWithConfig(typeof(DisplayPointDataConfig))]
    [Label("显示点")]
    [DisplayMember(nameof(DisplayPoint.Code))]
    public partial class DisplayPoint : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [MaxLength(80)]
        [NotDuplicate]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<DisplayPoint>.Register(e => e.Code);

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
        [MaxLength(80)]
        [NotDuplicate]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<DisplayPoint>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源ID
        /// </summary>
        [Label("资源")]
        public static readonly IRefIdProperty ResourceIdProperty = P<DisplayPoint>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源ID
        /// </summary>
        public double ResourceId
        {
            get { return (double)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<DisplayPoint>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 播放间隔 PlaySpace
        /// <summary>
        /// 播放间隔
        /// </summary>
        [MinValue(1)]
        [Label("播放间隔")]
        public static readonly Property<int?> PlaySpaceProperty = P<DisplayPoint>.Register(e => e.PlaySpace);

        /// <summary>
        /// 播放间隔
        /// </summary>
        public int? PlaySpace
        {
            get { return this.GetProperty(PlaySpaceProperty); }
            set { this.SetProperty(PlaySpaceProperty, value); }
        }
        #endregion

        #region 扩展屏 PlayScreenNum
        /// <summary>
        /// 扩展屏
        /// </summary>
        [MinValue(1)]
        [Label("扩展屏")]
        public static readonly Property<int?> PlayScreenNumProperty = P<DisplayPoint>.Register(e => e.PlayScreenNum);

        /// <summary>
        /// 扩展屏
        /// </summary>
        public int? PlayScreenNum
        {
            get { return this.GetProperty(PlayScreenNumProperty); }
            set { this.SetProperty(PlayScreenNumProperty, value); }
        }
        #endregion


        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<DisplayPoint>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 显示点对应工序列表 DisplayPointProcessList
        /// <summary>
        /// 显示点对应工序列表
        /// </summary>
        public static readonly ListProperty<EntityList<DisplayPointProcess>> DisplayPointProcessListProperty = P<DisplayPoint>.RegisterList(e => e.DisplayPointProcessList);

        /// <summary>
        /// 显示点对应工序列表
        /// </summary>
        public EntityList<DisplayPointProcess> DisplayPointProcessList
        {
            get { return this.GetLazyList(DisplayPointProcessListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 显示点 实体配置
    /// </summary>
    internal class DisplayPointConfig : EntityConfig<DisplayPoint>
    {
        /// <summary>
        /// 配置数据库的映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ESOP_DISPLAY_POINT").MapAllProperties();
            Meta.Property(DisplayPoint.ResourceIdProperty).ColumnMeta.HasIndex();
            Meta.Property(DisplayPoint.RemarkProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}