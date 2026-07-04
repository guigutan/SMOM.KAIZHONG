using SIE.Common.ImportHelper;
using SIE.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Core.Import
{
    /// <summary>
    /// 主从结构导入控制器
    /// </summary>
    public class ImportMasterSubordinateController : DomainController
    {

        /// <summary>
        /// 处理业务逻辑数据
        /// </summary>
        /// <param name="import">需要处理的数据</param>
        /// <param name="type">导入处理逻辑类型</param>
        /// <returns>返回需要处理的逻辑数据</returns>
        public virtual DataTable ProcessImportData(MasterSubordinateImportData import, Type type)
        {
            try
            {
                //1.创建具体业务的导入处理类
                IMasterSubordinateBusinessImport businessImport = AppRuntime.Service.Resolve(type) as IMasterSubordinateBusinessImport;
                businessImport = businessImport.CreateColumnValid();
                //2.导入
                return ProcessBusinessData(import, businessImport);
            }
            catch (Exception ex)
            {
                LogManager.Logger.Debug(ex);
                throw new PlatformException("处理导入数据失败", ex);
            }
        }

        /// <summary>
        /// 处理导入数据逻辑
        /// </summary>
        /// <param name="import">excel文件数据</param>
        /// <param name="businessImport">处理业务数据的对象</param>
        /// <returns>返回需要处理的逻辑数据</returns>
        private DataTable ProcessBusinessData(MasterSubordinateImportData import, IMasterSubordinateBusinessImport businessImport)
        {
            const string sheetName = "Sheet";
            //1.创建返回的DataTable对象，要求从表和主表的字段叠加形成一个新的DataTable对象
            //用来展示错误或成功数据
            DataTable dt = CreateDT(import, sheetName);
            //2.主表的DataTable增加行号索引列和错误列
            import.MasterDataTable.Columns.Add(ImportDataHandle.MessageColumnName);
            import.MasterDataTable.Columns.Add(ImportDataHandle.RowIndex, typeof(int));
            import.MasterDataTable.Columns.Add(sheetName);
            import.MasterDataTable.AsEnumerable().ForEach(delegate (DataRow row)
            {
                row[ImportDataHandle.MessageColumnName] = string.Empty;
                row[ImportDataHandle.RowIndex] = import.MasterDataTable.Rows.IndexOf(row);
                row[sheetName] = import.MasterDataTable.TableName;
            });

            //3.从表的DataTable增加行号索引列和错误列
            foreach (var tempDt in import.SubordinateDataTables)
            {
                tempDt.Columns.Add(ImportDataHandle.MessageColumnName);
                tempDt.Columns.Add(ImportDataHandle.RowIndex, typeof(int));
                tempDt.Columns.Add(sheetName);
                tempDt.AsEnumerable().ForEach(delegate (DataRow row)
                {
                    row[ImportDataHandle.MessageColumnName] = string.Empty;
                    row[ImportDataHandle.RowIndex] = tempDt.Rows.IndexOf(row);
                    row[sheetName] = tempDt.TableName;
                });
            }

            //4.去除空白行
            Trim(import, businessImport);

            //5.验证每一行的数据,返回验证通过的数据
            Dictionary<string, DataRow[]> correctDataRowDic = ValidDataFormat(import, businessImport);

            //6.具体业务处理对象处理导入数据
            businessImport.ProcessBusinessDataHandle(correctDataRowDic);

            //7.把主表和从表的数据行添加到返回DataTable对象
            foreach (DataRow row in import.MasterDataTable.Rows)
            {
                DataRow newDr = dt.NewRow();
                foreach (DataColumn column in import.MasterDataTable.Columns)
                {
                    newDr[column.ColumnName] = row[column.ColumnName];
                }
                dt.Rows.Add(newDr);
            }

            foreach (var tempDt in import.SubordinateDataTables)
            {
                foreach (DataRow row in tempDt.Rows)
                {
                    DataRow newDr = dt.NewRow();
                    foreach (DataColumn column in tempDt.Columns)
                    {
                        newDr[column.ColumnName] = row[column.ColumnName];
                    }
                    dt.Rows.Add(newDr);
                }
            }

            return dt;
        }

        /// <summary>
        /// 创建DataTable对象，主表和从表列名叠加
        /// </summary>
        /// <param name="import"></param>
        /// <param name="sheetName"></param>
        /// <returns></returns>
        private DataTable CreateDT(MasterSubordinateImportData import,string sheetName)
        {

            DataTable dt = new DataTable();
            dt.Columns.Add(ImportDataHandle.MessageColumnName);
            dt.Columns.Add(ImportDataHandle.RowIndex, typeof(int));
            dt.Columns.Add(sheetName);
            foreach (DataColumn column in import.MasterDataTable.Columns)
            {
                dt.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
            }
            foreach (var tempDt in import.SubordinateDataTables)
            {
                foreach (DataColumn column in tempDt.Columns)
                {
                    if (!dt.Columns.Contains(column.ColumnName))
                        dt.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
                }
            }
            return dt;
        }


        /// <summary>
        /// 验证每一行数据内容
        /// </summary>
        /// <param name="import">需要验证的DataTable</param>
        /// <param name="businessImport">处理业务数据的对象</param>
        /// <returns>返回验证通过的行对象集合</returns>
        private Dictionary<string, DataRow[]> ValidDataFormat(MasterSubordinateImportData import, IMasterSubordinateBusinessImport businessImport)
        {
            //1.验证主表的每一行数据
            Dictionary<string, DataRow[]> correctDataRowDic = new Dictionary<string, DataRow[]>();
            correctDataRowDic[import.MasterDataTable.TableName] = import.MasterDataTable.AsEnumerable().Where(delegate (DataRow r)
            {
                string text = string.Empty;
                foreach (KeyValuePair<string, ValidColumn> columnValid in businessImport.MasterColumnValidList)
                {
                    string tip = string.Empty;

                    if (!columnValid.Value.OnValidation(r.Field<object>(columnValid.Key), out tip, r))
                    {
                        string text2 = columnValid.Key;
                        text = ((!text.IsNullOrEmpty()) ? (text + ";" + text2 + tip) : (text2 + tip));
                    }
                }
                if (!string.IsNullOrEmpty(text))
                {
                    r[ImportDataHandle.MessageColumnName] = text.Trim(new char[1] { ',' });
                    return false;
                }
                return true;
            }).ToArray();

            //2.验证从表的每一行数据
            foreach (var dt in import.SubordinateDataTables)
            {
                correctDataRowDic[dt.TableName] = ValidSubordinateDataRow(dt, import.AssociationColumnName, businessImport, import.MasterDataTable);
            }
            return correctDataRowDic;

        }

        private DataRow[] ValidSubordinateDataRow(DataTable dt, string associationColumnName, IMasterSubordinateBusinessImport businessImport, DataTable masterDataTable)
        {

            return dt.AsEnumerable().Where(delegate (DataRow r)
            {
                string text = string.Empty;

                //1.获取从表此行数据对应的主表数据
                var masterRow = masterDataTable.AsEnumerable().FirstOrDefault(c => c[associationColumnName].ToString() == r[associationColumnName].ToString());
                bool isMasterError = false;
                if (masterRow != null && masterRow[ImportDataHandle.MessageColumnName].ToString().IsNotEmpty())
                    isMasterError = true;

                //2.如果对应的主表数据验证不通过，从表标记（主表验证失败）
                if (isMasterError)
                    text = "主表验证失败".L10N();

                //3.验证从表数据行
                if (text.IsNullOrEmpty())
                    foreach (KeyValuePair<string, ValidColumn> columnValid in businessImport.SubordinateColumnValidDic[dt.TableName])
                    {
                        string tip = string.Empty;

                        if (!columnValid.Value.OnValidation(r.Field<object>(columnValid.Key), out tip, r))
                        {
                            string text2 = columnValid.Key;
                            text = ((!text.IsNullOrEmpty()) ? (text + ";" + text2 + tip) : (text2 + tip));
                        }
                    }
                if (!string.IsNullOrEmpty(text))
                {
                    r[ImportDataHandle.MessageColumnName] = text.Trim(new char[1] { ',' });
                    return false;
                }
                return true;
            }).ToArray();
        }



        /// <summary>
        /// 去除空白格
        /// </summary>
        /// <param name="import">需要验证的DataTable</param>
        /// <param name="businessImport">处理业务数据的对象</param>
        private void Trim(MasterSubordinateImportData import, IMasterSubordinateBusinessImport businessImport)
        {
            List<KeyValuePair<string, ValidColumn>> trimMasterColList = businessImport.MasterColumnValidList.Where((KeyValuePair<string, ValidColumn> p) => p.Value.IsTrim).ToList();
            Trim(import.MasterDataTable, trimMasterColList, businessImport.MasterColumnNameList);

            foreach (var dt in import.SubordinateDataTables)
            {
                List<KeyValuePair<string, ValidColumn>> trimSubordinateColumnValidList = businessImport.SubordinateColumnValidDic[dt.TableName].Where((KeyValuePair<string, ValidColumn> p) => p.Value.IsTrim).ToList();
                Trim(dt, trimSubordinateColumnValidList, businessImport.SubordinateColumnNameDic[dt.TableName]);
            }
        }


        /// <summary>
        /// 去除空白格
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="columnValidList"></param>
        /// <param name="columnNameList"></param>
        private void Trim(DataTable dt, List<KeyValuePair<string, ValidColumn>> columnValidList, List<string> columnNameList)
        {
            foreach (DataRow row in dt.Rows)
            {
                foreach (var item in columnValidList.Select(c => c.Key))
                {
                    int num = columnNameList.IndexOf(item);
                    if (num == -1)
                    {
                        throw new SIE.Domain.Validation.ValidationException("columnNameList中无法找到字段[{0}]".L10nFormat(item));
                    }
                    row[num] = row[num].ToString().Trim();
                }
            }
        }
    }
}
