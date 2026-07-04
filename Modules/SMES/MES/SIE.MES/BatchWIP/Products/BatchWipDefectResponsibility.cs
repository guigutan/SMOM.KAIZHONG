using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.BatchWIP.Products
{
    /// <summary>
    /// 产品缺陷责任
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品缺陷责任")]
    public partial class BatchWipDefectResponsibility : DataEntity
    {
        #region 责任描述 Description
        /// <summary>
        /// 责任描述
        /// </summary>
        [Label("责任描述")]
        public static readonly Property<string> DescriptionProperty = P<BatchWipDefectResponsibility>.Register(e => e.Description);

        /// <summary>
        /// 责任描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 缺陷责任 Responsibility
        /// <summary>
        /// 缺陷责任Id
        /// </summary>
        public static readonly IRefIdProperty ResponsibilityIdProperty = P<BatchWipDefectResponsibility>.RegisterRefId(e => e.ResponsibilityId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷责任Id
        /// </summary>
        public double ResponsibilityId
        {
            get { return (double)GetRefId(ResponsibilityIdProperty); }
            set { SetRefId(ResponsibilityIdProperty, value); }
        }

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public static readonly RefEntityProperty<DefectResponsibility> ResponsibilityProperty = P<BatchWipDefectResponsibility>.RegisterRef(e => e.Responsibility, ResponsibilityIdProperty);

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public DefectResponsibility Responsibility
        {
            get { return GetRefEntity(ResponsibilityProperty); }
            set { SetRefEntity(ResponsibilityProperty, value); }
        }
        #endregion

        #region 责任列表 Defect
        /// <summary>
        /// 责任列表Id
        /// </summary>
        public static readonly IRefIdProperty DefectIdProperty = P<BatchWipDefectResponsibility>.RegisterRefId(e => e.DefectId, ReferenceType.Parent);

        /// <summary>
        /// 责任列表Id
        /// </summary>
        public double DefectId
        {
            get { return (double)GetRefId(DefectIdProperty); }
            set { SetRefId(DefectIdProperty, value); }
        }

        /// <summary>
        /// 责任列表
        /// </summary>
        public static readonly RefEntityProperty<BatchWipProductDefect> DefectProperty = P<BatchWipDefectResponsibility>.RegisterRef(e => e.Defect, DefectIdProperty);

        /// <summary>
        /// 责任列表
        /// </summary>
        public BatchWipProductDefect Defect
        {
            get { return GetRefEntity(DefectProperty); }
            set { SetRefEntity(DefectProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 缺陷责任编码 ResponseCode
        /// <summary>
        /// 缺陷责任编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> ResponseCodeProperty = P<BatchWipDefectResponsibility>.RegisterView(e => e.ResponseCode, p => p.Responsibility.Code);

        /// <summary>
        /// 缺陷责任编码
        /// </summary>
        public string ResponseCode
        {
            get { return this.GetProperty(ResponseCodeProperty); }
        }
        #endregion

        #region 缺陷责任名称 ResponseDesc
        /// <summary>
        /// 缺陷责任名称
        /// </summary>
        [Label("描述")]
        public static readonly Property<string> ResponseDescProperty = P<BatchWipDefectResponsibility>.RegisterView(e => e.ResponseDesc, p => p.Responsibility.Description);

        /// <summary>
        /// 缺陷责任名称
        /// </summary>
        public string ResponseDesc
        {
            get { return this.GetProperty(ResponseDescProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 产品缺陷责任 实体配置
    /// </summary>
    internal class BatchWipDefectResponsibilityConfig : EntityConfig<BatchWipDefectResponsibility>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("BWIP_DEF_RESP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}