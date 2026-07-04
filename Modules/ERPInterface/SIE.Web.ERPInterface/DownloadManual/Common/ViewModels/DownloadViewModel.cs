using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.ERPInterface.DownloadManual.Common.ViewModels
{
    /// <summary>
    /// 工单手动下载VM
    /// </summary>
    [RootEntity, Serializable]
    public class DownloadViewModel : ViewModel
    {
        #region 唯一主键 KeyWord
        /// <summary>
        /// 唯一主键
        /// </summary>
        [Label("唯一主键")]
        public static readonly Property<string> KeyWordProperty = P<DownloadViewModel>.Register(e => e.KeyWord);

        /// <summary>
        /// 唯一主键
        /// </summary>
        public string KeyWord
        {
            get { return this.GetProperty(KeyWordProperty); }
            set { this.SetProperty(KeyWordProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 工单手动下载VM视图
    /// </summary>
    public class DownloadViewModelViewConfig : WebViewConfig<DownloadViewModel>
    {
        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.KeyWord);
        }
    }
}
