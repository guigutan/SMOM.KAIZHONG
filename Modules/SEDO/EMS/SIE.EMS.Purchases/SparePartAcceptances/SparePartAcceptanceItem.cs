using SIE.Common;
using SIE.Domain;
using SIE.Equipments.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Purchases.SparePartAcceptances
{
    /// <summary>
    /// 备件验收项目
    /// </summary>
    [ChildEntity, Serializable]
    [Label("备件验收项目")]
    public partial class SparePartAcceptanceItem : DataEntity
    {
        #region 项目名称 ItemName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        [Required]
        public static readonly Property<string> ItemNameProperty = P<SparePartAcceptanceItem>.Register(e => e.ItemName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ItemName
        {
            get { return GetProperty(ItemNameProperty); }
            set { SetProperty(ItemNameProperty, value); }
        }
        #endregion

        #region 验收值 AcceptanceValue
        /// <summary>
        /// 验收值
        /// </summary>
        [Label("验收值")]
        [Required]
        public static readonly Property<string> AcceptanceValueProperty = P<SparePartAcceptanceItem>.Register(e => e.AcceptanceValue);

        /// <summary>
        /// 验收值
        /// </summary>
        public string AcceptanceValue
        {
            get { return GetProperty(AcceptanceValueProperty); }
            set { SetProperty(AcceptanceValueProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(1000)]
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<SparePartAcceptanceItem>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 验收结果 AcceptanceResult
        /// <summary>
        /// 验收结果
        /// </summary>
        [Label("检验结果")]
        public static readonly Property<InspectionResult> AcceptanceResultProperty = P<SparePartAcceptanceItem>.Register(e => e.AcceptanceResult);

        /// <summary>
        /// 验收结果
        /// </summary>
        public InspectionResult AcceptanceResult
        {
            get { return GetProperty(AcceptanceResultProperty); }
            set { SetProperty(AcceptanceResultProperty, value); }
        }
        #endregion

        #region 备件验收 SparePartAcceptance
        /// <summary>
        /// 备件验收Id
        /// </summary>
        public static readonly IRefIdProperty SparePartAcceptanceIdProperty = P<SparePartAcceptanceItem>.RegisterRefId(e => e.SparePartAcceptanceId, ReferenceType.Parent);

        /// <summary>
        /// 备件验收Id
        /// </summary>
        public double SparePartAcceptanceId
        {
            get { return (double)GetRefId(SparePartAcceptanceIdProperty); }
            set { SetRefId(SparePartAcceptanceIdProperty, value); }
        }

        /// <summary>
        /// 备件验收
        /// </summary>
        public static readonly RefEntityProperty<SparePartAcceptance> SparePartAcceptanceProperty = P<SparePartAcceptanceItem>.RegisterRef(e => e.SparePartAcceptance, SparePartAcceptanceIdProperty);

        /// <summary>
        /// 备件验收
        /// </summary>
        public SparePartAcceptance SparePartAcceptance
        {
            get { return GetRefEntity(SparePartAcceptanceProperty); }
            set { SetRefEntity(SparePartAcceptanceProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 审核状态 ApprovalStatus
        /// <summary>
        /// 审核状态
        /// </summary>
        [Label("审核状态")]
        public static readonly Property<ApprovalStatus> ApprovalStatusProperty = P<SparePartAcceptanceItem>.RegisterView(e => e.ApprovalStatus, p => p.SparePartAcceptance.ApprovalStatus);

        /// <summary>
        /// 审核状态
        /// </summary>
        public ApprovalStatus ApprovalStatus
        {
            get { return this.GetProperty(ApprovalStatusProperty); }
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 备件验收项目 实体配置
    /// </summary>
    internal class SparePartAcceptanceItemConfig : EntityConfig<SparePartAcceptanceItem>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_SP_ACPT_ITEM").MapAllProperties();
            Meta.Property(SparePartAcceptanceItem.RemarkProperty).ColumnMeta.HasLength(2000);
            Meta.EnablePhantoms();
        }
    }
}