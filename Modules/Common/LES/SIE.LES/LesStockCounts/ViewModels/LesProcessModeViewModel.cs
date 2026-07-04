using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.LES.LesStockCounts.ViewModels
{
    /// <summary>
    /// 盘点差异处理方式
    /// </summary>
    [RootEntity, Serializable]
    [Label("差异处理方式")]
    public class LesProcessModeViewModel : ViewModel
    {
        #region 差异复核 IsDiffReview
        /// <summary>
        /// 差异复核
        /// </summary>
        [Label("差异复核")]
        public static readonly Property<bool> IsDiffReviewProperty = P<LesProcessModeViewModel>.Register(e => e.IsDiffReview);

        /// <summary>
        /// 差异复核
        /// </summary>
        public bool IsDiffReview
        {
            get { return this.GetProperty(IsDiffReviewProperty); }
            set { this.SetProperty(IsDiffReviewProperty, value); }
        }
        #endregion

        #region 差异调账 IsDiffAdjust
        /// <summary>
        /// 差异调账
        /// </summary>
        [Label("差异调账")]
        public static readonly Property<bool> IsDiffAdjustProperty = P<LesProcessModeViewModel>.Register(e => e.IsDiffAdjust);

        /// <summary>
        /// 差异调账
        /// </summary>
        public bool IsDiffAdjust
        {
            get { return this.GetProperty(IsDiffAdjustProperty); }
            set { this.SetProperty(IsDiffAdjustProperty, value); }
        }
        #endregion

        #region 不处理 IsNotDeal
        /// <summary>
        /// 不处理
        /// </summary>
        [Label("不处理")]
        public static readonly Property<bool> IsNotDealProperty = P<LesProcessModeViewModel>.Register(e => e.IsNotDeal);

        /// <summary>
        /// 不处理
        /// </summary>
        public bool IsNotDeal
        {
            get { return this.GetProperty(IsNotDealProperty); }
            set { this.SetProperty(IsNotDealProperty, value); }
        }
        #endregion

        #region 提示 Tips
        /// <summary>
        /// 提示
        /// </summary>
        [Label("提示")]
        public static readonly Property<string> TipsProperty = P<LesProcessModeViewModel>.Register(e => e.Tips);

        /// <summary>
        /// 提示
        /// </summary>
        public string Tips
        {
            get { return this.GetProperty(TipsProperty); }
            set { this.SetProperty(TipsProperty, value); }
        }
        #endregion
    }
}
