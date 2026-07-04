using SIE.Domain;
using SIE.Domain.ORM;
using SIE.RedCardManagment.WorkFlow;

//指定此程序集中实体的默认仓库
[assembly: Repository(typeof(RedCardWorkFlowEntityRepository<>))]

namespace SIE.RedCardManagment.WorkFlow
{
    /// <summary>
    /// 本程序集的数据提供者
    /// </summary>
    /// <typeparam name="T">t</typeparam>
    [DataProvider(typeof(RedCardWorkFlowEntityDataProvider))]//指定仓库的数据提供者
    public class RedCardWorkFlowEntityRepository<T> : EntityRepository<T> where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// Iqc实体数据提供者
    /// </summary>
    public class RedCardWorkFlowEntityDataProvider : RdbDataProvider
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