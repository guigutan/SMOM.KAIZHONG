using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.MES.Workbench.Experiences
{
    /// <summary>
    /// 经验明细
    /// </summary>
    [ChildEntity, Serializable]
    [Label("经验明细")]
    public partial class ExperienceDetail : DataEntity
    {
        #region 图片 Photo
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PhotoProperty = P<ExperienceDetail>.Register(e => e.Photo);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Photo
        {
            get { return GetProperty(PhotoProperty); }
            set { SetProperty(PhotoProperty, value); }
        }
        #endregion

        #region 是否正确 IsRight
        /// <summary>
        /// 是否正确
        /// </summary>
        [Label("是否正确")]
        public static readonly Property<bool> IsRightProperty = P<ExperienceDetail>.Register(e => e.IsRight);

        /// <summary>
        /// 是否正确
        /// </summary>
        public bool IsRight
        {
            get { return GetProperty(IsRightProperty); }
            set { SetProperty(IsRightProperty, value); }
        }
        #endregion

        #region 图片描述 Description
        /// <summary>
        /// 图片描述
        /// </summary>
        [MaxLength(2000)]
        [Label("图片描述")]
        public static readonly Property<string> DescriptionProperty = P<ExperienceDetail>.Register(e => e.Description);

        /// <summary>
        /// 图片描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 历史经验库 HistoryExperience
        /// <summary>
        /// 历史经验库Id
        /// </summary>
        public static readonly IRefIdProperty HistoryExperienceIdProperty = P<ExperienceDetail>.RegisterRefId(e => e.HistoryExperienceId, ReferenceType.Parent);

        /// <summary>
        /// 历史经验库Id
        /// </summary>
        public double HistoryExperienceId
        {
            get { return (double)GetRefId(HistoryExperienceIdProperty); }
            set { SetRefId(HistoryExperienceIdProperty, value); }
        }

        /// <summary>
        /// 历史经验库
        /// </summary>
        public static readonly RefEntityProperty<HistoryExperience> HistoryExperienceProperty = P<ExperienceDetail>.RegisterRef(e => e.HistoryExperience, HistoryExperienceIdProperty);

        /// <summary>
        /// 历史经验库
        /// </summary>
        public HistoryExperience HistoryExperience
        {
            get { return GetRefEntity(HistoryExperienceProperty); }
            set { SetRefEntity(HistoryExperienceProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 经验明细 实体配置
    /// </summary>
    internal class ExperienceDetailConfig : EntityConfig<ExperienceDetail>
    {
        /// <summary>
        /// 配置数据表
        /// </summary>
		protected override void ConfigMeta()
        {
            Meta.MapTable("WB_HIS_EXP_DTL").MapAllProperties();
            Meta.Property(ExperienceDetail.DescriptionProperty).ColumnMeta.HasLength(4000);
            Meta.EnablePhantoms();
        }
    }
}