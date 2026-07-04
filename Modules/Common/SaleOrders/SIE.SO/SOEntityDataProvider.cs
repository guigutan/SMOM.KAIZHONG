using SIE.Domain;
using SIE.Domain.ORM;
using SIE.SO;

[assembly: Repository(typeof(SOEntityRepository<>))]

namespace SIE.SO
{
    /// <summary>
    /// 指定APS的数据提供者
    /// </summary>
    /// <typeparam name="T">实体仓库</typeparam> 
    [DataProvider(typeof(SOEntityDataProvider))]
    public class SOEntityRepository<T> : EntityRepository<T>
        where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// APS数据提供者
    /// </summary>
    public class SOEntityDataProvider : RdbDataProvider
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
