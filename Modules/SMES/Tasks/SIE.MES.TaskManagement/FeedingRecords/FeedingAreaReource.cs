using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.TaskManagement.FeedingRecords
{
    /// <summary>
    /// 供料区产线
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Id))]
    [Label("供料区产线")]
    public class FeedingAreaReource : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FeedingAreaReource()
        {
        }

        #region 供料区 FeedingArea
        /// <summary>
        /// 供料区Id
        /// </summary>
        [Label("供料区")]
        public static readonly IRefIdProperty FeedingAreaIdProperty =
            P<FeedingAreaReource>.RegisterRefId(e => e.FeedingAreaId, ReferenceType.Parent);

        /// <summary>
        /// 供料区Id
        /// </summary>
        public double FeedingAreaId
        {
            get { return (double)this.GetRefId(FeedingAreaIdProperty); }
            set { this.SetRefId(FeedingAreaIdProperty, value); }
        }

        /// <summary>
        /// 供料区
        /// </summary>
        public static readonly RefEntityProperty<FeedingArea> FeedingAreaProperty =
            P<FeedingAreaReource>.RegisterRef(e => e.FeedingArea, FeedingAreaIdProperty);

        /// <summary>
        /// 供料区
        /// </summary>
        public FeedingArea FeedingArea
        {
            get { return this.GetRefEntity(FeedingAreaProperty); }
            set { this.SetRefEntity(FeedingAreaProperty, value); }
        }
        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<FeedingAreaReource>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 产线Id
        /// </summary>
        public double ResourceId
        {
            get { return (double)this.GetRefId(ResourceIdProperty); }
            set { this.SetRefId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 产线
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty =
            P<FeedingAreaReource>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 产线
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion


        #region 视图属性

        #region 产线编码 ResourceCode
        /// <summary>
        /// 产线编码
        /// </summary>
        [Label("产线编码")]
        public static readonly Property<string> ResourceCodeProperty = P<FeedingAreaReource>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

        /// <summary>
        /// 产线编码
        /// </summary>
        public string ResourceCode
        {
            get { return this.GetProperty(ResourceCodeProperty); }
        }

        #endregion

        #region 产线名称 ResourceName
        /// <summary>
        /// 产线名称
        /// </summary>
        [Label("产线名称")]
        public static readonly Property<string> ResourceNameProperty = P<FeedingAreaReource>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

        /// <summary>
        /// 产线名称
        /// </summary>
        public string ResourceName
        {
            get { return this.GetProperty(ResourceNameProperty); }
        }

        #endregion


        #endregion
    }

    internal class FeedingAreaLineEntityConfig : EntityConfig<FeedingAreaReource>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("FEEDING_AREA_RES").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
