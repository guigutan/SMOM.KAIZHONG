using SIE.Common.ImportHelper;
using System.Collections.Generic;
using System.Data;

namespace SIE.Core.Import
{
    /// <summary>
    /// 主从结构导入 业务要继承这个接口
    /// </summary>
    public interface IMasterSubordinateBusinessImport
    {

        /// <summary>
        /// 导入主表模板的列头名
        /// </summary>
        List<string> MasterColumnNameList { get; set; }

        /// <summary>
        /// 主表列的标准验证 (列名 列对应验证 )
        /// </summary>
        Dictionary<string, ValidColumn> MasterColumnValidList { get; set; }

        /// <summary>
        /// 导入从表模板的列头名(key=SheetName)
        /// </summary>
        Dictionary<string, List<string>> SubordinateColumnNameDic { get; set; }

        /// <summary>
        /// 导入从表列的标准验证 (列名 列对应验证) (key=SheetName)
        /// </summary>
        Dictionary<string, Dictionary<string, ValidColumn>> SubordinateColumnValidDic { get; set; }

        /// <summary>
        /// 从表关联主表的列名
        /// </summary>
        string AssociationColumnName { get; set; }

        /// <summary>
        /// 创建列验证
        /// </summary>
        /// <returns></returns>
        IMasterSubordinateBusinessImport CreateColumnValid();
        /// <summary>
        /// 业务处理
        /// </summary>
        /// <param name="correctDataRowDic"></param>
        void ProcessBusinessDataHandle(Dictionary<string, DataRow[]> correctDataRowDic);
    }
}
