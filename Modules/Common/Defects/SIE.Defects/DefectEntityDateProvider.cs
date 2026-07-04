using SIE.Defects;
using SIE.Domain;
using SIE.Domain.ORM;

[assembly: Repository(typeof(DefectEntityRepository<>))] //指定此程序集中实体的默认仓库

namespace SIE.Defects
{
    /// <summary>
    ///  此类的作用在于批量配置数据提供者(DataProvider)
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [DataProvider(typeof(DefectEntityDataProvider))] //指定仓库的数据提供者

    public class DefectEntityRepository<T> : EntityRepository<T>
        where T : Entity
    {
    }

    /// <summary>
    /// 数据提供者
    /// </summary>
    public class DefectEntityDataProvider : RdbDataProvider
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