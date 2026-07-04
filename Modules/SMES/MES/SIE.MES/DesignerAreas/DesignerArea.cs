using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;


namespace SIE.MES.DesignerAreas
{  

    /// <summary>
    /// 看板区域维护
    /// </summary>
    [RootEntity, Serializable]
    [Label("看板区域维护")]
    [ConditionQueryType(typeof(DesignerAreaCriteria))]
    
    public partial class DesignerArea : DataEntity
    {


        #region 看板区域编码 AreaCode
        /// <summary>
        /// 看板区域编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("看板区域编码")]
        public static readonly Property<string> AreaCodeProperty = P<DesignerArea>.Register(e => e.AreaCode);

        /// <summary>
        /// 看板区域编码
        /// </summary>
        public string AreaCode
        {
            get { return this.GetProperty(AreaCodeProperty); }
            set { this.SetProperty(AreaCodeProperty, value); }
        }
        #endregion


        #region 看板区域名称 AreaName
        /// <summary>
        /// 看板区域名称
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("看板区域名称")]
        public static readonly Property<string> AreaNameProperty = P<DesignerArea>.Register(e => e.AreaName);

        /// <summary>
        /// 看板区域名称
        /// </summary>
        public string AreaName
        {
            get { return this.GetProperty(AreaNameProperty); }
            set { this.SetProperty(AreaNameProperty, value); }
        }
        #endregion                   


        #region 看板区域与产线 ResourceList
        /// <summary>
        /// 看板区域与产线 列表
        /// </summary>
        [Label("产线")]
        public static readonly ListProperty<EntityList<DesignerAreaResources>> ResourceListProperty = P<DesignerArea>.RegisterList(e => e.ResourceList);

        /// <summary>
        /// 看板区域与产线 列表
        /// </summary>
        public EntityList<DesignerAreaResources> ResourceList
        {
            get { return this.GetLazyList(ResourceListProperty); }
        }
        #endregion


        #region 看板区域与MRP控制者 MrpList
        /// <summary>
        /// 看板区域与MRP控制者 列表
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("MRP控制者")]
        public static readonly ListProperty<EntityList<DesignerAreaMrp>> MrpListListProperty = P<DesignerArea>.RegisterList(e => e.MrpList);

        /// <summary>
        /// 看板区域与MRP控制者 列表
        /// </summary>
        public EntityList<DesignerAreaMrp> MrpList
        {
            get { return this.GetLazyList(MrpListListProperty); }
        }
        #endregion


    }


    internal class ProdAreasEntityConfig : EntityConfig<DesignerArea>
    {       
        protected override void ConfigMeta()
        {
            Meta.MapTable("DESIGNER_AREA").MapAllProperties();
            Meta.EnablePhantoms();
            Meta.EnableInvOrg();
        }
    }
}
