using SIE.Warehouses;

namespace SIE.Wpf.Warehouses.ViewBehaviors
{
    /// <summary>
    /// 库区所属范围变更
    /// </summary>
    public class StorageAreaChangeBehavior : ViewBehavior
    {
        /// <summary>
        /// 附加
        /// </summary>
        protected override void OnAttach()
        {
            var view = View as ListLogicalView;
            if (view != null)
            {
                view.CurrentChanged -= StorageArea_CurrentChanged;
                view.CurrentChanged += StorageArea_CurrentChanged;
            }
        }

        /// <summary>
        /// 当前库区对象变更
        /// </summary>
        /// <param name="sender">当前变更的视图对象</param>
        /// <param name="e">事件参数</param>
        private void StorageArea_CurrentChanged(object sender, System.EventArgs e)
        {
            ListLogicalView logicalView = sender as ListLogicalView;
            StorageArea area = logicalView.Current as StorageArea;
            if (area != null)
            {
                area.PropertyChanged -= Area_PropertyChanged;
                area.PropertyChanged += Area_PropertyChanged;
            }
        }

        /// <summary>
        /// 值变更
        /// </summary>
        /// <param name="sender">变更的对象</param>
        /// <param name="e">事件参数</param>
        private void Area_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == StorageArea.LibraryTypeProperty.Name)
            {
                StorageArea area = sender as StorageArea;
                area.WarehouseId = 0;
                area.Warehouse = null;
            }
        }
    }
}
