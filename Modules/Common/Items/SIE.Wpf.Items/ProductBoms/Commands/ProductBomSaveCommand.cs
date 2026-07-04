using SIE.Wpf.Command;

namespace SIE.Wpf.Items.Commands
{
    /// <summary>
    /// 产品BOM保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", GroupType = CommandGroupType.Edit)]
    class ProductBomSaveCommand : ListSaveCommand
    {
        /// <summary>
        /// 是否可执行
        /// </summary>
        /// <param name="view">视图</param>
        /// <returns>bool</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Data.IsDirty;
        }

        protected override void OnSaved(ListLogicalView view)
        {
            if (view.DataLoader.AnyLoaded)
                view.DataLoader.ReloadDataAsync();
        }
    }
}
