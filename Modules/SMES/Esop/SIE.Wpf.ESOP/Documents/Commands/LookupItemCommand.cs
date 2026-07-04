using SIE.Domain;
using SIE.ESop.Documents;
using SIE.MetaModel.View;
using SIE.View.Workbench;
using SIE.Wpf.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Wpf.ESop.Documents.Commands
{
    /// <summary>
    /// 适用物料的选择命令
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = 10, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class LookupItemCommand : LookupCommand
    {
        /// <summary>
        /// 命令是否能够执行
        /// </summary>
        /// <param name="view">逻辑视图</param>
        /// <returns>
        /// 返回是否可执行
        /// </returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return (view.Parent.Current != null) && view.Parent.Current.PersistenceStatus != PersistenceStatus.New;
        }
        /// <summary>
        /// 此处增加了多语言
        /// </summary>
        /// <param name="ui"></param>
        protected override void ShowDialog(ControlResult ui)
        {
            if (ClientRuntime.Workbench.ShowDialog(ui, delegate (IDialogContent w)
            {
                w.Title = "选择{0}".L10nFormat(ui.MainView.Meta.Label.L10N()); ;
            }) == 0)
            {
                OnAccept();
            }
            else
            {
                OnCancel();
            }
        }
        /// <summary>
        /// 确定
        /// </summary>
        protected override void OnAccept()
        {
            FilterItem();
            base.OnAccept();
            var parentView = View.Parent;
            if (parentView != null && parentView.DataLoader.AnyLoaded)
                parentView.DataLoader.LoadDataAsync();
        }

        /// <summary>
        /// 过滤物料
        /// </summary>
        protected void FilterItem()
        {
            var collection = View.GetParentView(typeof(DocumentCollection)).Current as DocumentCollection;
            var itemList = View.Data.OfType<DocumentCollectionItem>().Select(p => p.ItemId);
            var selectedEntities = SelectedView.Data.OfType<DocumentCollectionItem>();
            List<DocumentCollectionItem> collectionItems = new List<DocumentCollectionItem>();
            collectionItems.AddRange(selectedEntities);
            var itemIds = selectedEntities.Select(p => p.ItemId).ToArray();
            var dicRefItems = RT.Service.Resolve<DocumentCollectionController>().IsItemRefCollection(collection.Id, itemIds);
            foreach (DocumentCollectionItem collectionItem in collectionItems)
            {
                if (itemList.Contains(collectionItem.ItemId))   //已经选择过的过滤掉
                    continue;
                if (dicRefItems.ContainsKey(collectionItem.ItemId) && !dicRefItems[collectionItem.ItemId])
                    continue;
                if (!CRT.MessageService.AskQuestion("物料{0}已关联文档集,是否重新关联至此文档集".L10nFormat(collectionItem.Item.Code)))
                {
                    var result = selectedEntities.FirstOrDefault(p => p.ItemId == collectionItem.ItemId);
                    SelectedView.Data.Remove(result);
                }
            }
        }
    }

    /// <summary>
    /// 适用物料的选择命令
    /// </summary>
    [Command(ImageName = "PlaylistCheck", Label = "选择", ToolTip = "选择", GroupType = 10, DisplayMode = CommandDisplayMode.LabelAndIcon)]
    public class DetailLookupItemCommand : LookupItemCommand
    {
        /// <summary>
        /// 确定
        /// </summary>
        protected override void OnAccept()
        {
            FilterItem();
            var selectedEntities = SelectedView.Data.OfType<DocumentCollectionItem>();
            var itemList = View.Data.OfType<DocumentCollectionItem>().Select(p => p.ItemId);
            foreach (DocumentCollectionItem collectionItem in selectedEntities)
            {
                if (itemList.Contains(collectionItem.ItemId))
                    continue;
                View.Data.Add(collectionItem);
            }
        }
    }
}