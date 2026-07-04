using SIE.Domain;
using SIE.Domain.ORM;
using SIE.WorkBenchCommon;

//指定此程序集中实体的默认仓库s
[assembly: Repository(typeof(WorkBenchCommonEntityRepository<>))]
namespace SIE.WorkBenchCommon
{
    /// <summary>
    /// 指定仓库的数据提供者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataProvider(typeof(WorkBenchCommonEntityDataProvider))]//指定仓库的数据提供者
    public class WorkBenchCommonEntityRepository<T> : EntityRepository<T>
        where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// COMMON数据提供者
    /// </summary>
    public class WorkBenchCommonEntityDataProvider : RdbDataProvider
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
