using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("评分附件")]
    public partial class ScoreAttachment : ScoreAttachBase
    {
        #region 评分记录 SocreRecord
        /// <summary>
        /// 评分记录Id
        /// </summary>
        [Label("评分记录")]
        public static readonly IRefIdProperty SocreRecordIdProperty =
            P<ScoreAttachment>.RegisterRefId(e => e.SocreRecordId, ReferenceType.Parent);

        /// <summary>
        /// 评分记录Id
        /// </summary>
        public double SocreRecordId
        {
            get { return (double)this.GetRefId(SocreRecordIdProperty); }
            set { this.SetRefId(SocreRecordIdProperty, value); }
        }

        /// <summary>
        /// 评分记录
        /// </summary>
        public static readonly RefEntityProperty<ScoreRecord> SocreRecordProperty =
            P<ScoreAttachment>.RegisterRef(e => e.SocreRecord, SocreRecordIdProperty);

        /// <summary>
        /// 评分记录
        /// </summary>
        public ScoreRecord SocreRecord
        {
            get { return this.GetRefEntity(SocreRecordProperty); }
            set { this.SetRefEntity(SocreRecordProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 评分附件 实体配置
    /// </summary>
    internal class ScoreAttachmentConfig : EntityConfig<ScoreAttachment>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WG_SCORE_ATTACHMENT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}