using ICSharpCode.SharpZipLib.Zip;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SIE.Common.Catalogs;
using SIE.Common.Import;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.MetaModel.View;
using SIE.MetaModel.XmlConfig;
using SIE.Reflection;
using SIE.Threading;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.Import
{
    public class CommonImporter: Importer
    {

        protected virtual Catalog GetCatalogData(CacheData cacheData, EntityPropertyViewMeta property, string value)
        {
            List<Catalog> value2 = null;
            if (!cacheData.CatalogData.TryGetValue(property.CatalogType, out value2))
            {
                value2 = AppRuntime.Service.Resolve<CatalogController>().GetCatalogList(property.CatalogType).ToList();
                cacheData.CatalogData.Add(property.CatalogType, value2);
            }

            if (!string.IsNullOrWhiteSpace(property.CatalogField))
            {
                if (property.CatalogField == Catalog.CodeProperty.Name)
                {
                    return value2.FirstOrDefault((Catalog p) => p.Code == value);
                }

                if (property.CatalogField == Catalog.NameProperty.Name)
                {
                    return value2.FirstOrDefault((Catalog p) => p.Name == value);
                }
            }

            return null;
        }

        protected virtual object GetEnumValue(CacheData cacheData, EntityPropertyViewMeta property, object value)
        {
            try
            {
                Type type = property.PropertyType.IgnoreNullable();
                Dictionary<string, Enum> value2 = null;
                if (!cacheData.EnumData.TryGetValue(type, out value2))
                {
                    value2 = new Dictionary<string, Enum>();
                    foreach (Enum value4 in Enum.GetValues(type))
                    {
                        value2[value4.ToString()] = value4;
                        string text = value4.ToLabel();
                        if (text.IsNotEmpty())
                        {
                            value2[text] = value4;
                        }
                    }

                    cacheData.EnumData.Add(type, value2);
                }

                Enum value3 = null;
                if (value2.TryGetValue(value.ToString(), out value3))
                {
                    return value3;
                }

                throw new Exception("[{0}]枚举值转换失败,请参考字段备注输入正确的值".L10nFormat(value));
            }
            catch (Exception)
            {
                throw new Exception("[{0}]枚举值转换失败,请参考字段备注输入正确的值".L10nFormat(value));
            }
        }

        //
        // 摘要:
        //     获取实体Id
        //
        // 参数:
        //   propertyType:
        //     类型
        //
        //   conditions:
        //     查询条件
        protected virtual Entity GetEntityId(Type propertyType, Dictionary<IManagedProperty, object> conditions)
        {
            if (conditions.Count == 0)
            {
                return null;
            }

            CommonQueryCriteria indexerCriteria = new CommonQueryCriteria();
            conditions.ForEach(delegate (KeyValuePair<IManagedProperty, object> p)
            {
                indexerCriteria.Add(p.Key, p.Value);
            });
            EntityList by = RepositoryFactory.Find(propertyType).GetBy(indexerCriteria);
            if (by == null || by.Count <= 0)
            {
                return null;
            }

            return by[0];
        }


        protected virtual Dictionary<string, object> GetRefData(CacheData cacheData, EntityPropertyViewMeta property)
        {
            Dictionary<string, object> refData = null;
            string name = property.Name;
            if (!cacheData.RefPropertyData.TryGetValue(name, out refData))
            {
                Type refType = property.SelectionViewMeta?.SelectionEntityType ?? property.Owner.EntityType;
                string refPropertyName = property?.XPath?.Substring(property.XPath.IndexOf(".") + 1) ?? property.Name;
                EntityPropertyMeta refProperty = CommonModel.Entities.Find(refType).Property(refPropertyName);
                EntityList all = RepositoryFactory.Find(refType).GetAll(new PagingInfo(1, 5000));
                refData = new Dictionary<string, object>(all.Count);
                (from p in all.OfType<Entity>()
                 select new
                 {
                     Key = p.GetProperty(refProperty.ManagedProperty).ToString(),
                     Value = p.GetId()
                 }).ForEach(p =>
                 {
                     try
                     {
                         if (p.Key.IsNotEmpty())
                         {
                             refData.Add(p.Key, p.Value);
                         }
                     }
                     catch (ArgumentException)
                     {
                         throw new ValidationException("查找[{0}]属性[{1}]发现重复的值[{2}]".L10nFormat(refType.GetQualifiedName(), refPropertyName, p.Key));
                     }
                 });
                cacheData.RefPropertyData.Add(name, refData);
            }

            return refData;
        }


        public virtual void SaveData(IList<RowData> data, Action<IList<RowData>> saveAction, Action<string> completeCallback)
        {
            string errors = "";
            IParallelActions parallelActions = AsyncHelper.CreateParallelActions();
            parallelActions.MaxThreadCount = 50;
            int i;
            for (i = 0; i + 100 < data.Count; i += 100)
            {
                List<RowData> batch2 = data.Skip(i).Take(100).ToList();
                Action action = AsyncHelper.WithCurrentThreadContext(delegate
                {
                    try
                    {
                        saveAction(batch2);
                    }
                    catch (Exception ex2)
                    {
                        errors += ex2.GetBaseException().Message;
                    }
                });
                parallelActions.Prepare(action);
            }

            if (i < data.Count)
            {
                List<RowData> batch = data.Skip(i).ToList();
                Action action2 = AsyncHelper.WithCurrentThreadContext(delegate
                {
                    try
                    {
                        saveAction(batch);
                    }
                    catch (Exception ex)
                    {
                        errors += ex.GetBaseException().Message;
                    }
                });
                parallelActions.Prepare(action2);
            }

            parallelActions.RunAll();
            completeCallback(errors);
        }

        //
        // 摘要:
        //     按实体类型的视图配置组的定义，导入指定的文件的数据。 引用类型可以通过唯一索引字段进行关联， 例如:
        //
        //     View.PropertyRef(p => p.DefectCategory.Code).HasLabel("分类编码");//web 使用PropertyRef
        //
        //
        //     View.Property(p => p.DefectCategory.Code).HasLabel("分类编码");//wpf 使用Property
        //
        //     默认50个线程并发保存，不支持主从导入
        //
        // 参数:
        //   importResult:
        //
        //   entityType:
        //     实体类型
        //
        //   viewGroup:
        //
        //   stream:
        //
        //   rowAction:
        //     行数据处理操作
        //
        //   saveAction:
        //     数据保存操作
        //
        //   completeCallback:
        public virtual void Import(ref ImportResult importResult, Type entityType, string viewGroup, Stream stream, Action<RowData, CacheData> rowAction, Action<IList<RowData>> saveAction, Action<string> completeCallback)
        {
            Check.NotNull(entityType, "entityType");
            Check.NotNull(viewGroup, "viewGroup");
            Check.NotNull(stream, "stream");
            Check.NotNull(rowAction, "rowAction");
            Check.NotNull(saveAction, "saveAction");
            try
            {
                ISheet sheetAt = ((IWorkbook)new XSSFWorkbook(stream)).GetSheetAt(0);
                LoadData(importResult, entityType, viewGroup, sheetAt, rowAction, saveAction, completeCallback);
            }
            catch (ZipException ex)
            {
                throw new Exception("excel格式无法识别:{0}".L10nFormat(ex.Message));
            }
        }

        protected virtual object GetRefId(Dictionary<string, object> refData, EntityPropertyViewMeta property, string value)
        {
            object value2 = null;
            if (!refData.TryGetValue(value, out value2))
            {
                Type refType = property.SelectionViewMeta?.SelectionEntityType ?? property.Owner.EntityType;
                string refPropertyName = property?.XPath?.Substring(property.XPath.IndexOf(".") + 1) ?? property.Name;
                EntityPropertyMeta refProperty = CommonModel.Entities.Find(refType).Property(refPropertyName);
                CommonQueryCriteria commonQueryCriteria = new CommonQueryCriteria();
                commonQueryCriteria.Add(refProperty.ManagedProperty, value);
                (from p in RepositoryFactory.Find(refType).GetBy(commonQueryCriteria).OfType<Entity>()
                 select new
                 {
                     Key = p.GetProperty(refProperty.ManagedProperty).ToString(),
                     Value = p.GetId()
                 }).ForEach(p =>
                 {
                     try
                     {
                         if (p.Key.IsNotEmpty())
                         {
                             refData.Add(p.Key, p.Value);
                         }
                     }
                     catch (ArgumentException)
                     {
                         throw new ValidationException("查找[{0}]属性[{1}]发现重复的值[{2}]".L10nFormat(refType.GetQualifiedName(), refPropertyName, p.Key));
                     }
                 });
                refData.TryGetValue(value, out value2);
            }

            return value2;
        }

        protected virtual object GetCellValue(ICell cell, Type propertyType)
        {
            object result = "";
            if ((cell == null || cell.CellType == CellType.Blank) && propertyType.IsNullable())
            {
                return null;
            }

            if (cell == null)
            {
                return result;
            }

            switch (cell.CellType)
            {
                case CellType.Blank:
                    return "";
                case CellType.Numeric:
                    if (propertyType == typeof(DateTime) || propertyType == typeof(DateTime?))
                    {
                        try
                        {
                            return cell.DateCellValue;
                        }
                        catch
                        {
                            throw new ValidationException("错误值：{0}，请输入正确格式的时间".L10nFormat(cell.NumericCellValue));
                        }
                    }

                    return cell.NumericCellValue;
                case CellType.Boolean:
                    return cell.BooleanCellValue;
                case CellType.String:
                    return cell.StringCellValue;
                default:
                    return cell.StringCellValue;
            }
        }


        public virtual RowData LoadRowData(IRow row, Type entityType, IEnumerable<EntityPropertyViewMeta> properties, CacheData cacheData)
        {
            int num = -1;
            Entity entity = Activator.CreateInstance(entityType) as Entity;
            entity.PersistenceStatus = PersistenceStatus.New;
            Dictionary<IManagedProperty, object> dictionary = new Dictionary<IManagedProperty, object>();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (EntityPropertyViewMeta property in properties)
            {
                string text = (property.Label ?? property.Name).Replace("<span style=\"color:red\">*</span>", "*").L10N();
                try
                {
                    num++;
                    object cellValue = GetCellValue(row.GetCell(num), property.PropertyType);
                    if (cellValue == null)
                    {
                        continue;
                    }

                    cellValue = ((property.BeforeImportFunc == null) ? cellValue.ToString().Trim() : property.BeforeImportFunc(cellValue));
                    if (property.IsReference)
                    {
                        string text2 = cellValue?.ToString();
                        if (!text2.IsNullOrEmpty())
                        {
                            if (property.XPath.Where((char p) => p == '.').Count() != 1)
                            {
                                throw new PlatformException("引用属性[{0}]配置不正确".L10nFormat(property.XPath));
                            }

                            _ = property.SelectionViewMeta.SelectionEntityType;
                            object refId = GetRefId(GetRefData(cacheData, property), property, text2);
                            if (refId == null)
                            {
                                throw new ValidationException("找不到值为[{0}]对应的数据".L10nFormat(text2));
                            }

                            if (property.IsImportIndexer)
                            {
                                dictionary.Add(property.PropertyMeta.ManagedProperty as IRefIdProperty, refId);
                            }

                            entity.SetRefId(property.PropertyMeta.ManagedProperty as IRefIdProperty, refId);
                        }

                        continue;
                    }

                    if (entity.SupportTree && property.IsImportTreeIndexer)
                    {
                        string text3 = cellValue?.ToString();
                        EntityPropertyViewMeta entityPropertyViewMeta = properties.Where((EntityPropertyViewMeta p) => p.IsImportIndexer).FirstOrDefault();
                        if (entityPropertyViewMeta != null)
                        {
                            object refId2 = GetRefId(GetRefData(cacheData, entityPropertyViewMeta), property, text3);
                            if (refId2 == null)
                            {
                                throw new ValidationException("找不到值为[{0}]对应的数据".L10nFormat(text3));
                            }

                            if (property.IsImportIndexer)
                            {
                                dictionary.Add(property.PropertyMeta.ManagedProperty, refId2);
                            }

                            entity.SetProperty(property.PropertyMeta.ManagedProperty, refId2);
                        }

                        continue;
                    }

                    if (!string.IsNullOrWhiteSpace(property.CatalogType) && !string.IsNullOrWhiteSpace(property.CatalogField) && !string.IsNullOrWhiteSpace(cellValue?.ToString()))
                    {
                        Catalog catalogData = GetCatalogData(cacheData, property, cellValue.ToString());
                        if (catalogData == null)
                        {
                            throw new ValidationException("快码[{0}]找不到为[{1}]的值".L10nFormat(property.CatalogType, cellValue.ToString()));
                        }

                        if (property.IsImportIndexer)
                        {
                            dictionary.Add(property.PropertyMeta.ManagedProperty, catalogData.Code);
                        }

                        entity.SetProperty(property.PropertyMeta.ManagedProperty, catalogData.Code);
                        continue;
                    }

                    if (property.PropertyType.IgnoreNullable().IsEnum)
                    {
                        cellValue = GetEnumValue(cacheData, property, cellValue);
                    }

                    try
                    {
                        cellValue.ConvertTo(property.PropertyType);
                    }
                    catch
                    {
                        string text4 = "错误值：{0}，类型转换失败".L10nFormat(cellValue);
                        Type type = property.PropertyType.IgnoreNullable();
                        if (type == typeof(decimal) || type == typeof(double))
                        {
                            text4 = "错误值：{0}，请输入正确格式的数字".L10nFormat(cellValue);
                            goto IL_0440;
                        }

                        if (type == typeof(int))
                        {
                            text4 = "错误值：{0}，请输入正确格式的整数".L10nFormat(cellValue);
                            goto IL_0440;
                        }

                        if (!(type == typeof(DateTime)))
                        {
                            goto IL_0440;
                        }

                        if (property.PropertyType.IsNullable() && string.IsNullOrWhiteSpace(cellValue?.ToString()))
                        {
                            continue;
                        }

                        text4 = "错误值：{0}，请输入正确格式的时间".L10nFormat(cellValue);
                        goto IL_0440;
                        IL_0440:
                        stringBuilder.Append("读取列[{0}]失败，".L10nFormat(text) + text4 + ";");
                        goto end_IL_005f;
                    }

                    if (property.IsImportIndexer)
                    {
                        dictionary.Add(property.PropertyMeta.ManagedProperty, cellValue.ConvertTo(property.PropertyType));
                    }

                    entity.SetProperty(property.PropertyMeta.ManagedProperty, cellValue.ConvertTo(property.PropertyType));
                    end_IL_005f:;
                }
                catch (Exception ex)
                {
                    stringBuilder.Append("读取列[{0}]失败，".L10nFormat(text) + ex.Message + ";");
                }
            }

            if (!stringBuilder.ToString().IsNullOrWhiteSpace())
            {
                throw new Exception(stringBuilder.ToString());
            }

            Entity entityId = GetEntityId(entityType, dictionary);
            if (entityId != null)
            {
                foreach (EntityPropertyViewMeta property2 in properties)
                {
                    object obj2 = null;
                    if (property2.IsReference)
                    {
                        obj2 = entity.GetRefId(property2.PropertyMeta.ManagedProperty as IRefIdProperty);
                        if (obj2 != null)
                        {
                            entityId.SetRefId(property2.PropertyMeta.ManagedProperty as IRefIdProperty, obj2);
                        }
                    }
                    else
                    {
                        obj2 = entity.GetProperty(property2.PropertyMeta.ManagedProperty);
                        entityId.SetProperty(property2.PropertyMeta.ManagedProperty, obj2);
                    }
                }

                entity = entityId;
                entity.PersistenceStatus = PersistenceStatus.Modified;
            }

            return new RowData
            {
                RowIndex = row.RowNum,
                Entity = entity
            };
        }


        public virtual void LoadData(ImportResult importResult, Type entityType, string viewGroup, ISheet sheet, Action<RowData, CacheData> rowAction, Action<IList<RowData>> saveAction, Action<string> completeCallback)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            List<RowData> list = new List<RowData>();
            new List<IRow>();
            EntityViewMeta entityViewMeta = UIModel.Views.CreateBaseView(entityType, viewGroup, BlockConfigType.Customization);
            CacheData cacheData = new CacheData();
            List<EntityPropertyViewMeta> list2 = (from p in entityViewMeta.OrderedEntityProperties()
                                                  where p.CanShowIn(ShowInWhere.Import)
                                                  select p).ToList();
            try
            {
                for (int i = sheet.FirstRowNum + 1; i <= sheet.LastRowNum; i++)
                {
                    IRow row = sheet.GetRow(i);
                    bool flag = true;
                    for (int j = 0; j < list2.Count; j++)
                    {
                        ICell cell = row.GetCell(j);
                        if (cell != null && !string.IsNullOrWhiteSpace(cell.ToString()))
                        {
                            flag = false;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        try
                        {
                            RowData rowData = LoadRowData(row, entityType, list2, cacheData);
                            rowAction(rowData, cacheData);
                            list.Add(rowData);
                            importResult.MessageList.Add(new ImportMessageResult
                            {
                                RowNum = row.RowNum + 1,
                                MsgType = ImportMessageType.LoadSucess,
                                Message = "读取成功！".L10N()
                            });
                        }
                        catch (Exception ex)
                        {
                            importResult.MessageList.Add(new ImportMessageResult
                            {
                                RowNum = row.RowNum + 1,
                                MsgType = ImportMessageType.LoadFail,
                                Message = ex.GetBaseException().Message
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("模板解释失败,请对比导入模板！");
            }

            stopwatch.Stop();
            importResult.LoadTime = stopwatch.ElapsedMilliseconds;
            stopwatch = Stopwatch.StartNew();
            SaveData(list, saveAction, completeCallback);
            stopwatch.Stop();
            importResult.SaveTime = stopwatch.ElapsedMilliseconds;
        }

    }
}
