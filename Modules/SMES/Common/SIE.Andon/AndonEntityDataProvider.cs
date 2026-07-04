using SIE.Andon;
using SIE.Domain;
using SIE.Domain.ORM;

[assembly: Repository(typeof(AndonEntityRepository<>))]
namespace SIE.Andon
{
    /// <summary>
    /// 指定仓库数据提供者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataProvider(typeof(AndonEntityDataProvider))]
    public class AndonEntityRepository<T> : EntityRepository<T> where T : Entity
    {

    }

    /// <summary>
    /// Andon数据提供者
    /// </summary>
    public class AndonEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public const string ConnectionStringName = "MES";

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }
}
