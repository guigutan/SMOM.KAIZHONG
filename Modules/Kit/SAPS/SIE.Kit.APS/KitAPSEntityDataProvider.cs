using SIE.Domain;
using SIE.Domain.ORM;
using SIE.Kit.APS;


[assembly: Repository(typeof(KitAPSEntityDataProvider<>))]
namespace SIE.Kit.APS
{

    /// <summary>
    /// 指定仓库的数据提供者
    /// </summary>
    /// <typeparam name="T">实体仓库</typeparam> 
    [DataProvider(typeof(KitAPSEntityDataProvider))]
    public class KitAPSEntityDataProvider<T> : EntityRepository<T>
            where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// APS数据提供者
    /// </summary>
    public class KitAPSEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        public const string ConnectionStringName = "APS";

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }


}
