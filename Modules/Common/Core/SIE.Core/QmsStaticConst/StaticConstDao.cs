using SIE.Core.Common.Dao;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Core.QmsStaticConst
{
    /// <summary>
    /// MSA常用参数DAO
    /// </summary>
    public class StaticConstDao : BaseDao<StaticConst>
    {
        /// <summary>
        /// 根据名称获取MSA常用参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public StaticConst GetMsaConstByName(string name)
        {
            return Query().Where(c => c.Name == name).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 根据编码获取MSA常用参数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public StaticConst GetMsaConstByCode(string code)
        {
            return Query().Where(c => c.Code == code).FirstOrDefault(new EagerLoadOptions().LoadWithViewProperty());
        }
    }
}
