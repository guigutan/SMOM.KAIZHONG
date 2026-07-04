using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Core.Import
{
    /// <summary>
    /// 主从孙结构导入结构
    /// </summary>
    [Serializable]
    public class MasterSubordinateDynamicImportData
    {

        /// <summary>
        /// 主表SheetName
        /// </summary>
        public string MasterSheetName { get; set; }

        /// <summary>
        /// 从表SheetName集合
        /// </summary>

        public List<string> SubordinateSheetNameList { get; set; }= new List<string>();

        /// <summary>
        /// 孙表SheetName集合
        /// </summary>
        public List<string> GrandsonSheetNameList { get; set; } = new List<string>();

        /// <summary>
        /// 主表DataTable
        /// </summary>
        public DataTable MasterDataTable { get; set; }

        /// <summary>
        /// 从表DataTable列表，TableName=SheetName
        /// </summary>
        public List<DataTable> SubordinateDataTables { get; set; } = new List<DataTable>();

        /// <summary>
        /// 孙表DataTable列表，TableName=SheetName
        /// </summary>
        public List<DataTable> GrandsonDataTables { get; set; } = new List<DataTable>();

        /// <summary>
        /// 从表关联主表的列名
        /// </summary>
        public string AssociationColumnName { get; set; }

        /// <summary>
        /// 孙表关联从表的列名（tuple是item1是从表SheetName，item2是关联字段）
        /// </summary>
        public Dictionary<string, Tuple<string, string>> AssociationSubordinateColumnName { get; set; } = new Dictionary<string, Tuple<string, string>>();

    }
}
