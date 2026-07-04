using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.EMS.EquipRepair.EquipRepairs.ViewModels
{
    /// <summary>
    /// 文件信息
    /// </summary>
    [RootEntity, Serializable]
    [Label("文件信息")]
    public class ImportFileViewModel : ViewModel
    {
        #region 文件名称 FileName
        /// <summary>
        /// FileName
        /// </summary>
        [Label("文件名称")]
        public static readonly Property<string> FileNameProperty = P<ImportFileViewModel>.Register(e => e.FileName);

        /// <summary>
        /// FileName
        /// </summary>
        public string FileName
        {
            get { return GetProperty(FileNameProperty); }
            set { SetProperty(FileNameProperty, value); }
        }
        #endregion
    }
}
