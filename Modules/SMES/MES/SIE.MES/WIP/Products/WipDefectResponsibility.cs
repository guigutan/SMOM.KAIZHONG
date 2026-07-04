using SIE.Defects;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 产品缺陷责任
    /// </summary>
    [ChildEntity, Serializable]
    [Label("缺陷责任")]
    public class WipDefectResponsibility : DataEntity
    {
        #region 产品缺陷记录 WipProductDefect
        /// <summary>
        /// 产品缺陷记录Id
        /// </summary>
        [Label("产品缺陷记录")]
        public static readonly IRefIdProperty WipProductDefectIdProperty =
            P<WipDefectResponsibility>.RegisterRefId(e => e.WipProductDefectId, ReferenceType.Parent);

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
            P<WipDefectResponsibility>.RegisterRef(e => e.WipProductDefect, WipProductDefectIdProperty);

        /// <summary>
        /// 产品缺陷记录
        /// </summary>
        public WipProductDefect WipProductDefect
        {
            get { return this.GetRefEntity(WipProductDefectProperty); }
            set { this.SetRefEntity(WipProductDefectProperty, value); }
        }
        #endregion

        #region 缺陷责任 DefectResponsibility
        /// <summary>
        /// 缺陷责任Id
        /// </summary>
        [Label("缺陷责任")]
        public static readonly IRefIdProperty DefectResponsibilityIdProperty =
            P<WipDefectResponsibility>.RegisterRefId(e => e.DefectResponsibilityId, ReferenceType.Normal);

        /// <summary>
        /// 缺陷责任Id
        /// </summary>
        public double DefectResponsibilityId
        {
            get { return (double)this.GetRefId(DefectResponsibilityIdProperty); }
            set { this.SetRefId(DefectResponsibilityIdProperty, value); }
        }

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public static readonly RefEntityProperty<DefectResponsibility> DefectResponsibilityProperty =
            P<WipDefectResponsibility>.RegisterRef(e => e.DefectResponsibility, DefectResponsibilityIdProperty);

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public DefectResponsibility DefectResponsibility
        {
            get { return this.GetRefEntity(DefectResponsibilityProperty); }
            set { this.SetRefEntity(DefectResponsibilityProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 缺陷责任编码 ResponseCode
        /// <summary>
        /// 缺陷责任编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> ResponseCodeProperty = P<WipDefectResponsibility>.RegisterView(e => e.ResponseCode, p => p.DefectResponsibility.Code);

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
        public static readonly Property<string> ResponseDescProperty = P<WipDefectResponsibility>.RegisterView(e => e.ResponseDesc, p => p.DefectResponsibility.Description);

        /// <summary>
        /// 缺陷责任名称
        /// </summary>
        public string ResponseDesc
        {
            get { return this.GetProperty(ResponseDescProperty); }
        }
        #endregion

        #region 缺陷位置 DefectLocation
        /// <summary>
        /// 缺陷位置
        /// </summary>
        [Label("缺陷位置")]
        public static readonly Property<string> DefectLocationProperty = P<WipDefectResponsibility>.RegisterView(e => e.DefectLocation, p => p.WipProductDefect.Location);

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
    /// 产品缺陷责任 实体配置
    /// </summary>
    internal class WipDefectResponsibilityConfig : EntityConfig<WipDefectResponsibility>
    {
        /// <summary>
        /// 数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_DEF_RESP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
