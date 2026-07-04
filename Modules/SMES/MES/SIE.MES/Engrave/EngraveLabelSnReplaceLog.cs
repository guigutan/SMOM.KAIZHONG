using SIE.Domain;
using SIE.MES.PackingQC;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.Engrave
{
    /// <summary>
    /// 刻码替换记录
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(EngraveLabelSnReplaceLogCriteria))]
    [Label("刻码替换记录")]
    public class EngraveLabelSnReplaceLog : DataEntity
    {
        #region EngraveSn EngraveSn
        /// <summary>
        /// EngraveSnId
        /// </summary>
        [Label("属性名")]
        public static readonly IRefIdProperty EngraveSnIdProperty =
            P<EngraveLabelSnReplaceLog>.RegisterRefId(e => e.EngraveSnId, ReferenceType.Normal);

        /// <summary>
        /// EngraveSnId
        /// </summary>
        public double EngraveSnId
        {
            get { return (double)this.GetRefId(EngraveSnIdProperty); }
            set { this.SetRefId(EngraveSnIdProperty, value); }
        }

        /// <summary>
        /// EngraveSn
        /// </summary>
        public static readonly RefEntityProperty<EngraveSn> EngraveSnProperty =
            P<EngraveLabelSnReplaceLog>.RegisterRef(e => e.EngraveSn, EngraveSnIdProperty);

        /// <summary>
        /// EngraveSn
        /// </summary>
        public EngraveSn EngraveSn
        {
            get { return this.GetRefEntity(EngraveSnProperty); }
            set { this.SetRefEntity(EngraveSnProperty, value); }
        }
        #endregion

        #region 包装QC确认明细 PackingDetail
        /// <summary>
        /// 包装QC确认明细Id
        /// </summary>
        [Label("包装QC确认明细")]
        public static readonly IRefIdProperty PackingDetailIdProperty =
            P<EngraveLabelSnReplaceLog>.RegisterRefId(e => e.PackingDetailId, ReferenceType.Normal);

        /// <summary>
        /// 包装QC确认明细Id
        /// </summary>
        public double PackingDetailId
        {
            get { return (double)this.GetRefId(PackingDetailIdProperty); }
            set { this.SetRefId(PackingDetailIdProperty, value); }
        }

        /// <summary>
        /// 包装QC确认明细
        /// </summary>
        public static readonly RefEntityProperty<PackingDetail> PackingDetailProperty =
            P<EngraveLabelSnReplaceLog>.RegisterRef(e => e.PackingDetail, PackingDetailIdProperty);

        /// <summary>
        /// 包装QC确认明细
        /// </summary>
        public PackingDetail PackingDetail
        {
            get { return this.GetRefEntity(PackingDetailProperty); }
            set { this.SetRefEntity(PackingDetailProperty, value); }
        }
        #endregion

        #region 原刻码 OldSn
        /// <summary>
        /// 原刻码
        /// </summary>
        [Label("原刻码")]
        public static readonly Property<string> OldSnProperty = P<EngraveLabelSnReplaceLog>.Register(e => e.OldSn);

        /// <summary>
        /// 原刻码
        /// </summary>
        public string OldSn
        {
            get { return this.GetProperty(OldSnProperty); }
            set { this.SetProperty(OldSnProperty, value); }
        }
        #endregion

        #region 新刻码 NewSn
        /// <summary>
        /// 新刻码
        /// </summary>
        [Label("新刻码")]
        public static readonly Property<string> NewSnProperty = P<EngraveLabelSnReplaceLog>.Register(e => e.NewSn);

        /// <summary>
        /// 新刻码
        /// </summary>
        public string NewSn
        {
            get { return this.GetProperty(NewSnProperty); }
            set { this.SetProperty(NewSnProperty, value); }
        }
        #endregion

        #region 视图属性

        #region 产品编码 ProductCode
        /// <summary>
        /// 产品编码
        /// </summary>
        [Label("产品编码")]
        public static readonly Property<string> ProductCodeProperty = P<EngraveLabelSnReplaceLog>.RegisterView(e => e.ProductCode, p => p.EngraveSn.EngraveLabel.Product.Code);

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ProductCode
        {
            get { return this.GetProperty(ProductCodeProperty); }
        }
        #endregion

        #region 产品名称 ProductName
        /// <summary>
        /// 产品名称
        /// </summary>
        [Label("产品名称")]
        public static readonly Property<string> ProductNameProperty = P<EngraveLabelSnReplaceLog>.RegisterView(e => e.ProductName, p => p.EngraveSn.EngraveLabel.Product.Name);

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName
        {
            get { return this.GetProperty(ProductNameProperty); }
        }
        #endregion

        #region 旧料号 ShortDescription
        /// <summary>
        /// 旧料号
        /// </summary>
        [Label("旧料号")]
        public static readonly Property<string> ShortDescriptionProperty = P<EngraveLabelSnReplaceLog>.RegisterView(e => e.ShortDescription, p => p.EngraveSn.EngraveLabel.Product.ShortDescription);

        /// <summary>
        /// 旧料号
        /// </summary>
        public string ShortDescription
        {
            get { return this.GetProperty(ShortDescriptionProperty); }
        }
        #endregion

        #region 标签号 BatchNo
        /// <summary>
        /// 标签号
        /// </summary>
        [Label("标签号")]
        public static readonly Property<string> BatchNoProperty = P<EngraveLabelSnReplaceLog>.RegisterView(e => e.BatchNo, p => p.EngraveSn.EngraveLabel.BatchNo);

        /// <summary>
        /// 标签号
        /// </summary>
        public string BatchNo
        {
            get { return this.GetProperty(BatchNoProperty); }
        }
        #endregion

        #region 蓝标 BlueLabel
        /// <summary>
        /// 蓝标
        /// </summary>
        [Label("蓝标")]
        public static readonly Property<string> BlueLabelProperty = P<EngraveLabelSnReplaceLog>.RegisterView(e => e.BlueLabel,p=>p.PackingDetail.PackingQc.BlueLabel);

        /// <summary>
        /// 蓝标
        /// </summary>
        public string BlueLabel
        {
            get { return this.GetProperty(BlueLabelProperty); }
        }
        #endregion

        #endregion
    }

    internal class EngraveLabelSnReplaceLogConfig : EntityConfig<EngraveLabelSnReplaceLog>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("ENGRAVE_LABEL_SN_R_LOG").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
