using SIE.Equipments;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using SIE.Equipments.EquipModels;
using SIE.Defects;

namespace SIE.Equipments.EquipFaults
{
    /// <summary>
    /// 设备故障与系统缺陷对应关系查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("设备故障与系统缺陷对应关系查询实体")]
    public class EquipFaultAndDefectCriteria : Criteria
    {
        #region 设备不良编码 EquipBadCode
        /// <summary>
        /// 设备不良编码
        /// </summary>
        [Label("设备不良编码")]
        public static readonly Property<string> EquipBadCodeProperty = P<EquipFaultAndDefectCriteria>.Register(e => e.EquipBadCode);

        /// <summary>
        /// 设备不良编码
        /// </summary>
        public string EquipBadCode
        {
            get { return this.GetProperty(EquipBadCodeProperty); }
            set { this.SetProperty(EquipBadCodeProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<EquipFaultAndDefectCriteria>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double? EquipModelId
        {
            get { return (double?)this.GetRefNullableId(EquipModelIdProperty); }
            set { this.SetRefNullableId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<EquipFaultAndDefectCriteria>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 型号名称 EquipModelName
        /// <summary>
        /// 型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipFaultAndDefectCriteria>.Register(e => e.EquipModelName);

        /// <summary>
        /// 型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
            set { this.SetProperty(EquipModelNameProperty, value); }
        }
        #endregion

        #region 缺陷分类 DefectCategory
        /// <summary>
        /// 缺陷分类Id
        /// </summary>
        [Label("缺陷分类")]
        public static readonly IRefIdProperty DefectCategoryIdProperty =
            P<EquipFaultAndDefectCriteria>.RegisterRefId(e => e.DefectCategoryId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷分类Id
        /// </summary>
        public double? DefectCategoryId
        {
            get { return (double?)this.GetRefNullableId(DefectCategoryIdProperty); }
            set { this.SetRefNullableId(DefectCategoryIdProperty, value); }
        }

        /// <summary>
        /// 缺陷分类
        /// </summary>
        public static readonly RefEntityProperty<DefectCategory> DefectCategoryProperty =
            P<EquipFaultAndDefectCriteria>.RegisterRef(e => e.DefectCategory, DefectCategoryIdProperty);

        /// <summary>
        /// 缺陷分类
        /// </summary>
        public DefectCategory DefectCategory
        {
            get { return this.GetRefEntity(DefectCategoryProperty); }
            set { this.SetRefEntity(DefectCategoryProperty, value); }
        }
        #endregion

        #region 缺陷 Defect
        /// <summary>
        /// 缺陷Id
        /// </summary>
        [Label("缺陷")]
        public static readonly IRefIdProperty DefectIdProperty = P<EquipFaultAndDefectCriteria>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷Id
        /// </summary>
        public double? DefectId
        {
            get { return (double?)this.GetRefNullableId(DefectIdProperty); }
            set { this.SetRefNullableId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty = P<EquipFaultAndDefectCriteria>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 创建时间 CreateDate
        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<EquipFaultAndDefectCriteria>.Register(e => e.CreateDate);

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return this.GetProperty(CreateDateProperty); }
            set { this.SetProperty(CreateDateProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询设备故障与系统缺陷对应关系
        /// </summary>
        /// <returns>设备故障与系统缺陷对应关系列表</returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipFaultAndDefectController>().GetEquipFaultAndDefects(this);
        }
    }
}