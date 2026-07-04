using SIE.AbnormalInfo.AbnormalMonitors.Dao;
using SIE.AbnormalInfo.AbnormalMonitors.Service;
using SIE.AbnormalInfo.Common;
using SIE.Configuration;
using SIE.Core.Common.Service;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ObjectModel;
using SIE.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace SIE.AbnormalInfo.AbnormalMonitors
{
    /// <summary>
    /// 异常采集服务
    /// </summary>
    public class CollectionService : DomainService
    {
        /// <summary>
        /// 获取表，字段及备注
        /// </summary>
        private readonly SqlViewModelCurerDao tabColumnDao;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlViewModelCurerDao"></param>
        public  CollectionService(SqlViewModelCurerDao sqlViewModelCurerDao) {

            tabColumnDao=sqlViewModelCurerDao;
        }

        #region 查询 Get

        #region 获取数据库字段，备注，字表结构

        /// <summary>
        /// 读取数据表的字段及备注
        /// </summary>
        /// <param name="tabName"></param>
        /// <returns></returns>
        private EntityList<AbnormalMonitorTab> GetTabColumns(string tabName)
        {
            var cfg = RT.Config.GetConnectionString(AbnormalInfoDataProvider.ConnectionStringName);
            string sql;
            if (cfg.ProviderName.Contains("Oracle")|| cfg.ProviderName.Contains("Dm") || cfg.ProviderName.Contains("VastData"))
            {
                sql = string.Format(@"SELECT a.COLUMN_NAME ,b.COMMENTS  FROM USER_TAB_COLUMNS a  LEFT JOIN  USER_COL_COMMENTS  b ON a.TABLE_NAME =b.TABLE_NAME  
WHERE  a.TABLE_NAME ='{0}' AND a.COLUMN_NAME=b.COLUMN_NAME AND b.COMMENTS IS NOT NULL ", tabName);
            }
            else if (cfg.ProviderName.Contains("SqlServer"))
            {
                sql = $"SELECT   COLUMN_NAME = a.name,  COMMENTS = isnull(g.[value], '') FROM  syscolumns a  left join systypes b on a.xusertype = b.xusertype  inner join sysobjects d on a.id = d.id  and d.xtype = 'U'  and d.name <> 'dtproperties'  left join syscomments e on a.cdefault = e.id  left join sys.extended_properties g on a.id = G.major_id  and a.colid = g.minor_id  left join sys.extended_properties f on d.id = f.major_id  and f.minor_id = 0 where  d.name = '{tabName}' order by  a.id, a.colorder";
            }
            else 
                throw new ValidationException("未支持的数据库".L10N());
            var entityLit = new EntityList<AbnormalMonitorTab>();
            tabColumnDao.QueryList(sql, entityLit);
            return entityLit;
        }

        /// <summary>
        /// 获取数据实体，子表集合
        /// </summary>
        /// <param name="type"></param>
        /// <param name="relationField"></param>
        /// <param name="oneToMore"></param>
        /// <returns></returns>
        private QuerySelectTree GetChildTabByType(Type type, string relationField, bool oneToMore)
        {
            var repository = RepositoryFactoryHost.Factory.FindByEntity(type);
            if (repository != null && repository.TableInfo.TabelName!=null)
            {
                var node = new QuerySelectTree
                {
                    TabName = repository.TableInfo.TabelName
                };
                var att = type.GetCustomAttributes(false).FirstOrDefault(c => { return c is LabelAttribute; }) as LabelAttribute;
                node.text = att?.Label;
                node.id = node.TabName+ relationField;
                node.children = null;//设置null 前端才会知道你是没加载的
                node.leaf = false;
				node.type = type.GetQualifiedName();
				node.field = relationField;
				node.expandable = true;
                //一对多关系建立
                if(oneToMore)
                    node.oneToMoreRelationField = GetOneToMoreRelationFiled(repository);
                return node;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private string GetOneToMoreRelationFiled(IRepository repository) {
            var str = string.Empty;
              repository.TableInfo.Columns.ForEach(column => {
                  if (column.Property is IRefIdProperty refIdProperty && refIdProperty.ReferenceType == ReferenceType.Parent)
                  {
                      str = column.Name;
                  }
              });
            return str;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="monitorTab"></param>
        /// <param name="TabName"></param>
        /// <param name="level"></param>
        /// <param name="expand"></param>
        /// <returns></returns>
        private QuerySelectTree GetEntityColumnTree(AbnormalMonitorTab monitorTab, string TabName, double level = 100, bool expand = false)
        {
            QuerySelectTree tree = new QuerySelectTree
            {
                id = monitorTab.ColumnName + TabName,
                text = monitorTab.Comments,
                TabName = TabName,
                field = monitorTab.ColumnName,
                root = false,
                isLast = false,
                isFirst = false,
                leaf = true,
                expandable = true,
                expanded = expand
            };
            return tree;
        }

        /// <summary>
        /// 获取监控树
        /// </summary>
        public virtual List<QuerySelectTree> GetAnomalyMonitorTree(string typeName)
        {
            var type = Type.GetType(typeName);
            var List = new List<QuerySelectTree>();
            var rep = tabColumnDao.Rep = RepositoryFactoryHost.Factory.FindByEntity(type) as EntityRepository;
            tabColumnDao.TabType = typeof(AbnormalMonitorTab);
            var tabName = rep.TableInfo.TabelName;
            //添加表字段
            var columns = GetTabColumns(tabName);
            rep.TableInfo.Columns.ForEach(col => {
                var column = columns.FirstOrDefault(c => c.ColumnName == col.Name);
                //添加关联表
                if (col.Property is IRefIdProperty refIdProperty && column != null)
                {
                    var refProp = refIdProperty.RefEntityProperty;
                    var node = GetChildTabByType(refProp.PropertyType, col.Name, false);
                    if (node != null && refProp.PropertyType.FullName != type.FullName)
                    {
                        node.text = column.Comments;
                        List.Add(node);
                    }
                }
                else if (columns.Any(c => c.ColumnName == col.Name))
                {
                    if (column != null)
                    {
                        var node = GetEntityColumnTree(column, tabName);
                        var propType = col.DataType;
                        if (!string.IsNullOrWhiteSpace(node.text))
                        {
                            node.type = propType.FullName;
                            node.editType = AbnormalRuleSqlHelper.GetPropEditType(propType);
                            //枚举数据源
                            SetNodeComboxSource(node, propType);
                            List.Add(node);
                        }
                    }
                }
            });
            //获取子集实体
            var fields = TypeHelper.GetHierarchy(type).SelectMany(p => p.GetFields(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic)).Where(p => typeof(IListProperty).IsAssignableFrom(p.FieldType)).ToList();
            foreach (var field in fields)
            {
                var value = field.GetValue(null);
                var property = value as IListProperty;
                var node = GetChildTabByType(property.ListEntityType, "", true);
                if (node != null && property.ListEntityType.FullName != type.FullName)
                {
                    List.Add(node);
                }
            }
            return List;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <param name="propType"></param>
        private void SetNodeComboxSource(QuerySelectTree node,Type propType)
        {
            // 枚举数据源
            if (node.editType == FieldProp.EnumField)
            {
                node.type= propType.GetQualifiedName();
                AddEnumValuesToList(node.comboxSource, propType);
            }
            // 时间数据源
            if (node.editType == FieldProp.DateTime)
            {
                AddEnumValuesToList(node.comboxSource,typeof(DateType));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="type"></param>
        private void AddEnumValuesToList(List<string> list,Type type)
        {
            Array values = Enum.GetValues(type);
            foreach (var value in values)
            {
                list.Add(AbnormalRuleSqlHelper.GetEnumDescription((Enum)value));
            }
        }

        #endregion


        /// <summary> 
        /// 获取监控实体的，实体仓储
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public virtual EntityRepository GetAnomalyMonitorRepository(string typeName)
        {
            Type type = Type.GetType(typeName) ?? throw new ValidationException("监控类型未找到{0}！".L10nFormat(typeName));
            return RepositoryFactoryHost.Factory.FindByEntity(type) as EntityRepository;
        }

        /// <summary>
        /// 查询数据
        /// </summary>
        /// <param name="strSQL"></param>
        /// <param name="ruleName"></param>
        /// <returns></returns>
        public virtual DataTable QueryData(string strSQL,string ruleName)
        {
            try
            {
                using (var db = DbAccesserFactory.Create(AbnormalInfoDataProvider.ConnectionStringName))
                {
                    strSQL = strSQL.Replace("&gt;", ">");
                    strSQL = strSQL.Replace("&lt;", "<");
                    var dt= db.ExecuteDataTable(strSQL);
                    if(dt.Rows.Count<=0)
                        RT.Logger.Error($"判异规则，SQL执行获取的数据集为空！".L10N());
                    return dt;
                }
            }
            catch (Exception ex)
            {
                RT.Logger.Error($"{ruleName}异常判异规则-执行异常：{strSQL}".L10N());
                throw new ValidationException(ex.Message);
            }
        }
        #endregion
    }
}
