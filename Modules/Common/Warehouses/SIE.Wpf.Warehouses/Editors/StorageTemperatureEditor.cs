using SIE.Utils;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.Wpf.Warehouses.Editors
{
    /// <summary>
    /// 存储温度编辑器
    /// </summary>
    public class StorageTemperatureEditor : StorageEditor
    {
        /// <summary>
        /// 存储温度编辑器
        /// </summary>
        public const string EditorName = "StorageTemperatureEditor";

        /// <summary>
        /// 创建下拉框的数据源
        /// </summary>
        /// <returns>返回下拉框的数据源</returns>
        protected override List<EnumViewModel> GetDataSource()
        {
            return EnumViewModel.GetByEnumType(typeof(TemperatureType));
        }
    }
}
