using SIE.Utils;
using SIE.Warehouses;
using System.Collections.Generic;

namespace SIE.Wpf.Warehouses.Editors
{
    /// <summary>
    /// 存储湿度编辑器
    /// </summary>
    public class StorageHumidityEditor : StorageEditor
    {
        /// <summary>
        /// 存储湿度编辑器
        /// </summary>
        public const string EditorName = "StorageHumidityEditor";

        /// <summary>
        /// 创建下拉框的数据源
        /// </summary>
        /// <returns>返回下拉框的数据源</returns>
        protected override List<EnumViewModel> GetDataSource()
        {
            return EnumViewModel.GetByEnumType(typeof(HumidityType));
        }
    }
}
