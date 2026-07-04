using SIE.Domain;
using SIE.EMS.SpareParts;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.MainenanceProjects
{
    /// <summary>
    /// 备件清单
    /// </summary>
    [ChildEntity, Serializable]
    [Label("备件清单")]
    public partial class SparePartItem : DataEntity
    {
        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<decimal> QtyProperty = P<SparePartItem>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public decimal Qty
        {
            get { return GetProperty(QtyProperty); }
            set { SetProperty(QtyProperty, value); }
        }
        #endregion

        #region 备件基础数据 SparePart
        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public static readonly IRefIdProperty SparePartIdProperty = P<SparePartItem>.RegisterRefId(e => e.SparePartId, ReferenceType.Normal);

        /// <summary>
        /// 备件基础数据Id
        /// </summary>
        public double SparePartId
        {
            get { return (double)GetRefId(SparePartIdProperty); }
            set { SetRefId(SparePartIdProperty, value); }
        }

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public static readonly RefEntityProperty<SparePart> SparePartProperty = P<SparePartItem>.RegisterRef(e => e.SparePart, SparePartIdProperty);

        /// <summary>
        /// 备件基础数据
        /// </summary>
        public SparePart SparePart
        {
            get { return GetRefEntity(SparePartProperty); }
            set { SetRefEntity(SparePartProperty, value); }
        }
        #endregion

        #region 点检保养项目 ProjectDetail
        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public static readonly IRefIdProperty ProjectDetailIdProperty = P<SparePartItem>.RegisterRefId(e => e.ProjectDetailId, ReferenceType.Parent);

        /// <summary>
        /// 点检保养项目Id
        /// </summary>
        public double ProjectDetailId
        {
            get { return (double)GetRefId(ProjectDetailIdProperty); }
            set { SetRefId(ProjectDetailIdProperty, value); }
        }

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public static readonly RefEntityProperty<ProjectDetail> ProjectDetailProperty = P<SparePartItem>.RegisterRef(e => e.ProjectDetail, ProjectDetailIdProperty);

        /// <summary>
        /// 点检保养项目
        /// </summary>
        public ProjectDetail ProjectDetail
        {
            get { return GetRefEntity(ProjectDetailProperty); }
            set { SetRefEntity(ProjectDetailProperty, value); }
        }
        #endregion

        #region 视图引用属性

        #region 编码 SparePartCode
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> SparePartCodeProperty = P<SparePartItem>.RegisterView(e => e.SparePartCode, e => e.SparePart.SparePartCode);

        /// <summary>
        /// 编码
        /// </summary>
        public string SparePartCode
        {
            get { return GetProperty(SparePartCodeProperty); }
            set { SetProperty(SparePartCodeProperty, value); }
        }
        #endregion

        #region 名称 SparePartName
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> SparePartNameProperty = P<SparePartItem>.RegisterView(e => e.SparePartName, e => e.SparePart.SparePartName);

        /// <summary>
        /// 名称
        /// </summary>
        public string SparePartName
        {
            get { return GetProperty(SparePartNameProperty); }
            set { SetProperty(SparePartNameProperty, value); }
        }
        #endregion

        #region 规格型号 Specification
        /// <summary>
        /// 规格型号
        /// </summary>
        [Label("规格型号")]
        public static readonly Property<string> SpecificationProperty = P<SparePartItem>.RegisterView(e => e.Specification, e => e.SparePart.Specification);

        /// <summary>
        /// 规格型号
        /// </summary>
        public string Specification
        {
            get { return GetProperty(SpecificationProperty); }
            set { SetProperty(SpecificationProperty, value); }
        }
        #endregion

        #region 单位 UnitName
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        public static readonly Property<string> UnitNameProperty = P<SparePartItem>.RegisterView(e => e.UnitName, p => p.SparePart.Unit.Name);

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName
        {
            get { return this.GetProperty(UnitNameProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 备件清单 实体配置
    /// </summary>
    internal class SparePartItemConfig : EntityConfig<SparePartItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_PROJ_SP_ITEM").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}