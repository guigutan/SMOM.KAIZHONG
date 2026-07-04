using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using System;
using System.Collections.Generic;
using System.Data;

namespace SIE.ERPInterface.Ebs.Connection
{
    /// <summary>
    /// 数据库执行帮助类
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// SQL查询数据
        /// </summary>
        public static IEnumerable<DataRow> ExecuteSqlForDataTable(string sql)
        {
            using (var db = DB.Create(InterfaceEntityDataProvider.ConnectionStringName))
            {
                try
                {
                    var dt = db.ExecuteDataTable(sql, CommandType.Text);
                    return dt.AsEnumerable();
                }
                catch (Exception e)
                {
                    throw new ValidationException(e.Message);
                }
            }
        }

        /// <summary>
        /// SQL查询数据
        /// </summary>
        public static object ExecuteSqlScalar(string sql)
        {
            using (var db = DB.Create(InterfaceEntityDataProvider.ConnectionStringName))
            {
                try
                {
                    return db.ExecuteScalar(sql, CommandType.Text);
                }
                catch (Exception e)
                {
                    throw new ValidationException(e.Message);
                }
            }
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        public static void ExecuteNonQuery(string sql)
        {
            using (var db = DB.Create(InterfaceEntityDataProvider.ConnectionStringName))
            {
                try
                {
                    db.ExecuteNonQuery(sql, CommandType.Text);
                }
                catch (Exception e)
                {
                    throw new ValidationException(e.Message);
                }
            }
        }

        /// <summary>
        /// 执行存储过程查询
        /// </summary>
        /// <param name="procedureName">存储过程名</param>
        /// <param name="dateTime">时间戳</param>
        public static DataTable ExecuteProcedure(string procedureName, DateTime? dateTime, string keyWord)
        {
            using (var db = DB.Create(InterfaceEntityDataProvider.ConnectionStringName))
            {
                try
                {
                    var dt = db.ExecuteDataTable(procedureName, CommandType.StoredProcedure,
                        db.ParameterFactory.CreateParameter("P_INV_ORG_ID", RT.InvOrg.Value, DbType.Int32, ParameterDirection.Input),
                        db.ParameterFactory.CreateParameter("P_LAST_UPDATE_DATE", dateTime, DbType.DateTime, ParameterDirection.Input),
                        db.ParameterFactory.CreateParameter("P_KEY_WORD", keyWord, DbType.String, ParameterDirection.Input),
                        db.ParameterFactory.CreateParameter("X_RETURN_DATA", null, DbType.Object, ParameterDirection.Output));

                    return dt;
                }
                catch (Exception e)
                {
                    throw new ValidationException(e.Message);
                }
            }
        }
    }
}
