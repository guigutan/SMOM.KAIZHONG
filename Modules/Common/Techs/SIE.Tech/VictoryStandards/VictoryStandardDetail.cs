using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Tech.VictoryStandards
{
    /// <summary>
    /// 胜局标准明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("胜局标准")]
    public partial class VictoryStandardDetail : DataEntity
    {
        #region 胜局标准 Standard
        /// <summary>
        /// 胜局标准
        /// </summary>
        [Label("胜局标准")]
        [Required]
        public static readonly Property<string> StandardProperty = P<VictoryStandardDetail>.Register(e => e.Standard);

        /// <summary>
        /// 胜局标准
        /// </summary>
        public string Standard
        {
            get { return this.GetProperty(StandardProperty); }
            set { this.SetProperty(StandardProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<VictoryStandardDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 胜制方案 VictoryStandard
        /// <summary>
        /// 胜制方案Id
        /// </summary>
        [Label("胜制方案")]
        public static readonly IRefIdProperty VictoryStandardIdProperty =
            P<VictoryStandardDetail>.RegisterRefId(e => e.VictoryStandardId, ReferenceType.Parent);

        /// <summary>
        /// 胜制方案Id
        /// </summary>
        public double VictoryStandardId
        {
            get { return (double)this.GetRefId(VictoryStandardIdProperty); }
            set { this.SetRefId(VictoryStandardIdProperty, value); }
        }

        /// <summary>
        /// 胜制方案
        /// </summary>
        public static readonly RefEntityProperty<VictoryStandard> VictoryStandardProperty =
            P<VictoryStandardDetail>.RegisterRef(e => e.VictoryStandard, VictoryStandardIdProperty);

        /// <summary>
        /// 胜制方案
        /// </summary>
        public VictoryStandard VictoryStandard
        {
            get { return this.GetRefEntity(VictoryStandardProperty); }
            set { this.SetRefEntity(VictoryStandardProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 胜局标准明细 实体配置
    /// </summary>
    internal class VictoryStandardDetailConfig : EntityConfig<VictoryStandardDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("TECH_VIC_STA_DET").MapAllProperties();
            Meta.Property(VictoryStandardDetail.VictoryStandardIdProperty).ColumnMeta.HasIndex();
            Meta.EnablePhantoms();
        }
    }
}
