using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Andon.Andons.ViewModels
{
    /// <summary>
    /// 安灯类型维护推送对象扩展
    /// </summary>
    [RootEntity, Serializable]
    [Label("推送对象")]
    [DisplayMember(nameof(Code))]
    public class AndonTypePushObjectViewModel : ViewModel
    {
        #region 对象编码 Code
        /// <summary>
        /// 对象编码
        /// </summary>
        [Label("对象编码")]
        public static readonly Property<string> CodeProperty = P<AndonTypePushObjectViewModel>.Register(e => e.Code);

        /// <summary>
        /// 对象编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 对象名称 Name
        /// <summary>
        /// 对象名称
        /// </summary>
        [Label("对象名称")]
        public static readonly Property<string> NameProperty = P<AndonTypePushObjectViewModel>.Register(e => e.Name);

        /// <summary>
        /// 对象名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

    }
}
