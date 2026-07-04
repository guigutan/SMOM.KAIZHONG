using SIE.WorkBenchCommon.Workbench.Chatting;

namespace SIE.Wpf.WorkBenchCommon.Workbench.Chatting
{
    /// <summary>
    /// 聊天消息视图配置
    /// </summary>
    internal class ChatRecordViewConfig : WPFViewConfig<ChatRecord>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Content);
            View.Property(p => p.SendDate);
            View.Property(p => p.ReciveDate);
            View.Property(p => p.From);
            View.Property(p => p.To);
        }

        protected override void ConfigQueryView()
        {
            View.Property(p => p.SendDate).UseDateRangeEditor();
            View.Property(p => p.ReciveDate).UseDateRangeEditor();
            View.Property(p => p.From);
            View.Property(p => p.To);
        }
    }
}
