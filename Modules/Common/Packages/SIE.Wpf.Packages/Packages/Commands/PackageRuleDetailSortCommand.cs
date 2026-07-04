using SIE.Common.Sort;
using SIE.Domain;
using SIE.Packages;
using SIE.Wpf.Common.Sort;
using System.Linq;

namespace SIE.Wpf.Packages.Packages.Commands
{
    /// <summary>
    /// 上移命令
    /// 主单位禁用上移按钮，主单位下的第一个单位禁用上移按钮
    /// </summary>
    [Command(ImageName = "ArrowLongUp", Label = "上移", ToolTip = "当前行数据上移", GroupType = CommandGroupType.Business)]
    public class PackageRuleDetailMoveUpCommand : MoveUpCommand
    {
        /// <summary>
        /// 上移按钮是否可以执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>是否可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            bool curCanExecuteFlag = false;
            var entitys = view.Data;
            var current = view.Current;
            if (entitys != null && current != null)
            {
                var curEntity = current as PackageRuleDetail;
                var orderEntitys = entitys.OfType<Entity>().OrderBy(f => SortExtension.GetIndex(f)).ToList();
                var curIndex = orderEntitys.IndexOf(curEntity);
                curCanExecuteFlag = (curEntity.PackageUnit?.IsMasterUnit == false) && curIndex > 1;
            }

            return base.CanExecute(view) && curCanExecuteFlag;
        }
    }

    /// <summary>
    /// 下移命令
    /// 主单位禁用上移按钮，主单位下的最后一个单位禁用上移按钮
    /// </summary>
    [Command(ImageName = "ArrowLongDown", Label = "下移", ToolTip = "当前行数据下移", GroupType = CommandGroupType.Business)]
    public class PackageRuleDetailMoveDownCommand : MoveDownCommand
    {
        /// <summary>
        /// 下移按钮是否可以执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>是否可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            bool curCanExecuteFlag = false;
            var entitys = view.Data;
            var current = view.Current;
            if (entitys != null && current != null)
            {
                var curEntity = current as PackageRuleDetail;
                var orderEntitys = entitys.OfType<Entity>().OrderBy(f => SortExtension.GetIndex(f)).ToList();
                var curIndex = orderEntitys.IndexOf(curEntity);
                curCanExecuteFlag = (curEntity.PackageUnit?.IsMasterUnit == false) && curIndex + 1 != orderEntitys.Count;
            }

            return base.CanExecute(view) && curCanExecuteFlag;
        }
    }

    /// <summary>
    /// 置顶命令
    /// 主单位且主单位不是第一个则允许使用置顶命令
    /// 其它单位禁用该按钮
    /// </summary>
    [Command(ImageName = "AlignTop", Label = "置顶", ToolTip = "当前行数据移到顶部", GroupType = CommandGroupType.Business)]
    public class PackageRuleDetailMoveTopCommand : MoveTopCommand
    {
        /// <summary>
        /// 置顶命令是否可以执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>是否可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            bool curCanExecuteFlag = false;
            var entitys = view.Data;
            var current = view.Current;
            if (entitys != null && current != null)
            {
                var curEntity = current as PackageRuleDetail;
                var orderEntitys = entitys.OfType<Entity>().OrderBy(f => SortExtension.GetIndex(f)).ToList();
                var curIndex = orderEntitys.IndexOf(curEntity);
                curCanExecuteFlag = (curEntity.PackageUnit?.IsMasterUnit == true) && curIndex > 0;
            }

            return base.CanExecute(view) && curCanExecuteFlag;
        }
    }

    /// <summary>
    /// 置底命令
    /// 主单位禁用该命令按钮，非主单位允许使用
    /// </summary>
    [Command(ImageName = "AlignBottom", Label = "置底", ToolTip = "当前行数据移到底部", GroupType = CommandGroupType.Business)]
    public class PackageRuleDetailMoveBottomCommand : MoveBottomCommand
    {
        /// <summary>
        /// 置底命令是否可以执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>是否可以执行</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            bool curCanExecuteFlag = false;
            var entitys = view.Data;
            var current = view.Current;
            if (entitys != null && current != null)
            {
                var curEntity = current as PackageRuleDetail;
                curCanExecuteFlag = (curEntity.PackageUnit?.IsMasterUnit != true);
            }

            return base.CanExecute(view) && curCanExecuteFlag;
        }
    }
}
