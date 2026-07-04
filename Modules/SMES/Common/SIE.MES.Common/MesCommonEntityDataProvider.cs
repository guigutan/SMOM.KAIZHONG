using SIE.Domain;
using SIE.Domain.ORM;
using SIE.MES.Common;

[assembly: Repository(typeof(MesCommonEntityRepository<>))]
namespace SIE.MES.Common
{
    /// <summary>
    /// 指定仓库的数据提供者
    /// </summary>
    /// <typeparam name="T">实体仓库</typeparam> 
    [DataProvider(typeof(MesCommonEntityDataProvider))]
    public class MesCommonEntityRepository<T> : EntityRepository<T>
            where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// MES数据提供者
    /// </summary>
    public class MesCommonEntityDataProvider : RdbDataProvider
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