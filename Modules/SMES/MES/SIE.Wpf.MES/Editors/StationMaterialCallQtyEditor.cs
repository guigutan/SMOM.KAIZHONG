using SIE.MES.LoadItems;
using System.Windows;

namespace SIE.Wpf.MES.Editors
{
    /// <summary>
    /// 工位叫料数量编辑器
    /// </summary>
    public class StationMaterialCallQtyEditor : EditNumCalculateEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public new const string EditorName = "StationMaterialCallQtyEditor";

        /// <summary>
        /// 最大数量命令
        /// </summary>
        /// <param name="sender">对象</param>
        /// <param name="e">参数</param>
        protected override void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            var stationMvml = Source as StationMateriaViewModel;
            if (stationMvml == null || this.Control.IsReadOnly) return;

            if (this.Control.Value == stationMvml.Capacity)
                this.Control.Value = 0;
            else
                this.Control.Value = stationMvml.Capacity;
        }
    }
}
