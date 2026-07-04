using SIE.Warehouses;

namespace SIE.Wpf.Warehouses.ViewBehaviors
{
    /// <summary>
    /// 库位所属范围变更
    /// </summary>
    public class StorageLocationChangeBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            if (view != null)
            {
                view.CurrentChanged -= StorageLocation_CurrentChanged;
                view.CurrentChanged += StorageLocation_CurrentChanged;
            }
        }

        /// <summary>
        /// 当前库位对象变更
        /// </summary>
        /// <param name="sender">当前变更的视图对象</param>
        /// <param name="e">事件参数</param>
        private void StorageLocation_CurrentChanged(object sender, System.EventArgs e)
        {
            ListLogicalView logicalView = sender as ListLogicalView;
            StorageLocation location = logicalView.Current as StorageLocation;
            if (location != null)
            {
                location.PropertyChanged -= Location_PropertyChanged;
                location.PropertyChanged += Location_PropertyChanged;
            }
        }

        /// <summary>
        /// 值变更
        /// </summary>
        /// <param name="sender">变更的对象</param>
        /// <param name="e">事件参数</param>
        private void Location_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == StorageLocation.LibraryTypeProperty.Name)
            {
                StorageLocation location = sender as StorageLocation;
                location.WarehouseId = 0;
                location.Warehouse = null;
            }
            else if (e.PropertyName == StorageLocation.WarehouseIdProperty.Name)
            {
                StorageLocation location = sender as StorageLocation;
                location.AreaId = 0;
                location.Area = null;
            }
        }
    }
}
