using SIE.Core.Common.Dao;
using SIE.Data;
using SIE.Domain;
using SIE.Domain.ORM;
using SIE.Reflection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SIE.AbnormalInfo.AbnormalMonitors.Dao
{
    /// <summary>
    /// 
    /// </summary>
	public class SqlViewModelCurerDao : SqlEntityCurerDao
    {
        public Type TabType { get; set; }

        /// <summary>
        /// 创建实体
        /// </summary>
        /// <returns></returns>
        protected override Entity CreateEntity()
        {
            var entity = Entity.New(TabType);

            entity.PersistenceStatus = PersistenceStatus.Unchanged;

            return entity;
        }
        /// <summary>
        /// 此方法只针对读取表的字段与备注有效
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="val"></param>
        /// <param name="name"></param>
        protected override void WriteEntity(Entity entity, object val, string name)
        {
            if (name == "COLUMN_NAME" || name == "column_name")
            {
                entity.LoadProperty(AbnormalMonitorTab.ColumnNameProperty, val);
            }
            else if (name == "COMMENTS" || name == "comments")
            {
                entity.LoadProperty(AbnormalMonitorTab.CommentsProperty, val);
            }
        }

    }
}
