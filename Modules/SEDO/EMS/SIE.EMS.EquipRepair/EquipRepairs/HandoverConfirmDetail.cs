using SIE.Domain;
using SIE.EMS.EquipRepair.EquipRepairs.Enums;
using SIE.EMS.Tpms;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.EquipRepair.EquipRepairs
{
    /// <summary>
    /// 交机确认评分表
    /// </summary>
    [RootEntity, Serializable]
    [Label("评分项")]
    public partial class HandoverConfirmDetail : DataEntity
    {
        #region 设备维修单 EquipRepairBill
        /// <summary>
        /// 设备维修单Id
        /// </summary>
        [Label("设备维修单")]
        public static readonly IRefIdProperty EquipRepairBillIdProperty =
            P<HandoverConfirmDetail>.RegisterRefId(e => e.EquipRepairBillId, ReferenceType.Parent);

        /// <summary>
        /// 设备维修单Id
        /// </summary>
        public double EquipRepairBillId
        {
            get { return (double)this.GetRefId(EquipRepairBillIdProperty); }
            set { this.SetRefId(EquipRepairBillIdProperty, value); }
        }

        /// <summary>
        /// 设备维修单
        /// </summary>
        public static readonly RefEntityProperty<EquipRepairBill> EquipRepairBillProperty =
            P<HandoverConfirmDetail>.RegisterRef(e => e.EquipRepairBill, EquipRepairBillIdProperty);

        /// <summary>
        /// 设备维修单
        /// </summary>
        public EquipRepairBill EquipRepairBill
        {
            get { return this.GetRefEntity(EquipRepairBillProperty); }
            set { this.SetRefEntity(EquipRepairBillProperty, value); }
        }
        #endregion

        #region TPM检查评分项 TpmWeekInspectScore
        /// <summary>
        /// TPM检查评分项Id
        /// </summary>
        [Label("TPM检查评分项")]
        public static readonly IRefIdProperty TpmWeekInspectScoreIdProperty =
            P<HandoverConfirmDetail>.RegisterRefId(e => e.TpmWeekInspectScoreId, ReferenceType.Normal);

        /// <summary>
        /// TPM检查评分项Id
        /// </summary>
        public double TpmWeekInspectScoreId
        {
            get { return (double)this.GetRefId(TpmWeekInspectScoreIdProperty); }
            set { this.SetRefId(TpmWeekInspectScoreIdProperty, value); }
        }

        /// <summary>
        /// TPM检查评分项
        /// </summary>
        public static readonly RefEntityProperty<TpmWeekInspectScore> TpmWeekInspectScoreProperty =
            P<HandoverConfirmDetail>.RegisterRef(e => e.TpmWeekInspectScore, TpmWeekInspectScoreIdProperty);

        /// <summary>
        /// TPM检查评分项
        /// </summary>
        public TpmWeekInspectScore TpmWeekInspectScore
        {
            get { return this.GetRefEntity(TpmWeekInspectScoreProperty); }
            set { this.SetRefEntity(TpmWeekInspectScoreProperty, value); }
        }
        #endregion

        #region 评分项 EquipRepairScore
        /// <summary>
        /// 评分项
        /// </summary>
        [Label("评分项")]
        public static readonly Property<EquipRepairScore?> EquipRepairScoreProperty = P<HandoverConfirmDetail>.Register(e => e.EquipRepairScore);

        /// <summary>
        /// 评分项
        /// </summary>
        public EquipRepairScore? EquipRepairScore
        {
            get { return this.GetProperty(EquipRepairScoreProperty); }
            set { this.SetProperty(EquipRepairScoreProperty, value); }
        }
        #endregion

        #region 图片路径 HandoverAttachment
        /// <summary>
        /// 图片路径
        /// </summary>
        [Label("图片路径")]
        [MaxLength(4000)]
        public static readonly Property<string> HandoverAttachmentProperty = P<HandoverConfirmDetail>.Register(e => e.HandoverAttachment);

        /// <summary>
        /// 图片路径
        /// </summary>
        public string HandoverAttachment
        {
            get { return GetProperty(HandoverAttachmentProperty); }
            set { SetProperty(HandoverAttachmentProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<HandoverConfirmDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return this.GetProperty(RemarkProperty); }
            set { this.SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 图片内容 Content
        /// <summary>
        /// 图片内容
        /// </summary>
        [Label("图片内容")]
        public static readonly Property<byte[]> ContentProperty = P<HandoverConfirmDetail>.Register(e => e.Content);

        /// <summary>
        /// 图片内容
        /// </summary>
        public byte[] Content
        {
            get { return this.GetProperty(ContentProperty); }
            set { this.SetProperty(ContentProperty, value); }
        }
        #endregion

        #region 项目 ProjectName
        /// <summary>
        /// 项目
        /// </summary>
        [Label("项目")]
        public static readonly Property<string> ProjectNameProperty = P<HandoverConfirmDetail>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectName
        {
            get { return this.GetProperty(ProjectNameProperty); }
            set { this.SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 项目 ProjectNameView
        /// <summary>
        /// 项目
        /// </summary>
        [Label("项目")]
        public static readonly Property<string> ProjectNameViewProperty = P<HandoverConfirmDetail>.RegisterView(e => e.ProjectNameView, p => p.TpmWeekInspectScore.ProjectName);

        /// <summary>
        /// 项目
        /// </summary>
        public string ProjectNameView
        {
            get { return this.GetProperty(ProjectNameViewProperty); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 交机确认评分 实体配置
    /// </summary>
    internal class HandoverConfirmDetailConfig : EntityConfig<HandoverConfirmDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_HAND_CONF_DET").MapAllProperties();
            Meta.Property(HandoverConfirmDetail.HandoverAttachmentProperty).ColumnMeta.HasLength(4000);
            Meta.Property(HandoverConfirmDetail.ContentProperty).DontMapColumn();
            Meta.Property(HandoverConfirmDetail.ProjectNameProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
