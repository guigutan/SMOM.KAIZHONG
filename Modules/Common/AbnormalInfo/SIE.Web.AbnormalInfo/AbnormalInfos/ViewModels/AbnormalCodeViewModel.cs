using SIE.Alert;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;

namespace SIE.Web.AbnormalInfo.AbnormalInfos.ViewModels
{
    /// <summary>
    /// 异常编码ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("异常编码ViewModel")]
    public class AbnormalCodeViewModel : ViewModel
    {
        #region 预警配置 Alerter
        /// <summary>
        /// 预警配置Id
        /// </summary>
        [Label("预警配置")]
        public static readonly IRefIdProperty AlerterIdProperty =
            P<AbnormalCodeViewModel>.RegisterRefId(e => e.AlerterId, ReferenceType.Normal);

        /// <summary>
        /// 预警配置Id
        /// </summary>
        public double AlerterId
        {
            get { return (double)this.GetRefId(AlerterIdProperty); }
            set { this.SetRefId(AlerterIdProperty, value); }
        }

        /// <summary>
        /// 预警配置
        /// </summary>
        public static readonly RefEntityProperty<Alerter> AlerterProperty =
            P<AbnormalCodeViewModel>.RegisterRef(e => e.Alerter, AlerterIdProperty);

        /// <summary>
        /// 预警配置
        /// </summary>
        public Alerter Alerter
        {
            get { return this.GetRefEntity(AlerterProperty); }
            set { this.SetRefEntity(AlerterProperty, value); }
        }
        #endregion

        #region 预警名称 AlerterName
        /// <summary>
        /// 预警名称
        /// </summary>
        [Label("预警名称")]
        public static readonly Property<string> AlerterNameProperty = P<AbnormalCodeViewModel>.RegisterView(e => e.AlerterName, p => p.Alerter.Name);

        /// <summary>
        /// 预警名称
        /// </summary>
        public string AlerterName
        {
            get { return this.GetProperty(AlerterNameProperty); }
        }
        #endregion


        #region 异常编码 Code
        /// <summary>
        /// 异常编码
        /// </summary>
        [Label("异常编码")]
        public static readonly Property<string> CodeProperty = P<AbnormalCodeViewModel>.Register(e => e.Code);

        /// <summary>
        /// 异常编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 异常编码编辑器视图配置
    /// </summary>
    public class AbnormalCodeViewModelViewConfig : WebViewConfig<AbnormalCodeViewModel>
    {
        /// <summary>
        /// 明细视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.Property(p => p.AlerterId).UsePagingLookUpEditor((m, e) =>
            {
                Dictionary<string, string> keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.AlerterName), nameof(e.Alerter.Name));
                m.DicLinkField = keyValues;
            }).ShowInDetail(hideLabel: true).Show();
            View.Property(p => p.AlerterName).Show(ShowInWhere.Hide);
            View.Property(p => p.Code).ShowInDetail(hideLabel: true).Show();
        }
    }
}
