using NPOI.SS.Formula.Functions;
using NPOI.Util;
using SIE.Api;
using SIE.Domain.Validation;
using SIE.Units;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Items
{
    /// <summary>
    /// 物料控制器  API
    /// </summary>
    public partial class ItemController : DomainController
    {
        #region API
        /// <summary>
        /// 获取物料编码
        /// </summary>
        /// <returns>物料编码</returns>
        [ApiService("按任务盘点：获取物料编码")]
        [return: ApiReturn("返回物料编码：ItemCode")]
        public virtual string GetItemCode([ApiParameter("产品")] string code)
        {
            var item = GetItemFromCode(code);
            if (item == null) throw new ValidationException("产品[{0}]不存在物料表中!".L10nFormat(code));

            return item.Code;
        }

        /// <summary>
        /// 获取物料列表
        /// </summary>
        /// <param name="keyword">物料编码/名称</param>
        /// <param name="pageNumber">页数，为空查第一页</param>
        /// <param name="pageSize">页数据数量，为空查所有</param>
        /// <returns>物料信息列表</returns>
        [ApiService("获取物料列表")]
        [return: ApiReturn("分页物料信息列表 PagingItemInfo")]
        public virtual PagingItemInfo GetItems([ApiParameter("查询字符串")] string keyword, [ApiParameter("页数，为空查第一页")] int? pageNumber, [ApiParameter("页数据数量，为空查所有")] int? pageSize)
        {
            var pagingInfo = new PagingInfo()
            {
                PageNumber = pageNumber.HasValue ? pageNumber.Value : 1,
                PageSize = pageSize.HasValue ? pageSize.Value : int.MaxValue - 1,
                IsNeedCount = true
            };
            var items = GetItemByCode(keyword, pagingInfo);
            var infos = new List<ItemInfo>();
            items.ForEach(e =>
            {
                infos.Add(new ItemInfo()
                {
                    ItemId = e.Id,
                    ItemCode = e.Code,
                    ItemName = e.Name
                });
            });
            PagingItemInfo result = new PagingItemInfo()
            {
                PageNumber = pagingInfo.PageNumber,
                PageSize = pagingInfo.PageSize,
                TotalCount = items.TotalCount
            };
            result.ItemInfos.AddRange(infos);
            return result;
        }

        /// <summary>
        /// 扫描物料编码
        /// </summary>
        /// <returns>任务组集合</returns>
        [ApiService("线边仓盘点：获取物料扩展属性")]
        [return: ApiReturn("返回物料扩展属性数据：List<ExtensionData>")]
        public virtual List<StockExtensionData> GetCountScanItemCodeData([ApiParameter("物料编码")] string itemCode)
        {
            var item = RT.Service.Resolve<ItemController>().GetItem(itemCode);
            List<StockExtensionData> list = new List<StockExtensionData>();
            if (item.EnableExtendProperty)
            {
                var propertyValues = RT.Service.Resolve<ItemController>().GetItemPropertys(item.Id);
                var definitionList = propertyValues.GroupBy(p => new { Name = p.DefinitionName, Id = p.DefinitionId }).Distinct().ToList();
                definitionList.ForEach(p =>
                {
                    var data = new StockExtensionData();
                    data.ItemName = item.Name;
                    data.name = p.Key.Name;
                    data.id = p.Key.Id;
                    data.valList = p.Select(q => q.Value).ToList();
                    list.Add(data);
                });
            }

            return list;
        }

        /// <summary>
        /// 物料单位精度
        /// </summary>
        /// <returns>任务组集合</returns>
        [ApiService("物料单位精度")]
        [return: ApiReturn("返回物料单位精度")]
        public virtual ItemUnitPrecsionInfo GetItemUnitPrecisions(double itemId)
        {
            return RT.Service.Resolve<ItemUnitController>().GetItemPrecision(itemId);
        }

       
        #endregion
    }
}