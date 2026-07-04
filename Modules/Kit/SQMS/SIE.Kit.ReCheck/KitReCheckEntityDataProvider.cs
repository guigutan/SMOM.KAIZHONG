using SIE.Domain;
using SIE.Domain.ORM;
using SIE.Kit.ReCheck;

//指定此程序集中实体的默认仓库
[assembly: Repository(typeof(KitReCheckEntityDataProvider<>))]

namespace SIE.Kit.ReCheck
{
    /// <summary>
    /// 本程序集的数据提供者
    /// </summary>
    /// <typeparam name="T">t</typeparam>
    [DataProvider(typeof(KitReCheckEntityDataProvider))]//指定仓库的数据提供者
    public class KitReCheckEntityDataProvider<T> : EntityRepository<T> where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// KitReCheck实体数据提供者
    /// </summary>
    public class KitReCheckEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        public const string ConnectionStringName = "QMS";

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }
}
