using SIE.Domain;
using SIE.MES.Storages;
using SIE.Wpf.Command;
using SIE.Wpf.MES.Storages.Commands;
using System.Collections.Generic;

namespace SIE.Wpf.MES.Storages
{
    /// <summary>
    /// 产线货区货位视图配置
    /// </summary>
    internal class StorageLocationViewConfig : WPFViewConfig<StorageLocation>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal();
            View.UseDefaultCommands();
            View.RemoveCommands(WPFCommandNames.ListSave);
            View.ReplaceCommands(typeof(ListAddCommand), typeof(AddStorageLocationCommand));
            View.ReplaceCommands(typeof(ListEditCommand), typeof(EditStorageAreaExtCommand));
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Location);
            View.Property(p => p.IsCommon).Readonly();
            View.AssociateChildrenProperty(ItemStorageExtStorageLocation.ItemStorageExtListProperty, (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var storageLocation = args.Parent as StorageLocation;
                var itemStorage = RT.Service.Resolve<StorageController>().GetItemStorages(storageLocation.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return itemStorage;
            }).Show(ChildShowInWhere.All);
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Location);
            View.Property(p => p.IsCommon).Readonly();
        }
    }
}