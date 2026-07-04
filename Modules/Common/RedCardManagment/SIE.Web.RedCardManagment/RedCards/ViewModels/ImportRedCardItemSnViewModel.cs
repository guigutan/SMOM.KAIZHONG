using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.RedCardManagment.RedCards.ViewModels
{
    /// <summary>
    /// 导入红牌物料SN追溯清单 实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("导入红牌物料SN追溯清单")]
    public class ImportRedCardItemSnViewModel : ViewModel
    {
        #region 序列号 SN
        /// <summary>
        /// 红牌ReelId
        /// </summary>
        [Required]
        [Label("序列号")]
        public static readonly Property<string> SNProperty = P<ImportRedCardItemSnViewModel>.Register(e => e.SN);

        /// <summary>
        /// 序列号
        /// </summary>
        public string SN
        {
            get { return GetProperty(SNProperty); }
            set { SetProperty(SNProperty, value); }
        }
        #endregion

        #region 数量 Quannity
        /// <summary>
        /// 数量
        /// </summary>
        [Label("数量")]
        public static readonly Property<double> QuannityProperty = P<ImportRedCardItemSnViewModel>.Register(e => e.Quannity);

        /// <summary>
        /// 数量
        /// </summary>
        public double Quannity
        {
            get { return GetProperty(QuannityProperty); }
            set { SetProperty(QuannityProperty, value); }
        }
        #endregion

        #region 生产日期 ProductDate
        /// <summary>
        /// 生产日期
        /// </summary>
        [Label("生产日期")]
        public static readonly Property<DateTime> ProductDateProperty = P<ImportRedCardItemSnViewModel>.Register(e => e.ProductDate);

        /// <summary>
        /// 生产日期
        /// </summary>
        public DateTime ProductDate
        {
            get { return GetProperty(ProductDateProperty); }
            set { SetProperty(ProductDateProperty, value); }
        }
        #endregion

        #region ErrorMessage 导入失败原因
        /// <summary>
        /// 导入失败原因
        /// </summary>
        [Label("导入失败原因")]
        public static readonly Property<string> ErrorMessageProperty = P<ImportRedCardItemSnViewModel>.Register(e => e.ErrorMessage);

        /// <summary>
        /// 导入失败原因
        /// </summary>
        public string ErrorMessage
        {
            get { return this.GetProperty(ErrorMessageProperty); }
            set { this.SetProperty(ErrorMessageProperty, value); }
        }

        #endregion
    }

    /// <summary>
    /// 导入失败数据 视图配置
    /// </summary>
    public class ImportRedCardItemSnViewModelConfig : WebViewConfig<ImportRedCardItemSnViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ItemSnRetroactiveViewConfig));
        }

        /// <summary>
        /// 配置列表试图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.ErrorMessage);
            View.Property(p => p.SN);
            View.Property(p => p.Quannity);
            View.Property(p => p.ProductDate).UseDateEditor();
        }
    }
}
