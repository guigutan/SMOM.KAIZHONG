using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Tpms
{
    /// <summary>
    /// TMP评分明细
    /// </summary>
    [ChildEntity, Serializable]
    //[CriteriaQuery]
    [Label("TPM评分明细")]
    public partial class TpmRecordDetail : DataEntity
    {
        #region TMP评分 TmpScroe
        /// <summary>
        /// TMP评分Id
        /// </summary>
        [Label("TPM评分")]
        public static readonly IRefIdProperty TpmRecordIdProperty = P<TpmRecordDetail>.RegisterRefId(e => e.TpmRecordId, ReferenceType.Parent);

        /// <summary>
        /// TMP评分Id
        /// </summary>
        public double TpmRecordId
        {
            get { return (double)GetRefId(TpmRecordIdProperty); }
            set { SetRefId(TpmRecordIdProperty, value); }
        }

        /// <summary>
        /// TMP评分
        /// </summary>
        public static readonly RefEntityProperty<TpmRecord> TpmRecordProperty = P<TpmRecordDetail>.RegisterRef(e => e.TpmRecord, TpmRecordIdProperty);

        /// <summary>
        /// TMP评分
        /// </summary>
        public TpmRecord TpmRecord
        {
            get { return GetRefEntity(TpmRecordProperty); }
            set { SetRefEntity(TpmRecordProperty, value); }
        }
        #endregion

        #region 扣分 DeductScore
        /// <summary>
        /// 扣分
        /// </summary>
        [Required]
        [Label("扣分")]
        public static readonly Property<int> DeductScoreProperty = P<TpmRecordDetail>.Register(e => e.DeductScore);

        /// <summary>
        /// 扣分
        /// </summary>
        public int DeductScore
        {
            get { return GetProperty(DeductScoreProperty); }
            set { SetProperty(DeductScoreProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<TpmRecordDetail>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }
        #endregion

        #region 评分项 WeekInspectScore
        /// <summary>
        /// 评分项Id
        /// </summary>
        [Label("评分项")]
        public static readonly IRefIdProperty WeekInspectScoreIdProperty = P<TpmRecordDetail>.RegisterRefId(e => e.WeekInspectScoreId, ReferenceType.Normal);

        /// <summary>
        /// 评分项Id
        /// </summary>
        public double WeekInspectScoreId
        {
            get { return (double)GetRefId(WeekInspectScoreIdProperty); }
            set { SetRefId(WeekInspectScoreIdProperty, value); }
        }

        /// <summary>
        /// 评分项
        /// </summary>
        public static readonly RefEntityProperty<TpmWeekInspectScore> WeekInspectScoreProperty = P<TpmRecordDetail>.RegisterRef(e => e.WeekInspectScore, WeekInspectScoreIdProperty);

        /// <summary>
        /// 评分项
        /// </summary>
        public TpmWeekInspectScore WeekInspectScore
        {
            get { return GetRefEntity(WeekInspectScoreProperty); }
            set { SetRefEntity(WeekInspectScoreProperty, value); }
        }
        #endregion

        #region 图片 Photo
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> PhotoProperty = P<TpmRecordDetail>.Register(e => e.Photo);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] Photo
        {
            get { return GetProperty(PhotoProperty); }
            set { SetProperty(PhotoProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<TpmRecordDetail>.RegisterView(e => e.ProjectName, p => p.WeekInspectScore.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 类型 ProjectType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ScoreType> ProjectTypeProperty = P<TpmRecordDetail>.RegisterView(e => e.ProjectType, p => p.WeekInspectScore.ScoreType);

        /// <summary>
        /// 类型
        /// </summary>
        public ScoreType ProjectType
        {
            get { return GetProperty(ProjectTypeProperty); }
            set { SetProperty(ProjectTypeProperty, value); }
        }
        #endregion

        #region EDO_检查标准或要求 CheckSatandard
        ///// <summary>
        ///// 检查标准或要求
        ///// </summary>
        //[Label("检查标准或要求")]
        //public static readonly Property<string> CheckStandardProperty = P<TpmRecordDetail>.RegisterView(e => e.CheckStandard, p => p.WeekInspectScore.CheckStandard);

        ///// <summary>
        ///// 检查标准或要求
        ///// </summary>
        //public string CheckStandard
        //{
        //    get { return GetProperty(CheckStandardProperty); }
        //    set { SetProperty(CheckStandardProperty, value); }
        //}
        #endregion

        #region EDO_扣除分数上限 ScoreRate
        ///// <summary>
        ///// 扣除分数上限
        ///// </summary>
        //[Label("扣除分数上限")]
        //public static readonly Property<int> ScoreRateProperty = P<TpmRecordDetail>.RegisterView(e => e.ScoreRate, p => p.WeekInspectScore.ScoreRate);

        ///// <summary>
        ///// 扣除分数上限
        ///// </summary>
        //public int ScoreRate
        //{
        //    get { return GetProperty(ScoreRateProperty); }
        //    set { SetProperty(ScoreRateProperty, value); }
        //}
        #endregion
        #endregion
    }

    /// <summary>
    /// TMP评分明细 实体配置
    /// </summary>
    internal class TpmRecordDetailConfig : EntityConfig<TpmRecordDetail>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_TPM_RECORD_DTL").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}