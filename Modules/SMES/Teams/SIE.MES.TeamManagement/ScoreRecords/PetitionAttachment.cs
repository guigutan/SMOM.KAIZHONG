using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 申诉附件
    /// </summary>
    [ChildEntity, Serializable]
    [Label("申诉附件")]
    public partial class PetitionAttachment : ScoreAttachBase
    {
        #region 申诉记录 PetitionRecord
        /// <summary>
        /// 申诉记录Id
        /// </summary>
        [Label("申诉记录")]
        public static readonly IRefIdProperty PetitionRecordIdProperty =
            P<PetitionAttachment>.RegisterRefId(e => e.PetitionRecordId, ReferenceType.Parent);

        /// <summary>
        /// 申诉记录Id
        /// </summary>
        public double PetitionRecordId
        {
            get { return (double)this.GetRefId(PetitionRecordIdProperty); }
            set { this.SetRefId(PetitionRecordIdProperty, value); }
        }

        /// <summary>
        /// 申诉记录
        /// </summary>
        public static readonly RefEntityProperty<PetitionRecord> PetitionRecordProperty =
            P<PetitionAttachment>.RegisterRef(e => e.PetitionRecord, PetitionRecordIdProperty);

        /// <summary>
        /// 申诉记录
        /// </summary>
        public PetitionRecord PetitionRecord
        {
            get { return this.GetRefEntity(PetitionRecordProperty); }
            set { this.SetRefEntity(PetitionRecordProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 申诉附件 实体配置
    /// </summary>
    internal class PetitionAttachmentConfig : EntityConfig<PetitionAttachment>
    {
        /// <summary>
        /// 实体配置
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WG_PET_ATTACHMENT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}