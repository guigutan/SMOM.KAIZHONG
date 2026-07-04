using SIE.Api;
using SIE.Domain;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Warehouses.OutLines
{
    /// <summary>
    /// 仓库信息下载控制器
    /// </summary>
    public class WhInfoDownloadController : DomainController
    {
        /// <summary>
        /// 标签下载
        /// </summary>
        /// <param name="whId"></param>
        /// <returns></returns>
        [ApiService("离线扫描获取库位数据")]
        [return: ApiReturn("标签数据")]
        public virtual List<StorageLocationData> DownloadLocData([ApiParameter("仓库Id")] double whId)
        {
            List<StorageLocationData> rst = new List<StorageLocationData>();

            var locs = Query<StorageLocation>().Where(p => p.WarehouseId == whId && p.State == State.Enable && !p.IsFrozen).ToList();
            locs.ForEach(f =>
            {
                rst.Add(new StorageLocationData() { Code = f.Code, Id = f.Id, IsLayIn = f.IsLayIn, IsTemporary = f.IsTemporary, IsPick = f.IsPick,WarehouseId=f.WarehouseId});
            });

            return rst;
        }

        /// <summary>
        /// 库位下载-获取所有库位
        /// </summary>
        /// <returns></returns>
        [ApiService("离线扫描获取库位数据")]
        [return: ApiReturn("标签数据")]
        public virtual List<StorageLocationData> DownloadAllLocData()
        {
            List<StorageLocationData> rst = new List<StorageLocationData>();

            var locs = Query<StorageLocation>().Where(p => p.State == State.Enable && !p.IsFrozen).ToList();
            locs.ForEach(f =>
            {
                rst.Add(new StorageLocationData() { Code = f.Code, Id = f.Id, IsLayIn = f.IsLayIn, IsTemporary = f.IsTemporary, IsPick = f.IsPick, WarehouseId = f.WarehouseId, Name = f.Name });
            });

            return rst;
        }
    }
}
