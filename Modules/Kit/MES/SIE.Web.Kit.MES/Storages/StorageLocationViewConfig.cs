using SIE.Domain;
using SIE.Kit.MES.Storages;
using SIE.MetaModel.View;
using System.Collections.Generic;

namespace SIE.Web.Kit.MES.Storages
{
    /// <summary>
    /// 产线货区货位视图配置
    /// </summary>
    internal class StorageLocationViewConfig : WebViewConfig<StorageLocation>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            //方法重写
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseChildrenAsHorizontal();
            View.UseDefaultCommands().UseImportCommands();
            View.RemoveCommands(WebCommandNames.Save);
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.Kit.MES.Storages.Commands.AddStorageSafetyCommand");
            View.Property(p => p.Code).Readonly(p => p.PersistenceStatus != PersistenceStatus.New)
                .UseListSetting(e => { e.HelpInfo = "新增状态可编辑"; });
            View.Property(p => p.Name);
            View.Property(p => p.Location);
            View.Property(p => p.IsCommon).Readonly();
            View.Property(p => p.CreateByName);
            View.Property(p => p.CreateDate).ShowInList(150);
            View.Property(p => p.UpdateByName);
            View.Property(p => p.UpdateDate).ShowInList(150);
            View.AssociateChildrenProperty(ItemStorageExtStorageLocation.ItemStorageExtListProperty, (w) =>
            {
                var args = w as ChildPagingDataArgs;
                var storageLocation = args.Parent as StorageLocation;
                var itemStorage = RT.Service.Resolve<StorageController>().GetItemStorages(storageLocation.Id, (List<OrderInfo>)args.SortInfo, args.PagingInfo);
                return itemStorage;
            }).HasLabel("货位物料明细").Show(ChildShowInWhere.All);
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

        /// <summary>
        /// 配置导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            View.PropertyRef(p => p.StorageArea.Code).HasLabel("工位货区编码");
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Location);
            View.Property(p => p.IsCommon);
        }
    }
}