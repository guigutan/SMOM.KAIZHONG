using SIE.Domain;
using SIE.LES.LesStockCounts;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.LES.LesStockCounts.ViewModels
{
    /// <summary>
    /// 批量分析ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class LesStockCountDetailAnalysisViewModel : ViewModel
    {
        #region 分析结果 AnalysisResult
        /// <summary>
        /// 分析结果
        /// </summary>
        [Label("分析结果")]
        public static readonly Property<AnalysisResult?> AnalysisResultProperty = P<LesStockCountDetailAnalysisViewModel>.Register(e => e.AnalysisResult);

        /// <summary>
        /// 分析结果
        /// </summary>
        public AnalysisResult? AnalysisResult
        {
            get { return this.GetProperty(AnalysisResultProperty); }
            set { this.SetProperty(AnalysisResultProperty, value); }
        }
        #endregion

        #region 结果描述 ResultDesc
        /// <summary>
        /// 结果描述
        /// </summary>
        [Label("结果描述")]
        public static readonly Property<string> ResultDescProperty = P<LesStockCountDetailAnalysisViewModel>.Register(e => e.ResultDesc);

        /// <summary>
        /// 结果描述
        /// </summary>
        public string ResultDesc
        {
            get { return this.GetProperty(ResultDescProperty); }
            set { this.SetProperty(ResultDescProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 输入实盘 视图配置
    /// </summary>
    public class LesStockCountDetailAnalysisViewModelViewConfig : WebViewConfig<LesStockCountDetailAnalysisViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(LesStockCountDetail));
        }

        /// <summary>
        /// 配置明细试图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.AnalysisResult);
            View.Property(p => p.ResultDesc);
        }
    }
}
