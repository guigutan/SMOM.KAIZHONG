using SIE.Domain;
using SIE.Domain.ORM;
using SIE.Recheck.Common;

[assembly: Repository(typeof(RecheckCommonEntityRepository<>))]
namespace SIE.Recheck.Common
{
    /// <summary>
    /// 指定仓库的数据提供者
    /// </summary>
    /// <typeparam name="T">实体仓库</typeparam> 
    [DataProvider(typeof(RecheckCommonEntityDataProvider))]
    public class RecheckCommonEntityRepository<T> : EntityRepository<T>
        where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// 复检数据提供者
    /// </summary>
    public class RecheckCommonEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        public static string ConnectionStringName { get { return "master"; } }

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }
}
