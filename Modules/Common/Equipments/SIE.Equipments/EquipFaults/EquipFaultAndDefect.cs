using SIE.Defects;
using SIE.Domain;
using SIE.Equipments.EquipModels;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Equipments.EquipFaults
{
    /// <summary>
    /// 设备故障与系统缺陷对应关系
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EquipFaultAndDefectCriteria))]
    [Label("设备故障与系统缺陷对应关系")]
    [DisplayMember(nameof(EquipBadCode))]
    public class EquipFaultAndDefect : DataEntity
    {
        #region 设备不良编码 EquipBadCode
        /// <summary>
        /// 设备不良编码
        /// </summary>
        [Label("设备不良编码")]
        [Required]
        public static readonly Property<string> EquipBadCodeProperty = P<EquipFaultAndDefect>.Register(e => e.EquipBadCode);

        /// <summary>
        /// 设备不良编码
        /// </summary>
        public string EquipBadCode
        {
            get { return this.GetProperty(EquipBadCodeProperty); }
            set { this.SetProperty(EquipBadCodeProperty, value); }
        }
        #endregion

        #region 设备缺陷名称 EquipDefectName
        /// <summary>
        /// 设备缺陷名称
        /// </summary>
        [Label("设备缺陷名称")]
        [Required]
        public static readonly Property<string> EquipDefectNameProperty = P<EquipFaultAndDefect>.Register(e => e.EquipDefectName);

        /// <summary>
        /// 设备缺陷名称
        /// </summary>
        public string EquipDefectName
        {
            get { return this.GetProperty(EquipDefectNameProperty); }
            set { this.SetProperty(EquipDefectNameProperty, value); }
        }
        #endregion

        #region 设备型号 EquipModel
        /// <summary>
        /// 设备型号Id
        /// </summary>
        [Label("设备型号")]
        public static readonly IRefIdProperty EquipModelIdProperty =
            P<EquipFaultAndDefect>.RegisterRefId(e => e.EquipModelId, ReferenceType.Normal);

        /// <summary>
        /// 设备型号Id
        /// </summary>
        public double EquipModelId
        {
            get { return (double)this.GetRefId(EquipModelIdProperty); }
            set { this.SetRefId(EquipModelIdProperty, value); }
        }

        /// <summary>
        /// 设备型号
        /// </summary>
        public static readonly RefEntityProperty<EquipModel> EquipModelProperty =
            P<EquipFaultAndDefect>.RegisterRef(e => e.EquipModel, EquipModelIdProperty);

        /// <summary>
        /// 设备型号
        /// </summary>
        public EquipModel EquipModel
        {
            get { return this.GetRefEntity(EquipModelProperty); }
            set { this.SetRefEntity(EquipModelProperty, value); }
        }
        #endregion

        #region 缺陷代码 Defect
        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        [Label("缺陷代码")]
        public static readonly IRefIdProperty DefectIdProperty = P<EquipFaultAndDefect>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        public double DefectId
        {
            get { return (double)this.GetRefId(DefectIdProperty); }
            set { this.SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty = P<EquipFaultAndDefect>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public Defect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 设备型号编码 EquipModelCode
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> EquipModelCodeProperty = P<EquipFaultAndDefect>.RegisterView(e => e.EquipModelCode, p => p.EquipModel.Code);

        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string EquipModelCode
        {
            get { return this.GetProperty(EquipModelCodeProperty); }
        }
        #endregion

        #region 设备型号名称 EquipModelName
        /// <summary>
        /// 设备型号名称
        /// </summary>
        [Label("型号名称")]
        public static readonly Property<string> EquipModelNameProperty = P<EquipFaultAndDefect>.RegisterView(e => e.EquipModelName, p => p.EquipModel.Name);

        /// <summary>
        /// 设备型号名称
        /// </summary>
        public string EquipModelName
        {
            get { return this.GetProperty(EquipModelNameProperty); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<EquipFaultAndDefect>.RegisterView(e => e.DefectDesc, p => p.Defect.Description);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get { return this.GetProperty(DefectDescProperty); }
        }
        #endregion

        #region 缺陷分类代码 DefectCategoryCode
        /// <summary>
        /// 缺陷分类代码
        /// </summary>
        [Label("缺陷分类代码")]
        public static readonly Property<string> DefectCategoryCodeProperty = P<EquipFaultAndDefect>.RegisterView(e => e.DefectCategoryCode, p => p.Defect.DefectCategory.Code);

        /// <summary>
        /// 缺陷分类代码
        /// </summary>
        public string DefectCategoryCode
        {
            get { return this.GetProperty(DefectCategoryCodeProperty); }
        }
        #endregion

        #region 缺陷分类描述 DefectCategoryDesc
        /// <summary>
        /// 缺陷分类描述
        /// </summary>
        [Label("缺陷分类描述")]
        public static readonly Property<string> DefectCategoryDescProperty = P<EquipFaultAndDefect>.RegisterView(e => e.DefectCategoryDesc, p => p.Defect.DefectCategory.Description);

        /// <summary>
        /// 缺陷分类描述
        /// </summary>
        public string DefectCategoryDesc
        {
            get { return this.GetProperty(DefectCategoryDescProperty); }
        }
        #endregion

        #region 缺陷代码 DefectCode
        /// <summary>
        /// 缺陷代码
        /// </summary>
        [Label("缺陷代码")]
        public static readonly Property<string> DefectCodeProperty = P<EquipFaultAndDefect>.RegisterView(e => e.DefectCode, p => p.Defect.Code);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public string DefectCode
        {
            get { return this.GetProperty(DefectCodeProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 设备故障与系统缺陷对应关系 实体配置
    /// </summary>
    internal class EquipFaultAndDefectConfig : EntityConfig<EquipFaultAndDefect>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("ELEC_EQUIP_FAULT_DEFECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}