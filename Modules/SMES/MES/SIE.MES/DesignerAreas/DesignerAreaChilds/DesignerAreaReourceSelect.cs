using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;

namespace SIE.MES.DesignerAreas
{
    /// <summary>
    /// 看板区域产线
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Id))]
    [Label("看板区域产线")]
    public class DesignerAreaReourceSelect : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DesignerAreaReourceSelect()
        {
        }

        #region 看板区域 DesignerArea


        /// <summary>
        /// 看板区域Id
        /// </summary>
        [Label("看板区域")]
        public static readonly IRefIdProperty DesignerAreaIdProperty =
            P<DesignerAreaReourceSelect>.RegisterRefId(e => e.DesignerAreaId, ReferenceType.Parent);

        /// <summary>
        /// 看板区域Id
        /// </summary>
        public double DesignerAreaId
        {
            get { return (double)this.GetRefId(DesignerAreaIdProperty); }
            set { this.SetRefId(DesignerAreaIdProperty, value); }
        }



        /// <summary>
        /// 看板区域
        /// </summary>
        public static readonly RefEntityProperty<DesignerArea> DesignerAreaProperty =
            P<DesignerAreaResources>.RegisterRef(e => e.DesignerArea, DesignerAreaIdProperty);

        /// <summary>
        /// 看板区域
        /// </summary>
        /// 
        public DesignerArea DesignerArea
        {
            get { return this.GetRefEntity(DesignerAreaProperty); }
            set { this.SetRefEntity(DesignerAreaProperty, value); }
        }

        #endregion

        #region 产线 Resource
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("产线")]
        public static readonly IRefIdProperty ResourceIdProperty =
            P<DesignerAreaResources>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

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
            P<DesignerAreaResources>.RegisterRef(e => e.Resource, ResourceIdProperty);

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
        public static readonly Property<string> ResourceCodeProperty = P<DesignerAreaResources>.RegisterView(e => e.ResourceCode, p => p.Resource.Code);

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
        public static readonly Property<string> ResourceNameProperty = P<DesignerAreaResources>.RegisterView(e => e.ResourceName, p => p.Resource.Name);

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

    internal class DesignerAreaLineEntityConfig : EntityConfig<DesignerAreaResources>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("DESIGNER_AREA_RES").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
