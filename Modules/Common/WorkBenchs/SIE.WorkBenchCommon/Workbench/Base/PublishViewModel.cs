using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.WorkBenchCommon.Workbench.Base
{
    /// <summary>
    /// 发布弹出窗ViewModel
    /// </summary>
    [RootEntity, Serializable]
    public class PublishViewModel : ViewModel
    {
        #region 显视模式 DisplayMode
        /// <summary>
        /// 显视模式
        /// </summary>
        [Required]
        [Label("显视模式")]
        public static readonly Property<DisplayMode> DisplayModeProperty = P<PublishViewModel>.Register(e => e.DisplayMode);

        /// <summary>
        /// 显视模式
        /// </summary>
        public DisplayMode DisplayMode
        {
            get { return this.GetProperty(DisplayModeProperty); }
            set { this.SetProperty(DisplayModeProperty, value); }
        }
        #endregion

    }
}
