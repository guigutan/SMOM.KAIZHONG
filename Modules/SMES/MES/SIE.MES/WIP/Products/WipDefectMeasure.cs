using SIE.Defects.Measures;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品缺陷维修措施
    /// </summary>
    [ChildEntity, Serializable]
    [Label("维修措施")]
    public class WipDefectMeasure : DataEntity
    {
        #region 产品缺陷记录 WipProductDefect
        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        [Label("产品缺陷记录")]
        public static readonly IRefIdProperty WipProductDefectIdProperty =
            P<WipDefectMeasure>.RegisterRefId(e => e.WipProductDefectId, ReferenceType.Parent);

        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        public double WipProductDefectId
        {
            get { return (double)this.GetRefId(WipProductDefectIdProperty); }
            set { this.SetRefId(WipProductDefectIdProperty, value); }
        }

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public static readonly RefEntityProperty<WipProductDefect> WipProductDefectProperty =
            P<WipDefectMeasure>.RegisterRef(e => e.WipProductDefect, WipProductDefectIdProperty);

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public WipProductDefect WipProductDefect
        {
            get { return this.GetRefEntity(WipProductDefectProperty); }
            set { this.SetRefEntity(WipProductDefectProperty, value); }
        }
        #endregion

        #region 维修措施 RepairMeasure
        /// <summary>
        /// 维修措施Id
        /// </summary>
        [Label("维修措施")]
        public static readonly IRefIdProperty RepairMeasureIdProperty =
            P<WipDefectMeasure>.RegisterRefId(e => e.RepairMeasureId, ReferenceType.Normal);

        /// <summary>
        /// 维修措施Id
        /// </summary>
        public double RepairMeasureId
        {
            get { return (double)this.GetRefId(RepairMeasureIdProperty); }
            set { this.SetRefId(RepairMeasureIdProperty, value); }
        }

        /// <summary>
        /// 维修措施
        /// </summary>
        public static readonly RefEntityProperty<RepairMeasure> RepairMeasureProperty =
            P<WipDefectMeasure>.RegisterRef(e => e.RepairMeasure, RepairMeasureIdProperty);

        /// <summary>
        /// 维修措施
        /// </summary>
        public RepairMeasure RepairMeasure
        {
            get { return this.GetRefEntity(RepairMeasureProperty); }
            set { this.SetRefEntity(RepairMeasureProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 维修措施编码 MeasureCode
        /// <summary>
        /// 维修措施编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> MeasureCodeProperty = P<WipDefectMeasure>.RegisterView(e => e.MeasureCode, p => p.RepairMeasure.Code);

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
        public static readonly Property<string> MeasureNameProperty = P<WipDefectMeasure>.RegisterView(e => e.MeasureName, p => p.RepairMeasure.Name);

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
        public static readonly Property<string> MeasureDescProperty = P<WipDefectMeasure>.RegisterView(e => e.MeasureDesc, p => p.RepairMeasure.Description);

        /// <summary>
        /// 维修措施描述
        /// </summary>
        public string MeasureDesc
        {
            get { return this.GetProperty(MeasureDescProperty); }
        }
        #endregion

        #region 缺陷位置 DefectLocation
        /// <summary>
        /// 缺陷位置
        /// </summary>
        [Label("缺陷位置")]
        public static readonly Property<string> DefectLocationProperty = P<WipDefectMeasure>.RegisterView(e => e.DefectLocation, p => p.WipProductDefect.Location);

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string DefectLocation
        {
            get { return this.GetProperty(DefectLocationProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品缺陷维修措施 实体配置
    /// </summary>
    internal class WipDefectMeasureConfig : EntityConfig<WipDefectMeasure>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_DEF_MEA").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
