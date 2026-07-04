using SIE.Domain;
using SIE.Domain.ORM;
using SIE.EAP;
using System;
using System.Collections.Generic;
using System.Text;

////指定此程序集中实体的默认仓库
[assembly: Repository(typeof(EAPEntityRepository<>))]
namespace SIE.EAP
{
    /// <summary>
    /// 指定仓库的数据提供者
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [DataProvider(typeof(EAPEntityDataProvider))]
    public class EAPEntityRepository<T> : EntityRepository<T>
        where T : Entity
    {
        //此类的作用在于批量配置数据提供者(DataProvider)
    }

    /// <summary>
    /// 数据提供者
    /// </summary>
    public class EAPEntityDataProvider : RdbDataProvider
    {
        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        public const string ConnectionStringName = "INTERFACE";

        /// <summary>
        /// 数据库连接字符串的名称
        /// </summary>
        protected override string ConnectionStringSettingName
        {
            get { return ConnectionStringName; }
        }
    }
}
