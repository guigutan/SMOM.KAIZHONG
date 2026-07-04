using SIE.Domain;
using SIE.Domain.ORM;
using SIE.KZ.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: Repository(typeof(BaseEntityRepository<>))]
namespace SIE.KZ.Base
{

    /// <summary>
    /// 指定仓库的数据提供者
    /// </summary>
    /// <typeparam name="T">实体仓库</typeparam>
    [DataProvider(typeof(BaseEntityDataProvider))]
    public class BaseEntityRepository<T> : EntityRepository<T>
        where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }


    /// <summary>
    /// MES数据提供者
    /// </summary>
    public class BaseEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        public const string ConnectionStringName = "master";

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }
}
