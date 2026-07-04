using SIE.Common.ImportHelper;
using SIE.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Core.Import
{
    /// <summary>
    ///主从结构 导入处理逻辑类
    /// </summary>
    public class ImportMasterSubordinateDynamicDataHandle : ImportDataHandle
    {

        /// <summary>
        /// 正确数据集
        /// </summary>
        public DataRow[] DrSuccessArray { get; private set; }

        /// <summary>
        /// 错误数据集
        /// </summary>
        public DataRow[] DrFailedListArray { get; private set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="import"></param>
        /// <param name="type"></param>
        /// <param name="callImportCompleted"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string ImportProcess(MasterSubordinateDynamicImportData import, Type type, ImportCompleted callImportCompleted, string fileName = "")
        {
            LogManager.Logger.Error("============================== ImportProcess 开始============================".L10N());
            string strMsg = string.Empty;
            string text = (fileName.IsNullOrEmpty() ? string.Empty : "【{0}】".FormatArgs(fileName));
            try
            {
                //1.导入文件是否符合要求
                if (CheckImportFile(import, type, callImportCompleted, ref strMsg))
                {
                    LogManager.Logger.Error("============================== flag begin============================".L10N());
                    //2.清除空白行
                    RemoveEmptyData(import);
                    //3.导入
                    DataTable dt = AppRuntime.Service.Resolve<ImportMasterSubordinateDynamicController>().ProcessImportData(import, type);

                    //4.主表导入成功与失败
                    DrFailedListArray = (from r in dt.AsEnumerable()
                                         where !string.IsNullOrEmpty(r[MessageColumnName].ToString())
                                         select r).ToArray();
                    DrSuccessArray = (from r in dt.AsEnumerable()
                                      where string.IsNullOrEmpty(r[MessageColumnName].ToString())
                                      select r).ToArray();

                    callImportCompleted?.Invoke(DrSuccessArray, DrFailedListArray);
                    strMsg = $"文件{text}导入完成！".L10N();
                    LogManager.Logger.Error("============================== flag end============================".L10N());
                    return strMsg;
                }
                return strMsg;
            }
            catch (Exception ex)
            {
                strMsg = "错误异常:".L10N() + ex.Message;
                if (ex.InnerException != null)
                {
                    strMsg = "错误异常:".L10N() + ex.InnerException?.Message;
                }
                LogManager.Logger.Error("============================== ImportProcess 异常".L10N(), ex);
                return strMsg;
            }
        }


        /// <summary>
        /// 清除空白行
        /// </summary>
        /// <param name="import">需要清除空白行的数据集</param>
        private void RemoveEmptyData(MasterSubordinateDynamicImportData import)
        {
            List<DataTable> dts = new List<DataTable>()
            {
                import.MasterDataTable
            };
            dts.AddRange(import.SubordinateDataTables);

            foreach (var dt in dts)
            {
                DataRow[] array = dt.AsEnumerable().Where(delegate (DataRow p)
                {
                    bool flag = true;
                    foreach (DataColumn column in dt.Columns)
                    {
                        if (p[column] != null && !string.IsNullOrEmpty(p[column].ToString().Trim()))
                        {
                            flag = false;
                        }
                        if (!flag)
                        {
                            return flag;
                        }
                    }
                    return flag;
                }).ToArray();
                for (int i = 0; i < array.Length; i++)
                {
                    dt.Rows.Remove(array[i]);
                }
            }
        }

        /// <summary>
        /// 检验导入的文件是否符合要求
        /// </summary>
        /// <param name="import">导入的数据</param>
        /// <param name="type">业务导入对象类型</param>
        /// <param name="callImportCompleted">导入完成回调</param>
        /// <param name="strMsg">检验的结果</param>
        /// <returns></returns>
        private bool CheckImportFile(MasterSubordinateDynamicImportData import, Type type, ImportCompleted callImportCompleted, ref string strMsg)
        {
            //1.创建具体业务的导入处理类
            IMasterSubordinateDynamicBusinessImport businessImport = AppRuntime.Service.Resolve(type) as IMasterSubordinateDynamicBusinessImport;
            bool flag = true;
            if (!typeof(IMasterSubordinateDynamicBusinessImport).IsAssignableFrom(type))
            {
                strMsg = "请检查传入的业务导入对象类型，必须继承IMasterSubordinateDynamicBusinessImport";
                flag = false;
                return flag;
            }
            Action action = delegate
            {
                callImportCompleted(new DataRow[0], new DataRow[0]);
            };

            //2.检验主表的数据是否符合要求
            flag = CheckImportDataTable(import.MasterDataTable, action, businessImport.MasterColumnNameList, string.Empty, businessImport.SubordinateDynamicList, ref strMsg);

            //3.检验从表的数据是否符合要求
            foreach (var table in import.SubordinateDataTables)
            {
                flag = CheckImportDataTable(table, action, businessImport.SubordinateColumnNameDic[table.TableName], table.TableName, businessImport.SubordinateDynamicList, ref strMsg);
                if (!flag)
                {
                    action();
                    break;
                }
            }

            //4.检验孙表的数据是否符合要求
            foreach (var table in import.GrandsonDataTables)
            {
                flag = CheckImportDataTable(table, action, businessImport.GrandsonColumnNameDic[table.TableName], table.TableName, businessImport.GrandsonDynamicList, ref strMsg);
                if (!flag)
                {
                    action();
                    break;
                }
            }
            return flag;
        }

        private bool CheckImportDataTable(DataTable dt, Action action, List<string> columnList, string tableName, List<string> subordinateDynamicList, ref string strMsg)
        {
            bool flag = true;

            if (dt == null)
            {
                return false;
            }

            if (dt.Rows.Count == 0)
            {
                strMsg = string.Format("导入的文件中Sheet[{0}]内容为空".L10N(), dt.TableName);
                action();
                flag = false;
            }
            //动态列子表不验证列头
            if (tableName.IsNotEmpty() && subordinateDynamicList.IsNotEmpty() && subordinateDynamicList.Exists(c => c == tableName))
                return flag;

            //验证数据
            if (flag && !ValidColumnNames(dt, columnList))
            {
                strMsg = string.Format("导入的文件中Sheet[{0}]列头不符合要求".L10N(), dt.TableName);
                action();
                flag = false;
            }
            return flag;
        }
    }
}
