using SIE.Andon.Andons;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 
    /// </summary>
    internal class AndonManageOperateLogViewConfig : WPFViewConfig<AndonManageOperateLog>
    {
        /// <summary>
        /// 单个字符宽度
        /// </summary>
        private static readonly int SingleCharWidth = 20;

        /// <summary>
        /// 取消视图
        /// </summary>
        public const string CancelViewGroup = "CancelViewGroup";


        /// <summary>
        /// 驳回视图
        /// </summary>
        public const string RejectViewGroup = "RejectViewGroup";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {

            View.DeclareExtendViewGroup(new string[]
            {
                RejectViewGroup, CancelViewGroup
            });

            if (ViewGroup == RejectViewGroup)
            {
                ConfigRejectViewGroup();
            }
            else if (ViewGroup == CancelViewGroup)
            {
                ConfigCancelView();
            }
        }

        /// <summary>
        /// 驳回视图配置
        /// </summary>        
        private void ConfigRejectViewGroup()
        {
            View.ClearCommands();
            View.UseDetail(columnCount: 1);
            View.Property(p => p.Remark).HasLabel("驳回原因").ShowInDetail();
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.OperateTime).Readonly().ShowInList(gridWidth: SingleCharWidth * 10);
            View.Property(p => p.OperateType).Readonly().ShowInList(gridWidth: SingleCharWidth * 5);
            View.Property(p => p.OperaterId).Readonly().ShowInList(gridWidth: SingleCharWidth * 4);
            View.Property(p => p.Remark).Readonly().ShowInList(gridWidth: SingleCharWidth * 10);
            View.Property(p => p.LastOperate).Readonly().ShowInList(gridWidth: SingleCharWidth * 12);
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        private void ConfigCancelView()
        {
            View.ClearCommands();

            View.UseDetail(columnCount: 1);

            View.Property(p => p.Remark).HasLabel("取消原因").ShowInDetail();
        }
    }
}
