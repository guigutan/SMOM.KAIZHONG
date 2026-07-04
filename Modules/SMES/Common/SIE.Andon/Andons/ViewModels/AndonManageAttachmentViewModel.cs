using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.ViewModels
{
    /// <summary>
    /// 附件信息
    /// </summary>
    [RootEntity,Serializable]
    [Label("附件信息")]
    public class AndonManageAttachmentViewModel : ViewModel
    {
        #region 附件名称 AttachmentName
        /// <summary>
        /// 附件名称
        /// </summary>
        [Label("附件名称")]
        public static readonly Property<string> AttachmentNameProperty = P<AndonManageAttachmentViewModel>.Register(e => e.AttachmentName);

        /// <summary>
        /// 附件名称
        /// </summary>
        public string AttachmentName
        {
            get { return this.GetProperty(AttachmentNameProperty); }
            set { this.SetProperty(AttachmentNameProperty, value); }
        }
        #endregion

    }
}
