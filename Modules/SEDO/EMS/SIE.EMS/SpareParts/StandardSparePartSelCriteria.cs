using SIE.Domain;
using SIE.EMS.MainenanceProjects;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.SpareParts
{

    /// <summary>
    /// 备件选择查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("备件选择查询")]
    public partial class StandardSparePartSelCriteria : Criteria
    {

        #region 设备型号Id EquipModelId
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly Property<double> EquipModelIdProperty = P<StandardSparePartSelCriteria>.Register(e => e.EquipModelId);

        /// <summary>
        /// 设备型号
        /// </summary>
        public double EquipModelId
        {
            get { return this.GetProperty(EquipModelIdProperty); }
            set { this.SetProperty(EquipModelIdProperty, value); }
        }
        #endregion

        #region 备件编码 SparePartCode
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> SparePartCodeProperty = P<StandardSparePartSelCriteria>.Register(e => e.SparePartCode);

        /// <summary>
        /// 备件编码
        /// </summary>
        public string SparePartCode
        {
            get { return this.GetProperty(SparePartCodeProperty); }
            set { this.SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 备件名称 SparePartName
        /// <summary>
        /// 备件名称
        /// </summary>
        [Label("备件名称")]
        public static readonly Property<string> SparePartNameProperty = P<StandardSparePartSelCriteria>.Register(e => e.SparePartName);

        /// <summary>
        /// 备件名称
        /// </summary>
        public string SparePartName
        {
            get { return this.GetProperty(SparePartNameProperty); }
            set { this.SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 点检保养项目 ProjectDetail
        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        [Label("项目名称")]
        public static readonly IRefIdProperty ProjectDetailIdProperty = P<StandardSparePartSelCriteria>.RegisterRefId(e => e.ProjectDetailId, ReferenceType.Normal);

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double? ProjectDetailId
        {
            get { return (double?)this.GetRefNullableId(ProjectDetailIdProperty); }
            set { this.SetRefNullableId(ProjectDetailIdProperty, value); }
        }

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public static readonly RefEntityProperty<ProjectDetail> ProjectDetailProperty = P<StandardSparePartSelCriteria>.RegisterRef(e => e.ProjectDetail, ProjectDetailIdProperty);

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public ProjectDetail ProjectDetail
        {
            get { return GetRefEntity(ProjectDetailProperty); }
            set { SetRefEntity(ProjectDetailProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
           return RT.Service.Resolve<SparePartController>().GetStandardSparePartList(this);
        }
    }
 
}
