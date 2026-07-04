using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Core.Import
{
    /// <summary>
    /// 主从结构导入结构
    /// </summary>
    [Serializable]
    public class MasterSubordinateImportData
    {

        /// <summary>
        /// 主表SheetName
        /// </summary>
        public string MasterSheetName { get; set; }

        /// <summary>
        /// 从表SheetName集合
        /// </summary>

        public List<string> SubordinateSheetNameList { get; set; }

        /// <summary>
        /// 主表DataTable
        /// </summary>
        public DataTable MasterDataTable { get; set; }

        /// <summary>
        /// 从表DataTable列表，TableName=SheetName
        /// </summary>
        public List<DataTable> SubordinateDataTables { get; set; }

        /// <summary>
        /// 从表关联主表的列名
        /// </summary>
        public string AssociationColumnName { get; set; }

    }
}
