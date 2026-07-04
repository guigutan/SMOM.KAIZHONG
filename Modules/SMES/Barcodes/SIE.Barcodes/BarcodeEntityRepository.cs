using SIE.Barcodes;
using SIE.Domain;
using SIE.Domain.ORM;

//指定此程序集中实体的默认仓库
[assembly: Repository(typeof(BarcodeEntityRepository<>))]

namespace SIE.Barcodes
{
    /// <summary>
    /// 本程序集的数据提供者
    /// </summary>
    /// <typeparam name="T">泛型实体参数</typeparam>
    [DataProvider(typeof(BarcodeEntityDataProvider))]//指定仓库的数据提供者
    public class BarcodeEntityRepository<T> : EntityRepository<T> where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// 数据提供者
    /// </summary>
    public class BarcodeEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        public static readonly string ConnectionStringName = "MES";

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }
}