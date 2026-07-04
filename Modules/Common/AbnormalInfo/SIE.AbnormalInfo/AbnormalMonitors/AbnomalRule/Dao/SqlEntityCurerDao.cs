using SIE.Core.Common.Dao;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.ORM;
using SIE.Reflection;
using System;
using System.Data;
using System.Linq;

namespace SIE.AbnormalInfo.AbnormalMonitors.Dao
{
    /// <summary>
    /// 
    /// </summary>
	public class SqlEntityCurerDao : BaseDao<Entity>
    {
        /// <summary>
        /// 
        /// </summary>
        public IRepository Rep { get; set; }

        #region List
        /// <summary>
        /// Sql查询，List
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="entityList"></param>
        public virtual  void QueryList(string sql, EntityList entityList)
        {
            using (var dba = DbAccesserFactory.Create(AbnormalInfoDataProvider.ConnectionStringName))
            {
                try
                { 
                    var reader = dba.ExecuteReader(sql);
                    FillDataIntoList(reader, entityList);
                }
                catch (Exception ex)
                {
                    RT.Logger.Error($"数据库连接失败！：{ex.Message}".L10N());
                }
            }

        }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        protected virtual Entity CreateEntity()
        {
            var entity = Rep.New();

            entity.PersistenceStatus = PersistenceStatus.Unchanged;

            return entity;
        }

        private  void FillDataIntoList(IDataReader reader, EntityList list, bool fetchingFirst = false, bool loadViewProperty = false)
        {
            Func<IDataReader, bool, string> rowReader = (dr, isWriteLog) =>
            {
                var entity = ReaderToEntity(dr, loadViewProperty);
                list.Add(entity);
                return isWriteLog ? entity.GetType().FullName : null;
            };
            if (fetchingFirst)
            {
                if (reader.Read())
                {
                    rowReader(reader, false);
                }
            }
            else
            {
                PagingHelper.MemoryPaging(reader, rowReader, null);
            }
        }

        private  Entity ReaderToEntity(IDataReader reader, bool readViewProperty)
        {
            var entity = CreateEntity();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                var value = reader.GetValue(i);
                if (value != DBNull.Value)
                {
                    var name = reader.GetName(i);
                    WriteEntity(entity,value,name);
                    //else if (readViewProperty && (col = FindDisplayColumn(name)) != null)
                    //	entity.ExtValues[col.Info.Property.Name + "_Display"] = value;
                }
            }
            return entity;
        }

        /// <summary>
        /// 写入实体属性值
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="val"></param>
        /// <param name="name"></param>
        protected virtual void WriteEntity(Entity entity, object val, string name)
        {
            var col = FindByColumnName(name);
            if (col == null) return;
            if (col.Property is IRefIdProperty refIdProperty)
            {
                if (val != null)
                {
                    var id = TypeHelper.CoerceValue(refIdProperty.PropertyType, val);
                    entity.LoadProperty(refIdProperty, id);
                }
                return;
            }
            val ??= string.Empty;
            entity.LoadProperty(col.Property, val);
        }

        /// <summary>
        /// 根据栏位名称查找，不区分大小写
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IPersistanceColumnInfo FindByColumnName(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            return Rep.TableInfo.Columns.First(c => c.Name == name);//(keyColumns, out column);
        }
        #endregion
    }
}
