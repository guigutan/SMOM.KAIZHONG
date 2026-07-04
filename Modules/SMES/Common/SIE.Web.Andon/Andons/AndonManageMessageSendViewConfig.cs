using SIE.Andon.Andons;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯管理消息推送视图配置
    /// </summary>
    public class AndonManageMessageSendViewConfig :WebViewConfig<AndonManageMessageSend>
    {
        /// <summary>
        /// 查看视图
        /// </summary>
        public const string LookUpViewGroup = "LookUpViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(LookUpViewGroup);
            if (ViewGroup == LookUpViewGroup)
            {
                LookUpEventView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.DisableEditing();
            using (View.OrderProperties())
            {
                View.Property(p => p.MessageSendTime).ShowInList(width: 150).Readonly();
                View.Property(p => p.AbnormalTime).Show().Readonly();
                View.Property(p => p.WaitinglTime).Show().Readonly();
                View.Property(p => p.MessageSendTemplate).ShowInList(width: 200).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 查看视图
        /// </summary>
        protected void LookUpEventView()
        {
            using (View.OrderProperties())
            {
                View.Property(p => p.MessageSendTime).ShowInList(width: 150).Readonly();
                View.Property(p => p.AbnormalTime).Show().Readonly();
                View.Property(p => p.WaitinglTime).Show().Readonly();
                View.Property(p => p.MessageSendTemplate).ShowInList(width: 200).Readonly();
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}
