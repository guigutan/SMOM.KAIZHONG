using SIE.Domain;
using SIE.EventMessages.Items.Datas;
using SIE.EventMessages.WMS.Receipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.Items
{
    /// <summary>
    /// 
    /// </summary>
    [Services.Service(FallbackType = typeof(DefaultIItem))]
    public interface IItem
    {

        /// <summary>
        /// 随机获取一个客户料码数据
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        ItemCusotmerRelationData GetItemCusotmerRelationData(double ItemId);

        /// <summary>
        /// 根据物料Id获取单位名称
        /// </summary>
        /// <returns></returns>
        string GetUnitNameGetItemId(double Id);

        /// <summary>
        /// 根据物料，获取父级物料信息(随机一个)
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        ParentItemData GetParentItemData(string itemCode);
    }

    /// <summary>
    /// 
    /// </summary>
    public class DefaultIItem : IItem
    {
        /// <summary>
        /// 随机获取一个客户料码数据
        /// </summary>
        /// <param name="ItemId"></param>
        /// <returns></returns>
        public ItemCusotmerRelationData GetItemCusotmerRelationData(double ItemId)
        {
                return null;
        }

        public string GetUnitNameGetItemId(double Id)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// 根据物料，获取父级物料信息(随机一个)
        /// </summary>
        /// <param name="itemCode"></param>
        /// <returns></returns>
        public ParentItemData GetParentItemData(string itemCode)
        {
            return null;
        }
    }
}
