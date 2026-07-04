using SIE.Domain;
using SIE.Domain.ORM;
using SIE.EMS.SpecialEquipment;

//指定此程序集中实体的默认仓库
[assembly: Repository(typeof(EntityDataRepository<>))]
namespace SIE.EMS.SpecialEquipment
{
    /// <summary>
    /// 当前模块数据仓库
    /// </summary>
    /// <typeparam name="T">实体泛型参数</typeparam>
    [DataProvider(typeof(EntityDataProvider))]//指定仓库的数据提供者
    public class EntityDataRepository<T> : EntityRepository<T>
        where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// 数据提供者
    /// </summary>
    class EntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        public const string ConnectionStringName = "EMS";

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }
}
