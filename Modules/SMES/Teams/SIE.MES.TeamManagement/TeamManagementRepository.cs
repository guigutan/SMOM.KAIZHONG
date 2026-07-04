using SIE.Domain;
using SIE.Domain.ORM;
using SIE.MES.TeamManagement;

[assembly: Repository(typeof(TeamManagementRepository<>))]

namespace SIE.MES.TeamManagement
{
    /// <summary>
    /// 指定仓库的数据提供者
    /// </summary>
    /// <typeparam name="T">实体仓库</typeparam> 
    [DataProvider(typeof(TeamManagementDataProvider))]
    public class TeamManagementRepository<T> : EntityRepository<T>
        where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)        
    }

    /// <summary>
    /// 班组管理数据提供者
    /// </summary> 
    public class TeamManagementDataProvider : RdbDataProvider
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