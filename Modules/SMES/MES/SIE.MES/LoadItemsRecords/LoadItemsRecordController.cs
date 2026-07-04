using SIE.Common.ImportHelper;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.LoadItemRecords;
using SIE.MES.LoadItems;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Resources.WipResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.LoadItemsRecords
{
    /// <summary>
    /// 上料下料记录控制器
    /// </summary>
    public class LoadItemsRecordController : DomainController
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="loadItemsRecordCriterial"></param>
        /// <returns></returns>
        public virtual EntityList Fetch(LoadItemsRecordCriterial loadItemsRecordCriterial)
        {
            if (!loadItemsRecordCriterial.OpareteType.HasValue)
            {
                throw new ValidationException("请选择操作类型".L10N());
            }
            if (loadItemsRecordCriterial.OpareteType.Value == OpareteType.LoadItem && loadItemsRecordCriterial.UnloadItemType.HasValue)
            {
                throw new ValidationException("操作类型为上料时，下料类型选项请置空".L10N());
            }

            EntityList<LoadItemsRecord> result = new EntityList<LoadItemsRecord>();
            switch (loadItemsRecordCriterial.OpareteType)
            {
                case OpareteType.LoadItem:
                    return GetLoadItemRecord(loadItemsRecordCriterial);
                    break;
                case OpareteType.UnloadItem:
                    GetUnloadItemRecord(loadItemsRecordCriterial, result);
                    break;
                default:
                    break;

            }
            return result;
        }

        /// <summary>
        /// 获取下料记录
        /// </summary>
        /// <param name="loadItemsRecordCriterial"></param>
        /// <param name="result"></param>
        /// <param name="q"></param>
        private void GetUnloadItemRecord(LoadItemsRecordCriterial loadItemsRecordCriterial, EntityList<LoadItemsRecord> result)
        {
            var q = Query<UnloadItem>().Exists<EmployeeResource>((x, y) =>
                            y.Where(z => x.ResourceId == z.ResourceId && z.EmployeeId == RT.IdentityId));
            if (loadItemsRecordCriterial.OparetorId.HasValue)
            {
                q.Where(m => m.CreateBy == loadItemsRecordCriterial.OparetorId);
            }
            if (loadItemsRecordCriterial.OparetorTime.BeginValue.HasValue)
            {
                q.Where(m => m.CreateDate >= loadItemsRecordCriterial.OparetorTime.BeginValue);
            }
            if (loadItemsRecordCriterial.OparetorTime.EndValue.HasValue)
            {
                q.Where(m => m.CreateDate <= loadItemsRecordCriterial.OparetorTime.EndValue);
            }
            if (loadItemsRecordCriterial.ResourceId.HasValue)
            {
                q.Where(m => m.ResourceId == loadItemsRecordCriterial.ResourceId);
            }
            if (loadItemsRecordCriterial.StationId.HasValue)
            {
                q.Where(m => m.StationId == loadItemsRecordCriterial.StationId);
            }
            if (loadItemsRecordCriterial.IsDiaplayZero.HasValue && loadItemsRecordCriterial.IsDiaplayZero.Value)
            {
                q.Where(m => m.Qty != 0);
            }
            if (!loadItemsRecordCriterial.LoadItemWokerOrder.IsNullOrEmpty())
            {
                q.Where(m => m.WorkOrder.No.Contains(loadItemsRecordCriterial.LoadItemWokerOrder));
            }
            if (!loadItemsRecordCriterial.ItemCode.IsNullOrEmpty())
            {
                q.Where(m => m.Item.Code.Contains(loadItemsRecordCriterial.ItemCode));
            }
            if (!loadItemsRecordCriterial.ItemName.IsNullOrEmpty())
            {
                q.Where(m => m.Item.Name.Contains(loadItemsRecordCriterial.ItemName));
            }
            if (!loadItemsRecordCriterial.Sn.IsNullOrEmpty())
            {
                q.Where(m => m.SourceCode.Contains(loadItemsRecordCriterial.Sn));
            }
            if (loadItemsRecordCriterial.UnloadItemType.HasValue)
            {
                q.Where(m => m.IsNg == (loadItemsRecordCriterial.UnloadItemType == UnloadItemType.NG));
            }
            var list = q.ToList(loadItemsRecordCriterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var item in list)
            {
                var res = new LoadItemsRecord()
                {
                    Id = item.Id,
                    Item = item.Item,
                    ItemCode = item.ItemCode,
                    ItemName = item.ItemName,
                    FactoryName = item.Resource.FactoryName,
                    Qty = item.RemainderQty.ToString(),
                    Station = item.Station,
                    SourceCode = item.SourceCode,
                    Resource = item.Resource,
                    LoadDownQty = item.Qty,
                    OpareteTime = item.CreateDate,
                    WorkOrder = item.WorkOrder,
                    OparetorName = item.CreateByName,
                    SourceType = item.SourceType,
                    OpareteType = OpareteType.UnloadItem,
                    ItemExtPropName = item.ItemExtPropName,
                    UnloadItemType = item.IsNg ? UnloadItemType.NG : UnloadItemType.Pass
                };
                result.Add(res);
            }
            result.SetTotalCount(list.TotalCount);
        }

        /// <summary>
        /// 获取上料记录
        /// </summary>
        /// <param name="loadItemsRecordCriterial"></param>
        private EntityList<LoadItemsRecord> GetLoadItemRecord(LoadItemsRecordCriterial loadItemsRecordCriterial)
        {
            var q = Query<LoadItemsRecord>().Exists<EmployeeResource>((x, y) =>
                            y.Where(z => x.ResourceId == z.ResourceId && z.EmployeeId == RT.IdentityId));
            if (loadItemsRecordCriterial.OparetorId.HasValue)
            {
                q.Where(m => m.CreateBy == loadItemsRecordCriterial.OparetorId);
            }
            if (loadItemsRecordCriterial.OparetorTime.BeginValue.HasValue)
            {
                q.Where(m => m.CreateDate >= loadItemsRecordCriterial.OparetorTime.BeginValue);
            }
            if (loadItemsRecordCriterial.OparetorTime.EndValue.HasValue)
            {
                q.Where(m => m.CreateDate <= loadItemsRecordCriterial.OparetorTime.EndValue);
            }
            if (loadItemsRecordCriterial.ResourceId.HasValue)
            {
                q.Where(m => m.ResourceId == loadItemsRecordCriterial.ResourceId);
            }
            if (loadItemsRecordCriterial.StationId.HasValue)
            {
                q.Where(m => m.StationId == loadItemsRecordCriterial.StationId);
            }
            if (loadItemsRecordCriterial.IsDiaplayZero.HasValue && loadItemsRecordCriterial.IsDiaplayZero.Value)
            {
                q.Where(m => (m.Qty != "0" && m.Qty != ""));
            }
            if (!loadItemsRecordCriterial.LoadItemWokerOrder.IsNullOrEmpty())
            {
                q.Where(m => m.WorkOrder.No.Contains(loadItemsRecordCriterial.LoadItemWokerOrder));
            }
            if (!loadItemsRecordCriterial.ItemCode.IsNullOrEmpty())
            {
                q.Where(m => m.Item.Code.Contains(loadItemsRecordCriterial.ItemCode));
            }
            if (!loadItemsRecordCriterial.ItemName.IsNullOrEmpty())
            {
                q.Where(m => m.Item.Name.Contains(loadItemsRecordCriterial.ItemName));
            }
            if (!loadItemsRecordCriterial.Sn.IsNullOrEmpty())
            {
                q.Where(m => m.SourceCode.Contains(loadItemsRecordCriterial.Sn));
            }

            return q.ToList(loadItemsRecordCriterial.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
        }

        /// <summary>
        /// 获取带权限生产资源
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<WipResource> GetAuthResource(PagingInfo pagingInfo, string keyword)
        {
            return Query<WipResource>().Join<EmployeeResource>((x, y) => x.Id == y.ResourceId && y.EmployeeId == RT.IdentityId)
                 .WhereIf(!keyword.IsNullOrEmpty(), p => p.Code.Contains(keyword) || p.Name.Contains(keyword)).ToList(pagingInfo);
        }
    }
}
