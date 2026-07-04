using SIE.Domain;
using SIE.Domain.ORM;
using SIE.ProductIntfc;
////指定此程序集中实体的默认仓库
[assembly: Repository(typeof(ProductIntfcEntityDataProviderEntityRepository<>))]
namespace SIE.ProductIntfc
{
    /// <summary>
    /// 指定仓库的数据提供者
    /// </summary>
    /// <typeparam name="T">实体仓库</typeparam>
    [DataProvider(typeof(ProductIntfcEntityDataProvider))]
    public class ProductIntfcEntityDataProviderEntityRepository<T> : EntityRepository<T>
        where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// 数据提供者
    /// </summary>
    public class ProductIntfcEntityDataProvider : RdbDataProvider
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
