using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.Tpms
{
    /// <summary>
    /// TPM检查评分项
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("TPM检查评分项")]
    public partial class TpmWeekInspectScore : DataEntity
    {
        #region 项目名称 ProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Required]
        //[NotDuplicate]
        [Label("项目名称")]
        public static readonly Property<string> ProjectNameProperty = P<TpmWeekInspectScore>.Register(e => e.ProjectName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string ProjectName
        {
            get { return GetProperty(ProjectNameProperty); }
            set { SetProperty(ProjectNameProperty, value); }
        }
        #endregion

        #region 类型 ScoreType
        /// <summary>
        /// 类型
        /// </summary>
        [Required]
        [Label("类型")]
        public static readonly Property<ScoreType> ScoreTypeProperty = P<TpmWeekInspectScore>.Register(e => e.ScoreType);

        /// <summary>
        /// 类型
        /// </summary>
        public ScoreType ScoreType
        {
            get { return GetProperty(ScoreTypeProperty); }
            set { SetProperty(ScoreTypeProperty, value); }
        }
        #endregion

        #region EDO_检查标准或要求 CheckStandard
        ///// <summary>
        ///// 检查标准或要求
        ///// </summary>
        //[MaxLength(1000)]
        //[Label("检查标准或要求")]
        //public static readonly Property<string> CheckStandardProperty = P<TpmWeekInspectScore>.Register(e => e.CheckStandard);

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
        //[Required]
        //[MinValue(0)]
        //[MaxValue(100)]
        //[Label("扣除分数上限")]
        //public static readonly Property<int> ScoreRateProperty = P<TpmWeekInspectScore>.Register(e => e.ScoreRate);

        ///// <summary>
        ///// 扣除分数上限
        ///// </summary>
        //public int ScoreRate
        //{
        //    get { return GetProperty(ScoreRateProperty); }
        //    set { SetProperty(ScoreRateProperty, value); }
        //}
        #endregion

        #region 是否拍照 IsPhoto
        /// <summary>
        /// 是否拍照
        /// </summary>
        [Required]
        [Label("是否拍照")]
        public static readonly Property<bool> IsPhotoProperty = P<TpmWeekInspectScore>.Register(e => e.IsPhoto);

        /// <summary>
        /// 是否拍照
        /// </summary>
        public bool IsPhoto
        {
            get { return GetProperty(IsPhotoProperty); }
            set { SetProperty(IsPhotoProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// TPM检查评分项 实体配置
    /// </summary>
    internal class TpmWeekInspectScoreConfig : EntityConfig<TpmWeekInspectScore>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("EMS_TMP_INSP_SOCRE").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}