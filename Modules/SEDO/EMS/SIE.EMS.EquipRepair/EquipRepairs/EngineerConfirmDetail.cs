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
    /// 工程确认评分表
    /// </summary>
    [RootEntity, Serializable]
    [Label("工程确认")]
    public partial class EngineerConfirmDetail : DataEntity
    {
        #region 设备维修单 EquipRepairBill
        /// <summary>
        /// 设备维修单Id
        /// </summary>
        [Label("设备维修单")]
        public static readonly IRefIdProperty EquipRepairBillIdProperty =
            P<EngineerConfirmDetail>.RegisterRefId(e => e.EquipRepairBillId, ReferenceType.Parent);

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
            P<EngineerConfirmDetail>.RegisterRef(e => e.EquipRepairBill, EquipRepairBillIdProperty);

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
            P<EngineerConfirmDetail>.RegisterRefId(e => e.TpmWeekInspectScoreId, ReferenceType.Normal);

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
            P<EngineerConfirmDetail>.RegisterRef(e => e.TpmWeekInspectScore, TpmWeekInspectScoreIdProperty);

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
        public static readonly Property<EquipRepairScore?> EquipRepairScoreProperty = P<EngineerConfirmDetail>.Register(e => e.EquipRepairScore);

        /// <summary>
        /// 评分项
        /// </summary>
        public EquipRepairScore? EquipRepairScore
        {
            get { return this.GetProperty(EquipRepairScoreProperty); }
            set { this.SetProperty(EquipRepairScoreProperty, value); }
        }
        #endregion

        #region 图片路径 EngineerAttachment
        /// <summary>
        /// 图片路径
        /// </summary>
        [Label("图片路径")]
        [MaxLength(4000)]
        public static readonly Property<string> EngineerAttachmentProperty = P<EngineerConfirmDetail>.Register(e => e.EngineerAttachment);

        /// <summary>
        /// 图片路径
        /// </summary>
        public string EngineerAttachment
        {
            get { return GetProperty(EngineerAttachmentProperty); }
            set { SetProperty(EngineerAttachmentProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<EngineerConfirmDetail>.Register(e => e.Remark);

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
        public static readonly Property<byte[]> ContentProperty = P<EngineerConfirmDetail>.Register(e => e.Content);

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
        public static readonly Property<string> ProjectNameProperty = P<EngineerConfirmDetail>.Register(e => e.ProjectName);

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
        public static readonly Property<string> ProjectNameViewProperty = P<EngineerConfirmDetail>.RegisterView(e => e.ProjectNameView, p => p.TpmWeekInspectScore.ProjectName);

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
    /// 工程确认评分 实体配置
    /// </summary>
    internal class EngineerConfirmDetailConfig : EntityConfig<EngineerConfirmDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_ENGI_CONF_DET").MapAllProperties();
            Meta.Property(EngineerConfirmDetail.EngineerAttachmentProperty).ColumnMeta.HasLength(4000);
            Meta.Property(EngineerConfirmDetail.ContentProperty).DontMapColumn();
            Meta.Property(EngineerConfirmDetail.ProjectNameProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }
}
