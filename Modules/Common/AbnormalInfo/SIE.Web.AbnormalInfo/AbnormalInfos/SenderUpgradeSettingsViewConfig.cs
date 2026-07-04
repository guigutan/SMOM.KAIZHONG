using SIE.Domain;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.ManagedProperty;
using SIE.MetaModel.View;
using System.Linq;

namespace SIE.Web.AbnormalInfo.AbnormalInfoss
{
    /// <summary>
    /// 推送升级设置 视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class SenderUpgradeSettingsViewConfig: WebViewConfig<SenderUpgradeSettings>
    {
        #region 接收人 ReceiverValue
        /// <summary>
        /// 接收人
        /// </summary>
        public static readonly Property<string> ReceiverValueProperty = P<SenderUpgradeSettings>.RegisterExtensionReadOnly("ReceiverValue", typeof(SenderUpgradeSettingsViewConfig),
            GetReceiverValue, SenderUpgradeSettings.PusherProperty);

        /// <summary>
        /// 接收人
        /// </summary>
        /// <param name="me">实体</param>
        /// <returns>string</returns>
        public static string GetReceiverValue(Entity me)
        {
            var current = me as SenderUpgradeSettings;
            var pusher = current.Pusher;
            if (pusher == null) return "";
            return string.Join(";", pusher.ReceiverList.Select(p => p.ReceiverName));
        }
        #endregion

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Delete);
            View.Property(p => p.ConditionType).DefaultValue("大于").Readonly();
            View.Property(p => p.TimeType).UseSpinEditor(p => p.AllowDecimals = false);
            View.Property(p => p.UnitType);
            View.Property(p => p.PusherId);
            View.Property(ReceiverValueProperty).HasLabel("接收人");
        }
    }
}
