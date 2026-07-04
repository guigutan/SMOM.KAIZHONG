using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.ViewModels
{
    /// <summary>
    /// 图片信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("图片信息")]
    public class AndonManageImageViewModel : ViewModel
    {
        #region 图片名称 FileName
        /// <summary>
        /// 图片名称
        /// </summary>
        [Label("图片名称")]
        public static readonly Property<string> FileNameProperty = P<AndonManageImageViewModel>.Register(e => e.FileName);

        /// <summary>
        /// 图片名称
        /// </summary>
        public string FileName
        {
            get { return this.GetProperty(FileNameProperty); }
            set { this.SetProperty(FileNameProperty, value); }
        }
        #endregion

    }
}
