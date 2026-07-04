using SIE.Defects.Measures;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品缺陷维修措施
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷维修措施")]
    public partial class BatchWipDefectMeasure : DataEntity
    {
        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<BatchWipDefectMeasure>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 维修措施 RepairMeasure
        /// <summary>
        /// 维修措施Id
        /// </summary>
        public static readonly IRefIdProperty RepairMeasureIdProperty = P<BatchWipDefectMeasure>.RegisterRefId(e => e.RepairMeasureId, ReferenceType.Normal);

        /// <summary>
        /// 维修措施Id
        /// </summary>
        public double RepairMeasureId
        {
            get { return (double)GetRefId(RepairMeasureIdProperty); }
            set { SetRefId(RepairMeasureIdProperty, value); }
        }

        /// <summary>
        /// 维修措施
        /// </summary>
        public static readonly RefEntityProperty<RepairMeasure> RepairMeasureProperty = P<BatchWipDefectMeasure>.RegisterRef(e => e.RepairMeasure, RepairMeasureIdProperty);

        /// <summary>
        /// 维修措施
        /// </summary>
        public RepairMeasure RepairMeasure
        {
            get { return GetRefEntity(RepairMeasureProperty); }
            set { SetRefEntity(RepairMeasureProperty, value); }
        }
        #endregion

        #region 维修措施列表 Defect
        /// <summary>
        /// 维修措施列表Id
        /// </summary>
        public static readonly IRefIdProperty DefectIdProperty = P<BatchWipDefectMeasure>.RegisterRefId(e => e.DefectId, ReferenceType.Parent);

        /// <summary>
        /// 维修措施列表Id
        /// </summary>
        public double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductDefect> DefectProperty = P<BatchWipDefectMeasure>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public BatchWipProductDefect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 维修措施编码 MeasureCode
        /// <summary>
        /// 维修措施编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> MeasureCodeProperty = P<BatchWipDefectMeasure>.RegisterView(e => e.MeasureCode, p => p.RepairMeasure.Code);

        /// <summary>
        /// 维修措施编码
        /// </summary>
        public string MeasureCode
        {
            get { return this.GetProperty(MeasureCodeProperty); }
        }
        #endregion

        #region 维修措施名称 MeasureName
        /// <summary>
        /// 维修措施名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> MeasureNameProperty = P<BatchWipDefectMeasure>.RegisterView(e => e.MeasureName, p => p.RepairMeasure.Name);

        /// <summary>
        /// 维修措施名称
        /// </summary>
        public string MeasureName
        {
            get { return this.GetProperty(MeasureNameProperty); }
        }
        #endregion

        #region 维修措施描述 MeasureDesc
        /// <summary>
        /// 维修措施描述
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> MeasureDescProperty = P<BatchWipDefectMeasure>.RegisterView(e => e.MeasureDesc, p => p.RepairMeasure.Description);

        /// <summary>
        /// 维修措施描述
        /// </summary>
        public string MeasureDesc
        {
            get { return this.GetProperty(MeasureDescProperty); }
        }
        #endregion
        #endregion 
    }

    /// <summary>
    /// 产品缺陷维修措施 实体配置
    /// </summary>
    internal class BatchWipDefectMeasureConfig : EntityConfig<BatchWipDefectMeasure>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_DEF_MEA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}