using SIE.Domain;
using SIE.Domain.ORM;
using SIE.Packages;

[assembly: Repository(typeof(PackageEntityRepository<>))]

namespace SIE.Packages
{
    /// <summary>
    /// 本程序集的数据提供者
    /// </summary>
    /// <typeparam name="T">传入实体仓库的泛类</typeparam>
    [DataProvider(typeof(PackageEntityDataProvider))]//指定仓库的数据提供者
    public class PackageEntityRepository<T> : EntityRepository<T> where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// DataProvider
    /// </summary>
    public class PackageEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        public static readonly string ConnectionStringName = "master";

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }
}
