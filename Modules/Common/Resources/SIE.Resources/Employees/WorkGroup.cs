using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Enterprises;
using System;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 班组
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("班组")]
    [DisplayMember(nameof(Name))]
    public partial class WorkGroup : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [NotDuplicate]
        [MaxLength(40)]
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<WorkGroup>.Register(e => e.Code);

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
        [NotDuplicate]
        [MaxLength(40)]
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<WorkGroup>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 定编人数 DemandQty
        /// <summary>
        /// 定编人数
        /// </summary>
        [Label("定编人数")]
        [MinValue(0)]
        public static readonly Property<int> DemandQtyProperty = P<WorkGroup>.Register(e => e.DemandQty);

        /// <summary>
        /// 定编人数
        /// </summary>
        public int DemandQty
        {
            get { return GetProperty(DemandQtyProperty); }
            set { SetProperty(DemandQtyProperty, value); }
        }
        #endregion

        #region 在编人数 ActualQty
        /// <summary>
        /// 在编人数
        /// </summary>
        [Label("在编人数")]
        public static readonly Property<int?> ActualQtyProperty = P<WorkGroup>.Register(e => e.ActualQty);

        /// <summary>
        /// 在编人数
        /// </summary>
        public int? ActualQty
        {
            get { return GetProperty(ActualQtyProperty); }
            set { SetProperty(ActualQtyProperty, value); }
        }
        #endregion

        #region 部门 Department
        /// <summary>
        /// 部门Id
        /// </summary>
        [Label("部门")]
        public static readonly IRefIdProperty DepartmentIdProperty = P<WorkGroup>.RegisterRefId(e => e.DepartmentId, ReferenceType.Normal);

        /// <summary>
        /// 部门Id
        /// </summary>
        public double? DepartmentId
        {
            get { return (double?)GetRefNullableId(DepartmentIdProperty); }
            set { SetRefNullableId(DepartmentIdProperty, value); }
        }

        /// <summary>
        /// 部门
        /// </summary>
        public static readonly RefEntityProperty<Enterprise> DepartmentProperty = P<WorkGroup>.RegisterRef(e => e.Department, DepartmentIdProperty);

        /// <summary>
        /// 部门
        /// </summary>
        public Enterprise Department
        {
            get { return GetRefEntity(DepartmentProperty); }
            set { SetRefEntity(DepartmentProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 班组 实体配置
    /// </summary>
    internal class WorkGroupConfig : EntityConfig<WorkGroup>
    {
        /// <summary>
        /// 班组实体数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("RES_WG").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}