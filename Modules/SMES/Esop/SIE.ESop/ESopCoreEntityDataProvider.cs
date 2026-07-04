using SIE.Domain;
using SIE.Domain.ORM;
using SIE.ESop;
////指定此程序集中实体的默认仓库
[assembly: Repository(typeof(ESopEntityRepository<>))]

namespace SIE.ESop
{
    /// <summary>
    /// 实体仓库
    /// </summary>
    /// <typeparam name="T">泛型</typeparam>
    ////指定仓库的数据提供者
    [DataProvider(typeof(ESopEntityDataProvider))]
    public class ESopEntityRepository<T> : EntityRepository<T>
        where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// 实体数据提供者
    /// </summary>
    public class ESopEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        public const string ConnectionStringName = "MES";

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }
}
