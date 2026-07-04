using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.StationChecks
{
    /// <summary>
    /// 工位点检设备项
    /// </summary>
    [ChildEntity, Serializable]
    public class CheckEquipmentDetail : DataEntity
    {
        #region 工位点检设备 CheckEquipment
        /// <summary>
        /// 工位点检设备Id
        /// </summary>
        [Label("工位点检设备")]
        public static readonly IRefIdProperty CheckEquipmentIdProperty =
            P<CheckEquipmentDetail>.RegisterRefId(e => e.CheckEquipmentId, ReferenceType.Parent);

        /// <summary>
        /// 工位点检设备Id
        /// </summary>
        public double CheckEquipmentId
        {
            get { return (double)this.GetRefId(CheckEquipmentIdProperty); }
            set { this.SetRefId(CheckEquipmentIdProperty, value); }
        }

        /// <summary>
        /// 工位点检设备
        /// </summary>
        public static readonly RefEntityProperty<CheckEquipment> CheckEquipmentProperty =
            P<CheckEquipmentDetail>.RegisterRef(e => e.CheckEquipment, CheckEquipmentIdProperty);

        /// <summary>
        /// 工位点检设备
        /// </summary>
        public CheckEquipment CheckEquipment
        {
            get { return this.GetRefEntity(CheckEquipmentProperty); }
            set { this.SetRefEntity(CheckEquipmentProperty, value); }
        }
        #endregion

        #region 点检项目 Project
        /// <summary>
        /// 点检项目Id
        /// </summary>
        [Label("点检项目")]
        public static readonly IRefIdProperty ProjectIdProperty =
            P<CheckEquipmentDetail>.RegisterRefId(e => e.ProjectId, ReferenceType.Normal);

        /// <summary>
        /// 点检项目Id
        /// </summary>
        public double ProjectId
        {
            get { return (double)this.GetRefId(ProjectIdProperty); }
            set { this.SetRefId(ProjectIdProperty, value); }
        }

        /// <summary>
        /// 点检项目
        /// </summary>
        public static readonly RefEntityProperty<CheckProject> ProjectProperty =
            P<CheckEquipmentDetail>.RegisterRef(e => e.Project, ProjectIdProperty);

        /// <summary>
        /// 点检项目
        /// </summary>
        public CheckProject Project
        {
            get { return this.GetRefEntity(ProjectProperty); }
            set { this.SetRefEntity(ProjectProperty, value); }
        }
        #endregion 
    }

    /// <summary>
    /// 元数据配置
    /// </summary>
    internal class CheckEquipmentDetailEntityConfig : EntityConfig<CheckEquipmentDetail>
    {
        /// <summary>
        /// 配置实体元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CHK_STATION_EQT_Detail").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}