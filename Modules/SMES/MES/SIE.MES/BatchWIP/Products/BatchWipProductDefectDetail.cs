using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品缺陷记录明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷记录明细")]
    public partial class BatchWipProductDefectDetail : DataEntity
    {
        #region 缺陷代码 Defect
        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        public static readonly IRefIdProperty DefectIdProperty = P<BatchWipProductDefectDetail>.RegisterRefId(e => e.DefectId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷代码Id
        /// </summary>
        public double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public static readonly RefEntityProperty<Defect> DefectProperty = P<BatchWipProductDefectDetail>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 缺陷代码
        /// </summary>
        public Defect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 产品缺陷记录 BatchWipProductDefect
        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        [Label("产品缺陷记录")]
        public static readonly IRefIdProperty BatchWipProductDefectIdProperty =
            P<BatchWipProductDefectDetail>.RegisterRefId(e => e.BatchWipProductDefectId, ReferenceType.Parent);

        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        public double BatchWipProductDefectId
        {
            get { return (double)this.GetRefId(BatchWipProductDefectIdProperty); }
            set { this.SetRefId(BatchWipProductDefectIdProperty, value); }
        }

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductDefect> BatchWipProductDefectProperty =
            P<BatchWipProductDefectDetail>.RegisterRef(e => e.BatchWipProductDefect, BatchWipProductDefectIdProperty);

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public BatchWipProductDefect BatchWipProductDefect
        {
            get { return this.GetRefEntity(BatchWipProductDefectProperty); }
            set { this.SetRefEntity(BatchWipProductDefectProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 缺陷编码 DefectCode
        /// <summary>
        /// 缺陷编码
        /// </summary>
        [Label("缺陷编码")]
        public static readonly Property<string> DefectCodeProperty = P<BatchWipProductDefectDetail>.RegisterView(e => e.DefectCode, p => p.Defect.Code);

        /// <summary>
        /// 缺陷编码
        /// </summary>
        public string DefectCode
        {
            get { return this.GetProperty(DefectCodeProperty); }
        }
        #endregion

        #region 缺陷描述 DefectDesc
        /// <summary>
        /// 缺陷描述
        /// </summary>
        [Label("缺陷描述")]
        public static readonly Property<string> DefectDescProperty = P<BatchWipProductDefectDetail>.RegisterView(e => e.DefectDesc, p => p.Defect.Description);

        /// <summary>
        /// 缺陷描述
        /// </summary>
        public string DefectDesc
        {
            get { return this.GetProperty(DefectDescProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品缺陷记录明细 实体配置
    /// </summary>
    internal class BatchWipProductDefectDetailConfig : EntityConfig<BatchWipProductDefectDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_PROD_DEFECT_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}