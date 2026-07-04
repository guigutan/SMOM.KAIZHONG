using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using System;


namespace SIE.MES.DesignerAreas
{

    /// <summary>
    /// 看板区域MRP
    /// </summary>
    [RootEntity, Serializable]
    [DisplayMember(nameof(Id))]
    [Label("看板区域MRP")]
    public class DesignerAreaMrp : DataEntity
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DesignerAreaMrp()
        {
        }

        #region 看板区域 DesignerArea

        /// <summary>
        /// 看板区域Id
        /// </summary>
        [Label("看板区域")]
        public static readonly IRefIdProperty DesignerAreaIdProperty =
            P<DesignerAreaMrp>.RegisterRefId(e => e.DesignerAreaId, ReferenceType.Parent);

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
            P<DesignerAreaMrp>.RegisterRef(e => e.DesignerArea, DesignerAreaIdProperty);

        /// <summary>
        /// 看板区域
        /// </summary>
        public DesignerArea DesignerArea
        {
            get { return this.GetRefEntity(DesignerAreaProperty); }
            set { this.SetRefEntity(DesignerAreaProperty, value); }
        }


        #endregion


        #region MRP控制者 MrpController

        /// <summary>
        /// MRP控制者
        /// </summary>
        [Required]
        [NotDuplicate]
        [Label("MRP控制者")]
        public static readonly Property<string> MrpControllerProperty = P<DesignerAreaMrp>.Register(e => e.MrpController);

        /// <summary>
        /// MRP控制者
        /// </summary>
        public string MrpController
        {
            get { return this.GetProperty(MrpControllerProperty); }
            set { this.SetProperty(MrpControllerProperty, value); }
        }

        #endregion


    }

    internal class DesignerAreaMrpEntityConfig : EntityConfig<DesignerAreaMrp>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("DESIGNER_AREA_MRP").MapAllProperties();
            Meta.EnablePhantoms();
            //Meta.EnableInvOrg();
        }
    }





}
