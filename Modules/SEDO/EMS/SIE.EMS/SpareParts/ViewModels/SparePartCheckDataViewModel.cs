using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.SpareParts.ViewModels
{
    /// <summary>
    /// 备件 实体
    /// </summary>
    [RootEntity, Serializable]
    public class SparePartCheckDataViewModel : ViewModel
    {
        #region 备件编码 Code
        /// <summary>
        /// 备件编码
        /// </summary>
        [Label("备件编码")]
        public static readonly Property<string> CodeProperty = P<SparePartCheckDataViewModel>.Register(e => e.Code);
        /// <summary>
        /// 备件编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 备件 视图配置
    /// </summary>
    class SparePartCheckDataViewModelConfig : WebViewConfig<SparePartCheckDataViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("导入失败数据");
        }

        /// <summary>
        /// 配置列表试图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.Code);
        }
    }
}
