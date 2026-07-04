using SIE.Andon.Andons;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 
    /// </summary>
    internal class AndonManageMessageSendViewConfig : WPFViewConfig<AndonManageMessageSend>
    {
        /// <summary>
        /// 单个字符宽度
        /// </summary>
        private static readonly int SingleCharWidth = 20;

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.MessageSendTime).Readonly().ShowInList(gridWidth: SingleCharWidth * 10);
            //View.Property(p => p.MessageSendPersonId).Readonly().ShowInList(gridWidth: SingleCharWidth * 4);
            View.Property(p => p.MessageSendTemplate).Readonly().ShowInList(gridWidth: SingleCharWidth * 20);
        }
    }
}
