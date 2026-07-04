using SIE.Common.ImportHelper;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.Core.Import
{
    /// <summary>
    /// 主从孙结构导入 业务要继承这个接口
    /// </summary>
    public interface IMasterSubordinateDynamicBusinessImport
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
        /// 动态列的从表
        /// </summary>
        List<string> SubordinateDynamicList { get; set; }

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
        /// 动态列的孙表
        /// </summary>
        List<string> GrandsonDynamicList { get; set; }

        /// <summary>
        /// 导入孙表模板的列头名(key=SheetName)
        /// </summary>
        Dictionary<string, List<string>> GrandsonColumnNameDic { get; set; }

        /// <summary>
        /// 导入孙表列的标准验证 (列名 列对应验证) (key=SheetName)
        /// </summary>
        Dictionary<string, Dictionary<string, ValidColumn>> GrandsonColumnValidDic { get; set; }

        /// <summary>
        ///  孙表关联从表的列名（tuple是item1是从表SheetName，item2是关联字段）
        /// </summary>
        Dictionary<string, Tuple<string, string>> AssociationSubordinateColumnName { get; set; }

        /// <summary>
        /// 创建列验证
        /// </summary>
        /// <returns></returns>
        IMasterSubordinateDynamicBusinessImport CreateColumnValid();
        /// <summary>
        /// 业务处理
        /// </summary>
        /// <param name="correctDataRowDic"></param>
        void ProcessBusinessDataHandle(Dictionary<string, DataRow[]> correctDataRowDic);
    }
}
