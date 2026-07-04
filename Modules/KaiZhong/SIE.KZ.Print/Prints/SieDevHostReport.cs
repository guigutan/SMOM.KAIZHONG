using DevExpress.XtraPrinting;
using DevExpress.XtraReports.Security;
using DevExpress.XtraReports.UI;
using SIE.Domain;
using SIE.KZ.Print;
using SIE.ManagedProperty;
using SIE.MetaModel;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace SIE.Common.Prints
{
    /// <summary>
    /// 
    /// </summary>
    public class SieDevHostReport : HostReport
    {
        /// <summary>
        /// 
        /// </summary>
        public override string Extension
        {
            get
            {
                return ".siedev";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        protected string FilePath { get; set; }

        private string GetDataSourceTypeName(Type typeName, string propertyName = null)
        {
            string text = typeName.Name;
            if (!propertyName.IsNullOrWhiteSpace())
            {
                text = "{0}_{1}".FormatArgs(new object[] { typeName.Name, propertyName });
            }
            return text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        /// <exception cref="SIE.Domain.Validation.ValidationException"></exception>
        public override string GenerateNewTemplate(IPrintable printable, string fileName)
        {
            string text = Path.Combine(base.GetPath(), base.GetFileFullName(fileName));
            IEnumerable<object> simpleDatas = printable.GetSimpleDatas();
            if (simpleDatas.Count<object>() <= 0)
            {
                throw new SIE.Domain.Validation.ValidationException("当前设计类型没有相关数据，请新建一些数据后重新设计");
            }
            Type type = simpleDatas.FirstOrDefault<object>().GetType();
            string text2 = XmlSerializeHelper.Serialize<XtraReportsLayoutSerializer>(this.CreateXtraReportsLayoutSerializer(printable, type));
            System.IO.File.WriteAllText(text, text2);
            return text;
        }


        private void GenerateQuerysAndRelatonsAndViews(IPrintable printable, Type dataType, ref List<Query> querys, ref List<Relation> relations, ref List<ResultView> views, int levelIndex = 0, string propertyName = null)
        {
            string dataSourceTypeName = this.GetDataSourceTypeName(dataType, propertyName);
            Columns columns = new Columns
            {
                ColumnList = new List<Column>()
            };
            ResultView resultView = new ResultView
            {
                Name = dataSourceTypeName,
                FieldList = new List<Field>()
            };
            foreach (string text in printable.GetPropertys(dataType))
            {
                columns.ColumnList.Add(new Column
                {
                    Table = dataSourceTypeName,
                    Name = text
                });
                resultView.FieldList.Add(new Field
                {
                    Name = text,
                    Type = "String"
                });
            }
            views.Add(resultView);
            Query query = new Query
            {
                Type = "SelectQuery",
                Name = dataSourceTypeName,
                Tables = new Tables
                {
                    TableList = new List<Table>
                    {
                        new Table
                        {
                            Name = dataSourceTypeName
                        }
                    }
                },
                Columns = columns
            };
            querys.Add(query);
            if (!(printable is IBillPrintable) || levelIndex > 0)
            {
                return;
            }
            IBillPrintable billPrintable = printable as IBillPrintable;
            if (dataType.IsSubclassOf(typeof(DataEntity)))
            {
                IEnumerable<IListProperty> listManagedPropertys = AppRuntime.Service.Resolve<PrintableController>().GetListManagedPropertys(dataType);
                using (IEnumerator<string> enumerator = billPrintable.GetListPropertys(dataType).GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        string listPropertyName = enumerator.Current;
                        IListProperty listProperty = listManagedPropertys.FirstOrDefault((IListProperty p) => p.Name == listPropertyName);
                        if (listProperty != null)
                        {
                            if (listProperty.ListEntityType.Name == dataType.Name || dataType.IsSubclassOf(listProperty.ListEntityType))
                            {
                                levelIndex++;
                            }
                            else
                            {
                                levelIndex = 0;
                            }
                            RefEntityPropertyMeta refEntityPropertyMeta = RepositoryFactory.Find(listProperty.ListEntityType).EntityMeta.FindParentReferenceProperty();
                            if (refEntityPropertyMeta == null)
                            {
                                throw new SIE.Domain.Validation.ValidationException("{0}实体类型不存在父引用Id".L10nFormat(new object[] { listProperty.ListEntityType.Name }));
                            }
                            string name = refEntityPropertyMeta.ManagedProperty.Name;
                            Relation relation = new Relation
                            {
                                Master = dataSourceTypeName,
                                Detail = this.GetDataSourceTypeName(listProperty.ListEntityType, listPropertyName),
                                KeyColumn = new KeyColumn
                                {
                                    Master = "Id",
                                    Detail = name
                                }
                            };
                            relations.Add(relation);
                            this.GenerateQuerysAndRelatonsAndViews(billPrintable, listProperty.ListEntityType, ref querys, ref relations, ref views, levelIndex, listPropertyName);
                        }
                    }
                    return;
                }
            }
            List<string> list = new List<string>();
            foreach (string text2 in billPrintable.GetListPropertys(dataType))
            {
                PropertyInfo property = dataType.GetProperty(text2);
                if (!(property == null))
                {
                    if (property.PropertyType.Name == dataType.Name || dataType.IsSubclassOf(property.PropertyType))
                    {
                        levelIndex++;
                    }
                    else
                    {
                        levelIndex = 0;
                    }
                    Type childType = property.PropertyType.GetGenericArguments().FirstOrDefault<Type>();
                    if (!(childType == null) && (!(childType.Name == dataType.Name) || levelIndex++ <= 0))
                    {
                        if (list.Any((string p) => p == childType.FullName))
                        {
                            throw new Exception("实体：{0} 不允许存在多个类型：{1} 的子列表".L10nFormat(new object[] { dataType.FullName, childType.FullName }));
                        }
                        list.Add(childType.FullName);
                        KeyAttribute customAttribute = dataType.GetCustomAttribute<KeyAttribute>();
                        if (customAttribute == null)
                        {
                            throw new Exception("实体：{0} 未标记主键特性：{1}".L10nFormat(new object[] { dataType.FullName, "KeyAttribute" }));
                        }
                        ParentKeyAttribute customAttribute2 = childType.GetCustomAttribute<ParentKeyAttribute>();
                        if (customAttribute2 == null)
                        {
                            throw new Exception("实体：{0} 未标记父主键特性：{1}".L10nFormat(new object[] { childType.FullName, "ParentKeyAttribute" }));
                        }
                        string name2 = customAttribute2.Name;
                        string name3 = customAttribute.Name;
                        Relation relation2 = new Relation
                        {
                            Master = dataType.Name,
                            Detail = this.GetDataSourceTypeName(childType, text2),
                            KeyColumn = new KeyColumn
                            {
                                Master = name2,
                                Detail = name3
                            }
                        };
                        relations.Add(relation2);
                        this.GenerateQuerysAndRelatonsAndViews(billPrintable, childType, ref querys, ref relations, ref views, levelIndex, text2);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        /// <exception cref="PlatformException"></exception>
        private XtraReportsLayoutSerializer CreateXtraReportsLayoutSerializer(IPrintable printable, Type dataType)
        {
            XtraReportsLayoutSerializer xtraReportsLayoutSerializer;
            try
            {
                List<Parameter> list = new List<Parameter>();
                Parameter parameter = new Parameter
                {
                    Name = "database",
                    Value = DataXmlName
                };
                Parameter parameter2 = new Parameter
                {
                    Name = "read only",
                    Value = "1"
                };
                Parameter parameter3 = new Parameter
                {
                    Name = "generateConnectionHelper",
                    Value = "true"
                };
                list.Add(parameter);
                list.Add(parameter2);
                list.Add(parameter3);
                Parameters parameters = new Parameters
                {
                    ParameterList = list
                };
                Connection connection = new Connection
                {
                    Name = "data",
                    ProviderKey = "InMemorySetFull",
                    Parameters = parameters
                };
                List<Query> list2 = new List<Query>();
                List<Relation> list3 = new List<Relation>();
                List<ResultView> list4 = new List<ResultView>();
                this.GenerateQuerysAndRelatonsAndViews(printable, dataType, ref list2, ref list3, ref list4, 0, null);
                ResultSchema resultSchema = new ResultSchema
                {
                    DataSet = new SqlDataSet
                    {
                        Name = "sqlDataSource(XML)",
                        View = list4,
                        Relation = list3
                    }
                };
                string text = XmlSerializeHelper.Serialize<SqlDataSource>(new SqlDataSource
                {
                    Name = "DataSource",
                    Connection = connection,
                    Query = list2,
                    Relation = list3,
                    ResultSchema = resultSchema,
                    ConnectionOptions = new ConnectionOptions
                    {
                        CloseConnection = "true"
                    }
                });
                string text2 = "(<SqlDataSource[^>])([\\s\\S]*?)(</SqlDataSource>)";
                MatchCollection matchCollection = Regex.Matches(text, text2, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                ComponentStorage componentStorage = new ComponentStorage();
                Item1 item = new Item1();
                item.Ref = "0";
                item.Name = "DataSource";
                item.ObjectType = "DevExpress.DataAccess.Sql.SqlDataSource,DevExpress.DataAccess.v21.2";
                Encoding utf = Encoding.UTF8;
                System.Text.RegularExpressions.Match match = matchCollection[0];
                item.Base64 = Convert.ToBase64String(utf.GetBytes((match != null) ? match.Value : null));
                componentStorage.Item1 = item;
                ComponentStorage componentStorage2 = componentStorage;
                xtraReportsLayoutSerializer = new XtraReportsLayoutSerializer
                {
                    Ref = "1",
                    ControlType = "DevExpress.XtraReports.UI.XtraReport, DevExpress.XtraReports.v21.2",
                    PageWidth = "850",
                    PageHeight = "1100",
                    Version = "21.2",
                    ForeColor = "Black",
                    DataSource = "#Ref-0",
                    DataMember = dataType.Name,
                    ComponentStorage = componentStorage2
                };
            }
            catch (Exception ex)
            {
                throw new PlatformException(ex.Message);
            }
            return xtraReportsLayoutSerializer;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="SIE.Domain.Validation.ValidationException"></exception>
        internal void TaskValidation()
        {
            if (this.task == null || this.task.Exception != null)
            {
                throw new SIE.Domain.Validation.ValidationException("数据文件生成失败，可能是数据文件data.xml被占用，请退出相关程序重试！".L10N());
            }
            if (!this.task.IsCompleted)
            {
                this.task.Wait();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void ClearCsvFile()
        {
            string path = base.GetPath();
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            Directory.GetFiles(path, "*.xml").ForEach(delegate (string p)
            {
                File.Delete(p);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        protected void ClearXMLFile()
        {
            string text = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", DateTime.Today.AddDays(-7.0).ToString("yyyy-MM-dd"));
            if (Directory.Exists(text))
            {
                Directory.Delete(text, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public virtual string BeforeDesignerProcess(byte[] content)
        {
            DevExpress.XtraReports.UI.XtraReport xtraReport = new DevExpress.XtraReports.UI.XtraReport();
            xtraReport.LoadLayoutFromXml(new MemoryStream(content));
            this.UpdateCsvDataPath(xtraReport);
            MemoryStream memoryStream = new MemoryStream();
            xtraReport.SaveLayoutToXml(memoryStream);
            string text = Path.Combine(base.GetPath(), base.GetFileFullName("rpt"));
            xtraReport.SaveLayoutToXml(text);
            byte[] buffer = memoryStream.GetBuffer();
            memoryStream.Dispose();
            return Convert.ToBase64String(buffer);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="templateId"></param>
        /// <param name="content"></param>
        /// <param name="datas"></param>
        /// <param name="copies"></param>
        /// <returns></returns>
        public virtual string PrintProcess(IPrintable printable, double templateId, byte[] content, Func<IEnumerable<object>> datas, short copies = 1)
        {
            DevExpress.XtraReports.UI.XtraReport xtraReport = new DevExpress.XtraReports.UI.XtraReport();
            try
            {
                this.ClearXMLFile();
                this.GenerateDataFile(printable, datas());
                using (MemoryStream memoryStream = new MemoryStream(content))
                {
                    xtraReport.LoadLayoutFromXml(memoryStream);
                }
                this.UpdateCsvDataPath(xtraReport);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource = xtraReport.DataSource as DevExpress.DataAccess.Sql.SqlDataSource;
                DevExpress.DataAccess.Sql.SqlDataConnection sqlDataConnection = ((sqlDataSource != null) ? sqlDataSource.Connection : null);
                if (sqlDataConnection != null)
                {
                    sqlDataConnection.Close();
                }
            }
            string text;
            using (MemoryStream memoryStream2 = new MemoryStream())
            {
                xtraReport.SaveLayoutToXml(memoryStream2);
                text = Convert.ToBase64String(memoryStream2.GetBuffer());
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="report"></param>
        public override void UpdateCsvDataPath(DevExpress.XtraReports.UI.XtraReport report)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                dataSet.WriteXmlSchema(ms);
                report.DataSourceSchema = Encoding.UTF8.GetString(ms.ToArray());
                report.DataSource = dataSet;
            }
            //return;
            //DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource = report.ComponentStorage.FirstOrDefault<IComponent>() as DevExpress.DataAccess.Sql.SqlDataSource;
            //if (sqlDataSource != null)
            //{
            //    sqlDataSource.ConnectionParameters = new DevExpress.DataAccess.ConnectionParameters.XmlFileConnectionParameters(this.DataXmlName);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="datas"></param>
        /// <returns></returns>
        public override string GenerateDataFile(IPrintable printable, IEnumerable<object> datas)
        {
            string guid = Guid.NewGuid().ToString();
            this.DataXmlName = Path.Combine(base.GetPath(), guid + "-data.xml");
            task = Task.Run(delegate
            {
                //if (!isPrint)
                //{
                //    ClearCsvFile();
                //}

                foreach (DataTable table in dataSet.Tables)
                {
                    table.Constraints.Clear();
                }

                dataSet.Relations.Clear();
                dataSet.Tables.Clear();
                dataSet.Clear();
                if (printable is ILabelPrintable)
                {
                    GenerateDataFiles(printable, datas);
                }

                if (printable is IBillPrintable)
                {
                    GenerateDataFiles(printable as IBillPrintable, datas);
                }

                //DataSetConvertHelper.ConvertDataSetToXMLFile(dataSet, DataXmlName);

            });
            return DataXmlName;

        }

        //
        // 摘要:
        //     生成标签数据文件
        //
        // 参数:
        //   printable:
        //     打印提供者
        //
        //   datas:
        //     打印数据集合
        //
        //   dataType:
        //     打印类型
        protected virtual void GenerateDataFiles(IPrintable printable, IEnumerable<object> datas, Type dataType = null)
        {
            if (datas.Count() > 0)
            {
                dataType = datas.FirstOrDefault().GetType();
            }

            if (datas == null || dataType == null)
            {
                return;
            }

            DataTable dt = new DataTable(dataType.Name);
            printable.GetPropertys(dataType).ForEach(delegate (string p)
            {
                dt.Columns.Add(p, typeof(string));
            });
            dataSet.Tables.Add(dt);
            foreach (object data in datas)
            {
                string text = printable.ConverterData(data).Trim().TrimEnd(printable.Separator);
                DataRowCollection rows = dt.Rows;
                object[] values = text.Split(printable.Separator);
                rows.Add(values);
            }
        }

        /// <summary>
        /// 生成单据数据文件
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="datas"></param>
        /// <param name="dataType"></param>
        /// <param name="propertyName"></param>
        /// <param name="parentIdName"></param>
        /// <param name="parentTableName"></param>
        /// <param name="parentType"></param>
        /// <param name="levelIndex"></param>
        protected virtual void GenerateDataFiles(IBillPrintable printable, IEnumerable<object> datas, Type dataType = null, string propertyName = null, string parentIdName = null, string parentTableName = null, Type parentType = null, int levelIndex = 0)
        {
            if (datas.Count() > 0)
            {
                dataType = datas.FirstOrDefault().GetType();
            }

            if (datas == null || dataType == null)
            {
                return;
            }

            string text = dataType.Name;
            if (!propertyName.IsNullOrWhiteSpace())
            {
                text = "{0}_{1}".FormatArgs(dataType.Name, propertyName);
            }

            DataTable dt = new DataTable(text);
            printable.GetPropertys(dataType).ForEach(delegate (string p)
            {
                dt.Columns.Add(p, typeof(string));
            });
            foreach (object data in datas)
            {
                string text2 = printable.ConverterData(data).Trim().TrimEnd(printable.Separator);
                DataRowCollection rows = dt.Rows;
                object[] values = text2.Split(printable.Separator);
                rows.Add(values);
            }

            dataSet.Tables.Add(dt);
            if (parentIdName != null && parentType != null && parentTableName != null)
            {
                KeyAttribute keyAttribute = parentType.GetCustomAttributes<KeyAttribute>().FirstOrDefault();
                if (keyAttribute == null)
                {
                    dataSet.Relations.Add(dt.TableName + "Relation", dataSet.Tables[parentTableName].Columns["Id"], dt.Columns[parentIdName]);
                }
                else
                {
                    dataSet.Relations.Add(dt.TableName + "Relation", dataSet.Tables[parentTableName].Columns[keyAttribute.Name], dt.Columns[parentIdName]);
                }
            }

            if (levelIndex <= 0)
            {
                if (dataType.IsSubclassOf(typeof(DataEntity)))
                {
                    GenerateEntityData(printable, datas, dataType, levelIndex, text);
                }
                else
                {
                    GenerateNormalClassData(printable, datas, dataType, levelIndex, text);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="datas"></param>
        /// <param name="dataType"></param>
        /// <param name="levelIndex"></param>
        /// <param name="parentTableName"></param>
        private void GenerateEntityData(IBillPrintable printable, IEnumerable<object> datas, Type dataType, int levelIndex, string parentTableName)
        {
            IEnumerable<IListProperty> listManagedPropertys = AppRuntime.Service.Resolve<PrintableController>().GetListManagedPropertys(dataType);
            using (IEnumerator<string> enumerator = printable.GetListPropertys(dataType).GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    string listPropertyName = enumerator.Current;
                    IListProperty listProperty = listManagedPropertys.FirstOrDefault((IListProperty p) => p.Name == listPropertyName);
                    IManagedProperty managedProperty = RepositoryFactory.Find(listProperty.ListEntityType).EntityMeta.FindParentReferenceProperty().ManagedProperty;
                    if (listProperty != null)
                    {
                        List<object> list = new List<object>();
                        foreach (object obj in datas)
                        {
                            EntityList lazyList = (obj as DataEntity).GetLazyList(listProperty, null);
                            list.AddRange(lazyList.Cast<object>());
                        }
                        if (listProperty.ListEntityType.Name == dataType.Name || dataType.IsSubclassOf(listProperty.ListEntityType))
                        {
                            levelIndex++;
                        }
                        else
                        {
                            levelIndex = 0;
                        }
                        this.GenerateDataFiles(printable, list, listProperty.ListEntityType, listPropertyName, managedProperty.Name, parentTableName, dataType, levelIndex);
                    }
                }
            }
        }


        private void GenerateNormalClassData(IBillPrintable printable, IEnumerable<object> datas, Type dataType, int levelIndex, string parentTableName)
        {
            List<string> list = new List<string>();
            foreach (string text in printable.GetListPropertys(dataType))
            {
                PropertyInfo property = dataType.GetProperty(text);
                if (!(property == null))
                {
                    Type childType = property.PropertyType.GetGenericArguments().FirstOrDefault<Type>();
                    if (!(childType == null))
                    {
                        if (list.Any((string p) => p == childType.FullName))
                        {
                            throw new Exception("实体：{0} 不允许存在多个类型：{1} 的子列表".L10nFormat(new object[] { dataType.FullName, childType.FullName }));
                        }
                        list.Add(childType.FullName);
                        if (dataType.GetCustomAttribute<KeyAttribute>() == null)
                        {
                            throw new Exception("实体：{0} 未标记主键特性：{1}".L10nFormat(new object[] { dataType.FullName, "KeyAttribute" }));
                        }
                        ParentKeyAttribute customAttribute = childType.GetCustomAttribute<ParentKeyAttribute>();
                        if (customAttribute == null)
                        {
                            throw new Exception("实体：{0} 未标记父主键特性：{1}".L10nFormat(new object[] { childType.FullName, "ParentKeyAttribute" }));
                        }
                        List<object> list2 = new List<object>();
                        foreach (object obj in datas)
                        {
                            IEnumerable<object> enumerable = property.GetValue(obj) as IEnumerable<object>;
                            if (enumerable != null)
                            {
                                list2.AddRange(enumerable);
                            }
                        }
                        if (childType.Name == dataType.Name || dataType.IsSubclassOf(childType))
                        {
                            levelIndex++;
                        }
                        else
                        {
                            levelIndex = 0;
                        }
                        this.GenerateDataFiles(printable, list2, childType, text, customAttribute.Name, parentTableName, dataType, levelIndex);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="filePath"></param>
        /// <param name="printerName"></param>
        /// <param name="datas"></param>
        /// <param name="completeCallBack"></param>
        /// <param name="copies"></param>
        public override void PrintProcess(IPrintable printable, string filePath, string printerName, Func<IEnumerable<object>> datas, Action completeCallBack, short copies)
        {
            DevExpress.XtraReports.UI.XtraReport xtraReport = new DevExpress.XtraReports.UI.XtraReport();
            try
            {
                this.ValidateFilePath(filePath);
                this.GenerateDataFile(printable, datas());
                this.TaskValidation();
                xtraReport.LoadLayoutFromXml(filePath);
                this.UpdateCsvDataPath(xtraReport);
                ////todo 通过pdf打印
                //var pdfFilePatth = "";
                //xtraReport.ExportToPdf(pdfFilePatth);

                //var reportPrintTool = new DevExpress.XtraReports.UI.ReportPrintTool(xtraReport);
                var reportPrintTool = new PrintToolBase(xtraReport.PrintingSystem);
                //reportPrintTool.AutoShowParametersPanel = false;
                reportPrintTool.PrinterSettings.Copies = copies;
                reportPrintTool.PrinterSettings.Collate = true;
                reportPrintTool.PrinterSettings.PrinterName = printerName;
                if (!reportPrintTool.PrinterSettings.IsValid)
                {
                    throw new SIE.Domain.Validation.ValidationException("该打印机：{0} 无效".L10nFormat(new object[] { printerName }));
                }
                reportPrintTool.Print();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                DevExpress.DataAccess.Sql.SqlDataSource sqlDataSource = xtraReport.DataSource as DevExpress.DataAccess.Sql.SqlDataSource;
                DevExpress.DataAccess.Sql.SqlDataConnection sqlDataConnection = ((sqlDataSource != null) ? sqlDataSource.Connection : null);
                if (sqlDataConnection != null)
                {
                    sqlDataConnection.Close();
                }
            }
            completeCallBack();
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="content"></param>
        /// <param name="printerName"></param>
        /// <param name="datas"></param>
        /// <param name="completeCallBack"></param>
        /// <param name="copies"></param>
        /// <param name="marginLeft"></param>
        /// <param name="marginTop"></param>
        /// <param name="marginRight"></param>
        /// <param name="marginBottom"></param>
        /// <exception cref="SIE.Domain.Validation.ValidationException"></exception>
        /// <exception cref="PrintException"></exception>
        public void PrintProcess(IPrintable printable, byte[] content, string printerName, Func<IEnumerable<object>> datas, Action completeCallBack, short copies = 1,
            int marginLeft = 0, int marginTop = 0, int marginRight = 0, int marginBottom = 0)
        {
            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);
            XtraReport xtraReport = new XtraReport();
            var dataFile = string.Empty;
            try
            {
                dataFile = this.GenerateDataFile(printable, datas());
                this.TaskValidation();
                using (var ms = new MemoryStream(content))
                {
                    xtraReport.LoadLayoutFromXml(ms);
                }
                this.UpdateCsvDataPath(xtraReport);
                xtraReport.PrinterName = printerName;
                xtraReport.CreateDocument();
                xtraReport.PrintingSystem.PageSettings.LeftMargin = marginLeft;
                xtraReport.PrintingSystem.PageSettings.TopMargin = marginTop;
                xtraReport.PrintingSystem.PageSettings.RightMargin = marginRight;
                xtraReport.PrintingSystem.PageSettings.BottomMargin = marginBottom;
                //ReportPrintTool reportPrintTool = new DevExpress.XtraReports.UI.ReportPrintTool(xtraReport);
                var reportPrintTool = new PrintToolBase(xtraReport.PrintingSystem);
                //reportPrintTool.AutoShowParametersPanel = false;
                reportPrintTool.PrinterSettings.Copies = copies;
                reportPrintTool.PrinterSettings.Collate = true;
                reportPrintTool.PrinterSettings.PrinterName = printerName;
                if (!reportPrintTool.PrinterSettings.IsValid)
                {
                    throw new SIE.Domain.Validation.ValidationException("该打印机：{0} 无效".L10nFormat(printerName));
                }

                reportPrintTool.Print();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //((xtraReport.DataSource as DevExpress.DataAccess.Sql.SqlDataSource)?.Connection)?.Close();
                //Task.Run(async () =>
                //{
                //    await Task.Delay(5000);
                //    ((xtraReport.DataSource as DevExpress.DataAccess.Sql.SqlDataSource)?.Connection)?.Close();
                //    if (File.Exists(dataFile))
                //    {
                //        File.Delete(dataFile);
                //    }
                //});
            }

            completeCallBack();
        }

        /// <summary>
        /// 导出图像
        /// </summary>
        /// <param name="printable"></param>
        /// <param name="content"></param>
        /// <param name="printerName"></param>
        /// <param name="datas"></param>
        /// <param name="resolution"></param>
        /// <param name="marginLeft"></param>
        /// <param name="marginTop"></param>
        /// <param name="marginRight"></param>
        /// <param name="marginBottom"></param>
        /// <returns></returns>
        public byte[] ExportToImage(IPrintable printable, byte[] content, string printerName,
            Func<IEnumerable<object>> datas, int resolution = 96, int marginLeft = 0, int marginTop = 0, int marginRight = 0, int marginBottom = 0)
        {

            ScriptPermissionManager.GlobalInstance = new ScriptPermissionManager(ExecutionMode.Unrestricted);
            var devReport = new XtraReport();
            var dataFile = string.Empty;
            try
            {
                dataFile = this.GenerateDataFile(printable, datas.Invoke());
                this.TaskValidation();
                using (var ms = new MemoryStream(content))
                {
                    devReport.LoadLayoutFromXml(ms);
                }
                this.UpdateCsvDataPath(devReport);

                devReport.PrinterName = printerName;
                devReport.CreateDocument();
                devReport.PrintingSystem.PageMargins.Left = marginLeft;
                devReport.PrintingSystem.PageMargins.Top = marginTop;
                devReport.PrintingSystem.PageMargins.Right = marginRight;
                devReport.PrintingSystem.PageMargins.Bottom = marginBottom;

                var imageExportOptions = new ImageExportOptions()
                {
                    Format = System.Drawing.Imaging.ImageFormat.Png,
                    Resolution = resolution,
                    ExportMode = ImageExportMode.SingleFilePageByPage,
                    PageBorderWidth = 0
                };
                var stream = new MemoryStream();
                devReport.ExportToImage(stream, imageExportOptions);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                //((devReport.DataSource as DevExpress.DataAccess.Sql.SqlDataSource)?.Connection)?.Close();
                //Task.Run(async () =>
                //{
                //    await Task.Delay(5000);
                //    ((devReport.DataSource as DevExpress.DataAccess.Sql.SqlDataSource)?.Connection)?.Close();
                //    if (File.Exists(dataFile))
                //    {
                //        File.Delete(dataFile);
                //    }
                //});
            }
        }

        private const string XmlHeader = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n";


        private const string ChildTableExtension = "_Child";


        protected string DataXmlName = "data.xml";


        internal Task task;


        private DataSet dataSet = new DataSet("Data");

        public override void UpdateCsvDataPath(string filePath)
        {

        }
    }
}
