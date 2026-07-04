using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Warehouses;
using System;
using System.Linq;

namespace SIE.Wpf.Warehouses.ViewBehaviors
{
    /// <summary>
    /// 专储物料行为
    /// </summary>
    public class SpecialItemBehavior : ViewBehavior
    {
        /// <summary>
        /// 是否正在修改
        /// </summary>
        private bool isChanging;

        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttach()
        {
            if (View.EntityType == typeof(StorageLocationLayinInfo))
            {
                var view = View as DetailLogicalView;
                if (view != null)
                {
                    view.CurrentChanged -= LayinInfoView_CurrentChanged;
                    view.CurrentChanged += LayinInfoView_CurrentChanged;
                }
            }
            else if (View.EntityType == typeof(StorageLocation))
            {
                var view = View as ListLogicalView;
                if (view != null)
                {
                    view.CurrentChanged -= StorageLocation_CurrentChanged;
                    view.CurrentChanged += StorageLocation_CurrentChanged;
                }
            }
        }

        /// <summary>
        /// 当前库位对象变更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StorageLocation_CurrentChanged(object sender, System.EventArgs e)
        {
            ListLogicalView logicalView = sender as ListLogicalView;
            StorageLocation location = logicalView.Current as StorageLocation;
            if (location != null)
            {
                LogicalView layInInfoLogicalview = View.ChildrenViews.FirstOrDefault(p => p.EntityType == typeof(StorageLocationLayinInfo));
                if (layInInfoLogicalview != null && !layInInfoLogicalview.IsActive)
                {
                    StorageLocationLayinInfo layInInfo = StorageLocationDetailProperty.GetLayinInfo(location);
                    if (layInInfo == null)
                    {
                        PersistenceStatus oldStatus = location.PersistenceStatus;
                        var storageLocationInfo = RT.Service.Resolve<WarehouseController>().GetStorageLocationLayinInfo(location.Id);
                        layInInfo = storageLocationInfo == null ? new StorageLocationLayinInfo() { StorageLocation = location, StorageLocationId = location.Id } : storageLocationInfo;
                        StorageLocationDetailProperty.SetLayinInfo(location, layInInfo);
                        layInInfo.PropertyChanged += (s1, e1) =>
                        {
                            if (location.PersistenceStatus == PersistenceStatus.Unchanged)
                            {
                                location.PersistenceStatus = PersistenceStatus.Modified;
                            }
                        };
                        location.PersistenceStatus = oldStatus;
                    }

                    LogicalView logicalview = View.ChildrenViews.FirstOrDefault(p => p.EntityType == typeof(StorageLocationItemList));
                    if (logicalview != null)
                    {
                        logicalview.IsVisible = layInInfo.IsSpecialItem;
                    }
                }
            }
        }

        #region 仓储资料
        /// <summary>
        /// 当前仓储资料对象变更
        /// </summary>
        /// <param name="sender">当前变更的视图对象</param>
        /// <param name="e">事件参数</param>
        private void LayinInfoView_CurrentChanged(object sender, System.EventArgs e)
        {
            DetailLogicalView logicalView = sender as DetailLogicalView;
            StorageLocationLayinInfo layinInfo = logicalView.Current as StorageLocationLayinInfo;
            if (layinInfo != null)
            {
                layinInfo.PropertyChanged -= LayinInfo_PropertyChanged;
                layinInfo.PropertyChanged += LayinInfo_PropertyChanged;
                LogicalView logicalview = View.Parent.ChildrenViews.FirstOrDefault(p => p.EntityType == typeof(StorageLocationItemList));
                if (logicalview != null)
                {
                    logicalview.IsVisible = layinInfo.IsSpecialItem;
                }
            }
        }

        /// <summary>
        /// 值变更
        /// </summary>
        /// <param name="sender">变更的对象</param>
        /// <param name="e">事件参数</param>
        private void LayinInfo_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (!isChanging)
            {
                try
                {
                    isChanging = true;
                    if (e.PropertyName == StorageLocationLayinInfo.IsSpecialItemProperty.Name)
                    {
                        StorageLocationLayinInfo layinInfo = sender as StorageLocationLayinInfo;
                        if (layinInfo.IsSpecialItem)
                        {
                            if (CRT.MessageService.AskQuestion("库位能放置的物料只能是专储物料清单内的物料！点击是继续操作，点击否取消该操作。".L10N()))
                            {
                                LogicalView logicalview = View.Parent.ChildrenViews.FirstOrDefault(p => p.EntityType == typeof(StorageLocationItemList));
                                if (logicalview != null)
                                {
                                    logicalview.IsVisible = true;
                                }
                            }
                            else layinInfo.IsSpecialItem = false;
                        }
                        else
                        {
                            if (CRT.MessageService.AskQuestion("专储物料清单内的物料将被清除！(谨慎操作)点击是继续操作，点击否取消该操作。".L10N()))
                            {
                                LogicalView logicalview = View.Parent.ChildrenViews.FirstOrDefault(p => p.EntityType == typeof(StorageLocationItemList));
                                if (logicalview != null)
                                {
                                    logicalview.IsVisible = false;
                                    StorageLocation parentData = View.Parent.Current as StorageLocation;
                                    if (parentData != null)
                                    {
                                        parentData.StorageLocationItemListList.Clear();
                                    }
                                }
                            }
                            else layinInfo.IsSpecialItem = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new ValidationException(ex.Message);
                }
                finally
                {
                    isChanging = false;
                }
            }
        }
        #endregion
    }
}
