using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.EMS.Checks.Confirmations.ViewModels
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("文件信息")]
    public class ImportFileViewModel : ViewModel
    {
        #region 图片名称 FileName
        /// <summary>
        /// 图片名称
        /// </summary>
        [Label("图片名称")]
        public static readonly Property<string> FileNameProperty = P<ImportFileViewModel>.Register(e => e.FileName);

        /// <summary>
        /// 图片名称
        /// </summary>
        public string FileName
        {
            get { return GetProperty(FileNameProperty); }
            set { SetProperty(FileNameProperty, value); }
        }
        #endregion
    }
}
