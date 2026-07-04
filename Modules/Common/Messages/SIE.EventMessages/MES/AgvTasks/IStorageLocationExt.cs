using SIE.EventMessages.MES.AgvTasks.Datas;
using System.Collections.Generic;

namespace SIE.EventMessages.MES.AgvTasks
{

    /// <summary>
    /// 工位货区编码 获取货位信息
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultStorageLocationExtInterface))]
    public interface IStorageLocationExt
    {
        /// <summary>
        /// 
        /// </summary>
        List<StorageLocationData> GetStorageLocationExtList(string storageAreaCode);

    }

    /// <summary>
    /// 默认实现
    /// </summary>
    public class DefaultStorageLocationExtInterface : IStorageLocationExt
    {
        /// <summary>
        /// 
        /// </summary>
        public List<StorageLocationData> GetStorageLocationExtList(string storageAreaCode)
        {
            return new List<StorageLocationData>();
    }

    }

}
