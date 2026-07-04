using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.ViewModels
{
    /// <summary>
    /// 异常预警推送对象扩展
    /// </summary>
    [RootEntity, Serializable]
    [Label("推送对象")]
    [DisplayMember(nameof(TargetCode))]
    public class PushTargetViewModel : ViewModel
    {
        #region 对象编码 TargetId
        /// <summary>
        /// 对象Id
        /// </summary>
        [Label("对象Id")]
        public static readonly Property<double> TargetIdProperty = P<PushTargetViewModel>.Register(e => e.TargetId);

        /// <summary>
        /// 对象Id
        /// </summary>
        public double TargetId
        {
            get { return this.GetProperty(TargetIdProperty); }
            set { this.SetProperty(TargetIdProperty, value); }
        }
        #endregion

        #region 对象编码 TargetCode
        /// <summary>
        /// 对象编码
        /// </summary>
        [Label("对象编码")]
        public static readonly Property<string> TargetCodeProperty = P<PushTargetViewModel>.Register(e => e.TargetCode);

        /// <summary>
        /// 对象编码
        /// </summary>
        public string TargetCode
        {
            get { return this.GetProperty(TargetCodeProperty); }
            set { this.SetProperty(TargetCodeProperty, value); }
        }
        #endregion

        #region 对象名称 TargetName
        /// <summary>
        /// 对象名称
        /// </summary>
        [Label("对象名称")]
        public static readonly Property<string> TargetNameProperty = P<PushTargetViewModel>.Register(e => e.TargetName);

        /// <summary>
        /// 对象名称
        /// </summary>
        public string TargetName
        {
            get { return this.GetProperty(TargetNameProperty); }
            set { this.SetProperty(TargetNameProperty, value); }
        }
        #endregion

    }
}
