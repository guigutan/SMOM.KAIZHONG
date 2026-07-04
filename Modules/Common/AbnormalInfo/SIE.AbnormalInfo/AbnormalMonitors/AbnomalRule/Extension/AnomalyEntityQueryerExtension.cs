using SIE.Domain;
using System;


namespace SIE.AbnormalInfo.AnomalyMonitors.Extension
{
    /// <summary>
    /// 
    /// </summary>
	public static class AnomalyDB
	{
        /// <summary>
        /// 最近一周单据不合格率
        /// </summary>
        public static IEntityQueryer Query(Type type)
        {
            var Query = typeof(DB).GetMethod("Query").MakeGenericMethod(type);
            var parameters = new object[] { "" /*传递给泛型方法的参数*/ };
            return Query.Invoke(null, parameters) as IEntityQueryer;
        }

         /// <summary>
         /// 
         /// </summary>
         /// <param name="query"></param>
         /// <param name="paging"></param>
         /// <param name="eagerLoad"></param>
         /// <returns></returns>
        public static EntityList ToEntityList(this IEntityQueryer query, PagingInfo paging = null, EagerLoadOptions eagerLoad = null)
        {
            return query.Repository.QueryList(query, paging, eagerLoad);
        }
    }
}
