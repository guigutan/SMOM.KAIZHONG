using SIE.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.Andons.ForWinform.ApiModels
{
    [Serializable]
    public class XPItem
    {
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public SIE.Domain.State State { get; set; }

        /// <summary>
        /// 物料消耗类型
        /// </summary>
        public ConsumeMode ConsumeMode { get; set; }

        /// <summary>
        /// 最小包装数
        /// </summary>
        public decimal? MinPackingQty { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public ItemType Type { get; set; }

        public static XPItem Gen(Item item)
        {
            return new XPItem()
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                Description = item.Description,
                State = item.State,
                ConsumeMode = item.ConsumeMode,
                MinPackingQty = item.MinPackingQty,
                Type = item.Type
            };
        }
    }
}
