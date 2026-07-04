using SIE.CSM;
using SIE.Domain;
using SIE.Domain.ORM;

[assembly: Repository(typeof(CsmEntityRepository<>))]

namespace SIE.CSM
{
    /// <summary>
    /// 本程序集的数提供者
    /// </summary>
    /// <typeparam name="T">仓库</typeparam>
    [DataProvider(typeof(CsmEntityDataProvider))]//指定仓库的数据提供者

    public class CsmEntityRepository<T> : EntityRepository<T> where T : Entity
    {
        //此类的作用在于批量配置数据提供者
    }

    /// <summary>
    /// //指定此程序集实体的默认仓库
    /// </summary>
    public class CsmEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库链接字符串
        /// </summary>
        public const string ConnectionStringName = "master";

        /// <summary>
        /// 仓库链接
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get
            {
                return ConnectionStringName;
            }
        }
    }
}
