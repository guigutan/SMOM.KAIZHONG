using SIE.Defects;
using SIE.Domain;
using SIE.MES.InspectionStandards;
using SIE.ObjectModel;
using System;

namespace SIE.Wpf.MES.WIP.Inspects
{
    /// <summary>
    /// 缺陷录入实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("不良信息")]
    public class DefectItemViewModel : ViewModel
    {
        #region 数量 Qty
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<double> QtyProperty = P<DefectItemViewModel>.Register(e => e.Qty);

        /// <summary>
        /// 数量
        /// </summary>
        public double Qty
        {
            get { return this.GetProperty(QtyProperty); }
            set { this.SetProperty(QtyProperty, value); }
        }
        #endregion


        #region 缺陷代码 Defect
        /// <summary>
        /// 缺陷代码ID
        /// </summary>
        [Label("缺陷代码")]
        public static readonly IRefIdProperty DefectIdProperty =
            P<DefectItemViewModel>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷代码ID
        /// </summary>
        public double DefectId
        {
            get { return (double)this.GetRefId(DefectIdProperty); }
            set { this.SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷项目
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty =
            P<DefectItemViewModel>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷项目
        /// </summary>
        public Defect Defect
        {
            get { return this.GetRefEntity(DefectProperty); }
            set { this.SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 缺陷描述 DefectDescription
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescriptionProperty = P<DefectItemViewModel>.RegisterView(e => e.DefectDescription, p => p.Defect.Description);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDescription
        {
            get { return this.GetProperty(DefectDescriptionProperty); }
        }
        #endregion

        #region 缺陷分类代码 DefectCategoryCode
        /// <summary>
        /// 缺陷分类代码
        /// </summary>
        [Label("缺陷分类代码")]
        public static readonly Property<string> DefectCategoryCodeProperty = P<DefectItemViewModel>.RegisterView(e => e.DefectCategoryCode, p => p.Defect.DefectCategory.Code);

        /// <summary>
        /// 缺陷分类代码
        /// </summary>
        public string DefectCategoryCode
        {
            get { return this.GetProperty(DefectCategoryCodeProperty); }
        }
        #endregion

        #region 缺陷分类描述 DefectCategoryDescription
        /// <summary>
        /// 缺陷分类描述
        /// </summary>
        [Label("缺陷分类描述")]
        public static readonly Property<string> DefectCategoryDescriptionProperty = P<DefectItemViewModel>.RegisterView(e => e.DefectCategoryDescription, p => p.Defect.DefectCategory.Description);

        /// <summary>
        /// 缺陷分类描述
        /// </summary>
        public string DefectCategoryDescription
        {
            get { return this.GetProperty(DefectCategoryDescriptionProperty); }
        }
        #endregion


        #region 检验项目 ModelInspectionItem
        /// <summary>
        /// 机型检验项目ID
        /// </summary>
        [Label("检验项目")]
        public static readonly IRefIdProperty ModelInspectionItemIdProperty =
            P<DefectItemViewModel>.RegisterRefId(e => e.ModelInspectionItemId, ReferenceType.Normal);

        /// <summary>
        /// 机型检验项目ID
        /// </summary>
        public double ModelInspectionItemId
        {
            get { return (double)this.GetRefId(ModelInspectionItemIdProperty); }
            set { this.SetRefId(ModelInspectionItemIdProperty, value); }
        }

        /// <summary>
        /// 机型检验项目
        /// </summary>
        public static readonly RefEntityProperty<ModelInspectionItem> ModelInspectionItemProperty =
            P<DefectItemViewModel>.RegisterRef(e => e.ModelInspectionItem, ModelInspectionItemIdProperty);

        /// <summary>
        /// 机型检验项目
        /// </summary>
        public ModelInspectionItem ModelInspectionItem
        {
            get { return this.GetRefEntity(ModelInspectionItemProperty); }
            set { this.SetRefEntity(ModelInspectionItemProperty, value); }
        }
        #endregion
    }
}
